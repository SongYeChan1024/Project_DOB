import test from 'node:test';
import assert from 'node:assert/strict';
import * as R from '../src/system/rules.js';
import * as Move from '../src/system/movement.js';
import {openingLine} from '../src/system/greeting.js';
import {D,CFG} from '../src/define/data.js';
import {OPENING_LINES} from '../src/define/service.js';
import {loadMeal} from './helpers.mjs';
const clone=s=>JSON.parse(JSON.stringify(s));
function prepare(s,index,recipe='bread'){R.selectVisit(s,s.visits[index].id);assert.ok(R.chooseSeat(s,index));assert.ok(R.chooseMeal(s,recipe));loadMeal(s);}
test('all open seats admit distinct simultaneous guests with exclusive seat ownership',()=>{
  for(const mode of ['normal','relations']){
    const s=R.newState(mode,1);R.startService(s);assert.equal(s.visits.length,s.seats.length);
    assert.equal(new Set(s.visits.map(v=>v.guestId)).size,s.visits.length);
    prepare(s,0);const id=s.visits[1].id;R.selectVisit(s,id);assert.equal(R.chooseSeat(s,0),false);assert.equal(R.chooseSeat(s,1),true);
    assert.equal(R.currentVisit(s).id,id);assert.equal(s.visits[0].recipe,'bread');assert.equal(s.visits[1].recipe,null);
    const targets=Move.targets(s).filter(x=>x.id.startsWith('guest:'));assert.equal(targets.length,s.visits.length);assert.equal(new Set(targets.map(x=>x.x+','+x.z)).size,s.visits.length);
    assert.equal(R.selectVisit(s,'missing'),false);assert.equal(R.nextMorning(s).length,0);
  }
});
test('orders cook independently of selection; carried food stays with its owner across reload',()=>{
  let s=R.newState('relations',14);R.startService(s);prepare(s,0);R.cook(s,0);prepare(s,1,'stew');
  const stock=clone(s.stock);assert.equal(R.cook(s,0),false);assert.deepEqual(s.stock,stock);
  assert.equal(R.finishCook(s,CFG.cookMs),true);assert.equal(s.visits[0].status,'ready');assert.equal(R.currentVisit(s).status,'waiting');
  assert.ok(R.cook(s,CFG.cookMs));R.selectVisit(s,s.visits[0].id);assert.ok(R.pickup(s));
  R.selectVisit(s,s.visits[1].id);assert.equal(R.serve(s),false);R.finishCook(s,CFG.cookMs*2);
  assert.equal(R.pickup(s),false);s=clone(s);R.migrateService(s);assert.equal(R.carriedVisit(s).id,s.visits[0].id);
  const gold=s.gold;R.selectVisit(s,s.visits[0].id);assert.ok(R.serve(s));assert.equal(s.pending[0].guestId,s.visits[0].guestId);assert.ok(s.gold>gold);assert.equal(R.serve(s),false);
  R.advanceGuest(s);assert.equal(s.phase,'service');assert.equal(R.seatOwner(s,0),undefined);
  R.selectVisit(s,s.visits[1].id);assert.ok(R.pickup(s));assert.ok(R.serve(s));R.advanceGuest(s);
  while(s.phase==='service')assert.ok(R.advanceGuest(s,true));assert.equal(s.phase,'close');assert.equal(R.nextMorning(s).length,2);
});
test('manual ingredients require exact counts and stock is consumed only once at start',()=>{
  let s=R.newState('normal',2);R.startService(s);R.chooseSeat(s,0);R.chooseMeal(s,'bread');const before=clone(s.stock);
  assert.equal(R.cook(s,0),false);assert.ok(R.addIngredient(s,'flour'));assert.equal(R.cook(s,0),false);assert.deepEqual(s.stock,before);
  s=clone(s);R.migrateService(s);assert.equal(R.currentVisit(s).ingredients.flour,1);assert.ok(R.addIngredient(s,'flour'));assert.equal(R.addIngredient(s,'flour'),false);
  assert.equal(R.addIngredient(s,'potato'),false);assert.ok(R.chooseSpice(s,'salt'));assert.equal(R.chooseSpice(s,'garlic'),false);
  assert.ok(R.removeIngredient(s,'flour'));assert.equal(R.cook(s,0),false);R.addIngredient(s,'flour');assert.ok(R.cook(s,0));
  assert.equal(s.stock.flour,before.flour-2);assert.equal(s.stock.salt,before.salt-1);const after=clone(s.stock);assert.equal(R.cook(s,0),false);assert.deepEqual(s.stock,after);
});
test('staged food does not reserve stock and a competing order cannot overdraw it',()=>{
  const s=R.newState('normal',7);s.stock.flour=2;R.startService(s);prepare(s,0);prepare(s,1);
  assert.ok(R.cook(s,0));R.finishCook(s,CFG.cookMs);R.selectVisit(s,s.visits[0].id);assert.equal(R.cook(s,CFG.cookMs),false);assert.equal(s.stock.flour,0);
});
test('legacy sequential saves hide departed customers and preserve current plate, money and reports',()=>{
  const s=R.newState('relations',1);R.startService(s);prepare(s,0);R.cook(s,0);R.finishCook(s,CFG.cookMs);R.pickup(s);R.serve(s);
  // Old format retained status=served after incrementing the index.
  s.index=1;delete s.serviceVersion;prepare(s,1);R.cook(s,CFG.cookMs);R.finishCook(s,CFG.cookMs*2);R.pickup(s);
  const money=s.gold,stock=clone(s.stock),pending=clone(s.pending);delete s.shelf;
  R.migrateService(s);assert.equal(s.visits[0].status,'departed');assert.equal(s.visits[1].status,'carrying');assert.equal(s.shelf,2);
  assert.equal(s.gold,money);assert.deepEqual(s.stock,stock);assert.deepEqual(s.pending,pending);const first=clone(s);R.migrateService(s);assert.deepEqual(s,first);
});
test('opening lines reflect tiredness or a real hazard and never reroll on selection/reload',()=>{
  const g={id:'g',tired:true};const tired=openingLine(g,{poison:3},'v');assert.equal(tired.key,'tired');assert.ok(OPENING_LINES.tired.includes(tired.text));
  g.tired=false;const poison=openingLine(g,{poison:3,fire:1},'v');assert.equal(poison.key,'poison');g.lastOpening=poison.text;assert.notEqual(openingLine(g,{poison:3},'v').text,poison.text);
  assert.equal(openingLine(g,{},'empty').key,'well');const s=R.newState('normal',1);R.startService(s);const lines=s.visits.map(v=>v.opening);assert.ok(new Set(lines).size>1);
  R.selectVisit(s,s.visits[2].id);const saved=clone(s);R.migrateService(saved);assert.deepEqual(saved.visits.map(v=>v.opening),lines);
});
test('original economy, shelf and post-closing upgrades are available without wiping old progression',()=>{
  const s=R.newState();assert.equal(s.gold,50);assert.equal(s.stock.flour,4);assert.equal(s.shelf,1);assert.deepEqual(Object.keys(s.discovered),['flour','potato','meat']);
  s.gold=500;s.phase='close';assert.ok(R.upgrade(s,'seat',3));assert.equal(s.gold,430);assert.ok(R.upgrade(s,'stove'));assert.equal(s.gold,340);
  assert.ok(R.upgrade(s,'shelf'));assert.equal(s.gold,220);assert.equal(s.shelf,2);assert.equal(R.upgrade(s,'shelf'),false);assert.equal(R.upgrade(s,'seat',3),false);
  const before=s.stock.flour;assert.ok(R.buy(s,'flour',5));assert.equal(s.stock.flour,before+5);const saved=clone(s);R.migrateService(saved);assert.equal(saved.seats.length,4);assert.equal(saved.shelf,2);
  s.phase='service';const money=s.gold;assert.equal(R.upgrade(s,'seat',4),false);assert.equal(R.buy(s,'flour'),false);assert.equal(s.gold,money);
});
test('return reveals at most two unknown hazard axes and discovered recipe ingredients',()=>{
  const s=R.newState('story',99);R.startService(s);prepare(s,0);R.cook(s,0);R.finishCook(s,CFG.cookMs);R.pickup(s);R.serve(s);R.advanceGuest(s);while(s.phase==='service')R.advanceGuest(s,true);
  const reports=R.nextMorning(s);assert.equal(reports.length,1);assert.ok(reports[0].report.learned.length<=2);assert.ok(Object.keys(s.known[5]).length<=2);
});
test('early closing cannot discard paid ingredients or held plates',()=>{
  const s=R.newState('normal',22);R.startService(s);prepare(s,0);assert.ok(R.cook(s,0));assert.equal(R.earlyClose(s),false);
  R.finishCook(s,CFG.cookMs);assert.equal(R.earlyClose(s),false);R.pickup(s);assert.equal(R.earlyClose(s),false);R.serve(s);
  const gold=s.gold,stock=clone(s.stock);assert.ok(R.earlyClose(s));assert.equal(s.phase,'close');assert.equal(s.pending.length,1);assert.equal(s.gold,gold);assert.deepEqual(s.stock,stock);
});
