// 💡 동시 영업·조리 도구 배정·대사는 시제품 제안. 최종 규격은 팀 결정.
export const SERVICE={version:4,burners:1,maxGuests:6};
export const ORIGINAL_ECONOMY={gold:50,stock:{flour:4,potato:3,meat:2,garlic:2,salt:2},discovered:{flour:true,potato:true,meat:true},seatCosts:[70,130,200],stoveCosts:[90,180],shelfCost:120,baseShelf:1,maxShelf:2};
export const FEEL_LINES=['몸이 한결 가볍다. 오늘은 잘 풀릴 것 같아.','든든하긴 한데… 뭔가 하나 모자란 느낌이야.','음… 조금 불안하지만 일단 가볼게.','이걸로 괜찮을까. 솔직히 자신은 없는데.'];
export const HINT_WORDS={poison:['안개','독기','속'],fire:['열기','뜨거운','열이','목이'],cold:['손끝','한기','서리'],dark:['횃불','어둠','캄캄'],vital:['돌아오는 길','힘','먼 길','허기'],sense:['딸깍','함정','낌새','길바닥'],tired:['다리가','기운','지쳐','숨'],well:['몸은 가뿐해','발걸음','힘']};
export const VISIT_STATUS={waiting:'주문 준비',cooking:'조리 중',ready:'완성 · 수령 대기',carrying:'배달 중',served:'배웅 대기',departed:'출발',declined:'귀가'};
export const isActiveVisit=v=>!['departed','declined'].includes(v.status);
export const waitingSpot=slot=>({x:-6.65,z:-1.8+slot*1.5});
export const COOKWARE={
  oven:{name:'장작 돌화덕',verb:'굽기',tip:'달군 돌판과 장작불로 천천히 굽습니다.'},
  pot:{name:'무쇠 솥',verb:'끓이기',tip:'삼발이에 건 무쇠 솥에 재료를 넣고 끓입니다.'},
  pan:{name:'무쇠 팬',verb:'익히기',tip:'장작 화로 위 무쇠 팬에서 고르게 익힙니다.'},
  bowl:{name:'나무 조리 볼',verb:'섞기',tip:'나무 볼에 재료를 담고 주걱으로 섞습니다.'}
};
export const RECIPE_TOOL={bread:'oven',herbpie:'oven',carrotcake:'oven',moldgratin:'oven',stew:'pot',shroomsoup:'pot',shrimpstew:'pot',inkrisotto:'pot',abyss:'pot',grill:'pan',batfry:'pan',pepperfry:'pan',tailgrill:'pan',vsaltpotato:'pan',jellysalad:'bowl',mosssoup:'bowl',mossmuch:'bowl',parfait:'bowl',bloomsalad:'bowl'};
export const OPENING_LINES={
  tired:['계단을 오르는데 다리가 후들거렸어. 돌아오는 길까지 버틸 만한 걸 먹고 싶어.','배낭이 평소보다 무겁게 느껴져. 아직 갈 길이 남았는데 기운이 없어.','숨 좀 돌려도 될까. 오늘은 조금만 걸어도 지쳐 버리네.'],
  poison:['지난번 안개를 마신 뒤로 속이 좋지 않았어. 이번 길은 미리 대비하고 싶어.','축축한 숲으로 가야 해. 독기 때문에 또 속을 버릴까 걱정이야.','목에 남은 안개 냄새가 잊히질 않아. 속을 지켜 줄 한 끼가 있을까.'],
  fire:['목이 자꾸 말라. 아래쪽 열기를 견딜 준비를 하고 싶어.','뜨거운 바닥을 다시 지나야 해. 벌써 얼굴에 열이 오르는 기분이야.','한 걸음마다 열기가 올라오던 길이었어. 이번엔 대비하고 내려가려고.'],
  cold:['손끝이 시려서 장비를 놓칠 뻔했어. 몸을 덥힐 한 끼가 필요해.','옷을 껴입어도 한기가 파고들더라. 속부터 따뜻했으면 해.','아래에 서리가 더 짙어졌대. 다시 손이 얼까 봐 걱정이야.'],
  dark:['횃불이 꺼진 뒤로 어둠이 자꾸 신경 쓰여. 앞을 놓치고 싶지 않아.','아래로 내려가면 눈앞이 캄캄해져. 길을 잘 살필 준비가 필요해.','희미한 불빛만 믿고 가려니 불안해. 어둠에 대비할 수 있을까.'],
  vital:['갈 때는 괜찮은데 돌아오는 길이 길어. 끝까지 걸을 힘을 남기고 싶어.','오늘은 먼 길을 가야 해. 허기진 채 돌아오고 싶지는 않아.','아직 발걸음은 괜찮아. 그래도 귀환할 몫까지 든든히 먹어 두려고.'],
  sense:['발밑에서 딸깍 소리가 난 뒤로 긴장이 풀리질 않아. 감각을 깨워 두고 싶어.','길바닥이 수상해서 신경이 곤두서 있어. 작은 낌새도 놓치기 싫어.','오늘 갈 길에는 함정이 많대. 한눈팔지 않도록 준비하고 싶어.'],
  well:['몸은 가뿐해. 돌아올 때도 이랬으면 좋겠어. 한 끼 부탁할게.','오늘은 숨도 고르고 발걸음도 괜찮아. 든든히 챙겨 먹고 출발하려고.','아직 힘은 충분해. 그렇다고 돌아오는 길의 몫을 빼놓을 순 없지.']
};
