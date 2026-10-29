import {D} from '../define/data.js';
import {RELATION,REGULAR_STORIES} from '../define/relationships.js';
import {escapeHTML as esc} from '../utils/runtime.js';
const signed=n=>n>0?'+'+n:String(n);
const button=(id,text)=>`<button data-act="regular-story" data-id="${esc(id)}">${text}</button>`;
export function relationshipSummary(g){
  if(!g?.regularId)return '<p class="small muted">과객 · 개인 이름과 취향을 기억합니다.</p>';
  return `<div class="relationship"><span class="tag">단골 · 레벨 ${g.level}</span><p>💡 호감 ${g.affinity} / ${RELATION.max}</p><progress max="${RELATION.max}" value="${g.affinity}" aria-label="단골 호감"></progress><small>${g.nextAt===null?'모든 사연을 들었습니다':`다음 사연: 호감 ${g.nextAt} · ${g.remaining} 남음`}</small></div>`;
}
export function promiseLine(c){return c?.promise?`<div class="notice">💡 약속 · “무사히 다녀오면 ${esc(D.ING[c.promise].n)} 두 개를 가져올게.”<br>성공·대성공 귀환 시 받습니다.</div>`:'';}
export function regularNotebook(v){
  return `<p class="intro">식탁에서 쌓인 관계와 손님들이 남긴 이야기.</p><div class="notice">💡 호감 수치·사연 초안입니다. 🔴 최종 대사와 물건별 효과는 미정입니다. 사연 속 지도·열쇠는 옛 파티의 생존이나 위치를 확정하지 않습니다.</div>`+v.relationships.regulars.map(g=>`<article class="regular-card"><h3>${esc(g.name)}${g.visits?'':' · 아직 만나지 못함'}</h3><p class="small muted">${esc(g.intro)} · 방문 ${g.visits}회${g.restUntil>v.s.day?' · 부상 회복 중':''}</p>${relationshipSummary(g)}${g.unlocked.length?g.unlocked.map(st=>`<div class="story-entry"><span>💡 호감 ${st.at}에서 들은 이야기</span>${button(st.id,'사연 다시 읽기')}${st.gift?`<p>남긴 물건 · ${esc(st.gift.name)}</p>`:''}</div>`).join(''):'<p class="small muted">호감 3이 되면 첫 사연을 들려줍니다.</p>'}</article>`).join('');
}
export function relationshipReport(r){
  const a=r.relationship;
  const loot=r.loot===undefined?'이전 버전 보고 · 내역 미기록':Object.entries(r.loot).map(([id,n])=>`${esc(D.ING[id]?.n||id)} ×${n}`).join(' · ')||'없음';
  let h=`<div class="notice">가져온 재료 · ${loot}${r.promise?`<br>약속: ${esc(D.ING[r.promise].n)} · ${r.promiseDelivered?'두 개 전달됨':'이번에는 가져오지 못했습니다'}`:''}${r.maxAffinityBonus&&r.result.tier!=='hurt'?'<br>💡 출발 시 최대 호감 보너스 +1 포함':''}</div>`;
  if(!a)return h;
  h+=`<div class="relationship"><h3>💡 호감 ${a.before} → ${a.after} (${signed(a.gain)})</h3><p class="small muted">귀환 결과 ${signed(a.resultGain)} · 맛 반응 ${signed(a.tasteGain)} · 0~12 범위 적용</p>${a.level>a.levelBefore?`<p>레벨 ${a.levelBefore} → ${a.level}</p>`:''}</div>`;
  for(const id of a.stories){const st=REGULAR_STORIES[id];h+=`<section class="story-entry"><h3>💡 새 사연 · 호감 ${st.at}</h3><p>${esc(st.text)}</p>${st.gift?`<div class="notice">진열 선반에 남긴 물건 · ${esc(st.gift.name)} 💡 임시 모델</div>`:''}${button(id,'이 사연 다시 읽기')}</section>`;}
  if(a.map)h+=`<div class="notice">💡 지도로 ${a.map.floor}층의 ${D.AX[a.map.axis].name} 위험 ${a.map.value}을 수첩에 기록했습니다. 구조 사건의 위치 단서는 아닙니다.</div>`;
  return h;
}
export function regularStoryDialog(st){return `<span class="eyebrow">REGULAR'S STORY</span><h2>${esc(st.guestName)} · 호감 ${st.at}의 이야기</h2><p>${esc(st.text)}</p>${st.gift?`<p>남긴 물건 · ${esc(st.gift.name)}</p>`:''}<div class="notice">💡 사연 초안 · 🔴 최종 대사·물건 효과 미정. 다시 읽어도 보상을 중복 지급하지 않습니다.</div><div class="actions"><button data-act="close-dialog" class="primary">돌아가기</button></div>`;}
