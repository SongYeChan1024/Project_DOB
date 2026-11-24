// [data-present]에서 연 챕터와 확대 상태를 관리한다. 자료와 순서는 DOB_CHAPTERS가 제공한다.
(() => {
  "use strict";

  const dialog = document.createElement("dialog");
  dialog.className = "dob-slides";
  dialog.setAttribute("aria-label", "기획 자료 발표");
  dialog.innerHTML = `
    <div class="dob-slides__frame">
      <div class="dob-slides__header">
        <span class="dob-slides__count" aria-hidden="true"></span>
        <h2 class="dob-slides__title" tabindex="-1"></h2>
        <button type="button" data-act="full">전체화면</button>
        <button type="button" data-act="close" aria-label="발표 닫기">닫기</button>
      </div>
      <div class="dob-slides__stage" role="region" aria-label="발표 자료">
        <img class="dob-slides__image" alt="">
        <p class="dob-slides__image-message" role="status" hidden></p>
      </div>
      <section class="dob-slides__notes" id="dob-slides-notes" aria-label="자료 설명" tabindex="0" hidden>
        <h3 class="dob-slides__question"></h3>
        <p class="dob-slides__summary"></p>
        <p class="dob-slides__status"></p>
      </section>
      <div class="dob-slides__footer">
        <div class="dob-slides__tools">
          <button type="button" data-act="zoom" aria-pressed="false">원본 크기</button>
          <button type="button" data-act="notes" aria-expanded="false" aria-controls="dob-slides-notes">자료 설명</button>
        </div>
        <div class="dob-slides__nav" role="group" aria-label="슬라이드 이동">
          <button type="button" data-act="prev"><span aria-hidden="true">←</span> 이전</button>
          <button type="button" data-act="next">다음 <span aria-hidden="true">→</span></button>
        </div>
      </div>
      <span class="dob-slides__announcement" role="status" aria-atomic="true"></span>
      <p class="dob-slides__notice" role="status" hidden></p>
    </div>`;

  const part = (name) => dialog.querySelector(".dob-slides__" + name);
  const control = (name) => dialog.querySelector('[data-act="' + name + '"]');
  const frame = part("frame");
  const title = part("title");
  const stage = part("stage");
  const image = part("image");
  const imageMessage = part("image-message");
  const notes = part("notes");
  const notice = part("notice");
  const zoom = control("zoom");
  const full = control("full");
  const prev = control("prev");
  const next = control("next");
  let chapters = [];
  let index = 0;
  let opener;

  function setZoom(expanded) {
    stage.classList.toggle("dob-slides__stage--zoom", expanded);
    stage.tabIndex = expanded ? 0 : -1;
    stage.scrollTo(0, 0);
    zoom.setAttribute("aria-pressed", String(expanded));
  }

  function setNotes(expanded) {
    notes.hidden = !expanded;
    control("notes").setAttribute("aria-expanded", String(expanded));
  }

  function render(position) {
    index = position;
    const chapter = chapters[index];
    const focused = document.activeElement;
    title.textContent = chapter.title;
    part("count").textContent = (index + 1) + " / " + chapters.length;
    part("announcement").textContent = "전체 " + chapters.length + "장 중 " + (index + 1) + "장, " + chapter.title;
    part("question").textContent = chapter.question;
    part("summary").textContent = chapter.summary;
    part("status").textContent = chapter.status;
    notes.scrollTop = 0;
    notice.hidden = true;
    setZoom(false);
    image.alt = chapter.alt;
    image.hidden = true;
    imageMessage.textContent = "자료를 불러오는 중입니다.";
    imageMessage.hidden = false;
    image.src = chapter.image;
    zoom.disabled = true;
    prev.disabled = index === 0;
    next.disabled = index === chapters.length - 1;
    if (focused === prev && prev.disabled && !next.disabled) next.focus();
    if (focused === next && next.disabled && !prev.disabled) prev.focus();
    if (focused === zoom || focused === stage) title.focus({ preventScroll: true });
  }

  function move(position) {
    if (position !== index && position >= 0 && position < chapters.length) render(position);
  }

  image.addEventListener("load", () => {
    image.hidden = false;
    imageMessage.hidden = true;
    zoom.disabled = false;
  });

  image.addEventListener("error", () => {
    setZoom(false);
    zoom.disabled = true;
    image.hidden = true;
    imageMessage.hidden = false;
    imageMessage.textContent = "자료 이미지를 열 수 없습니다. " + image.alt;
  });

  function syncFullscreen() {
    full.textContent = document.fullscreenElement === frame ? "전체화면 해제" : "전체화면";
  }

  async function toggleFullscreen() {
    notice.hidden = true;
    try {
      if (document.fullscreenElement === frame) {
        await document.exitFullscreen();
      } else {
        // dialog 자체는 전체화면 대상이 될 수 있어 내부 프레임을 사용한다.
        await frame.requestFullscreen();
        if (!dialog.open && document.fullscreenElement === frame) await document.exitFullscreen();
      }
    } catch {
      notice.textContent = "브라우저에서 전체화면을 전환할 수 없습니다.";
      notice.hidden = false;
    }
  }

  async function closeSlides() {
    // 전체화면이 포커스를 복원한 다음 대화상자를 닫아 시작 버튼에 초점을 돌려준다.
    if (document.fullscreenElement === frame) await document.exitFullscreen().catch(() => {});
    if (dialog.open) dialog.close();
  }

  document.addEventListener("fullscreenchange", syncFullscreen);
  dialog.addEventListener("click", (event) => {
    const button = event.target.closest("[data-act]");
    if (!button) return;
    switch (button.dataset.act) {
      case "prev": move(index - 1); break;
      case "next": move(index + 1); break;
      case "notes": setNotes(notes.hidden); break;
      case "zoom":
        setZoom(zoom.getAttribute("aria-pressed") === "false");
        if (stage.tabIndex === 0) stage.focus({ preventScroll: true });
        break;
      case "full": toggleFullscreen(); break;
      case "close": closeSlides(); break;
    }
  });

  dialog.addEventListener("keydown", (event) => {
    if (event.altKey || event.ctrlKey || event.metaKey) return;
    if (event.key === "Tab") {
      const stops = [...dialog.querySelectorAll('button:enabled, [tabindex="0"]')]
        .filter((el) => !el.hidden);
      if (event.shiftKey && (document.activeElement === stops[0] || document.activeElement === title)) {
        event.preventDefault();
        stops[stops.length - 1].focus();
      } else if (!event.shiftKey && document.activeElement === stops[stops.length - 1]) {
        event.preventDefault();
        stops[0].focus();
      }
      return;
    }
    // 확대 자료와 설명에 초점이 있으면 방향키로 내용 자체를 스크롤한다.
    const scrolling = event.target === notes || (event.target === stage && stage.tabIndex === 0);
    if (scrolling && event.key.startsWith("Arrow")) return;
    switch (event.key) {
      case "ArrowLeft": case "ArrowUp": move(index - 1); break;
      case "ArrowRight": case "ArrowDown": move(index + 1); break;
      case "Home": move(0); break;
      case "End": move(chapters.length - 1); break;
      case "Escape": closeSlides(); break;
      default: return;
    }
    event.preventDefault();
    event.stopPropagation();
  });

  dialog.addEventListener("cancel", (event) => {
    event.preventDefault();
    closeSlides();
  });

  dialog.addEventListener("close", () => {
    if (dialog.open) return;
    document.documentElement.classList.remove("dob-slides-open");
    if (opener?.isConnected) opener.focus({ preventScroll: true });
    opener = null;
  });

  document.addEventListener("click", (event) => {
    const trigger = event.target.closest("[data-present]");
    if (!trigger || dialog.open) return;
    chapters = window.DOB_CHAPTERS;
    if (!Array.isArray(chapters) || !chapters.length) return;
    event.preventDefault();
    opener = trigger;
    if (!dialog.isConnected) document.body.append(dialog);
    const id = trigger.dataset.present;
    const first = id ? chapters.findIndex((chapter) => String(chapter.id) === id) : 0;
    setNotes(false);
    full.hidden = !document.fullscreenEnabled || typeof frame.requestFullscreen !== "function";
    syncFullscreen();
    render(Math.max(0, first));
    dialog.showModal();
    document.documentElement.classList.add("dob-slides-open");
    title.focus({ preventScroll: true });
  });
})();
