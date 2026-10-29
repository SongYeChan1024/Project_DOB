import {loadMeal} from './helpers.mjs';
import test from 'node:test';
import assert from 'node:assert/strict';
import * as R from '../src/system/rules.js';
import {REGULARS,REGULAR_STORIES} from '../src/define/relationships.js';
import {D} from '../src/define/data.js';
import {clone} from '../src/utils/runtime.js';
function report(s,id,regular='boris',tier='great',score=4,extra={}){
  const g=s.guests['regular-'+regular];
  const r={id,guestId:g.id,name:g.name,floor:1,taste:{score},result:R.outcome({poison:1},{poison:1}),...extra};r.result.tier=tier;
  s.phase='close';s.pending=[r];R.nextMorning(s);return r;
}
function cookFirst(s){R.startService(s);R.chooseSeat(s,0);R.chooseMeal(s,'bread');loadMeal(s);R.cook(s,0);R.finishCook(s,3000);R.pickup(s);R.serve(s);}
test('legacy migration preserves guest identity, running visit, settled reports and separate names',()=>{
  const s=R.newState('normal',17);s.guests={};const g=R.createGuest(s,1);g.name='솔';g.knownLikes=['고소'];
  s.visits=[{id:'legacy',guestId:g.id,status:'carrying'}];s.committedIds=['old'];const legacy=clone(g),visits=clone(s.visits),seed=s.seed;
  R.migrateRelationships(s);R.migrateRelationships(s);assert.deepEqual(s.guests[g.id],legacy);assert.deepEqual(s.visits,visits);assert.deepEqual(s.committedIds,['old']);assert.equal(s.seed,seed);
  assert.equal(Object.values(s.guests).filter(g=>g.regularId).length,6);assert.notEqual(s.guests['regular-sol'].id,g.id);assert.equal(s.guests['regular-sol'].affinity,0);
});
test('normal first day is fixed; rotation includes all six and named passers without duplicates',()=>{
  const s=R.newState('normal',18),seen=new Set();
  for(let day=0;day<4;day++){
    R.startService(s);if(day===0)assert.deepEqual(s.visits.map(v=>s.guests[v.guestId].regularId),['boris','amel','jwi']);
    else assert.equal(s.visits.filter(v=>!s.guests[v.guestId].regularId).length,1);
    assert.equal(new Set(s.visits.map(v=>v.guestId)).size,3);
    s.visits.forEach(v=>{const g=s.guests[v.guestId];if(g.regularId)seen.add(g.regularId);});
    while(s.phase==='service')R.advanceGuest(s,true);R.nextMorning(s);
  }assert.equal(seen.size,6);
});
test('affinity combines return and taste, clamps bounds, and never depends on name',()=>{
  const cases=[['great',4,2,5],['good',3,2,4],['narrow',2,2,2],['hurt',4,2,3],['good',0,2,2],['hurt',0,0,0],['great',4,11,12]];
  for(const [tier,score,before,after]of cases){const s=R.newState();s.guests['regular-boris'].affinity=before;const r=report(s,'r','boris',tier,score);assert.equal(r.relationship.after,after);assert.equal(r.relationship.gain,after-before);}
  const s=R.newState();const g=R.createGuest(s,1);g.name='보리';report(s,'passer','boris','great',4,{guestId:g.id});assert.equal(g.affinity,undefined);assert.equal(s.guests['regular-boris'].affinity,0);
});
test('all 18 stories unlock in order and eight keepsakes survive reload and affinity loss',()=>{
  let s=R.newState('normal',1);
  for(const g of REGULARS){s.guests['regular-'+g.id].affinity=2;const order=[];
    for(let n=0;n<4;n++){order.push(...report(s,g.id+n,g.id).relationship.stories);s=clone(s);R.migrateRelationships(s);}
    assert.deepEqual(order,g.stories.map(st=>st.id));assert.equal(s.guests['regular-'+g.id].affinity,12);
  }
  assert.equal(R.relationshipView(s).keepsakes.length,8);assert.equal(Object.keys(REGULAR_STORIES).length,18);
  const g=s.guests['regular-boris'];s.phase='prep';s.day=1;R.startService(s);assert.equal(R.advanceGuest(s,true),true);assert.equal(g.affinity,11);assert.equal(R.relationshipView(s).keepsakes.length,8);
});
test('same report cannot duplicate affection, levels, gifts, or inventory after reload',()=>{
  let s=R.newState();s.guests['regular-amel'].affinity=6;const r=report(s,'once','amel');const g=clone(s.guests['regular-amel']),stock=clone(s.stock);
  s=clone(s);s.phase='close';s.pending=[clone(r)];R.nextMorning(s);assert.deepEqual(s.guests['regular-amel'],g);assert.deepEqual(s.stock,stock);assert.equal(s.reports.length,0);
});
test('promised materials are paid only on good/great return and included in actual loot',()=>{
  for(const tier of ['great','good','narrow','hurt']){
    const s=R.newState();const before=s.stock.flour;const r=report(s,'promise','boris',tier,2,{promise:'flour'});
    assert.equal(r.promiseDelivered||0,['great','good'].includes(tier)?2:0);
    assert.equal(s.stock.flour-before,['great','good'].includes(tier)?2:0);
  }
});
test('visit hints and promises persist through seat changes and serialization',()=>{
  const s=R.newState('normal',42);s.day=2;s.guests['regular-boris'].affinity=7;R.startService(s);
  const v=s.visits.find(v=>v.guestId==='regular-boris');assert.equal(v.hints.length,2);assert.equal(new Set(v.hints).size,2);
  const before=clone(v),seed=s.seed;R.chooseSeat(s,0);R.chooseSeat(s,1);assert.equal(s.seed,seed);assert.equal(v.promise,before.promise);assert.deepEqual(v.hints,before.hints);
  const copy=clone(s);R.migrateRelationships(copy);assert.deepEqual(copy.visits,s.visits);
  for(const x of s.visits)if(x.promise)assert.ok(!D.ING[x.promise].rare);
});
test('maximum affection bonus is captured on departure, not newly earned at return',()=>{
  for(const before of [11,12]){const s=R.newState('normal',31);s.guests['regular-boris'].affinity=before;cookFirst(s);const r=s.pending[0];assert.equal(r.maxAffinityBonus,before===12?1:0);}
});
test('level cap and level-limited floors follow the existing growth rule',()=>{
  const s=R.newState();report(s,'one');assert.equal(s.guests['regular-boris'].level,2);
  report(s,'two','boris','narrow');assert.equal(s.guests['regular-boris'].level,2);
  report(s,'three','boris','good');assert.equal(s.guests['regular-boris'].level,3);
  const g=s.guests['regular-boris'];g.level=6;g.xp=0;report(s,'cap');assert.equal(g.level,6);assert.equal(g.xp,0);
  for(const x of Object.values(s.guests))x.level=1;s.floor=5;s.phase='prep';R.startService(s);assert.ok(s.visits.filter(v=>s.guests[v.guestId].regularId).every(v=>s.guests[v.guestId].floor<=2));
});
test('Sol map reveals one future-floor hazard without granting party clues or ending',()=>{
  const s=R.newState();s.guests['regular-sol'].affinity=11;const r=report(s,'map','sol');assert.equal(r.relationship.map.floor,2);assert.equal(Object.keys(s.known[2]).length,1);assert.equal(s.floor,1);assert.deepEqual(s.story.clues,[]);assert.equal(s.story.afterReturn,false);
});
test('relationship trial is separate and preserves pre-threshold starting states',()=>{
  const normal=R.newState('normal',1),trial=R.newState('relations',1);
  assert.equal(normal.guests['regular-sol'].affinity,0);assert.equal(trial.guests['regular-sol'].affinity,11);
  assert.deepEqual(trial.guests['regular-sol'].stories,['sol:3','sol:7']);R.startService(trial);assert.deepEqual(trial.visits.slice(0,3).map(v=>v.guestId),['regular-boris','regular-amel','regular-sol']);
});
