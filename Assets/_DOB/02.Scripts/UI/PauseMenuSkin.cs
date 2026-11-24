// =====================================================================
// PauseMenuSkin — ESC 메뉴의 겉모습 (글꼴 · 나무 창 · 이름표 단추 · 환경설정 창 프리팹)
// 역할   : 코드로 만드는 ESC 메뉴(PauseMenuView)가 다른 화면과 같은 그림을 쓰도록 참조를 모아 둔다.
// 위치   : Resources/UI/PauseMenuSkin.asset (BasementPauseMenu가 씬 연결 없이 읽는다)
// 만드는 곳 : DOB > HJW > 환경설정 만들기
// =====================================================================
using TMPro;
using UnityEngine;

namespace DOB.UI
{
    [CreateAssetMenu(menuName = "DOB/UI/Pause Menu Skin", fileName = "PauseMenuSkin")]
    public sealed class PauseMenuSkin : ScriptableObject
    {
        public TMP_FontAsset font;
        /// <summary>나무 창 (9-slice).</summary>
        public Sprite window;
        /// <summary>이름표 단추 · 제목판 (9-slice).</summary>
        public Sprite plate;
        public SettingsView settingsPrefab;
        public Color titleColor = new Color32(240, 196, 110, 255);
        public Color textColor = new Color32(245, 232, 205, 255);
        public Color subColor = new Color32(225, 205, 170, 255);
    }
}
