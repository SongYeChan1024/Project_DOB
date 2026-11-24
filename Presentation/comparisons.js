// source는 저장소 원본, src는 오프라인 발표와 Pages가 함께 읽는 배포 경로다.
(() => {
  function picture(source, filename, label, alt, caption) {
    return { source, src: 'assets/comparisons/' + filename + '.png', label, alt, caption };
  }

  function play(name, alt, caption) {
    return picture('Presentation/comparisons/after/' + name + '.png', 'play-' + name, '실제 플레이', alt, caption);
  }

  const entrance = play('restaurant', '식당 입구에서 손님과 마주한 카엘. 상단에 E를 눌러 손님을 맞이하라는 안내가 뜬다.',
    '입구에서 손님에게 다가가면 E로 인사할 수 있습니다.');
  const dialogue = play('notebook-generated', '아라의 묵직한 발걸음을 관찰한 뒤 수첩의 대사 단서와 초상화 단서에 각각 기록이 생기고 함정은 한 칸이다.',
    '대사와 외형에 기록이 하나씩 남았습니다. 둘 다 드워프의 발걸음에서 생긴 위험을 가리킵니다.');
  const bread = play('recipe', '구운 빵 조리 창. 밀가루 두 개와 마늘 한 개, 조리 시간과 조리 시작 버튼이 보인다.',
    '첫 주문은 구운 빵입니다. 재료와 6초의 조리 시간을 확인하고 시작합니다.');
  const reaction = play('serving', '자리에 음식을 건넨 직후 손님의 소감과 서빙으로 받은 금화가 표시된 식당.',
    '빵을 건네자 손님의 한 줄 소감과 서빙 대금 9금화가 함께 뜹니다.');
  const firstTitle = play('title-first', '첫 실행 타이틀 하단에 새로 시작, 설정, 나가기 버튼이 보이는 화면.',
    '저장 기록이 없는 첫 타이틀입니다. 첫 영업을 시작하려면 왼쪽 아래의 새로 시작을 누릅니다.');
  const savedTitle = play('title', '게임 타이틀 하단 메뉴에 마지막 저장 일차와 시각이 붙은 이어하기 버튼이 보인다.',
    '저장 후 돌아온 타이틀에서는 이어하기로 최근에 저장한 일차를 열 수 있습니다.');

  globalThis.DOB_COMPARISONS = {
    welcome: {
      before: picture('Docs/References/3D/restaurant-layout.png', 'design-restaurant', '초기 기획 원화',
        '던전 계단이 식당 입구와 이어지고, 테이블 너머에 주방이 보이는 공간 원화.',
        '계단으로 들어온 손님이 좌석을 지나 주방을 볼 수 있게 그린 초기 배치입니다.'),
      after: entrance,
    },
    clues: {
      before: picture('Presentation/comparisons/before/guest.png', 'design-guest', '발표용 기획 시안',
        '손님이 한 말 중 슬라임 점액에 밑줄이 그어지고, 펼친 수첩에 독 한 칸이 기록된 그림.',
        '플레이어가 대사에서 단어를 찾아 누른 직후를 그렸습니다.'),
      after: play('guest-generated', '아라가 말한 바닥돌이 덜컥에 밑줄이 생기고 수첩에 함정 1과 대사 단서가 보이는 화면.',
        '아라가 들려준 표현을 누른 순간입니다. 방금 누른 말이 사전에 남고 왼쪽 함정 줄에 한 칸이 찼습니다.'),
    },
    cook: {
      before: picture('Docs/References/3D/automatic-kitchen.png', 'design-kitchen', '초기 기획 원화',
        '주방 재료대와 불 위의 냄비, 완성한 음식을 옮기는 장면을 그린 원화.',
        '재료가 조리대를 거쳐 손님의 접시로 가는 과정을 그린 초기 원화입니다.'),
      after: bread,
    },
    guestbook: {
      before: picture('Presentation/comparisons/before/departure.png', 'design-departure', '발표용 기획 시안',
        '계단 옆 펼쳐진 방명록과 웃는 얼굴 말풍선을 띄우고 던전으로 내려가는 손님.',
        '손님이 떠나는 모습을 보면서 식사가 어땠는지 짐작하는 연출입니다.'),
      after: play('guestbook', '손님이 던전으로 떠난 식당 위에 열린 방명록. 손님의 이름과 서빙 때 한 말이 적혀 있다.',
        '손님이 자리를 떠나면 서빙할 때 한 말이 방명록에 남습니다.'),
    },
    'return-report': {
      before: picture('Docs/References/3D/morning-return.png', 'design-return', '초기 기획 원화',
        '던전에서 돌아와 식당 주인에게 전리품을 건네는 손님과 의자에 앉아 쉬는 모험가.',
        '전날 식사를 대접한 손님에게 탐험 결과를 직접 듣는 연출입니다.'),
      after: play('morning', '2일차 아침 카드. 귀환 보고 준비 안내와 보유 금화, 마늘 개수, 보고 확인 버튼.',
        '둘째 날은 보고 카드로 시작합니다. 손님별 탐험 결과가 들어갈 자리입니다.'),
    },
  };

  globalThis.DOB_SCENES = {
    title: firstTitle,
    'menu-new': play('menu-new', '저장 데이터가 있는 타이틀에서 새로 시작을 누르자 1일차부터 시작할지 묻는 확인 창.',
      '저장된 게임이 있으면 새로 시작과 취소 중에서 고릅니다.'),
    'menu-continue': savedTitle,
    'menu-settings': play('settings', '타이틀 위에 열린 설정 창과 닫기 버튼.',
      '타이틀에서 설정을 누르면 열리는 창입니다. 닫기를 누르면 메뉴로 돌아갑니다.'),
    'menu-quit': play('title-first', '타이틀 하단 메뉴의 오른쪽 끝에 있는 나가기 버튼.',
      '타이틀의 나가기를 누르면 실행 중인 게임 창이 닫힙니다.'),
    'choose-new': firstTitle,
    tutorial: entrance,
    notebook: dialogue,
    seat: play('seating', '손님이 기본석에 앉은 뒤 오른쪽 주방에서 구운 빵을 준비하라는 안내가 뜬다.',
      '빈자리에 안내한 손님이 앉으면 첫 주문을 요리할 수 있습니다.'),
    serve: play('carrying', '주방에서 접시를 든 채 손님이 앉은 자리로 돌아가라는 안내를 보는 장면.',
      '조리가 끝난 뒤 접시를 들고 손님 자리로 돌아갑니다.'),
    taste: reaction,
    'day-one': play('departure', '식사를 마친 아라가 앉았던 자리에서 던전 계단 쪽으로 걸어가고 있다.',
      '첫 손님을 보낸 순간입니다. 지금 플레이할 수 있는 첫날은 한 명의 방문으로 마감합니다.'),
    settlement: play('settlement', '방명록을 닫은 뒤 식당 오른쪽 아래에 활성화된 마감 확인 버튼.',
      '마지막 손님이 떠나고 기록이 끝난 뒤 마감 확인을 눌러 다음 날로 갑니다.'),
    market: picture('Presentation/comparisons/evidence/market-test.png', 'market-test', '시장 UI · Unity 테스트 씬',
      '시장 행상인 창. 마늘과 박하 등 양념별 효과, 가격, 보유량, 구매 수량과 결제 버튼이 보인다.',
      '팀원이 테스트 씬에서 열어 본 시장입니다. 실제 아침 준비 화면에서 시장으로 들어가는 연결은 후속 작업입니다.'),
    'next-service': play('preparation', '2일차 영업 준비 카드에 보유 금화와 마늘, 영업 시작 버튼이 보인다.',
      '보고를 확인하면 준비 카드가 열립니다. 영업 시작을 누르면 식당으로 돌아갑니다.'),
  };
})();
