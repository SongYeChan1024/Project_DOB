const clone = (value) => JSON.parse(JSON.stringify(value));
class Bus {
  constructor() {
    this.handlers = /* @__PURE__ */ new Map();
  }
  on(type, fn) {
    if (!this.handlers.has(type)) this.handlers.set(type, []);
    this.handlers.get(type).push(fn);
  }
  emit(type, payload) {
    for (const fn of this.handlers.get(type) || []) fn(payload);
  }
}
function random(s) {
  let t = s.seed = s.seed + 1831565813 >>> 0;
  t = Math.imul(t ^ t >>> 15, t | 1);
  t ^= t + Math.imul(t ^ t >>> 7, t | 61);
  return ((t ^ t >>> 14) >>> 0) / 4294967296;
}
const pick = (s, items) => items[Math.floor(random(s) * items.length)];
function save(s) {
  try {
    localStorage.setItem("dob-3d-v1-" + s.mode, JSON.stringify(s));
    return true;
  } catch {
    return false;
  }
}
function load(mode) {
  try {
    const s = JSON.parse(localStorage.getItem("dob-3d-v1-" + mode) || "null");
    return s?.version === 1 && s.mode === mode ? s : null;
  } catch {
    return null;
  }
}
const escapeHTML = (s) => String(s ?? "").replace(/[&<>"']/g, (c) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" })[c]);
export {
  Bus,
  clone,
  escapeHTML,
  load,
  pick,
  random,
  save
};
