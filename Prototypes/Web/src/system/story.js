import { SCENES } from "../define/data.js";
function enqueue(s, id, cause) {
  const st = s.story;
  if (st.done.includes(id) || st.queue.some((q) => q.id === id)) return;
  st.queue.push({ id, cause, order: st.processed.length + st.queue.length });
}
function evaluateStory(s, cause = "facts") {
  const has = (id) => s.story.clues.includes(id);
  if (has("mark")) enqueue(s, "PARTY_MARK_FOUND", cause);
  if (has("mark") && has("reply")) enqueue(s, "CURRENT_SURVIVAL_CONFIRMED", cause);
  if (has("reply") && has("cooking")) enqueue(s, "THEY_KEPT_COOKING", cause);
  if (["mark", "gap", "inner", "outer"].every(has)) enqueue(s, "ISOLATED_AREA_LOCATED", cause);
}
function acquireClue(s, id) {
  if (!s.story.clues.includes(id)) s.story.clues.push(id);
  evaluateStory(s, id);
}
function processReport(s, r) {
  const st = s.story;
  if (st.processed.includes(r.id)) return false;
  st.processed.push(r.id);
  const tier = r.result.tier;
  const first = { great: "FIRST_GREAT_SUCCESS", good: "FIRST_SUCCESS", narrow: "FIRST_NARROW_ESCAPE", hurt: "FIRST_INJURY" }[tier];
  if (!st.first[tier]) {
    st.first[tier] = r.id;
    if (!(tier === "narrow" && (st.done.includes("THEY_KEPT_COOKING") || st.afterReturn))) enqueue(s, first, r.id);
  }
  if (tier === "great" && st.injuries[r.guestId]) enqueue(s, "INJURED_GUEST_RETURNS_WELL", r.id);
  if (tier === "hurt") st.injuries[r.guestId] = r.id;
  if (r.result.surprise && r.result.absorbed && ["great", "good"].includes(tier)) enqueue(s, "RESERVE_HELPED_RETURN", r.id);
  if (!st.afterReturn && r.floor === 5) {
    if (!st.clues.includes("mark")) {
      acquireClue(s, "mark");
      acquireClue(s, "gap");
    }
    const req = st.requests[r.assignment];
    if (req && !req.done && req.sentDay <= r.day) {
      req.done = true;
      req.reportId = r.id;
      req.guestId = r.guestId;
      if (req.kind === "question") {
        for (const id of ["reply", "inner", "cooking"]) acquireClue(s, id);
      } else acquireClue(s, "outer");
    }
  }
  return true;
}
function requestInvestigation(s, kind) {
  const st = s.story;
  if (s.phase !== "prep" || st.afterReturn || !st.clues.includes("gap") || !["question", "survey"].includes(kind) || Object.values(st.requests).some((r) => r.kind === kind)) return false;
  const id = kind + "-" + s.day;
  st.requests[id] = { id, kind, sentDay: s.day, done: false };
  return true;
}
function nextScene(s) {
  if (s.story.sceneDay === s.day) return null;
  return s.story.queue.filter((q) => !SCENES[q.id].after || s.story.done.includes(SCENES[q.id].after)).sort((a, b) => SCENES[a.id].priority - SCENES[b.id].priority || a.order - b.order || a.id.localeCompare(b.id))[0] || null;
}
function finishScene(s, id) {
  const next = nextScene(s);
  if (!next || next.id !== id) return false;
  s.story.done.push(id);
  s.story.queue = s.story.queue.filter((q) => q.id !== id);
  s.story.sceneDay = s.day;
  if (id === "THEY_KEPT_COOKING") s.story.queue = s.story.queue.filter((q) => q.id !== "FIRST_NARROW_ESCAPE");
  return true;
}
function afterReturn(s) {
  s.story.afterReturn = true;
  s.story.queue = [];
  s.endingStep = "complete";
  s.phase = "prep";
  s.prepared = null;
}
export {
  acquireClue,
  afterReturn,
  enqueue,
  evaluateStory,
  finishScene,
  nextScene,
  processReport,
  requestInvestigation
};
