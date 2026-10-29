export class PlayerInput{
  constructor(onInteract,canControl){
    this.keys=new Set();this.onInteract=onInteract;this.canControl=canControl;this.events=new AbortController();
    const options={signal:this.events.signal};
    this.codes=new Set(['KeyW','KeyA','KeyS','KeyD','ArrowUp','ArrowLeft','ArrowDown','ArrowRight']);
    document.addEventListener('keydown',e=>{
      if(!this.canControl()||e.ctrlKey||e.metaKey||e.altKey||e.target.closest('input,textarea,select,[contenteditable="true"]'))return;
      if(this.codes.has(e.code)){e.preventDefault();this.keys.add(e.code);}
      if(e.code==='KeyE'){e.preventDefault();if(!e.repeat)this.onInteract();}
    },options);
    document.addEventListener('keyup',e=>this.keys.delete(e.code),options);
    window.addEventListener('blur',()=>this.clear(),options);
    document.addEventListener('visibilitychange',()=>{if(document.hidden)this.clear();},options);
    document.querySelectorAll('[data-move]').forEach(b=>{
      b.addEventListener('pointerdown',e=>{if(!this.canControl())return;e.preventDefault();b.setPointerCapture(e.pointerId);this.keys.add(b.dataset.move);},options);
      for(const event of ['pointerup','pointercancel','lostpointercapture'])b.addEventListener(event,()=>this.keys.delete(b.dataset.move),options);
    });
  }
  clear(){this.keys.clear();}
  axes(){if(!this.canControl()){this.clear();return{h:0,v:0};}const has=(...keys)=>keys.some(k=>this.keys.has(k));return{h:Number(has('KeyD','ArrowRight'))-Number(has('KeyA','ArrowLeft')),v:Number(has('KeyS','ArrowDown'))-Number(has('KeyW','ArrowUp'))};}
  dispose(){this.events.abort();this.clear();}
}
