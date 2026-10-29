import source from './regulars.json' with {type:'json'};
export const REGULARS=source.regulars;
export const RELATION={min:0,max:12,thresholds:[3,7,12],resultGain:{great:2,good:1,narrow:0,hurt:0},tasteGoodAt:3,tasteGoodGain:1,tasteBadAt:0,tasteBadGain:-1,declineGain:-1,promiseAt:5,promiseChance:.7,promiseAmount:2,hintsAt:7,maxBonus:1,regularsPerDay:2,maxLevel:6,firstDay:['boris','amel','jwi'],trialDay:['boris','amel','sol'],trialAffinity:{boris:2,amel:6,sol:11},trialGold:240,trialStock:20,trialStove:2};
export const REGULAR_BY_ID=Object.fromEntries(REGULARS.map(r=>[r.id,r]));
export const REGULAR_STORIES=Object.fromEntries(REGULARS.flatMap(r=>r.stories.map(st=>[st.id,{...st,guestName:r.name,regularId:r.id}])));
