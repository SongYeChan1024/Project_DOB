// DOB_FEATURES의 첫 항목은 표지다. 장 번호·입력·전체화면은 여기서 관리하고
// 자료실은 slides.js, 장 전환에 따른 모험가 표시는 trail.js에 맡긴다.
(() => {
  "use strict";

  const deck = document.querySelector(".deck");
  const scene = deck.querySelector(".deck-scene");
  const page = scene.querySelector(".deck-page");
  const reading = deck.querySelector(".deck-reading");
  const art = deck.querySelector(".deck-art");
  const title = document.getElementById("deck-title");
  const number = document.getElementById("deck-number");
  const lead = document.getElementById("deck-lead");
  const detail = document.getElementById("deck-detail");
  const related = document.getElementById("deck-related");
  const position = document.getElementById("deck-position");
  const picker = document.getElementById("deck-picker");
  const previous = document.getElementById("deck-prev");
  const next = document.getElementById("deck-next");
  const full = document.getElementById("deck-full");
  const notice = document.getElementById("deck-notice");
  const announcement = document.getElementById("deck-announcement");
  const features = window.DOB_FEATURES;
  const chapters = window.DOB_CHAPTERS || [];
  const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)");
  let index = 0;
  let swipeStart = null;
  let outgoing = null;
  let transitions = [];

  if (!Array.isArray(features) || features.length === 0) {
    previous.disabled = true;
    next.disabled = true;
    notice.textContent = "발표 내용을 불러오지 못했습니다. 흐름도 자료실에서 기획 자료를 열 수 있습니다.";
    notice.hidden = false;
    return;
  }

  features.forEach((feature, i) => {
    const option = document.createElement("option");
    option.textContent = (i === 0 ? "표지" : "기능 " + String(i).padStart(2, "0")) + " · " + feature.title;
    option.value = String(i);
    picker.append(option);
  });

  function finishTransition() {
    for (const animation of transitions) animation.cancel();
    transitions = [];
    outgoing?.remove();
    outgoing = null;
  }

  function show(newIndex, direction = 0) {
    const animate = newIndex !== index && !reducedMotion.matches && typeof page.animate === "function";
    finishTransition();
    if (animate) {
      // 나가는 면은 잠시 보이기만 한다. 폼·버튼·문서 ID는 현재 장에서만 사용한다.
      outgoing = page.cloneNode(true);
      outgoing.inert = true;
      outgoing.setAttribute("aria-hidden", "true");
      outgoing.querySelectorAll("[id]").forEach((node) => node.removeAttribute("id"));
      page.before(outgoing);
      outgoing.querySelector(".deck-reading").scrollTop = reading.scrollTop;
    }
    index = newIndex;
    const feature = features[index];
    const cover = index === 0;
    deck.toggleAttribute("data-cover", cover);
    page.toggleAttribute("data-cover", cover);
    number.textContent = cover ? "게임 기획 발표" : "기능 " + String(index).padStart(2, "0");
    title.textContent = cover ? feature.title.replace(": ", ":\n") : feature.title;
    lead.textContent = feature.lead;
    detail.textContent = feature.detail;
    detail.hidden = !feature.detail;
    position.textContent = String(index + 1).padStart(2, "0") + " / " + String(features.length).padStart(2, "0");
    picker.value = String(index);
    const focused = document.activeElement;
    previous.disabled = cover;
    next.disabled = index === features.length - 1;
    if (focused === next && next.disabled && !previous.disabled) previous.focus();
    if (focused === previous && previous.disabled && !next.disabled) next.focus();
    const chapter = chapters.find((item) => item.id === feature.chapterId);
    related.hidden = !chapter || cover;
    if (chapter) {
      related.dataset.present = chapter.id;
      related.setAttribute("aria-label", chapter.title + " 흐름도 보기");
    }
    if (art.getAttribute("src") !== feature.background) art.src = feature.background;
    notice.hidden = true;
    reading.scrollTop = 0;
    if (outgoing) {
      const timing = { duration: 300, easing: "cubic-bezier(.25,.65,.28,1)", fill: "both" };
      const moving = (offset) => ({ transform: "translateX(" + offset + "%)" });
      const leave = outgoing.animate([moving(0), moving(-direction * 100)], timing);
      const enter = page.animate([moving(direction * 100), moving(0)], timing);
      transitions = [leave, enter];
      enter.onfinish = () => { if (transitions.includes(enter)) finishTransition(); };
    }
    announcement.textContent = (index + 1) + " / " + features.length + ", " +
      (cover ? "표지" : number.textContent) + ", " + feature.title;
    document.dispatchEvent(new CustomEvent("dob:slidechange", {
      detail: { index, total: features.length, feature }
    }));
  }

  function step(direction) {
    const to = index + direction;
    if (to >= 0 && to < features.length) show(to, direction);
  }

  picker.addEventListener("change", () => {
    const to = picker.selectedIndex;
    if (to >= 0 && to !== index) show(to, Math.sign(to - index));
  });
  reducedMotion.addEventListener("change", () => { if (reducedMotion.matches) finishTransition(); });
  previous.addEventListener("click", () => step(-1));
  next.addEventListener("click", () => step(1));
  document.addEventListener("keydown", (event) => {
    if (event.defaultPrevented || event.altKey || event.ctrlKey || event.metaKey ||
        document.querySelector("dialog[open]") ||
        event.target.closest("input, textarea, select, [contenteditable=true]")) return;
    switch (event.key) {
      case "ArrowLeft": step(-1); break;
      case "ArrowRight": step(1); break;
      case "Home": show(0, -1); break;
      case "End": show(features.length - 1, 1); break;
      default: return;
    }
    event.preventDefault();
  });

  scene.addEventListener("pointerdown", (event) => {
    if (!event.isPrimary || event.pointerType !== "touch" || event.target.closest("button, a")) {
      swipeStart = null;
      return;
    }
    swipeStart = { id: event.pointerId, x: event.clientX, y: event.clientY };
    scene.setPointerCapture(event.pointerId);
  });
  scene.addEventListener("pointercancel", () => { swipeStart = null; });
  scene.addEventListener("pointerup", (event) => {
    if (!swipeStart || swipeStart.id !== event.pointerId) return;
    const dx = event.clientX - swipeStart.x;
    const dy = event.clientY - swipeStart.y;
    swipeStart = null;
    if (Math.abs(dx) >= 45 && Math.abs(dx) > Math.abs(dy) * 1.5) step(dx < 0 ? 1 : -1);
  });

  function syncFullscreen() {
    full.hidden = !document.fullscreenEnabled || typeof document.documentElement.requestFullscreen !== "function";
    full.querySelector("span").textContent = document.fullscreenElement ? "전체화면 해제" : "전체화면";
  }
  full.addEventListener("click", async () => {
    notice.hidden = true;
    try {
      if (document.fullscreenElement) await document.exitFullscreen();
      else await document.documentElement.requestFullscreen();
    } catch {
      notice.textContent = "전체화면을 열 수 없습니다. 이 창을 선택하고 다시 시도해 주세요.";
      notice.hidden = false;
    }
  });
  document.addEventListener("fullscreenchange", syncFullscreen);
  syncFullscreen();
  show(0);
  // 배경 네 장을 미리 읽어 다음 장이 움직일 때 원화도 곧바로 보이게 한다.
  for (const source of new Set(features.map((feature) => feature.background))) {
    if (source !== art.getAttribute("src")) new Image().src = source;
  }
})();
