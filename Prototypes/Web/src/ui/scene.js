import { buildRoom } from './roomScene.js';
import { WAITING, CONTROL } from '../define/room.js';
import * as T from "three";
import {VISIT_STATUS,waitingSpot} from "../define/service.js";
import { SEATS } from "../define/data.js";
class RestaurantScene {
  constructor(container, onPick) {
    this.container = container;
    this.onPick = onPick;
    this.scene = new T.Scene();
    this.targets = [];
    this.angle = 0.72;
    this.zoom = 1;
    this.disposed = false;
    this.materials = /* @__PURE__ */ new Map();
    this.view = {};
    this.mouse = new T.Vector2();
    this.ray = new T.Raycaster();
    this.renderer = new T.WebGLRenderer({ antialias: true, alpha: true });
    this.renderer.setPixelRatio(Math.min(devicePixelRatio, 1.7));
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = T.PCFSoftShadowMap;
    this.renderer.outputColorSpace = T.SRGBColorSpace;
    this.renderer.toneMapping = T.ACESFilmicToneMapping;
    this.renderer.toneMappingExposure = 1.4;
    container.appendChild(this.renderer.domElement);
    this.camera = new T.OrthographicCamera(-9, 9, 9, -9, 0.1, 100);
    this.scene.add(new T.HemisphereLight("#c5e5e4", "#4b3b35", 2.1));
    const key = new T.DirectionalLight("#ffe0ad", 3.2);
    key.position.set(-3, 12, 6);
    key.castShadow = true;
    key.shadow.mapSize.set(2048, 2048);
    Object.assign(key.shadow.camera, { left: -13, right: 13, top: 13, bottom: -13 });
    key.shadow.bias = -8e-4;
    this.scene.add(key);
    const fill = new T.DirectionalLight("#84c5c6", 1.3);
    fill.position.set(5, 7, -7);
    this.scene.add(fill);
    this.build();
    this.bind();
    this.resizeObserver = new ResizeObserver(() => this.resize());
    this.resizeObserver.observe(container);
    this.resize();
    this.tick(0);
  }
  mat(color) {
    if (!this.materials.has(color)) this.materials.set(color, new T.MeshStandardMaterial({ color, roughness: 0.86, flatShading: true }));
    return this.materials.get(color);
  }
  box(w, h, d, x, y, z, color, parent = this.scene) {
    const m = new T.Mesh(new T.BoxGeometry(w, h, d), this.mat(color));
    m.position.set(x, y, z);
    m.castShadow = true;
    m.receiveShadow = true;
    parent.add(m);
    return m;
  }
  cylinder(top, bottom, h, x, y, z, color, parent = this.scene, sides = 12) {
    const m = new T.Mesh(new T.CylinderGeometry(top, bottom, h, sides), this.mat(color));
    m.position.set(x, y, z);
    m.castShadow = true;
    m.receiveShadow = true;
    parent.add(m);
    return m;
  }
  sphere(r, x, y, z, color, parent = this.scene) {
    const m = new T.Mesh(new T.SphereGeometry(r, 10, 7), this.mat(color));
    m.position.set(x, y, z);
    m.castShadow = true;
    parent.add(m);
    return m;
  }
  label(text, x, y, z, color = "#f2dfb8", scale = 1) {
    if (typeof color === "number") {
      scale = color;
      color = "#f2dfb8";
    }
    const c = document.createElement("canvas");
    c.width = 512;
    c.height = 100;
    const ctx = c.getContext("2d");
    ctx.font = '600 31px "Malgun Gothic",sans-serif';
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillStyle = "rgba(22,36,37,.85)";
    ctx.beginPath();
    ctx.roundRect(12, 14, 488, 72, 12);
    ctx.fill();
    ctx.fillStyle = color;
    ctx.fillText(text, 256, 50);
    const tex = new T.CanvasTexture(c);
    const m = new T.Sprite(new T.SpriteMaterial({ map: tex, depthTest: false, transparent: true }));
    m.scale.set(2.5 * scale, 0.49 * scale, 1);
    m.position.set(x, y, z);
    this.scene.add(m);
    return m;
  }
  target(mesh, id, name) {
    mesh.userData = { station: id, name };
    this.targets.push(mesh);
    return mesh;
  }
  build() { buildRoom(this); }
  person(color, race, job) {
    const g = new T.Group();
    this.cylinder(0.23, 0.34, 0.72, 0, 0.79, 0, color, g);
    this.sphere(0.25, 0, 1.4, 0, "#d7b08a", g);
    this.box(0.44, 0.12, 0.3, 0, 1.61, -0.07, "#524738", g);
    for (const x of [-0.13, 0.13]) {
      this.box(0.15, 0.42, 0.2, x, 0.22, 0, "#394748", g);
      this.box(0.18, 0.13, 0.32, x, 0.08, 0.08, "#353d35", g);
      this.sphere(0.024, x * 0.65, 1.43, 0.225, "#343c35", g);
    }
    this.box(0.12, 0.55, 0.13, -0.34, 0.84, 0.02, color, g);
    this.box(0.12, 0.55, 0.13, 0.34, 0.84, 0.02, color, g);
    if (job === "chef") {
      this.cylinder(0.29, 0.25, 0.25, 0, 1.72, 0, "#e9e3cc", g);
      this.box(0.34, 0.55, 0.07, 0, 0.7, 0.31, "#ded4b8", g);
    } else {
      this.box(0.35, 0.55, 0.2, 0, 0.92, -0.28, "#8d7759", g);
      this.box(0.1, 1, 0.12, 0.42, 0.78, -0.1, "#b2b8a6", g);
    }
    return g;
  }
  update(view) {
    this.view = view;
    const gifts=new Set((view.keepsakes||[]).map(g=>g.id));
    this.keepsakeMeshes.forEach((mesh,id)=>{mesh.visible=gifts.has(id);});
    this.seatGroups.forEach((g, i) => {
      const unlocked = view.seats?.includes(i);
      g.userData.ring.material.opacity = view.seat === i ? 0.95 : unlocked ? 0.25 : 0.08;
      g.scale.y = unlocked ? 1 : 0.72;
    });
    const active=new Set((view.guests||[]).map(g=>g.key));
    for(const [id,npc] of this.guests)if(!active.has(id)){
      const removed=new Set();npc.mesh.traverse(m=>{removed.add(m);m.geometry?.dispose();});
      this.targets=this.targets.filter(m=>!removed.has(m));this.scene.remove(npc.mesh,npc.label);
      npc.label.material.map.dispose();npc.label.material.dispose();this.guests.delete(id);
    }
    for(const entry of view.guests||[]){
      let npc=this.guests.get(entry.key);const spawn=entry.report?WAITING:waitingSpot(entry.slot);
      if(!npc){
        const mesh=this.person('#748caa',entry.data.race,entry.data.job);mesh.position.set(spawn.x,0,spawn.z);this.scene.add(mesh);
        this.target(mesh,entry.key,entry.data.name+' · 가까이에서 E');mesh.traverse(m=>{if(m.isMesh){m.userData=mesh.userData;this.targets.push(m);}});
        npc={mesh,label:this.label('',spawn.x,2.2,spawn.z,'#f1e4c5',.78),path:[],target:{...spawn}};
        this.guests.set(entry.key,npc);this.dressGuest(entry.data,npc);
      }
      const seated=entry.seat!==null&&entry.seat!==undefined;
      const pos=seated?{x:SEATS[entry.seat].x+1.02,z:SEATS[entry.seat].z}:spawn;
      if(npc.target.x!==pos.x||npc.target.z!==pos.z){
        if(seated){const lane=SEATS[entry.seat].z-1.7;npc.path=[{x:npc.mesh.position.x,z:npc.mesh.position.z-1},{x:-6.2,z:npc.mesh.position.z-1},{x:-6.2,z:lane},{x:pos.x,z:lane},{...pos}];}
        else npc.path=[{...pos}];npc.target={...pos};
      }
      npc.mesh.rotation.x=entry.data.tired?.12:0;
      npc.mesh.scale.setScalar(['dwarf','halfling'].includes(entry.data.race)?.83:1);
      const texture=npc.label.material.map,ctx=texture.image.getContext('2d');ctx.clearRect(0,0,512,100);
      ctx.fillStyle=entry.selected?'#68512b':'rgba(22,36,37,.92)';ctx.beginPath();ctx.roundRect(12,14,488,72,12);ctx.fill();
      ctx.fillStyle='#f1e4c5';ctx.font='600 27px "Malgun Gothic",sans-serif';ctx.textAlign='center';ctx.textBaseline='middle';
      ctx.fillText(entry.data.name+' · '+(entry.report?'귀환':VISIT_STATUS[entry.status]),256,50);texture.needsUpdate=true;
    }
    this.steam.visible=(view.guests||[]).some(g=>g.status==='cooking');
  }
  isGuestMoving(key){return !!this.guests.get(key)?.path.length;}

