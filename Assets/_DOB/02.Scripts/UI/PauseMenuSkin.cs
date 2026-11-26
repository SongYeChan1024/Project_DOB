// =====================================================================
// PauseMenuSkin — ESC 메뉴 · 환경설정 창 프리팹을 씬 연결 없이 찾는 곳
// 역할   : ESC 메뉴 프리팹(PF_PauseMenu)과 환경설정 창 프리팹(PF_Settings)을 가리킨다.
//          글꼴 · 그림 · 색은 프리팹을 만들 때 쓰는 값이다 (실행 중 화면은 프리팹의 값을 쓴다).
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
        /// <summary>ESC 메뉴 프리팹 (PF_PauseMenu). BasementPauseMenu가 복제한다.</summary>
        public PauseMenuView menuPrefab;
        public Color titleColor = new Color32(240, 196, 110, 255);
        public Color textColor = new Color32(245, 232, 205, 255);
        public Color subColor = new Color32(225, 205, 170, 255);
    }
}
