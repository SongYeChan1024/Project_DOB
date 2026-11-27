# Loading 페이드 테스트 버그 리포트 초안

작성자 이기혁 · 검증일 2026-11-27 · 팀 등록 전 검토용

노션 「버그 리포트」에 새 항목을 만들지 못해 로컬 초안으로 남깁니다. 워크스페이스가 노션 무료 플랜의 블록 한도에 도달했기 때문입니다. 아래 절의 제목과 본문을 노션에 옮길 수 있습니다. 노션 「모든 이슈」의 기존 4건에는 같은 증상이 없었습니다(2026-11-27 확인). [등록 안내](../../../README.md#버그-리포트) · [발견한 PR #60](https://github.com/SongYeChan1024/Project_DOB/pull/60)

## [UI] Loading 페이드 PlayMode 테스트가 실행마다 통과·실패가 갈림

**등급** 사소. 게임 화면의 문제는 확인하지 않았습니다. 관계없는 PR의 PlayMode 결과에 실패가 섞여 리뷰 판단이 흐려집니다.

**환경** Windows, Unity `6000.4.0f1` 에디터 PlayMode. `main` `539b0d1`(#59 병합)과 art `a7cd051`(`539b0d1` + PR #60의 식당 벽 커밋)에서 실행했습니다. PR #60은 Loading·Title 관련 파일을 바꾸지 않았습니다.

**전제** 테스트 경로는 Title → Loading → Title이며 Restaurant 씬은 불러오지 않습니다.

**재현 방법**

1. Test Runner의 PlayMode에서 `DOB.Tests.SceneNavigationTests.LoadingScreenStaysForTheMinimumTimeAndFadesToBlackBeforeClosing`을 실행합니다. 전체 실행과 단독 실행 모두 해당합니다.
2. 같은 커밋에서 여러 번 반복하고 결과를 비교합니다.

**기대 결과** Loading이 닫히기 전에 FadeOverlay 투명도가 1.0(완전히 검음)에 도달한 것이 확인되고, 테스트가 매번 통과합니다.

**실제 결과** 테스트가 마지막으로 읽은 투명도가 0.989 또는 0.987에 머물러 "Loading을 닫기 전에 검은 페이드가 끝나야 합니다."로 실패할 때가 있습니다. [실패한 확인](https://github.com/SongYeChan1024/Project_DOB/blob/539b0d1/Assets/_DOB/Tests/PlayMode/SceneNavigationTests.cs#L117)

| 커밋 | 실행 | 결과 |
| --- | --- | --- |
| art `a7cd051` 전체 PlayMode | 1회 | 173개 중 172개 통과, 이 테스트만 실패 (0.989) |
| art `a7cd051` 단독 | 3회 | 통과 1 · 실패 2 (0.989, 0.987) |
| main `539b0d1` 단독 | 2회 | 통과 2 |

**추정 원인** 코드를 읽고 추정한 것으로, 확정하지 않았습니다.

- `LoadingScreenView.FadeTo`는 마지막 단계에서 투명도를 목표값 1로 맞춥니다. [FadeTo](https://github.com/SongYeChan1024/Project_DOB/blob/539b0d1/Assets/_DOB/02.Scripts/UI/LoadingScreenView.cs#L133-L145)
- `UIManager`는 페이드 아웃이 끝나는 대로 로딩을 닫습니다. [페이드 아웃 뒤 닫기](https://github.com/SongYeChan1024/Project_DOB/blob/539b0d1/Assets/_DOB/02.Scripts/Manager/UIManager.cs#L405-L408)
- 테스트는 `SceneLoadService.IsLoading` 동안 한 프레임에 한 번 투명도를 읽습니다. 1이 된 뒤 Loading이 닫히기 전에 읽을 기회가 없으면 직전 값(약 0.99)이 남습니다. [투명도 읽기 반복](https://github.com/SongYeChan1024/Project_DOB/blob/539b0d1/Assets/_DOB/Tests/PlayMode/SceneNavigationTests.cs#L91-L104)
- main도 같은 구조라 실패할 수 있습니다. 다만 main에서 2회 실행했을 때는 실패가 나오지 않았습니다.

**다음 단계** Loading 화면 담당 홍준우가 원인을 확인합니다. 💡 고칠 방향은 두 가지를 제안합니다(팀 확인 전).

- 테스트를 고쳐 Loading이 닫히는 시점의 마지막 투명도를 확실히 읽게 하기
- 화면을 고쳐 페이드가 1이 된 프레임을 한 번 보여 준 뒤 닫기

**재검증 기준** 수정한 커밋에서 이 테스트를 단독으로 10회 이상 반복해도 모두 통과합니다. PlayMode 전체 실행에서도 통과합니다.
