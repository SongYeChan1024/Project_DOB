// RestaurantGreyboxSetup
// 역할: 아트 01 「바르코3D 제작 대상·외형 규격」의 건물 조각·가구 크기로 Restaurant 씬에 레이아웃을 만든다.
//       PF_Env_* 프리팹(바르코3D 모델)이 있는 조각은 프리팹을, 없는 조각은 같은 위치·크기의 임시 블록을 놓는다.
// 호출: 에디터 메뉴 DOB > Art > Build Environment Prefabs(모델 → 프리팹), DOB > Art > Build Restaurant Greybox(씬 배치).
// 호출 대상: Restaurant.unity, 04.Art Models/Environment·Textures·Materials, 03.Prefabs/Environment,
//           04.Art/Materials/Greybox 임시 재질, Temp/DOB_RestaurantPreview 미리보기 PNG.
// 상태: 없음. 실행할 때마다 Environment/ArtPlacement/Greybox를 지우고 다시 만든다.
// 주의: SceneLayoutSetup.Rebuild()는 Restaurant 씬을 새로 만들기 때문에 이 레이아웃도 지워진다. 그 뒤에는 이 메뉴를 다시 실행한다.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DOB.Editor
{
    /// <summary>Restaurant 씬의 건물 조각·가구를 규격 크기의 임시 블록으로 배치한다.</summary>
    public static class RestaurantGreyboxSetup
    {
        private const string ScenePath = "Assets/_DOB/01.Scenes/Restaurant.unity";
        private const string MaterialFolder = "Assets/_DOB/04.Art/Materials/Greybox";
        private const string PreviewFolder = "Temp/DOB_RestaurantPreview";
        private const string RootName = "Greybox";

        // 씬 골격에서 정한 동선 기준점. 이 메뉴는 위치를 검사만 하고 옮기지 않는다.
        private static readonly Dictionary<string, Vector3> ExpectedAnchors = new Dictionary<string, Vector3>
        {
            { "PlayerSpawn", new Vector3(0, 0, -1) },
            { "GuestEntrance", new Vector3(-4, 0, -2) },
            { "GreetingPoint", new Vector3(-2, 0, -1) },
            { "Kitchen", new Vector3(4, 0, 0) },
            { "StairsExit", new Vector3(4, 0, 4) },
        };

        /// <summary>
        /// 자리 처방 좌석(기획 7장·4-5). 좌석마다 고정 소품 자리가 하나 있고, 소품을 개량으로 사야 효과가 생긴다.
        /// start = 처음부터 열린 좌석(시작 3석). 나머지는 '좌석 늘리기' 개량으로 열린다.
        /// 💡 좌석-소품(효과) 짝과 위치는 아트 배치 제안이며, 실제 효과·가격·해금 순서는 08.Data 기획 데이터가 정한다.
        /// 좌석 기준점(LayoutAnchors/SeatNN)은 이 표의 위치로 맞추고 없으면 만든다. 의자 위치 = 좌석 기준점.
        /// table = 이 좌석이 함께 여는 테이블(Table 1인용 / TableLong 2인 단체 테이블 / null = 앞 좌석의 단체 테이블을 함께 씀).
        /// 한 좌석에는 손님 한 명(기획 4-5). 단체 테이블은 두 좌석이 한 테이블을 나눠 쓰는 모습이며, 앞 좌석이 열릴 때 테이블이 나온다.
        /// </summary>
        private static readonly (string id, Vector3 seat, bool start, string table, Vector3 tablePos, string prop, Vector3 propPos, float propYaw)[] Seats =
        {
            // 앞줄(시작 3석): 1인석 3개. 2026-11-25 이기혁이 테이블·의자를 1.4m 뒤(의자 z 2.8)로 옮긴 배치를 따른다.
            // 소품은 의자와 겹치지 않게 좌석 옆으로 둔다. 창가 커튼은 창(z 0~2)에 붙어 있어 그대로 둔다.
            ("Seat01", new Vector3(-4.3f, 0, 2.8f), true, "Table", new Vector3(-4.3f, 0, 2.0f), "Curtain", new Vector3(-5.85f, 0.3f, 1f), -90), // 바람 드는 창가 · 내열
            ("Seat02", new Vector3(-1.8f, 0, 2.8f), true, "Table", new Vector3(-1.8f, 0, 2.0f), "HerbPot", new Vector3(-2.9f, 0, 2.9f), 0),     // 약초 화분 옆 · 해독
            ("Seat03", new Vector3(0.7f, 0, 2.8f), true, "Table", new Vector3(0.7f, 0, 2.0f), "GlowLamp", new Vector3(1.6f, 0, 3.0f), 0),       // 빛나는 장식 · 야광
            // 뒷줄(좌석 늘리기로 열림): 난로 구석 1인석 + 2인 단체 테이블. 앞줄 의자(z 2.8)와 겹치지 않게 1m 뒤(의자 z 5.0)에 둔다.
            ("Seat04", new Vector3(-4.3f, 0, 5f), false, "Table", new Vector3(-4.3f, 0, 4.2f), "Stove", new Vector3(-5.2f, 0, 5.3f), 0),       // 난로 옆 · 방한
            ("Seat05", new Vector3(-1.2f, 0, 5f), false, "TableLong", new Vector3(-0.6f, 0, 4.2f), "Sofa", new Vector3(-1.2f, 0, 5f), 0),      // 푹신한 소파 · 활력(의자를 대신함)
            // 칸막이 천 면(-Z)이 Seat06 쪽(-X)을 보도록 90°로 돌려 단체 테이블 오른쪽 끝을 막는다.
            ("Seat06", new Vector3(0f, 0, 5f), false, null, Vector3.zero, "Booth", new Vector3(0.9f, 0, 5f), 90),                              // 조용한 칸막이석 · 감각
        };

        // 소파는 의자 자리에 놓이므로 소파가 보이면 그 좌석의 의자를 숨긴다.
        private const string ChairReplacingProp = "Sofa";

        // 개량 후에만 보이는 주방 조각. 실제 화구 수(조리 로직)는 08.Data·개발의 해금 연결이 정한다.
        private static readonly string[] UpgradeOnlyKitchen = { "Stove_02" };

        // 아트 01 2-1의 건물 격자(m). 바닥은 x -6~6, z -4~6 범위를 덮는다.
        private const float Grid = 2f;
        private const float MinX = -6f, MaxX = 6f, MinZ = -4f, MaxZ = 6f;

        private static Dictionary<string, Material> materials;

        [MenuItem("DOB/Art/Build Restaurant Greybox")]
        public static void Build()
        {
            Require(!EditorApplication.isPlaying, "플레이 중에는 식당 레이아웃을 만들 수 없습니다.");
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            ValidateAnchors(scene);
            ApplySeatAnchors(scene);
            var artPlacement = FindPath(scene, "Environment/ArtPlacement");
            Require(artPlacement != null, "Environment/ArtPlacement 누락");

            var old = artPlacement.Find(RootName);
            if (old != null)
                UnityEngine.Object.DestroyImmediate(old.gameObject);
            // 골격의 임시 Plane은 블록 바닥과 같은 높이라 겹쳐 깜빡이므로 지운다.
            var plane = FindPath(scene, "Environment/Floor");
            if (plane != null)
                UnityEngine.Object.DestroyImmediate(plane.gameObject);

            LoadMaterials();
            var root = Group(artPlacement, RootName);
            BuildFloor(Group(root, "Floor"));
            BuildWalls(Group(root, "Walls"));
            BuildStairs(Group(root, "Stairs"));
            BuildKitchen(Group(root, "Kitchen"));
            BuildDining(Group(root, "Dining"));
            BuildDecor(Group(root, "Decor"));

            // 확인 화면은 '전부 개량' 상태를 먼저 찍고, 씬은 업그레이드 전 시작 상태(1인석 3개, 소품 없음)로 저장한다.
            // 2026-11-25 이기혁 요청으로 11-23의 '전부 개량 저장'을 바꿨다. 게임 화면이 처음 영업 모습과 같아야 하기 때문이다.
            SetSeatState(scene, true);
            CapturePreview("GameplayCamera", new Vector3(0, 8, -8), new Vector3(35, 0, 0));
            CapturePreview("Overview", new Vector3(-9, 12, -11), new Vector3(38, 37, 0));
            SetSeatState(scene, false);
            CapturePreview("Start_GameplayCamera", new Vector3(0, 8, -8), new Vector3(35, 0, 0));
            CapturePreview("Start_Overview", new Vector3(-9, 12, -11), new Vector3(38, 37, 0));

            EditorSceneManager.MarkSceneDirty(scene);
            Require(EditorSceneManager.SaveScene(scene), "Restaurant 씬 저장 실패");
            ValidateAnchors(scene);
            Require(scene.GetRootGameObjects().All(o => o.GetComponentsInChildren<Canvas>(true).Length == 0), "Restaurant에 Canvas가 포함됨");
            Debug.Log("DOB_RESTAURANT_GREYBOX_BUILT: " + root.GetComponentsInChildren<Transform>(true).Length + " transforms. Preview: " + Path.GetFullPath(PreviewFolder));
        }

        // ── 건물 ─────────────────────────────────────────────

        private static void BuildFloor(Transform parent)
        {
            // 2×0.2×2 바닥 타일. 원점은 바닥면 모서리, 윗면이 y=0이 되도록 0.2 내린다.
            // 레퍼런스에 맞춰 식당 안쪽 바닥은 나무로 한다(바깥 기단·계단은 돌).
            var index = 0;
            for (var x = MinX; x < MaxX; x += Grid)
                for (var z = MinZ; z < MaxZ; z += Grid, index++)
                {
                    // 격자 단위가 눈에 보이도록 두 색을 번갈아 쓴다.
                    var mat = ((int)((x - MinX) / Grid) + (int)((z - MinZ) / Grid)) % 2 == 0 ? "FloorWood" : "FloorWoodLight";
                    var name = "FloorTile_" + index.ToString("00");
                    var corner = new Vector3(x, -0.2f, z);
                    if (PlaceEnv(parent, "FloorTile", name, corner, 0) == null)
                        CornerPiece(parent, name, corner, 0, new Vector3(2, 0.2f, 2), mat);
                }
            // 💡 층계참 아래 돌 기단. 1-3 목록 밖의 분위기용 조각이며, 계단 높이 차 3m에 맞춘다.
            if (PlaceEnv(parent, "Foundation", "Foundation", new Vector3(MinX, -3.2f, MinZ), 0) == null)
                Visual(parent, "Foundation", new Vector3((MinX + MaxX) / 2, -1.7f, (MinZ + MaxZ) / 2 + 0.2f), new Vector3(MaxX - MinX, 3f, MaxZ - MinZ + 0.4f), "StoneDark");
        }

        private static void BuildWalls(Transform parent)
        {
            // 뒤쪽 벽: 오른쪽 끝 x 4~6은 던전 출구로 비운다.
            for (var x = MinX; x < 4f; x += Grid)
                Wall(parent, "Wall_Back_" + x, new Vector3(x, 0, MaxZ), 0);
            // 왼쪽 벽: 앞쪽 z -4~-2는 입장 계단으로 비우고, z 0~2는 창가 좌석(Seat01) 옆 창 달린 벽.
            for (var z = -2f; z < MaxZ; z += Grid)
            {
                if (Mathf.Approximately(z, 0f))
                    WindowWall(parent, "WallWindow_Left_" + z, new Vector3(MinX, 0, z), -90);
                else
                    Wall(parent, "Wall_Left_" + z, new Vector3(MinX, 0, z), -90);
            }
            // 오른쪽 벽: 회전 90°에서 로컬 +X가 월드 -Z이므로 원점은 각 칸의 뒤쪽 모서리다.
            for (var z = MinZ; z < MaxZ; z += Grid)
                Wall(parent, "Wall_Right_" + z, new Vector3(MaxX, 0, z + Grid), 90);

            // 나무 기둥은 벽 두께 중앙의 뒤 모서리와 입장 계단 옆에 세운다.
            // 앞 모서리 기둥은 Gameplay 카메라 화면 아래쪽을 가려 두지 않는다(2026-11-23 결정).
            Pillar(parent, "Pillar_BackLeft", new Vector3(MinX - 0.2f, 0, MaxZ + 0.2f));
            // 입장 계단의 벽 쪽 난간(z -2.28~-2.03)이 기둥에 박히지 않게 기둥을 벽 쪽으로 0.3m 옮긴다(기둥 z -2.0~-1.4).
            Pillar(parent, "Pillar_EntranceBack", new Vector3(MinX - 0.2f, 0, -1.7f));

            // 벽 이음새 기둥: 바르코3D 벽은 가장자리가 둥글어 2m 이음새마다 틈이 보이므로 가는 나무 기둥으로 가린다.
            // 벽 안쪽 면(두께 0.4m 중 실내 쪽)에 걸치도록 중심을 벽 면에서 0.1m 바깥에 둔다.
            // 모서리 큰 기둥(뒤 왼쪽, 입장 계단 옆)과 아치 기둥이 있는 자리는 뺀다.
            for (var x = MinX + Grid; x < 4f; x += Grid)
                WallPost(parent, "WallPost_Back_" + x, new Vector3(x, 0, MaxZ + 0.1f));
            for (var z = 0f; z < MaxZ; z += Grid)
                WallPost(parent, "WallPost_Left_" + z, new Vector3(MinX - 0.1f, 0, z));
            for (var z = MinZ; z < MaxZ; z += Grid)
                WallPost(parent, "WallPost_Right_" + z, new Vector3(MaxX + 0.1f, 0, z));

            // 앞 가장자리 난간. 💡 카메라 쪽 화면을 덜 가리도록 규격의 1m보다 낮은 0.6m로 둔다.
            for (var x = MinX; x < MaxX; x += Grid)
                Railing(parent, "Railing_Front_" + x, new Vector3(x, 0, MinZ));
        }

        /// <summary>2×3×0.4m 회벽. 레퍼런스처럼 윗면에 나무 보를 얹는다.</summary>
        private static void Pillar(Transform parent, string name, Vector3 position)
        {
            if (PlaceEnv(parent, "Pillar", name, position, 0) == null)
                CenterPiece(parent, name, position, 0, new Vector3(0.6f, 3, 0.6f), "WoodDark");
        }

        /// <summary>벽 이음새를 가리는 0.3×3×0.3m 나무 기둥. 충돌은 벽이 맡는다.</summary>
        private static void WallPost(Transform parent, string name, Vector3 position)
        {
            if (PlaceEnv(parent, "WallPost", name, position, 0) == null)
                Visual(parent, name, position + new Vector3(0, 1.5f, 0), new Vector3(0.3f, 3, 0.3f), "WoodDark");
        }

        private static void Wall(Transform parent, string name, Vector3 corner, float yaw)
        {
            if (PlaceEnv(parent, "Wall", name, corner, yaw) != null)
                return;
            var root = CornerPiece(parent, name, corner, yaw, new Vector3(2, 3, 0.4f), "Plaster");
            WallBeam(root);
        }

        private static void WallBeam(Transform wall)
        {
            // 보 윗면을 벽 윗면(3m)보다 2cm 올려 두 면이 겹쳐 깜빡이지 않게 한다.
            Visual(wall, "Beam", new Vector3(1, 2.92f, 0.2f), new Vector3(2, 0.2f, 0.5f), "WoodDark");
        }

        private static void WindowWall(Transform parent, string name, Vector3 corner, float yaw)
        {
            if (PlaceEnv(parent, "WindowWall", name, corner, yaw) != null)
                return;
            var root = CornerPiece(parent, name, corner, yaw, new Vector3(2, 3, 0.4f), null);
            // 창 둘레를 회벽으로 채우고 가운데 1.2×1.2m 유리를 나무 창틀로 두른다.
            Visual(root, "Sill", new Vector3(1, 0.5f, 0.2f), new Vector3(2, 1, 0.4f), "Plaster");
            Visual(root, "Head", new Vector3(1, 2.6f, 0.2f), new Vector3(2, 0.8f, 0.4f), "Plaster");
            Visual(root, "JambA", new Vector3(0.2f, 1.6f, 0.2f), new Vector3(0.4f, 1.2f, 0.4f), "Plaster");
            Visual(root, "JambB", new Vector3(1.8f, 1.6f, 0.2f), new Vector3(0.4f, 1.2f, 0.4f), "Plaster");
            Visual(root, "Frame", new Vector3(1, 1.6f, 0.2f), new Vector3(1.3f, 1.3f, 0.3f), "WoodDark");
            Visual(root, "Glass", new Vector3(1, 1.6f, 0.2f), new Vector3(1.2f, 1.2f, 0.34f), "Glass");
            WallBeam(root);
        }

        private static void Railing(Transform parent, string name, Vector3 corner)
        {
            if (PlaceEnv(parent, "Railing", name, corner, 0) != null)
                return;
            var root = CornerPiece(parent, name, corner, 0, new Vector3(2, 0.6f, 0.3f), null);
            Visual(root, "TopRail", new Vector3(1, 0.54f, 0.15f), new Vector3(2, 0.12f, 0.3f), "WoodDark");
            Visual(root, "MidRail", new Vector3(1, 0.27f, 0.15f), new Vector3(2, 0.06f, 0.12f), "WoodDark");
            Visual(root, "PostA", new Vector3(0.08f, 0.24f, 0.15f), new Vector3(0.16f, 0.48f, 0.16f), "WoodDark");
            Visual(root, "PostB", new Vector3(1.92f, 0.24f, 0.15f), new Vector3(0.16f, 0.48f, 0.16f), "WoodDark");
        }

        private static void BuildStairs(Transform parent)
        {
            // 입장 계단: 왼쪽 앞 z -4~-2에서 바깥(-X)으로 4m 동안 3m 오른다. 회전 180°로 로컬 +X를 월드 -X로 둔다.
            if (PlaceEnv(parent, "Stairs", "Stairs_Entrance", new Vector3(MinX, 0, -2f), 180) == null)
                Stair(parent, "Stairs_Entrance", new Vector3(MinX, 0, -2f), 180, true);
            // 던전 출구 계단: 뒤 오른쪽 x 4~6에서 바깥(+Z)으로 4m 동안 3m 내려간다.
            // 계단 모델은 낮은 끝이 원점이므로, 낮은 끝을 바깥 4m·아래 3m에 두고 회전 90°로 층계참 쪽을 향해 오르게 한다.
            if (PlaceEnv(parent, "Stairs", "Stairs_DungeonExit", new Vector3(4f, -3f, MaxZ + 4f), 90) == null)
                Stair(parent, "Stairs_DungeonExit", new Vector3(MaxX, 0, MaxZ), -90, false);

            // 아치 3×3.5m를 출구 계단 첫머리에 얹는다. 기둥 사이 2m가 계단 폭과 같다.
            if (PlaceEnv(parent, "Arch", "Arch_DungeonExit", new Vector3(3.5f, 0, MaxZ), 0) != null)
                return;
            var arch = Group(parent, "Arch_DungeonExit");
            arch.localPosition = new Vector3(3.5f, 0, MaxZ);
            Visual(arch, "PostA", new Vector3(0.25f, 1.5f, 0.2f), new Vector3(0.5f, 3, 0.4f), "WoodDark");
            Visual(arch, "PostB", new Vector3(2.75f, 1.5f, 0.2f), new Vector3(0.5f, 3, 0.4f), "WoodDark");
            Visual(arch, "Lintel", new Vector3(1.5f, 3.25f, 0.2f), new Vector3(3, 0.5f, 0.4f), "WoodDark");
        }

        /// <summary>폭 2m, 길이 4m, 높이 차 3m의 계단. 원점은 층계참과 만나는 바닥 모서리다.</summary>
        private static void Stair(Transform parent, string name, Vector3 corner, float yaw, bool ascending)
        {
            const int steps = 10;
            const float run = 0.4f, rise = 0.3f, width = 2f;
            // 계단과 난간은 걷는 충돌체를 아트 4순위(기준점·충돌) 계약에서 정하므로 외형만 둔다.
            var root = Group(parent, name);
            root.localPosition = corner;
            root.localRotation = Quaternion.Euler(0, yaw, 0);
            for (var i = 0; i < steps; i++)
            {
                var top = ascending ? rise * (i + 1) : -rise * (i + 1);
                var bottom = ascending ? -0.2f : -3.2f;
                var height = top - bottom;
                Visual(root, "Step_" + i.ToString("00"), new Vector3(run * (i + 0.5f), bottom + height / 2, width / 2), new Vector3(run, height, width), i % 2 == 0 ? "Stone" : "StoneLight");
            }
            // 양옆 난간: 길이 5m(4m·3m 경사), 디딤판 위 0.9m.
            var length = Mathf.Sqrt(16f + 9f);
            var angle = Mathf.Atan2(3f, 4f) * Mathf.Rad2Deg * (ascending ? 1 : -1);
            foreach (var side in new[] { 0.06f, width - 0.06f })
            {
                var rail = Visual(root, "Handrail_" + (side < 1 ? "A" : "B"), new Vector3(2f, (ascending ? 1.5f : -1.5f) + 0.9f, side), new Vector3(length, 0.1f, 0.1f), "WoodDark");
                rail.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }

        // ── 주방 (Kitchen 4,0 · 오른쪽 벽) ───────────────────

        private static void BuildKitchen(Transform parent)
        {
            // 주방 앞쪽 칸막이: 응대 구역과 주방을 쿼터뷰에서 바로 구분한다. 2m 단위.
            // 앞 난간(z -4~-3.7) 안쪽에서 시작해 난간과 겹치지 않게 한다. 끝(z -1.7)은 픽업대(z -1.6~) 앞에서 멈춘다.
            var dividerCorner = new Vector3(3f, 0, MinZ + 0.3f);
            if (PlaceEnv(parent, "KitchenDivider", "Divider_Kitchen", dividerCorner, -90) == null)
                CornerPiece(parent, "Divider_Kitchen", dividerCorner, -90, new Vector3(2, 1.2f, 0.2f), "Wood");

            // 주방 가구 모델은 길이가 로컬 X·앞면이 -Z이므로 90° 돌려 오른쪽 벽을 따라 놓고 앞면이 홀(-X)을 보게 한다.
            // 재료 투입 조리대(높이 0.9m)와 기본 재료(밀가루·감자·고기). Kitchen 기준점이 조리대 앞에 온다.
            if (PlaceEnv(parent, "KitchenCounter", "CookingCounter", new Vector3(5.6f, 0, 0), 90) == null)
            {
                var counter = CenterPiece(parent, "CookingCounter", new Vector3(5.6f, 0, 0), 0, new Vector3(0.8f, 0.9f, 2f), "Wood");
                Visual(counter, "IngredientBox_Flour", new Vector3(0, 1.1f, -0.6f), new Vector3(0.4f, 0.4f, 0.4f), "Flour");
                Visual(counter, "IngredientBox_Potato", new Vector3(0, 1.1f, 0), new Vector3(0.4f, 0.4f, 0.4f), "Potato");
                Visual(counter, "IngredientBox_Meat", new Vector3(0, 1.1f, 0.6f), new Vector3(0.4f, 0.4f, 0.4f), "Meat");
            }

            // 화구 2개와 그 위 후드. 시작 상태에서는 Stove_02를 숨긴다(UpgradeOnlyKitchen, SetSeatState).
            for (var i = 0; i < 2; i++)
            {
                var name = "Stove_" + (i + 1).ToString("00");
                // 오른쪽 벽면(x 6.0)과 벽 이음새 기둥(x 5.95~, z 2.0±0.15)에 박히지 않고 조리대(z ~1.0)에 붙도록 둔다.
                var position = new Vector3(5.52f, 0, 1.45f + i * 1.15f);
                var range = PlaceEnv(parent, "CookingRange", name, position, 90);
                if (range != null)
                {
                    PlacePotOnRange(range);
                    continue;
                }
                var stove = CenterPiece(parent, name, position, 0, new Vector3(0.8f, 0.9f, 0.9f), "Metal");
                Visual(stove, "Burner", new Vector3(0, 0.91f, 0), new Vector3(0.5f, 0.02f, 0.5f), "Fire");
            }
            // 후드 모델은 길이가 로컬 X이므로 90° 돌려 화구 줄(Z 방향)을 덮는다. 원점은 바닥 중앙(높이 2.2m).
            if (PlaceEnv(parent, "Hood", "Hood", new Vector3(5.55f, 2.2f, 2.05f), 90) == null)
                Visual(parent, "Hood", new Vector3(5.55f, 2.5f, 2.05f), new Vector3(0.9f, 0.6f, 2.1f), "Metal");

            // 양념 선반: 첫날 마늘·소금만 보인다.
            // 벽면(x 6.0)과 벽 이음새 기둥(z -2.0±0.15)에서 떼어 둔다(선반 z -3.5~-2.2).
            var shelfPosition = new Vector3(5.77f, 0, -2.85f);
            if (PlaceEnv(parent, "SpiceShelf", "SpiceShelf", shelfPosition, 90) == null)
            {
                var shelf = CenterPiece(parent, "SpiceShelf", shelfPosition, 0, new Vector3(0.4f, 1.8f, 1.4f), "WoodDark");
                Visual(shelf, "Jar_Garlic", new Vector3(-0.1f, 1.3f, -0.3f), new Vector3(0.18f, 0.22f, 0.18f), "Flour");
                Visual(shelf, "Jar_Salt", new Vector3(-0.1f, 1.3f, 0.3f), new Vector3(0.18f, 0.22f, 0.18f), "Glass");
            }

            // 완성 접시 픽업대: 주방 앞, 홀 쪽을 향한다. 접시 기준점 위치는 아트 4순위에서 확정한다.
            var pickup = PlaceEnv(parent, "PickupCounter", "PickupCounter", new Vector3(3.2f, 0, -1f), 90)
                ?? CenterPiece(parent, "PickupCounter", new Vector3(3.2f, 0, -1f), 0, new Vector3(0.6f, 0.9f, 1.2f), "Wood");
            var platePoint = new GameObject("PlatePoint").transform;
            platePoint.SetParent(pickup, false);
            platePoint.localPosition = new Vector3(0, 0.9f, 0);
        }

        // ── 좌석 (Seat01~06) · 자리 소품 ─────────────────────

        /// <summary>
        /// 좌석마다 Dining/SeatNN/Furniture(테이블·의자)와 Dining/SeatNN/EffectProp_{소품}을 만든다.
        /// 의자는 좌석 기준점에서 카메라(-Z)를 보고, 테이블은 그 앞 0.8m에 둔다(쿼터뷰에서 손님 얼굴이 보이게).
        /// 보이기·숨기기는 SetSeatState(미리보기)와 개발의 해금 연결이 맡는다.
        /// </summary>
        private static void BuildDining(Transform parent)
        {
            foreach (var seat in Seats)
            {
                var group = Group(parent, seat.id);
                var furniture = Group(group, "Furniture");
                if (PlaceEnv(furniture, "Chair", "Chair", seat.seat, 0) == null)
                    Chair(furniture, "Chair", seat.seat, 180);
                // 단체 테이블의 두 번째 좌석은 의자만 둔다(테이블은 앞 좌석 Furniture에 있음).
                if (seat.table != null && PlaceEnv(furniture, seat.table, seat.table, seat.tablePos, 0) == null)
                    Table(furniture, seat.table, seat.tablePos, EnvPieces.First(p => p.kind == seat.table).size);

                var propName = "EffectProp_" + seat.prop;
                var prop = PlaceEnv(group, seat.prop, propName, seat.propPos, seat.propYaw) ?? PropBlock(group, seat.prop, propName, seat.propPos, seat.propYaw);
                // 난로·빛나는 장식은 효과가 화면에서 바로 읽히도록 소품과 함께 켜지는 빛을 단다.
                if (seat.prop == "Stove")
                    EffectLight(prop, new Vector3(0, 0.6f, -0.5f), new Color(1f, 0.55f, 0.25f), 3.5f, 2f);
                if (seat.prop == "GlowLamp")
                    EffectLight(prop, new Vector3(0, 1.1f, 0), new Color(0.6f, 0.7f, 1f), 3.5f, 2f);
            }
        }

        /// <summary>소품 프리팹이 없을 때 같은 자리·대략적 크기로 놓는 임시 블록.</summary>
        private static Transform PropBlock(Transform parent, string kind, string name, Vector3 position, float yaw)
        {
            var size = EnvPieces.First(p => p.kind == kind).size;
            var color = kind == "Stove" ? "Metal" : kind == "HerbPot" ? "Marker" : kind == "GlowLamp" ? "Glass" : kind == "Curtain" ? "Meat" : "Wood";
            return CenterPiece(parent, name, position, yaw, size, color);
        }

        private static void EffectLight(Transform prop, Vector3 localPosition, Color color, float range, float intensity)
        {
            var go = new GameObject("Light_Effect", typeof(Light));
            go.transform.SetParent(prop, false);
            go.transform.localPosition = localPosition;
            var light = go.GetComponent<Light>();
            light.type = LightType.Point;
            light.range = range;
            light.intensity = intensity;
            light.color = color;
        }

        /// <summary>
        /// 좌석 기준점을 Seats 표의 위치로 맞추고, 없는 좌석(Seat04~06)은 새로 만든다.
        /// 동선 기준점(입장·응대·주방·출구·스폰)은 건드리지 않는다.
        /// </summary>
        private static void ApplySeatAnchors(Scene scene)
        {
            var layout = scene.GetRootGameObjects().Single(o => o.name == "LayoutAnchors").transform;
            foreach (var seat in Seats)
            {
                var anchor = layout.Find(seat.id);
                if (anchor == null)
                {
                    anchor = new GameObject(seat.id).transform;
                    anchor.SetParent(layout, false);
                }
                anchor.localPosition = seat.seat;
            }
        }

        /// <summary>미리보기: false = 게임 시작(시작 좌석 가구·화구 1개만), true = 좌석·소품·화구 개량을 모두 마친 상태.</summary>
        private static void SetSeatState(Scene scene, bool full)
        {
            var dining = FindPath(scene, "Environment/ArtPlacement/" + RootName + "/Dining");
            Require(dining != null, "Dining 그룹 누락 — 먼저 Build Restaurant Greybox를 실행하세요.");
            foreach (var seat in Seats)
            {
                var group = dining.Find(seat.id);
                group.Find("Furniture").gameObject.SetActive(seat.start || full);
                var prop = group.Find("EffectProp_" + seat.prop);
                prop.gameObject.SetActive(full);
                if (seat.prop == ChairReplacingProp)
                    group.Find("Furniture/Chair").gameObject.SetActive(!full);
            }
            // 화구도 처음엔 1개, 개량하면 2개(2026-11-25 이기혁 배치 요청). 후드는 두 화구 자리를 함께 덮는 그대로 둔다.
            var kitchen = FindPath(scene, "Environment/ArtPlacement/" + RootName + "/Kitchen");
            foreach (var name in UpgradeOnlyKitchen)
            {
                var piece = kitchen.Find(name);
                Require(piece != null, "주방 개량 조각 누락: " + name);
                piece.gameObject.SetActive(full);
            }
            EditorSceneManager.MarkSceneDirty(scene);
        }

        [MenuItem("DOB/Art/Preview Seats/Start (3 seats, 1 stove, no props)")]
        public static void PreviewSeatsStart() => SetSeatState(SceneManager.GetActiveScene(), false);

        [MenuItem("DOB/Art/Preview Seats/Full Upgrade (6 seats, 2 stoves, 6 props)")]
        public static void PreviewSeatsFull() => SetSeatState(SceneManager.GetActiveScene(), true);

        private static void Table(Transform parent, string name, Vector3 position, Vector3 size)
        {
            // 상판 높이 0.75m (아트 01 2-1 가구 기준). 1인용·단체 테이블 모두 같은 높이.
            var root = CenterPiece(parent, name, position, 0, size, null);
            Visual(root, "Top", new Vector3(0, 0.72f, 0), new Vector3(size.x, 0.06f, size.z), "Wood");
            Visual(root, "Leg", new Vector3(0, 0.345f, 0), new Vector3(0.15f, 0.69f, 0.15f), "WoodDark");
            Visual(root, "Base", new Vector3(0, 0.02f, 0), new Vector3(0.5f, 0.04f, 0.5f), "WoodDark");
        }

        private static void Chair(Transform parent, string name, Vector3 position, float yaw)
        {
            // 앉는 면 0.45m. 등받이는 로컬 -Z(앉은 사람 뒤)에 둔다.
            var root = CenterPiece(parent, name, position, yaw, new Vector3(0.45f, 0.95f, 0.45f), null);
            Visual(root, "Legs", new Vector3(0, 0.195f, 0), new Vector3(0.38f, 0.39f, 0.38f), "WoodDark");
            Visual(root, "Seat", new Vector3(0, 0.42f, 0), new Vector3(0.45f, 0.06f, 0.45f), "Wood");
            Visual(root, "Back", new Vector3(0, 0.7f, -0.195f), new Vector3(0.45f, 0.5f, 0.06f), "Wood");
        }

        // ── 응대 표시·분위기 ─────────────────────────────────

        private static void BuildDecor(Transform parent)
        {
            // 장식에는 식물·불·빛나는 결정·쿠션·칸막이·커튼을 쓰지 않는다. 자리 효과 소품과 헷갈리지 않게 하기 위해서다.

            // GreetingPoint 표시: 환영 깔개. E 응대 자리가 눈에 띄게 한다.
            var greeting = ExpectedAnchors["GreetingPoint"];
            if (PlaceEnv(parent, "WelcomeRug", "GreetingMarker", greeting, 0) == null)
                Visual(parent, "GreetingMarker", greeting + new Vector3(0, 0.01f, 0), new Vector3(0.9f, 0.02f, 0.9f), "Marker", PrimitiveType.Cylinder);
            // 재료 주문 장부(2026-11-25 이기혁 배치): 주방 칸막이(x 2.8) 앞에 오른쪽 옆면을 3cm 띄워 붙인 계산대 자리.
            // 장부를 펼친 면이 카메라 쪽(-Z)이고 재료 견본 상자가 홀 쪽(-X)이라 게임 카메라에서 함께 보인다.
            // 앞 난간(z -3.7)까지 0.82m, 픽업대(z -1.6)까지 0.42m를 비워 둔다.
            DecorPiece(parent, "OrderLedger", "OrderLedger", new Vector3(2.14f, 0, -2.45f), 0, "Wood");
            // 방명록: 손님이 식사 후 글을 남기고 던전으로 내려가므로(기획 MainPlanning 5. 서빙) 출구 계단 아치 왼쪽,
            // 통 더미와 아치 기둥 사이에 둔다. 펼친 면(-Z)이 출구 동선(+X)을 보도록 -90° 돌린다.
            DecorPiece(parent, "ReceptionPodium", "Guestbook", new Vector3(3.3f, 0, 5.2f), -90, "WoodDark");

            // 입구 쪽 왼쪽 벽 게시판(준비 화면 '오늘의 소식'과 같은 물건). 벽걸이라 바닥에서 0.9m 띄운다.
            // 입구 기둥(z -1.4까지)과 벽 이음새 기둥(z -0.15부터) 사이 칸 가운데.
            DecorPiece(parent, "NoticeBoard", "NoticeBoard", new Vector3(MinX + 0.05f, 0.9f, -0.78f), -90, "Wood");
            // 뒤 벽 단골 물건 진열 선반(기획: 단골 사연 보상 물건을 식당에 진열).
            // 깊이 0.45m 선반의 뒷면이 뒷벽(z 6.0)에 박히지 않게 0.23m 앞에 둔다.
            DecorPiece(parent, "DisplayShelf", "DisplayShelf_Regulars", new Vector3(-1f, 0, MaxZ - 0.23f), 0, "WoodDark");
            // 뒤 오른쪽 통 더미. 던전 출구 아치(x 3.5~6.5) 통로 밖에 둔다.
            // 통 더미는 1.31m 정사각이라 뒤 벽(z 6)에 닿지 않게 0.7m 띄운다.
            DecorPiece(parent, "BarrelSet", "BarrelSet", new Vector3(2.3f, 0, MaxZ - 0.7f), 0, "Wood");

            // 분위기용 벽난로는 두지 않는다. 불·열기는 Seat04의 '난로' 효과 소품만 보여 줘야 효과가 헷갈리지 않는다.

            // 벽 등잔: 따뜻한 점광원. 벽 이음새 기둥(2m 간격)·게시판·창과 겹치지 않게 벽 칸 가운데에 둔다.
            // (벽 쪽 바닥 위치, 앞면이 실내를 보게 하는 회전)
            var lanterns = new[]
            {
                (new Vector3(MinX + 0.15f, 1.8f, 3f), -90f), (new Vector3(MinX + 0.15f, 1.8f, 5f), -90f),
                (new Vector3(-3.5f, 1.8f, MaxZ - 0.15f), 0f), (new Vector3(3f, 1.8f, MaxZ - 0.15f), 0f),
                (new Vector3(MaxX - 0.15f, 1.8f, -1f), 90f),
            };
            for (var i = 0; i < lanterns.Length; i++)
            {
                var (position, yaw) = lanterns[i];
                var name = "Lantern_" + (i + 1).ToString("00");
                if (PlaceEnv(parent, "WallLantern", name, position, yaw) == null)
                    Visual(parent, name, position + new Vector3(0, 0.4f, 0), new Vector3(0.2f, 0.3f, 0.2f), "Fire");
                // 빛은 등잔 유리 높이에서 벽보다 조금 실내 쪽으로 낸다.
                var inward = Quaternion.Euler(0, yaw, 0) * new Vector3(0, 0, -0.25f);
                PointLight(parent, "Light_Lantern_" + (i + 1).ToString("00"), position + new Vector3(0, 0.35f, 0) + inward, 4.5f, 1.1f);
            }
        }

        /// <summary>장식 조각: 프리팹이 있으면 프리팹, 없으면 표의 크기로 임시 블록을 놓는다.</summary>
        private static Transform DecorPiece(Transform parent, string kind, string name, Vector3 position, float yaw, string fallbackMaterial)
        {
            return PlaceEnv(parent, kind, name, position, yaw)
                ?? CenterPiece(parent, name, position, yaw, EnvPieces.First(p => p.kind == kind).size, fallbackMaterial);
        }

        private static void PointLight(Transform parent, string name, Vector3 position, float range, float intensity)
        {
            var go = new GameObject(name, typeof(Light));
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            var light = go.GetComponent<Light>();
            light.type = LightType.Point;
            light.range = range;
            light.intensity = intensity;
            light.color = new Color(1f, 0.72f, 0.42f);
        }

        // ── 바르코3D 환경 프리팹 ─────────────────────────────

        private const string EnvModelFolder = "Assets/_DOB/04.Art/Models/Environment/";
        private const string EnvTextureFolder = "Assets/_DOB/04.Art/Textures/";
        private const string EnvMaterialFolder = "Assets/_DOB/04.Art/Materials/";
        private const string EnvPrefabFolder = "Assets/_DOB/03.Prefabs/Environment/";

        // 아트 01 1-3 건물 조각 크기(가로 X × 높이 Y × 두께 Z, m)와 원점 방식.
        // corner = 바닥면 한쪽 모서리(건물 격자), center = 바닥 중앙. 계단·아치·후드는 충돌체를 아트 4순위에서 정한다.
        private static readonly (string kind, Vector3 size, bool corner, bool collider)[] EnvPieces =
        {
            ("FloorTile", new Vector3(2, 0.2f, 2), true, true),
            ("Wall", new Vector3(2, 3, 0.4f), true, true),
            ("WindowWall", new Vector3(2, 3, 0.4f), true, true),
            ("Pillar", new Vector3(0.6f, 3, 0.6f), false, true),
            ("Railing", new Vector3(2, 0.6f, 0.3f), true, true),
            ("Stairs", new Vector3(4, 3, 2), true, false),
            ("Arch", new Vector3(3, 3.5f, 0.4f), true, false),
            ("KitchenDivider", new Vector3(2, 1.2f, 0.2f), true, true),
            // 후드는 굴뚝 목까지 0.8m(화구 위 2.2m에서 벽 윗면 3m까지).
            ("Hood", new Vector3(2.1f, 0.8f, 0.9f), false, false),
            // 기단은 Blender에서 직접 만든 상자(2m마다 반복되는 UV)이고 바닥 아래라 충돌체가 필요 없다.
            ("Foundation", new Vector3(12, 3, 10.4f), true, false),
            // 좌석 가구·자리 소품(바닥 중앙 원점, 앞면이 -Z). 단단한 가구는 높이 기준 균일 축소한 실측값이다.
            // 높이: 테이블 상판 0.75, 의자 0.95(앉는 면 약 0.45), 소파 0.72 / 커튼·칸막이는 창·좌석 길이에 맞춰 늘린 값.
            ("Table", new Vector3(0.85f, 0.75f, 0.85f), false, true),
            ("TableLong", new Vector3(2.2f, 0.75f, 0.9f), false, true),
            ("Chair", new Vector3(0.54f, 0.95f, 0.57f), false, true),
            ("Curtain", new Vector3(2.2f, 2.3f, 0.25f), false, false),
            ("HerbPot", new Vector3(0.87f, 1f, 0.76f), false, true),
            ("Booth", new Vector3(1.6f, 1.5f, 0.25f), false, true),
            ("Stove", new Vector3(0.71f, 1.2f, 0.72f), false, true),
            ("Sofa", new Vector3(1.11f, 0.72f, 0.75f), false, true),
            ("GlowLamp", new Vector3(0.68f, 1.3f, 0.72f), false, true),
            // 주방·장식(바닥 중앙 원점, 앞면 -Z, 길이 로컬 X). 등잔·통·접수대는 높이 기준 균일 축소 실측값.
            ("KitchenCounter", new Vector3(2f, 0.9f, 0.8f), false, true),
            // 2026-11-24 뒤판을 다시 짜 맞추며 높이 0.88m. 불·받침쇠·PotPoint가 덧붙어 있어 메뉴가 다시 만들지 않는다.
            ("CookingRange", new Vector3(0.9f, 0.88f, 0.85f), false, true),
            ("SpiceShelf", new Vector3(1.3f, 1.8f, 0.45f), false, true),
            ("PickupCounter", new Vector3(1.2f, 0.9f, 0.6f), false, true),
            ("WallLantern", new Vector3(0.29f, 0.6f, 0.36f), false, false),
            // 게시판은 바르코3D가 '눕힌 쟁반'으로 만들어 Blender에서 얇은 판 + 정면 이미지 텍스처로 직접 만들었다.
            ("NoticeBoard", new Vector3(1.2f, 1f, 0.08f), false, false),
            ("DisplayShelf", new Vector3(1.6f, 1.8f, 0.45f), false, true),
            // 2026-11-25 통 밖으로 떠 있던 상자를 뒤쪽 통 위로 옮겨 높이 1.17m(상자가 통 테두리 위에 얹힘).
            ("BarrelSet", new Vector3(1.31f, 1.17f, 1.31f), false, true),
            ("WelcomeRug", new Vector3(1.4f, 0.03f, 1f), false, false),
            ("ReceptionPodium", new Vector3(0.59f, 1.1f, 0.52f), false, true),
            // 재료 주문 장부대(2026-11-25 바르코3D 재제작): 펼친 장부 + 뒷판(주문서·저울) + 오른쪽 재료 견본 상자.
            // 책상 윗면이 0.85m가 되도록 균일 확대한 실측값. 장부를 펼친 앞면이 -Z, 뒷판이 +Z, 견본 상자가 +X 쪽이다.
            ("OrderLedger", new Vector3(1.25f, 1.25f, 0.87f), false, true),
        };

        // 바르코3D가 아닌 외부 텍스처를 쓰는 조각. 출처는 README 5장 「외부 에셋 출처」에 적는다.
        private static readonly Dictionary<string, (string albedo, string normal)> EnvTextureOverrides = new Dictionary<string, (string, string)>
        {
            // Poly Haven castle_wall_slates(CC0). 99.ThirdParty는 Git에서 제외되므로 04.Art/Textures에 이름을 바꿔 둔다.
            { "Foundation", (EnvTextureFolder + "T_Env_Foundation_Albedo.jpg", EnvTextureFolder + "T_Env_Foundation_Normal.jpg") },
        };

        /// <summary>
        /// 화구 프리팹의 FirePoint/PotPoint(받침쇠 위) 자리에 냄비 프리팹을 올린다. 둘 중 하나라도 없으면 아무것도 하지 않는다.
        /// 냄비는 화구의 자식이라 화구를 옮기면 같이 움직인다.
        /// </summary>
        private static void PlacePotOnRange(Transform range)
        {
            var potPoint = range.Find("FirePoint/PotPoint");
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(EnvPrefabFolder + "PF_Env_CookingPot.prefab");
            if (potPoint == null || prefab == null)
                return;
            var pot = (GameObject)PrefabUtility.InstantiatePrefab(prefab, range.gameObject.scene);
            pot.name = "Pot";
            pot.transform.SetParent(potPoint, false);
            pot.transform.localPosition = Vector3.zero;
            pot.transform.localRotation = Quaternion.identity;
        }

        /// <summary>PF_Env_{kind} 프리팹이 있으면 같은 위치·회전으로 놓고 그 Transform을, 없으면 null을 돌려준다.</summary>
        private static Transform PlaceEnv(Transform parent, string kind, string name, Vector3 position, float yaw)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(EnvPrefabFolder + "PF_Env_" + kind + ".prefab");
            if (prefab == null)
                return null;
            var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent.gameObject.scene);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            go.transform.localRotation = Quaternion.Euler(0, yaw, 0);
            return go.transform;
        }

        /// <summary>
        /// 이 메뉴가 만드는 기본 구성(루트의 Transform·BoxCollider와 모델 자식 하나)보다 더 들어 있으면 true.
        /// 프리팹이 아직 없으면 false.
        /// </summary>
        private static bool HasAddedContent(string prefabPath)
        {
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (existing == null)
                return false;
            if (existing.transform.childCount > 1)
                return true;
            foreach (var component in existing.GetComponents<Component>())
                if (!(component is Transform) && !(component is BoxCollider))
                    return true;
            return false;
        }

        [MenuItem("DOB/Art/Build Environment Prefabs")]
        public static void BuildEnvironmentPrefabs()
        {
            Require(!EditorApplication.isPlaying, "플레이 중에는 프리팹을 만들 수 없습니다.");
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            Require(shader != null, "URP Lit 셰이더를 찾을 수 없습니다.");
            var built = new List<string>();
            foreach (var piece in EnvPieces)
            {
                var modelPath = EnvModelFolder + "SM_Env_" + piece.kind + ".fbx";
                if (!(AssetImporter.GetAtPath(modelPath) is ModelImporter importer))
                {
                    Debug.Log("DOB_ENV_SKIP: 모델 없음 " + modelPath);
                    continue;
                }
                // 정적 건물 조각: 리그·애니메이션·모델 안 카메라/라이트를 가져오지 않고 재질은 따로 만든다.
                importer.globalScale = 1;
                importer.useFileScale = true;
                importer.importCameras = false;
                importer.importLights = false;
                importer.importAnimation = false;
                importer.animationType = ModelImporterAnimationType.None;
                importer.materialImportMode = ModelImporterMaterialImportMode.None;
                importer.SaveAndReimport();

                // 연출·기준점 등을 덧붙인 프리팹(예: 화구의 불·받침쇠·PotPoint)은 다시 만들면 덧붙인 것이 사라지므로 건너뛴다.
                var prefabPath = EnvPrefabFolder + "PF_Env_" + piece.kind + ".prefab";
                if (HasAddedContent(prefabPath))
                {
                    Debug.Log("DOB_ENV_KEEP: 덧붙인 구성이 있어 다시 만들지 않음 " + prefabPath);
                    continue;
                }

                var material = EnvMaterial(piece.kind, shader);
                var model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
                var root = new GameObject("PF_Env_" + piece.kind);
                try
                {
                    if (piece.collider)
                    {
                        var box = root.AddComponent<BoxCollider>();
                        box.center = piece.corner ? piece.size / 2 : new Vector3(0, piece.size.y / 2, 0);
                        box.size = piece.size;
                    }
                    var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
                    instance.transform.SetParent(root.transform, false);
                    foreach (var collider in instance.GetComponentsInChildren<Collider>(true))
                        UnityEngine.Object.DestroyImmediate(collider);
                    var renderers = instance.GetComponentsInChildren<Renderer>(true);
                    foreach (var renderer in renderers)
                        renderer.sharedMaterial = material;

                    // Blender에서 맞춘 크기·원점이 표와 다르면 배치가 어긋나므로 경고한다.
                    var bounds = renderers[0].bounds;
                    foreach (var renderer in renderers)
                        bounds.Encapsulate(renderer.bounds);
                    var expectedMin = piece.corner ? Vector3.zero : new Vector3(-piece.size.x / 2, 0, -piece.size.z / 2);
                    if ((bounds.size - piece.size).magnitude > 0.05f || (bounds.min - expectedMin).magnitude > 0.05f)
                        Debug.LogWarning("DOB_ENV_SIZE: " + piece.kind + " 크기 " + bounds.size + " 최소점 " + bounds.min + " (기준 " + piece.size + " / " + expectedMin + ")");

                    PrefabUtility.SaveAsPrefabAsset(root, EnvPrefabFolder + "PF_Env_" + piece.kind + ".prefab");
                    built.Add(piece.kind);
                }
                finally
                {
                    UnityEngine.Object.DestroyImmediate(root);
                }
            }
            if (BuildWallPostPrefab())
                built.Add("WallPost");
            AssetDatabase.SaveAssets();
            Debug.Log("DOB_ENV_PREFABS_BUILT: " + string.Join(", ", built));
        }

        /// <summary>
        /// 기둥 모델을 가로·두께 절반으로 줄여 벽 이음새용 PF_Env_WallPost를 만든다.
        /// 루트 scale은 1로 두고 축소는 자식 모델에서 한다(아트 01 2-1). 기둥 모델이 없으면 false.
        /// </summary>
        private static bool BuildWallPostPrefab()
        {
            var model = AssetDatabase.LoadAssetAtPath<GameObject>(EnvModelFolder + "SM_Env_Pillar.fbx");
            var material = AssetDatabase.LoadAssetAtPath<Material>(EnvMaterialFolder + "M_Env_Pillar.mat");
            if (model == null || material == null)
                return false;
            var root = new GameObject("PF_Env_WallPost");
            try
            {
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
                instance.transform.SetParent(root.transform, false);
                instance.transform.localScale = new Vector3(0.5f, 1, 0.5f);
                foreach (var collider in instance.GetComponentsInChildren<Collider>(true))
                    UnityEngine.Object.DestroyImmediate(collider);
                foreach (var renderer in instance.GetComponentsInChildren<Renderer>(true))
                    renderer.sharedMaterial = material;
                PrefabUtility.SaveAsPrefabAsset(root, EnvPrefabFolder + "PF_Env_WallPost.prefab");
                return true;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static string TexturePath(string name)
        {
            var jpg = EnvTextureFolder + name + ".jpg";
            return File.Exists(jpg) ? jpg : EnvTextureFolder + name + ".png";
        }

        /// <summary>T_Env_{kind}_Albedo/Normal 텍스처로 M_Env_{kind} URP Lit 재질을 만들거나 갱신한다.</summary>
        private static Material EnvMaterial(string kind, Shader shader)
        {
            var path = EnvMaterialFolder + "M_Env_" + kind + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }
            // 저장소 크기를 줄이려고 텍스처는 JPG(품질 92)로 둔다. PNG로 새로 넣은 조각도 찾을 수 있게 둘 다 확인한다.
            var albedoPath = TexturePath("T_Env_" + kind + "_Albedo");
            var normalPath = TexturePath("T_Env_" + kind + "_Normal");
            if (EnvTextureOverrides.TryGetValue(kind, out var external))
            {
                albedoPath = external.albedo;
                normalPath = external.normal;
            }
            // 쿼터뷰에서 작게 보이므로 건물 조각 텍스처는 1024로 제한한다(아트 01 2-2).
            if (AssetImporter.GetAtPath(albedoPath) is TextureImporter albedoImporter)
            {
                albedoImporter.maxTextureSize = 1024;
                albedoImporter.SaveAndReimport();
                material.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(albedoPath));
            }
            if (AssetImporter.GetAtPath(normalPath) is TextureImporter normalImporter)
            {
                normalImporter.textureType = TextureImporterType.NormalMap;
                normalImporter.maxTextureSize = 1024;
                normalImporter.SaveAndReimport();
                material.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture2D>(normalPath));
                material.EnableKeyword("_NORMALMAP");
            }
            material.SetColor("_BaseColor", Color.white);
            material.SetFloat("_Smoothness", 0.15f);
            EditorUtility.SetDirty(material);
            return material;
        }

        // ── 조각 도우미 ──────────────────────────────────────

        /// <summary>건물 조각. 원점은 바닥면 한쪽 모서리이고, 로컬 +X가 길이, +Z가 두께 방향이다.</summary>
        private static Transform CornerPiece(Transform parent, string name, Vector3 corner, float yaw, Vector3 size, string mat)
        {
            var root = Group(parent, name);
            root.localPosition = corner;
            root.localRotation = Quaternion.Euler(0, yaw, 0);
            var box = root.gameObject.AddComponent<BoxCollider>();
            box.center = size / 2;
            box.size = size;
            if (mat != null)
                Visual(root, "Block", size / 2, size, mat);
            return root;
        }

        /// <summary>가구. 원점은 바닥 중앙(Y=0)이다.</summary>
        private static Transform CenterPiece(Transform parent, string name, Vector3 position, float yaw, Vector3 size, string mat)
        {
            var root = Group(parent, name);
            root.localPosition = position;
            root.localRotation = Quaternion.Euler(0, yaw, 0);
            var box = root.gameObject.AddComponent<BoxCollider>();
            box.center = new Vector3(0, size.y / 2, 0);
            box.size = size;
            if (mat != null)
                Visual(root, "Block", new Vector3(0, size.y / 2, 0), size, mat);
            return root;
        }

        /// <summary>충돌체 없는 외형 블록. 충돌체는 조각 루트에만 둔다(아트 01 2-1).</summary>
        private static Transform Visual(Transform parent, string name, Vector3 center, Vector3 size, string mat, PrimitiveType type = PrimitiveType.Cube)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>());
            go.transform.SetParent(parent, false);
            go.transform.localPosition = center;
            // 원기둥 기본 높이는 2m라서 높이를 절반으로 맞춘다.
            go.transform.localScale = type == PrimitiveType.Cylinder ? new Vector3(size.x, size.y / 2, size.z) : size;
            go.GetComponent<MeshRenderer>().sharedMaterial = materials[mat];
            return go.transform;
        }

        private static Transform Group(Transform parent, string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.transform;
        }

        private static void LoadMaterials()
        {
            // 임시 블록 색. 레퍼런스의 회벽·나무·돌·따뜻한 불빛 톤만 맞추며 최종 재질이 아니다.
            var colors = new Dictionary<string, (Color color, bool emissive)>
            {
                { "Plaster", (new Color(0.88f, 0.80f, 0.64f), false) },
                // 바닥은 가구보다 어둡고 회색기 있게 두어 쿼터뷰에서 테이블·의자가 구분되게 한다.
                { "FloorWood", (new Color(0.45f, 0.34f, 0.25f), false) },
                { "FloorWoodLight", (new Color(0.51f, 0.39f, 0.29f), false) },
                { "Stone", (new Color(0.47f, 0.45f, 0.42f), false) },
                { "StoneLight", (new Color(0.56f, 0.54f, 0.50f), false) },
                { "StoneDark", (new Color(0.30f, 0.29f, 0.28f), false) },
                { "Wood", (new Color(0.76f, 0.54f, 0.32f), false) },
                { "WoodDark", (new Color(0.40f, 0.26f, 0.16f), false) },
                { "Metal", (new Color(0.27f, 0.27f, 0.30f), false) },
                { "Marker", (new Color(0.42f, 0.62f, 0.36f), false) },
                { "Glass", (new Color(0.62f, 0.80f, 0.92f), false) },
                { "Flour", (new Color(0.92f, 0.88f, 0.78f), false) },
                { "Potato", (new Color(0.72f, 0.58f, 0.34f), false) },
                { "Meat", (new Color(0.70f, 0.30f, 0.28f), false) },
                { "Fire", (new Color(1f, 0.55f, 0.18f), true) },
            };
            if (!AssetDatabase.IsValidFolder(MaterialFolder))
                AssetDatabase.CreateFolder("Assets/_DOB/04.Art/Materials", "Greybox");
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            Require(shader != null, "URP Lit 셰이더를 찾을 수 없습니다.");
            materials = new Dictionary<string, Material>();
            foreach (var pair in colors)
            {
                var path = MaterialFolder + "/M_Greybox_" + pair.Key + ".mat";
                var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat == null)
                {
                    mat = new Material(shader);
                    AssetDatabase.CreateAsset(mat, path);
                }
                mat.SetColor("_BaseColor", pair.Value.color);
                if (pair.Value.emissive)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", pair.Value.color * 2f);
                    mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                }
                EditorUtility.SetDirty(mat);
                materials[pair.Key] = mat;
            }
            AssetDatabase.SaveAssets();
        }

        // ── 확인 ─────────────────────────────────────────────

        private static void ValidateAnchors(Scene scene)
        {
            var layout = scene.GetRootGameObjects().SingleOrDefault(o => o.name == "LayoutAnchors");
            Require(layout != null, "식당 기준점 루트 누락");
            foreach (var pair in ExpectedAnchors)
            {
                var anchor = layout.transform.Find(pair.Key);
                Require(anchor != null, "식당 기준점 누락: " + pair.Key);
                Require((anchor.position - pair.Value).sqrMagnitude < 0.0001f, "식당 기준점 위치가 바뀜: " + pair.Key + " " + anchor.position);
            }
        }

        /// <summary>확인용 PNG를 Temp에 저장한다. 임시 카메라는 씬에 저장하지 않는다.</summary>
        private static void CapturePreview(string name, Vector3 position, Vector3 euler)
        {
            const int width = 1600, height = 900;
            var cameraObject = new GameObject("PreviewCamera") { hideFlags = HideFlags.HideAndDontSave };
            var texture = new RenderTexture(width, height, 24);
            try
            {
                var camera = cameraObject.AddComponent<Camera>();
                camera.transform.SetPositionAndRotation(position, Quaternion.Euler(euler));
                camera.fieldOfView = 60;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.07f, 0.09f, 0.12f);
                camera.targetTexture = texture;
                // 씬을 연 직후 첫 URP 렌더는 빈 화면이 나올 수 있어 한 번 더 렌더한 결과를 저장한다.
                camera.Render();
                camera.Render();
                var previous = RenderTexture.active;
                RenderTexture.active = texture;
                var image = new Texture2D(width, height, TextureFormat.RGB24, false);
                image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                image.Apply();
                RenderTexture.active = previous;
                Directory.CreateDirectory(PreviewFolder);
                File.WriteAllBytes(Path.Combine(PreviewFolder, name + ".png"), image.EncodeToPNG());
                UnityEngine.Object.DestroyImmediate(image);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(cameraObject);
                texture.Release();
                UnityEngine.Object.DestroyImmediate(texture);
            }
        }

        private static Transform FindPath(Scene scene, string path)
        {
            var parts = path.Split('/');
            var root = scene.GetRootGameObjects().FirstOrDefault(o => o.name == parts[0]);
            return root == null ? null : parts.Length == 1 ? root.transform : root.transform.Find(string.Join("/", parts.Skip(1)));
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }
}
