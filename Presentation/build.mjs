import { copyFile, mkdir, readdir, rm } from 'node:fs/promises';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import './comparisons.js';

const source = dirname(fileURLToPath(import.meta.url));
const repo = resolve(source, '..');
const output = join(source, 'dist');
const assets = join(output, 'assets');
const charts = join(repo, 'Docs', '기획', '플레이', 'Flowcharts');
const loadingArt = join(repo, 'Assets', '_DOB', '07.UI', 'Sprites', 'Loading');

const pages = [
  'index.html', 'content.js', 'slides.css', 'slides.js',
  'features.js', 'comparisons.js', 'deck.css', 'deck.js', 'trail.css', 'trail.js',
];
const figures = [
  '01-first-service.png',
  '02-guest-clues.png',
  '03-expedition-result.png',
  '04-guest-reactions.png',
  '05-game-overview.png',
];

const images = [
  ...Object.values(globalThis.DOB_COMPARISONS).flatMap(pair => [pair.before, pair.after]),
  ...Object.values(globalThis.DOB_SCENES),
];
// 같은 화면이 여러 장에 등장해도 파일은 한 번만 복사한다.
const comparisonImages = new Map(images.map(image => [image.src, image.source]));
const comparisonDir = join(assets, 'comparisons');
await mkdir(comparisonDir, { recursive: true });
await Promise.all([
  ...pages.map(name => copyFile(join(source, name), join(output, name))),
  ...Array.from(comparisonImages, ([destination, original]) => copyFile(join(repo, original), join(output, destination))),
  ...figures.map(name => copyFile(join(charts, name), join(assets, name))),
  ...['Title', 'Tutorial', 'Restaurant', 'Report'].map(scene =>
    copyFile(join(loadingArt, `T_UI_Loading${scene}.png`), join(assets, `loading-${scene.toLowerCase()}.png`))),
  copyFile(join(source, 'art', 'adventurers-walk.png'), join(assets, 'adventurers-walk.png')),
]);
// Remove retired files from earlier local builds before sharing the offline deck.
for (const file of await readdir(comparisonDir, { withFileTypes: true })) {
  if (file.isFile() && file.name.endsWith('.png') && !comparisonImages.has('assets/comparisons/' + file.name))
    await rm(join(comparisonDir, file.name));
}
await Promise.all(['styles.css', 'reader.js', 'assets/restaurant-layout.png'].map(name =>
  rm(join(output, name), { force: true })));
console.log('Built Presentation/dist/index.html');
