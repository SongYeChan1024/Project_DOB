# 구현 기능과 총괄 Manager 구성안

작성자: 김언호 · 2026-11-24

상태: 💡 제안. 노션 GDD·작업 보드와 Git 기획 문서·`origin/main` 코드를 대조해 만들 기능과 총괄 Manager 구성을 정리한 초안이다.
Manager 신설·이름·범위는 팀 확정 전이며, 실제 생성은 백엔드 담당(김언호)과 정한다. 구현 완료나 실행 검증 결과가 아니다.

참고: [최신 결정](../기획/CurrentDecisions.md) · [메인 기획서 동기화본](../기획/MainPlanning.md) · [첫 플레이 범위](../기획/플레이/FirstPlayScope.md) · [개발 연결 지침](EngineeringGuide.md) · [씬 분리 설계](SceneComposition.md) · README 4·6장

> 노션 원본 링크(README 상단 GDD·「기획」)는 2026-11-24 조회 시 연결 계정에서 열리지 않았다. 같은 내용의 사본 워크스페이스(2026-11-20 수정)를 읽었으므로 원본과의 차이는 확인이 필요하다.

---

## 1. 현재 구현 상태 (origin/main, 2026-11-24)

| 영역 | 상태 | 관련 스크립트 |
| --- | --- | --- |
| 타이틀·로딩·씬 조합 로드 | 구현 | `TitlePresenter`, `UIManager`, `SceneLoadService`, `SceneComposition`, `BasementComposition` |
| 이동·E 상호작용·ESC 메뉴·커서 | 구현 | `PlayerMovementController`, `BasementPlayerInput`, `BasementPauseMenu`, `GameCursor` |
| 첫 손님 1명: 입장 → 줌인 응대 → 숨은 단서·수첩 메모 → 착석 → 구운 빵 조리 → 서빙 | 시험판(1명 전용) | `FirstGuestService`, `FirstGuestDialogue`, `FirstGuestKitchen`, `FirstGuestOrder`, `RestaurantSeating` |
| 일차 저장 슬롯 | 시험판 | `FirstServiceSaveData`, `FirstServiceSaveStore` |
| 식당 건물·가구·카엘 모델 | `art` 브랜치에만 있음 (main 미병합) | — |

- `GameManager`는 아직 없다. 현재 코드는 첫 손님 한 명용이므로 여러 손님·여러 날로 일반화가 필요하다.
- 노션 작업 보드의 백엔드 01~08, 아트 01~07 카드는 모두 대기 상태다.

## 2. 만들어야 할 기능

첫 플레이 범위(손님 3명 영업 → 마감 → 아침 귀환 보고 → 준비 → 재영업 시작)를 먼저 만든다. 세부 규칙은 참고 문서를 따른다.

| 번호 | 기능 | 핵심 규칙 | 첫 플레이 |
| --- | --- | --- | --- |
| F1 | 하루 진행 | 아침 보고 → 준비 → 영업(💡 약 180초) → 마감 → 다음 아침. 마지막 손님의 방명록 작성 완료 시 정산 1회 | 포함 |
| F2 | 시간·정지 | 영업 시계와 조리 시계 분리. 정지 사유는 겹쳐 쌓이며 하나를 닫아도 나머지 유지 | 포함 |
| F3 | 손님 생성 | 이름 풀 60개·종족 5·직업 6·피곤 30%·개인 취약점 30%(💡). guestId 고정, 방문마다 visitId | 첫날 3명은 고정 데이터 |
| F4 | 응대·단서 | 1인칭 초상·레이어·밑줄 대사, 수락·돌려보내기 | 포함 |
| F5 | 자리 처방 | 고정 소품 6종, 각 효과 +1 | 🔴 첫날 포함 여부 미정 |
| F6 | 조리 | 재료 투입·양념 선택 → 확정 → 6초 자동 조리, 💡 화구 2개. 접시는 지정 주문에만, 중복 확정·서빙 방지 | 포함 |
| F7 | 위험 판정 | 위험(층·이변·종족·컨디션·개인 취약점 − 직업) vs 효과(요리·양념·화덕·자리), 돌발·활력 보험 후 부족 합으로 등급. 미리보기와 최종 판정이 같은 순수 C# 계산기 사용 | 포함 |
| F8 | 경제 | 맛 점수 0~4, 금화 = 기본가 × (0.6 + 0.2 × 맛) + 양념 수. 기본 재료 무한, 양념·던전 재료만 재고. 시장·가게 개량 | 금화·재고 포함, 개량은 후속 |
| F9 | 귀환 보고·정산 | 결과 도장·축별 결과·전리품·새 레시피·수첩 기록(최대 2축)·층 개방·부상 휴식 | 포함 |
| F10 | 호감·단골·사연 | 호감 0~12, 3 단골 전환, 3·7·12 사연 해금(💡). 방명록·재방문 대사·재료 약속 | 방명록만 포함 |
| F11 | 수첩 | 던전·단골·사전 탭, Tab 키 | 포함 |
| F12 | 스토리 트리거 | 처리 이벤트 ID로 중복 방지, 한 아침에 장면 1개. 양피지 독백 | 첫 독백 1개 |
| F13 | 저장 | 일차 시작 스냅샷 자동·수동 저장, 불러오기. 손님·주문·보고·처리 ID·난수 상태 보존 | 포함 |
| F14 | 튜토리얼 | 안내 1명 + 병행 연습 2명, 💡 5~8분 | 포함 |
| F15 | 사운드 | BGM 9종, 필수 효과음(입장·조리 완성·서빙·금화·귀환 도장), 음량 설정 | 필수 효과음 |

