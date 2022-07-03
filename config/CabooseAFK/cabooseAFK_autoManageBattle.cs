// Manage the battlefield
// As in move yourself around so you're not hitting someone

function checkWhoHitting(%whoHit) {
	// echo(">>> Building player list since I hit " @ %whoHit @ "...");
	%len = buildPlayerList();
	// echo(">>> There are " @ %len @ " players connected...");

	for(%i = 0; %i < %len; %i++) {
		if($CabooseAFK::playerList[%i] == %whoHit) {  // This means who you tried to hit is a player
			$manageBattle::TrackPack = true;
			trackPack();
		}  
	}
}

function moveToAvoidHitting(%pack::distance) {
	schedule::cancel("trackPack");
	$manageBattle::TrackPack = false;
	if(%pack::distance >= 1179) {
		schedule("Backward();", 0);
		schedule("MoveLeft();", 0);
		schedule("StopBackward();", 0.3);
		schedule("StopMoveLeft();", 0.3);
		schedule("Forward();", 0.3);
		schedule("StopForward();", 1);
		schedule("MoveRight();", 1);
		schedule("StopMoveRight();", 1.3);
		schedule("Backward();", 1.2);
		schedule("StopBackward();", 1.7);
	}
	else if(%pack::distance < 1179) {
		schedule("Backward();", 0);
		schedule("StopBackward();", 0.3);
		schedule("MoveRight();", 0.5);
		schedule("StopMoveRight();", 0.7);
		schedule("Forward();", 0.7);
		schedule("StopForward();", 1);
		schedule("MoveLeft();", 1.2);
		schedule("StopMoveLeft();", 1.4);
	}
}

function periodicCheck(%pack::distance) {
	schedule::cancel("trackPack");

}