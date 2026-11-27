// 슬롯별 JSON을 같은 디렉터리의 임시 파일에 완전히 쓴 후 교체한다. 손상된 원본은 직전 백업으로 읽는다.
// 정산은 쓰기 잠금 안에서 저장 일차를 다시 읽고, 아직 처리하지 않은 방문일의 지급 후 상태만 기록한다.
// 스냅샷에는 양념·던전 재료 재고, 다음 아침의 미처리 귀환 보고, 전날 방문 호감과 입소문 추가 인원, 손님 명부와 일차 시드, 층 진행이 함께 들어간다 (FirstServiceSaveData).
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using DOB.Define;

namespace DOB.Systems
{
    public sealed class FirstServiceSaveStore
    {
        private static readonly object WriteGate = new object();
        private readonly string directory;
        // 저장 슬롯의 양념 id 검사 기준. 병렬 정산이 다른 스레드에서 읽어도 되도록 만들 때 한 번 읽는다.
        private readonly ICollection<string> knownSpices;
        private readonly ICollection<string> knownRecipes;

        /// <summary>프리뷰 전용 저장 경로를 호출자가 정한다. 슬롯을 찾아보기만 할 때는 폴더를 만들지 않는다.</summary>
        public FirstServiceSaveStore(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentException("저장 경로가 필요합니다.", nameof(directory));
            this.directory = Path.GetFullPath(directory);
            knownSpices = FirstServiceSaveData.LoadKnownSpices();
            knownRecipes = FirstServiceSaveData.LoadKnownRecipes();
        }

        public bool TryLoad(FirstServiceSaveSlot slot, out FirstServiceSaveData data)
        {
            string path = SlotPath(slot);
            return TryRead(path, slot, out data) || TryRead(path + ".bak", slot, out data);
        }

        /// <summary>자동·수동 슬롯 중 최신. 데모 완료 슬롯은 고르지 않는다(타이틀이 따로 보인다).</summary>
        public bool TryLoadLatest(out FirstServiceSaveData data)
        {
            bool hasAuto = TryLoad(FirstServiceSaveSlot.Auto, out var automatic);
            bool hasManual = TryLoad(FirstServiceSaveSlot.Manual, out var manual);
            data = hasManual && (!hasAuto || manual.SavedAtUtc >= automatic.SavedAtUtc) ? manual : automatic;
            return data != null;
        }

        /// <summary>일차 시작 스냅샷을 쓴다. 재료·미처리 귀환 보고·전날 방문 호감을 생략하면 비어 있는 것으로,
        /// 입소문 추가 인원을 생략하면 0으로 저장한다. unlockedRecipes는 열린 레시피 id다. progress를 주면 수동 저장을 누른 시점의 진행을 함께 쓴다.</summary>
        public FirstServiceSaveData SavePreparation(FirstServiceSaveSlot slot, int day, int gold, IEnumerable<MaterialCount> spices,
            IEnumerable<MaterialCount> materials = null, IEnumerable<VisitResult> pendingReports = null,
            IEnumerable<VisitAffinity> visitAffinities = null, int wordOfMouthBonus = 0,
            IEnumerable<GuestRecord> guests = null, int? daySeed = null, DayProgress progress = null, FloorProgressState floors = null,
            IEnumerable<string> unlockedRecipes = null, int? burners = null)
        {
            lock (WriteGate)
            using (LockDirectory())
                return Write(slot, day, gold, spices, materials, pendingReports, visitAffinities, wordOfMouthBonus, guests, daySeed, progress, floors, unlockedRecipes, burners);
        }