## 3. 총괄 Manager 구성안 💡

기능마다 총괄 Manager 하나를 두고, 실제 상태 변경은 그 아래의 작은 System 스크립트가 맡는다 (README 6장: 상태 변경은 System, Manager는 조립·중재).
System 이름은 책임 설명용 예시이며 실제 클래스 이름은 구현할 때 맞춘다.

| Manager | 총괄 기능 | 아래 둘 System (예) | 현재 | 담당 |
| --- | --- | --- | --- | --- |
| GameManager | F1·F2 하루 단계 전환, Manager 연결·초기화 순서 | DayCycle, PauseReasons | 있음: 일차·시작 스냅샷·자동·수동 저장·정산을 `BasementComposition`에서 분리. 단계 전환·정지 사유는 미구현 | 김언호 |
| SceneLoadService · UIManager | 씬 조합·로딩·공용 화면 | SceneComposition | 있음 | 김언호 · 홍준우 |
| GuestManager | F3·F4 손님 생성·입장·좌석·응대·돌려보내기 | GuestGenerator, GuestRoster, RestaurantSeating(있음), VisitQueue | `FirstGuestService`가 1명 전용으로 대신 | 김언호 |
| KitchenManager | F6 주문·화구·조리·접시 소유·서빙 | OrderBook, CookingStation, PlateRegistry, TasteScore | `FirstGuestKitchen`·`FirstGuestOrder`가 1명 전용 | 김언호 |
| EconomyManager | F8 금화·재고·시장·가게 개량 | Wallet, Inventory, UpgradeState | 없음 (금화가 주방에 섞여 있음) | 김언호 |
| SettlementManager | F7·F9 판정·마감 정산·귀환 보고·전리품·층 개방 | RiskCalculator(순수 C#), ReturnResolver, FloorProgress | 없음 | 김언호 |
| RelationshipManager | F10·F11 호감·단골·사연·방명록·수첩 기록 | Affinity, RegularStory, NotebookRecords | 없음 (수첩 UI만 있음) | 김언호 |
| StoryManager | F12·F14 스토리 트리거·양피지 독백·튜토리얼 단계 | StoryTriggerSystem, TutorialSteps | 없음 | 김언호 |
| SaveManager | F13 스냅샷 저장·복구·처리 ID·난수 상태 | FirstServiceSaveStore 일반화 | 시험판 있음 | 김언호 (데이터 계약은 홍준우와 공동) |
| SoundManager | F15 BGM·효과음 재생·음량·씬 전환 시 중복 방지 | — | 없음 | 김언호 |

- 화면 쪽은 기능별 Presenter(응대·주방·보고·준비·수첩·일시 정지)가 Manager의 요청·결과를 잇는다. 담당은 홍준우.
- F5 자리 처방은 GuestManager(좌석 배정)와 SettlementManager(효과 합산)가 나눠 쓴다. 효과 스냅샷 시점은 🔴 검토 대기.

## 4. 정리가 필요한 불일치

| 항목 | 기준 차이 | 이 문서의 처리 |
| --- | --- | --- |
| GameManager 범위 | 노션 「프로그래밍」은 날짜·금화·하루 진행, 노션 보드 옛 카드는 날짜·골드·재료·상태 전환, README 6장은 금화 변경을 경제 System이 수행 | README 기준. 금화는 EconomyManager 아래 System이 소유 |
| 저장 기준 | 노션 사본 16장은 준비 완료·마감 시점 저장(가안), Git `CurrentDecisions.md`(11/23)는 일차 시작 스냅샷 확정 | Git 최신 결정 기준 |
| 음원 수급 담당 | 노션 사운드 페이지는 홍준우, README·작업 보드는 미정 | 🔴 팀 확인 필요 |
| 노션 보드 옛 카드 | V0.2 사냥 시절 카드(자동 공격·몬스터 AI·MenuResolver 등) 잔존 | 현재 기획과 무관. 정리 여부 팀 결정 |

🔴 남은 결정: 첫날 자리 처방 포함 여부, 준비 화면 씬 위치, 둘째·셋째 손님 데이터, 조리 도구 규칙.

노션의 아트·플레이 테스트 페이지와 첨부 명세 4개(core-spec, guest-camera-cooking, tutorial-spec, story-candidates)는 이 정리에 반영하지 않았다.

## 변경 기록

| 날짜 | 작성자 | 내용 |
| --- | --- | --- |
| 2026.11.24 | 김언호 | 노션 GDD·작업 보드와 Git 기획·코드를 대조해 구현 기능 F1~F15와 총괄 Manager 구성안 초안 작성 |
| 2026.11.24 | 김언호 | GameManager 구현 상태 반영. 나머지 Manager 8개는 기능 없는 파일만 생성 |
