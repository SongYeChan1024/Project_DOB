(() => {
  const chapters = window.DOB_CHAPTERS;
  const nav = document.querySelector(".chapter-nav");
  const panel = document.querySelector(".chapter-panel");
  const title = panel.querySelector(".chapter-title");
  const status = panel.querySelector(".chapter-status");
  const question = panel.querySelector(".chapter-question");
  const summary = panel.querySelector(".chapter-summary");
  const original = panel.querySelector(".chapter-original");
  const figureLink = panel.querySelector(".chapter-figure a");
  const image = panel.querySelector(".chapter-image");
  const present = panel.querySelector("[data-present]");

  function selectChapter(chapter) {
    for (const button of nav.children) {
      const selected = button.dataset.chapter === chapter.id;
      button.setAttribute("aria-pressed", String(selected));
      button.classList.toggle("is-active", selected);
    }

    title.textContent = chapter.title;
    status.textContent = chapter.status;
    question.textContent = chapter.question;
    summary.textContent = chapter.summary;
    image.alt = chapter.alt;
    image.src = chapter.image;
    original.href = chapter.image;
    original.setAttribute("aria-label", chapter.title + " 흐름도 원본 확대 (새 탭)");
    figureLink.href = chapter.image;
    figureLink.title = "원본 확대 (새 탭)";
    present.dataset.present = chapter.id;
    panel.hidden = false;
  }

  for (const chapter of chapters) {
    const button = document.createElement("button");
    button.type = "button";
    button.className = "chapter-tab";
    button.dataset.chapter = chapter.id;
    button.setAttribute("aria-controls", panel.id);
    button.textContent = chapter.title;
    button.addEventListener("click", () => selectChapter(chapter));
    nav.append(button);
  }
  selectChapter(chapters[0]);
})();
