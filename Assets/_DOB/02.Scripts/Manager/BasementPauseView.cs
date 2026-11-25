// 코드로 오버레이와 선택 항목을 만든다. 화면 전환 시 이전 버튼 리스너와 선택을 정리한다.
// 호출자: GameManager(임시 데모 엔딩 창). 일시정지 메뉴는 PauseMenuView로 옮겨 갔다.
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOB.Manager
{
    internal sealed class BasementPauseView
    {
        private readonly GameObject overlay;
        private readonly RectTransform panel;
        private readonly Font font;
        private readonly List<Selectable> controls = new List<Selectable>();

        public BasementPauseView(Transform parent, Font textFont)
        {
            font = textFont;
            overlay = new GameObject("BasementPauseCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            overlay.transform.SetParent(parent, false);
            if (parent.GetComponentInParent<Canvas>())
                Stretch((RectTransform)overlay.transform);
            var canvas = overlay.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 850;
            var scaler = overlay.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;

            var shade = Rect("Backdrop", overlay.transform);
            Stretch(shade);
            shade.gameObject.AddComponent<Image>().color = new Color(0.04f, 0.06f, 0.07f, 0.88f);
            panel = Rect("Menu", shade);
            panel.sizeDelta = new Vector2(572, 0);
            panel.gameObject.AddComponent<Image>().color = new Color(0.14f, 0.19f, 0.19f);
            var stack = panel.gameObject.AddComponent<VerticalLayoutGroup>();
            stack.padding = new RectOffset(26, 26, 20, 20);
            stack.spacing = 8;
            stack.childControlWidth = true;
            stack.childForceExpandWidth = true;
            stack.childControlHeight = true;
            stack.childForceExpandHeight = false;
            panel.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            Hide();
        }

        public void Begin(string title)
        {
            overlay.SetActive(true);
            controls.Clear();
            for (int i = panel.childCount - 1; i >= 0; i--)
            {
                var child = panel.GetChild(i).gameObject;
                child.SetActive(false);
                UnityEngine.Object.Destroy(child);
            }
            Label(title, 34, 52);
        }

        public Text Label(string caption, int size = 23, float height = 48)
        {
            var rect = Rect("Message", panel);
            var text = CreateText(rect, caption, size);
            rect.gameObject.AddComponent<LayoutElement>().preferredHeight = height;
            return text;
        }

        public Button Action(string objectName, string caption, Action onClick, bool enabled = true)
        {
            var rect = Rect(objectName, panel);
            rect.gameObject.AddComponent<LayoutElement>().preferredHeight = caption.Contains("\n") ? 64 : 48;
            var background = rect.gameObject.AddComponent<Image>();
            background.color = new Color(0.30f, 0.39f, 0.35f);
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = background;
            button.interactable = enabled;
            button.onClick.AddListener(() => onClick());
            var label = Rect("Label", rect);
            Stretch(label);
            CreateText(label, caption, 23);
            if (enabled) controls.Add(button);
            return button;
        }

        public void Volume(float value, Action<float> changed)
        {
            var holder = Rect("MasterVolume", panel);
            holder.gameObject.AddComponent<LayoutElement>().preferredHeight = 44;
            var slider = holder.gameObject.AddComponent<Slider>();
            var track = Rect("Track", holder);
            Stretch(track);
            track.offsetMin = new Vector2(14, 16);
            track.offsetMax = new Vector2(-14, -16);
            track.gameObject.AddComponent<Image>().color = new Color(0.28f, 0.34f, 0.33f);
            var filled = Rect("Fill", track);
            Stretch(filled);
            filled.gameObject.AddComponent<Image>().color = new Color(0.77f, 0.78f, 0.59f);
            var thumb = Rect("Handle", track);
            thumb.sizeDelta = new Vector2(22, 15);
            var image = thumb.gameObject.AddComponent<Image>();
            image.color = new Color(0.99f, 0.92f, 0.77f);
            slider.targetGraphic = image;
            slider.fillRect = filled;
            slider.handleRect = thumb;
            slider.value = value;
            slider.onValueChanged.AddListener(v => changed(v));
            controls.Add(slider);
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

        public void Hide() => overlay.SetActive(false);

        private static RectTransform Rect(string name, Transform parent)
        {
            var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
            rect.SetParent(parent, false);
            return rect;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        private Text CreateText(RectTransform rect, string caption, int size)
        {
            var text = rect.gameObject.AddComponent<Text>();
            text.font = font;
            text.fontSize = size;
            text.text = caption;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(1f, 0.96f, 0.85f);
            text.raycastTarget = false;
            return text;
        }
    }
}
