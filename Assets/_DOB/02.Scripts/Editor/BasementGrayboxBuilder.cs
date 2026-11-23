// 전용 에디터 생성기: 층계참 바닥과 동선 기준점을 먼저 배치하고 기능별 소품·표지를 얹는다.
// 각 좌석 인덱스 및 주방·계단 기준점을 RestaurantLayout에 직렬화한 뒤 새로운 레벨 씬에 저장한다.
using System;
using DOB.Manager;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace DOB.Editor
{
    /// <summary>Basement 데모의 공간만 생성한다. 카메라·캐릭터와 영업 화면은 각자의 씬에서 조합한다.</summary>
    public static class BasementGrayboxBuilder
    {
        private const string ScenePath = "Assets/_DOB/01.Scenes/BasementRestaurant.unity";
        private const string MaterialParent = "Assets/_DOB/04.Art/Materials";
        private const string MaterialFolder = MaterialParent + "/Basement";

        /// <summary>에디터 배치 생성 진입점. 이미 저장한 레벨은 보존하고, 열려 있는 다른 씬을 닫지 않는다.</summary>
        public static void CreateScene()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath))
                return;

            if (!AssetDatabase.IsValidFolder(MaterialFolder))
                AssetDatabase.CreateFolder(MaterialParent, "Basement");
            var palette = new Palette();
            var previous = SceneManager.GetActiveScene();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            SceneManager.SetActiveScene(scene);
            try
            {
                var environment = new GameObject("Environment").transform;
                var art = Group(environment, "ArtPlacement", Vector3.zero);
                var anchors = new GameObject("LayoutAnchors").transform;
                var spawn = Group(anchors, "PlayerSpawn", new Vector3(0f, 0f, -1f));
                var entrance = Group(anchors, "GuestEntrance", new Vector3(-4f, 0f, -2f));
                var greeting = Group(anchors, "GreetingPoint", new Vector3(-2f, 0f, -1f));
                var kitchen = Group(anchors, "Kitchen", new Vector3(4f, 0f, 0f));
                var stairs = Group(anchors, "StairsExit", new Vector3(4f, 0f, 4f));
                var seats = new Transform[3];

                var floor = CreateFloor(environment, art, palette);
                for (int index = 0; index < seats.Length; index++)
                {
                    seats[index] = Group(anchors, $"Seat{index + 1:00}", new Vector3((index - 1) * 2f, 0f, 2f));
                    CreateDiningSet(art, seats[index].localPosition, index + 1, palette);
                }
                CreateEntrance(art, entrance.localPosition, palette);
                CreateKitchen(art, kitchen.localPosition, palette);
                CreateStairs(art, stairs.localPosition, palette);
                CreateLight(environment);

                var layout = new SerializedObject(anchors.gameObject.AddComponent<RestaurantLayout>());
                layout.FindProperty("playerSpawn").objectReferenceValue = spawn;
                layout.FindProperty("guestEntrance").objectReferenceValue = entrance;
                layout.FindProperty("greetingPoint").objectReferenceValue = greeting;
                layout.FindProperty("kitchen").objectReferenceValue = kitchen;
                layout.FindProperty("stairsExit").objectReferenceValue = stairs;
                layout.FindProperty("floor").objectReferenceValue = floor;
                var seatList = layout.FindProperty("seats");
                seatList.arraySize = seats.Length;
                for (int index = 0; index < seats.Length; index++)
                    seatList.GetArrayElementAtIndex(index).objectReferenceValue = seats[index];
                layout.ApplyModifiedPropertiesWithoutUndo();

                if (!EditorSceneManager.SaveScene(scene, ScenePath))
                    throw new InvalidOperationException("식당 데모 레벨을 저장하지 못했습니다.");
            }
            finally
            {
                if (previous.IsValid() && previous.isLoaded)
                    SceneManager.SetActiveScene(previous);
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        private static Collider CreateFloor(Transform environment, Transform art, Palette p)
        {
            // 원래 Floor의 12×12 이동 범위와 y=0 윗면을 유지한다. 이동 충돌을 갖는 유일한 메시다.
            var floor = Shape(environment, "Floor", PrimitiveType.Cube, new Vector3(0f, -0.14f, 0f),
                new Vector3(12f, 0.28f, 12f), p.Floor, true);
            for (int i = -5; i <= 5; i++)
                Box(art, $"BoardJoint_{i + 5:00}", new Vector3(0f, 0.002f, i + 0.5f),
                    new Vector3(11.9f, 0.004f, 0.022f), p.Joints);
            Box(art, "BackSill", new Vector3(0f, 0.15f, 5.88f), new Vector3(12f, 0.3f, 0.22f), p.Wood);
            Box(art, "LeftSill", new Vector3(-5.88f, 0.15f, 0f), new Vector3(0.22f, 0.3f, 12f), p.Wood);
            Box(art, "RightSill", new Vector3(5.88f, 0.15f, 0f), new Vector3(0.22f, 0.3f, 12f), p.Wood);
            return floor.GetComponent<Collider>();
        }

        private static void CreateDiningSet(Transform parent, Vector3 position, int number, Palette p)
        {
            var dining = Group(parent, $"DiningSet_{number:00}", position);
            Box(dining, "TableBase", new Vector3(0f, 0.32f, 0.83f), new Vector3(0.52f, 0.64f, 0.48f), p.Wood);
            Box(dining, "TableTop", new Vector3(0f, 0.69f, 0.83f), new Vector3(1.38f, 0.13f, 0.87f), p.Oak);
            Box(dining, "Runner", new Vector3(0f, 0.763f, 0.83f), new Vector3(0.53f, 0.015f, 0.81f), p.Teal);
            Disc(dining, "Plate", new Vector3(0f, 0.782f, 0.67f), 0.35f, 0.025f, p.Cream);
            Box(dining, "ChairBase", new Vector3(0f, 0.18f, 0f), new Vector3(0.38f, 0.36f, 0.38f), p.Wood);
            Box(dining, "ChairSeat", new Vector3(0f, 0.39f, 0f), new Vector3(0.62f, 0.12f, 0.55f), p.Teal);
            // 번호는 손님이 착석한 뒤에도 머리와 겹치지 않도록 테이블 뒤쪽 모서리에 둔다.
            Label(dining, $"SeatNumber_{number}", number.ToString(), new Vector3(0.47f, 1.17f, 1.12f), 0.45f, p);
        }

        private static void CreateEntrance(Transform parent, Vector3 position, Palette p)
        {
            var doorway = Group(parent, "Entry", position);
            Box(doorway, "EntryMat", Vector3.up * 0.017f, new Vector3(1.65f, 0.034f, 1.12f), p.Teal);
            Box(doorway, "Lintel", new Vector3(-1.13f, 1.75f, 0f), new Vector3(0.19f, 0.2f, 1.54f), p.Oak);
            foreach (var side in new[] { -0.67f, 0.67f })
                Box(doorway, side < 0f ? "NearPost" : "FarPost", new Vector3(-1.13f, 0.85f, side),
                    new Vector3(0.16f, 1.7f, 0.16f), p.Wood);
            Label(doorway, "EntranceSign", "입구", new Vector3(-0.8f, 1.95f, 0.1f), 1f, p);
        }

        private static void CreateKitchen(Transform parent, Vector3 position, Palette p)
        {
            var kitchen = Group(parent, "KitchenCounter", position);
            Box(kitchen, "ApronMat", new Vector3(-0.1f, 0.022f, 0f), new Vector3(1.05f, 0.044f, 1.8f), p.Teal);
            Box(kitchen, "Cabinet", new Vector3(0.88f, 0.48f, 0f), new Vector3(0.94f, 0.94f, 1.95f), p.Wood);
            Box(kitchen, "Worktop", new Vector3(0.88f, 1f, 0f), new Vector3(1.03f, 0.12f, 2.06f), p.Copper);
            Disc(kitchen, "StoveRing", new Vector3(0.94f, 1.09f, 0.48f), 0.6f, 0.05f, p.Iron);
            Disc(kitchen, "Pot", new Vector3(0.94f, 1.22f, 0.48f), 0.49f, 0.25f, p.Teal);
            Disc(kitchen, "PotLid", new Vector3(0.94f, 1.36f, 0.48f), 0.51f, 0.05f, p.Cream);
            Box(kitchen, "BreadBoard", new Vector3(0.83f, 1.09f, -0.45f), new Vector3(0.6f, 0.05f, 0.57f), p.Oak);
            Shape(kitchen, "Bread", PrimitiveType.Sphere, new Vector3(0.83f, 1.19f, -0.45f),
                new Vector3(0.41f, 0.21f, 0.33f), p.Copper);
            Label(kitchen, "KitchenSign", "주방", new Vector3(0.88f, 1.75f, 0.12f), 1.12f, p);
        }

        private static void CreateStairs(Transform parent, Vector3 position, Palette p)
        {
            var stairs = Group(parent, "DungeonStairs", position);
            for (int i = 0; i < 4; i++)
            {
                float height = 0.12f * (i + 1);
                float z = 0.01f + i * 0.34f;
                Box(stairs, $"Step_{i + 1:00}", new Vector3(0f, height * 0.5f, z), new Vector3(1.55f, height, 0.35f), p.Teal);
                Box(stairs, $"StepEdge_{i + 1:00}", new Vector3(0f, height + 0.006f, z - 0.135f),
                    new Vector3(1.55f, 0.015f, 0.055f), p.Cream);
            }
            Label(stairs, "StairsSign", "던전 계단", new Vector3(0f, 1.15f, 1.25f), 1.76f, p);
        }

        private static void CreateLight(Transform parent)
        {
            var sun = Group(parent, "Directional Light", new Vector3(0f, 6f, 0f)).gameObject.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.transform.rotation = Quaternion.Euler(48f, -32f, 0f);
            sun.color = new Color(1f, 0.92f, 0.8f);
            sun.intensity = 1.4f;
            sun.shadows = LightShadows.Soft;
            RenderSettings.sun = sun;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.62f, 0.64f, 0.66f);
        }

        private static Transform Group(Transform parent, string name, Vector3 position)
        {
            var item = new GameObject(name).transform;
            item.SetParent(parent, false);
            item.localPosition = position;
            return item;
        }

        private static GameObject Shape(Transform parent, string name, PrimitiveType kind, Vector3 position,
            Vector3 scale, Material material, bool keepCollider = false)
        {
            var shape = GameObject.CreatePrimitive(kind);
            shape.name = name;
            shape.transform.SetParent(parent, false);
            shape.transform.localPosition = position;
            shape.transform.localScale = scale;
            shape.GetComponent<Renderer>().sharedMaterial = material;
            if (!keepCollider)
                UnityEngine.Object.DestroyImmediate(shape.GetComponent<Collider>());
            return shape;
        }

        private static void Box(Transform parent, string name, Vector3 center, Vector3 size, Material material) =>
            Shape(parent, name, PrimitiveType.Cube, center, size, material);

        private static void Disc(Transform parent, string name, Vector3 center, float diameter, float height, Material material) =>
            Shape(parent, name, PrimitiveType.Cylinder, center, new Vector3(diameter, height * 0.5f, diameter), material);

        private static void Label(Transform parent, string name, string caption, Vector3 position, float width, Palette p)
        {
            // 고정 쿼터뷰 카메라를 마주보는 표지판. 3D 문자로 표시하여 UI 씬·카메라를 추가하지 않는다.
            var sign = Group(parent, name, position);
            sign.localRotation = Quaternion.Euler(45f, 0f, 0f);
            Box(sign, "SignBoard", Vector3.zero, new Vector3(width, 0.48f, 0.08f), p.Iron);
            var label = Group(sign, "Caption", new Vector3(0f, 0f, -0.05f)).gameObject.AddComponent<TextMesh>();
            label.font = p.LabelFont;
            label.GetComponent<MeshRenderer>().sharedMaterial = p.LabelFont.material;
            label.fontSize = 72;
            label.characterSize = 0.055f;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.color = new Color(1f, 0.96f, 0.84f);
            label.text = caption;
        }

        private sealed class Palette
        {
            public readonly Material Floor = MaterialAsset("Floor", new Color32(177, 150, 122, 255));
            public readonly Material Joints = MaterialAsset("BoardJoints", new Color32(145, 117, 98, 255));
            public readonly Material Wood = MaterialAsset("DarkTimber", new Color32(89, 63, 54, 255));
            public readonly Material Oak = MaterialAsset("Oak", new Color32(190, 126, 75, 255));
            public readonly Material Teal = MaterialAsset("GlazedTeal", new Color32(66, 117, 111, 255));
            public readonly Material Copper = MaterialAsset("Copper", new Color32(185, 103, 55, 255));
            public readonly Material Cream = MaterialAsset("Cream", new Color32(245, 218, 171, 255));
            public readonly Material Iron = MaterialAsset("Iron", new Color32(47, 53, 58, 255));
            public readonly Font LabelFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/_DOB/07.UI/Fonts/YeonSung-Regular.ttf");

            public Palette()
            {
                if (!LabelFont)
                    throw new InvalidOperationException("데모 표지에 필요한 프로젝트 글꼴을 찾을 수 없습니다.");
            }
        }

        private static Material MaterialAsset(string name, Color color)
        {
            var path = $"{MaterialFolder}/{name}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material)
                return material;
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (!shader)
                throw new InvalidOperationException("프로젝트의 URP Lit 셰이더를 찾을 수 없습니다.");
            material = new Material(shader) { name = name };
            material.SetColor("_BaseColor", color);
            material.SetFloat("_Smoothness", 0.13f);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }
    }
}
