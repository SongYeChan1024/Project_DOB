# Project_DOB — Codex 작업 안내

이 저장소의 작업 규칙은 [CLAUDE.md](CLAUDE.md)와 팀 규칙의 기준인 [README.md](README.md)를 따릅니다. 작업 전에 두 문서를 읽습니다(CLAUDE.md의 `@` 가져오기는 Codex가 자동으로 읽지 않습니다).

## Basement에서 작업을 마칠 때

사용자가 맡긴 통합 작업은 `Basement`에서 진행합니다. 작업 하나를 마칠 때마다 `git fetch origin`으로 원격을 갱신하고 `origin/Basement`, `origin/main` 및 팀 작업 브랜치에 아직 반영하지 않은 커밋이 있는지 확인합니다. 새 커밋을 `Basement`에 병합한 뒤 해당 기능을 검증하고 `origin/Basement`에 일반 푸시합니다. 충돌이 생기면 겹친 파일과 양쪽 변경을 사용자에게 먼저 알립니다. 팀원이 작성한 기능을 직접 수정할 때도 먼저 사용자와 범위를 확인합니다.
