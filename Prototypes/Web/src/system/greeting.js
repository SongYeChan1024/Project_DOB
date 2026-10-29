import {AXES} from '../define/data.js';
import {OPENING_LINES} from '../define/service.js';
export function openingLine(g,h,visitId){
  const axis=AXES.filter(a=>h[a]>0).sort((a,b)=>h[b]-h[a])[0];
  const key=g.tired?'tired':axis||'well',lines=OPENING_LINES[key];
  const hash=[...g.id+visitId].reduce((n,c)=>n+c.charCodeAt(0),0);
  let index=hash%lines.length;if(lines[index]===g.lastOpening)index=(index+1)%lines.length;
  return {key,text:lines[index]};
}
