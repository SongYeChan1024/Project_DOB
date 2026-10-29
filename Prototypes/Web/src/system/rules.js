import { D, AXES, RACES, SEATS, CFG, NAMES, HINTS, PREVIEW } from "../define/data.js";
import { random, pick, clone } from "../utils/runtime.js";
import {ensureTaste,dishTags,tasteScore,mealPrice,learnTaste} from './taste.js';
import {TASTE} from '../define/taste.js';
import {REGULARS,REGULAR_BY_ID,RELATION} from '../define/relationships.js';
import {SERVICE,isActiveVisit,ORIGINAL_ECONOMY,FEEL_LINES} from '../define/service.js';
import {openingLine} from './greeting.js';

// 보고 ID의 정산 범위 안에서만 관계를 바꾼다. 과객을 이름으로 단골에 연결하지 않는다.
function migrateRelationships(s){
  for(const g of Object.values(s.guests)){g.regularId??=null;g.level??=1;g.xp??=0;}
  for(const r of REGULARS){
    const id='regular-'+r.id;
    s.guests[id]??={id,regularId:r.id,name:r.name,job:r.job,race:r.race,likes:[...r.likes],dislikes:[...r.dislikes],knownLikes:[r.likes[0]],knownDislikes:[],tired:false,floor:1,restUntil:0,visits:0,injured:false,affinity:0,stories:[],level:1,xp:0};
    const g=s.guests[id];g.affinity=Math.min(RELATION.max,Math.max(RELATION.min,g.affinity??0));g.stories??=[];
  }
}
function weighted(s,items){let n=random(s)*items.reduce((sum,[,w])=>sum+w,0);for(const [id,w]of items){n-=w;if(n<0)return id;}return items.at(-1)?.[0];}
function relationshipView(s){
  const regulars=REGULARS.map(r=>{const g=s.guests['regular-'+r.id];const next=r.stories.find(st=>!g.stories.includes(st.id));return{...g,intro:r.intro,nextAt:next?.at??null,remaining:next?Math.max(0,next.at-g.affinity):0,unlocked:r.stories.filter(st=>g.stories.includes(st.id))};});
  return{regulars,keepsakes:regulars.flatMap(g=>g.unlocked.filter(st=>st.gift).map(st=>({id:st.id,name:st.gift.name,kind:st.gift.kind,guestName:g.name}))),passers:Object.values(s.guests).filter(g=>!g.regularId)};
}
function settleRelationship(s,g,r){
  if(!g.regularId)return;
  const before=g.affinity,resultGain=RELATION.resultGain[r.result.tier]||0;
  const score=r.taste?.score;
  const tasteGain=score>=RELATION.tasteGoodAt?RELATION.tasteGoodGain:score===RELATION.tasteBadAt?RELATION.tasteBadGain:0;
  g.affinity=Math.max(RELATION.min,Math.min(RELATION.max,before+resultGain+tasteGain));
  const change=r.relationship={before,after:g.affinity,gain:g.affinity-before,resultGain,tasteGain,stories:[],levelBefore:g.level,level:g.level,map:null};
  if(['great','good'].includes(r.result.tier)&&g.level<RELATION.maxLevel){
    g.xp++;if(g.xp>=Math.ceil(g.level/2)){g.level++;g.xp=0;change.level=g.level;}
  }
  for(const st of REGULAR_BY_ID[g.regularId].stories){
    if(g.affinity<st.at||g.stories.includes(st.id))continue;
    g.stories.push(st.id);change.stories.push(st.id);
    if(st.gift?.kind==='map'){
      const floors=s.floor<CFG.authoredFloors?[s.floor+1]:Array.from({length:CFG.authoredFloors},(_,i)=>i+1);
      for(const floor of floors){const axis=AXES.find(a=>(D.FLOORS[floor].haz[a]||0)>0&&s.known[floor]?.[a]===undefined);if(!axis)continue;
        s.known[floor]??={};s.known[floor][axis]=D.FLOORS[floor].haz[axis];change.map={floor,axis,value:D.FLOORS[floor].haz[axis]};break;}
    }
  }
}
const vector = (x) => Object.fromEntries(AXES.map((a) => [a, Math.max(0, x?.[a] || 0)]));
function hazards(s, g) {
  const h = vector(D.FLOORS[g.floor].haz);
  if (s.anomaly?.floor === g.floor) h[s.anomaly.axis]++;
  if (g.tired) h.vital++;
  const weak = RACES[g.race].weak;
  if (weak) h[weak]++;
  const resist = D.CLASSES[g.job].res;
  h[resist] = Math.max(0, h[resist] - 1);
  return h;
}
function effects(s, recipeId, spices = [], seatId = null) {
  const r = D.RECIPES.find((r2) => r2.id === recipeId);
  const e = vector(r?.eff);
  if (!r) return e;
  const order = AXES.filter((a) => e[a] > 0).sort((a, b) => e[b] - e[a]);
  for (const a of order.slice(0, s.stove)) e[a]++;
  for (const id of spices) {
    if (D.SPICES[id]) e[D.SPICES[id].ax]++;
  }
  if (seatId !== null && s.seats.includes(seatId)) e[SEATS[seatId].axis]++;
  return e;
}
function outcome(h, e, surpriseAxis = null) {
  const H = vector(h), E = vector(e);
  const absorbed = !!surpriseAxis && E.vital - H.vital >= 1;
  if (surpriseAxis && !absorbed) H[surpriseAxis]++;
  const gaps = Object.fromEntries(AXES.map((a) => [a, Math.max(0, H[a] - E[a])]));
  const deficit = Object.values(gaps).reduce((a, b) => a + b, 0);
  return { H, E, gaps, deficit, tier: deficit === 0 ? "great" : deficit === 1 ? "good" : deficit === 2 ? "narrow" : "hurt", surprise: surpriseAxis, absorbed };
}
function createGuest(s, floor) {
  const id = "guest-" + ++s.nextGuest;
  const taken = [...Object.values(s.guests).map((g2) => g2.name),...REGULARS.map(r=>r.name)];
  const pool = NAMES.filter((n) => !taken.includes(n));
  const name = pick(s, pool.length ? pool : NAMES);
  const g = { id, name, regularId:null, level:1, xp:0, job: pick(s, Object.keys(D.CLASSES)), race: pick(s, Object.keys(RACES)), tired: s.day > 1 && random(s) < 0.3, floor, restUntil: 0, visits: 0, injured: false };
  s.guests[id] = g;
  ensureTaste(s,g);
  return g;
}
function newState(mode = "normal", seed = Date.now() >>> 0) {
  const s = { version: 1, mode, seed, day: 1, gold: CFG.startGold, phase: "prep", floor: mode === "story" ? 5 : 1, stove: 0, seats: [0, 1, 2], stock: { flour: 12, potato: 8, meat: 8, shroom: 8, herb: 6, carrot: 6, jelly: 4, garlic: 8, mint: 6, ginger: 6, carjuice: 8, salt: 8, blackpepper: 6 }, discovered: { flour: true, potato: true, meat: true, shroom: true, herb: true, carrot: true, jelly: true }, guests: {}, nextGuest: 0, nextVisit: 0, visits: [], index: 0, pending: [], reports: [], reportIndex: 0, committedIds: [], wins: {}, known: {}, income: 0, anomaly: null, prepared: null, previewEnabled: false, endingStep: null, endingTransaction: null, story: { clues: [], processed: [], first: {}, queue: [], done: [], injuries: {}, requests: {}, sceneDay: 0, afterReturn: false }, stats: { served: 0, returned: 0, hurt: 0 }, log: [] };
  s.shelf=ORIGINAL_ECONOMY.baseShelf;s.spent=0;s.economyVersion=4;
  if(mode==='normal'){s.gold=ORIGINAL_ECONOMY.gold;s.stock={...ORIGINAL_ECONOMY.stock};s.discovered={...ORIGINAL_ECONOMY.discovered};}
  if (mode === "story") {
    s.shelf=ORIGINAL_ECONOMY.maxShelf;
    s.gold = 240;
    s.stove = 1;
    s.seats = [0, 1, 2, 3, 4, 5];
    for (const k of Object.keys(D.ING)) {
      s.stock[k] = 8;
      s.discovered[k] = true;
    }
  }
  migrateRelationships(s);
  if(mode==='relations'){
    s.shelf=ORIGINAL_ECONOMY.maxShelf;
    s.gold=RELATION.trialGold;s.stove=RELATION.trialStove;s.seats=SEATS.map(t=>t.id);
    for(const id of [...Object.keys(D.ING),...Object.keys(D.SPICES)]){s.stock[id]=RELATION.trialStock;s.discovered[id]=true;}
    for(const [id,aff]of Object.entries(RELATION.trialAffinity)){const g=s.guests['regular-'+id];g.affinity=aff;g.stories=REGULAR_BY_ID[id].stories.filter(st=>st.at<=aff).map(st=>st.id);}
  }
  return s;
}
const currentVisit = (s) => s.visits[s.index];
const carriedVisit = s=>s.visits.find(v=>v.status==='carrying');
function selectVisit(s,id){const index=s.visits.findIndex(v=>v.id===id&&isActiveVisit(v));if(s.phase!=='service'||index<0)return false;s.index=index;return true;}
function seatOwner(s,id){return s.visits.find(v=>isActiveVisit(v)&&v.seat===id);}
function migrateService(s){
  s.shelf??=CFG.shelf;s.spent??=0;
  if(s.serviceVersion!==SERVICE.version){
    s.visits.forEach((v,i)=>{if(i<s.index&&v.status==='served')v.status='departed';});
    s.serviceVersion=SERVICE.version;
  }
  s.visits.forEach((v,i)=>{
    v.waitSlot??=i;v.ingredients??={};
    if(!v.opening){const line=openingLine(s.guests[v.guestId],hazards(s,s.guests[v.guestId]),v.id);v.opening=line.text;v.openingKey=line.key;}
  });
  if(s.phase==='service'&&!isActiveVisit(currentVisit(s)||{status:'departed'}))s.index=s.visits.findIndex(isActiveVisit);
}
const currentGuest = (s) => s.guests[currentVisit(s)?.guestId];
function startService(s) {
  if (s.phase !== "prep") return false;
  migrateRelationships(s);
  s.visits = [];
  s.index = 0;
  s.income = 0;
  s.serviceVersion=SERVICE.version;
  const capacity=Math.min(SERVICE.maxGuests,s.seats.length);
  const out=new Set(s.pending.map(r=>r.guestId));
  const available = Object.values(s.guests).filter((g) => g.restUntil <= s.day&&!out.has(g.id));
  const regulars=available.filter(g=>g.regularId).sort((a,b)=>a.visits-b.visits);
  const passers=available.filter(g=>!g.regularId).sort((a,b)=>a.visits-b.visits);
  const first=s.day===1?(s.mode==='relations'?RELATION.trialDay:RELATION.firstDay):null;
  const queue=first?first.map(id=>available.find(g=>g.regularId===id)).filter(Boolean):regulars.slice(0,RELATION.regularsPerDay);
  while(queue.length<capacity)queue.push(passers.shift()||createGuest(s,s.floor));
  for (let i = 0; i < capacity; i++) {
    const g=queue[i];
    const maxFloor=Math.min(s.floor,g.regularId?g.level+1:s.floor);
    g.floor=s.mode==='story'?s.floor:s.day===1?1:weighted(s,Array.from({length:maxFloor},(_,j)=>[j+1,j+1===maxFloor?3:1]));
    g.tired = s.day > 1 && random(s) < 0.3;
    g.visits++;
    const h = hazards(s, g);
    const axes=AXES.filter(a=>h[a]>0).sort((a,b)=>h[b]-h[a]).slice(0,g.regularId&&g.affinity>=RELATION.hintsAt?2:1);
    const hints=axes.map(a=>HINTS[a]);if(!hints.length)hints.push('별일 없을 거야. 가볍게 한 바퀴 돌고 올게.');
    const promise=g.regularId&&g.affinity>=RELATION.promiseAt&&random(s)<RELATION.promiseChance?weighted(s,D.FLOORS[g.floor].loot.filter(([id])=>!D.ING[id].rare)):null;
    const v = { id: "visit-" + ++s.nextVisit, guestId: g.id, seat: null, recipe: null, spices: [], status: "waiting", hints,hint:hints.join(' / '),promise:promise||null, assignment: null };
    v.waitSlot=i;v.ingredients={};const line=openingLine(g,h,v.id);v.opening=line.text;v.openingKey=line.key;g.lastOpening=line.text;
    if (g.floor === 5 && !s.story.afterReturn) {
      const req = Object.values(s.story.requests).find((r) => !r.done && !s.visits.some((v2) => v2.assignment === r.id));
      if (req) v.assignment = req.id;
    }
    s.visits.push(v);
  }
  s.phase = "service";
  return true;
}
function chooseSeat(s, id) {
  const v = currentVisit(s);
  if (s.phase !== "service" || v?.status !== "waiting" || !s.seats.includes(id) || (seatOwner(s,id)&&seatOwner(s,id)!==v)) return false;
  v.seat = id;
  return true;
}
function chooseMeal(s, id) {
  const v = currentVisit(s);
  if (s.phase !== "service" || v?.status !== "waiting" || v.seat === null || !D.RECIPES.some((r) => r.id === id)) return false;
  v.recipe = id;
  v.spices = [];
  v.ingredients={};
  return true;
}
function chooseSpice(s, id) {
  const v = currentVisit(s);
  if (!v?.recipe || v.status !== "waiting" || !D.SPICES[id]) return false;
  if (v.spices.length >= s.shelf) return false;
  if ((s.stock[id] || 0) <= v.spices.filter((k) => k === id).length) return false;
  v.spices.push(id);
  return true;
}
function canCook(s, r, spices = []) {
  if (!r || spices.some((k) => !D.SPICES[k]) || spices.length > s.shelf) return false;
  const need = { ...r.ing };
  for (const id of spices) need[id] = (need[id] || 0) + 1;
  return Object.entries(need).every(([id, n]) => (s.stock[id] || 0) >= n);
}
function addIngredient(s,id){
  const v=currentVisit(s),r=D.RECIPES.find(r=>r.id===v?.recipe);
  if(s.phase!=='service'||v?.status!=='waiting'||!r?.ing[id])return false;
  v.ingredients??={};const n=v.ingredients[id]||0;
  if(n>=r.ing[id]||n>=(s.stock[id]||0))return false;
  v.ingredients[id]=n+1;return true;
}
function removeIngredient(s,id){const v=currentVisit(s);if(s.phase!=='service'||v?.status!=='waiting'||!(v.ingredients?.[id]>0))return false;v.ingredients[id]--;return true;}
function ingredientsReady(v){const r=D.RECIPES.find(r=>r.id===v?.recipe);return !!r&&Object.entries(r.ing).every(([id,n])=>v.ingredients?.[id]===n);}
function burnerAvailable(s){return s.visits.filter(v=>v.status==='cooking').length<SERVICE.burners;}
function cook(s, now) {
  const v = currentVisit(s);
  const r = D.RECIPES.find((r2) => r2.id === v?.recipe);
  if (s.phase !== "service" || v?.status !== "waiting" || v.seat === null || !ingredientsReady(v)||!burnerAvailable(s)||!canCook(s, r, v.spices)) return false;
  for (const [id, n] of Object.entries(r.ing)) s.stock[id] -= n;
  for (const id of v.spices) s.stock[id]--;
  v.status = "cooking";
  v.readyAt = now + CFG.cookMs;
  v.E = effects(s, r.id, v.spices, v.seat);
  v.H = hazards(s, currentGuest(s));
  return true;
}
function finishCook(s, now) {
  let changed=false;if(s.phase!=='service')return false;
  for(const v of s.visits)if(v.status==='cooking'&&now>=v.readyAt){v.status='ready';changed=true;}
  return changed;
}
function pickup(s){
  const v=currentVisit(s);if(s.phase!=='service'||v?.status!=='ready'||carriedVisit(s))return false;
  v.status='carrying';return true;
}
function serve(s) {
  const v = currentVisit(s), g = currentGuest(s);
  if (s.phase !== "service" || v?.status !== "carrying") return false;
  const r = D.RECIPES.find((r2) => r2.id === v.recipe);
  const surprise = random(s) < CFG.surpriseChance ? pick(s, AXES.filter((a) => a !== "vital")) : null;
  const result = outcome(v.H, v.E, surprise);
  ensureTaste(s,g);
  const taste=tasteScore(g,dishTags(r,v.spices)),price=mealPrice(r,v.spices,taste);
  const learned=learnTaste(g,taste);
  const lootBonus=result.tier!=='hurt'&&taste.score>=3&&random(s)<TASTE.lootChance?1:0;
  s.gold += price;
  s.income += price;
  s.stats.served++;
  s.pending.push({ id: "report-" + v.id, visitId: v.id, guestId: g.id, name: g.name, floor: g.floor, recipe: r.n, assignment: v.assignment, result, taste, lootBonus, promise:v.promise||null,maxAffinityBonus:g.regularId&&g.affinity>=RELATION.max?RELATION.maxBonus:0, day: s.day });
  v.status = "served";
  v.price = price;
  v.taste=taste;v.learnedTaste=learned;
  const noise=random(s);v.feel=FEEL_LINES[Math.max(0,Math.min(3,result.deficit+(noise<.15?-1:noise>.85?1:0)))];
  return true;
}
function advanceGuest(s, decline = false) {
  const v = currentVisit(s);
  if (s.phase !== "service" || !v) return false;
  if (decline) {
    if (v.status !== "waiting") return false;
    v.status = "declined";
    const g=currentGuest(s);if(g.regularId){const before=g.affinity;g.affinity=Math.max(RELATION.min,g.affinity+RELATION.declineGain);v.declineAffinity=g.affinity-before;}
  } else {if (v.status !== "served") return false;v.status='departed';}
  const next=s.visits.findIndex(isActiveVisit);
  if(next<0)s.phase='close';else s.index=next;
  return true;
}
function earlyClose(s){
  if(s.phase!=='service'||s.visits.some(v=>['cooking','ready','carrying'].includes(v.status)))return false;
  for(const v of s.visits)if(isActiveVisit(v))v.status=v.status==='served'?'departed':'declined';
  s.phase='close';return true;
}
function nextMorning(s) {
  if (s.phase !== "close") return [];
  s.day++;
  s.spent=0;
  s.reports = [];
  s.reportIndex = 0;
  const events = [];
  for (const r of s.pending) {
    if (s.committedIds.includes(r.id)) continue;
    s.committedIds.push(r.id);
    s.reports.push(r);
    s.stats.returned++;
    const g = s.guests[r.guestId];
    r.loot={};
    r.newIngredients=[];
    const grant=(id,n)=>{if(!s.discovered[id])r.newIngredients.push(id);s.stock[id]=(s.stock[id]||0)+n;s.discovered[id]=true;r.loot[id]=(r.loot[id]||0)+n;};
    if (r.result.tier === "hurt") {
      s.stats.hurt++;
      g.restUntil = s.day + CFG.injuryRest;
    } else {
      const loot = D.FLOORS[r.floor].loot;
      for (let j = 0; j < ({great:3,good:2,narrow:1}[r.result.tier]+(r.lootBonus||0)+(r.maxAffinityBonus||0)); j++) {
        const id = weighted(s,loot);
        grant(id,1);
      }
    }
    if (["great", "good"].includes(r.result.tier)) {
      if(r.promise&&D.ING[r.promise]){grant(r.promise,RELATION.promiseAmount);r.promiseDelivered=RELATION.promiseAmount;}
      s.wins[r.floor] = (s.wins[r.floor] || 0) + 1;
      if (r.floor === s.floor && s.wins[r.floor] >= CFG.unlockWins && s.floor < CFG.authoredFloors){s.floor++;r.newFloor=s.floor;}
    }
    const knownRecipes=new Set(D.RECIPES.filter(recipe=>Object.keys(recipe.ing).every(id=>s.discovered[id]&&!(r.newIngredients||[]).includes(id))).map(recipe=>recipe.id));
    s.known[r.floor]??={};
    r.learned=AXES.filter(a=>(D.FLOORS[r.floor].haz[a]||0)>0&&s.known[r.floor][a]===undefined).sort((a,b)=>(r.result.gaps[b]>0?1:0)-(r.result.gaps[a]>0?1:0)).slice(0,2);
    for(const axis of r.learned)s.known[r.floor][axis]=D.FLOORS[r.floor].haz[axis];
    r.newRecipes=D.RECIPES.filter(recipe=>!knownRecipes.has(recipe.id)&&Object.keys(recipe.ing).every(id=>s.discovered[id])).map(recipe=>recipe.id);
    settleRelationship(s,g,r);
    events.push({ type: "ReportCommitted", report: r });
  }
  s.pending = [];
  s.phase = s.reports.length ? "report" : "prep";
  s.anomaly = random(s) < 0.6 ? { floor: 1 + Math.floor(random(s) * s.floor), axis: pick(s, AXES) } : null;
  if (s.gold < 15 && !D.RECIPES.some((r) => canCook(s, r))) {
    s.stock.flour = (s.stock.flour || 0) + 4;
    s.log.push("이웃 상인이 밀가루를 보태 주었다.");
  }
  return events;
}
function buy(s, id, qty=1) {
  const item = D.ING[id] || D.SPICES[id];
  if (!["prep","close"].includes(s.phase) || !item || ![1,5].includes(qty)||D.ING[id]?.rare||(D.ING[id]?.floor||0)>s.floor) return false;
  const cost = (item.price || 8) * (D.ING[id]?.floor ? 4 : 1)*qty;
  if (s.gold < cost) return false;
  s.gold -= cost;
  s.spent=(s.spent||0)+cost;
  s.stock[id] = (s.stock[id] || 0) + qty;
  s.discovered[id] = true;
  invalidate(s);
  return true;
}
function upgrade(s, type, id) {
  if (!["prep","close"].includes(s.phase)) return false;
  const cost = type === 'stove'?ORIGINAL_ECONOMY.stoveCosts[s.stove]:type==='shelf'?s.shelf<ORIGINAL_ECONOMY.maxShelf&&ORIGINAL_ECONOMY.shelfCost:type==='seat'?ORIGINAL_ECONOMY.seatCosts[s.seats.length-3]:null;
  if (!cost || s.gold < cost || type === "seat" && (!SEATS[id]||s.seats.includes(id))) return false;
  s.gold -= cost;
  s.spent=(s.spent||0)+cost;
  if (type === "stove") s.stove++;
  else if(type==='shelf')s.shelf++;
  else s.seats.push(id);
  invalidate(s);
  return true;
}
function invalidate(s) {
  s.prepared = null;
}
function previewReadiness(s, recipeId, spices) {
  const E = effects(s, recipeId, spices, null);
  const res = outcome(PREVIEW.selfHazard, E);
  const clues = ["mark", "gap", "reply", "inner", "outer", "cooking"];
  const r = D.RECIPES.find((r2) => r2.id === recipeId);
  const reasons = [];
  if (!s.previewEnabled) reasons.push("미정인 자기 처방·엔딩 A안 시험 설정을 먼저 켜세요.");
  if (!clues.every((k) => s.story.clues.includes(k))) reasons.push("생존 답장과 안팎의 지도, 식사 기록이 필요합니다.");
  if (!s.story.done.includes("ISOLATED_AREA_LOCATED")) reasons.push("위치 확인 장면을 먼저 확인하세요.");
  if (!s.story.plan) reasons.push("시험용 경로·장비 계획 확인이 필요합니다.");
  if (res.deficit || E.vital - PREVIEW.selfHazard.vital < PREVIEW.reserve) reasons.push("시험 경로의 위험을 모두 막고 활력 여유 1을 남겨야 합니다.");
  if (!canCook(s, r, spices) || s.gold < PREVIEW.supplyCost) reasons.push("식사 재료와 시험 보급비 12g가 필요합니다.");
  if (s.phase !== "prep" || s.pending.length) reasons.push("모든 귀환 보고를 마친 영업 준비 시간에 가능합니다.");
  return { ok: !reasons.length, reasons, E };
}
function prepareSelf(s, recipeId, spices) {
  const check = previewReadiness(s, recipeId, spices);
  if (!check.ok) return check;
  s.prepared = { id: "prep-" + s.day, recipeId, spices: [...spices], config: PREVIEW.version };
  return check;
}
function descend(s) {
  const p = s.prepared;
  if (s.story.afterReturn || s.endingTransaction || !p || p.config !== PREVIEW.version) return false;
  const check = previewReadiness(s, p.recipeId, p.spices);
  if (!check.ok) {
    invalidate(s);
    return false;
  }
  const r = D.RECIPES.find((r2) => r2.id === p.recipeId);
  for (const [id, n] of Object.entries(r.ing)) s.stock[id] -= n;
  for (const id of p.spices) s.stock[id]--;
  s.gold -= PREVIEW.supplyCost;
  s.endingTransaction = p.id;
  s.endingStep = "depart";
  s.phase = "ending";
  return true;
}
export {
  earlyClose,
  selectVisit,seatOwner,carriedVisit,migrateService,addIngredient,removeIngredient,ingredientsReady,burnerAvailable,
  migrateRelationships,
  relationshipView,
  pickup,
  advanceGuest,
  buy,
  canCook,
  chooseMeal,
  chooseSeat,
  chooseSpice,
  cook,
  createGuest,
  currentGuest,
  currentVisit,
  descend,
  effects,
  finishCook,
  hazards,
  invalidate,
  newState,
  nextMorning,
  outcome,
  prepareSelf,
  previewReadiness,
  serve,
  startService,
  upgrade,
  vector
};
