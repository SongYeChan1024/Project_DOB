# Unity 프로젝트 기반 구성 기록

작성자: 송예찬 · 2026-11-01

사용자 요청: Unity 프로젝트와 AD(Assembly Definition으로 해석), 씬 분리·씬 로드, 팀원별 우선순위 할 일 구성.
게임 규칙·판정·저장·식당 플레이·사운드 시스템은 아직 구현하지 않는다.
현재 씬 구성은 [씬 분리와 조합 로드 설계](SceneComposition.md)의 일곱 씬을 따른다. 아래 초기 세 씬 설계·검증은 기반을 처음 만든 당시 기록이다.

## 초기 세 씬의 구현 전 의사코드 — 이전 구성 기록

### 씬 이동 서비스 — Manager

1. 이동할 씬 이름을 받는다. 비어 있거나 Build Settings에 없는 씬이면 오류를 알리고 이동하지 않는다.
2. 이미 로드 중이면 반복 요청을 무시한다.
3. 전환 전용 오브젝트를 만들고 씬 전환 중 유지한다.
4. LoadSceneAsync의 Single 모드로 대상 씬을 연다.
5. 완료 뒤 잠금을 풀고 전환 전용 오브젝트를 정리한다.
6. 서비스에는 금화·손님·주문·날짜를 보관하지 않는다. 이후 실제 게임 상태는 별도 세션에서 관리한다.

### 화면 이동 Presenter — Presenter

1. 버튼 입력으로 지정된 대상 씬 이름을 읽는다.
2. Manager의 씬 이동 서비스에 요청한다.
3. 게임 상태·재고·귀환 정산은 계산하지 않는다.

### 에디터 초기 구성 — Editor 전용

1. 프로젝트 식별·직렬화·Visible Meta Files·New Input System 설정을 맞춘다.
2. 세 씬을 만들고 카메라·기본 조명·Canvas·EventSystem을 연결한다.
3. 타이틀은 식당으로, 식당은 귀환 보고로, 귀환 보고는 식당 또는 타이틀로 이동할 버튼을 연결한다.
4. 식당에는 손님 입구·응대 위치·좌석 3개·주방·계단의 빈 기준점을 둔다.
5. Build Settings에 Title → Restaurant → ReturnReport를 등록하고 씬을 저장한다.
6. 배치 검증에서 세 씬의 버튼 연결·빌드 등록·URP·입력 설정·계층 참조를 확인한다.

## 초기 씬과 책임 — 현재 구성은 별도 설계 참조

| 씬 | 역할 | 담당·연결 |
| --- | --- | --- |
| Title | 시작과 식당 씬 로드 | 홍준우 UI, 김언호 로드 연결 |
| Restaurant | 준비·영업 공간. 두 단계는 같은 씬의 상태·화면 전환 | 김언호 통합, 홍준우 화면, 이기혁 배치·프리팹 |
| ReturnReport | 다음 아침 결과 표시·식당 복귀 | 홍준우 UI, 김언호 정산·세션 연결 |

현재 버튼은 씬 이동을 확인하는 초기 골격이다. 식당 → 보고 이동이 실제 영업 종료·보상 정산 구현을 뜻하지 않는다.
최초 세팅 씬 작성은 사용자 요청에 따른 공용 기반 구성이다. 이후 편집·인계는 README 담당 규칙을 따른다.
이후 승인에 따라 Loading·Gameplay·Tutorial·CharacterTest를 추가하고 Restaurant의 공간과 실행 구성을 분리했다. 현재 역할·조합·검증 결과는 [씬 분리 설계](SceneComposition.md)를 우선한다.

## 계층 참조

| 어셈블리 | 참조 |
| --- | --- |
| DOB.Define | 없음 |
| DOB.Utils | DOB.Define |
| DOB.Systems | DOB.Define·DOB.Utils |
| DOB.UI | DOB.Define·DOB.Utils·Unity.ugui·Unity.InputSystem |
| DOB.Manager | DOB.Define·DOB.Utils·DOB.Systems·DOB.UI·Unity.ugui |
| DOB.Editor (에디터 전용) | 위 런타임 어셈블리·Unity.ugui·Unity.InputSystem |

Presenter 폴더는 asmref로 DOB.Manager에 포함한다. UI와 System은 서로 직접 참조하지 않는다.
프레임워크 클래스는 준비 골격이며 게임 기능별 구현은 팀 할 일로 이어간다.

## 초기 기반 검증 기록 — 당시 세 씬 기준

초기 생성: Unity 6000.4.0f1 URP 템플릿 배치 실행 정상 종료 확인.
URP 17.4.0·Input System 1.19.0, Force Text·Visible Meta Files·새 입력 시스템 설정 확인.
구성 후 배치 검증: 3개 씬·카메라·InputSystem UI 모듈·직렬화된 버튼 연결 검사 통과.
PlayMode 자동 검사 1개 통과: 실제 버튼으로 Title → Restaurant → ReturnReport → Restaurant → Title 왕복, 같은 프레임 반복 입력 시 로더 1개 유지·완료 후 정리 확인.
Windows 개발 빌드 성공: Build/Smoke/DOB.exe. 빌드 출력은 Git에 올리지 않는다.
PF_Player·PF_Guest·PF_Enemy 골격 생성 완료. [모델 연결 규격](CharacterPrefabStructure.md)을 따르며 실제 기능 완료와 구분한다.

## 작업 보드와 다음 단계

[노션 일정·작업 관리 — 분류별 보드](https://app.notion.com/p/21b2d924de9a412fad35ff3eb12d9ea0?v=3fff7d6373c381f68825000c52b0dd47)에 제작 작업 36개를 반영했다.
기획·프론트엔드·백엔드·아트·공통의 다섯 분류에서 1순위부터 진행한다. 일차·날짜로 구현 순서를 나누지 않는다.
카드 메모에 담당자·선행 작업·완료 기준을 적었으며, 이전 사냥안 작업은 원본 데이터에서 보존하고 현재 보드에서는 제외한다.
기획 데이터와 개발자 간 요청·결과 계약을 먼저 맞춘 뒤, 백엔드 실제 로직·프론트엔드 더미 화면·바르코3D 아트 제작은 병렬 진행한다.
사운드는 백엔드의 BGM·효과음 재생 시스템, 프론트엔드의 요청·음량 설정 연결, 공통의 음원 목록·수급·라이선스로 나눈다. 시스템은 김언호가 담당하며 음원 수급 담당은 미정이다.
`Assets/_DOB/06.Audio/BGM`·`SFX`는 폴더 골격만 존재한다. 사운드 시스템 구현·검증 결과는 위 초기 구조 검증 기록에 포함되지 않는다.

## 사용한 공식 참고

- [Unity Editor CLI 프로젝트 생성](https://docs.unity.com/en-us/engine/6000.0/manual/unity-editor/command-line-arguments/editor): 설치된 URP 템플릿으로 임시 프로젝트를 생성한 뒤 기존 저장소 파일을 보존하며 Unity 프로젝트 루트를 추가했다.
