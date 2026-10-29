// 제공된 원본 HTML의 선화 초상과 제목 그림을 보존한다.
function portrait(cls, mood, opts) {
  opts = opts || {};
  const mouth = { happy: 'M26 38 Q32 44 38 38', neutral: 'M27 39 H37', sad: 'M26 42 Q32 36 38 42' }[mood || 'neutral'];
  const extras = {
    warrior: '<path d="M17 31 C17 14 47 14 47 31"/><path d="M15 31 H49"/><path d="M32 18 V35"/><path d="M32 14 C30 7 38 5 40 10"/>',
    mage: '<path d="M16 25 L32 2 L48 25"/><path d="M10 26 Q32 33 54 26"/><path d="M32 12 l1.6 3.4 3.4.4-2.6 2.3.7 3.4-3.1-1.8-3.1 1.8.7-3.4-2.6-2.3 3.4-.4z" stroke-width="1.4"/>',
    rogue: '<path d="M16 38 C13 12 51 12 48 38"/><path d="M19 26 H45 V34 H19 Z"/>',
    priest: '<ellipse cx="32" cy="11" rx="11" ry="3.6"/><path d="M32 50 V62"/><path d="M26 55 H38"/>',
    miner: '<path d="M17 29 C17 14 47 14 47 29 Z"/><circle cx="32" cy="18" r="3.2"/><path d="M32 11 V7 M24 13 L21 10 M40 13 L43 10"/><path d="M20 38 Q32 58 44 38"/>',
    archer: '<path d="M16 37 C13 13 51 13 48 37"/><path d="M45 17 C54 8 57 13 51 22"/><path d="M58 22 Q68 40 58 58"/><path d="M58 22 V58"/>',
  }[cls] || '';
  const eyeY = (cls === 'rogue') ? 30 : 31;
  const sweat = opts.tired ? '<path d="M52 22 q3 4 0 7 q-3 -3 0 -7" stroke-width="1.6"/>' : '';
  const bandage = opts.hurt ? '<path d="M20 24 L44 19" stroke-width="4.5" opacity=".9"/>' : '';
  return '<svg class="portrait' + (opts.sm ? ' sm' : '') + '" viewBox="-4 0 72 64" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">' +
    '<path d="M8 63 C8 49 18 46 32 46 C46 46 56 49 56 63"/><circle cx="32" cy="32" r="13"/>' + extras + bandage +
    '<circle class="hl" cx="27" cy="' + eyeY + '" r="1.7"/><circle class="hl" cx="37" cy="' + eyeY + '" r="1.7"/><path d="' + mouth + '"/>' + sweat + '</svg>';
}

function titleArt() {
  let p = 'M10 30 ';
  const down = (x, y, n, sx, sy) => { let d = ''; for (let i = 0; i < n; i++) { d += 'h' + sx + ' v' + sy + ' '; } return d; };
  p += down(0, 0, 5, 38, 20);              // 왼쪽 계단 (아래로)
  const landingX = 10 + 5 * 38, landingY = 30 + 5 * 20;
  p += 'h150 ';                            // 계단참
  p += down(0, 0, 4, 38, 20);              // 오른쪽 계단
  return '<svg class="title-art" viewBox="0 0 640 230" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">' +
    '<path d="' + p + '" opacity=".85"/>' +
    '<path d="M10 30 H0" opacity=".0"/>' +
    /* 탁자 */
    '<path d="M' + (landingX + 34) + ' ' + (landingY - 30) + ' h80 M' + (landingX + 42) + ' ' + (landingY - 30) + ' v30 M' + (landingX + 106) + ' ' + (landingY - 30) + ' v30"/>' +
    /* 컵과 김 */
    '<path d="M' + (landingX + 58) + ' ' + (landingY - 30) + ' v-14 h16 v14"/><path d="M' + (landingX + 66) + ' ' + (landingY - 54) + ' q-6 -8 0 -14 q6 -6 0 -14" opacity=".7"/>' +
    /* 랜턴 */
    '<path d="M' + (landingX + 120) + ' 60 V' + (landingY - 78) + '"/><rect x="' + (landingX + 110) + '" y="' + (landingY - 78) + '" width="20" height="26" rx="4"/><path d="M' + (landingX + 120) + ' ' + (landingY - 72) + ' v14" opacity=".6"/>' +
    /* 아래로 이어지는 어둠 */
    '<path d="M470 190 v0" /><path d="M515 190 l40 0 M515 210 l60 0" opacity=".35"/>' +
    '</svg>';
}


export {portrait,titleArt};
