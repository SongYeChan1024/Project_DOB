// =====================================================================
// GoldCounter — 금화를 쓴 뒤 지갑 숫자가 하나씩 줄고 -N이 떠오르는 연출 (시안 16 ② · 21 ②)
// 역할   : 시장 결제 · 가게 개량이 같이 쓴다. 숫자를 바꾸기만 하고 금화 값은 계산하지 않는다.
// 호출자 : MarketView, UpgradeView (StartCoroutine(GoldCounter.PayOut(...)))
// 상태   : 없음
// =====================================================================
using System.Collections;
using TMPro;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>지갑 숫자 카운트다운.</summary>
    public static class GoldCounter
    {
        // 의사코드: 지갑 글자를 복사해 빨간 -N을 만들고, 정해진 시간 동안 숫자를 결제 전 → 후로 줄이며
        //           -N은 위로 떠오르며 흐려진다. 끝나면 -N을 지우고 결제 후 숫자로 맞춘다
        /// <summary>goldText를 from → to로 줄이며 -N을 띄운다. 실제 시간으로 움직인다.</summary>
        public static IEnumerator PayOut(TMP_Text goldText, int from, int to, float seconds)
        {
            var minus = Object.Instantiate(goldText, goldText.transform.parent);
            minus.text = "-" + (from - to); // 글꼴에 − 기호가 없어 하이픈을 쓴다
            minus.color = new Color32(220, 90, 60, 255);
            minus.raycastTarget = false;
            var mr = minus.rectTransform;
            Vector2 home = mr.anchoredPosition + new Vector2(0f, mr.rect.height * 0.6f); // 지갑 숫자 바로 위에서 떠오른다
            for (float t = 0f; t < seconds; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float k = t / seconds;
                goldText.text = Mathf.RoundToInt(Mathf.Lerp(from, to, k)).ToString();
                mr.anchoredPosition = home + new Vector2(0f, 40f * k);
                minus.alpha = 1f - k * k;
                yield return null;
            }
            Object.Destroy(minus.gameObject);
            goldText.text = to.ToString();
        }
    }
}
