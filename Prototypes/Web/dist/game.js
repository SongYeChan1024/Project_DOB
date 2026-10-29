(()=>{var Pc={AXES:["poison","fire","cold","dark","vital","sense"],AX:{poison:{name:"\uB3C5",eff:"\uD574\uB3C5",word:"\uB3C5",words:"\uC2AC\uB77C\uC784 \uC810\uC561 \xB7 \uB3C5 \uC548\uAC1C \xB7 \uB530\uB054\uD568"},fire:{name:"\uC5F4",eff:"\uB0B4\uC5F4",word:"\uC5F4\uAE30",words:"\uB728\uAC70\uC6B4 \uBC14\uB2E5 \xB7 \uBD88\uAE38 \xB7 \uD0C0\uB294 \uBAA9"},cold:{name:"\uB0C9\uAE30",eff:"\uBC29\uD55C",word:"\uD55C\uAE30",words:"\uC5B8 \uC190\uAC00\uB77D \xB7 \uC11C\uB9AC \xB7 \uC624\uD55C"},dark:{name:"\uC5B4\uB460",eff:"\uC57C\uAD11",word:"\uC5B4\uB460",words:"\uAEBC\uC9C4 \uD683\uBD88 \xB7 \uBCF4\uC774\uC9C0 \uC54A\uC74C"},vital:{name:"\uCCB4\uB825",eff:"\uD65C\uB825",word:"\uCCB4\uB825",words:"\uC9C0\uCE68 \xB7 \uD5C8\uAE30 \xB7 \uBA3C \uAE38"},sense:{name:"\uD568\uC815",eff:"\uAC10\uAC01",word:"\uD568\uC815",words:"\uBC1C\uBC11 \xB7 \uC6B8\uB9AC\uB294 \uBC14\uB2E5 \xB7 \uB208\uCE58"}},FLOORS:{"1":{name:"\uC2AC\uB77C\uC784 \uC232",flavor:"\uC816\uC740 \uC774\uB07C\uC640 \uC2AC\uB77C\uC784\uC774 \uAE54\uB9B0 \uCCAB \uBC88\uC9F8 \uCE35",haz:{poison:1,vital:1},loot:[["jelly",3],["shroom",3],["herb",3],["carrot",2]]},"2":{name:"\uACF0\uD321\uC774 \uB3D9\uAD74",flavor:"\uD478\uB978 \uD3EC\uC790\uAC00 \uB5A0\uB2E4\uB2C8\uB294 \uCD95\uCD95\uD55C \uB3D9\uAD74",haz:{poison:2,dark:1,sense:1},loot:[["moss",3],["bat",3],["mold",2],["herb",1]]},"3":{name:"\uC6A9\uC554 \uADE0\uC5F4",flavor:"\uBC14\uB2E5 \uD2C8\uB9C8\uB2E4 \uBD89\uC740 \uC5F4\uC774 \uC0C8\uC5B4 \uB098\uC624\uB294 \uCE35",haz:{fire:3,vital:1,sense:1},loot:[["pepper",3],["tail",2],["vsalt",1]]},"4":{name:"\uC11C\uB9AC \uD68C\uB791",flavor:"\uBCBD\uC774 \uD558\uC597\uAC8C \uC5BC\uC5B4\uBD99\uC740 \uAE34 \uBCF5\uB3C4",haz:{cold:3,dark:1,vital:1},loot:[["berry",3],["shrimp",3],["bloom",2]]},"5":{name:"\uB208\uBA3C \uC2EC\uC5F0",flavor:"5\uCE35\uC758 \uC5B4\uB450\uC6B4 \uC77C\uBC18 \uD1B5\uB85C. \uACE0\uB9BD \uAD6C\uC5ED\uC740 \uBCC4\uB3C4 \uC870\uC0AC \uB300\uC0C1\uC774\uB2E4.",haz:{dark:3,poison:1,cold:1,vital:1},loot:[["ink",3],["pearl",1],["bloom",1],["tail",1]]}},ING:{flour:{n:"\uBC00\uAC00\uB8E8",base:!0,price:3},potato:{n:"\uAC10\uC790",base:!0,price:3},meat:{n:"\uACE0\uAE30",base:!0,price:6},jelly:{n:"\uC2AC\uB77C\uC784 \uC824\uB9AC",floor:1,price:3},shroom:{n:"\uC232\uBC84\uC12F",floor:1,price:2},herb:{n:"\uB4E4\uD480",floor:1,price:2},carrot:{n:"\uC232 \uB2F9\uADFC",floor:1,price:2},moss:{n:"\uBB3C\uC774\uB07C",floor:2,price:3},bat:{n:"\uBC15\uC950 \uB0A0\uAC1C",floor:2,price:3},mold:{n:"\uD478\uB978\uACF0\uD321\uC774",floor:2,price:3},pepper:{n:"\uC6A9\uC554 \uACE0\uCD94",floor:3,price:4},tail:{n:"\uBD88\uB3C4\uB9C8\uBC40 \uAF2C\uB9AC",floor:3,price:5},vsalt:{n:"\uD654\uC0B0 \uC18C\uAE08",floor:3,rare:!0},berry:{n:"\uC11C\uB9AC \uBCA0\uB9AC",floor:4,price:4},shrimp:{n:"\uC5BC\uC74C \uAC00\uC7AC",floor:4,price:5},bloom:{n:"\uB2EC\uBE5B \uC11C\uB9AC\uAF43",floor:4,price:5},ink:{n:"\uC2EC\uC5F0 \uBA39\uBB3C",floor:5,price:6},pearl:{n:"\uC2EC\uC5F0 \uC9C4\uC8FC",floor:5,rare:!0}},SPICES:{garlic:{n:"\uB9C8\uB298",ax:"poison",tag:"\uD5A5\uAE0B",price:2},mint:{n:"\uBC15\uD558",ax:"fire",tag:"\uC2DC\uC6D0",price:2},ginger:{n:"\uC0DD\uAC15",ax:"cold",tag:"\uC5BC\uD070",price:2},carjuice:{n:"\uB2F9\uADFC\uC999",ax:"dark",tag:"\uB2EC\uCF64",price:3},salt:{n:"\uC18C\uAE08",ax:"vital",tag:"\uC9ED\uC9E4",price:2},blackpepper:{n:"\uD6C4\uCD94",ax:"sense",tag:"\uC54C\uC2F8",price:2}},RECIPES:[{id:"bread",n:"\uAD6C\uC6B4 \uBE75",ing:{flour:2},eff:{vital:1,sense:1},tags:["\uACE0\uC18C","\uB2F4\uBC31"],price:8},{id:"stew",n:"\uAC10\uC790 \uC2A4\uD29C",ing:{potato:1,meat:1},eff:{vital:2},tags:["\uC9C4\uD55C"],price:12},{id:"grill",n:"\uACE0\uAE30 \uAD6C\uC774",ing:{meat:2},eff:{vital:3},tags:["\uC9C4\uD55C","\uBC14\uC0AD"],price:16},{id:"jellysalad",n:"\uC2AC\uB77C\uC784 \uC824\uB9AC \uC0D0\uB7EC\uB4DC",ing:{jelly:1,herb:1},eff:{poison:2,sense:1},tags:["\uC0C1\uD07C","\uC2DC\uC6D0"],price:14},{id:"shroomsoup",n:"\uC232\uBC84\uC12F \uC218\uD504",ing:{shroom:2,potato:1},eff:{vital:2,poison:1},tags:["\uB2F4\uBC31"],price:13},{id:"herbpie",n:"\uB4E4\uD480 \uD30C\uC774",ing:{herb:1,flour:1},eff:{sense:2,vital:1},tags:["\uACE0\uC18C","\uD5A5\uAE0B"],price:11},{id:"carrotcake",n:"\uB2F9\uADFC \uCF00\uC774\uD06C",ing:{carrot:2,flour:1},eff:{dark:2,vital:1},tags:["\uB2EC\uCF64"],price:12},{id:"mosssoup",n:"\uBB3C\uC774\uB07C \uB0C9\uAD6D",ing:{moss:2},eff:{fire:2,vital:1},tags:["\uC2DC\uC6D0","\uB2F4\uBC31"],price:15},{id:"mossmuch",n:"\uBB3C\uC774\uB07C \uBB34\uCE68",ing:{moss:1,herb:1},eff:{fire:2,sense:1},tags:["\uC2DC\uC6D0","\uD5A5\uAE0B"],price:14},{id:"batfry",n:"\uBC15\uC950 \uB0A0\uAC1C \uD280\uAE40",ing:{bat:2},eff:{sense:2,dark:1},tags:["\uBC14\uC0AD","\uACE0\uC18C"],price:15},{id:"moldgratin",n:"\uD478\uB978\uACF0\uD321\uC774 \uADF8\uB77C\uD0F1",ing:{mold:1,potato:1,meat:1},eff:{poison:2,vital:1},tags:["\uCFF0\uCFF0","\uC9C4\uD55C"],price:16},{id:"pepperfry",n:"\uC6A9\uC554 \uACE0\uCD94 \uBCF6\uC74C",ing:{pepper:2,meat:1},eff:{cold:3},tags:["\uC5BC\uD070","\uC9C4\uD55C"],price:22},{id:"tailgrill",n:"\uBD88\uB3C4\uB9C8\uBC40 \uAF2C\uB9AC\uAD6C\uC774",ing:{tail:1,meat:1},eff:{vital:2,cold:1},tags:["\uC9C4\uD55C","\uC5BC\uD070"],price:20},{id:"vsaltpotato",n:"\uD654\uC0B0 \uC18C\uAE08 \uAC10\uC790",ing:{vsalt:1,potato:2},eff:{vital:2,cold:1},tags:["\uC9ED\uC9E4","\uACE0\uC18C"],price:15},{id:"parfait",n:"\uC11C\uB9AC \uBCA0\uB9AC \uD30C\uB974\uD398",ing:{berry:2},eff:{fire:2,sense:1},tags:["\uB2EC\uCF64","\uC2DC\uC6D0"],price:22},{id:"shrimpstew",n:"\uC5BC\uC74C \uAC00\uC7AC \uC2A4\uD29C",ing:{shrimp:1,potato:1},eff:{vital:2,fire:1},tags:["\uC9C4\uD55C","\uC2DC\uC6D0"],price:24},{id:"bloomsalad",n:"\uC11C\uB9AC\uAF43 \uC0D0\uB7EC\uB4DC",ing:{bloom:2},eff:{dark:3},tags:["\uD5A5\uAE0B","\uC309\uC300"],price:24},{id:"inkrisotto",n:"\uBA39\uBB3C \uB9AC\uC870\uB610",ing:{ink:1,flour:1,meat:1},eff:{dark:3,vital:1},tags:["\uC9C4\uD55C","\uC309\uC300"],price:30},{id:"abyss",n:"\uC2EC\uC5F0 \uCF54\uC2A4",ing:{pearl:1,ink:1,bloom:1},eff:{dark:3,poison:1,cold:1,vital:1},tags:["\uC309\uC300","\uD5A5\uAE0B"],price:45}],CLASSES:{warrior:{n:"\uC804\uC0AC",res:"vital",trait:"\uCCB4\uB825\uC774 \uC88B\uB2E4"},mage:{n:"\uB9C8\uBC95\uC0AC",res:"fire",trait:"\uBD88\uC5D0 \uC775\uC219\uD558\uB2E4"},rogue:{n:"\uB3C4\uC801",res:"sense",trait:"\uD568\uC815\uC5D0 \uBC1D\uB2E4"},priest:{n:"\uC0AC\uC81C",res:"poison",trait:"\uC815\uD654\uC758 \uAC00\uD638\uAC00 \uC788\uB2E4"},miner:{n:"\uAD11\uBD80",res:"dark",trait:"\uC5B4\uB460\uC5D0 \uC775\uC219\uD558\uB2E4"},archer:{n:"\uAD81\uC218",res:"cold",trait:"\uCD94\uC704\uC5D0 \uAC15\uD558\uB2E4"}},metadata:{author:"\uC1A1\uC608\uCC2C",status:"\u{1F4A1} \uC6F9 \uAE30\uD68D \uAC80\uC99D\uC6A9 \uC218\uCE58. \uD300 \uD655\uC815 \uC544\uB2D8",source:"\uAE30\uC874 \uCE35\uACC4\uCC38 \uC2DD\uB2F9 HTML\uC758 \uB370\uC774\uD130 \uD45C\uB97C \uC7AC\uC0AC\uC6A9"}};var Z=Pc,Ct=Z.AXES,$n={human:{name:"\uC778\uAC04",weak:null},elf:{name:"\uC5D8\uD504",weak:"fire"},beast:{name:"\uC218\uC778",weak:"poison"},dwarf:{name:"\uB4DC\uC6CC\uD504",weak:"sense"},halfling:{name:"\uD558\uD50C\uB9C1",weak:"vital"}},Po=["\uC544\uC2A4\uD2B8\uB77C","\uB178\uC544","\uB9AC\uC628","\uC774\uC548","\uC138\uB77C","\uB8E8\uB098","\uC5D0\uB9B0","\uD14C\uC624","\uBBF8\uB77C","\uB85C\uC6EC","\uC720\uB9AC","\uB808\uB098","\uB098\uB8E8","\uB8E8\uC5D4","\uD558\uB8E8","\uC5D8\uB9B0"],_t=[{id:0,name:"\uC57D\uCD08 \uD654\uBD84",axis:"poison",x:-4.5,z:.6,color:"#95bd89",cost:0},{id:1,name:"\uC26C\uC5B4 \uAC00\uB294 \uC758\uC790",axis:"vital",x:0,z:.6,color:"#e7b76e",cost:0},{id:2,name:"\uB09C\uB85C \uACC1",axis:"cold",x:4.5,z:.6,color:"#f2a97e",cost:0},{id:3,name:"\uBC14\uB78C \uB4DC\uB294 \uCC3D\uAC00",axis:"fire",x:-4.5,z:4.1,color:"#8fc6c1",cost:45},{id:4,name:"\uC5B4\uB451\uD55C \uAD6C\uC11D",axis:"dark",x:0,z:4.1,color:"#aca5dc",cost:55},{id:5,name:"\uC870\uC6A9\uD55C \uCE78\uB9C9\uC774",axis:"sense",x:4.5,z:4.1,color:"#e0ce87",cost:65}],Gt={author:"\uC1A1\uC608\uCC2C",version:1,startGold:80,guestsPerDay:3,cookMs:2400,surpriseChance:.25,unlockWins:2,authoredFloors:5,injuryRest:2,shelf:2,stoveCosts:[60,120],maxScenesPerMorning:1,proposal:!0},Lc={poison:"\uC9C0\uB09C\uBC88\uC5D0\uB294 \uC548\uAC1C\uB97C \uC870\uAE08 \uB9C8\uC168\uB294\uB370\uB3C4 \uC18D\uC774 \uB4A4\uC9D1\uD614\uC5B4.",fire:"\uC228\uB9CC \uC26C\uC5B4\uB3C4 \uBAA9\uC774 \uD0C0\uACE0, \uC7A5\uD654 \uBC11\uCC3D\uC774 \uB728\uAC70\uC6CC.",cold:"\uCE7C\uC744 \uC954 \uC190\uAC00\uB77D\uC774 \uC5BC\uC5B4\uC11C \uC798 \uC6C0\uC9C1\uC774\uC9C0 \uC54A\uC558\uC5B4.",dark:"\uD683\uBD88\uC774 \uC790\uAFB8 \uAEBC\uC838. \uC55E\uC774 \uBCF4\uC774\uC9C0 \uC54A\uB294 \uAC8C \uC81C\uC77C \uAC71\uC815\uC774\uC57C.",vital:"\uC6B0\uD68C\uB85C\uAC00 \uAE38\uC5B4\uC84C\uB300. \uB3CC\uC544\uC62C \uB54C\uAE4C\uC9C0 \uB2E4\uB9AC\uAC00 \uBC84\uD168 \uC904\uAE4C.",sense:"\uBC1C\uBC11\uC5D0\uC11C \uB538\uAE4D \uC18C\uB9AC\uAC00 \uB0AC\uC5B4. \uD55C \uAC78\uC74C\uB9C8\uB2E4 \uC2E0\uACBD\uC774 \uC4F0\uC5EC."},Lo={mark:{name:"\uC57C\uC601 \uD45C\uC2DD",text:"\uB0A1\uC740 \uC870\uB9AC\uB3C4\uAD6C\uC5D0 \uC0C8\uAE34 \uC6B0\uB9AC \uD30C\uD2F0\uC758 \uD45C\uC2DD. \uC9C0\uAE08 \uC0B4\uC544 \uC788\uB2E4\uB294 \uC99D\uAC70\uB294 \uC544\uB2C8\uB2E4."},gap:{name:"\uC0C8 \uC5F0\uB77D \uD2C8",text:"\uC791\uC740 \uCABD\uC9C0\uB294 \uC624\uAC08 \uC218 \uC788\uB2E4. \uC0AC\uB78C\uACFC \uAD6C\uC870 \uC7A5\uBE44\uAC00 \uD1B5\uACFC\uD560 \uAE38\uC740 \uC544\uB2C8\uB2E4."},reply:{name:"\uD604\uC7AC \uC9C8\uBB38\uC758 \uB2F5\uC7A5",text:"\uC774\uBC88\uC5D0 \uBCF4\uB0B8 \uC9C8\uBB38\uC5D0 \uB300\uD55C \uB2F5\uACFC \uB3D9\uB8CC\uB4E4\uC758 \uD604\uC7AC \uC0C1\uD0DC\uAC00 \uB2F4\uACBC\uB2E4. \u{1F4A1} \uB2F5\uC7A5 \uC6D0\uACE0\xB7\uD30C\uD2F0 \uAD6C\uC131 \uBBF8\uC815."},inner:{name:"\uD53C\uB09C\uCC98 \uB0B4\uBD80 \uC9C0\uB3C4",text:"\uB3D9\uB8CC\uB4E4\uC774 \uBA38\uBB34\uB294 \uAD6C\uC5ED\uACFC \uC548\uCABD \uC9C0\uD615\uC758 \uAE30\uB85D."},outer:{name:"\uC678\uBD80 \uACBD\uB85C \uC870\uC0AC",text:"\uC815\uBE44\uB85C\uC640 \uB099\uCC28, \uC811\uADFC \uAC00\uB2A5\uD55C \uACE0\uC815\uC810\uC744 \uAD00\uCC30\uD588\uB2E4. \u{1F4A1} \uC9C0\uD615\xB7\uC7A5\uBE44 \uBBF8\uC815."},cooking:{name:"\uC774\uC5B4 \uC628 \uC2DD\uC0AC",text:"\uB0A8\uACA8 \uB454 \uC870\uB9AC \uB3C4\uAD6C\uC640 \uBC30\uC6B4 \uC190\uC9C8\uBC95\uC73C\uB85C \uC11C\uB85C\uC758 \uC2DD\uC0AC\uB97C \uB9C8\uB828\uD574 \uC654\uB2E4. \u{1F4A1} \uC7AC\uB8CC\xB7\uC0DD\uC874 \uAE30\uAC04 \uBBF8\uC815."}},xn={FIRST_GREAT_SUCCESS:{priority:2,title:"\uB2E4\uB140\uC654\uC5B4",text:"\uADF8 \uB9D0 \uD55C\uB9C8\uB514\uC5D0 \uC950\uACE0 \uC788\uB358 \uD589\uC8FC\uB97C \uB193\uC558\uB2E4. \uC624\uB298\uC740 \uB3CC\uC544\uC62C \uD798\uAE4C\uC9C0 \uB0A8\uAE38 \uC218 \uC788\uC5C8\uAD6C\uB098."},FIRST_SUCCESS:{priority:2,title:"\uC870\uAE08 \uBAA8\uC790\uB790\uB358 \uAC83",text:"\uBB34\uC0AC\uD788 \uB3CC\uC544\uC654\uC9C0\uB9CC \uC644\uBCBD\uD55C \uC900\uBE44\uB294 \uC544\uB2C8\uC5C8\uB2E4. \uC190\uB2D8\uC758 \uC774\uC57C\uAE30\uB97C \uB4E3\uACE0 \uB2E4\uC74C \uC2DD\uC0AC\uC758 \uBE48\uCE78\uC744 \uC801\uB294\uB2E4."},FIRST_NARROW_ESCAPE:{priority:2,title:"\uB3CC\uC544\uC62C \uBAAB",text:"\uAC70\uAE30\uAE4C\uC9C0 \uAC08 \uD798\uB9CC \uC0DD\uAC01\uD588\uC5C8\uC9C0. \uC190\uB2D8\uC758 \uBE48 \uADF8\uB987\uC744 \uBCF4\uBA70, \uB3CC\uC544\uC624\uB294 \uAE38\uC758 \uBAAB\uC744 \uB2E4\uC2DC \uC0DD\uAC01\uD55C\uB2E4."},FIRST_INJURY:{priority:2,title:"\uBA3C\uC800 \uC549\uC544\uC694",text:"\uCC98\uC74C \uB5A0\uC624\uB978 \uAC83\uC740 \uC798\uBABB\uB41C \uCC98\uBC29\uC774 \uC544\uB2C8\uB77C \uC190\uB2D8\uC758 \uC0C1\uCC98\uC600\uB2E4. \uBB3C\uACFC \uC758\uC790\uB97C \uBA3C\uC800 \uAC00\uC838\uC654\uB2E4."},RESERVE_HELPED_RETURN:{priority:3,title:"\uB0A8\uACA8 \uB454 \uD798",text:"\u201C\uC608\uC0C1 \uBABB \uD55C \uC77C\uC774 \uC788\uC5C8\uC9C0\uB9CC, \uB0A8\uACA8 \uB454 \uD798 \uB355\uBD84\uC5D0 \uB3CC\uC544\uC654\uC5B4\uC694.\u201D \uB354 \uAE4A\uC774 \uAC00\uB294 \uAC83\uACFC \uBB34\uC0AC\uD788 \uB3CC\uC544\uC624\uB294 \uAC83\uC740 \uB2E4\uB974\uB2E4."},INJURED_GUEST_RETURNS_WELL:{priority:3,title:"\uB2E4\uC2DC, \uAC19\uC740 \uC790\uB9AC",text:"\uC804\uC5D0 \uB2E4\uCCE4\uB358 \uC190\uB2D8\uC774 \uC774\uBC88\uC5D0\uB294 \uD3B8\uC548\uD558\uAC8C \uC6C3\uC5C8\uB2E4. \uC774\uB984\uACFC \uC5BC\uAD74\uC744 \uAE30\uC5B5\uD55C \uCC44 \uC624\uB298\uC758 \uADF8\uB987\uC744 \uB0B4\uC5B4 \uC900\uB2E4."},PARTY_MARK_FOUND:{priority:1,title:"\uB0AF\uC775\uC740 \uD45C\uC2DD",text:"5\uCE35\uC758 \uC791\uC740 \uD2C8 \uC55E\uC5D0\uC11C \uCC3E\uC558\uB2E4\uB294 \uC870\uB9AC\uB3C4\uAD6C. \uB0B4\uAC00 \uC0C8\uAE34 \uD45C\uC2DD\uC774\uB2E4. \uC624\uB798\uB41C \uBB3C\uAC74\uB9CC\uC73C\uB85C\uB294 \uC54C \uC218 \uC5C6\uB2E4. \uC9C0\uAE08\uC758 \uC9C8\uBB38\uC744 \uBCF4\uB0B4\uC57C \uD55C\uB2E4."},CURRENT_SURVIVAL_CONFIRMED:{priority:1,after:"PARTY_MARK_FOUND",title:"\uC9C0\uAE08\uC758 \uB2F5\uC7A5",text:"\uC774\uBC88 \uC9C8\uBB38\uC5D0 \uB300\uD55C \uB2F5\uC774 \uB3CC\uC544\uC654\uB2E4. \uB3D9\uB8CC\uB4E4\uB9CC \uC544\uB294 \uB300\uB2F5\uACFC \uAC01\uC790\uC758 \uD604\uC7AC \uC0C1\uD0DC. \uBAA8\uB450 \uC544\uC9C1 \uADF8\uACF3\uC5D0 \uC788\uB2E4."},THEY_KEPT_COOKING:{priority:1,after:"CURRENT_SURVIVAL_CONFIRMED",title:"\uB0A8\uACA8 \uB454 \uAC83\uC740",text:"\u201C\uB124\uAC00 \uC54C\uB824 \uC900 \uB300\uB85C \uC190\uC9C8\uD574\uC11C \uB098\uB220 \uBA39\uACE0 \uC788\uC5B4. \uC774\uBC88\uC5D4 \uB3CC\uC544\uAC08 \uBAAB\uB3C4 \uAC19\uC774 \uCC59\uACA8 \uC640.\u201D \uB0B4\uAC00 \uB0A8\uAE34 \uAC83\uC774 \uC2E4\uD328\uBFD0\uC740 \uC544\uB2C8\uC5C8\uB2E4."},ISOLATED_AREA_LOCATED:{priority:1,after:"THEY_KEPT_COOKING",title:"\uB450 \uC9C0\uB3C4\uAC00 \uB9CC\uB098\uB294 \uACF3",text:"\uBC14\uAE65\uC758 \uC870\uC0AC\uC640 \uC548\uCABD\uC758 \uC9C0\uB3C4\uAC00 5\uCE35 \uD55C \uACF3\uC744 \uAC00\uB9AC\uD0A8\uB2E4. \uC5F0\uB77D \uD2C8\uACFC \uC0AC\uB78C\uC774 \uB3CC\uC544\uC62C \uAE38\uC740 \uB2E4\uB974\uB2E4. \uACBD\uB85C\uC640 \uC7A5\uBE44\uAE4C\uC9C0 \uC900\uBE44\uD574\uC57C \uD55C\uB2E4."}},Dc={great:"\uB300\uC131\uACF5",good:"\uC131\uACF5",narrow:"\uAC04\uC2E0\uD788",hurt:"\uBD80\uC0C1"},Yn={label:"\u{1F4A1} \uC5D4\uB529 A\uC548 \uC2DC\uD5D8 \uC124\uC815",version:"preview-A-1",selfHazard:{dark:2,poison:1,vital:2},reserve:1,supplyCost:12,endingPolicy:"return-and-reopen"};var yr=class{constructor(){this.handlers=new Map}on(e,t){this.handlers.has(e)||this.handlers.set(e,[]),this.handlers.get(e).push(t)}emit(e,t){for(let i of this.handlers.get(e)||[])i(t)}};function on(n){let e=n.seed=n.seed+1831565813>>>0;return e=Math.imul(e^e>>>15,e|1),e^=e+Math.imul(e^e>>>7,e|61),((e^e>>>14)>>>0)/4294967296}var Pn=(n,e)=>e[Math.floor(on(n)*e.length)];function Uc(n){try{return localStorage.setItem("dob-3d-v1-"+n.mode,JSON.stringify(n)),!0}catch{return!1}}function Bi(n){try{let e=JSON.parse(localStorage.getItem("dob-3d-v1-"+n)||"null");return e?.version===1&&e.mode===n?e:null}catch{return null}}var We=n=>String(n??"").replace(/[&<>"']/g,e=>({"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#39;"})[e]);var vn={tags:["\uACE0\uC18C","\uB2F4\uBC31","\uC9C4\uD55C","\uC2DC\uC6D0","\uB2EC\uCF64","\uC5BC\uD070","\uC309\uC300","\uCFF0\uCFF0","\uC9ED\uC9E4","\uD5A5\uAE0B","\uBC14\uC0AD","\uC0C1\uD07C","\uC54C\uC2F8"],base:2,maxLikeBonus:2,dislikePenalty:2,priceBase:.6,priceStep:.2,lootChance:.35},Nc={human:[],elf:["\uD5A5\uAE0B","\uC0C1\uD07C"],beast:["\uACE0\uC18C","\uBC14\uC0AD"],dwarf:["\uC9C4\uD55C","\uC5BC\uD070"],halfling:["\uB2EC\uCF64","\uACE0\uC18C"]},Fc=["\u2026\uC774\uAC74 \uC880 \uC544\uB2C8\uB2E4.","\uC74C, \uC785\uC5D0\uB294 \uC798 \uC548 \uB9DE\uB124.","\uB098\uC058\uC9C0 \uC54A\uC740 \uB9DB\uC774\uC57C.","\uB9DB\uC788\uB124. \uD798\uC774 \uB098\uB294 \uAC83 \uAC19\uC544.","\uC774\uAC70\uC57C! \uB531 \uB0B4\uAC00 \uC6D0\uD558\uB358 \uB9DB\uC774\uC57C."];function ws(n,e){if(!Array.isArray(e.likes)||!Array.isArray(e.dislikes)){let t=Nc[e.race]||[];for(e.likes=t.length?[...t]:[];e.likes.length<2;)e.likes.push(Pn(n,vn.tags.filter(i=>!e.likes.includes(i))));e.dislikes=[Pn(n,vn.tags.filter(i=>!e.likes.includes(i)))]}e.knownLikes??(e.knownLikes=[e.likes[0]]),e.knownDislikes??(e.knownDislikes=[])}function Mr(n,e=[]){return[...new Set([...n?.tags||[],...e.map(t=>Z.SPICES[t]?.tag).filter(Boolean)])]}function Oc(n,e){let t=n.likes.filter(r=>e.includes(r)),i=n.dislikes.filter(r=>e.includes(r));return{score:Math.max(0,Math.min(4,vn.base+Math.min(vn.maxLikeBonus,t.length)-(i.length?vn.dislikePenalty:0))),liked:t,disliked:i}}function kc(n,e,t){return Math.round(n.price*(vn.priceBase+vn.priceStep*t.score))+e.length}function Bc(n,e){let t=[];if(e.score>=3){let s=e.liked.find(r=>!n.knownLikes.includes(r));s&&(n.knownLikes.push(s),t.push({kind:"like",tag:s}))}let i=e.disliked.find(s=>!n.knownDislikes.includes(s));return i&&(n.knownDislikes.push(i),t.push({kind:"dislike",tag:i})),t}var zc={author:"\uC1A1\uC608\uCC2C",status:"\u{1F4A1} \uAE30\uC874 \uC6D0\uD615 \uAE30\uBC18 \uC0AC\uC5F0\xB7\uC885\uC871\xB7\uC218\uCE58 \uCD08\uC548. \u{1F534} \uCD5C\uC885 \uB300\uC0AC\uC640 \uBB3C\uAC74 \uD6A8\uACFC \uBBF8\uC815",regulars:[{id:"boris",name:"\uBCF4\uB9AC",job:"warrior",race:"human",likes:["\uC9C4\uD55C","\uC9ED\uC9E4"],dislikes:["\uC309\uC300"],intro:"\uC2E0\uC785 \uC804\uC0AC",stories:[{id:"boris:3",at:3,text:"\uB9D0\uC218\uAC00 \uC5C6\uB358 \uBCF4\uB9AC\uAC00 \uC785\uC744 \uC5F4\uC5C8\uB2E4. \uB9C8\uC744 \uACBD\uBE44\uB300\uC5D0 \uB4E4\uC5B4\uAC00\uB824\uB2E4 \uB5A8\uC5B4\uC84C\uB2E4\uACE0. \u201C\uADF8\uB798\uC11C \uC99D\uBA85\uD558\uB7EC \uC654\uC5B4.\u201D",gift:null},{id:"boris:7",at:7,text:"\uBCF4\uB9AC\uB294 \uBC88 \uB3C8\uC758 \uC808\uBC18\uC744 \uACE0\uD5A5\uC758 \uB3D9\uC0DD\uC5D0\uAC8C \uBCF4\uB0B8\uB2E4\uACE0 \uD588\uB2E4. \u201C\uAC11\uC637 \uD55C \uBC8C \uB9DE\uCDB0\uC8FC\uACE0 \uC2F6\uC5B4\uC11C.\u201D",gift:null},{id:"boris:12",at:12,text:"\u201C5\uCE35 \uC544\uB798\uC5D0 \uC624\uB798\uB41C \uAE38\uB4DC\uC758 \uBB38\uC7A5\uC774 \uC788\uB2E4\uB354\uB77C.\u201D \uBCF4\uB9AC\uAC00 \uB0A1\uC740 \uBB38\uC7A5\uC744 \uC2DD\uB2F9 \uBCBD\uC5D0 \uAC78\uC5B4\uB450\uACE0 \uAC14\uB2E4.",gift:{kind:"emblem",name:"\uB0A1\uC740 \uBB38\uC7A5"}}]},{id:"lu",name:"\uB8E8",job:"mage",race:"human",likes:["\uB2EC\uCF64","\uC0C1\uD07C"],dislikes:["\uCFF0\uCFF0"],intro:"\uAE4C\uCE60\uD55C \uB9C8\uBC95\uC0AC",stories:[{id:"lu:3",at:3,text:"\uB8E8\uB294 \uB9C8\uBC95 \uD559\uAD50\uB97C \uC911\uD1F4\uD588\uB2E4\uACE0 \uD588\uB2E4. \uC2DC\uD5D8 \uB54C\uB9C8\uB2E4 \uBD88\uAF43\uC774 \uC5C9\uB6B1\uD55C \uACF3\uC73C\uB85C \uD280\uC5C8\uB2E4\uB098. \u201C\uADF8\uB798\uB3C4 \uBD88\uC740 \uB0B4\uAC00 \uC81C\uC77C \uC798 \uC54C\uC544.\u201D",gift:null},{id:"lu:7",at:7,text:"\uB8E8\uAC00 \uBAB0\uB798 \uB514\uC800\uD2B8 \uB808\uC2DC\uD53C\uB97C \uBB3C\uC5C8\uB2E4. \uB358\uC804\uC5D0\uC11C \uB9CC\uB09C \uCE5C\uAD6C\uC5D0\uAC8C \uC8FC\uACE0 \uC2F6\uB2E4\uACE0. \uB204\uAD6C\uB0D0\uACE0 \uBB3B\uC790 \uC5BC\uAD74\uC774 \uBE68\uAC1C\uC84C\uB2E4.",gift:null},{id:"lu:12",at:12,text:"\u201C\uC2EC\uC5F0\uC5D0\uB294 \uBE5B\uC744 \uBA39\uB294 \uAF43\uC774 \uC788\uB300.\u201D \uB8E8\uAC00 \uC2A4\uD06C\uB864 \uD55C \uC7A5\uC744 \uC120\uBC18\uC5D0 \uC62C\uB824\uB450\uC5C8\uB2E4. \uC8FC\uBC29\uC774 \uC870\uAE08 \uB354 \uD658\uD574\uC84C\uB2E4.",gift:{kind:"scroll",name:"\uBE5B\uC758 \uC2A4\uD06C\uB864"}}]},{id:"jwi",name:"\uC950",job:"rogue",race:"beast",likes:["\uBC14\uC0AD","\uC54C\uC2F8"],dislikes:["\uC2DC\uC6D0"],intro:"\uACFC\uBB35\uD55C \uB3C4\uC801",stories:[{id:"jwi:3",at:3,text:"\uC950\uB294 \uC774\uB984\uC774 \uC5C6\uC5B4\uC11C \uB2E4\uB4E4 \uADF8\uB807\uAC8C \uBD80\uB978\uB2E4\uACE0 \uD588\uB2E4. \uC7A5\uBD80\uC5D0 \uC801\uC5B4 \uB2EC\uB77C\uACE0, \uC791\uAC8C \uC18D\uC0AD\uC600\uB2E4.",gift:null},{id:"jwi:7",at:7,text:"\uC950\uAC00 \uCC98\uC74C\uC73C\uB85C \uC6C3\uC5C8\uB2E4. \uBC14\uC0AD\uD55C \uD280\uAE40\uC744 \uBA39\uC744 \uB54C\uB9CC \uB098\uC624\uB294 \uD45C\uC815\uC774\uB77C\uB294 \uAC78 \uC54C\uAC8C \uB410\uB2E4.",gift:null},{id:"jwi:12",at:12,text:"\uC950\uAC00 \uB0A1\uC740 \uC5F4\uC1E0 \uD558\uB098\uB97C \uB0B4\uB824\uB193\uC558\uB2E4. \u201C\uC5B8\uC820\uAC00 \uB9DE\uB294 \uBB38\uC744 \uCC3E\uC73C\uBA74, \uC5EC\uAE30\uC11C \uC81C\uC77C \uBA3C\uC800 \uB9D0\uD560\uAC8C.\u201D",gift:{kind:"key",name:"\uB0A1\uC740 \uC5F4\uC1E0"}}]},{id:"amel",name:"\uC544\uBA5C",job:"priest",race:"elf",likes:["\uB2F4\uBC31","\uD5A5\uAE0B"],dislikes:["\uC5BC\uD070"],intro:"\uC2E0\uC804 \uCD9C\uC2E0 \uC0AC\uC81C",stories:[{id:"amel:3",at:3,text:"\uC544\uBA5C\uC740 \uC2E0\uC804\uC5D0\uC11C \uC694\uB9AC \uB2F9\uBC88\uC774\uC5C8\uB2E4\uACE0 \uD588\uB2E4. \uADF8\uB798\uC11C\uC778\uC9C0 \uAC04\uC744 \uBCF4\uB294 \uC190\uB180\uB9BC\uC774 \uC608\uC0AC\uB86D\uC9C0 \uC54A\uB2E4.",gift:null},{id:"amel:7",at:7,text:"\u201C\uB2E4\uCE5C \uC0AC\uB78C\uC740 \uBA3C\uC800 \uBA39\uC5EC\uC57C \uD574\uC694.\u201D \uC544\uBA5C\uC774 \uC2DD\uB2F9 \uAD6C\uC11D\uC5D0 \uC791\uC740 \uAD6C\uAE09\uD568\uC744 \uB193\uC544\uC8FC\uC5C8\uB2E4.",gift:{kind:"firstaid",name:"\uC791\uC740 \uAD6C\uAE09\uD568"}},{id:"amel:12",at:12,text:"\uC544\uBA5C\uC774 \uAE30\uB3C4\uBB38 \uD55C \uAD6C\uC808\uC744 \uBA54\uB274\uD310 \uADC0\uD241\uC774\uC5D0 \uC801\uC5B4 \uC8FC\uC5C8\uB2E4. \uC77D\uC73C\uBA74 \uC774\uC0C1\uD558\uAC8C \uBC30\uAC00 \uB4E0\uB4E0\uD574\uC9C4\uB2E4.",gift:{kind:"prayer",name:"\uAE30\uB3C4\uBB38"}}]},{id:"gokgwi",name:"\uACE1\uAD2D\uC774",job:"miner",race:"dwarf",likes:["\uC5BC\uD070","\uC9C4\uD55C"],dislikes:["\uB2EC\uCF64"],intro:"\uB4DC\uC6CC\uD504 \uAD11\uBD80",stories:[{id:"gokgwi:3",at:3,text:"\uACE1\uAD2D\uC774\uB294 \uC99D\uC870\uBD80 \uB54C\uBD80\uD130 \uC774 \uB358\uC804\uC758 \uAD11\uB9E5\uC744 \uCE90\uB358 \uC9D1\uC548\uC774\uB77C\uACE0 \uD588\uB2E4. \uAD6C\uC870\uB97C \uB208 \uAC10\uACE0\uB3C4 \uC548\uB2E4\uACE0.",gift:null},{id:"gokgwi:7",at:7,text:"\uACE1\uAD2D\uC774\uAC00 \uAD11\uC11D \uD55C \uB369\uC774\uB97C \uC2DD\uD0C1 \uB2E4\uB9AC \uBC11\uC5D0 \uAD34\uC5B4 \uC8FC\uC5C8\uB2E4. \uC2DD\uD0C1\uC774 \uB354 \uC774\uC0C1 \uD754\uB4E4\uB9AC\uC9C0 \uC54A\uB294\uB2E4.",gift:{kind:"ore",name:"\uBC1B\uCE68 \uAD11\uC11D"}},{id:"gokgwi:12",at:12,text:"\u201C4\uCE35 \uC544\uB798\uB294 \uAE38\uC774 \uC544\uB2C8\uB77C \uBE48 \uACF5\uAC04\uC774\uC57C.\u201D \uACE1\uAD2D\uC774\uAC00 \uCC98\uC74C\uC73C\uB85C \uC9C4\uC9C0\uD55C \uC5BC\uAD74\uC744 \uD588\uB2E4.",gift:null}]},{id:"sol",name:"\uC194",job:"archer",race:"halfling",likes:["\uC2DC\uC6D0","\uACE0\uC18C"],dislikes:["\uC9ED\uC9E4"],intro:"\uC0AC\uB0E5\uAFBC \uC9D1\uC548\uC758 \uB9C9\uB0B4",stories:[{id:"sol:3",at:3,text:"\uC194\uC740 \uC0AC\uB0E5\uAFBC \uC9D1\uC548\uC758 \uB9C9\uB0B4\uB77C\uACE0 \uD588\uB2E4. \uC0AC\uB0E5\uAC10\uC744 \uB208\uC73C\uB85C \uB530\uB77C\uAC00\uB294 \uBC84\uB987\uC774 \uC811\uAC1D \uC911\uC5D0\uB3C4 \uB098\uC628\uB2E4.",gift:null},{id:"sol:7",at:7,text:"\uC194\uC774 \uD654\uC0B4\uCD09 \uD558\uB098\uB97C \uB193\uACE0 \uAC14\uB2E4. \u201C\uB3CC\uC544\uC624\uB294 \uAE38\uC5D0 \uC4F8 \uB9CC\uD55C \uC7AC\uB8CC\uB97C \uCC59\uACA8 \uC62C\uAC8C. \uB0B4 \uBAAB\uC758 \uBC25\uC740 \uC5EC\uAE30 \uB0A8\uACA8 \uB46C.\u201D",gift:{kind:"arrow",name:"\uD654\uC0B4\uCD09"}},{id:"sol:12",at:12,text:"\uC194\uC774 \uB358\uC804 \uC9C0\uB3C4\uB97C \uD3BC\uCCD0 \uBCF4\uC600\uB2E4. \uC544\uC9C1 \uC544\uBB34\uB3C4 \uAC00\uBCF4\uC9C0 \uBABB\uD55C \uCE78\uC774 5\uCE35 \uD55C\uAC00\uC6B4\uB370\uC5D0 \uBE44\uC5B4 \uC788\uB2E4.",gift:{kind:"map",name:"\uB358\uC804 \uC9C0\uB3C4"}}]}]};var zi=zc.regulars,je={min:0,max:12,thresholds:[3,7,12],resultGain:{great:2,good:1,narrow:0,hurt:0},tasteGoodAt:3,tasteGoodGain:1,tasteBadAt:0,tasteBadGain:-1,declineGain:-1,promiseAt:5,promiseChance:.7,promiseAmount:2,hintsAt:7,maxBonus:1,regularsPerDay:2,maxLevel:6,firstDay:["boris","amel","jwi"],trialDay:["boris","amel","sol"],trialAffinity:{boris:2,amel:6,sol:11},trialGold:240,trialStock:20,trialStove:2},Do=Object.fromEntries(zi.map(n=>[n.id,n])),Vi=Object.fromEntries(zi.flatMap(n=>n.stories.map(e=>[e.id,{...e,guestName:n.name,regularId:n.id}])));var Gi={version:4,burners:1,maxGuests:6},Ut={gold:50,stock:{flour:4,potato:3,meat:2,garlic:2,salt:2},discovered:{flour:!0,potato:!0,meat:!0},seatCosts:[70,130,200],stoveCosts:[90,180],shelfCost:120,baseShelf:1,maxShelf:2},Vc=["\uBAB8\uC774 \uD55C\uACB0 \uAC00\uBCCD\uB2E4. \uC624\uB298\uC740 \uC798 \uD480\uB9B4 \uAC83 \uAC19\uC544.","\uB4E0\uB4E0\uD558\uAE34 \uD55C\uB370\u2026 \uBB54\uAC00 \uD558\uB098 \uBAA8\uC790\uB780 \uB290\uB08C\uC774\uC57C.","\uC74C\u2026 \uC870\uAE08 \uBD88\uC548\uD558\uC9C0\uB9CC \uC77C\uB2E8 \uAC00\uBCFC\uAC8C.","\uC774\uAC78\uB85C \uAD1C\uCC2E\uC744\uAE4C. \uC194\uC9C1\uD788 \uC790\uC2E0\uC740 \uC5C6\uB294\uB370."],Gc={poison:["\uC548\uAC1C","\uB3C5\uAE30","\uC18D"],fire:["\uC5F4\uAE30","\uB728\uAC70\uC6B4","\uC5F4\uC774","\uBAA9\uC774"],cold:["\uC190\uB05D","\uD55C\uAE30","\uC11C\uB9AC"],dark:["\uD683\uBD88","\uC5B4\uB460","\uCE84\uCE84"],vital:["\uB3CC\uC544\uC624\uB294 \uAE38","\uD798","\uBA3C \uAE38","\uD5C8\uAE30"],sense:["\uB538\uAE4D","\uD568\uC815","\uB08C\uC0C8","\uAE38\uBC14\uB2E5"],tired:["\uB2E4\uB9AC\uAC00","\uAE30\uC6B4","\uC9C0\uCCD0","\uC228"],well:["\uBAB8\uC740 \uAC00\uBFD0\uD574","\uBC1C\uAC78\uC74C","\uD798"]},As={waiting:"\uC8FC\uBB38 \uC900\uBE44",cooking:"\uC870\uB9AC \uC911",ready:"\uC644\uC131 \xB7 \uC218\uB839 \uB300\uAE30",carrying:"\uBC30\uB2EC \uC911",served:"\uBC30\uC6C5 \uB300\uAE30",departed:"\uCD9C\uBC1C",declined:"\uADC0\uAC00"},Et=n=>!["departed","declined"].includes(n.status),Sr=n=>({x:-6.65,z:-1.8+n*1.5}),Rs={oven:{name:"\uC7A5\uC791 \uB3CC\uD654\uB355",verb:"\uAD7D\uAE30",tip:"\uB2EC\uAD70 \uB3CC\uD310\uACFC \uC7A5\uC791\uBD88\uB85C \uCC9C\uCC9C\uD788 \uAD7D\uC2B5\uB2C8\uB2E4."},pot:{name:"\uBB34\uC1E0 \uC1A5",verb:"\uB053\uC774\uAE30",tip:"\uC0BC\uBC1C\uC774\uC5D0 \uAC74 \uBB34\uC1E0 \uC1A5\uC5D0 \uC7AC\uB8CC\uB97C \uB123\uACE0 \uB053\uC785\uB2C8\uB2E4."},pan:{name:"\uBB34\uC1E0 \uD32C",verb:"\uC775\uD788\uAE30",tip:"\uC7A5\uC791 \uD654\uB85C \uC704 \uBB34\uC1E0 \uD32C\uC5D0\uC11C \uACE0\uB974\uAC8C \uC775\uD799\uB2C8\uB2E4."},bowl:{name:"\uB098\uBB34 \uC870\uB9AC \uBCFC",verb:"\uC11E\uAE30",tip:"\uB098\uBB34 \uBCFC\uC5D0 \uC7AC\uB8CC\uB97C \uB2F4\uACE0 \uC8FC\uAC71\uC73C\uB85C \uC11E\uC2B5\uB2C8\uB2E4."}},Uo={bread:"oven",herbpie:"oven",carrotcake:"oven",moldgratin:"oven",stew:"pot",shroomsoup:"pot",shrimpstew:"pot",inkrisotto:"pot",abyss:"pot",grill:"pan",batfry:"pan",pepperfry:"pan",tailgrill:"pan",vsaltpotato:"pan",jellysalad:"bowl",mosssoup:"bowl",mossmuch:"bowl",parfait:"bowl",bloomsalad:"bowl"},Hc={tired:["\uACC4\uB2E8\uC744 \uC624\uB974\uB294\uB370 \uB2E4\uB9AC\uAC00 \uD6C4\uB4E4\uAC70\uB838\uC5B4. \uB3CC\uC544\uC624\uB294 \uAE38\uAE4C\uC9C0 \uBC84\uD2F8 \uB9CC\uD55C \uAC78 \uBA39\uACE0 \uC2F6\uC5B4.","\uBC30\uB0AD\uC774 \uD3C9\uC18C\uBCF4\uB2E4 \uBB34\uAC81\uAC8C \uB290\uAEF4\uC838. \uC544\uC9C1 \uAC08 \uAE38\uC774 \uB0A8\uC558\uB294\uB370 \uAE30\uC6B4\uC774 \uC5C6\uC5B4.","\uC228 \uC880 \uB3CC\uB824\uB3C4 \uB420\uAE4C. \uC624\uB298\uC740 \uC870\uAE08\uB9CC \uAC78\uC5B4\uB3C4 \uC9C0\uCCD0 \uBC84\uB9AC\uB124."],poison:["\uC9C0\uB09C\uBC88 \uC548\uAC1C\uB97C \uB9C8\uC2E0 \uB4A4\uB85C \uC18D\uC774 \uC88B\uC9C0 \uC54A\uC558\uC5B4. \uC774\uBC88 \uAE38\uC740 \uBBF8\uB9AC \uB300\uBE44\uD558\uACE0 \uC2F6\uC5B4.","\uCD95\uCD95\uD55C \uC232\uC73C\uB85C \uAC00\uC57C \uD574. \uB3C5\uAE30 \uB54C\uBB38\uC5D0 \uB610 \uC18D\uC744 \uBC84\uB9B4\uAE4C \uAC71\uC815\uC774\uC57C.","\uBAA9\uC5D0 \uB0A8\uC740 \uC548\uAC1C \uB0C4\uC0C8\uAC00 \uC78A\uD788\uC9C8 \uC54A\uC544. \uC18D\uC744 \uC9C0\uCF1C \uC904 \uD55C \uB07C\uAC00 \uC788\uC744\uAE4C."],fire:["\uBAA9\uC774 \uC790\uAFB8 \uB9D0\uB77C. \uC544\uB798\uCABD \uC5F4\uAE30\uB97C \uACAC\uB51C \uC900\uBE44\uB97C \uD558\uACE0 \uC2F6\uC5B4.","\uB728\uAC70\uC6B4 \uBC14\uB2E5\uC744 \uB2E4\uC2DC \uC9C0\uB098\uC57C \uD574. \uBC8C\uC368 \uC5BC\uAD74\uC5D0 \uC5F4\uC774 \uC624\uB974\uB294 \uAE30\uBD84\uC774\uC57C.","\uD55C \uAC78\uC74C\uB9C8\uB2E4 \uC5F4\uAE30\uAC00 \uC62C\uB77C\uC624\uB358 \uAE38\uC774\uC5C8\uC5B4. \uC774\uBC88\uC5D4 \uB300\uBE44\uD558\uACE0 \uB0B4\uB824\uAC00\uB824\uACE0."],cold:["\uC190\uB05D\uC774 \uC2DC\uB824\uC11C \uC7A5\uBE44\uB97C \uB193\uCE60 \uBED4\uD588\uC5B4. \uBAB8\uC744 \uB365\uD790 \uD55C \uB07C\uAC00 \uD544\uC694\uD574.","\uC637\uC744 \uAEF4\uC785\uC5B4\uB3C4 \uD55C\uAE30\uAC00 \uD30C\uACE0\uB4E4\uB354\uB77C. \uC18D\uBD80\uD130 \uB530\uB73B\uD588\uC73C\uBA74 \uD574.","\uC544\uB798\uC5D0 \uC11C\uB9AC\uAC00 \uB354 \uC9D9\uC5B4\uC84C\uB300. \uB2E4\uC2DC \uC190\uC774 \uC5BC\uAE4C \uBD10 \uAC71\uC815\uC774\uC57C."],dark:["\uD683\uBD88\uC774 \uAEBC\uC9C4 \uB4A4\uB85C \uC5B4\uB460\uC774 \uC790\uAFB8 \uC2E0\uACBD \uC4F0\uC5EC. \uC55E\uC744 \uB193\uCE58\uACE0 \uC2F6\uC9C0 \uC54A\uC544.","\uC544\uB798\uB85C \uB0B4\uB824\uAC00\uBA74 \uB208\uC55E\uC774 \uCE84\uCE84\uD574\uC838. \uAE38\uC744 \uC798 \uC0B4\uD544 \uC900\uBE44\uAC00 \uD544\uC694\uD574.","\uD76C\uBBF8\uD55C \uBD88\uBE5B\uB9CC \uBBFF\uACE0 \uAC00\uB824\uB2C8 \uBD88\uC548\uD574. \uC5B4\uB460\uC5D0 \uB300\uBE44\uD560 \uC218 \uC788\uC744\uAE4C."],vital:["\uAC08 \uB54C\uB294 \uAD1C\uCC2E\uC740\uB370 \uB3CC\uC544\uC624\uB294 \uAE38\uC774 \uAE38\uC5B4. \uB05D\uAE4C\uC9C0 \uAC78\uC744 \uD798\uC744 \uB0A8\uAE30\uACE0 \uC2F6\uC5B4.","\uC624\uB298\uC740 \uBA3C \uAE38\uC744 \uAC00\uC57C \uD574. \uD5C8\uAE30\uC9C4 \uCC44 \uB3CC\uC544\uC624\uACE0 \uC2F6\uC9C0\uB294 \uC54A\uC544.","\uC544\uC9C1 \uBC1C\uAC78\uC74C\uC740 \uAD1C\uCC2E\uC544. \uADF8\uB798\uB3C4 \uADC0\uD658\uD560 \uBAAB\uAE4C\uC9C0 \uB4E0\uB4E0\uD788 \uBA39\uC5B4 \uB450\uB824\uACE0."],sense:["\uBC1C\uBC11\uC5D0\uC11C \uB538\uAE4D \uC18C\uB9AC\uAC00 \uB09C \uB4A4\uB85C \uAE34\uC7A5\uC774 \uD480\uB9AC\uC9C8 \uC54A\uC544. \uAC10\uAC01\uC744 \uAE68\uC6CC \uB450\uACE0 \uC2F6\uC5B4.","\uAE38\uBC14\uB2E5\uC774 \uC218\uC0C1\uD574\uC11C \uC2E0\uACBD\uC774 \uACE4\uB450\uC11C \uC788\uC5B4. \uC791\uC740 \uB08C\uC0C8\uB3C4 \uB193\uCE58\uAE30 \uC2EB\uC5B4.","\uC624\uB298 \uAC08 \uAE38\uC5D0\uB294 \uD568\uC815\uC774 \uB9CE\uB300. \uD55C\uB208\uD314\uC9C0 \uC54A\uB3C4\uB85D \uC900\uBE44\uD558\uACE0 \uC2F6\uC5B4."],well:["\uBAB8\uC740 \uAC00\uBFD0\uD574. \uB3CC\uC544\uC62C \uB54C\uB3C4 \uC774\uB7AC\uC73C\uBA74 \uC88B\uACA0\uC5B4. \uD55C \uB07C \uBD80\uD0C1\uD560\uAC8C.","\uC624\uB298\uC740 \uC228\uB3C4 \uACE0\uB974\uACE0 \uBC1C\uAC78\uC74C\uB3C4 \uAD1C\uCC2E\uC544. \uB4E0\uB4E0\uD788 \uCC59\uACA8 \uBA39\uACE0 \uCD9C\uBC1C\uD558\uB824\uACE0.","\uC544\uC9C1 \uD798\uC740 \uCDA9\uBD84\uD574. \uADF8\uB807\uB2E4\uACE0 \uB3CC\uC544\uC624\uB294 \uAE38\uC758 \uBAAB\uC744 \uBE7C\uB193\uC744 \uC21C \uC5C6\uC9C0."]};function No(n,e,t){let i=Ct.filter(c=>e[c]>0).sort((c,l)=>e[l]-e[c])[0],s=n.tired?"tired":i||"well",r=Hc[s],a=[...n.id+t].reduce((c,l)=>c+l.charCodeAt(0),0)%r.length;return r[a]===n.lastOpening&&(a=(a+1)%r.length),{key:s,text:r[a]}}function Er(n){var e;for(let t of Object.values(n.guests))t.regularId??(t.regularId=null),t.level??(t.level=1),t.xp??(t.xp=0);for(let t of zi){let i="regular-"+t.id;(e=n.guests)[i]??(e[i]={id:i,regularId:t.id,name:t.name,job:t.job,race:t.race,likes:[...t.likes],dislikes:[...t.dislikes],knownLikes:[t.likes[0]],knownDislikes:[],tired:!1,floor:1,restUntil:0,visits:0,injured:!1,affinity:0,stories:[],level:1,xp:0});let s=n.guests[i];s.affinity=Math.min(je.max,Math.max(je.min,s.affinity??0)),s.stories??(s.stories=[])}}function Fo(n,e){let t=on(n)*e.reduce((i,[,s])=>i+s,0);for(let[i,s]of e)if(t-=s,t<0)return i;return e.at(-1)?.[0]}function Xc(n){let e=zi.map(t=>{let i=n.guests["regular-"+t.id],s=t.stories.find(r=>!i.stories.includes(r.id));return{...i,intro:t.intro,nextAt:s?.at??null,remaining:s?Math.max(0,s.at-i.affinity):0,unlocked:t.stories.filter(r=>i.stories.includes(r.id))}});return{regulars:e,keepsakes:e.flatMap(t=>t.unlocked.filter(i=>i.gift).map(i=>({id:i.id,name:i.gift.name,kind:i.gift.kind,guestName:t.name}))),passers:Object.values(n.guests).filter(t=>!t.regularId)}}function kd(n,e,t){var c;if(!e.regularId)return;let i=e.affinity,s=je.resultGain[t.result.tier]||0,r=t.taste?.score,o=r>=je.tasteGoodAt?je.tasteGoodGain:r===je.tasteBadAt?je.tasteBadGain:0;e.affinity=Math.max(je.min,Math.min(je.max,i+s+o));let a=t.relationship={before:i,after:e.affinity,gain:e.affinity-i,resultGain:s,tasteGain:o,stories:[],levelBefore:e.level,level:e.level,map:null};["great","good"].includes(t.result.tier)&&e.level<je.maxLevel&&(e.xp++,e.xp>=Math.ceil(e.level/2)&&(e.level++,e.xp=0,a.level=e.level));for(let l of Do[e.regularId].stories)if(!(e.affinity<l.at||e.stories.includes(l.id))&&(e.stories.push(l.id),a.stories.push(l.id),l.gift?.kind==="map")){let h=n.floor<Gt.authoredFloors?[n.floor+1]:Array.from({length:Gt.authoredFloors},(u,p)=>p+1);for(let u of h){let p=Ct.find(d=>(Z.FLOORS[u].haz[d]||0)>0&&n.known[u]?.[d]===void 0);if(p){(c=n.known)[u]??(c[u]={}),n.known[u][p]=Z.FLOORS[u].haz[p],a.map={floor:u,axis:p,value:Z.FLOORS[u].haz[p]};break}}}}var br=n=>Object.fromEntries(Ct.map(e=>[e,Math.max(0,n?.[e]||0)]));function Oo(n,e){let t=br(Z.FLOORS[e.floor].haz);n.anomaly?.floor===e.floor&&t[n.anomaly.axis]++,e.tired&&t.vital++;let i=$n[e.race].weak;i&&t[i]++;let s=Z.CLASSES[e.job].res;return t[s]=Math.max(0,t[s]-1),t}function Cs(n,e,t=[],i=null){let s=Z.RECIPES.find(a=>a.id===e),r=br(s?.eff);if(!s)return r;let o=Ct.filter(a=>r[a]>0).sort((a,c)=>r[c]-r[a]);for(let a of o.slice(0,n.stove))r[a]++;for(let a of t)Z.SPICES[a]&&r[Z.SPICES[a].ax]++;return i!==null&&n.seats.includes(i)&&r[_t[i].axis]++,r}function qc(n,e,t=null){let i=br(n),s=br(e),r=!!t&&s.vital-i.vital>=1;t&&!r&&i[t]++;let o=Object.fromEntries(Ct.map(c=>[c,Math.max(0,i[c]-s[c])])),a=Object.values(o).reduce((c,l)=>c+l,0);return{H:i,E:s,gaps:o,deficit:a,tier:a===0?"great":a===1?"good":a===2?"narrow":"hurt",surprise:t,absorbed:r}}function Bd(n,e){let t="guest-"+ ++n.nextGuest,i=[...Object.values(n.guests).map(a=>a.name),...zi.map(a=>a.name)],s=Po.filter(a=>!i.includes(a)),r=Pn(n,s.length?s:Po),o={id:t,name:r,regularId:null,level:1,xp:0,job:Pn(n,Object.keys(Z.CLASSES)),race:Pn(n,Object.keys($n)),tired:n.day>1&&on(n)<.3,floor:e,restUntil:0,visits:0,injured:!1};return n.guests[t]=o,ws(n,o),o}function $c(n="normal",e=Date.now()>>>0){let t={version:1,mode:n,seed:e,day:1,gold:Gt.startGold,phase:"prep",floor:n==="story"?5:1,stove:0,seats:[0,1,2],stock:{flour:12,potato:8,meat:8,shroom:8,herb:6,carrot:6,jelly:4,garlic:8,mint:6,ginger:6,carjuice:8,salt:8,blackpepper:6},discovered:{flour:!0,potato:!0,meat:!0,shroom:!0,herb:!0,carrot:!0,jelly:!0},guests:{},nextGuest:0,nextVisit:0,visits:[],index:0,pending:[],reports:[],reportIndex:0,committedIds:[],wins:{},known:{},income:0,anomaly:null,prepared:null,previewEnabled:!1,endingStep:null,endingTransaction:null,story:{clues:[],processed:[],first:{},queue:[],done:[],injuries:{},requests:{},sceneDay:0,afterReturn:!1},stats:{served:0,returned:0,hurt:0},log:[]};if(t.shelf=Ut.baseShelf,t.spent=0,t.economyVersion=4,n==="normal"&&(t.gold=Ut.gold,t.stock={...Ut.stock},t.discovered={...Ut.discovered}),n==="story"){t.shelf=Ut.maxShelf,t.gold=240,t.stove=1,t.seats=[0,1,2,3,4,5];for(let i of Object.keys(Z.ING))t.stock[i]=8,t.discovered[i]=!0}if(Er(t),n==="relations"){t.shelf=Ut.maxShelf,t.gold=je.trialGold,t.stove=je.trialStove,t.seats=_t.map(i=>i.id);for(let i of[...Object.keys(Z.ING),...Object.keys(Z.SPICES)])t.stock[i]=je.trialStock,t.discovered[i]=!0;for(let[i,s]of Object.entries(je.trialAffinity)){let r=t.guests["regular-"+i];r.affinity=s,r.stories=Do[i].stories.filter(o=>o.at<=s).map(o=>o.id)}}return t}var yt=n=>n.visits[n.index],Zn=n=>n.visits.find(e=>e.status==="carrying");function Tr(n,e){let t=n.visits.findIndex(i=>i.id===e&&Et(i));return n.phase!=="service"||t<0?!1:(n.index=t,!0)}function Wc(n,e){return n.visits.find(t=>Et(t)&&t.seat===e)}function Yc(n){n.shelf??(n.shelf=Gt.shelf),n.spent??(n.spent=0),n.serviceVersion!==Gi.version&&(n.visits.forEach((e,t)=>{t<n.index&&e.status==="served"&&(e.status="departed")}),n.serviceVersion=Gi.version),n.visits.forEach((e,t)=>{if(e.waitSlot??(e.waitSlot=t),e.ingredients??(e.ingredients={}),!e.opening){let i=No(n.guests[e.guestId],Oo(n,n.guests[e.guestId]),e.id);e.opening=i.text,e.openingKey=i.key}}),n.phase==="service"&&!Et(yt(n)||{status:"departed"})&&(n.index=n.visits.findIndex(Et))}var _i=n=>n.guests[yt(n)?.guestId];function Zc(n){if(n.phase!=="prep")return!1;Er(n),n.visits=[],n.index=0,n.income=0,n.serviceVersion=Gi.version;let e=Math.min(Gi.maxGuests,n.seats.length),t=new Set(n.pending.map(c=>c.guestId)),i=Object.values(n.guests).filter(c=>c.restUntil<=n.day&&!t.has(c.id)),s=i.filter(c=>c.regularId).sort((c,l)=>c.visits-l.visits),r=i.filter(c=>!c.regularId).sort((c,l)=>c.visits-l.visits),o=n.day===1?n.mode==="relations"?je.trialDay:je.firstDay:null,a=o?o.map(c=>i.find(l=>l.regularId===c)).filter(Boolean):s.slice(0,je.regularsPerDay);for(;a.length<e;)a.push(r.shift()||Bd(n,n.floor));for(let c=0;c<e;c++){let l=a[c],h=Math.min(n.floor,l.regularId?l.level+1:n.floor);l.floor=n.mode==="story"?n.floor:n.day===1?1:Fo(n,Array.from({length:h},(f,T)=>[T+1,T+1===h?3:1])),l.tired=n.day>1&&on(n)<.3,l.visits++;let u=Oo(n,l),d=Ct.filter(f=>u[f]>0).sort((f,T)=>u[T]-u[f]).slice(0,l.regularId&&l.affinity>=je.hintsAt?2:1).map(f=>Lc[f]);d.length||d.push("\uBCC4\uC77C \uC5C6\uC744 \uAC70\uC57C. \uAC00\uBCCD\uAC8C \uD55C \uBC14\uD034 \uB3CC\uACE0 \uC62C\uAC8C.");let g=l.regularId&&l.affinity>=je.promiseAt&&on(n)<je.promiseChance?Fo(n,Z.FLOORS[l.floor].loot.filter(([f])=>!Z.ING[f].rare)):null,v={id:"visit-"+ ++n.nextVisit,guestId:l.id,seat:null,recipe:null,spices:[],status:"waiting",hints:d,hint:d.join(" / "),promise:g||null,assignment:null};v.waitSlot=c,v.ingredients={};let m=No(l,u,v.id);if(v.opening=m.text,v.openingKey=m.key,l.lastOpening=m.text,l.floor===5&&!n.story.afterReturn){let f=Object.values(n.story.requests).find(T=>!T.done&&!n.visits.some(E=>E.assignment===T.id));f&&(v.assignment=f.id)}n.visits.push(v)}return n.phase="service",!0}function Jc(n,e){let t=yt(n);return n.phase!=="service"||t?.status!=="waiting"||!n.seats.includes(e)||Wc(n,e)&&Wc(n,e)!==t?!1:(t.seat=e,!0)}function Kc(n,e){let t=yt(n);return n.phase!=="service"||t?.status!=="waiting"||t.seat===null||!Z.RECIPES.some(i=>i.id===e)?!1:(t.recipe=e,t.spices=[],t.ingredients={},!0)}function jc(n,e){let t=yt(n);return!t?.recipe||t.status!=="waiting"||!Z.SPICES[e]||t.spices.length>=n.shelf||(n.stock[e]||0)<=t.spices.filter(i=>i===e).length?!1:(t.spices.push(e),!0)}function Hi(n,e,t=[]){if(!e||t.some(s=>!Z.SPICES[s])||t.length>n.shelf)return!1;let i={...e.ing};for(let s of t)i[s]=(i[s]||0)+1;return Object.entries(i).every(([s,r])=>(n.stock[s]||0)>=r)}function Qc(n,e){let t=yt(n),i=Z.RECIPES.find(r=>r.id===t?.recipe);if(n.phase!=="service"||t?.status!=="waiting"||!i?.ing[e])return!1;t.ingredients??(t.ingredients={});let s=t.ingredients[e]||0;return s>=i.ing[e]||s>=(n.stock[e]||0)?!1:(t.ingredients[e]=s+1,!0)}function eh(n,e){let t=yt(n);return n.phase!=="service"||t?.status!=="waiting"||!(t.ingredients?.[e]>0)?!1:(t.ingredients[e]--,!0)}function ko(n){let e=Z.RECIPES.find(t=>t.id===n?.recipe);return!!e&&Object.entries(e.ing).every(([t,i])=>n.ingredients?.[t]===i)}function Bo(n){return n.visits.filter(e=>e.status==="cooking").length<Gi.burners}function th(n,e){let t=yt(n),i=Z.RECIPES.find(s=>s.id===t?.recipe);if(n.phase!=="service"||t?.status!=="waiting"||t.seat===null||!ko(t)||!Bo(n)||!Hi(n,i,t.spices))return!1;for(let[s,r]of Object.entries(i.ing))n.stock[s]-=r;for(let s of t.spices)n.stock[s]--;return t.status="cooking",t.readyAt=e+Gt.cookMs,t.E=Cs(n,i.id,t.spices,t.seat),t.H=Oo(n,_i(n)),!0}function zo(n,e){let t=!1;if(n.phase!=="service")return!1;for(let i of n.visits)i.status==="cooking"&&e>=i.readyAt&&(i.status="ready",t=!0);return t}function nh(n){let e=yt(n);return n.phase!=="service"||e?.status!=="ready"||Zn(n)?!1:(e.status="carrying",!0)}function ih(n){let e=yt(n),t=_i(n);if(n.phase!=="service"||e?.status!=="carrying")return!1;let i=Z.RECIPES.find(u=>u.id===e.recipe),s=on(n)<Gt.surpriseChance?Pn(n,Ct.filter(u=>u!=="vital")):null,r=qc(e.H,e.E,s);ws(n,t);let o=Oc(t,Mr(i,e.spices)),a=kc(i,e.spices,o),c=Bc(t,o),l=r.tier!=="hurt"&&o.score>=3&&on(n)<vn.lootChance?1:0;n.gold+=a,n.income+=a,n.stats.served++,n.pending.push({id:"report-"+e.id,visitId:e.id,guestId:t.id,name:t.name,floor:t.floor,recipe:i.n,assignment:e.assignment,result:r,taste:o,lootBonus:l,promise:e.promise||null,maxAffinityBonus:t.regularId&&t.affinity>=je.max?je.maxBonus:0,day:n.day}),e.status="served",e.price=a,e.taste=o,e.learnedTaste=c;let h=on(n);return e.feel=Vc[Math.max(0,Math.min(3,r.deficit+(h<.15?-1:h>.85?1:0)))],!0}function Vo(n,e=!1){let t=yt(n);if(n.phase!=="service"||!t)return!1;if(e){if(t.status!=="waiting")return!1;t.status="declined";let s=_i(n);if(s.regularId){let r=s.affinity;s.affinity=Math.max(je.min,s.affinity+je.declineGain),t.declineAffinity=s.affinity-r}}else{if(t.status!=="served")return!1;t.status="departed"}let i=n.visits.findIndex(Et);return i<0?n.phase="close":n.index=i,!0}function sh(n){if(n.phase!=="service"||n.visits.some(e=>["cooking","ready","carrying"].includes(e.status)))return!1;for(let e of n.visits)Et(e)&&(e.status=e.status==="served"?"departed":"declined");return n.phase="close",!0}function rh(n){var t,i;if(n.phase!=="close")return[];n.day++,n.spent=0,n.reports=[],n.reportIndex=0;let e=[];for(let s of n.pending){if(n.committedIds.includes(s.id))continue;n.committedIds.push(s.id),n.reports.push(s),n.stats.returned++;let r=n.guests[s.guestId];s.loot={},s.newIngredients=[];let o=(c,l)=>{n.discovered[c]||s.newIngredients.push(c),n.stock[c]=(n.stock[c]||0)+l,n.discovered[c]=!0,s.loot[c]=(s.loot[c]||0)+l};if(s.result.tier==="hurt")n.stats.hurt++,r.restUntil=n.day+Gt.injuryRest;else{let c=Z.FLOORS[s.floor].loot;for(let l=0;l<{great:3,good:2,narrow:1}[s.result.tier]+(s.lootBonus||0)+(s.maxAffinityBonus||0);l++){let h=Fo(n,c);o(h,1)}}["great","good"].includes(s.result.tier)&&(s.promise&&Z.ING[s.promise]&&(o(s.promise,je.promiseAmount),s.promiseDelivered=je.promiseAmount),n.wins[s.floor]=(n.wins[s.floor]||0)+1,s.floor===n.floor&&n.wins[s.floor]>=Gt.unlockWins&&n.floor<Gt.authoredFloors&&(n.floor++,s.newFloor=n.floor));let a=new Set(Z.RECIPES.filter(c=>Object.keys(c.ing).every(l=>n.discovered[l]&&!(s.newIngredients||[]).includes(l))).map(c=>c.id));(t=n.known)[i=s.floor]??(t[i]={}),s.learned=Ct.filter(c=>(Z.FLOORS[s.floor].haz[c]||0)>0&&n.known[s.floor][c]===void 0).sort((c,l)=>(s.result.gaps[l]>0?1:0)-(s.result.gaps[c]>0?1:0)).slice(0,2);for(let c of s.learned)n.known[s.floor][c]=Z.FLOORS[s.floor].haz[c];s.newRecipes=Z.RECIPES.filter(c=>!a.has(c.id)&&Object.keys(c.ing).every(l=>n.discovered[l])).map(c=>c.id),kd(n,r,s),e.push({type:"ReportCommitted",report:s})}return n.pending=[],n.phase=n.reports.length?"report":"prep",n.anomaly=on(n)<.6?{floor:1+Math.floor(on(n)*n.floor),axis:Pn(n,Ct)}:null,n.gold<15&&!Z.RECIPES.some(s=>Hi(n,s))&&(n.stock.flour=(n.stock.flour||0)+4,n.log.push("\uC774\uC6C3 \uC0C1\uC778\uC774 \uBC00\uAC00\uB8E8\uB97C \uBCF4\uD0DC \uC8FC\uC5C8\uB2E4.")),e}function Go(n,e,t=1){let i=Z.ING[e]||Z.SPICES[e];if(!["prep","close"].includes(n.phase)||!i||![1,5].includes(t)||Z.ING[e]?.rare||(Z.ING[e]?.floor||0)>n.floor)return!1;let s=(i.price||8)*(Z.ING[e]?.floor?4:1)*t;return n.gold<s?!1:(n.gold-=s,n.spent=(n.spent||0)+s,n.stock[e]=(n.stock[e]||0)+t,n.discovered[e]=!0,Jn(n),!0)}function wr(n,e,t){if(!["prep","close"].includes(n.phase))return!1;let i=e==="stove"?Ut.stoveCosts[n.stove]:e==="shelf"?n.shelf<Ut.maxShelf&&Ut.shelfCost:e==="seat"?Ut.seatCosts[n.seats.length-3]:null;return!i||n.gold<i||e==="seat"&&(!_t[t]||n.seats.includes(t))?!1:(n.gold-=i,n.spent=(n.spent||0)+i,e==="stove"?n.stove++:e==="shelf"?n.shelf++:n.seats.push(t),Jn(n),!0)}function Jn(n){n.prepared=null}function Ar(n,e,t){let i=Cs(n,e,t,null),s=qc(Yn.selfHazard,i),r=["mark","gap","reply","inner","outer","cooking"],o=Z.RECIPES.find(c=>c.id===e),a=[];return n.previewEnabled||a.push("\uBBF8\uC815\uC778 \uC790\uAE30 \uCC98\uBC29\xB7\uC5D4\uB529 A\uC548 \uC2DC\uD5D8 \uC124\uC815\uC744 \uBA3C\uC800 \uCF1C\uC138\uC694."),r.every(c=>n.story.clues.includes(c))||a.push("\uC0DD\uC874 \uB2F5\uC7A5\uACFC \uC548\uD30E\uC758 \uC9C0\uB3C4, \uC2DD\uC0AC \uAE30\uB85D\uC774 \uD544\uC694\uD569\uB2C8\uB2E4."),n.story.done.includes("ISOLATED_AREA_LOCATED")||a.push("\uC704\uCE58 \uD655\uC778 \uC7A5\uBA74\uC744 \uBA3C\uC800 \uD655\uC778\uD558\uC138\uC694."),n.story.plan||a.push("\uC2DC\uD5D8\uC6A9 \uACBD\uB85C\xB7\uC7A5\uBE44 \uACC4\uD68D \uD655\uC778\uC774 \uD544\uC694\uD569\uB2C8\uB2E4."),(s.deficit||i.vital-Yn.selfHazard.vital<Yn.reserve)&&a.push("\uC2DC\uD5D8 \uACBD\uB85C\uC758 \uC704\uD5D8\uC744 \uBAA8\uB450 \uB9C9\uACE0 \uD65C\uB825 \uC5EC\uC720 1\uC744 \uB0A8\uACA8\uC57C \uD569\uB2C8\uB2E4."),(!Hi(n,o,t)||n.gold<Yn.supplyCost)&&a.push("\uC2DD\uC0AC \uC7AC\uB8CC\uC640 \uC2DC\uD5D8 \uBCF4\uAE09\uBE44 12g\uAC00 \uD544\uC694\uD569\uB2C8\uB2E4."),(n.phase!=="prep"||n.pending.length)&&a.push("\uBAA8\uB4E0 \uADC0\uD658 \uBCF4\uACE0\uB97C \uB9C8\uCE5C \uC601\uC5C5 \uC900\uBE44 \uC2DC\uAC04\uC5D0 \uAC00\uB2A5\uD569\uB2C8\uB2E4."),{ok:!a.length,reasons:a,E:i}}function ah(n,e,t){let i=Ar(n,e,t);return i.ok&&(n.prepared={id:"prep-"+n.day,recipeId:e,spices:[...t],config:Yn.version}),i}function oh(n){let e=n.prepared;if(n.story.afterReturn||n.endingTransaction||!e||e.config!==Yn.version)return!1;if(!Ar(n,e.recipeId,e.spices).ok)return Jn(n),!1;let i=Z.RECIPES.find(s=>s.id===e.recipeId);for(let[s,r]of Object.entries(i.ing))n.stock[s]-=r;for(let s of e.spices)n.stock[s]--;return n.gold-=Yn.supplyCost,n.endingTransaction=e.id,n.endingStep="depart",n.phase="ending",!0}function xi(n,e,t){let i=n.story;i.done.includes(e)||i.queue.some(s=>s.id===e)||i.queue.push({id:e,cause:t,order:i.processed.length+i.queue.length})}function Vd(n,e="facts"){let t=i=>n.story.clues.includes(i);t("mark")&&xi(n,"PARTY_MARK_FOUND",e),t("mark")&&t("reply")&&xi(n,"CURRENT_SURVIVAL_CONFIRMED",e),t("reply")&&t("cooking")&&xi(n,"THEY_KEPT_COOKING",e),["mark","gap","inner","outer"].every(t)&&xi(n,"ISOLATED_AREA_LOCATED",e)}function Rr(n,e){n.story.clues.includes(e)||n.story.clues.push(e),Vd(n,e)}function Ho(n,e){let t=n.story;if(t.processed.includes(e.id))return!1;t.processed.push(e.id);let i=e.result.tier,s={great:"FIRST_GREAT_SUCCESS",good:"FIRST_SUCCESS",narrow:"FIRST_NARROW_ESCAPE",hurt:"FIRST_INJURY"}[i];if(t.first[i]||(t.first[i]=e.id,i==="narrow"&&(t.done.includes("THEY_KEPT_COOKING")||t.afterReturn)||xi(n,s,e.id)),i==="great"&&t.injuries[e.guestId]&&xi(n,"INJURED_GUEST_RETURNS_WELL",e.id),i==="hurt"&&(t.injuries[e.guestId]=e.id),e.result.surprise&&e.result.absorbed&&["great","good"].includes(i)&&xi(n,"RESERVE_HELPED_RETURN",e.id),!t.afterReturn&&e.floor===5){t.clues.includes("mark")||(Rr(n,"mark"),Rr(n,"gap"));let r=t.requests[e.assignment];if(r&&!r.done&&r.sentDay<=e.day)if(r.done=!0,r.reportId=e.id,r.guestId=e.guestId,r.kind==="question")for(let o of["reply","inner","cooking"])Rr(n,o);else Rr(n,"outer")}return!0}function lh(n,e){let t=n.story;if(n.phase!=="prep"||t.afterReturn||!t.clues.includes("gap")||!["question","survey"].includes(e)||Object.values(t.requests).some(s=>s.kind===e))return!1;let i=e+"-"+n.day;return t.requests[i]={id:i,kind:e,sentDay:n.day,done:!1},!0}function Wo(n){return n.story.sceneDay===n.day?null:n.story.queue.filter(e=>!xn[e.id].after||n.story.done.includes(xn[e.id].after)).sort((e,t)=>xn[e.id].priority-xn[t.id].priority||e.order-t.order||e.id.localeCompare(t.id))[0]||null}function ch(n,e){let t=Wo(n);return!t||t.id!==e?!1:(n.story.done.push(e),n.story.queue=n.story.queue.filter(i=>i.id!==e),n.story.sceneDay=n.day,e==="THEY_KEPT_COOKING"&&(n.story.queue=n.story.queue.filter(i=>i.id!=="FIRST_NARROW_ESCAPE")),!0)}function hh(n){n.story.afterReturn=!0,n.story.queue=[],n.endingStep="complete",n.phase="prep",n.prepared=null}/**
 * @license
 * Copyright 2010-2025 Three.js Authors
 * SPDX-License-Identifier: MIT
 */var Nh=0,Tl=1,Fh=2;var wl=1,ba=2,En=3,Bn=0,Bt=1,qt=2,Hn=0,Ei=1,Al=2,Rl=3,Cl=4,Oh=5,ri=100,kh=101,Bh=102,zh=103,Vh=104,Gh=200,Hh=201,Wh=202,Xh=203,Kr=204,jr=205,qh=206,$h=207,Yh=208,Zh=209,Jh=210,Kh=211,jh=212,Qh=213,eu=214,Ea=0,Ta=1,wa=2,Ti=3,Aa=4,Ra=5,Ca=6,Ia=7,Il=0,tu=1,nu=2,Wn=0,iu=1,su=2,ru=3,Pa=4,au=5,ou=6,lu=7;var Pl=300,Pi=301,Li=302,La=303,Da=304,lr=306,Qr=1e3,ii=1001,ea=1002,rn=1003,cu=1004;var cr=1005;var dn=1006,Ua=1007;var ui=1008;var mn=1009,Ll=1010,Dl=1011,_s=1012,Na=1013,di=1014,Tn=1015,xs=1016,Fa=1017,Oa=1018,vs=1020,Ul=35902,Nl=35899,Fl=1021,Ol=1022,an=1023,os=1026,ys=1027,kl=1028,ka=1029,Bl=1030,Ba=1031;var za=1033,hr=33776,ur=33777,dr=33778,fr=33779,Va=35840,Ga=35841,Ha=35842,Wa=35843,Xa=36196,qa=37492,$a=37496,Ya=37808,Za=37809,Ja=37810,Ka=37811,ja=37812,Qa=37813,eo=37814,to=37815,no=37816,io=37817,so=37818,ro=37819,ao=37820,oo=37821,lo=36492,co=36494,ho=36495,uo=36283,fo=36284,po=36285,mo=36286;var ks=2300,ta=2301,Jr=2302,_l=2400,xl=2401,vl=2402;var hu=3200,uu=3201;var zl=0,du=1,Xn="",Ft="srgb",wi="srgb-linear",Bs="linear",et="srgb";var bi=7680;var yl=519,fu=512,pu=513,mu=514,Vl=515,gu=516,_u=517,xu=518,vu=519,na=35044;var Gl="300 es",un=2e3,zs=2001;var zn=class{addEventListener(e,t){this._listeners===void 0&&(this._listeners={});let i=this._listeners;i[e]===void 0&&(i[e]=[]),i[e].indexOf(t)===-1&&i[e].push(t)}hasEventListener(e,t){let i=this._listeners;return i===void 0?!1:i[e]!==void 0&&i[e].indexOf(t)!==-1}removeEventListener(e,t){let i=this._listeners;if(i===void 0)return;let s=i[e];if(s!==void 0){let r=s.indexOf(t);r!==-1&&s.splice(r,1)}}dispatchEvent(e){let t=this._listeners;if(t===void 0)return;let i=t[e.type];if(i!==void 0){e.target=this;let s=i.slice(0);for(let r=0,o=s.length;r<o;r++)s[r].call(this,e);e.target=null}}},It=["00","01","02","03","04","05","06","07","08","09","0a","0b","0c","0d","0e","0f","10","11","12","13","14","15","16","17","18","19","1a","1b","1c","1d","1e","1f","20","21","22","23","24","25","26","27","28","29","2a","2b","2c","2d","2e","2f","30","31","32","33","34","35","36","37","38","39","3a","3b","3c","3d","3e","3f","40","41","42","43","44","45","46","47","48","49","4a","4b","4c","4d","4e","4f","50","51","52","53","54","55","56","57","58","59","5a","5b","5c","5d","5e","5f","60","61","62","63","64","65","66","67","68","69","6a","6b","6c","6d","6e","6f","70","71","72","73","74","75","76","77","78","79","7a","7b","7c","7d","7e","7f","80","81","82","83","84","85","86","87","88","89","8a","8b","8c","8d","8e","8f","90","91","92","93","94","95","96","97","98","99","9a","9b","9c","9d","9e","9f","a0","a1","a2","a3","a4","a5","a6","a7","a8","a9","aa","ab","ac","ad","ae","af","b0","b1","b2","b3","b4","b5","b6","b7","b8","b9","ba","bb","bc","bd","be","bf","c0","c1","c2","c3","c4","c5","c6","c7","c8","c9","ca","cb","cc","cd","ce","cf","d0","d1","d2","d3","d4","d5","d6","d7","d8","d9","da","db","dc","dd","de","df","e0","e1","e2","e3","e4","e5","e6","e7","e8","e9","ea","eb","ec","ed","ee","ef","f0","f1","f2","f3","f4","f5","f6","f7","f8","f9","fa","fb","fc","fd","fe","ff"];var Xo=Math.PI/180,ia=180/Math.PI;function si(){let n=Math.random()*4294967295|0,e=Math.random()*4294967295|0,t=Math.random()*4294967295|0,i=Math.random()*4294967295|0;return(It[n&255]+It[n>>8&255]+It[n>>16&255]+It[n>>24&255]+"-"+It[e&255]+It[e>>8&255]+"-"+It[e>>16&15|64]+It[e>>24&255]+"-"+It[t&63|128]+It[t>>8&255]+"-"+It[t>>16&255]+It[t>>24&255]+It[i&255]+It[i>>8&255]+It[i>>16&255]+It[i>>24&255]).toLowerCase()}function qe(n,e,t){return Math.max(e,Math.min(t,n))}function Hd(n,e){return(n%e+e)%e}function qo(n,e,t){return(1-t)*n+t*e}function Mn(n,e){switch(e.constructor){case Float32Array:return n;case Uint32Array:return n/4294967295;case Uint16Array:return n/65535;case Uint8Array:return n/255;case Int32Array:return Math.max(n/2147483647,-1);case Int16Array:return Math.max(n/32767,-1);case Int8Array:return Math.max(n/127,-1);default:throw new Error("Invalid component type.")}}function nt(n,e){switch(e.constructor){case Float32Array:return n;case Uint32Array:return Math.round(n*4294967295);case Uint16Array:return Math.round(n*65535);case Uint8Array:return Math.round(n*255);case Int32Array:return Math.round(n*2147483647);case Int16Array:return Math.round(n*32767);case Int8Array:return Math.round(n*127);default:throw new Error("Invalid component type.")}}var Ue=class n{constructor(e=0,t=0){n.prototype.isVector2=!0,this.x=e,this.y=t}get width(){return this.x}set width(e){this.x=e}get height(){return this.y}set height(e){this.y=e}set(e,t){return this.x=e,this.y=t,this}setScalar(e){return this.x=e,this.y=e,this}setX(e){return this.x=e,this}setY(e){return this.y=e,this}setComponent(e,t){switch(e){case 0:this.x=t;break;case 1:this.y=t;break;default:throw new Error("index is out of range: "+e)}return this}getComponent(e){switch(e){case 0:return this.x;case 1:return this.y;default:throw new Error("index is out of range: "+e)}}clone(){return new this.constructor(this.x,this.y)}copy(e){return this.x=e.x,this.y=e.y,this}add(e){return this.x+=e.x,this.y+=e.y,this}addScalar(e){return this.x+=e,this.y+=e,this}addVectors(e,t){return this.x=e.x+t.x,this.y=e.y+t.y,this}addScaledVector(e,t){return this.x+=e.x*t,this.y+=e.y*t,this}sub(e){return this.x-=e.x,this.y-=e.y,this}subScalar(e){return this.x-=e,this.y-=e,this}subVectors(e,t){return this.x=e.x-t.x,this.y=e.y-t.y,this}multiply(e){return this.x*=e.x,this.y*=e.y,this}multiplyScalar(e){return this.x*=e,this.y*=e,this}divide(e){return this.x/=e.x,this.y/=e.y,this}divideScalar(e){return this.multiplyScalar(1/e)}applyMatrix3(e){let t=this.x,i=this.y,s=e.elements;return this.x=s[0]*t+s[3]*i+s[6],this.y=s[1]*t+s[4]*i+s[7],this}min(e){return this.x=Math.min(this.x,e.x),this.y=Math.min(this.y,e.y),this}max(e){return this.x=Math.max(this.x,e.x),this.y=Math.max(this.y,e.y),this}clamp(e,t){return this.x=qe(this.x,e.x,t.x),this.y=qe(this.y,e.y,t.y),this}clampScalar(e,t){return this.x=qe(this.x,e,t),this.y=qe(this.y,e,t),this}clampLength(e,t){let i=this.length();return this.divideScalar(i||1).multiplyScalar(qe(i,e,t))}floor(){return this.x=Math.floor(this.x),this.y=Math.floor(this.y),this}ceil(){return this.x=Math.ceil(this.x),this.y=Math.ceil(this.y),this}round(){return this.x=Math.round(this.x),this.y=Math.round(this.y),this}roundToZero(){return this.x=Math.trunc(this.x),this.y=Math.trunc(this.y),this}negate(){return this.x=-this.x,this.y=-this.y,this}dot(e){return this.x*e.x+this.y*e.y}cross(e){return this.x*e.y-this.y*e.x}lengthSq(){return this.x*this.x+this.y*this.y}length(){return Math.sqrt(this.x*this.x+this.y*this.y)}manhattanLength(){return Math.abs(this.x)+Math.abs(this.y)}normalize(){return this.divideScalar(this.length()||1)}angle(){return Math.atan2(-this.y,-this.x)+Math.PI}angleTo(e){let t=Math.sqrt(this.lengthSq()*e.lengthSq());if(t===0)return Math.PI/2;let i=this.dot(e)/t;return Math.acos(qe(i,-1,1))}distanceTo(e){return Math.sqrt(this.distanceToSquared(e))}distanceToSquared(e){let t=this.x-e.x,i=this.y-e.y;return t*t+i*i}manhattanDistanceTo(e){return Math.abs(this.x-e.x)+Math.abs(this.y-e.y)}setLength(e){return this.normalize().multiplyScalar(e)}lerp(e,t){return this.x+=(e.x-this.x)*t,this.y+=(e.y-this.y)*t,this}lerpVectors(e,t,i){return this.x=e.x+(t.x-e.x)*i,this.y=e.y+(t.y-e.y)*i,this}equals(e){return e.x===this.x&&e.y===this.y}fromArray(e,t=0){return this.x=e[t],this.y=e[t+1],this}toArray(e=[],t=0){return e[t]=this.x,e[t+1]=this.y,e}fromBufferAttribute(e,t){return this.x=e.getX(t),this.y=e.getY(t),this}rotateAround(e,t){let i=Math.cos(t),s=Math.sin(t),r=this.x-e.x,o=this.y-e.y;return this.x=r*i-o*s+e.x,this.y=r*s+o*i+e.y,this}random(){return this.x=Math.random(),this.y=Math.random(),this}*[Symbol.iterator](){yield this.x,yield this.y}},Vn=class{constructor(e=0,t=0,i=0,s=1){this.isQuaternion=!0,this._x=e,this._y=t,this._z=i,this._w=s}static slerpFlat(e,t,i,s,r,o,a){let c=i[s+0],l=i[s+1],h=i[s+2],u=i[s+3],p=r[o+0],d=r[o+1],g=r[o+2],v=r[o+3];if(a===0){e[t+0]=c,e[t+1]=l,e[t+2]=h,e[t+3]=u;return}if(a===1){e[t+0]=p,e[t+1]=d,e[t+2]=g,e[t+3]=v;return}if(u!==v||c!==p||l!==d||h!==g){let m=1-a,f=c*p+l*d+h*g+u*v,T=f>=0?1:-1,E=1-f*f;if(E>Number.EPSILON){let R=Math.sqrt(E),A=Math.atan2(R,f*T);m=Math.sin(m*A)/R,a=Math.sin(a*A)/R}let S=a*T;if(c=c*m+p*S,l=l*m+d*S,h=h*m+g*S,u=u*m+v*S,m===1-a){let R=1/Math.sqrt(c*c+l*l+h*h+u*u);c*=R,l*=R,h*=R,u*=R}}e[t]=c,e[t+1]=l,e[t+2]=h,e[t+3]=u}static multiplyQuaternionsFlat(e,t,i,s,r,o){let a=i[s],c=i[s+1],l=i[s+2],h=i[s+3],u=r[o],p=r[o+1],d=r[o+2],g=r[o+3];return e[t]=a*g+h*u+c*d-l*p,e[t+1]=c*g+h*p+l*u-a*d,e[t+2]=l*g+h*d+a*p-c*u,e[t+3]=h*g-a*u-c*p-l*d,e}get x(){return this._x}set x(e){this._x=e,this._onChangeCallback()}get y(){return this._y}set y(e){this._y=e,this._onChangeCallback()}get z(){return this._z}set z(e){this._z=e,this._onChangeCallback()}get w(){return this._w}set w(e){this._w=e,this._onChangeCallback()}set(e,t,i,s){return this._x=e,this._y=t,this._z=i,this._w=s,this._onChangeCallback(),this}clone(){return new this.constructor(this._x,this._y,this._z,this._w)}copy(e){return this._x=e.x,this._y=e.y,this._z=e.z,this._w=e.w,this._onChangeCallback(),this}setFromEuler(e,t=!0){let i=e._x,s=e._y,r=e._z,o=e._order,a=Math.cos,c=Math.sin,l=a(i/2),h=a(s/2),u=a(r/2),p=c(i/2),d=c(s/2),g=c(r/2);switch(o){case"XYZ":this._x=p*h*u+l*d*g,this._y=l*d*u-p*h*g,this._z=l*h*g+p*d*u,this._w=l*h*u-p*d*g;break;case"YXZ":this._x=p*h*u+l*d*g,this._y=l*d*u-p*h*g,this._z=l*h*g-p*d*u,this._w=l*h*u+p*d*g;break;case"ZXY":this._x=p*h*u-l*d*g,this._y=l*d*u+p*h*g,this._z=l*h*g+p*d*u,this._w=l*h*u-p*d*g;break;case"ZYX":this._x=p*h*u-l*d*g,this._y=l*d*u+p*h*g,this._z=l*h*g-p*d*u,this._w=l*h*u+p*d*g;break;case"YZX":this._x=p*h*u+l*d*g,this._y=l*d*u+p*h*g,this._z=l*h*g-p*d*u,this._w=l*h*u-p*d*g;break;case"XZY":this._x=p*h*u-l*d*g,this._y=l*d*u-p*h*g,this._z=l*h*g+p*d*u,this._w=l*h*u+p*d*g;break;default:console.warn("THREE.Quaternion: .setFromEuler() encountered an unknown order: "+o)}return t===!0&&this._onChangeCallback(),this}setFromAxisAngle(e,t){let i=t/2,s=Math.sin(i);return this._x=e.x*s,this._y=e.y*s,this._z=e.z*s,this._w=Math.cos(i),this._onChangeCallback(),this}setFromRotationMatrix(e){let t=e.elements,i=t[0],s=t[4],r=t[8],o=t[1],a=t[5],c=t[9],l=t[2],h=t[6],u=t[10],p=i+a+u;if(p>0){let d=.5/Math.sqrt(p+1);this._w=.25/d,this._x=(h-c)*d,this._y=(r-l)*d,this._z=(o-s)*d}else if(i>a&&i>u){let d=2*Math.sqrt(1+i-a-u);this._w=(h-c)/d,this._x=.25*d,this._y=(s+o)/d,this._z=(r+l)/d}else if(a>u){let d=2*Math.sqrt(1+a-i-u);this._w=(r-l)/d,this._x=(s+o)/d,this._y=.25*d,this._z=(c+h)/d}else{let d=2*Math.sqrt(1+u-i-a);this._w=(o-s)/d,this._x=(r+l)/d,this._y=(c+h)/d,this._z=.25*d}return this._onChangeCallback(),this}setFromUnitVectors(e,t){let i=e.dot(t)+1;return i<1e-8?(i=0,Math.abs(e.x)>Math.abs(e.z)?(this._x=-e.y,this._y=e.x,this._z=0,this._w=i):(this._x=0,this._y=-e.z,this._z=e.y,this._w=i)):(this._x=e.y*t.z-e.z*t.y,this._y=e.z*t.x-e.x*t.z,this._z=e.x*t.y-e.y*t.x,this._w=i),this.normalize()}angleTo(e){return 2*Math.acos(Math.abs(qe(this.dot(e),-1,1)))}rotateTowards(e,t){let i=this.angleTo(e);if(i===0)return this;let s=Math.min(1,t/i);return this.slerp(e,s),this}identity(){return this.set(0,0,0,1)}invert(){return this.conjugate()}conjugate(){return this._x*=-1,this._y*=-1,this._z*=-1,this._onChangeCallback(),this}dot(e){return this._x*e._x+this._y*e._y+this._z*e._z+this._w*e._w}lengthSq(){return this._x*this._x+this._y*this._y+this._z*this._z+this._w*this._w}length(){return Math.sqrt(this._x*this._x+this._y*this._y+this._z*this._z+this._w*this._w)}normalize(){let e=this.length();return e===0?(this._x=0,this._y=0,this._z=0,this._w=1):(e=1/e,this._x=this._x*e,this._y=this._y*e,this._z=this._z*e,this._w=this._w*e),this._onChangeCallback(),this}multiply(e){return this.multiplyQuaternions(this,e)}premultiply(e){return this.multiplyQuaternions(e,this)}multiplyQuaternions(e,t){let i=e._x,s=e._y,r=e._z,o=e._w,a=t._x,c=t._y,l=t._z,h=t._w;return this._x=i*h+o*a+s*l-r*c,this._y=s*h+o*c+r*a-i*l,this._z=r*h+o*l+i*c-s*a,this._w=o*h-i*a-s*c-r*l,this._onChangeCallback(),this}slerp(e,t){if(t===0)return this;if(t===1)return this.copy(e);let i=this._x,s=this._y,r=this._z,o=this._w,a=o*e._w+i*e._x+s*e._y+r*e._z;if(a<0?(this._w=-e._w,this._x=-e._x,this._y=-e._y,this._z=-e._z,a=-a):this.copy(e),a>=1)return this._w=o,this._x=i,this._y=s,this._z=r,this;let c=1-a*a;if(c<=Number.EPSILON){let d=1-t;return this._w=d*o+t*this._w,this._x=d*i+t*this._x,this._y=d*s+t*this._y,this._z=d*r+t*this._z,this.normalize(),this}let l=Math.sqrt(c),h=Math.atan2(l,a),u=Math.sin((1-t)*h)/l,p=Math.sin(t*h)/l;return this._w=o*u+this._w*p,this._x=i*u+this._x*p,this._y=s*u+this._y*p,this._z=r*u+this._z*p,this._onChangeCallback(),this}slerpQuaternions(e,t,i){return this.copy(e).slerp(t,i)}random(){let e=2*Math.PI*Math.random(),t=2*Math.PI*Math.random(),i=Math.random(),s=Math.sqrt(1-i),r=Math.sqrt(i);return this.set(s*Math.sin(e),s*Math.cos(e),r*Math.sin(t),r*Math.cos(t))}equals(e){return e._x===this._x&&e._y===this._y&&e._z===this._z&&e._w===this._w}fromArray(e,t=0){return this._x=e[t],this._y=e[t+1],this._z=e[t+2],this._w=e[t+3],this._onChangeCallback(),this}toArray(e=[],t=0){return e[t]=this._x,e[t+1]=this._y,e[t+2]=this._z,e[t+3]=this._w,e}fromBufferAttribute(e,t){return this._x=e.getX(t),this._y=e.getY(t),this._z=e.getZ(t),this._w=e.getW(t),this._onChangeCallback(),this}toJSON(){return this.toArray()}_onChange(e){return this._onChangeCallback=e,this}_onChangeCallback(){}*[Symbol.iterator](){yield this._x,yield this._y,yield this._z,yield this._w}},L=class n{constructor(e=0,t=0,i=0){n.prototype.isVector3=!0,this.x=e,this.y=t,this.z=i}set(e,t,i){return i===void 0&&(i=this.z),this.x=e,this.y=t,this.z=i,this}setScalar(e){return this.x=e,this.y=e,this.z=e,this}setX(e){return this.x=e,this}setY(e){return this.y=e,this}setZ(e){return this.z=e,this}setComponent(e,t){switch(e){case 0:this.x=t;break;case 1:this.y=t;break;case 2:this.z=t;break;default:throw new Error("index is out of range: "+e)}return this}getComponent(e){switch(e){case 0:return this.x;case 1:return this.y;case 2:return this.z;default:throw new Error("index is out of range: "+e)}}clone(){return new this.constructor(this.x,this.y,this.z)}copy(e){return this.x=e.x,this.y=e.y,this.z=e.z,this}add(e){return this.x+=e.x,this.y+=e.y,this.z+=e.z,this}addScalar(e){return this.x+=e,this.y+=e,this.z+=e,this}addVectors(e,t){return this.x=e.x+t.x,this.y=e.y+t.y,this.z=e.z+t.z,this}addScaledVector(e,t){return this.x+=e.x*t,this.y+=e.y*t,this.z+=e.z*t,this}sub(e){return this.x-=e.x,this.y-=e.y,this.z-=e.z,this}subScalar(e){return this.x-=e,this.y-=e,this.z-=e,this}subVectors(e,t){return this.x=e.x-t.x,this.y=e.y-t.y,this.z=e.z-t.z,this}multiply(e){return this.x*=e.x,this.y*=e.y,this.z*=e.z,this}multiplyScalar(e){return this.x*=e,this.y*=e,this.z*=e,this}multiplyVectors(e,t){return this.x=e.x*t.x,this.y=e.y*t.y,this.z=e.z*t.z,this}applyEuler(e){return this.applyQuaternion(uh.setFromEuler(e))}applyAxisAngle(e,t){return this.applyQuaternion(uh.setFromAxisAngle(e,t))}applyMatrix3(e){let t=this.x,i=this.y,s=this.z,r=e.elements;return this.x=r[0]*t+r[3]*i+r[6]*s,this.y=r[1]*t+r[4]*i+r[7]*s,this.z=r[2]*t+r[5]*i+r[8]*s,this}applyNormalMatrix(e){return this.applyMatrix3(e).normalize()}applyMatrix4(e){let t=this.x,i=this.y,s=this.z,r=e.elements,o=1/(r[3]*t+r[7]*i+r[11]*s+r[15]);return this.x=(r[0]*t+r[4]*i+r[8]*s+r[12])*o,this.y=(r[1]*t+r[5]*i+r[9]*s+r[13])*o,this.z=(r[2]*t+r[6]*i+r[10]*s+r[14])*o,this}applyQuaternion(e){let t=this.x,i=this.y,s=this.z,r=e.x,o=e.y,a=e.z,c=e.w,l=2*(o*s-a*i),h=2*(a*t-r*s),u=2*(r*i-o*t);return this.x=t+c*l+o*u-a*h,this.y=i+c*h+a*l-r*u,this.z=s+c*u+r*h-o*l,this}project(e){return this.applyMatrix4(e.matrixWorldInverse).applyMatrix4(e.projectionMatrix)}unproject(e){return this.applyMatrix4(e.projectionMatrixInverse).applyMatrix4(e.matrixWorld)}transformDirection(e){let t=this.x,i=this.y,s=this.z,r=e.elements;return this.x=r[0]*t+r[4]*i+r[8]*s,this.y=r[1]*t+r[5]*i+r[9]*s,this.z=r[2]*t+r[6]*i+r[10]*s,this.normalize()}divide(e){return this.x/=e.x,this.y/=e.y,this.z/=e.z,this}divideScalar(e){return this.multiplyScalar(1/e)}min(e){return this.x=Math.min(this.x,e.x),this.y=Math.min(this.y,e.y),this.z=Math.min(this.z,e.z),this}max(e){return this.x=Math.max(this.x,e.x),this.y=Math.max(this.y,e.y),this.z=Math.max(this.z,e.z),this}clamp(e,t){return this.x=qe(this.x,e.x,t.x),this.y=qe(this.y,e.y,t.y),this.z=qe(this.z,e.z,t.z),this}clampScalar(e,t){return this.x=qe(this.x,e,t),this.y=qe(this.y,e,t),this.z=qe(this.z,e,t),this}clampLength(e,t){let i=this.length();return this.divideScalar(i||1).multiplyScalar(qe(i,e,t))}floor(){return this.x=Math.floor(this.x),this.y=Math.floor(this.y),this.z=Math.floor(this.z),this}ceil(){return this.x=Math.ceil(this.x),this.y=Math.ceil(this.y),this.z=Math.ceil(this.z),this}round(){return this.x=Math.round(this.x),this.y=Math.round(this.y),this.z=Math.round(this.z),this}roundToZero(){return this.x=Math.trunc(this.x),this.y=Math.trunc(this.y),this.z=Math.trunc(this.z),this}negate(){return this.x=-this.x,this.y=-this.y,this.z=-this.z,this}dot(e){return this.x*e.x+this.y*e.y+this.z*e.z}lengthSq(){return this.x*this.x+this.y*this.y+this.z*this.z}length(){return Math.sqrt(this.x*this.x+this.y*this.y+this.z*this.z)}manhattanLength(){return Math.abs(this.x)+Math.abs(this.y)+Math.abs(this.z)}normalize(){return this.divideScalar(this.length()||1)}setLength(e){return this.normalize().multiplyScalar(e)}lerp(e,t){return this.x+=(e.x-this.x)*t,this.y+=(e.y-this.y)*t,this.z+=(e.z-this.z)*t,this}lerpVectors(e,t,i){return this.x=e.x+(t.x-e.x)*i,this.y=e.y+(t.y-e.y)*i,this.z=e.z+(t.z-e.z)*i,this}cross(e){return this.crossVectors(this,e)}crossVectors(e,t){let i=e.x,s=e.y,r=e.z,o=t.x,a=t.y,c=t.z;return this.x=s*c-r*a,this.y=r*o-i*c,this.z=i*a-s*o,this}projectOnVector(e){let t=e.lengthSq();if(t===0)return this.set(0,0,0);let i=e.dot(this)/t;return this.copy(e).multiplyScalar(i)}projectOnPlane(e){return $o.copy(this).projectOnVector(e),this.sub($o)}reflect(e){return this.sub($o.copy(e).multiplyScalar(2*this.dot(e)))}angleTo(e){let t=Math.sqrt(this.lengthSq()*e.lengthSq());if(t===0)return Math.PI/2;let i=this.dot(e)/t;return Math.acos(qe(i,-1,1))}distanceTo(e){return Math.sqrt(this.distanceToSquared(e))}distanceToSquared(e){let t=this.x-e.x,i=this.y-e.y,s=this.z-e.z;return t*t+i*i+s*s}manhattanDistanceTo(e){return Math.abs(this.x-e.x)+Math.abs(this.y-e.y)+Math.abs(this.z-e.z)}setFromSpherical(e){return this.setFromSphericalCoords(e.radius,e.phi,e.theta)}setFromSphericalCoords(e,t,i){let s=Math.sin(t)*e;return this.x=s*Math.sin(i),this.y=Math.cos(t)*e,this.z=s*Math.cos(i),this}setFromCylindrical(e){return this.setFromCylindricalCoords(e.radius,e.theta,e.y)}setFromCylindricalCoords(e,t,i){return this.x=e*Math.sin(t),this.y=i,this.z=e*Math.cos(t),this}setFromMatrixPosition(e){let t=e.elements;return this.x=t[12],this.y=t[13],this.z=t[14],this}setFromMatrixScale(e){let t=this.setFromMatrixColumn(e,0).length(),i=this.setFromMatrixColumn(e,1).length(),s=this.setFromMatrixColumn(e,2).length();return this.x=t,this.y=i,this.z=s,this}setFromMatrixColumn(e,t){return this.fromArray(e.elements,t*4)}setFromMatrix3Column(e,t){return this.fromArray(e.elements,t*3)}setFromEuler(e){return this.x=e._x,this.y=e._y,this.z=e._z,this}setFromColor(e){return this.x=e.r,this.y=e.g,this.z=e.b,this}equals(e){return e.x===this.x&&e.y===this.y&&e.z===this.z}fromArray(e,t=0){return this.x=e[t],this.y=e[t+1],this.z=e[t+2],this}toArray(e=[],t=0){return e[t]=this.x,e[t+1]=this.y,e[t+2]=this.z,e}fromBufferAttribute(e,t){return this.x=e.getX(t),this.y=e.getY(t),this.z=e.getZ(t),this}random(){return this.x=Math.random(),this.y=Math.random(),this.z=Math.random(),this}randomDirection(){let e=Math.random()*Math.PI*2,t=Math.random()*2-1,i=Math.sqrt(1-t*t);return this.x=i*Math.cos(e),this.y=t,this.z=i*Math.sin(e),this}*[Symbol.iterator](){yield this.x,yield this.y,yield this.z}},$o=new L,uh=new Vn,Oe=class n{constructor(e,t,i,s,r,o,a,c,l){n.prototype.isMatrix3=!0,this.elements=[1,0,0,0,1,0,0,0,1],e!==void 0&&this.set(e,t,i,s,r,o,a,c,l)}set(e,t,i,s,r,o,a,c,l){let h=this.elements;return h[0]=e,h[1]=s,h[2]=a,h[3]=t,h[4]=r,h[5]=c,h[6]=i,h[7]=o,h[8]=l,this}identity(){return this.set(1,0,0,0,1,0,0,0,1),this}copy(e){let t=this.elements,i=e.elements;return t[0]=i[0],t[1]=i[1],t[2]=i[2],t[3]=i[3],t[4]=i[4],t[5]=i[5],t[6]=i[6],t[7]=i[7],t[8]=i[8],this}extractBasis(e,t,i){return e.setFromMatrix3Column(this,0),t.setFromMatrix3Column(this,1),i.setFromMatrix3Column(this,2),this}setFromMatrix4(e){let t=e.elements;return this.set(t[0],t[4],t[8],t[1],t[5],t[9],t[2],t[6],t[10]),this}multiply(e){return this.multiplyMatrices(this,e)}premultiply(e){return this.multiplyMatrices(e,this)}multiplyMatrices(e,t){let i=e.elements,s=t.elements,r=this.elements,o=i[0],a=i[3],c=i[6],l=i[1],h=i[4],u=i[7],p=i[2],d=i[5],g=i[8],v=s[0],m=s[3],f=s[6],T=s[1],E=s[4],S=s[7],R=s[2],A=s[5],P=s[8];return r[0]=o*v+a*T+c*R,r[3]=o*m+a*E+c*A,r[6]=o*f+a*S+c*P,r[1]=l*v+h*T+u*R,r[4]=l*m+h*E+u*A,r[7]=l*f+h*S+u*P,r[2]=p*v+d*T+g*R,r[5]=p*m+d*E+g*A,r[8]=p*f+d*S+g*P,this}multiplyScalar(e){let t=this.elements;return t[0]*=e,t[3]*=e,t[6]*=e,t[1]*=e,t[4]*=e,t[7]*=e,t[2]*=e,t[5]*=e,t[8]*=e,this}determinant(){let e=this.elements,t=e[0],i=e[1],s=e[2],r=e[3],o=e[4],a=e[5],c=e[6],l=e[7],h=e[8];return t*o*h-t*a*l-i*r*h+i*a*c+s*r*l-s*o*c}invert(){let e=this.elements,t=e[0],i=e[1],s=e[2],r=e[3],o=e[4],a=e[5],c=e[6],l=e[7],h=e[8],u=h*o-a*l,p=a*c-h*r,d=l*r-o*c,g=t*u+i*p+s*d;if(g===0)return this.set(0,0,0,0,0,0,0,0,0);let v=1/g;return e[0]=u*v,e[1]=(s*l-h*i)*v,e[2]=(a*i-s*o)*v,e[3]=p*v,e[4]=(h*t-s*c)*v,e[5]=(s*r-a*t)*v,e[6]=d*v,e[7]=(i*c-l*t)*v,e[8]=(o*t-i*r)*v,this}transpose(){let e,t=this.elements;return e=t[1],t[1]=t[3],t[3]=e,e=t[2],t[2]=t[6],t[6]=e,e=t[5],t[5]=t[7],t[7]=e,this}getNormalMatrix(e){return this.setFromMatrix4(e).invert().transpose()}transposeIntoArray(e){let t=this.elements;return e[0]=t[0],e[1]=t[3],e[2]=t[6],e[3]=t[1],e[4]=t[4],e[5]=t[7],e[6]=t[2],e[7]=t[5],e[8]=t[8],this}setUvTransform(e,t,i,s,r,o,a){let c=Math.cos(r),l=Math.sin(r);return this.set(i*c,i*l,-i*(c*o+l*a)+o+e,-s*l,s*c,-s*(-l*o+c*a)+a+t,0,0,1),this}scale(e,t){return this.premultiply(Yo.makeScale(e,t)),this}rotate(e){return this.premultiply(Yo.makeRotation(-e)),this}translate(e,t){return this.premultiply(Yo.makeTranslation(e,t)),this}makeTranslation(e,t){return e.isVector2?this.set(1,0,e.x,0,1,e.y,0,0,1):this.set(1,0,e,0,1,t,0,0,1),this}makeRotation(e){let t=Math.cos(e),i=Math.sin(e);return this.set(t,-i,0,i,t,0,0,0,1),this}makeScale(e,t){return this.set(e,0,0,0,t,0,0,0,1),this}equals(e){let t=this.elements,i=e.elements;for(let s=0;s<9;s++)if(t[s]!==i[s])return!1;return!0}fromArray(e,t=0){for(let i=0;i<9;i++)this.elements[i]=e[i+t];return this}toArray(e=[],t=0){let i=this.elements;return e[t]=i[0],e[t+1]=i[1],e[t+2]=i[2],e[t+3]=i[3],e[t+4]=i[4],e[t+5]=i[5],e[t+6]=i[6],e[t+7]=i[7],e[t+8]=i[8],e}clone(){return new this.constructor().fromArray(this.elements)}},Yo=new Oe;function Hl(n){for(let e=n.length-1;e>=0;--e)if(n[e]>=65535)return!0;return!1}function Vs(n){return document.createElementNS("http://www.w3.org/1999/xhtml",n)}function yu(){let n=Vs("canvas");return n.style.display="block",n}var dh={};function ls(n){n in dh||(dh[n]=!0,console.warn(n))}function Mu(n,e,t){return new Promise(function(i,s){function r(){switch(n.clientWaitSync(e,n.SYNC_FLUSH_COMMANDS_BIT,0)){case n.WAIT_FAILED:s();break;case n.TIMEOUT_EXPIRED:setTimeout(r,t);break;default:i()}}setTimeout(r,t)})}var fh=new Oe().set(.4123908,.3575843,.1804808,.212639,.7151687,.0721923,.0193308,.1191948,.9505322),ph=new Oe().set(3.2409699,-1.5373832,-.4986108,-.9692436,1.8759675,.0415551,.0556301,-.203977,1.0569715);function Wd(){let n={enabled:!0,workingColorSpace:wi,spaces:{},convert:function(s,r,o){return this.enabled===!1||r===o||!r||!o||(this.spaces[r].transfer===et&&(s.r=kn(s.r),s.g=kn(s.g),s.b=kn(s.b)),this.spaces[r].primaries!==this.spaces[o].primaries&&(s.applyMatrix3(this.spaces[r].toXYZ),s.applyMatrix3(this.spaces[o].fromXYZ)),this.spaces[o].transfer===et&&(s.r=as(s.r),s.g=as(s.g),s.b=as(s.b))),s},workingToColorSpace:function(s,r){return this.convert(s,this.workingColorSpace,r)},colorSpaceToWorking:function(s,r){return this.convert(s,r,this.workingColorSpace)},getPrimaries:function(s){return this.spaces[s].primaries},getTransfer:function(s){return s===Xn?Bs:this.spaces[s].transfer},getToneMappingMode:function(s){return this.spaces[s].outputColorSpaceConfig.toneMappingMode||"standard"},getLuminanceCoefficients:function(s,r=this.workingColorSpace){return s.fromArray(this.spaces[r].luminanceCoefficients)},define:function(s){Object.assign(this.spaces,s)},_getMatrix:function(s,r,o){return s.copy(this.spaces[r].toXYZ).multiply(this.spaces[o].fromXYZ)},_getDrawingBufferColorSpace:function(s){return this.spaces[s].outputColorSpaceConfig.drawingBufferColorSpace},_getUnpackColorSpace:function(s=this.workingColorSpace){return this.spaces[s].workingColorSpaceConfig.unpackColorSpace},fromWorkingColorSpace:function(s,r){return ls("THREE.ColorManagement: .fromWorkingColorSpace() has been renamed to .workingToColorSpace()."),n.workingToColorSpace(s,r)},toWorkingColorSpace:function(s,r){return ls("THREE.ColorManagement: .toWorkingColorSpace() has been renamed to .colorSpaceToWorking()."),n.colorSpaceToWorking(s,r)}},e=[.64,.33,.3,.6,.15,.06],t=[.2126,.7152,.0722],i=[.3127,.329];return n.define({[wi]:{primaries:e,whitePoint:i,transfer:Bs,toXYZ:fh,fromXYZ:ph,luminanceCoefficients:t,workingColorSpaceConfig:{unpackColorSpace:Ft},outputColorSpaceConfig:{drawingBufferColorSpace:Ft}},[Ft]:{primaries:e,whitePoint:i,transfer:et,toXYZ:fh,fromXYZ:ph,luminanceCoefficients:t,outputColorSpaceConfig:{drawingBufferColorSpace:Ft}}}),n}var Ye=Wd();function kn(n){return n<.04045?n*.0773993808:Math.pow(n*.9478672986+.0521327014,2.4)}function as(n){return n<.0031308?n*12.92:1.055*Math.pow(n,.41666)-.055}var Wi,sa=class{static getDataURL(e,t="image/png"){if(/^data:/i.test(e.src)||typeof HTMLCanvasElement>"u")return e.src;let i;if(e instanceof HTMLCanvasElement)i=e;else{Wi===void 0&&(Wi=Vs("canvas")),Wi.width=e.width,Wi.height=e.height;let s=Wi.getContext("2d");e instanceof ImageData?s.putImageData(e,0,0):s.drawImage(e,0,0,e.width,e.height),i=Wi}return i.toDataURL(t)}static sRGBToLinear(e){if(typeof HTMLImageElement<"u"&&e instanceof HTMLImageElement||typeof HTMLCanvasElement<"u"&&e instanceof HTMLCanvasElement||typeof ImageBitmap<"u"&&e instanceof ImageBitmap){let t=Vs("canvas");t.width=e.width,t.height=e.height;let i=t.getContext("2d");i.drawImage(e,0,0,e.width,e.height);let s=i.getImageData(0,0,e.width,e.height),r=s.data;for(let o=0;o<r.length;o++)r[o]=kn(r[o]/255)*255;return i.putImageData(s,0,0),t}else if(e.data){let t=e.data.slice(0);for(let i=0;i<t.length;i++)t instanceof Uint8Array||t instanceof Uint8ClampedArray?t[i]=Math.floor(kn(t[i]/255)*255):t[i]=kn(t[i]);return{data:t,width:e.width,height:e.height}}else return console.warn("THREE.ImageUtils.sRGBToLinear(): Unsupported image type. No color space conversion applied."),e}},Xd=0,cs=class{constructor(e=null){this.isSource=!0,Object.defineProperty(this,"id",{value:Xd++}),this.uuid=si(),this.data=e,this.dataReady=!0,this.version=0}getSize(e){let t=this.data;return typeof HTMLVideoElement<"u"&&t instanceof HTMLVideoElement?e.set(t.videoWidth,t.videoHeight,0):t instanceof VideoFrame?e.set(t.displayHeight,t.displayWidth,0):t!==null?e.set(t.width,t.height,t.depth||0):e.set(0,0,0),e}set needsUpdate(e){e===!0&&this.version++}toJSON(e){let t=e===void 0||typeof e=="string";if(!t&&e.images[this.uuid]!==void 0)return e.images[this.uuid];let i={uuid:this.uuid,url:""},s=this.data;if(s!==null){let r;if(Array.isArray(s)){r=[];for(let o=0,a=s.length;o<a;o++)s[o].isDataTexture?r.push(Zo(s[o].image)):r.push(Zo(s[o]))}else r=Zo(s);i.url=r}return t||(e.images[this.uuid]=i),i}};function Zo(n){return typeof HTMLImageElement<"u"&&n instanceof HTMLImageElement||typeof HTMLCanvasElement<"u"&&n instanceof HTMLCanvasElement||typeof ImageBitmap<"u"&&n instanceof ImageBitmap?sa.getDataURL(n):n.data?{data:Array.from(n.data),width:n.width,height:n.height,type:n.data.constructor.name}:(console.warn("THREE.Texture: Unable to serialize Texture."),{})}var qd=0,Jo=new L,Wt=class n extends zn{constructor(e=n.DEFAULT_IMAGE,t=n.DEFAULT_MAPPING,i=ii,s=ii,r=dn,o=ui,a=an,c=mn,l=n.DEFAULT_ANISOTROPY,h=Xn){super(),this.isTexture=!0,Object.defineProperty(this,"id",{value:qd++}),this.uuid=si(),this.name="",this.source=new cs(e),this.mipmaps=[],this.mapping=t,this.channel=0,this.wrapS=i,this.wrapT=s,this.magFilter=r,this.minFilter=o,this.anisotropy=l,this.format=a,this.internalFormat=null,this.type=c,this.offset=new Ue(0,0),this.repeat=new Ue(1,1),this.center=new Ue(0,0),this.rotation=0,this.matrixAutoUpdate=!0,this.matrix=new Oe,this.generateMipmaps=!0,this.premultiplyAlpha=!1,this.flipY=!0,this.unpackAlignment=4,this.colorSpace=h,this.userData={},this.updateRanges=[],this.version=0,this.onUpdate=null,this.renderTarget=null,this.isRenderTargetTexture=!1,this.isArrayTexture=!!(e&&e.depth&&e.depth>1),this.pmremVersion=0}get width(){return this.source.getSize(Jo).x}get height(){return this.source.getSize(Jo).y}get depth(){return this.source.getSize(Jo).z}get image(){return this.source.data}set image(e=null){this.source.data=e}updateMatrix(){this.matrix.setUvTransform(this.offset.x,this.offset.y,this.repeat.x,this.repeat.y,this.rotation,this.center.x,this.center.y)}addUpdateRange(e,t){this.updateRanges.push({start:e,count:t})}clearUpdateRanges(){this.updateRanges.length=0}clone(){return new this.constructor().copy(this)}copy(e){return this.name=e.name,this.source=e.source,this.mipmaps=e.mipmaps.slice(0),this.mapping=e.mapping,this.channel=e.channel,this.wrapS=e.wrapS,this.wrapT=e.wrapT,this.magFilter=e.magFilter,this.minFilter=e.minFilter,this.anisotropy=e.anisotropy,this.format=e.format,this.internalFormat=e.internalFormat,this.type=e.type,this.offset.copy(e.offset),this.repeat.copy(e.repeat),this.center.copy(e.center),this.rotation=e.rotation,this.matrixAutoUpdate=e.matrixAutoUpdate,this.matrix.copy(e.matrix),this.generateMipmaps=e.generateMipmaps,this.premultiplyAlpha=e.premultiplyAlpha,this.flipY=e.flipY,this.unpackAlignment=e.unpackAlignment,this.colorSpace=e.colorSpace,this.renderTarget=e.renderTarget,this.isRenderTargetTexture=e.isRenderTargetTexture,this.isArrayTexture=e.isArrayTexture,this.userData=JSON.parse(JSON.stringify(e.userData)),this.needsUpdate=!0,this}setValues(e){for(let t in e){let i=e[t];if(i===void 0){console.warn(`THREE.Texture.setValues(): parameter '${t}' has value of undefined.`);continue}let s=this[t];if(s===void 0){console.warn(`THREE.Texture.setValues(): property '${t}' does not exist.`);continue}s&&i&&s.isVector2&&i.isVector2||s&&i&&s.isVector3&&i.isVector3||s&&i&&s.isMatrix3&&i.isMatrix3?s.copy(i):this[t]=i}}toJSON(e){let t=e===void 0||typeof e=="string";if(!t&&e.textures[this.uuid]!==void 0)return e.textures[this.uuid];let i={metadata:{version:4.7,type:"Texture",generator:"Texture.toJSON"},uuid:this.uuid,name:this.name,image:this.source.toJSON(e).uuid,mapping:this.mapping,channel:this.channel,repeat:[this.repeat.x,this.repeat.y],offset:[this.offset.x,this.offset.y],center:[this.center.x,this.center.y],rotation:this.rotation,wrap:[this.wrapS,this.wrapT],format:this.format,internalFormat:this.internalFormat,type:this.type,colorSpace:this.colorSpace,minFilter:this.minFilter,magFilter:this.magFilter,anisotropy:this.anisotropy,flipY:this.flipY,generateMipmaps:this.generateMipmaps,premultiplyAlpha:this.premultiplyAlpha,unpackAlignment:this.unpackAlignment};return Object.keys(this.userData).length>0&&(i.userData=this.userData),t||(e.textures[this.uuid]=i),i}dispose(){this.dispatchEvent({type:"dispose"})}transformUv(e){if(this.mapping!==Pl)return e;if(e.applyMatrix3(this.matrix),e.x<0||e.x>1)switch(this.wrapS){case Qr:e.x=e.x-Math.floor(e.x);break;case ii:e.x=e.x<0?0:1;break;case ea:Math.abs(Math.floor(e.x)%2)===1?e.x=Math.ceil(e.x)-e.x:e.x=e.x-Math.floor(e.x);break}if(e.y<0||e.y>1)switch(this.wrapT){case Qr:e.y=e.y-Math.floor(e.y);break;case ii:e.y=e.y<0?0:1;break;case ea:Math.abs(Math.floor(e.y)%2)===1?e.y=Math.ceil(e.y)-e.y:e.y=e.y-Math.floor(e.y);break}return this.flipY&&(e.y=1-e.y),e}set needsUpdate(e){e===!0&&(this.version++,this.source.needsUpdate=!0)}set needsPMREMUpdate(e){e===!0&&this.pmremVersion++}};Wt.DEFAULT_IMAGE=null;Wt.DEFAULT_MAPPING=Pl;Wt.DEFAULT_ANISOTROPY=1;var Qe=class n{constructor(e=0,t=0,i=0,s=1){n.prototype.isVector4=!0,this.x=e,this.y=t,this.z=i,this.w=s}get width(){return this.z}set width(e){this.z=e}get height(){return this.w}set height(e){this.w=e}set(e,t,i,s){return this.x=e,this.y=t,this.z=i,this.w=s,this}setScalar(e){return this.x=e,this.y=e,this.z=e,this.w=e,this}setX(e){return this.x=e,this}setY(e){return this.y=e,this}setZ(e){return this.z=e,this}setW(e){return this.w=e,this}setComponent(e,t){switch(e){case 0:this.x=t;break;case 1:this.y=t;break;case 2:this.z=t;break;case 3:this.w=t;break;default:throw new Error("index is out of range: "+e)}return this}getComponent(e){switch(e){case 0:return this.x;case 1:return this.y;case 2:return this.z;case 3:return this.w;default:throw new Error("index is out of range: "+e)}}clone(){return new this.constructor(this.x,this.y,this.z,this.w)}copy(e){return this.x=e.x,this.y=e.y,this.z=e.z,this.w=e.w!==void 0?e.w:1,this}add(e){return this.x+=e.x,this.y+=e.y,this.z+=e.z,this.w+=e.w,this}addScalar(e){return this.x+=e,this.y+=e,this.z+=e,this.w+=e,this}addVectors(e,t){return this.x=e.x+t.x,this.y=e.y+t.y,this.z=e.z+t.z,this.w=e.w+t.w,this}addScaledVector(e,t){return this.x+=e.x*t,this.y+=e.y*t,this.z+=e.z*t,this.w+=e.w*t,this}sub(e){return this.x-=e.x,this.y-=e.y,this.z-=e.z,this.w-=e.w,this}subScalar(e){return this.x-=e,this.y-=e,this.z-=e,this.w-=e,this}subVectors(e,t){return this.x=e.x-t.x,this.y=e.y-t.y,this.z=e.z-t.z,this.w=e.w-t.w,this}multiply(e){return this.x*=e.x,this.y*=e.y,this.z*=e.z,this.w*=e.w,this}multiplyScalar(e){return this.x*=e,this.y*=e,this.z*=e,this.w*=e,this}applyMatrix4(e){let t=this.x,i=this.y,s=this.z,r=this.w,o=e.elements;return this.x=o[0]*t+o[4]*i+o[8]*s+o[12]*r,this.y=o[1]*t+o[5]*i+o[9]*s+o[13]*r,this.z=o[2]*t+o[6]*i+o[10]*s+o[14]*r,this.w=o[3]*t+o[7]*i+o[11]*s+o[15]*r,this}divide(e){return this.x/=e.x,this.y/=e.y,this.z/=e.z,this.w/=e.w,this}divideScalar(e){return this.multiplyScalar(1/e)}setAxisAngleFromQuaternion(e){this.w=2*Math.acos(e.w);let t=Math.sqrt(1-e.w*e.w);return t<1e-4?(this.x=1,this.y=0,this.z=0):(this.x=e.x/t,this.y=e.y/t,this.z=e.z/t),this}setAxisAngleFromRotationMatrix(e){let t,i,s,r,c=e.elements,l=c[0],h=c[4],u=c[8],p=c[1],d=c[5],g=c[9],v=c[2],m=c[6],f=c[10];if(Math.abs(h-p)<.01&&Math.abs(u-v)<.01&&Math.abs(g-m)<.01){if(Math.abs(h+p)<.1&&Math.abs(u+v)<.1&&Math.abs(g+m)<.1&&Math.abs(l+d+f-3)<.1)return this.set(1,0,0,0),this;t=Math.PI;let E=(l+1)/2,S=(d+1)/2,R=(f+1)/2,A=(h+p)/4,P=(u+v)/4,N=(g+m)/4;return E>S&&E>R?E<.01?(i=0,s=.707106781,r=.707106781):(i=Math.sqrt(E),s=A/i,r=P/i):S>R?S<.01?(i=.707106781,s=0,r=.707106781):(s=Math.sqrt(S),i=A/s,r=N/s):R<.01?(i=.707106781,s=.707106781,r=0):(r=Math.sqrt(R),i=P/r,s=N/r),this.set(i,s,r,t),this}let T=Math.sqrt((m-g)*(m-g)+(u-v)*(u-v)+(p-h)*(p-h));return Math.abs(T)<.001&&(T=1),this.x=(m-g)/T,this.y=(u-v)/T,this.z=(p-h)/T,this.w=Math.acos((l+d+f-1)/2),this}setFromMatrixPosition(e){let t=e.elements;return this.x=t[12],this.y=t[13],this.z=t[14],this.w=t[15],this}min(e){return this.x=Math.min(this.x,e.x),this.y=Math.min(this.y,e.y),this.z=Math.min(this.z,e.z),this.w=Math.min(this.w,e.w),this}max(e){return this.x=Math.max(this.x,e.x),this.y=Math.max(this.y,e.y),this.z=Math.max(this.z,e.z),this.w=Math.max(this.w,e.w),this}clamp(e,t){return this.x=qe(this.x,e.x,t.x),this.y=qe(this.y,e.y,t.y),this.z=qe(this.z,e.z,t.z),this.w=qe(this.w,e.w,t.w),this}clampScalar(e,t){return this.x=qe(this.x,e,t),this.y=qe(this.y,e,t),this.z=qe(this.z,e,t),this.w=qe(this.w,e,t),this}clampLength(e,t){let i=this.length();return this.divideScalar(i||1).multiplyScalar(qe(i,e,t))}floor(){return this.x=Math.floor(this.x),this.y=Math.floor(this.y),this.z=Math.floor(this.z),this.w=Math.floor(this.w),this}ceil(){return this.x=Math.ceil(this.x),this.y=Math.ceil(this.y),this.z=Math.ceil(this.z),this.w=Math.ceil(this.w),this}round(){return this.x=Math.round(this.x),this.y=Math.round(this.y),this.z=Math.round(this.z),this.w=Math.round(this.w),this}roundToZero(){return this.x=Math.trunc(this.x),this.y=Math.trunc(this.y),this.z=Math.trunc(this.z),this.w=Math.trunc(this.w),this}negate(){return this.x=-this.x,this.y=-this.y,this.z=-this.z,this.w=-this.w,this}dot(e){return this.x*e.x+this.y*e.y+this.z*e.z+this.w*e.w}lengthSq(){return this.x*this.x+this.y*this.y+this.z*this.z+this.w*this.w}length(){return Math.sqrt(this.x*this.x+this.y*this.y+this.z*this.z+this.w*this.w)}manhattanLength(){return Math.abs(this.x)+Math.abs(this.y)+Math.abs(this.z)+Math.abs(this.w)}normalize(){return this.divideScalar(this.length()||1)}setLength(e){return this.normalize().multiplyScalar(e)}lerp(e,t){return this.x+=(e.x-this.x)*t,this.y+=(e.y-this.y)*t,this.z+=(e.z-this.z)*t,this.w+=(e.w-this.w)*t,this}lerpVectors(e,t,i){return this.x=e.x+(t.x-e.x)*i,this.y=e.y+(t.y-e.y)*i,this.z=e.z+(t.z-e.z)*i,this.w=e.w+(t.w-e.w)*i,this}equals(e){return e.x===this.x&&e.y===this.y&&e.z===this.z&&e.w===this.w}fromArray(e,t=0){return this.x=e[t],this.y=e[t+1],this.z=e[t+2],this.w=e[t+3],this}toArray(e=[],t=0){return e[t]=this.x,e[t+1]=this.y,e[t+2]=this.z,e[t+3]=this.w,e}fromBufferAttribute(e,t){return this.x=e.getX(t),this.y=e.getY(t),this.z=e.getZ(t),this.w=e.getW(t),this}random(){return this.x=Math.random(),this.y=Math.random(),this.z=Math.random(),this.w=Math.random(),this}*[Symbol.iterator](){yield this.x,yield this.y,yield this.z,yield this.w}},ra=class extends zn{constructor(e=1,t=1,i={}){super(),i=Object.assign({generateMipmaps:!1,internalFormat:null,minFilter:dn,depthBuffer:!0,stencilBuffer:!1,resolveDepthBuffer:!0,resolveStencilBuffer:!0,depthTexture:null,samples:0,count:1,depth:1,multiview:!1},i),this.isRenderTarget=!0,this.width=e,this.height=t,this.depth=i.depth,this.scissor=new Qe(0,0,e,t),this.scissorTest=!1,this.viewport=new Qe(0,0,e,t);let s={width:e,height:t,depth:i.depth},r=new Wt(s);this.textures=[];let o=i.count;for(let a=0;a<o;a++)this.textures[a]=r.clone(),this.textures[a].isRenderTargetTexture=!0,this.textures[a].renderTarget=this;this._setTextureOptions(i),this.depthBuffer=i.depthBuffer,this.stencilBuffer=i.stencilBuffer,this.resolveDepthBuffer=i.resolveDepthBuffer,this.resolveStencilBuffer=i.resolveStencilBuffer,this._depthTexture=null,this.depthTexture=i.depthTexture,this.samples=i.samples,this.multiview=i.multiview}_setTextureOptions(e={}){let t={minFilter:dn,generateMipmaps:!1,flipY:!1,internalFormat:null};e.mapping!==void 0&&(t.mapping=e.mapping),e.wrapS!==void 0&&(t.wrapS=e.wrapS),e.wrapT!==void 0&&(t.wrapT=e.wrapT),e.wrapR!==void 0&&(t.wrapR=e.wrapR),e.magFilter!==void 0&&(t.magFilter=e.magFilter),e.minFilter!==void 0&&(t.minFilter=e.minFilter),e.format!==void 0&&(t.format=e.format),e.type!==void 0&&(t.type=e.type),e.anisotropy!==void 0&&(t.anisotropy=e.anisotropy),e.colorSpace!==void 0&&(t.colorSpace=e.colorSpace),e.flipY!==void 0&&(t.flipY=e.flipY),e.generateMipmaps!==void 0&&(t.generateMipmaps=e.generateMipmaps),e.internalFormat!==void 0&&(t.internalFormat=e.internalFormat);for(let i=0;i<this.textures.length;i++)this.textures[i].setValues(t)}get texture(){return this.textures[0]}set texture(e){this.textures[0]=e}set depthTexture(e){this._depthTexture!==null&&(this._depthTexture.renderTarget=null),e!==null&&(e.renderTarget=this),this._depthTexture=e}get depthTexture(){return this._depthTexture}setSize(e,t,i=1){if(this.width!==e||this.height!==t||this.depth!==i){this.width=e,this.height=t,this.depth=i;for(let s=0,r=this.textures.length;s<r;s++)this.textures[s].image.width=e,this.textures[s].image.height=t,this.textures[s].image.depth=i,this.textures[s].isArrayTexture=this.textures[s].image.depth>1;this.dispose()}this.viewport.set(0,0,e,t),this.scissor.set(0,0,e,t)}clone(){return new this.constructor().copy(this)}copy(e){this.width=e.width,this.height=e.height,this.depth=e.depth,this.scissor.copy(e.scissor),this.scissorTest=e.scissorTest,this.viewport.copy(e.viewport),this.textures.length=0;for(let t=0,i=e.textures.length;t<i;t++){this.textures[t]=e.textures[t].clone(),this.textures[t].isRenderTargetTexture=!0,this.textures[t].renderTarget=this;let s=Object.assign({},e.textures[t].image);this.textures[t].source=new cs(s)}return this.depthBuffer=e.depthBuffer,this.stencilBuffer=e.stencilBuffer,this.resolveDepthBuffer=e.resolveDepthBuffer,this.resolveStencilBuffer=e.resolveStencilBuffer,e.depthTexture!==null&&(this.depthTexture=e.depthTexture.clone()),this.samples=e.samples,this}dispose(){this.dispatchEvent({type:"dispose"})}},Sn=class extends ra{constructor(e=1,t=1,i={}){super(e,t,i),this.isWebGLRenderTarget=!0}},Gs=class extends Wt{constructor(e=null,t=1,i=1,s=1){super(null),this.isDataArrayTexture=!0,this.image={data:e,width:t,height:i,depth:s},this.magFilter=rn,this.minFilter=rn,this.wrapR=ii,this.generateMipmaps=!1,this.flipY=!1,this.unpackAlignment=1,this.layerUpdates=new Set}addLayerUpdate(e){this.layerUpdates.add(e)}clearLayerUpdates(){this.layerUpdates.clear()}};var aa=class extends Wt{constructor(e=null,t=1,i=1,s=1){super(null),this.isData3DTexture=!0,this.image={data:e,width:t,height:i,depth:s},this.magFilter=rn,this.minFilter=rn,this.wrapR=ii,this.generateMipmaps=!1,this.flipY=!1,this.unpackAlignment=1}};var ai=class{constructor(e=new L(1/0,1/0,1/0),t=new L(-1/0,-1/0,-1/0)){this.isBox3=!0,this.min=e,this.max=t}set(e,t){return this.min.copy(e),this.max.copy(t),this}setFromArray(e){this.makeEmpty();for(let t=0,i=e.length;t<i;t+=3)this.expandByPoint(ln.fromArray(e,t));return this}setFromBufferAttribute(e){this.makeEmpty();for(let t=0,i=e.count;t<i;t++)this.expandByPoint(ln.fromBufferAttribute(e,t));return this}setFromPoints(e){this.makeEmpty();for(let t=0,i=e.length;t<i;t++)this.expandByPoint(e[t]);return this}setFromCenterAndSize(e,t){let i=ln.copy(t).multiplyScalar(.5);return this.min.copy(e).sub(i),this.max.copy(e).add(i),this}setFromObject(e,t=!1){return this.makeEmpty(),this.expandByObject(e,t)}clone(){return new this.constructor().copy(this)}copy(e){return this.min.copy(e.min),this.max.copy(e.max),this}makeEmpty(){return this.min.x=this.min.y=this.min.z=1/0,this.max.x=this.max.y=this.max.z=-1/0,this}isEmpty(){return this.max.x<this.min.x||this.max.y<this.min.y||this.max.z<this.min.z}getCenter(e){return this.isEmpty()?e.set(0,0,0):e.addVectors(this.min,this.max).multiplyScalar(.5)}getSize(e){return this.isEmpty()?e.set(0,0,0):e.subVectors(this.max,this.min)}expandByPoint(e){return this.min.min(e),this.max.max(e),this}expandByVector(e){return this.min.sub(e),this.max.add(e),this}expandByScalar(e){return this.min.addScalar(-e),this.max.addScalar(e),this}expandByObject(e,t=!1){e.updateWorldMatrix(!1,!1);let i=e.geometry;if(i!==void 0){let r=i.getAttribute("position");if(t===!0&&r!==void 0&&e.isInstancedMesh!==!0)for(let o=0,a=r.count;o<a;o++)e.isMesh===!0?e.getVertexPosition(o,ln):ln.fromBufferAttribute(r,o),ln.applyMatrix4(e.matrixWorld),this.expandByPoint(ln);else e.boundingBox!==void 0?(e.boundingBox===null&&e.computeBoundingBox(),Cr.copy(e.boundingBox)):(i.boundingBox===null&&i.computeBoundingBox(),Cr.copy(i.boundingBox)),Cr.applyMatrix4(e.matrixWorld),this.union(Cr)}let s=e.children;for(let r=0,o=s.length;r<o;r++)this.expandByObject(s[r],t);return this}containsPoint(e){return e.x>=this.min.x&&e.x<=this.max.x&&e.y>=this.min.y&&e.y<=this.max.y&&e.z>=this.min.z&&e.z<=this.max.z}containsBox(e){return this.min.x<=e.min.x&&e.max.x<=this.max.x&&this.min.y<=e.min.y&&e.max.y<=this.max.y&&this.min.z<=e.min.z&&e.max.z<=this.max.z}getParameter(e,t){return t.set((e.x-this.min.x)/(this.max.x-this.min.x),(e.y-this.min.y)/(this.max.y-this.min.y),(e.z-this.min.z)/(this.max.z-this.min.z))}intersectsBox(e){return e.max.x>=this.min.x&&e.min.x<=this.max.x&&e.max.y>=this.min.y&&e.min.y<=this.max.y&&e.max.z>=this.min.z&&e.min.z<=this.max.z}intersectsSphere(e){return this.clampPoint(e.center,ln),ln.distanceToSquared(e.center)<=e.radius*e.radius}intersectsPlane(e){let t,i;return e.normal.x>0?(t=e.normal.x*this.min.x,i=e.normal.x*this.max.x):(t=e.normal.x*this.max.x,i=e.normal.x*this.min.x),e.normal.y>0?(t+=e.normal.y*this.min.y,i+=e.normal.y*this.max.y):(t+=e.normal.y*this.max.y,i+=e.normal.y*this.min.y),e.normal.z>0?(t+=e.normal.z*this.min.z,i+=e.normal.z*this.max.z):(t+=e.normal.z*this.max.z,i+=e.normal.z*this.min.z),t<=-e.constant&&i>=-e.constant}intersectsTriangle(e){if(this.isEmpty())return!1;this.getCenter(Is),Ir.subVectors(this.max,Is),Xi.subVectors(e.a,Is),qi.subVectors(e.b,Is),$i.subVectors(e.c,Is),Kn.subVectors(qi,Xi),jn.subVectors($i,qi),vi.subVectors(Xi,$i);let t=[0,-Kn.z,Kn.y,0,-jn.z,jn.y,0,-vi.z,vi.y,Kn.z,0,-Kn.x,jn.z,0,-jn.x,vi.z,0,-vi.x,-Kn.y,Kn.x,0,-jn.y,jn.x,0,-vi.y,vi.x,0];return!Ko(t,Xi,qi,$i,Ir)||(t=[1,0,0,0,1,0,0,0,1],!Ko(t,Xi,qi,$i,Ir))?!1:(Pr.crossVectors(Kn,jn),t=[Pr.x,Pr.y,Pr.z],Ko(t,Xi,qi,$i,Ir))}clampPoint(e,t){return t.copy(e).clamp(this.min,this.max)}distanceToPoint(e){return this.clampPoint(e,ln).distanceTo(e)}getBoundingSphere(e){return this.isEmpty()?e.makeEmpty():(this.getCenter(e.center),e.radius=this.getSize(ln).length()*.5),e}intersect(e){return this.min.max(e.min),this.max.min(e.max),this.isEmpty()&&this.makeEmpty(),this}union(e){return this.min.min(e.min),this.max.max(e.max),this}applyMatrix4(e){return this.isEmpty()?this:(Ln[0].set(this.min.x,this.min.y,this.min.z).applyMatrix4(e),Ln[1].set(this.min.x,this.min.y,this.max.z).applyMatrix4(e),Ln[2].set(this.min.x,this.max.y,this.min.z).applyMatrix4(e),Ln[3].set(this.min.x,this.max.y,this.max.z).applyMatrix4(e),Ln[4].set(this.max.x,this.min.y,this.min.z).applyMatrix4(e),Ln[5].set(this.max.x,this.min.y,this.max.z).applyMatrix4(e),Ln[6].set(this.max.x,this.max.y,this.min.z).applyMatrix4(e),Ln[7].set(this.max.x,this.max.y,this.max.z).applyMatrix4(e),this.setFromPoints(Ln),this)}translate(e){return this.min.add(e),this.max.add(e),this}equals(e){return e.min.equals(this.min)&&e.max.equals(this.max)}toJSON(){return{min:this.min.toArray(),max:this.max.toArray()}}fromJSON(e){return this.min.fromArray(e.min),this.max.fromArray(e.max),this}},Ln=[new L,new L,new L,new L,new L,new L,new L,new L],ln=new L,Cr=new ai,Xi=new L,qi=new L,$i=new L,Kn=new L,jn=new L,vi=new L,Is=new L,Ir=new L,Pr=new L,yi=new L;function Ko(n,e,t,i,s){for(let r=0,o=n.length-3;r<=o;r+=3){yi.fromArray(n,r);let a=s.x*Math.abs(yi.x)+s.y*Math.abs(yi.y)+s.z*Math.abs(yi.z),c=e.dot(yi),l=t.dot(yi),h=i.dot(yi);if(Math.max(-Math.max(c,l,h),Math.min(c,l,h))>a)return!1}return!0}var $d=new ai,Ps=new L,jo=new L,hs=class{constructor(e=new L,t=-1){this.isSphere=!0,this.center=e,this.radius=t}set(e,t){return this.center.copy(e),this.radius=t,this}setFromPoints(e,t){let i=this.center;t!==void 0?i.copy(t):$d.setFromPoints(e).getCenter(i);let s=0;for(let r=0,o=e.length;r<o;r++)s=Math.max(s,i.distanceToSquared(e[r]));return this.radius=Math.sqrt(s),this}copy(e){return this.center.copy(e.center),this.radius=e.radius,this}isEmpty(){return this.radius<0}makeEmpty(){return this.center.set(0,0,0),this.radius=-1,this}containsPoint(e){return e.distanceToSquared(this.center)<=this.radius*this.radius}distanceToPoint(e){return e.distanceTo(this.center)-this.radius}intersectsSphere(e){let t=this.radius+e.radius;return e.center.distanceToSquared(this.center)<=t*t}intersectsBox(e){return e.intersectsSphere(this)}intersectsPlane(e){return Math.abs(e.distanceToPoint(this.center))<=this.radius}clampPoint(e,t){let i=this.center.distanceToSquared(e);return t.copy(e),i>this.radius*this.radius&&(t.sub(this.center).normalize(),t.multiplyScalar(this.radius).add(this.center)),t}getBoundingBox(e){return this.isEmpty()?(e.makeEmpty(),e):(e.set(this.center,this.center),e.expandByScalar(this.radius),e)}applyMatrix4(e){return this.center.applyMatrix4(e),this.radius=this.radius*e.getMaxScaleOnAxis(),this}translate(e){return this.center.add(e),this}expandByPoint(e){if(this.isEmpty())return this.center.copy(e),this.radius=0,this;Ps.subVectors(e,this.center);let t=Ps.lengthSq();if(t>this.radius*this.radius){let i=Math.sqrt(t),s=(i-this.radius)*.5;this.center.addScaledVector(Ps,s/i),this.radius+=s}return this}union(e){return e.isEmpty()?this:this.isEmpty()?(this.copy(e),this):(this.center.equals(e.center)===!0?this.radius=Math.max(this.radius,e.radius):(jo.subVectors(e.center,this.center).setLength(e.radius),this.expandByPoint(Ps.copy(e.center).add(jo)),this.expandByPoint(Ps.copy(e.center).sub(jo))),this)}equals(e){return e.center.equals(this.center)&&e.radius===this.radius}clone(){return new this.constructor().copy(this)}toJSON(){return{radius:this.radius,center:this.center.toArray()}}fromJSON(e){return this.radius=e.radius,this.center.fromArray(e.center),this}},Dn=new L,Qo=new L,Lr=new L,Qn=new L,el=new L,Dr=new L,tl=new L,Hs=class{constructor(e=new L,t=new L(0,0,-1)){this.origin=e,this.direction=t}set(e,t){return this.origin.copy(e),this.direction.copy(t),this}copy(e){return this.origin.copy(e.origin),this.direction.copy(e.direction),this}at(e,t){return t.copy(this.origin).addScaledVector(this.direction,e)}lookAt(e){return this.direction.copy(e).sub(this.origin).normalize(),this}recast(e){return this.origin.copy(this.at(e,Dn)),this}closestPointToPoint(e,t){t.subVectors(e,this.origin);let i=t.dot(this.direction);return i<0?t.copy(this.origin):t.copy(this.origin).addScaledVector(this.direction,i)}distanceToPoint(e){return Math.sqrt(this.distanceSqToPoint(e))}distanceSqToPoint(e){let t=Dn.subVectors(e,this.origin).dot(this.direction);return t<0?this.origin.distanceToSquared(e):(Dn.copy(this.origin).addScaledVector(this.direction,t),Dn.distanceToSquared(e))}distanceSqToSegment(e,t,i,s){Qo.copy(e).add(t).multiplyScalar(.5),Lr.copy(t).sub(e).normalize(),Qn.copy(this.origin).sub(Qo);let r=e.distanceTo(t)*.5,o=-this.direction.dot(Lr),a=Qn.dot(this.direction),c=-Qn.dot(Lr),l=Qn.lengthSq(),h=Math.abs(1-o*o),u,p,d,g;if(h>0)if(u=o*c-a,p=o*a-c,g=r*h,u>=0)if(p>=-g)if(p<=g){let v=1/h;u*=v,p*=v,d=u*(u+o*p+2*a)+p*(o*u+p+2*c)+l}else p=r,u=Math.max(0,-(o*p+a)),d=-u*u+p*(p+2*c)+l;else p=-r,u=Math.max(0,-(o*p+a)),d=-u*u+p*(p+2*c)+l;else p<=-g?(u=Math.max(0,-(-o*r+a)),p=u>0?-r:Math.min(Math.max(-r,-c),r),d=-u*u+p*(p+2*c)+l):p<=g?(u=0,p=Math.min(Math.max(-r,-c),r),d=p*(p+2*c)+l):(u=Math.max(0,-(o*r+a)),p=u>0?r:Math.min(Math.max(-r,-c),r),d=-u*u+p*(p+2*c)+l);else p=o>0?-r:r,u=Math.max(0,-(o*p+a)),d=-u*u+p*(p+2*c)+l;return i&&i.copy(this.origin).addScaledVector(this.direction,u),s&&s.copy(Qo).addScaledVector(Lr,p),d}intersectSphere(e,t){Dn.subVectors(e.center,this.origin);let i=Dn.dot(this.direction),s=Dn.dot(Dn)-i*i,r=e.radius*e.radius;if(s>r)return null;let o=Math.sqrt(r-s),a=i-o,c=i+o;return c<0?null:a<0?this.at(c,t):this.at(a,t)}intersectsSphere(e){return e.radius<0?!1:this.distanceSqToPoint(e.center)<=e.radius*e.radius}distanceToPlane(e){let t=e.normal.dot(this.direction);if(t===0)return e.distanceToPoint(this.origin)===0?0:null;let i=-(this.origin.dot(e.normal)+e.constant)/t;return i>=0?i:null}intersectPlane(e,t){let i=this.distanceToPlane(e);return i===null?null:this.at(i,t)}intersectsPlane(e){let t=e.distanceToPoint(this.origin);return t===0||e.normal.dot(this.direction)*t<0}intersectBox(e,t){let i,s,r,o,a,c,l=1/this.direction.x,h=1/this.direction.y,u=1/this.direction.z,p=this.origin;return l>=0?(i=(e.min.x-p.x)*l,s=(e.max.x-p.x)*l):(i=(e.max.x-p.x)*l,s=(e.min.x-p.x)*l),h>=0?(r=(e.min.y-p.y)*h,o=(e.max.y-p.y)*h):(r=(e.max.y-p.y)*h,o=(e.min.y-p.y)*h),i>o||r>s||((r>i||isNaN(i))&&(i=r),(o<s||isNaN(s))&&(s=o),u>=0?(a=(e.min.z-p.z)*u,c=(e.max.z-p.z)*u):(a=(e.max.z-p.z)*u,c=(e.min.z-p.z)*u),i>c||a>s)||((a>i||i!==i)&&(i=a),(c<s||s!==s)&&(s=c),s<0)?null:this.at(i>=0?i:s,t)}intersectsBox(e){return this.intersectBox(e,Dn)!==null}intersectTriangle(e,t,i,s,r){el.subVectors(t,e),Dr.subVectors(i,e),tl.crossVectors(el,Dr);let o=this.direction.dot(tl),a;if(o>0){if(s)return null;a=1}else if(o<0)a=-1,o=-o;else return null;Qn.subVectors(this.origin,e);let c=a*this.direction.dot(Dr.crossVectors(Qn,Dr));if(c<0)return null;let l=a*this.direction.dot(el.cross(Qn));if(l<0||c+l>o)return null;let h=-a*Qn.dot(tl);return h<0?null:this.at(h/o,r)}applyMatrix4(e){return this.origin.applyMatrix4(e),this.direction.transformDirection(e),this}equals(e){return e.origin.equals(this.origin)&&e.direction.equals(this.direction)}clone(){return new this.constructor().copy(this)}},dt=class n{constructor(e,t,i,s,r,o,a,c,l,h,u,p,d,g,v,m){n.prototype.isMatrix4=!0,this.elements=[1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1],e!==void 0&&this.set(e,t,i,s,r,o,a,c,l,h,u,p,d,g,v,m)}set(e,t,i,s,r,o,a,c,l,h,u,p,d,g,v,m){let f=this.elements;return f[0]=e,f[4]=t,f[8]=i,f[12]=s,f[1]=r,f[5]=o,f[9]=a,f[13]=c,f[2]=l,f[6]=h,f[10]=u,f[14]=p,f[3]=d,f[7]=g,f[11]=v,f[15]=m,this}identity(){return this.set(1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1),this}clone(){return new n().fromArray(this.elements)}copy(e){let t=this.elements,i=e.elements;return t[0]=i[0],t[1]=i[1],t[2]=i[2],t[3]=i[3],t[4]=i[4],t[5]=i[5],t[6]=i[6],t[7]=i[7],t[8]=i[8],t[9]=i[9],t[10]=i[10],t[11]=i[11],t[12]=i[12],t[13]=i[13],t[14]=i[14],t[15]=i[15],this}copyPosition(e){let t=this.elements,i=e.elements;return t[12]=i[12],t[13]=i[13],t[14]=i[14],this}setFromMatrix3(e){let t=e.elements;return this.set(t[0],t[3],t[6],0,t[1],t[4],t[7],0,t[2],t[5],t[8],0,0,0,0,1),this}extractBasis(e,t,i){return e.setFromMatrixColumn(this,0),t.setFromMatrixColumn(this,1),i.setFromMatrixColumn(this,2),this}makeBasis(e,t,i){return this.set(e.x,t.x,i.x,0,e.y,t.y,i.y,0,e.z,t.z,i.z,0,0,0,0,1),this}extractRotation(e){let t=this.elements,i=e.elements,s=1/Yi.setFromMatrixColumn(e,0).length(),r=1/Yi.setFromMatrixColumn(e,1).length(),o=1/Yi.setFromMatrixColumn(e,2).length();return t[0]=i[0]*s,t[1]=i[1]*s,t[2]=i[2]*s,t[3]=0,t[4]=i[4]*r,t[5]=i[5]*r,t[6]=i[6]*r,t[7]=0,t[8]=i[8]*o,t[9]=i[9]*o,t[10]=i[10]*o,t[11]=0,t[12]=0,t[13]=0,t[14]=0,t[15]=1,this}makeRotationFromEuler(e){let t=this.elements,i=e.x,s=e.y,r=e.z,o=Math.cos(i),a=Math.sin(i),c=Math.cos(s),l=Math.sin(s),h=Math.cos(r),u=Math.sin(r);if(e.order==="XYZ"){let p=o*h,d=o*u,g=a*h,v=a*u;t[0]=c*h,t[4]=-c*u,t[8]=l,t[1]=d+g*l,t[5]=p-v*l,t[9]=-a*c,t[2]=v-p*l,t[6]=g+d*l,t[10]=o*c}else if(e.order==="YXZ"){let p=c*h,d=c*u,g=l*h,v=l*u;t[0]=p+v*a,t[4]=g*a-d,t[8]=o*l,t[1]=o*u,t[5]=o*h,t[9]=-a,t[2]=d*a-g,t[6]=v+p*a,t[10]=o*c}else if(e.order==="ZXY"){let p=c*h,d=c*u,g=l*h,v=l*u;t[0]=p-v*a,t[4]=-o*u,t[8]=g+d*a,t[1]=d+g*a,t[5]=o*h,t[9]=v-p*a,t[2]=-o*l,t[6]=a,t[10]=o*c}else if(e.order==="ZYX"){let p=o*h,d=o*u,g=a*h,v=a*u;t[0]=c*h,t[4]=g*l-d,t[8]=p*l+v,t[1]=c*u,t[5]=v*l+p,t[9]=d*l-g,t[2]=-l,t[6]=a*c,t[10]=o*c}else if(e.order==="YZX"){let p=o*c,d=o*l,g=a*c,v=a*l;t[0]=c*h,t[4]=v-p*u,t[8]=g*u+d,t[1]=u,t[5]=o*h,t[9]=-a*h,t[2]=-l*h,t[6]=d*u+g,t[10]=p-v*u}else if(e.order==="XZY"){let p=o*c,d=o*l,g=a*c,v=a*l;t[0]=c*h,t[4]=-u,t[8]=l*h,t[1]=p*u+v,t[5]=o*h,t[9]=d*u-g,t[2]=g*u-d,t[6]=a*h,t[10]=v*u+p}return t[3]=0,t[7]=0,t[11]=0,t[12]=0,t[13]=0,t[14]=0,t[15]=1,this}makeRotationFromQuaternion(e){return this.compose(Yd,e,Zd)}lookAt(e,t,i){let s=this.elements;return Kt.subVectors(e,t),Kt.lengthSq()===0&&(Kt.z=1),Kt.normalize(),ei.crossVectors(i,Kt),ei.lengthSq()===0&&(Math.abs(i.z)===1?Kt.x+=1e-4:Kt.z+=1e-4,Kt.normalize(),ei.crossVectors(i,Kt)),ei.normalize(),Ur.crossVectors(Kt,ei),s[0]=ei.x,s[4]=Ur.x,s[8]=Kt.x,s[1]=ei.y,s[5]=Ur.y,s[9]=Kt.y,s[2]=ei.z,s[6]=Ur.z,s[10]=Kt.z,this}multiply(e){return this.multiplyMatrices(this,e)}premultiply(e){return this.multiplyMatrices(e,this)}multiplyMatrices(e,t){let i=e.elements,s=t.elements,r=this.elements,o=i[0],a=i[4],c=i[8],l=i[12],h=i[1],u=i[5],p=i[9],d=i[13],g=i[2],v=i[6],m=i[10],f=i[14],T=i[3],E=i[7],S=i[11],R=i[15],A=s[0],P=s[4],N=s[8],M=s[12],y=s[1],I=s[5],O=s[9],z=s[13],H=s[2],q=s[6],W=s[10],ne=s[14],V=s[3],ae=s[7],ue=s[11],Ee=s[15];return r[0]=o*A+a*y+c*H+l*V,r[4]=o*P+a*I+c*q+l*ae,r[8]=o*N+a*O+c*W+l*ue,r[12]=o*M+a*z+c*ne+l*Ee,r[1]=h*A+u*y+p*H+d*V,r[5]=h*P+u*I+p*q+d*ae,r[9]=h*N+u*O+p*W+d*ue,r[13]=h*M+u*z+p*ne+d*Ee,r[2]=g*A+v*y+m*H+f*V,r[6]=g*P+v*I+m*q+f*ae,r[10]=g*N+v*O+m*W+f*ue,r[14]=g*M+v*z+m*ne+f*Ee,r[3]=T*A+E*y+S*H+R*V,r[7]=T*P+E*I+S*q+R*ae,r[11]=T*N+E*O+S*W+R*ue,r[15]=T*M+E*z+S*ne+R*Ee,this}multiplyScalar(e){let t=this.elements;return t[0]*=e,t[4]*=e,t[8]*=e,t[12]*=e,t[1]*=e,t[5]*=e,t[9]*=e,t[13]*=e,t[2]*=e,t[6]*=e,t[10]*=e,t[14]*=e,t[3]*=e,t[7]*=e,t[11]*=e,t[15]*=e,this}determinant(){let e=this.elements,t=e[0],i=e[4],s=e[8],r=e[12],o=e[1],a=e[5],c=e[9],l=e[13],h=e[2],u=e[6],p=e[10],d=e[14],g=e[3],v=e[7],m=e[11],f=e[15];return g*(+r*c*u-s*l*u-r*a*p+i*l*p+s*a*d-i*c*d)+v*(+t*c*d-t*l*p+r*o*p-s*o*d+s*l*h-r*c*h)+m*(+t*l*u-t*a*d-r*o*u+i*o*d+r*a*h-i*l*h)+f*(-s*a*h-t*c*u+t*a*p+s*o*u-i*o*p+i*c*h)}transpose(){let e=this.elements,t;return t=e[1],e[1]=e[4],e[4]=t,t=e[2],e[2]=e[8],e[8]=t,t=e[6],e[6]=e[9],e[9]=t,t=e[3],e[3]=e[12],e[12]=t,t=e[7],e[7]=e[13],e[13]=t,t=e[11],e[11]=e[14],e[14]=t,this}setPosition(e,t,i){let s=this.elements;return e.isVector3?(s[12]=e.x,s[13]=e.y,s[14]=e.z):(s[12]=e,s[13]=t,s[14]=i),this}invert(){let e=this.elements,t=e[0],i=e[1],s=e[2],r=e[3],o=e[4],a=e[5],c=e[6],l=e[7],h=e[8],u=e[9],p=e[10],d=e[11],g=e[12],v=e[13],m=e[14],f=e[15],T=u*m*l-v*p*l+v*c*d-a*m*d-u*c*f+a*p*f,E=g*p*l-h*m*l-g*c*d+o*m*d+h*c*f-o*p*f,S=h*v*l-g*u*l+g*a*d-o*v*d-h*a*f+o*u*f,R=g*u*c-h*v*c-g*a*p+o*v*p+h*a*m-o*u*m,A=t*T+i*E+s*S+r*R;if(A===0)return this.set(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);let P=1/A;return e[0]=T*P,e[1]=(v*p*r-u*m*r-v*s*d+i*m*d+u*s*f-i*p*f)*P,e[2]=(a*m*r-v*c*r+v*s*l-i*m*l-a*s*f+i*c*f)*P,e[3]=(u*c*r-a*p*r-u*s*l+i*p*l+a*s*d-i*c*d)*P,e[4]=E*P,e[5]=(h*m*r-g*p*r+g*s*d-t*m*d-h*s*f+t*p*f)*P,e[6]=(g*c*r-o*m*r-g*s*l+t*m*l+o*s*f-t*c*f)*P,e[7]=(o*p*r-h*c*r+h*s*l-t*p*l-o*s*d+t*c*d)*P,e[8]=S*P,e[9]=(g*u*r-h*v*r-g*i*d+t*v*d+h*i*f-t*u*f)*P,e[10]=(o*v*r-g*a*r+g*i*l-t*v*l-o*i*f+t*a*f)*P,e[11]=(h*a*r-o*u*r-h*i*l+t*u*l+o*i*d-t*a*d)*P,e[12]=R*P,e[13]=(h*v*s-g*u*s+g*i*p-t*v*p-h*i*m+t*u*m)*P,e[14]=(g*a*s-o*v*s-g*i*c+t*v*c+o*i*m-t*a*m)*P,e[15]=(o*u*s-h*a*s+h*i*c-t*u*c-o*i*p+t*a*p)*P,this}scale(e){let t=this.elements,i=e.x,s=e.y,r=e.z;return t[0]*=i,t[4]*=s,t[8]*=r,t[1]*=i,t[5]*=s,t[9]*=r,t[2]*=i,t[6]*=s,t[10]*=r,t[3]*=i,t[7]*=s,t[11]*=r,this}getMaxScaleOnAxis(){let e=this.elements,t=e[0]*e[0]+e[1]*e[1]+e[2]*e[2],i=e[4]*e[4]+e[5]*e[5]+e[6]*e[6],s=e[8]*e[8]+e[9]*e[9]+e[10]*e[10];return Math.sqrt(Math.max(t,i,s))}makeTranslation(e,t,i){return e.isVector3?this.set(1,0,0,e.x,0,1,0,e.y,0,0,1,e.z,0,0,0,1):this.set(1,0,0,e,0,1,0,t,0,0,1,i,0,0,0,1),this}makeRotationX(e){let t=Math.cos(e),i=Math.sin(e);return this.set(1,0,0,0,0,t,-i,0,0,i,t,0,0,0,0,1),this}makeRotationY(e){let t=Math.cos(e),i=Math.sin(e);return this.set(t,0,i,0,0,1,0,0,-i,0,t,0,0,0,0,1),this}makeRotationZ(e){let t=Math.cos(e),i=Math.sin(e);return this.set(t,-i,0,0,i,t,0,0,0,0,1,0,0,0,0,1),this}makeRotationAxis(e,t){let i=Math.cos(t),s=Math.sin(t),r=1-i,o=e.x,a=e.y,c=e.z,l=r*o,h=r*a;return this.set(l*o+i,l*a-s*c,l*c+s*a,0,l*a+s*c,h*a+i,h*c-s*o,0,l*c-s*a,h*c+s*o,r*c*c+i,0,0,0,0,1),this}makeScale(e,t,i){return this.set(e,0,0,0,0,t,0,0,0,0,i,0,0,0,0,1),this}makeShear(e,t,i,s,r,o){return this.set(1,i,r,0,e,1,o,0,t,s,1,0,0,0,0,1),this}compose(e,t,i){let s=this.elements,r=t._x,o=t._y,a=t._z,c=t._w,l=r+r,h=o+o,u=a+a,p=r*l,d=r*h,g=r*u,v=o*h,m=o*u,f=a*u,T=c*l,E=c*h,S=c*u,R=i.x,A=i.y,P=i.z;return s[0]=(1-(v+f))*R,s[1]=(d+S)*R,s[2]=(g-E)*R,s[3]=0,s[4]=(d-S)*A,s[5]=(1-(p+f))*A,s[6]=(m+T)*A,s[7]=0,s[8]=(g+E)*P,s[9]=(m-T)*P,s[10]=(1-(p+v))*P,s[11]=0,s[12]=e.x,s[13]=e.y,s[14]=e.z,s[15]=1,this}decompose(e,t,i){let s=this.elements,r=Yi.set(s[0],s[1],s[2]).length(),o=Yi.set(s[4],s[5],s[6]).length(),a=Yi.set(s[8],s[9],s[10]).length();this.determinant()<0&&(r=-r),e.x=s[12],e.y=s[13],e.z=s[14],cn.copy(this);let l=1/r,h=1/o,u=1/a;return cn.elements[0]*=l,cn.elements[1]*=l,cn.elements[2]*=l,cn.elements[4]*=h,cn.elements[5]*=h,cn.elements[6]*=h,cn.elements[8]*=u,cn.elements[9]*=u,cn.elements[10]*=u,t.setFromRotationMatrix(cn),i.x=r,i.y=o,i.z=a,this}makePerspective(e,t,i,s,r,o,a=un,c=!1){let l=this.elements,h=2*r/(t-e),u=2*r/(i-s),p=(t+e)/(t-e),d=(i+s)/(i-s),g,v;if(c)g=r/(o-r),v=o*r/(o-r);else if(a===un)g=-(o+r)/(o-r),v=-2*o*r/(o-r);else if(a===zs)g=-o/(o-r),v=-o*r/(o-r);else throw new Error("THREE.Matrix4.makePerspective(): Invalid coordinate system: "+a);return l[0]=h,l[4]=0,l[8]=p,l[12]=0,l[1]=0,l[5]=u,l[9]=d,l[13]=0,l[2]=0,l[6]=0,l[10]=g,l[14]=v,l[3]=0,l[7]=0,l[11]=-1,l[15]=0,this}makeOrthographic(e,t,i,s,r,o,a=un,c=!1){let l=this.elements,h=2/(t-e),u=2/(i-s),p=-(t+e)/(t-e),d=-(i+s)/(i-s),g,v;if(c)g=1/(o-r),v=o/(o-r);else if(a===un)g=-2/(o-r),v=-(o+r)/(o-r);else if(a===zs)g=-1/(o-r),v=-r/(o-r);else throw new Error("THREE.Matrix4.makeOrthographic(): Invalid coordinate system: "+a);return l[0]=h,l[4]=0,l[8]=0,l[12]=p,l[1]=0,l[5]=u,l[9]=0,l[13]=d,l[2]=0,l[6]=0,l[10]=g,l[14]=v,l[3]=0,l[7]=0,l[11]=0,l[15]=1,this}equals(e){let t=this.elements,i=e.elements;for(let s=0;s<16;s++)if(t[s]!==i[s])return!1;return!0}fromArray(e,t=0){for(let i=0;i<16;i++)this.elements[i]=e[i+t];return this}toArray(e=[],t=0){let i=this.elements;return e[t]=i[0],e[t+1]=i[1],e[t+2]=i[2],e[t+3]=i[3],e[t+4]=i[4],e[t+5]=i[5],e[t+6]=i[6],e[t+7]=i[7],e[t+8]=i[8],e[t+9]=i[9],e[t+10]=i[10],e[t+11]=i[11],e[t+12]=i[12],e[t+13]=i[13],e[t+14]=i[14],e[t+15]=i[15],e}},Yi=new L,cn=new dt,Yd=new L(0,0,0),Zd=new L(1,1,1),ei=new L,Ur=new L,Kt=new L,mh=new dt,gh=new Vn,fn=class n{constructor(e=0,t=0,i=0,s=n.DEFAULT_ORDER){this.isEuler=!0,this._x=e,this._y=t,this._z=i,this._order=s}get x(){return this._x}set x(e){this._x=e,this._onChangeCallback()}get y(){return this._y}set y(e){this._y=e,this._onChangeCallback()}get z(){return this._z}set z(e){this._z=e,this._onChangeCallback()}get order(){return this._order}set order(e){this._order=e,this._onChangeCallback()}set(e,t,i,s=this._order){return this._x=e,this._y=t,this._z=i,this._order=s,this._onChangeCallback(),this}clone(){return new this.constructor(this._x,this._y,this._z,this._order)}copy(e){return this._x=e._x,this._y=e._y,this._z=e._z,this._order=e._order,this._onChangeCallback(),this}setFromRotationMatrix(e,t=this._order,i=!0){let s=e.elements,r=s[0],o=s[4],a=s[8],c=s[1],l=s[5],h=s[9],u=s[2],p=s[6],d=s[10];switch(t){case"XYZ":this._y=Math.asin(qe(a,-1,1)),Math.abs(a)<.9999999?(this._x=Math.atan2(-h,d),this._z=Math.atan2(-o,r)):(this._x=Math.atan2(p,l),this._z=0);break;case"YXZ":this._x=Math.asin(-qe(h,-1,1)),Math.abs(h)<.9999999?(this._y=Math.atan2(a,d),this._z=Math.atan2(c,l)):(this._y=Math.atan2(-u,r),this._z=0);break;case"ZXY":this._x=Math.asin(qe(p,-1,1)),Math.abs(p)<.9999999?(this._y=Math.atan2(-u,d),this._z=Math.atan2(-o,l)):(this._y=0,this._z=Math.atan2(c,r));break;case"ZYX":this._y=Math.asin(-qe(u,-1,1)),Math.abs(u)<.9999999?(this._x=Math.atan2(p,d),this._z=Math.atan2(c,r)):(this._x=0,this._z=Math.atan2(-o,l));break;case"YZX":this._z=Math.asin(qe(c,-1,1)),Math.abs(c)<.9999999?(this._x=Math.atan2(-h,l),this._y=Math.atan2(-u,r)):(this._x=0,this._y=Math.atan2(a,d));break;case"XZY":this._z=Math.asin(-qe(o,-1,1)),Math.abs(o)<.9999999?(this._x=Math.atan2(p,l),this._y=Math.atan2(a,r)):(this._x=Math.atan2(-h,d),this._y=0);break;default:console.warn("THREE.Euler: .setFromRotationMatrix() encountered an unknown order: "+t)}return this._order=t,i===!0&&this._onChangeCallback(),this}setFromQuaternion(e,t,i){return mh.makeRotationFromQuaternion(e),this.setFromRotationMatrix(mh,t,i)}setFromVector3(e,t=this._order){return this.set(e.x,e.y,e.z,t)}reorder(e){return gh.setFromEuler(this),this.setFromQuaternion(gh,e)}equals(e){return e._x===this._x&&e._y===this._y&&e._z===this._z&&e._order===this._order}fromArray(e){return this._x=e[0],this._y=e[1],this._z=e[2],e[3]!==void 0&&(this._order=e[3]),this._onChangeCallback(),this}toArray(e=[],t=0){return e[t]=this._x,e[t+1]=this._y,e[t+2]=this._z,e[t+3]=this._order,e}_onChange(e){return this._onChangeCallback=e,this}_onChangeCallback(){}*[Symbol.iterator](){yield this._x,yield this._y,yield this._z,yield this._order}};fn.DEFAULT_ORDER="XYZ";var us=class{constructor(){this.mask=1}set(e){this.mask=(1<<e|0)>>>0}enable(e){this.mask|=1<<e|0}enableAll(){this.mask=-1}toggle(e){this.mask^=1<<e|0}disable(e){this.mask&=~(1<<e|0)}disableAll(){this.mask=0}test(e){return(this.mask&e.mask)!==0}isEnabled(e){return(this.mask&(1<<e|0))!==0}},Jd=0,_h=new L,Zi=new Vn,Un=new dt,Nr=new L,Ls=new L,Kd=new L,jd=new Vn,xh=new L(1,0,0),vh=new L(0,1,0),yh=new L(0,0,1),Mh={type:"added"},Qd={type:"removed"},Ji={type:"childadded",child:null},nl={type:"childremoved",child:null},wt=class n extends zn{constructor(){super(),this.isObject3D=!0,Object.defineProperty(this,"id",{value:Jd++}),this.uuid=si(),this.name="",this.type="Object3D",this.parent=null,this.children=[],this.up=n.DEFAULT_UP.clone();let e=new L,t=new fn,i=new Vn,s=new L(1,1,1);function r(){i.setFromEuler(t,!1)}function o(){t.setFromQuaternion(i,void 0,!1)}t._onChange(r),i._onChange(o),Object.defineProperties(this,{position:{configurable:!0,enumerable:!0,value:e},rotation:{configurable:!0,enumerable:!0,value:t},quaternion:{configurable:!0,enumerable:!0,value:i},scale:{configurable:!0,enumerable:!0,value:s},modelViewMatrix:{value:new dt},normalMatrix:{value:new Oe}}),this.matrix=new dt,this.matrixWorld=new dt,this.matrixAutoUpdate=n.DEFAULT_MATRIX_AUTO_UPDATE,this.matrixWorldAutoUpdate=n.DEFAULT_MATRIX_WORLD_AUTO_UPDATE,this.matrixWorldNeedsUpdate=!1,this.layers=new us,this.visible=!0,this.castShadow=!1,this.receiveShadow=!1,this.frustumCulled=!0,this.renderOrder=0,this.animations=[],this.customDepthMaterial=void 0,this.customDistanceMaterial=void 0,this.userData={}}onBeforeShadow(){}onAfterShadow(){}onBeforeRender(){}onAfterRender(){}applyMatrix4(e){this.matrixAutoUpdate&&this.updateMatrix(),this.matrix.premultiply(e),this.matrix.decompose(this.position,this.quaternion,this.scale)}applyQuaternion(e){return this.quaternion.premultiply(e),this}setRotationFromAxisAngle(e,t){this.quaternion.setFromAxisAngle(e,t)}setRotationFromEuler(e){this.quaternion.setFromEuler(e,!0)}setRotationFromMatrix(e){this.quaternion.setFromRotationMatrix(e)}setRotationFromQuaternion(e){this.quaternion.copy(e)}rotateOnAxis(e,t){return Zi.setFromAxisAngle(e,t),this.quaternion.multiply(Zi),this}rotateOnWorldAxis(e,t){return Zi.setFromAxisAngle(e,t),this.quaternion.premultiply(Zi),this}rotateX(e){return this.rotateOnAxis(xh,e)}rotateY(e){return this.rotateOnAxis(vh,e)}rotateZ(e){return this.rotateOnAxis(yh,e)}translateOnAxis(e,t){return _h.copy(e).applyQuaternion(this.quaternion),this.position.add(_h.multiplyScalar(t)),this}translateX(e){return this.translateOnAxis(xh,e)}translateY(e){return this.translateOnAxis(vh,e)}translateZ(e){return this.translateOnAxis(yh,e)}localToWorld(e){return this.updateWorldMatrix(!0,!1),e.applyMatrix4(this.matrixWorld)}worldToLocal(e){return this.updateWorldMatrix(!0,!1),e.applyMatrix4(Un.copy(this.matrixWorld).invert())}lookAt(e,t,i){e.isVector3?Nr.copy(e):Nr.set(e,t,i);let s=this.parent;this.updateWorldMatrix(!0,!1),Ls.setFromMatrixPosition(this.matrixWorld),this.isCamera||this.isLight?Un.lookAt(Ls,Nr,this.up):Un.lookAt(Nr,Ls,this.up),this.quaternion.setFromRotationMatrix(Un),s&&(Un.extractRotation(s.matrixWorld),Zi.setFromRotationMatrix(Un),this.quaternion.premultiply(Zi.invert()))}add(e){if(arguments.length>1){for(let t=0;t<arguments.length;t++)this.add(arguments[t]);return this}return e===this?(console.error("THREE.Object3D.add: object can't be added as a child of itself.",e),this):(e&&e.isObject3D?(e.removeFromParent(),e.parent=this,this.children.push(e),e.dispatchEvent(Mh),Ji.child=e,this.dispatchEvent(Ji),Ji.child=null):console.error("THREE.Object3D.add: object not an instance of THREE.Object3D.",e),this)}remove(e){if(arguments.length>1){for(let i=0;i<arguments.length;i++)this.remove(arguments[i]);return this}let t=this.children.indexOf(e);return t!==-1&&(e.parent=null,this.children.splice(t,1),e.dispatchEvent(Qd),nl.child=e,this.dispatchEvent(nl),nl.child=null),this}removeFromParent(){let e=this.parent;return e!==null&&e.remove(this),this}clear(){return this.remove(...this.children)}attach(e){return this.updateWorldMatrix(!0,!1),Un.copy(this.matrixWorld).invert(),e.parent!==null&&(e.parent.updateWorldMatrix(!0,!1),Un.multiply(e.parent.matrixWorld)),e.applyMatrix4(Un),e.removeFromParent(),e.parent=this,this.children.push(e),e.updateWorldMatrix(!1,!0),e.dispatchEvent(Mh),Ji.child=e,this.dispatchEvent(Ji),Ji.child=null,this}getObjectById(e){return this.getObjectByProperty("id",e)}getObjectByName(e){return this.getObjectByProperty("name",e)}getObjectByProperty(e,t){if(this[e]===t)return this;for(let i=0,s=this.children.length;i<s;i++){let o=this.children[i].getObjectByProperty(e,t);if(o!==void 0)return o}}getObjectsByProperty(e,t,i=[]){this[e]===t&&i.push(this);let s=this.children;for(let r=0,o=s.length;r<o;r++)s[r].getObjectsByProperty(e,t,i);return i}getWorldPosition(e){return this.updateWorldMatrix(!0,!1),e.setFromMatrixPosition(this.matrixWorld)}getWorldQuaternion(e){return this.updateWorldMatrix(!0,!1),this.matrixWorld.decompose(Ls,e,Kd),e}getWorldScale(e){return this.updateWorldMatrix(!0,!1),this.matrixWorld.decompose(Ls,jd,e),e}getWorldDirection(e){this.updateWorldMatrix(!0,!1);let t=this.matrixWorld.elements;return e.set(t[8],t[9],t[10]).normalize()}raycast(){}traverse(e){e(this);let t=this.children;for(let i=0,s=t.length;i<s;i++)t[i].traverse(e)}traverseVisible(e){if(this.visible===!1)return;e(this);let t=this.children;for(let i=0,s=t.length;i<s;i++)t[i].traverseVisible(e)}traverseAncestors(e){let t=this.parent;t!==null&&(e(t),t.traverseAncestors(e))}updateMatrix(){this.matrix.compose(this.position,this.quaternion,this.scale),this.matrixWorldNeedsUpdate=!0}updateMatrixWorld(e){this.matrixAutoUpdate&&this.updateMatrix(),(this.matrixWorldNeedsUpdate||e)&&(this.matrixWorldAutoUpdate===!0&&(this.parent===null?this.matrixWorld.copy(this.matrix):this.matrixWorld.multiplyMatrices(this.parent.matrixWorld,this.matrix)),this.matrixWorldNeedsUpdate=!1,e=!0);let t=this.children;for(let i=0,s=t.length;i<s;i++)t[i].updateMatrixWorld(e)}updateWorldMatrix(e,t){let i=this.parent;if(e===!0&&i!==null&&i.updateWorldMatrix(!0,!1),this.matrixAutoUpdate&&this.updateMatrix(),this.matrixWorldAutoUpdate===!0&&(this.parent===null?this.matrixWorld.copy(this.matrix):this.matrixWorld.multiplyMatrices(this.parent.matrixWorld,this.matrix)),t===!0){let s=this.children;for(let r=0,o=s.length;r<o;r++)s[r].updateWorldMatrix(!1,!0)}}toJSON(e){let t=e===void 0||typeof e=="string",i={};t&&(e={geometries:{},materials:{},textures:{},images:{},shapes:{},skeletons:{},animations:{},nodes:{}},i.metadata={version:4.7,type:"Object",generator:"Object3D.toJSON"});let s={};s.uuid=this.uuid,s.type=this.type,this.name!==""&&(s.name=this.name),this.castShadow===!0&&(s.castShadow=!0),this.receiveShadow===!0&&(s.receiveShadow=!0),this.visible===!1&&(s.visible=!1),this.frustumCulled===!1&&(s.frustumCulled=!1),this.renderOrder!==0&&(s.renderOrder=this.renderOrder),Object.keys(this.userData).length>0&&(s.userData=this.userData),s.layers=this.layers.mask,s.matrix=this.matrix.toArray(),s.up=this.up.toArray(),this.matrixAutoUpdate===!1&&(s.matrixAutoUpdate=!1),this.isInstancedMesh&&(s.type="InstancedMesh",s.count=this.count,s.instanceMatrix=this.instanceMatrix.toJSON(),this.instanceColor!==null&&(s.instanceColor=this.instanceColor.toJSON())),this.isBatchedMesh&&(s.type="BatchedMesh",s.perObjectFrustumCulled=this.perObjectFrustumCulled,s.sortObjects=this.sortObjects,s.drawRanges=this._drawRanges,s.reservedRanges=this._reservedRanges,s.geometryInfo=this._geometryInfo.map(a=>({...a,boundingBox:a.boundingBox?a.boundingBox.toJSON():void 0,boundingSphere:a.boundingSphere?a.boundingSphere.toJSON():void 0})),s.instanceInfo=this._instanceInfo.map(a=>({...a})),s.availableInstanceIds=this._availableInstanceIds.slice(),s.availableGeometryIds=this._availableGeometryIds.slice(),s.nextIndexStart=this._nextIndexStart,s.nextVertexStart=this._nextVertexStart,s.geometryCount=this._geometryCount,s.maxInstanceCount=this._maxInstanceCount,s.maxVertexCount=this._maxVertexCount,s.maxIndexCount=this._maxIndexCount,s.geometryInitialized=this._geometryInitialized,s.matricesTexture=this._matricesTexture.toJSON(e),s.indirectTexture=this._indirectTexture.toJSON(e),this._colorsTexture!==null&&(s.colorsTexture=this._colorsTexture.toJSON(e)),this.boundingSphere!==null&&(s.boundingSphere=this.boundingSphere.toJSON()),this.boundingBox!==null&&(s.boundingBox=this.boundingBox.toJSON()));function r(a,c){return a[c.uuid]===void 0&&(a[c.uuid]=c.toJSON(e)),c.uuid}if(this.isScene)this.background&&(this.background.isColor?s.background=this.background.toJSON():this.background.isTexture&&(s.background=this.background.toJSON(e).uuid)),this.environment&&this.environment.isTexture&&this.environment.isRenderTargetTexture!==!0&&(s.environment=this.environment.toJSON(e).uuid);else if(this.isMesh||this.isLine||this.isPoints){s.geometry=r(e.geometries,this.geometry);let a=this.geometry.parameters;if(a!==void 0&&a.shapes!==void 0){let c=a.shapes;if(Array.isArray(c))for(let l=0,h=c.length;l<h;l++){let u=c[l];r(e.shapes,u)}else r(e.shapes,c)}}if(this.isSkinnedMesh&&(s.bindMode=this.bindMode,s.bindMatrix=this.bindMatrix.toArray(),this.skeleton!==void 0&&(r(e.skeletons,this.skeleton),s.skeleton=this.skeleton.uuid)),this.material!==void 0)if(Array.isArray(this.material)){let a=[];for(let c=0,l=this.material.length;c<l;c++)a.push(r(e.materials,this.material[c]));s.material=a}else s.material=r(e.materials,this.material);if(this.children.length>0){s.children=[];for(let a=0;a<this.children.length;a++)s.children.push(this.children[a].toJSON(e).object)}if(this.animations.length>0){s.animations=[];for(let a=0;a<this.animations.length;a++){let c=this.animations[a];s.animations.push(r(e.animations,c))}}if(t){let a=o(e.geometries),c=o(e.materials),l=o(e.textures),h=o(e.images),u=o(e.shapes),p=o(e.skeletons),d=o(e.animations),g=o(e.nodes);a.length>0&&(i.geometries=a),c.length>0&&(i.materials=c),l.length>0&&(i.textures=l),h.length>0&&(i.images=h),u.length>0&&(i.shapes=u),p.length>0&&(i.skeletons=p),d.length>0&&(i.animations=d),g.length>0&&(i.nodes=g)}return i.object=s,i;function o(a){let c=[];for(let l in a){let h=a[l];delete h.metadata,c.push(h)}return c}}clone(e){return new this.constructor().copy(this,e)}copy(e,t=!0){if(this.name=e.name,this.up.copy(e.up),this.position.copy(e.position),this.rotation.order=e.rotation.order,this.quaternion.copy(e.quaternion),this.scale.copy(e.scale),this.matrix.copy(e.matrix),this.matrixWorld.copy(e.matrixWorld),this.matrixAutoUpdate=e.matrixAutoUpdate,this.matrixWorldAutoUpdate=e.matrixWorldAutoUpdate,this.matrixWorldNeedsUpdate=e.matrixWorldNeedsUpdate,this.layers.mask=e.layers.mask,this.visible=e.visible,this.castShadow=e.castShadow,this.receiveShadow=e.receiveShadow,this.frustumCulled=e.frustumCulled,this.renderOrder=e.renderOrder,this.animations=e.animations.slice(),this.userData=JSON.parse(JSON.stringify(e.userData)),t===!0)for(let i=0;i<e.children.length;i++){let s=e.children[i];this.add(s.clone())}return this}};wt.DEFAULT_UP=new L(0,1,0);wt.DEFAULT_MATRIX_AUTO_UPDATE=!0;wt.DEFAULT_MATRIX_WORLD_AUTO_UPDATE=!0;var hn=new L,Nn=new L,il=new L,Fn=new L,Ki=new L,ji=new L,Sh=new L,sl=new L,rl=new L,al=new L,ol=new Qe,ll=new Qe,cl=new Qe,On=class n{constructor(e=new L,t=new L,i=new L){this.a=e,this.b=t,this.c=i}static getNormal(e,t,i,s){s.subVectors(i,t),hn.subVectors(e,t),s.cross(hn);let r=s.lengthSq();return r>0?s.multiplyScalar(1/Math.sqrt(r)):s.set(0,0,0)}static getBarycoord(e,t,i,s,r){hn.subVectors(s,t),Nn.subVectors(i,t),il.subVectors(e,t);let o=hn.dot(hn),a=hn.dot(Nn),c=hn.dot(il),l=Nn.dot(Nn),h=Nn.dot(il),u=o*l-a*a;if(u===0)return r.set(0,0,0),null;let p=1/u,d=(l*c-a*h)*p,g=(o*h-a*c)*p;return r.set(1-d-g,g,d)}static containsPoint(e,t,i,s){return this.getBarycoord(e,t,i,s,Fn)===null?!1:Fn.x>=0&&Fn.y>=0&&Fn.x+Fn.y<=1}static getInterpolation(e,t,i,s,r,o,a,c){return this.getBarycoord(e,t,i,s,Fn)===null?(c.x=0,c.y=0,"z"in c&&(c.z=0),"w"in c&&(c.w=0),null):(c.setScalar(0),c.addScaledVector(r,Fn.x),c.addScaledVector(o,Fn.y),c.addScaledVector(a,Fn.z),c)}static getInterpolatedAttribute(e,t,i,s,r,o){return ol.setScalar(0),ll.setScalar(0),cl.setScalar(0),ol.fromBufferAttribute(e,t),ll.fromBufferAttribute(e,i),cl.fromBufferAttribute(e,s),o.setScalar(0),o.addScaledVector(ol,r.x),o.addScaledVector(ll,r.y),o.addScaledVector(cl,r.z),o}static isFrontFacing(e,t,i,s){return hn.subVectors(i,t),Nn.subVectors(e,t),hn.cross(Nn).dot(s)<0}set(e,t,i){return this.a.copy(e),this.b.copy(t),this.c.copy(i),this}setFromPointsAndIndices(e,t,i,s){return this.a.copy(e[t]),this.b.copy(e[i]),this.c.copy(e[s]),this}setFromAttributeAndIndices(e,t,i,s){return this.a.fromBufferAttribute(e,t),this.b.fromBufferAttribute(e,i),this.c.fromBufferAttribute(e,s),this}clone(){return new this.constructor().copy(this)}copy(e){return this.a.copy(e.a),this.b.copy(e.b),this.c.copy(e.c),this}getArea(){return hn.subVectors(this.c,this.b),Nn.subVectors(this.a,this.b),hn.cross(Nn).length()*.5}getMidpoint(e){return e.addVectors(this.a,this.b).add(this.c).multiplyScalar(1/3)}getNormal(e){return n.getNormal(this.a,this.b,this.c,e)}getPlane(e){return e.setFromCoplanarPoints(this.a,this.b,this.c)}getBarycoord(e,t){return n.getBarycoord(e,this.a,this.b,this.c,t)}getInterpolation(e,t,i,s,r){return n.getInterpolation(e,this.a,this.b,this.c,t,i,s,r)}containsPoint(e){return n.containsPoint(e,this.a,this.b,this.c)}isFrontFacing(e){return n.isFrontFacing(this.a,this.b,this.c,e)}intersectsBox(e){return e.intersectsTriangle(this)}closestPointToPoint(e,t){let i=this.a,s=this.b,r=this.c,o,a;Ki.subVectors(s,i),ji.subVectors(r,i),sl.subVectors(e,i);let c=Ki.dot(sl),l=ji.dot(sl);if(c<=0&&l<=0)return t.copy(i);rl.subVectors(e,s);let h=Ki.dot(rl),u=ji.dot(rl);if(h>=0&&u<=h)return t.copy(s);let p=c*u-h*l;if(p<=0&&c>=0&&h<=0)return o=c/(c-h),t.copy(i).addScaledVector(Ki,o);al.subVectors(e,r);let d=Ki.dot(al),g=ji.dot(al);if(g>=0&&d<=g)return t.copy(r);let v=d*l-c*g;if(v<=0&&l>=0&&g<=0)return a=l/(l-g),t.copy(i).addScaledVector(ji,a);let m=h*g-d*u;if(m<=0&&u-h>=0&&d-g>=0)return Sh.subVectors(r,s),a=(u-h)/(u-h+(d-g)),t.copy(s).addScaledVector(Sh,a);let f=1/(m+v+p);return o=v*f,a=p*f,t.copy(i).addScaledVector(Ki,o).addScaledVector(ji,a)}equals(e){return e.a.equals(this.a)&&e.b.equals(this.b)&&e.c.equals(this.c)}},Su={aliceblue:15792383,antiquewhite:16444375,aqua:65535,aquamarine:8388564,azure:15794175,beige:16119260,bisque:16770244,black:0,blanchedalmond:16772045,blue:255,blueviolet:9055202,brown:10824234,burlywood:14596231,cadetblue:6266528,chartreuse:8388352,chocolate:13789470,coral:16744272,cornflowerblue:6591981,cornsilk:16775388,crimson:14423100,cyan:65535,darkblue:139,darkcyan:35723,darkgoldenrod:12092939,darkgray:11119017,darkgreen:25600,darkgrey:11119017,darkkhaki:12433259,darkmagenta:9109643,darkolivegreen:5597999,darkorange:16747520,darkorchid:10040012,darkred:9109504,darksalmon:15308410,darkseagreen:9419919,darkslateblue:4734347,darkslategray:3100495,darkslategrey:3100495,darkturquoise:52945,darkviolet:9699539,deeppink:16716947,deepskyblue:49151,dimgray:6908265,dimgrey:6908265,dodgerblue:2003199,firebrick:11674146,floralwhite:16775920,forestgreen:2263842,fuchsia:16711935,gainsboro:14474460,ghostwhite:16316671,gold:16766720,goldenrod:14329120,gray:8421504,green:32768,greenyellow:11403055,grey:8421504,honeydew:15794160,hotpink:16738740,indianred:13458524,indigo:4915330,ivory:16777200,khaki:15787660,lavender:15132410,lavenderblush:16773365,lawngreen:8190976,lemonchiffon:16775885,lightblue:11393254,lightcoral:15761536,lightcyan:14745599,lightgoldenrodyellow:16448210,lightgray:13882323,lightgreen:9498256,lightgrey:13882323,lightpink:16758465,lightsalmon:16752762,lightseagreen:2142890,lightskyblue:8900346,lightslategray:7833753,lightslategrey:7833753,lightsteelblue:11584734,lightyellow:16777184,lime:65280,limegreen:3329330,linen:16445670,magenta:16711935,maroon:8388608,mediumaquamarine:6737322,mediumblue:205,mediumorchid:12211667,mediumpurple:9662683,mediumseagreen:3978097,mediumslateblue:8087790,mediumspringgreen:64154,mediumturquoise:4772300,mediumvioletred:13047173,midnightblue:1644912,mintcream:16121850,mistyrose:16770273,moccasin:16770229,navajowhite:16768685,navy:128,oldlace:16643558,olive:8421376,olivedrab:7048739,orange:16753920,orangered:16729344,orchid:14315734,palegoldenrod:15657130,palegreen:10025880,paleturquoise:11529966,palevioletred:14381203,papayawhip:16773077,peachpuff:16767673,peru:13468991,pink:16761035,plum:14524637,powderblue:11591910,purple:8388736,rebeccapurple:6697881,red:16711680,rosybrown:12357519,royalblue:4286945,saddlebrown:9127187,salmon:16416882,sandybrown:16032864,seagreen:3050327,seashell:16774638,sienna:10506797,silver:12632256,skyblue:8900331,slateblue:6970061,slategray:7372944,slategrey:7372944,snow:16775930,springgreen:65407,steelblue:4620980,tan:13808780,teal:32896,thistle:14204888,tomato:16737095,turquoise:4251856,violet:15631086,wheat:16113331,white:16777215,whitesmoke:16119285,yellow:16776960,yellowgreen:10145074},ti={h:0,s:0,l:0},Fr={h:0,s:0,l:0};function hl(n,e,t){return t<0&&(t+=1),t>1&&(t-=1),t<1/6?n+(e-n)*6*t:t<1/2?e:t<2/3?n+(e-n)*6*(2/3-t):n}var Xe=class{constructor(e,t,i){return this.isColor=!0,this.r=1,this.g=1,this.b=1,this.set(e,t,i)}set(e,t,i){if(t===void 0&&i===void 0){let s=e;s&&s.isColor?this.copy(s):typeof s=="number"?this.setHex(s):typeof s=="string"&&this.setStyle(s)}else this.setRGB(e,t,i);return this}setScalar(e){return this.r=e,this.g=e,this.b=e,this}setHex(e,t=Ft){return e=Math.floor(e),this.r=(e>>16&255)/255,this.g=(e>>8&255)/255,this.b=(e&255)/255,Ye.colorSpaceToWorking(this,t),this}setRGB(e,t,i,s=Ye.workingColorSpace){return this.r=e,this.g=t,this.b=i,Ye.colorSpaceToWorking(this,s),this}setHSL(e,t,i,s=Ye.workingColorSpace){if(e=Hd(e,1),t=qe(t,0,1),i=qe(i,0,1),t===0)this.r=this.g=this.b=i;else{let r=i<=.5?i*(1+t):i+t-i*t,o=2*i-r;this.r=hl(o,r,e+1/3),this.g=hl(o,r,e),this.b=hl(o,r,e-1/3)}return Ye.colorSpaceToWorking(this,s),this}setStyle(e,t=Ft){function i(r){r!==void 0&&parseFloat(r)<1&&console.warn("THREE.Color: Alpha component of "+e+" will be ignored.")}let s;if(s=/^(\w+)\(([^\)]*)\)/.exec(e)){let r,o=s[1],a=s[2];switch(o){case"rgb":case"rgba":if(r=/^\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*(\d*\.?\d+)\s*)?$/.exec(a))return i(r[4]),this.setRGB(Math.min(255,parseInt(r[1],10))/255,Math.min(255,parseInt(r[2],10))/255,Math.min(255,parseInt(r[3],10))/255,t);if(r=/^\s*(\d+)\%\s*,\s*(\d+)\%\s*,\s*(\d+)\%\s*(?:,\s*(\d*\.?\d+)\s*)?$/.exec(a))return i(r[4]),this.setRGB(Math.min(100,parseInt(r[1],10))/100,Math.min(100,parseInt(r[2],10))/100,Math.min(100,parseInt(r[3],10))/100,t);break;case"hsl":case"hsla":if(r=/^\s*(\d*\.?\d+)\s*,\s*(\d*\.?\d+)\%\s*,\s*(\d*\.?\d+)\%\s*(?:,\s*(\d*\.?\d+)\s*)?$/.exec(a))return i(r[4]),this.setHSL(parseFloat(r[1])/360,parseFloat(r[2])/100,parseFloat(r[3])/100,t);break;default:console.warn("THREE.Color: Unknown color model "+e)}}else if(s=/^\#([A-Fa-f\d]+)$/.exec(e)){let r=s[1],o=r.length;if(o===3)return this.setRGB(parseInt(r.charAt(0),16)/15,parseInt(r.charAt(1),16)/15,parseInt(r.charAt(2),16)/15,t);if(o===6)return this.setHex(parseInt(r,16),t);console.warn("THREE.Color: Invalid hex color "+e)}else if(e&&e.length>0)return this.setColorName(e,t);return this}setColorName(e,t=Ft){let i=Su[e.toLowerCase()];return i!==void 0?this.setHex(i,t):console.warn("THREE.Color: Unknown color "+e),this}clone(){return new this.constructor(this.r,this.g,this.b)}copy(e){return this.r=e.r,this.g=e.g,this.b=e.b,this}copySRGBToLinear(e){return this.r=kn(e.r),this.g=kn(e.g),this.b=kn(e.b),this}copyLinearToSRGB(e){return this.r=as(e.r),this.g=as(e.g),this.b=as(e.b),this}convertSRGBToLinear(){return this.copySRGBToLinear(this),this}convertLinearToSRGB(){return this.copyLinearToSRGB(this),this}getHex(e=Ft){return Ye.workingToColorSpace(Pt.copy(this),e),Math.round(qe(Pt.r*255,0,255))*65536+Math.round(qe(Pt.g*255,0,255))*256+Math.round(qe(Pt.b*255,0,255))}getHexString(e=Ft){return("000000"+this.getHex(e).toString(16)).slice(-6)}getHSL(e,t=Ye.workingColorSpace){Ye.workingToColorSpace(Pt.copy(this),t);let i=Pt.r,s=Pt.g,r=Pt.b,o=Math.max(i,s,r),a=Math.min(i,s,r),c,l,h=(a+o)/2;if(a===o)c=0,l=0;else{let u=o-a;switch(l=h<=.5?u/(o+a):u/(2-o-a),o){case i:c=(s-r)/u+(s<r?6:0);break;case s:c=(r-i)/u+2;break;case r:c=(i-s)/u+4;break}c/=6}return e.h=c,e.s=l,e.l=h,e}getRGB(e,t=Ye.workingColorSpace){return Ye.workingToColorSpace(Pt.copy(this),t),e.r=Pt.r,e.g=Pt.g,e.b=Pt.b,e}getStyle(e=Ft){Ye.workingToColorSpace(Pt.copy(this),e);let t=Pt.r,i=Pt.g,s=Pt.b;return e!==Ft?`color(${e} ${t.toFixed(3)} ${i.toFixed(3)} ${s.toFixed(3)})`:`rgb(${Math.round(t*255)},${Math.round(i*255)},${Math.round(s*255)})`}offsetHSL(e,t,i){return this.getHSL(ti),this.setHSL(ti.h+e,ti.s+t,ti.l+i)}add(e){return this.r+=e.r,this.g+=e.g,this.b+=e.b,this}addColors(e,t){return this.r=e.r+t.r,this.g=e.g+t.g,this.b=e.b+t.b,this}addScalar(e){return this.r+=e,this.g+=e,this.b+=e,this}sub(e){return this.r=Math.max(0,this.r-e.r),this.g=Math.max(0,this.g-e.g),this.b=Math.max(0,this.b-e.b),this}multiply(e){return this.r*=e.r,this.g*=e.g,this.b*=e.b,this}multiplyScalar(e){return this.r*=e,this.g*=e,this.b*=e,this}lerp(e,t){return this.r+=(e.r-this.r)*t,this.g+=(e.g-this.g)*t,this.b+=(e.b-this.b)*t,this}lerpColors(e,t,i){return this.r=e.r+(t.r-e.r)*i,this.g=e.g+(t.g-e.g)*i,this.b=e.b+(t.b-e.b)*i,this}lerpHSL(e,t){this.getHSL(ti),e.getHSL(Fr);let i=qo(ti.h,Fr.h,t),s=qo(ti.s,Fr.s,t),r=qo(ti.l,Fr.l,t);return this.setHSL(i,s,r),this}setFromVector3(e){return this.r=e.x,this.g=e.y,this.b=e.z,this}applyMatrix3(e){let t=this.r,i=this.g,s=this.b,r=e.elements;return this.r=r[0]*t+r[3]*i+r[6]*s,this.g=r[1]*t+r[4]*i+r[7]*s,this.b=r[2]*t+r[5]*i+r[8]*s,this}equals(e){return e.r===this.r&&e.g===this.g&&e.b===this.b}fromArray(e,t=0){return this.r=e[t],this.g=e[t+1],this.b=e[t+2],this}toArray(e=[],t=0){return e[t]=this.r,e[t+1]=this.g,e[t+2]=this.b,e}fromBufferAttribute(e,t){return this.r=e.getX(t),this.g=e.getY(t),this.b=e.getZ(t),this}toJSON(){return this.getHex()}*[Symbol.iterator](){yield this.r,yield this.g,yield this.b}},Pt=new Xe;Xe.NAMES=Su;var ef=0,Gn=class extends zn{constructor(){super(),this.isMaterial=!0,Object.defineProperty(this,"id",{value:ef++}),this.uuid=si(),this.name="",this.type="Material",this.blending=Ei,this.side=Bn,this.vertexColors=!1,this.opacity=1,this.transparent=!1,this.alphaHash=!1,this.blendSrc=Kr,this.blendDst=jr,this.blendEquation=ri,this.blendSrcAlpha=null,this.blendDstAlpha=null,this.blendEquationAlpha=null,this.blendColor=new Xe(0,0,0),this.blendAlpha=0,this.depthFunc=Ti,this.depthTest=!0,this.depthWrite=!0,this.stencilWriteMask=255,this.stencilFunc=yl,this.stencilRef=0,this.stencilFuncMask=255,this.stencilFail=bi,this.stencilZFail=bi,this.stencilZPass=bi,this.stencilWrite=!1,this.clippingPlanes=null,this.clipIntersection=!1,this.clipShadows=!1,this.shadowSide=null,this.colorWrite=!0,this.precision=null,this.polygonOffset=!1,this.polygonOffsetFactor=0,this.polygonOffsetUnits=0,this.dithering=!1,this.alphaToCoverage=!1,this.premultipliedAlpha=!1,this.forceSinglePass=!1,this.allowOverride=!0,this.visible=!0,this.toneMapped=!0,this.userData={},this.version=0,this._alphaTest=0}get alphaTest(){return this._alphaTest}set alphaTest(e){this._alphaTest>0!=e>0&&this.version++,this._alphaTest=e}onBeforeRender(){}onBeforeCompile(){}customProgramCacheKey(){return this.onBeforeCompile.toString()}setValues(e){if(e!==void 0)for(let t in e){let i=e[t];if(i===void 0){console.warn(`THREE.Material: parameter '${t}' has value of undefined.`);continue}let s=this[t];if(s===void 0){console.warn(`THREE.Material: '${t}' is not a property of THREE.${this.type}.`);continue}s&&s.isColor?s.set(i):s&&s.isVector3&&i&&i.isVector3?s.copy(i):this[t]=i}}toJSON(e){let t=e===void 0||typeof e=="string";t&&(e={textures:{},images:{}});let i={metadata:{version:4.7,type:"Material",generator:"Material.toJSON"}};i.uuid=this.uuid,i.type=this.type,this.name!==""&&(i.name=this.name),this.color&&this.color.isColor&&(i.color=this.color.getHex()),this.roughness!==void 0&&(i.roughness=this.roughness),this.metalness!==void 0&&(i.metalness=this.metalness),this.sheen!==void 0&&(i.sheen=this.sheen),this.sheenColor&&this.sheenColor.isColor&&(i.sheenColor=this.sheenColor.getHex()),this.sheenRoughness!==void 0&&(i.sheenRoughness=this.sheenRoughness),this.emissive&&this.emissive.isColor&&(i.emissive=this.emissive.getHex()),this.emissiveIntensity!==void 0&&this.emissiveIntensity!==1&&(i.emissiveIntensity=this.emissiveIntensity),this.specular&&this.specular.isColor&&(i.specular=this.specular.getHex()),this.specularIntensity!==void 0&&(i.specularIntensity=this.specularIntensity),this.specularColor&&this.specularColor.isColor&&(i.specularColor=this.specularColor.getHex()),this.shininess!==void 0&&(i.shininess=this.shininess),this.clearcoat!==void 0&&(i.clearcoat=this.clearcoat),this.clearcoatRoughness!==void 0&&(i.clearcoatRoughness=this.clearcoatRoughness),this.clearcoatMap&&this.clearcoatMap.isTexture&&(i.clearcoatMap=this.clearcoatMap.toJSON(e).uuid),this.clearcoatRoughnessMap&&this.clearcoatRoughnessMap.isTexture&&(i.clearcoatRoughnessMap=this.clearcoatRoughnessMap.toJSON(e).uuid),this.clearcoatNormalMap&&this.clearcoatNormalMap.isTexture&&(i.clearcoatNormalMap=this.clearcoatNormalMap.toJSON(e).uuid,i.clearcoatNormalScale=this.clearcoatNormalScale.toArray()),this.sheenColorMap&&this.sheenColorMap.isTexture&&(i.sheenColorMap=this.sheenColorMap.toJSON(e).uuid),this.sheenRoughnessMap&&this.sheenRoughnessMap.isTexture&&(i.sheenRoughnessMap=this.sheenRoughnessMap.toJSON(e).uuid),this.dispersion!==void 0&&(i.dispersion=this.dispersion),this.iridescence!==void 0&&(i.iridescence=this.iridescence),this.iridescenceIOR!==void 0&&(i.iridescenceIOR=this.iridescenceIOR),this.iridescenceThicknessRange!==void 0&&(i.iridescenceThicknessRange=this.iridescenceThicknessRange),this.iridescenceMap&&this.iridescenceMap.isTexture&&(i.iridescenceMap=this.iridescenceMap.toJSON(e).uuid),this.iridescenceThicknessMap&&this.iridescenceThicknessMap.isTexture&&(i.iridescenceThicknessMap=this.iridescenceThicknessMap.toJSON(e).uuid),this.anisotropy!==void 0&&(i.anisotropy=this.anisotropy),this.anisotropyRotation!==void 0&&(i.anisotropyRotation=this.anisotropyRotation),this.anisotropyMap&&this.anisotropyMap.isTexture&&(i.anisotropyMap=this.anisotropyMap.toJSON(e).uuid),this.map&&this.map.isTexture&&(i.map=this.map.toJSON(e).uuid),this.matcap&&this.matcap.isTexture&&(i.matcap=this.matcap.toJSON(e).uuid),this.alphaMap&&this.alphaMap.isTexture&&(i.alphaMap=this.alphaMap.toJSON(e).uuid),this.lightMap&&this.lightMap.isTexture&&(i.lightMap=this.lightMap.toJSON(e).uuid,i.lightMapIntensity=this.lightMapIntensity),this.aoMap&&this.aoMap.isTexture&&(i.aoMap=this.aoMap.toJSON(e).uuid,i.aoMapIntensity=this.aoMapIntensity),this.bumpMap&&this.bumpMap.isTexture&&(i.bumpMap=this.bumpMap.toJSON(e).uuid,i.bumpScale=this.bumpScale),this.normalMap&&this.normalMap.isTexture&&(i.normalMap=this.normalMap.toJSON(e).uuid,i.normalMapType=this.normalMapType,i.normalScale=this.normalScale.toArray()),this.displacementMap&&this.displacementMap.isTexture&&(i.displacementMap=this.displacementMap.toJSON(e).uuid,i.displacementScale=this.displacementScale,i.displacementBias=this.displacementBias),this.roughnessMap&&this.roughnessMap.isTexture&&(i.roughnessMap=this.roughnessMap.toJSON(e).uuid),this.metalnessMap&&this.metalnessMap.isTexture&&(i.metalnessMap=this.metalnessMap.toJSON(e).uuid),this.emissiveMap&&this.emissiveMap.isTexture&&(i.emissiveMap=this.emissiveMap.toJSON(e).uuid),this.specularMap&&this.specularMap.isTexture&&(i.specularMap=this.specularMap.toJSON(e).uuid),this.specularIntensityMap&&this.specularIntensityMap.isTexture&&(i.specularIntensityMap=this.specularIntensityMap.toJSON(e).uuid),this.specularColorMap&&this.specularColorMap.isTexture&&(i.specularColorMap=this.specularColorMap.toJSON(e).uuid),this.envMap&&this.envMap.isTexture&&(i.envMap=this.envMap.toJSON(e).uuid,this.combine!==void 0&&(i.combine=this.combine)),this.envMapRotation!==void 0&&(i.envMapRotation=this.envMapRotation.toArray()),this.envMapIntensity!==void 0&&(i.envMapIntensity=this.envMapIntensity),this.reflectivity!==void 0&&(i.reflectivity=this.reflectivity),this.refractionRatio!==void 0&&(i.refractionRatio=this.refractionRatio),this.gradientMap&&this.gradientMap.isTexture&&(i.gradientMap=this.gradientMap.toJSON(e).uuid),this.transmission!==void 0&&(i.transmission=this.transmission),this.transmissionMap&&this.transmissionMap.isTexture&&(i.transmissionMap=this.transmissionMap.toJSON(e).uuid),this.thickness!==void 0&&(i.thickness=this.thickness),this.thicknessMap&&this.thicknessMap.isTexture&&(i.thicknessMap=this.thicknessMap.toJSON(e).uuid),this.attenuationDistance!==void 0&&this.attenuationDistance!==1/0&&(i.attenuationDistance=this.attenuationDistance),this.attenuationColor!==void 0&&(i.attenuationColor=this.attenuationColor.getHex()),this.size!==void 0&&(i.size=this.size),this.shadowSide!==null&&(i.shadowSide=this.shadowSide),this.sizeAttenuation!==void 0&&(i.sizeAttenuation=this.sizeAttenuation),this.blending!==Ei&&(i.blending=this.blending),this.side!==Bn&&(i.side=this.side),this.vertexColors===!0&&(i.vertexColors=!0),this.opacity<1&&(i.opacity=this.opacity),this.transparent===!0&&(i.transparent=!0),this.blendSrc!==Kr&&(i.blendSrc=this.blendSrc),this.blendDst!==jr&&(i.blendDst=this.blendDst),this.blendEquation!==ri&&(i.blendEquation=this.blendEquation),this.blendSrcAlpha!==null&&(i.blendSrcAlpha=this.blendSrcAlpha),this.blendDstAlpha!==null&&(i.blendDstAlpha=this.blendDstAlpha),this.blendEquationAlpha!==null&&(i.blendEquationAlpha=this.blendEquationAlpha),this.blendColor&&this.blendColor.isColor&&(i.blendColor=this.blendColor.getHex()),this.blendAlpha!==0&&(i.blendAlpha=this.blendAlpha),this.depthFunc!==Ti&&(i.depthFunc=this.depthFunc),this.depthTest===!1&&(i.depthTest=this.depthTest),this.depthWrite===!1&&(i.depthWrite=this.depthWrite),this.colorWrite===!1&&(i.colorWrite=this.colorWrite),this.stencilWriteMask!==255&&(i.stencilWriteMask=this.stencilWriteMask),this.stencilFunc!==yl&&(i.stencilFunc=this.stencilFunc),this.stencilRef!==0&&(i.stencilRef=this.stencilRef),this.stencilFuncMask!==255&&(i.stencilFuncMask=this.stencilFuncMask),this.stencilFail!==bi&&(i.stencilFail=this.stencilFail),this.stencilZFail!==bi&&(i.stencilZFail=this.stencilZFail),this.stencilZPass!==bi&&(i.stencilZPass=this.stencilZPass),this.stencilWrite===!0&&(i.stencilWrite=this.stencilWrite),this.rotation!==void 0&&this.rotation!==0&&(i.rotation=this.rotation),this.polygonOffset===!0&&(i.polygonOffset=!0),this.polygonOffsetFactor!==0&&(i.polygonOffsetFactor=this.polygonOffsetFactor),this.polygonOffsetUnits!==0&&(i.polygonOffsetUnits=this.polygonOffsetUnits),this.linewidth!==void 0&&this.linewidth!==1&&(i.linewidth=this.linewidth),this.dashSize!==void 0&&(i.dashSize=this.dashSize),this.gapSize!==void 0&&(i.gapSize=this.gapSize),this.scale!==void 0&&(i.scale=this.scale),this.dithering===!0&&(i.dithering=!0),this.alphaTest>0&&(i.alphaTest=this.alphaTest),this.alphaHash===!0&&(i.alphaHash=!0),this.alphaToCoverage===!0&&(i.alphaToCoverage=!0),this.premultipliedAlpha===!0&&(i.premultipliedAlpha=!0),this.forceSinglePass===!0&&(i.forceSinglePass=!0),this.wireframe===!0&&(i.wireframe=!0),this.wireframeLinewidth>1&&(i.wireframeLinewidth=this.wireframeLinewidth),this.wireframeLinecap!=="round"&&(i.wireframeLinecap=this.wireframeLinecap),this.wireframeLinejoin!=="round"&&(i.wireframeLinejoin=this.wireframeLinejoin),this.flatShading===!0&&(i.flatShading=!0),this.visible===!1&&(i.visible=!1),this.toneMapped===!1&&(i.toneMapped=!1),this.fog===!1&&(i.fog=!1),Object.keys(this.userData).length>0&&(i.userData=this.userData);function s(r){let o=[];for(let a in r){let c=r[a];delete c.metadata,o.push(c)}return o}if(t){let r=s(e.textures),o=s(e.images);r.length>0&&(i.textures=r),o.length>0&&(i.images=o)}return i}clone(){return new this.constructor().copy(this)}copy(e){this.name=e.name,this.blending=e.blending,this.side=e.side,this.vertexColors=e.vertexColors,this.opacity=e.opacity,this.transparent=e.transparent,this.blendSrc=e.blendSrc,this.blendDst=e.blendDst,this.blendEquation=e.blendEquation,this.blendSrcAlpha=e.blendSrcAlpha,this.blendDstAlpha=e.blendDstAlpha,this.blendEquationAlpha=e.blendEquationAlpha,this.blendColor.copy(e.blendColor),this.blendAlpha=e.blendAlpha,this.depthFunc=e.depthFunc,this.depthTest=e.depthTest,this.depthWrite=e.depthWrite,this.stencilWriteMask=e.stencilWriteMask,this.stencilFunc=e.stencilFunc,this.stencilRef=e.stencilRef,this.stencilFuncMask=e.stencilFuncMask,this.stencilFail=e.stencilFail,this.stencilZFail=e.stencilZFail,this.stencilZPass=e.stencilZPass,this.stencilWrite=e.stencilWrite;let t=e.clippingPlanes,i=null;if(t!==null){let s=t.length;i=new Array(s);for(let r=0;r!==s;++r)i[r]=t[r].clone()}return this.clippingPlanes=i,this.clipIntersection=e.clipIntersection,this.clipShadows=e.clipShadows,this.shadowSide=e.shadowSide,this.colorWrite=e.colorWrite,this.precision=e.precision,this.polygonOffset=e.polygonOffset,this.polygonOffsetFactor=e.polygonOffsetFactor,this.polygonOffsetUnits=e.polygonOffsetUnits,this.dithering=e.dithering,this.alphaTest=e.alphaTest,this.alphaHash=e.alphaHash,this.alphaToCoverage=e.alphaToCoverage,this.premultipliedAlpha=e.premultipliedAlpha,this.forceSinglePass=e.forceSinglePass,this.visible=e.visible,this.toneMapped=e.toneMapped,this.userData=JSON.parse(JSON.stringify(e.userData)),this}dispose(){this.dispatchEvent({type:"dispose"})}set needsUpdate(e){e===!0&&this.version++}},bn=class extends Gn{constructor(e){super(),this.isMeshBasicMaterial=!0,this.type="MeshBasicMaterial",this.color=new Xe(16777215),this.map=null,this.lightMap=null,this.lightMapIntensity=1,this.aoMap=null,this.aoMapIntensity=1,this.specularMap=null,this.alphaMap=null,this.envMap=null,this.envMapRotation=new fn,this.combine=Il,this.reflectivity=1,this.refractionRatio=.98,this.wireframe=!1,this.wireframeLinewidth=1,this.wireframeLinecap="round",this.wireframeLinejoin="round",this.fog=!0,this.setValues(e)}copy(e){return super.copy(e),this.color.copy(e.color),this.map=e.map,this.lightMap=e.lightMap,this.lightMapIntensity=e.lightMapIntensity,this.aoMap=e.aoMap,this.aoMapIntensity=e.aoMapIntensity,this.specularMap=e.specularMap,this.alphaMap=e.alphaMap,this.envMap=e.envMap,this.envMapRotation.copy(e.envMapRotation),this.combine=e.combine,this.reflectivity=e.reflectivity,this.refractionRatio=e.refractionRatio,this.wireframe=e.wireframe,this.wireframeLinewidth=e.wireframeLinewidth,this.wireframeLinecap=e.wireframeLinecap,this.wireframeLinejoin=e.wireframeLinejoin,this.fog=e.fog,this}};var vt=new L,Or=new Ue,tf=0,Ht=class{constructor(e,t,i=!1){if(Array.isArray(e))throw new TypeError("THREE.BufferAttribute: array should be a Typed Array.");this.isBufferAttribute=!0,Object.defineProperty(this,"id",{value:tf++}),this.name="",this.array=e,this.itemSize=t,this.count=e!==void 0?e.length/t:0,this.normalized=i,this.usage=na,this.updateRanges=[],this.gpuType=Tn,this.version=0}onUploadCallback(){}set needsUpdate(e){e===!0&&this.version++}setUsage(e){return this.usage=e,this}addUpdateRange(e,t){this.updateRanges.push({start:e,count:t})}clearUpdateRanges(){this.updateRanges.length=0}copy(e){return this.name=e.name,this.array=new e.array.constructor(e.array),this.itemSize=e.itemSize,this.count=e.count,this.normalized=e.normalized,this.usage=e.usage,this.gpuType=e.gpuType,this}copyAt(e,t,i){e*=this.itemSize,i*=t.itemSize;for(let s=0,r=this.itemSize;s<r;s++)this.array[e+s]=t.array[i+s];return this}copyArray(e){return this.array.set(e),this}applyMatrix3(e){if(this.itemSize===2)for(let t=0,i=this.count;t<i;t++)Or.fromBufferAttribute(this,t),Or.applyMatrix3(e),this.setXY(t,Or.x,Or.y);else if(this.itemSize===3)for(let t=0,i=this.count;t<i;t++)vt.fromBufferAttribute(this,t),vt.applyMatrix3(e),this.setXYZ(t,vt.x,vt.y,vt.z);return this}applyMatrix4(e){for(let t=0,i=this.count;t<i;t++)vt.fromBufferAttribute(this,t),vt.applyMatrix4(e),this.setXYZ(t,vt.x,vt.y,vt.z);return this}applyNormalMatrix(e){for(let t=0,i=this.count;t<i;t++)vt.fromBufferAttribute(this,t),vt.applyNormalMatrix(e),this.setXYZ(t,vt.x,vt.y,vt.z);return this}transformDirection(e){for(let t=0,i=this.count;t<i;t++)vt.fromBufferAttribute(this,t),vt.transformDirection(e),this.setXYZ(t,vt.x,vt.y,vt.z);return this}set(e,t=0){return this.array.set(e,t),this}getComponent(e,t){let i=this.array[e*this.itemSize+t];return this.normalized&&(i=Mn(i,this.array)),i}setComponent(e,t,i){return this.normalized&&(i=nt(i,this.array)),this.array[e*this.itemSize+t]=i,this}getX(e){let t=this.array[e*this.itemSize];return this.normalized&&(t=Mn(t,this.array)),t}setX(e,t){return this.normalized&&(t=nt(t,this.array)),this.array[e*this.itemSize]=t,this}getY(e){let t=this.array[e*this.itemSize+1];return this.normalized&&(t=Mn(t,this.array)),t}setY(e,t){return this.normalized&&(t=nt(t,this.array)),this.array[e*this.itemSize+1]=t,this}getZ(e){let t=this.array[e*this.itemSize+2];return this.normalized&&(t=Mn(t,this.array)),t}setZ(e,t){return this.normalized&&(t=nt(t,this.array)),this.array[e*this.itemSize+2]=t,this}getW(e){let t=this.array[e*this.itemSize+3];return this.normalized&&(t=Mn(t,this.array)),t}setW(e,t){return this.normalized&&(t=nt(t,this.array)),this.array[e*this.itemSize+3]=t,this}setXY(e,t,i){return e*=this.itemSize,this.normalized&&(t=nt(t,this.array),i=nt(i,this.array)),this.array[e+0]=t,this.array[e+1]=i,this}setXYZ(e,t,i,s){return e*=this.itemSize,this.normalized&&(t=nt(t,this.array),i=nt(i,this.array),s=nt(s,this.array)),this.array[e+0]=t,this.array[e+1]=i,this.array[e+2]=s,this}setXYZW(e,t,i,s,r){return e*=this.itemSize,this.normalized&&(t=nt(t,this.array),i=nt(i,this.array),s=nt(s,this.array),r=nt(r,this.array)),this.array[e+0]=t,this.array[e+1]=i,this.array[e+2]=s,this.array[e+3]=r,this}onUpload(e){return this.onUploadCallback=e,this}clone(){return new this.constructor(this.array,this.itemSize).copy(this)}toJSON(){let e={itemSize:this.itemSize,type:this.array.constructor.name,array:Array.from(this.array),normalized:this.normalized};return this.name!==""&&(e.name=this.name),this.usage!==na&&(e.usage=this.usage),e}};var Ws=class extends Ht{constructor(e,t,i){super(new Uint16Array(e),t,i)}};var Xs=class extends Ht{constructor(e,t,i){super(new Uint32Array(e),t,i)}};var lt=class extends Ht{constructor(e,t,i){super(new Float32Array(e),t,i)}},nf=0,sn=new dt,ul=new wt,Qi=new L,jt=new ai,Ds=new ai,Tt=new L,Xt=class n extends zn{constructor(){super(),this.isBufferGeometry=!0,Object.defineProperty(this,"id",{value:nf++}),this.uuid=si(),this.name="",this.type="BufferGeometry",this.index=null,this.indirect=null,this.attributes={},this.morphAttributes={},this.morphTargetsRelative=!1,this.groups=[],this.boundingBox=null,this.boundingSphere=null,this.drawRange={start:0,count:1/0},this.userData={}}getIndex(){return this.index}setIndex(e){return Array.isArray(e)?this.index=new(Hl(e)?Xs:Ws)(e,1):this.index=e,this}setIndirect(e){return this.indirect=e,this}getIndirect(){return this.indirect}getAttribute(e){return this.attributes[e]}setAttribute(e,t){return this.attributes[e]=t,this}deleteAttribute(e){return delete this.attributes[e],this}hasAttribute(e){return this.attributes[e]!==void 0}addGroup(e,t,i=0){this.groups.push({start:e,count:t,materialIndex:i})}clearGroups(){this.groups=[]}setDrawRange(e,t){this.drawRange.start=e,this.drawRange.count=t}applyMatrix4(e){let t=this.attributes.position;t!==void 0&&(t.applyMatrix4(e),t.needsUpdate=!0);let i=this.attributes.normal;if(i!==void 0){let r=new Oe().getNormalMatrix(e);i.applyNormalMatrix(r),i.needsUpdate=!0}let s=this.attributes.tangent;return s!==void 0&&(s.transformDirection(e),s.needsUpdate=!0),this.boundingBox!==null&&this.computeBoundingBox(),this.boundingSphere!==null&&this.computeBoundingSphere(),this}applyQuaternion(e){return sn.makeRotationFromQuaternion(e),this.applyMatrix4(sn),this}rotateX(e){return sn.makeRotationX(e),this.applyMatrix4(sn),this}rotateY(e){return sn.makeRotationY(e),this.applyMatrix4(sn),this}rotateZ(e){return sn.makeRotationZ(e),this.applyMatrix4(sn),this}translate(e,t,i){return sn.makeTranslation(e,t,i),this.applyMatrix4(sn),this}scale(e,t,i){return sn.makeScale(e,t,i),this.applyMatrix4(sn),this}lookAt(e){return ul.lookAt(e),ul.updateMatrix(),this.applyMatrix4(ul.matrix),this}center(){return this.computeBoundingBox(),this.boundingBox.getCenter(Qi).negate(),this.translate(Qi.x,Qi.y,Qi.z),this}setFromPoints(e){let t=this.getAttribute("position");if(t===void 0){let i=[];for(let s=0,r=e.length;s<r;s++){let o=e[s];i.push(o.x,o.y,o.z||0)}this.setAttribute("position",new lt(i,3))}else{let i=Math.min(e.length,t.count);for(let s=0;s<i;s++){let r=e[s];t.setXYZ(s,r.x,r.y,r.z||0)}e.length>t.count&&console.warn("THREE.BufferGeometry: Buffer size too small for points data. Use .dispose() and create a new geometry."),t.needsUpdate=!0}return this}computeBoundingBox(){this.boundingBox===null&&(this.boundingBox=new ai);let e=this.attributes.position,t=this.morphAttributes.position;if(e&&e.isGLBufferAttribute){console.error("THREE.BufferGeometry.computeBoundingBox(): GLBufferAttribute requires a manual bounding box.",this),this.boundingBox.set(new L(-1/0,-1/0,-1/0),new L(1/0,1/0,1/0));return}if(e!==void 0){if(this.boundingBox.setFromBufferAttribute(e),t)for(let i=0,s=t.length;i<s;i++){let r=t[i];jt.setFromBufferAttribute(r),this.morphTargetsRelative?(Tt.addVectors(this.boundingBox.min,jt.min),this.boundingBox.expandByPoint(Tt),Tt.addVectors(this.boundingBox.max,jt.max),this.boundingBox.expandByPoint(Tt)):(this.boundingBox.expandByPoint(jt.min),this.boundingBox.expandByPoint(jt.max))}}else this.boundingBox.makeEmpty();(isNaN(this.boundingBox.min.x)||isNaN(this.boundingBox.min.y)||isNaN(this.boundingBox.min.z))&&console.error('THREE.BufferGeometry.computeBoundingBox(): Computed min/max have NaN values. The "position" attribute is likely to have NaN values.',this)}computeBoundingSphere(){this.boundingSphere===null&&(this.boundingSphere=new hs);let e=this.attributes.position,t=this.morphAttributes.position;if(e&&e.isGLBufferAttribute){console.error("THREE.BufferGeometry.computeBoundingSphere(): GLBufferAttribute requires a manual bounding sphere.",this),this.boundingSphere.set(new L,1/0);return}if(e){let i=this.boundingSphere.center;if(jt.setFromBufferAttribute(e),t)for(let r=0,o=t.length;r<o;r++){let a=t[r];Ds.setFromBufferAttribute(a),this.morphTargetsRelative?(Tt.addVectors(jt.min,Ds.min),jt.expandByPoint(Tt),Tt.addVectors(jt.max,Ds.max),jt.expandByPoint(Tt)):(jt.expandByPoint(Ds.min),jt.expandByPoint(Ds.max))}jt.getCenter(i);let s=0;for(let r=0,o=e.count;r<o;r++)Tt.fromBufferAttribute(e,r),s=Math.max(s,i.distanceToSquared(Tt));if(t)for(let r=0,o=t.length;r<o;r++){let a=t[r],c=this.morphTargetsRelative;for(let l=0,h=a.count;l<h;l++)Tt.fromBufferAttribute(a,l),c&&(Qi.fromBufferAttribute(e,l),Tt.add(Qi)),s=Math.max(s,i.distanceToSquared(Tt))}this.boundingSphere.radius=Math.sqrt(s),isNaN(this.boundingSphere.radius)&&console.error('THREE.BufferGeometry.computeBoundingSphere(): Computed radius is NaN. The "position" attribute is likely to have NaN values.',this)}}computeTangents(){let e=this.index,t=this.attributes;if(e===null||t.position===void 0||t.normal===void 0||t.uv===void 0){console.error("THREE.BufferGeometry: .computeTangents() failed. Missing required attributes (index, position, normal or uv)");return}let i=t.position,s=t.normal,r=t.uv;this.hasAttribute("tangent")===!1&&this.setAttribute("tangent",new Ht(new Float32Array(4*i.count),4));let o=this.getAttribute("tangent"),a=[],c=[];for(let N=0;N<i.count;N++)a[N]=new L,c[N]=new L;let l=new L,h=new L,u=new L,p=new Ue,d=new Ue,g=new Ue,v=new L,m=new L;function f(N,M,y){l.fromBufferAttribute(i,N),h.fromBufferAttribute(i,M),u.fromBufferAttribute(i,y),p.fromBufferAttribute(r,N),d.fromBufferAttribute(r,M),g.fromBufferAttribute(r,y),h.sub(l),u.sub(l),d.sub(p),g.sub(p);let I=1/(d.x*g.y-g.x*d.y);isFinite(I)&&(v.copy(h).multiplyScalar(g.y).addScaledVector(u,-d.y).multiplyScalar(I),m.copy(u).multiplyScalar(d.x).addScaledVector(h,-g.x).multiplyScalar(I),a[N].add(v),a[M].add(v),a[y].add(v),c[N].add(m),c[M].add(m),c[y].add(m))}let T=this.groups;T.length===0&&(T=[{start:0,count:e.count}]);for(let N=0,M=T.length;N<M;++N){let y=T[N],I=y.start,O=y.count;for(let z=I,H=I+O;z<H;z+=3)f(e.getX(z+0),e.getX(z+1),e.getX(z+2))}let E=new L,S=new L,R=new L,A=new L;function P(N){R.fromBufferAttribute(s,N),A.copy(R);let M=a[N];E.copy(M),E.sub(R.multiplyScalar(R.dot(M))).normalize(),S.crossVectors(A,M);let I=S.dot(c[N])<0?-1:1;o.setXYZW(N,E.x,E.y,E.z,I)}for(let N=0,M=T.length;N<M;++N){let y=T[N],I=y.start,O=y.count;for(let z=I,H=I+O;z<H;z+=3)P(e.getX(z+0)),P(e.getX(z+1)),P(e.getX(z+2))}}computeVertexNormals(){let e=this.index,t=this.getAttribute("position");if(t!==void 0){let i=this.getAttribute("normal");if(i===void 0)i=new Ht(new Float32Array(t.count*3),3),this.setAttribute("normal",i);else for(let p=0,d=i.count;p<d;p++)i.setXYZ(p,0,0,0);let s=new L,r=new L,o=new L,a=new L,c=new L,l=new L,h=new L,u=new L;if(e)for(let p=0,d=e.count;p<d;p+=3){let g=e.getX(p+0),v=e.getX(p+1),m=e.getX(p+2);s.fromBufferAttribute(t,g),r.fromBufferAttribute(t,v),o.fromBufferAttribute(t,m),h.subVectors(o,r),u.subVectors(s,r),h.cross(u),a.fromBufferAttribute(i,g),c.fromBufferAttribute(i,v),l.fromBufferAttribute(i,m),a.add(h),c.add(h),l.add(h),i.setXYZ(g,a.x,a.y,a.z),i.setXYZ(v,c.x,c.y,c.z),i.setXYZ(m,l.x,l.y,l.z)}else for(let p=0,d=t.count;p<d;p+=3)s.fromBufferAttribute(t,p+0),r.fromBufferAttribute(t,p+1),o.fromBufferAttribute(t,p+2),h.subVectors(o,r),u.subVectors(s,r),h.cross(u),i.setXYZ(p+0,h.x,h.y,h.z),i.setXYZ(p+1,h.x,h.y,h.z),i.setXYZ(p+2,h.x,h.y,h.z);this.normalizeNormals(),i.needsUpdate=!0}}normalizeNormals(){let e=this.attributes.normal;for(let t=0,i=e.count;t<i;t++)Tt.fromBufferAttribute(e,t),Tt.normalize(),e.setXYZ(t,Tt.x,Tt.y,Tt.z)}toNonIndexed(){function e(a,c){let l=a.array,h=a.itemSize,u=a.normalized,p=new l.constructor(c.length*h),d=0,g=0;for(let v=0,m=c.length;v<m;v++){a.isInterleavedBufferAttribute?d=c[v]*a.data.stride+a.offset:d=c[v]*h;for(let f=0;f<h;f++)p[g++]=l[d++]}return new Ht(p,h,u)}if(this.index===null)return console.warn("THREE.BufferGeometry.toNonIndexed(): BufferGeometry is already non-indexed."),this;let t=new n,i=this.index.array,s=this.attributes;for(let a in s){let c=s[a],l=e(c,i);t.setAttribute(a,l)}let r=this.morphAttributes;for(let a in r){let c=[],l=r[a];for(let h=0,u=l.length;h<u;h++){let p=l[h],d=e(p,i);c.push(d)}t.morphAttributes[a]=c}t.morphTargetsRelative=this.morphTargetsRelative;let o=this.groups;for(let a=0,c=o.length;a<c;a++){let l=o[a];t.addGroup(l.start,l.count,l.materialIndex)}return t}toJSON(){let e={metadata:{version:4.7,type:"BufferGeometry",generator:"BufferGeometry.toJSON"}};if(e.uuid=this.uuid,e.type=this.type,this.name!==""&&(e.name=this.name),Object.keys(this.userData).length>0&&(e.userData=this.userData),this.parameters!==void 0){let c=this.parameters;for(let l in c)c[l]!==void 0&&(e[l]=c[l]);return e}e.data={attributes:{}};let t=this.index;t!==null&&(e.data.index={type:t.array.constructor.name,array:Array.prototype.slice.call(t.array)});let i=this.attributes;for(let c in i){let l=i[c];e.data.attributes[c]=l.toJSON(e.data)}let s={},r=!1;for(let c in this.morphAttributes){let l=this.morphAttributes[c],h=[];for(let u=0,p=l.length;u<p;u++){let d=l[u];h.push(d.toJSON(e.data))}h.length>0&&(s[c]=h,r=!0)}r&&(e.data.morphAttributes=s,e.data.morphTargetsRelative=this.morphTargetsRelative);let o=this.groups;o.length>0&&(e.data.groups=JSON.parse(JSON.stringify(o)));let a=this.boundingSphere;return a!==null&&(e.data.boundingSphere=a.toJSON()),e}clone(){return new this.constructor().copy(this)}copy(e){this.index=null,this.attributes={},this.morphAttributes={},this.groups=[],this.boundingBox=null,this.boundingSphere=null;let t={};this.name=e.name;let i=e.index;i!==null&&this.setIndex(i.clone());let s=e.attributes;for(let l in s){let h=s[l];this.setAttribute(l,h.clone(t))}let r=e.morphAttributes;for(let l in r){let h=[],u=r[l];for(let p=0,d=u.length;p<d;p++)h.push(u[p].clone(t));this.morphAttributes[l]=h}this.morphTargetsRelative=e.morphTargetsRelative;let o=e.groups;for(let l=0,h=o.length;l<h;l++){let u=o[l];this.addGroup(u.start,u.count,u.materialIndex)}let a=e.boundingBox;a!==null&&(this.boundingBox=a.clone());let c=e.boundingSphere;return c!==null&&(this.boundingSphere=c.clone()),this.drawRange.start=e.drawRange.start,this.drawRange.count=e.drawRange.count,this.userData=e.userData,this}dispose(){this.dispatchEvent({type:"dispose"})}},bh=new dt,Mi=new Hs,kr=new hs,Eh=new L,Br=new L,zr=new L,Vr=new L,dl=new L,Gr=new L,Th=new L,Hr=new L,ft=class extends wt{constructor(e=new Xt,t=new bn){super(),this.isMesh=!0,this.type="Mesh",this.geometry=e,this.material=t,this.morphTargetDictionary=void 0,this.morphTargetInfluences=void 0,this.count=1,this.updateMorphTargets()}copy(e,t){return super.copy(e,t),e.morphTargetInfluences!==void 0&&(this.morphTargetInfluences=e.morphTargetInfluences.slice()),e.morphTargetDictionary!==void 0&&(this.morphTargetDictionary=Object.assign({},e.morphTargetDictionary)),this.material=Array.isArray(e.material)?e.material.slice():e.material,this.geometry=e.geometry,this}updateMorphTargets(){let t=this.geometry.morphAttributes,i=Object.keys(t);if(i.length>0){let s=t[i[0]];if(s!==void 0){this.morphTargetInfluences=[],this.morphTargetDictionary={};for(let r=0,o=s.length;r<o;r++){let a=s[r].name||String(r);this.morphTargetInfluences.push(0),this.morphTargetDictionary[a]=r}}}}getVertexPosition(e,t){let i=this.geometry,s=i.attributes.position,r=i.morphAttributes.position,o=i.morphTargetsRelative;t.fromBufferAttribute(s,e);let a=this.morphTargetInfluences;if(r&&a){Gr.set(0,0,0);for(let c=0,l=r.length;c<l;c++){let h=a[c],u=r[c];h!==0&&(dl.fromBufferAttribute(u,e),o?Gr.addScaledVector(dl,h):Gr.addScaledVector(dl.sub(t),h))}t.add(Gr)}return t}raycast(e,t){let i=this.geometry,s=this.material,r=this.matrixWorld;s!==void 0&&(i.boundingSphere===null&&i.computeBoundingSphere(),kr.copy(i.boundingSphere),kr.applyMatrix4(r),Mi.copy(e.ray).recast(e.near),!(kr.containsPoint(Mi.origin)===!1&&(Mi.intersectSphere(kr,Eh)===null||Mi.origin.distanceToSquared(Eh)>(e.far-e.near)**2))&&(bh.copy(r).invert(),Mi.copy(e.ray).applyMatrix4(bh),!(i.boundingBox!==null&&Mi.intersectsBox(i.boundingBox)===!1)&&this._computeIntersections(e,t,Mi)))}_computeIntersections(e,t,i){let s,r=this.geometry,o=this.material,a=r.index,c=r.attributes.position,l=r.attributes.uv,h=r.attributes.uv1,u=r.attributes.normal,p=r.groups,d=r.drawRange;if(a!==null)if(Array.isArray(o))for(let g=0,v=p.length;g<v;g++){let m=p[g],f=o[m.materialIndex],T=Math.max(m.start,d.start),E=Math.min(a.count,Math.min(m.start+m.count,d.start+d.count));for(let S=T,R=E;S<R;S+=3){let A=a.getX(S),P=a.getX(S+1),N=a.getX(S+2);s=Wr(this,f,e,i,l,h,u,A,P,N),s&&(s.faceIndex=Math.floor(S/3),s.face.materialIndex=m.materialIndex,t.push(s))}}else{let g=Math.max(0,d.start),v=Math.min(a.count,d.start+d.count);for(let m=g,f=v;m<f;m+=3){let T=a.getX(m),E=a.getX(m+1),S=a.getX(m+2);s=Wr(this,o,e,i,l,h,u,T,E,S),s&&(s.faceIndex=Math.floor(m/3),t.push(s))}}else if(c!==void 0)if(Array.isArray(o))for(let g=0,v=p.length;g<v;g++){let m=p[g],f=o[m.materialIndex],T=Math.max(m.start,d.start),E=Math.min(c.count,Math.min(m.start+m.count,d.start+d.count));for(let S=T,R=E;S<R;S+=3){let A=S,P=S+1,N=S+2;s=Wr(this,f,e,i,l,h,u,A,P,N),s&&(s.faceIndex=Math.floor(S/3),s.face.materialIndex=m.materialIndex,t.push(s))}}else{let g=Math.max(0,d.start),v=Math.min(c.count,d.start+d.count);for(let m=g,f=v;m<f;m+=3){let T=m,E=m+1,S=m+2;s=Wr(this,o,e,i,l,h,u,T,E,S),s&&(s.faceIndex=Math.floor(m/3),t.push(s))}}}};function sf(n,e,t,i,s,r,o,a){let c;if(e.side===Bt?c=i.intersectTriangle(o,r,s,!0,a):c=i.intersectTriangle(s,r,o,e.side===Bn,a),c===null)return null;Hr.copy(a),Hr.applyMatrix4(n.matrixWorld);let l=t.ray.origin.distanceTo(Hr);return l<t.near||l>t.far?null:{distance:l,point:Hr.clone(),object:n}}function Wr(n,e,t,i,s,r,o,a,c,l){n.getVertexPosition(a,Br),n.getVertexPosition(c,zr),n.getVertexPosition(l,Vr);let h=sf(n,e,t,i,Br,zr,Vr,Th);if(h){let u=new L;On.getBarycoord(Th,Br,zr,Vr,u),s&&(h.uv=On.getInterpolatedAttribute(s,a,c,l,u,new Ue)),r&&(h.uv1=On.getInterpolatedAttribute(r,a,c,l,u,new Ue)),o&&(h.normal=On.getInterpolatedAttribute(o,a,c,l,u,new L),h.normal.dot(i.direction)>0&&h.normal.multiplyScalar(-1));let p={a,b:c,c:l,normal:new L,materialIndex:0};On.getNormal(Br,zr,Vr,p.normal),h.face=p,h.barycoord=u}return h}var oi=class n extends Xt{constructor(e=1,t=1,i=1,s=1,r=1,o=1){super(),this.type="BoxGeometry",this.parameters={width:e,height:t,depth:i,widthSegments:s,heightSegments:r,depthSegments:o};let a=this;s=Math.floor(s),r=Math.floor(r),o=Math.floor(o);let c=[],l=[],h=[],u=[],p=0,d=0;g("z","y","x",-1,-1,i,t,e,o,r,0),g("z","y","x",1,-1,i,t,-e,o,r,1),g("x","z","y",1,1,e,i,t,s,o,2),g("x","z","y",1,-1,e,i,-t,s,o,3),g("x","y","z",1,-1,e,t,i,s,r,4),g("x","y","z",-1,-1,e,t,-i,s,r,5),this.setIndex(c),this.setAttribute("position",new lt(l,3)),this.setAttribute("normal",new lt(h,3)),this.setAttribute("uv",new lt(u,2));function g(v,m,f,T,E,S,R,A,P,N,M){let y=S/P,I=R/N,O=S/2,z=R/2,H=A/2,q=P+1,W=N+1,ne=0,V=0,ae=new L;for(let ue=0;ue<W;ue++){let Ee=ue*I-z;for(let Ve=0;Ve<q;Ve++){let st=Ve*y-O;ae[v]=st*T,ae[m]=Ee*E,ae[f]=H,l.push(ae.x,ae.y,ae.z),ae[v]=0,ae[m]=0,ae[f]=A>0?1:-1,h.push(ae.x,ae.y,ae.z),u.push(Ve/P),u.push(1-ue/N),ne+=1}}for(let ue=0;ue<N;ue++)for(let Ee=0;Ee<P;Ee++){let Ve=p+Ee+q*ue,st=p+Ee+q*(ue+1),ct=p+(Ee+1)+q*(ue+1),Je=p+(Ee+1)+q*ue;c.push(Ve,st,Je),c.push(st,ct,Je),V+=6}a.addGroup(d,V,M),d+=V,p+=ne}}copy(e){return super.copy(e),this.parameters=Object.assign({},e.parameters),this}static fromJSON(e){return new n(e.width,e.height,e.depth,e.widthSegments,e.heightSegments,e.depthSegments)}};function Di(n){let e={};for(let t in n){e[t]={};for(let i in n[t]){let s=n[t][i];s&&(s.isColor||s.isMatrix3||s.isMatrix4||s.isVector2||s.isVector3||s.isVector4||s.isTexture||s.isQuaternion)?s.isRenderTargetTexture?(console.warn("UniformsUtils: Textures of render targets cannot be cloned via cloneUniforms() or mergeUniforms()."),e[t][i]=null):e[t][i]=s.clone():Array.isArray(s)?e[t][i]=s.slice():e[t][i]=s}}return e}function Lt(n){let e={};for(let t=0;t<n.length;t++){let i=Di(n[t]);for(let s in i)e[s]=i[s]}return e}function rf(n){let e=[];for(let t=0;t<n.length;t++)e.push(n[t].clone());return e}function Wl(n){let e=n.getRenderTarget();return e===null?n.outputColorSpace:e.isXRRenderTarget===!0?e.texture.colorSpace:Ye.workingColorSpace}var bu={clone:Di,merge:Lt},af=`void main() {
	gl_Position = projectionMatrix * modelViewMatrix * vec4( position, 1.0 );
}`,of=`void main() {
	gl_FragColor = vec4( 1.0, 0.0, 0.0, 1.0 );
}`,pn=class extends Gn{constructor(e){super(),this.isShaderMaterial=!0,this.type="ShaderMaterial",this.defines={},this.uniforms={},this.uniformsGroups=[],this.vertexShader=af,this.fragmentShader=of,this.linewidth=1,this.wireframe=!1,this.wireframeLinewidth=1,this.fog=!1,this.lights=!1,this.clipping=!1,this.forceSinglePass=!0,this.extensions={clipCullDistance:!1,multiDraw:!1},this.defaultAttributeValues={color:[1,1,1],uv:[0,0],uv1:[0,0]},this.index0AttributeName=void 0,this.uniformsNeedUpdate=!1,this.glslVersion=null,e!==void 0&&this.setValues(e)}copy(e){return super.copy(e),this.fragmentShader=e.fragmentShader,this.vertexShader=e.vertexShader,this.uniforms=Di(e.uniforms),this.uniformsGroups=rf(e.uniformsGroups),this.defines=Object.assign({},e.defines),this.wireframe=e.wireframe,this.wireframeLinewidth=e.wireframeLinewidth,this.fog=e.fog,this.lights=e.lights,this.clipping=e.clipping,this.extensions=Object.assign({},e.extensions),this.glslVersion=e.glslVersion,this}toJSON(e){let t=super.toJSON(e);t.glslVersion=this.glslVersion,t.uniforms={};for(let s in this.uniforms){let o=this.uniforms[s].value;o&&o.isTexture?t.uniforms[s]={type:"t",value:o.toJSON(e).uuid}:o&&o.isColor?t.uniforms[s]={type:"c",value:o.getHex()}:o&&o.isVector2?t.uniforms[s]={type:"v2",value:o.toArray()}:o&&o.isVector3?t.uniforms[s]={type:"v3",value:o.toArray()}:o&&o.isVector4?t.uniforms[s]={type:"v4",value:o.toArray()}:o&&o.isMatrix3?t.uniforms[s]={type:"m3",value:o.toArray()}:o&&o.isMatrix4?t.uniforms[s]={type:"m4",value:o.toArray()}:t.uniforms[s]={value:o}}Object.keys(this.defines).length>0&&(t.defines=this.defines),t.vertexShader=this.vertexShader,t.fragmentShader=this.fragmentShader,t.lights=this.lights,t.clipping=this.clipping;let i={};for(let s in this.extensions)this.extensions[s]===!0&&(i[s]=!0);return Object.keys(i).length>0&&(t.extensions=i),t}},qs=class extends wt{constructor(){super(),this.isCamera=!0,this.type="Camera",this.matrixWorldInverse=new dt,this.projectionMatrix=new dt,this.projectionMatrixInverse=new dt,this.coordinateSystem=un,this._reversedDepth=!1}get reversedDepth(){return this._reversedDepth}copy(e,t){return super.copy(e,t),this.matrixWorldInverse.copy(e.matrixWorldInverse),this.projectionMatrix.copy(e.projectionMatrix),this.projectionMatrixInverse.copy(e.projectionMatrixInverse),this.coordinateSystem=e.coordinateSystem,this}getWorldDirection(e){return super.getWorldDirection(e).negate()}updateMatrixWorld(e){super.updateMatrixWorld(e),this.matrixWorldInverse.copy(this.matrixWorld).invert()}updateWorldMatrix(e,t){super.updateWorldMatrix(e,t),this.matrixWorldInverse.copy(this.matrixWorld).invert()}clone(){return new this.constructor().copy(this)}},ni=new L,wh=new Ue,Ah=new Ue,Ot=class extends qs{constructor(e=50,t=1,i=.1,s=2e3){super(),this.isPerspectiveCamera=!0,this.type="PerspectiveCamera",this.fov=e,this.zoom=1,this.near=i,this.far=s,this.focus=10,this.aspect=t,this.view=null,this.filmGauge=35,this.filmOffset=0,this.updateProjectionMatrix()}copy(e,t){return super.copy(e,t),this.fov=e.fov,this.zoom=e.zoom,this.near=e.near,this.far=e.far,this.focus=e.focus,this.aspect=e.aspect,this.view=e.view===null?null:Object.assign({},e.view),this.filmGauge=e.filmGauge,this.filmOffset=e.filmOffset,this}setFocalLength(e){let t=.5*this.getFilmHeight()/e;this.fov=ia*2*Math.atan(t),this.updateProjectionMatrix()}getFocalLength(){let e=Math.tan(Xo*.5*this.fov);return .5*this.getFilmHeight()/e}getEffectiveFOV(){return ia*2*Math.atan(Math.tan(Xo*.5*this.fov)/this.zoom)}getFilmWidth(){return this.filmGauge*Math.min(this.aspect,1)}getFilmHeight(){return this.filmGauge/Math.max(this.aspect,1)}getViewBounds(e,t,i){ni.set(-1,-1,.5).applyMatrix4(this.projectionMatrixInverse),t.set(ni.x,ni.y).multiplyScalar(-e/ni.z),ni.set(1,1,.5).applyMatrix4(this.projectionMatrixInverse),i.set(ni.x,ni.y).multiplyScalar(-e/ni.z)}getViewSize(e,t){return this.getViewBounds(e,wh,Ah),t.subVectors(Ah,wh)}setViewOffset(e,t,i,s,r,o){this.aspect=e/t,this.view===null&&(this.view={enabled:!0,fullWidth:1,fullHeight:1,offsetX:0,offsetY:0,width:1,height:1}),this.view.enabled=!0,this.view.fullWidth=e,this.view.fullHeight=t,this.view.offsetX=i,this.view.offsetY=s,this.view.width=r,this.view.height=o,this.updateProjectionMatrix()}clearViewOffset(){this.view!==null&&(this.view.enabled=!1),this.updateProjectionMatrix()}updateProjectionMatrix(){let e=this.near,t=e*Math.tan(Xo*.5*this.fov)/this.zoom,i=2*t,s=this.aspect*i,r=-.5*s,o=this.view;if(this.view!==null&&this.view.enabled){let c=o.fullWidth,l=o.fullHeight;r+=o.offsetX*s/c,t-=o.offsetY*i/l,s*=o.width/c,i*=o.height/l}let a=this.filmOffset;a!==0&&(r+=e*a/this.getFilmWidth()),this.projectionMatrix.makePerspective(r,r+s,t,t-i,e,this.far,this.coordinateSystem,this.reversedDepth),this.projectionMatrixInverse.copy(this.projectionMatrix).invert()}toJSON(e){let t=super.toJSON(e);return t.object.fov=this.fov,t.object.zoom=this.zoom,t.object.near=this.near,t.object.far=this.far,t.object.focus=this.focus,t.object.aspect=this.aspect,this.view!==null&&(t.object.view=Object.assign({},this.view)),t.object.filmGauge=this.filmGauge,t.object.filmOffset=this.filmOffset,t}},es=-90,ts=1,oa=class extends wt{constructor(e,t,i){super(),this.type="CubeCamera",this.renderTarget=i,this.coordinateSystem=null,this.activeMipmapLevel=0;let s=new Ot(es,ts,e,t);s.layers=this.layers,this.add(s);let r=new Ot(es,ts,e,t);r.layers=this.layers,this.add(r);let o=new Ot(es,ts,e,t);o.layers=this.layers,this.add(o);let a=new Ot(es,ts,e,t);a.layers=this.layers,this.add(a);let c=new Ot(es,ts,e,t);c.layers=this.layers,this.add(c);let l=new Ot(es,ts,e,t);l.layers=this.layers,this.add(l)}updateCoordinateSystem(){let e=this.coordinateSystem,t=this.children.concat(),[i,s,r,o,a,c]=t;for(let l of t)this.remove(l);if(e===un)i.up.set(0,1,0),i.lookAt(1,0,0),s.up.set(0,1,0),s.lookAt(-1,0,0),r.up.set(0,0,-1),r.lookAt(0,1,0),o.up.set(0,0,1),o.lookAt(0,-1,0),a.up.set(0,1,0),a.lookAt(0,0,1),c.up.set(0,1,0),c.lookAt(0,0,-1);else if(e===zs)i.up.set(0,-1,0),i.lookAt(-1,0,0),s.up.set(0,-1,0),s.lookAt(1,0,0),r.up.set(0,0,1),r.lookAt(0,1,0),o.up.set(0,0,-1),o.lookAt(0,-1,0),a.up.set(0,-1,0),a.lookAt(0,0,1),c.up.set(0,-1,0),c.lookAt(0,0,-1);else throw new Error("THREE.CubeCamera.updateCoordinateSystem(): Invalid coordinate system: "+e);for(let l of t)this.add(l),l.updateMatrixWorld()}update(e,t){this.parent===null&&this.updateMatrixWorld();let{renderTarget:i,activeMipmapLevel:s}=this;this.coordinateSystem!==e.coordinateSystem&&(this.coordinateSystem=e.coordinateSystem,this.updateCoordinateSystem());let[r,o,a,c,l,h]=this.children,u=e.getRenderTarget(),p=e.getActiveCubeFace(),d=e.getActiveMipmapLevel(),g=e.xr.enabled;e.xr.enabled=!1;let v=i.texture.generateMipmaps;i.texture.generateMipmaps=!1,e.setRenderTarget(i,0,s),e.render(t,r),e.setRenderTarget(i,1,s),e.render(t,o),e.setRenderTarget(i,2,s),e.render(t,a),e.setRenderTarget(i,3,s),e.render(t,c),e.setRenderTarget(i,4,s),e.render(t,l),i.texture.generateMipmaps=v,e.setRenderTarget(i,5,s),e.render(t,h),e.setRenderTarget(u,p,d),e.xr.enabled=g,i.texture.needsPMREMUpdate=!0}},$s=class extends Wt{constructor(e=[],t=Pi,i,s,r,o,a,c,l,h){super(e,t,i,s,r,o,a,c,l,h),this.isCubeTexture=!0,this.flipY=!1}get images(){return this.image}set images(e){this.image=e}},la=class extends Sn{constructor(e=1,t={}){super(e,e,t),this.isWebGLCubeRenderTarget=!0;let i={width:e,height:e,depth:1},s=[i,i,i,i,i,i];this.texture=new $s(s),this._setTextureOptions(t),this.texture.isRenderTargetTexture=!0}fromEquirectangularTexture(e,t){this.texture.type=t.type,this.texture.colorSpace=t.colorSpace,this.texture.generateMipmaps=t.generateMipmaps,this.texture.minFilter=t.minFilter,this.texture.magFilter=t.magFilter;let i={uniforms:{tEquirect:{value:null}},vertexShader:`

				varying vec3 vWorldDirection;

				vec3 transformDirection( in vec3 dir, in mat4 matrix ) {

					return normalize( ( matrix * vec4( dir, 0.0 ) ).xyz );

				}

				void main() {

					vWorldDirection = transformDirection( position, modelMatrix );

					#include <begin_vertex>
					#include <project_vertex>

				}
			`,fragmentShader:`

				uniform sampler2D tEquirect;

				varying vec3 vWorldDirection;

				#include <common>

				void main() {

					vec3 direction = normalize( vWorldDirection );

					vec2 sampleUV = equirectUv( direction );

					gl_FragColor = texture2D( tEquirect, sampleUV );

				}
			`},s=new oi(5,5,5),r=new pn({name:"CubemapFromEquirect",uniforms:Di(i.uniforms),vertexShader:i.vertexShader,fragmentShader:i.fragmentShader,side:Bt,blending:Hn});r.uniforms.tEquirect.value=t;let o=new ft(s,r),a=t.minFilter;return t.minFilter===ui&&(t.minFilter=dn),new oa(1,10,this).update(e,o),t.minFilter=a,o.geometry.dispose(),o.material.dispose(),this}clear(e,t=!0,i=!0,s=!0){let r=e.getRenderTarget();for(let o=0;o<6;o++)e.setRenderTarget(this,o),e.clear(t,i,s);e.setRenderTarget(r)}},kt=class extends wt{constructor(){super(),this.isGroup=!0,this.type="Group"}},lf={type:"move"},ds=class{constructor(){this._targetRay=null,this._grip=null,this._hand=null}getHandSpace(){return this._hand===null&&(this._hand=new kt,this._hand.matrixAutoUpdate=!1,this._hand.visible=!1,this._hand.joints={},this._hand.inputState={pinching:!1}),this._hand}getTargetRaySpace(){return this._targetRay===null&&(this._targetRay=new kt,this._targetRay.matrixAutoUpdate=!1,this._targetRay.visible=!1,this._targetRay.hasLinearVelocity=!1,this._targetRay.linearVelocity=new L,this._targetRay.hasAngularVelocity=!1,this._targetRay.angularVelocity=new L),this._targetRay}getGripSpace(){return this._grip===null&&(this._grip=new kt,this._grip.matrixAutoUpdate=!1,this._grip.visible=!1,this._grip.hasLinearVelocity=!1,this._grip.linearVelocity=new L,this._grip.hasAngularVelocity=!1,this._grip.angularVelocity=new L),this._grip}dispatchEvent(e){return this._targetRay!==null&&this._targetRay.dispatchEvent(e),this._grip!==null&&this._grip.dispatchEvent(e),this._hand!==null&&this._hand.dispatchEvent(e),this}connect(e){if(e&&e.hand){let t=this._hand;if(t)for(let i of e.hand.values())this._getHandJoint(t,i)}return this.dispatchEvent({type:"connected",data:e}),this}disconnect(e){return this.dispatchEvent({type:"disconnected",data:e}),this._targetRay!==null&&(this._targetRay.visible=!1),this._grip!==null&&(this._grip.visible=!1),this._hand!==null&&(this._hand.visible=!1),this}update(e,t,i){let s=null,r=null,o=null,a=this._targetRay,c=this._grip,l=this._hand;if(e&&t.session.visibilityState!=="visible-blurred"){if(l&&e.hand){o=!0;for(let v of e.hand.values()){let m=t.getJointPose(v,i),f=this._getHandJoint(l,v);m!==null&&(f.matrix.fromArray(m.transform.matrix),f.matrix.decompose(f.position,f.rotation,f.scale),f.matrixWorldNeedsUpdate=!0,f.jointRadius=m.radius),f.visible=m!==null}let h=l.joints["index-finger-tip"],u=l.joints["thumb-tip"],p=h.position.distanceTo(u.position),d=.02,g=.005;l.inputState.pinching&&p>d+g?(l.inputState.pinching=!1,this.dispatchEvent({type:"pinchend",handedness:e.handedness,target:this})):!l.inputState.pinching&&p<=d-g&&(l.inputState.pinching=!0,this.dispatchEvent({type:"pinchstart",handedness:e.handedness,target:this}))}else c!==null&&e.gripSpace&&(r=t.getPose(e.gripSpace,i),r!==null&&(c.matrix.fromArray(r.transform.matrix),c.matrix.decompose(c.position,c.rotation,c.scale),c.matrixWorldNeedsUpdate=!0,r.linearVelocity?(c.hasLinearVelocity=!0,c.linearVelocity.copy(r.linearVelocity)):c.hasLinearVelocity=!1,r.angularVelocity?(c.hasAngularVelocity=!0,c.angularVelocity.copy(r.angularVelocity)):c.hasAngularVelocity=!1));a!==null&&(s=t.getPose(e.targetRaySpace,i),s===null&&r!==null&&(s=r),s!==null&&(a.matrix.fromArray(s.transform.matrix),a.matrix.decompose(a.position,a.rotation,a.scale),a.matrixWorldNeedsUpdate=!0,s.linearVelocity?(a.hasLinearVelocity=!0,a.linearVelocity.copy(s.linearVelocity)):a.hasLinearVelocity=!1,s.angularVelocity?(a.hasAngularVelocity=!0,a.angularVelocity.copy(s.angularVelocity)):a.hasAngularVelocity=!1,this.dispatchEvent(lf)))}return a!==null&&(a.visible=s!==null),c!==null&&(c.visible=r!==null),l!==null&&(l.visible=o!==null),this}_getHandJoint(e,t){if(e.joints[t.jointName]===void 0){let i=new kt;i.matrixAutoUpdate=!1,i.visible=!1,e.joints[t.jointName]=i,e.add(i)}return e.joints[t.jointName]}};var Ys=class extends wt{constructor(){super(),this.isScene=!0,this.type="Scene",this.background=null,this.environment=null,this.fog=null,this.backgroundBlurriness=0,this.backgroundIntensity=1,this.backgroundRotation=new fn,this.environmentIntensity=1,this.environmentRotation=new fn,this.overrideMaterial=null,typeof __THREE_DEVTOOLS__<"u"&&__THREE_DEVTOOLS__.dispatchEvent(new CustomEvent("observe",{detail:this}))}copy(e,t){return super.copy(e,t),e.background!==null&&(this.background=e.background.clone()),e.environment!==null&&(this.environment=e.environment.clone()),e.fog!==null&&(this.fog=e.fog.clone()),this.backgroundBlurriness=e.backgroundBlurriness,this.backgroundIntensity=e.backgroundIntensity,this.backgroundRotation.copy(e.backgroundRotation),this.environmentIntensity=e.environmentIntensity,this.environmentRotation.copy(e.environmentRotation),e.overrideMaterial!==null&&(this.overrideMaterial=e.overrideMaterial.clone()),this.matrixAutoUpdate=e.matrixAutoUpdate,this}toJSON(e){let t=super.toJSON(e);return this.fog!==null&&(t.object.fog=this.fog.toJSON()),this.backgroundBlurriness>0&&(t.object.backgroundBlurriness=this.backgroundBlurriness),this.backgroundIntensity!==1&&(t.object.backgroundIntensity=this.backgroundIntensity),t.object.backgroundRotation=this.backgroundRotation.toArray(),this.environmentIntensity!==1&&(t.object.environmentIntensity=this.environmentIntensity),t.object.environmentRotation=this.environmentRotation.toArray(),t}},ca=class{constructor(e,t){this.isInterleavedBuffer=!0,this.array=e,this.stride=t,this.count=e!==void 0?e.length/t:0,this.usage=na,this.updateRanges=[],this.version=0,this.uuid=si()}onUploadCallback(){}set needsUpdate(e){e===!0&&this.version++}setUsage(e){return this.usage=e,this}addUpdateRange(e,t){this.updateRanges.push({start:e,count:t})}clearUpdateRanges(){this.updateRanges.length=0}copy(e){return this.array=new e.array.constructor(e.array),this.count=e.count,this.stride=e.stride,this.usage=e.usage,this}copyAt(e,t,i){e*=this.stride,i*=t.stride;for(let s=0,r=this.stride;s<r;s++)this.array[e+s]=t.array[i+s];return this}set(e,t=0){return this.array.set(e,t),this}clone(e){e.arrayBuffers===void 0&&(e.arrayBuffers={}),this.array.buffer._uuid===void 0&&(this.array.buffer._uuid=si()),e.arrayBuffers[this.array.buffer._uuid]===void 0&&(e.arrayBuffers[this.array.buffer._uuid]=this.array.slice(0).buffer);let t=new this.array.constructor(e.arrayBuffers[this.array.buffer._uuid]),i=new this.constructor(t,this.stride);return i.setUsage(this.usage),i}onUpload(e){return this.onUploadCallback=e,this}toJSON(e){return e.arrayBuffers===void 0&&(e.arrayBuffers={}),this.array.buffer._uuid===void 0&&(this.array.buffer._uuid=si()),e.arrayBuffers[this.array.buffer._uuid]===void 0&&(e.arrayBuffers[this.array.buffer._uuid]=Array.from(new Uint32Array(this.array.buffer))),{uuid:this.uuid,buffer:this.array.buffer._uuid,type:this.array.constructor.name,stride:this.stride}}},Nt=new L,Zs=class n{constructor(e,t,i,s=!1){this.isInterleavedBufferAttribute=!0,this.name="",this.data=e,this.itemSize=t,this.offset=i,this.normalized=s}get count(){return this.data.count}get array(){return this.data.array}set needsUpdate(e){this.data.needsUpdate=e}applyMatrix4(e){for(let t=0,i=this.data.count;t<i;t++)Nt.fromBufferAttribute(this,t),Nt.applyMatrix4(e),this.setXYZ(t,Nt.x,Nt.y,Nt.z);return this}applyNormalMatrix(e){for(let t=0,i=this.count;t<i;t++)Nt.fromBufferAttribute(this,t),Nt.applyNormalMatrix(e),this.setXYZ(t,Nt.x,Nt.y,Nt.z);return this}transformDirection(e){for(let t=0,i=this.count;t<i;t++)Nt.fromBufferAttribute(this,t),Nt.transformDirection(e),this.setXYZ(t,Nt.x,Nt.y,Nt.z);return this}getComponent(e,t){let i=this.array[e*this.data.stride+this.offset+t];return this.normalized&&(i=Mn(i,this.array)),i}setComponent(e,t,i){return this.normalized&&(i=nt(i,this.array)),this.data.array[e*this.data.stride+this.offset+t]=i,this}setX(e,t){return this.normalized&&(t=nt(t,this.array)),this.data.array[e*this.data.stride+this.offset]=t,this}setY(e,t){return this.normalized&&(t=nt(t,this.array)),this.data.array[e*this.data.stride+this.offset+1]=t,this}setZ(e,t){return this.normalized&&(t=nt(t,this.array)),this.data.array[e*this.data.stride+this.offset+2]=t,this}setW(e,t){return this.normalized&&(t=nt(t,this.array)),this.data.array[e*this.data.stride+this.offset+3]=t,this}getX(e){let t=this.data.array[e*this.data.stride+this.offset];return this.normalized&&(t=Mn(t,this.array)),t}getY(e){let t=this.data.array[e*this.data.stride+this.offset+1];return this.normalized&&(t=Mn(t,this.array)),t}getZ(e){let t=this.data.array[e*this.data.stride+this.offset+2];return this.normalized&&(t=Mn(t,this.array)),t}getW(e){let t=this.data.array[e*this.data.stride+this.offset+3];return this.normalized&&(t=Mn(t,this.array)),t}setXY(e,t,i){return e=e*this.data.stride+this.offset,this.normalized&&(t=nt(t,this.array),i=nt(i,this.array)),this.data.array[e+0]=t,this.data.array[e+1]=i,this}setXYZ(e,t,i,s){return e=e*this.data.stride+this.offset,this.normalized&&(t=nt(t,this.array),i=nt(i,this.array),s=nt(s,this.array)),this.data.array[e+0]=t,this.data.array[e+1]=i,this.data.array[e+2]=s,this}setXYZW(e,t,i,s,r){return e=e*this.data.stride+this.offset,this.normalized&&(t=nt(t,this.array),i=nt(i,this.array),s=nt(s,this.array),r=nt(r,this.array)),this.data.array[e+0]=t,this.data.array[e+1]=i,this.data.array[e+2]=s,this.data.array[e+3]=r,this}clone(e){if(e===void 0){console.log("THREE.InterleavedBufferAttribute.clone(): Cloning an interleaved buffer attribute will de-interleave buffer data.");let t=[];for(let i=0;i<this.count;i++){let s=i*this.data.stride+this.offset;for(let r=0;r<this.itemSize;r++)t.push(this.data.array[s+r])}return new Ht(new this.array.constructor(t),this.itemSize,this.normalized)}else return e.interleavedBuffers===void 0&&(e.interleavedBuffers={}),e.interleavedBuffers[this.data.uuid]===void 0&&(e.interleavedBuffers[this.data.uuid]=this.data.clone(e)),new n(e.interleavedBuffers[this.data.uuid],this.itemSize,this.offset,this.normalized)}toJSON(e){if(e===void 0){console.log("THREE.InterleavedBufferAttribute.toJSON(): Serializing an interleaved buffer attribute will de-interleave buffer data.");let t=[];for(let i=0;i<this.count;i++){let s=i*this.data.stride+this.offset;for(let r=0;r<this.itemSize;r++)t.push(this.data.array[s+r])}return{itemSize:this.itemSize,type:this.array.constructor.name,array:t,normalized:this.normalized}}else return e.interleavedBuffers===void 0&&(e.interleavedBuffers={}),e.interleavedBuffers[this.data.uuid]===void 0&&(e.interleavedBuffers[this.data.uuid]=this.data.toJSON(e)),{isInterleavedBufferAttribute:!0,itemSize:this.itemSize,data:this.data.uuid,offset:this.offset,normalized:this.normalized}}},fs=class extends Gn{constructor(e){super(),this.isSpriteMaterial=!0,this.type="SpriteMaterial",this.color=new Xe(16777215),this.map=null,this.alphaMap=null,this.rotation=0,this.sizeAttenuation=!0,this.transparent=!0,this.fog=!0,this.setValues(e)}copy(e){return super.copy(e),this.color.copy(e.color),this.map=e.map,this.alphaMap=e.alphaMap,this.rotation=e.rotation,this.sizeAttenuation=e.sizeAttenuation,this.fog=e.fog,this}},ns,Us=new L,is=new L,ss=new L,rs=new Ue,Ns=new Ue,Eu=new dt,Xr=new L,Fs=new L,qr=new L,Rh=new Ue,fl=new Ue,Ch=new Ue,Js=class extends wt{constructor(e=new fs){if(super(),this.isSprite=!0,this.type="Sprite",ns===void 0){ns=new Xt;let t=new Float32Array([-.5,-.5,0,0,0,.5,-.5,0,1,0,.5,.5,0,1,1,-.5,.5,0,0,1]),i=new ca(t,5);ns.setIndex([0,1,2,0,2,3]),ns.setAttribute("position",new Zs(i,3,0,!1)),ns.setAttribute("uv",new Zs(i,2,3,!1))}this.geometry=ns,this.material=e,this.center=new Ue(.5,.5),this.count=1}raycast(e,t){e.camera===null&&console.error('THREE.Sprite: "Raycaster.camera" needs to be set in order to raycast against sprites.'),is.setFromMatrixScale(this.matrixWorld),Eu.copy(e.camera.matrixWorld),this.modelViewMatrix.multiplyMatrices(e.camera.matrixWorldInverse,this.matrixWorld),ss.setFromMatrixPosition(this.modelViewMatrix),e.camera.isPerspectiveCamera&&this.material.sizeAttenuation===!1&&is.multiplyScalar(-ss.z);let i=this.material.rotation,s,r;i!==0&&(r=Math.cos(i),s=Math.sin(i));let o=this.center;$r(Xr.set(-.5,-.5,0),ss,o,is,s,r),$r(Fs.set(.5,-.5,0),ss,o,is,s,r),$r(qr.set(.5,.5,0),ss,o,is,s,r),Rh.set(0,0),fl.set(1,0),Ch.set(1,1);let a=e.ray.intersectTriangle(Xr,Fs,qr,!1,Us);if(a===null&&($r(Fs.set(-.5,.5,0),ss,o,is,s,r),fl.set(0,1),a=e.ray.intersectTriangle(Xr,qr,Fs,!1,Us),a===null))return;let c=e.ray.origin.distanceTo(Us);c<e.near||c>e.far||t.push({distance:c,point:Us.clone(),uv:On.getInterpolation(Us,Xr,Fs,qr,Rh,fl,Ch,new Ue),face:null,object:this})}copy(e,t){return super.copy(e,t),e.center!==void 0&&this.center.copy(e.center),this.material=e.material,this}};function $r(n,e,t,i,s,r){rs.subVectors(n,t).addScalar(.5).multiply(i),s!==void 0?(Ns.x=r*rs.x-s*rs.y,Ns.y=s*rs.x+r*rs.y):Ns.copy(rs),n.copy(e),n.x+=Ns.x,n.y+=Ns.y,n.applyMatrix4(Eu)}var pl=new L,cf=new L,hf=new Oe,yn=class{constructor(e=new L(1,0,0),t=0){this.isPlane=!0,this.normal=e,this.constant=t}set(e,t){return this.normal.copy(e),this.constant=t,this}setComponents(e,t,i,s){return this.normal.set(e,t,i),this.constant=s,this}setFromNormalAndCoplanarPoint(e,t){return this.normal.copy(e),this.constant=-t.dot(this.normal),this}setFromCoplanarPoints(e,t,i){let s=pl.subVectors(i,t).cross(cf.subVectors(e,t)).normalize();return this.setFromNormalAndCoplanarPoint(s,e),this}copy(e){return this.normal.copy(e.normal),this.constant=e.constant,this}normalize(){let e=1/this.normal.length();return this.normal.multiplyScalar(e),this.constant*=e,this}negate(){return this.constant*=-1,this.normal.negate(),this}distanceToPoint(e){return this.normal.dot(e)+this.constant}distanceToSphere(e){return this.distanceToPoint(e.center)-e.radius}projectPoint(e,t){return t.copy(e).addScaledVector(this.normal,-this.distanceToPoint(e))}intersectLine(e,t){let i=e.delta(pl),s=this.normal.dot(i);if(s===0)return this.distanceToPoint(e.start)===0?t.copy(e.start):null;let r=-(e.start.dot(this.normal)+this.constant)/s;return r<0||r>1?null:t.copy(e.start).addScaledVector(i,r)}intersectsLine(e){let t=this.distanceToPoint(e.start),i=this.distanceToPoint(e.end);return t<0&&i>0||i<0&&t>0}intersectsBox(e){return e.intersectsPlane(this)}intersectsSphere(e){return e.intersectsPlane(this)}coplanarPoint(e){return e.copy(this.normal).multiplyScalar(-this.constant)}applyMatrix4(e,t){let i=t||hf.getNormalMatrix(e),s=this.coplanarPoint(pl).applyMatrix4(e),r=this.normal.applyMatrix3(i).normalize();return this.constant=-s.dot(r),this}translate(e){return this.constant-=e.dot(this.normal),this}equals(e){return e.normal.equals(this.normal)&&e.constant===this.constant}clone(){return new this.constructor().copy(this)}},Si=new hs,uf=new Ue(.5,.5),Yr=new L,ps=class{constructor(e=new yn,t=new yn,i=new yn,s=new yn,r=new yn,o=new yn){this.planes=[e,t,i,s,r,o]}set(e,t,i,s,r,o){let a=this.planes;return a[0].copy(e),a[1].copy(t),a[2].copy(i),a[3].copy(s),a[4].copy(r),a[5].copy(o),this}copy(e){let t=this.planes;for(let i=0;i<6;i++)t[i].copy(e.planes[i]);return this}setFromProjectionMatrix(e,t=un,i=!1){let s=this.planes,r=e.elements,o=r[0],a=r[1],c=r[2],l=r[3],h=r[4],u=r[5],p=r[6],d=r[7],g=r[8],v=r[9],m=r[10],f=r[11],T=r[12],E=r[13],S=r[14],R=r[15];if(s[0].setComponents(l-o,d-h,f-g,R-T).normalize(),s[1].setComponents(l+o,d+h,f+g,R+T).normalize(),s[2].setComponents(l+a,d+u,f+v,R+E).normalize(),s[3].setComponents(l-a,d-u,f-v,R-E).normalize(),i)s[4].setComponents(c,p,m,S).normalize(),s[5].setComponents(l-c,d-p,f-m,R-S).normalize();else if(s[4].setComponents(l-c,d-p,f-m,R-S).normalize(),t===un)s[5].setComponents(l+c,d+p,f+m,R+S).normalize();else if(t===zs)s[5].setComponents(c,p,m,S).normalize();else throw new Error("THREE.Frustum.setFromProjectionMatrix(): Invalid coordinate system: "+t);return this}intersectsObject(e){if(e.boundingSphere!==void 0)e.boundingSphere===null&&e.computeBoundingSphere(),Si.copy(e.boundingSphere).applyMatrix4(e.matrixWorld);else{let t=e.geometry;t.boundingSphere===null&&t.computeBoundingSphere(),Si.copy(t.boundingSphere).applyMatrix4(e.matrixWorld)}return this.intersectsSphere(Si)}intersectsSprite(e){Si.center.set(0,0,0);let t=uf.distanceTo(e.center);return Si.radius=.7071067811865476+t,Si.applyMatrix4(e.matrixWorld),this.intersectsSphere(Si)}intersectsSphere(e){let t=this.planes,i=e.center,s=-e.radius;for(let r=0;r<6;r++)if(t[r].distanceToPoint(i)<s)return!1;return!0}intersectsBox(e){let t=this.planes;for(let i=0;i<6;i++){let s=t[i];if(Yr.x=s.normal.x>0?e.max.x:e.min.x,Yr.y=s.normal.y>0?e.max.y:e.min.y,Yr.z=s.normal.z>0?e.max.z:e.min.z,s.distanceToPoint(Yr)<0)return!1}return!0}containsPoint(e){let t=this.planes;for(let i=0;i<6;i++)if(t[i].distanceToPoint(e)<0)return!1;return!0}clone(){return new this.constructor().copy(this)}};var Ks=class extends Wt{constructor(e,t,i,s,r,o,a,c,l){super(e,t,i,s,r,o,a,c,l),this.isCanvasTexture=!0,this.needsUpdate=!0}},js=class extends Wt{constructor(e,t,i=di,s,r,o,a=rn,c=rn,l,h=os,u=1){if(h!==os&&h!==ys)throw new Error("DepthTexture format must be either THREE.DepthFormat or THREE.DepthStencilFormat");let p={width:e,height:t,depth:u};super(p,s,r,o,a,c,h,i,l),this.isDepthTexture=!0,this.flipY=!1,this.generateMipmaps=!1,this.compareFunction=null}copy(e){return super.copy(e),this.source=new cs(Object.assign({},e.image)),this.compareFunction=e.compareFunction,this}toJSON(e){let t=super.toJSON(e);return this.compareFunction!==null&&(t.compareFunction=this.compareFunction),t}},Qs=class extends Wt{constructor(e=null){super(),this.sourceTexture=e,this.isExternalTexture=!0}copy(e){return super.copy(e),this.sourceTexture=e.sourceTexture,this}};var er=class n extends Xt{constructor(e=1,t=32,i=0,s=Math.PI*2){super(),this.type="CircleGeometry",this.parameters={radius:e,segments:t,thetaStart:i,thetaLength:s},t=Math.max(3,t);let r=[],o=[],a=[],c=[],l=new L,h=new Ue;o.push(0,0,0),a.push(0,0,1),c.push(.5,.5);for(let u=0,p=3;u<=t;u++,p+=3){let d=i+u/t*s;l.x=e*Math.cos(d),l.y=e*Math.sin(d),o.push(l.x,l.y,l.z),a.push(0,0,1),h.x=(o[p]/e+1)/2,h.y=(o[p+1]/e+1)/2,c.push(h.x,h.y)}for(let u=1;u<=t;u++)r.push(u,u+1,0);this.setIndex(r),this.setAttribute("position",new lt(o,3)),this.setAttribute("normal",new lt(a,3)),this.setAttribute("uv",new lt(c,2))}copy(e){return super.copy(e),this.parameters=Object.assign({},e.parameters),this}static fromJSON(e){return new n(e.radius,e.segments,e.thetaStart,e.thetaLength)}},tr=class n extends Xt{constructor(e=1,t=1,i=1,s=32,r=1,o=!1,a=0,c=Math.PI*2){super(),this.type="CylinderGeometry",this.parameters={radiusTop:e,radiusBottom:t,height:i,radialSegments:s,heightSegments:r,openEnded:o,thetaStart:a,thetaLength:c};let l=this;s=Math.floor(s),r=Math.floor(r);let h=[],u=[],p=[],d=[],g=0,v=[],m=i/2,f=0;T(),o===!1&&(e>0&&E(!0),t>0&&E(!1)),this.setIndex(h),this.setAttribute("position",new lt(u,3)),this.setAttribute("normal",new lt(p,3)),this.setAttribute("uv",new lt(d,2));function T(){let S=new L,R=new L,A=0,P=(t-e)/i;for(let N=0;N<=r;N++){let M=[],y=N/r,I=y*(t-e)+e;for(let O=0;O<=s;O++){let z=O/s,H=z*c+a,q=Math.sin(H),W=Math.cos(H);R.x=I*q,R.y=-y*i+m,R.z=I*W,u.push(R.x,R.y,R.z),S.set(q,P,W).normalize(),p.push(S.x,S.y,S.z),d.push(z,1-y),M.push(g++)}v.push(M)}for(let N=0;N<s;N++)for(let M=0;M<r;M++){let y=v[M][N],I=v[M+1][N],O=v[M+1][N+1],z=v[M][N+1];(e>0||M!==0)&&(h.push(y,I,z),A+=3),(t>0||M!==r-1)&&(h.push(I,O,z),A+=3)}l.addGroup(f,A,0),f+=A}function E(S){let R=g,A=new Ue,P=new L,N=0,M=S===!0?e:t,y=S===!0?1:-1;for(let O=1;O<=s;O++)u.push(0,m*y,0),p.push(0,y,0),d.push(.5,.5),g++;let I=g;for(let O=0;O<=s;O++){let H=O/s*c+a,q=Math.cos(H),W=Math.sin(H);P.x=M*W,P.y=m*y,P.z=M*q,u.push(P.x,P.y,P.z),p.push(0,y,0),A.x=q*.5+.5,A.y=W*.5*y+.5,d.push(A.x,A.y),g++}for(let O=0;O<s;O++){let z=R+O,H=I+O;S===!0?h.push(H,H+1,z):h.push(H+1,H,z),N+=3}l.addGroup(f,N,S===!0?1:2),f+=N}}copy(e){return super.copy(e),this.parameters=Object.assign({},e.parameters),this}static fromJSON(e){return new n(e.radiusTop,e.radiusBottom,e.height,e.radialSegments,e.heightSegments,e.openEnded,e.thetaStart,e.thetaLength)}};var nr=class n extends Xt{constructor(e=1,t=1,i=1,s=1){super(),this.type="PlaneGeometry",this.parameters={width:e,height:t,widthSegments:i,heightSegments:s};let r=e/2,o=t/2,a=Math.floor(i),c=Math.floor(s),l=a+1,h=c+1,u=e/a,p=t/c,d=[],g=[],v=[],m=[];for(let f=0;f<h;f++){let T=f*p-o;for(let E=0;E<l;E++){let S=E*u-r;g.push(S,-T,0),v.push(0,0,1),m.push(E/a),m.push(1-f/c)}}for(let f=0;f<c;f++)for(let T=0;T<a;T++){let E=T+l*f,S=T+l*(f+1),R=T+1+l*(f+1),A=T+1+l*f;d.push(E,S,A),d.push(S,R,A)}this.setIndex(d),this.setAttribute("position",new lt(g,3)),this.setAttribute("normal",new lt(v,3)),this.setAttribute("uv",new lt(m,2))}copy(e){return super.copy(e),this.parameters=Object.assign({},e.parameters),this}static fromJSON(e){return new n(e.width,e.height,e.widthSegments,e.heightSegments)}},Ai=class n extends Xt{constructor(e=.5,t=1,i=32,s=1,r=0,o=Math.PI*2){super(),this.type="RingGeometry",this.parameters={innerRadius:e,outerRadius:t,thetaSegments:i,phiSegments:s,thetaStart:r,thetaLength:o},i=Math.max(3,i),s=Math.max(1,s);let a=[],c=[],l=[],h=[],u=e,p=(t-e)/s,d=new L,g=new Ue;for(let v=0;v<=s;v++){for(let m=0;m<=i;m++){let f=r+m/i*o;d.x=u*Math.cos(f),d.y=u*Math.sin(f),c.push(d.x,d.y,d.z),l.push(0,0,1),g.x=(d.x/t+1)/2,g.y=(d.y/t+1)/2,h.push(g.x,g.y)}u+=p}for(let v=0;v<s;v++){let m=v*(i+1);for(let f=0;f<i;f++){let T=f+m,E=T,S=T+i+1,R=T+i+2,A=T+1;a.push(E,S,A),a.push(S,R,A)}}this.setIndex(a),this.setAttribute("position",new lt(c,3)),this.setAttribute("normal",new lt(l,3)),this.setAttribute("uv",new lt(h,2))}copy(e){return super.copy(e),this.parameters=Object.assign({},e.parameters),this}static fromJSON(e){return new n(e.innerRadius,e.outerRadius,e.thetaSegments,e.phiSegments,e.thetaStart,e.thetaLength)}};var ir=class n extends Xt{constructor(e=1,t=32,i=16,s=0,r=Math.PI*2,o=0,a=Math.PI){super(),this.type="SphereGeometry",this.parameters={radius:e,widthSegments:t,heightSegments:i,phiStart:s,phiLength:r,thetaStart:o,thetaLength:a},t=Math.max(3,Math.floor(t)),i=Math.max(2,Math.floor(i));let c=Math.min(o+a,Math.PI),l=0,h=[],u=new L,p=new L,d=[],g=[],v=[],m=[];for(let f=0;f<=i;f++){let T=[],E=f/i,S=0;f===0&&o===0?S=.5/t:f===i&&c===Math.PI&&(S=-.5/t);for(let R=0;R<=t;R++){let A=R/t;u.x=-e*Math.cos(s+A*r)*Math.sin(o+E*a),u.y=e*Math.cos(o+E*a),u.z=e*Math.sin(s+A*r)*Math.sin(o+E*a),g.push(u.x,u.y,u.z),p.copy(u).normalize(),v.push(p.x,p.y,p.z),m.push(A+S,1-E),T.push(l++)}h.push(T)}for(let f=0;f<i;f++)for(let T=0;T<t;T++){let E=h[f][T+1],S=h[f][T],R=h[f+1][T],A=h[f+1][T+1];(f!==0||o>0)&&d.push(E,S,A),(f!==i-1||c<Math.PI)&&d.push(S,R,A)}this.setIndex(d),this.setAttribute("position",new lt(g,3)),this.setAttribute("normal",new lt(v,3)),this.setAttribute("uv",new lt(m,2))}copy(e){return super.copy(e),this.parameters=Object.assign({},e.parameters),this}static fromJSON(e){return new n(e.radius,e.widthSegments,e.heightSegments,e.phiStart,e.phiLength,e.thetaStart,e.thetaLength)}};var li=class n extends Xt{constructor(e=1,t=.4,i=12,s=48,r=Math.PI*2){super(),this.type="TorusGeometry",this.parameters={radius:e,tube:t,radialSegments:i,tubularSegments:s,arc:r},i=Math.floor(i),s=Math.floor(s);let o=[],a=[],c=[],l=[],h=new L,u=new L,p=new L;for(let d=0;d<=i;d++)for(let g=0;g<=s;g++){let v=g/s*r,m=d/i*Math.PI*2;u.x=(e+t*Math.cos(m))*Math.cos(v),u.y=(e+t*Math.cos(m))*Math.sin(v),u.z=t*Math.sin(m),a.push(u.x,u.y,u.z),h.x=e*Math.cos(v),h.y=e*Math.sin(v),p.subVectors(u,h).normalize(),c.push(p.x,p.y,p.z),l.push(g/s),l.push(d/i)}for(let d=1;d<=i;d++)for(let g=1;g<=s;g++){let v=(s+1)*d+g-1,m=(s+1)*(d-1)+g-1,f=(s+1)*(d-1)+g,T=(s+1)*d+g;o.push(v,m,T),o.push(m,f,T)}this.setIndex(o),this.setAttribute("position",new lt(a,3)),this.setAttribute("normal",new lt(c,3)),this.setAttribute("uv",new lt(l,2))}copy(e){return super.copy(e),this.parameters=Object.assign({},e.parameters),this}static fromJSON(e){return new n(e.radius,e.tube,e.radialSegments,e.tubularSegments,e.arc)}};var Ri=class extends Gn{constructor(e){super(),this.isMeshStandardMaterial=!0,this.type="MeshStandardMaterial",this.defines={STANDARD:""},this.color=new Xe(16777215),this.roughness=1,this.metalness=0,this.map=null,this.lightMap=null,this.lightMapIntensity=1,this.aoMap=null,this.aoMapIntensity=1,this.emissive=new Xe(0),this.emissiveIntensity=1,this.emissiveMap=null,this.bumpMap=null,this.bumpScale=1,this.normalMap=null,this.normalMapType=zl,this.normalScale=new Ue(1,1),this.displacementMap=null,this.displacementScale=1,this.displacementBias=0,this.roughnessMap=null,this.metalnessMap=null,this.alphaMap=null,this.envMap=null,this.envMapRotation=new fn,this.envMapIntensity=1,this.wireframe=!1,this.wireframeLinewidth=1,this.wireframeLinecap="round",this.wireframeLinejoin="round",this.flatShading=!1,this.fog=!0,this.setValues(e)}copy(e){return super.copy(e),this.defines={STANDARD:""},this.color.copy(e.color),this.roughness=e.roughness,this.metalness=e.metalness,this.map=e.map,this.lightMap=e.lightMap,this.lightMapIntensity=e.lightMapIntensity,this.aoMap=e.aoMap,this.aoMapIntensity=e.aoMapIntensity,this.emissive.copy(e.emissive),this.emissiveMap=e.emissiveMap,this.emissiveIntensity=e.emissiveIntensity,this.bumpMap=e.bumpMap,this.bumpScale=e.bumpScale,this.normalMap=e.normalMap,this.normalMapType=e.normalMapType,this.normalScale.copy(e.normalScale),this.displacementMap=e.displacementMap,this.displacementScale=e.displacementScale,this.displacementBias=e.displacementBias,this.roughnessMap=e.roughnessMap,this.metalnessMap=e.metalnessMap,this.alphaMap=e.alphaMap,this.envMap=e.envMap,this.envMapRotation.copy(e.envMapRotation),this.envMapIntensity=e.envMapIntensity,this.wireframe=e.wireframe,this.wireframeLinewidth=e.wireframeLinewidth,this.wireframeLinecap=e.wireframeLinecap,this.wireframeLinejoin=e.wireframeLinejoin,this.flatShading=e.flatShading,this.fog=e.fog,this}};var ha=class extends Gn{constructor(e){super(),this.isMeshDepthMaterial=!0,this.type="MeshDepthMaterial",this.depthPacking=hu,this.map=null,this.alphaMap=null,this.displacementMap=null,this.displacementScale=1,this.displacementBias=0,this.wireframe=!1,this.wireframeLinewidth=1,this.setValues(e)}copy(e){return super.copy(e),this.depthPacking=e.depthPacking,this.map=e.map,this.alphaMap=e.alphaMap,this.displacementMap=e.displacementMap,this.displacementScale=e.displacementScale,this.displacementBias=e.displacementBias,this.wireframe=e.wireframe,this.wireframeLinewidth=e.wireframeLinewidth,this}},ua=class extends Gn{constructor(e){super(),this.isMeshDistanceMaterial=!0,this.type="MeshDistanceMaterial",this.map=null,this.alphaMap=null,this.displacementMap=null,this.displacementScale=1,this.displacementBias=0,this.setValues(e)}copy(e){return super.copy(e),this.map=e.map,this.alphaMap=e.alphaMap,this.displacementMap=e.displacementMap,this.displacementScale=e.displacementScale,this.displacementBias=e.displacementBias,this}};function Zr(n,e){return!n||n.constructor===e?n:typeof e.BYTES_PER_ELEMENT=="number"?new e(n):Array.prototype.slice.call(n)}function df(n){return ArrayBuffer.isView(n)&&!(n instanceof DataView)}var Ci=class{constructor(e,t,i,s){this.parameterPositions=e,this._cachedIndex=0,this.resultBuffer=s!==void 0?s:new t.constructor(i),this.sampleValues=t,this.valueSize=i,this.settings=null,this.DefaultSettings_={}}evaluate(e){let t=this.parameterPositions,i=this._cachedIndex,s=t[i],r=t[i-1];n:{e:{let o;t:{i:if(!(e<s)){for(let a=i+2;;){if(s===void 0){if(e<r)break i;return i=t.length,this._cachedIndex=i,this.copySampleValue_(i-1)}if(i===a)break;if(r=s,s=t[++i],e<s)break e}o=t.length;break t}if(!(e>=r)){let a=t[1];e<a&&(i=2,r=a);for(let c=i-2;;){if(r===void 0)return this._cachedIndex=0,this.copySampleValue_(0);if(i===c)break;if(s=r,r=t[--i-1],e>=r)break e}o=i,i=0;break t}break n}for(;i<o;){let a=i+o>>>1;e<t[a]?o=a:i=a+1}if(s=t[i],r=t[i-1],r===void 0)return this._cachedIndex=0,this.copySampleValue_(0);if(s===void 0)return i=t.length,this._cachedIndex=i,this.copySampleValue_(i-1)}this._cachedIndex=i,this.intervalChanged_(i,r,s)}return this.interpolate_(i,r,e,s)}getSettings_(){return this.settings||this.DefaultSettings_}copySampleValue_(e){let t=this.resultBuffer,i=this.sampleValues,s=this.valueSize,r=e*s;for(let o=0;o!==s;++o)t[o]=i[r+o];return t}interpolate_(){throw new Error("call to abstract method")}intervalChanged_(){}},da=class extends Ci{constructor(e,t,i,s){super(e,t,i,s),this._weightPrev=-0,this._offsetPrev=-0,this._weightNext=-0,this._offsetNext=-0,this.DefaultSettings_={endingStart:_l,endingEnd:_l}}intervalChanged_(e,t,i){let s=this.parameterPositions,r=e-2,o=e+1,a=s[r],c=s[o];if(a===void 0)switch(this.getSettings_().endingStart){case xl:r=e,a=2*t-i;break;case vl:r=s.length-2,a=t+s[r]-s[r+1];break;default:r=e,a=i}if(c===void 0)switch(this.getSettings_().endingEnd){case xl:o=e,c=2*i-t;break;case vl:o=1,c=i+s[1]-s[0];break;default:o=e-1,c=t}let l=(i-t)*.5,h=this.valueSize;this._weightPrev=l/(t-a),this._weightNext=l/(c-i),this._offsetPrev=r*h,this._offsetNext=o*h}interpolate_(e,t,i,s){let r=this.resultBuffer,o=this.sampleValues,a=this.valueSize,c=e*a,l=c-a,h=this._offsetPrev,u=this._offsetNext,p=this._weightPrev,d=this._weightNext,g=(i-t)/(s-t),v=g*g,m=v*g,f=-p*m+2*p*v-p*g,T=(1+p)*m+(-1.5-2*p)*v+(-.5+p)*g+1,E=(-1-d)*m+(1.5+d)*v+.5*g,S=d*m-d*v;for(let R=0;R!==a;++R)r[R]=f*o[h+R]+T*o[l+R]+E*o[c+R]+S*o[u+R];return r}},fa=class extends Ci{constructor(e,t,i,s){super(e,t,i,s)}interpolate_(e,t,i,s){let r=this.resultBuffer,o=this.sampleValues,a=this.valueSize,c=e*a,l=c-a,h=(i-t)/(s-t),u=1-h;for(let p=0;p!==a;++p)r[p]=o[l+p]*u+o[c+p]*h;return r}},pa=class extends Ci{constructor(e,t,i,s){super(e,t,i,s)}interpolate_(e){return this.copySampleValue_(e-1)}},Qt=class{constructor(e,t,i,s){if(e===void 0)throw new Error("THREE.KeyframeTrack: track name is undefined");if(t===void 0||t.length===0)throw new Error("THREE.KeyframeTrack: no keyframes in track named "+e);this.name=e,this.times=Zr(t,this.TimeBufferType),this.values=Zr(i,this.ValueBufferType),this.setInterpolation(s||this.DefaultInterpolation)}static toJSON(e){let t=e.constructor,i;if(t.toJSON!==this.toJSON)i=t.toJSON(e);else{i={name:e.name,times:Zr(e.times,Array),values:Zr(e.values,Array)};let s=e.getInterpolation();s!==e.DefaultInterpolation&&(i.interpolation=s)}return i.type=e.ValueTypeName,i}InterpolantFactoryMethodDiscrete(e){return new pa(this.times,this.values,this.getValueSize(),e)}InterpolantFactoryMethodLinear(e){return new fa(this.times,this.values,this.getValueSize(),e)}InterpolantFactoryMethodSmooth(e){return new da(this.times,this.values,this.getValueSize(),e)}setInterpolation(e){let t;switch(e){case ks:t=this.InterpolantFactoryMethodDiscrete;break;case ta:t=this.InterpolantFactoryMethodLinear;break;case Jr:t=this.InterpolantFactoryMethodSmooth;break}if(t===void 0){let i="unsupported interpolation for "+this.ValueTypeName+" keyframe track named "+this.name;if(this.createInterpolant===void 0)if(e!==this.DefaultInterpolation)this.setInterpolation(this.DefaultInterpolation);else throw new Error(i);return console.warn("THREE.KeyframeTrack:",i),this}return this.createInterpolant=t,this}getInterpolation(){switch(this.createInterpolant){case this.InterpolantFactoryMethodDiscrete:return ks;case this.InterpolantFactoryMethodLinear:return ta;case this.InterpolantFactoryMethodSmooth:return Jr}}getValueSize(){return this.values.length/this.times.length}shift(e){if(e!==0){let t=this.times;for(let i=0,s=t.length;i!==s;++i)t[i]+=e}return this}scale(e){if(e!==1){let t=this.times;for(let i=0,s=t.length;i!==s;++i)t[i]*=e}return this}trim(e,t){let i=this.times,s=i.length,r=0,o=s-1;for(;r!==s&&i[r]<e;)++r;for(;o!==-1&&i[o]>t;)--o;if(++o,r!==0||o!==s){r>=o&&(o=Math.max(o,1),r=o-1);let a=this.getValueSize();this.times=i.slice(r,o),this.values=this.values.slice(r*a,o*a)}return this}validate(){let e=!0,t=this.getValueSize();t-Math.floor(t)!==0&&(console.error("THREE.KeyframeTrack: Invalid value size in track.",this),e=!1);let i=this.times,s=this.values,r=i.length;r===0&&(console.error("THREE.KeyframeTrack: Track is empty.",this),e=!1);let o=null;for(let a=0;a!==r;a++){let c=i[a];if(typeof c=="number"&&isNaN(c)){console.error("THREE.KeyframeTrack: Time is not a valid number.",this,a,c),e=!1;break}if(o!==null&&o>c){console.error("THREE.KeyframeTrack: Out of order keys.",this,a,c,o),e=!1;break}o=c}if(s!==void 0&&df(s))for(let a=0,c=s.length;a!==c;++a){let l=s[a];if(isNaN(l)){console.error("THREE.KeyframeTrack: Value is not a valid number.",this,a,l),e=!1;break}}return e}optimize(){let e=this.times.slice(),t=this.values.slice(),i=this.getValueSize(),s=this.getInterpolation()===Jr,r=e.length-1,o=1;for(let a=1;a<r;++a){let c=!1,l=e[a],h=e[a+1];if(l!==h&&(a!==1||l!==e[0]))if(s)c=!0;else{let u=a*i,p=u-i,d=u+i;for(let g=0;g!==i;++g){let v=t[u+g];if(v!==t[p+g]||v!==t[d+g]){c=!0;break}}}if(c){if(a!==o){e[o]=e[a];let u=a*i,p=o*i;for(let d=0;d!==i;++d)t[p+d]=t[u+d]}++o}}if(r>0){e[o]=e[r];for(let a=r*i,c=o*i,l=0;l!==i;++l)t[c+l]=t[a+l];++o}return o!==e.length?(this.times=e.slice(0,o),this.values=t.slice(0,o*i)):(this.times=e,this.values=t),this}clone(){let e=this.times.slice(),t=this.values.slice(),i=this.constructor,s=new i(this.name,e,t);return s.createInterpolant=this.createInterpolant,s}};Qt.prototype.ValueTypeName="";Qt.prototype.TimeBufferType=Float32Array;Qt.prototype.ValueBufferType=Float32Array;Qt.prototype.DefaultInterpolation=ta;var ci=class extends Qt{constructor(e,t,i){super(e,t,i)}};ci.prototype.ValueTypeName="bool";ci.prototype.ValueBufferType=Array;ci.prototype.DefaultInterpolation=ks;ci.prototype.InterpolantFactoryMethodLinear=void 0;ci.prototype.InterpolantFactoryMethodSmooth=void 0;var ma=class extends Qt{constructor(e,t,i,s){super(e,t,i,s)}};ma.prototype.ValueTypeName="color";var ga=class extends Qt{constructor(e,t,i,s){super(e,t,i,s)}};ga.prototype.ValueTypeName="number";var _a=class extends Ci{constructor(e,t,i,s){super(e,t,i,s)}interpolate_(e,t,i,s){let r=this.resultBuffer,o=this.sampleValues,a=this.valueSize,c=(i-t)/(s-t),l=e*a;for(let h=l+a;l!==h;l+=4)Vn.slerpFlat(r,0,o,l-a,o,l,c);return r}},sr=class extends Qt{constructor(e,t,i,s){super(e,t,i,s)}InterpolantFactoryMethodLinear(e){return new _a(this.times,this.values,this.getValueSize(),e)}};sr.prototype.ValueTypeName="quaternion";sr.prototype.InterpolantFactoryMethodSmooth=void 0;var hi=class extends Qt{constructor(e,t,i){super(e,t,i)}};hi.prototype.ValueTypeName="string";hi.prototype.ValueBufferType=Array;hi.prototype.DefaultInterpolation=ks;hi.prototype.InterpolantFactoryMethodLinear=void 0;hi.prototype.InterpolantFactoryMethodSmooth=void 0;var xa=class extends Qt{constructor(e,t,i,s){super(e,t,i,s)}};xa.prototype.ValueTypeName="vector";var va=class{constructor(e,t,i){let s=this,r=!1,o=0,a=0,c,l=[];this.onStart=void 0,this.onLoad=e,this.onProgress=t,this.onError=i,this.abortController=new AbortController,this.itemStart=function(h){a++,r===!1&&s.onStart!==void 0&&s.onStart(h,o,a),r=!0},this.itemEnd=function(h){o++,s.onProgress!==void 0&&s.onProgress(h,o,a),o===a&&(r=!1,s.onLoad!==void 0&&s.onLoad())},this.itemError=function(h){s.onError!==void 0&&s.onError(h)},this.resolveURL=function(h){return c?c(h):h},this.setURLModifier=function(h){return c=h,this},this.addHandler=function(h,u){return l.push(h,u),this},this.removeHandler=function(h){let u=l.indexOf(h);return u!==-1&&l.splice(u,2),this},this.getHandler=function(h){for(let u=0,p=l.length;u<p;u+=2){let d=l[u],g=l[u+1];if(d.global&&(d.lastIndex=0),d.test(h))return g}return null},this.abort=function(){return this.abortController.abort(),this.abortController=new AbortController,this}}},Tu=new va,ya=class{constructor(e){this.manager=e!==void 0?e:Tu,this.crossOrigin="anonymous",this.withCredentials=!1,this.path="",this.resourcePath="",this.requestHeader={}}load(){}loadAsync(e,t){let i=this;return new Promise(function(s,r){i.load(e,s,t,r)})}parse(){}setCrossOrigin(e){return this.crossOrigin=e,this}setWithCredentials(e){return this.withCredentials=e,this}setPath(e){return this.path=e,this}setResourcePath(e){return this.resourcePath=e,this}setRequestHeader(e){return this.requestHeader=e,this}abort(){return this}};ya.DEFAULT_MATERIAL_NAME="__DEFAULT";var ms=class extends wt{constructor(e,t=1){super(),this.isLight=!0,this.type="Light",this.color=new Xe(e),this.intensity=t}dispose(){}copy(e,t){return super.copy(e,t),this.color.copy(e.color),this.intensity=e.intensity,this}toJSON(e){let t=super.toJSON(e);return t.object.color=this.color.getHex(),t.object.intensity=this.intensity,this.groundColor!==void 0&&(t.object.groundColor=this.groundColor.getHex()),this.distance!==void 0&&(t.object.distance=this.distance),this.angle!==void 0&&(t.object.angle=this.angle),this.decay!==void 0&&(t.object.decay=this.decay),this.penumbra!==void 0&&(t.object.penumbra=this.penumbra),this.shadow!==void 0&&(t.object.shadow=this.shadow.toJSON()),this.target!==void 0&&(t.object.target=this.target.uuid),t}},rr=class extends ms{constructor(e,t,i){super(e,i),this.isHemisphereLight=!0,this.type="HemisphereLight",this.position.copy(wt.DEFAULT_UP),this.updateMatrix(),this.groundColor=new Xe(t)}copy(e,t){return super.copy(e,t),this.groundColor.copy(e.groundColor),this}},ml=new dt,Ih=new L,Ph=new L,Ma=class{constructor(e){this.camera=e,this.intensity=1,this.bias=0,this.normalBias=0,this.radius=1,this.blurSamples=8,this.mapSize=new Ue(512,512),this.mapType=mn,this.map=null,this.mapPass=null,this.matrix=new dt,this.autoUpdate=!0,this.needsUpdate=!1,this._frustum=new ps,this._frameExtents=new Ue(1,1),this._viewportCount=1,this._viewports=[new Qe(0,0,1,1)]}getViewportCount(){return this._viewportCount}getFrustum(){return this._frustum}updateMatrices(e){let t=this.camera,i=this.matrix;Ih.setFromMatrixPosition(e.matrixWorld),t.position.copy(Ih),Ph.setFromMatrixPosition(e.target.matrixWorld),t.lookAt(Ph),t.updateMatrixWorld(),ml.multiplyMatrices(t.projectionMatrix,t.matrixWorldInverse),this._frustum.setFromProjectionMatrix(ml,t.coordinateSystem,t.reversedDepth),t.reversedDepth?i.set(.5,0,0,.5,0,.5,0,.5,0,0,1,0,0,0,0,1):i.set(.5,0,0,.5,0,.5,0,.5,0,0,.5,.5,0,0,0,1),i.multiply(ml)}getViewport(e){return this._viewports[e]}getFrameExtents(){return this._frameExtents}dispose(){this.map&&this.map.dispose(),this.mapPass&&this.mapPass.dispose()}copy(e){return this.camera=e.camera.clone(),this.intensity=e.intensity,this.bias=e.bias,this.radius=e.radius,this.autoUpdate=e.autoUpdate,this.needsUpdate=e.needsUpdate,this.normalBias=e.normalBias,this.blurSamples=e.blurSamples,this.mapSize.copy(e.mapSize),this}clone(){return new this.constructor().copy(this)}toJSON(){let e={};return this.intensity!==1&&(e.intensity=this.intensity),this.bias!==0&&(e.bias=this.bias),this.normalBias!==0&&(e.normalBias=this.normalBias),this.radius!==1&&(e.radius=this.radius),(this.mapSize.x!==512||this.mapSize.y!==512)&&(e.mapSize=this.mapSize.toArray()),e.camera=this.camera.toJSON(!1).object,delete e.camera.matrix,e}};var Lh=new dt,Os=new L,gl=new L,Ml=class extends Ma{constructor(){super(new Ot(90,1,.5,500)),this.isPointLightShadow=!0,this._frameExtents=new Ue(4,2),this._viewportCount=6,this._viewports=[new Qe(2,1,1,1),new Qe(0,1,1,1),new Qe(3,1,1,1),new Qe(1,1,1,1),new Qe(3,0,1,1),new Qe(1,0,1,1)],this._cubeDirections=[new L(1,0,0),new L(-1,0,0),new L(0,0,1),new L(0,0,-1),new L(0,1,0),new L(0,-1,0)],this._cubeUps=[new L(0,1,0),new L(0,1,0),new L(0,1,0),new L(0,1,0),new L(0,0,1),new L(0,0,-1)]}updateMatrices(e,t=0){let i=this.camera,s=this.matrix,r=e.distance||i.far;r!==i.far&&(i.far=r,i.updateProjectionMatrix()),Os.setFromMatrixPosition(e.matrixWorld),i.position.copy(Os),gl.copy(i.position),gl.add(this._cubeDirections[t]),i.up.copy(this._cubeUps[t]),i.lookAt(gl),i.updateMatrixWorld(),s.makeTranslation(-Os.x,-Os.y,-Os.z),Lh.multiplyMatrices(i.projectionMatrix,i.matrixWorldInverse),this._frustum.setFromProjectionMatrix(Lh,i.coordinateSystem,i.reversedDepth)}},ar=class extends ms{constructor(e,t,i=0,s=2){super(e,t),this.isPointLight=!0,this.type="PointLight",this.distance=i,this.decay=s,this.shadow=new Ml}get power(){return this.intensity*4*Math.PI}set power(e){this.intensity=e/(4*Math.PI)}dispose(){this.shadow.dispose()}copy(e,t){return super.copy(e,t),this.distance=e.distance,this.decay=e.decay,this.shadow=e.shadow.clone(),this}},Ii=class extends qs{constructor(e=-1,t=1,i=1,s=-1,r=.1,o=2e3){super(),this.isOrthographicCamera=!0,this.type="OrthographicCamera",this.zoom=1,this.view=null,this.left=e,this.right=t,this.top=i,this.bottom=s,this.near=r,this.far=o,this.updateProjectionMatrix()}copy(e,t){return super.copy(e,t),this.left=e.left,this.right=e.right,this.top=e.top,this.bottom=e.bottom,this.near=e.near,this.far=e.far,this.zoom=e.zoom,this.view=e.view===null?null:Object.assign({},e.view),this}setViewOffset(e,t,i,s,r,o){this.view===null&&(this.view={enabled:!0,fullWidth:1,fullHeight:1,offsetX:0,offsetY:0,width:1,height:1}),this.view.enabled=!0,this.view.fullWidth=e,this.view.fullHeight=t,this.view.offsetX=i,this.view.offsetY=s,this.view.width=r,this.view.height=o,this.updateProjectionMatrix()}clearViewOffset(){this.view!==null&&(this.view.enabled=!1),this.updateProjectionMatrix()}updateProjectionMatrix(){let e=(this.right-this.left)/(2*this.zoom),t=(this.top-this.bottom)/(2*this.zoom),i=(this.right+this.left)/2,s=(this.top+this.bottom)/2,r=i-e,o=i+e,a=s+t,c=s-t;if(this.view!==null&&this.view.enabled){let l=(this.right-this.left)/this.view.fullWidth/this.zoom,h=(this.top-this.bottom)/this.view.fullHeight/this.zoom;r+=l*this.view.offsetX,o=r+l*this.view.width,a-=h*this.view.offsetY,c=a-h*this.view.height}this.projectionMatrix.makeOrthographic(r,o,a,c,this.near,this.far,this.coordinateSystem,this.reversedDepth),this.projectionMatrixInverse.copy(this.projectionMatrix).invert()}toJSON(e){let t=super.toJSON(e);return t.object.zoom=this.zoom,t.object.left=this.left,t.object.right=this.right,t.object.top=this.top,t.object.bottom=this.bottom,t.object.near=this.near,t.object.far=this.far,this.view!==null&&(t.object.view=Object.assign({},this.view)),t}},Sl=class extends Ma{constructor(){super(new Ii(-5,5,5,-5,.5,500)),this.isDirectionalLightShadow=!0}},gs=class extends ms{constructor(e,t){super(e,t),this.isDirectionalLight=!0,this.type="DirectionalLight",this.position.copy(wt.DEFAULT_UP),this.updateMatrix(),this.target=new wt,this.shadow=new Sl}dispose(){this.shadow.dispose()}copy(e){return super.copy(e),this.target=e.target.clone(),this.shadow=e.shadow.clone(),this}};var Sa=class extends Ot{constructor(e=[]){super(),this.isArrayCamera=!0,this.isMultiViewCamera=!1,this.cameras=e}};var Xl="\\[\\]\\.:\\/",ff=new RegExp("["+Xl+"]","g"),ql="[^"+Xl+"]",pf="[^"+Xl.replace("\\.","")+"]",mf=/((?:WC+[\/:])*)/.source.replace("WC",ql),gf=/(WCOD+)?/.source.replace("WCOD",pf),_f=/(?:\.(WC+)(?:\[(.+)\])?)?/.source.replace("WC",ql),xf=/\.(WC+)(?:\[(.+)\])?/.source.replace("WC",ql),vf=new RegExp("^"+mf+gf+_f+xf+"$"),yf=["material","materials","bones","map"],bl=class{constructor(e,t,i){let s=i||ot.parseTrackName(t);this._targetGroup=e,this._bindings=e.subscribe_(t,s)}getValue(e,t){this.bind();let i=this._targetGroup.nCachedObjects_,s=this._bindings[i];s!==void 0&&s.getValue(e,t)}setValue(e,t){let i=this._bindings;for(let s=this._targetGroup.nCachedObjects_,r=i.length;s!==r;++s)i[s].setValue(e,t)}bind(){let e=this._bindings;for(let t=this._targetGroup.nCachedObjects_,i=e.length;t!==i;++t)e[t].bind()}unbind(){let e=this._bindings;for(let t=this._targetGroup.nCachedObjects_,i=e.length;t!==i;++t)e[t].unbind()}},ot=class n{constructor(e,t,i){this.path=t,this.parsedPath=i||n.parseTrackName(t),this.node=n.findNode(e,this.parsedPath.nodeName),this.rootNode=e,this.getValue=this._getValue_unbound,this.setValue=this._setValue_unbound}static create(e,t,i){return e&&e.isAnimationObjectGroup?new n.Composite(e,t,i):new n(e,t,i)}static sanitizeNodeName(e){return e.replace(/\s/g,"_").replace(ff,"")}static parseTrackName(e){let t=vf.exec(e);if(t===null)throw new Error("PropertyBinding: Cannot parse trackName: "+e);let i={nodeName:t[2],objectName:t[3],objectIndex:t[4],propertyName:t[5],propertyIndex:t[6]},s=i.nodeName&&i.nodeName.lastIndexOf(".");if(s!==void 0&&s!==-1){let r=i.nodeName.substring(s+1);yf.indexOf(r)!==-1&&(i.nodeName=i.nodeName.substring(0,s),i.objectName=r)}if(i.propertyName===null||i.propertyName.length===0)throw new Error("PropertyBinding: can not parse propertyName from trackName: "+e);return i}static findNode(e,t){if(t===void 0||t===""||t==="."||t===-1||t===e.name||t===e.uuid)return e;if(e.skeleton){let i=e.skeleton.getBoneByName(t);if(i!==void 0)return i}if(e.children){let i=function(r){for(let o=0;o<r.length;o++){let a=r[o];if(a.name===t||a.uuid===t)return a;let c=i(a.children);if(c)return c}return null},s=i(e.children);if(s)return s}return null}_getValue_unavailable(){}_setValue_unavailable(){}_getValue_direct(e,t){e[t]=this.targetObject[this.propertyName]}_getValue_array(e,t){let i=this.resolvedProperty;for(let s=0,r=i.length;s!==r;++s)e[t++]=i[s]}_getValue_arrayElement(e,t){e[t]=this.resolvedProperty[this.propertyIndex]}_getValue_toArray(e,t){this.resolvedProperty.toArray(e,t)}_setValue_direct(e,t){this.targetObject[this.propertyName]=e[t]}_setValue_direct_setNeedsUpdate(e,t){this.targetObject[this.propertyName]=e[t],this.targetObject.needsUpdate=!0}_setValue_direct_setMatrixWorldNeedsUpdate(e,t){this.targetObject[this.propertyName]=e[t],this.targetObject.matrixWorldNeedsUpdate=!0}_setValue_array(e,t){let i=this.resolvedProperty;for(let s=0,r=i.length;s!==r;++s)i[s]=e[t++]}_setValue_array_setNeedsUpdate(e,t){let i=this.resolvedProperty;for(let s=0,r=i.length;s!==r;++s)i[s]=e[t++];this.targetObject.needsUpdate=!0}_setValue_array_setMatrixWorldNeedsUpdate(e,t){let i=this.resolvedProperty;for(let s=0,r=i.length;s!==r;++s)i[s]=e[t++];this.targetObject.matrixWorldNeedsUpdate=!0}_setValue_arrayElement(e,t){this.resolvedProperty[this.propertyIndex]=e[t]}_setValue_arrayElement_setNeedsUpdate(e,t){this.resolvedProperty[this.propertyIndex]=e[t],this.targetObject.needsUpdate=!0}_setValue_arrayElement_setMatrixWorldNeedsUpdate(e,t){this.resolvedProperty[this.propertyIndex]=e[t],this.targetObject.matrixWorldNeedsUpdate=!0}_setValue_fromArray(e,t){this.resolvedProperty.fromArray(e,t)}_setValue_fromArray_setNeedsUpdate(e,t){this.resolvedProperty.fromArray(e,t),this.targetObject.needsUpdate=!0}_setValue_fromArray_setMatrixWorldNeedsUpdate(e,t){this.resolvedProperty.fromArray(e,t),this.targetObject.matrixWorldNeedsUpdate=!0}_getValue_unbound(e,t){this.bind(),this.getValue(e,t)}_setValue_unbound(e,t){this.bind(),this.setValue(e,t)}bind(){let e=this.node,t=this.parsedPath,i=t.objectName,s=t.propertyName,r=t.propertyIndex;if(e||(e=n.findNode(this.rootNode,t.nodeName),this.node=e),this.getValue=this._getValue_unavailable,this.setValue=this._setValue_unavailable,!e){console.warn("THREE.PropertyBinding: No target node found for track: "+this.path+".");return}if(i){let l=t.objectIndex;switch(i){case"materials":if(!e.material){console.error("THREE.PropertyBinding: Can not bind to material as node does not have a material.",this);return}if(!e.material.materials){console.error("THREE.PropertyBinding: Can not bind to material.materials as node.material does not have a materials array.",this);return}e=e.material.materials;break;case"bones":if(!e.skeleton){console.error("THREE.PropertyBinding: Can not bind to bones as node does not have a skeleton.",this);return}e=e.skeleton.bones;for(let h=0;h<e.length;h++)if(e[h].name===l){l=h;break}break;case"map":if("map"in e){e=e.map;break}if(!e.material){console.error("THREE.PropertyBinding: Can not bind to material as node does not have a material.",this);return}if(!e.material.map){console.error("THREE.PropertyBinding: Can not bind to material.map as node.material does not have a map.",this);return}e=e.material.map;break;default:if(e[i]===void 0){console.error("THREE.PropertyBinding: Can not bind to objectName of node undefined.",this);return}e=e[i]}if(l!==void 0){if(e[l]===void 0){console.error("THREE.PropertyBinding: Trying to bind to objectIndex of objectName, but is undefined.",this,e);return}e=e[l]}}let o=e[s];if(o===void 0){let l=t.nodeName;console.error("THREE.PropertyBinding: Trying to update property for track: "+l+"."+s+" but it wasn't found.",e);return}let a=this.Versioning.None;this.targetObject=e,e.isMaterial===!0?a=this.Versioning.NeedsUpdate:e.isObject3D===!0&&(a=this.Versioning.MatrixWorldNeedsUpdate);let c=this.BindingType.Direct;if(r!==void 0){if(s==="morphTargetInfluences"){if(!e.geometry){console.error("THREE.PropertyBinding: Can not bind to morphTargetInfluences because node does not have a geometry.",this);return}if(!e.geometry.morphAttributes){console.error("THREE.PropertyBinding: Can not bind to morphTargetInfluences because node does not have a geometry.morphAttributes.",this);return}e.morphTargetDictionary[r]!==void 0&&(r=e.morphTargetDictionary[r])}c=this.BindingType.ArrayElement,this.resolvedProperty=o,this.propertyIndex=r}else o.fromArray!==void 0&&o.toArray!==void 0?(c=this.BindingType.HasFromToArray,this.resolvedProperty=o):Array.isArray(o)?(c=this.BindingType.EntireArray,this.resolvedProperty=o):this.propertyName=s;this.getValue=this.GetterByBindingType[c],this.setValue=this.SetterByBindingTypeAndVersioning[c][a]}unbind(){this.node=null,this.getValue=this._getValue_unbound,this.setValue=this._setValue_unbound}};ot.Composite=bl;ot.prototype.BindingType={Direct:0,EntireArray:1,ArrayElement:2,HasFromToArray:3};ot.prototype.Versioning={None:0,NeedsUpdate:1,MatrixWorldNeedsUpdate:2};ot.prototype.GetterByBindingType=[ot.prototype._getValue_direct,ot.prototype._getValue_array,ot.prototype._getValue_arrayElement,ot.prototype._getValue_toArray];ot.prototype.SetterByBindingTypeAndVersioning=[[ot.prototype._setValue_direct,ot.prototype._setValue_direct_setNeedsUpdate,ot.prototype._setValue_direct_setMatrixWorldNeedsUpdate],[ot.prototype._setValue_array,ot.prototype._setValue_array_setNeedsUpdate,ot.prototype._setValue_array_setMatrixWorldNeedsUpdate],[ot.prototype._setValue_arrayElement,ot.prototype._setValue_arrayElement_setNeedsUpdate,ot.prototype._setValue_arrayElement_setMatrixWorldNeedsUpdate],[ot.prototype._setValue_fromArray,ot.prototype._setValue_fromArray_setNeedsUpdate,ot.prototype._setValue_fromArray_setMatrixWorldNeedsUpdate]];var D_=new Float32Array(1);var Dh=new dt,or=class{constructor(e,t,i=0,s=1/0){this.ray=new Hs(e,t),this.near=i,this.far=s,this.camera=null,this.layers=new us,this.params={Mesh:{},Line:{threshold:1},LOD:{},Points:{threshold:1},Sprite:{}}}set(e,t){this.ray.set(e,t)}setFromCamera(e,t){t.isPerspectiveCamera?(this.ray.origin.setFromMatrixPosition(t.matrixWorld),this.ray.direction.set(e.x,e.y,.5).unproject(t).sub(this.ray.origin).normalize(),this.camera=t):t.isOrthographicCamera?(this.ray.origin.set(e.x,e.y,(t.near+t.far)/(t.near-t.far)).unproject(t),this.ray.direction.set(0,0,-1).transformDirection(t.matrixWorld),this.camera=t):console.error("THREE.Raycaster: Unsupported camera type: "+t.type)}setFromXRController(e){return Dh.identity().extractRotation(e.matrixWorld),this.ray.origin.setFromMatrixPosition(e.matrixWorld),this.ray.direction.set(0,0,-1).applyMatrix4(Dh),this}intersectObject(e,t=!0,i=[]){return El(e,this,i,t),i.sort(Uh),i}intersectObjects(e,t=!0,i=[]){for(let s=0,r=e.length;s<r;s++)El(e[s],this,i,t);return i.sort(Uh),i}};function Uh(n,e){return n.distance-e.distance}function El(n,e,t,i){let s=!0;if(n.layers.test(e.layers)&&n.raycast(e,t)===!1&&(s=!1),s===!0&&i===!0){let r=n.children;for(let o=0,a=r.length;o<a;o++)El(r[o],e,t,!0)}}function $l(n,e,t,i){let s=Mf(i);switch(t){case Fl:return n*e;case kl:return n*e/s.components*s.byteLength;case ka:return n*e/s.components*s.byteLength;case Bl:return n*e*2/s.components*s.byteLength;case Ba:return n*e*2/s.components*s.byteLength;case Ol:return n*e*3/s.components*s.byteLength;case an:return n*e*4/s.components*s.byteLength;case za:return n*e*4/s.components*s.byteLength;case hr:case ur:return Math.floor((n+3)/4)*Math.floor((e+3)/4)*8;case dr:case fr:return Math.floor((n+3)/4)*Math.floor((e+3)/4)*16;case Ga:case Wa:return Math.max(n,16)*Math.max(e,8)/4;case Va:case Ha:return Math.max(n,8)*Math.max(e,8)/2;case Xa:case qa:return Math.floor((n+3)/4)*Math.floor((e+3)/4)*8;case $a:return Math.floor((n+3)/4)*Math.floor((e+3)/4)*16;case Ya:return Math.floor((n+3)/4)*Math.floor((e+3)/4)*16;case Za:return Math.floor((n+4)/5)*Math.floor((e+3)/4)*16;case Ja:return Math.floor((n+4)/5)*Math.floor((e+4)/5)*16;case Ka:return Math.floor((n+5)/6)*Math.floor((e+4)/5)*16;case ja:return Math.floor((n+5)/6)*Math.floor((e+5)/6)*16;case Qa:return Math.floor((n+7)/8)*Math.floor((e+4)/5)*16;case eo:return Math.floor((n+7)/8)*Math.floor((e+5)/6)*16;case to:return Math.floor((n+7)/8)*Math.floor((e+7)/8)*16;case no:return Math.floor((n+9)/10)*Math.floor((e+4)/5)*16;case io:return Math.floor((n+9)/10)*Math.floor((e+5)/6)*16;case so:return Math.floor((n+9)/10)*Math.floor((e+7)/8)*16;case ro:return Math.floor((n+9)/10)*Math.floor((e+9)/10)*16;case ao:return Math.floor((n+11)/12)*Math.floor((e+9)/10)*16;case oo:return Math.floor((n+11)/12)*Math.floor((e+11)/12)*16;case lo:case co:case ho:return Math.ceil(n/4)*Math.ceil(e/4)*16;case uo:case fo:return Math.ceil(n/4)*Math.ceil(e/4)*8;case po:case mo:return Math.ceil(n/4)*Math.ceil(e/4)*16}throw new Error(`Unable to determine texture byte length for ${t} format.`)}function Mf(n){switch(n){case mn:case Ll:return{byteLength:1,components:1};case _s:case Dl:case xs:return{byteLength:2,components:1};case Fa:case Oa:return{byteLength:2,components:4};case di:case Na:case Tn:return{byteLength:4,components:1};case Ul:case Nl:return{byteLength:4,components:3}}throw new Error(`Unknown texture type ${n}.`)}typeof __THREE_DEVTOOLS__<"u"&&__THREE_DEVTOOLS__.dispatchEvent(new CustomEvent("register",{detail:{revision:"180"}}));typeof window<"u"&&(window.__THREE__?console.warn("WARNING: Multiple instances of Three.js being imported."):window.__THREE__="180");/**
 * @license
 * Copyright 2010-2025 Three.js Authors
 * SPDX-License-Identifier: MIT
 */function Zu(){let n=null,e=!1,t=null,i=null;function s(r,o){t(r,o),i=n.requestAnimationFrame(s)}return{start:function(){e!==!0&&t!==null&&(i=n.requestAnimationFrame(s),e=!0)},stop:function(){n.cancelAnimationFrame(i),e=!1},setAnimationLoop:function(r){t=r},setContext:function(r){n=r}}}function bf(n){let e=new WeakMap;function t(a,c){let l=a.array,h=a.usage,u=l.byteLength,p=n.createBuffer();n.bindBuffer(c,p),n.bufferData(c,l,h),a.onUploadCallback();let d;if(l instanceof Float32Array)d=n.FLOAT;else if(typeof Float16Array<"u"&&l instanceof Float16Array)d=n.HALF_FLOAT;else if(l instanceof Uint16Array)a.isFloat16BufferAttribute?d=n.HALF_FLOAT:d=n.UNSIGNED_SHORT;else if(l instanceof Int16Array)d=n.SHORT;else if(l instanceof Uint32Array)d=n.UNSIGNED_INT;else if(l instanceof Int32Array)d=n.INT;else if(l instanceof Int8Array)d=n.BYTE;else if(l instanceof Uint8Array)d=n.UNSIGNED_BYTE;else if(l instanceof Uint8ClampedArray)d=n.UNSIGNED_BYTE;else throw new Error("THREE.WebGLAttributes: Unsupported buffer data format: "+l);return{buffer:p,type:d,bytesPerElement:l.BYTES_PER_ELEMENT,version:a.version,size:u}}function i(a,c,l){let h=c.array,u=c.updateRanges;if(n.bindBuffer(l,a),u.length===0)n.bufferSubData(l,0,h);else{u.sort((d,g)=>d.start-g.start);let p=0;for(let d=1;d<u.length;d++){let g=u[p],v=u[d];v.start<=g.start+g.count+1?g.count=Math.max(g.count,v.start+v.count-g.start):(++p,u[p]=v)}u.length=p+1;for(let d=0,g=u.length;d<g;d++){let v=u[d];n.bufferSubData(l,v.start*h.BYTES_PER_ELEMENT,h,v.start,v.count)}c.clearUpdateRanges()}c.onUploadCallback()}function s(a){return a.isInterleavedBufferAttribute&&(a=a.data),e.get(a)}function r(a){a.isInterleavedBufferAttribute&&(a=a.data);let c=e.get(a);c&&(n.deleteBuffer(c.buffer),e.delete(a))}function o(a,c){if(a.isInterleavedBufferAttribute&&(a=a.data),a.isGLBufferAttribute){let h=e.get(a);(!h||h.version<a.version)&&e.set(a,{buffer:a.buffer,type:a.type,bytesPerElement:a.elementSize,version:a.version});return}let l=e.get(a);if(l===void 0)e.set(a,t(a,c));else if(l.version<a.version){if(l.size!==a.array.byteLength)throw new Error("THREE.WebGLAttributes: The size of the buffer attribute's array buffer does not match the original size. Resizing buffer attributes is not supported.");i(l.buffer,a,c),l.version=a.version}}return{get:s,remove:r,update:o}}var Ef=`#ifdef USE_ALPHAHASH
	if ( diffuseColor.a < getAlphaHashThreshold( vPosition ) ) discard;
#endif`,Tf=`#ifdef USE_ALPHAHASH
	const float ALPHA_HASH_SCALE = 0.05;
	float hash2D( vec2 value ) {
		return fract( 1.0e4 * sin( 17.0 * value.x + 0.1 * value.y ) * ( 0.1 + abs( sin( 13.0 * value.y + value.x ) ) ) );
	}
	float hash3D( vec3 value ) {
		return hash2D( vec2( hash2D( value.xy ), value.z ) );
	}
	float getAlphaHashThreshold( vec3 position ) {
		float maxDeriv = max(
			length( dFdx( position.xyz ) ),
			length( dFdy( position.xyz ) )
		);
		float pixScale = 1.0 / ( ALPHA_HASH_SCALE * maxDeriv );
		vec2 pixScales = vec2(
			exp2( floor( log2( pixScale ) ) ),
			exp2( ceil( log2( pixScale ) ) )
		);
		vec2 alpha = vec2(
			hash3D( floor( pixScales.x * position.xyz ) ),
			hash3D( floor( pixScales.y * position.xyz ) )
		);
		float lerpFactor = fract( log2( pixScale ) );
		float x = ( 1.0 - lerpFactor ) * alpha.x + lerpFactor * alpha.y;
		float a = min( lerpFactor, 1.0 - lerpFactor );
		vec3 cases = vec3(
			x * x / ( 2.0 * a * ( 1.0 - a ) ),
			( x - 0.5 * a ) / ( 1.0 - a ),
			1.0 - ( ( 1.0 - x ) * ( 1.0 - x ) / ( 2.0 * a * ( 1.0 - a ) ) )
		);
		float threshold = ( x < ( 1.0 - a ) )
			? ( ( x < a ) ? cases.x : cases.y )
			: cases.z;
		return clamp( threshold , 1.0e-6, 1.0 );
	}
#endif`,wf=`#ifdef USE_ALPHAMAP
	diffuseColor.a *= texture2D( alphaMap, vAlphaMapUv ).g;
#endif`,Af=`#ifdef USE_ALPHAMAP
	uniform sampler2D alphaMap;
#endif`,Rf=`#ifdef USE_ALPHATEST
	#ifdef ALPHA_TO_COVERAGE
	diffuseColor.a = smoothstep( alphaTest, alphaTest + fwidth( diffuseColor.a ), diffuseColor.a );
	if ( diffuseColor.a == 0.0 ) discard;
	#else
	if ( diffuseColor.a < alphaTest ) discard;
	#endif
#endif`,Cf=`#ifdef USE_ALPHATEST
	uniform float alphaTest;
#endif`,If=`#ifdef USE_AOMAP
	float ambientOcclusion = ( texture2D( aoMap, vAoMapUv ).r - 1.0 ) * aoMapIntensity + 1.0;
	reflectedLight.indirectDiffuse *= ambientOcclusion;
	#if defined( USE_CLEARCOAT ) 
		clearcoatSpecularIndirect *= ambientOcclusion;
	#endif
	#if defined( USE_SHEEN ) 
		sheenSpecularIndirect *= ambientOcclusion;
	#endif
	#if defined( USE_ENVMAP ) && defined( STANDARD )
		float dotNV = saturate( dot( geometryNormal, geometryViewDir ) );
		reflectedLight.indirectSpecular *= computeSpecularOcclusion( dotNV, ambientOcclusion, material.roughness );
	#endif
#endif`,Pf=`#ifdef USE_AOMAP
	uniform sampler2D aoMap;
	uniform float aoMapIntensity;
#endif`,Lf=`#ifdef USE_BATCHING
	#if ! defined( GL_ANGLE_multi_draw )
	#define gl_DrawID _gl_DrawID
	uniform int _gl_DrawID;
	#endif
	uniform highp sampler2D batchingTexture;
	uniform highp usampler2D batchingIdTexture;
	mat4 getBatchingMatrix( const in float i ) {
		int size = textureSize( batchingTexture, 0 ).x;
		int j = int( i ) * 4;
		int x = j % size;
		int y = j / size;
		vec4 v1 = texelFetch( batchingTexture, ivec2( x, y ), 0 );
		vec4 v2 = texelFetch( batchingTexture, ivec2( x + 1, y ), 0 );
		vec4 v3 = texelFetch( batchingTexture, ivec2( x + 2, y ), 0 );
		vec4 v4 = texelFetch( batchingTexture, ivec2( x + 3, y ), 0 );
		return mat4( v1, v2, v3, v4 );
	}
	float getIndirectIndex( const in int i ) {
		int size = textureSize( batchingIdTexture, 0 ).x;
		int x = i % size;
		int y = i / size;
		return float( texelFetch( batchingIdTexture, ivec2( x, y ), 0 ).r );
	}
#endif
#ifdef USE_BATCHING_COLOR
	uniform sampler2D batchingColorTexture;
	vec3 getBatchingColor( const in float i ) {
		int size = textureSize( batchingColorTexture, 0 ).x;
		int j = int( i );
		int x = j % size;
		int y = j / size;
		return texelFetch( batchingColorTexture, ivec2( x, y ), 0 ).rgb;
	}
#endif`,Df=`#ifdef USE_BATCHING
	mat4 batchingMatrix = getBatchingMatrix( getIndirectIndex( gl_DrawID ) );
#endif`,Uf=`vec3 transformed = vec3( position );
#ifdef USE_ALPHAHASH
	vPosition = vec3( position );
#endif`,Nf=`vec3 objectNormal = vec3( normal );
#ifdef USE_TANGENT
	vec3 objectTangent = vec3( tangent.xyz );
#endif`,Ff=`float G_BlinnPhong_Implicit( ) {
	return 0.25;
}
float D_BlinnPhong( const in float shininess, const in float dotNH ) {
	return RECIPROCAL_PI * ( shininess * 0.5 + 1.0 ) * pow( dotNH, shininess );
}
vec3 BRDF_BlinnPhong( const in vec3 lightDir, const in vec3 viewDir, const in vec3 normal, const in vec3 specularColor, const in float shininess ) {
	vec3 halfDir = normalize( lightDir + viewDir );
	float dotNH = saturate( dot( normal, halfDir ) );
	float dotVH = saturate( dot( viewDir, halfDir ) );
	vec3 F = F_Schlick( specularColor, 1.0, dotVH );
	float G = G_BlinnPhong_Implicit( );
	float D = D_BlinnPhong( shininess, dotNH );
	return F * ( G * D );
} // validated`,Of=`#ifdef USE_IRIDESCENCE
	const mat3 XYZ_TO_REC709 = mat3(
		 3.2404542, -0.9692660,  0.0556434,
		-1.5371385,  1.8760108, -0.2040259,
		-0.4985314,  0.0415560,  1.0572252
	);
	vec3 Fresnel0ToIor( vec3 fresnel0 ) {
		vec3 sqrtF0 = sqrt( fresnel0 );
		return ( vec3( 1.0 ) + sqrtF0 ) / ( vec3( 1.0 ) - sqrtF0 );
	}
	vec3 IorToFresnel0( vec3 transmittedIor, float incidentIor ) {
		return pow2( ( transmittedIor - vec3( incidentIor ) ) / ( transmittedIor + vec3( incidentIor ) ) );
	}
	float IorToFresnel0( float transmittedIor, float incidentIor ) {
		return pow2( ( transmittedIor - incidentIor ) / ( transmittedIor + incidentIor ));
	}
	vec3 evalSensitivity( float OPD, vec3 shift ) {
		float phase = 2.0 * PI * OPD * 1.0e-9;
		vec3 val = vec3( 5.4856e-13, 4.4201e-13, 5.2481e-13 );
		vec3 pos = vec3( 1.6810e+06, 1.7953e+06, 2.2084e+06 );
		vec3 var = vec3( 4.3278e+09, 9.3046e+09, 6.6121e+09 );
		vec3 xyz = val * sqrt( 2.0 * PI * var ) * cos( pos * phase + shift ) * exp( - pow2( phase ) * var );
		xyz.x += 9.7470e-14 * sqrt( 2.0 * PI * 4.5282e+09 ) * cos( 2.2399e+06 * phase + shift[ 0 ] ) * exp( - 4.5282e+09 * pow2( phase ) );
		xyz /= 1.0685e-7;
		vec3 rgb = XYZ_TO_REC709 * xyz;
		return rgb;
	}
	vec3 evalIridescence( float outsideIOR, float eta2, float cosTheta1, float thinFilmThickness, vec3 baseF0 ) {
		vec3 I;
		float iridescenceIOR = mix( outsideIOR, eta2, smoothstep( 0.0, 0.03, thinFilmThickness ) );
		float sinTheta2Sq = pow2( outsideIOR / iridescenceIOR ) * ( 1.0 - pow2( cosTheta1 ) );
		float cosTheta2Sq = 1.0 - sinTheta2Sq;
		if ( cosTheta2Sq < 0.0 ) {
			return vec3( 1.0 );
		}
		float cosTheta2 = sqrt( cosTheta2Sq );
		float R0 = IorToFresnel0( iridescenceIOR, outsideIOR );
		float R12 = F_Schlick( R0, 1.0, cosTheta1 );
		float T121 = 1.0 - R12;
		float phi12 = 0.0;
		if ( iridescenceIOR < outsideIOR ) phi12 = PI;
		float phi21 = PI - phi12;
		vec3 baseIOR = Fresnel0ToIor( clamp( baseF0, 0.0, 0.9999 ) );		vec3 R1 = IorToFresnel0( baseIOR, iridescenceIOR );
		vec3 R23 = F_Schlick( R1, 1.0, cosTheta2 );
		vec3 phi23 = vec3( 0.0 );
		if ( baseIOR[ 0 ] < iridescenceIOR ) phi23[ 0 ] = PI;
		if ( baseIOR[ 1 ] < iridescenceIOR ) phi23[ 1 ] = PI;
		if ( baseIOR[ 2 ] < iridescenceIOR ) phi23[ 2 ] = PI;
		float OPD = 2.0 * iridescenceIOR * thinFilmThickness * cosTheta2;
		vec3 phi = vec3( phi21 ) + phi23;
		vec3 R123 = clamp( R12 * R23, 1e-5, 0.9999 );
		vec3 r123 = sqrt( R123 );
		vec3 Rs = pow2( T121 ) * R23 / ( vec3( 1.0 ) - R123 );
		vec3 C0 = R12 + Rs;
		I = C0;
		vec3 Cm = Rs - T121;
		for ( int m = 1; m <= 2; ++ m ) {
			Cm *= r123;
			vec3 Sm = 2.0 * evalSensitivity( float( m ) * OPD, float( m ) * phi );
			I += Cm * Sm;
		}
		return max( I, vec3( 0.0 ) );
	}
#endif`,kf=`#ifdef USE_BUMPMAP
	uniform sampler2D bumpMap;
	uniform float bumpScale;
	vec2 dHdxy_fwd() {
		vec2 dSTdx = dFdx( vBumpMapUv );
		vec2 dSTdy = dFdy( vBumpMapUv );
		float Hll = bumpScale * texture2D( bumpMap, vBumpMapUv ).x;
		float dBx = bumpScale * texture2D( bumpMap, vBumpMapUv + dSTdx ).x - Hll;
		float dBy = bumpScale * texture2D( bumpMap, vBumpMapUv + dSTdy ).x - Hll;
		return vec2( dBx, dBy );
	}
	vec3 perturbNormalArb( vec3 surf_pos, vec3 surf_norm, vec2 dHdxy, float faceDirection ) {
		vec3 vSigmaX = normalize( dFdx( surf_pos.xyz ) );
		vec3 vSigmaY = normalize( dFdy( surf_pos.xyz ) );
		vec3 vN = surf_norm;
		vec3 R1 = cross( vSigmaY, vN );
		vec3 R2 = cross( vN, vSigmaX );
		float fDet = dot( vSigmaX, R1 ) * faceDirection;
		vec3 vGrad = sign( fDet ) * ( dHdxy.x * R1 + dHdxy.y * R2 );
		return normalize( abs( fDet ) * surf_norm - vGrad );
	}
#endif`,Bf=`#if NUM_CLIPPING_PLANES > 0
	vec4 plane;
	#ifdef ALPHA_TO_COVERAGE
		float distanceToPlane, distanceGradient;
		float clipOpacity = 1.0;
		#pragma unroll_loop_start
		for ( int i = 0; i < UNION_CLIPPING_PLANES; i ++ ) {
			plane = clippingPlanes[ i ];
			distanceToPlane = - dot( vClipPosition, plane.xyz ) + plane.w;
			distanceGradient = fwidth( distanceToPlane ) / 2.0;
			clipOpacity *= smoothstep( - distanceGradient, distanceGradient, distanceToPlane );
			if ( clipOpacity == 0.0 ) discard;
		}
		#pragma unroll_loop_end
		#if UNION_CLIPPING_PLANES < NUM_CLIPPING_PLANES
			float unionClipOpacity = 1.0;
			#pragma unroll_loop_start
			for ( int i = UNION_CLIPPING_PLANES; i < NUM_CLIPPING_PLANES; i ++ ) {
				plane = clippingPlanes[ i ];
				distanceToPlane = - dot( vClipPosition, plane.xyz ) + plane.w;
				distanceGradient = fwidth( distanceToPlane ) / 2.0;
				unionClipOpacity *= 1.0 - smoothstep( - distanceGradient, distanceGradient, distanceToPlane );
			}
			#pragma unroll_loop_end
			clipOpacity *= 1.0 - unionClipOpacity;
		#endif
		diffuseColor.a *= clipOpacity;
		if ( diffuseColor.a == 0.0 ) discard;
	#else
		#pragma unroll_loop_start
		for ( int i = 0; i < UNION_CLIPPING_PLANES; i ++ ) {
			plane = clippingPlanes[ i ];
			if ( dot( vClipPosition, plane.xyz ) > plane.w ) discard;
		}
		#pragma unroll_loop_end
		#if UNION_CLIPPING_PLANES < NUM_CLIPPING_PLANES
			bool clipped = true;
			#pragma unroll_loop_start
			for ( int i = UNION_CLIPPING_PLANES; i < NUM_CLIPPING_PLANES; i ++ ) {
				plane = clippingPlanes[ i ];
				clipped = ( dot( vClipPosition, plane.xyz ) > plane.w ) && clipped;
			}
			#pragma unroll_loop_end
			if ( clipped ) discard;
		#endif
	#endif
#endif`,zf=`#if NUM_CLIPPING_PLANES > 0
	varying vec3 vClipPosition;
	uniform vec4 clippingPlanes[ NUM_CLIPPING_PLANES ];
#endif`,Vf=`#if NUM_CLIPPING_PLANES > 0
	varying vec3 vClipPosition;
#endif`,Gf=`#if NUM_CLIPPING_PLANES > 0
	vClipPosition = - mvPosition.xyz;
#endif`,Hf=`#if defined( USE_COLOR_ALPHA )
	diffuseColor *= vColor;
#elif defined( USE_COLOR )
	diffuseColor.rgb *= vColor;
#endif`,Wf=`#if defined( USE_COLOR_ALPHA )
	varying vec4 vColor;
#elif defined( USE_COLOR )
	varying vec3 vColor;
#endif`,Xf=`#if defined( USE_COLOR_ALPHA )
	varying vec4 vColor;
#elif defined( USE_COLOR ) || defined( USE_INSTANCING_COLOR ) || defined( USE_BATCHING_COLOR )
	varying vec3 vColor;
#endif`,qf=`#if defined( USE_COLOR_ALPHA )
	vColor = vec4( 1.0 );
#elif defined( USE_COLOR ) || defined( USE_INSTANCING_COLOR ) || defined( USE_BATCHING_COLOR )
	vColor = vec3( 1.0 );
#endif
#ifdef USE_COLOR
	vColor *= color;
#endif
#ifdef USE_INSTANCING_COLOR
	vColor.xyz *= instanceColor.xyz;
#endif
#ifdef USE_BATCHING_COLOR
	vec3 batchingColor = getBatchingColor( getIndirectIndex( gl_DrawID ) );
	vColor.xyz *= batchingColor.xyz;
#endif`,$f=`#define PI 3.141592653589793
#define PI2 6.283185307179586
#define PI_HALF 1.5707963267948966
#define RECIPROCAL_PI 0.3183098861837907
#define RECIPROCAL_PI2 0.15915494309189535
#define EPSILON 1e-6
#ifndef saturate
#define saturate( a ) clamp( a, 0.0, 1.0 )
#endif
#define whiteComplement( a ) ( 1.0 - saturate( a ) )
float pow2( const in float x ) { return x*x; }
vec3 pow2( const in vec3 x ) { return x*x; }
float pow3( const in float x ) { return x*x*x; }
float pow4( const in float x ) { float x2 = x*x; return x2*x2; }
float max3( const in vec3 v ) { return max( max( v.x, v.y ), v.z ); }
float average( const in vec3 v ) { return dot( v, vec3( 0.3333333 ) ); }
highp float rand( const in vec2 uv ) {
	const highp float a = 12.9898, b = 78.233, c = 43758.5453;
	highp float dt = dot( uv.xy, vec2( a,b ) ), sn = mod( dt, PI );
	return fract( sin( sn ) * c );
}
#ifdef HIGH_PRECISION
	float precisionSafeLength( vec3 v ) { return length( v ); }
#else
	float precisionSafeLength( vec3 v ) {
		float maxComponent = max3( abs( v ) );
		return length( v / maxComponent ) * maxComponent;
	}
#endif
struct IncidentLight {
	vec3 color;
	vec3 direction;
	bool visible;
};
struct ReflectedLight {
	vec3 directDiffuse;
	vec3 directSpecular;
	vec3 indirectDiffuse;
	vec3 indirectSpecular;
};
#ifdef USE_ALPHAHASH
	varying vec3 vPosition;
#endif
vec3 transformDirection( in vec3 dir, in mat4 matrix ) {
	return normalize( ( matrix * vec4( dir, 0.0 ) ).xyz );
}
vec3 inverseTransformDirection( in vec3 dir, in mat4 matrix ) {
	return normalize( ( vec4( dir, 0.0 ) * matrix ).xyz );
}
mat3 transposeMat3( const in mat3 m ) {
	mat3 tmp;
	tmp[ 0 ] = vec3( m[ 0 ].x, m[ 1 ].x, m[ 2 ].x );
	tmp[ 1 ] = vec3( m[ 0 ].y, m[ 1 ].y, m[ 2 ].y );
	tmp[ 2 ] = vec3( m[ 0 ].z, m[ 1 ].z, m[ 2 ].z );
	return tmp;
}
bool isPerspectiveMatrix( mat4 m ) {
	return m[ 2 ][ 3 ] == - 1.0;
}
vec2 equirectUv( in vec3 dir ) {
	float u = atan( dir.z, dir.x ) * RECIPROCAL_PI2 + 0.5;
	float v = asin( clamp( dir.y, - 1.0, 1.0 ) ) * RECIPROCAL_PI + 0.5;
	return vec2( u, v );
}
vec3 BRDF_Lambert( const in vec3 diffuseColor ) {
	return RECIPROCAL_PI * diffuseColor;
}
vec3 F_Schlick( const in vec3 f0, const in float f90, const in float dotVH ) {
	float fresnel = exp2( ( - 5.55473 * dotVH - 6.98316 ) * dotVH );
	return f0 * ( 1.0 - fresnel ) + ( f90 * fresnel );
}
float F_Schlick( const in float f0, const in float f90, const in float dotVH ) {
	float fresnel = exp2( ( - 5.55473 * dotVH - 6.98316 ) * dotVH );
	return f0 * ( 1.0 - fresnel ) + ( f90 * fresnel );
} // validated`,Yf=`#ifdef ENVMAP_TYPE_CUBE_UV
	#define cubeUV_minMipLevel 4.0
	#define cubeUV_minTileSize 16.0
	float getFace( vec3 direction ) {
		vec3 absDirection = abs( direction );
		float face = - 1.0;
		if ( absDirection.x > absDirection.z ) {
			if ( absDirection.x > absDirection.y )
				face = direction.x > 0.0 ? 0.0 : 3.0;
			else
				face = direction.y > 0.0 ? 1.0 : 4.0;
		} else {
			if ( absDirection.z > absDirection.y )
				face = direction.z > 0.0 ? 2.0 : 5.0;
			else
				face = direction.y > 0.0 ? 1.0 : 4.0;
		}
		return face;
	}
	vec2 getUV( vec3 direction, float face ) {
		vec2 uv;
		if ( face == 0.0 ) {
			uv = vec2( direction.z, direction.y ) / abs( direction.x );
		} else if ( face == 1.0 ) {
			uv = vec2( - direction.x, - direction.z ) / abs( direction.y );
		} else if ( face == 2.0 ) {
			uv = vec2( - direction.x, direction.y ) / abs( direction.z );
		} else if ( face == 3.0 ) {
			uv = vec2( - direction.z, direction.y ) / abs( direction.x );
		} else if ( face == 4.0 ) {
			uv = vec2( - direction.x, direction.z ) / abs( direction.y );
		} else {
			uv = vec2( direction.x, direction.y ) / abs( direction.z );
		}
		return 0.5 * ( uv + 1.0 );
	}
	vec3 bilinearCubeUV( sampler2D envMap, vec3 direction, float mipInt ) {
		float face = getFace( direction );
		float filterInt = max( cubeUV_minMipLevel - mipInt, 0.0 );
		mipInt = max( mipInt, cubeUV_minMipLevel );
		float faceSize = exp2( mipInt );
		highp vec2 uv = getUV( direction, face ) * ( faceSize - 2.0 ) + 1.0;
		if ( face > 2.0 ) {
			uv.y += faceSize;
			face -= 3.0;
		}
		uv.x += face * faceSize;
		uv.x += filterInt * 3.0 * cubeUV_minTileSize;
		uv.y += 4.0 * ( exp2( CUBEUV_MAX_MIP ) - faceSize );
		uv.x *= CUBEUV_TEXEL_WIDTH;
		uv.y *= CUBEUV_TEXEL_HEIGHT;
		#ifdef texture2DGradEXT
			return texture2DGradEXT( envMap, uv, vec2( 0.0 ), vec2( 0.0 ) ).rgb;
		#else
			return texture2D( envMap, uv ).rgb;
		#endif
	}
	#define cubeUV_r0 1.0
	#define cubeUV_m0 - 2.0
	#define cubeUV_r1 0.8
	#define cubeUV_m1 - 1.0
	#define cubeUV_r4 0.4
	#define cubeUV_m4 2.0
	#define cubeUV_r5 0.305
	#define cubeUV_m5 3.0
	#define cubeUV_r6 0.21
	#define cubeUV_m6 4.0
	float roughnessToMip( float roughness ) {
		float mip = 0.0;
		if ( roughness >= cubeUV_r1 ) {
			mip = ( cubeUV_r0 - roughness ) * ( cubeUV_m1 - cubeUV_m0 ) / ( cubeUV_r0 - cubeUV_r1 ) + cubeUV_m0;
		} else if ( roughness >= cubeUV_r4 ) {
			mip = ( cubeUV_r1 - roughness ) * ( cubeUV_m4 - cubeUV_m1 ) / ( cubeUV_r1 - cubeUV_r4 ) + cubeUV_m1;
		} else if ( roughness >= cubeUV_r5 ) {
			mip = ( cubeUV_r4 - roughness ) * ( cubeUV_m5 - cubeUV_m4 ) / ( cubeUV_r4 - cubeUV_r5 ) + cubeUV_m4;
		} else if ( roughness >= cubeUV_r6 ) {
			mip = ( cubeUV_r5 - roughness ) * ( cubeUV_m6 - cubeUV_m5 ) / ( cubeUV_r5 - cubeUV_r6 ) + cubeUV_m5;
		} else {
			mip = - 2.0 * log2( 1.16 * roughness );		}
		return mip;
	}
	vec4 textureCubeUV( sampler2D envMap, vec3 sampleDir, float roughness ) {
		float mip = clamp( roughnessToMip( roughness ), cubeUV_m0, CUBEUV_MAX_MIP );
		float mipF = fract( mip );
		float mipInt = floor( mip );
		vec3 color0 = bilinearCubeUV( envMap, sampleDir, mipInt );
		if ( mipF == 0.0 ) {
			return vec4( color0, 1.0 );
		} else {
			vec3 color1 = bilinearCubeUV( envMap, sampleDir, mipInt + 1.0 );
			return vec4( mix( color0, color1, mipF ), 1.0 );
		}
	}
#endif`,Zf=`vec3 transformedNormal = objectNormal;
#ifdef USE_TANGENT
	vec3 transformedTangent = objectTangent;
#endif
#ifdef USE_BATCHING
	mat3 bm = mat3( batchingMatrix );
	transformedNormal /= vec3( dot( bm[ 0 ], bm[ 0 ] ), dot( bm[ 1 ], bm[ 1 ] ), dot( bm[ 2 ], bm[ 2 ] ) );
	transformedNormal = bm * transformedNormal;
	#ifdef USE_TANGENT
		transformedTangent = bm * transformedTangent;
	#endif
#endif
#ifdef USE_INSTANCING
	mat3 im = mat3( instanceMatrix );
	transformedNormal /= vec3( dot( im[ 0 ], im[ 0 ] ), dot( im[ 1 ], im[ 1 ] ), dot( im[ 2 ], im[ 2 ] ) );
	transformedNormal = im * transformedNormal;
	#ifdef USE_TANGENT
		transformedTangent = im * transformedTangent;
	#endif
#endif
transformedNormal = normalMatrix * transformedNormal;
#ifdef FLIP_SIDED
	transformedNormal = - transformedNormal;
#endif
#ifdef USE_TANGENT
	transformedTangent = ( modelViewMatrix * vec4( transformedTangent, 0.0 ) ).xyz;
	#ifdef FLIP_SIDED
		transformedTangent = - transformedTangent;
	#endif
#endif`,Jf=`#ifdef USE_DISPLACEMENTMAP
	uniform sampler2D displacementMap;
	uniform float displacementScale;
	uniform float displacementBias;
#endif`,Kf=`#ifdef USE_DISPLACEMENTMAP
	transformed += normalize( objectNormal ) * ( texture2D( displacementMap, vDisplacementMapUv ).x * displacementScale + displacementBias );
#endif`,jf=`#ifdef USE_EMISSIVEMAP
	vec4 emissiveColor = texture2D( emissiveMap, vEmissiveMapUv );
	#ifdef DECODE_VIDEO_TEXTURE_EMISSIVE
		emissiveColor = sRGBTransferEOTF( emissiveColor );
	#endif
	totalEmissiveRadiance *= emissiveColor.rgb;
#endif`,Qf=`#ifdef USE_EMISSIVEMAP
	uniform sampler2D emissiveMap;
#endif`,ep="gl_FragColor = linearToOutputTexel( gl_FragColor );",tp=`vec4 LinearTransferOETF( in vec4 value ) {
	return value;
}
vec4 sRGBTransferEOTF( in vec4 value ) {
	return vec4( mix( pow( value.rgb * 0.9478672986 + vec3( 0.0521327014 ), vec3( 2.4 ) ), value.rgb * 0.0773993808, vec3( lessThanEqual( value.rgb, vec3( 0.04045 ) ) ) ), value.a );
}
vec4 sRGBTransferOETF( in vec4 value ) {
	return vec4( mix( pow( value.rgb, vec3( 0.41666 ) ) * 1.055 - vec3( 0.055 ), value.rgb * 12.92, vec3( lessThanEqual( value.rgb, vec3( 0.0031308 ) ) ) ), value.a );
}`,np=`#ifdef USE_ENVMAP
	#ifdef ENV_WORLDPOS
		vec3 cameraToFrag;
		if ( isOrthographic ) {
			cameraToFrag = normalize( vec3( - viewMatrix[ 0 ][ 2 ], - viewMatrix[ 1 ][ 2 ], - viewMatrix[ 2 ][ 2 ] ) );
		} else {
			cameraToFrag = normalize( vWorldPosition - cameraPosition );
		}
		vec3 worldNormal = inverseTransformDirection( normal, viewMatrix );
		#ifdef ENVMAP_MODE_REFLECTION
			vec3 reflectVec = reflect( cameraToFrag, worldNormal );
		#else
			vec3 reflectVec = refract( cameraToFrag, worldNormal, refractionRatio );
		#endif
	#else
		vec3 reflectVec = vReflect;
	#endif
	#ifdef ENVMAP_TYPE_CUBE
		vec4 envColor = textureCube( envMap, envMapRotation * vec3( flipEnvMap * reflectVec.x, reflectVec.yz ) );
	#else
		vec4 envColor = vec4( 0.0 );
	#endif
	#ifdef ENVMAP_BLENDING_MULTIPLY
		outgoingLight = mix( outgoingLight, outgoingLight * envColor.xyz, specularStrength * reflectivity );
	#elif defined( ENVMAP_BLENDING_MIX )
		outgoingLight = mix( outgoingLight, envColor.xyz, specularStrength * reflectivity );
	#elif defined( ENVMAP_BLENDING_ADD )
		outgoingLight += envColor.xyz * specularStrength * reflectivity;
	#endif
#endif`,ip=`#ifdef USE_ENVMAP
	uniform float envMapIntensity;
	uniform float flipEnvMap;
	uniform mat3 envMapRotation;
	#ifdef ENVMAP_TYPE_CUBE
		uniform samplerCube envMap;
	#else
		uniform sampler2D envMap;
	#endif
	
#endif`,sp=`#ifdef USE_ENVMAP
	uniform float reflectivity;
	#if defined( USE_BUMPMAP ) || defined( USE_NORMALMAP ) || defined( PHONG ) || defined( LAMBERT )
		#define ENV_WORLDPOS
	#endif
	#ifdef ENV_WORLDPOS
		varying vec3 vWorldPosition;
		uniform float refractionRatio;
	#else
		varying vec3 vReflect;
	#endif
#endif`,rp=`#ifdef USE_ENVMAP
	#if defined( USE_BUMPMAP ) || defined( USE_NORMALMAP ) || defined( PHONG ) || defined( LAMBERT )
		#define ENV_WORLDPOS
	#endif
	#ifdef ENV_WORLDPOS
		
		varying vec3 vWorldPosition;
	#else
		varying vec3 vReflect;
		uniform float refractionRatio;
	#endif
#endif`,ap=`#ifdef USE_ENVMAP
	#ifdef ENV_WORLDPOS
		vWorldPosition = worldPosition.xyz;
	#else
		vec3 cameraToVertex;
		if ( isOrthographic ) {
			cameraToVertex = normalize( vec3( - viewMatrix[ 0 ][ 2 ], - viewMatrix[ 1 ][ 2 ], - viewMatrix[ 2 ][ 2 ] ) );
		} else {
			cameraToVertex = normalize( worldPosition.xyz - cameraPosition );
		}
		vec3 worldNormal = inverseTransformDirection( transformedNormal, viewMatrix );
		#ifdef ENVMAP_MODE_REFLECTION
			vReflect = reflect( cameraToVertex, worldNormal );
		#else
			vReflect = refract( cameraToVertex, worldNormal, refractionRatio );
		#endif
	#endif
#endif`,op=`#ifdef USE_FOG
	vFogDepth = - mvPosition.z;
#endif`,lp=`#ifdef USE_FOG
	varying float vFogDepth;
#endif`,cp=`#ifdef USE_FOG
	#ifdef FOG_EXP2
		float fogFactor = 1.0 - exp( - fogDensity * fogDensity * vFogDepth * vFogDepth );
	#else
		float fogFactor = smoothstep( fogNear, fogFar, vFogDepth );
	#endif
	gl_FragColor.rgb = mix( gl_FragColor.rgb, fogColor, fogFactor );
#endif`,hp=`#ifdef USE_FOG
	uniform vec3 fogColor;
	varying float vFogDepth;
	#ifdef FOG_EXP2
		uniform float fogDensity;
	#else
		uniform float fogNear;
		uniform float fogFar;
	#endif
#endif`,up=`#ifdef USE_GRADIENTMAP
	uniform sampler2D gradientMap;
#endif
vec3 getGradientIrradiance( vec3 normal, vec3 lightDirection ) {
	float dotNL = dot( normal, lightDirection );
	vec2 coord = vec2( dotNL * 0.5 + 0.5, 0.0 );
	#ifdef USE_GRADIENTMAP
		return vec3( texture2D( gradientMap, coord ).r );
	#else
		vec2 fw = fwidth( coord ) * 0.5;
		return mix( vec3( 0.7 ), vec3( 1.0 ), smoothstep( 0.7 - fw.x, 0.7 + fw.x, coord.x ) );
	#endif
}`,dp=`#ifdef USE_LIGHTMAP
	uniform sampler2D lightMap;
	uniform float lightMapIntensity;
#endif`,fp=`LambertMaterial material;
material.diffuseColor = diffuseColor.rgb;
material.specularStrength = specularStrength;`,pp=`varying vec3 vViewPosition;
struct LambertMaterial {
	vec3 diffuseColor;
	float specularStrength;
};
void RE_Direct_Lambert( const in IncidentLight directLight, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in LambertMaterial material, inout ReflectedLight reflectedLight ) {
	float dotNL = saturate( dot( geometryNormal, directLight.direction ) );
	vec3 irradiance = dotNL * directLight.color;
	reflectedLight.directDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
}
void RE_IndirectDiffuse_Lambert( const in vec3 irradiance, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in LambertMaterial material, inout ReflectedLight reflectedLight ) {
	reflectedLight.indirectDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
}
#define RE_Direct				RE_Direct_Lambert
#define RE_IndirectDiffuse		RE_IndirectDiffuse_Lambert`,mp=`uniform bool receiveShadow;
uniform vec3 ambientLightColor;
#if defined( USE_LIGHT_PROBES )
	uniform vec3 lightProbe[ 9 ];
#endif
vec3 shGetIrradianceAt( in vec3 normal, in vec3 shCoefficients[ 9 ] ) {
	float x = normal.x, y = normal.y, z = normal.z;
	vec3 result = shCoefficients[ 0 ] * 0.886227;
	result += shCoefficients[ 1 ] * 2.0 * 0.511664 * y;
	result += shCoefficients[ 2 ] * 2.0 * 0.511664 * z;
	result += shCoefficients[ 3 ] * 2.0 * 0.511664 * x;
	result += shCoefficients[ 4 ] * 2.0 * 0.429043 * x * y;
	result += shCoefficients[ 5 ] * 2.0 * 0.429043 * y * z;
	result += shCoefficients[ 6 ] * ( 0.743125 * z * z - 0.247708 );
	result += shCoefficients[ 7 ] * 2.0 * 0.429043 * x * z;
	result += shCoefficients[ 8 ] * 0.429043 * ( x * x - y * y );
	return result;
}
vec3 getLightProbeIrradiance( const in vec3 lightProbe[ 9 ], const in vec3 normal ) {
	vec3 worldNormal = inverseTransformDirection( normal, viewMatrix );
	vec3 irradiance = shGetIrradianceAt( worldNormal, lightProbe );
	return irradiance;
}
vec3 getAmbientLightIrradiance( const in vec3 ambientLightColor ) {
	vec3 irradiance = ambientLightColor;
	return irradiance;
}
float getDistanceAttenuation( const in float lightDistance, const in float cutoffDistance, const in float decayExponent ) {
	float distanceFalloff = 1.0 / max( pow( lightDistance, decayExponent ), 0.01 );
	if ( cutoffDistance > 0.0 ) {
		distanceFalloff *= pow2( saturate( 1.0 - pow4( lightDistance / cutoffDistance ) ) );
	}
	return distanceFalloff;
}
float getSpotAttenuation( const in float coneCosine, const in float penumbraCosine, const in float angleCosine ) {
	return smoothstep( coneCosine, penumbraCosine, angleCosine );
}
#if NUM_DIR_LIGHTS > 0
	struct DirectionalLight {
		vec3 direction;
		vec3 color;
	};
	uniform DirectionalLight directionalLights[ NUM_DIR_LIGHTS ];
	void getDirectionalLightInfo( const in DirectionalLight directionalLight, out IncidentLight light ) {
		light.color = directionalLight.color;
		light.direction = directionalLight.direction;
		light.visible = true;
	}
#endif
#if NUM_POINT_LIGHTS > 0
	struct PointLight {
		vec3 position;
		vec3 color;
		float distance;
		float decay;
	};
	uniform PointLight pointLights[ NUM_POINT_LIGHTS ];
	void getPointLightInfo( const in PointLight pointLight, const in vec3 geometryPosition, out IncidentLight light ) {
		vec3 lVector = pointLight.position - geometryPosition;
		light.direction = normalize( lVector );
		float lightDistance = length( lVector );
		light.color = pointLight.color;
		light.color *= getDistanceAttenuation( lightDistance, pointLight.distance, pointLight.decay );
		light.visible = ( light.color != vec3( 0.0 ) );
	}
#endif
#if NUM_SPOT_LIGHTS > 0
	struct SpotLight {
		vec3 position;
		vec3 direction;
		vec3 color;
		float distance;
		float decay;
		float coneCos;
		float penumbraCos;
	};
	uniform SpotLight spotLights[ NUM_SPOT_LIGHTS ];
	void getSpotLightInfo( const in SpotLight spotLight, const in vec3 geometryPosition, out IncidentLight light ) {
		vec3 lVector = spotLight.position - geometryPosition;
		light.direction = normalize( lVector );
		float angleCos = dot( light.direction, spotLight.direction );
		float spotAttenuation = getSpotAttenuation( spotLight.coneCos, spotLight.penumbraCos, angleCos );
		if ( spotAttenuation > 0.0 ) {
			float lightDistance = length( lVector );
			light.color = spotLight.color * spotAttenuation;
			light.color *= getDistanceAttenuation( lightDistance, spotLight.distance, spotLight.decay );
			light.visible = ( light.color != vec3( 0.0 ) );
		} else {
			light.color = vec3( 0.0 );
			light.visible = false;
		}
	}
#endif
#if NUM_RECT_AREA_LIGHTS > 0
	struct RectAreaLight {
		vec3 color;
		vec3 position;
		vec3 halfWidth;
		vec3 halfHeight;
	};
	uniform sampler2D ltc_1;	uniform sampler2D ltc_2;
	uniform RectAreaLight rectAreaLights[ NUM_RECT_AREA_LIGHTS ];
#endif
#if NUM_HEMI_LIGHTS > 0
	struct HemisphereLight {
		vec3 direction;
		vec3 skyColor;
		vec3 groundColor;
	};
	uniform HemisphereLight hemisphereLights[ NUM_HEMI_LIGHTS ];
	vec3 getHemisphereLightIrradiance( const in HemisphereLight hemiLight, const in vec3 normal ) {
		float dotNL = dot( normal, hemiLight.direction );
		float hemiDiffuseWeight = 0.5 * dotNL + 0.5;
		vec3 irradiance = mix( hemiLight.groundColor, hemiLight.skyColor, hemiDiffuseWeight );
		return irradiance;
	}
#endif`,gp=`#ifdef USE_ENVMAP
	vec3 getIBLIrradiance( const in vec3 normal ) {
		#ifdef ENVMAP_TYPE_CUBE_UV
			vec3 worldNormal = inverseTransformDirection( normal, viewMatrix );
			vec4 envMapColor = textureCubeUV( envMap, envMapRotation * worldNormal, 1.0 );
			return PI * envMapColor.rgb * envMapIntensity;
		#else
			return vec3( 0.0 );
		#endif
	}
	vec3 getIBLRadiance( const in vec3 viewDir, const in vec3 normal, const in float roughness ) {
		#ifdef ENVMAP_TYPE_CUBE_UV
			vec3 reflectVec = reflect( - viewDir, normal );
			reflectVec = normalize( mix( reflectVec, normal, roughness * roughness) );
			reflectVec = inverseTransformDirection( reflectVec, viewMatrix );
			vec4 envMapColor = textureCubeUV( envMap, envMapRotation * reflectVec, roughness );
			return envMapColor.rgb * envMapIntensity;
		#else
			return vec3( 0.0 );
		#endif
	}
	#ifdef USE_ANISOTROPY
		vec3 getIBLAnisotropyRadiance( const in vec3 viewDir, const in vec3 normal, const in float roughness, const in vec3 bitangent, const in float anisotropy ) {
			#ifdef ENVMAP_TYPE_CUBE_UV
				vec3 bentNormal = cross( bitangent, viewDir );
				bentNormal = normalize( cross( bentNormal, bitangent ) );
				bentNormal = normalize( mix( bentNormal, normal, pow2( pow2( 1.0 - anisotropy * ( 1.0 - roughness ) ) ) ) );
				return getIBLRadiance( viewDir, bentNormal, roughness );
			#else
				return vec3( 0.0 );
			#endif
		}
	#endif
#endif`,_p=`ToonMaterial material;
material.diffuseColor = diffuseColor.rgb;`,xp=`varying vec3 vViewPosition;
struct ToonMaterial {
	vec3 diffuseColor;
};
void RE_Direct_Toon( const in IncidentLight directLight, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in ToonMaterial material, inout ReflectedLight reflectedLight ) {
	vec3 irradiance = getGradientIrradiance( geometryNormal, directLight.direction ) * directLight.color;
	reflectedLight.directDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
}
void RE_IndirectDiffuse_Toon( const in vec3 irradiance, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in ToonMaterial material, inout ReflectedLight reflectedLight ) {
	reflectedLight.indirectDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
}
#define RE_Direct				RE_Direct_Toon
#define RE_IndirectDiffuse		RE_IndirectDiffuse_Toon`,vp=`BlinnPhongMaterial material;
material.diffuseColor = diffuseColor.rgb;
material.specularColor = specular;
material.specularShininess = shininess;
material.specularStrength = specularStrength;`,yp=`varying vec3 vViewPosition;
struct BlinnPhongMaterial {
	vec3 diffuseColor;
	vec3 specularColor;
	float specularShininess;
	float specularStrength;
};
void RE_Direct_BlinnPhong( const in IncidentLight directLight, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in BlinnPhongMaterial material, inout ReflectedLight reflectedLight ) {
	float dotNL = saturate( dot( geometryNormal, directLight.direction ) );
	vec3 irradiance = dotNL * directLight.color;
	reflectedLight.directDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
	reflectedLight.directSpecular += irradiance * BRDF_BlinnPhong( directLight.direction, geometryViewDir, geometryNormal, material.specularColor, material.specularShininess ) * material.specularStrength;
}
void RE_IndirectDiffuse_BlinnPhong( const in vec3 irradiance, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in BlinnPhongMaterial material, inout ReflectedLight reflectedLight ) {
	reflectedLight.indirectDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
}
#define RE_Direct				RE_Direct_BlinnPhong
#define RE_IndirectDiffuse		RE_IndirectDiffuse_BlinnPhong`,Mp=`PhysicalMaterial material;
material.diffuseColor = diffuseColor.rgb * ( 1.0 - metalnessFactor );
vec3 dxy = max( abs( dFdx( nonPerturbedNormal ) ), abs( dFdy( nonPerturbedNormal ) ) );
float geometryRoughness = max( max( dxy.x, dxy.y ), dxy.z );
material.roughness = max( roughnessFactor, 0.0525 );material.roughness += geometryRoughness;
material.roughness = min( material.roughness, 1.0 );
#ifdef IOR
	material.ior = ior;
	#ifdef USE_SPECULAR
		float specularIntensityFactor = specularIntensity;
		vec3 specularColorFactor = specularColor;
		#ifdef USE_SPECULAR_COLORMAP
			specularColorFactor *= texture2D( specularColorMap, vSpecularColorMapUv ).rgb;
		#endif
		#ifdef USE_SPECULAR_INTENSITYMAP
			specularIntensityFactor *= texture2D( specularIntensityMap, vSpecularIntensityMapUv ).a;
		#endif
		material.specularF90 = mix( specularIntensityFactor, 1.0, metalnessFactor );
	#else
		float specularIntensityFactor = 1.0;
		vec3 specularColorFactor = vec3( 1.0 );
		material.specularF90 = 1.0;
	#endif
	material.specularColor = mix( min( pow2( ( material.ior - 1.0 ) / ( material.ior + 1.0 ) ) * specularColorFactor, vec3( 1.0 ) ) * specularIntensityFactor, diffuseColor.rgb, metalnessFactor );
#else
	material.specularColor = mix( vec3( 0.04 ), diffuseColor.rgb, metalnessFactor );
	material.specularF90 = 1.0;
#endif
#ifdef USE_CLEARCOAT
	material.clearcoat = clearcoat;
	material.clearcoatRoughness = clearcoatRoughness;
	material.clearcoatF0 = vec3( 0.04 );
	material.clearcoatF90 = 1.0;
	#ifdef USE_CLEARCOATMAP
		material.clearcoat *= texture2D( clearcoatMap, vClearcoatMapUv ).x;
	#endif
	#ifdef USE_CLEARCOAT_ROUGHNESSMAP
		material.clearcoatRoughness *= texture2D( clearcoatRoughnessMap, vClearcoatRoughnessMapUv ).y;
	#endif
	material.clearcoat = saturate( material.clearcoat );	material.clearcoatRoughness = max( material.clearcoatRoughness, 0.0525 );
	material.clearcoatRoughness += geometryRoughness;
	material.clearcoatRoughness = min( material.clearcoatRoughness, 1.0 );
#endif
#ifdef USE_DISPERSION
	material.dispersion = dispersion;
#endif
#ifdef USE_IRIDESCENCE
	material.iridescence = iridescence;
	material.iridescenceIOR = iridescenceIOR;
	#ifdef USE_IRIDESCENCEMAP
		material.iridescence *= texture2D( iridescenceMap, vIridescenceMapUv ).r;
	#endif
	#ifdef USE_IRIDESCENCE_THICKNESSMAP
		material.iridescenceThickness = (iridescenceThicknessMaximum - iridescenceThicknessMinimum) * texture2D( iridescenceThicknessMap, vIridescenceThicknessMapUv ).g + iridescenceThicknessMinimum;
	#else
		material.iridescenceThickness = iridescenceThicknessMaximum;
	#endif
#endif
#ifdef USE_SHEEN
	material.sheenColor = sheenColor;
	#ifdef USE_SHEEN_COLORMAP
		material.sheenColor *= texture2D( sheenColorMap, vSheenColorMapUv ).rgb;
	#endif
	material.sheenRoughness = clamp( sheenRoughness, 0.07, 1.0 );
	#ifdef USE_SHEEN_ROUGHNESSMAP
		material.sheenRoughness *= texture2D( sheenRoughnessMap, vSheenRoughnessMapUv ).a;
	#endif
#endif
#ifdef USE_ANISOTROPY
	#ifdef USE_ANISOTROPYMAP
		mat2 anisotropyMat = mat2( anisotropyVector.x, anisotropyVector.y, - anisotropyVector.y, anisotropyVector.x );
		vec3 anisotropyPolar = texture2D( anisotropyMap, vAnisotropyMapUv ).rgb;
		vec2 anisotropyV = anisotropyMat * normalize( 2.0 * anisotropyPolar.rg - vec2( 1.0 ) ) * anisotropyPolar.b;
	#else
		vec2 anisotropyV = anisotropyVector;
	#endif
	material.anisotropy = length( anisotropyV );
	if( material.anisotropy == 0.0 ) {
		anisotropyV = vec2( 1.0, 0.0 );
	} else {
		anisotropyV /= material.anisotropy;
		material.anisotropy = saturate( material.anisotropy );
	}
	material.alphaT = mix( pow2( material.roughness ), 1.0, pow2( material.anisotropy ) );
	material.anisotropyT = tbn[ 0 ] * anisotropyV.x + tbn[ 1 ] * anisotropyV.y;
	material.anisotropyB = tbn[ 1 ] * anisotropyV.x - tbn[ 0 ] * anisotropyV.y;
#endif`,Sp=`struct PhysicalMaterial {
	vec3 diffuseColor;
	float roughness;
	vec3 specularColor;
	float specularF90;
	float dispersion;
	#ifdef USE_CLEARCOAT
		float clearcoat;
		float clearcoatRoughness;
		vec3 clearcoatF0;
		float clearcoatF90;
	#endif
	#ifdef USE_IRIDESCENCE
		float iridescence;
		float iridescenceIOR;
		float iridescenceThickness;
		vec3 iridescenceFresnel;
		vec3 iridescenceF0;
	#endif
	#ifdef USE_SHEEN
		vec3 sheenColor;
		float sheenRoughness;
	#endif
	#ifdef IOR
		float ior;
	#endif
	#ifdef USE_TRANSMISSION
		float transmission;
		float transmissionAlpha;
		float thickness;
		float attenuationDistance;
		vec3 attenuationColor;
	#endif
	#ifdef USE_ANISOTROPY
		float anisotropy;
		float alphaT;
		vec3 anisotropyT;
		vec3 anisotropyB;
	#endif
};
vec3 clearcoatSpecularDirect = vec3( 0.0 );
vec3 clearcoatSpecularIndirect = vec3( 0.0 );
vec3 sheenSpecularDirect = vec3( 0.0 );
vec3 sheenSpecularIndirect = vec3(0.0 );
vec3 Schlick_to_F0( const in vec3 f, const in float f90, const in float dotVH ) {
    float x = clamp( 1.0 - dotVH, 0.0, 1.0 );
    float x2 = x * x;
    float x5 = clamp( x * x2 * x2, 0.0, 0.9999 );
    return ( f - vec3( f90 ) * x5 ) / ( 1.0 - x5 );
}
float V_GGX_SmithCorrelated( const in float alpha, const in float dotNL, const in float dotNV ) {
	float a2 = pow2( alpha );
	float gv = dotNL * sqrt( a2 + ( 1.0 - a2 ) * pow2( dotNV ) );
	float gl = dotNV * sqrt( a2 + ( 1.0 - a2 ) * pow2( dotNL ) );
	return 0.5 / max( gv + gl, EPSILON );
}
float D_GGX( const in float alpha, const in float dotNH ) {
	float a2 = pow2( alpha );
	float denom = pow2( dotNH ) * ( a2 - 1.0 ) + 1.0;
	return RECIPROCAL_PI * a2 / pow2( denom );
}
#ifdef USE_ANISOTROPY
	float V_GGX_SmithCorrelated_Anisotropic( const in float alphaT, const in float alphaB, const in float dotTV, const in float dotBV, const in float dotTL, const in float dotBL, const in float dotNV, const in float dotNL ) {
		float gv = dotNL * length( vec3( alphaT * dotTV, alphaB * dotBV, dotNV ) );
		float gl = dotNV * length( vec3( alphaT * dotTL, alphaB * dotBL, dotNL ) );
		float v = 0.5 / ( gv + gl );
		return saturate(v);
	}
	float D_GGX_Anisotropic( const in float alphaT, const in float alphaB, const in float dotNH, const in float dotTH, const in float dotBH ) {
		float a2 = alphaT * alphaB;
		highp vec3 v = vec3( alphaB * dotTH, alphaT * dotBH, a2 * dotNH );
		highp float v2 = dot( v, v );
		float w2 = a2 / v2;
		return RECIPROCAL_PI * a2 * pow2 ( w2 );
	}
#endif
#ifdef USE_CLEARCOAT
	vec3 BRDF_GGX_Clearcoat( const in vec3 lightDir, const in vec3 viewDir, const in vec3 normal, const in PhysicalMaterial material) {
		vec3 f0 = material.clearcoatF0;
		float f90 = material.clearcoatF90;
		float roughness = material.clearcoatRoughness;
		float alpha = pow2( roughness );
		vec3 halfDir = normalize( lightDir + viewDir );
		float dotNL = saturate( dot( normal, lightDir ) );
		float dotNV = saturate( dot( normal, viewDir ) );
		float dotNH = saturate( dot( normal, halfDir ) );
		float dotVH = saturate( dot( viewDir, halfDir ) );
		vec3 F = F_Schlick( f0, f90, dotVH );
		float V = V_GGX_SmithCorrelated( alpha, dotNL, dotNV );
		float D = D_GGX( alpha, dotNH );
		return F * ( V * D );
	}
#endif
vec3 BRDF_GGX( const in vec3 lightDir, const in vec3 viewDir, const in vec3 normal, const in PhysicalMaterial material ) {
	vec3 f0 = material.specularColor;
	float f90 = material.specularF90;
	float roughness = material.roughness;
	float alpha = pow2( roughness );
	vec3 halfDir = normalize( lightDir + viewDir );
	float dotNL = saturate( dot( normal, lightDir ) );
	float dotNV = saturate( dot( normal, viewDir ) );
	float dotNH = saturate( dot( normal, halfDir ) );
	float dotVH = saturate( dot( viewDir, halfDir ) );
	vec3 F = F_Schlick( f0, f90, dotVH );
	#ifdef USE_IRIDESCENCE
		F = mix( F, material.iridescenceFresnel, material.iridescence );
	#endif
	#ifdef USE_ANISOTROPY
		float dotTL = dot( material.anisotropyT, lightDir );
		float dotTV = dot( material.anisotropyT, viewDir );
		float dotTH = dot( material.anisotropyT, halfDir );
		float dotBL = dot( material.anisotropyB, lightDir );
		float dotBV = dot( material.anisotropyB, viewDir );
		float dotBH = dot( material.anisotropyB, halfDir );
		float V = V_GGX_SmithCorrelated_Anisotropic( material.alphaT, alpha, dotTV, dotBV, dotTL, dotBL, dotNV, dotNL );
		float D = D_GGX_Anisotropic( material.alphaT, alpha, dotNH, dotTH, dotBH );
	#else
		float V = V_GGX_SmithCorrelated( alpha, dotNL, dotNV );
		float D = D_GGX( alpha, dotNH );
	#endif
	return F * ( V * D );
}
vec2 LTC_Uv( const in vec3 N, const in vec3 V, const in float roughness ) {
	const float LUT_SIZE = 64.0;
	const float LUT_SCALE = ( LUT_SIZE - 1.0 ) / LUT_SIZE;
	const float LUT_BIAS = 0.5 / LUT_SIZE;
	float dotNV = saturate( dot( N, V ) );
	vec2 uv = vec2( roughness, sqrt( 1.0 - dotNV ) );
	uv = uv * LUT_SCALE + LUT_BIAS;
	return uv;
}
float LTC_ClippedSphereFormFactor( const in vec3 f ) {
	float l = length( f );
	return max( ( l * l + f.z ) / ( l + 1.0 ), 0.0 );
}
vec3 LTC_EdgeVectorFormFactor( const in vec3 v1, const in vec3 v2 ) {
	float x = dot( v1, v2 );
	float y = abs( x );
	float a = 0.8543985 + ( 0.4965155 + 0.0145206 * y ) * y;
	float b = 3.4175940 + ( 4.1616724 + y ) * y;
	float v = a / b;
	float theta_sintheta = ( x > 0.0 ) ? v : 0.5 * inversesqrt( max( 1.0 - x * x, 1e-7 ) ) - v;
	return cross( v1, v2 ) * theta_sintheta;
}
vec3 LTC_Evaluate( const in vec3 N, const in vec3 V, const in vec3 P, const in mat3 mInv, const in vec3 rectCoords[ 4 ] ) {
	vec3 v1 = rectCoords[ 1 ] - rectCoords[ 0 ];
	vec3 v2 = rectCoords[ 3 ] - rectCoords[ 0 ];
	vec3 lightNormal = cross( v1, v2 );
	if( dot( lightNormal, P - rectCoords[ 0 ] ) < 0.0 ) return vec3( 0.0 );
	vec3 T1, T2;
	T1 = normalize( V - N * dot( V, N ) );
	T2 = - cross( N, T1 );
	mat3 mat = mInv * transposeMat3( mat3( T1, T2, N ) );
	vec3 coords[ 4 ];
	coords[ 0 ] = mat * ( rectCoords[ 0 ] - P );
	coords[ 1 ] = mat * ( rectCoords[ 1 ] - P );
	coords[ 2 ] = mat * ( rectCoords[ 2 ] - P );
	coords[ 3 ] = mat * ( rectCoords[ 3 ] - P );
	coords[ 0 ] = normalize( coords[ 0 ] );
	coords[ 1 ] = normalize( coords[ 1 ] );
	coords[ 2 ] = normalize( coords[ 2 ] );
	coords[ 3 ] = normalize( coords[ 3 ] );
	vec3 vectorFormFactor = vec3( 0.0 );
	vectorFormFactor += LTC_EdgeVectorFormFactor( coords[ 0 ], coords[ 1 ] );
	vectorFormFactor += LTC_EdgeVectorFormFactor( coords[ 1 ], coords[ 2 ] );
	vectorFormFactor += LTC_EdgeVectorFormFactor( coords[ 2 ], coords[ 3 ] );
	vectorFormFactor += LTC_EdgeVectorFormFactor( coords[ 3 ], coords[ 0 ] );
	float result = LTC_ClippedSphereFormFactor( vectorFormFactor );
	return vec3( result );
}
#if defined( USE_SHEEN )
float D_Charlie( float roughness, float dotNH ) {
	float alpha = pow2( roughness );
	float invAlpha = 1.0 / alpha;
	float cos2h = dotNH * dotNH;
	float sin2h = max( 1.0 - cos2h, 0.0078125 );
	return ( 2.0 + invAlpha ) * pow( sin2h, invAlpha * 0.5 ) / ( 2.0 * PI );
}
float V_Neubelt( float dotNV, float dotNL ) {
	return saturate( 1.0 / ( 4.0 * ( dotNL + dotNV - dotNL * dotNV ) ) );
}
vec3 BRDF_Sheen( const in vec3 lightDir, const in vec3 viewDir, const in vec3 normal, vec3 sheenColor, const in float sheenRoughness ) {
	vec3 halfDir = normalize( lightDir + viewDir );
	float dotNL = saturate( dot( normal, lightDir ) );
	float dotNV = saturate( dot( normal, viewDir ) );
	float dotNH = saturate( dot( normal, halfDir ) );
	float D = D_Charlie( sheenRoughness, dotNH );
	float V = V_Neubelt( dotNV, dotNL );
	return sheenColor * ( D * V );
}
#endif
float IBLSheenBRDF( const in vec3 normal, const in vec3 viewDir, const in float roughness ) {
	float dotNV = saturate( dot( normal, viewDir ) );
	float r2 = roughness * roughness;
	float a = roughness < 0.25 ? -339.2 * r2 + 161.4 * roughness - 25.9 : -8.48 * r2 + 14.3 * roughness - 9.95;
	float b = roughness < 0.25 ? 44.0 * r2 - 23.7 * roughness + 3.26 : 1.97 * r2 - 3.27 * roughness + 0.72;
	float DG = exp( a * dotNV + b ) + ( roughness < 0.25 ? 0.0 : 0.1 * ( roughness - 0.25 ) );
	return saturate( DG * RECIPROCAL_PI );
}
vec2 DFGApprox( const in vec3 normal, const in vec3 viewDir, const in float roughness ) {
	float dotNV = saturate( dot( normal, viewDir ) );
	const vec4 c0 = vec4( - 1, - 0.0275, - 0.572, 0.022 );
	const vec4 c1 = vec4( 1, 0.0425, 1.04, - 0.04 );
	vec4 r = roughness * c0 + c1;
	float a004 = min( r.x * r.x, exp2( - 9.28 * dotNV ) ) * r.x + r.y;
	vec2 fab = vec2( - 1.04, 1.04 ) * a004 + r.zw;
	return fab;
}
vec3 EnvironmentBRDF( const in vec3 normal, const in vec3 viewDir, const in vec3 specularColor, const in float specularF90, const in float roughness ) {
	vec2 fab = DFGApprox( normal, viewDir, roughness );
	return specularColor * fab.x + specularF90 * fab.y;
}
#ifdef USE_IRIDESCENCE
void computeMultiscatteringIridescence( const in vec3 normal, const in vec3 viewDir, const in vec3 specularColor, const in float specularF90, const in float iridescence, const in vec3 iridescenceF0, const in float roughness, inout vec3 singleScatter, inout vec3 multiScatter ) {
#else
void computeMultiscattering( const in vec3 normal, const in vec3 viewDir, const in vec3 specularColor, const in float specularF90, const in float roughness, inout vec3 singleScatter, inout vec3 multiScatter ) {
#endif
	vec2 fab = DFGApprox( normal, viewDir, roughness );
	#ifdef USE_IRIDESCENCE
		vec3 Fr = mix( specularColor, iridescenceF0, iridescence );
	#else
		vec3 Fr = specularColor;
	#endif
	vec3 FssEss = Fr * fab.x + specularF90 * fab.y;
	float Ess = fab.x + fab.y;
	float Ems = 1.0 - Ess;
	vec3 Favg = Fr + ( 1.0 - Fr ) * 0.047619;	vec3 Fms = FssEss * Favg / ( 1.0 - Ems * Favg );
	singleScatter += FssEss;
	multiScatter += Fms * Ems;
}
#if NUM_RECT_AREA_LIGHTS > 0
	void RE_Direct_RectArea_Physical( const in RectAreaLight rectAreaLight, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in PhysicalMaterial material, inout ReflectedLight reflectedLight ) {
		vec3 normal = geometryNormal;
		vec3 viewDir = geometryViewDir;
		vec3 position = geometryPosition;
		vec3 lightPos = rectAreaLight.position;
		vec3 halfWidth = rectAreaLight.halfWidth;
		vec3 halfHeight = rectAreaLight.halfHeight;
		vec3 lightColor = rectAreaLight.color;
		float roughness = material.roughness;
		vec3 rectCoords[ 4 ];
		rectCoords[ 0 ] = lightPos + halfWidth - halfHeight;		rectCoords[ 1 ] = lightPos - halfWidth - halfHeight;
		rectCoords[ 2 ] = lightPos - halfWidth + halfHeight;
		rectCoords[ 3 ] = lightPos + halfWidth + halfHeight;
		vec2 uv = LTC_Uv( normal, viewDir, roughness );
		vec4 t1 = texture2D( ltc_1, uv );
		vec4 t2 = texture2D( ltc_2, uv );
		mat3 mInv = mat3(
			vec3( t1.x, 0, t1.y ),
			vec3(    0, 1,    0 ),
			vec3( t1.z, 0, t1.w )
		);
		vec3 fresnel = ( material.specularColor * t2.x + ( vec3( 1.0 ) - material.specularColor ) * t2.y );
		reflectedLight.directSpecular += lightColor * fresnel * LTC_Evaluate( normal, viewDir, position, mInv, rectCoords );
		reflectedLight.directDiffuse += lightColor * material.diffuseColor * LTC_Evaluate( normal, viewDir, position, mat3( 1.0 ), rectCoords );
	}
#endif
void RE_Direct_Physical( const in IncidentLight directLight, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in PhysicalMaterial material, inout ReflectedLight reflectedLight ) {
	float dotNL = saturate( dot( geometryNormal, directLight.direction ) );
	vec3 irradiance = dotNL * directLight.color;
	#ifdef USE_CLEARCOAT
		float dotNLcc = saturate( dot( geometryClearcoatNormal, directLight.direction ) );
		vec3 ccIrradiance = dotNLcc * directLight.color;
		clearcoatSpecularDirect += ccIrradiance * BRDF_GGX_Clearcoat( directLight.direction, geometryViewDir, geometryClearcoatNormal, material );
	#endif
	#ifdef USE_SHEEN
		sheenSpecularDirect += irradiance * BRDF_Sheen( directLight.direction, geometryViewDir, geometryNormal, material.sheenColor, material.sheenRoughness );
	#endif
	reflectedLight.directSpecular += irradiance * BRDF_GGX( directLight.direction, geometryViewDir, geometryNormal, material );
	reflectedLight.directDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
}
void RE_IndirectDiffuse_Physical( const in vec3 irradiance, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in PhysicalMaterial material, inout ReflectedLight reflectedLight ) {
	reflectedLight.indirectDiffuse += irradiance * BRDF_Lambert( material.diffuseColor );
}
void RE_IndirectSpecular_Physical( const in vec3 radiance, const in vec3 irradiance, const in vec3 clearcoatRadiance, const in vec3 geometryPosition, const in vec3 geometryNormal, const in vec3 geometryViewDir, const in vec3 geometryClearcoatNormal, const in PhysicalMaterial material, inout ReflectedLight reflectedLight) {
	#ifdef USE_CLEARCOAT
		clearcoatSpecularIndirect += clearcoatRadiance * EnvironmentBRDF( geometryClearcoatNormal, geometryViewDir, material.clearcoatF0, material.clearcoatF90, material.clearcoatRoughness );
	#endif
	#ifdef USE_SHEEN
		sheenSpecularIndirect += irradiance * material.sheenColor * IBLSheenBRDF( geometryNormal, geometryViewDir, material.sheenRoughness );
	#endif
	vec3 singleScattering = vec3( 0.0 );
	vec3 multiScattering = vec3( 0.0 );
	vec3 cosineWeightedIrradiance = irradiance * RECIPROCAL_PI;
	#ifdef USE_IRIDESCENCE
		computeMultiscatteringIridescence( geometryNormal, geometryViewDir, material.specularColor, material.specularF90, material.iridescence, material.iridescenceFresnel, material.roughness, singleScattering, multiScattering );
	#else
		computeMultiscattering( geometryNormal, geometryViewDir, material.specularColor, material.specularF90, material.roughness, singleScattering, multiScattering );
	#endif
	vec3 totalScattering = singleScattering + multiScattering;
	vec3 diffuse = material.diffuseColor * ( 1.0 - max( max( totalScattering.r, totalScattering.g ), totalScattering.b ) );
	reflectedLight.indirectSpecular += radiance * singleScattering;
	reflectedLight.indirectSpecular += multiScattering * cosineWeightedIrradiance;
	reflectedLight.indirectDiffuse += diffuse * cosineWeightedIrradiance;
}
#define RE_Direct				RE_Direct_Physical
#define RE_Direct_RectArea		RE_Direct_RectArea_Physical
#define RE_IndirectDiffuse		RE_IndirectDiffuse_Physical
#define RE_IndirectSpecular		RE_IndirectSpecular_Physical
float computeSpecularOcclusion( const in float dotNV, const in float ambientOcclusion, const in float roughness ) {
	return saturate( pow( dotNV + ambientOcclusion, exp2( - 16.0 * roughness - 1.0 ) ) - 1.0 + ambientOcclusion );
}`,bp=`
vec3 geometryPosition = - vViewPosition;
vec3 geometryNormal = normal;
vec3 geometryViewDir = ( isOrthographic ) ? vec3( 0, 0, 1 ) : normalize( vViewPosition );
vec3 geometryClearcoatNormal = vec3( 0.0 );
#ifdef USE_CLEARCOAT
	geometryClearcoatNormal = clearcoatNormal;
#endif
#ifdef USE_IRIDESCENCE
	float dotNVi = saturate( dot( normal, geometryViewDir ) );
	if ( material.iridescenceThickness == 0.0 ) {
		material.iridescence = 0.0;
	} else {
		material.iridescence = saturate( material.iridescence );
	}
	if ( material.iridescence > 0.0 ) {
		material.iridescenceFresnel = evalIridescence( 1.0, material.iridescenceIOR, dotNVi, material.iridescenceThickness, material.specularColor );
		material.iridescenceF0 = Schlick_to_F0( material.iridescenceFresnel, 1.0, dotNVi );
	}
#endif
IncidentLight directLight;
#if ( NUM_POINT_LIGHTS > 0 ) && defined( RE_Direct )
	PointLight pointLight;
	#if defined( USE_SHADOWMAP ) && NUM_POINT_LIGHT_SHADOWS > 0
	PointLightShadow pointLightShadow;
	#endif
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_POINT_LIGHTS; i ++ ) {
		pointLight = pointLights[ i ];
		getPointLightInfo( pointLight, geometryPosition, directLight );
		#if defined( USE_SHADOWMAP ) && ( UNROLLED_LOOP_INDEX < NUM_POINT_LIGHT_SHADOWS )
		pointLightShadow = pointLightShadows[ i ];
		directLight.color *= ( directLight.visible && receiveShadow ) ? getPointShadow( pointShadowMap[ i ], pointLightShadow.shadowMapSize, pointLightShadow.shadowIntensity, pointLightShadow.shadowBias, pointLightShadow.shadowRadius, vPointShadowCoord[ i ], pointLightShadow.shadowCameraNear, pointLightShadow.shadowCameraFar ) : 1.0;
		#endif
		RE_Direct( directLight, geometryPosition, geometryNormal, geometryViewDir, geometryClearcoatNormal, material, reflectedLight );
	}
	#pragma unroll_loop_end
#endif
#if ( NUM_SPOT_LIGHTS > 0 ) && defined( RE_Direct )
	SpotLight spotLight;
	vec4 spotColor;
	vec3 spotLightCoord;
	bool inSpotLightMap;
	#if defined( USE_SHADOWMAP ) && NUM_SPOT_LIGHT_SHADOWS > 0
	SpotLightShadow spotLightShadow;
	#endif
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_SPOT_LIGHTS; i ++ ) {
		spotLight = spotLights[ i ];
		getSpotLightInfo( spotLight, geometryPosition, directLight );
		#if ( UNROLLED_LOOP_INDEX < NUM_SPOT_LIGHT_SHADOWS_WITH_MAPS )
		#define SPOT_LIGHT_MAP_INDEX UNROLLED_LOOP_INDEX
		#elif ( UNROLLED_LOOP_INDEX < NUM_SPOT_LIGHT_SHADOWS )
		#define SPOT_LIGHT_MAP_INDEX NUM_SPOT_LIGHT_MAPS
		#else
		#define SPOT_LIGHT_MAP_INDEX ( UNROLLED_LOOP_INDEX - NUM_SPOT_LIGHT_SHADOWS + NUM_SPOT_LIGHT_SHADOWS_WITH_MAPS )
		#endif
		#if ( SPOT_LIGHT_MAP_INDEX < NUM_SPOT_LIGHT_MAPS )
			spotLightCoord = vSpotLightCoord[ i ].xyz / vSpotLightCoord[ i ].w;
			inSpotLightMap = all( lessThan( abs( spotLightCoord * 2. - 1. ), vec3( 1.0 ) ) );
			spotColor = texture2D( spotLightMap[ SPOT_LIGHT_MAP_INDEX ], spotLightCoord.xy );
			directLight.color = inSpotLightMap ? directLight.color * spotColor.rgb : directLight.color;
		#endif
		#undef SPOT_LIGHT_MAP_INDEX
		#if defined( USE_SHADOWMAP ) && ( UNROLLED_LOOP_INDEX < NUM_SPOT_LIGHT_SHADOWS )
		spotLightShadow = spotLightShadows[ i ];
		directLight.color *= ( directLight.visible && receiveShadow ) ? getShadow( spotShadowMap[ i ], spotLightShadow.shadowMapSize, spotLightShadow.shadowIntensity, spotLightShadow.shadowBias, spotLightShadow.shadowRadius, vSpotLightCoord[ i ] ) : 1.0;
		#endif
		RE_Direct( directLight, geometryPosition, geometryNormal, geometryViewDir, geometryClearcoatNormal, material, reflectedLight );
	}
	#pragma unroll_loop_end
#endif
#if ( NUM_DIR_LIGHTS > 0 ) && defined( RE_Direct )
	DirectionalLight directionalLight;
	#if defined( USE_SHADOWMAP ) && NUM_DIR_LIGHT_SHADOWS > 0
	DirectionalLightShadow directionalLightShadow;
	#endif
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_DIR_LIGHTS; i ++ ) {
		directionalLight = directionalLights[ i ];
		getDirectionalLightInfo( directionalLight, directLight );
		#if defined( USE_SHADOWMAP ) && ( UNROLLED_LOOP_INDEX < NUM_DIR_LIGHT_SHADOWS )
		directionalLightShadow = directionalLightShadows[ i ];
		directLight.color *= ( directLight.visible && receiveShadow ) ? getShadow( directionalShadowMap[ i ], directionalLightShadow.shadowMapSize, directionalLightShadow.shadowIntensity, directionalLightShadow.shadowBias, directionalLightShadow.shadowRadius, vDirectionalShadowCoord[ i ] ) : 1.0;
		#endif
		RE_Direct( directLight, geometryPosition, geometryNormal, geometryViewDir, geometryClearcoatNormal, material, reflectedLight );
	}
	#pragma unroll_loop_end
#endif
#if ( NUM_RECT_AREA_LIGHTS > 0 ) && defined( RE_Direct_RectArea )
	RectAreaLight rectAreaLight;
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_RECT_AREA_LIGHTS; i ++ ) {
		rectAreaLight = rectAreaLights[ i ];
		RE_Direct_RectArea( rectAreaLight, geometryPosition, geometryNormal, geometryViewDir, geometryClearcoatNormal, material, reflectedLight );
	}
	#pragma unroll_loop_end
#endif
#if defined( RE_IndirectDiffuse )
	vec3 iblIrradiance = vec3( 0.0 );
	vec3 irradiance = getAmbientLightIrradiance( ambientLightColor );
	#if defined( USE_LIGHT_PROBES )
		irradiance += getLightProbeIrradiance( lightProbe, geometryNormal );
	#endif
	#if ( NUM_HEMI_LIGHTS > 0 )
		#pragma unroll_loop_start
		for ( int i = 0; i < NUM_HEMI_LIGHTS; i ++ ) {
			irradiance += getHemisphereLightIrradiance( hemisphereLights[ i ], geometryNormal );
		}
		#pragma unroll_loop_end
	#endif
#endif
#if defined( RE_IndirectSpecular )
	vec3 radiance = vec3( 0.0 );
	vec3 clearcoatRadiance = vec3( 0.0 );
#endif`,Ep=`#if defined( RE_IndirectDiffuse )
	#ifdef USE_LIGHTMAP
		vec4 lightMapTexel = texture2D( lightMap, vLightMapUv );
		vec3 lightMapIrradiance = lightMapTexel.rgb * lightMapIntensity;
		irradiance += lightMapIrradiance;
	#endif
	#if defined( USE_ENVMAP ) && defined( STANDARD ) && defined( ENVMAP_TYPE_CUBE_UV )
		iblIrradiance += getIBLIrradiance( geometryNormal );
	#endif
#endif
#if defined( USE_ENVMAP ) && defined( RE_IndirectSpecular )
	#ifdef USE_ANISOTROPY
		radiance += getIBLAnisotropyRadiance( geometryViewDir, geometryNormal, material.roughness, material.anisotropyB, material.anisotropy );
	#else
		radiance += getIBLRadiance( geometryViewDir, geometryNormal, material.roughness );
	#endif
	#ifdef USE_CLEARCOAT
		clearcoatRadiance += getIBLRadiance( geometryViewDir, geometryClearcoatNormal, material.clearcoatRoughness );
	#endif
#endif`,Tp=`#if defined( RE_IndirectDiffuse )
	RE_IndirectDiffuse( irradiance, geometryPosition, geometryNormal, geometryViewDir, geometryClearcoatNormal, material, reflectedLight );
#endif
#if defined( RE_IndirectSpecular )
	RE_IndirectSpecular( radiance, iblIrradiance, clearcoatRadiance, geometryPosition, geometryNormal, geometryViewDir, geometryClearcoatNormal, material, reflectedLight );
#endif`,wp=`#if defined( USE_LOGARITHMIC_DEPTH_BUFFER )
	gl_FragDepth = vIsPerspective == 0.0 ? gl_FragCoord.z : log2( vFragDepth ) * logDepthBufFC * 0.5;
#endif`,Ap=`#if defined( USE_LOGARITHMIC_DEPTH_BUFFER )
	uniform float logDepthBufFC;
	varying float vFragDepth;
	varying float vIsPerspective;
#endif`,Rp=`#ifdef USE_LOGARITHMIC_DEPTH_BUFFER
	varying float vFragDepth;
	varying float vIsPerspective;
#endif`,Cp=`#ifdef USE_LOGARITHMIC_DEPTH_BUFFER
	vFragDepth = 1.0 + gl_Position.w;
	vIsPerspective = float( isPerspectiveMatrix( projectionMatrix ) );
#endif`,Ip=`#ifdef USE_MAP
	vec4 sampledDiffuseColor = texture2D( map, vMapUv );
	#ifdef DECODE_VIDEO_TEXTURE
		sampledDiffuseColor = sRGBTransferEOTF( sampledDiffuseColor );
	#endif
	diffuseColor *= sampledDiffuseColor;
#endif`,Pp=`#ifdef USE_MAP
	uniform sampler2D map;
#endif`,Lp=`#if defined( USE_MAP ) || defined( USE_ALPHAMAP )
	#if defined( USE_POINTS_UV )
		vec2 uv = vUv;
	#else
		vec2 uv = ( uvTransform * vec3( gl_PointCoord.x, 1.0 - gl_PointCoord.y, 1 ) ).xy;
	#endif
#endif
#ifdef USE_MAP
	diffuseColor *= texture2D( map, uv );
#endif
#ifdef USE_ALPHAMAP
	diffuseColor.a *= texture2D( alphaMap, uv ).g;
#endif`,Dp=`#if defined( USE_POINTS_UV )
	varying vec2 vUv;
#else
	#if defined( USE_MAP ) || defined( USE_ALPHAMAP )
		uniform mat3 uvTransform;
	#endif
#endif
#ifdef USE_MAP
	uniform sampler2D map;
#endif
#ifdef USE_ALPHAMAP
	uniform sampler2D alphaMap;
#endif`,Up=`float metalnessFactor = metalness;
#ifdef USE_METALNESSMAP
	vec4 texelMetalness = texture2D( metalnessMap, vMetalnessMapUv );
	metalnessFactor *= texelMetalness.b;
#endif`,Np=`#ifdef USE_METALNESSMAP
	uniform sampler2D metalnessMap;
#endif`,Fp=`#ifdef USE_INSTANCING_MORPH
	float morphTargetInfluences[ MORPHTARGETS_COUNT ];
	float morphTargetBaseInfluence = texelFetch( morphTexture, ivec2( 0, gl_InstanceID ), 0 ).r;
	for ( int i = 0; i < MORPHTARGETS_COUNT; i ++ ) {
		morphTargetInfluences[i] =  texelFetch( morphTexture, ivec2( i + 1, gl_InstanceID ), 0 ).r;
	}
#endif`,Op=`#if defined( USE_MORPHCOLORS )
	vColor *= morphTargetBaseInfluence;
	for ( int i = 0; i < MORPHTARGETS_COUNT; i ++ ) {
		#if defined( USE_COLOR_ALPHA )
			if ( morphTargetInfluences[ i ] != 0.0 ) vColor += getMorph( gl_VertexID, i, 2 ) * morphTargetInfluences[ i ];
		#elif defined( USE_COLOR )
			if ( morphTargetInfluences[ i ] != 0.0 ) vColor += getMorph( gl_VertexID, i, 2 ).rgb * morphTargetInfluences[ i ];
		#endif
	}
#endif`,kp=`#ifdef USE_MORPHNORMALS
	objectNormal *= morphTargetBaseInfluence;
	for ( int i = 0; i < MORPHTARGETS_COUNT; i ++ ) {
		if ( morphTargetInfluences[ i ] != 0.0 ) objectNormal += getMorph( gl_VertexID, i, 1 ).xyz * morphTargetInfluences[ i ];
	}
#endif`,Bp=`#ifdef USE_MORPHTARGETS
	#ifndef USE_INSTANCING_MORPH
		uniform float morphTargetBaseInfluence;
		uniform float morphTargetInfluences[ MORPHTARGETS_COUNT ];
	#endif
	uniform sampler2DArray morphTargetsTexture;
	uniform ivec2 morphTargetsTextureSize;
	vec4 getMorph( const in int vertexIndex, const in int morphTargetIndex, const in int offset ) {
		int texelIndex = vertexIndex * MORPHTARGETS_TEXTURE_STRIDE + offset;
		int y = texelIndex / morphTargetsTextureSize.x;
		int x = texelIndex - y * morphTargetsTextureSize.x;
		ivec3 morphUV = ivec3( x, y, morphTargetIndex );
		return texelFetch( morphTargetsTexture, morphUV, 0 );
	}
#endif`,zp=`#ifdef USE_MORPHTARGETS
	transformed *= morphTargetBaseInfluence;
	for ( int i = 0; i < MORPHTARGETS_COUNT; i ++ ) {
		if ( morphTargetInfluences[ i ] != 0.0 ) transformed += getMorph( gl_VertexID, i, 0 ).xyz * morphTargetInfluences[ i ];
	}
#endif`,Vp=`float faceDirection = gl_FrontFacing ? 1.0 : - 1.0;
#ifdef FLAT_SHADED
	vec3 fdx = dFdx( vViewPosition );
	vec3 fdy = dFdy( vViewPosition );
	vec3 normal = normalize( cross( fdx, fdy ) );
#else
	vec3 normal = normalize( vNormal );
	#ifdef DOUBLE_SIDED
		normal *= faceDirection;
	#endif
#endif
#if defined( USE_NORMALMAP_TANGENTSPACE ) || defined( USE_CLEARCOAT_NORMALMAP ) || defined( USE_ANISOTROPY )
	#ifdef USE_TANGENT
		mat3 tbn = mat3( normalize( vTangent ), normalize( vBitangent ), normal );
	#else
		mat3 tbn = getTangentFrame( - vViewPosition, normal,
		#if defined( USE_NORMALMAP )
			vNormalMapUv
		#elif defined( USE_CLEARCOAT_NORMALMAP )
			vClearcoatNormalMapUv
		#else
			vUv
		#endif
		);
	#endif
	#if defined( DOUBLE_SIDED ) && ! defined( FLAT_SHADED )
		tbn[0] *= faceDirection;
		tbn[1] *= faceDirection;
	#endif
#endif
#ifdef USE_CLEARCOAT_NORMALMAP
	#ifdef USE_TANGENT
		mat3 tbn2 = mat3( normalize( vTangent ), normalize( vBitangent ), normal );
	#else
		mat3 tbn2 = getTangentFrame( - vViewPosition, normal, vClearcoatNormalMapUv );
	#endif
	#if defined( DOUBLE_SIDED ) && ! defined( FLAT_SHADED )
		tbn2[0] *= faceDirection;
		tbn2[1] *= faceDirection;
	#endif
#endif
vec3 nonPerturbedNormal = normal;`,Gp=`#ifdef USE_NORMALMAP_OBJECTSPACE
	normal = texture2D( normalMap, vNormalMapUv ).xyz * 2.0 - 1.0;
	#ifdef FLIP_SIDED
		normal = - normal;
	#endif
	#ifdef DOUBLE_SIDED
		normal = normal * faceDirection;
	#endif
	normal = normalize( normalMatrix * normal );
#elif defined( USE_NORMALMAP_TANGENTSPACE )
	vec3 mapN = texture2D( normalMap, vNormalMapUv ).xyz * 2.0 - 1.0;
	mapN.xy *= normalScale;
	normal = normalize( tbn * mapN );
#elif defined( USE_BUMPMAP )
	normal = perturbNormalArb( - vViewPosition, normal, dHdxy_fwd(), faceDirection );
#endif`,Hp=`#ifndef FLAT_SHADED
	varying vec3 vNormal;
	#ifdef USE_TANGENT
		varying vec3 vTangent;
		varying vec3 vBitangent;
	#endif
#endif`,Wp=`#ifndef FLAT_SHADED
	varying vec3 vNormal;
	#ifdef USE_TANGENT
		varying vec3 vTangent;
		varying vec3 vBitangent;
	#endif
#endif`,Xp=`#ifndef FLAT_SHADED
	vNormal = normalize( transformedNormal );
	#ifdef USE_TANGENT
		vTangent = normalize( transformedTangent );
		vBitangent = normalize( cross( vNormal, vTangent ) * tangent.w );
	#endif
#endif`,qp=`#ifdef USE_NORMALMAP
	uniform sampler2D normalMap;
	uniform vec2 normalScale;
#endif
#ifdef USE_NORMALMAP_OBJECTSPACE
	uniform mat3 normalMatrix;
#endif
#if ! defined ( USE_TANGENT ) && ( defined ( USE_NORMALMAP_TANGENTSPACE ) || defined ( USE_CLEARCOAT_NORMALMAP ) || defined( USE_ANISOTROPY ) )
	mat3 getTangentFrame( vec3 eye_pos, vec3 surf_norm, vec2 uv ) {
		vec3 q0 = dFdx( eye_pos.xyz );
		vec3 q1 = dFdy( eye_pos.xyz );
		vec2 st0 = dFdx( uv.st );
		vec2 st1 = dFdy( uv.st );
		vec3 N = surf_norm;
		vec3 q1perp = cross( q1, N );
		vec3 q0perp = cross( N, q0 );
		vec3 T = q1perp * st0.x + q0perp * st1.x;
		vec3 B = q1perp * st0.y + q0perp * st1.y;
		float det = max( dot( T, T ), dot( B, B ) );
		float scale = ( det == 0.0 ) ? 0.0 : inversesqrt( det );
		return mat3( T * scale, B * scale, N );
	}
#endif`,$p=`#ifdef USE_CLEARCOAT
	vec3 clearcoatNormal = nonPerturbedNormal;
#endif`,Yp=`#ifdef USE_CLEARCOAT_NORMALMAP
	vec3 clearcoatMapN = texture2D( clearcoatNormalMap, vClearcoatNormalMapUv ).xyz * 2.0 - 1.0;
	clearcoatMapN.xy *= clearcoatNormalScale;
	clearcoatNormal = normalize( tbn2 * clearcoatMapN );
#endif`,Zp=`#ifdef USE_CLEARCOATMAP
	uniform sampler2D clearcoatMap;
#endif
#ifdef USE_CLEARCOAT_NORMALMAP
	uniform sampler2D clearcoatNormalMap;
	uniform vec2 clearcoatNormalScale;
#endif
#ifdef USE_CLEARCOAT_ROUGHNESSMAP
	uniform sampler2D clearcoatRoughnessMap;
#endif`,Jp=`#ifdef USE_IRIDESCENCEMAP
	uniform sampler2D iridescenceMap;
#endif
#ifdef USE_IRIDESCENCE_THICKNESSMAP
	uniform sampler2D iridescenceThicknessMap;
#endif`,Kp=`#ifdef OPAQUE
diffuseColor.a = 1.0;
#endif
#ifdef USE_TRANSMISSION
diffuseColor.a *= material.transmissionAlpha;
#endif
gl_FragColor = vec4( outgoingLight, diffuseColor.a );`,jp=`vec3 packNormalToRGB( const in vec3 normal ) {
	return normalize( normal ) * 0.5 + 0.5;
}
vec3 unpackRGBToNormal( const in vec3 rgb ) {
	return 2.0 * rgb.xyz - 1.0;
}
const float PackUpscale = 256. / 255.;const float UnpackDownscale = 255. / 256.;const float ShiftRight8 = 1. / 256.;
const float Inv255 = 1. / 255.;
const vec4 PackFactors = vec4( 1.0, 256.0, 256.0 * 256.0, 256.0 * 256.0 * 256.0 );
const vec2 UnpackFactors2 = vec2( UnpackDownscale, 1.0 / PackFactors.g );
const vec3 UnpackFactors3 = vec3( UnpackDownscale / PackFactors.rg, 1.0 / PackFactors.b );
const vec4 UnpackFactors4 = vec4( UnpackDownscale / PackFactors.rgb, 1.0 / PackFactors.a );
vec4 packDepthToRGBA( const in float v ) {
	if( v <= 0.0 )
		return vec4( 0., 0., 0., 0. );
	if( v >= 1.0 )
		return vec4( 1., 1., 1., 1. );
	float vuf;
	float af = modf( v * PackFactors.a, vuf );
	float bf = modf( vuf * ShiftRight8, vuf );
	float gf = modf( vuf * ShiftRight8, vuf );
	return vec4( vuf * Inv255, gf * PackUpscale, bf * PackUpscale, af );
}
vec3 packDepthToRGB( const in float v ) {
	if( v <= 0.0 )
		return vec3( 0., 0., 0. );
	if( v >= 1.0 )
		return vec3( 1., 1., 1. );
	float vuf;
	float bf = modf( v * PackFactors.b, vuf );
	float gf = modf( vuf * ShiftRight8, vuf );
	return vec3( vuf * Inv255, gf * PackUpscale, bf );
}
vec2 packDepthToRG( const in float v ) {
	if( v <= 0.0 )
		return vec2( 0., 0. );
	if( v >= 1.0 )
		return vec2( 1., 1. );
	float vuf;
	float gf = modf( v * 256., vuf );
	return vec2( vuf * Inv255, gf );
}
float unpackRGBAToDepth( const in vec4 v ) {
	return dot( v, UnpackFactors4 );
}
float unpackRGBToDepth( const in vec3 v ) {
	return dot( v, UnpackFactors3 );
}
float unpackRGToDepth( const in vec2 v ) {
	return v.r * UnpackFactors2.r + v.g * UnpackFactors2.g;
}
vec4 pack2HalfToRGBA( const in vec2 v ) {
	vec4 r = vec4( v.x, fract( v.x * 255.0 ), v.y, fract( v.y * 255.0 ) );
	return vec4( r.x - r.y / 255.0, r.y, r.z - r.w / 255.0, r.w );
}
vec2 unpackRGBATo2Half( const in vec4 v ) {
	return vec2( v.x + ( v.y / 255.0 ), v.z + ( v.w / 255.0 ) );
}
float viewZToOrthographicDepth( const in float viewZ, const in float near, const in float far ) {
	return ( viewZ + near ) / ( near - far );
}
float orthographicDepthToViewZ( const in float depth, const in float near, const in float far ) {
	return depth * ( near - far ) - near;
}
float viewZToPerspectiveDepth( const in float viewZ, const in float near, const in float far ) {
	return ( ( near + viewZ ) * far ) / ( ( far - near ) * viewZ );
}
float perspectiveDepthToViewZ( const in float depth, const in float near, const in float far ) {
	return ( near * far ) / ( ( far - near ) * depth - far );
}`,Qp=`#ifdef PREMULTIPLIED_ALPHA
	gl_FragColor.rgb *= gl_FragColor.a;
#endif`,em=`vec4 mvPosition = vec4( transformed, 1.0 );
#ifdef USE_BATCHING
	mvPosition = batchingMatrix * mvPosition;
#endif
#ifdef USE_INSTANCING
	mvPosition = instanceMatrix * mvPosition;
#endif
mvPosition = modelViewMatrix * mvPosition;
gl_Position = projectionMatrix * mvPosition;`,tm=`#ifdef DITHERING
	gl_FragColor.rgb = dithering( gl_FragColor.rgb );
#endif`,nm=`#ifdef DITHERING
	vec3 dithering( vec3 color ) {
		float grid_position = rand( gl_FragCoord.xy );
		vec3 dither_shift_RGB = vec3( 0.25 / 255.0, -0.25 / 255.0, 0.25 / 255.0 );
		dither_shift_RGB = mix( 2.0 * dither_shift_RGB, -2.0 * dither_shift_RGB, grid_position );
		return color + dither_shift_RGB;
	}
#endif`,im=`float roughnessFactor = roughness;
#ifdef USE_ROUGHNESSMAP
	vec4 texelRoughness = texture2D( roughnessMap, vRoughnessMapUv );
	roughnessFactor *= texelRoughness.g;
#endif`,sm=`#ifdef USE_ROUGHNESSMAP
	uniform sampler2D roughnessMap;
#endif`,rm=`#if NUM_SPOT_LIGHT_COORDS > 0
	varying vec4 vSpotLightCoord[ NUM_SPOT_LIGHT_COORDS ];
#endif
#if NUM_SPOT_LIGHT_MAPS > 0
	uniform sampler2D spotLightMap[ NUM_SPOT_LIGHT_MAPS ];
#endif
#ifdef USE_SHADOWMAP
	#if NUM_DIR_LIGHT_SHADOWS > 0
		uniform sampler2D directionalShadowMap[ NUM_DIR_LIGHT_SHADOWS ];
		varying vec4 vDirectionalShadowCoord[ NUM_DIR_LIGHT_SHADOWS ];
		struct DirectionalLightShadow {
			float shadowIntensity;
			float shadowBias;
			float shadowNormalBias;
			float shadowRadius;
			vec2 shadowMapSize;
		};
		uniform DirectionalLightShadow directionalLightShadows[ NUM_DIR_LIGHT_SHADOWS ];
	#endif
	#if NUM_SPOT_LIGHT_SHADOWS > 0
		uniform sampler2D spotShadowMap[ NUM_SPOT_LIGHT_SHADOWS ];
		struct SpotLightShadow {
			float shadowIntensity;
			float shadowBias;
			float shadowNormalBias;
			float shadowRadius;
			vec2 shadowMapSize;
		};
		uniform SpotLightShadow spotLightShadows[ NUM_SPOT_LIGHT_SHADOWS ];
	#endif
	#if NUM_POINT_LIGHT_SHADOWS > 0
		uniform sampler2D pointShadowMap[ NUM_POINT_LIGHT_SHADOWS ];
		varying vec4 vPointShadowCoord[ NUM_POINT_LIGHT_SHADOWS ];
		struct PointLightShadow {
			float shadowIntensity;
			float shadowBias;
			float shadowNormalBias;
			float shadowRadius;
			vec2 shadowMapSize;
			float shadowCameraNear;
			float shadowCameraFar;
		};
		uniform PointLightShadow pointLightShadows[ NUM_POINT_LIGHT_SHADOWS ];
	#endif
	float texture2DCompare( sampler2D depths, vec2 uv, float compare ) {
		float depth = unpackRGBAToDepth( texture2D( depths, uv ) );
		#ifdef USE_REVERSED_DEPTH_BUFFER
			return step( depth, compare );
		#else
			return step( compare, depth );
		#endif
	}
	vec2 texture2DDistribution( sampler2D shadow, vec2 uv ) {
		return unpackRGBATo2Half( texture2D( shadow, uv ) );
	}
	float VSMShadow( sampler2D shadow, vec2 uv, float compare ) {
		float occlusion = 1.0;
		vec2 distribution = texture2DDistribution( shadow, uv );
		#ifdef USE_REVERSED_DEPTH_BUFFER
			float hard_shadow = step( distribution.x, compare );
		#else
			float hard_shadow = step( compare, distribution.x );
		#endif
		if ( hard_shadow != 1.0 ) {
			float distance = compare - distribution.x;
			float variance = max( 0.00000, distribution.y * distribution.y );
			float softness_probability = variance / (variance + distance * distance );			softness_probability = clamp( ( softness_probability - 0.3 ) / ( 0.95 - 0.3 ), 0.0, 1.0 );			occlusion = clamp( max( hard_shadow, softness_probability ), 0.0, 1.0 );
		}
		return occlusion;
	}
	float getShadow( sampler2D shadowMap, vec2 shadowMapSize, float shadowIntensity, float shadowBias, float shadowRadius, vec4 shadowCoord ) {
		float shadow = 1.0;
		shadowCoord.xyz /= shadowCoord.w;
		shadowCoord.z += shadowBias;
		bool inFrustum = shadowCoord.x >= 0.0 && shadowCoord.x <= 1.0 && shadowCoord.y >= 0.0 && shadowCoord.y <= 1.0;
		bool frustumTest = inFrustum && shadowCoord.z <= 1.0;
		if ( frustumTest ) {
		#if defined( SHADOWMAP_TYPE_PCF )
			vec2 texelSize = vec2( 1.0 ) / shadowMapSize;
			float dx0 = - texelSize.x * shadowRadius;
			float dy0 = - texelSize.y * shadowRadius;
			float dx1 = + texelSize.x * shadowRadius;
			float dy1 = + texelSize.y * shadowRadius;
			float dx2 = dx0 / 2.0;
			float dy2 = dy0 / 2.0;
			float dx3 = dx1 / 2.0;
			float dy3 = dy1 / 2.0;
			shadow = (
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx0, dy0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( 0.0, dy0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx1, dy0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx2, dy2 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( 0.0, dy2 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx3, dy2 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx0, 0.0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx2, 0.0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy, shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx3, 0.0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx1, 0.0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx2, dy3 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( 0.0, dy3 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx3, dy3 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx0, dy1 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( 0.0, dy1 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, shadowCoord.xy + vec2( dx1, dy1 ), shadowCoord.z )
			) * ( 1.0 / 17.0 );
		#elif defined( SHADOWMAP_TYPE_PCF_SOFT )
			vec2 texelSize = vec2( 1.0 ) / shadowMapSize;
			float dx = texelSize.x;
			float dy = texelSize.y;
			vec2 uv = shadowCoord.xy;
			vec2 f = fract( uv * shadowMapSize + 0.5 );
			uv -= f * texelSize;
			shadow = (
				texture2DCompare( shadowMap, uv, shadowCoord.z ) +
				texture2DCompare( shadowMap, uv + vec2( dx, 0.0 ), shadowCoord.z ) +
				texture2DCompare( shadowMap, uv + vec2( 0.0, dy ), shadowCoord.z ) +
				texture2DCompare( shadowMap, uv + texelSize, shadowCoord.z ) +
				mix( texture2DCompare( shadowMap, uv + vec2( -dx, 0.0 ), shadowCoord.z ),
					 texture2DCompare( shadowMap, uv + vec2( 2.0 * dx, 0.0 ), shadowCoord.z ),
					 f.x ) +
				mix( texture2DCompare( shadowMap, uv + vec2( -dx, dy ), shadowCoord.z ),
					 texture2DCompare( shadowMap, uv + vec2( 2.0 * dx, dy ), shadowCoord.z ),
					 f.x ) +
				mix( texture2DCompare( shadowMap, uv + vec2( 0.0, -dy ), shadowCoord.z ),
					 texture2DCompare( shadowMap, uv + vec2( 0.0, 2.0 * dy ), shadowCoord.z ),
					 f.y ) +
				mix( texture2DCompare( shadowMap, uv + vec2( dx, -dy ), shadowCoord.z ),
					 texture2DCompare( shadowMap, uv + vec2( dx, 2.0 * dy ), shadowCoord.z ),
					 f.y ) +
				mix( mix( texture2DCompare( shadowMap, uv + vec2( -dx, -dy ), shadowCoord.z ),
						  texture2DCompare( shadowMap, uv + vec2( 2.0 * dx, -dy ), shadowCoord.z ),
						  f.x ),
					 mix( texture2DCompare( shadowMap, uv + vec2( -dx, 2.0 * dy ), shadowCoord.z ),
						  texture2DCompare( shadowMap, uv + vec2( 2.0 * dx, 2.0 * dy ), shadowCoord.z ),
						  f.x ),
					 f.y )
			) * ( 1.0 / 9.0 );
		#elif defined( SHADOWMAP_TYPE_VSM )
			shadow = VSMShadow( shadowMap, shadowCoord.xy, shadowCoord.z );
		#else
			shadow = texture2DCompare( shadowMap, shadowCoord.xy, shadowCoord.z );
		#endif
		}
		return mix( 1.0, shadow, shadowIntensity );
	}
	vec2 cubeToUV( vec3 v, float texelSizeY ) {
		vec3 absV = abs( v );
		float scaleToCube = 1.0 / max( absV.x, max( absV.y, absV.z ) );
		absV *= scaleToCube;
		v *= scaleToCube * ( 1.0 - 2.0 * texelSizeY );
		vec2 planar = v.xy;
		float almostATexel = 1.5 * texelSizeY;
		float almostOne = 1.0 - almostATexel;
		if ( absV.z >= almostOne ) {
			if ( v.z > 0.0 )
				planar.x = 4.0 - v.x;
		} else if ( absV.x >= almostOne ) {
			float signX = sign( v.x );
			planar.x = v.z * signX + 2.0 * signX;
		} else if ( absV.y >= almostOne ) {
			float signY = sign( v.y );
			planar.x = v.x + 2.0 * signY + 2.0;
			planar.y = v.z * signY - 2.0;
		}
		return vec2( 0.125, 0.25 ) * planar + vec2( 0.375, 0.75 );
	}
	float getPointShadow( sampler2D shadowMap, vec2 shadowMapSize, float shadowIntensity, float shadowBias, float shadowRadius, vec4 shadowCoord, float shadowCameraNear, float shadowCameraFar ) {
		float shadow = 1.0;
		vec3 lightToPosition = shadowCoord.xyz;
		
		float lightToPositionLength = length( lightToPosition );
		if ( lightToPositionLength - shadowCameraFar <= 0.0 && lightToPositionLength - shadowCameraNear >= 0.0 ) {
			float dp = ( lightToPositionLength - shadowCameraNear ) / ( shadowCameraFar - shadowCameraNear );			dp += shadowBias;
			vec3 bd3D = normalize( lightToPosition );
			vec2 texelSize = vec2( 1.0 ) / ( shadowMapSize * vec2( 4.0, 2.0 ) );
			#if defined( SHADOWMAP_TYPE_PCF ) || defined( SHADOWMAP_TYPE_PCF_SOFT ) || defined( SHADOWMAP_TYPE_VSM )
				vec2 offset = vec2( - 1, 1 ) * shadowRadius * texelSize.y;
				shadow = (
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.xyy, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.yyy, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.xyx, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.yyx, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.xxy, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.yxy, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.xxx, texelSize.y ), dp ) +
					texture2DCompare( shadowMap, cubeToUV( bd3D + offset.yxx, texelSize.y ), dp )
				) * ( 1.0 / 9.0 );
			#else
				shadow = texture2DCompare( shadowMap, cubeToUV( bd3D, texelSize.y ), dp );
			#endif
		}
		return mix( 1.0, shadow, shadowIntensity );
	}
#endif`,am=`#if NUM_SPOT_LIGHT_COORDS > 0
	uniform mat4 spotLightMatrix[ NUM_SPOT_LIGHT_COORDS ];
	varying vec4 vSpotLightCoord[ NUM_SPOT_LIGHT_COORDS ];
#endif
#ifdef USE_SHADOWMAP
	#if NUM_DIR_LIGHT_SHADOWS > 0
		uniform mat4 directionalShadowMatrix[ NUM_DIR_LIGHT_SHADOWS ];
		varying vec4 vDirectionalShadowCoord[ NUM_DIR_LIGHT_SHADOWS ];
		struct DirectionalLightShadow {
			float shadowIntensity;
			float shadowBias;
			float shadowNormalBias;
			float shadowRadius;
			vec2 shadowMapSize;
		};
		uniform DirectionalLightShadow directionalLightShadows[ NUM_DIR_LIGHT_SHADOWS ];
	#endif
	#if NUM_SPOT_LIGHT_SHADOWS > 0
		struct SpotLightShadow {
			float shadowIntensity;
			float shadowBias;
			float shadowNormalBias;
			float shadowRadius;
			vec2 shadowMapSize;
		};
		uniform SpotLightShadow spotLightShadows[ NUM_SPOT_LIGHT_SHADOWS ];
	#endif
	#if NUM_POINT_LIGHT_SHADOWS > 0
		uniform mat4 pointShadowMatrix[ NUM_POINT_LIGHT_SHADOWS ];
		varying vec4 vPointShadowCoord[ NUM_POINT_LIGHT_SHADOWS ];
		struct PointLightShadow {
			float shadowIntensity;
			float shadowBias;
			float shadowNormalBias;
			float shadowRadius;
			vec2 shadowMapSize;
			float shadowCameraNear;
			float shadowCameraFar;
		};
		uniform PointLightShadow pointLightShadows[ NUM_POINT_LIGHT_SHADOWS ];
	#endif
#endif`,om=`#if ( defined( USE_SHADOWMAP ) && ( NUM_DIR_LIGHT_SHADOWS > 0 || NUM_POINT_LIGHT_SHADOWS > 0 ) ) || ( NUM_SPOT_LIGHT_COORDS > 0 )
	vec3 shadowWorldNormal = inverseTransformDirection( transformedNormal, viewMatrix );
	vec4 shadowWorldPosition;
#endif
#if defined( USE_SHADOWMAP )
	#if NUM_DIR_LIGHT_SHADOWS > 0
		#pragma unroll_loop_start
		for ( int i = 0; i < NUM_DIR_LIGHT_SHADOWS; i ++ ) {
			shadowWorldPosition = worldPosition + vec4( shadowWorldNormal * directionalLightShadows[ i ].shadowNormalBias, 0 );
			vDirectionalShadowCoord[ i ] = directionalShadowMatrix[ i ] * shadowWorldPosition;
		}
		#pragma unroll_loop_end
	#endif
	#if NUM_POINT_LIGHT_SHADOWS > 0
		#pragma unroll_loop_start
		for ( int i = 0; i < NUM_POINT_LIGHT_SHADOWS; i ++ ) {
			shadowWorldPosition = worldPosition + vec4( shadowWorldNormal * pointLightShadows[ i ].shadowNormalBias, 0 );
			vPointShadowCoord[ i ] = pointShadowMatrix[ i ] * shadowWorldPosition;
		}
		#pragma unroll_loop_end
	#endif
#endif
#if NUM_SPOT_LIGHT_COORDS > 0
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_SPOT_LIGHT_COORDS; i ++ ) {
		shadowWorldPosition = worldPosition;
		#if ( defined( USE_SHADOWMAP ) && UNROLLED_LOOP_INDEX < NUM_SPOT_LIGHT_SHADOWS )
			shadowWorldPosition.xyz += shadowWorldNormal * spotLightShadows[ i ].shadowNormalBias;
		#endif
		vSpotLightCoord[ i ] = spotLightMatrix[ i ] * shadowWorldPosition;
	}
	#pragma unroll_loop_end
#endif`,lm=`float getShadowMask() {
	float shadow = 1.0;
	#ifdef USE_SHADOWMAP
	#if NUM_DIR_LIGHT_SHADOWS > 0
	DirectionalLightShadow directionalLight;
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_DIR_LIGHT_SHADOWS; i ++ ) {
		directionalLight = directionalLightShadows[ i ];
		shadow *= receiveShadow ? getShadow( directionalShadowMap[ i ], directionalLight.shadowMapSize, directionalLight.shadowIntensity, directionalLight.shadowBias, directionalLight.shadowRadius, vDirectionalShadowCoord[ i ] ) : 1.0;
	}
	#pragma unroll_loop_end
	#endif
	#if NUM_SPOT_LIGHT_SHADOWS > 0
	SpotLightShadow spotLight;
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_SPOT_LIGHT_SHADOWS; i ++ ) {
		spotLight = spotLightShadows[ i ];
		shadow *= receiveShadow ? getShadow( spotShadowMap[ i ], spotLight.shadowMapSize, spotLight.shadowIntensity, spotLight.shadowBias, spotLight.shadowRadius, vSpotLightCoord[ i ] ) : 1.0;
	}
	#pragma unroll_loop_end
	#endif
	#if NUM_POINT_LIGHT_SHADOWS > 0
	PointLightShadow pointLight;
	#pragma unroll_loop_start
	for ( int i = 0; i < NUM_POINT_LIGHT_SHADOWS; i ++ ) {
		pointLight = pointLightShadows[ i ];
		shadow *= receiveShadow ? getPointShadow( pointShadowMap[ i ], pointLight.shadowMapSize, pointLight.shadowIntensity, pointLight.shadowBias, pointLight.shadowRadius, vPointShadowCoord[ i ], pointLight.shadowCameraNear, pointLight.shadowCameraFar ) : 1.0;
	}
	#pragma unroll_loop_end
	#endif
	#endif
	return shadow;
}`,cm=`#ifdef USE_SKINNING
	mat4 boneMatX = getBoneMatrix( skinIndex.x );
	mat4 boneMatY = getBoneMatrix( skinIndex.y );
	mat4 boneMatZ = getBoneMatrix( skinIndex.z );
	mat4 boneMatW = getBoneMatrix( skinIndex.w );
#endif`,hm=`#ifdef USE_SKINNING
	uniform mat4 bindMatrix;
	uniform mat4 bindMatrixInverse;
	uniform highp sampler2D boneTexture;
	mat4 getBoneMatrix( const in float i ) {
		int size = textureSize( boneTexture, 0 ).x;
		int j = int( i ) * 4;
		int x = j % size;
		int y = j / size;
		vec4 v1 = texelFetch( boneTexture, ivec2( x, y ), 0 );
		vec4 v2 = texelFetch( boneTexture, ivec2( x + 1, y ), 0 );
		vec4 v3 = texelFetch( boneTexture, ivec2( x + 2, y ), 0 );
		vec4 v4 = texelFetch( boneTexture, ivec2( x + 3, y ), 0 );
		return mat4( v1, v2, v3, v4 );
	}
#endif`,um=`#ifdef USE_SKINNING
	vec4 skinVertex = bindMatrix * vec4( transformed, 1.0 );
	vec4 skinned = vec4( 0.0 );
	skinned += boneMatX * skinVertex * skinWeight.x;
	skinned += boneMatY * skinVertex * skinWeight.y;
	skinned += boneMatZ * skinVertex * skinWeight.z;
	skinned += boneMatW * skinVertex * skinWeight.w;
	transformed = ( bindMatrixInverse * skinned ).xyz;
#endif`,dm=`#ifdef USE_SKINNING
	mat4 skinMatrix = mat4( 0.0 );
	skinMatrix += skinWeight.x * boneMatX;
	skinMatrix += skinWeight.y * boneMatY;
	skinMatrix += skinWeight.z * boneMatZ;
	skinMatrix += skinWeight.w * boneMatW;
	skinMatrix = bindMatrixInverse * skinMatrix * bindMatrix;
	objectNormal = vec4( skinMatrix * vec4( objectNormal, 0.0 ) ).xyz;
	#ifdef USE_TANGENT
		objectTangent = vec4( skinMatrix * vec4( objectTangent, 0.0 ) ).xyz;
	#endif
#endif`,fm=`float specularStrength;
#ifdef USE_SPECULARMAP
	vec4 texelSpecular = texture2D( specularMap, vSpecularMapUv );
	specularStrength = texelSpecular.r;
#else
	specularStrength = 1.0;
#endif`,pm=`#ifdef USE_SPECULARMAP
	uniform sampler2D specularMap;
#endif`,mm=`#if defined( TONE_MAPPING )
	gl_FragColor.rgb = toneMapping( gl_FragColor.rgb );
#endif`,gm=`#ifndef saturate
#define saturate( a ) clamp( a, 0.0, 1.0 )
#endif
uniform float toneMappingExposure;
vec3 LinearToneMapping( vec3 color ) {
	return saturate( toneMappingExposure * color );
}
vec3 ReinhardToneMapping( vec3 color ) {
	color *= toneMappingExposure;
	return saturate( color / ( vec3( 1.0 ) + color ) );
}
vec3 CineonToneMapping( vec3 color ) {
	color *= toneMappingExposure;
	color = max( vec3( 0.0 ), color - 0.004 );
	return pow( ( color * ( 6.2 * color + 0.5 ) ) / ( color * ( 6.2 * color + 1.7 ) + 0.06 ), vec3( 2.2 ) );
}
vec3 RRTAndODTFit( vec3 v ) {
	vec3 a = v * ( v + 0.0245786 ) - 0.000090537;
	vec3 b = v * ( 0.983729 * v + 0.4329510 ) + 0.238081;
	return a / b;
}
vec3 ACESFilmicToneMapping( vec3 color ) {
	const mat3 ACESInputMat = mat3(
		vec3( 0.59719, 0.07600, 0.02840 ),		vec3( 0.35458, 0.90834, 0.13383 ),
		vec3( 0.04823, 0.01566, 0.83777 )
	);
	const mat3 ACESOutputMat = mat3(
		vec3(  1.60475, -0.10208, -0.00327 ),		vec3( -0.53108,  1.10813, -0.07276 ),
		vec3( -0.07367, -0.00605,  1.07602 )
	);
	color *= toneMappingExposure / 0.6;
	color = ACESInputMat * color;
	color = RRTAndODTFit( color );
	color = ACESOutputMat * color;
	return saturate( color );
}
const mat3 LINEAR_REC2020_TO_LINEAR_SRGB = mat3(
	vec3( 1.6605, - 0.1246, - 0.0182 ),
	vec3( - 0.5876, 1.1329, - 0.1006 ),
	vec3( - 0.0728, - 0.0083, 1.1187 )
);
const mat3 LINEAR_SRGB_TO_LINEAR_REC2020 = mat3(
	vec3( 0.6274, 0.0691, 0.0164 ),
	vec3( 0.3293, 0.9195, 0.0880 ),
	vec3( 0.0433, 0.0113, 0.8956 )
);
vec3 agxDefaultContrastApprox( vec3 x ) {
	vec3 x2 = x * x;
	vec3 x4 = x2 * x2;
	return + 15.5 * x4 * x2
		- 40.14 * x4 * x
		+ 31.96 * x4
		- 6.868 * x2 * x
		+ 0.4298 * x2
		+ 0.1191 * x
		- 0.00232;
}
vec3 AgXToneMapping( vec3 color ) {
	const mat3 AgXInsetMatrix = mat3(
		vec3( 0.856627153315983, 0.137318972929847, 0.11189821299995 ),
		vec3( 0.0951212405381588, 0.761241990602591, 0.0767994186031903 ),
		vec3( 0.0482516061458583, 0.101439036467562, 0.811302368396859 )
	);
	const mat3 AgXOutsetMatrix = mat3(
		vec3( 1.1271005818144368, - 0.1413297634984383, - 0.14132976349843826 ),
		vec3( - 0.11060664309660323, 1.157823702216272, - 0.11060664309660294 ),
		vec3( - 0.016493938717834573, - 0.016493938717834257, 1.2519364065950405 )
	);
	const float AgxMinEv = - 12.47393;	const float AgxMaxEv = 4.026069;
	color *= toneMappingExposure;
	color = LINEAR_SRGB_TO_LINEAR_REC2020 * color;
	color = AgXInsetMatrix * color;
	color = max( color, 1e-10 );	color = log2( color );
	color = ( color - AgxMinEv ) / ( AgxMaxEv - AgxMinEv );
	color = clamp( color, 0.0, 1.0 );
	color = agxDefaultContrastApprox( color );
	color = AgXOutsetMatrix * color;
	color = pow( max( vec3( 0.0 ), color ), vec3( 2.2 ) );
	color = LINEAR_REC2020_TO_LINEAR_SRGB * color;
	color = clamp( color, 0.0, 1.0 );
	return color;
}
vec3 NeutralToneMapping( vec3 color ) {
	const float StartCompression = 0.8 - 0.04;
	const float Desaturation = 0.15;
	color *= toneMappingExposure;
	float x = min( color.r, min( color.g, color.b ) );
	float offset = x < 0.08 ? x - 6.25 * x * x : 0.04;
	color -= offset;
	float peak = max( color.r, max( color.g, color.b ) );
	if ( peak < StartCompression ) return color;
	float d = 1. - StartCompression;
	float newPeak = 1. - d * d / ( peak + d - StartCompression );
	color *= newPeak / peak;
	float g = 1. - 1. / ( Desaturation * ( peak - newPeak ) + 1. );
	return mix( color, vec3( newPeak ), g );
}
vec3 CustomToneMapping( vec3 color ) { return color; }`,_m=`#ifdef USE_TRANSMISSION
	material.transmission = transmission;
	material.transmissionAlpha = 1.0;
	material.thickness = thickness;
	material.attenuationDistance = attenuationDistance;
	material.attenuationColor = attenuationColor;
	#ifdef USE_TRANSMISSIONMAP
		material.transmission *= texture2D( transmissionMap, vTransmissionMapUv ).r;
	#endif
	#ifdef USE_THICKNESSMAP
		material.thickness *= texture2D( thicknessMap, vThicknessMapUv ).g;
	#endif
	vec3 pos = vWorldPosition;
	vec3 v = normalize( cameraPosition - pos );
	vec3 n = inverseTransformDirection( normal, viewMatrix );
	vec4 transmitted = getIBLVolumeRefraction(
		n, v, material.roughness, material.diffuseColor, material.specularColor, material.specularF90,
		pos, modelMatrix, viewMatrix, projectionMatrix, material.dispersion, material.ior, material.thickness,
		material.attenuationColor, material.attenuationDistance );
	material.transmissionAlpha = mix( material.transmissionAlpha, transmitted.a, material.transmission );
	totalDiffuse = mix( totalDiffuse, transmitted.rgb, material.transmission );
#endif`,xm=`#ifdef USE_TRANSMISSION
	uniform float transmission;
	uniform float thickness;
	uniform float attenuationDistance;
	uniform vec3 attenuationColor;
	#ifdef USE_TRANSMISSIONMAP
		uniform sampler2D transmissionMap;
	#endif
	#ifdef USE_THICKNESSMAP
		uniform sampler2D thicknessMap;
	#endif
	uniform vec2 transmissionSamplerSize;
	uniform sampler2D transmissionSamplerMap;
	uniform mat4 modelMatrix;
	uniform mat4 projectionMatrix;
	varying vec3 vWorldPosition;
	float w0( float a ) {
		return ( 1.0 / 6.0 ) * ( a * ( a * ( - a + 3.0 ) - 3.0 ) + 1.0 );
	}
	float w1( float a ) {
		return ( 1.0 / 6.0 ) * ( a *  a * ( 3.0 * a - 6.0 ) + 4.0 );
	}
	float w2( float a ){
		return ( 1.0 / 6.0 ) * ( a * ( a * ( - 3.0 * a + 3.0 ) + 3.0 ) + 1.0 );
	}
	float w3( float a ) {
		return ( 1.0 / 6.0 ) * ( a * a * a );
	}
	float g0( float a ) {
		return w0( a ) + w1( a );
	}
	float g1( float a ) {
		return w2( a ) + w3( a );
	}
	float h0( float a ) {
		return - 1.0 + w1( a ) / ( w0( a ) + w1( a ) );
	}
	float h1( float a ) {
		return 1.0 + w3( a ) / ( w2( a ) + w3( a ) );
	}
	vec4 bicubic( sampler2D tex, vec2 uv, vec4 texelSize, float lod ) {
		uv = uv * texelSize.zw + 0.5;
		vec2 iuv = floor( uv );
		vec2 fuv = fract( uv );
		float g0x = g0( fuv.x );
		float g1x = g1( fuv.x );
		float h0x = h0( fuv.x );
		float h1x = h1( fuv.x );
		float h0y = h0( fuv.y );
		float h1y = h1( fuv.y );
		vec2 p0 = ( vec2( iuv.x + h0x, iuv.y + h0y ) - 0.5 ) * texelSize.xy;
		vec2 p1 = ( vec2( iuv.x + h1x, iuv.y + h0y ) - 0.5 ) * texelSize.xy;
		vec2 p2 = ( vec2( iuv.x + h0x, iuv.y + h1y ) - 0.5 ) * texelSize.xy;
		vec2 p3 = ( vec2( iuv.x + h1x, iuv.y + h1y ) - 0.5 ) * texelSize.xy;
		return g0( fuv.y ) * ( g0x * textureLod( tex, p0, lod ) + g1x * textureLod( tex, p1, lod ) ) +
			g1( fuv.y ) * ( g0x * textureLod( tex, p2, lod ) + g1x * textureLod( tex, p3, lod ) );
	}
	vec4 textureBicubic( sampler2D sampler, vec2 uv, float lod ) {
		vec2 fLodSize = vec2( textureSize( sampler, int( lod ) ) );
		vec2 cLodSize = vec2( textureSize( sampler, int( lod + 1.0 ) ) );
		vec2 fLodSizeInv = 1.0 / fLodSize;
		vec2 cLodSizeInv = 1.0 / cLodSize;
		vec4 fSample = bicubic( sampler, uv, vec4( fLodSizeInv, fLodSize ), floor( lod ) );
		vec4 cSample = bicubic( sampler, uv, vec4( cLodSizeInv, cLodSize ), ceil( lod ) );
		return mix( fSample, cSample, fract( lod ) );
	}
	vec3 getVolumeTransmissionRay( const in vec3 n, const in vec3 v, const in float thickness, const in float ior, const in mat4 modelMatrix ) {
		vec3 refractionVector = refract( - v, normalize( n ), 1.0 / ior );
		vec3 modelScale;
		modelScale.x = length( vec3( modelMatrix[ 0 ].xyz ) );
		modelScale.y = length( vec3( modelMatrix[ 1 ].xyz ) );
		modelScale.z = length( vec3( modelMatrix[ 2 ].xyz ) );
		return normalize( refractionVector ) * thickness * modelScale;
	}
	float applyIorToRoughness( const in float roughness, const in float ior ) {
		return roughness * clamp( ior * 2.0 - 2.0, 0.0, 1.0 );
	}
	vec4 getTransmissionSample( const in vec2 fragCoord, const in float roughness, const in float ior ) {
		float lod = log2( transmissionSamplerSize.x ) * applyIorToRoughness( roughness, ior );
		return textureBicubic( transmissionSamplerMap, fragCoord.xy, lod );
	}
	vec3 volumeAttenuation( const in float transmissionDistance, const in vec3 attenuationColor, const in float attenuationDistance ) {
		if ( isinf( attenuationDistance ) ) {
			return vec3( 1.0 );
		} else {
			vec3 attenuationCoefficient = -log( attenuationColor ) / attenuationDistance;
			vec3 transmittance = exp( - attenuationCoefficient * transmissionDistance );			return transmittance;
		}
	}
	vec4 getIBLVolumeRefraction( const in vec3 n, const in vec3 v, const in float roughness, const in vec3 diffuseColor,
		const in vec3 specularColor, const in float specularF90, const in vec3 position, const in mat4 modelMatrix,
		const in mat4 viewMatrix, const in mat4 projMatrix, const in float dispersion, const in float ior, const in float thickness,
		const in vec3 attenuationColor, const in float attenuationDistance ) {
		vec4 transmittedLight;
		vec3 transmittance;
		#ifdef USE_DISPERSION
			float halfSpread = ( ior - 1.0 ) * 0.025 * dispersion;
			vec3 iors = vec3( ior - halfSpread, ior, ior + halfSpread );
			for ( int i = 0; i < 3; i ++ ) {
				vec3 transmissionRay = getVolumeTransmissionRay( n, v, thickness, iors[ i ], modelMatrix );
				vec3 refractedRayExit = position + transmissionRay;
				vec4 ndcPos = projMatrix * viewMatrix * vec4( refractedRayExit, 1.0 );
				vec2 refractionCoords = ndcPos.xy / ndcPos.w;
				refractionCoords += 1.0;
				refractionCoords /= 2.0;
				vec4 transmissionSample = getTransmissionSample( refractionCoords, roughness, iors[ i ] );
				transmittedLight[ i ] = transmissionSample[ i ];
				transmittedLight.a += transmissionSample.a;
				transmittance[ i ] = diffuseColor[ i ] * volumeAttenuation( length( transmissionRay ), attenuationColor, attenuationDistance )[ i ];
			}
			transmittedLight.a /= 3.0;
		#else
			vec3 transmissionRay = getVolumeTransmissionRay( n, v, thickness, ior, modelMatrix );
			vec3 refractedRayExit = position + transmissionRay;
			vec4 ndcPos = projMatrix * viewMatrix * vec4( refractedRayExit, 1.0 );
			vec2 refractionCoords = ndcPos.xy / ndcPos.w;
			refractionCoords += 1.0;
			refractionCoords /= 2.0;
			transmittedLight = getTransmissionSample( refractionCoords, roughness, ior );
			transmittance = diffuseColor * volumeAttenuation( length( transmissionRay ), attenuationColor, attenuationDistance );
		#endif
		vec3 attenuatedColor = transmittance * transmittedLight.rgb;
		vec3 F = EnvironmentBRDF( n, v, specularColor, specularF90, roughness );
		float transmittanceFactor = ( transmittance.r + transmittance.g + transmittance.b ) / 3.0;
		return vec4( ( 1.0 - F ) * attenuatedColor, 1.0 - ( 1.0 - transmittedLight.a ) * transmittanceFactor );
	}
#endif`,vm=`#if defined( USE_UV ) || defined( USE_ANISOTROPY )
	varying vec2 vUv;
#endif
#ifdef USE_MAP
	varying vec2 vMapUv;
#endif
#ifdef USE_ALPHAMAP
	varying vec2 vAlphaMapUv;
#endif
#ifdef USE_LIGHTMAP
	varying vec2 vLightMapUv;
#endif
#ifdef USE_AOMAP
	varying vec2 vAoMapUv;
#endif
#ifdef USE_BUMPMAP
	varying vec2 vBumpMapUv;
#endif
#ifdef USE_NORMALMAP
	varying vec2 vNormalMapUv;
#endif
#ifdef USE_EMISSIVEMAP
	varying vec2 vEmissiveMapUv;
#endif
#ifdef USE_METALNESSMAP
	varying vec2 vMetalnessMapUv;
#endif
#ifdef USE_ROUGHNESSMAP
	varying vec2 vRoughnessMapUv;
#endif
#ifdef USE_ANISOTROPYMAP
	varying vec2 vAnisotropyMapUv;
#endif
#ifdef USE_CLEARCOATMAP
	varying vec2 vClearcoatMapUv;
#endif
#ifdef USE_CLEARCOAT_NORMALMAP
	varying vec2 vClearcoatNormalMapUv;
#endif
#ifdef USE_CLEARCOAT_ROUGHNESSMAP
	varying vec2 vClearcoatRoughnessMapUv;
#endif
#ifdef USE_IRIDESCENCEMAP
	varying vec2 vIridescenceMapUv;
#endif
#ifdef USE_IRIDESCENCE_THICKNESSMAP
	varying vec2 vIridescenceThicknessMapUv;
#endif
#ifdef USE_SHEEN_COLORMAP
	varying vec2 vSheenColorMapUv;
#endif
#ifdef USE_SHEEN_ROUGHNESSMAP
	varying vec2 vSheenRoughnessMapUv;
#endif
#ifdef USE_SPECULARMAP
	varying vec2 vSpecularMapUv;
#endif
#ifdef USE_SPECULAR_COLORMAP
	varying vec2 vSpecularColorMapUv;
#endif
#ifdef USE_SPECULAR_INTENSITYMAP
	varying vec2 vSpecularIntensityMapUv;
#endif
#ifdef USE_TRANSMISSIONMAP
	uniform mat3 transmissionMapTransform;
	varying vec2 vTransmissionMapUv;
#endif
#ifdef USE_THICKNESSMAP
	uniform mat3 thicknessMapTransform;
	varying vec2 vThicknessMapUv;
#endif`,ym=`#if defined( USE_UV ) || defined( USE_ANISOTROPY )
	varying vec2 vUv;
#endif
#ifdef USE_MAP
	uniform mat3 mapTransform;
	varying vec2 vMapUv;
#endif
#ifdef USE_ALPHAMAP
	uniform mat3 alphaMapTransform;
	varying vec2 vAlphaMapUv;
#endif
#ifdef USE_LIGHTMAP
	uniform mat3 lightMapTransform;
	varying vec2 vLightMapUv;
#endif
#ifdef USE_AOMAP
	uniform mat3 aoMapTransform;
	varying vec2 vAoMapUv;
#endif
#ifdef USE_BUMPMAP
	uniform mat3 bumpMapTransform;
	varying vec2 vBumpMapUv;
#endif
#ifdef USE_NORMALMAP
	uniform mat3 normalMapTransform;
	varying vec2 vNormalMapUv;
#endif
#ifdef USE_DISPLACEMENTMAP
	uniform mat3 displacementMapTransform;
	varying vec2 vDisplacementMapUv;
#endif
#ifdef USE_EMISSIVEMAP
	uniform mat3 emissiveMapTransform;
	varying vec2 vEmissiveMapUv;
#endif
#ifdef USE_METALNESSMAP
	uniform mat3 metalnessMapTransform;
	varying vec2 vMetalnessMapUv;
#endif
#ifdef USE_ROUGHNESSMAP
	uniform mat3 roughnessMapTransform;
	varying vec2 vRoughnessMapUv;
#endif
#ifdef USE_ANISOTROPYMAP
	uniform mat3 anisotropyMapTransform;
	varying vec2 vAnisotropyMapUv;
#endif
#ifdef USE_CLEARCOATMAP
	uniform mat3 clearcoatMapTransform;
	varying vec2 vClearcoatMapUv;
#endif
#ifdef USE_CLEARCOAT_NORMALMAP
	uniform mat3 clearcoatNormalMapTransform;
	varying vec2 vClearcoatNormalMapUv;
#endif
#ifdef USE_CLEARCOAT_ROUGHNESSMAP
	uniform mat3 clearcoatRoughnessMapTransform;
	varying vec2 vClearcoatRoughnessMapUv;
#endif
#ifdef USE_SHEEN_COLORMAP
	uniform mat3 sheenColorMapTransform;
	varying vec2 vSheenColorMapUv;
#endif
#ifdef USE_SHEEN_ROUGHNESSMAP
	uniform mat3 sheenRoughnessMapTransform;
	varying vec2 vSheenRoughnessMapUv;
#endif
#ifdef USE_IRIDESCENCEMAP
	uniform mat3 iridescenceMapTransform;
	varying vec2 vIridescenceMapUv;
#endif
#ifdef USE_IRIDESCENCE_THICKNESSMAP
	uniform mat3 iridescenceThicknessMapTransform;
	varying vec2 vIridescenceThicknessMapUv;
#endif
#ifdef USE_SPECULARMAP
	uniform mat3 specularMapTransform;
	varying vec2 vSpecularMapUv;
#endif
#ifdef USE_SPECULAR_COLORMAP
	uniform mat3 specularColorMapTransform;
	varying vec2 vSpecularColorMapUv;
#endif
#ifdef USE_SPECULAR_INTENSITYMAP
	uniform mat3 specularIntensityMapTransform;
	varying vec2 vSpecularIntensityMapUv;
#endif
#ifdef USE_TRANSMISSIONMAP
	uniform mat3 transmissionMapTransform;
	varying vec2 vTransmissionMapUv;
#endif
#ifdef USE_THICKNESSMAP
	uniform mat3 thicknessMapTransform;
	varying vec2 vThicknessMapUv;
#endif`,Mm=`#if defined( USE_UV ) || defined( USE_ANISOTROPY )
	vUv = vec3( uv, 1 ).xy;
#endif
#ifdef USE_MAP
	vMapUv = ( mapTransform * vec3( MAP_UV, 1 ) ).xy;
#endif
#ifdef USE_ALPHAMAP
	vAlphaMapUv = ( alphaMapTransform * vec3( ALPHAMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_LIGHTMAP
	vLightMapUv = ( lightMapTransform * vec3( LIGHTMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_AOMAP
	vAoMapUv = ( aoMapTransform * vec3( AOMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_BUMPMAP
	vBumpMapUv = ( bumpMapTransform * vec3( BUMPMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_NORMALMAP
	vNormalMapUv = ( normalMapTransform * vec3( NORMALMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_DISPLACEMENTMAP
	vDisplacementMapUv = ( displacementMapTransform * vec3( DISPLACEMENTMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_EMISSIVEMAP
	vEmissiveMapUv = ( emissiveMapTransform * vec3( EMISSIVEMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_METALNESSMAP
	vMetalnessMapUv = ( metalnessMapTransform * vec3( METALNESSMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_ROUGHNESSMAP
	vRoughnessMapUv = ( roughnessMapTransform * vec3( ROUGHNESSMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_ANISOTROPYMAP
	vAnisotropyMapUv = ( anisotropyMapTransform * vec3( ANISOTROPYMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_CLEARCOATMAP
	vClearcoatMapUv = ( clearcoatMapTransform * vec3( CLEARCOATMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_CLEARCOAT_NORMALMAP
	vClearcoatNormalMapUv = ( clearcoatNormalMapTransform * vec3( CLEARCOAT_NORMALMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_CLEARCOAT_ROUGHNESSMAP
	vClearcoatRoughnessMapUv = ( clearcoatRoughnessMapTransform * vec3( CLEARCOAT_ROUGHNESSMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_IRIDESCENCEMAP
	vIridescenceMapUv = ( iridescenceMapTransform * vec3( IRIDESCENCEMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_IRIDESCENCE_THICKNESSMAP
	vIridescenceThicknessMapUv = ( iridescenceThicknessMapTransform * vec3( IRIDESCENCE_THICKNESSMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_SHEEN_COLORMAP
	vSheenColorMapUv = ( sheenColorMapTransform * vec3( SHEEN_COLORMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_SHEEN_ROUGHNESSMAP
	vSheenRoughnessMapUv = ( sheenRoughnessMapTransform * vec3( SHEEN_ROUGHNESSMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_SPECULARMAP
	vSpecularMapUv = ( specularMapTransform * vec3( SPECULARMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_SPECULAR_COLORMAP
	vSpecularColorMapUv = ( specularColorMapTransform * vec3( SPECULAR_COLORMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_SPECULAR_INTENSITYMAP
	vSpecularIntensityMapUv = ( specularIntensityMapTransform * vec3( SPECULAR_INTENSITYMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_TRANSMISSIONMAP
	vTransmissionMapUv = ( transmissionMapTransform * vec3( TRANSMISSIONMAP_UV, 1 ) ).xy;
#endif
#ifdef USE_THICKNESSMAP
	vThicknessMapUv = ( thicknessMapTransform * vec3( THICKNESSMAP_UV, 1 ) ).xy;
#endif`,Sm=`#if defined( USE_ENVMAP ) || defined( DISTANCE ) || defined ( USE_SHADOWMAP ) || defined ( USE_TRANSMISSION ) || NUM_SPOT_LIGHT_COORDS > 0
	vec4 worldPosition = vec4( transformed, 1.0 );
	#ifdef USE_BATCHING
		worldPosition = batchingMatrix * worldPosition;
	#endif
	#ifdef USE_INSTANCING
		worldPosition = instanceMatrix * worldPosition;
	#endif
	worldPosition = modelMatrix * worldPosition;
#endif`,bm=`varying vec2 vUv;
uniform mat3 uvTransform;
void main() {
	vUv = ( uvTransform * vec3( uv, 1 ) ).xy;
	gl_Position = vec4( position.xy, 1.0, 1.0 );
}`,Em=`uniform sampler2D t2D;
uniform float backgroundIntensity;
varying vec2 vUv;
void main() {
	vec4 texColor = texture2D( t2D, vUv );
	#ifdef DECODE_VIDEO_TEXTURE
		texColor = vec4( mix( pow( texColor.rgb * 0.9478672986 + vec3( 0.0521327014 ), vec3( 2.4 ) ), texColor.rgb * 0.0773993808, vec3( lessThanEqual( texColor.rgb, vec3( 0.04045 ) ) ) ), texColor.w );
	#endif
	texColor.rgb *= backgroundIntensity;
	gl_FragColor = texColor;
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
}`,Tm=`varying vec3 vWorldDirection;
#include <common>
void main() {
	vWorldDirection = transformDirection( position, modelMatrix );
	#include <begin_vertex>
	#include <project_vertex>
	gl_Position.z = gl_Position.w;
}`,wm=`#ifdef ENVMAP_TYPE_CUBE
	uniform samplerCube envMap;
#elif defined( ENVMAP_TYPE_CUBE_UV )
	uniform sampler2D envMap;
#endif
uniform float flipEnvMap;
uniform float backgroundBlurriness;
uniform float backgroundIntensity;
uniform mat3 backgroundRotation;
varying vec3 vWorldDirection;
#include <cube_uv_reflection_fragment>
void main() {
	#ifdef ENVMAP_TYPE_CUBE
		vec4 texColor = textureCube( envMap, backgroundRotation * vec3( flipEnvMap * vWorldDirection.x, vWorldDirection.yz ) );
	#elif defined( ENVMAP_TYPE_CUBE_UV )
		vec4 texColor = textureCubeUV( envMap, backgroundRotation * vWorldDirection, backgroundBlurriness );
	#else
		vec4 texColor = vec4( 0.0, 0.0, 0.0, 1.0 );
	#endif
	texColor.rgb *= backgroundIntensity;
	gl_FragColor = texColor;
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
}`,Am=`varying vec3 vWorldDirection;
#include <common>
void main() {
	vWorldDirection = transformDirection( position, modelMatrix );
	#include <begin_vertex>
	#include <project_vertex>
	gl_Position.z = gl_Position.w;
}`,Rm=`uniform samplerCube tCube;
uniform float tFlip;
uniform float opacity;
varying vec3 vWorldDirection;
void main() {
	vec4 texColor = textureCube( tCube, vec3( tFlip * vWorldDirection.x, vWorldDirection.yz ) );
	gl_FragColor = texColor;
	gl_FragColor.a *= opacity;
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
}`,Cm=`#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <displacementmap_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
varying vec2 vHighPrecisionZW;
void main() {
	#include <uv_vertex>
	#include <batching_vertex>
	#include <skinbase_vertex>
	#include <morphinstance_vertex>
	#ifdef USE_DISPLACEMENTMAP
		#include <beginnormal_vertex>
		#include <morphnormal_vertex>
		#include <skinnormal_vertex>
	#endif
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	vHighPrecisionZW = gl_Position.zw;
}`,Im=`#if DEPTH_PACKING == 3200
	uniform float opacity;
#endif
#include <common>
#include <packing>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
varying vec2 vHighPrecisionZW;
void main() {
	vec4 diffuseColor = vec4( 1.0 );
	#include <clipping_planes_fragment>
	#if DEPTH_PACKING == 3200
		diffuseColor.a = opacity;
	#endif
	#include <map_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	#include <logdepthbuf_fragment>
	#ifdef USE_REVERSED_DEPTH_BUFFER
		float fragCoordZ = vHighPrecisionZW[ 0 ] / vHighPrecisionZW[ 1 ];
	#else
		float fragCoordZ = 0.5 * vHighPrecisionZW[ 0 ] / vHighPrecisionZW[ 1 ] + 0.5;
	#endif
	#if DEPTH_PACKING == 3200
		gl_FragColor = vec4( vec3( 1.0 - fragCoordZ ), opacity );
	#elif DEPTH_PACKING == 3201
		gl_FragColor = packDepthToRGBA( fragCoordZ );
	#elif DEPTH_PACKING == 3202
		gl_FragColor = vec4( packDepthToRGB( fragCoordZ ), 1.0 );
	#elif DEPTH_PACKING == 3203
		gl_FragColor = vec4( packDepthToRG( fragCoordZ ), 0.0, 1.0 );
	#endif
}`,Pm=`#define DISTANCE
varying vec3 vWorldPosition;
#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <displacementmap_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <batching_vertex>
	#include <skinbase_vertex>
	#include <morphinstance_vertex>
	#ifdef USE_DISPLACEMENTMAP
		#include <beginnormal_vertex>
		#include <morphnormal_vertex>
		#include <skinnormal_vertex>
	#endif
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <worldpos_vertex>
	#include <clipping_planes_vertex>
	vWorldPosition = worldPosition.xyz;
}`,Lm=`#define DISTANCE
uniform vec3 referencePosition;
uniform float nearDistance;
uniform float farDistance;
varying vec3 vWorldPosition;
#include <common>
#include <packing>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <clipping_planes_pars_fragment>
void main () {
	vec4 diffuseColor = vec4( 1.0 );
	#include <clipping_planes_fragment>
	#include <map_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	float dist = length( vWorldPosition - referencePosition );
	dist = ( dist - nearDistance ) / ( farDistance - nearDistance );
	dist = saturate( dist );
	gl_FragColor = packDepthToRGBA( dist );
}`,Dm=`varying vec3 vWorldDirection;
#include <common>
void main() {
	vWorldDirection = transformDirection( position, modelMatrix );
	#include <begin_vertex>
	#include <project_vertex>
}`,Um=`uniform sampler2D tEquirect;
varying vec3 vWorldDirection;
#include <common>
void main() {
	vec3 direction = normalize( vWorldDirection );
	vec2 sampleUV = equirectUv( direction );
	gl_FragColor = texture2D( tEquirect, sampleUV );
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
}`,Nm=`uniform float scale;
attribute float lineDistance;
varying float vLineDistance;
#include <common>
#include <uv_pars_vertex>
#include <color_pars_vertex>
#include <fog_pars_vertex>
#include <morphtarget_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	vLineDistance = scale * lineDistance;
	#include <uv_vertex>
	#include <color_vertex>
	#include <morphinstance_vertex>
	#include <morphcolor_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	#include <fog_vertex>
}`,Fm=`uniform vec3 diffuse;
uniform float opacity;
uniform float dashSize;
uniform float totalSize;
varying float vLineDistance;
#include <common>
#include <color_pars_fragment>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <fog_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	if ( mod( vLineDistance, totalSize ) > dashSize ) {
		discard;
	}
	vec3 outgoingLight = vec3( 0.0 );
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <color_fragment>
	outgoingLight = diffuseColor.rgb;
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
}`,Om=`#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <envmap_pars_vertex>
#include <color_pars_vertex>
#include <fog_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <color_vertex>
	#include <morphinstance_vertex>
	#include <morphcolor_vertex>
	#include <batching_vertex>
	#if defined ( USE_ENVMAP ) || defined ( USE_SKINNING )
		#include <beginnormal_vertex>
		#include <morphnormal_vertex>
		#include <skinbase_vertex>
		#include <skinnormal_vertex>
		#include <defaultnormal_vertex>
	#endif
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	#include <worldpos_vertex>
	#include <envmap_vertex>
	#include <fog_vertex>
}`,km=`uniform vec3 diffuse;
uniform float opacity;
#ifndef FLAT_SHADED
	varying vec3 vNormal;
#endif
#include <common>
#include <dithering_pars_fragment>
#include <color_pars_fragment>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <aomap_pars_fragment>
#include <lightmap_pars_fragment>
#include <envmap_common_pars_fragment>
#include <envmap_pars_fragment>
#include <fog_pars_fragment>
#include <specularmap_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <color_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	#include <specularmap_fragment>
	ReflectedLight reflectedLight = ReflectedLight( vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ) );
	#ifdef USE_LIGHTMAP
		vec4 lightMapTexel = texture2D( lightMap, vLightMapUv );
		reflectedLight.indirectDiffuse += lightMapTexel.rgb * lightMapIntensity * RECIPROCAL_PI;
	#else
		reflectedLight.indirectDiffuse += vec3( 1.0 );
	#endif
	#include <aomap_fragment>
	reflectedLight.indirectDiffuse *= diffuseColor.rgb;
	vec3 outgoingLight = reflectedLight.indirectDiffuse;
	#include <envmap_fragment>
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
	#include <dithering_fragment>
}`,Bm=`#define LAMBERT
varying vec3 vViewPosition;
#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <displacementmap_pars_vertex>
#include <envmap_pars_vertex>
#include <color_pars_vertex>
#include <fog_pars_vertex>
#include <normal_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <shadowmap_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <color_vertex>
	#include <morphinstance_vertex>
	#include <morphcolor_vertex>
	#include <batching_vertex>
	#include <beginnormal_vertex>
	#include <morphnormal_vertex>
	#include <skinbase_vertex>
	#include <skinnormal_vertex>
	#include <defaultnormal_vertex>
	#include <normal_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	vViewPosition = - mvPosition.xyz;
	#include <worldpos_vertex>
	#include <envmap_vertex>
	#include <shadowmap_vertex>
	#include <fog_vertex>
}`,zm=`#define LAMBERT
uniform vec3 diffuse;
uniform vec3 emissive;
uniform float opacity;
#include <common>
#include <packing>
#include <dithering_pars_fragment>
#include <color_pars_fragment>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <aomap_pars_fragment>
#include <lightmap_pars_fragment>
#include <emissivemap_pars_fragment>
#include <envmap_common_pars_fragment>
#include <envmap_pars_fragment>
#include <fog_pars_fragment>
#include <bsdfs>
#include <lights_pars_begin>
#include <normal_pars_fragment>
#include <lights_lambert_pars_fragment>
#include <shadowmap_pars_fragment>
#include <bumpmap_pars_fragment>
#include <normalmap_pars_fragment>
#include <specularmap_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	ReflectedLight reflectedLight = ReflectedLight( vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ) );
	vec3 totalEmissiveRadiance = emissive;
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <color_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	#include <specularmap_fragment>
	#include <normal_fragment_begin>
	#include <normal_fragment_maps>
	#include <emissivemap_fragment>
	#include <lights_lambert_fragment>
	#include <lights_fragment_begin>
	#include <lights_fragment_maps>
	#include <lights_fragment_end>
	#include <aomap_fragment>
	vec3 outgoingLight = reflectedLight.directDiffuse + reflectedLight.indirectDiffuse + totalEmissiveRadiance;
	#include <envmap_fragment>
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
	#include <dithering_fragment>
}`,Vm=`#define MATCAP
varying vec3 vViewPosition;
#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <color_pars_vertex>
#include <displacementmap_pars_vertex>
#include <fog_pars_vertex>
#include <normal_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <color_vertex>
	#include <morphinstance_vertex>
	#include <morphcolor_vertex>
	#include <batching_vertex>
	#include <beginnormal_vertex>
	#include <morphnormal_vertex>
	#include <skinbase_vertex>
	#include <skinnormal_vertex>
	#include <defaultnormal_vertex>
	#include <normal_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	#include <fog_vertex>
	vViewPosition = - mvPosition.xyz;
}`,Gm=`#define MATCAP
uniform vec3 diffuse;
uniform float opacity;
uniform sampler2D matcap;
varying vec3 vViewPosition;
#include <common>
#include <dithering_pars_fragment>
#include <color_pars_fragment>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <fog_pars_fragment>
#include <normal_pars_fragment>
#include <bumpmap_pars_fragment>
#include <normalmap_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <color_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	#include <normal_fragment_begin>
	#include <normal_fragment_maps>
	vec3 viewDir = normalize( vViewPosition );
	vec3 x = normalize( vec3( viewDir.z, 0.0, - viewDir.x ) );
	vec3 y = cross( viewDir, x );
	vec2 uv = vec2( dot( x, normal ), dot( y, normal ) ) * 0.495 + 0.5;
	#ifdef USE_MATCAP
		vec4 matcapColor = texture2D( matcap, uv );
	#else
		vec4 matcapColor = vec4( vec3( mix( 0.2, 0.8, uv.y ) ), 1.0 );
	#endif
	vec3 outgoingLight = diffuseColor.rgb * matcapColor.rgb;
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
	#include <dithering_fragment>
}`,Hm=`#define NORMAL
#if defined( FLAT_SHADED ) || defined( USE_BUMPMAP ) || defined( USE_NORMALMAP_TANGENTSPACE )
	varying vec3 vViewPosition;
#endif
#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <displacementmap_pars_vertex>
#include <normal_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <batching_vertex>
	#include <beginnormal_vertex>
	#include <morphinstance_vertex>
	#include <morphnormal_vertex>
	#include <skinbase_vertex>
	#include <skinnormal_vertex>
	#include <defaultnormal_vertex>
	#include <normal_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
#if defined( FLAT_SHADED ) || defined( USE_BUMPMAP ) || defined( USE_NORMALMAP_TANGENTSPACE )
	vViewPosition = - mvPosition.xyz;
#endif
}`,Wm=`#define NORMAL
uniform float opacity;
#if defined( FLAT_SHADED ) || defined( USE_BUMPMAP ) || defined( USE_NORMALMAP_TANGENTSPACE )
	varying vec3 vViewPosition;
#endif
#include <packing>
#include <uv_pars_fragment>
#include <normal_pars_fragment>
#include <bumpmap_pars_fragment>
#include <normalmap_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( 0.0, 0.0, 0.0, opacity );
	#include <clipping_planes_fragment>
	#include <logdepthbuf_fragment>
	#include <normal_fragment_begin>
	#include <normal_fragment_maps>
	gl_FragColor = vec4( packNormalToRGB( normal ), diffuseColor.a );
	#ifdef OPAQUE
		gl_FragColor.a = 1.0;
	#endif
}`,Xm=`#define PHONG
varying vec3 vViewPosition;
#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <displacementmap_pars_vertex>
#include <envmap_pars_vertex>
#include <color_pars_vertex>
#include <fog_pars_vertex>
#include <normal_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <shadowmap_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <color_vertex>
	#include <morphcolor_vertex>
	#include <batching_vertex>
	#include <beginnormal_vertex>
	#include <morphinstance_vertex>
	#include <morphnormal_vertex>
	#include <skinbase_vertex>
	#include <skinnormal_vertex>
	#include <defaultnormal_vertex>
	#include <normal_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	vViewPosition = - mvPosition.xyz;
	#include <worldpos_vertex>
	#include <envmap_vertex>
	#include <shadowmap_vertex>
	#include <fog_vertex>
}`,qm=`#define PHONG
uniform vec3 diffuse;
uniform vec3 emissive;
uniform vec3 specular;
uniform float shininess;
uniform float opacity;
#include <common>
#include <packing>
#include <dithering_pars_fragment>
#include <color_pars_fragment>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <aomap_pars_fragment>
#include <lightmap_pars_fragment>
#include <emissivemap_pars_fragment>
#include <envmap_common_pars_fragment>
#include <envmap_pars_fragment>
#include <fog_pars_fragment>
#include <bsdfs>
#include <lights_pars_begin>
#include <normal_pars_fragment>
#include <lights_phong_pars_fragment>
#include <shadowmap_pars_fragment>
#include <bumpmap_pars_fragment>
#include <normalmap_pars_fragment>
#include <specularmap_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	ReflectedLight reflectedLight = ReflectedLight( vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ) );
	vec3 totalEmissiveRadiance = emissive;
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <color_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	#include <specularmap_fragment>
	#include <normal_fragment_begin>
	#include <normal_fragment_maps>
	#include <emissivemap_fragment>
	#include <lights_phong_fragment>
	#include <lights_fragment_begin>
	#include <lights_fragment_maps>
	#include <lights_fragment_end>
	#include <aomap_fragment>
	vec3 outgoingLight = reflectedLight.directDiffuse + reflectedLight.indirectDiffuse + reflectedLight.directSpecular + reflectedLight.indirectSpecular + totalEmissiveRadiance;
	#include <envmap_fragment>
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
	#include <dithering_fragment>
}`,$m=`#define STANDARD
varying vec3 vViewPosition;
#ifdef USE_TRANSMISSION
	varying vec3 vWorldPosition;
#endif
#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <displacementmap_pars_vertex>
#include <color_pars_vertex>
#include <fog_pars_vertex>
#include <normal_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <shadowmap_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <color_vertex>
	#include <morphinstance_vertex>
	#include <morphcolor_vertex>
	#include <batching_vertex>
	#include <beginnormal_vertex>
	#include <morphnormal_vertex>
	#include <skinbase_vertex>
	#include <skinnormal_vertex>
	#include <defaultnormal_vertex>
	#include <normal_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	vViewPosition = - mvPosition.xyz;
	#include <worldpos_vertex>
	#include <shadowmap_vertex>
	#include <fog_vertex>
#ifdef USE_TRANSMISSION
	vWorldPosition = worldPosition.xyz;
#endif
}`,Ym=`#define STANDARD
#ifdef PHYSICAL
	#define IOR
	#define USE_SPECULAR
#endif
uniform vec3 diffuse;
uniform vec3 emissive;
uniform float roughness;
uniform float metalness;
uniform float opacity;
#ifdef IOR
	uniform float ior;
#endif
#ifdef USE_SPECULAR
	uniform float specularIntensity;
	uniform vec3 specularColor;
	#ifdef USE_SPECULAR_COLORMAP
		uniform sampler2D specularColorMap;
	#endif
	#ifdef USE_SPECULAR_INTENSITYMAP
		uniform sampler2D specularIntensityMap;
	#endif
#endif
#ifdef USE_CLEARCOAT
	uniform float clearcoat;
	uniform float clearcoatRoughness;
#endif
#ifdef USE_DISPERSION
	uniform float dispersion;
#endif
#ifdef USE_IRIDESCENCE
	uniform float iridescence;
	uniform float iridescenceIOR;
	uniform float iridescenceThicknessMinimum;
	uniform float iridescenceThicknessMaximum;
#endif
#ifdef USE_SHEEN
	uniform vec3 sheenColor;
	uniform float sheenRoughness;
	#ifdef USE_SHEEN_COLORMAP
		uniform sampler2D sheenColorMap;
	#endif
	#ifdef USE_SHEEN_ROUGHNESSMAP
		uniform sampler2D sheenRoughnessMap;
	#endif
#endif
#ifdef USE_ANISOTROPY
	uniform vec2 anisotropyVector;
	#ifdef USE_ANISOTROPYMAP
		uniform sampler2D anisotropyMap;
	#endif
#endif
varying vec3 vViewPosition;
#include <common>
#include <packing>
#include <dithering_pars_fragment>
#include <color_pars_fragment>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <aomap_pars_fragment>
#include <lightmap_pars_fragment>
#include <emissivemap_pars_fragment>
#include <iridescence_fragment>
#include <cube_uv_reflection_fragment>
#include <envmap_common_pars_fragment>
#include <envmap_physical_pars_fragment>
#include <fog_pars_fragment>
#include <lights_pars_begin>
#include <normal_pars_fragment>
#include <lights_physical_pars_fragment>
#include <transmission_pars_fragment>
#include <shadowmap_pars_fragment>
#include <bumpmap_pars_fragment>
#include <normalmap_pars_fragment>
#include <clearcoat_pars_fragment>
#include <iridescence_pars_fragment>
#include <roughnessmap_pars_fragment>
#include <metalnessmap_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	ReflectedLight reflectedLight = ReflectedLight( vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ) );
	vec3 totalEmissiveRadiance = emissive;
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <color_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	#include <roughnessmap_fragment>
	#include <metalnessmap_fragment>
	#include <normal_fragment_begin>
	#include <normal_fragment_maps>
	#include <clearcoat_normal_fragment_begin>
	#include <clearcoat_normal_fragment_maps>
	#include <emissivemap_fragment>
	#include <lights_physical_fragment>
	#include <lights_fragment_begin>
	#include <lights_fragment_maps>
	#include <lights_fragment_end>
	#include <aomap_fragment>
	vec3 totalDiffuse = reflectedLight.directDiffuse + reflectedLight.indirectDiffuse;
	vec3 totalSpecular = reflectedLight.directSpecular + reflectedLight.indirectSpecular;
	#include <transmission_fragment>
	vec3 outgoingLight = totalDiffuse + totalSpecular + totalEmissiveRadiance;
	#ifdef USE_SHEEN
		float sheenEnergyComp = 1.0 - 0.157 * max3( material.sheenColor );
		outgoingLight = outgoingLight * sheenEnergyComp + sheenSpecularDirect + sheenSpecularIndirect;
	#endif
	#ifdef USE_CLEARCOAT
		float dotNVcc = saturate( dot( geometryClearcoatNormal, geometryViewDir ) );
		vec3 Fcc = F_Schlick( material.clearcoatF0, material.clearcoatF90, dotNVcc );
		outgoingLight = outgoingLight * ( 1.0 - material.clearcoat * Fcc ) + ( clearcoatSpecularDirect + clearcoatSpecularIndirect ) * material.clearcoat;
	#endif
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
	#include <dithering_fragment>
}`,Zm=`#define TOON
varying vec3 vViewPosition;
#include <common>
#include <batching_pars_vertex>
#include <uv_pars_vertex>
#include <displacementmap_pars_vertex>
#include <color_pars_vertex>
#include <fog_pars_vertex>
#include <normal_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <shadowmap_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	#include <color_vertex>
	#include <morphinstance_vertex>
	#include <morphcolor_vertex>
	#include <batching_vertex>
	#include <beginnormal_vertex>
	#include <morphnormal_vertex>
	#include <skinbase_vertex>
	#include <skinnormal_vertex>
	#include <defaultnormal_vertex>
	#include <normal_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <displacementmap_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	vViewPosition = - mvPosition.xyz;
	#include <worldpos_vertex>
	#include <shadowmap_vertex>
	#include <fog_vertex>
}`,Jm=`#define TOON
uniform vec3 diffuse;
uniform vec3 emissive;
uniform float opacity;
#include <common>
#include <packing>
#include <dithering_pars_fragment>
#include <color_pars_fragment>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <aomap_pars_fragment>
#include <lightmap_pars_fragment>
#include <emissivemap_pars_fragment>
#include <gradientmap_pars_fragment>
#include <fog_pars_fragment>
#include <bsdfs>
#include <lights_pars_begin>
#include <normal_pars_fragment>
#include <lights_toon_pars_fragment>
#include <shadowmap_pars_fragment>
#include <bumpmap_pars_fragment>
#include <normalmap_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	ReflectedLight reflectedLight = ReflectedLight( vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ) );
	vec3 totalEmissiveRadiance = emissive;
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <color_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	#include <normal_fragment_begin>
	#include <normal_fragment_maps>
	#include <emissivemap_fragment>
	#include <lights_toon_fragment>
	#include <lights_fragment_begin>
	#include <lights_fragment_maps>
	#include <lights_fragment_end>
	#include <aomap_fragment>
	vec3 outgoingLight = reflectedLight.directDiffuse + reflectedLight.indirectDiffuse + totalEmissiveRadiance;
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
	#include <dithering_fragment>
}`,Km=`uniform float size;
uniform float scale;
#include <common>
#include <color_pars_vertex>
#include <fog_pars_vertex>
#include <morphtarget_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
#ifdef USE_POINTS_UV
	varying vec2 vUv;
	uniform mat3 uvTransform;
#endif
void main() {
	#ifdef USE_POINTS_UV
		vUv = ( uvTransform * vec3( uv, 1 ) ).xy;
	#endif
	#include <color_vertex>
	#include <morphinstance_vertex>
	#include <morphcolor_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <project_vertex>
	gl_PointSize = size;
	#ifdef USE_SIZEATTENUATION
		bool isPerspective = isPerspectiveMatrix( projectionMatrix );
		if ( isPerspective ) gl_PointSize *= ( scale / - mvPosition.z );
	#endif
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	#include <worldpos_vertex>
	#include <fog_vertex>
}`,jm=`uniform vec3 diffuse;
uniform float opacity;
#include <common>
#include <color_pars_fragment>
#include <map_particle_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <fog_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	vec3 outgoingLight = vec3( 0.0 );
	#include <logdepthbuf_fragment>
	#include <map_particle_fragment>
	#include <color_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	outgoingLight = diffuseColor.rgb;
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
	#include <premultiplied_alpha_fragment>
}`,Qm=`#include <common>
#include <batching_pars_vertex>
#include <fog_pars_vertex>
#include <morphtarget_pars_vertex>
#include <skinning_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <shadowmap_pars_vertex>
void main() {
	#include <batching_vertex>
	#include <beginnormal_vertex>
	#include <morphinstance_vertex>
	#include <morphnormal_vertex>
	#include <skinbase_vertex>
	#include <skinnormal_vertex>
	#include <defaultnormal_vertex>
	#include <begin_vertex>
	#include <morphtarget_vertex>
	#include <skinning_vertex>
	#include <project_vertex>
	#include <logdepthbuf_vertex>
	#include <worldpos_vertex>
	#include <shadowmap_vertex>
	#include <fog_vertex>
}`,eg=`uniform vec3 color;
uniform float opacity;
#include <common>
#include <packing>
#include <fog_pars_fragment>
#include <bsdfs>
#include <lights_pars_begin>
#include <logdepthbuf_pars_fragment>
#include <shadowmap_pars_fragment>
#include <shadowmask_pars_fragment>
void main() {
	#include <logdepthbuf_fragment>
	gl_FragColor = vec4( color, opacity * ( 1.0 - getShadowMask() ) );
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
}`,tg=`uniform float rotation;
uniform vec2 center;
#include <common>
#include <uv_pars_vertex>
#include <fog_pars_vertex>
#include <logdepthbuf_pars_vertex>
#include <clipping_planes_pars_vertex>
void main() {
	#include <uv_vertex>
	vec4 mvPosition = modelViewMatrix[ 3 ];
	vec2 scale = vec2( length( modelMatrix[ 0 ].xyz ), length( modelMatrix[ 1 ].xyz ) );
	#ifndef USE_SIZEATTENUATION
		bool isPerspective = isPerspectiveMatrix( projectionMatrix );
		if ( isPerspective ) scale *= - mvPosition.z;
	#endif
	vec2 alignedPosition = ( position.xy - ( center - vec2( 0.5 ) ) ) * scale;
	vec2 rotatedPosition;
	rotatedPosition.x = cos( rotation ) * alignedPosition.x - sin( rotation ) * alignedPosition.y;
	rotatedPosition.y = sin( rotation ) * alignedPosition.x + cos( rotation ) * alignedPosition.y;
	mvPosition.xy += rotatedPosition;
	gl_Position = projectionMatrix * mvPosition;
	#include <logdepthbuf_vertex>
	#include <clipping_planes_vertex>
	#include <fog_vertex>
}`,ng=`uniform vec3 diffuse;
uniform float opacity;
#include <common>
#include <uv_pars_fragment>
#include <map_pars_fragment>
#include <alphamap_pars_fragment>
#include <alphatest_pars_fragment>
#include <alphahash_pars_fragment>
#include <fog_pars_fragment>
#include <logdepthbuf_pars_fragment>
#include <clipping_planes_pars_fragment>
void main() {
	vec4 diffuseColor = vec4( diffuse, opacity );
	#include <clipping_planes_fragment>
	vec3 outgoingLight = vec3( 0.0 );
	#include <logdepthbuf_fragment>
	#include <map_fragment>
	#include <alphamap_fragment>
	#include <alphatest_fragment>
	#include <alphahash_fragment>
	outgoingLight = diffuseColor.rgb;
	#include <opaque_fragment>
	#include <tonemapping_fragment>
	#include <colorspace_fragment>
	#include <fog_fragment>
}`,ze={alphahash_fragment:Ef,alphahash_pars_fragment:Tf,alphamap_fragment:wf,alphamap_pars_fragment:Af,alphatest_fragment:Rf,alphatest_pars_fragment:Cf,aomap_fragment:If,aomap_pars_fragment:Pf,batching_pars_vertex:Lf,batching_vertex:Df,begin_vertex:Uf,beginnormal_vertex:Nf,bsdfs:Ff,iridescence_fragment:Of,bumpmap_pars_fragment:kf,clipping_planes_fragment:Bf,clipping_planes_pars_fragment:zf,clipping_planes_pars_vertex:Vf,clipping_planes_vertex:Gf,color_fragment:Hf,color_pars_fragment:Wf,color_pars_vertex:Xf,color_vertex:qf,common:$f,cube_uv_reflection_fragment:Yf,defaultnormal_vertex:Zf,displacementmap_pars_vertex:Jf,displacementmap_vertex:Kf,emissivemap_fragment:jf,emissivemap_pars_fragment:Qf,colorspace_fragment:ep,colorspace_pars_fragment:tp,envmap_fragment:np,envmap_common_pars_fragment:ip,envmap_pars_fragment:sp,envmap_pars_vertex:rp,envmap_physical_pars_fragment:gp,envmap_vertex:ap,fog_vertex:op,fog_pars_vertex:lp,fog_fragment:cp,fog_pars_fragment:hp,gradientmap_pars_fragment:up,lightmap_pars_fragment:dp,lights_lambert_fragment:fp,lights_lambert_pars_fragment:pp,lights_pars_begin:mp,lights_toon_fragment:_p,lights_toon_pars_fragment:xp,lights_phong_fragment:vp,lights_phong_pars_fragment:yp,lights_physical_fragment:Mp,lights_physical_pars_fragment:Sp,lights_fragment_begin:bp,lights_fragment_maps:Ep,lights_fragment_end:Tp,logdepthbuf_fragment:wp,logdepthbuf_pars_fragment:Ap,logdepthbuf_pars_vertex:Rp,logdepthbuf_vertex:Cp,map_fragment:Ip,map_pars_fragment:Pp,map_particle_fragment:Lp,map_particle_pars_fragment:Dp,metalnessmap_fragment:Up,metalnessmap_pars_fragment:Np,morphinstance_vertex:Fp,morphcolor_vertex:Op,morphnormal_vertex:kp,morphtarget_pars_vertex:Bp,morphtarget_vertex:zp,normal_fragment_begin:Vp,normal_fragment_maps:Gp,normal_pars_fragment:Hp,normal_pars_vertex:Wp,normal_vertex:Xp,normalmap_pars_fragment:qp,clearcoat_normal_fragment_begin:$p,clearcoat_normal_fragment_maps:Yp,clearcoat_pars_fragment:Zp,iridescence_pars_fragment:Jp,opaque_fragment:Kp,packing:jp,premultiplied_alpha_fragment:Qp,project_vertex:em,dithering_fragment:tm,dithering_pars_fragment:nm,roughnessmap_fragment:im,roughnessmap_pars_fragment:sm,shadowmap_pars_fragment:rm,shadowmap_pars_vertex:am,shadowmap_vertex:om,shadowmask_pars_fragment:lm,skinbase_vertex:cm,skinning_pars_vertex:hm,skinning_vertex:um,skinnormal_vertex:dm,specularmap_fragment:fm,specularmap_pars_fragment:pm,tonemapping_fragment:mm,tonemapping_pars_fragment:gm,transmission_fragment:_m,transmission_pars_fragment:xm,uv_pars_fragment:vm,uv_pars_vertex:ym,uv_vertex:Mm,worldpos_vertex:Sm,background_vert:bm,background_frag:Em,backgroundCube_vert:Tm,backgroundCube_frag:wm,cube_vert:Am,cube_frag:Rm,depth_vert:Cm,depth_frag:Im,distanceRGBA_vert:Pm,distanceRGBA_frag:Lm,equirect_vert:Dm,equirect_frag:Um,linedashed_vert:Nm,linedashed_frag:Fm,meshbasic_vert:Om,meshbasic_frag:km,meshlambert_vert:Bm,meshlambert_frag:zm,meshmatcap_vert:Vm,meshmatcap_frag:Gm,meshnormal_vert:Hm,meshnormal_frag:Wm,meshphong_vert:Xm,meshphong_frag:qm,meshphysical_vert:$m,meshphysical_frag:Ym,meshtoon_vert:Zm,meshtoon_frag:Jm,points_vert:Km,points_frag:jm,shadow_vert:Qm,shadow_frag:eg,sprite_vert:tg,sprite_frag:ng},re={common:{diffuse:{value:new Xe(16777215)},opacity:{value:1},map:{value:null},mapTransform:{value:new Oe},alphaMap:{value:null},alphaMapTransform:{value:new Oe},alphaTest:{value:0}},specularmap:{specularMap:{value:null},specularMapTransform:{value:new Oe}},envmap:{envMap:{value:null},envMapRotation:{value:new Oe},flipEnvMap:{value:-1},reflectivity:{value:1},ior:{value:1.5},refractionRatio:{value:.98}},aomap:{aoMap:{value:null},aoMapIntensity:{value:1},aoMapTransform:{value:new Oe}},lightmap:{lightMap:{value:null},lightMapIntensity:{value:1},lightMapTransform:{value:new Oe}},bumpmap:{bumpMap:{value:null},bumpMapTransform:{value:new Oe},bumpScale:{value:1}},normalmap:{normalMap:{value:null},normalMapTransform:{value:new Oe},normalScale:{value:new Ue(1,1)}},displacementmap:{displacementMap:{value:null},displacementMapTransform:{value:new Oe},displacementScale:{value:1},displacementBias:{value:0}},emissivemap:{emissiveMap:{value:null},emissiveMapTransform:{value:new Oe}},metalnessmap:{metalnessMap:{value:null},metalnessMapTransform:{value:new Oe}},roughnessmap:{roughnessMap:{value:null},roughnessMapTransform:{value:new Oe}},gradientmap:{gradientMap:{value:null}},fog:{fogDensity:{value:25e-5},fogNear:{value:1},fogFar:{value:2e3},fogColor:{value:new Xe(16777215)}},lights:{ambientLightColor:{value:[]},lightProbe:{value:[]},directionalLights:{value:[],properties:{direction:{},color:{}}},directionalLightShadows:{value:[],properties:{shadowIntensity:1,shadowBias:{},shadowNormalBias:{},shadowRadius:{},shadowMapSize:{}}},directionalShadowMap:{value:[]},directionalShadowMatrix:{value:[]},spotLights:{value:[],properties:{color:{},position:{},direction:{},distance:{},coneCos:{},penumbraCos:{},decay:{}}},spotLightShadows:{value:[],properties:{shadowIntensity:1,shadowBias:{},shadowNormalBias:{},shadowRadius:{},shadowMapSize:{}}},spotLightMap:{value:[]},spotShadowMap:{value:[]},spotLightMatrix:{value:[]},pointLights:{value:[],properties:{color:{},position:{},decay:{},distance:{}}},pointLightShadows:{value:[],properties:{shadowIntensity:1,shadowBias:{},shadowNormalBias:{},shadowRadius:{},shadowMapSize:{},shadowCameraNear:{},shadowCameraFar:{}}},pointShadowMap:{value:[]},pointShadowMatrix:{value:[]},hemisphereLights:{value:[],properties:{direction:{},skyColor:{},groundColor:{}}},rectAreaLights:{value:[],properties:{color:{},position:{},width:{},height:{}}},ltc_1:{value:null},ltc_2:{value:null}},points:{diffuse:{value:new Xe(16777215)},opacity:{value:1},size:{value:1},scale:{value:1},map:{value:null},alphaMap:{value:null},alphaMapTransform:{value:new Oe},alphaTest:{value:0},uvTransform:{value:new Oe}},sprite:{diffuse:{value:new Xe(16777215)},opacity:{value:1},center:{value:new Ue(.5,.5)},rotation:{value:0},map:{value:null},mapTransform:{value:new Oe},alphaMap:{value:null},alphaMapTransform:{value:new Oe},alphaTest:{value:0}}},wn={basic:{uniforms:Lt([re.common,re.specularmap,re.envmap,re.aomap,re.lightmap,re.fog]),vertexShader:ze.meshbasic_vert,fragmentShader:ze.meshbasic_frag},lambert:{uniforms:Lt([re.common,re.specularmap,re.envmap,re.aomap,re.lightmap,re.emissivemap,re.bumpmap,re.normalmap,re.displacementmap,re.fog,re.lights,{emissive:{value:new Xe(0)}}]),vertexShader:ze.meshlambert_vert,fragmentShader:ze.meshlambert_frag},phong:{uniforms:Lt([re.common,re.specularmap,re.envmap,re.aomap,re.lightmap,re.emissivemap,re.bumpmap,re.normalmap,re.displacementmap,re.fog,re.lights,{emissive:{value:new Xe(0)},specular:{value:new Xe(1118481)},shininess:{value:30}}]),vertexShader:ze.meshphong_vert,fragmentShader:ze.meshphong_frag},standard:{uniforms:Lt([re.common,re.envmap,re.aomap,re.lightmap,re.emissivemap,re.bumpmap,re.normalmap,re.displacementmap,re.roughnessmap,re.metalnessmap,re.fog,re.lights,{emissive:{value:new Xe(0)},roughness:{value:1},metalness:{value:0},envMapIntensity:{value:1}}]),vertexShader:ze.meshphysical_vert,fragmentShader:ze.meshphysical_frag},toon:{uniforms:Lt([re.common,re.aomap,re.lightmap,re.emissivemap,re.bumpmap,re.normalmap,re.displacementmap,re.gradientmap,re.fog,re.lights,{emissive:{value:new Xe(0)}}]),vertexShader:ze.meshtoon_vert,fragmentShader:ze.meshtoon_frag},matcap:{uniforms:Lt([re.common,re.bumpmap,re.normalmap,re.displacementmap,re.fog,{matcap:{value:null}}]),vertexShader:ze.meshmatcap_vert,fragmentShader:ze.meshmatcap_frag},points:{uniforms:Lt([re.points,re.fog]),vertexShader:ze.points_vert,fragmentShader:ze.points_frag},dashed:{uniforms:Lt([re.common,re.fog,{scale:{value:1},dashSize:{value:1},totalSize:{value:2}}]),vertexShader:ze.linedashed_vert,fragmentShader:ze.linedashed_frag},depth:{uniforms:Lt([re.common,re.displacementmap]),vertexShader:ze.depth_vert,fragmentShader:ze.depth_frag},normal:{uniforms:Lt([re.common,re.bumpmap,re.normalmap,re.displacementmap,{opacity:{value:1}}]),vertexShader:ze.meshnormal_vert,fragmentShader:ze.meshnormal_frag},sprite:{uniforms:Lt([re.sprite,re.fog]),vertexShader:ze.sprite_vert,fragmentShader:ze.sprite_frag},background:{uniforms:{uvTransform:{value:new Oe},t2D:{value:null},backgroundIntensity:{value:1}},vertexShader:ze.background_vert,fragmentShader:ze.background_frag},backgroundCube:{uniforms:{envMap:{value:null},flipEnvMap:{value:-1},backgroundBlurriness:{value:0},backgroundIntensity:{value:1},backgroundRotation:{value:new Oe}},vertexShader:ze.backgroundCube_vert,fragmentShader:ze.backgroundCube_frag},cube:{uniforms:{tCube:{value:null},tFlip:{value:-1},opacity:{value:1}},vertexShader:ze.cube_vert,fragmentShader:ze.cube_frag},equirect:{uniforms:{tEquirect:{value:null}},vertexShader:ze.equirect_vert,fragmentShader:ze.equirect_frag},distanceRGBA:{uniforms:Lt([re.common,re.displacementmap,{referencePosition:{value:new L},nearDistance:{value:1},farDistance:{value:1e3}}]),vertexShader:ze.distanceRGBA_vert,fragmentShader:ze.distanceRGBA_frag},shadow:{uniforms:Lt([re.lights,re.fog,{color:{value:new Xe(0)},opacity:{value:1}}]),vertexShader:ze.shadow_vert,fragmentShader:ze.shadow_frag}};wn.physical={uniforms:Lt([wn.standard.uniforms,{clearcoat:{value:0},clearcoatMap:{value:null},clearcoatMapTransform:{value:new Oe},clearcoatNormalMap:{value:null},clearcoatNormalMapTransform:{value:new Oe},clearcoatNormalScale:{value:new Ue(1,1)},clearcoatRoughness:{value:0},clearcoatRoughnessMap:{value:null},clearcoatRoughnessMapTransform:{value:new Oe},dispersion:{value:0},iridescence:{value:0},iridescenceMap:{value:null},iridescenceMapTransform:{value:new Oe},iridescenceIOR:{value:1.3},iridescenceThicknessMinimum:{value:100},iridescenceThicknessMaximum:{value:400},iridescenceThicknessMap:{value:null},iridescenceThicknessMapTransform:{value:new Oe},sheen:{value:0},sheenColor:{value:new Xe(0)},sheenColorMap:{value:null},sheenColorMapTransform:{value:new Oe},sheenRoughness:{value:1},sheenRoughnessMap:{value:null},sheenRoughnessMapTransform:{value:new Oe},transmission:{value:0},transmissionMap:{value:null},transmissionMapTransform:{value:new Oe},transmissionSamplerSize:{value:new Ue},transmissionSamplerMap:{value:null},thickness:{value:0},thicknessMap:{value:null},thicknessMapTransform:{value:new Oe},attenuationDistance:{value:0},attenuationColor:{value:new Xe(0)},specularColor:{value:new Xe(1,1,1)},specularColorMap:{value:null},specularColorMapTransform:{value:new Oe},specularIntensity:{value:1},specularIntensityMap:{value:null},specularIntensityMapTransform:{value:new Oe},anisotropyVector:{value:new Ue},anisotropyMap:{value:null},anisotropyMapTransform:{value:new Oe}}]),vertexShader:ze.meshphysical_vert,fragmentShader:ze.meshphysical_frag};var go={r:0,b:0,g:0},Ui=new fn,ig=new dt;function sg(n,e,t,i,s,r,o){let a=new Xe(0),c=r===!0?0:1,l,h,u=null,p=0,d=null;function g(E){let S=E.isScene===!0?E.background:null;return S&&S.isTexture&&(S=(E.backgroundBlurriness>0?t:e).get(S)),S}function v(E){let S=!1,R=g(E);R===null?f(a,c):R&&R.isColor&&(f(R,1),S=!0);let A=n.xr.getEnvironmentBlendMode();A==="additive"?i.buffers.color.setClear(0,0,0,1,o):A==="alpha-blend"&&i.buffers.color.setClear(0,0,0,0,o),(n.autoClear||S)&&(i.buffers.depth.setTest(!0),i.buffers.depth.setMask(!0),i.buffers.color.setMask(!0),n.clear(n.autoClearColor,n.autoClearDepth,n.autoClearStencil))}function m(E,S){let R=g(S);R&&(R.isCubeTexture||R.mapping===lr)?(h===void 0&&(h=new ft(new oi(1,1,1),new pn({name:"BackgroundCubeMaterial",uniforms:Di(wn.backgroundCube.uniforms),vertexShader:wn.backgroundCube.vertexShader,fragmentShader:wn.backgroundCube.fragmentShader,side:Bt,depthTest:!1,depthWrite:!1,fog:!1,allowOverride:!1})),h.geometry.deleteAttribute("normal"),h.geometry.deleteAttribute("uv"),h.onBeforeRender=function(A,P,N){this.matrixWorld.copyPosition(N.matrixWorld)},Object.defineProperty(h.material,"envMap",{get:function(){return this.uniforms.envMap.value}}),s.update(h)),Ui.copy(S.backgroundRotation),Ui.x*=-1,Ui.y*=-1,Ui.z*=-1,R.isCubeTexture&&R.isRenderTargetTexture===!1&&(Ui.y*=-1,Ui.z*=-1),h.material.uniforms.envMap.value=R,h.material.uniforms.flipEnvMap.value=R.isCubeTexture&&R.isRenderTargetTexture===!1?-1:1,h.material.uniforms.backgroundBlurriness.value=S.backgroundBlurriness,h.material.uniforms.backgroundIntensity.value=S.backgroundIntensity,h.material.uniforms.backgroundRotation.value.setFromMatrix4(ig.makeRotationFromEuler(Ui)),h.material.toneMapped=Ye.getTransfer(R.colorSpace)!==et,(u!==R||p!==R.version||d!==n.toneMapping)&&(h.material.needsUpdate=!0,u=R,p=R.version,d=n.toneMapping),h.layers.enableAll(),E.unshift(h,h.geometry,h.material,0,0,null)):R&&R.isTexture&&(l===void 0&&(l=new ft(new nr(2,2),new pn({name:"BackgroundMaterial",uniforms:Di(wn.background.uniforms),vertexShader:wn.background.vertexShader,fragmentShader:wn.background.fragmentShader,side:Bn,depthTest:!1,depthWrite:!1,fog:!1,allowOverride:!1})),l.geometry.deleteAttribute("normal"),Object.defineProperty(l.material,"map",{get:function(){return this.uniforms.t2D.value}}),s.update(l)),l.material.uniforms.t2D.value=R,l.material.uniforms.backgroundIntensity.value=S.backgroundIntensity,l.material.toneMapped=Ye.getTransfer(R.colorSpace)!==et,R.matrixAutoUpdate===!0&&R.updateMatrix(),l.material.uniforms.uvTransform.value.copy(R.matrix),(u!==R||p!==R.version||d!==n.toneMapping)&&(l.material.needsUpdate=!0,u=R,p=R.version,d=n.toneMapping),l.layers.enableAll(),E.unshift(l,l.geometry,l.material,0,0,null))}function f(E,S){E.getRGB(go,Wl(n)),i.buffers.color.setClear(go.r,go.g,go.b,S,o)}function T(){h!==void 0&&(h.geometry.dispose(),h.material.dispose(),h=void 0),l!==void 0&&(l.geometry.dispose(),l.material.dispose(),l=void 0)}return{getClearColor:function(){return a},setClearColor:function(E,S=1){a.set(E),c=S,f(a,c)},getClearAlpha:function(){return c},setClearAlpha:function(E){c=E,f(a,c)},render:v,addToRenderList:m,dispose:T}}function rg(n,e){let t=n.getParameter(n.MAX_VERTEX_ATTRIBS),i={},s=p(null),r=s,o=!1;function a(y,I,O,z,H){let q=!1,W=u(z,O,I);r!==W&&(r=W,l(r.object)),q=d(y,z,O,H),q&&g(y,z,O,H),H!==null&&e.update(H,n.ELEMENT_ARRAY_BUFFER),(q||o)&&(o=!1,S(y,I,O,z),H!==null&&n.bindBuffer(n.ELEMENT_ARRAY_BUFFER,e.get(H).buffer))}function c(){return n.createVertexArray()}function l(y){return n.bindVertexArray(y)}function h(y){return n.deleteVertexArray(y)}function u(y,I,O){let z=O.wireframe===!0,H=i[y.id];H===void 0&&(H={},i[y.id]=H);let q=H[I.id];q===void 0&&(q={},H[I.id]=q);let W=q[z];return W===void 0&&(W=p(c()),q[z]=W),W}function p(y){let I=[],O=[],z=[];for(let H=0;H<t;H++)I[H]=0,O[H]=0,z[H]=0;return{geometry:null,program:null,wireframe:!1,newAttributes:I,enabledAttributes:O,attributeDivisors:z,object:y,attributes:{},index:null}}function d(y,I,O,z){let H=r.attributes,q=I.attributes,W=0,ne=O.getAttributes();for(let V in ne)if(ne[V].location>=0){let ue=H[V],Ee=q[V];if(Ee===void 0&&(V==="instanceMatrix"&&y.instanceMatrix&&(Ee=y.instanceMatrix),V==="instanceColor"&&y.instanceColor&&(Ee=y.instanceColor)),ue===void 0||ue.attribute!==Ee||Ee&&ue.data!==Ee.data)return!0;W++}return r.attributesNum!==W||r.index!==z}function g(y,I,O,z){let H={},q=I.attributes,W=0,ne=O.getAttributes();for(let V in ne)if(ne[V].location>=0){let ue=q[V];ue===void 0&&(V==="instanceMatrix"&&y.instanceMatrix&&(ue=y.instanceMatrix),V==="instanceColor"&&y.instanceColor&&(ue=y.instanceColor));let Ee={};Ee.attribute=ue,ue&&ue.data&&(Ee.data=ue.data),H[V]=Ee,W++}r.attributes=H,r.attributesNum=W,r.index=z}function v(){let y=r.newAttributes;for(let I=0,O=y.length;I<O;I++)y[I]=0}function m(y){f(y,0)}function f(y,I){let O=r.newAttributes,z=r.enabledAttributes,H=r.attributeDivisors;O[y]=1,z[y]===0&&(n.enableVertexAttribArray(y),z[y]=1),H[y]!==I&&(n.vertexAttribDivisor(y,I),H[y]=I)}function T(){let y=r.newAttributes,I=r.enabledAttributes;for(let O=0,z=I.length;O<z;O++)I[O]!==y[O]&&(n.disableVertexAttribArray(O),I[O]=0)}function E(y,I,O,z,H,q,W){W===!0?n.vertexAttribIPointer(y,I,O,H,q):n.vertexAttribPointer(y,I,O,z,H,q)}function S(y,I,O,z){v();let H=z.attributes,q=O.getAttributes(),W=I.defaultAttributeValues;for(let ne in q){let V=q[ne];if(V.location>=0){let ae=H[ne];if(ae===void 0&&(ne==="instanceMatrix"&&y.instanceMatrix&&(ae=y.instanceMatrix),ne==="instanceColor"&&y.instanceColor&&(ae=y.instanceColor)),ae!==void 0){let ue=ae.normalized,Ee=ae.itemSize,Ve=e.get(ae);if(Ve===void 0)continue;let st=Ve.buffer,ct=Ve.type,Je=Ve.bytesPerElement,$=ct===n.INT||ct===n.UNSIGNED_INT||ae.gpuType===Na;if(ae.isInterleavedBufferAttribute){let K=ae.data,pe=K.stride,De=ae.offset;if(K.isInstancedInterleavedBuffer){for(let be=0;be<V.locationSize;be++)f(V.location+be,K.meshPerAttribute);y.isInstancedMesh!==!0&&z._maxInstanceCount===void 0&&(z._maxInstanceCount=K.meshPerAttribute*K.count)}else for(let be=0;be<V.locationSize;be++)m(V.location+be);n.bindBuffer(n.ARRAY_BUFFER,st);for(let be=0;be<V.locationSize;be++)E(V.location+be,Ee/V.locationSize,ct,ue,pe*Je,(De+Ee/V.locationSize*be)*Je,$)}else{if(ae.isInstancedBufferAttribute){for(let K=0;K<V.locationSize;K++)f(V.location+K,ae.meshPerAttribute);y.isInstancedMesh!==!0&&z._maxInstanceCount===void 0&&(z._maxInstanceCount=ae.meshPerAttribute*ae.count)}else for(let K=0;K<V.locationSize;K++)m(V.location+K);n.bindBuffer(n.ARRAY_BUFFER,st);for(let K=0;K<V.locationSize;K++)E(V.location+K,Ee/V.locationSize,ct,ue,Ee*Je,Ee/V.locationSize*K*Je,$)}}else if(W!==void 0){let ue=W[ne];if(ue!==void 0)switch(ue.length){case 2:n.vertexAttrib2fv(V.location,ue);break;case 3:n.vertexAttrib3fv(V.location,ue);break;case 4:n.vertexAttrib4fv(V.location,ue);break;default:n.vertexAttrib1fv(V.location,ue)}}}}T()}function R(){N();for(let y in i){let I=i[y];for(let O in I){let z=I[O];for(let H in z)h(z[H].object),delete z[H];delete I[O]}delete i[y]}}function A(y){if(i[y.id]===void 0)return;let I=i[y.id];for(let O in I){let z=I[O];for(let H in z)h(z[H].object),delete z[H];delete I[O]}delete i[y.id]}function P(y){for(let I in i){let O=i[I];if(O[y.id]===void 0)continue;let z=O[y.id];for(let H in z)h(z[H].object),delete z[H];delete O[y.id]}}function N(){M(),o=!0,r!==s&&(r=s,l(r.object))}function M(){s.geometry=null,s.program=null,s.wireframe=!1}return{setup:a,reset:N,resetDefaultState:M,dispose:R,releaseStatesOfGeometry:A,releaseStatesOfProgram:P,initAttributes:v,enableAttribute:m,disableUnusedAttributes:T}}function ag(n,e,t){let i;function s(l){i=l}function r(l,h){n.drawArrays(i,l,h),t.update(h,i,1)}function o(l,h,u){u!==0&&(n.drawArraysInstanced(i,l,h,u),t.update(h,i,u))}function a(l,h,u){if(u===0)return;e.get("WEBGL_multi_draw").multiDrawArraysWEBGL(i,l,0,h,0,u);let d=0;for(let g=0;g<u;g++)d+=h[g];t.update(d,i,1)}function c(l,h,u,p){if(u===0)return;let d=e.get("WEBGL_multi_draw");if(d===null)for(let g=0;g<l.length;g++)o(l[g],h[g],p[g]);else{d.multiDrawArraysInstancedWEBGL(i,l,0,h,0,p,0,u);let g=0;for(let v=0;v<u;v++)g+=h[v]*p[v];t.update(g,i,1)}}this.setMode=s,this.render=r,this.renderInstances=o,this.renderMultiDraw=a,this.renderMultiDrawInstances=c}function og(n,e,t,i){let s;function r(){if(s!==void 0)return s;if(e.has("EXT_texture_filter_anisotropic")===!0){let P=e.get("EXT_texture_filter_anisotropic");s=n.getParameter(P.MAX_TEXTURE_MAX_ANISOTROPY_EXT)}else s=0;return s}function o(P){return!(P!==an&&i.convert(P)!==n.getParameter(n.IMPLEMENTATION_COLOR_READ_FORMAT))}function a(P){let N=P===xs&&(e.has("EXT_color_buffer_half_float")||e.has("EXT_color_buffer_float"));return!(P!==mn&&i.convert(P)!==n.getParameter(n.IMPLEMENTATION_COLOR_READ_TYPE)&&P!==Tn&&!N)}function c(P){if(P==="highp"){if(n.getShaderPrecisionFormat(n.VERTEX_SHADER,n.HIGH_FLOAT).precision>0&&n.getShaderPrecisionFormat(n.FRAGMENT_SHADER,n.HIGH_FLOAT).precision>0)return"highp";P="mediump"}return P==="mediump"&&n.getShaderPrecisionFormat(n.VERTEX_SHADER,n.MEDIUM_FLOAT).precision>0&&n.getShaderPrecisionFormat(n.FRAGMENT_SHADER,n.MEDIUM_FLOAT).precision>0?"mediump":"lowp"}let l=t.precision!==void 0?t.precision:"highp",h=c(l);h!==l&&(console.warn("THREE.WebGLRenderer:",l,"not supported, using",h,"instead."),l=h);let u=t.logarithmicDepthBuffer===!0,p=t.reversedDepthBuffer===!0&&e.has("EXT_clip_control"),d=n.getParameter(n.MAX_TEXTURE_IMAGE_UNITS),g=n.getParameter(n.MAX_VERTEX_TEXTURE_IMAGE_UNITS),v=n.getParameter(n.MAX_TEXTURE_SIZE),m=n.getParameter(n.MAX_CUBE_MAP_TEXTURE_SIZE),f=n.getParameter(n.MAX_VERTEX_ATTRIBS),T=n.getParameter(n.MAX_VERTEX_UNIFORM_VECTORS),E=n.getParameter(n.MAX_VARYING_VECTORS),S=n.getParameter(n.MAX_FRAGMENT_UNIFORM_VECTORS),R=g>0,A=n.getParameter(n.MAX_SAMPLES);return{isWebGL2:!0,getMaxAnisotropy:r,getMaxPrecision:c,textureFormatReadable:o,textureTypeReadable:a,precision:l,logarithmicDepthBuffer:u,reversedDepthBuffer:p,maxTextures:d,maxVertexTextures:g,maxTextureSize:v,maxCubemapSize:m,maxAttributes:f,maxVertexUniforms:T,maxVaryings:E,maxFragmentUniforms:S,vertexTextures:R,maxSamples:A}}function lg(n){let e=this,t=null,i=0,s=!1,r=!1,o=new yn,a=new Oe,c={value:null,needsUpdate:!1};this.uniform=c,this.numPlanes=0,this.numIntersection=0,this.init=function(u,p){let d=u.length!==0||p||i!==0||s;return s=p,i=u.length,d},this.beginShadows=function(){r=!0,h(null)},this.endShadows=function(){r=!1},this.setGlobalState=function(u,p){t=h(u,p,0)},this.setState=function(u,p,d){let g=u.clippingPlanes,v=u.clipIntersection,m=u.clipShadows,f=n.get(u);if(!s||g===null||g.length===0||r&&!m)r?h(null):l();else{let T=r?0:i,E=T*4,S=f.clippingState||null;c.value=S,S=h(g,p,E,d);for(let R=0;R!==E;++R)S[R]=t[R];f.clippingState=S,this.numIntersection=v?this.numPlanes:0,this.numPlanes+=T}};function l(){c.value!==t&&(c.value=t,c.needsUpdate=i>0),e.numPlanes=i,e.numIntersection=0}function h(u,p,d,g){let v=u!==null?u.length:0,m=null;if(v!==0){if(m=c.value,g!==!0||m===null){let f=d+v*4,T=p.matrixWorldInverse;a.getNormalMatrix(T),(m===null||m.length<f)&&(m=new Float32Array(f));for(let E=0,S=d;E!==v;++E,S+=4)o.copy(u[E]).applyMatrix4(T,a),o.normal.toArray(m,S),m[S+3]=o.constant}c.value=m,c.needsUpdate=!0}return e.numPlanes=v,e.numIntersection=0,m}}function cg(n){let e=new WeakMap;function t(o,a){return a===La?o.mapping=Pi:a===Da&&(o.mapping=Li),o}function i(o){if(o&&o.isTexture){let a=o.mapping;if(a===La||a===Da)if(e.has(o)){let c=e.get(o).texture;return t(c,o.mapping)}else{let c=o.image;if(c&&c.height>0){let l=new la(c.height);return l.fromEquirectangularTexture(n,o),e.set(o,l),o.addEventListener("dispose",s),t(l.texture,o.mapping)}else return null}}return o}function s(o){let a=o.target;a.removeEventListener("dispose",s);let c=e.get(a);c!==void 0&&(e.delete(a),c.dispose())}function r(){e=new WeakMap}return{get:i,dispose:r}}var Ss=4,wu=[.125,.215,.35,.446,.526,.582],Oi=20,Yl=new Ii,Au=new Xe,Zl=null,Jl=0,Kl=0,jl=!1,Fi=(1+Math.sqrt(5))/2,Ms=1/Fi,Ru=[new L(-Fi,Ms,0),new L(Fi,Ms,0),new L(-Ms,0,Fi),new L(Ms,0,Fi),new L(0,Fi,-Ms),new L(0,Fi,Ms),new L(-1,1,-1),new L(1,1,-1),new L(-1,1,1),new L(1,1,1)],hg=new L,vo=class{constructor(e){this._renderer=e,this._pingPongRenderTarget=null,this._lodMax=0,this._cubeSize=0,this._lodPlanes=[],this._sizeLods=[],this._sigmas=[],this._blurMaterial=null,this._cubemapMaterial=null,this._equirectMaterial=null,this._compileMaterial(this._blurMaterial)}fromScene(e,t=0,i=.1,s=100,r={}){let{size:o=256,position:a=hg}=r;Zl=this._renderer.getRenderTarget(),Jl=this._renderer.getActiveCubeFace(),Kl=this._renderer.getActiveMipmapLevel(),jl=this._renderer.xr.enabled,this._renderer.xr.enabled=!1,this._setSize(o);let c=this._allocateTargets();return c.depthBuffer=!0,this._sceneToCubeUV(e,i,s,c,a),t>0&&this._blur(c,0,0,t),this._applyPMREM(c),this._cleanup(c),c}fromEquirectangular(e,t=null){return this._fromTexture(e,t)}fromCubemap(e,t=null){return this._fromTexture(e,t)}compileCubemapShader(){this._cubemapMaterial===null&&(this._cubemapMaterial=Pu(),this._compileMaterial(this._cubemapMaterial))}compileEquirectangularShader(){this._equirectMaterial===null&&(this._equirectMaterial=Iu(),this._compileMaterial(this._equirectMaterial))}dispose(){this._dispose(),this._cubemapMaterial!==null&&this._cubemapMaterial.dispose(),this._equirectMaterial!==null&&this._equirectMaterial.dispose()}_setSize(e){this._lodMax=Math.floor(Math.log2(e)),this._cubeSize=Math.pow(2,this._lodMax)}_dispose(){this._blurMaterial!==null&&this._blurMaterial.dispose(),this._pingPongRenderTarget!==null&&this._pingPongRenderTarget.dispose();for(let e=0;e<this._lodPlanes.length;e++)this._lodPlanes[e].dispose()}_cleanup(e){this._renderer.setRenderTarget(Zl,Jl,Kl),this._renderer.xr.enabled=jl,e.scissorTest=!1,_o(e,0,0,e.width,e.height)}_fromTexture(e,t){e.mapping===Pi||e.mapping===Li?this._setSize(e.image.length===0?16:e.image[0].width||e.image[0].image.width):this._setSize(e.image.width/4),Zl=this._renderer.getRenderTarget(),Jl=this._renderer.getActiveCubeFace(),Kl=this._renderer.getActiveMipmapLevel(),jl=this._renderer.xr.enabled,this._renderer.xr.enabled=!1;let i=t||this._allocateTargets();return this._textureToCubeUV(e,i),this._applyPMREM(i),this._cleanup(i),i}_allocateTargets(){let e=3*Math.max(this._cubeSize,112),t=4*this._cubeSize,i={magFilter:dn,minFilter:dn,generateMipmaps:!1,type:xs,format:an,colorSpace:wi,depthBuffer:!1},s=Cu(e,t,i);if(this._pingPongRenderTarget===null||this._pingPongRenderTarget.width!==e||this._pingPongRenderTarget.height!==t){this._pingPongRenderTarget!==null&&this._dispose(),this._pingPongRenderTarget=Cu(e,t,i);let{_lodMax:r}=this;({sizeLods:this._sizeLods,lodPlanes:this._lodPlanes,sigmas:this._sigmas}=ug(r)),this._blurMaterial=dg(r,e,t)}return s}_compileMaterial(e){let t=new ft(this._lodPlanes[0],e);this._renderer.compile(t,Yl)}_sceneToCubeUV(e,t,i,s,r){let c=new Ot(90,1,t,i),l=[1,-1,1,1,1,1],h=[1,1,1,-1,-1,-1],u=this._renderer,p=u.autoClear,d=u.toneMapping;u.getClearColor(Au),u.toneMapping=Wn,u.autoClear=!1,u.state.buffers.depth.getReversed()&&(u.setRenderTarget(s),u.clearDepth(),u.setRenderTarget(null));let v=new bn({name:"PMREM.Background",side:Bt,depthWrite:!1,depthTest:!1}),m=new ft(new oi,v),f=!1,T=e.background;T?T.isColor&&(v.color.copy(T),e.background=null,f=!0):(v.color.copy(Au),f=!0);for(let E=0;E<6;E++){let S=E%3;S===0?(c.up.set(0,l[E],0),c.position.set(r.x,r.y,r.z),c.lookAt(r.x+h[E],r.y,r.z)):S===1?(c.up.set(0,0,l[E]),c.position.set(r.x,r.y,r.z),c.lookAt(r.x,r.y+h[E],r.z)):(c.up.set(0,l[E],0),c.position.set(r.x,r.y,r.z),c.lookAt(r.x,r.y,r.z+h[E]));let R=this._cubeSize;_o(s,S*R,E>2?R:0,R,R),u.setRenderTarget(s),f&&u.render(m,c),u.render(e,c)}m.geometry.dispose(),m.material.dispose(),u.toneMapping=d,u.autoClear=p,e.background=T}_textureToCubeUV(e,t){let i=this._renderer,s=e.mapping===Pi||e.mapping===Li;s?(this._cubemapMaterial===null&&(this._cubemapMaterial=Pu()),this._cubemapMaterial.uniforms.flipEnvMap.value=e.isRenderTargetTexture===!1?-1:1):this._equirectMaterial===null&&(this._equirectMaterial=Iu());let r=s?this._cubemapMaterial:this._equirectMaterial,o=new ft(this._lodPlanes[0],r),a=r.uniforms;a.envMap.value=e;let c=this._cubeSize;_o(t,0,0,3*c,2*c),i.setRenderTarget(t),i.render(o,Yl)}_applyPMREM(e){let t=this._renderer,i=t.autoClear;t.autoClear=!1;let s=this._lodPlanes.length;for(let r=1;r<s;r++){let o=Math.sqrt(this._sigmas[r]*this._sigmas[r]-this._sigmas[r-1]*this._sigmas[r-1]),a=Ru[(s-r-1)%Ru.length];this._blur(e,r-1,r,o,a)}t.autoClear=i}_blur(e,t,i,s,r){let o=this._pingPongRenderTarget;this._halfBlur(e,o,t,i,s,"latitudinal",r),this._halfBlur(o,e,i,i,s,"longitudinal",r)}_halfBlur(e,t,i,s,r,o,a){let c=this._renderer,l=this._blurMaterial;o!=="latitudinal"&&o!=="longitudinal"&&console.error("blur direction must be either latitudinal or longitudinal!");let h=3,u=new ft(this._lodPlanes[s],l),p=l.uniforms,d=this._sizeLods[i]-1,g=isFinite(r)?Math.PI/(2*d):2*Math.PI/(2*Oi-1),v=r/g,m=isFinite(r)?1+Math.floor(h*v):Oi;m>Oi&&console.warn(`sigmaRadians, ${r}, is too large and will clip, as it requested ${m} samples when the maximum is set to ${Oi}`);let f=[],T=0;for(let P=0;P<Oi;++P){let N=P/v,M=Math.exp(-N*N/2);f.push(M),P===0?T+=M:P<m&&(T+=2*M)}for(let P=0;P<f.length;P++)f[P]=f[P]/T;p.envMap.value=e.texture,p.samples.value=m,p.weights.value=f,p.latitudinal.value=o==="latitudinal",a&&(p.poleAxis.value=a);let{_lodMax:E}=this;p.dTheta.value=g,p.mipInt.value=E-i;let S=this._sizeLods[s],R=3*S*(s>E-Ss?s-E+Ss:0),A=4*(this._cubeSize-S);_o(t,R,A,3*S,2*S),c.setRenderTarget(t),c.render(u,Yl)}};function ug(n){let e=[],t=[],i=[],s=n,r=n-Ss+1+wu.length;for(let o=0;o<r;o++){let a=Math.pow(2,s);t.push(a);let c=1/a;o>n-Ss?c=wu[o-n+Ss-1]:o===0&&(c=0),i.push(c);let l=1/(a-2),h=-l,u=1+l,p=[h,h,u,h,u,u,h,h,u,u,h,u],d=6,g=6,v=3,m=2,f=1,T=new Float32Array(v*g*d),E=new Float32Array(m*g*d),S=new Float32Array(f*g*d);for(let A=0;A<d;A++){let P=A%3*2/3-1,N=A>2?0:-1,M=[P,N,0,P+2/3,N,0,P+2/3,N+1,0,P,N,0,P+2/3,N+1,0,P,N+1,0];T.set(M,v*g*A),E.set(p,m*g*A);let y=[A,A,A,A,A,A];S.set(y,f*g*A)}let R=new Xt;R.setAttribute("position",new Ht(T,v)),R.setAttribute("uv",new Ht(E,m)),R.setAttribute("faceIndex",new Ht(S,f)),e.push(R),s>Ss&&s--}return{lodPlanes:e,sizeLods:t,sigmas:i}}function Cu(n,e,t){let i=new Sn(n,e,t);return i.texture.mapping=lr,i.texture.name="PMREM.cubeUv",i.scissorTest=!0,i}function _o(n,e,t,i,s){n.viewport.set(e,t,i,s),n.scissor.set(e,t,i,s)}function dg(n,e,t){let i=new Float32Array(Oi),s=new L(0,1,0);return new pn({name:"SphericalGaussianBlur",defines:{n:Oi,CUBEUV_TEXEL_WIDTH:1/e,CUBEUV_TEXEL_HEIGHT:1/t,CUBEUV_MAX_MIP:`${n}.0`},uniforms:{envMap:{value:null},samples:{value:1},weights:{value:i},latitudinal:{value:!1},dTheta:{value:0},mipInt:{value:0},poleAxis:{value:s}},vertexShader:lc(),fragmentShader:`

			precision mediump float;
			precision mediump int;

			varying vec3 vOutputDirection;

			uniform sampler2D envMap;
			uniform int samples;
			uniform float weights[ n ];
			uniform bool latitudinal;
			uniform float dTheta;
			uniform float mipInt;
			uniform vec3 poleAxis;

			#define ENVMAP_TYPE_CUBE_UV
			#include <cube_uv_reflection_fragment>

			vec3 getSample( float theta, vec3 axis ) {

				float cosTheta = cos( theta );
				// Rodrigues' axis-angle rotation
				vec3 sampleDirection = vOutputDirection * cosTheta
					+ cross( axis, vOutputDirection ) * sin( theta )
					+ axis * dot( axis, vOutputDirection ) * ( 1.0 - cosTheta );

				return bilinearCubeUV( envMap, sampleDirection, mipInt );

			}

			void main() {

				vec3 axis = latitudinal ? poleAxis : cross( poleAxis, vOutputDirection );

				if ( all( equal( axis, vec3( 0.0 ) ) ) ) {

					axis = vec3( vOutputDirection.z, 0.0, - vOutputDirection.x );

				}

				axis = normalize( axis );

				gl_FragColor = vec4( 0.0, 0.0, 0.0, 1.0 );
				gl_FragColor.rgb += weights[ 0 ] * getSample( 0.0, axis );

				for ( int i = 1; i < n; i++ ) {

					if ( i >= samples ) {

						break;

					}

					float theta = dTheta * float( i );
					gl_FragColor.rgb += weights[ i ] * getSample( -1.0 * theta, axis );
					gl_FragColor.rgb += weights[ i ] * getSample( theta, axis );

				}

			}
		`,blending:Hn,depthTest:!1,depthWrite:!1})}function Iu(){return new pn({name:"EquirectangularToCubeUV",uniforms:{envMap:{value:null}},vertexShader:lc(),fragmentShader:`

			precision mediump float;
			precision mediump int;

			varying vec3 vOutputDirection;

			uniform sampler2D envMap;

			#include <common>

			void main() {

				vec3 outputDirection = normalize( vOutputDirection );
				vec2 uv = equirectUv( outputDirection );

				gl_FragColor = vec4( texture2D ( envMap, uv ).rgb, 1.0 );

			}
		`,blending:Hn,depthTest:!1,depthWrite:!1})}function Pu(){return new pn({name:"CubemapToCubeUV",uniforms:{envMap:{value:null},flipEnvMap:{value:-1}},vertexShader:lc(),fragmentShader:`

			precision mediump float;
			precision mediump int;

			uniform float flipEnvMap;

			varying vec3 vOutputDirection;

			uniform samplerCube envMap;

			void main() {

				gl_FragColor = textureCube( envMap, vec3( flipEnvMap * vOutputDirection.x, vOutputDirection.yz ) );

			}
		`,blending:Hn,depthTest:!1,depthWrite:!1})}function lc(){return`

		precision mediump float;
		precision mediump int;

		attribute float faceIndex;

		varying vec3 vOutputDirection;

		// RH coordinate system; PMREM face-indexing convention
		vec3 getDirection( vec2 uv, float face ) {

			uv = 2.0 * uv - 1.0;

			vec3 direction = vec3( uv, 1.0 );

			if ( face == 0.0 ) {

				direction = direction.zyx; // ( 1, v, u ) pos x

			} else if ( face == 1.0 ) {

				direction = direction.xzy;
				direction.xz *= -1.0; // ( -u, 1, -v ) pos y

			} else if ( face == 2.0 ) {

				direction.x *= -1.0; // ( -u, v, 1 ) pos z

			} else if ( face == 3.0 ) {

				direction = direction.zyx;
				direction.xz *= -1.0; // ( -1, v, -u ) neg x

			} else if ( face == 4.0 ) {

				direction = direction.xzy;
				direction.xy *= -1.0; // ( -u, -1, v ) neg y

			} else if ( face == 5.0 ) {

				direction.z *= -1.0; // ( u, v, -1 ) neg z

			}

			return direction;

		}

		void main() {

			vOutputDirection = getDirection( uv, faceIndex );
			gl_Position = vec4( position, 1.0 );

		}
	`}function fg(n){let e=new WeakMap,t=null;function i(a){if(a&&a.isTexture){let c=a.mapping,l=c===La||c===Da,h=c===Pi||c===Li;if(l||h){let u=e.get(a),p=u!==void 0?u.texture.pmremVersion:0;if(a.isRenderTargetTexture&&a.pmremVersion!==p)return t===null&&(t=new vo(n)),u=l?t.fromEquirectangular(a,u):t.fromCubemap(a,u),u.texture.pmremVersion=a.pmremVersion,e.set(a,u),u.texture;if(u!==void 0)return u.texture;{let d=a.image;return l&&d&&d.height>0||h&&d&&s(d)?(t===null&&(t=new vo(n)),u=l?t.fromEquirectangular(a):t.fromCubemap(a),u.texture.pmremVersion=a.pmremVersion,e.set(a,u),a.addEventListener("dispose",r),u.texture):null}}}return a}function s(a){let c=0,l=6;for(let h=0;h<l;h++)a[h]!==void 0&&c++;return c===l}function r(a){let c=a.target;c.removeEventListener("dispose",r);let l=e.get(c);l!==void 0&&(e.delete(c),l.dispose())}function o(){e=new WeakMap,t!==null&&(t.dispose(),t=null)}return{get:i,dispose:o}}function pg(n){let e={};function t(i){if(e[i]!==void 0)return e[i];let s;switch(i){case"WEBGL_depth_texture":s=n.getExtension("WEBGL_depth_texture")||n.getExtension("MOZ_WEBGL_depth_texture")||n.getExtension("WEBKIT_WEBGL_depth_texture");break;case"EXT_texture_filter_anisotropic":s=n.getExtension("EXT_texture_filter_anisotropic")||n.getExtension("MOZ_EXT_texture_filter_anisotropic")||n.getExtension("WEBKIT_EXT_texture_filter_anisotropic");break;case"WEBGL_compressed_texture_s3tc":s=n.getExtension("WEBGL_compressed_texture_s3tc")||n.getExtension("MOZ_WEBGL_compressed_texture_s3tc")||n.getExtension("WEBKIT_WEBGL_compressed_texture_s3tc");break;case"WEBGL_compressed_texture_pvrtc":s=n.getExtension("WEBGL_compressed_texture_pvrtc")||n.getExtension("WEBKIT_WEBGL_compressed_texture_pvrtc");break;default:s=n.getExtension(i)}return e[i]=s,s}return{has:function(i){return t(i)!==null},init:function(){t("EXT_color_buffer_float"),t("WEBGL_clip_cull_distance"),t("OES_texture_float_linear"),t("EXT_color_buffer_half_float"),t("WEBGL_multisampled_render_to_texture"),t("WEBGL_render_shared_exponent")},get:function(i){let s=t(i);return s===null&&ls("THREE.WebGLRenderer: "+i+" extension not supported."),s}}}function mg(n,e,t,i){let s={},r=new WeakMap;function o(u){let p=u.target;p.index!==null&&e.remove(p.index);for(let g in p.attributes)e.remove(p.attributes[g]);p.removeEventListener("dispose",o),delete s[p.id];let d=r.get(p);d&&(e.remove(d),r.delete(p)),i.releaseStatesOfGeometry(p),p.isInstancedBufferGeometry===!0&&delete p._maxInstanceCount,t.memory.geometries--}function a(u,p){return s[p.id]===!0||(p.addEventListener("dispose",o),s[p.id]=!0,t.memory.geometries++),p}function c(u){let p=u.attributes;for(let d in p)e.update(p[d],n.ARRAY_BUFFER)}function l(u){let p=[],d=u.index,g=u.attributes.position,v=0;if(d!==null){let T=d.array;v=d.version;for(let E=0,S=T.length;E<S;E+=3){let R=T[E+0],A=T[E+1],P=T[E+2];p.push(R,A,A,P,P,R)}}else if(g!==void 0){let T=g.array;v=g.version;for(let E=0,S=T.length/3-1;E<S;E+=3){let R=E+0,A=E+1,P=E+2;p.push(R,A,A,P,P,R)}}else return;let m=new(Hl(p)?Xs:Ws)(p,1);m.version=v;let f=r.get(u);f&&e.remove(f),r.set(u,m)}function h(u){let p=r.get(u);if(p){let d=u.index;d!==null&&p.version<d.version&&l(u)}else l(u);return r.get(u)}return{get:a,update:c,getWireframeAttribute:h}}function gg(n,e,t){let i;function s(p){i=p}let r,o;function a(p){r=p.type,o=p.bytesPerElement}function c(p,d){n.drawElements(i,d,r,p*o),t.update(d,i,1)}function l(p,d,g){g!==0&&(n.drawElementsInstanced(i,d,r,p*o,g),t.update(d,i,g))}function h(p,d,g){if(g===0)return;e.get("WEBGL_multi_draw").multiDrawElementsWEBGL(i,d,0,r,p,0,g);let m=0;for(let f=0;f<g;f++)m+=d[f];t.update(m,i,1)}function u(p,d,g,v){if(g===0)return;let m=e.get("WEBGL_multi_draw");if(m===null)for(let f=0;f<p.length;f++)l(p[f]/o,d[f],v[f]);else{m.multiDrawElementsInstancedWEBGL(i,d,0,r,p,0,v,0,g);let f=0;for(let T=0;T<g;T++)f+=d[T]*v[T];t.update(f,i,1)}}this.setMode=s,this.setIndex=a,this.render=c,this.renderInstances=l,this.renderMultiDraw=h,this.renderMultiDrawInstances=u}function _g(n){let e={geometries:0,textures:0},t={frame:0,calls:0,triangles:0,points:0,lines:0};function i(r,o,a){switch(t.calls++,o){case n.TRIANGLES:t.triangles+=a*(r/3);break;case n.LINES:t.lines+=a*(r/2);break;case n.LINE_STRIP:t.lines+=a*(r-1);break;case n.LINE_LOOP:t.lines+=a*r;break;case n.POINTS:t.points+=a*r;break;default:console.error("THREE.WebGLInfo: Unknown draw mode:",o);break}}function s(){t.calls=0,t.triangles=0,t.points=0,t.lines=0}return{memory:e,render:t,programs:null,autoReset:!0,reset:s,update:i}}function xg(n,e,t){let i=new WeakMap,s=new Qe;function r(o,a,c){let l=o.morphTargetInfluences,h=a.morphAttributes.position||a.morphAttributes.normal||a.morphAttributes.color,u=h!==void 0?h.length:0,p=i.get(a);if(p===void 0||p.count!==u){let M=function(){P.dispose(),i.delete(a),a.removeEventListener("dispose",M)};p!==void 0&&p.texture.dispose();let d=a.morphAttributes.position!==void 0,g=a.morphAttributes.normal!==void 0,v=a.morphAttributes.color!==void 0,m=a.morphAttributes.position||[],f=a.morphAttributes.normal||[],T=a.morphAttributes.color||[],E=0;d===!0&&(E=1),g===!0&&(E=2),v===!0&&(E=3);let S=a.attributes.position.count*E,R=1;S>e.maxTextureSize&&(R=Math.ceil(S/e.maxTextureSize),S=e.maxTextureSize);let A=new Float32Array(S*R*4*u),P=new Gs(A,S,R,u);P.type=Tn,P.needsUpdate=!0;let N=E*4;for(let y=0;y<u;y++){let I=m[y],O=f[y],z=T[y],H=S*R*4*y;for(let q=0;q<I.count;q++){let W=q*N;d===!0&&(s.fromBufferAttribute(I,q),A[H+W+0]=s.x,A[H+W+1]=s.y,A[H+W+2]=s.z,A[H+W+3]=0),g===!0&&(s.fromBufferAttribute(O,q),A[H+W+4]=s.x,A[H+W+5]=s.y,A[H+W+6]=s.z,A[H+W+7]=0),v===!0&&(s.fromBufferAttribute(z,q),A[H+W+8]=s.x,A[H+W+9]=s.y,A[H+W+10]=s.z,A[H+W+11]=z.itemSize===4?s.w:1)}}p={count:u,texture:P,size:new Ue(S,R)},i.set(a,p),a.addEventListener("dispose",M)}if(o.isInstancedMesh===!0&&o.morphTexture!==null)c.getUniforms().setValue(n,"morphTexture",o.morphTexture,t);else{let d=0;for(let v=0;v<l.length;v++)d+=l[v];let g=a.morphTargetsRelative?1:1-d;c.getUniforms().setValue(n,"morphTargetBaseInfluence",g),c.getUniforms().setValue(n,"morphTargetInfluences",l)}c.getUniforms().setValue(n,"morphTargetsTexture",p.texture,t),c.getUniforms().setValue(n,"morphTargetsTextureSize",p.size)}return{update:r}}function vg(n,e,t,i){let s=new WeakMap;function r(c){let l=i.render.frame,h=c.geometry,u=e.get(c,h);if(s.get(u)!==l&&(e.update(u),s.set(u,l)),c.isInstancedMesh&&(c.hasEventListener("dispose",a)===!1&&c.addEventListener("dispose",a),s.get(c)!==l&&(t.update(c.instanceMatrix,n.ARRAY_BUFFER),c.instanceColor!==null&&t.update(c.instanceColor,n.ARRAY_BUFFER),s.set(c,l))),c.isSkinnedMesh){let p=c.skeleton;s.get(p)!==l&&(p.update(),s.set(p,l))}return u}function o(){s=new WeakMap}function a(c){let l=c.target;l.removeEventListener("dispose",a),t.remove(l.instanceMatrix),l.instanceColor!==null&&t.remove(l.instanceColor)}return{update:r,dispose:o}}var Ju=new Wt,Lu=new js(1,1),Ku=new Gs,ju=new aa,Qu=new $s,Du=[],Uu=[],Nu=new Float32Array(16),Fu=new Float32Array(9),Ou=new Float32Array(4);function Es(n,e,t){let i=n[0];if(i<=0||i>0)return n;let s=e*t,r=Du[s];if(r===void 0&&(r=new Float32Array(s),Du[s]=r),e!==0){i.toArray(r,0);for(let o=1,a=0;o!==e;++o)a+=t,n[o].toArray(r,a)}return r}function Mt(n,e){if(n.length!==e.length)return!1;for(let t=0,i=n.length;t<i;t++)if(n[t]!==e[t])return!1;return!0}function St(n,e){for(let t=0,i=e.length;t<i;t++)n[t]=e[t]}function Mo(n,e){let t=Uu[e];t===void 0&&(t=new Int32Array(e),Uu[e]=t);for(let i=0;i!==e;++i)t[i]=n.allocateTextureUnit();return t}function yg(n,e){let t=this.cache;t[0]!==e&&(n.uniform1f(this.addr,e),t[0]=e)}function Mg(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y)&&(n.uniform2f(this.addr,e.x,e.y),t[0]=e.x,t[1]=e.y);else{if(Mt(t,e))return;n.uniform2fv(this.addr,e),St(t,e)}}function Sg(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y||t[2]!==e.z)&&(n.uniform3f(this.addr,e.x,e.y,e.z),t[0]=e.x,t[1]=e.y,t[2]=e.z);else if(e.r!==void 0)(t[0]!==e.r||t[1]!==e.g||t[2]!==e.b)&&(n.uniform3f(this.addr,e.r,e.g,e.b),t[0]=e.r,t[1]=e.g,t[2]=e.b);else{if(Mt(t,e))return;n.uniform3fv(this.addr,e),St(t,e)}}function bg(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y||t[2]!==e.z||t[3]!==e.w)&&(n.uniform4f(this.addr,e.x,e.y,e.z,e.w),t[0]=e.x,t[1]=e.y,t[2]=e.z,t[3]=e.w);else{if(Mt(t,e))return;n.uniform4fv(this.addr,e),St(t,e)}}function Eg(n,e){let t=this.cache,i=e.elements;if(i===void 0){if(Mt(t,e))return;n.uniformMatrix2fv(this.addr,!1,e),St(t,e)}else{if(Mt(t,i))return;Ou.set(i),n.uniformMatrix2fv(this.addr,!1,Ou),St(t,i)}}function Tg(n,e){let t=this.cache,i=e.elements;if(i===void 0){if(Mt(t,e))return;n.uniformMatrix3fv(this.addr,!1,e),St(t,e)}else{if(Mt(t,i))return;Fu.set(i),n.uniformMatrix3fv(this.addr,!1,Fu),St(t,i)}}function wg(n,e){let t=this.cache,i=e.elements;if(i===void 0){if(Mt(t,e))return;n.uniformMatrix4fv(this.addr,!1,e),St(t,e)}else{if(Mt(t,i))return;Nu.set(i),n.uniformMatrix4fv(this.addr,!1,Nu),St(t,i)}}function Ag(n,e){let t=this.cache;t[0]!==e&&(n.uniform1i(this.addr,e),t[0]=e)}function Rg(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y)&&(n.uniform2i(this.addr,e.x,e.y),t[0]=e.x,t[1]=e.y);else{if(Mt(t,e))return;n.uniform2iv(this.addr,e),St(t,e)}}function Cg(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y||t[2]!==e.z)&&(n.uniform3i(this.addr,e.x,e.y,e.z),t[0]=e.x,t[1]=e.y,t[2]=e.z);else{if(Mt(t,e))return;n.uniform3iv(this.addr,e),St(t,e)}}function Ig(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y||t[2]!==e.z||t[3]!==e.w)&&(n.uniform4i(this.addr,e.x,e.y,e.z,e.w),t[0]=e.x,t[1]=e.y,t[2]=e.z,t[3]=e.w);else{if(Mt(t,e))return;n.uniform4iv(this.addr,e),St(t,e)}}function Pg(n,e){let t=this.cache;t[0]!==e&&(n.uniform1ui(this.addr,e),t[0]=e)}function Lg(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y)&&(n.uniform2ui(this.addr,e.x,e.y),t[0]=e.x,t[1]=e.y);else{if(Mt(t,e))return;n.uniform2uiv(this.addr,e),St(t,e)}}function Dg(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y||t[2]!==e.z)&&(n.uniform3ui(this.addr,e.x,e.y,e.z),t[0]=e.x,t[1]=e.y,t[2]=e.z);else{if(Mt(t,e))return;n.uniform3uiv(this.addr,e),St(t,e)}}function Ug(n,e){let t=this.cache;if(e.x!==void 0)(t[0]!==e.x||t[1]!==e.y||t[2]!==e.z||t[3]!==e.w)&&(n.uniform4ui(this.addr,e.x,e.y,e.z,e.w),t[0]=e.x,t[1]=e.y,t[2]=e.z,t[3]=e.w);else{if(Mt(t,e))return;n.uniform4uiv(this.addr,e),St(t,e)}}function Ng(n,e,t){let i=this.cache,s=t.allocateTextureUnit();i[0]!==s&&(n.uniform1i(this.addr,s),i[0]=s);let r;this.type===n.SAMPLER_2D_SHADOW?(Lu.compareFunction=Vl,r=Lu):r=Ju,t.setTexture2D(e||r,s)}function Fg(n,e,t){let i=this.cache,s=t.allocateTextureUnit();i[0]!==s&&(n.uniform1i(this.addr,s),i[0]=s),t.setTexture3D(e||ju,s)}function Og(n,e,t){let i=this.cache,s=t.allocateTextureUnit();i[0]!==s&&(n.uniform1i(this.addr,s),i[0]=s),t.setTextureCube(e||Qu,s)}function kg(n,e,t){let i=this.cache,s=t.allocateTextureUnit();i[0]!==s&&(n.uniform1i(this.addr,s),i[0]=s),t.setTexture2DArray(e||Ku,s)}function Bg(n){switch(n){case 5126:return yg;case 35664:return Mg;case 35665:return Sg;case 35666:return bg;case 35674:return Eg;case 35675:return Tg;case 35676:return wg;case 5124:case 35670:return Ag;case 35667:case 35671:return Rg;case 35668:case 35672:return Cg;case 35669:case 35673:return Ig;case 5125:return Pg;case 36294:return Lg;case 36295:return Dg;case 36296:return Ug;case 35678:case 36198:case 36298:case 36306:case 35682:return Ng;case 35679:case 36299:case 36307:return Fg;case 35680:case 36300:case 36308:case 36293:return Og;case 36289:case 36303:case 36311:case 36292:return kg}}function zg(n,e){n.uniform1fv(this.addr,e)}function Vg(n,e){let t=Es(e,this.size,2);n.uniform2fv(this.addr,t)}function Gg(n,e){let t=Es(e,this.size,3);n.uniform3fv(this.addr,t)}function Hg(n,e){let t=Es(e,this.size,4);n.uniform4fv(this.addr,t)}function Wg(n,e){let t=Es(e,this.size,4);n.uniformMatrix2fv(this.addr,!1,t)}function Xg(n,e){let t=Es(e,this.size,9);n.uniformMatrix3fv(this.addr,!1,t)}function qg(n,e){let t=Es(e,this.size,16);n.uniformMatrix4fv(this.addr,!1,t)}function $g(n,e){n.uniform1iv(this.addr,e)}function Yg(n,e){n.uniform2iv(this.addr,e)}function Zg(n,e){n.uniform3iv(this.addr,e)}function Jg(n,e){n.uniform4iv(this.addr,e)}function Kg(n,e){n.uniform1uiv(this.addr,e)}function jg(n,e){n.uniform2uiv(this.addr,e)}function Qg(n,e){n.uniform3uiv(this.addr,e)}function e0(n,e){n.uniform4uiv(this.addr,e)}function t0(n,e,t){let i=this.cache,s=e.length,r=Mo(t,s);Mt(i,r)||(n.uniform1iv(this.addr,r),St(i,r));for(let o=0;o!==s;++o)t.setTexture2D(e[o]||Ju,r[o])}function n0(n,e,t){let i=this.cache,s=e.length,r=Mo(t,s);Mt(i,r)||(n.uniform1iv(this.addr,r),St(i,r));for(let o=0;o!==s;++o)t.setTexture3D(e[o]||ju,r[o])}function i0(n,e,t){let i=this.cache,s=e.length,r=Mo(t,s);Mt(i,r)||(n.uniform1iv(this.addr,r),St(i,r));for(let o=0;o!==s;++o)t.setTextureCube(e[o]||Qu,r[o])}function s0(n,e,t){let i=this.cache,s=e.length,r=Mo(t,s);Mt(i,r)||(n.uniform1iv(this.addr,r),St(i,r));for(let o=0;o!==s;++o)t.setTexture2DArray(e[o]||Ku,r[o])}function r0(n){switch(n){case 5126:return zg;case 35664:return Vg;case 35665:return Gg;case 35666:return Hg;case 35674:return Wg;case 35675:return Xg;case 35676:return qg;case 5124:case 35670:return $g;case 35667:case 35671:return Yg;case 35668:case 35672:return Zg;case 35669:case 35673:return Jg;case 5125:return Kg;case 36294:return jg;case 36295:return Qg;case 36296:return e0;case 35678:case 36198:case 36298:case 36306:case 35682:return t0;case 35679:case 36299:case 36307:return n0;case 35680:case 36300:case 36308:case 36293:return i0;case 36289:case 36303:case 36311:case 36292:return s0}}var ec=class{constructor(e,t,i){this.id=e,this.addr=i,this.cache=[],this.type=t.type,this.setValue=Bg(t.type)}},tc=class{constructor(e,t,i){this.id=e,this.addr=i,this.cache=[],this.type=t.type,this.size=t.size,this.setValue=r0(t.type)}},nc=class{constructor(e){this.id=e,this.seq=[],this.map={}}setValue(e,t,i){let s=this.seq;for(let r=0,o=s.length;r!==o;++r){let a=s[r];a.setValue(e,t[a.id],i)}}},Ql=/(\w+)(\])?(\[|\.)?/g;function ku(n,e){n.seq.push(e),n.map[e.id]=e}function a0(n,e,t){let i=n.name,s=i.length;for(Ql.lastIndex=0;;){let r=Ql.exec(i),o=Ql.lastIndex,a=r[1],c=r[2]==="]",l=r[3];if(c&&(a=a|0),l===void 0||l==="["&&o+2===s){ku(t,l===void 0?new ec(a,n,e):new tc(a,n,e));break}else{let u=t.map[a];u===void 0&&(u=new nc(a),ku(t,u)),t=u}}}var bs=class{constructor(e,t){this.seq=[],this.map={};let i=e.getProgramParameter(t,e.ACTIVE_UNIFORMS);for(let s=0;s<i;++s){let r=e.getActiveUniform(t,s),o=e.getUniformLocation(t,r.name);a0(r,o,this)}}setValue(e,t,i,s){let r=this.map[t];r!==void 0&&r.setValue(e,i,s)}setOptional(e,t,i){let s=t[i];s!==void 0&&this.setValue(e,i,s)}static upload(e,t,i,s){for(let r=0,o=t.length;r!==o;++r){let a=t[r],c=i[a.id];c.needsUpdate!==!1&&a.setValue(e,c.value,s)}}static seqWithValue(e,t){let i=[];for(let s=0,r=e.length;s!==r;++s){let o=e[s];o.id in t&&i.push(o)}return i}};function Bu(n,e,t){let i=n.createShader(e);return n.shaderSource(i,t),n.compileShader(i),i}var o0=37297,l0=0;function c0(n,e){let t=n.split(`
`),i=[],s=Math.max(e-6,0),r=Math.min(e+6,t.length);for(let o=s;o<r;o++){let a=o+1;i.push(`${a===e?">":" "} ${a}: ${t[o]}`)}return i.join(`
`)}var zu=new Oe;function h0(n){Ye._getMatrix(zu,Ye.workingColorSpace,n);let e=`mat3( ${zu.elements.map(t=>t.toFixed(4))} )`;switch(Ye.getTransfer(n)){case Bs:return[e,"LinearTransferOETF"];case et:return[e,"sRGBTransferOETF"];default:return console.warn("THREE.WebGLProgram: Unsupported color space: ",n),[e,"LinearTransferOETF"]}}function Vu(n,e,t){let i=n.getShaderParameter(e,n.COMPILE_STATUS),r=(n.getShaderInfoLog(e)||"").trim();if(i&&r==="")return"";let o=/ERROR: 0:(\d+)/.exec(r);if(o){let a=parseInt(o[1]);return t.toUpperCase()+`

`+r+`

`+c0(n.getShaderSource(e),a)}else return r}function u0(n,e){let t=h0(e);return[`vec4 ${n}( vec4 value ) {`,`	return ${t[1]}( vec4( value.rgb * ${t[0]}, value.a ) );`,"}"].join(`
`)}function d0(n,e){let t;switch(e){case iu:t="Linear";break;case su:t="Reinhard";break;case ru:t="Cineon";break;case Pa:t="ACESFilmic";break;case ou:t="AgX";break;case lu:t="Neutral";break;case au:t="Custom";break;default:console.warn("THREE.WebGLProgram: Unsupported toneMapping:",e),t="Linear"}return"vec3 "+n+"( vec3 color ) { return "+t+"ToneMapping( color ); }"}var xo=new L;function f0(){Ye.getLuminanceCoefficients(xo);let n=xo.x.toFixed(4),e=xo.y.toFixed(4),t=xo.z.toFixed(4);return["float luminance( const in vec3 rgb ) {",`	const vec3 weights = vec3( ${n}, ${e}, ${t} );`,"	return dot( weights, rgb );","}"].join(`
`)}function p0(n){return[n.extensionClipCullDistance?"#extension GL_ANGLE_clip_cull_distance : require":"",n.extensionMultiDraw?"#extension GL_ANGLE_multi_draw : require":""].filter(pr).join(`
`)}function m0(n){let e=[];for(let t in n){let i=n[t];i!==!1&&e.push("#define "+t+" "+i)}return e.join(`
`)}function g0(n,e){let t={},i=n.getProgramParameter(e,n.ACTIVE_ATTRIBUTES);for(let s=0;s<i;s++){let r=n.getActiveAttrib(e,s),o=r.name,a=1;r.type===n.FLOAT_MAT2&&(a=2),r.type===n.FLOAT_MAT3&&(a=3),r.type===n.FLOAT_MAT4&&(a=4),t[o]={type:r.type,location:n.getAttribLocation(e,o),locationSize:a}}return t}function pr(n){return n!==""}function Gu(n,e){let t=e.numSpotLightShadows+e.numSpotLightMaps-e.numSpotLightShadowsWithMaps;return n.replace(/NUM_DIR_LIGHTS/g,e.numDirLights).replace(/NUM_SPOT_LIGHTS/g,e.numSpotLights).replace(/NUM_SPOT_LIGHT_MAPS/g,e.numSpotLightMaps).replace(/NUM_SPOT_LIGHT_COORDS/g,t).replace(/NUM_RECT_AREA_LIGHTS/g,e.numRectAreaLights).replace(/NUM_POINT_LIGHTS/g,e.numPointLights).replace(/NUM_HEMI_LIGHTS/g,e.numHemiLights).replace(/NUM_DIR_LIGHT_SHADOWS/g,e.numDirLightShadows).replace(/NUM_SPOT_LIGHT_SHADOWS_WITH_MAPS/g,e.numSpotLightShadowsWithMaps).replace(/NUM_SPOT_LIGHT_SHADOWS/g,e.numSpotLightShadows).replace(/NUM_POINT_LIGHT_SHADOWS/g,e.numPointLightShadows)}function Hu(n,e){return n.replace(/NUM_CLIPPING_PLANES/g,e.numClippingPlanes).replace(/UNION_CLIPPING_PLANES/g,e.numClippingPlanes-e.numClipIntersection)}var _0=/^[ \t]*#include +<([\w\d./]+)>/gm;function ic(n){return n.replace(_0,v0)}var x0=new Map;function v0(n,e){let t=ze[e];if(t===void 0){let i=x0.get(e);if(i!==void 0)t=ze[i],console.warn('THREE.WebGLRenderer: Shader chunk "%s" has been deprecated. Use "%s" instead.',e,i);else throw new Error("Can not resolve #include <"+e+">")}return ic(t)}var y0=/#pragma unroll_loop_start\s+for\s*\(\s*int\s+i\s*=\s*(\d+)\s*;\s*i\s*<\s*(\d+)\s*;\s*i\s*\+\+\s*\)\s*{([\s\S]+?)}\s+#pragma unroll_loop_end/g;function Wu(n){return n.replace(y0,M0)}function M0(n,e,t,i){let s="";for(let r=parseInt(e);r<parseInt(t);r++)s+=i.replace(/\[\s*i\s*\]/g,"[ "+r+" ]").replace(/UNROLLED_LOOP_INDEX/g,r);return s}function Xu(n){let e=`precision ${n.precision} float;
	precision ${n.precision} int;
	precision ${n.precision} sampler2D;
	precision ${n.precision} samplerCube;
	precision ${n.precision} sampler3D;
	precision ${n.precision} sampler2DArray;
	precision ${n.precision} sampler2DShadow;
	precision ${n.precision} samplerCubeShadow;
	precision ${n.precision} sampler2DArrayShadow;
	precision ${n.precision} isampler2D;
	precision ${n.precision} isampler3D;
	precision ${n.precision} isamplerCube;
	precision ${n.precision} isampler2DArray;
	precision ${n.precision} usampler2D;
	precision ${n.precision} usampler3D;
	precision ${n.precision} usamplerCube;
	precision ${n.precision} usampler2DArray;
	`;return n.precision==="highp"?e+=`
#define HIGH_PRECISION`:n.precision==="mediump"?e+=`
#define MEDIUM_PRECISION`:n.precision==="lowp"&&(e+=`
#define LOW_PRECISION`),e}function S0(n){let e="SHADOWMAP_TYPE_BASIC";return n.shadowMapType===wl?e="SHADOWMAP_TYPE_PCF":n.shadowMapType===ba?e="SHADOWMAP_TYPE_PCF_SOFT":n.shadowMapType===En&&(e="SHADOWMAP_TYPE_VSM"),e}function b0(n){let e="ENVMAP_TYPE_CUBE";if(n.envMap)switch(n.envMapMode){case Pi:case Li:e="ENVMAP_TYPE_CUBE";break;case lr:e="ENVMAP_TYPE_CUBE_UV";break}return e}function E0(n){let e="ENVMAP_MODE_REFLECTION";if(n.envMap)switch(n.envMapMode){case Li:e="ENVMAP_MODE_REFRACTION";break}return e}function T0(n){let e="ENVMAP_BLENDING_NONE";if(n.envMap)switch(n.combine){case Il:e="ENVMAP_BLENDING_MULTIPLY";break;case tu:e="ENVMAP_BLENDING_MIX";break;case nu:e="ENVMAP_BLENDING_ADD";break}return e}function w0(n){let e=n.envMapCubeUVHeight;if(e===null)return null;let t=Math.log2(e)-2,i=1/e;return{texelWidth:1/(3*Math.max(Math.pow(2,t),112)),texelHeight:i,maxMip:t}}function A0(n,e,t,i){let s=n.getContext(),r=t.defines,o=t.vertexShader,a=t.fragmentShader,c=S0(t),l=b0(t),h=E0(t),u=T0(t),p=w0(t),d=p0(t),g=m0(r),v=s.createProgram(),m,f,T=t.glslVersion?"#version "+t.glslVersion+`
`:"";t.isRawShaderMaterial?(m=["#define SHADER_TYPE "+t.shaderType,"#define SHADER_NAME "+t.shaderName,g].filter(pr).join(`
`),m.length>0&&(m+=`
`),f=["#define SHADER_TYPE "+t.shaderType,"#define SHADER_NAME "+t.shaderName,g].filter(pr).join(`
`),f.length>0&&(f+=`
`)):(m=[Xu(t),"#define SHADER_TYPE "+t.shaderType,"#define SHADER_NAME "+t.shaderName,g,t.extensionClipCullDistance?"#define USE_CLIP_DISTANCE":"",t.batching?"#define USE_BATCHING":"",t.batchingColor?"#define USE_BATCHING_COLOR":"",t.instancing?"#define USE_INSTANCING":"",t.instancingColor?"#define USE_INSTANCING_COLOR":"",t.instancingMorph?"#define USE_INSTANCING_MORPH":"",t.useFog&&t.fog?"#define USE_FOG":"",t.useFog&&t.fogExp2?"#define FOG_EXP2":"",t.map?"#define USE_MAP":"",t.envMap?"#define USE_ENVMAP":"",t.envMap?"#define "+h:"",t.lightMap?"#define USE_LIGHTMAP":"",t.aoMap?"#define USE_AOMAP":"",t.bumpMap?"#define USE_BUMPMAP":"",t.normalMap?"#define USE_NORMALMAP":"",t.normalMapObjectSpace?"#define USE_NORMALMAP_OBJECTSPACE":"",t.normalMapTangentSpace?"#define USE_NORMALMAP_TANGENTSPACE":"",t.displacementMap?"#define USE_DISPLACEMENTMAP":"",t.emissiveMap?"#define USE_EMISSIVEMAP":"",t.anisotropy?"#define USE_ANISOTROPY":"",t.anisotropyMap?"#define USE_ANISOTROPYMAP":"",t.clearcoatMap?"#define USE_CLEARCOATMAP":"",t.clearcoatRoughnessMap?"#define USE_CLEARCOAT_ROUGHNESSMAP":"",t.clearcoatNormalMap?"#define USE_CLEARCOAT_NORMALMAP":"",t.iridescenceMap?"#define USE_IRIDESCENCEMAP":"",t.iridescenceThicknessMap?"#define USE_IRIDESCENCE_THICKNESSMAP":"",t.specularMap?"#define USE_SPECULARMAP":"",t.specularColorMap?"#define USE_SPECULAR_COLORMAP":"",t.specularIntensityMap?"#define USE_SPECULAR_INTENSITYMAP":"",t.roughnessMap?"#define USE_ROUGHNESSMAP":"",t.metalnessMap?"#define USE_METALNESSMAP":"",t.alphaMap?"#define USE_ALPHAMAP":"",t.alphaHash?"#define USE_ALPHAHASH":"",t.transmission?"#define USE_TRANSMISSION":"",t.transmissionMap?"#define USE_TRANSMISSIONMAP":"",t.thicknessMap?"#define USE_THICKNESSMAP":"",t.sheenColorMap?"#define USE_SHEEN_COLORMAP":"",t.sheenRoughnessMap?"#define USE_SHEEN_ROUGHNESSMAP":"",t.mapUv?"#define MAP_UV "+t.mapUv:"",t.alphaMapUv?"#define ALPHAMAP_UV "+t.alphaMapUv:"",t.lightMapUv?"#define LIGHTMAP_UV "+t.lightMapUv:"",t.aoMapUv?"#define AOMAP_UV "+t.aoMapUv:"",t.emissiveMapUv?"#define EMISSIVEMAP_UV "+t.emissiveMapUv:"",t.bumpMapUv?"#define BUMPMAP_UV "+t.bumpMapUv:"",t.normalMapUv?"#define NORMALMAP_UV "+t.normalMapUv:"",t.displacementMapUv?"#define DISPLACEMENTMAP_UV "+t.displacementMapUv:"",t.metalnessMapUv?"#define METALNESSMAP_UV "+t.metalnessMapUv:"",t.roughnessMapUv?"#define ROUGHNESSMAP_UV "+t.roughnessMapUv:"",t.anisotropyMapUv?"#define ANISOTROPYMAP_UV "+t.anisotropyMapUv:"",t.clearcoatMapUv?"#define CLEARCOATMAP_UV "+t.clearcoatMapUv:"",t.clearcoatNormalMapUv?"#define CLEARCOAT_NORMALMAP_UV "+t.clearcoatNormalMapUv:"",t.clearcoatRoughnessMapUv?"#define CLEARCOAT_ROUGHNESSMAP_UV "+t.clearcoatRoughnessMapUv:"",t.iridescenceMapUv?"#define IRIDESCENCEMAP_UV "+t.iridescenceMapUv:"",t.iridescenceThicknessMapUv?"#define IRIDESCENCE_THICKNESSMAP_UV "+t.iridescenceThicknessMapUv:"",t.sheenColorMapUv?"#define SHEEN_COLORMAP_UV "+t.sheenColorMapUv:"",t.sheenRoughnessMapUv?"#define SHEEN_ROUGHNESSMAP_UV "+t.sheenRoughnessMapUv:"",t.specularMapUv?"#define SPECULARMAP_UV "+t.specularMapUv:"",t.specularColorMapUv?"#define SPECULAR_COLORMAP_UV "+t.specularColorMapUv:"",t.specularIntensityMapUv?"#define SPECULAR_INTENSITYMAP_UV "+t.specularIntensityMapUv:"",t.transmissionMapUv?"#define TRANSMISSIONMAP_UV "+t.transmissionMapUv:"",t.thicknessMapUv?"#define THICKNESSMAP_UV "+t.thicknessMapUv:"",t.vertexTangents&&t.flatShading===!1?"#define USE_TANGENT":"",t.vertexColors?"#define USE_COLOR":"",t.vertexAlphas?"#define USE_COLOR_ALPHA":"",t.vertexUv1s?"#define USE_UV1":"",t.vertexUv2s?"#define USE_UV2":"",t.vertexUv3s?"#define USE_UV3":"",t.pointsUvs?"#define USE_POINTS_UV":"",t.flatShading?"#define FLAT_SHADED":"",t.skinning?"#define USE_SKINNING":"",t.morphTargets?"#define USE_MORPHTARGETS":"",t.morphNormals&&t.flatShading===!1?"#define USE_MORPHNORMALS":"",t.morphColors?"#define USE_MORPHCOLORS":"",t.morphTargetsCount>0?"#define MORPHTARGETS_TEXTURE_STRIDE "+t.morphTextureStride:"",t.morphTargetsCount>0?"#define MORPHTARGETS_COUNT "+t.morphTargetsCount:"",t.doubleSided?"#define DOUBLE_SIDED":"",t.flipSided?"#define FLIP_SIDED":"",t.shadowMapEnabled?"#define USE_SHADOWMAP":"",t.shadowMapEnabled?"#define "+c:"",t.sizeAttenuation?"#define USE_SIZEATTENUATION":"",t.numLightProbes>0?"#define USE_LIGHT_PROBES":"",t.logarithmicDepthBuffer?"#define USE_LOGARITHMIC_DEPTH_BUFFER":"",t.reversedDepthBuffer?"#define USE_REVERSED_DEPTH_BUFFER":"","uniform mat4 modelMatrix;","uniform mat4 modelViewMatrix;","uniform mat4 projectionMatrix;","uniform mat4 viewMatrix;","uniform mat3 normalMatrix;","uniform vec3 cameraPosition;","uniform bool isOrthographic;","#ifdef USE_INSTANCING","	attribute mat4 instanceMatrix;","#endif","#ifdef USE_INSTANCING_COLOR","	attribute vec3 instanceColor;","#endif","#ifdef USE_INSTANCING_MORPH","	uniform sampler2D morphTexture;","#endif","attribute vec3 position;","attribute vec3 normal;","attribute vec2 uv;","#ifdef USE_UV1","	attribute vec2 uv1;","#endif","#ifdef USE_UV2","	attribute vec2 uv2;","#endif","#ifdef USE_UV3","	attribute vec2 uv3;","#endif","#ifdef USE_TANGENT","	attribute vec4 tangent;","#endif","#if defined( USE_COLOR_ALPHA )","	attribute vec4 color;","#elif defined( USE_COLOR )","	attribute vec3 color;","#endif","#ifdef USE_SKINNING","	attribute vec4 skinIndex;","	attribute vec4 skinWeight;","#endif",`
`].filter(pr).join(`
`),f=[Xu(t),"#define SHADER_TYPE "+t.shaderType,"#define SHADER_NAME "+t.shaderName,g,t.useFog&&t.fog?"#define USE_FOG":"",t.useFog&&t.fogExp2?"#define FOG_EXP2":"",t.alphaToCoverage?"#define ALPHA_TO_COVERAGE":"",t.map?"#define USE_MAP":"",t.matcap?"#define USE_MATCAP":"",t.envMap?"#define USE_ENVMAP":"",t.envMap?"#define "+l:"",t.envMap?"#define "+h:"",t.envMap?"#define "+u:"",p?"#define CUBEUV_TEXEL_WIDTH "+p.texelWidth:"",p?"#define CUBEUV_TEXEL_HEIGHT "+p.texelHeight:"",p?"#define CUBEUV_MAX_MIP "+p.maxMip+".0":"",t.lightMap?"#define USE_LIGHTMAP":"",t.aoMap?"#define USE_AOMAP":"",t.bumpMap?"#define USE_BUMPMAP":"",t.normalMap?"#define USE_NORMALMAP":"",t.normalMapObjectSpace?"#define USE_NORMALMAP_OBJECTSPACE":"",t.normalMapTangentSpace?"#define USE_NORMALMAP_TANGENTSPACE":"",t.emissiveMap?"#define USE_EMISSIVEMAP":"",t.anisotropy?"#define USE_ANISOTROPY":"",t.anisotropyMap?"#define USE_ANISOTROPYMAP":"",t.clearcoat?"#define USE_CLEARCOAT":"",t.clearcoatMap?"#define USE_CLEARCOATMAP":"",t.clearcoatRoughnessMap?"#define USE_CLEARCOAT_ROUGHNESSMAP":"",t.clearcoatNormalMap?"#define USE_CLEARCOAT_NORMALMAP":"",t.dispersion?"#define USE_DISPERSION":"",t.iridescence?"#define USE_IRIDESCENCE":"",t.iridescenceMap?"#define USE_IRIDESCENCEMAP":"",t.iridescenceThicknessMap?"#define USE_IRIDESCENCE_THICKNESSMAP":"",t.specularMap?"#define USE_SPECULARMAP":"",t.specularColorMap?"#define USE_SPECULAR_COLORMAP":"",t.specularIntensityMap?"#define USE_SPECULAR_INTENSITYMAP":"",t.roughnessMap?"#define USE_ROUGHNESSMAP":"",t.metalnessMap?"#define USE_METALNESSMAP":"",t.alphaMap?"#define USE_ALPHAMAP":"",t.alphaTest?"#define USE_ALPHATEST":"",t.alphaHash?"#define USE_ALPHAHASH":"",t.sheen?"#define USE_SHEEN":"",t.sheenColorMap?"#define USE_SHEEN_COLORMAP":"",t.sheenRoughnessMap?"#define USE_SHEEN_ROUGHNESSMAP":"",t.transmission?"#define USE_TRANSMISSION":"",t.transmissionMap?"#define USE_TRANSMISSIONMAP":"",t.thicknessMap?"#define USE_THICKNESSMAP":"",t.vertexTangents&&t.flatShading===!1?"#define USE_TANGENT":"",t.vertexColors||t.instancingColor||t.batchingColor?"#define USE_COLOR":"",t.vertexAlphas?"#define USE_COLOR_ALPHA":"",t.vertexUv1s?"#define USE_UV1":"",t.vertexUv2s?"#define USE_UV2":"",t.vertexUv3s?"#define USE_UV3":"",t.pointsUvs?"#define USE_POINTS_UV":"",t.gradientMap?"#define USE_GRADIENTMAP":"",t.flatShading?"#define FLAT_SHADED":"",t.doubleSided?"#define DOUBLE_SIDED":"",t.flipSided?"#define FLIP_SIDED":"",t.shadowMapEnabled?"#define USE_SHADOWMAP":"",t.shadowMapEnabled?"#define "+c:"",t.premultipliedAlpha?"#define PREMULTIPLIED_ALPHA":"",t.numLightProbes>0?"#define USE_LIGHT_PROBES":"",t.decodeVideoTexture?"#define DECODE_VIDEO_TEXTURE":"",t.decodeVideoTextureEmissive?"#define DECODE_VIDEO_TEXTURE_EMISSIVE":"",t.logarithmicDepthBuffer?"#define USE_LOGARITHMIC_DEPTH_BUFFER":"",t.reversedDepthBuffer?"#define USE_REVERSED_DEPTH_BUFFER":"","uniform mat4 viewMatrix;","uniform vec3 cameraPosition;","uniform bool isOrthographic;",t.toneMapping!==Wn?"#define TONE_MAPPING":"",t.toneMapping!==Wn?ze.tonemapping_pars_fragment:"",t.toneMapping!==Wn?d0("toneMapping",t.toneMapping):"",t.dithering?"#define DITHERING":"",t.opaque?"#define OPAQUE":"",ze.colorspace_pars_fragment,u0("linearToOutputTexel",t.outputColorSpace),f0(),t.useDepthPacking?"#define DEPTH_PACKING "+t.depthPacking:"",`
`].filter(pr).join(`
`)),o=ic(o),o=Gu(o,t),o=Hu(o,t),a=ic(a),a=Gu(a,t),a=Hu(a,t),o=Wu(o),a=Wu(a),t.isRawShaderMaterial!==!0&&(T=`#version 300 es
`,m=[d,"#define attribute in","#define varying out","#define texture2D texture"].join(`
`)+`
`+m,f=["#define varying in",t.glslVersion===Gl?"":"layout(location = 0) out highp vec4 pc_fragColor;",t.glslVersion===Gl?"":"#define gl_FragColor pc_fragColor","#define gl_FragDepthEXT gl_FragDepth","#define texture2D texture","#define textureCube texture","#define texture2DProj textureProj","#define texture2DLodEXT textureLod","#define texture2DProjLodEXT textureProjLod","#define textureCubeLodEXT textureLod","#define texture2DGradEXT textureGrad","#define texture2DProjGradEXT textureProjGrad","#define textureCubeGradEXT textureGrad"].join(`
`)+`
`+f);let E=T+m+o,S=T+f+a,R=Bu(s,s.VERTEX_SHADER,E),A=Bu(s,s.FRAGMENT_SHADER,S);s.attachShader(v,R),s.attachShader(v,A),t.index0AttributeName!==void 0?s.bindAttribLocation(v,0,t.index0AttributeName):t.morphTargets===!0&&s.bindAttribLocation(v,0,"position"),s.linkProgram(v);function P(I){if(n.debug.checkShaderErrors){let O=s.getProgramInfoLog(v)||"",z=s.getShaderInfoLog(R)||"",H=s.getShaderInfoLog(A)||"",q=O.trim(),W=z.trim(),ne=H.trim(),V=!0,ae=!0;if(s.getProgramParameter(v,s.LINK_STATUS)===!1)if(V=!1,typeof n.debug.onShaderError=="function")n.debug.onShaderError(s,v,R,A);else{let ue=Vu(s,R,"vertex"),Ee=Vu(s,A,"fragment");console.error("THREE.WebGLProgram: Shader Error "+s.getError()+" - VALIDATE_STATUS "+s.getProgramParameter(v,s.VALIDATE_STATUS)+`

Material Name: `+I.name+`
Material Type: `+I.type+`

Program Info Log: `+q+`
`+ue+`
`+Ee)}else q!==""?console.warn("THREE.WebGLProgram: Program Info Log:",q):(W===""||ne==="")&&(ae=!1);ae&&(I.diagnostics={runnable:V,programLog:q,vertexShader:{log:W,prefix:m},fragmentShader:{log:ne,prefix:f}})}s.deleteShader(R),s.deleteShader(A),N=new bs(s,v),M=g0(s,v)}let N;this.getUniforms=function(){return N===void 0&&P(this),N};let M;this.getAttributes=function(){return M===void 0&&P(this),M};let y=t.rendererExtensionParallelShaderCompile===!1;return this.isReady=function(){return y===!1&&(y=s.getProgramParameter(v,o0)),y},this.destroy=function(){i.releaseStatesOfProgram(this),s.deleteProgram(v),this.program=void 0},this.type=t.shaderType,this.name=t.shaderName,this.id=l0++,this.cacheKey=e,this.usedTimes=1,this.program=v,this.vertexShader=R,this.fragmentShader=A,this}var R0=0,sc=class{constructor(){this.shaderCache=new Map,this.materialCache=new Map}update(e){let t=e.vertexShader,i=e.fragmentShader,s=this._getShaderStage(t),r=this._getShaderStage(i),o=this._getShaderCacheForMaterial(e);return o.has(s)===!1&&(o.add(s),s.usedTimes++),o.has(r)===!1&&(o.add(r),r.usedTimes++),this}remove(e){let t=this.materialCache.get(e);for(let i of t)i.usedTimes--,i.usedTimes===0&&this.shaderCache.delete(i.code);return this.materialCache.delete(e),this}getVertexShaderID(e){return this._getShaderStage(e.vertexShader).id}getFragmentShaderID(e){return this._getShaderStage(e.fragmentShader).id}dispose(){this.shaderCache.clear(),this.materialCache.clear()}_getShaderCacheForMaterial(e){let t=this.materialCache,i=t.get(e);return i===void 0&&(i=new Set,t.set(e,i)),i}_getShaderStage(e){let t=this.shaderCache,i=t.get(e);return i===void 0&&(i=new rc(e),t.set(e,i)),i}},rc=class{constructor(e){this.id=R0++,this.code=e,this.usedTimes=0}};function C0(n,e,t,i,s,r,o){let a=new us,c=new sc,l=new Set,h=[],u=s.logarithmicDepthBuffer,p=s.vertexTextures,d=s.precision,g={MeshDepthMaterial:"depth",MeshDistanceMaterial:"distanceRGBA",MeshNormalMaterial:"normal",MeshBasicMaterial:"basic",MeshLambertMaterial:"lambert",MeshPhongMaterial:"phong",MeshToonMaterial:"toon",MeshStandardMaterial:"physical",MeshPhysicalMaterial:"physical",MeshMatcapMaterial:"matcap",LineBasicMaterial:"basic",LineDashedMaterial:"dashed",PointsMaterial:"points",ShadowMaterial:"shadow",SpriteMaterial:"sprite"};function v(M){return l.add(M),M===0?"uv":`uv${M}`}function m(M,y,I,O,z){let H=O.fog,q=z.geometry,W=M.isMeshStandardMaterial?O.environment:null,ne=(M.isMeshStandardMaterial?t:e).get(M.envMap||W),V=ne&&ne.mapping===lr?ne.image.height:null,ae=g[M.type];M.precision!==null&&(d=s.getMaxPrecision(M.precision),d!==M.precision&&console.warn("THREE.WebGLProgram.getParameters:",M.precision,"not supported, using",d,"instead."));let ue=q.morphAttributes.position||q.morphAttributes.normal||q.morphAttributes.color,Ee=ue!==void 0?ue.length:0,Ve=0;q.morphAttributes.position!==void 0&&(Ve=1),q.morphAttributes.normal!==void 0&&(Ve=2),q.morphAttributes.color!==void 0&&(Ve=3);let st,ct,Je,$;if(ae){let Ke=wn[ae];st=Ke.vertexShader,ct=Ke.fragmentShader}else st=M.vertexShader,ct=M.fragmentShader,c.update(M),Je=c.getVertexShaderID(M),$=c.getFragmentShaderID(M);let K=n.getRenderTarget(),pe=n.state.buffers.depth.getReversed(),De=z.isInstancedMesh===!0,be=z.isBatchedMesh===!0,$e=!!M.map,Rt=!!M.matcap,w=!!ne,ht=!!M.aoMap,Fe=!!M.lightMap,Pe=!!M.bumpMap,_e=!!M.normalMap,ut=!!M.displacementMap,xe=!!M.emissiveMap,Be=!!M.metalnessMap,bt=!!M.roughnessMap,xt=M.anisotropy>0,b=M.clearcoat>0,_=M.dispersion>0,F=M.iridescence>0,X=M.sheen>0,J=M.transmission>0,G=xt&&!!M.anisotropyMap,Se=b&&!!M.clearcoatMap,ie=b&&!!M.clearcoatNormalMap,ve=b&&!!M.clearcoatRoughnessMap,ye=F&&!!M.iridescenceMap,ee=F&&!!M.iridescenceThicknessMap,he=X&&!!M.sheenColorMap,Ie=X&&!!M.sheenRoughnessMap,Me=!!M.specularMap,oe=!!M.specularColorMap,ke=!!M.specularIntensityMap,C=J&&!!M.transmissionMap,te=J&&!!M.thicknessMap,se=!!M.gradientMap,fe=!!M.alphaMap,j=M.alphaTest>0,Y=!!M.alphaHash,ge=!!M.extensions,Ne=Wn;M.toneMapped&&(K===null||K.isXRRenderTarget===!0)&&(Ne=n.toneMapping);let rt={shaderID:ae,shaderType:M.type,shaderName:M.name,vertexShader:st,fragmentShader:ct,defines:M.defines,customVertexShaderID:Je,customFragmentShaderID:$,isRawShaderMaterial:M.isRawShaderMaterial===!0,glslVersion:M.glslVersion,precision:d,batching:be,batchingColor:be&&z._colorsTexture!==null,instancing:De,instancingColor:De&&z.instanceColor!==null,instancingMorph:De&&z.morphTexture!==null,supportsVertexTextures:p,outputColorSpace:K===null?n.outputColorSpace:K.isXRRenderTarget===!0?K.texture.colorSpace:wi,alphaToCoverage:!!M.alphaToCoverage,map:$e,matcap:Rt,envMap:w,envMapMode:w&&ne.mapping,envMapCubeUVHeight:V,aoMap:ht,lightMap:Fe,bumpMap:Pe,normalMap:_e,displacementMap:p&&ut,emissiveMap:xe,normalMapObjectSpace:_e&&M.normalMapType===du,normalMapTangentSpace:_e&&M.normalMapType===zl,metalnessMap:Be,roughnessMap:bt,anisotropy:xt,anisotropyMap:G,clearcoat:b,clearcoatMap:Se,clearcoatNormalMap:ie,clearcoatRoughnessMap:ve,dispersion:_,iridescence:F,iridescenceMap:ye,iridescenceThicknessMap:ee,sheen:X,sheenColorMap:he,sheenRoughnessMap:Ie,specularMap:Me,specularColorMap:oe,specularIntensityMap:ke,transmission:J,transmissionMap:C,thicknessMap:te,gradientMap:se,opaque:M.transparent===!1&&M.blending===Ei&&M.alphaToCoverage===!1,alphaMap:fe,alphaTest:j,alphaHash:Y,combine:M.combine,mapUv:$e&&v(M.map.channel),aoMapUv:ht&&v(M.aoMap.channel),lightMapUv:Fe&&v(M.lightMap.channel),bumpMapUv:Pe&&v(M.bumpMap.channel),normalMapUv:_e&&v(M.normalMap.channel),displacementMapUv:ut&&v(M.displacementMap.channel),emissiveMapUv:xe&&v(M.emissiveMap.channel),metalnessMapUv:Be&&v(M.metalnessMap.channel),roughnessMapUv:bt&&v(M.roughnessMap.channel),anisotropyMapUv:G&&v(M.anisotropyMap.channel),clearcoatMapUv:Se&&v(M.clearcoatMap.channel),clearcoatNormalMapUv:ie&&v(M.clearcoatNormalMap.channel),clearcoatRoughnessMapUv:ve&&v(M.clearcoatRoughnessMap.channel),iridescenceMapUv:ye&&v(M.iridescenceMap.channel),iridescenceThicknessMapUv:ee&&v(M.iridescenceThicknessMap.channel),sheenColorMapUv:he&&v(M.sheenColorMap.channel),sheenRoughnessMapUv:Ie&&v(M.sheenRoughnessMap.channel),specularMapUv:Me&&v(M.specularMap.channel),specularColorMapUv:oe&&v(M.specularColorMap.channel),specularIntensityMapUv:ke&&v(M.specularIntensityMap.channel),transmissionMapUv:C&&v(M.transmissionMap.channel),thicknessMapUv:te&&v(M.thicknessMap.channel),alphaMapUv:fe&&v(M.alphaMap.channel),vertexTangents:!!q.attributes.tangent&&(_e||xt),vertexColors:M.vertexColors,vertexAlphas:M.vertexColors===!0&&!!q.attributes.color&&q.attributes.color.itemSize===4,pointsUvs:z.isPoints===!0&&!!q.attributes.uv&&($e||fe),fog:!!H,useFog:M.fog===!0,fogExp2:!!H&&H.isFogExp2,flatShading:M.flatShading===!0&&M.wireframe===!1,sizeAttenuation:M.sizeAttenuation===!0,logarithmicDepthBuffer:u,reversedDepthBuffer:pe,skinning:z.isSkinnedMesh===!0,morphTargets:q.morphAttributes.position!==void 0,morphNormals:q.morphAttributes.normal!==void 0,morphColors:q.morphAttributes.color!==void 0,morphTargetsCount:Ee,morphTextureStride:Ve,numDirLights:y.directional.length,numPointLights:y.point.length,numSpotLights:y.spot.length,numSpotLightMaps:y.spotLightMap.length,numRectAreaLights:y.rectArea.length,numHemiLights:y.hemi.length,numDirLightShadows:y.directionalShadowMap.length,numPointLightShadows:y.pointShadowMap.length,numSpotLightShadows:y.spotShadowMap.length,numSpotLightShadowsWithMaps:y.numSpotLightShadowsWithMaps,numLightProbes:y.numLightProbes,numClippingPlanes:o.numPlanes,numClipIntersection:o.numIntersection,dithering:M.dithering,shadowMapEnabled:n.shadowMap.enabled&&I.length>0,shadowMapType:n.shadowMap.type,toneMapping:Ne,decodeVideoTexture:$e&&M.map.isVideoTexture===!0&&Ye.getTransfer(M.map.colorSpace)===et,decodeVideoTextureEmissive:xe&&M.emissiveMap.isVideoTexture===!0&&Ye.getTransfer(M.emissiveMap.colorSpace)===et,premultipliedAlpha:M.premultipliedAlpha,doubleSided:M.side===qt,flipSided:M.side===Bt,useDepthPacking:M.depthPacking>=0,depthPacking:M.depthPacking||0,index0AttributeName:M.index0AttributeName,extensionClipCullDistance:ge&&M.extensions.clipCullDistance===!0&&i.has("WEBGL_clip_cull_distance"),extensionMultiDraw:(ge&&M.extensions.multiDraw===!0||be)&&i.has("WEBGL_multi_draw"),rendererExtensionParallelShaderCompile:i.has("KHR_parallel_shader_compile"),customProgramCacheKey:M.customProgramCacheKey()};return rt.vertexUv1s=l.has(1),rt.vertexUv2s=l.has(2),rt.vertexUv3s=l.has(3),l.clear(),rt}function f(M){let y=[];if(M.shaderID?y.push(M.shaderID):(y.push(M.customVertexShaderID),y.push(M.customFragmentShaderID)),M.defines!==void 0)for(let I in M.defines)y.push(I),y.push(M.defines[I]);return M.isRawShaderMaterial===!1&&(T(y,M),E(y,M),y.push(n.outputColorSpace)),y.push(M.customProgramCacheKey),y.join()}function T(M,y){M.push(y.precision),M.push(y.outputColorSpace),M.push(y.envMapMode),M.push(y.envMapCubeUVHeight),M.push(y.mapUv),M.push(y.alphaMapUv),M.push(y.lightMapUv),M.push(y.aoMapUv),M.push(y.bumpMapUv),M.push(y.normalMapUv),M.push(y.displacementMapUv),M.push(y.emissiveMapUv),M.push(y.metalnessMapUv),M.push(y.roughnessMapUv),M.push(y.anisotropyMapUv),M.push(y.clearcoatMapUv),M.push(y.clearcoatNormalMapUv),M.push(y.clearcoatRoughnessMapUv),M.push(y.iridescenceMapUv),M.push(y.iridescenceThicknessMapUv),M.push(y.sheenColorMapUv),M.push(y.sheenRoughnessMapUv),M.push(y.specularMapUv),M.push(y.specularColorMapUv),M.push(y.specularIntensityMapUv),M.push(y.transmissionMapUv),M.push(y.thicknessMapUv),M.push(y.combine),M.push(y.fogExp2),M.push(y.sizeAttenuation),M.push(y.morphTargetsCount),M.push(y.morphAttributeCount),M.push(y.numDirLights),M.push(y.numPointLights),M.push(y.numSpotLights),M.push(y.numSpotLightMaps),M.push(y.numHemiLights),M.push(y.numRectAreaLights),M.push(y.numDirLightShadows),M.push(y.numPointLightShadows),M.push(y.numSpotLightShadows),M.push(y.numSpotLightShadowsWithMaps),M.push(y.numLightProbes),M.push(y.shadowMapType),M.push(y.toneMapping),M.push(y.numClippingPlanes),M.push(y.numClipIntersection),M.push(y.depthPacking)}function E(M,y){a.disableAll(),y.supportsVertexTextures&&a.enable(0),y.instancing&&a.enable(1),y.instancingColor&&a.enable(2),y.instancingMorph&&a.enable(3),y.matcap&&a.enable(4),y.envMap&&a.enable(5),y.normalMapObjectSpace&&a.enable(6),y.normalMapTangentSpace&&a.enable(7),y.clearcoat&&a.enable(8),y.iridescence&&a.enable(9),y.alphaTest&&a.enable(10),y.vertexColors&&a.enable(11),y.vertexAlphas&&a.enable(12),y.vertexUv1s&&a.enable(13),y.vertexUv2s&&a.enable(14),y.vertexUv3s&&a.enable(15),y.vertexTangents&&a.enable(16),y.anisotropy&&a.enable(17),y.alphaHash&&a.enable(18),y.batching&&a.enable(19),y.dispersion&&a.enable(20),y.batchingColor&&a.enable(21),y.gradientMap&&a.enable(22),M.push(a.mask),a.disableAll(),y.fog&&a.enable(0),y.useFog&&a.enable(1),y.flatShading&&a.enable(2),y.logarithmicDepthBuffer&&a.enable(3),y.reversedDepthBuffer&&a.enable(4),y.skinning&&a.enable(5),y.morphTargets&&a.enable(6),y.morphNormals&&a.enable(7),y.morphColors&&a.enable(8),y.premultipliedAlpha&&a.enable(9),y.shadowMapEnabled&&a.enable(10),y.doubleSided&&a.enable(11),y.flipSided&&a.enable(12),y.useDepthPacking&&a.enable(13),y.dithering&&a.enable(14),y.transmission&&a.enable(15),y.sheen&&a.enable(16),y.opaque&&a.enable(17),y.pointsUvs&&a.enable(18),y.decodeVideoTexture&&a.enable(19),y.decodeVideoTextureEmissive&&a.enable(20),y.alphaToCoverage&&a.enable(21),M.push(a.mask)}function S(M){let y=g[M.type],I;if(y){let O=wn[y];I=bu.clone(O.uniforms)}else I=M.uniforms;return I}function R(M,y){let I;for(let O=0,z=h.length;O<z;O++){let H=h[O];if(H.cacheKey===y){I=H,++I.usedTimes;break}}return I===void 0&&(I=new A0(n,y,M,r),h.push(I)),I}function A(M){if(--M.usedTimes===0){let y=h.indexOf(M);h[y]=h[h.length-1],h.pop(),M.destroy()}}function P(M){c.remove(M)}function N(){c.dispose()}return{getParameters:m,getProgramCacheKey:f,getUniforms:S,acquireProgram:R,releaseProgram:A,releaseShaderCache:P,programs:h,dispose:N}}function I0(){let n=new WeakMap;function e(o){return n.has(o)}function t(o){let a=n.get(o);return a===void 0&&(a={},n.set(o,a)),a}function i(o){n.delete(o)}function s(o,a,c){n.get(o)[a]=c}function r(){n=new WeakMap}return{has:e,get:t,remove:i,update:s,dispose:r}}function P0(n,e){return n.groupOrder!==e.groupOrder?n.groupOrder-e.groupOrder:n.renderOrder!==e.renderOrder?n.renderOrder-e.renderOrder:n.material.id!==e.material.id?n.material.id-e.material.id:n.z!==e.z?n.z-e.z:n.id-e.id}function qu(n,e){return n.groupOrder!==e.groupOrder?n.groupOrder-e.groupOrder:n.renderOrder!==e.renderOrder?n.renderOrder-e.renderOrder:n.z!==e.z?e.z-n.z:n.id-e.id}function $u(){let n=[],e=0,t=[],i=[],s=[];function r(){e=0,t.length=0,i.length=0,s.length=0}function o(u,p,d,g,v,m){let f=n[e];return f===void 0?(f={id:u.id,object:u,geometry:p,material:d,groupOrder:g,renderOrder:u.renderOrder,z:v,group:m},n[e]=f):(f.id=u.id,f.object=u,f.geometry=p,f.material=d,f.groupOrder=g,f.renderOrder=u.renderOrder,f.z=v,f.group=m),e++,f}function a(u,p,d,g,v,m){let f=o(u,p,d,g,v,m);d.transmission>0?i.push(f):d.transparent===!0?s.push(f):t.push(f)}function c(u,p,d,g,v,m){let f=o(u,p,d,g,v,m);d.transmission>0?i.unshift(f):d.transparent===!0?s.unshift(f):t.unshift(f)}function l(u,p){t.length>1&&t.sort(u||P0),i.length>1&&i.sort(p||qu),s.length>1&&s.sort(p||qu)}function h(){for(let u=e,p=n.length;u<p;u++){let d=n[u];if(d.id===null)break;d.id=null,d.object=null,d.geometry=null,d.material=null,d.group=null}}return{opaque:t,transmissive:i,transparent:s,init:r,push:a,unshift:c,finish:h,sort:l}}function L0(){let n=new WeakMap;function e(i,s){let r=n.get(i),o;return r===void 0?(o=new $u,n.set(i,[o])):s>=r.length?(o=new $u,r.push(o)):o=r[s],o}function t(){n=new WeakMap}return{get:e,dispose:t}}function D0(){let n={};return{get:function(e){if(n[e.id]!==void 0)return n[e.id];let t;switch(e.type){case"DirectionalLight":t={direction:new L,color:new Xe};break;case"SpotLight":t={position:new L,direction:new L,color:new Xe,distance:0,coneCos:0,penumbraCos:0,decay:0};break;case"PointLight":t={position:new L,color:new Xe,distance:0,decay:0};break;case"HemisphereLight":t={direction:new L,skyColor:new Xe,groundColor:new Xe};break;case"RectAreaLight":t={color:new Xe,position:new L,halfWidth:new L,halfHeight:new L};break}return n[e.id]=t,t}}}function U0(){let n={};return{get:function(e){if(n[e.id]!==void 0)return n[e.id];let t;switch(e.type){case"DirectionalLight":t={shadowIntensity:1,shadowBias:0,shadowNormalBias:0,shadowRadius:1,shadowMapSize:new Ue};break;case"SpotLight":t={shadowIntensity:1,shadowBias:0,shadowNormalBias:0,shadowRadius:1,shadowMapSize:new Ue};break;case"PointLight":t={shadowIntensity:1,shadowBias:0,shadowNormalBias:0,shadowRadius:1,shadowMapSize:new Ue,shadowCameraNear:1,shadowCameraFar:1e3};break}return n[e.id]=t,t}}}var N0=0;function F0(n,e){return(e.castShadow?2:0)-(n.castShadow?2:0)+(e.map?1:0)-(n.map?1:0)}function O0(n){let e=new D0,t=U0(),i={version:0,hash:{directionalLength:-1,pointLength:-1,spotLength:-1,rectAreaLength:-1,hemiLength:-1,numDirectionalShadows:-1,numPointShadows:-1,numSpotShadows:-1,numSpotMaps:-1,numLightProbes:-1},ambient:[0,0,0],probe:[],directional:[],directionalShadow:[],directionalShadowMap:[],directionalShadowMatrix:[],spot:[],spotLightMap:[],spotShadow:[],spotShadowMap:[],spotLightMatrix:[],rectArea:[],rectAreaLTC1:null,rectAreaLTC2:null,point:[],pointShadow:[],pointShadowMap:[],pointShadowMatrix:[],hemi:[],numSpotLightShadowsWithMaps:0,numLightProbes:0};for(let l=0;l<9;l++)i.probe.push(new L);let s=new L,r=new dt,o=new dt;function a(l){let h=0,u=0,p=0;for(let M=0;M<9;M++)i.probe[M].set(0,0,0);let d=0,g=0,v=0,m=0,f=0,T=0,E=0,S=0,R=0,A=0,P=0;l.sort(F0);for(let M=0,y=l.length;M<y;M++){let I=l[M],O=I.color,z=I.intensity,H=I.distance,q=I.shadow&&I.shadow.map?I.shadow.map.texture:null;if(I.isAmbientLight)h+=O.r*z,u+=O.g*z,p+=O.b*z;else if(I.isLightProbe){for(let W=0;W<9;W++)i.probe[W].addScaledVector(I.sh.coefficients[W],z);P++}else if(I.isDirectionalLight){let W=e.get(I);if(W.color.copy(I.color).multiplyScalar(I.intensity),I.castShadow){let ne=I.shadow,V=t.get(I);V.shadowIntensity=ne.intensity,V.shadowBias=ne.bias,V.shadowNormalBias=ne.normalBias,V.shadowRadius=ne.radius,V.shadowMapSize=ne.mapSize,i.directionalShadow[d]=V,i.directionalShadowMap[d]=q,i.directionalShadowMatrix[d]=I.shadow.matrix,T++}i.directional[d]=W,d++}else if(I.isSpotLight){let W=e.get(I);W.position.setFromMatrixPosition(I.matrixWorld),W.color.copy(O).multiplyScalar(z),W.distance=H,W.coneCos=Math.cos(I.angle),W.penumbraCos=Math.cos(I.angle*(1-I.penumbra)),W.decay=I.decay,i.spot[v]=W;let ne=I.shadow;if(I.map&&(i.spotLightMap[R]=I.map,R++,ne.updateMatrices(I),I.castShadow&&A++),i.spotLightMatrix[v]=ne.matrix,I.castShadow){let V=t.get(I);V.shadowIntensity=ne.intensity,V.shadowBias=ne.bias,V.shadowNormalBias=ne.normalBias,V.shadowRadius=ne.radius,V.shadowMapSize=ne.mapSize,i.spotShadow[v]=V,i.spotShadowMap[v]=q,S++}v++}else if(I.isRectAreaLight){let W=e.get(I);W.color.copy(O).multiplyScalar(z),W.halfWidth.set(I.width*.5,0,0),W.halfHeight.set(0,I.height*.5,0),i.rectArea[m]=W,m++}else if(I.isPointLight){let W=e.get(I);if(W.color.copy(I.color).multiplyScalar(I.intensity),W.distance=I.distance,W.decay=I.decay,I.castShadow){let ne=I.shadow,V=t.get(I);V.shadowIntensity=ne.intensity,V.shadowBias=ne.bias,V.shadowNormalBias=ne.normalBias,V.shadowRadius=ne.radius,V.shadowMapSize=ne.mapSize,V.shadowCameraNear=ne.camera.near,V.shadowCameraFar=ne.camera.far,i.pointShadow[g]=V,i.pointShadowMap[g]=q,i.pointShadowMatrix[g]=I.shadow.matrix,E++}i.point[g]=W,g++}else if(I.isHemisphereLight){let W=e.get(I);W.skyColor.copy(I.color).multiplyScalar(z),W.groundColor.copy(I.groundColor).multiplyScalar(z),i.hemi[f]=W,f++}}m>0&&(n.has("OES_texture_float_linear")===!0?(i.rectAreaLTC1=re.LTC_FLOAT_1,i.rectAreaLTC2=re.LTC_FLOAT_2):(i.rectAreaLTC1=re.LTC_HALF_1,i.rectAreaLTC2=re.LTC_HALF_2)),i.ambient[0]=h,i.ambient[1]=u,i.ambient[2]=p;let N=i.hash;(N.directionalLength!==d||N.pointLength!==g||N.spotLength!==v||N.rectAreaLength!==m||N.hemiLength!==f||N.numDirectionalShadows!==T||N.numPointShadows!==E||N.numSpotShadows!==S||N.numSpotMaps!==R||N.numLightProbes!==P)&&(i.directional.length=d,i.spot.length=v,i.rectArea.length=m,i.point.length=g,i.hemi.length=f,i.directionalShadow.length=T,i.directionalShadowMap.length=T,i.pointShadow.length=E,i.pointShadowMap.length=E,i.spotShadow.length=S,i.spotShadowMap.length=S,i.directionalShadowMatrix.length=T,i.pointShadowMatrix.length=E,i.spotLightMatrix.length=S+R-A,i.spotLightMap.length=R,i.numSpotLightShadowsWithMaps=A,i.numLightProbes=P,N.directionalLength=d,N.pointLength=g,N.spotLength=v,N.rectAreaLength=m,N.hemiLength=f,N.numDirectionalShadows=T,N.numPointShadows=E,N.numSpotShadows=S,N.numSpotMaps=R,N.numLightProbes=P,i.version=N0++)}function c(l,h){let u=0,p=0,d=0,g=0,v=0,m=h.matrixWorldInverse;for(let f=0,T=l.length;f<T;f++){let E=l[f];if(E.isDirectionalLight){let S=i.directional[u];S.direction.setFromMatrixPosition(E.matrixWorld),s.setFromMatrixPosition(E.target.matrixWorld),S.direction.sub(s),S.direction.transformDirection(m),u++}else if(E.isSpotLight){let S=i.spot[d];S.position.setFromMatrixPosition(E.matrixWorld),S.position.applyMatrix4(m),S.direction.setFromMatrixPosition(E.matrixWorld),s.setFromMatrixPosition(E.target.matrixWorld),S.direction.sub(s),S.direction.transformDirection(m),d++}else if(E.isRectAreaLight){let S=i.rectArea[g];S.position.setFromMatrixPosition(E.matrixWorld),S.position.applyMatrix4(m),o.identity(),r.copy(E.matrixWorld),r.premultiply(m),o.extractRotation(r),S.halfWidth.set(E.width*.5,0,0),S.halfHeight.set(0,E.height*.5,0),S.halfWidth.applyMatrix4(o),S.halfHeight.applyMatrix4(o),g++}else if(E.isPointLight){let S=i.point[p];S.position.setFromMatrixPosition(E.matrixWorld),S.position.applyMatrix4(m),p++}else if(E.isHemisphereLight){let S=i.hemi[v];S.direction.setFromMatrixPosition(E.matrixWorld),S.direction.transformDirection(m),v++}}}return{setup:a,setupView:c,state:i}}function Yu(n){let e=new O0(n),t=[],i=[];function s(h){l.camera=h,t.length=0,i.length=0}function r(h){t.push(h)}function o(h){i.push(h)}function a(){e.setup(t)}function c(h){e.setupView(t,h)}let l={lightsArray:t,shadowsArray:i,camera:null,lights:e,transmissionRenderTarget:{}};return{init:s,state:l,setupLights:a,setupLightsView:c,pushLight:r,pushShadow:o}}function k0(n){let e=new WeakMap;function t(s,r=0){let o=e.get(s),a;return o===void 0?(a=new Yu(n),e.set(s,[a])):r>=o.length?(a=new Yu(n),o.push(a)):a=o[r],a}function i(){e=new WeakMap}return{get:t,dispose:i}}var B0=`void main() {
	gl_Position = vec4( position, 1.0 );
}`,z0=`uniform sampler2D shadow_pass;
uniform vec2 resolution;
uniform float radius;
#include <packing>
void main() {
	const float samples = float( VSM_SAMPLES );
	float mean = 0.0;
	float squared_mean = 0.0;
	float uvStride = samples <= 1.0 ? 0.0 : 2.0 / ( samples - 1.0 );
	float uvStart = samples <= 1.0 ? 0.0 : - 1.0;
	for ( float i = 0.0; i < samples; i ++ ) {
		float uvOffset = uvStart + i * uvStride;
		#ifdef HORIZONTAL_PASS
			vec2 distribution = unpackRGBATo2Half( texture2D( shadow_pass, ( gl_FragCoord.xy + vec2( uvOffset, 0.0 ) * radius ) / resolution ) );
			mean += distribution.x;
			squared_mean += distribution.y * distribution.y + distribution.x * distribution.x;
		#else
			float depth = unpackRGBAToDepth( texture2D( shadow_pass, ( gl_FragCoord.xy + vec2( 0.0, uvOffset ) * radius ) / resolution ) );
			mean += depth;
			squared_mean += depth * depth;
		#endif
	}
	mean = mean / samples;
	squared_mean = squared_mean / samples;
	float std_dev = sqrt( squared_mean - mean * mean );
	gl_FragColor = pack2HalfToRGBA( vec2( mean, std_dev ) );
}`;function V0(n,e,t){let i=new ps,s=new Ue,r=new Ue,o=new Qe,a=new ha({depthPacking:uu}),c=new ua,l={},h=t.maxTextureSize,u={[Bn]:Bt,[Bt]:Bn,[qt]:qt},p=new pn({defines:{VSM_SAMPLES:8},uniforms:{shadow_pass:{value:null},resolution:{value:new Ue},radius:{value:4}},vertexShader:B0,fragmentShader:z0}),d=p.clone();d.defines.HORIZONTAL_PASS=1;let g=new Xt;g.setAttribute("position",new Ht(new Float32Array([-1,-1,.5,3,-1,.5,-1,3,.5]),3));let v=new ft(g,p),m=this;this.enabled=!1,this.autoUpdate=!0,this.needsUpdate=!1,this.type=wl;let f=this.type;this.render=function(A,P,N){if(m.enabled===!1||m.autoUpdate===!1&&m.needsUpdate===!1||A.length===0)return;let M=n.getRenderTarget(),y=n.getActiveCubeFace(),I=n.getActiveMipmapLevel(),O=n.state;O.setBlending(Hn),O.buffers.depth.getReversed()===!0?O.buffers.color.setClear(0,0,0,0):O.buffers.color.setClear(1,1,1,1),O.buffers.depth.setTest(!0),O.setScissorTest(!1);let z=f!==En&&this.type===En,H=f===En&&this.type!==En;for(let q=0,W=A.length;q<W;q++){let ne=A[q],V=ne.shadow;if(V===void 0){console.warn("THREE.WebGLShadowMap:",ne,"has no shadow.");continue}if(V.autoUpdate===!1&&V.needsUpdate===!1)continue;s.copy(V.mapSize);let ae=V.getFrameExtents();if(s.multiply(ae),r.copy(V.mapSize),(s.x>h||s.y>h)&&(s.x>h&&(r.x=Math.floor(h/ae.x),s.x=r.x*ae.x,V.mapSize.x=r.x),s.y>h&&(r.y=Math.floor(h/ae.y),s.y=r.y*ae.y,V.mapSize.y=r.y)),V.map===null||z===!0||H===!0){let Ee=this.type!==En?{minFilter:rn,magFilter:rn}:{};V.map!==null&&V.map.dispose(),V.map=new Sn(s.x,s.y,Ee),V.map.texture.name=ne.name+".shadowMap",V.camera.updateProjectionMatrix()}n.setRenderTarget(V.map),n.clear();let ue=V.getViewportCount();for(let Ee=0;Ee<ue;Ee++){let Ve=V.getViewport(Ee);o.set(r.x*Ve.x,r.y*Ve.y,r.x*Ve.z,r.y*Ve.w),O.viewport(o),V.updateMatrices(ne,Ee),i=V.getFrustum(),S(P,N,V.camera,ne,this.type)}V.isPointLightShadow!==!0&&this.type===En&&T(V,N),V.needsUpdate=!1}f=this.type,m.needsUpdate=!1,n.setRenderTarget(M,y,I)};function T(A,P){let N=e.update(v);p.defines.VSM_SAMPLES!==A.blurSamples&&(p.defines.VSM_SAMPLES=A.blurSamples,d.defines.VSM_SAMPLES=A.blurSamples,p.needsUpdate=!0,d.needsUpdate=!0),A.mapPass===null&&(A.mapPass=new Sn(s.x,s.y)),p.uniforms.shadow_pass.value=A.map.texture,p.uniforms.resolution.value=A.mapSize,p.uniforms.radius.value=A.radius,n.setRenderTarget(A.mapPass),n.clear(),n.renderBufferDirect(P,null,N,p,v,null),d.uniforms.shadow_pass.value=A.mapPass.texture,d.uniforms.resolution.value=A.mapSize,d.uniforms.radius.value=A.radius,n.setRenderTarget(A.map),n.clear(),n.renderBufferDirect(P,null,N,d,v,null)}function E(A,P,N,M){let y=null,I=N.isPointLight===!0?A.customDistanceMaterial:A.customDepthMaterial;if(I!==void 0)y=I;else if(y=N.isPointLight===!0?c:a,n.localClippingEnabled&&P.clipShadows===!0&&Array.isArray(P.clippingPlanes)&&P.clippingPlanes.length!==0||P.displacementMap&&P.displacementScale!==0||P.alphaMap&&P.alphaTest>0||P.map&&P.alphaTest>0||P.alphaToCoverage===!0){let O=y.uuid,z=P.uuid,H=l[O];H===void 0&&(H={},l[O]=H);let q=H[z];q===void 0&&(q=y.clone(),H[z]=q,P.addEventListener("dispose",R)),y=q}if(y.visible=P.visible,y.wireframe=P.wireframe,M===En?y.side=P.shadowSide!==null?P.shadowSide:P.side:y.side=P.shadowSide!==null?P.shadowSide:u[P.side],y.alphaMap=P.alphaMap,y.alphaTest=P.alphaToCoverage===!0?.5:P.alphaTest,y.map=P.map,y.clipShadows=P.clipShadows,y.clippingPlanes=P.clippingPlanes,y.clipIntersection=P.clipIntersection,y.displacementMap=P.displacementMap,y.displacementScale=P.displacementScale,y.displacementBias=P.displacementBias,y.wireframeLinewidth=P.wireframeLinewidth,y.linewidth=P.linewidth,N.isPointLight===!0&&y.isMeshDistanceMaterial===!0){let O=n.properties.get(y);O.light=N}return y}function S(A,P,N,M,y){if(A.visible===!1)return;if(A.layers.test(P.layers)&&(A.isMesh||A.isLine||A.isPoints)&&(A.castShadow||A.receiveShadow&&y===En)&&(!A.frustumCulled||i.intersectsObject(A))){A.modelViewMatrix.multiplyMatrices(N.matrixWorldInverse,A.matrixWorld);let z=e.update(A),H=A.material;if(Array.isArray(H)){let q=z.groups;for(let W=0,ne=q.length;W<ne;W++){let V=q[W],ae=H[V.materialIndex];if(ae&&ae.visible){let ue=E(A,ae,M,y);A.onBeforeShadow(n,A,P,N,z,ue,V),n.renderBufferDirect(N,null,z,ue,A,V),A.onAfterShadow(n,A,P,N,z,ue,V)}}}else if(H.visible){let q=E(A,H,M,y);A.onBeforeShadow(n,A,P,N,z,q,null),n.renderBufferDirect(N,null,z,q,A,null),A.onAfterShadow(n,A,P,N,z,q,null)}}let O=A.children;for(let z=0,H=O.length;z<H;z++)S(O[z],P,N,M,y)}function R(A){A.target.removeEventListener("dispose",R);for(let N in l){let M=l[N],y=A.target.uuid;y in M&&(M[y].dispose(),delete M[y])}}}var G0={[Ea]:Ta,[wa]:Ca,[Aa]:Ia,[Ti]:Ra,[Ta]:Ea,[Ca]:wa,[Ia]:Aa,[Ra]:Ti};function H0(n,e){function t(){let C=!1,te=new Qe,se=null,fe=new Qe(0,0,0,0);return{setMask:function(j){se!==j&&!C&&(n.colorMask(j,j,j,j),se=j)},setLocked:function(j){C=j},setClear:function(j,Y,ge,Ne,rt){rt===!0&&(j*=Ne,Y*=Ne,ge*=Ne),te.set(j,Y,ge,Ne),fe.equals(te)===!1&&(n.clearColor(j,Y,ge,Ne),fe.copy(te))},reset:function(){C=!1,se=null,fe.set(-1,0,0,0)}}}function i(){let C=!1,te=!1,se=null,fe=null,j=null;return{setReversed:function(Y){if(te!==Y){let ge=e.get("EXT_clip_control");Y?ge.clipControlEXT(ge.LOWER_LEFT_EXT,ge.ZERO_TO_ONE_EXT):ge.clipControlEXT(ge.LOWER_LEFT_EXT,ge.NEGATIVE_ONE_TO_ONE_EXT),te=Y;let Ne=j;j=null,this.setClear(Ne)}},getReversed:function(){return te},setTest:function(Y){Y?K(n.DEPTH_TEST):pe(n.DEPTH_TEST)},setMask:function(Y){se!==Y&&!C&&(n.depthMask(Y),se=Y)},setFunc:function(Y){if(te&&(Y=G0[Y]),fe!==Y){switch(Y){case Ea:n.depthFunc(n.NEVER);break;case Ta:n.depthFunc(n.ALWAYS);break;case wa:n.depthFunc(n.LESS);break;case Ti:n.depthFunc(n.LEQUAL);break;case Aa:n.depthFunc(n.EQUAL);break;case Ra:n.depthFunc(n.GEQUAL);break;case Ca:n.depthFunc(n.GREATER);break;case Ia:n.depthFunc(n.NOTEQUAL);break;default:n.depthFunc(n.LEQUAL)}fe=Y}},setLocked:function(Y){C=Y},setClear:function(Y){j!==Y&&(te&&(Y=1-Y),n.clearDepth(Y),j=Y)},reset:function(){C=!1,se=null,fe=null,j=null,te=!1}}}function s(){let C=!1,te=null,se=null,fe=null,j=null,Y=null,ge=null,Ne=null,rt=null;return{setTest:function(Ke){C||(Ke?K(n.STENCIL_TEST):pe(n.STENCIL_TEST))},setMask:function(Ke){te!==Ke&&!C&&(n.stencilMask(Ke),te=Ke)},setFunc:function(Ke,In,_n){(se!==Ke||fe!==In||j!==_n)&&(n.stencilFunc(Ke,In,_n),se=Ke,fe=In,j=_n)},setOp:function(Ke,In,_n){(Y!==Ke||ge!==In||Ne!==_n)&&(n.stencilOp(Ke,In,_n),Y=Ke,ge=In,Ne=_n)},setLocked:function(Ke){C=Ke},setClear:function(Ke){rt!==Ke&&(n.clearStencil(Ke),rt=Ke)},reset:function(){C=!1,te=null,se=null,fe=null,j=null,Y=null,ge=null,Ne=null,rt=null}}}let r=new t,o=new i,a=new s,c=new WeakMap,l=new WeakMap,h={},u={},p=new WeakMap,d=[],g=null,v=!1,m=null,f=null,T=null,E=null,S=null,R=null,A=null,P=new Xe(0,0,0),N=0,M=!1,y=null,I=null,O=null,z=null,H=null,q=n.getParameter(n.MAX_COMBINED_TEXTURE_IMAGE_UNITS),W=!1,ne=0,V=n.getParameter(n.VERSION);V.indexOf("WebGL")!==-1?(ne=parseFloat(/^WebGL (\d)/.exec(V)[1]),W=ne>=1):V.indexOf("OpenGL ES")!==-1&&(ne=parseFloat(/^OpenGL ES (\d)/.exec(V)[1]),W=ne>=2);let ae=null,ue={},Ee=n.getParameter(n.SCISSOR_BOX),Ve=n.getParameter(n.VIEWPORT),st=new Qe().fromArray(Ee),ct=new Qe().fromArray(Ve);function Je(C,te,se,fe){let j=new Uint8Array(4),Y=n.createTexture();n.bindTexture(C,Y),n.texParameteri(C,n.TEXTURE_MIN_FILTER,n.NEAREST),n.texParameteri(C,n.TEXTURE_MAG_FILTER,n.NEAREST);for(let ge=0;ge<se;ge++)C===n.TEXTURE_3D||C===n.TEXTURE_2D_ARRAY?n.texImage3D(te,0,n.RGBA,1,1,fe,0,n.RGBA,n.UNSIGNED_BYTE,j):n.texImage2D(te+ge,0,n.RGBA,1,1,0,n.RGBA,n.UNSIGNED_BYTE,j);return Y}let $={};$[n.TEXTURE_2D]=Je(n.TEXTURE_2D,n.TEXTURE_2D,1),$[n.TEXTURE_CUBE_MAP]=Je(n.TEXTURE_CUBE_MAP,n.TEXTURE_CUBE_MAP_POSITIVE_X,6),$[n.TEXTURE_2D_ARRAY]=Je(n.TEXTURE_2D_ARRAY,n.TEXTURE_2D_ARRAY,1,1),$[n.TEXTURE_3D]=Je(n.TEXTURE_3D,n.TEXTURE_3D,1,1),r.setClear(0,0,0,1),o.setClear(1),a.setClear(0),K(n.DEPTH_TEST),o.setFunc(Ti),Pe(!1),_e(Tl),K(n.CULL_FACE),ht(Hn);function K(C){h[C]!==!0&&(n.enable(C),h[C]=!0)}function pe(C){h[C]!==!1&&(n.disable(C),h[C]=!1)}function De(C,te){return u[C]!==te?(n.bindFramebuffer(C,te),u[C]=te,C===n.DRAW_FRAMEBUFFER&&(u[n.FRAMEBUFFER]=te),C===n.FRAMEBUFFER&&(u[n.DRAW_FRAMEBUFFER]=te),!0):!1}function be(C,te){let se=d,fe=!1;if(C){se=p.get(te),se===void 0&&(se=[],p.set(te,se));let j=C.textures;if(se.length!==j.length||se[0]!==n.COLOR_ATTACHMENT0){for(let Y=0,ge=j.length;Y<ge;Y++)se[Y]=n.COLOR_ATTACHMENT0+Y;se.length=j.length,fe=!0}}else se[0]!==n.BACK&&(se[0]=n.BACK,fe=!0);fe&&n.drawBuffers(se)}function $e(C){return g!==C?(n.useProgram(C),g=C,!0):!1}let Rt={[ri]:n.FUNC_ADD,[kh]:n.FUNC_SUBTRACT,[Bh]:n.FUNC_REVERSE_SUBTRACT};Rt[zh]=n.MIN,Rt[Vh]=n.MAX;let w={[Gh]:n.ZERO,[Hh]:n.ONE,[Wh]:n.SRC_COLOR,[Kr]:n.SRC_ALPHA,[Jh]:n.SRC_ALPHA_SATURATE,[Yh]:n.DST_COLOR,[qh]:n.DST_ALPHA,[Xh]:n.ONE_MINUS_SRC_COLOR,[jr]:n.ONE_MINUS_SRC_ALPHA,[Zh]:n.ONE_MINUS_DST_COLOR,[$h]:n.ONE_MINUS_DST_ALPHA,[Kh]:n.CONSTANT_COLOR,[jh]:n.ONE_MINUS_CONSTANT_COLOR,[Qh]:n.CONSTANT_ALPHA,[eu]:n.ONE_MINUS_CONSTANT_ALPHA};function ht(C,te,se,fe,j,Y,ge,Ne,rt,Ke){if(C===Hn){v===!0&&(pe(n.BLEND),v=!1);return}if(v===!1&&(K(n.BLEND),v=!0),C!==Oh){if(C!==m||Ke!==M){if((f!==ri||S!==ri)&&(n.blendEquation(n.FUNC_ADD),f=ri,S=ri),Ke)switch(C){case Ei:n.blendFuncSeparate(n.ONE,n.ONE_MINUS_SRC_ALPHA,n.ONE,n.ONE_MINUS_SRC_ALPHA);break;case Al:n.blendFunc(n.ONE,n.ONE);break;case Rl:n.blendFuncSeparate(n.ZERO,n.ONE_MINUS_SRC_COLOR,n.ZERO,n.ONE);break;case Cl:n.blendFuncSeparate(n.DST_COLOR,n.ONE_MINUS_SRC_ALPHA,n.ZERO,n.ONE);break;default:console.error("THREE.WebGLState: Invalid blending: ",C);break}else switch(C){case Ei:n.blendFuncSeparate(n.SRC_ALPHA,n.ONE_MINUS_SRC_ALPHA,n.ONE,n.ONE_MINUS_SRC_ALPHA);break;case Al:n.blendFuncSeparate(n.SRC_ALPHA,n.ONE,n.ONE,n.ONE);break;case Rl:console.error("THREE.WebGLState: SubtractiveBlending requires material.premultipliedAlpha = true");break;case Cl:console.error("THREE.WebGLState: MultiplyBlending requires material.premultipliedAlpha = true");break;default:console.error("THREE.WebGLState: Invalid blending: ",C);break}T=null,E=null,R=null,A=null,P.set(0,0,0),N=0,m=C,M=Ke}return}j=j||te,Y=Y||se,ge=ge||fe,(te!==f||j!==S)&&(n.blendEquationSeparate(Rt[te],Rt[j]),f=te,S=j),(se!==T||fe!==E||Y!==R||ge!==A)&&(n.blendFuncSeparate(w[se],w[fe],w[Y],w[ge]),T=se,E=fe,R=Y,A=ge),(Ne.equals(P)===!1||rt!==N)&&(n.blendColor(Ne.r,Ne.g,Ne.b,rt),P.copy(Ne),N=rt),m=C,M=!1}function Fe(C,te){C.side===qt?pe(n.CULL_FACE):K(n.CULL_FACE);let se=C.side===Bt;te&&(se=!se),Pe(se),C.blending===Ei&&C.transparent===!1?ht(Hn):ht(C.blending,C.blendEquation,C.blendSrc,C.blendDst,C.blendEquationAlpha,C.blendSrcAlpha,C.blendDstAlpha,C.blendColor,C.blendAlpha,C.premultipliedAlpha),o.setFunc(C.depthFunc),o.setTest(C.depthTest),o.setMask(C.depthWrite),r.setMask(C.colorWrite);let fe=C.stencilWrite;a.setTest(fe),fe&&(a.setMask(C.stencilWriteMask),a.setFunc(C.stencilFunc,C.stencilRef,C.stencilFuncMask),a.setOp(C.stencilFail,C.stencilZFail,C.stencilZPass)),xe(C.polygonOffset,C.polygonOffsetFactor,C.polygonOffsetUnits),C.alphaToCoverage===!0?K(n.SAMPLE_ALPHA_TO_COVERAGE):pe(n.SAMPLE_ALPHA_TO_COVERAGE)}function Pe(C){y!==C&&(C?n.frontFace(n.CW):n.frontFace(n.CCW),y=C)}function _e(C){C!==Nh?(K(n.CULL_FACE),C!==I&&(C===Tl?n.cullFace(n.BACK):C===Fh?n.cullFace(n.FRONT):n.cullFace(n.FRONT_AND_BACK))):pe(n.CULL_FACE),I=C}function ut(C){C!==O&&(W&&n.lineWidth(C),O=C)}function xe(C,te,se){C?(K(n.POLYGON_OFFSET_FILL),(z!==te||H!==se)&&(n.polygonOffset(te,se),z=te,H=se)):pe(n.POLYGON_OFFSET_FILL)}function Be(C){C?K(n.SCISSOR_TEST):pe(n.SCISSOR_TEST)}function bt(C){C===void 0&&(C=n.TEXTURE0+q-1),ae!==C&&(n.activeTexture(C),ae=C)}function xt(C,te,se){se===void 0&&(ae===null?se=n.TEXTURE0+q-1:se=ae);let fe=ue[se];fe===void 0&&(fe={type:void 0,texture:void 0},ue[se]=fe),(fe.type!==C||fe.texture!==te)&&(ae!==se&&(n.activeTexture(se),ae=se),n.bindTexture(C,te||$[C]),fe.type=C,fe.texture=te)}function b(){let C=ue[ae];C!==void 0&&C.type!==void 0&&(n.bindTexture(C.type,null),C.type=void 0,C.texture=void 0)}function _(){try{n.compressedTexImage2D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function F(){try{n.compressedTexImage3D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function X(){try{n.texSubImage2D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function J(){try{n.texSubImage3D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function G(){try{n.compressedTexSubImage2D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function Se(){try{n.compressedTexSubImage3D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function ie(){try{n.texStorage2D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function ve(){try{n.texStorage3D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function ye(){try{n.texImage2D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function ee(){try{n.texImage3D(...arguments)}catch(C){console.error("THREE.WebGLState:",C)}}function he(C){st.equals(C)===!1&&(n.scissor(C.x,C.y,C.z,C.w),st.copy(C))}function Ie(C){ct.equals(C)===!1&&(n.viewport(C.x,C.y,C.z,C.w),ct.copy(C))}function Me(C,te){let se=l.get(te);se===void 0&&(se=new WeakMap,l.set(te,se));let fe=se.get(C);fe===void 0&&(fe=n.getUniformBlockIndex(te,C.name),se.set(C,fe))}function oe(C,te){let fe=l.get(te).get(C);c.get(te)!==fe&&(n.uniformBlockBinding(te,fe,C.__bindingPointIndex),c.set(te,fe))}function ke(){n.disable(n.BLEND),n.disable(n.CULL_FACE),n.disable(n.DEPTH_TEST),n.disable(n.POLYGON_OFFSET_FILL),n.disable(n.SCISSOR_TEST),n.disable(n.STENCIL_TEST),n.disable(n.SAMPLE_ALPHA_TO_COVERAGE),n.blendEquation(n.FUNC_ADD),n.blendFunc(n.ONE,n.ZERO),n.blendFuncSeparate(n.ONE,n.ZERO,n.ONE,n.ZERO),n.blendColor(0,0,0,0),n.colorMask(!0,!0,!0,!0),n.clearColor(0,0,0,0),n.depthMask(!0),n.depthFunc(n.LESS),o.setReversed(!1),n.clearDepth(1),n.stencilMask(4294967295),n.stencilFunc(n.ALWAYS,0,4294967295),n.stencilOp(n.KEEP,n.KEEP,n.KEEP),n.clearStencil(0),n.cullFace(n.BACK),n.frontFace(n.CCW),n.polygonOffset(0,0),n.activeTexture(n.TEXTURE0),n.bindFramebuffer(n.FRAMEBUFFER,null),n.bindFramebuffer(n.DRAW_FRAMEBUFFER,null),n.bindFramebuffer(n.READ_FRAMEBUFFER,null),n.useProgram(null),n.lineWidth(1),n.scissor(0,0,n.canvas.width,n.canvas.height),n.viewport(0,0,n.canvas.width,n.canvas.height),h={},ae=null,ue={},u={},p=new WeakMap,d=[],g=null,v=!1,m=null,f=null,T=null,E=null,S=null,R=null,A=null,P=new Xe(0,0,0),N=0,M=!1,y=null,I=null,O=null,z=null,H=null,st.set(0,0,n.canvas.width,n.canvas.height),ct.set(0,0,n.canvas.width,n.canvas.height),r.reset(),o.reset(),a.reset()}return{buffers:{color:r,depth:o,stencil:a},enable:K,disable:pe,bindFramebuffer:De,drawBuffers:be,useProgram:$e,setBlending:ht,setMaterial:Fe,setFlipSided:Pe,setCullFace:_e,setLineWidth:ut,setPolygonOffset:xe,setScissorTest:Be,activeTexture:bt,bindTexture:xt,unbindTexture:b,compressedTexImage2D:_,compressedTexImage3D:F,texImage2D:ye,texImage3D:ee,updateUBOMapping:Me,uniformBlockBinding:oe,texStorage2D:ie,texStorage3D:ve,texSubImage2D:X,texSubImage3D:J,compressedTexSubImage2D:G,compressedTexSubImage3D:Se,scissor:he,viewport:Ie,reset:ke}}function W0(n,e,t,i,s,r,o){let a=e.has("WEBGL_multisampled_render_to_texture")?e.get("WEBGL_multisampled_render_to_texture"):null,c=typeof navigator>"u"?!1:/OculusBrowser/g.test(navigator.userAgent),l=new Ue,h=new WeakMap,u,p=new WeakMap,d=!1;try{d=typeof OffscreenCanvas<"u"&&new OffscreenCanvas(1,1).getContext("2d")!==null}catch{}function g(b,_){return d?new OffscreenCanvas(b,_):Vs("canvas")}function v(b,_,F){let X=1,J=xt(b);if((J.width>F||J.height>F)&&(X=F/Math.max(J.width,J.height)),X<1)if(typeof HTMLImageElement<"u"&&b instanceof HTMLImageElement||typeof HTMLCanvasElement<"u"&&b instanceof HTMLCanvasElement||typeof ImageBitmap<"u"&&b instanceof ImageBitmap||typeof VideoFrame<"u"&&b instanceof VideoFrame){let G=Math.floor(X*J.width),Se=Math.floor(X*J.height);u===void 0&&(u=g(G,Se));let ie=_?g(G,Se):u;return ie.width=G,ie.height=Se,ie.getContext("2d").drawImage(b,0,0,G,Se),console.warn("THREE.WebGLRenderer: Texture has been resized from ("+J.width+"x"+J.height+") to ("+G+"x"+Se+")."),ie}else return"data"in b&&console.warn("THREE.WebGLRenderer: Image in DataTexture is too big ("+J.width+"x"+J.height+")."),b;return b}function m(b){return b.generateMipmaps}function f(b){n.generateMipmap(b)}function T(b){return b.isWebGLCubeRenderTarget?n.TEXTURE_CUBE_MAP:b.isWebGL3DRenderTarget?n.TEXTURE_3D:b.isWebGLArrayRenderTarget||b.isCompressedArrayTexture?n.TEXTURE_2D_ARRAY:n.TEXTURE_2D}function E(b,_,F,X,J=!1){if(b!==null){if(n[b]!==void 0)return n[b];console.warn("THREE.WebGLRenderer: Attempt to use non-existing WebGL internal format '"+b+"'")}let G=_;if(_===n.RED&&(F===n.FLOAT&&(G=n.R32F),F===n.HALF_FLOAT&&(G=n.R16F),F===n.UNSIGNED_BYTE&&(G=n.R8)),_===n.RED_INTEGER&&(F===n.UNSIGNED_BYTE&&(G=n.R8UI),F===n.UNSIGNED_SHORT&&(G=n.R16UI),F===n.UNSIGNED_INT&&(G=n.R32UI),F===n.BYTE&&(G=n.R8I),F===n.SHORT&&(G=n.R16I),F===n.INT&&(G=n.R32I)),_===n.RG&&(F===n.FLOAT&&(G=n.RG32F),F===n.HALF_FLOAT&&(G=n.RG16F),F===n.UNSIGNED_BYTE&&(G=n.RG8)),_===n.RG_INTEGER&&(F===n.UNSIGNED_BYTE&&(G=n.RG8UI),F===n.UNSIGNED_SHORT&&(G=n.RG16UI),F===n.UNSIGNED_INT&&(G=n.RG32UI),F===n.BYTE&&(G=n.RG8I),F===n.SHORT&&(G=n.RG16I),F===n.INT&&(G=n.RG32I)),_===n.RGB_INTEGER&&(F===n.UNSIGNED_BYTE&&(G=n.RGB8UI),F===n.UNSIGNED_SHORT&&(G=n.RGB16UI),F===n.UNSIGNED_INT&&(G=n.RGB32UI),F===n.BYTE&&(G=n.RGB8I),F===n.SHORT&&(G=n.RGB16I),F===n.INT&&(G=n.RGB32I)),_===n.RGBA_INTEGER&&(F===n.UNSIGNED_BYTE&&(G=n.RGBA8UI),F===n.UNSIGNED_SHORT&&(G=n.RGBA16UI),F===n.UNSIGNED_INT&&(G=n.RGBA32UI),F===n.BYTE&&(G=n.RGBA8I),F===n.SHORT&&(G=n.RGBA16I),F===n.INT&&(G=n.RGBA32I)),_===n.RGB&&(F===n.UNSIGNED_INT_5_9_9_9_REV&&(G=n.RGB9_E5),F===n.UNSIGNED_INT_10F_11F_11F_REV&&(G=n.R11F_G11F_B10F)),_===n.RGBA){let Se=J?Bs:Ye.getTransfer(X);F===n.FLOAT&&(G=n.RGBA32F),F===n.HALF_FLOAT&&(G=n.RGBA16F),F===n.UNSIGNED_BYTE&&(G=Se===et?n.SRGB8_ALPHA8:n.RGBA8),F===n.UNSIGNED_SHORT_4_4_4_4&&(G=n.RGBA4),F===n.UNSIGNED_SHORT_5_5_5_1&&(G=n.RGB5_A1)}return(G===n.R16F||G===n.R32F||G===n.RG16F||G===n.RG32F||G===n.RGBA16F||G===n.RGBA32F)&&e.get("EXT_color_buffer_float"),G}function S(b,_){let F;return b?_===null||_===di||_===vs?F=n.DEPTH24_STENCIL8:_===Tn?F=n.DEPTH32F_STENCIL8:_===_s&&(F=n.DEPTH24_STENCIL8,console.warn("DepthTexture: 16 bit depth attachment is not supported with stencil. Using 24-bit attachment.")):_===null||_===di||_===vs?F=n.DEPTH_COMPONENT24:_===Tn?F=n.DEPTH_COMPONENT32F:_===_s&&(F=n.DEPTH_COMPONENT16),F}function R(b,_){return m(b)===!0||b.isFramebufferTexture&&b.minFilter!==rn&&b.minFilter!==dn?Math.log2(Math.max(_.width,_.height))+1:b.mipmaps!==void 0&&b.mipmaps.length>0?b.mipmaps.length:b.isCompressedTexture&&Array.isArray(b.image)?_.mipmaps.length:1}function A(b){let _=b.target;_.removeEventListener("dispose",A),N(_),_.isVideoTexture&&h.delete(_)}function P(b){let _=b.target;_.removeEventListener("dispose",P),y(_)}function N(b){let _=i.get(b);if(_.__webglInit===void 0)return;let F=b.source,X=p.get(F);if(X){let J=X[_.__cacheKey];J.usedTimes--,J.usedTimes===0&&M(b),Object.keys(X).length===0&&p.delete(F)}i.remove(b)}function M(b){let _=i.get(b);n.deleteTexture(_.__webglTexture);let F=b.source,X=p.get(F);delete X[_.__cacheKey],o.memory.textures--}function y(b){let _=i.get(b);if(b.depthTexture&&(b.depthTexture.dispose(),i.remove(b.depthTexture)),b.isWebGLCubeRenderTarget)for(let X=0;X<6;X++){if(Array.isArray(_.__webglFramebuffer[X]))for(let J=0;J<_.__webglFramebuffer[X].length;J++)n.deleteFramebuffer(_.__webglFramebuffer[X][J]);else n.deleteFramebuffer(_.__webglFramebuffer[X]);_.__webglDepthbuffer&&n.deleteRenderbuffer(_.__webglDepthbuffer[X])}else{if(Array.isArray(_.__webglFramebuffer))for(let X=0;X<_.__webglFramebuffer.length;X++)n.deleteFramebuffer(_.__webglFramebuffer[X]);else n.deleteFramebuffer(_.__webglFramebuffer);if(_.__webglDepthbuffer&&n.deleteRenderbuffer(_.__webglDepthbuffer),_.__webglMultisampledFramebuffer&&n.deleteFramebuffer(_.__webglMultisampledFramebuffer),_.__webglColorRenderbuffer)for(let X=0;X<_.__webglColorRenderbuffer.length;X++)_.__webglColorRenderbuffer[X]&&n.deleteRenderbuffer(_.__webglColorRenderbuffer[X]);_.__webglDepthRenderbuffer&&n.deleteRenderbuffer(_.__webglDepthRenderbuffer)}let F=b.textures;for(let X=0,J=F.length;X<J;X++){let G=i.get(F[X]);G.__webglTexture&&(n.deleteTexture(G.__webglTexture),o.memory.textures--),i.remove(F[X])}i.remove(b)}let I=0;function O(){I=0}function z(){let b=I;return b>=s.maxTextures&&console.warn("THREE.WebGLTextures: Trying to use "+b+" texture units while this GPU supports only "+s.maxTextures),I+=1,b}function H(b){let _=[];return _.push(b.wrapS),_.push(b.wrapT),_.push(b.wrapR||0),_.push(b.magFilter),_.push(b.minFilter),_.push(b.anisotropy),_.push(b.internalFormat),_.push(b.format),_.push(b.type),_.push(b.generateMipmaps),_.push(b.premultiplyAlpha),_.push(b.flipY),_.push(b.unpackAlignment),_.push(b.colorSpace),_.join()}function q(b,_){let F=i.get(b);if(b.isVideoTexture&&Be(b),b.isRenderTargetTexture===!1&&b.isExternalTexture!==!0&&b.version>0&&F.__version!==b.version){let X=b.image;if(X===null)console.warn("THREE.WebGLRenderer: Texture marked for update but no image data found.");else if(X.complete===!1)console.warn("THREE.WebGLRenderer: Texture marked for update but image is incomplete");else{$(F,b,_);return}}else b.isExternalTexture&&(F.__webglTexture=b.sourceTexture?b.sourceTexture:null);t.bindTexture(n.TEXTURE_2D,F.__webglTexture,n.TEXTURE0+_)}function W(b,_){let F=i.get(b);if(b.isRenderTargetTexture===!1&&b.version>0&&F.__version!==b.version){$(F,b,_);return}t.bindTexture(n.TEXTURE_2D_ARRAY,F.__webglTexture,n.TEXTURE0+_)}function ne(b,_){let F=i.get(b);if(b.isRenderTargetTexture===!1&&b.version>0&&F.__version!==b.version){$(F,b,_);return}t.bindTexture(n.TEXTURE_3D,F.__webglTexture,n.TEXTURE0+_)}function V(b,_){let F=i.get(b);if(b.version>0&&F.__version!==b.version){K(F,b,_);return}t.bindTexture(n.TEXTURE_CUBE_MAP,F.__webglTexture,n.TEXTURE0+_)}let ae={[Qr]:n.REPEAT,[ii]:n.CLAMP_TO_EDGE,[ea]:n.MIRRORED_REPEAT},ue={[rn]:n.NEAREST,[cu]:n.NEAREST_MIPMAP_NEAREST,[cr]:n.NEAREST_MIPMAP_LINEAR,[dn]:n.LINEAR,[Ua]:n.LINEAR_MIPMAP_NEAREST,[ui]:n.LINEAR_MIPMAP_LINEAR},Ee={[fu]:n.NEVER,[vu]:n.ALWAYS,[pu]:n.LESS,[Vl]:n.LEQUAL,[mu]:n.EQUAL,[xu]:n.GEQUAL,[gu]:n.GREATER,[_u]:n.NOTEQUAL};function Ve(b,_){if(_.type===Tn&&e.has("OES_texture_float_linear")===!1&&(_.magFilter===dn||_.magFilter===Ua||_.magFilter===cr||_.magFilter===ui||_.minFilter===dn||_.minFilter===Ua||_.minFilter===cr||_.minFilter===ui)&&console.warn("THREE.WebGLRenderer: Unable to use linear filtering with floating point textures. OES_texture_float_linear not supported on this device."),n.texParameteri(b,n.TEXTURE_WRAP_S,ae[_.wrapS]),n.texParameteri(b,n.TEXTURE_WRAP_T,ae[_.wrapT]),(b===n.TEXTURE_3D||b===n.TEXTURE_2D_ARRAY)&&n.texParameteri(b,n.TEXTURE_WRAP_R,ae[_.wrapR]),n.texParameteri(b,n.TEXTURE_MAG_FILTER,ue[_.magFilter]),n.texParameteri(b,n.TEXTURE_MIN_FILTER,ue[_.minFilter]),_.compareFunction&&(n.texParameteri(b,n.TEXTURE_COMPARE_MODE,n.COMPARE_REF_TO_TEXTURE),n.texParameteri(b,n.TEXTURE_COMPARE_FUNC,Ee[_.compareFunction])),e.has("EXT_texture_filter_anisotropic")===!0){if(_.magFilter===rn||_.minFilter!==cr&&_.minFilter!==ui||_.type===Tn&&e.has("OES_texture_float_linear")===!1)return;if(_.anisotropy>1||i.get(_).__currentAnisotropy){let F=e.get("EXT_texture_filter_anisotropic");n.texParameterf(b,F.TEXTURE_MAX_ANISOTROPY_EXT,Math.min(_.anisotropy,s.getMaxAnisotropy())),i.get(_).__currentAnisotropy=_.anisotropy}}}function st(b,_){let F=!1;b.__webglInit===void 0&&(b.__webglInit=!0,_.addEventListener("dispose",A));let X=_.source,J=p.get(X);J===void 0&&(J={},p.set(X,J));let G=H(_);if(G!==b.__cacheKey){J[G]===void 0&&(J[G]={texture:n.createTexture(),usedTimes:0},o.memory.textures++,F=!0),J[G].usedTimes++;let Se=J[b.__cacheKey];Se!==void 0&&(J[b.__cacheKey].usedTimes--,Se.usedTimes===0&&M(_)),b.__cacheKey=G,b.__webglTexture=J[G].texture}return F}function ct(b,_,F){return Math.floor(Math.floor(b/F)/_)}function Je(b,_,F,X){let G=b.updateRanges;if(G.length===0)t.texSubImage2D(n.TEXTURE_2D,0,0,0,_.width,_.height,F,X,_.data);else{G.sort((ee,he)=>ee.start-he.start);let Se=0;for(let ee=1;ee<G.length;ee++){let he=G[Se],Ie=G[ee],Me=he.start+he.count,oe=ct(Ie.start,_.width,4),ke=ct(he.start,_.width,4);Ie.start<=Me+1&&oe===ke&&ct(Ie.start+Ie.count-1,_.width,4)===oe?he.count=Math.max(he.count,Ie.start+Ie.count-he.start):(++Se,G[Se]=Ie)}G.length=Se+1;let ie=n.getParameter(n.UNPACK_ROW_LENGTH),ve=n.getParameter(n.UNPACK_SKIP_PIXELS),ye=n.getParameter(n.UNPACK_SKIP_ROWS);n.pixelStorei(n.UNPACK_ROW_LENGTH,_.width);for(let ee=0,he=G.length;ee<he;ee++){let Ie=G[ee],Me=Math.floor(Ie.start/4),oe=Math.ceil(Ie.count/4),ke=Me%_.width,C=Math.floor(Me/_.width),te=oe,se=1;n.pixelStorei(n.UNPACK_SKIP_PIXELS,ke),n.pixelStorei(n.UNPACK_SKIP_ROWS,C),t.texSubImage2D(n.TEXTURE_2D,0,ke,C,te,se,F,X,_.data)}b.clearUpdateRanges(),n.pixelStorei(n.UNPACK_ROW_LENGTH,ie),n.pixelStorei(n.UNPACK_SKIP_PIXELS,ve),n.pixelStorei(n.UNPACK_SKIP_ROWS,ye)}}function $(b,_,F){let X=n.TEXTURE_2D;(_.isDataArrayTexture||_.isCompressedArrayTexture)&&(X=n.TEXTURE_2D_ARRAY),_.isData3DTexture&&(X=n.TEXTURE_3D);let J=st(b,_),G=_.source;t.bindTexture(X,b.__webglTexture,n.TEXTURE0+F);let Se=i.get(G);if(G.version!==Se.__version||J===!0){t.activeTexture(n.TEXTURE0+F);let ie=Ye.getPrimaries(Ye.workingColorSpace),ve=_.colorSpace===Xn?null:Ye.getPrimaries(_.colorSpace),ye=_.colorSpace===Xn||ie===ve?n.NONE:n.BROWSER_DEFAULT_WEBGL;n.pixelStorei(n.UNPACK_FLIP_Y_WEBGL,_.flipY),n.pixelStorei(n.UNPACK_PREMULTIPLY_ALPHA_WEBGL,_.premultiplyAlpha),n.pixelStorei(n.UNPACK_ALIGNMENT,_.unpackAlignment),n.pixelStorei(n.UNPACK_COLORSPACE_CONVERSION_WEBGL,ye);let ee=v(_.image,!1,s.maxTextureSize);ee=bt(_,ee);let he=r.convert(_.format,_.colorSpace),Ie=r.convert(_.type),Me=E(_.internalFormat,he,Ie,_.colorSpace,_.isVideoTexture);Ve(X,_);let oe,ke=_.mipmaps,C=_.isVideoTexture!==!0,te=Se.__version===void 0||J===!0,se=G.dataReady,fe=R(_,ee);if(_.isDepthTexture)Me=S(_.format===ys,_.type),te&&(C?t.texStorage2D(n.TEXTURE_2D,1,Me,ee.width,ee.height):t.texImage2D(n.TEXTURE_2D,0,Me,ee.width,ee.height,0,he,Ie,null));else if(_.isDataTexture)if(ke.length>0){C&&te&&t.texStorage2D(n.TEXTURE_2D,fe,Me,ke[0].width,ke[0].height);for(let j=0,Y=ke.length;j<Y;j++)oe=ke[j],C?se&&t.texSubImage2D(n.TEXTURE_2D,j,0,0,oe.width,oe.height,he,Ie,oe.data):t.texImage2D(n.TEXTURE_2D,j,Me,oe.width,oe.height,0,he,Ie,oe.data);_.generateMipmaps=!1}else C?(te&&t.texStorage2D(n.TEXTURE_2D,fe,Me,ee.width,ee.height),se&&Je(_,ee,he,Ie)):t.texImage2D(n.TEXTURE_2D,0,Me,ee.width,ee.height,0,he,Ie,ee.data);else if(_.isCompressedTexture)if(_.isCompressedArrayTexture){C&&te&&t.texStorage3D(n.TEXTURE_2D_ARRAY,fe,Me,ke[0].width,ke[0].height,ee.depth);for(let j=0,Y=ke.length;j<Y;j++)if(oe=ke[j],_.format!==an)if(he!==null)if(C){if(se)if(_.layerUpdates.size>0){let ge=$l(oe.width,oe.height,_.format,_.type);for(let Ne of _.layerUpdates){let rt=oe.data.subarray(Ne*ge/oe.data.BYTES_PER_ELEMENT,(Ne+1)*ge/oe.data.BYTES_PER_ELEMENT);t.compressedTexSubImage3D(n.TEXTURE_2D_ARRAY,j,0,0,Ne,oe.width,oe.height,1,he,rt)}_.clearLayerUpdates()}else t.compressedTexSubImage3D(n.TEXTURE_2D_ARRAY,j,0,0,0,oe.width,oe.height,ee.depth,he,oe.data)}else t.compressedTexImage3D(n.TEXTURE_2D_ARRAY,j,Me,oe.width,oe.height,ee.depth,0,oe.data,0,0);else console.warn("THREE.WebGLRenderer: Attempt to load unsupported compressed texture format in .uploadTexture()");else C?se&&t.texSubImage3D(n.TEXTURE_2D_ARRAY,j,0,0,0,oe.width,oe.height,ee.depth,he,Ie,oe.data):t.texImage3D(n.TEXTURE_2D_ARRAY,j,Me,oe.width,oe.height,ee.depth,0,he,Ie,oe.data)}else{C&&te&&t.texStorage2D(n.TEXTURE_2D,fe,Me,ke[0].width,ke[0].height);for(let j=0,Y=ke.length;j<Y;j++)oe=ke[j],_.format!==an?he!==null?C?se&&t.compressedTexSubImage2D(n.TEXTURE_2D,j,0,0,oe.width,oe.height,he,oe.data):t.compressedTexImage2D(n.TEXTURE_2D,j,Me,oe.width,oe.height,0,oe.data):console.warn("THREE.WebGLRenderer: Attempt to load unsupported compressed texture format in .uploadTexture()"):C?se&&t.texSubImage2D(n.TEXTURE_2D,j,0,0,oe.width,oe.height,he,Ie,oe.data):t.texImage2D(n.TEXTURE_2D,j,Me,oe.width,oe.height,0,he,Ie,oe.data)}else if(_.isDataArrayTexture)if(C){if(te&&t.texStorage3D(n.TEXTURE_2D_ARRAY,fe,Me,ee.width,ee.height,ee.depth),se)if(_.layerUpdates.size>0){let j=$l(ee.width,ee.height,_.format,_.type);for(let Y of _.layerUpdates){let ge=ee.data.subarray(Y*j/ee.data.BYTES_PER_ELEMENT,(Y+1)*j/ee.data.BYTES_PER_ELEMENT);t.texSubImage3D(n.TEXTURE_2D_ARRAY,0,0,0,Y,ee.width,ee.height,1,he,Ie,ge)}_.clearLayerUpdates()}else t.texSubImage3D(n.TEXTURE_2D_ARRAY,0,0,0,0,ee.width,ee.height,ee.depth,he,Ie,ee.data)}else t.texImage3D(n.TEXTURE_2D_ARRAY,0,Me,ee.width,ee.height,ee.depth,0,he,Ie,ee.data);else if(_.isData3DTexture)C?(te&&t.texStorage3D(n.TEXTURE_3D,fe,Me,ee.width,ee.height,ee.depth),se&&t.texSubImage3D(n.TEXTURE_3D,0,0,0,0,ee.width,ee.height,ee.depth,he,Ie,ee.data)):t.texImage3D(n.TEXTURE_3D,0,Me,ee.width,ee.height,ee.depth,0,he,Ie,ee.data);else if(_.isFramebufferTexture){if(te)if(C)t.texStorage2D(n.TEXTURE_2D,fe,Me,ee.width,ee.height);else{let j=ee.width,Y=ee.height;for(let ge=0;ge<fe;ge++)t.texImage2D(n.TEXTURE_2D,ge,Me,j,Y,0,he,Ie,null),j>>=1,Y>>=1}}else if(ke.length>0){if(C&&te){let j=xt(ke[0]);t.texStorage2D(n.TEXTURE_2D,fe,Me,j.width,j.height)}for(let j=0,Y=ke.length;j<Y;j++)oe=ke[j],C?se&&t.texSubImage2D(n.TEXTURE_2D,j,0,0,he,Ie,oe):t.texImage2D(n.TEXTURE_2D,j,Me,he,Ie,oe);_.generateMipmaps=!1}else if(C){if(te){let j=xt(ee);t.texStorage2D(n.TEXTURE_2D,fe,Me,j.width,j.height)}se&&t.texSubImage2D(n.TEXTURE_2D,0,0,0,he,Ie,ee)}else t.texImage2D(n.TEXTURE_2D,0,Me,he,Ie,ee);m(_)&&f(X),Se.__version=G.version,_.onUpdate&&_.onUpdate(_)}b.__version=_.version}function K(b,_,F){if(_.image.length!==6)return;let X=st(b,_),J=_.source;t.bindTexture(n.TEXTURE_CUBE_MAP,b.__webglTexture,n.TEXTURE0+F);let G=i.get(J);if(J.version!==G.__version||X===!0){t.activeTexture(n.TEXTURE0+F);let Se=Ye.getPrimaries(Ye.workingColorSpace),ie=_.colorSpace===Xn?null:Ye.getPrimaries(_.colorSpace),ve=_.colorSpace===Xn||Se===ie?n.NONE:n.BROWSER_DEFAULT_WEBGL;n.pixelStorei(n.UNPACK_FLIP_Y_WEBGL,_.flipY),n.pixelStorei(n.UNPACK_PREMULTIPLY_ALPHA_WEBGL,_.premultiplyAlpha),n.pixelStorei(n.UNPACK_ALIGNMENT,_.unpackAlignment),n.pixelStorei(n.UNPACK_COLORSPACE_CONVERSION_WEBGL,ve);let ye=_.isCompressedTexture||_.image[0].isCompressedTexture,ee=_.image[0]&&_.image[0].isDataTexture,he=[];for(let Y=0;Y<6;Y++)!ye&&!ee?he[Y]=v(_.image[Y],!0,s.maxCubemapSize):he[Y]=ee?_.image[Y].image:_.image[Y],he[Y]=bt(_,he[Y]);let Ie=he[0],Me=r.convert(_.format,_.colorSpace),oe=r.convert(_.type),ke=E(_.internalFormat,Me,oe,_.colorSpace),C=_.isVideoTexture!==!0,te=G.__version===void 0||X===!0,se=J.dataReady,fe=R(_,Ie);Ve(n.TEXTURE_CUBE_MAP,_);let j;if(ye){C&&te&&t.texStorage2D(n.TEXTURE_CUBE_MAP,fe,ke,Ie.width,Ie.height);for(let Y=0;Y<6;Y++){j=he[Y].mipmaps;for(let ge=0;ge<j.length;ge++){let Ne=j[ge];_.format!==an?Me!==null?C?se&&t.compressedTexSubImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge,0,0,Ne.width,Ne.height,Me,Ne.data):t.compressedTexImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge,ke,Ne.width,Ne.height,0,Ne.data):console.warn("THREE.WebGLRenderer: Attempt to load unsupported compressed texture format in .setTextureCube()"):C?se&&t.texSubImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge,0,0,Ne.width,Ne.height,Me,oe,Ne.data):t.texImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge,ke,Ne.width,Ne.height,0,Me,oe,Ne.data)}}}else{if(j=_.mipmaps,C&&te){j.length>0&&fe++;let Y=xt(he[0]);t.texStorage2D(n.TEXTURE_CUBE_MAP,fe,ke,Y.width,Y.height)}for(let Y=0;Y<6;Y++)if(ee){C?se&&t.texSubImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,0,0,0,he[Y].width,he[Y].height,Me,oe,he[Y].data):t.texImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,0,ke,he[Y].width,he[Y].height,0,Me,oe,he[Y].data);for(let ge=0;ge<j.length;ge++){let rt=j[ge].image[Y].image;C?se&&t.texSubImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge+1,0,0,rt.width,rt.height,Me,oe,rt.data):t.texImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge+1,ke,rt.width,rt.height,0,Me,oe,rt.data)}}else{C?se&&t.texSubImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,0,0,0,Me,oe,he[Y]):t.texImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,0,ke,Me,oe,he[Y]);for(let ge=0;ge<j.length;ge++){let Ne=j[ge];C?se&&t.texSubImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge+1,0,0,Me,oe,Ne.image[Y]):t.texImage2D(n.TEXTURE_CUBE_MAP_POSITIVE_X+Y,ge+1,ke,Me,oe,Ne.image[Y])}}}m(_)&&f(n.TEXTURE_CUBE_MAP),G.__version=J.version,_.onUpdate&&_.onUpdate(_)}b.__version=_.version}function pe(b,_,F,X,J,G){let Se=r.convert(F.format,F.colorSpace),ie=r.convert(F.type),ve=E(F.internalFormat,Se,ie,F.colorSpace),ye=i.get(_),ee=i.get(F);if(ee.__renderTarget=_,!ye.__hasExternalTextures){let he=Math.max(1,_.width>>G),Ie=Math.max(1,_.height>>G);J===n.TEXTURE_3D||J===n.TEXTURE_2D_ARRAY?t.texImage3D(J,G,ve,he,Ie,_.depth,0,Se,ie,null):t.texImage2D(J,G,ve,he,Ie,0,Se,ie,null)}t.bindFramebuffer(n.FRAMEBUFFER,b),xe(_)?a.framebufferTexture2DMultisampleEXT(n.FRAMEBUFFER,X,J,ee.__webglTexture,0,ut(_)):(J===n.TEXTURE_2D||J>=n.TEXTURE_CUBE_MAP_POSITIVE_X&&J<=n.TEXTURE_CUBE_MAP_NEGATIVE_Z)&&n.framebufferTexture2D(n.FRAMEBUFFER,X,J,ee.__webglTexture,G),t.bindFramebuffer(n.FRAMEBUFFER,null)}function De(b,_,F){if(n.bindRenderbuffer(n.RENDERBUFFER,b),_.depthBuffer){let X=_.depthTexture,J=X&&X.isDepthTexture?X.type:null,G=S(_.stencilBuffer,J),Se=_.stencilBuffer?n.DEPTH_STENCIL_ATTACHMENT:n.DEPTH_ATTACHMENT,ie=ut(_);xe(_)?a.renderbufferStorageMultisampleEXT(n.RENDERBUFFER,ie,G,_.width,_.height):F?n.renderbufferStorageMultisample(n.RENDERBUFFER,ie,G,_.width,_.height):n.renderbufferStorage(n.RENDERBUFFER,G,_.width,_.height),n.framebufferRenderbuffer(n.FRAMEBUFFER,Se,n.RENDERBUFFER,b)}else{let X=_.textures;for(let J=0;J<X.length;J++){let G=X[J],Se=r.convert(G.format,G.colorSpace),ie=r.convert(G.type),ve=E(G.internalFormat,Se,ie,G.colorSpace),ye=ut(_);F&&xe(_)===!1?n.renderbufferStorageMultisample(n.RENDERBUFFER,ye,ve,_.width,_.height):xe(_)?a.renderbufferStorageMultisampleEXT(n.RENDERBUFFER,ye,ve,_.width,_.height):n.renderbufferStorage(n.RENDERBUFFER,ve,_.width,_.height)}}n.bindRenderbuffer(n.RENDERBUFFER,null)}function be(b,_){if(_&&_.isWebGLCubeRenderTarget)throw new Error("Depth Texture with cube render targets is not supported");if(t.bindFramebuffer(n.FRAMEBUFFER,b),!(_.depthTexture&&_.depthTexture.isDepthTexture))throw new Error("renderTarget.depthTexture must be an instance of THREE.DepthTexture");let X=i.get(_.depthTexture);X.__renderTarget=_,(!X.__webglTexture||_.depthTexture.image.width!==_.width||_.depthTexture.image.height!==_.height)&&(_.depthTexture.image.width=_.width,_.depthTexture.image.height=_.height,_.depthTexture.needsUpdate=!0),q(_.depthTexture,0);let J=X.__webglTexture,G=ut(_);if(_.depthTexture.format===os)xe(_)?a.framebufferTexture2DMultisampleEXT(n.FRAMEBUFFER,n.DEPTH_ATTACHMENT,n.TEXTURE_2D,J,0,G):n.framebufferTexture2D(n.FRAMEBUFFER,n.DEPTH_ATTACHMENT,n.TEXTURE_2D,J,0);else if(_.depthTexture.format===ys)xe(_)?a.framebufferTexture2DMultisampleEXT(n.FRAMEBUFFER,n.DEPTH_STENCIL_ATTACHMENT,n.TEXTURE_2D,J,0,G):n.framebufferTexture2D(n.FRAMEBUFFER,n.DEPTH_STENCIL_ATTACHMENT,n.TEXTURE_2D,J,0);else throw new Error("Unknown depthTexture format")}function $e(b){let _=i.get(b),F=b.isWebGLCubeRenderTarget===!0;if(_.__boundDepthTexture!==b.depthTexture){let X=b.depthTexture;if(_.__depthDisposeCallback&&_.__depthDisposeCallback(),X){let J=()=>{delete _.__boundDepthTexture,delete _.__depthDisposeCallback,X.removeEventListener("dispose",J)};X.addEventListener("dispose",J),_.__depthDisposeCallback=J}_.__boundDepthTexture=X}if(b.depthTexture&&!_.__autoAllocateDepthBuffer){if(F)throw new Error("target.depthTexture not supported in Cube render targets");let X=b.texture.mipmaps;X&&X.length>0?be(_.__webglFramebuffer[0],b):be(_.__webglFramebuffer,b)}else if(F){_.__webglDepthbuffer=[];for(let X=0;X<6;X++)if(t.bindFramebuffer(n.FRAMEBUFFER,_.__webglFramebuffer[X]),_.__webglDepthbuffer[X]===void 0)_.__webglDepthbuffer[X]=n.createRenderbuffer(),De(_.__webglDepthbuffer[X],b,!1);else{let J=b.stencilBuffer?n.DEPTH_STENCIL_ATTACHMENT:n.DEPTH_ATTACHMENT,G=_.__webglDepthbuffer[X];n.bindRenderbuffer(n.RENDERBUFFER,G),n.framebufferRenderbuffer(n.FRAMEBUFFER,J,n.RENDERBUFFER,G)}}else{let X=b.texture.mipmaps;if(X&&X.length>0?t.bindFramebuffer(n.FRAMEBUFFER,_.__webglFramebuffer[0]):t.bindFramebuffer(n.FRAMEBUFFER,_.__webglFramebuffer),_.__webglDepthbuffer===void 0)_.__webglDepthbuffer=n.createRenderbuffer(),De(_.__webglDepthbuffer,b,!1);else{let J=b.stencilBuffer?n.DEPTH_STENCIL_ATTACHMENT:n.DEPTH_ATTACHMENT,G=_.__webglDepthbuffer;n.bindRenderbuffer(n.RENDERBUFFER,G),n.framebufferRenderbuffer(n.FRAMEBUFFER,J,n.RENDERBUFFER,G)}}t.bindFramebuffer(n.FRAMEBUFFER,null)}function Rt(b,_,F){let X=i.get(b);_!==void 0&&pe(X.__webglFramebuffer,b,b.texture,n.COLOR_ATTACHMENT0,n.TEXTURE_2D,0),F!==void 0&&$e(b)}function w(b){let _=b.texture,F=i.get(b),X=i.get(_);b.addEventListener("dispose",P);let J=b.textures,G=b.isWebGLCubeRenderTarget===!0,Se=J.length>1;if(Se||(X.__webglTexture===void 0&&(X.__webglTexture=n.createTexture()),X.__version=_.version,o.memory.textures++),G){F.__webglFramebuffer=[];for(let ie=0;ie<6;ie++)if(_.mipmaps&&_.mipmaps.length>0){F.__webglFramebuffer[ie]=[];for(let ve=0;ve<_.mipmaps.length;ve++)F.__webglFramebuffer[ie][ve]=n.createFramebuffer()}else F.__webglFramebuffer[ie]=n.createFramebuffer()}else{if(_.mipmaps&&_.mipmaps.length>0){F.__webglFramebuffer=[];for(let ie=0;ie<_.mipmaps.length;ie++)F.__webglFramebuffer[ie]=n.createFramebuffer()}else F.__webglFramebuffer=n.createFramebuffer();if(Se)for(let ie=0,ve=J.length;ie<ve;ie++){let ye=i.get(J[ie]);ye.__webglTexture===void 0&&(ye.__webglTexture=n.createTexture(),o.memory.textures++)}if(b.samples>0&&xe(b)===!1){F.__webglMultisampledFramebuffer=n.createFramebuffer(),F.__webglColorRenderbuffer=[],t.bindFramebuffer(n.FRAMEBUFFER,F.__webglMultisampledFramebuffer);for(let ie=0;ie<J.length;ie++){let ve=J[ie];F.__webglColorRenderbuffer[ie]=n.createRenderbuffer(),n.bindRenderbuffer(n.RENDERBUFFER,F.__webglColorRenderbuffer[ie]);let ye=r.convert(ve.format,ve.colorSpace),ee=r.convert(ve.type),he=E(ve.internalFormat,ye,ee,ve.colorSpace,b.isXRRenderTarget===!0),Ie=ut(b);n.renderbufferStorageMultisample(n.RENDERBUFFER,Ie,he,b.width,b.height),n.framebufferRenderbuffer(n.FRAMEBUFFER,n.COLOR_ATTACHMENT0+ie,n.RENDERBUFFER,F.__webglColorRenderbuffer[ie])}n.bindRenderbuffer(n.RENDERBUFFER,null),b.depthBuffer&&(F.__webglDepthRenderbuffer=n.createRenderbuffer(),De(F.__webglDepthRenderbuffer,b,!0)),t.bindFramebuffer(n.FRAMEBUFFER,null)}}if(G){t.bindTexture(n.TEXTURE_CUBE_MAP,X.__webglTexture),Ve(n.TEXTURE_CUBE_MAP,_);for(let ie=0;ie<6;ie++)if(_.mipmaps&&_.mipmaps.length>0)for(let ve=0;ve<_.mipmaps.length;ve++)pe(F.__webglFramebuffer[ie][ve],b,_,n.COLOR_ATTACHMENT0,n.TEXTURE_CUBE_MAP_POSITIVE_X+ie,ve);else pe(F.__webglFramebuffer[ie],b,_,n.COLOR_ATTACHMENT0,n.TEXTURE_CUBE_MAP_POSITIVE_X+ie,0);m(_)&&f(n.TEXTURE_CUBE_MAP),t.unbindTexture()}else if(Se){for(let ie=0,ve=J.length;ie<ve;ie++){let ye=J[ie],ee=i.get(ye),he=n.TEXTURE_2D;(b.isWebGL3DRenderTarget||b.isWebGLArrayRenderTarget)&&(he=b.isWebGL3DRenderTarget?n.TEXTURE_3D:n.TEXTURE_2D_ARRAY),t.bindTexture(he,ee.__webglTexture),Ve(he,ye),pe(F.__webglFramebuffer,b,ye,n.COLOR_ATTACHMENT0+ie,he,0),m(ye)&&f(he)}t.unbindTexture()}else{let ie=n.TEXTURE_2D;if((b.isWebGL3DRenderTarget||b.isWebGLArrayRenderTarget)&&(ie=b.isWebGL3DRenderTarget?n.TEXTURE_3D:n.TEXTURE_2D_ARRAY),t.bindTexture(ie,X.__webglTexture),Ve(ie,_),_.mipmaps&&_.mipmaps.length>0)for(let ve=0;ve<_.mipmaps.length;ve++)pe(F.__webglFramebuffer[ve],b,_,n.COLOR_ATTACHMENT0,ie,ve);else pe(F.__webglFramebuffer,b,_,n.COLOR_ATTACHMENT0,ie,0);m(_)&&f(ie),t.unbindTexture()}b.depthBuffer&&$e(b)}function ht(b){let _=b.textures;for(let F=0,X=_.length;F<X;F++){let J=_[F];if(m(J)){let G=T(b),Se=i.get(J).__webglTexture;t.bindTexture(G,Se),f(G),t.unbindTexture()}}}let Fe=[],Pe=[];function _e(b){if(b.samples>0){if(xe(b)===!1){let _=b.textures,F=b.width,X=b.height,J=n.COLOR_BUFFER_BIT,G=b.stencilBuffer?n.DEPTH_STENCIL_ATTACHMENT:n.DEPTH_ATTACHMENT,Se=i.get(b),ie=_.length>1;if(ie)for(let ye=0;ye<_.length;ye++)t.bindFramebuffer(n.FRAMEBUFFER,Se.__webglMultisampledFramebuffer),n.framebufferRenderbuffer(n.FRAMEBUFFER,n.COLOR_ATTACHMENT0+ye,n.RENDERBUFFER,null),t.bindFramebuffer(n.FRAMEBUFFER,Se.__webglFramebuffer),n.framebufferTexture2D(n.DRAW_FRAMEBUFFER,n.COLOR_ATTACHMENT0+ye,n.TEXTURE_2D,null,0);t.bindFramebuffer(n.READ_FRAMEBUFFER,Se.__webglMultisampledFramebuffer);let ve=b.texture.mipmaps;ve&&ve.length>0?t.bindFramebuffer(n.DRAW_FRAMEBUFFER,Se.__webglFramebuffer[0]):t.bindFramebuffer(n.DRAW_FRAMEBUFFER,Se.__webglFramebuffer);for(let ye=0;ye<_.length;ye++){if(b.resolveDepthBuffer&&(b.depthBuffer&&(J|=n.DEPTH_BUFFER_BIT),b.stencilBuffer&&b.resolveStencilBuffer&&(J|=n.STENCIL_BUFFER_BIT)),ie){n.framebufferRenderbuffer(n.READ_FRAMEBUFFER,n.COLOR_ATTACHMENT0,n.RENDERBUFFER,Se.__webglColorRenderbuffer[ye]);let ee=i.get(_[ye]).__webglTexture;n.framebufferTexture2D(n.DRAW_FRAMEBUFFER,n.COLOR_ATTACHMENT0,n.TEXTURE_2D,ee,0)}n.blitFramebuffer(0,0,F,X,0,0,F,X,J,n.NEAREST),c===!0&&(Fe.length=0,Pe.length=0,Fe.push(n.COLOR_ATTACHMENT0+ye),b.depthBuffer&&b.resolveDepthBuffer===!1&&(Fe.push(G),Pe.push(G),n.invalidateFramebuffer(n.DRAW_FRAMEBUFFER,Pe)),n.invalidateFramebuffer(n.READ_FRAMEBUFFER,Fe))}if(t.bindFramebuffer(n.READ_FRAMEBUFFER,null),t.bindFramebuffer(n.DRAW_FRAMEBUFFER,null),ie)for(let ye=0;ye<_.length;ye++){t.bindFramebuffer(n.FRAMEBUFFER,Se.__webglMultisampledFramebuffer),n.framebufferRenderbuffer(n.FRAMEBUFFER,n.COLOR_ATTACHMENT0+ye,n.RENDERBUFFER,Se.__webglColorRenderbuffer[ye]);let ee=i.get(_[ye]).__webglTexture;t.bindFramebuffer(n.FRAMEBUFFER,Se.__webglFramebuffer),n.framebufferTexture2D(n.DRAW_FRAMEBUFFER,n.COLOR_ATTACHMENT0+ye,n.TEXTURE_2D,ee,0)}t.bindFramebuffer(n.DRAW_FRAMEBUFFER,Se.__webglMultisampledFramebuffer)}else if(b.depthBuffer&&b.resolveDepthBuffer===!1&&c){let _=b.stencilBuffer?n.DEPTH_STENCIL_ATTACHMENT:n.DEPTH_ATTACHMENT;n.invalidateFramebuffer(n.DRAW_FRAMEBUFFER,[_])}}}function ut(b){return Math.min(s.maxSamples,b.samples)}function xe(b){let _=i.get(b);return b.samples>0&&e.has("WEBGL_multisampled_render_to_texture")===!0&&_.__useRenderToTexture!==!1}function Be(b){let _=o.render.frame;h.get(b)!==_&&(h.set(b,_),b.update())}function bt(b,_){let F=b.colorSpace,X=b.format,J=b.type;return b.isCompressedTexture===!0||b.isVideoTexture===!0||F!==wi&&F!==Xn&&(Ye.getTransfer(F)===et?(X!==an||J!==mn)&&console.warn("THREE.WebGLTextures: sRGB encoded textures have to use RGBAFormat and UnsignedByteType."):console.error("THREE.WebGLTextures: Unsupported texture color space:",F)),_}function xt(b){return typeof HTMLImageElement<"u"&&b instanceof HTMLImageElement?(l.width=b.naturalWidth||b.width,l.height=b.naturalHeight||b.height):typeof VideoFrame<"u"&&b instanceof VideoFrame?(l.width=b.displayWidth,l.height=b.displayHeight):(l.width=b.width,l.height=b.height),l}this.allocateTextureUnit=z,this.resetTextureUnits=O,this.setTexture2D=q,this.setTexture2DArray=W,this.setTexture3D=ne,this.setTextureCube=V,this.rebindTextures=Rt,this.setupRenderTarget=w,this.updateRenderTargetMipmap=ht,this.updateMultisampleRenderTarget=_e,this.setupDepthRenderbuffer=$e,this.setupFrameBufferTexture=pe,this.useMultisampledRTT=xe}function X0(n,e){function t(i,s=Xn){let r,o=Ye.getTransfer(s);if(i===mn)return n.UNSIGNED_BYTE;if(i===Fa)return n.UNSIGNED_SHORT_4_4_4_4;if(i===Oa)return n.UNSIGNED_SHORT_5_5_5_1;if(i===Ul)return n.UNSIGNED_INT_5_9_9_9_REV;if(i===Nl)return n.UNSIGNED_INT_10F_11F_11F_REV;if(i===Ll)return n.BYTE;if(i===Dl)return n.SHORT;if(i===_s)return n.UNSIGNED_SHORT;if(i===Na)return n.INT;if(i===di)return n.UNSIGNED_INT;if(i===Tn)return n.FLOAT;if(i===xs)return n.HALF_FLOAT;if(i===Fl)return n.ALPHA;if(i===Ol)return n.RGB;if(i===an)return n.RGBA;if(i===os)return n.DEPTH_COMPONENT;if(i===ys)return n.DEPTH_STENCIL;if(i===kl)return n.RED;if(i===ka)return n.RED_INTEGER;if(i===Bl)return n.RG;if(i===Ba)return n.RG_INTEGER;if(i===za)return n.RGBA_INTEGER;if(i===hr||i===ur||i===dr||i===fr)if(o===et)if(r=e.get("WEBGL_compressed_texture_s3tc_srgb"),r!==null){if(i===hr)return r.COMPRESSED_SRGB_S3TC_DXT1_EXT;if(i===ur)return r.COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT;if(i===dr)return r.COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT;if(i===fr)return r.COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT}else return null;else if(r=e.get("WEBGL_compressed_texture_s3tc"),r!==null){if(i===hr)return r.COMPRESSED_RGB_S3TC_DXT1_EXT;if(i===ur)return r.COMPRESSED_RGBA_S3TC_DXT1_EXT;if(i===dr)return r.COMPRESSED_RGBA_S3TC_DXT3_EXT;if(i===fr)return r.COMPRESSED_RGBA_S3TC_DXT5_EXT}else return null;if(i===Va||i===Ga||i===Ha||i===Wa)if(r=e.get("WEBGL_compressed_texture_pvrtc"),r!==null){if(i===Va)return r.COMPRESSED_RGB_PVRTC_4BPPV1_IMG;if(i===Ga)return r.COMPRESSED_RGB_PVRTC_2BPPV1_IMG;if(i===Ha)return r.COMPRESSED_RGBA_PVRTC_4BPPV1_IMG;if(i===Wa)return r.COMPRESSED_RGBA_PVRTC_2BPPV1_IMG}else return null;if(i===Xa||i===qa||i===$a)if(r=e.get("WEBGL_compressed_texture_etc"),r!==null){if(i===Xa||i===qa)return o===et?r.COMPRESSED_SRGB8_ETC2:r.COMPRESSED_RGB8_ETC2;if(i===$a)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ETC2_EAC:r.COMPRESSED_RGBA8_ETC2_EAC}else return null;if(i===Ya||i===Za||i===Ja||i===Ka||i===ja||i===Qa||i===eo||i===to||i===no||i===io||i===so||i===ro||i===ao||i===oo)if(r=e.get("WEBGL_compressed_texture_astc"),r!==null){if(i===Ya)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_4x4_KHR:r.COMPRESSED_RGBA_ASTC_4x4_KHR;if(i===Za)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_5x4_KHR:r.COMPRESSED_RGBA_ASTC_5x4_KHR;if(i===Ja)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_5x5_KHR:r.COMPRESSED_RGBA_ASTC_5x5_KHR;if(i===Ka)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_6x5_KHR:r.COMPRESSED_RGBA_ASTC_6x5_KHR;if(i===ja)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_6x6_KHR:r.COMPRESSED_RGBA_ASTC_6x6_KHR;if(i===Qa)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_8x5_KHR:r.COMPRESSED_RGBA_ASTC_8x5_KHR;if(i===eo)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_8x6_KHR:r.COMPRESSED_RGBA_ASTC_8x6_KHR;if(i===to)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_8x8_KHR:r.COMPRESSED_RGBA_ASTC_8x8_KHR;if(i===no)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_10x5_KHR:r.COMPRESSED_RGBA_ASTC_10x5_KHR;if(i===io)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_10x6_KHR:r.COMPRESSED_RGBA_ASTC_10x6_KHR;if(i===so)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_10x8_KHR:r.COMPRESSED_RGBA_ASTC_10x8_KHR;if(i===ro)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_10x10_KHR:r.COMPRESSED_RGBA_ASTC_10x10_KHR;if(i===ao)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_12x10_KHR:r.COMPRESSED_RGBA_ASTC_12x10_KHR;if(i===oo)return o===et?r.COMPRESSED_SRGB8_ALPHA8_ASTC_12x12_KHR:r.COMPRESSED_RGBA_ASTC_12x12_KHR}else return null;if(i===lo||i===co||i===ho)if(r=e.get("EXT_texture_compression_bptc"),r!==null){if(i===lo)return o===et?r.COMPRESSED_SRGB_ALPHA_BPTC_UNORM_EXT:r.COMPRESSED_RGBA_BPTC_UNORM_EXT;if(i===co)return r.COMPRESSED_RGB_BPTC_SIGNED_FLOAT_EXT;if(i===ho)return r.COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT_EXT}else return null;if(i===uo||i===fo||i===po||i===mo)if(r=e.get("EXT_texture_compression_rgtc"),r!==null){if(i===uo)return r.COMPRESSED_RED_RGTC1_EXT;if(i===fo)return r.COMPRESSED_SIGNED_RED_RGTC1_EXT;if(i===po)return r.COMPRESSED_RED_GREEN_RGTC2_EXT;if(i===mo)return r.COMPRESSED_SIGNED_RED_GREEN_RGTC2_EXT}else return null;return i===vs?n.UNSIGNED_INT_24_8:n[i]!==void 0?n[i]:null}return{convert:t}}var q0=`
void main() {

	gl_Position = vec4( position, 1.0 );

}`,$0=`
uniform sampler2DArray depthColor;
uniform float depthWidth;
uniform float depthHeight;

void main() {

	vec2 coord = vec2( gl_FragCoord.x / depthWidth, gl_FragCoord.y / depthHeight );

	if ( coord.x >= 1.0 ) {

		gl_FragDepth = texture( depthColor, vec3( coord.x - 1.0, coord.y, 1 ) ).r;

	} else {

		gl_FragDepth = texture( depthColor, vec3( coord.x, coord.y, 0 ) ).r;

	}

}`,ac=class{constructor(){this.texture=null,this.mesh=null,this.depthNear=0,this.depthFar=0}init(e,t){if(this.texture===null){let i=new Qs(e.texture);(e.depthNear!==t.depthNear||e.depthFar!==t.depthFar)&&(this.depthNear=e.depthNear,this.depthFar=e.depthFar),this.texture=i}}getMesh(e){if(this.texture!==null&&this.mesh===null){let t=e.cameras[0].viewport,i=new pn({vertexShader:q0,fragmentShader:$0,uniforms:{depthColor:{value:this.texture},depthWidth:{value:t.z},depthHeight:{value:t.w}}});this.mesh=new ft(new nr(20,20),i)}return this.mesh}reset(){this.texture=null,this.mesh=null}getDepthTexture(){return this.texture}},oc=class extends zn{constructor(e,t){super();let i=this,s=null,r=1,o=null,a="local-floor",c=1,l=null,h=null,u=null,p=null,d=null,g=null,v=typeof XRWebGLBinding<"u",m=new ac,f={},T=t.getContextAttributes(),E=null,S=null,R=[],A=[],P=new Ue,N=null,M=new Ot;M.viewport=new Qe;let y=new Ot;y.viewport=new Qe;let I=[M,y],O=new Sa,z=null,H=null;this.cameraAutoUpdate=!0,this.enabled=!1,this.isPresenting=!1,this.getController=function($){let K=R[$];return K===void 0&&(K=new ds,R[$]=K),K.getTargetRaySpace()},this.getControllerGrip=function($){let K=R[$];return K===void 0&&(K=new ds,R[$]=K),K.getGripSpace()},this.getHand=function($){let K=R[$];return K===void 0&&(K=new ds,R[$]=K),K.getHandSpace()};function q($){let K=A.indexOf($.inputSource);if(K===-1)return;let pe=R[K];pe!==void 0&&(pe.update($.inputSource,$.frame,l||o),pe.dispatchEvent({type:$.type,data:$.inputSource}))}function W(){s.removeEventListener("select",q),s.removeEventListener("selectstart",q),s.removeEventListener("selectend",q),s.removeEventListener("squeeze",q),s.removeEventListener("squeezestart",q),s.removeEventListener("squeezeend",q),s.removeEventListener("end",W),s.removeEventListener("inputsourceschange",ne);for(let $=0;$<R.length;$++){let K=A[$];K!==null&&(A[$]=null,R[$].disconnect(K))}z=null,H=null,m.reset();for(let $ in f)delete f[$];e.setRenderTarget(E),d=null,p=null,u=null,s=null,S=null,Je.stop(),i.isPresenting=!1,e.setPixelRatio(N),e.setSize(P.width,P.height,!1),i.dispatchEvent({type:"sessionend"})}this.setFramebufferScaleFactor=function($){r=$,i.isPresenting===!0&&console.warn("THREE.WebXRManager: Cannot change framebuffer scale while presenting.")},this.setReferenceSpaceType=function($){a=$,i.isPresenting===!0&&console.warn("THREE.WebXRManager: Cannot change reference space type while presenting.")},this.getReferenceSpace=function(){return l||o},this.setReferenceSpace=function($){l=$},this.getBaseLayer=function(){return p!==null?p:d},this.getBinding=function(){return u===null&&v&&(u=new XRWebGLBinding(s,t)),u},this.getFrame=function(){return g},this.getSession=function(){return s},this.setSession=async function($){if(s=$,s!==null){if(E=e.getRenderTarget(),s.addEventListener("select",q),s.addEventListener("selectstart",q),s.addEventListener("selectend",q),s.addEventListener("squeeze",q),s.addEventListener("squeezestart",q),s.addEventListener("squeezeend",q),s.addEventListener("end",W),s.addEventListener("inputsourceschange",ne),T.xrCompatible!==!0&&await t.makeXRCompatible(),N=e.getPixelRatio(),e.getSize(P),v&&"createProjectionLayer"in XRWebGLBinding.prototype){let pe=null,De=null,be=null;T.depth&&(be=T.stencil?t.DEPTH24_STENCIL8:t.DEPTH_COMPONENT24,pe=T.stencil?ys:os,De=T.stencil?vs:di);let $e={colorFormat:t.RGBA8,depthFormat:be,scaleFactor:r};u=this.getBinding(),p=u.createProjectionLayer($e),s.updateRenderState({layers:[p]}),e.setPixelRatio(1),e.setSize(p.textureWidth,p.textureHeight,!1),S=new Sn(p.textureWidth,p.textureHeight,{format:an,type:mn,depthTexture:new js(p.textureWidth,p.textureHeight,De,void 0,void 0,void 0,void 0,void 0,void 0,pe),stencilBuffer:T.stencil,colorSpace:e.outputColorSpace,samples:T.antialias?4:0,resolveDepthBuffer:p.ignoreDepthValues===!1,resolveStencilBuffer:p.ignoreDepthValues===!1})}else{let pe={antialias:T.antialias,alpha:!0,depth:T.depth,stencil:T.stencil,framebufferScaleFactor:r};d=new XRWebGLLayer(s,t,pe),s.updateRenderState({baseLayer:d}),e.setPixelRatio(1),e.setSize(d.framebufferWidth,d.framebufferHeight,!1),S=new Sn(d.framebufferWidth,d.framebufferHeight,{format:an,type:mn,colorSpace:e.outputColorSpace,stencilBuffer:T.stencil,resolveDepthBuffer:d.ignoreDepthValues===!1,resolveStencilBuffer:d.ignoreDepthValues===!1})}S.isXRRenderTarget=!0,this.setFoveation(c),l=null,o=await s.requestReferenceSpace(a),Je.setContext(s),Je.start(),i.isPresenting=!0,i.dispatchEvent({type:"sessionstart"})}},this.getEnvironmentBlendMode=function(){if(s!==null)return s.environmentBlendMode},this.getDepthTexture=function(){return m.getDepthTexture()};function ne($){for(let K=0;K<$.removed.length;K++){let pe=$.removed[K],De=A.indexOf(pe);De>=0&&(A[De]=null,R[De].disconnect(pe))}for(let K=0;K<$.added.length;K++){let pe=$.added[K],De=A.indexOf(pe);if(De===-1){for(let $e=0;$e<R.length;$e++)if($e>=A.length){A.push(pe),De=$e;break}else if(A[$e]===null){A[$e]=pe,De=$e;break}if(De===-1)break}let be=R[De];be&&be.connect(pe)}}let V=new L,ae=new L;function ue($,K,pe){V.setFromMatrixPosition(K.matrixWorld),ae.setFromMatrixPosition(pe.matrixWorld);let De=V.distanceTo(ae),be=K.projectionMatrix.elements,$e=pe.projectionMatrix.elements,Rt=be[14]/(be[10]-1),w=be[14]/(be[10]+1),ht=(be[9]+1)/be[5],Fe=(be[9]-1)/be[5],Pe=(be[8]-1)/be[0],_e=($e[8]+1)/$e[0],ut=Rt*Pe,xe=Rt*_e,Be=De/(-Pe+_e),bt=Be*-Pe;if(K.matrixWorld.decompose($.position,$.quaternion,$.scale),$.translateX(bt),$.translateZ(Be),$.matrixWorld.compose($.position,$.quaternion,$.scale),$.matrixWorldInverse.copy($.matrixWorld).invert(),be[10]===-1)$.projectionMatrix.copy(K.projectionMatrix),$.projectionMatrixInverse.copy(K.projectionMatrixInverse);else{let xt=Rt+Be,b=w+Be,_=ut-bt,F=xe+(De-bt),X=ht*w/b*xt,J=Fe*w/b*xt;$.projectionMatrix.makePerspective(_,F,X,J,xt,b),$.projectionMatrixInverse.copy($.projectionMatrix).invert()}}function Ee($,K){K===null?$.matrixWorld.copy($.matrix):$.matrixWorld.multiplyMatrices(K.matrixWorld,$.matrix),$.matrixWorldInverse.copy($.matrixWorld).invert()}this.updateCamera=function($){if(s===null)return;let K=$.near,pe=$.far;m.texture!==null&&(m.depthNear>0&&(K=m.depthNear),m.depthFar>0&&(pe=m.depthFar)),O.near=y.near=M.near=K,O.far=y.far=M.far=pe,(z!==O.near||H!==O.far)&&(s.updateRenderState({depthNear:O.near,depthFar:O.far}),z=O.near,H=O.far),O.layers.mask=$.layers.mask|6,M.layers.mask=O.layers.mask&3,y.layers.mask=O.layers.mask&5;let De=$.parent,be=O.cameras;Ee(O,De);for(let $e=0;$e<be.length;$e++)Ee(be[$e],De);be.length===2?ue(O,M,y):O.projectionMatrix.copy(M.projectionMatrix),Ve($,O,De)};function Ve($,K,pe){pe===null?$.matrix.copy(K.matrixWorld):($.matrix.copy(pe.matrixWorld),$.matrix.invert(),$.matrix.multiply(K.matrixWorld)),$.matrix.decompose($.position,$.quaternion,$.scale),$.updateMatrixWorld(!0),$.projectionMatrix.copy(K.projectionMatrix),$.projectionMatrixInverse.copy(K.projectionMatrixInverse),$.isPerspectiveCamera&&($.fov=ia*2*Math.atan(1/$.projectionMatrix.elements[5]),$.zoom=1)}this.getCamera=function(){return O},this.getFoveation=function(){if(!(p===null&&d===null))return c},this.setFoveation=function($){c=$,p!==null&&(p.fixedFoveation=$),d!==null&&d.fixedFoveation!==void 0&&(d.fixedFoveation=$)},this.hasDepthSensing=function(){return m.texture!==null},this.getDepthSensingMesh=function(){return m.getMesh(O)},this.getCameraTexture=function($){return f[$]};let st=null;function ct($,K){if(h=K.getViewerPose(l||o),g=K,h!==null){let pe=h.views;d!==null&&(e.setRenderTargetFramebuffer(S,d.framebuffer),e.setRenderTarget(S));let De=!1;pe.length!==O.cameras.length&&(O.cameras.length=0,De=!0);for(let w=0;w<pe.length;w++){let ht=pe[w],Fe=null;if(d!==null)Fe=d.getViewport(ht);else{let _e=u.getViewSubImage(p,ht);Fe=_e.viewport,w===0&&(e.setRenderTargetTextures(S,_e.colorTexture,_e.depthStencilTexture),e.setRenderTarget(S))}let Pe=I[w];Pe===void 0&&(Pe=new Ot,Pe.layers.enable(w),Pe.viewport=new Qe,I[w]=Pe),Pe.matrix.fromArray(ht.transform.matrix),Pe.matrix.decompose(Pe.position,Pe.quaternion,Pe.scale),Pe.projectionMatrix.fromArray(ht.projectionMatrix),Pe.projectionMatrixInverse.copy(Pe.projectionMatrix).invert(),Pe.viewport.set(Fe.x,Fe.y,Fe.width,Fe.height),w===0&&(O.matrix.copy(Pe.matrix),O.matrix.decompose(O.position,O.quaternion,O.scale)),De===!0&&O.cameras.push(Pe)}let be=s.enabledFeatures;if(be&&be.includes("depth-sensing")&&s.depthUsage=="gpu-optimized"&&v){u=i.getBinding();let w=u.getDepthInformation(pe[0]);w&&w.isValid&&w.texture&&m.init(w,s.renderState)}if(be&&be.includes("camera-access")&&v){e.state.unbindTexture(),u=i.getBinding();for(let w=0;w<pe.length;w++){let ht=pe[w].camera;if(ht){let Fe=f[ht];Fe||(Fe=new Qs,f[ht]=Fe);let Pe=u.getCameraImage(ht);Fe.sourceTexture=Pe}}}}for(let pe=0;pe<R.length;pe++){let De=A[pe],be=R[pe];De!==null&&be!==void 0&&be.update(De,K,l||o)}st&&st($,K),K.detectedPlanes&&i.dispatchEvent({type:"planesdetected",data:K}),g=null}let Je=new Zu;Je.setAnimationLoop(ct),this.setAnimationLoop=function($){st=$},this.dispose=function(){}}},Ni=new fn,Y0=new dt;function Z0(n,e){function t(m,f){m.matrixAutoUpdate===!0&&m.updateMatrix(),f.value.copy(m.matrix)}function i(m,f){f.color.getRGB(m.fogColor.value,Wl(n)),f.isFog?(m.fogNear.value=f.near,m.fogFar.value=f.far):f.isFogExp2&&(m.fogDensity.value=f.density)}function s(m,f,T,E,S){f.isMeshBasicMaterial||f.isMeshLambertMaterial?r(m,f):f.isMeshToonMaterial?(r(m,f),u(m,f)):f.isMeshPhongMaterial?(r(m,f),h(m,f)):f.isMeshStandardMaterial?(r(m,f),p(m,f),f.isMeshPhysicalMaterial&&d(m,f,S)):f.isMeshMatcapMaterial?(r(m,f),g(m,f)):f.isMeshDepthMaterial?r(m,f):f.isMeshDistanceMaterial?(r(m,f),v(m,f)):f.isMeshNormalMaterial?r(m,f):f.isLineBasicMaterial?(o(m,f),f.isLineDashedMaterial&&a(m,f)):f.isPointsMaterial?c(m,f,T,E):f.isSpriteMaterial?l(m,f):f.isShadowMaterial?(m.color.value.copy(f.color),m.opacity.value=f.opacity):f.isShaderMaterial&&(f.uniformsNeedUpdate=!1)}function r(m,f){m.opacity.value=f.opacity,f.color&&m.diffuse.value.copy(f.color),f.emissive&&m.emissive.value.copy(f.emissive).multiplyScalar(f.emissiveIntensity),f.map&&(m.map.value=f.map,t(f.map,m.mapTransform)),f.alphaMap&&(m.alphaMap.value=f.alphaMap,t(f.alphaMap,m.alphaMapTransform)),f.bumpMap&&(m.bumpMap.value=f.bumpMap,t(f.bumpMap,m.bumpMapTransform),m.bumpScale.value=f.bumpScale,f.side===Bt&&(m.bumpScale.value*=-1)),f.normalMap&&(m.normalMap.value=f.normalMap,t(f.normalMap,m.normalMapTransform),m.normalScale.value.copy(f.normalScale),f.side===Bt&&m.normalScale.value.negate()),f.displacementMap&&(m.displacementMap.value=f.displacementMap,t(f.displacementMap,m.displacementMapTransform),m.displacementScale.value=f.displacementScale,m.displacementBias.value=f.displacementBias),f.emissiveMap&&(m.emissiveMap.value=f.emissiveMap,t(f.emissiveMap,m.emissiveMapTransform)),f.specularMap&&(m.specularMap.value=f.specularMap,t(f.specularMap,m.specularMapTransform)),f.alphaTest>0&&(m.alphaTest.value=f.alphaTest);let T=e.get(f),E=T.envMap,S=T.envMapRotation;E&&(m.envMap.value=E,Ni.copy(S),Ni.x*=-1,Ni.y*=-1,Ni.z*=-1,E.isCubeTexture&&E.isRenderTargetTexture===!1&&(Ni.y*=-1,Ni.z*=-1),m.envMapRotation.value.setFromMatrix4(Y0.makeRotationFromEuler(Ni)),m.flipEnvMap.value=E.isCubeTexture&&E.isRenderTargetTexture===!1?-1:1,m.reflectivity.value=f.reflectivity,m.ior.value=f.ior,m.refractionRatio.value=f.refractionRatio),f.lightMap&&(m.lightMap.value=f.lightMap,m.lightMapIntensity.value=f.lightMapIntensity,t(f.lightMap,m.lightMapTransform)),f.aoMap&&(m.aoMap.value=f.aoMap,m.aoMapIntensity.value=f.aoMapIntensity,t(f.aoMap,m.aoMapTransform))}function o(m,f){m.diffuse.value.copy(f.color),m.opacity.value=f.opacity,f.map&&(m.map.value=f.map,t(f.map,m.mapTransform))}function a(m,f){m.dashSize.value=f.dashSize,m.totalSize.value=f.dashSize+f.gapSize,m.scale.value=f.scale}function c(m,f,T,E){m.diffuse.value.copy(f.color),m.opacity.value=f.opacity,m.size.value=f.size*T,m.scale.value=E*.5,f.map&&(m.map.value=f.map,t(f.map,m.uvTransform)),f.alphaMap&&(m.alphaMap.value=f.alphaMap,t(f.alphaMap,m.alphaMapTransform)),f.alphaTest>0&&(m.alphaTest.value=f.alphaTest)}function l(m,f){m.diffuse.value.copy(f.color),m.opacity.value=f.opacity,m.rotation.value=f.rotation,f.map&&(m.map.value=f.map,t(f.map,m.mapTransform)),f.alphaMap&&(m.alphaMap.value=f.alphaMap,t(f.alphaMap,m.alphaMapTransform)),f.alphaTest>0&&(m.alphaTest.value=f.alphaTest)}function h(m,f){m.specular.value.copy(f.specular),m.shininess.value=Math.max(f.shininess,1e-4)}function u(m,f){f.gradientMap&&(m.gradientMap.value=f.gradientMap)}function p(m,f){m.metalness.value=f.metalness,f.metalnessMap&&(m.metalnessMap.value=f.metalnessMap,t(f.metalnessMap,m.metalnessMapTransform)),m.roughness.value=f.roughness,f.roughnessMap&&(m.roughnessMap.value=f.roughnessMap,t(f.roughnessMap,m.roughnessMapTransform)),f.envMap&&(m.envMapIntensity.value=f.envMapIntensity)}function d(m,f,T){m.ior.value=f.ior,f.sheen>0&&(m.sheenColor.value.copy(f.sheenColor).multiplyScalar(f.sheen),m.sheenRoughness.value=f.sheenRoughness,f.sheenColorMap&&(m.sheenColorMap.value=f.sheenColorMap,t(f.sheenColorMap,m.sheenColorMapTransform)),f.sheenRoughnessMap&&(m.sheenRoughnessMap.value=f.sheenRoughnessMap,t(f.sheenRoughnessMap,m.sheenRoughnessMapTransform))),f.clearcoat>0&&(m.clearcoat.value=f.clearcoat,m.clearcoatRoughness.value=f.clearcoatRoughness,f.clearcoatMap&&(m.clearcoatMap.value=f.clearcoatMap,t(f.clearcoatMap,m.clearcoatMapTransform)),f.clearcoatRoughnessMap&&(m.clearcoatRoughnessMap.value=f.clearcoatRoughnessMap,t(f.clearcoatRoughnessMap,m.clearcoatRoughnessMapTransform)),f.clearcoatNormalMap&&(m.clearcoatNormalMap.value=f.clearcoatNormalMap,t(f.clearcoatNormalMap,m.clearcoatNormalMapTransform),m.clearcoatNormalScale.value.copy(f.clearcoatNormalScale),f.side===Bt&&m.clearcoatNormalScale.value.negate())),f.dispersion>0&&(m.dispersion.value=f.dispersion),f.iridescence>0&&(m.iridescence.value=f.iridescence,m.iridescenceIOR.value=f.iridescenceIOR,m.iridescenceThicknessMinimum.value=f.iridescenceThicknessRange[0],m.iridescenceThicknessMaximum.value=f.iridescenceThicknessRange[1],f.iridescenceMap&&(m.iridescenceMap.value=f.iridescenceMap,t(f.iridescenceMap,m.iridescenceMapTransform)),f.iridescenceThicknessMap&&(m.iridescenceThicknessMap.value=f.iridescenceThicknessMap,t(f.iridescenceThicknessMap,m.iridescenceThicknessMapTransform))),f.transmission>0&&(m.transmission.value=f.transmission,m.transmissionSamplerMap.value=T.texture,m.transmissionSamplerSize.value.set(T.width,T.height),f.transmissionMap&&(m.transmissionMap.value=f.transmissionMap,t(f.transmissionMap,m.transmissionMapTransform)),m.thickness.value=f.thickness,f.thicknessMap&&(m.thicknessMap.value=f.thicknessMap,t(f.thicknessMap,m.thicknessMapTransform)),m.attenuationDistance.value=f.attenuationDistance,m.attenuationColor.value.copy(f.attenuationColor)),f.anisotropy>0&&(m.anisotropyVector.value.set(f.anisotropy*Math.cos(f.anisotropyRotation),f.anisotropy*Math.sin(f.anisotropyRotation)),f.anisotropyMap&&(m.anisotropyMap.value=f.anisotropyMap,t(f.anisotropyMap,m.anisotropyMapTransform))),m.specularIntensity.value=f.specularIntensity,m.specularColor.value.copy(f.specularColor),f.specularColorMap&&(m.specularColorMap.value=f.specularColorMap,t(f.specularColorMap,m.specularColorMapTransform)),f.specularIntensityMap&&(m.specularIntensityMap.value=f.specularIntensityMap,t(f.specularIntensityMap,m.specularIntensityMapTransform))}function g(m,f){f.matcap&&(m.matcap.value=f.matcap)}function v(m,f){let T=e.get(f).light;m.referencePosition.value.setFromMatrixPosition(T.matrixWorld),m.nearDistance.value=T.shadow.camera.near,m.farDistance.value=T.shadow.camera.far}return{refreshFogUniforms:i,refreshMaterialUniforms:s}}function J0(n,e,t,i){let s={},r={},o=[],a=n.getParameter(n.MAX_UNIFORM_BUFFER_BINDINGS);function c(T,E){let S=E.program;i.uniformBlockBinding(T,S)}function l(T,E){let S=s[T.id];S===void 0&&(g(T),S=h(T),s[T.id]=S,T.addEventListener("dispose",m));let R=E.program;i.updateUBOMapping(T,R);let A=e.render.frame;r[T.id]!==A&&(p(T),r[T.id]=A)}function h(T){let E=u();T.__bindingPointIndex=E;let S=n.createBuffer(),R=T.__size,A=T.usage;return n.bindBuffer(n.UNIFORM_BUFFER,S),n.bufferData(n.UNIFORM_BUFFER,R,A),n.bindBuffer(n.UNIFORM_BUFFER,null),n.bindBufferBase(n.UNIFORM_BUFFER,E,S),S}function u(){for(let T=0;T<a;T++)if(o.indexOf(T)===-1)return o.push(T),T;return console.error("THREE.WebGLRenderer: Maximum number of simultaneously usable uniforms groups reached."),0}function p(T){let E=s[T.id],S=T.uniforms,R=T.__cache;n.bindBuffer(n.UNIFORM_BUFFER,E);for(let A=0,P=S.length;A<P;A++){let N=Array.isArray(S[A])?S[A]:[S[A]];for(let M=0,y=N.length;M<y;M++){let I=N[M];if(d(I,A,M,R)===!0){let O=I.__offset,z=Array.isArray(I.value)?I.value:[I.value],H=0;for(let q=0;q<z.length;q++){let W=z[q],ne=v(W);typeof W=="number"||typeof W=="boolean"?(I.__data[0]=W,n.bufferSubData(n.UNIFORM_BUFFER,O+H,I.__data)):W.isMatrix3?(I.__data[0]=W.elements[0],I.__data[1]=W.elements[1],I.__data[2]=W.elements[2],I.__data[3]=0,I.__data[4]=W.elements[3],I.__data[5]=W.elements[4],I.__data[6]=W.elements[5],I.__data[7]=0,I.__data[8]=W.elements[6],I.__data[9]=W.elements[7],I.__data[10]=W.elements[8],I.__data[11]=0):(W.toArray(I.__data,H),H+=ne.storage/Float32Array.BYTES_PER_ELEMENT)}n.bufferSubData(n.UNIFORM_BUFFER,O,I.__data)}}}n.bindBuffer(n.UNIFORM_BUFFER,null)}function d(T,E,S,R){let A=T.value,P=E+"_"+S;if(R[P]===void 0)return typeof A=="number"||typeof A=="boolean"?R[P]=A:R[P]=A.clone(),!0;{let N=R[P];if(typeof A=="number"||typeof A=="boolean"){if(N!==A)return R[P]=A,!0}else if(N.equals(A)===!1)return N.copy(A),!0}return!1}function g(T){let E=T.uniforms,S=0,R=16;for(let P=0,N=E.length;P<N;P++){let M=Array.isArray(E[P])?E[P]:[E[P]];for(let y=0,I=M.length;y<I;y++){let O=M[y],z=Array.isArray(O.value)?O.value:[O.value];for(let H=0,q=z.length;H<q;H++){let W=z[H],ne=v(W),V=S%R,ae=V%ne.boundary,ue=V+ae;S+=ae,ue!==0&&R-ue<ne.storage&&(S+=R-ue),O.__data=new Float32Array(ne.storage/Float32Array.BYTES_PER_ELEMENT),O.__offset=S,S+=ne.storage}}}let A=S%R;return A>0&&(S+=R-A),T.__size=S,T.__cache={},this}function v(T){let E={boundary:0,storage:0};return typeof T=="number"||typeof T=="boolean"?(E.boundary=4,E.storage=4):T.isVector2?(E.boundary=8,E.storage=8):T.isVector3||T.isColor?(E.boundary=16,E.storage=12):T.isVector4?(E.boundary=16,E.storage=16):T.isMatrix3?(E.boundary=48,E.storage=48):T.isMatrix4?(E.boundary=64,E.storage=64):T.isTexture?console.warn("THREE.WebGLRenderer: Texture samplers can not be part of an uniforms group."):console.warn("THREE.WebGLRenderer: Unsupported uniform value type.",T),E}function m(T){let E=T.target;E.removeEventListener("dispose",m);let S=o.indexOf(E.__bindingPointIndex);o.splice(S,1),n.deleteBuffer(s[E.id]),delete s[E.id],delete r[E.id]}function f(){for(let T in s)n.deleteBuffer(s[T]);o=[],s={},r={}}return{bind:c,update:l,dispose:f}}var yo=class{constructor(e={}){let{canvas:t=yu(),context:i=null,depth:s=!0,stencil:r=!1,alpha:o=!1,antialias:a=!1,premultipliedAlpha:c=!0,preserveDrawingBuffer:l=!1,powerPreference:h="default",failIfMajorPerformanceCaveat:u=!1,reversedDepthBuffer:p=!1}=e;this.isWebGLRenderer=!0;let d;if(i!==null){if(typeof WebGLRenderingContext<"u"&&i instanceof WebGLRenderingContext)throw new Error("THREE.WebGLRenderer: WebGL 1 is not supported since r163.");d=i.getContextAttributes().alpha}else d=o;let g=new Uint32Array(4),v=new Int32Array(4),m=null,f=null,T=[],E=[];this.domElement=t,this.debug={checkShaderErrors:!0,onShaderError:null},this.autoClear=!0,this.autoClearColor=!0,this.autoClearDepth=!0,this.autoClearStencil=!0,this.sortObjects=!0,this.clippingPlanes=[],this.localClippingEnabled=!1,this.toneMapping=Wn,this.toneMappingExposure=1,this.transmissionResolutionScale=1;let S=this,R=!1;this._outputColorSpace=Ft;let A=0,P=0,N=null,M=-1,y=null,I=new Qe,O=new Qe,z=null,H=new Xe(0),q=0,W=t.width,ne=t.height,V=1,ae=null,ue=null,Ee=new Qe(0,0,W,ne),Ve=new Qe(0,0,W,ne),st=!1,ct=new ps,Je=!1,$=!1,K=new dt,pe=new L,De=new Qe,be={background:null,fog:null,environment:null,overrideMaterial:null,isScene:!0},$e=!1;function Rt(){return N===null?V:1}let w=i;function ht(x,D){return t.getContext(x,D)}try{let x={alpha:!0,depth:s,stencil:r,antialias:a,premultipliedAlpha:c,preserveDrawingBuffer:l,powerPreference:h,failIfMajorPerformanceCaveat:u};if("setAttribute"in t&&t.setAttribute("data-engine",`three.js r${"180"}`),t.addEventListener("webglcontextlost",se,!1),t.addEventListener("webglcontextrestored",fe,!1),t.addEventListener("webglcontextcreationerror",j,!1),w===null){let D="webgl2";if(w=ht(D,x),w===null)throw ht(D)?new Error("Error creating WebGL context with your selected attributes."):new Error("Error creating WebGL context.")}}catch(x){throw console.error("THREE.WebGLRenderer: "+x.message),x}let Fe,Pe,_e,ut,xe,Be,bt,xt,b,_,F,X,J,G,Se,ie,ve,ye,ee,he,Ie,Me,oe,ke;function C(){Fe=new pg(w),Fe.init(),Me=new X0(w,Fe),Pe=new og(w,Fe,e,Me),_e=new H0(w,Fe),Pe.reversedDepthBuffer&&p&&_e.buffers.depth.setReversed(!0),ut=new _g(w),xe=new I0,Be=new W0(w,Fe,_e,xe,Pe,Me,ut),bt=new cg(S),xt=new fg(S),b=new bf(w),oe=new rg(w,b),_=new mg(w,b,ut,oe),F=new vg(w,_,b,ut),ee=new xg(w,Pe,Be),ie=new lg(xe),X=new C0(S,bt,xt,Fe,Pe,oe,ie),J=new Z0(S,xe),G=new L0,Se=new k0(Fe),ye=new sg(S,bt,xt,_e,F,d,c),ve=new V0(S,F,Pe),ke=new J0(w,ut,Pe,_e),he=new ag(w,Fe,ut),Ie=new gg(w,Fe,ut),ut.programs=X.programs,S.capabilities=Pe,S.extensions=Fe,S.properties=xe,S.renderLists=G,S.shadowMap=ve,S.state=_e,S.info=ut}C();let te=new oc(S,w);this.xr=te,this.getContext=function(){return w},this.getContextAttributes=function(){return w.getContextAttributes()},this.forceContextLoss=function(){let x=Fe.get("WEBGL_lose_context");x&&x.loseContext()},this.forceContextRestore=function(){let x=Fe.get("WEBGL_lose_context");x&&x.restoreContext()},this.getPixelRatio=function(){return V},this.setPixelRatio=function(x){x!==void 0&&(V=x,this.setSize(W,ne,!1))},this.getSize=function(x){return x.set(W,ne)},this.setSize=function(x,D,k=!0){if(te.isPresenting){console.warn("THREE.WebGLRenderer: Can't change size while VR device is presenting.");return}W=x,ne=D,t.width=Math.floor(x*V),t.height=Math.floor(D*V),k===!0&&(t.style.width=x+"px",t.style.height=D+"px"),this.setViewport(0,0,x,D)},this.getDrawingBufferSize=function(x){return x.set(W*V,ne*V).floor()},this.setDrawingBufferSize=function(x,D,k){W=x,ne=D,V=k,t.width=Math.floor(x*k),t.height=Math.floor(D*k),this.setViewport(0,0,x,D)},this.getCurrentViewport=function(x){return x.copy(I)},this.getViewport=function(x){return x.copy(Ee)},this.setViewport=function(x,D,k,B){x.isVector4?Ee.set(x.x,x.y,x.z,x.w):Ee.set(x,D,k,B),_e.viewport(I.copy(Ee).multiplyScalar(V).round())},this.getScissor=function(x){return x.copy(Ve)},this.setScissor=function(x,D,k,B){x.isVector4?Ve.set(x.x,x.y,x.z,x.w):Ve.set(x,D,k,B),_e.scissor(O.copy(Ve).multiplyScalar(V).round())},this.getScissorTest=function(){return st},this.setScissorTest=function(x){_e.setScissorTest(st=x)},this.setOpaqueSort=function(x){ae=x},this.setTransparentSort=function(x){ue=x},this.getClearColor=function(x){return x.copy(ye.getClearColor())},this.setClearColor=function(){ye.setClearColor(...arguments)},this.getClearAlpha=function(){return ye.getClearAlpha()},this.setClearAlpha=function(){ye.setClearAlpha(...arguments)},this.clear=function(x=!0,D=!0,k=!0){let B=0;if(x){let U=!1;if(N!==null){let Q=N.texture.format;U=Q===za||Q===Ba||Q===ka}if(U){let Q=N.texture.type,le=Q===mn||Q===di||Q===_s||Q===vs||Q===Fa||Q===Oa,me=ye.getClearColor(),de=ye.getClearAlpha(),Ce=me.r,Le=me.g,Te=me.b;le?(g[0]=Ce,g[1]=Le,g[2]=Te,g[3]=de,w.clearBufferuiv(w.COLOR,0,g)):(v[0]=Ce,v[1]=Le,v[2]=Te,v[3]=de,w.clearBufferiv(w.COLOR,0,v))}else B|=w.COLOR_BUFFER_BIT}D&&(B|=w.DEPTH_BUFFER_BIT),k&&(B|=w.STENCIL_BUFFER_BIT,this.state.buffers.stencil.setMask(4294967295)),w.clear(B)},this.clearColor=function(){this.clear(!0,!1,!1)},this.clearDepth=function(){this.clear(!1,!0,!1)},this.clearStencil=function(){this.clear(!1,!1,!0)},this.dispose=function(){t.removeEventListener("webglcontextlost",se,!1),t.removeEventListener("webglcontextrestored",fe,!1),t.removeEventListener("webglcontextcreationerror",j,!1),ye.dispose(),G.dispose(),Se.dispose(),xe.dispose(),bt.dispose(),xt.dispose(),F.dispose(),oe.dispose(),ke.dispose(),X.dispose(),te.dispose(),te.removeEventListener("sessionstart",_n),te.removeEventListener("sessionend",Tc),mi.stop()};function se(x){x.preventDefault(),console.log("THREE.WebGLRenderer: Context Lost."),R=!0}function fe(){console.log("THREE.WebGLRenderer: Context Restored."),R=!1;let x=ut.autoReset,D=ve.enabled,k=ve.autoUpdate,B=ve.needsUpdate,U=ve.type;C(),ut.autoReset=x,ve.enabled=D,ve.autoUpdate=k,ve.needsUpdate=B,ve.type=U}function j(x){console.error("THREE.WebGLRenderer: A WebGL context could not be created. Reason: ",x.statusMessage)}function Y(x){let D=x.target;D.removeEventListener("dispose",Y),ge(D)}function ge(x){Ne(x),xe.remove(x)}function Ne(x){let D=xe.get(x).programs;D!==void 0&&(D.forEach(function(k){X.releaseProgram(k)}),x.isShaderMaterial&&X.releaseShaderCache(x))}this.renderBufferDirect=function(x,D,k,B,U,Q){D===null&&(D=be);let le=U.isMesh&&U.matrixWorld.determinant()<0,me=Id(x,D,k,B,U);_e.setMaterial(B,le);let de=k.index,Ce=1;if(B.wireframe===!0){if(de=_.getWireframeAttribute(k),de===void 0)return;Ce=2}let Le=k.drawRange,Te=k.attributes.position,He=Le.start*Ce,tt=(Le.start+Le.count)*Ce;Q!==null&&(He=Math.max(He,Q.start*Ce),tt=Math.min(tt,(Q.start+Q.count)*Ce)),de!==null?(He=Math.max(He,0),tt=Math.min(tt,de.count)):Te!=null&&(He=Math.max(He,0),tt=Math.min(tt,Te.count));let gt=tt-He;if(gt<0||gt===1/0)return;oe.setup(U,B,me,k,de);let at,it=he;if(de!==null&&(at=b.get(de),it=Ie,it.setIndex(at)),U.isMesh)B.wireframe===!0?(_e.setLineWidth(B.wireframeLinewidth*Rt()),it.setMode(w.LINES)):it.setMode(w.TRIANGLES);else if(U.isLine){let we=B.linewidth;we===void 0&&(we=1),_e.setLineWidth(we*Rt()),U.isLineSegments?it.setMode(w.LINES):U.isLineLoop?it.setMode(w.LINE_LOOP):it.setMode(w.LINE_STRIP)}else U.isPoints?it.setMode(w.POINTS):U.isSprite&&it.setMode(w.TRIANGLES);if(U.isBatchedMesh)if(U._multiDrawInstances!==null)ls("THREE.WebGLRenderer: renderMultiDrawInstances has been deprecated and will be removed in r184. Append to renderMultiDraw arguments and use indirection."),it.renderMultiDrawInstances(U._multiDrawStarts,U._multiDrawCounts,U._multiDrawCount,U._multiDrawInstances);else if(Fe.get("WEBGL_multi_draw"))it.renderMultiDraw(U._multiDrawStarts,U._multiDrawCounts,U._multiDrawCount);else{let we=U._multiDrawStarts,pt=U._multiDrawCounts,Ze=U._multiDrawCount,Zt=de?b.get(de).bytesPerElement:1,ki=xe.get(B).currentProgram.getUniforms();for(let Jt=0;Jt<Ze;Jt++)ki.setValue(w,"_gl_DrawID",Jt),it.render(we[Jt]/Zt,pt[Jt])}else if(U.isInstancedMesh)it.renderInstances(He,gt,U.count);else if(k.isInstancedBufferGeometry){let we=k._maxInstanceCount!==void 0?k._maxInstanceCount:1/0,pt=Math.min(k.instanceCount,we);it.renderInstances(He,gt,pt)}else it.render(He,gt)};function rt(x,D,k){x.transparent===!0&&x.side===qt&&x.forceSinglePass===!1?(x.side=Bt,x.needsUpdate=!0,vr(x,D,k),x.side=Bn,x.needsUpdate=!0,vr(x,D,k),x.side=qt):vr(x,D,k)}this.compile=function(x,D,k=null){k===null&&(k=x),f=Se.get(k),f.init(D),E.push(f),k.traverseVisible(function(U){U.isLight&&U.layers.test(D.layers)&&(f.pushLight(U),U.castShadow&&f.pushShadow(U))}),x!==k&&x.traverseVisible(function(U){U.isLight&&U.layers.test(D.layers)&&(f.pushLight(U),U.castShadow&&f.pushShadow(U))}),f.setupLights();let B=new Set;return x.traverse(function(U){if(!(U.isMesh||U.isPoints||U.isLine||U.isSprite))return;let Q=U.material;if(Q)if(Array.isArray(Q))for(let le=0;le<Q.length;le++){let me=Q[le];rt(me,k,U),B.add(me)}else rt(Q,k,U),B.add(Q)}),f=E.pop(),B},this.compileAsync=function(x,D,k=null){let B=this.compile(x,D,k);return new Promise(U=>{function Q(){if(B.forEach(function(le){xe.get(le).currentProgram.isReady()&&B.delete(le)}),B.size===0){U(x);return}setTimeout(Q,10)}Fe.get("KHR_parallel_shader_compile")!==null?Q():setTimeout(Q,10)})};let Ke=null;function In(x){Ke&&Ke(x)}function _n(){mi.stop()}function Tc(){mi.start()}let mi=new Zu;mi.setAnimationLoop(In),typeof self<"u"&&mi.setContext(self),this.setAnimationLoop=function(x){Ke=x,te.setAnimationLoop(x),x===null?mi.stop():mi.start()},te.addEventListener("sessionstart",_n),te.addEventListener("sessionend",Tc),this.render=function(x,D){if(D!==void 0&&D.isCamera!==!0){console.error("THREE.WebGLRenderer.render: camera is not an instance of THREE.Camera.");return}if(R===!0)return;if(x.matrixWorldAutoUpdate===!0&&x.updateMatrixWorld(),D.parent===null&&D.matrixWorldAutoUpdate===!0&&D.updateMatrixWorld(),te.enabled===!0&&te.isPresenting===!0&&(te.cameraAutoUpdate===!0&&te.updateCamera(D),D=te.getCamera()),x.isScene===!0&&x.onBeforeRender(S,x,D,N),f=Se.get(x,E.length),f.init(D),E.push(f),K.multiplyMatrices(D.projectionMatrix,D.matrixWorldInverse),ct.setFromProjectionMatrix(K,un,D.reversedDepth),$=this.localClippingEnabled,Je=ie.init(this.clippingPlanes,$),m=G.get(x,T.length),m.init(),T.push(m),te.enabled===!0&&te.isPresenting===!0){let Q=S.xr.getDepthSensingMesh();Q!==null&&Co(Q,D,-1/0,S.sortObjects)}Co(x,D,0,S.sortObjects),m.finish(),S.sortObjects===!0&&m.sort(ae,ue),$e=te.enabled===!1||te.isPresenting===!1||te.hasDepthSensing()===!1,$e&&ye.addToRenderList(m,x),this.info.render.frame++,Je===!0&&ie.beginShadows();let k=f.state.shadowsArray;ve.render(k,x,D),Je===!0&&ie.endShadows(),this.info.autoReset===!0&&this.info.reset();let B=m.opaque,U=m.transmissive;if(f.setupLights(),D.isArrayCamera){let Q=D.cameras;if(U.length>0)for(let le=0,me=Q.length;le<me;le++){let de=Q[le];Ac(B,U,x,de)}$e&&ye.render(x);for(let le=0,me=Q.length;le<me;le++){let de=Q[le];wc(m,x,de,de.viewport)}}else U.length>0&&Ac(B,U,x,D),$e&&ye.render(x),wc(m,x,D);N!==null&&P===0&&(Be.updateMultisampleRenderTarget(N),Be.updateRenderTargetMipmap(N)),x.isScene===!0&&x.onAfterRender(S,x,D),oe.resetDefaultState(),M=-1,y=null,E.pop(),E.length>0?(f=E[E.length-1],Je===!0&&ie.setGlobalState(S.clippingPlanes,f.state.camera)):f=null,T.pop(),T.length>0?m=T[T.length-1]:m=null};function Co(x,D,k,B){if(x.visible===!1)return;if(x.layers.test(D.layers)){if(x.isGroup)k=x.renderOrder;else if(x.isLOD)x.autoUpdate===!0&&x.update(D);else if(x.isLight)f.pushLight(x),x.castShadow&&f.pushShadow(x);else if(x.isSprite){if(!x.frustumCulled||ct.intersectsSprite(x)){B&&De.setFromMatrixPosition(x.matrixWorld).applyMatrix4(K);let le=F.update(x),me=x.material;me.visible&&m.push(x,le,me,k,De.z,null)}}else if((x.isMesh||x.isLine||x.isPoints)&&(!x.frustumCulled||ct.intersectsObject(x))){let le=F.update(x),me=x.material;if(B&&(x.boundingSphere!==void 0?(x.boundingSphere===null&&x.computeBoundingSphere(),De.copy(x.boundingSphere.center)):(le.boundingSphere===null&&le.computeBoundingSphere(),De.copy(le.boundingSphere.center)),De.applyMatrix4(x.matrixWorld).applyMatrix4(K)),Array.isArray(me)){let de=le.groups;for(let Ce=0,Le=de.length;Ce<Le;Ce++){let Te=de[Ce],He=me[Te.materialIndex];He&&He.visible&&m.push(x,le,He,k,De.z,Te)}}else me.visible&&m.push(x,le,me,k,De.z,null)}}let Q=x.children;for(let le=0,me=Q.length;le<me;le++)Co(Q[le],D,k,B)}function wc(x,D,k,B){let U=x.opaque,Q=x.transmissive,le=x.transparent;f.setupLightsView(k),Je===!0&&ie.setGlobalState(S.clippingPlanes,k),B&&_e.viewport(I.copy(B)),U.length>0&&xr(U,D,k),Q.length>0&&xr(Q,D,k),le.length>0&&xr(le,D,k),_e.buffers.depth.setTest(!0),_e.buffers.depth.setMask(!0),_e.buffers.color.setMask(!0),_e.setPolygonOffset(!1)}function Ac(x,D,k,B){if((k.isScene===!0?k.overrideMaterial:null)!==null)return;f.state.transmissionRenderTarget[B.id]===void 0&&(f.state.transmissionRenderTarget[B.id]=new Sn(1,1,{generateMipmaps:!0,type:Fe.has("EXT_color_buffer_half_float")||Fe.has("EXT_color_buffer_float")?xs:mn,minFilter:ui,samples:4,stencilBuffer:r,resolveDepthBuffer:!1,resolveStencilBuffer:!1,colorSpace:Ye.workingColorSpace}));let Q=f.state.transmissionRenderTarget[B.id],le=B.viewport||I;Q.setSize(le.z*S.transmissionResolutionScale,le.w*S.transmissionResolutionScale);let me=S.getRenderTarget(),de=S.getActiveCubeFace(),Ce=S.getActiveMipmapLevel();S.setRenderTarget(Q),S.getClearColor(H),q=S.getClearAlpha(),q<1&&S.setClearColor(16777215,.5),S.clear(),$e&&ye.render(k);let Le=S.toneMapping;S.toneMapping=Wn;let Te=B.viewport;if(B.viewport!==void 0&&(B.viewport=void 0),f.setupLightsView(B),Je===!0&&ie.setGlobalState(S.clippingPlanes,B),xr(x,k,B),Be.updateMultisampleRenderTarget(Q),Be.updateRenderTargetMipmap(Q),Fe.has("WEBGL_multisampled_render_to_texture")===!1){let He=!1;for(let tt=0,gt=D.length;tt<gt;tt++){let at=D[tt],it=at.object,we=at.geometry,pt=at.material,Ze=at.group;if(pt.side===qt&&it.layers.test(B.layers)){let Zt=pt.side;pt.side=Bt,pt.needsUpdate=!0,Rc(it,k,B,we,pt,Ze),pt.side=Zt,pt.needsUpdate=!0,He=!0}}He===!0&&(Be.updateMultisampleRenderTarget(Q),Be.updateRenderTargetMipmap(Q))}S.setRenderTarget(me,de,Ce),S.setClearColor(H,q),Te!==void 0&&(B.viewport=Te),S.toneMapping=Le}function xr(x,D,k){let B=D.isScene===!0?D.overrideMaterial:null;for(let U=0,Q=x.length;U<Q;U++){let le=x[U],me=le.object,de=le.geometry,Ce=le.group,Le=le.material;Le.allowOverride===!0&&B!==null&&(Le=B),me.layers.test(k.layers)&&Rc(me,D,k,de,Le,Ce)}}function Rc(x,D,k,B,U,Q){x.onBeforeRender(S,D,k,B,U,Q),x.modelViewMatrix.multiplyMatrices(k.matrixWorldInverse,x.matrixWorld),x.normalMatrix.getNormalMatrix(x.modelViewMatrix),U.onBeforeRender(S,D,k,B,x,Q),U.transparent===!0&&U.side===qt&&U.forceSinglePass===!1?(U.side=Bt,U.needsUpdate=!0,S.renderBufferDirect(k,D,B,U,x,Q),U.side=Bn,U.needsUpdate=!0,S.renderBufferDirect(k,D,B,U,x,Q),U.side=qt):S.renderBufferDirect(k,D,B,U,x,Q),x.onAfterRender(S,D,k,B,U,Q)}function vr(x,D,k){D.isScene!==!0&&(D=be);let B=xe.get(x),U=f.state.lights,Q=f.state.shadowsArray,le=U.state.version,me=X.getParameters(x,U.state,Q,D,k),de=X.getProgramCacheKey(me),Ce=B.programs;B.environment=x.isMeshStandardMaterial?D.environment:null,B.fog=D.fog,B.envMap=(x.isMeshStandardMaterial?xt:bt).get(x.envMap||B.environment),B.envMapRotation=B.environment!==null&&x.envMap===null?D.environmentRotation:x.envMapRotation,Ce===void 0&&(x.addEventListener("dispose",Y),Ce=new Map,B.programs=Ce);let Le=Ce.get(de);if(Le!==void 0){if(B.currentProgram===Le&&B.lightsStateVersion===le)return Ic(x,me),Le}else me.uniforms=X.getUniforms(x),x.onBeforeCompile(me,S),Le=X.acquireProgram(me,de),Ce.set(de,Le),B.uniforms=me.uniforms;let Te=B.uniforms;return(!x.isShaderMaterial&&!x.isRawShaderMaterial||x.clipping===!0)&&(Te.clippingPlanes=ie.uniform),Ic(x,me),B.needsLights=Ld(x),B.lightsStateVersion=le,B.needsLights&&(Te.ambientLightColor.value=U.state.ambient,Te.lightProbe.value=U.state.probe,Te.directionalLights.value=U.state.directional,Te.directionalLightShadows.value=U.state.directionalShadow,Te.spotLights.value=U.state.spot,Te.spotLightShadows.value=U.state.spotShadow,Te.rectAreaLights.value=U.state.rectArea,Te.ltc_1.value=U.state.rectAreaLTC1,Te.ltc_2.value=U.state.rectAreaLTC2,Te.pointLights.value=U.state.point,Te.pointLightShadows.value=U.state.pointShadow,Te.hemisphereLights.value=U.state.hemi,Te.directionalShadowMap.value=U.state.directionalShadowMap,Te.directionalShadowMatrix.value=U.state.directionalShadowMatrix,Te.spotShadowMap.value=U.state.spotShadowMap,Te.spotLightMatrix.value=U.state.spotLightMatrix,Te.spotLightMap.value=U.state.spotLightMap,Te.pointShadowMap.value=U.state.pointShadowMap,Te.pointShadowMatrix.value=U.state.pointShadowMatrix),B.currentProgram=Le,B.uniformsList=null,Le}function Cc(x){if(x.uniformsList===null){let D=x.currentProgram.getUniforms();x.uniformsList=bs.seqWithValue(D.seq,x.uniforms)}return x.uniformsList}function Ic(x,D){let k=xe.get(x);k.outputColorSpace=D.outputColorSpace,k.batching=D.batching,k.batchingColor=D.batchingColor,k.instancing=D.instancing,k.instancingColor=D.instancingColor,k.instancingMorph=D.instancingMorph,k.skinning=D.skinning,k.morphTargets=D.morphTargets,k.morphNormals=D.morphNormals,k.morphColors=D.morphColors,k.morphTargetsCount=D.morphTargetsCount,k.numClippingPlanes=D.numClippingPlanes,k.numIntersection=D.numClipIntersection,k.vertexAlphas=D.vertexAlphas,k.vertexTangents=D.vertexTangents,k.toneMapping=D.toneMapping}function Id(x,D,k,B,U){D.isScene!==!0&&(D=be),Be.resetTextureUnits();let Q=D.fog,le=B.isMeshStandardMaterial?D.environment:null,me=N===null?S.outputColorSpace:N.isXRRenderTarget===!0?N.texture.colorSpace:wi,de=(B.isMeshStandardMaterial?xt:bt).get(B.envMap||le),Ce=B.vertexColors===!0&&!!k.attributes.color&&k.attributes.color.itemSize===4,Le=!!k.attributes.tangent&&(!!B.normalMap||B.anisotropy>0),Te=!!k.morphAttributes.position,He=!!k.morphAttributes.normal,tt=!!k.morphAttributes.color,gt=Wn;B.toneMapped&&(N===null||N.isXRRenderTarget===!0)&&(gt=S.toneMapping);let at=k.morphAttributes.position||k.morphAttributes.normal||k.morphAttributes.color,it=at!==void 0?at.length:0,we=xe.get(B),pt=f.state.lights;if(Je===!0&&($===!0||x!==y)){let Dt=x===y&&B.id===M;ie.setState(B,x,Dt)}let Ze=!1;B.version===we.__version?(we.needsLights&&we.lightsStateVersion!==pt.state.version||we.outputColorSpace!==me||U.isBatchedMesh&&we.batching===!1||!U.isBatchedMesh&&we.batching===!0||U.isBatchedMesh&&we.batchingColor===!0&&U.colorTexture===null||U.isBatchedMesh&&we.batchingColor===!1&&U.colorTexture!==null||U.isInstancedMesh&&we.instancing===!1||!U.isInstancedMesh&&we.instancing===!0||U.isSkinnedMesh&&we.skinning===!1||!U.isSkinnedMesh&&we.skinning===!0||U.isInstancedMesh&&we.instancingColor===!0&&U.instanceColor===null||U.isInstancedMesh&&we.instancingColor===!1&&U.instanceColor!==null||U.isInstancedMesh&&we.instancingMorph===!0&&U.morphTexture===null||U.isInstancedMesh&&we.instancingMorph===!1&&U.morphTexture!==null||we.envMap!==de||B.fog===!0&&we.fog!==Q||we.numClippingPlanes!==void 0&&(we.numClippingPlanes!==ie.numPlanes||we.numIntersection!==ie.numIntersection)||we.vertexAlphas!==Ce||we.vertexTangents!==Le||we.morphTargets!==Te||we.morphNormals!==He||we.morphColors!==tt||we.toneMapping!==gt||we.morphTargetsCount!==it)&&(Ze=!0):(Ze=!0,we.__version=B.version);let Zt=we.currentProgram;Ze===!0&&(Zt=vr(B,D,U));let ki=!1,Jt=!1,Ts=!1,mt=Zt.getUniforms(),tn=we.uniforms;if(_e.useProgram(Zt.program)&&(ki=!0,Jt=!0,Ts=!0),B.id!==M&&(M=B.id,Jt=!0),ki||y!==x){_e.buffers.depth.getReversed()&&x.reversedDepth!==!0&&(x._reversedDepth=!0,x.updateProjectionMatrix()),mt.setValue(w,"projectionMatrix",x.projectionMatrix),mt.setValue(w,"viewMatrix",x.matrixWorldInverse);let Vt=mt.map.cameraPosition;Vt!==void 0&&Vt.setValue(w,pe.setFromMatrixPosition(x.matrixWorld)),Pe.logarithmicDepthBuffer&&mt.setValue(w,"logDepthBufFC",2/(Math.log(x.far+1)/Math.LN2)),(B.isMeshPhongMaterial||B.isMeshToonMaterial||B.isMeshLambertMaterial||B.isMeshBasicMaterial||B.isMeshStandardMaterial||B.isShaderMaterial)&&mt.setValue(w,"isOrthographic",x.isOrthographicCamera===!0),y!==x&&(y=x,Jt=!0,Ts=!0)}if(U.isSkinnedMesh){mt.setOptional(w,U,"bindMatrix"),mt.setOptional(w,U,"bindMatrixInverse");let Dt=U.skeleton;Dt&&(Dt.boneTexture===null&&Dt.computeBoneTexture(),mt.setValue(w,"boneTexture",Dt.boneTexture,Be))}U.isBatchedMesh&&(mt.setOptional(w,U,"batchingTexture"),mt.setValue(w,"batchingTexture",U._matricesTexture,Be),mt.setOptional(w,U,"batchingIdTexture"),mt.setValue(w,"batchingIdTexture",U._indirectTexture,Be),mt.setOptional(w,U,"batchingColorTexture"),U._colorsTexture!==null&&mt.setValue(w,"batchingColorTexture",U._colorsTexture,Be));let nn=k.morphAttributes;if((nn.position!==void 0||nn.normal!==void 0||nn.color!==void 0)&&ee.update(U,k,Zt),(Jt||we.receiveShadow!==U.receiveShadow)&&(we.receiveShadow=U.receiveShadow,mt.setValue(w,"receiveShadow",U.receiveShadow)),B.isMeshGouraudMaterial&&B.envMap!==null&&(tn.envMap.value=de,tn.flipEnvMap.value=de.isCubeTexture&&de.isRenderTargetTexture===!1?-1:1),B.isMeshStandardMaterial&&B.envMap===null&&D.environment!==null&&(tn.envMapIntensity.value=D.environmentIntensity),Jt&&(mt.setValue(w,"toneMappingExposure",S.toneMappingExposure),we.needsLights&&Pd(tn,Ts),Q&&B.fog===!0&&J.refreshFogUniforms(tn,Q),J.refreshMaterialUniforms(tn,B,V,ne,f.state.transmissionRenderTarget[x.id]),bs.upload(w,Cc(we),tn,Be)),B.isShaderMaterial&&B.uniformsNeedUpdate===!0&&(bs.upload(w,Cc(we),tn,Be),B.uniformsNeedUpdate=!1),B.isSpriteMaterial&&mt.setValue(w,"center",U.center),mt.setValue(w,"modelViewMatrix",U.modelViewMatrix),mt.setValue(w,"normalMatrix",U.normalMatrix),mt.setValue(w,"modelMatrix",U.matrixWorld),B.isShaderMaterial||B.isRawShaderMaterial){let Dt=B.uniformsGroups;for(let Vt=0,Io=Dt.length;Vt<Io;Vt++){let gi=Dt[Vt];ke.update(gi,Zt),ke.bind(gi,Zt)}}return Zt}function Pd(x,D){x.ambientLightColor.needsUpdate=D,x.lightProbe.needsUpdate=D,x.directionalLights.needsUpdate=D,x.directionalLightShadows.needsUpdate=D,x.pointLights.needsUpdate=D,x.pointLightShadows.needsUpdate=D,x.spotLights.needsUpdate=D,x.spotLightShadows.needsUpdate=D,x.rectAreaLights.needsUpdate=D,x.hemisphereLights.needsUpdate=D}function Ld(x){return x.isMeshLambertMaterial||x.isMeshToonMaterial||x.isMeshPhongMaterial||x.isMeshStandardMaterial||x.isShadowMaterial||x.isShaderMaterial&&x.lights===!0}this.getActiveCubeFace=function(){return A},this.getActiveMipmapLevel=function(){return P},this.getRenderTarget=function(){return N},this.setRenderTargetTextures=function(x,D,k){let B=xe.get(x);B.__autoAllocateDepthBuffer=x.resolveDepthBuffer===!1,B.__autoAllocateDepthBuffer===!1&&(B.__useRenderToTexture=!1),xe.get(x.texture).__webglTexture=D,xe.get(x.depthTexture).__webglTexture=B.__autoAllocateDepthBuffer?void 0:k,B.__hasExternalTextures=!0},this.setRenderTargetFramebuffer=function(x,D){let k=xe.get(x);k.__webglFramebuffer=D,k.__useDefaultFramebuffer=D===void 0};let Dd=w.createFramebuffer();this.setRenderTarget=function(x,D=0,k=0){N=x,A=D,P=k;let B=!0,U=null,Q=!1,le=!1;if(x){let de=xe.get(x);if(de.__useDefaultFramebuffer!==void 0)_e.bindFramebuffer(w.FRAMEBUFFER,null),B=!1;else if(de.__webglFramebuffer===void 0)Be.setupRenderTarget(x);else if(de.__hasExternalTextures)Be.rebindTextures(x,xe.get(x.texture).__webglTexture,xe.get(x.depthTexture).__webglTexture);else if(x.depthBuffer){let Te=x.depthTexture;if(de.__boundDepthTexture!==Te){if(Te!==null&&xe.has(Te)&&(x.width!==Te.image.width||x.height!==Te.image.height))throw new Error("WebGLRenderTarget: Attached DepthTexture is initialized to the incorrect size.");Be.setupDepthRenderbuffer(x)}}let Ce=x.texture;(Ce.isData3DTexture||Ce.isDataArrayTexture||Ce.isCompressedArrayTexture)&&(le=!0);let Le=xe.get(x).__webglFramebuffer;x.isWebGLCubeRenderTarget?(Array.isArray(Le[D])?U=Le[D][k]:U=Le[D],Q=!0):x.samples>0&&Be.useMultisampledRTT(x)===!1?U=xe.get(x).__webglMultisampledFramebuffer:Array.isArray(Le)?U=Le[k]:U=Le,I.copy(x.viewport),O.copy(x.scissor),z=x.scissorTest}else I.copy(Ee).multiplyScalar(V).floor(),O.copy(Ve).multiplyScalar(V).floor(),z=st;if(k!==0&&(U=Dd),_e.bindFramebuffer(w.FRAMEBUFFER,U)&&B&&_e.drawBuffers(x,U),_e.viewport(I),_e.scissor(O),_e.setScissorTest(z),Q){let de=xe.get(x.texture);w.framebufferTexture2D(w.FRAMEBUFFER,w.COLOR_ATTACHMENT0,w.TEXTURE_CUBE_MAP_POSITIVE_X+D,de.__webglTexture,k)}else if(le){let de=D;for(let Ce=0;Ce<x.textures.length;Ce++){let Le=xe.get(x.textures[Ce]);w.framebufferTextureLayer(w.FRAMEBUFFER,w.COLOR_ATTACHMENT0+Ce,Le.__webglTexture,k,de)}}else if(x!==null&&k!==0){let de=xe.get(x.texture);w.framebufferTexture2D(w.FRAMEBUFFER,w.COLOR_ATTACHMENT0,w.TEXTURE_2D,de.__webglTexture,k)}M=-1},this.readRenderTargetPixels=function(x,D,k,B,U,Q,le,me=0){if(!(x&&x.isWebGLRenderTarget)){console.error("THREE.WebGLRenderer.readRenderTargetPixels: renderTarget is not THREE.WebGLRenderTarget.");return}let de=xe.get(x).__webglFramebuffer;if(x.isWebGLCubeRenderTarget&&le!==void 0&&(de=de[le]),de){_e.bindFramebuffer(w.FRAMEBUFFER,de);try{let Ce=x.textures[me],Le=Ce.format,Te=Ce.type;if(!Pe.textureFormatReadable(Le)){console.error("THREE.WebGLRenderer.readRenderTargetPixels: renderTarget is not in RGBA or implementation defined format.");return}if(!Pe.textureTypeReadable(Te)){console.error("THREE.WebGLRenderer.readRenderTargetPixels: renderTarget is not in UnsignedByteType or implementation defined type.");return}D>=0&&D<=x.width-B&&k>=0&&k<=x.height-U&&(x.textures.length>1&&w.readBuffer(w.COLOR_ATTACHMENT0+me),w.readPixels(D,k,B,U,Me.convert(Le),Me.convert(Te),Q))}finally{let Ce=N!==null?xe.get(N).__webglFramebuffer:null;_e.bindFramebuffer(w.FRAMEBUFFER,Ce)}}},this.readRenderTargetPixelsAsync=async function(x,D,k,B,U,Q,le,me=0){if(!(x&&x.isWebGLRenderTarget))throw new Error("THREE.WebGLRenderer.readRenderTargetPixels: renderTarget is not THREE.WebGLRenderTarget.");let de=xe.get(x).__webglFramebuffer;if(x.isWebGLCubeRenderTarget&&le!==void 0&&(de=de[le]),de)if(D>=0&&D<=x.width-B&&k>=0&&k<=x.height-U){_e.bindFramebuffer(w.FRAMEBUFFER,de);let Ce=x.textures[me],Le=Ce.format,Te=Ce.type;if(!Pe.textureFormatReadable(Le))throw new Error("THREE.WebGLRenderer.readRenderTargetPixelsAsync: renderTarget is not in RGBA or implementation defined format.");if(!Pe.textureTypeReadable(Te))throw new Error("THREE.WebGLRenderer.readRenderTargetPixelsAsync: renderTarget is not in UnsignedByteType or implementation defined type.");let He=w.createBuffer();w.bindBuffer(w.PIXEL_PACK_BUFFER,He),w.bufferData(w.PIXEL_PACK_BUFFER,Q.byteLength,w.STREAM_READ),x.textures.length>1&&w.readBuffer(w.COLOR_ATTACHMENT0+me),w.readPixels(D,k,B,U,Me.convert(Le),Me.convert(Te),0);let tt=N!==null?xe.get(N).__webglFramebuffer:null;_e.bindFramebuffer(w.FRAMEBUFFER,tt);let gt=w.fenceSync(w.SYNC_GPU_COMMANDS_COMPLETE,0);return w.flush(),await Mu(w,gt,4),w.bindBuffer(w.PIXEL_PACK_BUFFER,He),w.getBufferSubData(w.PIXEL_PACK_BUFFER,0,Q),w.deleteBuffer(He),w.deleteSync(gt),Q}else throw new Error("THREE.WebGLRenderer.readRenderTargetPixelsAsync: requested read bounds are out of range.")},this.copyFramebufferToTexture=function(x,D=null,k=0){let B=Math.pow(2,-k),U=Math.floor(x.image.width*B),Q=Math.floor(x.image.height*B),le=D!==null?D.x:0,me=D!==null?D.y:0;Be.setTexture2D(x,0),w.copyTexSubImage2D(w.TEXTURE_2D,k,0,0,le,me,U,Q),_e.unbindTexture()};let Ud=w.createFramebuffer(),Nd=w.createFramebuffer();this.copyTextureToTexture=function(x,D,k=null,B=null,U=0,Q=null){Q===null&&(U!==0?(ls("WebGLRenderer: copyTextureToTexture function signature has changed to support src and dst mipmap levels."),Q=U,U=0):Q=0);let le,me,de,Ce,Le,Te,He,tt,gt,at=x.isCompressedTexture?x.mipmaps[Q]:x.image;if(k!==null)le=k.max.x-k.min.x,me=k.max.y-k.min.y,de=k.isBox3?k.max.z-k.min.z:1,Ce=k.min.x,Le=k.min.y,Te=k.isBox3?k.min.z:0;else{let nn=Math.pow(2,-U);le=Math.floor(at.width*nn),me=Math.floor(at.height*nn),x.isDataArrayTexture?de=at.depth:x.isData3DTexture?de=Math.floor(at.depth*nn):de=1,Ce=0,Le=0,Te=0}B!==null?(He=B.x,tt=B.y,gt=B.z):(He=0,tt=0,gt=0);let it=Me.convert(D.format),we=Me.convert(D.type),pt;D.isData3DTexture?(Be.setTexture3D(D,0),pt=w.TEXTURE_3D):D.isDataArrayTexture||D.isCompressedArrayTexture?(Be.setTexture2DArray(D,0),pt=w.TEXTURE_2D_ARRAY):(Be.setTexture2D(D,0),pt=w.TEXTURE_2D),w.pixelStorei(w.UNPACK_FLIP_Y_WEBGL,D.flipY),w.pixelStorei(w.UNPACK_PREMULTIPLY_ALPHA_WEBGL,D.premultiplyAlpha),w.pixelStorei(w.UNPACK_ALIGNMENT,D.unpackAlignment);let Ze=w.getParameter(w.UNPACK_ROW_LENGTH),Zt=w.getParameter(w.UNPACK_IMAGE_HEIGHT),ki=w.getParameter(w.UNPACK_SKIP_PIXELS),Jt=w.getParameter(w.UNPACK_SKIP_ROWS),Ts=w.getParameter(w.UNPACK_SKIP_IMAGES);w.pixelStorei(w.UNPACK_ROW_LENGTH,at.width),w.pixelStorei(w.UNPACK_IMAGE_HEIGHT,at.height),w.pixelStorei(w.UNPACK_SKIP_PIXELS,Ce),w.pixelStorei(w.UNPACK_SKIP_ROWS,Le),w.pixelStorei(w.UNPACK_SKIP_IMAGES,Te);let mt=x.isDataArrayTexture||x.isData3DTexture,tn=D.isDataArrayTexture||D.isData3DTexture;if(x.isDepthTexture){let nn=xe.get(x),Dt=xe.get(D),Vt=xe.get(nn.__renderTarget),Io=xe.get(Dt.__renderTarget);_e.bindFramebuffer(w.READ_FRAMEBUFFER,Vt.__webglFramebuffer),_e.bindFramebuffer(w.DRAW_FRAMEBUFFER,Io.__webglFramebuffer);for(let gi=0;gi<de;gi++)mt&&(w.framebufferTextureLayer(w.READ_FRAMEBUFFER,w.COLOR_ATTACHMENT0,xe.get(x).__webglTexture,U,Te+gi),w.framebufferTextureLayer(w.DRAW_FRAMEBUFFER,w.COLOR_ATTACHMENT0,xe.get(D).__webglTexture,Q,gt+gi)),w.blitFramebuffer(Ce,Le,le,me,He,tt,le,me,w.DEPTH_BUFFER_BIT,w.NEAREST);_e.bindFramebuffer(w.READ_FRAMEBUFFER,null),_e.bindFramebuffer(w.DRAW_FRAMEBUFFER,null)}else if(U!==0||x.isRenderTargetTexture||xe.has(x)){let nn=xe.get(x),Dt=xe.get(D);_e.bindFramebuffer(w.READ_FRAMEBUFFER,Ud),_e.bindFramebuffer(w.DRAW_FRAMEBUFFER,Nd);for(let Vt=0;Vt<de;Vt++)mt?w.framebufferTextureLayer(w.READ_FRAMEBUFFER,w.COLOR_ATTACHMENT0,nn.__webglTexture,U,Te+Vt):w.framebufferTexture2D(w.READ_FRAMEBUFFER,w.COLOR_ATTACHMENT0,w.TEXTURE_2D,nn.__webglTexture,U),tn?w.framebufferTextureLayer(w.DRAW_FRAMEBUFFER,w.COLOR_ATTACHMENT0,Dt.__webglTexture,Q,gt+Vt):w.framebufferTexture2D(w.DRAW_FRAMEBUFFER,w.COLOR_ATTACHMENT0,w.TEXTURE_2D,Dt.__webglTexture,Q),U!==0?w.blitFramebuffer(Ce,Le,le,me,He,tt,le,me,w.COLOR_BUFFER_BIT,w.NEAREST):tn?w.copyTexSubImage3D(pt,Q,He,tt,gt+Vt,Ce,Le,le,me):w.copyTexSubImage2D(pt,Q,He,tt,Ce,Le,le,me);_e.bindFramebuffer(w.READ_FRAMEBUFFER,null),_e.bindFramebuffer(w.DRAW_FRAMEBUFFER,null)}else tn?x.isDataTexture||x.isData3DTexture?w.texSubImage3D(pt,Q,He,tt,gt,le,me,de,it,we,at.data):D.isCompressedArrayTexture?w.compressedTexSubImage3D(pt,Q,He,tt,gt,le,me,de,it,at.data):w.texSubImage3D(pt,Q,He,tt,gt,le,me,de,it,we,at):x.isDataTexture?w.texSubImage2D(w.TEXTURE_2D,Q,He,tt,le,me,it,we,at.data):x.isCompressedTexture?w.compressedTexSubImage2D(w.TEXTURE_2D,Q,He,tt,at.width,at.height,it,at.data):w.texSubImage2D(w.TEXTURE_2D,Q,He,tt,le,me,it,we,at);w.pixelStorei(w.UNPACK_ROW_LENGTH,Ze),w.pixelStorei(w.UNPACK_IMAGE_HEIGHT,Zt),w.pixelStorei(w.UNPACK_SKIP_PIXELS,ki),w.pixelStorei(w.UNPACK_SKIP_ROWS,Jt),w.pixelStorei(w.UNPACK_SKIP_IMAGES,Ts),Q===0&&D.generateMipmaps&&w.generateMipmap(pt),_e.unbindTexture()},this.initRenderTarget=function(x){xe.get(x).__webglFramebuffer===void 0&&Be.setupRenderTarget(x)},this.initTexture=function(x){x.isCubeTexture?Be.setTextureCube(x,0):x.isData3DTexture?Be.setTexture3D(x,0):x.isDataArrayTexture||x.isCompressedArrayTexture?Be.setTexture2DArray(x,0):Be.setTexture2D(x,0),_e.unbindTexture()},this.resetState=function(){A=0,P=0,N=null,_e.reset(),oe.reset()},typeof __THREE_DEVTOOLS__<"u"&&__THREE_DEVTOOLS__.dispatchEvent(new CustomEvent("observe",{detail:this}))}get coordinateSystem(){return un}get outputColorSpace(){return this._outputColorSpace}set outputColorSpace(e){this._outputColorSpace=e;let t=this.getContext();t.drawingBufferColorSpace=Ye._getDrawingBufferColorSpace(e),t.unpackColorSpace=Ye._getUnpackColorSpace()}};var An={width:16,depth:14,centerZ:.4,minX:-7.65,maxX:7.65,minZ:-6.25,maxZ:6.95},en={speed:3.8,radius:.28,reach:1.35,maxFrameSeconds:.06,spawn:{x:-2,z:-3.6,yaw:0}},td={door:{id:"door",name:"\uCD9C\uC785\uBB38",x:-7.05,z:-3.4},kitchen:{id:"kitchen",name:"\uBCBD \uBD99\uC784 \uC870\uB9AC\uB300",x:-1.8,z:-4.25},stove:{id:"stove",name:"\uD654\uB355",x:-5,z:-4.25},book:{id:"book",name:"\uC218\uCCA9",x:2,z:-4.35},stairs:{id:"stairs",name:"\uACC4\uB2E8 \uC55E",x:6.55,z:-1.8}},So={x:-6.5,z:-2.1},bo=[{x:-4.15,z:-2.3,width:3,depth:.16,height:1.9},{x:1.1,z:-2.3,width:3.8,depth:.16,height:1.9}],nd=[{id:"kitchen",x:-1.8,z:-5.65,width:3.7,depth:1.5},{id:"stove",x:-5,z:-5.65,width:1.8,depth:1.5},{id:"book",x:2,z:-5.7,width:2,depth:1.35},{id:"stairs",x:6.55,z:-4.45,width:2.15,depth:4.15}];function id(n){n.box(An.width,.5,An.depth,0,-.3,.4,"#364b4c"),n.box(An.width+.25,.2,An.depth+.25,0,-.57,.4,"#26383b");for(let l=0;l<16;l++)for(let h=0;h<14;h++)n.box(.98,.08,.98,l-7.5,.01,h-6.1,(l+h)%3===0?"#93866e":(l+h)%3===1?"#a19278":"#9e8d70");n.box(16,1.7,.35,0,.85,-6.6,"#596a62"),n.box(.35,1.5,14,-8,.75,.4,"#596a62"),n.box(16,.15,.4,0,1.72,-6.6,"#b29b77"),n.box(.4,.15,14,-8,1.55,.4,"#b29b77");for(let l=0;l<16;l++)for(let h=0;h<2;h++)n.box(.96,.48,.06,l-7.5,.3+h*.8,-6.39,"#718172");for(let l of[-7.8,7.8])n.box(.45,2.6,.5,l,1.3,-6.4,"#454e48"),n.box(.64,.18,.65,l,2.65,-6.4,"#b09c78");n.box(10.4,.025,1.2,-2,.09,-3.8,"#697d6c");let e=n.box(3.6,1.12,1.4,-1.8,.6,-5.65,"#4f5c53");n.target(e,"kitchen","\uC870\uB9AC\uB300 \xB7 \uAC00\uAE4C\uC774\uC5D0\uC11C E"),n.box(3.7,.18,1.5,-1.8,1.24,-5.65,"#b89b70");for(let l=0;l<3;l++){let h=-3+l*.8;n.cylinder(.18,.12,.13,h,1.4,-5.5,["#8eaa7b","#e2ab69","#bf855e"][l]);for(let u=0;u<3;u++)n.sphere(.08,h+(u-1)*.1,1.5,-5.5,"#e1c27c")}n.box(.7,.06,.48,-.7,1.37,-5.5,"#cfb48c"),n.box(4.2,.12,.42,-1.8,2.15,-6.15,"#b19b77");for(let l=0;l<6;l++)n.cylinder(.1,.12,.24,-3.4+l*.6,2.31,-6.15,["#bd9a72","#96a68b","#b8a8a0"][l%3]);let t=n.box(1.8,1.2,1.5,-5,.65,-5.65,"#65685b");n.target(t,"stove","\uD654\uB355 \xB7 \uC870\uB9AC \uC644\uB8CC \uD6C4 E\uB85C \uC9D1\uAE30"),n.box(.8,.5,.08,-5,.49,-4.85,"#251e19"),n.fire=n.sphere(.22,-5,.48,-4.77,"#e5a05a");let i=new ft(new li(.43,.12,5,9,Math.PI),n.mat("#b3a17f"));i.position.set(-5,.68,-4.8),n.scene.add(i);let s=new ft(new er(.38,16,0,Math.PI),n.mat("#251e19"));s.position.set(-5,.68,-4.79),n.scene.add(s);for(let l of[-5.44,-4.56])n.box(.22,.56,.18,l,.41,-4.8,"#a79879"),n.box(.25,.04,.19,l,.43,-4.79,"#655e4c");for(let l of[-4.83,-4.7]){let h=n.cylinder(.07,.08,.63,-5,.26,l,"#9d7547");h.rotation.z=Math.PI/2}n.box(.46,.85,.5,-5.55,1.88,-6.03,"#777762"),n.box(.59,.13,.61,-5.55,2.33,-6.03,"#ab9a76"),n.fire.material=new Ri({color:"#f7b45c",emissive:"#fb952a",emissiveIntensity:1.8});let r=new ar("#ffae5b",7,6,2);r.position.set(-5,1,-4.2),n.scene.add(r),n.cylinder(.54,.42,.45,-5,1.44,-5.65,"#303b3a"),n.cylinder(.49,.49,.03,-5,1.68,-5.65,"#d6ab65");for(let l of[-5.63,-4.37])n.box(.25,.1,.2,l,1.51,-5.65,"#39413b");n.steam=new kt;for(let l=0;l<4;l++){let h=n.sphere(.09+l*.025,Math.sin(l)*.18,l*.2,0,"#a2b9ab",n.steam);h.material=new bn({color:"#d8ddc8",transparent:!0,opacity:.22})}n.steam.position.set(-5,1.8,-5.65),n.scene.add(n.steam);let o=n.box(2,1,1.3,2,.55,-5.7,"#705a42");n.box(2,.12,1.35,2,1.11,-5.7,"#b0966f"),n.target(o,"book","\uC218\uCCA9 \xB7 \uAC00\uAE4C\uC774\uC5D0\uC11C E"),n.box(.68,.05,.6,1.8,1.21,-5.55,"#e5dbbc").rotation.y=.16,n.box(.02,.06,.6,1.8,1.23,-5.55,"#726448"),n.cylinder(.12,.16,.18,2.5,1.26,-5.7,"#4e605d");let a=n.box(.2,2.3,1.4,-7.8,1.18,-3.4,"#2b4c4a");n.target(a,"door","\uCD9C\uC785\uBB38 \xB7 \uAC00\uAE4C\uC774\uC5D0\uC11C E");for(let l of[-4.2,-2.6])n.box(.35,2.4,.1,-7.7,1.2,l,"#b39c79");n.box(.35,.12,1.75,-7.7,2.4,-3.4,"#b39c79"),n.box(1.3,.03,1.6,-7,.1,-3.4,"#c4ad7c"),n.box(1.8,.12,4.1,6.55,-.05,-4.45,"#182d31");for(let l=0;l<9;l++){let h=n.box(1.8,.12,.43,6.55,-l*.07,-2.6-l*.43,"#718c85");n.target(h,"stairs","\uACC4\uB2E8 \uC55E \xB7 \uAC00\uAE4C\uC774\uC5D0\uC11C E")}for(let l of[5.55,7.55])n.box(.15,1.25,4.1,l,.58,-4.45,"#586357"),n.box(.22,.16,4.15,l,1.24,-4.45,"#ba9a6d");for(let l of bo){let h=l.width/2,u=Math.ceil(l.width/.125),p=l.width/u;for(let d of[l.x-h,l.x+h])n.cylinder(.055,.08,2.12,d,1.06,l.z,"#b49b72"),n.sphere(.09,d,2.16,l.z,"#d8c28d");n.box(l.width+.1,.08,.08,l.x,2.02,l.z,"#b49b72");for(let d=0;d<u;d++){let g=l.x-h+(d+.5)*p,v=l.z+Math.sin(d*Math.PI/2)*.045;n.box(p+.005,1.55,.055,g,1.13,v,d%2?"#6f8982":"#82978d"),n.box(p+.005,.1,.065,g,.46,v,"#c2a774")}}n.seatGroups=[];for(let l of _t){let h=new kt;h.position.set(l.x,0,l.z),n.scene.add(h);let u=n.cylinder(.68,.62,.18,0,.9,0,"#c09a6a",h);n.cylinder(.14,.23,.8,0,.43,0,"#605441",h),n.box(.62,.12,.6,1.02,.5,0,"#a4815b",h),n.box(.1,.9,.6,1.3,.7,0,"#6f5c46",h);for(let d of[.78,1.26])for(let g of[-.2,.2])n.box(.09,.46,.09,d,.25,g,"#605441",h);if(n.cylinder(.22,.22,.04,-.12,1.02,.08,"#e6d9b5",h),n.cylinder(.1,.08,.18,.3,1.06,-.15,"#cab089",h),l.axis==="poison"){n.cylinder(.18,.12,.24,-.35,1.09,-.24,"#b87853",h);for(let d=0;d<5;d++)n.sphere(.15,-.35+Math.sin(d*2)*.12,1.34+d%2*.13,-.24+Math.cos(d*2)*.1,"#6a9f77",h).scale.set(.45,1.3,.6)}if(l.axis==="cold"&&(n.cylinder(.12,.15,.3,-.3,1.18,-.3,"#eece85",h),n.sphere(.08,-.3,1.4,-.3,"#fcbf66",h)),l.axis==="vital"&&n.box(.58,.18,.55,1.02,.63,0,"#7b9e8c",h),l.axis==="fire")for(let d=0;d<3;d++)n.box(.3,.05,.2,-.3,1.14+d*.12,-.22,"#89aaa2",h);l.axis==="dark"&&n.box(.24,.3,.24,-.25,1.17,-.22,"#6b718b",h),l.axis==="sense"&&n.box(.25,.08,.35,-.3,1.08,-.25,"#6b7e68",h),n.target(u,"seat:"+l.id,l.name+" \xB7 \uAC00\uAE4C\uC774\uC5D0\uC11C E"),h.traverse(d=>{d.isMesh&&d!==u&&(d.userData=u.userData,n.targets.push(d))});let p=new ft(new Ai(.82,.91,48),new bn({color:l.color,transparent:!0,opacity:.7,side:qt}));p.rotation.x=-Math.PI/2,p.position.y=.11,h.add(p),h.userData.ring=p,n.seatGroups.push(h)}n.chef=n.person("#ded1af","human","chef"),n.chef.position.set(en.spawn.x,0,en.spawn.z),n.scene.add(n.chef),n.plate=new kt,n.cylinder(.35,.33,.06,0,1,.55,"#e9dfbe",n.plate),n.sphere(.18,0,1.12,.55,"#b89555",n.plate),n.chef.add(n.plate),n.plate.visible=!1,n.playerRing=new ft(new Ai(.38,.46,48),new bn({color:"#f1d68f",side:qt,transparent:!0,opacity:.85})),n.playerRing.rotation.x=-Math.PI/2,n.scene.add(n.playerRing),n.playerLabel=n.label("\uB098 \xB7 WASD",en.spawn.x,2.35,en.spawn.z,"#ffe2a1",.65),n.destination=new ft(new Ai(.48,.57,48),new bn({color:"#f0c57d",side:qt,transparent:!0,opacity:.75})),n.destination.rotation.x=-Math.PI/2,n.destination.visible=!1,n.scene.add(n.destination),n.guests=new Map,n.label("01 \uCD9C\uC785\uBB38",-7.1,2.8,-3.4,.75),n.label("03 \uBCBD \uBD99\uC784 \uC870\uB9AC\uB300",-1.8,2.85,-5.7,.95),n.label("04 \uD654\uB355",-5,2.65,-5.65,.75),n.label("05 \uC218\uCCA9",2,2.4,-5.7,.75),n.label("06 \uACC4\uB2E8",6.55,2,-4.7,.8),n.keepsakeMeshes=new Map;for(let l of[1.2,1.95])n.box(2.15,.1,.4,4.1,l,-6.05,"#b19b77");Object.values(Vi).filter(l=>l.gift).forEach((l,h)=>{let u=new kt;u.position.set(3.32+h%4*.52,1.25+Math.floor(h/4)*.75,-6.03),n.scene.add(u);let p=l.gift.kind;if(p==="emblem"){let d=n.cylinder(.19,.19,.06,0,.22,0,"#d7b16d",u,6);d.rotation.x=Math.PI/2,n.box(.08,.22,.07,0,.22,.07,"#6c827a",u)}if(p==="scroll"){let d=n.cylinder(.07,.07,.34,0,.12,0,"#e3d8b2",u);d.rotation.z=Math.PI/2,n.box(.05,.15,.17,0,.12,0,"#a5705d",u)}if(p==="key"){let d=new ft(new li(.09,.025,6,12),n.mat("#d2b56b"));d.position.set(-.08,.13,.04),u.add(d),n.box(.23,.04,.05,.07,.13,.04,"#d2b56b",u),n.box(.04,.09,.05,.16,.09,.04,"#d2b56b",u)}if(p==="firstaid"&&(n.box(.32,.24,.23,0,.14,0,"#d5c8a5",u),n.box(.05,.14,.02,0,.15,.13,"#729985",u),n.box(.15,.05,.02,0,.15,.13,"#729985",u)),p==="prayer"||p==="map"){n.box(.34,.32,.035,0,.18,0,"#e3d8b2",u);for(let d=0;d<3;d++)n.box(p==="map"?.16:.22,.018,.018,d%2*.04,.09+d*.08,.025,p==="map"?"#7e927e":"#9c8967",u)}p==="ore"&&(n.sphere(.17,0,.16,0,"#809b99",u).scale.set(1,.8,.8),n.sphere(.08,.12,.24,.03,"#c3b46f",u)),p==="arrow"&&(n.box(.04,.29,.04,0,.17,0,"#9f8061",u),n.cylinder(0,.11,.16,0,.36,0,"#a7b7b2",u,3)),u.visible=!1,n.keepsakeMeshes.set(l.id,u)}),n.label("\uB2E8\uACE8\uC774 \uB0A8\uAE34 \uBB3C\uAC74",4.1,2.75,-6.1,.75)}var Eo=class{constructor(e,t){this.container=e,this.onPick=t,this.scene=new Ys,this.targets=[],this.angle=.72,this.zoom=1,this.disposed=!1,this.materials=new Map,this.view={},this.mouse=new Ue,this.ray=new or,this.renderer=new yo({antialias:!0,alpha:!0}),this.renderer.setPixelRatio(Math.min(devicePixelRatio,1.7)),this.renderer.shadowMap.enabled=!0,this.renderer.shadowMap.type=ba,this.renderer.outputColorSpace=Ft,this.renderer.toneMapping=Pa,this.renderer.toneMappingExposure=1.4,e.appendChild(this.renderer.domElement),this.camera=new Ii(-9,9,9,-9,.1,100),this.scene.add(new rr("#c5e5e4","#4b3b35",2.1));let i=new gs("#ffe0ad",3.2);i.position.set(-3,12,6),i.castShadow=!0,i.shadow.mapSize.set(2048,2048),Object.assign(i.shadow.camera,{left:-13,right:13,top:13,bottom:-13}),i.shadow.bias=-8e-4,this.scene.add(i);let s=new gs("#84c5c6",1.3);s.position.set(5,7,-7),this.scene.add(s),this.build(),this.bind(),this.resizeObserver=new ResizeObserver(()=>this.resize()),this.resizeObserver.observe(e),this.resize(),this.tick(0)}mat(e){return this.materials.has(e)||this.materials.set(e,new Ri({color:e,roughness:.86,flatShading:!0})),this.materials.get(e)}box(e,t,i,s,r,o,a,c=this.scene){let l=new ft(new oi(e,t,i),this.mat(a));return l.position.set(s,r,o),l.castShadow=!0,l.receiveShadow=!0,c.add(l),l}cylinder(e,t,i,s,r,o,a,c=this.scene,l=12){let h=new ft(new tr(e,t,i,l),this.mat(a));return h.position.set(s,r,o),h.castShadow=!0,h.receiveShadow=!0,c.add(h),h}sphere(e,t,i,s,r,o=this.scene){let a=new ft(new ir(e,10,7),this.mat(r));return a.position.set(t,i,s),a.castShadow=!0,o.add(a),a}label(e,t,i,s,r="#f2dfb8",o=1){typeof r=="number"&&(o=r,r="#f2dfb8");let a=document.createElement("canvas");a.width=512,a.height=100;let c=a.getContext("2d");c.font='600 31px "Malgun Gothic",sans-serif',c.textAlign="center",c.textBaseline="middle",c.fillStyle="rgba(22,36,37,.85)",c.beginPath(),c.roundRect(12,14,488,72,12),c.fill(),c.fillStyle=r,c.fillText(e,256,50);let l=new Ks(a),h=new Js(new fs({map:l,depthTest:!1,transparent:!0}));return h.scale.set(2.5*o,.49*o,1),h.position.set(t,i,s),this.scene.add(h),h}target(e,t,i){return e.userData={station:t,name:i},this.targets.push(e),e}build(){id(this)}person(e,t,i){let s=new kt;this.cylinder(.23,.34,.72,0,.79,0,e,s),this.sphere(.25,0,1.4,0,"#d7b08a",s),this.box(.44,.12,.3,0,1.61,-.07,"#524738",s);for(let r of[-.13,.13])this.box(.15,.42,.2,r,.22,0,"#394748",s),this.box(.18,.13,.32,r,.08,.08,"#353d35",s),this.sphere(.024,r*.65,1.43,.225,"#343c35",s);return this.box(.12,.55,.13,-.34,.84,.02,e,s),this.box(.12,.55,.13,.34,.84,.02,e,s),i==="chef"?(this.cylinder(.29,.25,.25,0,1.72,0,"#e9e3cc",s),this.box(.34,.55,.07,0,.7,.31,"#ded4b8",s)):(this.box(.35,.55,.2,0,.92,-.28,"#8d7759",s),this.box(.1,1,.12,.42,.78,-.1,"#b2b8a6",s)),s}update(e){this.view=e;let t=new Set((e.keepsakes||[]).map(s=>s.id));this.keepsakeMeshes.forEach((s,r)=>{s.visible=t.has(r)}),this.seatGroups.forEach((s,r)=>{let o=e.seats?.includes(r);s.userData.ring.material.opacity=e.seat===r?.95:o?.25:.08,s.scale.y=o?1:.72});let i=new Set((e.guests||[]).map(s=>s.key));for(let[s,r]of this.guests)if(!i.has(s)){let o=new Set;r.mesh.traverse(a=>{o.add(a),a.geometry?.dispose()}),this.targets=this.targets.filter(a=>!o.has(a)),this.scene.remove(r.mesh,r.label),r.label.material.map.dispose(),r.label.material.dispose(),this.guests.delete(s)}for(let s of e.guests||[]){let r=this.guests.get(s.key),o=s.report?So:Sr(s.slot);if(!r){let u=this.person("#748caa",s.data.race,s.data.job);u.position.set(o.x,0,o.z),this.scene.add(u),this.target(u,s.key,s.data.name+" \xB7 \uAC00\uAE4C\uC774\uC5D0\uC11C E"),u.traverse(p=>{p.isMesh&&(p.userData=u.userData,this.targets.push(p))}),r={mesh:u,label:this.label("",o.x,2.2,o.z,"#f1e4c5",.78),path:[],target:{...o}},this.guests.set(s.key,r),this.dressGuest(s.data,r)}let a=s.seat!==null&&s.seat!==void 0,c=a?{x:_t[s.seat].x+1.02,z:_t[s.seat].z}:o;if(r.target.x!==c.x||r.target.z!==c.z){if(a){let u=_t[s.seat].z-1.7;r.path=[{x:r.mesh.position.x,z:r.mesh.position.z-1},{x:-6.2,z:r.mesh.position.z-1},{x:-6.2,z:u},{x:c.x,z:u},{...c}]}else r.path=[{...c}];r.target={...c}}r.mesh.rotation.x=s.data.tired?.12:0,r.mesh.scale.setScalar(["dwarf","halfling"].includes(s.data.race)?.83:1);let l=r.label.material.map,h=l.image.getContext("2d");h.clearRect(0,0,512,100),h.fillStyle=s.selected?"#68512b":"rgba(22,36,37,.92)",h.beginPath(),h.roundRect(12,14,488,72,12),h.fill(),h.fillStyle="#f1e4c5",h.font='600 27px "Malgun Gothic",sans-serif',h.textAlign="center",h.textBaseline="middle",h.fillText(s.data.name+" \xB7 "+(s.report?"\uADC0\uD658":As[s.status]),256,50),l.needsUpdate=!0}this.steam.visible=(e.guests||[]).some(s=>s.status==="cooking")}isGuestMoving(e){return!!this.guests.get(e)?.path.length}setPlayer(e,t,i){this.player=e||en.spawn,this.playerMoving=!!t,this.chef.position.set(this.player.x,0,this.player.z),this.chef.rotation.y=this.player.yaw,this.plate.visible=!!i,this.playerRing.position.set(this.player.x,.12,this.player.z),this.playerLabel.position.set(this.player.x,2.35,this.player.z)}setDestination(e){this.destination.visible=!!e,e&&this.destination.position.set(e.x,.12,e.z)}dressGuest(e,t){let i=e.race+":"+e.job;if(t.style===i)return;t.style=i;let s=t.extras;if(s){let a=new Set;s.traverse(c=>{a.add(c),c.geometry?.dispose()}),this.targets=this.targets.filter(c=>!a.has(c)),t.mesh.remove(s)}let r=new kt;t.extras=r,t.mesh.add(r);let o={warrior:"#8296a7",mage:"#a391b4",rogue:"#657c6a",priest:"#d5c6a1",miner:"#b89361",archer:"#7b9c88"};if(t.mesh.children[0].material=this.mat(o[e.job]),e.race==="elf")for(let a of[-.28,.28]){let c=this.box(.24,.12,.12,a,1.47,0,"#d7b08a",r);c.rotation.z=a>0?.5:-.5}if(e.race==="beast"){for(let c of[-.18,.18])this.cylinder(0,.12,.28,c,1.75,0,"#927555",r,3);let a=this.cylinder(.06,.1,.65,0,.65,-.55,"#927555",r);a.rotation.x=.8}if(e.race==="dwarf"&&this.box(.37,.35,.17,0,1.22,.21,"#805a38",r),e.race==="halfling")for(let a of[-.13,.13])this.box(.23,.13,.43,a,.08,.12,"#d7b08a",r);if(e.job==="warrior"&&(this.box(.42,.48,.13,-.44,.93,.12,"#8aabb4",r),this.box(.3,.07,.08,-.44,.93,.21,"#c7cdba",r)),e.job==="mage"&&(this.cylinder(0,.35,.62,0,1.92,0,"#81749f",r),this.sphere(.14,.46,1.52,-.1,"#8bbbbd",r)),e.job==="rogue"&&(this.box(.44,.1,.16,0,1.34,.2,"#455e55",r),this.box(.15,.4,.1,-.42,.7,.15,"#c2c3a7",r)),e.job==="priest"&&(this.box(.2,.6,.09,0,.92,.3,"#e8d7a1",r),this.box(.08,.32,.07,0,1,.37,"#ab8c55",r),this.box(.23,.08,.07,0,1.04,.37,"#ab8c55",r)),e.job==="miner"&&(this.cylinder(.29,.3,.16,0,1.67,0,"#d0ad65",r),this.sphere(.09,0,1.68,.27,"#f1d6a0",r),this.box(.65,.09,.11,.42,1.2,-.1,"#a7b3aa",r)),e.job==="archer"){let a=new ft(new li(.43,.045,5,12,Math.PI),this.mat("#9b774e"));a.position.set(-.42,1,.06),a.rotation.z=-Math.PI/2,r.add(a),this.box(.03,.86,.03,-.42,1,.06,"#d4c59a",r)}r.traverse(a=>{a.isMesh&&(a.userData=t.mesh.userData,this.targets.push(a))})}reset(){this.angle=.72,this.zoom=1,this.resize()}resize(){let{clientWidth:e,clientHeight:t}=this.container;if(!e||!t)return;this.renderer.setSize(e,t,!1);let i=e/t,s=Math.max(9.4,12.3/i)/this.zoom;this.camera.left=-s*i,this.camera.right=s*i,this.camera.top=s,this.camera.bottom=-s,this.camera.position.set(Math.sin(this.angle)*22,22,Math.cos(this.angle)*22),this.camera.lookAt(0,0,.4),this.camera.updateProjectionMatrix()}hit(e){let t=this.renderer.domElement.getBoundingClientRect();return this.mouse.set((e.clientX-t.left)/t.width*2-1,-(e.clientY-t.top)/t.height*2+1),this.ray.setFromCamera(this.mouse,this.camera),this.ray.intersectObjects(this.targets,!1).find(i=>i.object.visible&&i.object.parent?.visible!==!1)?.object.userData}bind(){let e=this.renderer.domElement,t=null,i=!1;e.addEventListener("pointerdown",s=>{t={x:s.clientX,y:s.clientY,a:this.angle},i=!1,e.setPointerCapture(s.pointerId)}),e.addEventListener("pointermove",s=>{if(t){let r=s.clientX-t.x;Math.abs(r)>5&&(i=!0),i&&(this.angle=Math.max(-.35,Math.min(1.4,t.a-r*.004)),this.resize())}else{let r=this.hit(s);e.style.cursor=r?"pointer":"grab";let o=document.querySelector("#hover-label");o.hidden=!r,r&&(o.textContent=r.name)}}),e.addEventListener("pointerup",s=>{if(t&&!i){let r=this.hit(s);r&&this.onPick(r.station)}t=null}),e.addEventListener("pointercancel",()=>{t=null}),e.addEventListener("wheel",s=>{s.preventDefault(),this.zoom=Math.max(.75,Math.min(1.7,this.zoom-s.deltaY*.001)),this.resize()},{passive:!1})}tick(e){if(this.disposed)return;let t=Math.min(.06,Math.max(0,(e-(this.previousTime||e))/1e3));this.previousTime=e;for(let i of this.guests.values()){let s=i.path[0];if(s){let r=s.x-i.mesh.position.x,o=s.z-i.mesh.position.z,a=Math.hypot(r,o),c=t*3;a<=c?(i.mesh.position.x=s.x,i.mesh.position.z=s.z,i.path.shift()):(i.mesh.position.x+=r/a*c,i.mesh.position.z+=o/a*c,i.mesh.rotation.y=Math.atan2(r,o))}i.label.position.copy(i.mesh.position).add(new L(0,2.2,0))}this.playerMoving?this.chef.position.y=Math.sin(e*.018)*.045:this.chef.position.y=0,this.fire.scale.setScalar(1+Math.sin(e*.006)*.08),this.steam.visible&&this.steam.children.forEach((i,s)=>{i.position.y=(e*3e-4+s*.22)%1.1,i.material.opacity=.25*(1-i.position.y/1.1)}),this.renderer.render(this.scene,this.camera),this.frame=requestAnimationFrame(i=>this.tick(i))}dispose(){this.disposed=!0,cancelAnimationFrame(this.frame),this.resizeObserver.disconnect(),this.scene.traverse(e=>{e.geometry?.dispose()});for(let e of this.materials.values())e.dispose();this.renderer.dispose()}};function cc(n,e,t){t=t||{};let i={happy:"M26 38 Q32 44 38 38",neutral:"M27 39 H37",sad:"M26 42 Q32 36 38 42"}[e||"neutral"],s={warrior:'<path d="M17 31 C17 14 47 14 47 31"/><path d="M15 31 H49"/><path d="M32 18 V35"/><path d="M32 14 C30 7 38 5 40 10"/>',mage:'<path d="M16 25 L32 2 L48 25"/><path d="M10 26 Q32 33 54 26"/><path d="M32 12 l1.6 3.4 3.4.4-2.6 2.3.7 3.4-3.1-1.8-3.1 1.8.7-3.4-2.6-2.3 3.4-.4z" stroke-width="1.4"/>',rogue:'<path d="M16 38 C13 12 51 12 48 38"/><path d="M19 26 H45 V34 H19 Z"/>',priest:'<ellipse cx="32" cy="11" rx="11" ry="3.6"/><path d="M32 50 V62"/><path d="M26 55 H38"/>',miner:'<path d="M17 29 C17 14 47 14 47 29 Z"/><circle cx="32" cy="18" r="3.2"/><path d="M32 11 V7 M24 13 L21 10 M40 13 L43 10"/><path d="M20 38 Q32 58 44 38"/>',archer:'<path d="M16 37 C13 13 51 13 48 37"/><path d="M45 17 C54 8 57 13 51 22"/><path d="M58 22 Q68 40 58 58"/><path d="M58 22 V58"/>'}[n]||"",r=n==="rogue"?30:31,o=t.tired?'<path d="M52 22 q3 4 0 7 q-3 -3 0 -7" stroke-width="1.6"/>':"",a=t.hurt?'<path d="M20 24 L44 19" stroke-width="4.5" opacity=".9"/>':"";return'<svg class="portrait'+(t.sm?" sm":"")+'" viewBox="-4 0 72 64" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M8 63 C8 49 18 46 32 46 C46 46 56 49 56 63"/><circle cx="32" cy="32" r="13"/>'+s+a+'<circle class="hl" cx="27" cy="'+r+'" r="1.7"/><circle class="hl" cx="37" cy="'+r+'" r="1.7"/><path d="'+i+'"/>'+o+"</svg>"}function sd(){let n="M10 30 ",e=(s,r,o,a,c)=>{let l="";for(let h=0;h<o;h++)l+="h"+a+" v"+c+" ";return l};n+=e(0,0,5,38,20);let t=200,i=130;return n+="h150 ",n+=e(0,0,4,38,20),'<svg class="title-art" viewBox="0 0 640 230" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="'+n+'" opacity=".85"/><path d="M10 30 H0" opacity=".0"/><path d="M'+(t+34)+" "+(i-30)+" h80 M"+(t+42)+" "+(i-30)+" v30 M"+(t+106)+" "+(i-30)+' v30"/><path d="M'+(t+58)+" "+(i-30)+' v-14 h16 v14"/><path d="M'+(t+66)+" "+(i-54)+' q-6 -8 0 -14 q6 -6 0 -14" opacity=".7"/><path d="M'+(t+120)+" 60 V"+(i-78)+'"/><rect x="'+(t+110)+'" y="'+(i-78)+'" width="20" height="26" rx="4"/><path d="M'+(t+120)+" "+(i-72)+' v14" opacity=".6"/><path d="M470 190 v0" /><path d="M515 190 l40 0 M515 210 l60 0" opacity=".35"/></svg>'}var hc=n=>n>0?"+"+n:String(n),rd=(n,e)=>`<button data-act="regular-story" data-id="${We(n)}">${e}</button>`;function uc(n){return n?.regularId?`<div class="relationship"><span class="tag">\uB2E8\uACE8 \xB7 \uB808\uBCA8 ${n.level}</span><p>\u{1F4A1} \uD638\uAC10 ${n.affinity} / ${je.max}</p><progress max="${je.max}" value="${n.affinity}" aria-label="\uB2E8\uACE8 \uD638\uAC10"></progress><small>${n.nextAt===null?"\uBAA8\uB4E0 \uC0AC\uC5F0\uC744 \uB4E4\uC5C8\uC2B5\uB2C8\uB2E4":`\uB2E4\uC74C \uC0AC\uC5F0: \uD638\uAC10 ${n.nextAt} \xB7 ${n.remaining} \uB0A8\uC74C`}</small></div>`:'<p class="small muted">\uACFC\uAC1D \xB7 \uAC1C\uC778 \uC774\uB984\uACFC \uCDE8\uD5A5\uC744 \uAE30\uC5B5\uD569\uB2C8\uB2E4.</p>'}function ad(n){return n?.promise?`<div class="notice">\u{1F4A1} \uC57D\uC18D \xB7 \u201C\uBB34\uC0AC\uD788 \uB2E4\uB140\uC624\uBA74 ${We(Z.ING[n.promise].n)} \uB450 \uAC1C\uB97C \uAC00\uC838\uC62C\uAC8C.\u201D<br>\uC131\uACF5\xB7\uB300\uC131\uACF5 \uADC0\uD658 \uC2DC \uBC1B\uC2B5\uB2C8\uB2E4.</div>`:""}function od(n){return'<p class="intro">\uC2DD\uD0C1\uC5D0\uC11C \uC313\uC778 \uAD00\uACC4\uC640 \uC190\uB2D8\uB4E4\uC774 \uB0A8\uAE34 \uC774\uC57C\uAE30.</p><div class="notice">\u{1F4A1} \uD638\uAC10 \uC218\uCE58\xB7\uC0AC\uC5F0 \uCD08\uC548\uC785\uB2C8\uB2E4. \u{1F534} \uCD5C\uC885 \uB300\uC0AC\uC640 \uBB3C\uAC74\uBCC4 \uD6A8\uACFC\uB294 \uBBF8\uC815\uC785\uB2C8\uB2E4. \uC0AC\uC5F0 \uC18D \uC9C0\uB3C4\xB7\uC5F4\uC1E0\uB294 \uC61B \uD30C\uD2F0\uC758 \uC0DD\uC874\uC774\uB098 \uC704\uCE58\uB97C \uD655\uC815\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.</div>'+n.relationships.regulars.map(e=>`<article class="regular-card"><h3>${We(e.name)}${e.visits?"":" \xB7 \uC544\uC9C1 \uB9CC\uB098\uC9C0 \uBABB\uD568"}</h3><p class="small muted">${We(e.intro)} \xB7 \uBC29\uBB38 ${e.visits}\uD68C${e.restUntil>n.s.day?" \xB7 \uBD80\uC0C1 \uD68C\uBCF5 \uC911":""}</p>${uc(e)}${e.unlocked.length?e.unlocked.map(t=>`<div class="story-entry"><span>\u{1F4A1} \uD638\uAC10 ${t.at}\uC5D0\uC11C \uB4E4\uC740 \uC774\uC57C\uAE30</span>${rd(t.id,"\uC0AC\uC5F0 \uB2E4\uC2DC \uC77D\uAE30")}${t.gift?`<p>\uB0A8\uAE34 \uBB3C\uAC74 \xB7 ${We(t.gift.name)}</p>`:""}</div>`).join(""):'<p class="small muted">\uD638\uAC10 3\uC774 \uB418\uBA74 \uCCAB \uC0AC\uC5F0\uC744 \uB4E4\uB824\uC90D\uB2C8\uB2E4.</p>'}</article>`).join("")}function ld(n){let e=n.relationship,i=`<div class="notice">\uAC00\uC838\uC628 \uC7AC\uB8CC \xB7 ${n.loot===void 0?"\uC774\uC804 \uBC84\uC804 \uBCF4\uACE0 \xB7 \uB0B4\uC5ED \uBBF8\uAE30\uB85D":Object.entries(n.loot).map(([s,r])=>`${We(Z.ING[s]?.n||s)} \xD7${r}`).join(" \xB7 ")||"\uC5C6\uC74C"}${n.promise?`<br>\uC57D\uC18D: ${We(Z.ING[n.promise].n)} \xB7 ${n.promiseDelivered?"\uB450 \uAC1C \uC804\uB2EC\uB428":"\uC774\uBC88\uC5D0\uB294 \uAC00\uC838\uC624\uC9C0 \uBABB\uD588\uC2B5\uB2C8\uB2E4"}`:""}${n.maxAffinityBonus&&n.result.tier!=="hurt"?"<br>\u{1F4A1} \uCD9C\uBC1C \uC2DC \uCD5C\uB300 \uD638\uAC10 \uBCF4\uB108\uC2A4 +1 \uD3EC\uD568":""}</div>`;if(!e)return i;i+=`<div class="relationship"><h3>\u{1F4A1} \uD638\uAC10 ${e.before} \u2192 ${e.after} (${hc(e.gain)})</h3><p class="small muted">\uADC0\uD658 \uACB0\uACFC ${hc(e.resultGain)} \xB7 \uB9DB \uBC18\uC751 ${hc(e.tasteGain)} \xB7 0~12 \uBC94\uC704 \uC801\uC6A9</p>${e.level>e.levelBefore?`<p>\uB808\uBCA8 ${e.levelBefore} \u2192 ${e.level}</p>`:""}</div>`;for(let s of e.stories){let r=Vi[s];i+=`<section class="story-entry"><h3>\u{1F4A1} \uC0C8 \uC0AC\uC5F0 \xB7 \uD638\uAC10 ${r.at}</h3><p>${We(r.text)}</p>${r.gift?`<div class="notice">\uC9C4\uC5F4 \uC120\uBC18\uC5D0 \uB0A8\uAE34 \uBB3C\uAC74 \xB7 ${We(r.gift.name)} \u{1F4A1} \uC784\uC2DC \uBAA8\uB378</div>`:""}${rd(s,"\uC774 \uC0AC\uC5F0 \uB2E4\uC2DC \uC77D\uAE30")}</section>`}return e.map&&(i+=`<div class="notice">\u{1F4A1} \uC9C0\uB3C4\uB85C ${e.map.floor}\uCE35\uC758 ${Z.AX[e.map.axis].name} \uC704\uD5D8 ${e.map.value}\uC744 \uC218\uCCA9\uC5D0 \uAE30\uB85D\uD588\uC2B5\uB2C8\uB2E4. \uAD6C\uC870 \uC0AC\uAC74\uC758 \uC704\uCE58 \uB2E8\uC11C\uB294 \uC544\uB2D9\uB2C8\uB2E4.</div>`),i}function cd(n){return`<span class="eyebrow">REGULAR'S STORY</span><h2>${We(n.guestName)} \xB7 \uD638\uAC10 ${n.at}\uC758 \uC774\uC57C\uAE30</h2><p>${We(n.text)}</p>${n.gift?`<p>\uB0A8\uAE34 \uBB3C\uAC74 \xB7 ${We(n.gift.name)}</p>`:""}<div class="notice">\u{1F4A1} \uC0AC\uC5F0 \uCD08\uC548 \xB7 \u{1F534} \uCD5C\uC885 \uB300\uC0AC\xB7\uBB3C\uAC74 \uD6A8\uACFC \uBBF8\uC815. \uB2E4\uC2DC \uC77D\uC5B4\uB3C4 \uBCF4\uC0C1\uC744 \uC911\uBCF5 \uC9C0\uAE09\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.</div><div class="actions"><button data-act="close-dialog" class="primary">\uB3CC\uC544\uAC00\uAE30</button></div>`}var Ae=(n,e,t=!1,i="",s="")=>`<button data-act="${n}" data-id="${i}" class="${s}" ${t?"disabled":""}>${e}</button>`,$t=(n,e)=>`<span class="eyebrow">${n}</span><h2>${e}</h2>`,dc=n=>`<div class="pills">${Ct.filter(e=>n[e]).map(e=>`<span class="pill ax-${e}">${Z.AX[e].eff} ${n[e]}</span>`).join("")}</div>`,mr='<div class="divider"></div>',K0=n=>`<div class="pills">${n.map(e=>`<span class="pill flavor">\uB9DB \xB7 ${We(e)}</span>`).join("")}</div>`,fc=n=>`<div class="taste-info"><span>\uC54C\uC544\uB0B8 \uC785\uB9DB</span><p>\uC88B\uC544\uD568 \xB7 ${(n.knownLikes||[]).map(We).join(" \xB7 ")||"?"}${(n.knownLikes||[]).length<(n.likes||[]).length?" \xB7 ?":""}<br>\uC2EB\uC5B4\uD568 \xB7 ${(n.knownDislikes||[]).map(We).join(" \xB7 ")||"?"}</p></div>`;function hd(n,e){let t=n.s;return t?t.phase==="ending"?s_(t):t.phase==="report"&&e.station!=="book"?n_(n):t.phase==="close"&&e.station!=="book"?Q0(n):e.station==="book"?pd(n,e):e.station==="stairs"?i_(n,e):t.phase==="prep"?ud(n,e):e.station==="kitchen"?t_(n):e.station==="stove"?dd(n):pc(n):'<section class="title-screen"><span class="eyebrow">\uC624\uB298 \uBC24, \uB358\uC804 \uC2DD\uB2F9 \xB7 \u{1F4A1} \uAC00\uC81C</span><h1>\uCE35\uACC4\uCC38 \uC2DD\uB2F9</h1>'+sd()+'<p class="intro">\uB358\uC804\uC758 \uACC4\uB2E8\uCC38\uC5D0 \uC791\uC740 \uC2DD\uB2F9\uC744 \uC5F4\uC5C8\uB2E4.<br>\uC190\uB2D8\uC758 \uB9D0\uC744 \uB4E3\uACE0, \uB3CC\uC544\uC62C \uD798\uC744 \uB2F4\uC740 \uD55C \uB07C\uB97C \uC900\uBE44\uD55C\uB2E4.</p><div class="title-actions">'+Ae("start","\uC601\uC5C5 \uC2DC\uC791",!1,"normal","primary")+Ae("continue","\uC774\uC5B4\uD558\uAE30",!n.normalSave,"normal")+'</div><p class="small muted">\uC6D0\uBCF8\uC758 \uACBD\uC601 \uD750\uB984\uC5D0 3D \uC774\uB3D9\xB7\uC870\uB9AC\xB7\uC11C\uBE59\uC744 \uB354\uD55C \uC2DC\uC81C\uD488<br>\uC601\uC5C5 \uC911 WASD \uC774\uB3D9 \xB7 \uAC00\uAE4C\uC774\uC5D0\uC11C E \xB7 \uB4DC\uB798\uADF8\uB85C \uC2DC\uC810 \uD68C\uC804</p><details class="trial-options"><summary>\u{1F4A1} \uAE30\uD68D \uAC80\uD1A0\uC6A9 \uC2DC\uD5D8 \uBAA8\uB4DC</summary><p class="small muted">5\uCE35 \uC870\uC0AC \uB610\uB294 \uB2E8\uACE8 \uD638\uAC10 2\xB76\xB711\uC5D0\uC11C \uC2DC\uC791\uD569\uB2C8\uB2E4. \uC77C\uBC18 \uC601\uC5C5\uACFC \uB2E4\uB978 \uC800\uC7A5\uC785\uB2C8\uB2E4.</p><div class="actions">'+Ae("start","5\uCE35 \uC870\uC0AC\uBD80\uD130 \uCCB4\uD5D8",!1,"story")+Ae("continue","\uC2DC\uD5D8 \uC774\uC5B4\uD558\uAE30",!n.storySave,"story")+Ae("start","\uB2E8\uACE8 \uC0AC\uC5F0\uBD80\uD130 \uCCB4\uD5D8",!1,"relations")+Ae("continue","\uB2E8\uACE8 \uC2DC\uD5D8 \uC774\uC5B4\uD558\uAE30",!n.relationsSave,"relations")+"</div></details></section>"}function j0(n){return'<div class="recipe-library">'+Z.RECIPES.map(e=>{let t=Object.keys(e.ing).filter(s=>!n.s.discovered[s]);if(t.length)return'<article class="recipe-note locked"><h3>???</h3><p class="small muted">'+Math.min(...t.map(s=>Z.ING[s].floor||1))+"\uCE35 \uC7AC\uB8CC\uB97C \uBC1C\uACAC\uD558\uBA74 \uC5F4\uB9BD\uB2C8\uB2E4.</p></article>";let i=n.recipes.find(s=>s.id===e.id);return'<article class="recipe-note"><div class="row"><h3>'+e.n+'</h3><span class="small muted">'+e.price+"g</span></div>"+dc(i.effects)+K0(e.tags)+'<p class="small muted">'+Object.entries(e.ing).map(([s,r])=>Z.ING[s].n+" "+(n.s.stock[s]||0)+"/"+r).join(" \xB7 ")+"</p></article>"}).join("")+"</div>"}function ud(n,e){let t=n.s,i=["market","recipes","notes","upgrade"].includes(e.prepTab)?e.prepTab:"market",s=t.anomaly?t.anomaly.floor+"\uCE35\uC758 "+Z.AX[t.anomaly.axis].name+" \uC704\uD5D8\uC774 \uC9D9\uC5B4\uC84C\uB2E4\uB294 \uC18C\uC2DD\uC774\uB2E4.":"\uC624\uB298\uC740 \uB358\uC804\uC774 \uC7A0\uC7A0\uD558\uB2E4\uB294 \uC18C\uC2DD\uC774\uB2E4.",r='<div class="phase-news"><span class="eyebrow">'+t.day+"\uC77C\uCC28 \xB7 "+(t.phase==="close"?"\uC601\uC5C5 \uC885\uB8CC \uD6C4 \uC900\uBE44":"\uC624\uB298\uC758 \uC18C\uC2DD")+"</span><p>"+s+'</p></div><div class="tabs prep-tabs">'+[["market","\uC2DC\uC7A5"],["recipes","\uB808\uC2DC\uD53C"],["notes","\uC218\uCCA9"],["upgrade","\uAC00\uAC8C"]].map(([o,a])=>Ae("prep-tab",a,!1,o,i===o?"active":"")).join("")+"</div>";if(i==="market"){let o=c=>'<div class="market-row"><strong>'+c.name+'</strong><span class="small muted">\uBCF4\uC720 '+c.have+"</span><span>"+c.cost+"g</span>"+Ae("buy","+1",t.gold<c.cost,c.id)+Ae("buy-five","+5",t.gold<c.cost*5,c.id)+"</div>",a=[["\uAE30\uBCF8 \uC7AC\uB8CC",n.market.filter(c=>Z.ING[c.id]?.base)],["\uC591\uB150 \xB7 \uC694\uB9AC\uB9C8\uB2E4 \uD6A8\uACFC +1",n.market.filter(c=>Z.SPICES[c.id])],["\uD589\uC0C1\uC778\uC758 \uB358\uC804 \uC7AC\uB8CC \xB7 \uAD6C\uB9E4 \uAC00\uACA9 4\uBC30",n.market.filter(c=>Z.ING[c.id]&&!Z.ING[c.id].base)]];r+='<div class="market-groups">'+a.filter(([,c])=>c.length).map(([c,l])=>'<section class="paper-panel"><h3>'+c+"</h3>"+l.map(o).join("")+"</section>").join("")+"</div>"}else if(i==="recipes")r+=j0(n);else if(i==="notes")r+=pd(n,e);else{let o=Ut.seatCosts[t.seats.length-3],a=Ut.stoveCosts[t.stove],c=t.shelf<2?Ut.shelfCost:null,l=_t.find(h=>!t.seats.includes(h.id));r='<div class="upgrade-context">'+r+'</div><div class="upgrade-list">'+[["\uC88C\uC11D \uB298\uB9AC\uAE30",t.seats.length+"\uC11D","\uD558\uB8E8\uC5D0 \uBC1B\uC744 \uC218 \uC788\uB294 \uC190\uB2D8\uC774 \uD55C \uBA85 \uB298\uC5B4\uB0A9\uB2C8\uB2E4. \u{1F4A1} \uB3D9\uC2DC\uC5D0 \uBA38\uBB3C \uC218 \uC788\uB294 \uC790\uB9AC\uB3C4 \uB298\uC5B4\uB0A9\uB2C8\uB2E4.",o,"upgrade-seat",l?.id],["\uD654\uB355 \uAC1C\uB7C9","+"+t.stove,"\uC694\uB9AC\uC758 \uAC00\uC7A5 \uAC15\uD55C \uD6A8\uACFC +1. \uB450 \uBC88\uC9F8 \uAC1C\uB7C9\uC740 \uADF8\uB2E4\uC74C \uD6A8\uACFC\uB3C4 +1.",a,"upgrade-stove",""],["\uC591\uB150 \uC120\uBC18",t.shelf+"\uCE78","\uC694\uB9AC \uD558\uB098\uC5D0 \uC591\uB150\uC744 \uD558\uB098 \uB354 \uB123\uC744 \uC218 \uC788\uC2B5\uB2C8\uB2E4.",c,"upgrade-shelf",""]].map(([h,u,p,d,g,v])=>'<article class="upgrade-item"><div><h3>'+h+' <span class="small muted">\uD604\uC7AC '+u+'</span></h3><p class="intro">'+p+"</p></div>"+Ae(g,d?d+"g \xB7 \uAC1C\uB7C9":"\uCD5C\uB300 \uB2E8\uACC4",!d||t.gold<d,v??"")+"</article>").join("")+'</div><p class="small muted">\uC6D0\uBCF8 \uAC00\uACA9: \uC88C\uC11D 70/130/200g \xB7 \uD654\uB355 90/180g \xB7 \uC120\uBC18 120g.<br>\u{1F4A1} \uC790\uB9AC\uBCC4 \uD6A8\uACFC\uB294 3D \uC2DC\uC81C\uD488 \uC81C\uC548\uC785\uB2C8\uB2E4. \u{1F534} \uC778\uC811 \uBCF4\uB108\uC2A4\uB294 \uBBF8\uC815\uC785\uB2C8\uB2E4.</p>'}return r+'<div class="day-cta"><span>\uC88C\uC11D '+t.seats.length+"\uC11D \xB7 \uAE08\uD654 <b>"+t.gold+'g</b></span><div class="actions">'+(e.management?Ae("close-dialog","\uC2DD\uB2F9\uC73C\uB85C \uB3CC\uC544\uAC00\uAE30",!1,"","primary"):Ae("open","\uC601\uC5C5 \uC2DC\uC791",!1,"","primary"))+"</div></div>"}function Q0(n){let e=n.s;return $t(e.day+"\uC77C\uCC28 \xB7 \uC601\uC5C5 \uC885\uB8CC","\uAC00\uAC8C \uBB38\uC744 \uB2EB\uB294\uB2E4")+'<div class="daily-summary">'+[["\uBC1B\uC740 \uC190\uB2D8",e.pending.length],["\uB9E4\uCD9C",e.income+"g"],["\uC9C0\uCD9C",(e.spent||0)+"g"],["\uAE08\uACE0",e.gold+"g"]].map(([t,i])=>"<div><span>"+t+"</span><b>"+i+"</b></div>").join("")+'</div><section class="paper-panel"><h3>\uC9C0\uAE08 \uB358\uC804 \uC548\uC5D0 \uC788\uB294 \uC190\uB2D8</h3><div class="departed-list">'+(e.pending.map(t=>"<article>"+cc(e.guests[t.guestId].job,"neutral",{sm:!0})+"<div><b>"+We(t.name)+'</b><p class="small muted">'+t.floor+"\uCE35 \xB7 "+t.recipe+"</p></div></article>").join("")||'<p class="intro">\uC624\uB298\uC740 \uC544\uBB34\uB3C4 \uB0B4\uB824\uAC00\uC9C0 \uC54A\uC558\uB2E4.</p>')+'</div></section><div class="actions">'+Ae("management","\uBC88 \uB3C8\uC73C\uB85C \uAC00\uAC8C \uAC1C\uB7C9",!1,"","primary")+Ae("morning","\uBC24\uC774 \uC9C0\uB098\uACE0 \uB2E4\uC74C \uB0A0\uB85C")+"</div>"}function e_(n,e){let t=We(n);for(let i of Gc[e]||[])t=t.replaceAll(i,'<mark class="hint-word ax-'+(e==="tired"?"vital":e)+'">'+i+"</mark>");return t}function pc(n){let{guest:e,visit:t,s:i}=n;if(!e)return"<p>\uB2E4\uC74C \uD750\uB984\uC744 \uC120\uD0DD\uD558\uC138\uC694.</p>";let s=`<div class="guest-heading">${cc(e.job,"neutral",{tired:e.tired})}<div>${$t("\uC2DD\uD0C1\uC744 \uCC3E\uC740 \uC190\uB2D8",We(e.name))}<span class="small muted">${e.floor}\uCE35 \xB7 ${Z.FLOORS[e.floor].name} \xB7 Lv.${e.level||1}</span></div></div><div class="row"><span class="tag">${$n[e.race].name}</span><span class="tag">${Z.CLASSES[e.job].n}</span><span class="tag ${e.tired?"warn":"good"}">${e.tired?"\uC5B4\uAE68\uAC00 \uCD95 \uCC98\uC838 \uC788\uB2E4":"\uBC1C\uAC78\uC74C\uC774 \uAC00\uBCCD\uB2E4"}</span></div><p class="small muted" style="margin-top:10px">${Z.CLASSES[e.job].trait} \xB7 ${Z.AX[Z.CLASSES[e.job].res].name} \u22121<br>\u{1F4A1} \uC885\uC871 \uC57D\uC810: ${$n[e.race].weak?Z.AX[$n[e.race].weak].name+" +1":"\uC5C6\uC74C"}</p>`+(t.status==="waiting"?`<div class="quote">\u201C${e_(t.opening,t.openingKey)}\u201D</div>`:"")+fc(e)+uc(e)+ad(t);return t.status==="carrying"?s+`<div class="quote">\uC811\uC2DC\uB97C \uB4E4\uACE0 ${_t[t.seat].name} \uC790\uB9AC\uB85C.</div><p class="intro">\uC190\uB2D8 \uAC00\uAE4C\uC774\uC5D0\uC11C E\uB97C \uB204\uB974\uBA74 \uC74C\uC2DD\uC744 \uB0B4\uC5B4 \uC90D\uB2C8\uB2E4. \uAE08\uC0C9 \uC6D0\uC774 \uC804\uB2EC \uC704\uCE58\uB97C \uC54C\uB824 \uC90D\uB2C8\uB2E4.</p><div class="actions">${Ae("serve","\uC190\uB2D8\uC5D0\uAC8C \uC74C\uC2DD \uB0B4\uAE30",!1,"","primary")}</div>`:t.status==="served"?s+`<div class="quote">\u201C${Fc[t.taste?.score??2]}\u201D<br><span class="small muted">\u201C${We(t.feel||"\uB3CC\uC544\uC62C \uAE38\uAE4C\uC9C0 \uC798 \uCC59\uACA8 \uBCFC\uAC8C.")}\u201D</span></div><p class="intro">\uB9DB \uB9CC\uC871 ${t.taste?.score??2}/4 \xB7 ${e.name}\uC5D0\uAC8C ${t.price}g\uB97C \uBC1B\uC558\uC2B5\uB2C8\uB2E4. \uD0D0\uD5D8 \uACB0\uACFC\uB294 \uB0B4\uC77C \uC544\uCE68\uC785\uB2C8\uB2E4.</p>${(t.learnedTaste||[]).map(r=>`<div class="notice">\uC0C8\uB85C \uC54C\uC544\uB0B8 \uCDE8\uD5A5 \xB7 ${We(r.tag)} \uB9DB\uC744 ${r.kind==="like"?"\uC88B\uC544\uD569\uB2C8\uB2E4":"\uC2EB\uC5B4\uD569\uB2C8\uB2E4"}.</div>`).join("")}<div class="actions">${Ae("next",i.visits.filter(Et).length===1?"\uBC30\uC6C5\uD558\uACE0 \uC601\uC5C5 \uB9C8\uAC10":"\uC774 \uC190\uB2D8 \uBC30\uC6C5\uD558\uAE30",!1,"","primary")}</div>`:(t.seat===null?s+='<h3>\uBA3C\uC800 \uC790\uB9AC\uB97C \uC815\uD574 \uC8FC\uC138\uC694</h3><p class="small muted">\uB9D0\uACFC \uC678\uD615\uC5D0\uC11C \uB4DC\uB7EC\uB098\uB294 \uC190\uB2D8\uC758 \uC0C1\uD0DC\uB97C \uACE0\uB824\uD569\uB2C8\uB2E4. \u{1F4A1} \uC774 \uC2DC\uC81C\uD488\uC5D0\uC11C\uB294 \uC870\uB9AC \uC804 \uC790\uB9AC \uBCC0\uACBD\uC774 \uAC00\uB2A5\uD569\uB2C8\uB2E4.</p>':s+=`<div class="quote">\u201C${t.hint}\u201D</div><p class="intro">\uBAA9\uC801\uC9C0 \xB7 <b>${e.floor}\uCE35 ${Z.FLOORS[e.floor].name}</b></p>${t.assignment?'<div class="notice">\uC774\uBC88 \uBC29\uBB38\uC5D0 \uC774\uC57C\uAE30 \uC804\uB2EC\xB7\uC870\uC0AC \uC694\uCCAD\uC774 \uBC30\uC815\uB410\uC2B5\uB2C8\uB2E4.</div>':""}${mr}<h3>\uC120\uD0DD\uD55C \uC790\uB9AC \xB7 ${_t[t.seat].name}</h3>`,s+=`<div class="seat-grid">${_t.map(r=>Ae("seat",`${r.name}<small>${n.s.visits.some(o=>o!==t&&Et(o)&&o.seat===r.id)?"\uB2E4\uB978 \uC190\uB2D8 \uC774\uC6A9 \uC911":i.seats.includes(r.id)?Z.AX[r.axis].eff+" +1":"\uC544\uC9C1 \uC7A0\uAE34 \uC790\uB9AC"}</small>`,!i.seats.includes(r.id)||t.status!=="waiting"||i.visits.some(o=>o!==t&&Et(o)&&o.seat===r.id),r.id,t.seat===r.id?"active":"")).join("")}</div><div class="actions">${Ae("station",t.status==="waiting"?"\uC870\uB9AC\uB300\uB85C":"\uD654\uB355 \uD655\uC778",t.seat===null,t.status==="waiting"?"kitchen":"stove","primary")}${Ae("decline","\uC624\uB298\uC740 \uB3CC\uB824\uBCF4\uB0B4\uAE30",t.status!=="waiting")}</div>`,s)}function t_(n){return n.visit?n.visit.status!=="waiting"?dd(n):n.visit.seat===null?pc(n):$t("A MEAL AS A PRESCRIPTION",We(n.guest.name)+"\uC758 \uD55C \uB07C")+'<div class="quote">'+We(n.visit.hint)+'</div><p class="intro">\uAC00\uC6B4\uB370 \uC870\uB9AC \uB3C4\uAD6C\uC5D0 \uC7AC\uB8CC\uB97C \uD558\uB098\uC529 \uB123\uC5B4 \uC694\uB9AC\uB97C \uC900\uBE44\uD569\uB2C8\uB2E4.</p>'+fc(n.guest)+'<div class="actions">'+Ae("open-cooking","\uC694\uB9AC \uC804\uC6A9 \uD654\uBA74 \uC5F4\uAE30",!1,"","primary")+"</div>":$t("KITCHEN","\uC190\uB2D8\uC744 \uC120\uD0DD\uD574 \uC8FC\uC138\uC694")}function dd(n){let e=n.visit;if(!e)return $t("THE HEARTH","\uC190\uB2D8\uC744 \uC120\uD0DD\uD574 \uC8FC\uC138\uC694");if(e.status==="served"||e.status==="carrying")return pc(n);let t=n.s.visits.filter(s=>["cooking","ready"].includes(s.status)),i=$t("THE HEARTH","\uC644\uC131\uC744 \uAE30\uB2E4\uB9AC\uB294 \uD55C \uB07C")+'<p class="small muted">\uC190\uB2D8\uBCC4\uB85C \uC811\uC2DC\uAC00 \uAD6C\uBD84\uB429\uB2C8\uB2E4. \uD55C \uBC88\uC5D0 \uD55C \uC811\uC2DC\uB97C \uC6B4\uBC18\uD569\uB2C8\uB2E4.</p>';return i+=t.map(s=>'<div class="order-card">'+Ae("select-order",We(n.s.guests[s.guestId].name)+" \xB7 "+Z.RECIPES.find(r=>r.id===s.recipe).n+"<small>"+As[s.status]+"</small>",!1,s.id,s.id===e.id?"active":"")+'<div class="progress"><div data-cook-progress="'+s.id+'" style="width:'+(s.status==="ready"?100:0)+'%"></div></div></div>').join(""),["ready","cooking"].includes(e.status)&&(i+='<div class="actions">'+Ae("pickup",e.status==="ready"?n.guest.name+"\uC758 \uC811\uC2DC \uC9D1\uAE30":"\uC120\uD0DD\uD55C \uC694\uB9AC \uC870\uB9AC \uC911\u2026",e.status!=="ready"||!!n.held,"","primary")+"</div>"),t.length||(i+='<p class="empty">\uC644\uC131 \uB300\uAE30 \uC911\uC778 \uC74C\uC2DD\uC774 \uC5C6\uC2B5\uB2C8\uB2E4.</p>'),i+'<div class="actions">'+Ae("station","\uC870\uB9AC\uB300\uB85C",e.seat===null,"kitchen")+"</div>"}function fd(n){let e=n.s.visits.filter(Et);return'<section class="service-roster" aria-label="\uC2DD\uB2F9 \uC548 \uC190\uB2D8"><div class="row"><h3>\uC2DD\uB2F9 \uC548 \uC190\uB2D8 \xB7 '+e.length+'\uBA85</h3><small>\uC120\uD0DD\uD574\uC11C \uC751\uB300</small></div><div class="roster-grid">'+e.map(t=>Ae("select-visit",We(n.s.guests[t.guestId].name)+"<small>"+(t.seat===null?"\uC785\uC7A5 \uB300\uAE30":t.seat+1+"\uBC88 \uC790\uB9AC")+" \xB7 "+As[t.status]+"</small>",!1,t.id,t===n.visit?"active":"")).join("")+"</div>"+(n.held?'<p class="carry-notice">\uC6B4\uBC18 \uC911 \xB7 '+We(n.s.guests[n.held.guestId].name)+"\uC758 \uC811\uC2DC</p>":"")+'<div class="actions">'+Ae("ask-close","\uC624\uB298 \uC601\uC5C5 \uB9C8\uCE58\uAE30")+"</div></section>"}function mc(n,e){return'<span class="eyebrow">AFTER SERVICE \xB7 \uC624\uB298 \uB9E4\uCD9C '+n.s.income+'g</span><h2>\uBC88 \uB3C8\uC73C\uB85C, \uB0B4\uC77C\uC744 \uC900\uBE44\uD55C\uB2E4</h2><p class="manage-gold">\uBCF4\uC720 \uACE8\uB4DC <strong>'+n.s.gold+"g</strong></p>"+ud(n,{...e,management:!0})}function n_(n){let e=n.report;if(!e)return"<p>\uADC0\uD658 \uC815\uC0B0\uC744 \uB9C8\uCCE4\uC2B5\uB2C8\uB2E4.</p>";let t=e.result;return $t(`MORNING \xB7 ${n.s.reportIndex+1}/${n.s.reports.length}`,`${We(e.name)}\uC758 \uADC0\uD658`)+`<div class="result-title ${t.tier}">${Dc[t.tier]}</div><p class="intro">${e.floor}\uCE35 \xB7 ${e.recipe}<br>${t.tier==="hurt"?"\uB2E4\uCE5C \uACF3\uC744 \uBA3C\uC800 \uB3CC\uBD24\uB2E4. \uD68C\uBCF5\uD558\uB294 \uB3D9\uC548 \uC7A0\uC2DC \uC26C\uC5B4 \uAC00\uAE30\uB85C \uD588\uB2E4.":t.tier==="narrow"?"\uAC04\uC2E0\uD788 \uB3CC\uC544\uC654\uB2E4. \uC5B4\uB514\uC11C \uC900\uBE44\uAC00 \uBAA8\uC790\uB790\uB294\uC9C0 \uD568\uAED8 \uC9DA\uB294\uB2E4.":"\uC900\uBE44\uD55C \uD55C \uB07C \uB355\uBD84\uC5D0 \uBB34\uC0AC\uD788 \uB3CC\uC544\uC654\uB2E4."}</p><div class="notice">${t.surprise?t.absorbed?"\uB3CC\uBC1C \uC0C1\uD669\uC744 \uB0A8\uACA8 \uB454 \uD65C\uB825\uC73C\uB85C \uBC84\uD17C\uC2B5\uB2C8\uB2E4.":`\uB3CC\uBC1C \xB7 ${Z.AX[t.surprise].name} \uC704\uD5D8 +1`:"\uC774\uBC88 \uD0D0\uD5D8\uC5D0\uB294 \uB3CC\uBC1C \uC0C1\uD669\uC774 \uC5C6\uC5C8\uC2B5\uB2C8\uB2E4."}</div>${ld(e)}${e.newIngredients?.length?`<div class="notice">\uC0C8 \uC7AC\uB8CC \xB7 ${e.newIngredients.map(i=>Z.ING[i].n).join(" \xB7 ")}</div>`:""}${e.newRecipes?.length?`<div class="notice">\uC0C8 \uB808\uC2DC\uD53C \xB7 ${e.newRecipes.map(i=>Z.RECIPES.find(s=>s.id===i).n).join(" \xB7 ")}</div>`:""}${e.newFloor?`<div class="notice">${e.newFloor}\uCE35 ${Z.FLOORS[e.newFloor].name}\uC73C\uB85C \uAC00\uB294 \uAE38\uC774 \uC5F4\uB838\uC2B5\uB2C8\uB2E4.</div>`:""}<table><thead><tr><th>\uC704\uD5D8 / \uD6A8\uACFC</th><th>\uC704\uD5D8</th><th>\uD6A8\uACFC</th><th>\uBD80\uC871</th></tr></thead><tbody>${Ct.map(i=>`<tr><td>${Z.AX[i].name} / ${Z.AX[i].eff}</td><td class="num">${t.H[i]}</td><td class="num">${t.E[i]}</td><td class="num ${t.gaps[i]?"missing":""}">${t.gaps[i]}</td></tr>`).join("")}</tbody></table><p class="small muted" style="margin-top:12px">\uBD80\uC871 \uD569\uACC4 ${t.deficit} \xB7 \uBCF4\uC0C1\uACFC \uBC1C\uACAC\uD55C \uC0AC\uC2E4\uC740 \uC774\uBBF8 \uC800\uC7A5\uB418\uC5C8\uC2B5\uB2C8\uB2E4. \uB2E4\uC2DC \uC5F4\uC5B4\uB3C4 \uC911\uBCF5 \uC9C0\uAE09\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.</p><div class="actions">${Ae("report-next",n.s.reportIndex===n.s.reports.length-1?"\uAC00\uAC8C \uC900\uBE44\uD558\uAE30":"\uB2E4\uC74C \uADC0\uD658 \uBCF4\uACE0",!1,"","primary")}${n.s.reportIndex<n.s.reports.length-1?Ae("report-all","\uBAA8\uB450 \uD655\uC778\uD558\uACE0 \uAC00\uAC8C \uC900\uBE44"):""}</div>`}function pd(n,e){let t=n.s,i=$t("THE NOTEBOOK","\uB3CC\uC544\uC628 \uC774\uC57C\uAE30\uB4E4")+`<div class="tabs">${Ae("book-tab","\uC0AC\uAC74 \uB2E8\uC11C",!1,"story",e.bookTab==="story"?"active":"")}${Ae("book-tab","\uCE35 \uAE30\uB85D",!1,"floors",e.bookTab==="floors"?"active":"")}${Ae("book-tab","\uB2E8\uACE8 \uC0AC\uC5F0",!1,"regulars",e.bookTab==="regulars"?"active":"")}${Ae("book-tab","\uC190\uB2D8 \uAE30\uB85D",!1,"guests",e.bookTab==="guests"?"active":"")}${Ae("book-tab","\uC0AC\uC804",!1,"dict",e.bookTab==="dict"?"active":"")}</div>`;if(e.bookTab==="regulars")i+=od(n);else if(e.bookTab==="dict")i+='<div class="dictionary">'+Ct.map(s=>'<div class="clue"><span class="pill ax-'+s+'">'+Z.AX[s].name+" \u2192 "+Z.AX[s].eff+"</span><p>"+Z.AX[s].words+"</p></div>").join("")+'<p class="small muted">\uD53C\uACE4\uD55C \uC190\uB2D8\uC740 \uCCB4\uB825 \uC704\uD5D8 +1. \uC704\uD5D8\uACFC \uD6A8\uACFC\uB294 \uAC19\uC740 \uC885\uB958\uB07C\uB9AC \uB300\uC751\uD569\uB2C8\uB2E4. \uD65C\uB825 \uC5EC\uC720\uB294 \uB3CC\uBC1C \uC0C1\uD669\uC5D0 \uB300\uBE44\uD569\uB2C8\uB2E4.</p></div>';else if(e.bookTab==="floors")for(let s=1;s<=Math.max(t.floor,...Object.keys(t.known).map(Number));s++)i+=`<div class="clue"><b>${s}\uCE35 \xB7 ${Z.FLOORS[s].name}${s>t.floor?" \xB7 \u{1F4A1} \uC9C0\uB3C4 \uBBF8\uB9AC\uBCF4\uAE30":""}</b><p>${Ct.filter(r=>(Z.FLOORS[s].haz[r]||0)>0).map(r=>t.known[s]?.[r]===void 0?"? \uC544\uC9C1 \uBAA8\uB974\uB294 \uC704\uD5D8":Z.AX[r].name+" "+t.known[s][r]).join(" \xB7 ")}</p></div>`;else if(e.bookTab==="guests")i+=Object.values(t.guests).map(s=>`<div class="clue"><b>${We(s.name)}</b><p>${$n[s.race].name} \xB7 ${Z.CLASSES[s.job].n} \xB7 \uBC29\uBB38 ${s.visits}\uD68C<br>${s.restUntil>t.day?"\uD68C\uBCF5 \uC911 \xB7 "+(s.restUntil-t.day)+"\uC77C \uB4A4 \uBC29\uBB38 \uAC00\uB2A5":"\uB2E4\uC2DC \uB9CC\uB0A0 \uC218 \uC788\uB294 \uC190\uB2D8"}</p>${fc(s)}</div>`).join("")||'<p class="empty">\uC544\uC9C1 \uC190\uB2D8\uC774 \uC5C6\uC2B5\uB2C8\uB2E4.</p>';else{if(i+='<p class="intro">5\uCE35\uC740 \uB05D\uC774 \uC544\uB2C8\uB2E4. \uADF8 \uC5B4\uB518\uAC00\uC5D0, \uC544\uC9C1 \uD655\uC778\uD558\uC9C0 \uBABB\uD55C \uC774\uC57C\uAE30\uAC00 \uC788\uB2E4.</p>',i+=t.story.clues.length?t.story.clues.map(s=>`<div class="clue"><b>${Lo[s].name}</b><p>${Lo[s].text}</p></div>`).join(""):'<div class="empty">5\uCE35\uC744 \uB2E4\uB140\uC628 \uC190\uB2D8\uC774 \uC61B \uD30C\uD2F0\uC758 \uD754\uC801\uC744 \uAC00\uC838\uC62C \uC218 \uC788\uC2B5\uB2C8\uB2E4. \uB300\uC131\uACF5 \uD69F\uC218\uB9CC\uC73C\uB85C \uC0AC\uAC74\uC774 \uD574\uACB0\uB418\uC9C0\uB294 \uC54A\uC2B5\uB2C8\uB2E4.</div>',t.story.clues.includes("gap")&&!t.story.afterReturn){i+=`${mr}<h3>5\uCE35\uC5D0 \uBD80\uD0C1\uD560 \uC870\uC0AC</h3><p class="small muted">\uC900\uBE44 \uC2DC\uAC04\uC5D0 \uC694\uCCAD\uD558\uBA74 \uB2E4\uC74C \uC801\uACA9 \uC190\uB2D8\uC5D0\uAC8C \uBC30\uC815\uB429\uB2C8\uB2E4. \uB3CC\uB824\uBCF4\uB0B8 \uC694\uCCAD\uC740 \uB2E4\uC74C \uC601\uC5C5\uC5D0 \uB2E4\uC2DC \uBC30\uC815\uB429\uB2C8\uB2E4.</p>`;for(let[s,r]of[["question","\uD604\uC7AC \uC9C8\uBB38 \uC804\uB2EC"],["survey","\uC678\uBD80 \uACBD\uB85C \uC870\uC0AC"]]){let o=Object.values(t.story.requests).find(a=>a.kind===s);i+=`<div class="actions">${Ae("request",o?o.done?r+" \xB7 \uC644\uB8CC":r+" \xB7 \uB2F5\uBCC0 \uB300\uAE30":r,!!o||t.phase!=="prep",s)}</div>`}}i+=`${mr}<h3>\uD655\uC778\uD55C \uC7A5\uBA74</h3><div class="stack">${t.story.done.map(s=>Ae("replay",xn[s].title,!1,s,"ghost")).join("")||'<p class="small muted">\uADC0\uD658 \uBCF4\uACE0 \uD6C4 \uC548\uC804\uD55C \uC544\uCE68\uC5D0 \uC7A5\uBA74\uC744 \uD655\uC778\uD569\uB2C8\uB2E4.</p>'}</div><div class="notice">\u{1F4A1} \uD55C \uC544\uCE68\uC5D0 \uC774\uC57C\uAE30 \uC7A5\uBA74 \uD558\uB098. \uC0DD\uB7B5\uD574\uB3C4 \uC0AC\uC2E4\uC740 \uB0A8\uC2B5\uB2C8\uB2E4. \uAE30\uAC04\xB7\uC0DD\uC874 \uC7AC\uB8CC\xB7\uC9C0\uD615\xB7\uD30C\uD2F0 \uAD6C\uC131\uC740 \u{1F534} \uBBF8\uC815\uC785\uB2C8\uB2E4.</div>`}return i+(t.phase==="prep"||e.management?"":`<div class="actions">${Ae("return-service",t.phase==="report"?"\uADC0\uD658 \uBCF4\uACE0\uB85C":"\uC2DD\uB2F9\uC73C\uB85C",!1,"","primary")}</div>`)}function i_(n,e){let t=n.s;if(t.story.afterReturn)return $t("AFTER THE RETURN","\uC544\uB798\uB85C \uC774\uC5B4\uC9C0\uB294 \uAE38")+`<p class="intro">\uB3D9\uB8CC\uB4E4\uACFC \uD568\uAED8 \uB3CC\uC544\uC654\uB2E4. \uB358\uC804\uC740 \uB354 \uAE4A\uC774 \uC774\uC5B4\uC9C0\uACE0, \uC2DD\uB2F9\uC740 \uC624\uB298\uB3C4 \uB204\uAD70\uAC00\uC758 \uADC0\uD658\uC744 \uAE30\uB2E4\uB9B0\uB2E4.</p><div class="notice">\u{1F4A1} \uC5D4\uB529 A\uC548 \uC2DC\uD5D8 \uC0C1\uD0DC\uC785\uB2C8\uB2E4. \uC2E4\uC81C \uCD94\uAC00 \uCE35\uC740 \uC774 \uC2DC\uC81C\uD488\uC5D0 \uAD6C\uD604\uD558\uC9C0 \uC54A\uC558\uC2B5\uB2C8\uB2E4.</div><div class="actions">${Ae("ending-replay","\uD6C4\uC77C\uB2F4 \uB2E4\uC2DC \uBCF4\uAE30")}${Ae("station","\uC601\uC5C5\uC73C\uB85C",!1,"door","primary")}</div>`;if(!t.previewEnabled)return $t("THE STAIRS","\uC774\uBC88\uC5D4, \uB3CC\uC544\uAC08 \uBAAB\uAE4C\uC9C0")+`<p class="intro">\uB0B4 \uC2DD\uC0AC\uC640 \uB3D9\uB8CC\uB4E4\uC758 \uB3C4\uC2DC\uB77D\uB9CC\uC73C\uB85C\uB294 \uCDA9\uBD84\uD558\uC9C0 \uC54A\uB2E4. \uC0B4\uC544 \uC788\uB2E4\uB294 \uB2F5\uC7A5, \uC11C\uB85C \uB9DE\uB294 \uC9C0\uB3C4, \uB3CC\uC544\uC62C \uACBD\uB85C\uC640 \uC7A5\uBE44\uAC00 \uD544\uC694\uD558\uB2E4.</p><div class="notice">\u{1F534} \uC790\uAE30 \uCC98\uBC29 \uC0C1\uC138 \uAE30\uC900\uACFC \uC5D4\uB529 \uC815\uCC45\uC740 \uBBF8\uC815\uC774\uB77C \uCD9C\uBC1C\uC774 \uC7A0\uACA8 \uC788\uC2B5\uB2C8\uB2E4. \uC601\uC5C5\uACFC \uB2E8\uC11C \uC870\uC0AC\uB294 \uACC4\uC18D\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.</div><p class="small muted">\uCD94\uCC9C A\uC548\uC744 \uAC80\uD1A0\uD558\uB824\uBA74 \uC544\uB798 \uC2DC\uD5D8 \uC124\uC815\uC744 \uCF1C\uC138\uC694. \uACF5\uC2DD \uAE30\uD68D\uC744 \uD655\uC815\uD558\uB294 \uB3D9\uC791\uC740 \uC544\uB2D9\uB2C8\uB2E4.</p><div class="actions">${Ae("enable-preview","\u{1F4A1} \uC5D4\uB529 A\uC548 \uC2DC\uD5D8 \uC124\uC815 \uCF1C\uAE30",t.phase!=="prep","","primary")}</div>`;let i=$t("ENDING A \xB7 EXPERIMENT","\uB098\uC5D0\uAC8C \uB0B4\uB9AC\uB294 \uCC98\uBC29")+`<div class="notice">\u{1F4A1} \uC784\uC2DC \uC2DC\uD5D8\uAC12: \uC5B4\uB460 2 \xB7 \uB3C5 1 \xB7 \uCCB4\uB825 2, \uD65C\uB825 \uC5EC\uC720 1, \uBCF4\uAE09\uBE44 12g. \uC790\uB9AC \uD6A8\uACFC \uC81C\uC678. \uB3D9\uB8CC \uC778\uC6D0\xB7\uC2E4\uC81C \uB3C4\uC2DC\uB77D\xB7\uAD6C\uC870 \uC7A5\uBE44 \uAE30\uC900\uC744 \uB300\uC2E0 \uD655\uC815\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.</div><p class="small muted">\uD544\uC218 \uC0AC\uC2E4 ${t.story.clues.length}/6 \xB7 \uC704\uCE58 \uC7A5\uBA74 ${t.story.done.includes("ISOLATED_AREA_LOCATED")?"\uD655\uC778":"\uBBF8\uD655\uC778"}</p><div class="actions">${Ae("plan",t.story.plan?"\uC2DC\uD5D8 \uACBD\uB85C\xB7\uC7A5\uBE44 \uACC4\uD68D \uD655\uC778\uB428":"\uC2DC\uD5D8 \uACBD\uB85C\xB7\uC7A5\uBE44 \uACC4\uD68D \uD655\uC778",!t.story.clues.includes("outer")||!t.story.clues.includes("inner")||t.phase!=="prep")}</div>${mr}<h3>\uC790\uAE30 \uC2DD\uC0AC \uC870\uD569 \uC120\uD0DD</h3><div class="stack">${n.recipes.map(s=>Ae("self-recipe",`<strong>${s.n}</strong>${dc(s.eff)}`,!s.cookable,s.id,`recipe ${e.selfRecipe===s.id?"active":""}`)).join("")}</div>`;return e.selfRecipe&&(i+=`${mr}<h3>\uC591\uB150 ${e.selfSpices.length}/${t.shelf}</h3><div class="spices">${Object.entries(Z.SPICES).map(([s,r])=>Ae("self-spice",`${r.n}<small>${Z.AX[r.ax].eff} +1</small>`,e.selfSpices.length>=t.shelf||(t.stock[s]||0)<=e.selfSpices.filter(o=>o===s).length,s)).join("")}</div><p class="small muted">${e.selfSpices.map(s=>Z.SPICES[s].n).join(" + ")||"\uC591\uB150 \uC5C6\uC74C"}</p>${Ae("self-clear","\uC591\uB150 \uBE44\uC6B0\uAE30")}${dc(n.readiness.E)}<ul class="checklist">${n.readiness.reasons.map(s=>`<li>${s}</li>`).join("")||"<li>\uC2DC\uD5D8\uC6A9 \uB300\uBE44 \uAE30\uC900\uC744 \uB9CC\uC871\uD569\uB2C8\uB2E4.</li>"}</ul><div class="actions">${Ae("prepare","\uC790\uAE30 \uC2DD\uC0AC\xB7\uB3C4\uC2DC\uB77D \uC900\uBE44 \uD655\uC778",!n.readiness.ok,"","primary")}</div>`),t.prepared&&(i+=`<div class="notice">\uC900\uBE44\uAC00 \uAC80\uC99D\uB418\uC5C8\uC2B5\uB2C8\uB2E4. \uCD9C\uBC1C \uC2DC \uC2DD\uC0AC \uC7AC\uB8CC\uC640 \uBCF4\uAE09\uBE44\uB97C \uD55C \uBC88 \uC18C\uBAA8\uD569\uB2C8\uB2E4.</div><div class="actions">${Ae("descend","\uACC4\uB2E8\uC744 \uB0B4\uB824\uAC04\uB2E4",!1,"","primary")}</div>`),i}function s_(n){let e=n.endingStep==="depart";return $t("ENDING A \xB7 PROPOSAL",e?"\uC624\uB298\uC740, \uB3CC\uC544\uC62C \uBAAB\uAE4C\uC9C0.":"\uB2E4\uB140\uC654\uC5B4.")+`<p class="intro">${e?"\uC790\uC2E0\uC744 \uC704\uD55C \uC2DD\uC0AC\uC640 \uB3D9\uB8CC\uB4E4\uC758 \uB3C4\uC2DC\uB77D\uC744 \uCC59\uAE34\uB2E4. \uD655\uC778\uD55C \uACBD\uB85C\uC640 \uC900\uBE44\uD55C \uC7A5\uBE44\uB97C \uB2E4\uC2DC \uC0B4\uD53C\uACE0, \uCC98\uC74C\uC73C\uB85C \uC790\uC2E0\uC758 \uBC1C\uB85C \uACC4\uB2E8\uC744 \uB0B4\uB824\uAC04\uB2E4.":"\u{1F4A1} \uAD6C\uC870\uC640 \uADC0\uD658\uC740 \uC9E7\uC740 \uD6C4\uC77C\uB2F4\uC73C\uB85C \uC804\uB2EC\uD558\uB294 \uC548\uC785\uB2C8\uB2E4. \uB3D9\uB8CC\uB4E4\uACFC \uD568\uAED8 \uB3CC\uC544\uC628 \uB4A4, \uC8FC\uC778\uACF5\uC740 \uC2DD\uB2F9\uC758 \uBD88\uC744 \uB2E4\uC2DC \uCF2D\uB2C8\uB2E4. \uADF8\uB4E4\uC774 \uD68C\uBCF5\uD558\uB294 \uB3D9\uC548\uC5D0\uB3C4 \uB204\uAD70\uAC00\uB294 \uB3CC\uC544\uC62C \uD55C \uB07C\uB97C \uAE30\uB2E4\uB9BD\uB2C8\uB2E4."}</p><div class="end-word">${e?"\u201C\uC774\uBC88\uC5D0\uB294<br>\uD568\uAED8 \uB3CC\uC544\uC624\uC790.\u201D":"\uB3CC\uC544\uC628 \uB4A4\uC5D0\uB3C4,<br>\uC2DD\uD0C1\uC740 \uACC4\uC18D\uB41C\uB2E4."}</div><p class="small muted">\u{1F4A1} \uC2E4\uC81C \uAD6C\uC870 \uC808\uCC28\xB7\uACBD\uACFC \uAE30\uAC04\xB7\uB3D9\uB8CC \uC678\uD615\uC740 \uBBF8\uC815. \uC9C1\uC811 \uC870\uC791\uD558\uB294 \uAD6C\uC870\xB7\uC804\uD22C\uB294 \uD3EC\uD568\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.</p><div class="actions">${Ae("ending-next",e?"\u{1F4A1} \uADC0\uD658 \uD6C4\uC77C\uB2F4 \uD655\uC778":"\uC2DD\uB2F9\uC73C\uB85C \uB3CC\uC544\uC640 \uC601\uC5C5 \uACC4\uC18D",!1,"","primary")}</div>`}function md(n){let e=xn[n];return`${$t("A STORY AT THE TABLE",e.title)}<p>${e.text}</p><p class="small muted">\u{1F4A1} \uC0AC\uAC74 \uB300\uC0AC \uCD08\uC548 \xB7 \uD655\uC778\uD558\uAC70\uB098 \uC0DD\uB7B5\uD574\uB3C4 \uB2E8\uC11C\uC640 \uBCF4\uC0C1\uC740 \uC720\uC9C0\uB429\uB2C8\uB2E4.</p><div class="actions">${Ae("scene-done","\uC0DD\uB7B5\uD558\uACE0 \uAE30\uB85D \uB0A8\uAE30\uAE30",!1,n)}${Ae("scene-done","\uC774\uC57C\uAE30 \uD655\uC778",!1,n,"primary")}</div>`}function gd(n){return $t("HELP & SAVE","\uC2DD\uB2F9\uC744 \uB2E4\uB8E8\uB294 \uBC95")+`<ol class="checklist"><li>\uC900\uBE44 \uD654\uBA74\uC5D0\uC11C \uC2DC\uC7A5\xB7\uB808\uC2DC\uD53C\xB7\uC218\uCCA9\xB7\uAC00\uAC8C\uB97C \uC0B4\uD540 \uB4A4 \uC601\uC5C5\uC744 \uC2DC\uC791\uD558\uC138\uC694.</li><li>\uC190\uB2D8 \uBAA9\uB85D\uC73C\uB85C \uC751\uB300 \uB300\uC0C1\uC744 \uBC14\uAFB8\uACE0 \uC790\uB9AC\uB97C \uC548\uB0B4\uD558\uC138\uC694. \uC870\uB9AC\uB300\uC758 \uC804\uC6A9 \uD654\uBA74\uC5D0\uC11C \uC7AC\uB8CC\uB97C \uB123\uC73C\uC138\uC694.</li><li>\uD654\uB355\uC5D0\uC11C E\uB85C \uC811\uC2DC\uB97C \uC9D1\uACE0, \uC190\uB2D8 \uC790\uB9AC\uB85C \uC774\uB3D9\uD574 E\uB85C \uB0B4\uC5B4 \uC8FC\uC138\uC694.</li><li>\uBAA8\uB4E0 \uC190\uB2D8\uC744 \uB9DE\uACE0 \uB9C8\uAC10\uD55C \uB4A4 \uB2E4\uC74C \uC544\uCE68 \uACB0\uACFC\uB97C \uD655\uC778\uD558\uC138\uC694.</li><li>5\uCE35\uC758 \uB2E8\uC11C\uAC00 \uC0DD\uAE30\uBA74 \uC218\uCCA9\uC5D0\uC11C \uC9C8\uBB38\xB7\uC870\uC0AC\uB97C \uC694\uCCAD\uD558\uC138\uC694.</li></ol><p class="small muted">\uAC01 \uB3D9\uC791 \uB4A4 \uC774 \uBE0C\uB77C\uC6B0\uC800\uC5D0 \uC800\uC7A5\uD569\uB2C8\uB2E4. \uC77C\uBC18 \uC601\uC5C5\xB75\uCE35 \uC2DC\uD5D8\xB7\uB2E8\uACE8 \uC2DC\uD5D8\uC740 \uC11C\uB85C \uB2E4\uB978 \uC800\uC7A5 \uC2AC\uB86F\uC785\uB2C8\uB2E4. \uD30C\uC77C \uC2E4\uD589\uACFC localhost \uC2E4\uD589\uC740 \uC800\uC7A5 \uACF5\uAC04\uC774 \uB2E4\uB97C \uC218 \uC788\uC2B5\uB2C8\uB2E4.</p><div class="notice">\u{1F4A1} \uC5F4\uB9B0 \uC790\uB9AC \uC218\uB9CC\uD07C 3~6\uBA85\uC744 \uB3D9\uC2DC\uC5D0 \uBC1B\uC2B5\uB2C8\uB2E4. \uC7AC\uB8CC\uB97C \uB123\uC5B4 \uC870\uB9AC\uD558\uACE0 \uD55C \uC811\uC2DC\uC529 \uC6B4\uBC18\uD569\uB2C8\uB2E4. \uD654\uAD6C 1\uAC1C\xB7\uB3C4\uAD6C \uBC30\uC815\xB7\uC0C1\uD0DC \uB300\uC0AC\uB294 \uC2DC\uD5D8\uAC12\uC785\uB2C8\uB2E4. \uB9C8\uAC10\uACFC \uC601\uC5C5 \uC900\uBE44\uC5D0 \uAC00\uAC8C \uAC1C\uB7C9 \uCC3D\uC744 \uC5F4 \uC218 \uC788\uC2B5\uB2C8\uB2E4. \u{1F534} \uC785\uC7A5 \uAC04\uACA9\xB7\uC778\uB0B4\uC2EC\xB7\uCD5C\uC885 \uB300\uC0AC\xB7\uBB3C\uAC74 \uD6A8\uACFC\xB7\uC778\uC811 \uD6A8\uACFC\xB76\uCE35 \uC774\uC0C1\uC740 \uBBF8\uC815\uC785\uB2C8\uB2E4.</div>${n?`<p class="small muted">${n.mode==="story"?"5\uCE35 \uC870\uC0AC \uC2DC\uD5D8":n.mode==="relations"?"\u{1F4A1} \uB2E8\uACE8 \uC0AC\uC5F0 \uC2DC\uD5D8":"\uC77C\uBC18 \uC601\uC5C5"} \xB7 ${n.day}\uC77C\uCC28 \xB7 \uD655\uC778\uD55C \uB2E8\uC11C ${n.story.clues.length}\uAC1C</p><div class="actions">${Ae("export","\uD604\uC7AC \uC800\uC7A5 \uB0B4\uB824\uBC1B\uAE30")}${Ae("title","\uC2DC\uC791 \uD654\uBA74")}</div>`:""}<div class="actions">${Ae("close-dialog","\uB3CC\uC544\uAC00\uAE30",!1,"","primary")}</div>`}var To=(n,e,t,i,s)=>({id:n,minX:e-i/2,maxX:e+i/2,minZ:t-s/2,maxZ:t+s/2}),_d=[...nd.map(n=>To(n.id,n.x,n.z,n.width,n.depth)),...bo.map((n,e)=>To("curtain-"+e,n.x,n.z,n.width,n.depth)),..._t.flatMap(n=>[To("table-"+n.id,n.x,n.z,1.36,1.36),To("chair-"+n.id,n.x+1.02,n.z,.66,.68)])];function wo(n,e=en.radius){return!Number.isFinite(n?.x)||!Number.isFinite(n?.z)||n.x<An.minX+e||n.x>An.maxX-e||n.z<An.minZ+e||n.z>An.maxZ-e?!1:!_d.some(t=>n.x>t.minX-e&&n.x<t.maxX+e&&n.z>t.minZ-e&&n.z<t.maxZ+e)}function xd(n){return wo(n.player)||(n.player={...en.spawn}),Number.isFinite(n.player.yaw)||(n.player.yaw=0),n.player}function r_(n,e,t){let i=Math.hypot(n,e);if(!i)return{x:0,z:0};let s=n/i,r=e/i;return{x:s*Math.cos(t)+r*Math.sin(t),z:-s*Math.sin(t)+r*Math.cos(t)}}function vd(n,e,t,i,s){let r=r_(e,t,s),o=Math.min(en.maxFrameSeconds,Math.max(0,i)),a=en.speed*o,c=Math.max(1,Math.ceil(a/.08)),l={...n};for(let h=0;h<c;h++){let u=l.x+r.x*a/c,p=l.z+r.z*a/c;wo({x:u,z:p})?(l.x=u,l.z=p):(wo({x:u,z:l.z})&&(l.x=u),wo({x:l.x,z:p})&&(l.z=p))}return Math.hypot(l.x-n.x,l.z-n.z)>1e-6&&(l.yaw=Math.atan2(r.x,r.z)),l}function qn(n){let e=Object.values(td).map(t=>({...t}));if(n?.phase==="report"&&e.push({id:"guest",name:"\uADC0\uD658 \uC190\uB2D8",...So}),n?.phase==="service"){let t=n.visits[n.index];for(let i of n.visits.filter(Et)){let s=i.seat===null?Sr(i.waitSlot):{x:_t[i.seat].x+1.05,z:_t[i.seat].z-.95};e.push({id:"guest:"+i.id,name:n.guests[i.guestId].name,...s})}if(t?.status==="waiting")for(let i of _t)n.seats.includes(i.id)&&!n.visits.some(s=>s!==t&&Et(s)&&s.seat===i.id)&&e.push({id:"seat:"+i.id,name:i.name,x:i.x-.25,z:i.z-1.1})}return e}function a_(n,e){let t=Math.ceil(Math.hypot(e.x-n.x,e.z-n.z)/.08);for(let i=1;i<t;i++){let s={x:n.x+(e.x-n.x)*i/t,z:n.z+(e.z-n.z)*i/t};if(_d.some(r=>s.x>r.minX&&s.x<r.maxX&&s.z>r.minZ&&s.z<r.maxZ))return!1}return!0}function gr(n,e){let t=qn(n).find(i=>i.id===e);return t?Math.hypot(n.player.x-t.x,n.player.z-t.z):1/0}function Ao(n,e){let t=qn(n).find(i=>i.id===e);return!!t&&gr(n,e)<=en.reach&&a_(n.player,t)}function gc(n){return qn(n).filter(e=>Ao(n,e.id)).sort((e,t)=>gr(n,e.id)-gr(n,t.id))[0]||null}var Ro=class{constructor(e,t){this.keys=new Set,this.onInteract=e,this.canControl=t,this.events=new AbortController;let i={signal:this.events.signal};this.codes=new Set(["KeyW","KeyA","KeyS","KeyD","ArrowUp","ArrowLeft","ArrowDown","ArrowRight"]),document.addEventListener("keydown",s=>{!this.canControl()||s.ctrlKey||s.metaKey||s.altKey||s.target.closest('input,textarea,select,[contenteditable="true"]')||(this.codes.has(s.code)&&(s.preventDefault(),this.keys.add(s.code)),s.code==="KeyE"&&(s.preventDefault(),s.repeat||this.onInteract()))},i),document.addEventListener("keyup",s=>this.keys.delete(s.code),i),window.addEventListener("blur",()=>this.clear(),i),document.addEventListener("visibilitychange",()=>{document.hidden&&this.clear()},i),document.querySelectorAll("[data-move]").forEach(s=>{s.addEventListener("pointerdown",r=>{this.canControl()&&(r.preventDefault(),s.setPointerCapture(r.pointerId),this.keys.add(s.dataset.move))},i);for(let r of["pointerup","pointercancel","lostpointercapture"])s.addEventListener(r,()=>this.keys.delete(s.dataset.move),i)})}clear(){this.keys.clear()}axes(){if(!this.canControl())return this.clear(),{h:0,v:0};let e=(...t)=>t.some(i=>this.keys.has(i));return{h:Number(e("KeyD","ArrowRight"))-Number(e("KeyA","ArrowLeft")),v:Number(e("KeyS","ArrowDown"))-Number(e("KeyW","ArrowUp"))}}dispose(){this.events.abort(),this.clear()}};function yd(n,e,t,i){let s=e?'<g class="cooking-flame"><path d="M118 226Q92 202 120 177Q112 200 137 190Q163 210 146 226Z" fill="#e39b4c"/><path d="M178 226Q158 203 181 180Q177 202 196 192Q216 214 199 226Z" fill="#f0c475"/><path d="M146 228Q136 215 156 198Q148 215 170 213L177 228Z" fill="#f7dc9c"/></g>':"",r='<g stroke="#423225" stroke-width="3"><path d="M99 229l100 11m-87 2l95-16" stroke="#815534" stroke-width="14" stroke-linecap="round"/><ellipse cx="103" cy="229" rx="8" ry="7" fill="#c29760"/><ellipse cx="202" cy="226" rx="7" ry="6" fill="#b58b55"/></g>',o=e?'<g class="cooking-steam" stroke="#e4ddc0" stroke-width="4" fill="none" opacity=".5"><path d="M127 42q-15-14 0-27m31 27q-15-14 0-27m31 27q-15-14 0-27"/></g>':"",a="",c="";return n==="oven"?(a='<path d="M49 239L43 127Q38 47 158 40Q277 42 278 127L273 239Z" fill="#777568" stroke="#343d32" stroke-width="5"/><path d="M110 51V17h69v24" fill="#727062" stroke="#343d32" stroke-width="5"/><path d="M104 16h82v14h-82" fill="#96907a" stroke="#343d32" stroke-width="3"/><g fill="none" stroke="#4f5648" stroke-width="4"><path d="M45 124l234-4M48 164h225M48 202h224M65 85h192M120 44v40M204 50v35M87 84v39M232 84v38M64 127v37M256 123v40M63 166v36M254 164v37M91 205v32M225 205v32"/></g><path d="M86 223v-78Q86 77 160 76q74 1 74 69v78Z" fill="#c0ab81" stroke="#434638" stroke-width="4"/><path d="M104 217v-69q0-51 56-52q56 1 56 52v69Z" fill="#252a21" stroke="#685e43" stroke-width="4"/><path d="M108 183h104v13H108Z" fill="#8f8162" stroke="#4c4836" stroke-width="3"/><path d="M82 142l22 4M94 108l18 14M120 86l10 18M158 76v21M197 86l-11 19M224 110l-17 14M234 145l-19 1" stroke="#716e55" stroke-width="4"/><path d="M43 238h234v14H43Z" fill="#a29679" stroke="#454b3d" stroke-width="4"/><g fill="#62795a"><ellipse cx="64" cy="196" rx="12" ry="5"/><ellipse cx="247" cy="72" rx="11" ry="4"/><ellipse cx="70" cy="86" rx="9" ry="4"/></g><path d="M284 221l9-73" stroke="#8c6940" stroke-width="9" stroke-linecap="round"/><path d="M276 205h28v36h-28Z" fill="#ad8656" stroke="#5e4b32" stroke-width="3"/>',(i||e||t)&&(c='<ellipse cx="160" cy="169" rx="39" ry="14" fill="'+(t?"#bd8241":"#d7c093")+'"/><path d="M138 162l8 12m8-15l8 12m8-10l8 10" stroke="#f0dcb3" stroke-width="4" stroke-linecap="round"/>'),'<svg viewBox="0 0 320 270" role="img" aria-label="\uC7A5\uC791 \uB3CC\uD654\uB355'+(e?" \uC870\uB9AC \uC911":t?" \uC644\uC131":"")+'"><ellipse cx="160" cy="254" rx="136" ry="12" fill="#102219"/>'+a+'<g transform="translate(59 54) scale(.63)">'+r+s+"</g>"+c+"</svg>"):(n==="pot"?a='<path d="M72 246L108 27h109l38 219M106 28l54-15 57 15" fill="none" stroke="#5b4935" stroke-width="10" stroke-linecap="round"/><path d="M159 17v48q-15 3-9 15q7 10 15 0" fill="none" stroke="#9c9579" stroke-width="5"/><path d="M91 110q13-63 69-64q55 1 70 64" fill="none" stroke="#99977e" stroke-width="5"/><path d="M83 111q-5 95 77 97q83 0 77-97Z" fill="#465347" stroke="#1b2f25" stroke-width="5"/><ellipse cx="160" cy="111" rx="78" ry="20" fill="#87917b" stroke="#243a2b" stroke-width="4"/><ellipse cx="160" cy="113" rx="65" ry="12" fill="'+(i||e||t?"#ad9565":"#283d2d")+'"/><path d="M95 136q1 34 20 46" stroke="#77866b" stroke-width="6" fill="none" stroke-linecap="round"/><path d="M91 112l-12 8m150-8l13 8" stroke="#b3ad87" stroke-width="6"/>':n==="pan"?(a='<path d="M72 231q-5-55 80-58q86 0 96 58Z" fill="#797865" stroke="#444e3a" stroke-width="5"/><path d="M91 230q0-39 61-40q58 0 76 40" fill="#243322"/><path d="M70 208l25 2m-5-26l13 14m107-12l-9 13m43 11l-22 1" stroke="#434b37" stroke-width="4"/><path d="M222 155l66-58" stroke="#263b2a" stroke-width="15" stroke-linecap="round"/><path d="M261 120l29-25" stroke="#9a784c" stroke-width="17" stroke-linecap="round"/><path d="M264 117l9 7m-1-15l9 7m-1-15l9 7" stroke="#d0ab71" stroke-width="3"/><ellipse cx="147" cy="155" rx="106" ry="34" fill="#7b8771" stroke="#243c2b" stroke-width="5"/><ellipse cx="147" cy="150" rx="92" ry="25" fill="#2c4030"/>',(i||e||t)&&(c='<path d="M106 139q21-13 55 0l16 14q-16 19-48 8q-29 3-23-22" fill="'+(t?"#a9794e":"#b18f70")+'" stroke="#d6b388" stroke-width="3"/><path d="M123 139l23 22m-3-23l22 17" stroke="#6b5039" stroke-width="3"/>')):a='<path d="M38 212h244v35H38Z" fill="#715536" stroke="#3e3827" stroke-width="4"/><path d="M42 225h235m-169-11l-6 30m106-30l8 31" stroke="#b18a58" stroke-width="2"/><path d="M59 117q1 95 101 99q98-5 101-99Z" fill="#af8757" stroke="#5f4c31" stroke-width="5"/><path d="M72 153q88 43 176 0m-161 32q78 30 145 0" fill="none" stroke="#d4ac73" stroke-width="4"/><path d="M91 139q8 32 23 38m37-31v42m55-50q-5 31-16 42" stroke="#836340" stroke-width="3" fill="none"/><ellipse cx="160" cy="116" rx="102" ry="31" fill="#d3ad78" stroke="#6c5335" stroke-width="4"/><ellipse cx="160" cy="118" rx="84" ry="20" fill="#866a44"/><path d="M220 117l40-70" stroke="#c4a073" stroke-width="11" stroke-linecap="round"/><ellipse cx="214" cy="126" rx="11" ry="22" transform="rotate(30 214 126)" fill="#d8b382" stroke="#8d6b45" stroke-width="3"/>',(n==="pot"||n==="bowl")&&(i||e||t)&&(c='<g fill="#91ad69"><ellipse cx="129" cy="115" rx="12" ry="5"/><ellipse cx="182" cy="121" rx="15" ry="5"/></g><g fill="#dbc18b"><ellipse cx="157" cy="112" rx="10" ry="5"/><ellipse cx="153" cy="124" rx="9" ry="4"/></g>'),'<svg viewBox="0 0 320 270" role="img" aria-label="'+Rs[n].name+(e?" \uC870\uB9AC \uC911":t?" \uC644\uC131":"")+'"><ellipse cx="160" cy="254" rx="136" ry="12" fill="#102219"/>'+a+(n==="bowl"?"":r+s)+c+(n==="bowl"?"":o)+"</svg>")}var fi=(n,e,t="",i=!1,s="")=>`<button data-act="${n}" data-id="${t}" ${i?"disabled":""} class="${s}">${e}</button>`;function _c(n){let{s:e,visit:t,guest:i}=n;if(!t||!i)return"";let s=Z.RECIPES.find(d=>d.id===t.recipe),r=Uo[s?.id]||"oven",o=Rs[r],a=t.status==="waiting",c=t.status==="cooking",l=t.status==="ready",h=s?Object.values(s.ing).reduce((d,g)=>d+g,0):0,u=Object.values(t.ingredients||{}).reduce((d,g)=>d+g,0),p=c?"\uBD88 \uC704\uC5D0\uC11C \uC775\uB294 \uC911":l?"\uC644\uC131\uB41C \uD55C \uB07C":s?"\uC7AC\uB8CC\uB97C \uD558\uB098\uC529 \uB123\uC5B4 \uC8FC\uC138\uC694":"\uBA3C\uC800 \uC694\uB9AC\uB97C \uACE8\uB77C \uC8FC\uC138\uC694";return`<div class="cook-heading"><div><span class="eyebrow">A MEAL FOR ${We(i.name)}</span><h2>\uB3CC\uC544\uC62C \uD55C \uB07C\uB97C \uB9CC\uB4DC\uB294 \uC2DC\uAC04</h2><span class="small muted">${We(i.name)} \xB7 ${i.floor}\uCE35 \xB7 ${_t[t.seat].name}</span></div>${fi("close-cooking","\uC2DD\uB2F9\uC73C\uB85C \uB3CC\uC544\uAC00\uAE30")}</div>
  <div class="cook-brief"><p>\u201C${We(t.hint)}\u201D</p><span>\uC88B\uC544\uD568 \xB7 ${(i.knownLikes||[]).map(We).join(" \xB7 ")||"?"}\u3000\uC2EB\uC5B4\uD568 \xB7 ${(i.knownDislikes||[]).map(We).join(" \xB7 ")||"?"}</span></div><div class="cook-layout"><section class="cook-recipes" aria-label="\uC694\uB9AC \uC120\uD0DD"><h3>01 \xB7 \uB9CC\uB4E4 \uC694\uB9AC</h3><div class="stack">${n.recipes.map(d=>fi("recipe",`<strong>${d.n}</strong><small>${Rs[Uo[d.id]].name} \xB7 ${d.tags.join(" / ")}</small><span class="pills">${Object.entries(d.effects).filter(([,g])=>g).map(([g,v])=>`<span class="pill ax-${g}">${Z.AX[g].eff} ${v}</span>`).join("")}</span>`,d.id,!a||!d.cookable,"recipe "+(t.recipe===d.id?"active":""))).join("")}</div></section>
  <section class="cook-center" aria-label="\uC870\uB9AC \uB3C4\uAD6C"><div class="cook-order"><span class="eyebrow">${s?"02 \xB7 "+o.verb:"YOUR KITCHEN"}</span><h3>${s?.n||"\uC624\uB298\uC758 \uD55C \uB07C"}</h3><p>${p}</p></div><div class="cook-tool ${c?"working":""}">${yd(r,c,l,u>0)}<span>${o.name}</span><p class="small muted tool-tip">${o.tip}</p></div>
  <div class="loaded-ingredients" aria-live="polite">${s?Object.entries(s.ing).flatMap(([d,g])=>Array.from({length:g},(v,m)=>`<span class="ingredient-slot ${m<(t.ingredients?.[d]||0)?"filled":""}">${m<(t.ingredients?.[d]||0)?"\u25CF":"\u25CB"} ${Z.ING[d].n}</span>`)).join(""):""}${t.spices.map(d=>`<span class="ingredient-slot spice filled">${Z.SPICES[d].n}</span>`).join("")}</div>
  ${c||l?`<div class="progress"><div data-cook-progress="${t.id}" style="width:${l?100:0}%"></div></div><p class="small muted">${c?"\uB2E4\uB978 \uC190\uB2D8\uC744 \uC751\uB300\uD558\uB294 \uB3D9\uC548\uC5D0\uB3C4 \uC870\uB9AC\uAC00 \uC774\uC5B4\uC9D1\uB2C8\uB2E4.":"\uD654\uB355\uC73C\uB85C \uC774\uB3D9\uD574 \uC774 \uC190\uB2D8\uC758 \uC811\uC2DC\uB97C \uC9D1\uC5B4 \uC8FC\uC138\uC694."}</p>`:`<p class="small muted">${s?`\uD544\uC218 \uC7AC\uB8CC ${u}/${h} \xB7 \uC120\uD0DD \uC591\uB150 ${t.spices.length}/${e.shelf}`:"\uC7AC\uB8CC\uB97C \uACE0\uB974\uBA74 \uAC00\uC6B4\uB370 \uB3C4\uAD6C\uC5D0 \uB2F4\uAE41\uB2C8\uB2E4."}</p>`}
  <div class="cook-effects">${n.E?Object.entries(n.E).filter(([,d])=>d).map(([d,g])=>`<span class="pill ax-${d}">${Z.AX[d].eff} ${g}</span>`).join(""):""}${n.mealTags.map(d=>`<span class="pill flavor">${We(d)}</span>`).join("")}</div>
  <div class="actions">${a?fi("cook",n.burnerAvailable?`\uC7AC\uB8CC\uB97C \uB123\uACE0 ${o.verb} \uC2DC\uC791`:"\uB2E4\uB978 \uC694\uB9AC \uC870\uB9AC \uC911","",!n.canCook||!n.ingredientsReady||!n.burnerAvailable,"primary"):fi("close-cooking",c?"\uC870\uB9AC \uB9E1\uAE30\uACE0 \uB2E4\uB978 \uC190\uB2D8 \uC751\uB300":"\uC644\uC131 \uC811\uC2DC \uCC3E\uC73C\uB7EC \uAC00\uAE30","","","primary")}</div>
  <p class="cook-note">\u{1F4A1} \uC870\uB9AC \uB3C4\uAD6C \uBC30\uC815\xB7\uC870\uB9AC \uC2DC\uAC04 ${Gt.cookMs/1e3}\uCD08\uB294 \uC2DC\uD5D8\uAC12\uC785\uB2C8\uB2E4.<br>\uC7AC\uB8CC\uB294 \uC2DC\uC791\uD560 \uB54C \uC18C\uBAA8\uD569\uB2C8\uB2E4. \uD654\uBA74\uC744 \uB2EB\uC544\uB3C4 \uC900\uBE44\uD55C \uC7AC\uB8CC\uB294 \uB0A8\uC2B5\uB2C8\uB2E4.</p></section>
  <section class="cook-pantry" aria-label="\uC7AC\uB8CC \uB123\uAE30"><h3>03 \xB7 \uC7AC\uB8CC \uB123\uAE30</h3><p class="small muted">\uBC84\uD2BC\uC744 \uB204\uB97C \uB54C\uB9C8\uB2E4 1\uAC1C\uC529 \uB2F4\uC2B5\uB2C8\uB2E4.</p>${s?Object.entries(s.ing).map(([d,g])=>`<div class="pantry-item"><div><strong>${Z.ING[d].n}</strong><small>\uB2F4\uC740 \uC218 ${t.ingredients?.[d]||0}/${g} \xB7 \uC7AC\uACE0 ${e.stock[d]||0}</small></div><div class="row">${fi("ingredient",Z.ING[d].n+" 1\uAC1C \uB123\uAE30",d,!a||(t.ingredients?.[d]||0)>=Math.min(g,e.stock[d]||0))}${fi("remove-ingredient","1\uAC1C \uBE7C\uAE30",d,!a||!t.ingredients?.[d])}</div></div>`).join(""):'<p class="empty">\uC67C\uCABD\uC5D0\uC11C \uC694\uB9AC\uB97C \uACE8\uB77C \uC8FC\uC138\uC694.</p>'}<div class="divider"></div><h3>\uC120\uD0DD \uC591\uB150</h3><div class="spices">${Object.entries(Z.SPICES).map(([d,g])=>fi("spice",`${g.n}<small>${g.tag} \xB7 ${Z.AX[g.ax].eff} +1<br>\uC7AC\uACE0 ${e.stock[d]||0}</small>`,d,!s||!a||t.spices.length>=e.shelf||(e.stock[d]||0)<=t.spices.filter(v=>v===d).length)).join("")}</div><div class="actions">${fi("clear-spice","\uC591\uB150 \uBE44\uC6B0\uAE30","",!a||!t.spices.length)}</div>${s?.id==="bread"?'<p class="notice">\uAE30\uC874 \uC870\uD569: \uBC00\uAC00\uB8E8 2\uAC1C.<br>\uC18C\uAE08\uC740 \uC120\uD0DD \uC591\uB150\uC73C\uB85C \uB123\uC744 \uC218 \uC788\uC2B5\uB2C8\uB2E4.</p>':""}</section></div>`}var Rn=new yr,Ge={panel:document.querySelector("#panel"),dialog:document.querySelector("#dialog"),cooking:document.querySelector("#cooking-dialog"),status:document.querySelector("#status"),save:document.querySelector("#save-state"),toast:document.querySelector("#toast")},ce=null,At=null,Md,gn=null,Sd=0,bd=!1,Ed=0,Re={station:"door",prepTab:"home",bookTab:"story",selfRecipe:null,selfSpices:[]};try{At=new Eo(document.querySelector("#canvas-wrap"),n=>{Rn.emit("Action",{act:"interact-target",id:n})})}catch(n){document.querySelector("#scene-fallback").hidden=!1,console.error("3D renderer unavailable",n)}var _r=new Ro(()=>Mc(),()=>!!ce&&!!At&&ce.phase==="service"&&!Ge.dialog.open&&!Ge.cooking.open&&!document.hidden),vc=()=>ce?.phase==="service"?"guest:"+yt(ce)?.id:"guest",Sc=n=>n==="guest"?vc():n;function yc(){let n=ce&&yt(ce);if(ce?.phase!=="service"||!n||n.seat===null){zt("\uBA3C\uC800 \uC190\uB2D8\uC744 \uC120\uD0DD\uD558\uACE0 \uC790\uB9AC\uB97C \uC548\uB0B4\uD574 \uC8FC\uC138\uC694.");return}if(bc("kitchen")){if(["served","carrying"].includes(n.status)){zt("\uC774 \uC190\uB2D8\uC758 \uC74C\uC2DD\uC740 \uC774\uBBF8 \uC644\uC131\uB410\uC2B5\uB2C8\uB2E4. \uC804\uB2EC\uACFC \uBC30\uC6C5\uC744 \uB9C8\uCCD0 \uC8FC\uC138\uC694.");return}_r.clear(),Ge.cooking.innerHTML=_c(Ec()),Ge.cooking.showModal()}}function Td(n){n=Sc(n);let e=qn(ce).find(t=>t.id===n);return e?(gn=n,At?.setDestination(e),zt(`${e.name} \uAC00\uAE4C\uC774\uB85C \uC774\uB3D9\uD574 E\uB97C \uB204\uB974\uC138\uC694.`),!1):(zt("\uC9C0\uAE08\uC740 \uC774\uACF3\uC5D0 \uC751\uB300\uD560 \uC190\uB2D8\uC774 \uC5C6\uC2B5\uB2C8\uB2E4."),!1)}function bc(n){return n=Sc(n),!At||Ao(ce,n)||Td(n)}function Mc(n){if(!ce||Ge.dialog.open||Ge.cooking.open||ce.phase==="ending")return;let e=n?qn(ce).find(i=>i.id===n):gc(ce);if(!e){zt("\uB300\uC0C1 \uAC00\uAE4C\uC774\uB85C \uC774\uB3D9\uD558\uC138\uC694. \uC7A5\uC18C \uBC84\uD2BC\uC73C\uB85C \uC704\uCE58\uB97C \uD45C\uC2DC\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.");return}if(!bc(e.id))return;gn=null,At?.setDestination(null),e.id.startsWith("guest:")&&(Tr(ce,e.id.slice(6)),Cn());let t=yt(ce);if(e.id.startsWith("seat:")){Rn.emit("Action",{act:"seat",id:e.id.split(":")[1]});return}if(e.id==="door"&&["prep","close"].includes(ce.phase)){Rn.emit("Action",{act:ce.phase==="prep"?"open":"morning"});return}if(e.id==="stove"&&t?.status==="ready"){Rn.emit("Action",{act:"pickup"});return}if(e.id.startsWith("guest:")&&t?.status==="carrying"){Rn.emit("Action",{act:"serve"});return}if(e.id.startsWith("guest:")&&t?.status==="served"){Rn.emit("Action",{act:"next"});return}if(e.id==="kitchen"){Re.station="kitchen",Yt(!0),yc();return}if(e.id.startsWith("guest:")){Re.station="guest",Yt(!0);return}Re.station=e.id,Yt(!0)}function wd(n=!1){if(!ce){document.querySelector("#movement-hud").hidden=!0;return}let e=document.querySelector("#movement-hud");e.hidden=!1;let t=At?gc(ce):null,i=yt(ce),s=gn&&qn(ce).find(a=>a.id===gn),r=Zn(ce);document.querySelector("#interact-label").textContent=t?`${t.name} \xB7 ${t.id==="stove"&&i?.status==="ready"?"\uC811\uC2DC \uC9D1\uAE30":t.id==="guest:"+r?.id?"\uC74C\uC2DD \uB0B4\uAE30":"\uC0C1\uD638\uC791\uC6A9"}`:"\uAC00\uAE4C\uC6B4 \uB300\uC0C1 \uC5C6\uC74C",document.querySelector("#interact-button").disabled=!t||Ge.dialog.open||Ge.cooking.open||ce.phase==="ending";let o=document.querySelector("#player-state");o.textContent=r?`${ce.guests[r.guestId].name}\uC758 \uC811\uC2DC \uC6B4\uBC18 \uC911`:n?"\uC774\uB3D9 \uC911":"WASD / \uBC29\uD5A5\uD0A4 \uC774\uB3D9",o.dataset.x=ce.player.x.toFixed(3),o.dataset.z=ce.player.z.toFixed(3),document.querySelector("#destination-label").textContent=s?`${s.name}\uAE4C\uC9C0 ${gr(ce,s.id).toFixed(1)}m \xB7 \uAE08\uC0C9 \uC6D0\uC73C\uB85C \uC774\uB3D9`:"E \uC0C1\uD638\uC791\uC6A9 \xB7 \uB4DC\uB798\uADF8 \uC2DC\uC810 \uD68C\uC804"}function Ad(n){let e=(n-Sd)/1e3;if(Sd=n,ce){let t=_r.axes(),i=ce.player;ce.player=vd(i,t.h,t.v,e,At?.angle||.72);let s=Math.hypot(ce.player.x-i.x,ce.player.z-i.z)>1e-4;(bd&&!s||s&&n-Ed>1200)&&(Cn(),Ed=n),bd=s,At?.setPlayer(ce.player,s,!!Zn(ce)),wd(s)}requestAnimationFrame(Ad)}function Cn(){ce&&(Ge.save.textContent=Uc(ce)?"\uC774 \uBE0C\uB77C\uC6B0\uC800\uC5D0 \uC790\uB3D9 \uC800\uC7A5\uB428":"\uC800\uC7A5 \uACF5\uAC04\uC744 \uC0AC\uC6A9\uD560 \uC218 \uC5C6\uC2B5\uB2C8\uB2E4 \xB7 \uBA54\uB274\uC5D0\uC11C \uC800\uC7A5 \uB0B4\uB824\uBC1B\uAE30")}function zt(n){Ge.toast.textContent=n,Ge.toast.hidden=!1,clearTimeout(Md),Md=setTimeout(()=>Ge.toast.hidden=!0,3200)}function pi(n){_r.clear(),Ge.dialog.classList.toggle("management-dialog",!!Re.management),Ge.dialog.innerHTML=n,Ge.dialog.open||Ge.dialog.showModal()}function Ec(){if(!ce)return{s:null,normalSave:!!Bi("normal"),storySave:!!Bi("story"),relationsSave:!!Bi("relations")};let n=ce,e=yt(n),t=Xc(n),i=_i(n),s=t.regulars.find(a=>a.id===i?.id)||i,r=Z.RECIPES.filter(a=>Object.keys(a.ing).every(c=>n.discovered[c])).map(a=>({...a,effects:Cs(n,a.id),cookable:Hi(n,a)})),o=Object.entries({...Z.ING,...Z.SPICES}).filter(([a,c])=>!Z.ING[a]?.rare&&(!Z.ING[a]?.floor||Z.ING[a].floor<=n.floor)).map(([a,c])=>({id:a,name:c.n,have:n.stock[a]||0,cost:(c.price||8)*(Z.ING[a]?.floor?4:1)}));return{s:n,visit:e,guest:s,relationships:t,recipes:r,market:o,held:Zn(n),burnerAvailable:Bo(n),ingredientsReady:ko(e),mealTags:Mr(Z.RECIPES.find(a=>a.id===e?.recipe),e?.spices||[]),E:Cs(n,e?.recipe,e?.spices,e?.seat),canCook:Hi(n,Z.RECIPES.find(a=>a.id===e?.recipe),e?.spices||[]),report:n.reports[n.reportIndex],readiness:Ar(n,Re.selfRecipe,Re.selfSpices)}}function Yt(n=!1){let e=Ge.panel.scrollTop,t=Ec(),i=ce;if(document.body.dataset.phase=i?.phase||"title",Ge.panel.innerHTML=(i?.phase==="service"?fd(t):"")+hd(t,Re),Ge.panel.scrollTop=n?0:e,Ge.cooking.open){let o=[...Ge.cooking.querySelectorAll(".cook-recipes,.cook-pantry")].map(c=>c.scrollTop),a=Ge.cooking.scrollTop;Ge.cooking.innerHTML=_c(t),Ge.cooking.scrollTop=a,Ge.cooking.querySelectorAll(".cook-recipes,.cook-pantry").forEach((c,l)=>c.scrollTop=o[l]||0)}Ge.dialog.open&&Re.management&&(Ge.dialog.innerHTML=mc(t,Re)),document.querySelector("#management-button").hidden=!i||!["prep","close"].includes(i.phase),document.querySelector("#notebook-button").hidden=!i;let s={prep:"\uC601\uC5C5 \uC900\uBE44",service:"\uC190\uB2D8 \uB9DE\uC774",close:"\uC601\uC5C5 \uB9C8\uAC10",report:"\uADC0\uD658 \uBCF4\uACE0",ending:"\uADC0\uD658 \uC774\uC57C\uAE30"};Ge.status.innerHTML=i?`<span class="stat"><span>${i.mode==="story"?"5\uCE35 \uC2DC\uD5D8":i.mode==="relations"?"\u{1F4A1} \uB2E8\uACE8 \uC2DC\uD5D8":"\uC77C\uBC18 \uC601\uC5C5"} \xB7 ${s[i.phase]}</span><strong>${i.day}\uC77C\uCC28</strong></span><span class="stat"><span>\uAE08\uACE0</span><strong>${i.gold} g</strong></span>`:'<span class="stat"><span>3D WEB PROTOTYPE</span><strong>\uB3CC\uC544\uC62C \uD55C \uB07C</strong></span>',document.querySelector("#world-tag").textContent=i?i.story.afterReturn?"\u{1F4A1} \uC5D4\uB529 A\uC548 \uC774\uD6C4 \uC601\uC5C5":i.mode==="story"?"\u{1F4A1} 5\uCE35 \uC870\uC0AC \uC2DC\uD5D8":"\u{1F4A1} 3D \uC601\uC5C5 \uC2DC\uC81C\uD488":"A SMALL RESTAURANT, A LONG WAY HOME",document.querySelector("#steps").textContent=i?.phase==="service"?`\uC2DD\uB2F9 \uC548 ${i.visits.filter(Et).length}\uBA85 \xB7 \uC751\uB300 \uC911 ${t.guest?.name||""} \xB7 \uC190\uB2D8 \uBAA9\uB85D\uC73C\uB85C \uC120\uD0DD`:i?.phase==="report"?"\uC5B4\uC81C\uC758 \uCC98\uBC29\uC744 \uB3CC\uC544\uBCF4\uB294 \uC544\uCE68":i?.phase==="prep"?"\uAC00\uAC8C \uAC1C\uB7C9 \xB7 \uC7AC\uB8CC \uC900\uBE44 \u2192 \uBB38 \uC5F4\uAE30":"\uC190\uB2D8\uACFC \uBB3C\uAC74\uC744 \uB20C\uB7EC \uC0B4\uD3B4\uBCF4\uC138\uC694",document.querySelectorAll("#stations button").forEach(o=>{o.classList.toggle("active",o.dataset.id===Re.station)});let r=i?.phase==="service"?i.visits.filter(Et).map(o=>({key:"guest:"+o.id,data:i.guests[o.guestId],slot:o.waitSlot,seat:o.seat,status:o.status,selected:o.id===t.visit?.id})):i?.phase==="report"&&t.report?[{key:"guest",data:i.guests[t.report.guestId],seat:null,report:!0}]:[];At?.update({seats:i?.seats||[0,1,2],seat:i?.phase==="service"?t.visit?.seat:null,keepsakes:t.relationships?.keepsakes||[],guests:r}),Rd(),wd()}function Rd(){document.querySelectorAll("[data-cook-progress]").forEach(n=>{let e=ce?.visits.find(t=>t.id===n.dataset.cookProgress);e?.status==="cooking"&&(n.style.width=Math.min(100,Math.max(0,100*(1-(e.readyAt-Date.now())/Gt.cookMs)))+"%")})}function Cd(){if(ce?.phase!=="prep"||Ge.dialog.open)return;let n=Wo(ce);n&&pi(md(n.id))}function xc(n,e=!1){if(ce=e?Bi(n):$c(n),!ce){zt("\uBD88\uB7EC\uC62C \uC218 \uC788\uB294 \uC800\uC7A5\uC774 \uC5C6\uC2B5\uB2C8\uB2E4.");return}Er(ce),Yc(ce),xd(ce);for(let t of Object.values(ce.guests))ws(ce,t);_r.clear(),gn=null,At?.setDestination(null),Re={station:"door",prepTab:"home",bookTab:"story",selfRecipe:ce.prepared?.recipeId||null,selfSpices:ce.prepared?.spices||[]},zo(ce,Date.now()),Zn(ce)&&(gn="guest:"+Zn(ce).id,At?.setDestination(qn(ce).find(t=>t.id===gn)));for(let t of ce.reports)Ho(ce,t);Ge.dialog.close(),Cn(),Yt(!0),Cd()}Rn.on("ReportCommitted",n=>Ho(ce,n));Rn.on("Action",({act:n,id:e})=>{if(n==="menu"){Re.management=!1,pi(gd(ce));return}if(n==="close-dialog"){Re.management=!1,Ge.dialog.close();return}if(n==="camera"){At?.reset();return}if(n==="start"){if(Bi(e)){pi(`<span class="eyebrow">NEW SAVE</span><h2>\uC0C8 \uC601\uC5C5\uC744 \uC2DC\uC791\uD560\uAE4C\uC694?</h2><p>\uC774 \uBAA8\uB4DC\uC758 \uC774\uC804 \uC800\uC7A5\uC744 \uC0C8 \uC601\uC5C5\uC73C\uB85C \uBC14\uAFC9\uB2C8\uB2E4. \uB2E4\uB978 \uBAA8\uB4DC\uC758 \uC800\uC7A5\uC740 \uC720\uC9C0\uB429\uB2C8\uB2E4.</p><div class="actions"><button data-act="close-dialog">\uB3CC\uC544\uAC00\uAE30</button><button class="primary" data-act="confirm-new" data-id="${e}">\uC0C8\uB85C \uC2DC\uC791</button></div>`);return}xc(e);return}if(n==="confirm-new"){xc(e);return}if(n==="continue"){xc(e,!0);return}if(!ce){zt("\uBA3C\uC800 \uC601\uC5C5\uC744 \uC2DC\uC791\uD574 \uC8FC\uC138\uC694.");return}if(n==="interact"){Mc();return}if(n==="interact-target"){Mc(e);return}if(n==="close-cooking"){Ge.cooking.close(),Re.station=yt(ce)?.status==="waiting"?"kitchen":"stove",Yt();return}if(n==="open-cooking"){yc();return}if(n==="management"){["prep","close"].includes(ce.phase)&&(Re.management=!0,Re.prepTab="upgrade",pi(mc(Ec(),Re)));return}if(n==="open-notebook"){Re.station=ce.phase==="prep"?"door":"book",ce.phase==="prep"&&(Re.prepTab="notes"),Yt(!0);return}if(n==="return-service"){Re.station=ce.phase==="prep"?"door":"guest",Yt(!0);return}if(n==="select-visit"){Tr(ce,e)&&(Re.station="guest",Cn(),Yt(!0));return}if(n==="select-order"){Tr(ce,e)&&(Re.station="stove",Cn(),Yt());return}if(n==="ask-close"){if(ce.visits.some(a=>["cooking","ready","carrying"].includes(a.status))){zt("\uC774\uBBF8 \uC870\uB9AC\uD55C \uC74C\uC2DD\uC740 \uBA3C\uC800 \uC190\uB2D8\uC5D0\uAC8C \uC804\uB2EC\uD574 \uC8FC\uC138\uC694.");return}Re.management=!1,pi('<h2>\uC624\uB298 \uC601\uC5C5\uC744 \uB9C8\uCE60\uAE4C\uC694?</h2><p>\uC544\uC9C1 \uC694\uB9AC\uB97C \uC2DC\uC791\uD558\uC9C0 \uC54A\uC740 \uC190\uB2D8\uC740 \uB3CC\uB824\uBCF4\uB0B4\uACE0, \uC2DD\uC0AC\uB97C \uB9C8\uCE5C \uC190\uB2D8\uC740 \uBC30\uC6C5\uD569\uB2C8\uB2E4. \uC900\uBE44 \uC911\uC778 \uC7AC\uB8CC\uB294 \uC18C\uBAA8\uB418\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.</p><div class="actions"><button data-act="close-dialog">\uACC4\uC18D \uC601\uC5C5\uD558\uAE30</button><button class="primary" data-act="early-close">\uB9C8\uAC10\uD558\uAE30</button></div>');return}let t={open:"door",morning:"door",seat:"seat:"+e,recipe:"kitchen",ingredient:"kitchen","remove-ingredient":"kitchen",spice:"kitchen","clear-spice":"kitchen",cook:"kitchen",pickup:"stove",serve:"guest",next:"guest",decline:"guest",request:"book","enable-preview":"stairs",plan:"stairs","self-recipe":"stairs","self-spice":"stairs","self-clear":"stairs",prepare:"stairs",descend:"stairs"}[n];if(t&&!["open","morning"].includes(n)&&!bc(t))return;if(n==="serve"&&At?.isGuestMoving(vc())){zt("\uC190\uB2D8\uC774 \uC790\uB9AC\uC5D0 \uB3C4\uCC29\uD558\uBA74 \uC74C\uC2DD\uC744 \uB0B4\uC5B4 \uC8FC\uC138\uC694.");return}let i=!1,s=!1,r=ce,o=yt(r);switch(n){case"early-close":i=sh(r),i&&(Ge.dialog.close(),Re.station="door");break;case"regular-story":{Re.management=!1;let a=Vi[e];a&&r.guests["regular-"+a.regularId]?.stories.includes(e)&&pi(cd(a));return}case"station":if(At&&!Ao(r,Sc(e))){Td(e);return}if(Re.station=e,e==="kitchen"&&r.phase==="service"){Yt(!0),yc();return}s=!0;break;case"prep-tab":Re.prepTab=e,r.phase==="prep"&&(Re.station="door"),s=!0;break;case"book-tab":Re.bookTab=e,s=!0;break;case"open":i=Zc(r),i&&(Re.station="guest",s=!0);break;case"seat":i=Jc(r,Number(e)),i&&(Re.station="guest",zt("\uC790\uB9AC\uB97C \uC815\uD588\uC2B5\uB2C8\uB2E4. \uC774\uC81C \uBAA9\uC801\uC9C0\uC758 \uC774\uC57C\uAE30\uB97C \uB4E4\uC5B4 \uBCF4\uC138\uC694."));break;case"recipe":i=Kc(r,e);break;case"ingredient":i=Qc(r,e);break;case"remove-ingredient":i=eh(r,e);break;case"spice":i=jc(r,e);break;case"clear-spice":o?.status==="waiting"&&(o.spices=[],i=!0);break;case"cook":i=th(r,Date.now()),i&&(Re.station="stove",s=!0);break;case"serve":i=ih(r),i&&(gn=null,At?.setDestination(null),Re.station="guest",s=!0);break;case"pickup":i=nh(r),i?(Re.station="guest",gn=vc(),At?.setDestination(qn(r).find(a=>a.id===gn)),zt(`${_i(r).name}\uC758 \uC811\uC2DC\uB97C \uB4E4\uC5C8\uC2B5\uB2C8\uB2E4. \uD574\uB2F9 \uC190\uB2D8\uC5D0\uAC8C \uB2E4\uAC00\uAC00 E\uB85C \uB0B4\uC5B4 \uC8FC\uC138\uC694.`),s=!0):Zn(r)&&zt("\uBA3C\uC800 \uB4E4\uACE0 \uC788\uB294 \uC811\uC2DC\uB97C \uC804\uB2EC\uD574 \uC8FC\uC138\uC694.");break;case"next":i=Vo(r),s=!0;break;case"decline":{let a=_i(r),c=a?.affinity;i=Vo(r,!0),i&&a?.regularId&&zt(`${a.name}\uC744 \uB3CC\uB824\uBCF4\uB0C8\uC2B5\uB2C8\uB2E4. \u{1F4A1} \uD638\uAC10 ${c} \u2192 ${a.affinity}`)}s=!0;break;case"morning":{if(r.phase!=="close")break;let a=rh(r);Cn();for(let c of a)Rn.emit(c.type,c.report);i=!0,Re.station="guest",s=!0;break}case"report-next":r.phase==="report"&&(r.reportIndex++,r.reportIndex>=r.reports.length&&(r.phase="prep",Re.station="door",Re.prepTab="market"),i=!0,s=!0);break;case"report-all":r.phase==="report"&&(r.reportIndex=r.reports.length,r.phase="prep",Re.station="door",Re.prepTab="market",i=!0,s=!0);break;case"buy":i=Go(r,e);break;case"buy-five":i=Go(r,e,5);break;case"upgrade-shelf":i=wr(r,"shelf");break;case"upgrade-stove":i=wr(r,"stove");break;case"upgrade-seat":i=wr(r,"seat",Number(e));break;case"request":i=lh(r,e),i&&zt("\uB2E4\uC74C 5\uCE35 \uC190\uB2D8\uC5D0\uAC8C \uC694\uCCAD\uC744 \uB9E1\uAE41\uB2C8\uB2E4.");break;case"scene-done":i=ch(r,e),i&&Ge.dialog.close();break;case"replay":{let a=xn[e];if(!r.story.done.includes(e)||!a)return;pi(`<span class="eyebrow">STORY RECORD</span><h2>${a.title}</h2><p>${a.text}</p><p class="small muted">\uAE30\uB85D \uB2E4\uC2DC \uBCF4\uAE30 \xB7 \uC9C4\uD589\uACFC \uBCF4\uC0C1\uC740 \uBC14\uB00C\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.</p><div class="actions"><button data-act="close-dialog" class="primary">\uAE30\uB85D \uB2EB\uAE30</button></div>`);return}case"enable-preview":r.phase==="prep"&&(r.previewEnabled=!0,i=!0);break;case"plan":r.phase==="prep"&&r.story.clues.includes("inner")&&r.story.clues.includes("outer")&&(r.story.plan=!0,Jn(r),i=!0);break;case"self-recipe":Re.selfRecipe=e,Re.selfSpices=[],Jn(r),i=!0;break;case"self-spice":Re.selfSpices.length<r.shelf&&(r.stock[e]||0)>Re.selfSpices.filter(a=>a===e).length&&(Re.selfSpices.push(e),Jn(r),i=!0);break;case"self-clear":Re.selfSpices=[],Jn(r),i=!0;break;case"prepare":{let a=ah(r,Re.selfRecipe,Re.selfSpices);i=a.ok,zt(a.ok?"\uC2DC\uD5D8\uC6A9 \uADC0\uD658 \uC900\uBE44\uB97C \uD655\uC778\uD588\uC2B5\uB2C8\uB2E4.":a.reasons[0]);break}case"descend":i=oh(r),s=!0;break;case"ending-next":r.phase==="ending"&&(r.endingStep==="depart"?r.endingStep="epilogue":hh(r),i=!0,Re.station="door",s=!0);break;case"ending-replay":pi('<span class="eyebrow">ENDING A \xB7 PROPOSAL</span><h2>\uB2E4\uB140\uC654\uC5B4.</h2><p>\uB3D9\uB8CC\uB4E4\uACFC \uD568\uAED8 \uB3CC\uC544\uC628 \uB4A4, \uC2DD\uB2F9\uC758 \uBD88\uC744 \uB2E4\uC2DC \uCF20\uB2E4. \uC774\uBC88\uC5D0\uB294 \uC55E\uC73C\uB85C \uAC00\uB294 \uD798\uACFC \uB3CC\uC544\uC62C \uD798\uC744 \uD568\uAED8 \uC900\uBE44\uD55C\uB2E4.</p><p class="small muted">\u{1F4A1} \uC2DC\uD5D8\uC6A9 \uD6C4\uC77C\uB2F4 \xB7 \uAE30\uAC04\uACFC \uAD6C\uCCB4\uC801\uC778 \uAD6C\uC870 \uACFC\uC815\uC740 \uBBF8\uC815\uC785\uB2C8\uB2E4.</p><div class="actions"><button data-act="close-dialog" class="primary">\uC2DD\uB2F9\uC73C\uB85C</button></div>');return;case"export":{let a=URL.createObjectURL(new Blob([JSON.stringify(r,null,2)],{type:"application/json"})),c=document.createElement("a");c.href=a,c.download=`\uCE35\uACC4\uCC38\uC2DD\uB2F9-${r.mode}-${r.day}\uC77C\uCC28.json`,c.click(),setTimeout(()=>URL.revokeObjectURL(a),1e3);return}case"title":_r.clear(),gn=null,At?.setDestination(null),Cn(),ce=null,Re.station="door",Ge.dialog.close(),Ge.save.textContent="",Yt(!0);return;default:return}i&&Cn(),Yt(s),["report-next","report-all","morning"].includes(n)&&Cd()});document.addEventListener("click",n=>{let e=n.target.closest("button[data-act]");e&&!e.disabled&&Rn.emit("Action",{act:e.dataset.act,id:e.dataset.id||""})});setInterval(()=>{ce&&zo(ce,Date.now())?(Cn(),Yt()):Rd()},150);document.addEventListener("visibilitychange",()=>{document.hidden&&Cn()});Ge.dialog.addEventListener("close",()=>{Re.management=!1});Yt();requestAnimationFrame(Ad);})();
