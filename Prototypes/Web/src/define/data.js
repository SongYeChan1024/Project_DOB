import source from "./reference.json" with { type: "json" };
const D = source;
const AXES = D.AXES;
const RACES = { human: { name: "인간", weak: null }, elf: { name: "엘프", weak: "fire" }, beast: { name: "수인", weak: "poison" }, dwarf: { name: "드워프", weak: "sense" }, halfling: { name: "하플링", weak: "vital" } };
const NAMES = ["아스트라", "노아", "리온", "이안", "세라", "루나", "에린", "테오", "미라", "로웬", "유리", "레나", "나루", "루엔", "하루", "엘린"];
const SEATS = [
  { id: 0, name: "약초 화분", axis: "poison", x: -4.5, z: 0.6, color: "#95bd89", cost: 0 },
  { id: 1, name: "쉬어 가는 의자", axis: "vital", x: 0, z: 0.6, color: "#e7b76e", cost: 0 },
  { id: 2, name: "난로 곁", axis: "cold", x: 4.5, z: 0.6, color: "#f2a97e", cost: 0 },
  { id: 3, name: "바람 드는 창가", axis: "fire", x: -4.5, z: 4.1, color: "#8fc6c1", cost: 45 },
  { id: 4, name: "어둑한 구석", axis: "dark", x: 0, z: 4.1, color: "#aca5dc", cost: 55 },
  { id: 5, name: "조용한 칸막이", axis: "sense", x: 4.5, z: 4.1, color: "#e0ce87", cost: 65 }
];
const CFG = { author: "송예찬", version: 1, startGold: 80, guestsPerDay: 3, cookMs: 2400, surpriseChance: 0.25, unlockWins: 2, authoredFloors: 5, injuryRest: 2, shelf: 2, stoveCosts: [60, 120], maxScenesPerMorning: 1, proposal: true };
const HINTS = { poison: "지난번에는 안개를 조금 마셨는데도 속이 뒤집혔어.", fire: "숨만 쉬어도 목이 타고, 장화 밑창이 뜨거워.", cold: "칼을 쥔 손가락이 얼어서 잘 움직이지 않았어.", dark: "횃불이 자꾸 꺼져. 앞이 보이지 않는 게 제일 걱정이야.", vital: "우회로가 길어졌대. 돌아올 때까지 다리가 버텨 줄까.", sense: "발밑에서 딸깍 소리가 났어. 한 걸음마다 신경이 쓰여." };
const CLUES = {
  mark: { name: "야영 표식", text: "낡은 조리도구에 새긴 우리 파티의 표식. 지금 살아 있다는 증거는 아니다." },
  gap: { name: "새 연락 틈", text: "작은 쪽지는 오갈 수 있다. 사람과 구조 장비가 통과할 길은 아니다." },
  reply: { name: "현재 질문의 답장", text: "이번에 보낸 질문에 대한 답과 동료들의 현재 상태가 담겼다. 💡 답장 원고·파티 구성 미정." },
  inner: { name: "피난처 내부 지도", text: "동료들이 머무는 구역과 안쪽 지형의 기록." },
  outer: { name: "외부 경로 조사", text: "정비로와 낙차, 접근 가능한 고정점을 관찰했다. 💡 지형·장비 미정." },
  cooking: { name: "이어 온 식사", text: "남겨 둔 조리 도구와 배운 손질법으로 서로의 식사를 마련해 왔다. 💡 재료·생존 기간 미정." }
};
const SCENES = {
  FIRST_GREAT_SUCCESS: { priority: 2, title: "다녀왔어", text: "그 말 한마디에 쥐고 있던 행주를 놓았다. 오늘은 돌아올 힘까지 남길 수 있었구나." },
  FIRST_SUCCESS: { priority: 2, title: "조금 모자랐던 것", text: "무사히 돌아왔지만 완벽한 준비는 아니었다. 손님의 이야기를 듣고 다음 식사의 빈칸을 적는다." },
  FIRST_NARROW_ESCAPE: { priority: 2, title: "돌아올 몫", text: "거기까지 갈 힘만 생각했었지. 손님의 빈 그릇을 보며, 돌아오는 길의 몫을 다시 생각한다." },
  FIRST_INJURY: { priority: 2, title: "먼저 앉아요", text: "처음 떠오른 것은 잘못된 처방이 아니라 손님의 상처였다. 물과 의자를 먼저 가져왔다." },
  RESERVE_HELPED_RETURN: { priority: 3, title: "남겨 둔 힘", text: "“예상 못 한 일이 있었지만, 남겨 둔 힘 덕분에 돌아왔어요.” 더 깊이 가는 것과 무사히 돌아오는 것은 다르다." },
  INJURED_GUEST_RETURNS_WELL: { priority: 3, title: "다시, 같은 자리", text: "전에 다쳤던 손님이 이번에는 편안하게 웃었다. 이름과 얼굴을 기억한 채 오늘의 그릇을 내어 준다." },
  PARTY_MARK_FOUND: { priority: 1, title: "낯익은 표식", text: "5층의 작은 틈 앞에서 찾았다는 조리도구. 내가 새긴 표식이다. 오래된 물건만으로는 알 수 없다. 지금의 질문을 보내야 한다." },
  CURRENT_SURVIVAL_CONFIRMED: { priority: 1, after: "PARTY_MARK_FOUND", title: "지금의 답장", text: "이번 질문에 대한 답이 돌아왔다. 동료들만 아는 대답과 각자의 현재 상태. 모두 아직 그곳에 있다." },
  THEY_KEPT_COOKING: { priority: 1, after: "CURRENT_SURVIVAL_CONFIRMED", title: "남겨 둔 것은", text: "“네가 알려 준 대로 손질해서 나눠 먹고 있어. 이번엔 돌아갈 몫도 같이 챙겨 와.” 내가 남긴 것이 실패뿐은 아니었다." },
  ISOLATED_AREA_LOCATED: { priority: 1, after: "THEY_KEPT_COOKING", title: "두 지도가 만나는 곳", text: "바깥의 조사와 안쪽의 지도가 5층 한 곳을 가리킨다. 연락 틈과 사람이 돌아올 길은 다르다. 경로와 장비까지 준비해야 한다." }
};
const TIERS = { great: "대성공", good: "성공", narrow: "간신히", hurt: "부상" };
const PREVIEW = { label: "💡 엔딩 A안 시험 설정", version: "preview-A-1", selfHazard: { dark: 2, poison: 1, vital: 2 }, reserve: 1, supplyCost: 12, endingPolicy: "return-and-reopen" };
export {
  AXES,
  CFG,
  CLUES,
  D,
  HINTS,
  NAMES,
  PREVIEW,
  RACES,
  SCENES,
  SEATS,
  TIERS
};
