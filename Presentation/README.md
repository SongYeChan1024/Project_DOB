# 층계참 식당 기획·발표 사이트

팀원은 **기획 읽기**에서 궁금한 장을 고르고 그림을 눌러 확대할 수 있습니다. 회의를 진행할 때는 **발표 시작**을 누릅니다. 전체 흐름에서 시작해 첫 영업, 단서 추리, 탐험 판정, 손님의 반응 순으로 이어집니다. 특정 규칙을 설명하려면 **이 장에서 발표**로 시작합니다.

## 로컬에서 열기

Node.js가 설치된 PC에서 저장소 루트를 열고 다음 명령을 실행합니다.

```sh
node Presentation/build.mjs
```

생성된 `Presentation/dist/index.html`을 브라우저로 엽니다. 오프라인 발표를 위해 다른 PC로 옮길 때에는 `dist` 폴더 전체를 복사합니다. 발표 중에는 ←·→로 장을 바꾸고 Esc로 읽기 화면에 복귀합니다. **자료 설명**에서 각 장의 질문과 설명을 확인할 수 있습니다.

## 자료 갱신과 공개

두 화면은 [content.js](content.js)의 장 순서·설명·이미지를 함께 사용합니다. 기존 흐름도는 [기획 원본 폴더](../Docs/기획/플레이/Flowcharts/)에서 수정합니다. 게임 완성 후 실제 플레이 장면을 추가하려면 원본 이미지를 저장하고 [build.mjs](build.mjs)의 복사 목록과 `content.js`의 장 정보를 갱신한 다음 다시 빌드합니다.

공개하기로 결정하면 GitHub 저장소의 **Settings → Pages → Source**에서 **GitHub Actions**를 선택합니다. [배포 워크플로](../.github/workflows/presentation-pages.yml)는 관련 변경이 `main`에 병합되면 사이트를 빌드하고 `Presentation/dist`를 게시합니다. 최초 공개는 **Actions → 기획 발표 사이트 → Run workflow**에서 `main`을 선택해 실행할 수도 있습니다.
