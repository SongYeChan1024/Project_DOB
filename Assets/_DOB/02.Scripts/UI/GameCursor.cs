// =====================================================================
// GameCursor — 게임 마우스 커서 (갈색 속 + 황동 테 화살표)
// 역할   : 게임이 시작되면 기본 마우스 커서를 게임 커서 그림으로 바꾼다.
//          씬에 따로 붙일 필요 없이 자동으로 실행된다.
// 호출자 : 유니티 (게임 시작 시 한 번, RuntimeInitializeOnLoadMethod)
// 상태   : 없음
// 그림   : Resources/UI/T_UI_Cursor.png (32x32, 텍스처 종류 = Cursor)
//          윈도우 기본 커서 크기(32)와 같게 만들어서 줄어들며 깨지지 않는다.
// 주의   : 숨은 단서 위에서는 커서를 바꾸지 않는다 (바뀌면 정답이 들킴).
// =====================================================================
using UnityEngine;

namespace DOB.UI
{
    /// <summary>게임 시작 시 마우스 커서를 게임 그림으로 바꾼다.</summary>
    public static class GameCursor
    {
        /// <summary>Resources 폴더 기준 그림 경로 (확장자 없이).</summary>
        private const string CursorPath = "UI/T_UI_Cursor";

        /// <summary>클릭이 일어나는 점 = 화살표 끝 (그림 왼쪽 위에서 픽셀).</summary>
        private static readonly Vector2 Hotspot = new Vector2(1, 1);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Apply()
        {
            var texture = Resources.Load<Texture2D>(CursorPath);
            if (texture == null)
            {
                Debug.LogWarning("[GameCursor] 커서 그림을 못 찾았어요: Resources/" + CursorPath + ".png");
                return;
            }
            Cursor.SetCursor(texture, Hotspot, CursorMode.Auto);
        }
    }
}
