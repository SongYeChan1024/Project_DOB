(() => {
  // The footer stays mounted as chapters change, so each gait keeps its own phase.
  const trail = document.querySelector(".adventure-trail");
  if (!trail) return;
  trail.setAttribute("aria-hidden", "true");
  for (const role of ["miner", "mage", "guard", "scout"]) {
    const walker = document.createElement("span");
    walker.className = "adventure-trail__walker adventure-trail__walker--" + role;
    trail.append(walker);
  }
})();
