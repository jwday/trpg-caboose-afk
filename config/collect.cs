//////////////////////////////////////
///////////Pack Collector/////////////
///////////Rose   Edition/////////////
///////////(C) Rose, 2007/////////////
//////////////////////////////////////


echo("");
echo("");
echo("");
echo("clay1(); -For use in the 'main' Uber corner, or Centuria.");
echo("clay2(); -Stand in the far back left corner at the bottom (diagonally from the corner noobs like) and aim at the wall to collect.");
echo("Type nocollect(); to stop collecting packs.");
echo("");
echo("");
echo("");


function clay1(){
event::Attach(eventClientMessage, collectclay1);
function collectclay1(%cl, %msg) {
%msgcollect = Match::String(%msg, "* has died*");
	if (%msgcollect){
		%person = Match::Result(0);
		%msg2 = Match::Result(1);
		if(%person != ""){
		schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0.0);
		schedule("postAction(2048, IDACTION_MOVEFORWARD, 0);", 0.5);
		schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 1.5);
		schedule("postAction(2048, IDACTION_MOVEBACK, 0);", 4.0);
		}
	}
}
}

function clay2(){
event::Attach(eventClientMessage, collectclay2);
function collectclay2(%cl, %msg) {
%msgcollect = Match::String(%msg, "* has died*");
	if (%msgcollect){
		%person = Match::Result(0);
		%msg2 = Match::Result(1);
		if(%person != ""){
		schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0.0);
		schedule("postAction(2048, IDACTION_MOVEFORWARD, 0);", 1.0);
		schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 1.0);
		schedule("postAction(2048, IDACTION_MOVEBACK, 0);", 2.0);
		}
	}
}
}

function nocollect(){
function collectclay1(){}
function collectclay2(){}
function clay1(){}
function clay2(){}
echo("");
echo("");
echo("");
echo("No longer collecting packs.");
echo("Re-Execute the script to start collecting again.");
echo("");
echo("");
echo("");
}