  setPlayer(player,moving,carrying){
    this.player=player||CONTROL.spawn;this.playerMoving=!!moving;
    this.chef.position.set(this.player.x,0,this.player.z);this.chef.rotation.y=this.player.yaw;
    this.plate.visible=!!carrying;
    this.playerRing.position.set(this.player.x,.12,this.player.z);
    this.playerLabel.position.set(this.player.x,2.35,this.player.z);
  }
  setDestination(target){this.destination.visible=!!target;if(target)this.destination.position.set(target.x,.12,target.z);}
  dressGuest(data,npc) {
    const key = data.race + ":" + data.job;
    if (npc.style === key) return;
    npc.style = key;
    const extras = npc.extras;
    if (extras) {
      const oldMeshes = /* @__PURE__ */ new Set();
      extras.traverse((m) => {
        oldMeshes.add(m);
        m.geometry?.dispose();
      });
      this.targets = this.targets.filter((m) => !oldMeshes.has(m));
      npc.mesh.remove(extras);
    }
    const g = new T.Group();
    npc.extras = g;
    npc.mesh.add(g);
    const colors = { warrior: "#8296a7", mage: "#a391b4", rogue: "#657c6a", priest: "#d5c6a1", miner: "#b89361", archer: "#7b9c88" };
    npc.mesh.children[0].material = this.mat(colors[data.job]);
    if (data.race === "elf") for (const x of [-0.28, 0.28]) {
      const ear = this.box(0.24, 0.12, 0.12, x, 1.47, 0, "#d7b08a", g);
      ear.rotation.z = x > 0 ? 0.5 : -0.5;
    }
    if (data.race === "beast") {
      for (const x of [-0.18, 0.18]) this.cylinder(0, 0.12, 0.28, x, 1.75, 0, "#927555", g, 3);
      const tail = this.cylinder(0.06, 0.1, 0.65, 0, 0.65, -0.55, "#927555", g);
      tail.rotation.x = 0.8;
    }
    if (data.race === "dwarf") this.box(0.37, 0.35, 0.17, 0, 1.22, 0.21, "#805a38", g);
    if (data.race === "halfling") for (const x of [-0.13, 0.13]) this.box(0.23, 0.13, 0.43, x, 0.08, 0.12, "#d7b08a", g);
    if (data.job === "warrior") {
      this.box(0.42, 0.48, 0.13, -0.44, 0.93, 0.12, "#8aabb4", g);
      this.box(0.3, 0.07, 0.08, -0.44, 0.93, 0.21, "#c7cdba", g);
    }
    if (data.job === "mage") {
      this.cylinder(0, 0.35, 0.62, 0, 1.92, 0, "#81749f", g);
      this.sphere(0.14, 0.46, 1.52, -0.1, "#8bbbbd", g);
    }
    if (data.job === "rogue") {
      this.box(0.44, 0.1, 0.16, 0, 1.34, 0.2, "#455e55", g);
      this.box(0.15, 0.4, 0.1, -0.42, 0.7, 0.15, "#c2c3a7", g);
    }
    if (data.job === "priest") {
      this.box(0.2, 0.6, 0.09, 0, 0.92, 0.3, "#e8d7a1", g);
      this.box(0.08, 0.32, 0.07, 0, 1, 0.37, "#ab8c55", g);
      this.box(0.23, 0.08, 0.07, 0, 1.04, 0.37, "#ab8c55", g);
    }
    if (data.job === "miner") {
      this.cylinder(0.29, 0.3, 0.16, 0, 1.67, 0, "#d0ad65", g);
      this.sphere(0.09, 0, 1.68, 0.27, "#f1d6a0", g);
      this.box(0.65, 0.09, 0.11, 0.42, 1.2, -0.1, "#a7b3aa", g);
    }
    if (data.job === "archer") {
      const bow = new T.Mesh(new T.TorusGeometry(0.43, 0.045, 5, 12, Math.PI), this.mat("#9b774e"));
      bow.position.set(-0.42, 1, 0.06);
      bow.rotation.z = -Math.PI / 2;
      g.add(bow);
      this.box(0.03, 0.86, 0.03, -0.42, 1, 0.06, "#d4c59a", g);
    }
    g.traverse((m) => {
      if (m.isMesh) {
        m.userData = npc.mesh.userData;
        this.targets.push(m);
      }
    });
  }
  reset() {
    this.angle = 0.72;
    this.zoom = 1;
    this.resize();
  }
  resize() {
    const { clientWidth: w, clientHeight: h } = this.container;
    if (!w || !h) return;
    this.renderer.setSize(w, h, false);
    const aspect = w / h;
    const size = Math.max(9.4,12.3/aspect) / this.zoom;
    this.camera.left = -size * aspect;
    this.camera.right = size * aspect;
    this.camera.top = size;
    this.camera.bottom = -size;
    this.camera.position.set(Math.sin(this.angle) * 22, 22, Math.cos(this.angle) * 22);
    this.camera.lookAt(0, 0, 0.4);
    this.camera.updateProjectionMatrix();
  }
  hit(e) {
    const b = this.renderer.domElement.getBoundingClientRect();
    this.mouse.set((e.clientX - b.left) / b.width * 2 - 1, -(e.clientY - b.top) / b.height * 2 + 1);
    this.ray.setFromCamera(this.mouse, this.camera);
    return this.ray.intersectObjects(this.targets, false).find((h) => h.object.visible && h.object.parent?.visible !== false)?.object.userData;
  }
  bind() {
    const c = this.renderer.domElement;
    let start = null, moved = false;
    c.addEventListener("pointerdown", (e) => {
      start = { x: e.clientX, y: e.clientY, a: this.angle };
      moved = false;
      c.setPointerCapture(e.pointerId);
    });
    c.addEventListener("pointermove", (e) => {
      if (start) {
        const dx = e.clientX - start.x;
        if (Math.abs(dx) > 5) moved = true;
        if (moved) {
          this.angle = Math.max(-0.35, Math.min(1.4, start.a - dx * 4e-3));
          this.resize();
        }
      } else {
        const hit = this.hit(e);
        c.style.cursor = hit ? "pointer" : "grab";
        const label = document.querySelector("#hover-label");
        label.hidden = !hit;
        if (hit) label.textContent = hit.name;
      }
    });
    c.addEventListener("pointerup", (e) => {
      if (start && !moved) {
        const hit = this.hit(e);
        if (hit) this.onPick(hit.station);
      }
      start = null;
    });
    c.addEventListener("pointercancel", () => {
      start = null;
    });
    c.addEventListener("wheel", (e) => {
      e.preventDefault();
      this.zoom = Math.max(0.75, Math.min(1.7, this.zoom - e.deltaY * 1e-3));
      this.resize();
    }, { passive: false });
  }
  tick(time) {
    if (this.disposed) return;
    const dt=Math.min(.06,Math.max(0,(time-(this.previousTime||time))/1000));this.previousTime=time;
    for(const npc of this.guests.values()){
      const target=npc.path[0];
      if(target){const dx=target.x-npc.mesh.position.x,dz=target.z-npc.mesh.position.z,d=Math.hypot(dx,dz),step=dt*3;if(d<=step){npc.mesh.position.x=target.x;npc.mesh.position.z=target.z;npc.path.shift();}else{npc.mesh.position.x+=dx/d*step;npc.mesh.position.z+=dz/d*step;npc.mesh.rotation.y=Math.atan2(dx,dz);}}
      npc.label.position.copy(npc.mesh.position).add(new T.Vector3(0,2.2,0));
    }
    if(this.playerMoving)this.chef.position.y=Math.sin(time*.018)*.045;
    else this.chef.position.y=0;
    this.fire.scale.setScalar(1 + Math.sin(time * 6e-3) * 0.08);
    if (this.steam.visible) this.steam.children.forEach((m, i) => {
      m.position.y = (time * 3e-4 + i * 0.22) % 1.1;
      m.material.opacity = 0.25 * (1 - m.position.y / 1.1);
    });
    this.renderer.render(this.scene, this.camera);
    this.frame = requestAnimationFrame((t) => this.tick(t));
  }
  dispose() {
    this.disposed = true;
    cancelAnimationFrame(this.frame);
    this.resizeObserver.disconnect();
    this.scene.traverse((m) => {
      m.geometry?.dispose();
    });
    for (const m of this.materials.values()) m.dispose();
    this.renderer.dispose();
  }
}
export {
  RestaurantScene
};
