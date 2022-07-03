Event::attach(eventConnectionLost, exitOnConnectionLost);
Event::attach(eventConnectionTimeout, exitOnConnectionLost);

$connect::timer = 10;
$flag::killOnNoConnect = true;

function killOnNoConnect() {
	if($flag::killOnNoConnect) {
		if($connect::timer <= 0) {
			exitOnConnectionLost();
		}
		else {
			$connect::timer = $connect::timer - 1;
			schedule::add("killOnNoConnect();", 1, "killOnNoConnect");
			echo(">>> Killing Tribes.exe in " @ $connect::timer @ " seconds...");
		}
	}
}

killOnNoConnect(); // Run at script execution


event::Attach(eventConnected, joinCheck);
// Check if we've joined the game and have been given our assigned character info
// A workaround to check if we've spawned
%joinAttempts = 0;
function joinCheck() {
	schedule::cancel("killOnNoConnect");
	$flag::killOnNoConnect = false;

	if($rpgdata["CLASS"] != "") {
		Schedule::cancel("joinCheck");
		%supDelay = round(getRandom()*10) + 5;  // Random no. between 5 and 15 seconds
		%randHello = retHello();
		schedule::add("say(0,\"" @ %randHello @ "\");", %supDelay);

		if($PCFG::Name == "Caboose") {
			schedule::add("startAutoPlacePack();", 3.5);
		}
		else if($PCFG::Name == "SIayer") {
			// schedule::add("doSIayerStuff();", 3.5);
		}
		else {
			// Do nothing
		}
	}
	else {
		%joinAttempts++;
		echo(">>> Attempting to join server...");
		Schedule::add("joinCheck();", 5, "joinCheck");

		if(%joinAttempts >= 12) {  // If it takes a minute or more to join the server, then likely the server is down
			// And if it comes back up in the middle of a join attempt, I don't think you'll connect
			// Soooo...restart Tribes and try again!
			Schedule::cancel("joinCheck");
			exitOnConnectionLost();
		}
	}
}

function exitOnConnectionLost() {
	if($PCFG::Name == "SIayer") {
		$AUTOSELECTPLAYER::Name = "SIayer";
	} 
	else if($PCFG::Name == "Caboose") {
		$AUTOSELECTPLAYER::Name = "Caboose";
	}
	echo("");
	echo("=========================");
	echo(">>> RESETTING TO SELECT " @ $AUTOSELECTPLAYER::Name @ " AT NEXT OPEN");
	echo("=========================");
	echo("");
	export("$AUTOSELECTPLAYER::Name", "config\\SelectPlayers.cs", False);
	schedule("quit();", 2);
}