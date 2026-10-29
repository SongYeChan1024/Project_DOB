import { D, AXES, RACES, SEATS, CFG, CLUES, SCENES, TIERS, PREVIEW } from "../define/data.js";
import { escapeHTML as esc } from "../utils/runtime.js";
import {portrait,titleArt} from './originalArt.js';
import {VISIT_STATUS,isActiveVisit,ORIGINAL_ECONOMY,HINT_WORDS} from '../define/service.js';
import {TASTE_LINES} from '../define/taste.js';
import {relationshipSummary,promiseLine,regularNotebook,relationshipReport} from './relationshipPanels.js';
const b = (act, label, disabled = false, id = "", cls = "") => `<button data-act="${act}" data-id="${id}" class="${cls}" ${disabled ? "disabled" : ""}>${label}</button>`;
const head = (eyebrow, title) => `<span class="eyebrow">${eyebrow}</span><h2>${title}</h2>`;
const pills = (e) => `<div class="pills">${AXES.filter((a) => e[a]).map((a) => `<span class="pill ax-${a}">${D.AX[a].eff} ${e[a]}</span>`).join("")}</div>`;
const divider = '<div class="divider"></div>';
const flavorPills=tags=>`<div class="pills">${tags.map(t=>`<span class="pill flavor">맛 · ${esc(t)}</span>`).join('')}</div>`;
const preferences=g=>`<div class="taste-info"><span>알아낸 입맛</span><p>좋아함 · ${(g.knownLikes||[]).map(esc).join(' · ')||'?'}${(g.knownLikes||[]).length<(g.likes||[]).length?' · ?':''}<br>싫어함 · ${(g.knownDislikes||[]).map(esc).join(' · ')||'?'}</p></div>`;
function panel(v, u) {
  const s = v.s;
  if(!s)return '<section class="title-screen"><span class="eyebrow">오늘 밤, 던전 식당 · 💡 가제</span><h1>층계참 식당</h1>'+titleArt()+'<p class="intro">던전의 계단참에 작은 식당을 열었다.<br>손님의 말을 듣고, 돌아올 힘을 담은 한 끼를 준비한다.</p><div class="title-actions">'+b('start','영업 시작',false,'normal','primary')+b('continue','이어하기',!v.normalSave,'normal')+'</div><p class="small muted">원본의 경영 흐름에 3D 이동·조리·서빙을 더한 시제품<br>영업 중 WASD 이동 · 가까이에서 E · 드래그로 시점 회전</p><details class="trial-options"><summary>💡 기획 검토용 시험 모드</summary><p class="small muted">5층 조사 또는 단골 호감 2·6·11에서 시작합니다. 일반 영업과 다른 저장입니다.</p><div class="actions">'+b('start','5층 조사부터 체험',false,'story')+b('continue','시험 이어하기',!v.storySave,'story')+b('start','단골 사연부터 체험',false,'relations')+b('continue','단골 시험 이어하기',!v.relationsSave,'relations')+'</div></details></section>';
  if (s.phase === "ending") return ending(s);
  if (s.phase === "report" && u.station !== "book") return report(v);
  if(s.phase==='close'&&u.station!=='book')return closeDay(v);
  if (u.station === "book") return notebook(v, u);
  if (u.station === "stairs") return stairs(v, u);
  if (s.phase === "prep") return prep(v, u);
  if (u.station === "kitchen") return kitchen(v);
  if (u.station === "stove") return stove(v);
  return guest(v);
}
function recipeLibrary(v){
  return '<div class="recipe-library">'+D.RECIPES.map(r=>{
    const missing=Object.keys(r.ing).filter(id=>!v.s.discovered[id]);
    if(missing.length)return '<article class="recipe-note locked"><h3>???</h3><p class="small muted">'+Math.min(...missing.map(id=>D.ING[id].floor||1))+'층 재료를 발견하면 열립니다.</p></article>';
    const entry=v.recipes.find(x=>x.id===r.id);
    return '<article class="recipe-note"><div class="row"><h3>'+r.n+'</h3><span class="small muted">'+r.price+'g</span></div>'+pills(entry.effects)+flavorPills(r.tags)+'<p class="small muted">'+Object.entries(r.ing).map(([id,n])=>D.ING[id].n+' '+(v.s.stock[id]||0)+'/'+n).join(' · ')+'</p></article>';
  }).join('')+'</div>';
}
function prep(v,u){
  const s=v.s,tab=['market','recipes','notes','upgrade'].includes(u.prepTab)?u.prepTab:'market';
  const news=s.anomaly?s.anomaly.floor+'층의 '+D.AX[s.anomaly.axis].name+' 위험이 짙어졌다는 소식이다.':'오늘은 던전이 잠잠하다는 소식이다.';
  let h='<div class="phase-news"><span class="eyebrow">'+s.day+'일차 · '+(s.phase==='close'?'영업 종료 후 준비':'오늘의 소식')+'</span><p>'+news+'</p></div><div class="tabs prep-tabs">'+[['market','시장'],['recipes','레시피'],['notes','수첩'],['upgrade','가게']].map(([id,label])=>b('prep-tab',label,false,id,tab===id?'active':'')).join('')+'</div>';
  if(tab==='market'){
    const row=item=>'<div class="market-row"><strong>'+item.name+'</strong><span class="small muted">보유 '+item.have+'</span><span>'+item.cost+'g</span>'+b('buy','+1',s.gold<item.cost,item.id)+b('buy-five','+5',s.gold<item.cost*5,item.id)+'</div>';
    const groups=[['기본 재료',v.market.filter(x=>D.ING[x.id]?.base)],['양념 · 요리마다 효과 +1',v.market.filter(x=>D.SPICES[x.id])],['행상인의 던전 재료 · 구매 가격 4배',v.market.filter(x=>D.ING[x.id]&&!D.ING[x.id].base)]];
    h+='<div class="market-groups">'+groups.filter(([,items])=>items.length).map(([title,items])=>'<section class="paper-panel"><h3>'+title+'</h3>'+items.map(row).join('')+'</section>').join('')+'</div>';
  }else if(tab==='recipes')h+=recipeLibrary(v);
  else if(tab==='notes')h+=notebook(v,u);
  else{
    const seatCost=ORIGINAL_ECONOMY.seatCosts[s.seats.length-3],stoveCost=ORIGINAL_ECONOMY.stoveCosts[s.stove],shelfCost=s.shelf<2?ORIGINAL_ECONOMY.shelfCost:null;
    const nextSeat=SEATS.find(t=>!s.seats.includes(t.id));
    h='<div class="upgrade-context">'+h+'</div><div class="upgrade-list">'+[
      ['좌석 늘리기',s.seats.length+'석','하루에 받을 수 있는 손님이 한 명 늘어납니다. 💡 동시에 머물 수 있는 자리도 늘어납니다.',seatCost,'upgrade-seat',nextSeat?.id],
      ['화덕 개량','+'+s.stove,'요리의 가장 강한 효과 +1. 두 번째 개량은 그다음 효과도 +1.',stoveCost,'upgrade-stove',''],
      ['양념 선반',s.shelf+'칸','요리 하나에 양념을 하나 더 넣을 수 있습니다.',shelfCost,'upgrade-shelf','']
    ].map(([title,now,desc,cost,act,id])=>'<article class="upgrade-item"><div><h3>'+title+' <span class="small muted">현재 '+now+'</span></h3><p class="intro">'+desc+'</p></div>'+b(act,cost?cost+'g · 개량':'최대 단계',!cost||s.gold<cost,id??'')+'</article>').join('')+'</div><p class="small muted">원본 가격: 좌석 70/130/200g · 화덕 90/180g · 선반 120g.<br>💡 자리별 효과는 3D 시제품 제안입니다. 🔴 인접 보너스는 미정입니다.</p>';
  }
  return h+'<div class="day-cta"><span>좌석 '+s.seats.length+'석 · 금화 <b>'+s.gold+'g</b></span><div class="actions">'+(u.management?b('close-dialog','식당으로 돌아가기',false,'','primary'):b('open','영업 시작',false,'','primary'))+'</div></div>';
}
function closeDay(v){
  const s=v.s;return head(s.day+'일차 · 영업 종료','가게 문을 닫는다')+'<div class="daily-summary">'+[['받은 손님',s.pending.length],['매출',s.income+'g'],['지출',(s.spent||0)+'g'],['금고',s.gold+'g']].map(([label,n])=>'<div><span>'+label+'</span><b>'+n+'</b></div>').join('')+'</div><section class="paper-panel"><h3>지금 던전 안에 있는 손님</h3><div class="departed-list">'+(s.pending.map(r=>'<article>'+portrait(s.guests[r.guestId].job,'neutral',{sm:true})+'<div><b>'+esc(r.name)+'</b><p class="small muted">'+r.floor+'층 · '+r.recipe+'</p></div></article>').join('')||'<p class="intro">오늘은 아무도 내려가지 않았다.</p>')+'</div></section><div class="actions">'+b('management','번 돈으로 가게 개량',false,'','primary')+b('morning','밤이 지나고 다음 날로')+'</div>';
}
function markedLine(text,key){let result=esc(text);for(const word of HINT_WORDS[key]||[])result=result.replaceAll(word,'<mark class="hint-word ax-'+(key==='tired'?'vital':key)+'">'+word+'</mark>');return result;}
function guest(v) {
  const { guest: g, visit: c, s } = v;
  if (!g) return "<p>다음 흐름을 선택하세요.</p>";
  let h = `<div class="guest-heading">${portrait(g.job,"neutral",{tired:g.tired})}<div>${head("식탁을 찾은 손님",esc(g.name))}<span class="small muted">${g.floor}층 · ${D.FLOORS[g.floor].name} · Lv.${g.level||1}</span></div></div>` + `<div class="row"><span class="tag">${RACES[g.race].name}</span><span class="tag">${D.CLASSES[g.job].n}</span><span class="tag ${g.tired ? "warn" : "good"}">${g.tired ? "어깨가 축 처져 있다" : "발걸음이 가볍다"}</span></div><p class="small muted" style="margin-top:10px">${D.CLASSES[g.job].trait} · ${D.AX[D.CLASSES[g.job].res].name} −1<br>💡 종족 약점: ${RACES[g.race].weak ? D.AX[RACES[g.race].weak].name + " +1" : "없음"}</p>`+(c.status==="waiting"?`<div class="quote">“${markedLine(c.opening,c.openingKey)}”</div>`:"")+preferences(g)+relationshipSummary(g)+promiseLine(c);
  if(c.status==='carrying')return h+`<div class="quote">접시를 들고 ${SEATS[c.seat].name} 자리로.</div><p class="intro">손님 가까이에서 E를 누르면 음식을 내어 줍니다. 금색 원이 전달 위치를 알려 줍니다.</p><div class="actions">${b('serve','손님에게 음식 내기',false,'','primary')}</div>`;
  if (c.status === "served") return h + `<div class="quote">“${TASTE_LINES[c.taste?.score??2]}”<br><span class="small muted">“${esc(c.feel||"돌아올 길까지 잘 챙겨 볼게.")}”</span></div><p class="intro">맛 만족 ${c.taste?.score??2}/4 · ${g.name}에게 ${c.price}g를 받았습니다. 탐험 결과는 내일 아침입니다.</p>${(c.learnedTaste||[]).map(t=>`<div class="notice">새로 알아낸 취향 · ${esc(t.tag)} 맛을 ${t.kind==='like'?'좋아합니다':'싫어합니다'}.</div>`).join('')}<div class="actions">${b("next", s.visits.filter(isActiveVisit).length===1 ? "배웅하고 영업 마감" : "이 손님 배웅하기", false, "", "primary")}</div>`;
  if (c.seat === null) h += `<h3>먼저 자리를 정해 주세요</h3><p class="small muted">말과 외형에서 드러나는 손님의 상태를 고려합니다. 💡 이 시제품에서는 조리 전 자리 변경이 가능합니다.</p>`;
  else h += `<div class="quote">“${c.hint}”</div><p class="intro">목적지 · <b>${g.floor}층 ${D.FLOORS[g.floor].name}</b></p>${c.assignment ? '<div class="notice">이번 방문에 이야기 전달·조사 요청이 배정됐습니다.</div>' : ""}${divider}<h3>선택한 자리 · ${SEATS[c.seat].name}</h3>`;
  h += `<div class="seat-grid">${SEATS.map((t) => b("seat", `${t.name}<small>${v.s.visits.some(x=>x!==c&&isActiveVisit(x)&&x.seat===t.id)?"다른 손님 이용 중":s.seats.includes(t.id) ? D.AX[t.axis].eff + " +1" : "아직 잠긴 자리"}</small>`, !s.seats.includes(t.id) || c.status !== "waiting" || s.visits.some(x=>x!==c&&isActiveVisit(x)&&x.seat===t.id), t.id, c.seat === t.id ? "active" : "")).join("")}</div><div class="actions">${b("station", c.status === "waiting" ? "조리대로" : "화덕 확인", c.seat === null, c.status === "waiting" ? "kitchen" : "stove", "primary")}${b("decline", "오늘은 돌려보내기", c.status !== "waiting")}</div>`;
  return h;
}
function kitchen(v) {
  if(!v.visit)return head('KITCHEN','손님을 선택해 주세요');
  if(v.visit.status!=='waiting')return stove(v);
  if(v.visit.seat===null)return guest(v);
  return head('A MEAL AS A PRESCRIPTION',esc(v.guest.name)+'의 한 끼')+
    '<div class="quote">'+esc(v.visit.hint)+'</div><p class="intro">가운데 조리 도구에 재료를 하나씩 넣어 요리를 준비합니다.</p>'+preferences(v.guest)+
    '<div class="actions">'+b('open-cooking','요리 전용 화면 열기',false,'','primary')+'</div>';
}
function stove(v) {
  const c=v.visit;
  if(!c)return head('THE HEARTH','손님을 선택해 주세요');
  if(c.status==='served'||c.status==='carrying')return guest(v);
  const orders=v.s.visits.filter(x=>['cooking','ready'].includes(x.status));
  let h=head('THE HEARTH','완성을 기다리는 한 끼')+'<p class="small muted">손님별로 접시가 구분됩니다. 한 번에 한 접시를 운반합니다.</p>';
  h+=orders.map(x=>'<div class="order-card">'+b('select-order',esc(v.s.guests[x.guestId].name)+' · '+D.RECIPES.find(r=>r.id===x.recipe).n+'<small>'+VISIT_STATUS[x.status]+'</small>',false,x.id,x.id===c.id?'active':'')+'<div class="progress"><div data-cook-progress="'+x.id+'" style="width:'+(x.status==='ready'?100:0)+'%"></div></div></div>').join('');
  if(['ready','cooking'].includes(c.status))h+='<div class="actions">'+b('pickup',c.status==='ready'?v.guest.name+'의 접시 집기':'선택한 요리 조리 중…',c.status!=='ready'||!!v.held,'','primary')+'</div>';
  if(!orders.length)h+='<p class="empty">완성 대기 중인 음식이 없습니다.</p>';
  return h+'<div class="actions">'+b('station','조리대로',c.seat===null,'kitchen')+'</div>';
}
export function serviceRoster(v){
  const active=v.s.visits.filter(isActiveVisit);
  return '<section class="service-roster" aria-label="식당 안 손님"><div class="row"><h3>식당 안 손님 · '+active.length+'명</h3><small>선택해서 응대</small></div><div class="roster-grid">'+active.map(c=>b('select-visit',esc(v.s.guests[c.guestId].name)+'<small>'+(c.seat===null?'입장 대기':(c.seat+1)+'번 자리')+' · '+VISIT_STATUS[c.status]+'</small>',false,c.id,c===v.visit?'active':'')).join('')+'</div>'+(v.held?'<p class="carry-notice">운반 중 · '+esc(v.s.guests[v.held.guestId].name)+'의 접시</p>':'')+'<div class="actions">'+b('ask-close','오늘 영업 마치기')+'</div></section>';
}
export function managementDialog(v,u){
  return '<span class="eyebrow">AFTER SERVICE · 오늘 매출 '+v.s.income+'g</span><h2>번 돈으로, 내일을 준비한다</h2><p class="manage-gold">보유 골드 <strong>'+v.s.gold+'g</strong></p>'+prep(v,{...u,management:true});
}
function report(v) {
  const r = v.report;
  if (!r) return "<p>귀환 정산을 마쳤습니다.</p>";
  const result = r.result;
  return head(`MORNING · ${v.s.reportIndex + 1}/${v.s.reports.length}`, `${esc(r.name)}의 귀환`) + `<div class="result-title ${result.tier}">${TIERS[result.tier]}</div><p class="intro">${r.floor}층 · ${r.recipe}<br>${result.tier === "hurt" ? "다친 곳을 먼저 돌봤다. 회복하는 동안 잠시 쉬어 가기로 했다." : result.tier === "narrow" ? "간신히 돌아왔다. 어디서 준비가 모자랐는지 함께 짚는다." : "준비한 한 끼 덕분에 무사히 돌아왔다."}</p><div class="notice">${result.surprise ? result.absorbed ? "돌발 상황을 남겨 둔 활력으로 버텼습니다." : `돌발 · ${D.AX[result.surprise].name} 위험 +1` : "이번 탐험에는 돌발 상황이 없었습니다."}</div>${relationshipReport(r)}${r.newIngredients?.length?`<div class="notice">새 재료 · ${r.newIngredients.map(id=>D.ING[id].n).join(" · ")}</div>`:""}${r.newRecipes?.length?`<div class="notice">새 레시피 · ${r.newRecipes.map(id=>D.RECIPES.find(x=>x.id===id).n).join(" · ")}</div>`:""}${r.newFloor?`<div class="notice">${r.newFloor}층 ${D.FLOORS[r.newFloor].name}으로 가는 길이 열렸습니다.</div>`:""}<table><thead><tr><th>위험 / 효과</th><th>위험</th><th>효과</th><th>부족</th></tr></thead><tbody>${AXES.map((a) => `<tr><td>${D.AX[a].name} / ${D.AX[a].eff}</td><td class="num">${result.H[a]}</td><td class="num">${result.E[a]}</td><td class="num ${result.gaps[a] ? "missing" : ""}">${result.gaps[a]}</td></tr>`).join("")}</tbody></table><p class="small muted" style="margin-top:12px">부족 합계 ${result.deficit} · 보상과 발견한 사실은 이미 저장되었습니다. 다시 열어도 중복 지급하지 않습니다.</p><div class="actions">${b("report-next", v.s.reportIndex === v.s.reports.length - 1 ? "가게 준비하기" : "다음 귀환 보고", false, "", "primary")}${v.s.reportIndex<v.s.reports.length-1?b("report-all","모두 확인하고 가게 준비"):""}</div>`;
}
function notebook(v, u) {
  const s = v.s;
  let h = head("THE NOTEBOOK", "돌아온 이야기들") + `<div class="tabs">${b("book-tab", "사건 단서", false, "story", u.bookTab === "story" ? "active" : "")}${b("book-tab", "층 기록", false, "floors", u.bookTab === "floors" ? "active" : "")}${b("book-tab", "단골 사연", false, "regulars", u.bookTab === "regulars" ? "active" : "")}${b("book-tab", "손님 기록", false, "guests", u.bookTab === "guests" ? "active" : "")}${b("book-tab","사전",false,"dict",u.bookTab==="dict"?"active":"")}</div>`;
  if(u.bookTab === "regulars") h+=regularNotebook(v);
  else if(u.bookTab==="dict")h+='<div class="dictionary">'+AXES.map(a=>'<div class="clue"><span class="pill ax-'+a+'">'+D.AX[a].name+' → '+D.AX[a].eff+'</span><p>'+D.AX[a].words+'</p></div>').join('')+'<p class="small muted">피곤한 손님은 체력 위험 +1. 위험과 효과는 같은 종류끼리 대응합니다. 활력 여유는 돌발 상황에 대비합니다.</p></div>';
  else if (u.bookTab === "floors") for (let i = 1; i <= Math.max(s.floor,...Object.keys(s.known).map(Number)); i++) h += `<div class="clue"><b>${i}층 · ${D.FLOORS[i].name}${i>s.floor?" · 💡 지도 미리보기":""}</b><p>${AXES.filter(a=>(D.FLOORS[i].haz[a]||0)>0).map(a=>s.known[i]?.[a]===undefined?"? 아직 모르는 위험":D.AX[a].name+" "+s.known[i][a]).join(" · ")}</p></div>`;
  else if (u.bookTab === "guests") h += Object.values(s.guests).map((g) => `<div class="clue"><b>${esc(g.name)}</b><p>${RACES[g.race].name} · ${D.CLASSES[g.job].n} · 방문 ${g.visits}회<br>${g.restUntil > s.day ? "회복 중 · " + (g.restUntil - s.day) + "일 뒤 방문 가능" : "다시 만날 수 있는 손님"}</p>${preferences(g)}</div>`).join("") || '<p class="empty">아직 손님이 없습니다.</p>';
  else {
    h += `<p class="intro">5층은 끝이 아니다. 그 어딘가에, 아직 확인하지 못한 이야기가 있다.</p>`;
    h += s.story.clues.length ? s.story.clues.map((id) => `<div class="clue"><b>${CLUES[id].name}</b><p>${CLUES[id].text}</p></div>`).join("") : '<div class="empty">5층을 다녀온 손님이 옛 파티의 흔적을 가져올 수 있습니다. 대성공 횟수만으로 사건이 해결되지는 않습니다.</div>';
    if (s.story.clues.includes("gap") && !s.story.afterReturn) {
      h += `${divider}<h3>5층에 부탁할 조사</h3><p class="small muted">준비 시간에 요청하면 다음 적격 손님에게 배정됩니다. 돌려보낸 요청은 다음 영업에 다시 배정됩니다.</p>`;
      for (const [k, label] of [["question", "현재 질문 전달"], ["survey", "외부 경로 조사"]]) {
        const req = Object.values(s.story.requests).find((r) => r.kind === k);
        h += `<div class="actions">${b("request", req ? req.done ? label + " · 완료" : label + " · 답변 대기" : label, !!req || s.phase !== "prep", k)}</div>`;
      }
    }
    h += `${divider}<h3>확인한 장면</h3><div class="stack">${s.story.done.map((id) => b("replay", SCENES[id].title, false, id, "ghost")).join("") || '<p class="small muted">귀환 보고 후 안전한 아침에 장면을 확인합니다.</p>'}</div><div class="notice">💡 한 아침에 이야기 장면 하나. 생략해도 사실은 남습니다. 기간·생존 재료·지형·파티 구성은 🔴 미정입니다.</div>`;
  }
  return h + (s.phase==="prep"||u.management?"":`<div class="actions">${b("return-service",s.phase==="report"?"귀환 보고로":"식당으로",false,"","primary")}</div>`);
}
function stairs(v, u) {
  const s = v.s;
  if (s.story.afterReturn) return head("AFTER THE RETURN", "아래로 이어지는 길") + `<p class="intro">동료들과 함께 돌아왔다. 던전은 더 깊이 이어지고, 식당은 오늘도 누군가의 귀환을 기다린다.</p><div class="notice">💡 엔딩 A안 시험 상태입니다. 실제 추가 층은 이 시제품에 구현하지 않았습니다.</div><div class="actions">${b("ending-replay", "후일담 다시 보기")}${b("station", "영업으로", false, "door", "primary")}</div>`;
  if (!s.previewEnabled) return head("THE STAIRS", "이번엔, 돌아갈 몫까지") + `<p class="intro">내 식사와 동료들의 도시락만으로는 충분하지 않다. 살아 있다는 답장, 서로 맞는 지도, 돌아올 경로와 장비가 필요하다.</p><div class="notice">🔴 자기 처방 상세 기준과 엔딩 정책은 미정이라 출발이 잠겨 있습니다. 영업과 단서 조사는 계속할 수 있습니다.</div><p class="small muted">추천 A안을 검토하려면 아래 시험 설정을 켜세요. 공식 기획을 확정하는 동작은 아닙니다.</p><div class="actions">${b("enable-preview", "💡 엔딩 A안 시험 설정 켜기", s.phase !== "prep", "", "primary")}</div>`;
  let h = head("ENDING A · EXPERIMENT", "나에게 내리는 처방") + `<div class="notice">💡 임시 시험값: 어둠 2 · 독 1 · 체력 2, 활력 여유 1, 보급비 12g. 자리 효과 제외. 동료 인원·실제 도시락·구조 장비 기준을 대신 확정하지 않습니다.</div><p class="small muted">필수 사실 ${s.story.clues.length}/6 · 위치 장면 ${s.story.done.includes("ISOLATED_AREA_LOCATED") ? "확인" : "미확인"}</p><div class="actions">${b("plan", s.story.plan ? "시험 경로·장비 계획 확인됨" : "시험 경로·장비 계획 확인", !s.story.clues.includes("outer") || !s.story.clues.includes("inner") || s.phase !== "prep")}</div>${divider}<h3>자기 식사 조합 선택</h3><div class="stack">${v.recipes.map((r) => b("self-recipe", `<strong>${r.n}</strong>${pills(r.eff)}`, !r.cookable, r.id, `recipe ${u.selfRecipe === r.id ? "active" : ""}`)).join("")}</div>`;
  if (u.selfRecipe) h += `${divider}<h3>양념 ${u.selfSpices.length}/${s.shelf}</h3><div class="spices">${Object.entries(D.SPICES).map(([id, p]) => b("self-spice", `${p.n}<small>${D.AX[p.ax].eff} +1</small>`, u.selfSpices.length >= s.shelf || (s.stock[id] || 0) <= u.selfSpices.filter((x) => x === id).length, id)).join("")}</div><p class="small muted">${u.selfSpices.map((id) => D.SPICES[id].n).join(" + ") || "양념 없음"}</p>${b("self-clear", "양념 비우기")}${pills(v.readiness.E)}<ul class="checklist">${v.readiness.reasons.map((t) => `<li>${t}</li>`).join("") || "<li>시험용 대비 기준을 만족합니다.</li>"}</ul><div class="actions">${b("prepare", "자기 식사·도시락 준비 확인", !v.readiness.ok, "", "primary")}</div>`;
  if (s.prepared) h += `<div class="notice">준비가 검증되었습니다. 출발 시 식사 재료와 보급비를 한 번 소모합니다.</div><div class="actions">${b("descend", "계단을 내려간다", false, "", "primary")}</div>`;
  return h;
}
function ending(s) {
  const depart = s.endingStep === "depart";
  return head("ENDING A · PROPOSAL", depart ? "오늘은, 돌아올 몫까지." : "다녀왔어.") + `<p class="intro">${depart ? "자신을 위한 식사와 동료들의 도시락을 챙긴다. 확인한 경로와 준비한 장비를 다시 살피고, 처음으로 자신의 발로 계단을 내려간다." : "💡 구조와 귀환은 짧은 후일담으로 전달하는 안입니다. 동료들과 함께 돌아온 뒤, 주인공은 식당의 불을 다시 켭니다. 그들이 회복하는 동안에도 누군가는 돌아올 한 끼를 기다립니다."}</p><div class="end-word">${depart ? "“이번에는<br>함께 돌아오자.”" : "돌아온 뒤에도,<br>식탁은 계속된다."}</div><p class="small muted">💡 실제 구조 절차·경과 기간·동료 외형은 미정. 직접 조작하는 구조·전투는 포함하지 않습니다.</p><div class="actions">${b("ending-next", depart ? "💡 귀환 후일담 확인" : "식당으로 돌아와 영업 계속", false, "", "primary")}</div>`;
}
function storyDialog(id) {
  const scene = SCENES[id];
  return `${head("A STORY AT THE TABLE", scene.title)}<p>${scene.text}</p><p class="small muted">💡 사건 대사 초안 · 확인하거나 생략해도 단서와 보상은 유지됩니다.</p><div class="actions">${b("scene-done", "생략하고 기록 남기기", false, id)}${b("scene-done", "이야기 확인", false, id, "primary")}</div>`;
}
function menuDialog(s) {
  return head("HELP & SAVE", "식당을 다루는 법") + `<ol class="checklist"><li>준비 화면에서 시장·레시피·수첩·가게를 살핀 뒤 영업을 시작하세요.</li><li>손님 목록으로 응대 대상을 바꾸고 자리를 안내하세요. 조리대의 전용 화면에서 재료를 넣으세요.</li><li>화덕에서 E로 접시를 집고, 손님 자리로 이동해 E로 내어 주세요.</li><li>모든 손님을 맞고 마감한 뒤 다음 아침 결과를 확인하세요.</li><li>5층의 단서가 생기면 수첩에서 질문·조사를 요청하세요.</li></ol><p class="small muted">각 동작 뒤 이 브라우저에 저장합니다. 일반 영업·5층 시험·단골 시험은 서로 다른 저장 슬롯입니다. 파일 실행과 localhost 실행은 저장 공간이 다를 수 있습니다.</p><div class="notice">💡 열린 자리 수만큼 3~6명을 동시에 받습니다. 재료를 넣어 조리하고 한 접시씩 운반합니다. 화구 1개·도구 배정·상태 대사는 시험값입니다. 마감과 영업 준비에 가게 개량 창을 열 수 있습니다. 🔴 입장 간격·인내심·최종 대사·물건 효과·인접 효과·6층 이상은 미정입니다.</div>${s ? `<p class="small muted">${s.mode === "story" ? "5층 조사 시험" : s.mode === "relations" ? "💡 단골 사연 시험" : "일반 영업"} · ${s.day}일차 · 확인한 단서 ${s.story.clues.length}개</p><div class="actions">${b("export", "현재 저장 내려받기")}${b("title", "시작 화면")}</div>` : ""}<div class="actions">${b("close-dialog", "돌아가기", false, "", "primary")}</div>`;
}
export {
  menuDialog,
  panel,
  storyDialog
};
