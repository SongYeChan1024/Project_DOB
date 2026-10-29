// 💡 직접 조작을 검증하기 위한 배치와 수치. Unity 이식 시 데이터 에셋으로 옮긴다.
export const ROOM={width:16,depth:14,centerZ:.4,minX:-7.65,maxX:7.65,minZ:-6.25,maxZ:6.95};
export const CONTROL={speed:3.8,radius:.28,reach:1.35,maxFrameSeconds:.06,spawn:{x:-2,z:-3.6,yaw:0}};
export const STATIONS={
  door:{id:'door',name:'출입문',x:-7.05,z:-3.4},
  kitchen:{id:'kitchen',name:'벽 붙임 조리대',x:-1.8,z:-4.25},
  stove:{id:'stove',name:'화덕',x:-5,z:-4.25},
  book:{id:'book',name:'수첩',x:2,z:-4.35},
  stairs:{id:'stairs',name:'계단 앞',x:6.55,z:-1.8}
};
export const WAITING={x:-6.5,z:-2.1};
// 주방과 객석 사이. 가운데 및 양옆에 통로를 남긴다.
export const CURTAINS=[{x:-4.15,z:-2.3,width:3,depth:.16,height:1.9},{x:1.1,z:-2.3,width:3.8,depth:.16,height:1.9}];
export const FIXTURES=[
  {id:'kitchen',x:-1.8,z:-5.65,width:3.7,depth:1.5},
  {id:'stove',x:-5,z:-5.65,width:1.8,depth:1.5},
  {id:'book',x:2,z:-5.7,width:2,depth:1.35},
  {id:'stairs',x:6.55,z:-4.45,width:2.15,depth:4.15}
];
