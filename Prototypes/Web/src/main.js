import { D, CFG, SCENES } from "./define/data.js";
import { Bus, load, save } from "./utils/runtime.js";
import * as Rules from "./system/rules.js";
import * as Story from "./system/story.js";
import { RestaurantScene } from "./ui/scene.js";
import { panel, storyDialog, menuDialog } from "./ui/panels.js";
import * as Move from './system/movement.js';
import {ensureTaste,dishTags} from './system/taste.js';
import {PlayerInput} from './ui/input.js';
import {REGULAR_STORIES} from './define/relationships.js';
import {regularStoryDialog} from './ui/relationshipPanels.js';
import {cookingPanel} from './ui/cookingPanel.js';
import {isActiveVisit} from './define/service.js';
import {managementDialog,serviceRoster} from './ui/panels.js';
const bus = new Bus();
const dom = { panel: document.querySelector("#panel"), dialog: document.querySelector("#dialog"), cooking:document.querySelector('#cooking-dialog'), status: document.querySelector("#status"), save: document.querySelector("#save-state"), toast: document.querySelector("#toast") };
let state = null, scene = null, toastTimer;
let destination=null,previousFrame=0,wasMoving=false,lastPositionSave=0;
let ui = { station: "door", prepTab: "home", bookTab: "story", selfRecipe: null, selfSpices: [] };
try {
  scene = new RestaurantScene(document.querySelector("#canvas-wrap"), (id) => {
    bus.emit('Action',{act:'interact-target',id});
  });
} catch (error) {
  document.querySelector("#scene-fallback").hidden = false;
  console.error("3D renderer unavailable", error);
}
const input=new PlayerInput(()=>interact(),()=>!!state&&!!scene&&state.phase==='service'&&!dom.dialog.open&&!dom.cooking.open&&!document.hidden);
const guestTarget=()=>state?.phase==='service'?'guest:'+Rules.currentVisit(state)?.id:'guest';
const targetId=id=>id==='guest'?guestTarget():id;
function openCooking(){
  const c=state&&Rules.currentVisit(state);
  if(state?.phase!=='service'||!c||c.seat===null){toast('먼저 손님을 선택하고 자리를 안내해 주세요.');return;}
  if(!requireNear('kitchen'))return;
  if(['served','carrying'].includes(c.status)){toast('이 손님의 음식은 이미 완성됐습니다. 전달과 배웅을 마쳐 주세요.');return;}
  input.clear();dom.cooking.innerHTML=cookingPanel(viewModel());dom.cooking.showModal();
}
function guide(id){
  id=targetId(id);
  const target=Move.targets(state).find(t=>t.id===id);
  if(!target){toast('지금은 이곳에 응대할 손님이 없습니다.');return false;}
  destination=id;scene?.setDestination(target);
  toast(`${target.name} 가까이로 이동해 E를 누르세요.`);return false;
}
function requireNear(id){id=targetId(id);return !scene||Move.canReach(state,id)||guide(id);}
function interact(id){
  if(!state||dom.dialog.open||dom.cooking.open||state.phase==='ending')return;
  const target=id?Move.targets(state).find(t=>t.id===id):Move.nearestTarget(state);
  if(!target){toast('대상 가까이로 이동하세요. 장소 버튼으로 위치를 표시할 수 있습니다.');return;}
  if(!requireNear(target.id))return;
  destination=null;scene?.setDestination(null);
  if(target.id.startsWith('guest:')){Rules.selectVisit(state,target.id.slice(6));persist();}
  const visit=Rules.currentVisit(state);
  if(target.id.startsWith('seat:')){bus.emit('Action',{act:'seat',id:target.id.split(':')[1]});return;}
  if(target.id==='door'&&['prep','close'].includes(state.phase)){bus.emit('Action',{act:state.phase==='prep'?'open':'morning'});return;}
  if(target.id==='stove'&&visit?.status==='ready'){bus.emit('Action',{act:'pickup'});return;}
  if(target.id.startsWith('guest:')&&visit?.status==='carrying'){bus.emit('Action',{act:'serve'});return;}
  if(target.id.startsWith('guest:')&&visit?.status==='served'){bus.emit('Action',{act:'next'});return;}
  if(target.id==='kitchen'){ui.station='kitchen';render(true);openCooking();return;}
  if(target.id.startsWith('guest:')){ui.station='guest';render(true);return;}
  ui.station=target.id;render(true);
}
function refreshMovementUI(moving=false){
  if(!state){document.querySelector('#movement-hud').hidden=true;return;}
  const hud=document.querySelector('#movement-hud');hud.hidden=false;
  const nearest=scene?Move.nearestTarget(state):null,visit=Rules.currentVisit(state);
  const target=destination&&Move.targets(state).find(t=>t.id===destination);
  const held=Rules.carriedVisit(state);
  document.querySelector('#interact-label').textContent=nearest?`${nearest.name} · ${nearest.id==='stove'&&visit?.status==='ready'?'접시 집기':nearest.id==='guest:'+held?.id?'음식 내기':'상호작용'}`:'가까운 대상 없음';
  document.querySelector('#interact-button').disabled=!nearest||dom.dialog.open||dom.cooking.open||state.phase==='ending';
  const label=document.querySelector('#player-state');label.textContent=held?`${state.guests[held.guestId].name}의 접시 운반 중`:moving?'이동 중':'WASD / 방향키 이동';
  label.dataset.x=state.player.x.toFixed(3);label.dataset.z=state.player.z.toFixed(3);
  document.querySelector('#destination-label').textContent=target?`${target.name}까지 ${Move.targetDistance(state,target.id).toFixed(1)}m · 금색 원으로 이동`:'E 상호작용 · 드래그 시점 회전';
}
function frame(time){
  const dt=(time-previousFrame)/1000;previousFrame=time;
  if(state){
    const axes=input.axes(),before=state.player;
    state.player=Move.movePlayer(before,axes.h,axes.v,dt,scene?.angle||.72);
    const moving=Math.hypot(state.player.x-before.x,state.player.z-before.z)>.0001;
    if((wasMoving&&!moving)||(moving&&time-lastPositionSave>1200)){persist();lastPositionSave=time;}wasMoving=moving;
    scene?.setPlayer(state.player,moving,!!Rules.carriedVisit(state));refreshMovementUI(moving);
  }
  requestAnimationFrame(frame);
}
function persist() {
  if (state) dom.save.textContent = save(state) ? "이 브라우저에 자동 저장됨" : "저장 공간을 사용할 수 없습니다 · 메뉴에서 저장 내려받기";
}
function toast(message) {
  dom.toast.textContent = message;
  dom.toast.hidden = false;
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => dom.toast.hidden = true, 3200);
}
function showDialog(html) {
  input.clear();
  dom.dialog.classList.toggle('management-dialog',!!ui.management);
  dom.dialog.innerHTML = html;
  if (!dom.dialog.open) dom.dialog.showModal();
}
function viewModel() {
  if (!state) return { s: null, normalSave: !!load("normal"), storySave: !!load("story"),relationsSave:!!load('relations') };
  const s = state, visit = Rules.currentVisit(s), relationships=Rules.relationshipView(s);
  const rawGuest=Rules.currentGuest(s),guest=relationships.regulars.find(g=>g.id===rawGuest?.id)||rawGuest;
  const recipes = D.RECIPES.filter((r) => Object.keys(r.ing).every((k) => s.discovered[k])).map((r) => ({ ...r,effects:Rules.effects(s,r.id), cookable: Rules.canCook(s, r) }));
  const market = Object.entries({ ...D.ING, ...D.SPICES }).filter(([id, item]) => !D.ING[id]?.rare&&(!D.ING[id]?.floor || D.ING[id].floor <= s.floor)).map(([id, item]) => ({ id, name: item.n, have: s.stock[id] || 0, cost: (item.price || 8) * (D.ING[id]?.floor ? 4 : 1) }));
  return { s, visit, guest, relationships, recipes, market, held:Rules.carriedVisit(s), burnerAvailable:Rules.burnerAvailable(s),ingredientsReady:Rules.ingredientsReady(visit), mealTags:dishTags(D.RECIPES.find(r=>r.id===visit?.recipe),visit?.spices||[]), E: Rules.effects(s, visit?.recipe, visit?.spices, visit?.seat), canCook: Rules.canCook(s, D.RECIPES.find((r) => r.id === visit?.recipe), visit?.spices || []), report: s.reports[s.reportIndex], readiness: Rules.previewReadiness(s, ui.selfRecipe, ui.selfSpices) };
}
function render(resetScroll = false) {
  const scroll = dom.panel.scrollTop, v = viewModel(), s = state;
  document.body.dataset.phase=s?.phase||'title';
  dom.panel.innerHTML = (s?.phase==='service'?serviceRoster(v):'')+panel(v, ui);
  dom.panel.scrollTop = resetScroll ? 0 : scroll;
  if(dom.cooking.open){const positions=[...dom.cooking.querySelectorAll('.cook-recipes,.cook-pantry')].map(e=>e.scrollTop);const top=dom.cooking.scrollTop;dom.cooking.innerHTML=cookingPanel(v);dom.cooking.scrollTop=top;dom.cooking.querySelectorAll('.cook-recipes,.cook-pantry').forEach((e,i)=>e.scrollTop=positions[i]||0);}
  if(dom.dialog.open&&ui.management)dom.dialog.innerHTML=managementDialog(v,ui);
  document.querySelector('#management-button').hidden=!s||!['prep','close'].includes(s.phase);
  document.querySelector('#notebook-button').hidden=!s;
  const phases = { prep: "영업 준비", service: "손님 맞이", close: "영업 마감", report: "귀환 보고", ending: "귀환 이야기" };
  dom.status.innerHTML = s ? `<span class="stat"><span>${s.mode === "story" ? "5층 시험" : s.mode === "relations" ? "💡 단골 시험" : "일반 영업"} · ${phases[s.phase]}</span><strong>${s.day}일차</strong></span><span class="stat"><span>금고</span><strong>${s.gold} g</strong></span>` : '<span class="stat"><span>3D WEB PROTOTYPE</span><strong>돌아올 한 끼</strong></span>';
  document.querySelector("#world-tag").textContent = s ? s.story.afterReturn ? "💡 엔딩 A안 이후 영업" : s.mode === "story" ? "💡 5층 조사 시험" : "💡 3D 영업 시제품" : "A SMALL RESTAURANT, A LONG WAY HOME";
  document.querySelector("#steps").textContent = s?.phase === "service" ? `식당 안 ${s.visits.filter(isActiveVisit).length}명 · 응대 중 ${v.guest?.name||''} · 손님 목록으로 선택` : s?.phase === "report" ? "어제의 처방을 돌아보는 아침" : s?.phase === "prep" ? "가게 개량 · 재료 준비 → 문 열기" : "손님과 물건을 눌러 살펴보세요";
  document.querySelectorAll("#stations button").forEach((b) => {
    b.classList.toggle("active", b.dataset.id === ui.station);
  });
  const guests=s?.phase==='service'?s.visits.filter(isActiveVisit).map(c=>({key:'guest:'+c.id,data:s.guests[c.guestId],slot:c.waitSlot,seat:c.seat,status:c.status,selected:c.id===v.visit?.id})):s?.phase==='report'&&v.report?[{key:'guest',data:s.guests[v.report.guestId],seat:null,report:true}]:[];
  scene?.update({seats:s?.seats||[0,1,2],seat:s?.phase==='service'?v.visit?.seat:null,keepsakes:v.relationships?.keepsakes||[],guests});
  updateCookProgress();
  refreshMovementUI();
}
function updateCookProgress() {
  document.querySelectorAll('[data-cook-progress]').forEach(p=>{const c=state?.visits.find(v=>v.id===p.dataset.cookProgress);if(c?.status==='cooking')p.style.width=Math.min(100,Math.max(0,100*(1-(c.readyAt-Date.now())/CFG.cookMs)))+'%';});
}
function morningScene() {
  if (state?.phase !== "prep" || dom.dialog.open) return;
  const next = Story.nextScene(state);
  if (next) showDialog(storyDialog(next.id));
}
function begin(mode, existing = false) {
  state = existing ? load(mode) : Rules.newState(mode);
  if (!state) {
    toast("불러올 수 있는 저장이 없습니다.");
    return;
  }
  Rules.migrateRelationships(state);
  Rules.migrateService(state);
  Move.restorePlayer(state);for(const g of Object.values(state.guests))ensureTaste(state,g);
  input.clear();destination=null;scene?.setDestination(null);
  ui = { station: "door", prepTab: "home", bookTab: "story", selfRecipe: state.prepared?.recipeId || null, selfSpices: state.prepared?.spices || [] };
  Rules.finishCook(state, Date.now());
  if(Rules.carriedVisit(state)){destination='guest:'+Rules.carriedVisit(state).id;scene?.setDestination(Move.targets(state).find(t=>t.id===destination));}
  for (const report of state.reports) Story.processReport(state, report);
  dom.dialog.close();
  persist();
  render(true);
  morningScene();
}
bus.on("ReportCommitted", (report) => Story.processReport(state, report));
bus.on("Action", ({ act, id }) => {
  if (act === "menu") {
    ui.management=false;
    showDialog(menuDialog(state));
    return;
  }
  if (act === "close-dialog") {
    ui.management=false;
    dom.dialog.close();
    return;
  }
  if (act === "camera") {
    scene?.reset();
    return;
  }
  if (act === "start") {
    if (load(id)) {
      showDialog(`<span class="eyebrow">NEW SAVE</span><h2>새 영업을 시작할까요?</h2><p>이 모드의 이전 저장을 새 영업으로 바꿉니다. 다른 모드의 저장은 유지됩니다.</p><div class="actions"><button data-act="close-dialog">돌아가기</button><button class="primary" data-act="confirm-new" data-id="${id}">새로 시작</button></div>`);
      return;
    }
    begin(id);
    return;
  }
  if (act === "confirm-new") {
    begin(id);
    return;
  }
  if (act === "continue") {
    begin(id, true);
    return;
  }
  if (!state) {
    toast("먼저 영업을 시작해 주세요.");
    return;
  }
  if(act==='interact'){interact();return;}
  if(act==='interact-target'){interact(id);return;}
  if(act==='close-cooking'){dom.cooking.close();ui.station=Rules.currentVisit(state)?.status==='waiting'?'kitchen':'stove';render();return;}
  if(act==='open-cooking'){openCooking();return;}
  if(act==='management'){if(['prep','close'].includes(state.phase)){ui.management=true;ui.prepTab='upgrade';showDialog(managementDialog(viewModel(),ui));}return;}
  if(act==='open-notebook'){ui.station=state.phase==='prep'?'door':'book';if(state.phase==='prep')ui.prepTab='notes';render(true);return;}
  if(act==='return-service'){ui.station=state.phase==='prep'?'door':'guest';render(true);return;}
  if(act==='select-visit'){if(Rules.selectVisit(state,id)){ui.station='guest';persist();render(true);}return;}
  if(act==='select-order'){if(Rules.selectVisit(state,id)){ui.station='stove';persist();render();}return;}
  if(act==='ask-close'){
    if(state.visits.some(v=>['cooking','ready','carrying'].includes(v.status))){toast('이미 조리한 음식은 먼저 손님에게 전달해 주세요.');return;}
    ui.management=false;showDialog('<h2>오늘 영업을 마칠까요?</h2><p>아직 요리를 시작하지 않은 손님은 돌려보내고, 식사를 마친 손님은 배웅합니다. 준비 중인 재료는 소모되지 않습니다.</p><div class="actions"><button data-act="close-dialog">계속 영업하기</button><button class="primary" data-act="early-close">마감하기</button></div>');return;
  }
  const required={open:'door',morning:'door',seat:'seat:'+id,recipe:'kitchen',ingredient:'kitchen','remove-ingredient':'kitchen',spice:'kitchen','clear-spice':'kitchen',cook:'kitchen',pickup:'stove',serve:'guest',next:'guest',decline:'guest',request:'book','enable-preview':'stairs',plan:'stairs','self-recipe':'stairs','self-spice':'stairs','self-clear':'stairs',prepare:'stairs',descend:'stairs'}[act];
  if(required&&!['open','morning'].includes(act)&&!requireNear(required))return;
  if(act==='serve'&&scene?.isGuestMoving(guestTarget())){toast('손님이 자리에 도착하면 음식을 내어 주세요.');return;}
  let changed = false, resetScroll = false;
  const s = state, c = Rules.currentVisit(s);
  switch (act) {
    case 'early-close':changed=Rules.earlyClose(s);if(changed){dom.dialog.close();ui.station='door';}break;
    case 'regular-story': {
      ui.management=false;
      const story=REGULAR_STORIES[id];
      if(story&&s.guests['regular-'+story.regularId]?.stories.includes(id))showDialog(regularStoryDialog(story));
      return;
    }
    case "station":
      if(scene&&!Move.canReach(s,targetId(id))){guide(id);return;}
      ui.station = id;
      if(id==='kitchen'&&s.phase==='service'){render(true);openCooking();return;}
      resetScroll = true;
      break;
    case "prep-tab":
      ui.prepTab = id;
      if(s.phase==='prep')ui.station='door';
      resetScroll = true;
      break;
    case "book-tab":
      ui.bookTab = id;
      resetScroll = true;
      break;
    case "open":
      changed = Rules.startService(s);
      if (changed) {
        ui.station = "guest";
        resetScroll = true;
      }
      break;
    case "seat":
      changed = Rules.chooseSeat(s, Number(id));
      if (changed) {
        ui.station = "guest";
        toast("자리를 정했습니다. 이제 목적지의 이야기를 들어 보세요.");
      }
      break;
    case "recipe":
      changed = Rules.chooseMeal(s, id);
      break;
    case 'ingredient':changed=Rules.addIngredient(s,id);break;
    case 'remove-ingredient':changed=Rules.removeIngredient(s,id);break;
    case "spice":
      changed = Rules.chooseSpice(s, id);
      break;
    case "clear-spice":
      if (c?.status === "waiting") {
        c.spices = [];
        changed = true;
      }
      break;
    case "cook":
      changed = Rules.cook(s, Date.now());
      if (changed) {
        ui.station = "stove";
        resetScroll = true;
      }
      break;
    case "serve":
      changed = Rules.serve(s);
      if (changed) {
        destination=null;scene?.setDestination(null);
        ui.station = "guest";
        resetScroll = true;
      }
      break;
    case 'pickup':
      changed=Rules.pickup(s);if(changed){ui.station='guest';destination=guestTarget();scene?.setDestination(Move.targets(s).find(t=>t.id===destination));toast(`${Rules.currentGuest(s).name}의 접시를 들었습니다. 해당 손님에게 다가가 E로 내어 주세요.`);resetScroll=true;}else if(Rules.carriedVisit(s))toast('먼저 들고 있는 접시를 전달해 주세요.');break;
    case "next":
      changed = Rules.advanceGuest(s);
      resetScroll = true;
      break;
    case "decline":
      {const g=Rules.currentGuest(s);const before=g?.affinity;
      changed = Rules.advanceGuest(s, true);
      if(changed&&g?.regularId)toast(`${g.name}을 돌려보냈습니다. 💡 호감 ${before} → ${g.affinity}`);}
      resetScroll = true;
      break;
    case "morning": {
      if (s.phase !== "close") break;
      const events = Rules.nextMorning(s);
      persist();
      for (const event of events) bus.emit(event.type, event.report);
      changed = true;
      ui.station = "guest";
      resetScroll = true;
      break;
    }
    case "report-next":
      if (s.phase === "report") {
        s.reportIndex++;
        if (s.reportIndex >= s.reports.length) {
          s.phase = "prep";
          ui.station = "door";
          ui.prepTab='market';
        }
        changed = true;
        resetScroll = true;
      }
      break;
    case 'report-all':
      if(s.phase==='report'){s.reportIndex=s.reports.length;s.phase='prep';ui.station='door';ui.prepTab='market';changed=true;resetScroll=true;}break;
    case "buy":
      changed = Rules.buy(s, id);
      break;
    case 'buy-five':changed=Rules.buy(s,id,5);break;
    case 'upgrade-shelf':changed=Rules.upgrade(s,'shelf');break;
    case "upgrade-stove":
      changed = Rules.upgrade(s, "stove");
      break;
    case "upgrade-seat":
      changed = Rules.upgrade(s, "seat", Number(id));
      break;
    case "request":
      changed = Story.requestInvestigation(s, id);
      if (changed) toast("다음 5층 손님에게 요청을 맡깁니다.");
      break;
    case "scene-done":
      changed = Story.finishScene(s, id);
      if (changed) dom.dialog.close();
      break;
    case "replay": {
      const sceneData = SCENES[id];
      if (!s.story.done.includes(id) || !sceneData) return;
      showDialog(`<span class="eyebrow">STORY RECORD</span><h2>${sceneData.title}</h2><p>${sceneData.text}</p><p class="small muted">기록 다시 보기 · 진행과 보상은 바뀌지 않습니다.</p><div class="actions"><button data-act="close-dialog" class="primary">기록 닫기</button></div>`);
      return;
    }
    case "enable-preview":
      if (s.phase === "prep") {
        s.previewEnabled = true;
        changed = true;
      }
      break;
    case "plan":
      if (s.phase === "prep" && s.story.clues.includes("inner") && s.story.clues.includes("outer")) {
        s.story.plan = true;
        Rules.invalidate(s);
        changed = true;
      }
      break;
    case "self-recipe":
      ui.selfRecipe = id;
      ui.selfSpices = [];
      Rules.invalidate(s);
      changed = true;
      break;
    case "self-spice":
      if (ui.selfSpices.length < s.shelf && (s.stock[id] || 0) > ui.selfSpices.filter((k) => k === id).length) {
        ui.selfSpices.push(id);
        Rules.invalidate(s);
        changed = true;
      }
      break;
    case "self-clear":
      ui.selfSpices = [];
      Rules.invalidate(s);
      changed = true;
      break;
    case "prepare": {
      const check = Rules.prepareSelf(s, ui.selfRecipe, ui.selfSpices);
      changed = check.ok;
      toast(check.ok ? "시험용 귀환 준비를 확인했습니다." : check.reasons[0]);
      break;
    }
    case "descend":
      changed = Rules.descend(s);
      resetScroll = true;
      break;
    case "ending-next":
      if (s.phase === "ending") {
        if (s.endingStep === "depart") s.endingStep = "epilogue";
        else Story.afterReturn(s);
        changed = true;
        ui.station = "door";
        resetScroll = true;
      }
      break;
    case "ending-replay":
      showDialog('<span class="eyebrow">ENDING A · PROPOSAL</span><h2>다녀왔어.</h2><p>동료들과 함께 돌아온 뒤, 식당의 불을 다시 켠다. 이번에는 앞으로 가는 힘과 돌아올 힘을 함께 준비한다.</p><p class="small muted">💡 시험용 후일담 · 기간과 구체적인 구조 과정은 미정입니다.</p><div class="actions"><button data-act="close-dialog" class="primary">식당으로</button></div>');
      return;
    case "export": {
      const url = URL.createObjectURL(new Blob([JSON.stringify(s, null, 2)], { type: "application/json" }));
      const a = document.createElement("a");
      a.href = url;
      a.download = `층계참식당-${s.mode}-${s.day}일차.json`;
      a.click();
      setTimeout(() => URL.revokeObjectURL(url), 1e3);
      return;
    }
    case "title":
      input.clear();destination=null;scene?.setDestination(null);
      persist();
      state = null;
      ui.station = "door";
      dom.dialog.close();
      dom.save.textContent = "";
      render(true);
      return;
    default:
      return;
  }
  if (changed) persist();
  render(resetScroll);
  if (["report-next", "report-all", "morning"].includes(act)) morningScene();
});
document.addEventListener("click", (event) => {
  const b = event.target.closest("button[data-act]");
  if (b && !b.disabled) bus.emit("Action", { act: b.dataset.act, id: b.dataset.id || "" });
});
setInterval(() => {
  if (state && Rules.finishCook(state, Date.now())) {
    persist();
    render();
  } else updateCookProgress();
}, 150);
document.addEventListener("visibilitychange", () => {
  if (document.hidden) persist();
});
dom.dialog.addEventListener('close',()=>{ui.management=false;});
render();
requestAnimationFrame(frame);
