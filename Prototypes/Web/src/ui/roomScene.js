import * as T from 'three';
import {SEATS} from '../define/data.js';
import {ROOM,CONTROL,WAITING,CURTAINS} from '../define/room.js';
import {REGULAR_STORIES} from '../define/relationships.js';
export function buildRoom(s){
  s.box(ROOM.width,.5,ROOM.depth,0,-.3,.4,'#364b4c');
  s.box(ROOM.width+.25,.2,ROOM.depth+.25,0,-.57,.4,'#26383b');
  for(let x=0;x<16;x++)for(let z=0;z<14;z++)s.box(.98,.08,.98,x-7.5,.01,z-6.1,(x+z)%3===0?'#93866e':(x+z)%3===1?'#a19278':'#9e8d70');
  s.box(16,1.7,.35,0,.85,-6.6,'#596a62');s.box(.35,1.5,14,-8,.75,.4,'#596a62');
  s.box(16,.15,.4,0,1.72,-6.6,'#b29b77');s.box(.4,.15,14,-8,1.55,.4,'#b29b77');
  for(let x=0;x<16;x++)for(let z=0;z<2;z++)s.box(.96,.48,.06,x-7.5,.3+z*.8,-6.39,'#718172');
  for(const x of [-7.8,7.8]){s.box(.45,2.6,.5,x,1.3,-6.4,'#454e48');s.box(.64,.18,.65,x,2.65,-6.4,'#b09c78');}
  // 벽 붙임 주방: 모든 작업은 홀 쪽 통로에서 한다.
  s.box(10.4,.025,1.2,-2,.09,-3.8,'#697d6c');
  const counter=s.box(3.6,1.12,1.4,-1.8,.6,-5.65,'#4f5c53');s.target(counter,'kitchen','조리대 · 가까이에서 E');
  s.box(3.7,.18,1.5,-1.8,1.24,-5.65,'#b89b70');
  for(let i=0;i<3;i++){const x=-3+i*.8;s.cylinder(.18,.12,.13,x,1.4,-5.5,['#8eaa7b','#e2ab69','#bf855e'][i]);for(let n=0;n<3;n++)s.sphere(.08,x+(n-1)*.1,1.5,-5.5,'#e1c27c');}
  s.box(.7,.06,.48,-.7,1.37,-5.5,'#cfb48c');
  s.box(4.2,.12,.42,-1.8,2.15,-6.15,'#b19b77');
  for(let i=0;i<6;i++)s.cylinder(.1,.12,.24,-3.4+i*.6,2.31,-6.15,['#bd9a72','#96a68b','#b8a8a0'][i%3]);
  const stove=s.box(1.8,1.2,1.5,-5,.65,-5.65,'#65685b');s.target(stove,'stove','화덕 · 조리 완료 후 E로 집기');
  s.box(.8,.5,.08,-5,.49,-4.85,'#251e19');s.fire=s.sphere(.22,-5,.48,-4.77,'#e5a05a');
  // 돌화덕의 아치·장작·굴뚝. 조작 위치와 충돌 범위는 유지한다.
  const arch=new T.Mesh(new T.TorusGeometry(.43,.12,5,9,Math.PI),s.mat('#b3a17f'));arch.position.set(-5,.68,-4.8);s.scene.add(arch);
  const darkArch=new T.Mesh(new T.CircleGeometry(.38,16,0,Math.PI),s.mat('#251e19'));darkArch.position.set(-5,.68,-4.79);s.scene.add(darkArch);
  for(const x of [-5.44,-4.56]){s.box(.22,.56,.18,x,.41,-4.8,'#a79879');s.box(.25,.04,.19,x,.43,-4.79,'#655e4c');}
  for(const z of [-4.83,-4.7]){const log=s.cylinder(.07,.08,.63,-5,.26,z,'#9d7547');log.rotation.z=Math.PI/2;}
  s.box(.46,.85,.5,-5.55,1.88,-6.03,'#777762');s.box(.59,.13,.61,-5.55,2.33,-6.03,'#ab9a76');
  s.fire.material=new T.MeshStandardMaterial({color:'#f7b45c',emissive:'#fb952a',emissiveIntensity:1.8});
  const fireLight=new T.PointLight('#ffae5b',7,6,2);fireLight.position.set(-5,1,-4.2);s.scene.add(fireLight);
  s.cylinder(.54,.42,.45,-5,1.44,-5.65,'#303b3a');s.cylinder(.49,.49,.03,-5,1.68,-5.65,'#d6ab65');
  for(const x of [-5.63,-4.37])s.box(.25,.1,.2,x,1.51,-5.65,'#39413b');
  s.steam=new T.Group();for(let i=0;i<4;i++){const p=s.sphere(.09+i*.025,Math.sin(i)*.18,i*.2,0,'#a2b9ab',s.steam);p.material=new T.MeshBasicMaterial({color:'#d8ddc8',transparent:true,opacity:.22});}
  s.steam.position.set(-5,1.8,-5.65);s.scene.add(s.steam);
  const desk=s.box(2,1,1.3,2,.55,-5.7,'#705a42');s.box(2,.12,1.35,2,1.11,-5.7,'#b0966f');s.target(desk,'book','수첩 · 가까이에서 E');
  s.box(.68,.05,.6,1.8,1.21,-5.55,'#e5dbbc').rotation.y=.16;s.box(.02,.06,.6,1.8,1.23,-5.55,'#726448');s.cylinder(.12,.16,.18,2.5,1.26,-5.7,'#4e605d');
  const door=s.box(.2,2.3,1.4,-7.8,1.18,-3.4,'#2b4c4a');s.target(door,'door','출입문 · 가까이에서 E');
  for(const z of [-4.2,-2.6])s.box(.35,2.4,.1,-7.7,1.2,z,'#b39c79');s.box(.35,.12,1.75,-7.7,2.4,-3.4,'#b39c79');
  s.box(1.3,.03,1.6,-7,.1,-3.4,'#c4ad7c');
  s.box(1.8,.12,4.1,6.55,-.05,-4.45,'#182d31');
  for(let i=0;i<9;i++){const m=s.box(1.8,.12,.43,6.55,-i*.07,-2.6-i*.43,'#718c85');s.target(m,'stairs','계단 앞 · 가까이에서 E');}
  for(const x of [5.55,7.55]){s.box(.15,1.25,4.1,x,.58,-4.45,'#586357');s.box(.22,.16,4.15,x,1.24,-4.45,'#ba9a6d');}
  // 커튼은 주방과 객석을 구분하고 가운데 및 양옆 통로는 비워 둔다.
  for(const curtain of CURTAINS){
    const half=curtain.width/2,count=Math.ceil(curtain.width/.125),fold=curtain.width/count;
    for(const x of [curtain.x-half,curtain.x+half]){s.cylinder(.055,.08,2.12,x,1.06,curtain.z,'#b49b72');s.sphere(.09,x,2.16,curtain.z,'#d8c28d');}
    s.box(curtain.width+.1,.08,.08,curtain.x,2.02,curtain.z,'#b49b72');
    for(let i=0;i<count;i++){const x=curtain.x-half+(i+.5)*fold,z=curtain.z+Math.sin(i*Math.PI/2)*.045;s.box(fold+.005,1.55,.055,x,1.13,z,i%2?'#6f8982':'#82978d');s.box(fold+.005,.10,.065,x,.46,z,'#c2a774');}
  }
  s.seatGroups=[];
  for(const seat of SEATS){
    const g=new T.Group();g.position.set(seat.x,0,seat.z);s.scene.add(g);
    const table=s.cylinder(.68,.62,.18,0,.9,0,'#c09a6a',g);s.cylinder(.14,.23,.8,0,.43,0,'#605441',g);
    s.box(.62,.12,.6,1.02,.5,0,'#a4815b',g);s.box(.1,.9,.6,1.3,.7,0,'#6f5c46',g);
    for(const x of [.78,1.26])for(const z of [-.2,.2])s.box(.09,.46,.09,x,.25,z,'#605441',g);
    s.cylinder(.22,.22,.04,-.12,1.02,.08,'#e6d9b5',g);s.cylinder(.1,.08,.18,.3,1.06,-.15,'#cab089',g);
    if(seat.axis==='poison'){s.cylinder(.18,.12,.24,-.35,1.09,-.24,'#b87853',g);for(let i=0;i<5;i++){const leaf=s.sphere(.15,-.35+Math.sin(i*2)*.12,1.34+(i%2)*.13,-.24+Math.cos(i*2)*.1,'#6a9f77',g);leaf.scale.set(.45,1.3,.6);}}
    if(seat.axis==='cold'){s.cylinder(.12,.15,.3,-.3,1.18,-.3,'#eece85',g);s.sphere(.08,-.3,1.4,-.3,'#fcbf66',g);}
    if(seat.axis==='vital')s.box(.58,.18,.55,1.02,.63,0,'#7b9e8c',g);
    if(seat.axis==='fire')for(let i=0;i<3;i++)s.box(.3,.05,.2,-.3,1.14+i*.12,-.22,'#89aaa2',g);
    if(seat.axis==='dark')s.box(.24,.3,.24,-.25,1.17,-.22,'#6b718b',g);
    if(seat.axis==='sense')s.box(.25,.08,.35,-.3,1.08,-.25,'#6b7e68',g);
    s.target(table,'seat:'+seat.id,seat.name+' · 가까이에서 E');
    g.traverse(m=>{if(m.isMesh&&m!==table){m.userData=table.userData;s.targets.push(m);}});
    const ring=new T.Mesh(new T.RingGeometry(.82,.91,48),new T.MeshBasicMaterial({color:seat.color,transparent:true,opacity:.7,side:T.DoubleSide}));ring.rotation.x=-Math.PI/2;ring.position.y=.11;g.add(ring);g.userData.ring=ring;s.seatGroups.push(g);
  }
  s.chef=s.person('#ded1af','human','chef');s.chef.position.set(CONTROL.spawn.x,0,CONTROL.spawn.z);s.scene.add(s.chef);
  s.plate=new T.Group();s.cylinder(.35,.33,.06,0,1,.55,'#e9dfbe',s.plate);s.sphere(.18,0,1.12,.55,'#b89555',s.plate);s.chef.add(s.plate);s.plate.visible=false;
  s.playerRing=new T.Mesh(new T.RingGeometry(.38,.46,48),new T.MeshBasicMaterial({color:'#f1d68f',side:T.DoubleSide,transparent:true,opacity:.85}));s.playerRing.rotation.x=-Math.PI/2;s.scene.add(s.playerRing);
  s.playerLabel=s.label('나 · WASD',CONTROL.spawn.x,2.35,CONTROL.spawn.z,'#ffe2a1',.65);
  s.destination=new T.Mesh(new T.RingGeometry(.48,.57,48),new T.MeshBasicMaterial({color:'#f0c57d',side:T.DoubleSide,transparent:true,opacity:.75}));s.destination.rotation.x=-Math.PI/2;s.destination.visible=false;s.scene.add(s.destination);
  s.guests=new Map();
  s.label('01 출입문',-7.1,2.8,-3.4,.75);s.label('03 벽 붙임 조리대',-1.8,2.85,-5.7,.95);s.label('04 화덕',-5,2.65,-5.65,.75);s.label('05 수첩',2,2.4,-5.7,.75);s.label('06 계단',6.55,2,-4.7,.8);
  // 💡 사연에서 받은 물건의 임시 형상. 게임 효과는 System의 해금 기록으로만 결정한다.
  s.keepsakeMeshes=new Map();
  for(const y of [1.2,1.95])s.box(2.15,.1,.4,4.1,y,-6.05,'#b19b77');
  const gifts=Object.values(REGULAR_STORIES).filter(st=>st.gift);
  gifts.forEach((st,i)=>{
    const g=new T.Group();g.position.set(3.32+(i%4)*.52,1.25+Math.floor(i/4)*.75,-6.03);s.scene.add(g);
    const kind=st.gift.kind;
    if(kind==='emblem'){const m=s.cylinder(.19,.19,.06,0,.22,0,'#d7b16d',g,6);m.rotation.x=Math.PI/2;s.box(.08,.22,.07,0,.22,.07,'#6c827a',g);}
    if(kind==='scroll'){const m=s.cylinder(.07,.07,.34,0,.12,0,'#e3d8b2',g);m.rotation.z=Math.PI/2;s.box(.05,.15,.17,0,.12,0,'#a5705d',g);}
    if(kind==='key'){const m=new T.Mesh(new T.TorusGeometry(.09,.025,6,12),s.mat('#d2b56b'));m.position.set(-.08,.13,.04);g.add(m);s.box(.23,.04,.05,.07,.13,.04,'#d2b56b',g);s.box(.04,.09,.05,.16,.09,.04,'#d2b56b',g);}
    if(kind==='firstaid'){s.box(.32,.24,.23,0,.14,0,'#d5c8a5',g);s.box(.05,.14,.02,0,.15,.13,'#729985',g);s.box(.15,.05,.02,0,.15,.13,'#729985',g);}
    if(kind==='prayer'||kind==='map'){s.box(.34,.32,.035,0,.18,0,'#e3d8b2',g);for(let n=0;n<3;n++)s.box(kind==='map'?.16:.22,.018,.018,(n%2)*.04,.09+n*.08,.025,kind==='map'?'#7e927e':'#9c8967',g);}
    if(kind==='ore'){const m=s.sphere(.17,0,.16,0,'#809b99',g);m.scale.set(1,.8,.8);s.sphere(.08,.12,.24,.03,'#c3b46f',g);}
    if(kind==='arrow'){s.box(.04,.29,.04,0,.17,0,'#9f8061',g);s.cylinder(0,.11,.16,0,.36,0,'#a7b7b2',g,3);}
    g.visible=false;s.keepsakeMeshes.set(st.id,g);
  });
  s.label('단골이 남긴 물건',4.1,2.75,-6.1,.75);
}
