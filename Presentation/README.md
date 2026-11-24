# 층계참 식당 기획·발표 사이트

[발표 사이트 열기](https://songyechan1024.github.io/Project_DOB/)

표지에서 오른쪽으로 넘기면 첫 손님을 만나는 순간부터 다음 영업을 준비할 때까지 한 장씩 이어집니다. 각 장의 번호는 플레이 순서입니다. 프로젝트의 로딩 일러스트가 각 장의 배경이 되고, 화면 아래에는 모험가들이 걸어갑니다.

## 로컬에서 열기

Node.js가 설치된 PC에서 저장소 루트를 열고 다음 명령을 실행합니다.

```sh
node Presentation/build.mjs
```

생성된 `Presentation/dist/index.html`을 브라우저로 엽니다. 오프라인 발표를 위해 다른 PC로 옮길 때에는 `dist` 폴더 전체를 복사합니다. ←·→ 키나 화면의 이동 버튼으로 장을 바꾸고, 휴대폰에서는 좌우로 밀어 이동합니다. 하단의 장 번호를 누르면 목록에서 원하는 기능으로 바로 갈 수 있습니다. **흐름도 자료실**에서는 기존 다섯 장의 그림을 확대해 볼 수 있습니다.

## 자료 갱신과 공개

발표 순서·제목·설명·배경은 [features.js](features.js)에서 편집합니다. 배경 네 장은 Unity의 [Loading 스프라이트 폴더](../Assets/_DOB/07.UI/Sprites/Loading/)에서 가져옵니다. 새 플레이 장면을 추가할 때는 [build.mjs](build.mjs)의 복사 목록에 이미지를 넣고 해당 장의 배경 경로를 갱신합니다. 자료실의 설명은 [content.js](content.js), 다섯 흐름도 이미지는 [기획 원본 폴더](../Docs/기획/플레이/Flowcharts/)에서 관리합니다.

[배포 워크플로](../.github/workflows/presentation-pages.yml)는 관련 변경이 `main`에 병합되면 사이트를 빌드하고 `Presentation/dist`를 GitHub Pages에 게시합니다.