        /// <summary>수입 대신 지급이 끝난 절대 잔액·재고와 그날의 방문 결과·방문 호감, 다음 영업의 입소문 추가 인원을 받아
        /// 다음 일차 시작으로 쓴다. 같은 날의 두 번째 호출은 기존 저장본을 반환한다.</summary>
        public FirstServiceSaveData SettleDay(int completedDay, int goldAfterPayment, IEnumerable<MaterialCount> spicesAfterService,
            IEnumerable<MaterialCount> materialsAfterReports = null, IEnumerable<VisitResult> visitResults = null,
            IEnumerable<VisitAffinity> visitAffinities = null, int wordOfMouthBonus = 0,
            IEnumerable<GuestRecord> guests = null, int? daySeed = null, FloorProgressState floors = null,
            IEnumerable<string> unlockedRecipes = null, int? burners = null)
        {
            if (completedDay < 1 || completedDay == int.MaxValue) throw new ArgumentOutOfRangeException(nameof(completedDay));
            lock (WriteGate)
            using (LockDirectory())
            {
                if (TryLoad(FirstServiceSaveSlot.Auto, out var previous) && previous.Day > completedDay)
                    return previous;
                return Write(FirstServiceSaveSlot.Auto, completedDay + 1, goldAfterPayment, spicesAfterService,
                    materialsAfterReports, visitResults, visitAffinities, wordOfMouthBonus, guests, daySeed, null, floors, unlockedRecipes, burners);
            }
        }

        private FileStream LockDirectory()
        {
            Directory.CreateDirectory(directory);
            return new FileStream(Path.Combine(directory, "save.lock"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        private string SlotPath(FirstServiceSaveSlot slot) => Path.Combine(directory, FirstServiceSaveData.SlotName(slot) + ".json");

        private bool TryRead(string path, FirstServiceSaveSlot slot, out FirstServiceSaveData data)
        {
            data = null;
            try
            {
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
                {
                    var reader = new DataContractJsonSerializer(typeof(FirstServiceSaveData));
                    var candidate = (FirstServiceSaveData)reader.ReadObject(file);
                    if (candidate == null || !candidate.IsValidFor(slot, knownSpices, knownRecipes)) return false;
                    data = candidate;
                    return true;
                }
            }
            catch (FileNotFoundException) { return false; }
            catch (DirectoryNotFoundException) { return false; }
            catch (SerializationException) { return false; }
            catch (XmlException) { return false; }
            // Unity의 Mono JSON 역직렬화기는 숫자·형식 오류를 리플렉션 예외 안에 전달하기도 한다.
            catch (TargetInvocationException ex) when (ex.GetBaseException() is FormatException ||
                ex.GetBaseException() is OverflowException || ex.GetBaseException() is XmlException ||
                ex.GetBaseException() is SerializationException) { return false; }
        }

        private FirstServiceSaveData Write(FirstServiceSaveSlot slot, int day, int gold, IEnumerable<MaterialCount> spices,
            IEnumerable<MaterialCount> materials, IEnumerable<VisitResult> pendingReports,
            IEnumerable<VisitAffinity> visitAffinities, int wordOfMouthBonus, IEnumerable<GuestRecord> guests, int? daySeed,
            DayProgress progress, FloorProgressState floors, IEnumerable<string> unlockedRecipes, int? burners)
        {
            DateTime timestamp = DateTime.UtcNow;
            if (TryLoadLatest(out var latest) && timestamp <= latest.SavedAtUtc && latest.SavedAtUtc < DateTime.MaxValue)
                timestamp = latest.SavedAtUtc.AddTicks(1);
            var data = new FirstServiceSaveData(slot, day, gold, spices, timestamp, materials, pendingReports,
                visitAffinities, wordOfMouthBonus, guests, daySeed, progress, floors, unlockedRecipes, burners, knownSpices, knownRecipes);
            string path = SlotPath(slot);
            string backup = path + ".bak";
            string temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            string backupTemp = temp + ".bak";
            try
            {
                using (var file = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    new DataContractJsonSerializer(typeof(FirstServiceSaveData)).WriteObject(file, data);
                    file.Flush(true);
                }

                bool primaryIsValid = TryRead(path, slot, out _);
                if (!primaryIsValid && !TryRead(backup, slot, out _))
                {
                    // 첫 저장도 복구할 수 있게 백업을 만든다. 손상된 원본으로 올바른 백업을 덮지 않는다.
                    File.Copy(temp, backupTemp);
                    ReplaceOrMove(backupTemp, backup, null);
                }
                ReplaceOrMove(temp, path, primaryIsValid ? backup : null);
                return data;
            }
            finally
            {
                File.Delete(temp);
                File.Delete(backupTemp);
            }
        }

        private static void ReplaceOrMove(string source, string destination, string backup)
        {
            if (File.Exists(destination))
                File.Replace(source, destination, backup);
            else
                File.Move(source, destination);
        }
    }
}
