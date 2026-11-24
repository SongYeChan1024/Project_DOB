# 발표용 기획 시안 출처

송예찬 · 2026.11.24

| 발표 장면 | 그림과 출처 |
| --- | --- |
| 첫 손님 맞이 | 2026.10.26 최초 분위기 시안. 식당으로 통하는 계단과 좌석, 뒤쪽 주방 동선을 보여 준다. [식당 공간 원화](../../../Docs/References/3D/restaurant-layout.png) |
| 손님 응대 | 2026.10.26 원화는 입구에서 손님의 차림을 관찰하는 장면이다. [최초 관찰 원화](../../../Docs/References/3D/guest-observation.png) |
| 대사에서 단서 찾기 | 2026.11.24 발표용 신규 시안. 시험 손님 하루가 말한 ‘슬라임 점액’을 한 번 눌렀을 때의 밑줄과 수첩의 독 한 칸을 그렸다. [응대 시안](guest.png) · [시험 A 대사 원고](../../../Docs/기획/콘텐츠/FirstGuestsScript.md) |
| 주문에 맞춰 요리하기 | 2026.10.26 최초 분위기 시안. 재료대와 불 위에 올린 냄비, 주방에서 손님에게 접시를 가져가는 모습을 보여 준다. [자동 조리 원화](../../../Docs/References/3D/automatic-kitchen.png) |
| 방명록을 쓰고 퇴장하는 손님 | 2026.11.24 발표용 신규 시안. 계단 옆의 열린 방명록과 떠나는 하루의 미소 말풍선으로 식후 소감을 표현한다. [퇴장 시안](departure.png) · [첫 영업 연출안](../../../Docs/기획/콘텐츠/FirstServiceDirection.md) |

10월 원화의 제작 기록은 [3D 제작 참고도](../../../Docs/References/3D/README.md)와 [최초 이미지 프롬프트](../../../Docs/References/3D/prompts.json)에 있다.

## 신규 시안 제작 기록

응대와 퇴장 시안은 내장 ImageGen으로 각각 생성해 발표용 1600 × 900 PNG로 저장했다.

<details>
<summary>손님 응대 생성 프롬프트</summary>

```text
Use case: ui-mockup. Create one WIDE 16:9 1536 by 864 hand-painted art director's design study for the Korean cozy fantasy game 다녀왔어: 층계참 식당. This is a NEW illustrated planning mockup for the team, unmistakably a handmade gouache and ink concept on flat warm ivory art paper, not a screenshot or a replica of a running game. Horizontal and flat composition; readable in a presentation at 640 pixels wide. In upper left margin, put the small unobtrusive editorial caption "손님 응대 시안". One main focus: a human adult traveling mage (short brown hair, calm expressive eyes, simple slate robe with lightly singed sleeve and a wooden staff), three-quarter ink-and-gouache bust at left; behind her only loosely sketched stone steps and one amber restaurant lantern, lots of breathing space. Bottom left/center: a wide cream dialogue box with a strong dark ink border, a modest name tab "하루", and ONE sentence in large CLEAR PRINTED KOREAN in two lines: "슬라임 점액이 튀면" / "따끔하더라." Underline ONLY the words "슬라임 점액" in a ground ochre pencil stroke, leave following particle '이' and the whole second line unmarked. A small sketched hand pointer just below the underline indicates the player just clicked that phrase. At right, outside the dialogue panel, add a large beautiful open pocket notebook with tidy bookbinding and a green cloth spine. On its page only, print header "손님 수첩"; below it, print "독" beside a clearly filled purple ink square, then '1칸' in bold ink. Exactly one square on the whole drawing. The recorded clue is poison one square; no health meter, no further symbols, no extra writing or fake scribbles like letters. Leave corners and 6% margins clear. Paint in warm parchment, dense umber linework, muted teal and subtle violet ink. Keep exact Korean text legible; avoid logos, branding, watermarks, software chrome, large advertising headlines, cinematic 3D render, debug elements, promotional cover layout or symmetrical panels. The result should read as an attractive production designer's notebook plate about clicking dialogue evidence.
```

</details>

<details>
<summary>퇴장 생성 프롬프트</summary>

```text
Use case: stylized-concept. Draw one NEW 16:9 wide 1600x900 production concept sketch for a Korean game about a cozy restaurant built into a dungeon stair landing. Clearly a hand-drawn Korean game planner's storyboard illustration, with loose warm pencil lines and transparent watercolor over cream paper. Medium wide view from inside the little medieval restaurant looking toward the blue-green stone stairway GOING DOWN into the dungeon on the right. One guest, an adult HUMAN traveling mage with short brown tousled hair, muted indigo robe with a sage collar, tan small leather pouch and a plain hooked wooden staff, has FINISHED eating. She is stepping down away from the viewer at the TOP of the descending stairs, a visible silhouette in three-quarter back view, with her face slightly turned, contented at the good taste. Above her head ONE clear small cream speech bubble, tail pointing to the guest, containing only a SIMPLE cheerful emoji drawn in ochre ink: two eyes and a smiling mouth. This is an in-world proposed reaction sign for liking the meal. It must be readable even as a thumbnail. Foreground left, LARGE, a plain low wooden writing desk RIGHT BESIDE the stairs, on it an OPEN broad handwritten GUESTBOOK, an ink pot and quill. On the left page only a tiny ink sketch of a rustic loaf with a recognizable clove of garlic beside it, and short natural cursive pencil strokes suggesting a guestbook entry without legible words. One modest paper tab on the desk can read exactly “방명록” in small Korean, ONLY if legible; no other text anywhere. The two focal elements are the actual open book in the near left foreground and the departing guest with the smiling bubble in the right midground. Cozy soft light from a lantern to the left; cooler bluish daylight and depth on the steps. Keep it natural and characteristic: earthy umbers, warm parchment, blue slate stone, selective fine line details around the face and book, feathery blank paper areas around edges. Enough negative space between focal points. The action reads as departing a restaurant, with a small cleared plate on a distant dining table if visible. No digital HUD, no UI panels, no screenshot, no labels or fake text, no scene titles, no poster lettering, no watermarks, no colon, no royalty decorations. Compose the whole image as a single scene, no split panels.
```

</details>
