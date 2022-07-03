// Auto-place coin pack for GPS
function startAutoPlacePack() {
	if(!$autoPlacePack::ongoing && !$autoPlacePack::coarseCorrecting) {
		$autoPlacePack::ongoing = true;
		$autoPlacePack::coarseCorrecting = true;
		schedule::add("cast::transportGeneral(\"jaten\");", 0);
		schedule::add("postAction(2048, IDACTION_LOOKDOWN, 0.00314159);", 1);
		schedule::add("postAction(2048, IDACTION_LOOKDOWN, 0.00000000);", 2.5);
	}
}

function autoPlacePack() {
		schedule("say(1,\"#compass town\");", 5);
}

function coarseCorrection(%direction) {
	$autoPlacePack::coarseCorrecting = false;
	if($autoPlacePack::ongoing == true) {
		if(%direction == "West") {
			schedule("MoveLeft();", 0);
			schedule("StopMoveLeft();", 4);

			for(%i = 0; %i <= 7; %i++) {
				schedule("Backward();", 0+(%i*0.5));
				schedule("StopBackward();", 0.5+(%i*0.5));
			}

			schedule("MoveLeft();", 5);
			schedule("StopMoveLeft();", 8.75);

			for(%i = 0; %i <= 7; %i++) {
				schedule("Forward();", 5+(%i*0.5));
				schedule("StopForward();", 5.25+(%i*0.5));
			}

			schedule("MoveLeft();", 9);
			schedule("StopMoveLeft();", 11);

			for(%i = 0; %i <= 2; %i++) {
				schedule("Backward();", 9+(%i*0.5));
				schedule("StopBackward();", 10+(%i*0.5));
			}		

			schedule("Forward();", 11.5);
			schedule("StopForward();", 13);

			schedule("placeGladiusPack();", 16);
		}

		else if(%direction == "North West") {
			schedule("MoveLeft();", 0);

			for(%i = 0; %i <= 4; %i++) {
				schedule("Forward();", 0+(%i*0.5));
				schedule("StopForward();", 0.5+(%i*0.5));
			}

			schedule("Backward();", 2.5);
			schedule("StopBackward();", 6);
			schedule("StopMoveLeft();", 8);
			schedule("Forward();", 8);
			schedule("StopForward();", 10);

			schedule("placeGladiusPack();", 13);
		}

		else {
			echo("what the fuck");
			schedule("say(1,\"#say I have no idea where the fuck I am\");", 1);
			schedule("autoPlacePack();", 2);
		}
	}
}

function placeGladiusPack() {	
	if($autoPlacePack::ongoing == true) {
		schedule("Forward();", 0);
		schedule("StopForward();", 3);
		schedule("remoteEval(2048, ToggleInventoryMode);", 4);
		schedule("sell(\"Gladius\");", 5);
		schedule("sell(\"Gladius\");", 6);
		schedule("say(1,\"#createpack\");", 7);
		
		schedule("Backward();", 7);
		schedule("StopBackward();", 8);
		schedule("say(1,\"#trackpack caboose\");", 9);
		schedule::add("placeGladiusPack();", 15, "placeGladiusPack");
		$autoPlacePack::verifyLocation = true;
	}
}

function verifyPackLocation(%pack::direction, %pack::distance) {
	if($autoPlacePack::ongoing == true) {
		echo("Verifying pack location...");
		if(%pack::direction == "North East" && %pack::distance == 8) {
			schedule::cancel("placeGladiusPack");
			echo("Pack location verified, moving on.");
			$autoPlacePack::verifyLocation = false;
			autoOpenInvo();
	}
}

	else {
		echo("Pack location cannot be verified, starting over.");
		$autoPlacePack::ongoing = false;
		schedule("startAutoPlacePack();", 1);  // Try again
	}
}

function autoOpenInvo() {
	schedule("Backward();", 0);
	schedule("StopBackward();", 3);
	schedule("say(1,\"#say hi\");", 4);
	schedule("say(1,\"#say storage\");", 5);
	schedule("say(1,\"#cast transport tene\");", 6);
	$ultimateAFK = true;
	$autoPlacePack::ongoing = false;
}