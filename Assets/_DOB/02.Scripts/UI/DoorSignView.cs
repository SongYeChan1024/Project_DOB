// =====================================================================
// DoorSignView — 입구 문패 (영업 시작 확인)
// 역할   : 오늘 영업 요약(손님 · 좌석 · 화구)을 큰 칸으로, 가진 양념 · 재료를 그림 + 개수 칸으로 보여 주고
//          [문 열기] / [조금 더 준비] 를 알린다.
// 호출자 : PrepScreensView (Show)
// 상태   : 복사해 만든 칸 목록, 팻말을 뒤집는 중인지
//
// 의사코드
// [요약]   큰 칸 3개 (이름 위, 값 아래)
// [양념]   그림 + ×개수 칸을 줄 바꿈 격자로. 없으면 "없음" 글
// [재료]   같은 격자. 칸이 많아 영역을 넘치면 그 안에서 위아래로 넘겨 본다 (창 크기는 그대로)
// [문 열기] (시안 21 ③) 버튼을 잠그고 위 팻말을 위아래로 뒤집어 초록 「영업 중」으로 바꾼 뒤 → 문 열기를 알린다
//          (뒤집는 0.35초 동안 두 번 눌리지 않게 잠금. 다음에 열 때 팻말 글을 되돌린다)
// =====================================================================
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>입구 문패 확인 창.</summary>
    public sealed class DoorSignView : MonoBehaviour
    {
        [Header("요약 큰 칸"), SerializeField] private RectTransform statList;
        /// <summary>원본 (자식: Label, Value).</summary>
        [SerializeField] private GameObject statTemplate;

        [Header("가진 물건 격자"), SerializeField] private RectTransform spiceList;
        [SerializeField] private RectTransform ingredientList;
        [SerializeField] private GameObject spiceEmpty;
        [SerializeField] private GameObject ingredientEmpty;
        /// <summary>원본 (자식: Icon, Count). 마우스를 올리면 이름 · 효과 (HoverInfo).</summary>
        [SerializeField] private GameObject stockTemplate;
        /// <summary>재료 칸을 넘겨 보는 곳. 열 때마다 맨 위로 돌린다.</summary>
        [SerializeField] private ScrollRect ingredientScroll;

        [Header("버튼"), SerializeField] private Button openButton;
        [SerializeField] private Button cancelButton;

        private readonly List<GameObject> spawned = new List<GameObject>();

        /// <summary>[문 열기]를 눌렀다.</summary>
        public event Action OpenClicked;
        /// <summary>[조금 더 준비]를 눌렀다.</summary>
        public event Action CancelClicked;

        [Header("문 열기 팻말 (시안 21 ③)")]
        /// <summary>뒤집을 위 팻말. 비워 두면 뒤집지 않고 바로 문 열기를 알린다.</summary>
        [SerializeField] private RectTransform signPlate;
        [SerializeField] private TMP_Text signText;
        [SerializeField] private string openWord = "영업 중";
        [SerializeField] private Color openColor = new Color32(160, 225, 120, 255);

        private string signWord;
        private Color signColor;
        private bool flipping;

        private void Awake()
        {
            if (signText) { signWord = signText.text; signColor = signText.color; }
            openButton.onClick.AddListener(OnOpen);
            cancelButton.onClick.AddListener(() => CancelClicked?.Invoke());
            statTemplate.SetActive(false);
            stockTemplate.SetActive(false);
        }

        private void OnOpen()
        {
            if (flipping) return;
            if (!signPlate || !signText || !isActiveAndEnabled) { OpenClicked?.Invoke(); return; }
            StartCoroutine(FlipThenOpen());
        }

        private System.Collections.IEnumerator FlipThenOpen()
        {
            flipping = true;
            openButton.interactable = false;
            const float half = 0.12f;
            for (float t = 0f; t < half * 2f; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float k = t < half ? t / half : 1f - (t - half) / half;
                if (t >= half && signText.text != openWord) { signText.text = openWord; signText.color = openColor; }
                signPlate.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.05f, k), 1f);
                yield return null;
            }
            signText.text = openWord; signText.color = openColor;
            signPlate.localScale = Vector3.one;
            for (float t = 0f; t < 0.12f; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f)) yield return null; // 「영업 중」을 잠깐 보여 준다
            flipping = false;
            OpenClicked?.Invoke();
        }

        public void Show(PrepViewData data)
        {
            // 다시 열 때 팻말 · 버튼을 처음 상태로
            if (signText && signWord != null) { signText.text = signWord; signText.color = signColor; }
            if (signPlate) signPlate.localScale = Vector3.one;
            openButton.interactable = true;
            flipping = false;

            // 지난 칸 지우기
            foreach (var go in spawned) Destroy(go);
            spawned.Clear();

            // 요약 큰 칸
            foreach (var stat in data.doorStats)
            {
                var go = Spawn(statTemplate, statList);
                go.transform.Find("Label").GetComponent<TMP_Text>().text = stat.label;
                go.transform.Find("Value").GetComponent<TMP_Text>().text = stat.value;
            }

            // 가진 양념 · 재료
            FillStock(spiceList, spiceEmpty, data.doorSpices);
            FillStock(ingredientList, ingredientEmpty, data.doorIngredients);
            if (ingredientScroll) ingredientScroll.verticalNormalizedPosition = 1f;
        }

        private void FillStock(RectTransform list, GameObject empty, StockItemData[] items)
        {
            foreach (var item in items)
            {
                var go = Spawn(stockTemplate, list);
                var icon = go.transform.Find("Icon").GetComponent<Image>();
                icon.sprite = item.icon;
                icon.enabled = item.icon;
                go.transform.Find("Count").GetComponent<TMP_Text>().text = "×" + item.count;
                var hover = go.GetComponent<HoverInfo>();
                if (hover) hover.Set(item.name, item.description);
            }
            empty.SetActive(items.Length == 0);
        }

        private GameObject Spawn(GameObject template, Transform parent)
        {
            var go = Instantiate(template, parent);
            go.SetActive(true);
            spawned.Add(go);
            return go;
        }
    }
}
