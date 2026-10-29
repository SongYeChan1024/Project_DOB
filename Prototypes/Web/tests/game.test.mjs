import {loadMeal} from './helpers.mjs';
import test from 'node:test';
import assert from 'node:assert/strict';
import {D,AXES,CFG,PREVIEW} from '../src/define/data.js';
import * as R from '../src/system/rules.js';
import * as S from '../src/system/story.js';
import {clone} from '../src/utils/runtime.js';

function finishDay(s){
  assert.equal(R.startService(s),true);
  for(let i=0;i<s.visits.length;i++){
    const r=D.RECIPES.find(r=>R.canCook(s,r));assert.ok(r);
    R.chooseSeat(s,1);R.chooseMeal(s,r.id);loadMeal(s);assert.equal(R.cook(s,0),true);
    R.finishCook(s,CFG.cookMs);R.pickup(s);R.serve(s);R.advanceGuest(s);
  }
  const events=R.nextMorning(s);for(const e of events)S.processReport(s,e.report);
  s.reportIndex=s.reports.length;s.phase='prep';
  const scene=S.nextScene(s);if(scene)assert.equal(S.finishScene(s,scene.id),true);
  return events;
}
const report=(id,guestId,tier,extra={})=>({id,guestId,floor:1,day:1,result:{tier,surprise:null,absorbed:false},...extra});
test('six-axis deficit boundaries and reserve only absorb an actual surprise',()=>{
  for(const [n,tier]of [[0,'great'],[1,'good'],[2,'narrow'],[3,'hurt'],[9,'hurt']])assert.equal(R.outcome({poison:n},{vital:9}).tier,tier);
  assert.equal(R.outcome({poison:1},{poison:1,vital:1},'fire').tier,'great');
  assert.equal(R.outcome({poison:1},{poison:1},'fire').tier,'good');
  assert.equal(R.outcome({poison:1},{poison:1,vital:9}).absorbed,false);
  assert.deepEqual(R.vector({poison:-3}),Object.fromEntries(AXES.map(a=>[a,0])));
});
test('hazard uses floor, anomaly, tiredness, race and profession; name is cosmetic',()=>{
  const s=R.newState('normal',42),g=R.createGuest(s,1);g.race='elf';g.job=Object.keys(D.CLASSES).find(k=>D.CLASSES[k].res==='fire');g.tired=true;s.anomaly={floor:1,axis:'fire'};
  const h=R.hazards(s,g);assert.equal(h.fire,(D.FLOORS[1].haz.fire||0)+1);assert.equal(h.vital,(D.FLOORS[1].haz.vital||0)+1);
  g.name='임의 이름';assert.deepEqual(R.hazards(s,g),h);
});
test('serve and morning settlement are idempotent across a serialized save',()=>{
  let s=R.newState('normal',21);R.startService(s);R.chooseSeat(s,0);const r=D.RECIPES.find(r=>R.canCook(s,r));R.chooseMeal(s,r.id);loadMeal(s);
  assert.equal(R.cook(s,100),true);const stock=clone(s.stock);assert.equal(R.cook(s,100),false);assert.deepEqual(s.stock,stock);
  assert.equal(R.finishCook(s,100),false);s=clone(s);assert.equal(R.finishCook(s,100+CFG.cookMs),true);
  assert.equal(R.serve(s),false);assert.equal(R.pickup(s),true);assert.equal(R.pickup(s),false);assert.equal(R.serve(s),true);const gold=s.gold;assert.equal(R.serve(s),false);assert.equal(s.gold,gold);
  R.advanceGuest(s);R.advanceGuest(s,true);R.advanceGuest(s,true);
  const events=R.nextMorning(s);assert.equal(events.length,1);const settled=clone(s);
  assert.deepEqual(R.nextMorning(s),[]);assert.deepEqual(s,settled);
  assert.equal(S.processReport(s,events[0].report),true);const story=clone(s.story);assert.equal(S.processReport(s,events[0].report),false);assert.deepEqual(s.story,story);
});
test('repeat guests retain personal names and rest period excludes injured guests',()=>{
  const s=R.newState('normal',13);R.startService(s);const originals=clone(s.guests);while(s.phase==='service')R.advanceGuest(s,true);R.nextMorning(s);
  const excluded=Object.keys(s.guests)[0];s.guests[excluded].restUntil=s.day+2;R.startService(s);
  assert.ok(s.visits.every(v=>v.guestId!==excluded));for(const g of Object.values(originals))assert.equal(s.guests[g.id].name,g.name);
});
test('old object alone is not current survival proof; request must match a report',()=>{
  const s=R.newState('story',4);S.processReport(s,report('first','g','hurt',{floor:5}));assert.deepEqual(s.story.clues,['mark','gap']);
  assert.equal(S.requestInvestigation(s,'question'),true);S.processReport(s,report('wrong','g','good',{floor:5,assignment:'no-such-request'}));assert.ok(!s.story.clues.includes('reply'));
  S.processReport(s,report('answer','g','good',{floor:5,assignment:'question-1'}));assert.ok(s.story.clues.includes('reply'));assert.ok(s.story.clues.includes('cooking'));
});
test('out-of-order facts queue dependent scenes; at most one scene per morning',()=>{
  const s=R.newState('story',8);for(const id of ['outer','inner','cooking','reply','gap','mark'])S.acquireClue(s,id);
  for(const id of ['PARTY_MARK_FOUND','CURRENT_SURVIVAL_CONFIRMED','THEY_KEPT_COOKING','ISOLATED_AREA_LOCATED']){
    assert.equal(S.nextScene(s).id,id);assert.equal(S.finishScene(s,id),true);assert.equal(S.nextScene(s),null);s.day++;
  }
  assert.equal(S.finishScene(s,'PARTY_MARK_FOUND'),false);
});
test('injured return is tied to identity, not reused display name',()=>{
  const s=R.newState();S.processReport(s,report('hurt','a','hurt'));S.processReport(s,report('other','b','great'));
  assert.ok(!s.story.queue.some(q=>q.id==='INJURED_GUEST_RETURNS_WELL'));
  S.processReport(s,report('same','a','great'));assert.ok(s.story.queue.some(q=>q.id==='INJURED_GUEST_RETURNS_WELL'));
});
test('declined investigation remains assignable on the following day',()=>{
  const s=R.newState('story',8);S.acquireClue(s,'gap');S.requestInvestigation(s,'question');R.startService(s);
  assert.ok(s.visits.some(v=>v.assignment==='question-1'));while(s.phase==='service')R.advanceGuest(s,true);R.nextMorning(s);R.startService(s);
  assert.ok(s.visits.some(v=>v.assignment==='question-1'));assert.equal(s.story.requests['question-1'].done,false);
});
test('story can reach all facts through actual service days, then ending trial continues same save',()=>{
  const s=R.newState('story',20261026);
  finishDay(s);assert.ok(s.story.clues.includes('mark'));assert.ok(!s.story.clues.includes('reply'));
  S.requestInvestigation(s,'question');S.requestInvestigation(s,'survey');finishDay(s);
  assert.equal(s.story.clues.length,6);
  while(!s.story.done.includes('ISOLATED_AREA_LOCATED')&&s.day<9)finishDay(s);
  assert.ok(s.story.done.includes('ISOLATED_AREA_LOCATED'));
  assert.equal(R.descend(s),false);s.previewEnabled=true;s.story.plan=true;
  for(const id of [...Object.keys(D.ING),...Object.keys(D.SPICES)])s.stock[id]=20;
  s.gold=200;s.stove=2;
  const spiceKeys=Object.keys(D.SPICES);let combo;
  for(const r of D.RECIPES)for(const a of spiceKeys)for(const b of spiceKeys)if(R.previewReadiness(s,r.id,[a,b]).ok)combo={id:r.id,spices:[a,b]};
  assert.ok(combo,'An attainable meal must satisfy the temporary self-prescription values');
  assert.equal(R.prepareSelf(s,combo.id,combo.spices).ok,true);const before=s.gold;
  assert.equal(R.descend(s),true);assert.equal(s.gold,before-PREVIEW.supplyCost);const stock=clone(s.stock);
  assert.equal(R.descend(s),false);assert.deepEqual(s.stock,stock);
  const growth={floor:s.floor,stove:s.stove,seats:clone(s.seats),known:clone(s.known)};
  S.afterReturn(s);assert.equal(s.phase,'prep');assert.deepEqual({floor:s.floor,stove:s.stove,seats:s.seats,known:s.known},growth);
  assert.equal(S.requestInvestigation(s,'survey'),false);assert.equal(R.startService(s),true);assert.ok(s.visits.every(v=>!v.assignment));
});
test('prepared departure is revalidated after missing stock or stale configuration',()=>{
  const s=R.newState('story');s.previewEnabled=true;s.story.plan=true;s.story.clues=['mark','gap','reply','inner','outer','cooking'];s.story.done=['ISOLATED_AREA_LOCATED'];
  s.prepared={recipeId:D.RECIPES[0].id,spices:[],config:'old'};assert.equal(R.descend(s),false);
  s.prepared.config=PREVIEW.version;s.stock={};assert.equal(R.descend(s),false);assert.equal(s.prepared,null);
});
