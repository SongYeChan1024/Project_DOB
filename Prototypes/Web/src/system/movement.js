import {ROOM,CONTROL,STATIONS,CURTAINS,FIXTURES,WAITING} from '../define/room.js';
import {SEATS} from '../define/data.js';
import {isActiveVisit,waitingSpot} from '../define/service.js';
const rect=(id,x,z,width,depth)=>({id,minX:x-width/2,maxX:x+width/2,minZ:z-depth/2,maxZ:z+depth/2});
export const OBSTACLES=[...FIXTURES.map(o=>rect(o.id,o.x,o.z,o.width,o.depth)),...CURTAINS.map((o,i)=>rect('curtain-'+i,o.x,o.z,o.width,o.depth)),...SEATS.flatMap(s=>[
  rect('table-'+s.id,s.x,s.z,1.36,1.36),rect('chair-'+s.id,s.x+1.02,s.z,.66,.68)
])];
export function isWalkable(p,radius=CONTROL.radius){
  if(!Number.isFinite(p?.x)||!Number.isFinite(p?.z))return false;
  if(p.x<ROOM.minX+radius||p.x>ROOM.maxX-radius||p.z<ROOM.minZ+radius||p.z>ROOM.maxZ-radius)return false;
  return !OBSTACLES.some(o=>p.x>o.minX-radius&&p.x<o.maxX+radius&&p.z>o.minZ-radius&&p.z<o.maxZ+radius);
}
export function restorePlayer(s){
  if(!isWalkable(s.player))s.player={...CONTROL.spawn};
  if(!Number.isFinite(s.player.yaw))s.player.yaw=0;
  return s.player;
}
export function screenDirection(horizontal,vertical,angle){
  const n=Math.hypot(horizontal,vertical);if(!n)return{x:0,z:0};
  const h=horizontal/n,v=vertical/n;
  return{x:h*Math.cos(angle)+v*Math.sin(angle),z:-h*Math.sin(angle)+v*Math.cos(angle)};
}
export function movePlayer(p,horizontal,vertical,seconds,angle){
  const d=screenDirection(horizontal,vertical,angle),dt=Math.min(CONTROL.maxFrameSeconds,Math.max(0,seconds));
  const distance=CONTROL.speed*dt,steps=Math.max(1,Math.ceil(distance/.08));
  const next={...p};
  for(let i=0;i<steps;i++){
    const x=next.x+d.x*distance/steps,z=next.z+d.z*distance/steps;
    if(isWalkable({x,z})){next.x=x;next.z=z;}
    else {if(isWalkable({x,z:next.z}))next.x=x;if(isWalkable({x:next.x,z}))next.z=z;}
  }
  if(Math.hypot(next.x-p.x,next.z-p.z)>1e-6)next.yaw=Math.atan2(d.x,d.z);
  return next;
}
export function targets(s){
  const list=Object.values(STATIONS).map(o=>({...o}));
  if(s?.phase==='report')list.push({id:'guest',name:'귀환 손님',...WAITING});
  if(s?.phase==='service'){
    const visit=s.visits[s.index];
    for(const v of s.visits.filter(isActiveVisit)){
      const p=v.seat===null?waitingSpot(v.waitSlot):{x:SEATS[v.seat].x+1.05,z:SEATS[v.seat].z-.95};
      list.push({id:'guest:'+v.id,name:s.guests[v.guestId].name,...p});
    }
    if(visit?.status==='waiting')for(const t of SEATS)if(s.seats.includes(t.id)&&!s.visits.some(v=>v!==visit&&isActiveVisit(v)&&v.seat===t.id))list.push({id:'seat:'+t.id,name:t.name,x:t.x-.25,z:t.z-1.1});
  }
  return list;
}
export function clearLine(a,b){
  const n=Math.ceil(Math.hypot(b.x-a.x,b.z-a.z)/.08);
  for(let i=1;i<n;i++){const p={x:a.x+(b.x-a.x)*i/n,z:a.z+(b.z-a.z)*i/n};if(OBSTACLES.some(o=>p.x>o.minX&&p.x<o.maxX&&p.z>o.minZ&&p.z<o.maxZ))return false;}
  return true;
}
export function targetDistance(s,id){const t=targets(s).find(t=>t.id===id);return t?Math.hypot(s.player.x-t.x,s.player.z-t.z):Infinity;}
export function canReach(s,id){const t=targets(s).find(t=>t.id===id);return !!t&&targetDistance(s,id)<=CONTROL.reach&&clearLine(s.player,t);}
export function nearestTarget(s){return targets(s).filter(t=>canReach(s,t.id)).sort((a,b)=>targetDistance(s,a.id)-targetDistance(s,b.id))[0]||null;}
