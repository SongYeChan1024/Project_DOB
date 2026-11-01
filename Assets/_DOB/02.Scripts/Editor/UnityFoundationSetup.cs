using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DOB.Editor
{
    /// <summary>명시적으로 요청한 씬 구성과 공용 캐릭터 프리팹 골격을 생성·검증한다.</summary>
    public static class UnityFoundationSetup
    {
        /// <summary>기존 씬을 보존하며 일곱 씬의 초기 구성을 만든다.</summary>
        [MenuItem("DOB/Setup/Create Initial Scenes")]
        public static void Create()
        {
            SceneLayoutSetup.Create();
        }

        /// <summary>승인된 씬을 재구성한다. 실행 전 기존 씬과 메타를 별도로 백업한다.</summary>
        public static void RebuildSceneLayout()
        {
            SceneLayoutSetup.Rebuild();
        }

        /// <summary>일곱 씬의 역할·공용 프리팹·조합과 빌드 등록을 검사한다.</summary>
        public static void Validate()
        {
            SceneLayoutSetup.Validate();
        }

        /// <summary>CharacterTest를 제외한 여섯 런타임 씬으로 Windows 개발 빌드를 만든다.</summary>
        public static void BuildWindows()
        {
            SceneLayoutSetup.BuildWindows();
        }

        private static void AddAnchor(Transform parent, string name, Vector3 position)
        {
            var anchor = new GameObject(name);
            anchor.transform.SetParent(parent, false);
            anchor.transform.localPosition = position;
        }

        /// <summary>모델 교체와 기능 연결 영역이 분리된 캐릭터 프리팹 골격을 생성한다.</summary>
        [MenuItem("DOB/Setup/Create Character Shells")]
        public static void CreateCharacterShells()
        {
            const string folder = "Assets/_DOB/03.Prefabs/Characters/";
            foreach (var kind in new[] { "Player", "Guest", "Enemy" })
                if (File.Exists(folder + "PF_" + kind + ".prefab"))
                    throw new InvalidOperationException("기존 프리팹 보존: " + kind);

            foreach (var kind in new[] { "Player", "Guest", "Enemy" })
            {
                var root = new GameObject("PF_" + kind);
                if (kind == "Player")
                {
                    var controller = root.AddComponent<CharacterController>();
                    controller.height = 1.8f;
                    controller.radius = 0.3f;
                    controller.center = new Vector3(0, 0.9f, 0);
                }
                else
                {
                    var collider = root.AddComponent<CapsuleCollider>();
                    collider.height = 1.8f;
                    collider.radius = 0.3f;
                    collider.center = new Vector3(0, 0.9f, 0);
                }
                var visual = new GameObject("VisualRoot");
                visual.transform.SetParent(root.transform, false);
                var slot = new GameObject("ModelSlot");
                slot.transform.SetParent(visual.transform, false);
                var placeholder = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                placeholder.name = "Placeholder";
                placeholder.transform.SetParent(slot.transform, false);
                placeholder.transform.localPosition = new Vector3(0, 0.9f, 0);
                placeholder.transform.localScale = new Vector3(0.6f, 0.9f, 0.6f);
                UnityEngine.Object.DestroyImmediate(placeholder.GetComponent<Collider>());
                var anchors = new GameObject("Anchors");
                anchors.transform.SetParent(root.transform, false);
                AddAnchor(anchors.transform, "InteractionOrigin", new Vector3(0, 1, 0.5f));
                AddAnchor(anchors.transform, "FacePoint", new Vector3(0, 1.55f, 0));
                AddAnchor(anchors.transform, "PlatePoint", new Vector3(0, 0.9f, 0.45f));
                AddAnchor(anchors.transform, "CameraTarget", new Vector3(0, 1.5f, 0));
                PrefabUtility.SaveAsPrefabAsset(root, folder + root.name + ".prefab");
                UnityEngine.Object.DestroyImmediate(root);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("DOB_CHARACTER_SHELLS_CREATED: Player, Guest, Enemy (expansion shell only).");
        }
    }
}
