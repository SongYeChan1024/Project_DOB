// DOB_FEATURES의 플레이 순서대로 장을 연다. 장 번호·입력·전체화면은 여기서 관리하고
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
  const dialogueGeneratorLink = document.getElementById("dialogue-generator-link");
  const choices = page.querySelector(".deck-choices");
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
  let suppressPreviewClick = false;

  if (!Array.isArray(features) || features.length === 0) {
    previous.disabled = true;
    next.disabled = true;
    notice.textContent = "발표 내용을 불러오지 못했습니다. 흐름도 자료실에서 기획 자료를 열 수 있습니다.";
    notice.hidden = false;
    return;
  }

  features.forEach((feature, i) => {
    const option = document.createElement("option");
    const stepTitle = feature.stage && feature.stage !== feature.title ? feature.stage + " · " + feature.title : feature.title;
    option.textContent = String(i + 1).padStart(2, "0") + " · " + stepTitle;
    option.value = String(i);
    picker.append(option);
  });

  const renderComparison = setupComparisons();

  // 등록된 장면 자료를 한 장 또는 비교 쌍으로 표시한다. 탭과 확대창은 각자 입력을 처리한다.
  function setupComparisons() {
    const section = page.querySelector(".deck-comparison");
    const pair = section.querySelector(".compare-pair");
    const tabs = [...section.querySelectorAll('[role="tab"]')];
    const template = document.getElementById("compare-figure");
    const narrow = window.matchMedia("(max-width: 760px)");
    const viewer = document.querySelector(".compare-viewer");
    const stage = viewer.querySelector(".compare-stage");
    const largeImage = stage.querySelector("img");
    const zoom = viewer.querySelector(".compare-zoom");
    const close = viewer.querySelector(".compare-close");
    let activeSide;
    let opener;

    function selectSide(side) {
      activeSide = side;
      for (const tab of tabs) {
        const selected = tab.dataset.side === side;
        tab.setAttribute("aria-selected", String(selected));
        tab.tabIndex = selected ? 0 : -1;
      }
      const tabbed = narrow.matches && pair.childElementCount === 2;
      for (const figure of pair.children) {
        figure.hidden = tabbed && figure.dataset.side !== side;
        if (tabbed) {
          figure.setAttribute("role", "tabpanel");
          figure.setAttribute("aria-labelledby", "compare-tab-" + figure.dataset.side);
        } else {
          figure.removeAttribute("role");
          figure.removeAttribute("aria-labelledby");
        }
      }
    }

    for (const tab of tabs) {
      tab.addEventListener("click", () => selectSide(tab.dataset.side));
      tab.addEventListener("keydown", (event) => {
        let side;
        switch (event.key) {
          case "ArrowLeft": case "ArrowRight": side = activeSide === "before" ? "after" : "before"; break;
          case "Home": side = "before"; break;
          case "End": side = "after"; break;
          default: return;
        }
        event.preventDefault();
        selectSide(side);
        tabs.find((item) => item.dataset.side === side).focus();
      });
    }
    narrow.addEventListener("change", () => {
      const focused = document.activeElement.closest(".compare-figure");
      selectSide(focused?.dataset.side || activeSide);
    });

    function setZoom(expanded) {
      stage.classList.toggle("is-zoomed", expanded);
      stage.tabIndex = expanded ? 0 : -1;
      stage.scrollTo(0, 0);
      zoom.setAttribute("aria-pressed", String(expanded));
      if (expanded) stage.focus();
    }
    pair.addEventListener("click", (event) => {
      const button = event.target.closest(".compare-open");
      if (!button || viewer.open || (suppressPreviewClick && event.detail > 0)) return;
      const figure = button.closest("figure");
      const img = button.querySelector("img");
      opener = button;
      viewer.querySelector("h2").textContent = features[index].title + " · " + figure.querySelector("strong").textContent;
      viewer.querySelector(".compare-caption").textContent = figure.querySelector("figcaption").textContent;
      largeImage.alt = img.alt;
      largeImage.src = img.src;
      setZoom(false);
      viewer.showModal();
      close.focus();
    });
    zoom.addEventListener("click", () => setZoom(!stage.classList.contains("is-zoomed")));
    function closeViewer() {
      viewer.close();
      if (opener?.isConnected) {
        if (narrow.matches) selectSide(opener.closest("figure").dataset.side);
        opener.focus({ preventScroll: true });
      }
      opener = null;
    }
    close.addEventListener("click", closeViewer);
    viewer.addEventListener("cancel", (event) => {
      event.preventDefault();
      closeViewer();
    });
    viewer.addEventListener("keydown", (event) => {
      if (event.key !== "Tab") return;
      const last = stage.tabIndex === 0 ? stage : close;
      if (event.shiftKey && document.activeElement === zoom) {
        event.preventDefault(); last.focus();
      } else if (!event.shiftKey && document.activeElement === last) {
        event.preventDefault(); zoom.focus();
      }
    });

    return function render(feature) {
      const entry = window.DOB_COMPARISONS?.[feature.key];
      const sceneImage = window.DOB_SCENES?.[feature.key];
      const paired = Boolean(entry?.before?.src && entry?.after?.src);
      const ready = paired || Boolean(sceneImage?.src);
      page.classList.toggle("has-comparison", ready);
      section.classList.toggle("compare-single", !paired);
      section.setAttribute("aria-label", paired ? "기획 시안과 구현 화면 비교" : (sceneImage?.label || "장면 자료"));
      section.hidden = !ready;
      pair.replaceChildren();
      if (!ready) return;
      const sides = paired ? ["before", "after"] : ["single"];
      for (const side of sides) {
        const item = paired ? entry[side] : sceneImage;
        const label = item.label || "장면 자료";
        const figure = template.content.firstElementChild.cloneNode(true);
        figure.id = "compare-" + side;
        figure.dataset.side = side;
        figure.querySelector(".compare-label span").textContent = paired ? (side === "before" ? "Before" : "After") : "";
        figure.querySelector("strong").textContent = label;
        const open = figure.querySelector(".compare-open");
        open.setAttribute("aria-label", label + " 원본 확대");
        const img = figure.querySelector("img");
        img.alt = item.alt || label;
        img.addEventListener("error", () => {
          open.disabled = true;
          open.querySelector(".compare-enlarge").textContent = "이미지를 열 수 없습니다";
        });
        img.src = item.src;
        figure.querySelector("figcaption").textContent = item.caption || "";
        if (paired) tabs.find((tab) => tab.dataset.side === side).textContent = label;
        pair.append(figure);
      }
      selectSide(paired ? "after" : "single");
    };
  }

  function finishTransition() {
    for (const animation of transitions) animation.cancel();
    transitions = [];
    outgoing?.remove();
    outgoing = null;
  }

  function show(newIndex, direction = 0) {
    const focused = document.activeElement;
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
    number.textContent = String(index + 1).padStart(2, "0") + (feature.stage ? " · " + feature.stage : "");
    title.textContent = cover ? feature.title.replace(": ", ":\n") : feature.title;
    lead.textContent = feature.lead;
    detail.textContent = feature.detail;
    detail.hidden = !feature.detail;
    choices.replaceChildren();
    for (const choice of feature.choices || []) {
      const target = features.findIndex((item) => item.key === choice.target);
      if (target < 0 || !choice.label) continue;
      const button = document.createElement("button");
      button.type = "button";
      button.textContent = choice.label;
      button.addEventListener("click", () => {
        show(target, Math.sign(target - index));
        title.focus({ preventScroll: true });
      });
      choices.append(button);
    }
    choices.hidden = !choices.childElementCount;
    position.textContent = String(index + 1).padStart(2, "0") + " / " + String(features.length).padStart(2, "0");
    picker.value = String(index);
    previous.disabled = cover;
    next.disabled = index === features.length - 1;
    if (focused === next && next.disabled && !previous.disabled) previous.focus();
    if (focused === previous && previous.disabled && !next.disabled) next.focus();
    const chapter = chapters.find((item) => item.id === feature.chapterId);
    related.hidden = !chapter;
    dialogueGeneratorLink.hidden = feature.key !== "clues" && feature.key !== "notebook";
    if (chapter) {
      related.dataset.present = chapter.id;
      related.setAttribute("aria-label", chapter.title + " 흐름도 보기");
    }
    if (art.getAttribute("src") !== feature.background) art.src = feature.background;
    renderComparison(feature);
    if (focused && (!focused.isConnected || focused.closest("[hidden]"))) title.focus({ preventScroll: true });
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
      (feature.stage || "") + ", " + feature.title;
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
    suppressPreviewClick = false;
    const preview = event.target.closest(".compare-open");
    if (!event.isPrimary || event.pointerType !== "touch" || (!preview && event.target.closest("button, a"))) {
      swipeStart = null;
      return;
    }
    swipeStart = { id: event.pointerId, x: event.clientX, y: event.clientY, preview };
    if (!preview) scene.setPointerCapture(event.pointerId);
  });
  scene.addEventListener("pointercancel", () => { swipeStart = null; });
  scene.addEventListener("pointerup", (event) => {
    if (!swipeStart || swipeStart.id !== event.pointerId) return;
    const dx = event.clientX - swipeStart.x;
    const dy = event.clientY - swipeStart.y;
    const horizontal = Math.abs(dx) >= 45 && Math.abs(dx) > Math.abs(dy) * 1.5;
    suppressPreviewClick = horizontal && Boolean(swipeStart.preview);
    swipeStart = null;
    if (horizontal) step(dx < 0 ? 1 : -1);
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
