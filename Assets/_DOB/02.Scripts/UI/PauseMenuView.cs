// =====================================================================
// PauseMenuView — ESC 메뉴 화면 (BasementPauseView를 다른 화면과 같은 나무 창 · 이름표 단추로 바꾼 것)
// 역할   : 쪽(첫 화면 · 저장 확인 · 불러오기 · 끝내기 확인)마다 제목 · 안내 글 · 단추를 새로 쌓아 그린다.
//          설정 쪽은 SettingsView 프리팹을 띄운다. 무엇을 할지는 BasementPauseMenu가 정한다.
// 호출자 : BasementPauseMenu (Begin, Label, Action, Focus, ShowSettings, Hide)
// 상태   : 지금 쪽의 단추 목록, 설정 창
// 비고   : 단추 오브젝트 이름은 BasementPauseMenu가 주는 이름 그대로 (팀 테스트가 Save · Load · Exit 등으로 누른다).
//          스킨이 없으면 그림 없이 기본 글꼴로 그린다 (Resources/UI/PauseMenuSkin.asset).
// =====================================================================
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DOB.UI
{
    /// <summary>ESC 메뉴를 코드로 쌓아 그린다.</summary>
    public sealed class PauseMenuView
    {
        private const float Width = 620f;
        private readonly GameObject overlay;
        private readonly RectTransform panel;
        private readonly TMP_Text titleText;
        private readonly PauseMenuSkin skin;
        private readonly TMP_FontAsset font;
        private readonly List<Selectable> controls = new List<Selectable>();

        /// <summary>설정 창 (스킨에 프리팹이 없으면 null).</summary>
        public SettingsView Settings { get; }

        public PauseMenuView(Transform parent, PauseMenuSkin menuSkin)
        {
            skin = menuSkin ? menuSkin : ScriptableObject.CreateInstance<PauseMenuSkin>();
            if (!menuSkin) Debug.LogWarning("[DOB] Resources/UI/PauseMenuSkin이 없어 ESC 메뉴를 그림 없이 그립니다.");
            font = skin.font ? skin.font : TMP_Settings.defaultFontAsset;

            overlay = new GameObject("BasementPauseCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            overlay.transform.SetParent(parent, false);
            if (parent.GetComponentInParent<Canvas>())
                Stretch((RectTransform)overlay.transform);
            var canvas = overlay.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 850; // 모든 게임 화면 위
            var scaler = overlay.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            // 뒤 화면을 어둡게 하고 입력을 막는다
            var shade = Rect("Backdrop", overlay.transform);
            Stretch(shade);
            shade.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);

            // 나무 창: 내용 높이에 맞춰 늘어난다
            panel = Rect("Menu", shade);
            panel.sizeDelta = new Vector2(Width, 0);
            var window = panel.gameObject.AddComponent<Image>();
            Slice(window, skin.window, 1f);
            var stack = panel.gameObject.AddComponent<VerticalLayoutGroup>();
            stack.padding = new RectOffset(80, 80, 84, 44);
            stack.spacing = 20;
            stack.childAlignment = TextAnchor.UpperCenter;
            stack.childControlWidth = true;
            stack.childForceExpandWidth = true;
            stack.childControlHeight = true;
            stack.childForceExpandHeight = false;
            panel.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // 제목판: 창 위 가운데에 걸침 (쌓기에서 빠짐)
            var plate = Rect("TitlePlate", panel);
            plate.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
            plate.anchorMin = plate.anchorMax = new Vector2(0.5f, 1f);
            plate.sizeDelta = new Vector2(400, 78);
            plate.anchoredPosition = new Vector2(0, 0);
            Slice(plate.gameObject.AddComponent<Image>(), skin.plate, 3f);
            titleText = CreateText(Stretched("Title", plate), string.Empty, 40, skin.titleColor);

            if (skin.settingsPrefab)
            {
                Settings = Object.Instantiate(skin.settingsPrefab, overlay.transform);
                Settings.name = "SettingsWindow"; // ESC 메뉴의 "Settings" 단추와 이름이 겹치지 않게
                Settings.Hide();
            }
            Hide();
        }

        /// <summary>새 쪽을 시작한다: 지난 단추 · 글을 지우고 제목을 바꾼다.</summary>
        public void Begin(string title)
        {
            overlay.SetActive(true);
            panel.gameObject.SetActive(true);
            if (Settings) Settings.Hide();
            controls.Clear();
            for (int i = panel.childCount - 1; i >= 0; i--)
            {
                var child = panel.GetChild(i).gameObject;
                if (child.name == "TitlePlate") continue;
                child.SetActive(false);
                Object.Destroy(child);
            }
            titleText.text = title;
        }

        /// <summary>안내 글 한 줄 (여러 줄이면 height를 늘린다).</summary>
        public void Label(string caption, int size = 24, float height = 48)
        {
            var rect = Rect("Message", panel);
            CreateText(rect, caption, size, skin.textColor);
            rect.gameObject.AddComponent<LayoutElement>().preferredHeight = height;
        }

        /// <summary>이름표 단추. 글에 줄바꿈이 있으면 둘째 줄을 작게 (예: 이어하기 + 최근 자동 저장).</summary>
        public Button Action(string objectName, string caption, Action onClick, bool enabled = true)
        {
            bool twoLines = caption.Contains("\n");
            var rect = Rect(objectName, panel);
            rect.gameObject.AddComponent<LayoutElement>().preferredHeight = twoLines ? 92 : 78;
            var background = rect.gameObject.AddComponent<Image>();
            Slice(background, skin.plate, 3f);
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = background;
            var colors = button.colors;
            colors.highlightedColor = new Color(1.1f, 1.05f, 0.95f);
            colors.pressedColor = new Color(0.8f, 0.75f, 0.7f);
            colors.disabledColor = new Color(0.45f, 0.42f, 0.4f);
            button.colors = colors;
            button.interactable = enabled;
            button.onClick.AddListener(() => onClick());

            string text = caption;
            if (twoLines)
            {
                int cut = caption.IndexOf('\n');
                text = caption.Substring(0, cut) + $"\n<size=18><color=#{ColorUtility.ToHtmlStringRGB(skin.subColor)}>{caption.Substring(cut + 1)}</color></size>";
            }
            CreateText(Stretched("Label", rect), text, 32, skin.textColor);
            if (enabled) controls.Add(button);
            return button;
        }

        /// <summary>설정 창을 띄운다 (메뉴 창은 숨김). 창이 없으면 false.</summary>
        public bool ShowSettings()
        {
            if (!Settings) return false;
            overlay.SetActive(true);
            panel.gameObject.SetActive(false);
            controls.Clear();
            Settings.Open();
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
            if (Settings) Settings.Hide();
            overlay.SetActive(false);
        }

        // ---------------- 도우미 ----------------

        private static void Slice(Image image, Sprite sprite, float multiplier)
        {
            image.sprite = sprite;
            if (!sprite) { image.color = new Color(0.3f, 0.2f, 0.12f); return; }
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = multiplier;
        }

        private static RectTransform Rect(string name, Transform parent)
        {
            var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
            rect.SetParent(parent, false);
            return rect;
        }

        private static RectTransform Stretched(string name, Transform parent)
        {
            var rect = Rect(name, parent);
            Stretch(rect);
            return rect;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        private TMP_Text CreateText(RectTransform rect, string caption, int size, Color color)
        {
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.font = font;
            text.fontSize = size;
            text.text = caption;
            text.alignment = TextAlignmentOptions.Center;
            text.color = color;
            text.richText = true;
            text.raycastTarget = false;
            return text;
        }
    }
}
