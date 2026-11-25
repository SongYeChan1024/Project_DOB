// =====================================================================
// PauseMenuView — ESC 메뉴 화면 (PF_PauseMenu 프리팹의 루트)
// 역할   : 쪽(첫 화면 · 저장 확인 · 불러오기 · 끝내기 확인)마다 제목을 바꾸고, 프리팹 안의 단추 · 안내 글 견본을 복제해 쌓는다.
//          설정 쪽은 프리팹 안의 SettingsView(PF_Settings)를 띄운다. 무엇을 할지는 BasementPauseMenu가 정한다.
// 호출자 : BasementPauseMenu (Begin, Label, Action, Focus, ShowSettings, Hide)
// 상태   : 지금 쪽에 복제한 단추 · 글 목록, 고를 수 있는 단추 목록
// 비고   : 단추 오브젝트 이름은 BasementPauseMenu가 주는 이름 그대로 (팀 테스트가 Save · Load · Exit 등으로 누른다).
//          (홍준우 2026-11-25) 코드로 창을 쌓던 방식을 프리팹(PF_PauseMenu)으로 옮겼다. 겉모습은 프리팹에서 고친다.
//
// 의사코드
// [Begin]  창을 켜고 설정 창을 숨긴다 → 지난 쪽에 복제한 단추 · 글을 지운다 → 제목을 바꾼다
// [Label]  안내 글 견본을 복제해 글 · 크기 · 높이를 넣는다
// [Action] 단추 견본을 복제해 이름 · 글 · 누름 동작을 넣는다. 글에 줄바꿈이 있으면 둘째 줄을 작게, 단추를 높게
// [Focus]  고를 수 있는 단추끼리 위 · 아래로 돌게 잇고 첫 단추를 고른다
// [설정]   메뉴 창을 숨기고 설정 창을 연다 / [Hide] 설정 창과 전체를 숨긴다
// =====================================================================
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>ESC 메뉴. 프리팹 안의 견본을 복제해 쪽을 그린다.</summary>
    public sealed class PauseMenuView : MonoBehaviour
    {
        /// <summary>단추 · 글을 쌓는 나무 창 (VerticalLayoutGroup).</summary>
        [SerializeField] private RectTransform panel;
        [SerializeField] private TMP_Text titleText;
        /// <summary>이름표 단추 견본. 자식 Label에 글. 평소 비활성.</summary>
        [SerializeField] private Button buttonTemplate;
        /// <summary>안내 글 견본. 평소 비활성.</summary>
        [SerializeField] private TMP_Text labelTemplate;
        /// <summary>환경설정 창 (PF_Settings).</summary>
        [SerializeField] private SettingsView settings;
        /// <summary>두 줄 단추의 둘째 줄 색.</summary>
        [SerializeField] private Color subColor = new Color32(225, 205, 170, 255);

        private const float OneLineHeight = 78f, TwoLineHeight = 92f;
        private readonly List<GameObject> spawned = new List<GameObject>();
        private readonly List<Selectable> controls = new List<Selectable>();

        /// <summary>설정 창 (프리팹에 없으면 null).</summary>
        public SettingsView Settings => settings;

        private void Awake()
        {
            if (buttonTemplate) buttonTemplate.gameObject.SetActive(false);
            if (labelTemplate) labelTemplate.gameObject.SetActive(false);
            if (settings) settings.Hide();
        }

        /// <summary>새 쪽을 시작한다: 지난 단추 · 글을 지우고 제목을 바꾼다.</summary>
        public void Begin(string title)
        {
            gameObject.SetActive(true);
            panel.gameObject.SetActive(true);
            if (settings) settings.Hide();
            controls.Clear();
            foreach (var go in spawned)
            {
                if (!go) continue;
                go.SetActive(false); // 같은 프레임에 레이아웃에서 바로 빠지게
                Destroy(go);
            }
            spawned.Clear();
            titleText.text = title;
        }

        /// <summary>안내 글 한 줄 (여러 줄이면 height를 늘린다).</summary>
        public void Label(string caption, int size = 24, float height = 48)
        {
            var text = Instantiate(labelTemplate, panel);
            text.name = "Message";
            text.text = caption;
            text.fontSize = size;
            text.GetComponent<LayoutElement>().preferredHeight = height;
            text.gameObject.SetActive(true);
            spawned.Add(text.gameObject);
        }

        /// <summary>이름표 단추. 글에 줄바꿈이 있으면 둘째 줄을 작게 (예: 이어하기 + 최근 자동 저장).</summary>
        public Button Action(string objectName, string caption, Action onClick, bool enabled = true)
        {
            bool twoLines = caption.Contains("\n");
            var button = Instantiate(buttonTemplate, panel);
            button.name = objectName;
            button.GetComponent<LayoutElement>().preferredHeight = twoLines ? TwoLineHeight : OneLineHeight;
            button.interactable = enabled;
            button.onClick.AddListener(() => onClick());

            string text = caption;
            if (twoLines)
            {
                int cut = caption.IndexOf('\n');
                text = caption.Substring(0, cut) + $"\n<size=18><color=#{ColorUtility.ToHtmlStringRGB(subColor)}>{caption.Substring(cut + 1)}</color></size>";
            }
            button.GetComponentInChildren<TMP_Text>(true).text = text;
            button.gameObject.SetActive(true);
            spawned.Add(button.gameObject);
            if (enabled) controls.Add(button);
            return button;
        }

        /// <summary>설정 창을 띄운다 (메뉴 창은 숨김). 창이 없으면 false.</summary>
        public bool ShowSettings()
        {
            if (!settings) return false;
            gameObject.SetActive(true);
            panel.gameObject.SetActive(false);
            controls.Clear();
            settings.Open();
            return true;
        }

        public void Focus()
        {
            // 이 프레임의 버튼 onClick이 끝나기 전에 선택 대상을 교체해 키 입력이 뒤의 화면에 전달되지 않게 한다.
            for (int i = 0; i < controls.Count; i++)
            {
                var navigation = new Navigation { mode = Navigation.Mode.Explicit };
                navigation.selectOnUp = controls[(i + controls.Count - 1) % controls.Count];
                navigation.selectOnDown = controls[(i + 1) % controls.Count];
                controls[i].navigation = navigation;
            }
            if (EventSystem.current && controls.Count > 0)
                EventSystem.current.SetSelectedGameObject(controls[0].gameObject);
        }

        public void Hide()
        {
            if (settings) settings.Hide();
            if (this) gameObject.SetActive(false);
        }
    }
}
