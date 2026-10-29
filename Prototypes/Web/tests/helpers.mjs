import * as R from '../src/system/rules.js';
import {D} from '../src/define/data.js';
export function loadMeal(s){const r=D.RECIPES.find(r=>r.id===R.currentVisit(s)?.recipe);for(const [id,n]of Object.entries(r?.ing||{}))for(let i=0;i<n;i++)R.addIngredient(s,id);}
