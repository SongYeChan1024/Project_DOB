import {readFile,writeFile} from 'node:fs/promises';
import {transform} from 'esbuild';
for(const file of ['main.js','define/data.js','utils/runtime.js','system/rules.js','system/story.js','ui/scene.js','ui/panels.js']){
  const url=new URL('../src/'+file,import.meta.url);
  const text=await readFile(url,'utf8');const result=await transform(text,{loader:'js',format:'esm',target:'esnext',minify:false,charset:'utf8',legalComments:'inline'});
  await writeFile(url,result.code);
}
