// cabooseAFK_main
// Main control script to execute helper functions as part of the CabooseAFK routine

include("runFunctions.cs");
include("presto\\event.cs");
include("convertTimeFormat.cs");
include("autostore.cs");
include("autoremort.cs");

exec("CabooseAFK\\cabooseAFK_autoPlacePack.cs");
exec("CabooseAFK\\cabooseAFK_autoManageBattle.cs");
exec("CabooseAFK\\cabooseAFK_helperFuncs.cs");
// exec("CabooseAFK\\cabooseAFK_autoremort.cs");
// exec("CabooseAFK\\cabooseAFK_autoskill.cs");

event::Attach(eventClientMessage, readClientMsg);

function readClientMsg(%client, %msg) {
	if(Match::String(%msg, "Welcome to level 101!")) {
		// autoremort();
	}

	if(Match::String(%msg, "Welcome to Remort Level *!")) {
		// autoskills();
	}

	if(Match::String(%msg, "The nearest town is * of here.")) {
		%direction = Match::Result(0);

		if($autoPlacePack::coarseCorrecting) {
			coarseCorrection(%direction);
		}
		// Now you can add other conditions to use this trackpack function, i.e. when tracking for Tene run
	}

	if(Match::String(%msg, "Caboose's nearest backpack is * of here, * meters away.")) {
		schedule::cancel("trackPack");
		%pack::direction = Match::Result(0);
		%pack::distance = Match::Result(1);

		if($autoPlacePack::ongoing && $autoPlacePack::verifyLocation) {
			verifyPackLocation(%pack::direction, %pack::distance);
		}

		if($manageBattle::TrackPack) {
			moveToAvoidHitting(%pack::distance);
		}

		if($determine::tene::position) {
			// determineTenePosition(%pack::direction, %pack::distance);
		}

		if($tene::check1) {
			// tene::waypoint::check1(%pack::direction, %pack::distance);
		}
		// Now you can add other conditions to use this trackpack function, i.e. when tracking for Tene run
	}

	if(Match::String(%msg, "Transporting to *")) {
		schedule::cancel("cast::transportGeneral");
		%transport::destination = Match::Result(0);

		if(%transport::destination == "Jaten Outpost") {
			if($autoPlacePack::ongoing && $autoPlacePack::coarseCorrecting) {
				autoPlacePack();
			}
			
			// else if you're there to drop off coins before heading off to Tene
		}
		else if(%transport::destination == "Tenebrous Cavern") {
			// $determine::tene::position = true;
			// trackPack();
		}
		else if(%transport::destination == "Keldrin Mines") {
			// $determine::mines::position = true;
			// trackPack();
		}
		else if(%transport::destination == "Keldrin Town") {
			// $determine::ktown::position = true;
			// trackPack();
		}
	}

	

	if(Match::String(%msg, "You try to hit *, but miss!")) {
		%whoHit = Match::Result(0);
		if($ultimateAFK) {
			// echo(">>> Checking if I hit " @ %whoHit @ "...");
			checkWhoHitting(%whoHit);
		}
	}
}