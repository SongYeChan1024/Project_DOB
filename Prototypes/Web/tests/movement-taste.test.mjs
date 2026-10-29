import {loadMeal} from './helpers.mjs';
import test from 'node:test';
import assert from 'node:assert/strict';
import * as Move from '../src/system/movement.js';
import * as Taste from '../src/system/taste.js';
import * as R from '../src/system/rules.js';
import {CONTROL,STATIONS,CURTAINS} from '../src/define/room.js';
import {D,SEATS} from '../src/define/data.js';

test('movement is camera-relative, diagonal normalized, and capped after suspension',()=>{
  const p={x:0,z:-3.5,yaw:0};
  const forward=Move.movePlayer(p,0,-1,.05,0);assert.equal(forward.x,0);assert.ok(forward.z<p.z);
  const right=Move.movePlayer(p,1,0,.05,Math.PI/2);assert.ok(right.z<p.z);assert.ok(Math.abs(right.x)<1e-6);
  const diagonal=Move.movePlayer(p,1,-1,.05,0);assert.ok(Math.abs(Math.hypot(diagonal.x-p.x,diagonal.z-p.z)-CONTROL.speed*.05)<1e-8);
  const resumed=Move.movePlayer(p,1,0,600,0);assert.ok(resumed.x-p.x<=CONTROL.speed*CONTROL.maxFrameSeconds+1e-8);
});
test('walls, kitchen, table, chair and curtain stop the character',()=>{
  assert.equal(Move.isWalkable({x:-1.8,z:-5.65}),false);assert.equal(Move.isWalkable({x:SEATS[0].x,z:SEATS[0].z}),false);
  assert.equal(Move.isWalkable({x:SEATS[0].x+1.02,z:SEATS[0].z}),false);
  assert.equal(Move.isWalkable(CURTAINS[0]),false);
  for(const [start,h,v] of [[{x:0,z:-4.2,yaw:0},0,-1],[{x:-4.15,z:-3,yaw:0},0,1],[{x:7,z:5.8,yaw:0},1,0]]){
    let p=start;assert.ok(Move.isWalkable(p));for(let i=0;i<300;i++){p=Move.movePlayer(p,h,v,.016,0);assert.ok(Move.isWalkable(p));}
  }
});
test('all workstations and all six serving locations are reachable through open aisles',()=>{
  // Flood-fill independent of the movement integrator: detect rooms isolated by furniture or curtains.
  const step=.2,start={x:-2,z:-3.6},queue=[start],visited=new Set(['-10,-18']);
  for(let head=0;head<queue.length;head++){
    const p=queue[head];for(const [dx,dz]of [[step,0],[-step,0],[0,step],[0,-step]]){
      const x=Math.round((p.x+dx)*5)/5,z=Math.round((p.z+dz)*5)/5,key=`${Math.round(x*5)},${Math.round(z*5)}`;
      if(!visited.has(key)&&Move.isWalkable({x,z})){visited.add(key);queue.push({x,z});}
    }
  }
  const goals=[...Object.values(STATIONS),...SEATS.flatMap(t=>[{x:t.x-.25,z:t.z-1.1},{x:t.x+1.05,z:t.z-.95}])];
  for(const t of goals)assert.ok(queue.some(p=>Math.hypot(p.x-t.x,p.z-t.z)<.3),JSON.stringify(t));
});
test('distance and curtains block remote interaction; invalid saved position recovers safely',()=>{
  const s=R.newState();Move.restorePlayer(s);assert.ok(Move.canReach(s,'kitchen'));assert.equal(Move.canReach(s,'door'),false);
  assert.equal(Move.clearLine({x:CURTAINS[0].x,z:-3},{x:CURTAINS[0].x,z:-1.5}),false);
  s.player={x:NaN,z:10};Move.restorePlayer(s);assert.deepEqual(s.player,CONTROL.spawn);
  s.player={x:-1.8,z:-5.65,yaw:0};Move.restorePlayer(s);assert.deepEqual(s.player,CONTROL.spawn);
});
test('taste tags are unique and likes/dislikes affect money independently of hazard',()=>{
  const g={likes:['고소','담백'],dislikes:['향긋']},r=D.RECIPES.find(r=>r.id==='bread');
  const t=Taste.tasteScore(g,Taste.dishTags(r,['garlic','garlic']));assert.equal(t.score,2);assert.deepEqual(t.disliked,['향긋']);
  assert.equal(Taste.tasteScore(g,Taste.dishTags(r)).score,4);
  assert.equal(Taste.mealPrice(r,[],{score:4}),11);assert.equal(Taste.mealPrice(r,[],{score:0}),5);
  const s=R.newState(),guest=R.createGuest(s,1),haz=R.hazards(s,guest);guest.likes=['얼큰'];guest.dislikes=['담백'];assert.deepEqual(R.hazards(s,guest),haz);
});
test('taste discovery survives serialization and does not reveal all preferences upfront',()=>{
  const s=R.newState('normal',52),g=R.createGuest(s,1);assert.equal(g.knownLikes.length,1);assert.equal(g.knownDislikes.length,0);
  const taste=Taste.tasteScore(g,[g.likes[1]]);const found=Taste.learnTaste(g,taste);assert.equal(found.length,1);assert.equal(g.knownLikes.length,2);
  const copy=JSON.parse(JSON.stringify(g)),seed=s.seed;Taste.ensureTaste(s,copy);assert.deepEqual(copy,g);assert.equal(s.seed,seed);
  assert.deepEqual(Taste.learnTaste(g,taste),[]);
});
test('carried plate survives reload, cannot be served twice, and taste price applies once',()=>{
  let s=R.newState('normal',17);R.startService(s);const g=R.currentGuest(s);g.likes=['고소','담백'];g.dislikes=['쿰쿰'];
  R.chooseSeat(s,0);R.chooseMeal(s,'bread');loadMeal(s);R.cook(s,0);R.finishCook(s,3000);
  assert.equal(R.serve(s),false);assert.equal(R.pickup(s),true);s=JSON.parse(JSON.stringify(s));
  const before=s.gold;assert.equal(R.serve(s),true);assert.equal(s.gold-before,11);assert.equal(s.pending[0].taste.score,4);
  assert.equal(R.serve(s),false);assert.equal(s.pending.length,1);
});
