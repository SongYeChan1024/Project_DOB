import { copyFile, mkdir } from 'node:fs/promises';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const source = dirname(fileURLToPath(import.meta.url));
const repo = resolve(source, '..');
const output = join(source, 'dist');
const assets = join(output, 'assets');
const charts = join(repo, 'Docs', '기획', '플레이', 'Flowcharts');
const visuals = join(repo, 'Docs', 'References', '3D');

const pages = ['index.html', 'styles.css', 'content.js', 'reader.js', 'slides.css', 'slides.js'];
const figures = [
  '01-first-service.png',
  '02-guest-clues.png',
  '03-expedition-result.png',
  '04-guest-reactions.png',
  '05-game-overview.png',
];

await mkdir(assets, { recursive: true });
await Promise.all([
  ...pages.map(name => copyFile(join(source, name), join(output, name))),
  ...figures.map(name => copyFile(join(charts, name), join(assets, name))),
  copyFile(join(visuals, 'restaurant-layout.png'), join(assets, 'restaurant-layout.png')),
]);
console.log('Built Presentation/dist/index.html');
