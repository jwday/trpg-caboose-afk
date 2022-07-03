// cabooseAFK helper functions

function cast::transportGeneral(%destination) {
		say(1,"#cast transport " @ %destination);
        schedule::add("cast::transportGeneral(" @ %destination @ ");", 5, "cast::transportGeneral");
}

function trackPack() {
	schedule("say(1,\"#trackpack caboose\");", 0);
	schedule::add("trackPack();", 3, "trackPack");
}

function buildPlayerList() {
	$CabooseAFK::playerList[0] = "";
	%p = 0;
	// echo("Returns global variable '$playerList[i]'");
	//Client ID sweep between 2000 and 2100. This is as arbitrary as this function gets.
	for(%i = 2000; %i <= 2100; %i++) {
		if(client::getname(%i) != "") {  // If there is a name associated with the client ID
			if(client::getteam(%i) == 0) {  // If the client is on team 0 (Citizen), i.e. if they are a player
				$CabooseAFK::playerList[%p] = client::getname(%i);  // Build an array of players who are connected to the game
				%p++; //The number of players who are connected to the game
			}
		}
	}
	return(%p); // Returns the number of players, even though we can't return the actual list
}

function printPlayerList() {
	%len = buildPlayerList();

	for(%i = 0; %i < %len; %i++) {
		echo(">> " @ $CabooseAFK::playerList[%i]);
	}
}

function retHello() {
	%CabooseAFK::randHello[0] = "sup";
	%CabooseAFK::randHello[1] = "back at it again";
	%CabooseAFK::randHello[2] = "afk check";
	%CabooseAFK::randHello[3] = "hey";
	%CabooseAFK::randHello[4] = "welp";
	%CabooseAFK::randHello[5] = "wb";
	%CabooseAFK::randHello[6] = "howdy";
	%CabooseAFK::randHello[7] = "idk why but im so happy this sorta works lol";
	%CabooseAFK::randHello[8] = "todays a good day to tribe, or something";
	%CabooseAFK::randHello[9] = "lol awesome";
	%CabooseAFK::randHello[10] = "full server i see";
	%CabooseAFK::randHello[11] = "what a life";
	%CabooseAFK::randHello[12] = "work work work";
	%CabooseAFK::randHello[13] = "wort wort wort";
	%CabooseAFK::randHello[14] = "heh";

	%supRand = round(getRandom()*14);  // Random no. between 0 and 14
	// echo(">>> Return Hello: retuning \"" @ %CabooseAFK::randHello[%supRand] @ "\". Did it send?");
	return(%CabooseAFK::randHello[%supRand]);
}

function randInt(%arg1, %arg2) {  // Return a random integer
	// Return random integer from 0 to whatever this num is
	if(%arg2=="") {
		if(%arg1 < 0) {
			%rand = round(getRandom()*(%arg1));
		}
		else {
			%rand = round(getRandom()*(%arg1 + 1));
		}
	}
	
	// Return random integer between two numbers (inclusive)
	else if(%arg1!="" && %arg2!="") {
		if(%arg2 < %arg1) {  // If a user is dumb and put in args in the wrong order
			%rand = round(%arg2 + getRandom()*((%arg1-%arg2) + 1));	
		}
		else {
			%rand = round(%arg1 + getRandom()*((%arg2-%arg1) + 1));	
		}
	}

	else {
		%rand = getRandom();
	}
	return(%rand);
}