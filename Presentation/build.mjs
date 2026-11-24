import { copyFile, mkdir, rm } from 'node:fs/promises';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const source = dirname(fileURLToPath(import.meta.url));
const repo = resolve(source, '..');
const output = join(source, 'dist');
const assets = join(output, 'assets');
const charts = join(repo, 'Docs', '기획', '플레이', 'Flowcharts');
const loadingArt = join(repo, 'Assets', '_DOB', '07.UI', 'Sprites', 'Loading');

const pages = [
  'index.html', 'content.js', 'slides.css', 'slides.js',
  'features.js', 'deck.css', 'deck.js', 'trail.css', 'trail.js',
];
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
  ...['Title', 'Tutorial', 'Restaurant', 'Report'].map(scene =>
    copyFile(join(loadingArt, `T_UI_Loading${scene}.png`), join(assets, `loading-${scene.toLowerCase()}.png`))),
  copyFile(join(source, 'art', 'adventurers-walk.png'), join(assets, 'adventurers-walk.png')),
]);
// Remove retired files from earlier local builds before sharing the offline deck.
await Promise.all(['styles.css', 'reader.js', 'assets/restaurant-layout.png'].map(name =>
  rm(join(output, name), { force: true })));
console.log('Built Presentation/dist/index.html');
