import {D} from '../define/data.js';
import {TASTE,RACE_TASTES} from '../define/taste.js';
import {pick} from '../utils/runtime.js';
export function ensureTaste(s,g){
  if(!Array.isArray(g.likes)||!Array.isArray(g.dislikes)){
    const preferred=RACE_TASTES[g.race]||[];
    g.likes=preferred.length?[...preferred]:[];
    while(g.likes.length<2)g.likes.push(pick(s,TASTE.tags.filter(t=>!g.likes.includes(t))));
    g.dislikes=[pick(s,TASTE.tags.filter(t=>!g.likes.includes(t)))];
  }
  g.knownLikes??=[g.likes[0]];g.knownDislikes??=[];
}
export function dishTags(recipe,spices=[]){return [...new Set([...(recipe?.tags||[]),...spices.map(id=>D.SPICES[id]?.tag).filter(Boolean)])];}
export function tasteScore(guest,tags){
  const liked=guest.likes.filter(t=>tags.includes(t)),disliked=guest.dislikes.filter(t=>tags.includes(t));
  const score=Math.max(0,Math.min(4,TASTE.base+Math.min(TASTE.maxLikeBonus,liked.length)-(disliked.length?TASTE.dislikePenalty:0)));
  return{score,liked,disliked};
}
export function mealPrice(recipe,spices,taste){return Math.round(recipe.price*(TASTE.priceBase+TASTE.priceStep*taste.score))+spices.length;}
export function learnTaste(g,taste){
  const learned=[];
  if(taste.score>=3){const tag=taste.liked.find(t=>!g.knownLikes.includes(t));if(tag){g.knownLikes.push(tag);learned.push({kind:'like',tag});}}
  const tag=taste.disliked.find(t=>!g.knownDislikes.includes(t));if(tag){g.knownDislikes.push(tag);learned.push({kind:'dislike',tag});}
  return learned;
}
