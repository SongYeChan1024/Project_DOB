// 플레이어 캡슐의 CarryPoint에 첫 손님 접시(접시 + 구운 빵) 외형을 붙이고, 주방이 접시를 들고 있다고 알릴 때만 보인다.
// 호출: Unity 생명주기. 참조: FirstGuestKitchen.IsCarrying(읽기 전용).
// 가진 상태: 처음 한 번 만든 외형 루트와 마지막으로 반영한 표시 여부. 주문·접시 소유 상태는 주방이 소유한다.
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>들고 있는 서빙 접시를 기본 도형으로 표시한다. 상태를 바꾸지 않고 보이기만 전환한다.</summary>
    public sealed class FirstGuestPlateView : MonoBehaviour
    {
        /// <summary>접시를 들고 있는지 알려 주는 첫 손님 주방.</summary>
        [SerializeField] private FirstGuestKitchen kitchen;

        /// <summary>캡슐의 CarryPoint. 외형은 이 로컬 좌표 아래에 붙는다.</summary>
        [SerializeField] private Transform carryAnchor;

        // 아트 프리팹이 오기 전의 임시 외형 크기·색이다. 판정·경제와 무관한 표시용 값이다.
        private static readonly Vector3 PlateScale = new Vector3(0.36f, 0.015f, 0.36f);
        private static readonly Vector3 BreadScale = new Vector3(0.22f, 0.1f, 0.16f);
        private static readonly Vector3 BreadOffset = new Vector3(0f, 0.06f, 0f);
        private static readonly Color PlateColor = new Color(0.92f, 0.9f, 0.85f);
        private static readonly Color BreadColor = new Color(0.72f, 0.47f, 0.22f);

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        private GameObject visualRoot;
        private bool shown;

        private void OnEnable()
        {
            if (kitchen == null || carryAnchor == null)
            {
                Debug.LogError("[DOB] FirstGuestPlateView: kitchen 또는 carryAnchor가 연결되지 않았습니다.", this);
                enabled = false;
                return;
            }

            if (visualRoot == null)
                visualRoot = BuildVisual();

            // 재활성화 직후 이전 표시 여부에 기대지 않고 현재 상태로 다시 맞춘다.
            shown = kitchen.IsCarrying;
            visualRoot.SetActive(shown);
        }

        private void Update()
        {
            bool carrying = kitchen.IsCarrying;
            if (carrying == shown)
                return;
            shown = carrying;
            visualRoot.SetActive(shown);
        }

        private void OnDisable()
        {
            // 비활성화된 뷰가 접시를 남겨 두지 않도록 숨긴다. 외형은 재사용을 위해 유지한다.
            if (visualRoot != null)
                visualRoot.SetActive(false);
            shown = false;
        }

        private void OnDestroy()
        {
            // 외형은 carryAnchor 아래에 있어 이 오브젝트와 함께 사라지지 않으므로 직접 정리한다.
            if (visualRoot != null)
                Destroy(visualRoot);
            visualRoot = null;
        }

        private GameObject BuildVisual()
        {
            var root = new GameObject("FirstGuestPlateVisual");
            root.SetActive(false);
            root.transform.SetParent(carryAnchor, false);

            CreatePart(PrimitiveType.Cylinder, "Plate", root.transform, Vector3.zero, PlateScale, PlateColor);
            CreatePart(PrimitiveType.Sphere, "Bread", root.transform, BreadOffset, BreadScale, BreadColor);
            return root;
        }

        private static void CreatePart(PrimitiveType type, string name, Transform parent, Vector3 localPosition, Vector3 localScale, Color color)
        {
            var part = GameObject.CreatePrimitive(type);
            part.name = name;

            // 들고 다니는 외형이 캐릭터 이동·상호작용 판정에 끼어들지 않도록 충돌체를 즉시 끄고 제거한다.
            var collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Destroy(collider);
            }

            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localRotation = Quaternion.identity;
            part.transform.localScale = localScale;

            // 공유 기본 머티리얼을 복제하지 않고 렌더러별 색만 지정한다.
            var renderer = part.GetComponent<Renderer>();
            var block = new MaterialPropertyBlock();
            block.SetColor(BaseColorId, color);
            block.SetColor(ColorId, color);
            renderer.SetPropertyBlock(block);
        }
    }
}
