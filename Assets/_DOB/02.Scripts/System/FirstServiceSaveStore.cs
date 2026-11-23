// 슬롯별 JSON을 같은 디렉터리의 임시 파일에 완전히 쓴 후 교체한다. 손상된 원본은 직전 백업으로 읽는다.
// 정산은 쓰기 잠금 안에서 저장 일차를 다시 읽고, 아직 처리하지 않은 방문일의 지급 후 상태만 기록한다.
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;

namespace DOB.Systems
{
    public sealed class FirstServiceSaveStore
    {
        private static readonly object WriteGate = new object();
        private readonly string directory;

        /// <summary>프리뷰 전용 저장 경로를 호출자가 정한다. 슬롯을 찾아보기만 할 때는 폴더를 만들지 않는다.</summary>
        public FirstServiceSaveStore(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentException("저장 경로가 필요합니다.", nameof(directory));
            this.directory = Path.GetFullPath(directory);
        }

        public bool TryLoad(FirstServiceSaveSlot slot, out FirstServiceSaveData data)
        {
            string path = SlotPath(slot);
            return TryRead(path, slot, out data) || TryRead(path + ".bak", slot, out data);
        }

        public bool TryLoadLatest(out FirstServiceSaveData data)
        {
            bool hasAuto = TryLoad(FirstServiceSaveSlot.Auto, out var automatic);
            bool hasManual = TryLoad(FirstServiceSaveSlot.Manual, out var manual);
            data = hasManual && (!hasAuto || manual.SavedAtUtc >= automatic.SavedAtUtc) ? manual : automatic;
            return data != null;
        }

        public FirstServiceSaveData SavePreparation(FirstServiceSaveSlot slot, int day, int gold, int garlicStock)
        {
            lock (WriteGate)
            using (LockDirectory())
                return Write(slot, day, gold, garlicStock);
        }

        /// <summary>수입 대신 지급이 끝난 절대 잔액을 받는다. 같은 날의 두 번째 호출은 기존 저장본을 반환한다.</summary>
        public FirstServiceSaveData SettleDay(int completedDay, int goldAfterPayment, int garlicAfterService)
        {
            if (completedDay < 1 || completedDay == int.MaxValue) throw new ArgumentOutOfRangeException(nameof(completedDay));
            lock (WriteGate)
            using (LockDirectory())
            {
                if (TryLoad(FirstServiceSaveSlot.Auto, out var previous) && previous.Day > completedDay)
                    return previous;
                return Write(FirstServiceSaveSlot.Auto, completedDay + 1, goldAfterPayment, garlicAfterService);
            }
        }

        private FileStream LockDirectory()
        {
            Directory.CreateDirectory(directory);
            return new FileStream(Path.Combine(directory, "save.lock"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        private string SlotPath(FirstServiceSaveSlot slot) => Path.Combine(directory, FirstServiceSaveData.SlotName(slot) + ".json");

        private static bool TryRead(string path, FirstServiceSaveSlot slot, out FirstServiceSaveData data)
        {
            data = null;
            try
            {
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
                {
                    var reader = new DataContractJsonSerializer(typeof(FirstServiceSaveData));
                    var candidate = (FirstServiceSaveData)reader.ReadObject(file);
                    if (candidate == null || !candidate.IsValidFor(slot)) return false;
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

        private FirstServiceSaveData Write(FirstServiceSaveSlot slot, int day, int gold, int garlicStock)
        {
            DateTime timestamp = DateTime.UtcNow;
            if (TryLoadLatest(out var latest) && timestamp <= latest.SavedAtUtc && latest.SavedAtUtc < DateTime.MaxValue)
                timestamp = latest.SavedAtUtc.AddTicks(1);
            var data = new FirstServiceSaveData(slot, day, gold, garlicStock, timestamp);
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
