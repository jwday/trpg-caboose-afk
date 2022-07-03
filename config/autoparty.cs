

///////////////////////////////////////////////////////
// Auto Party Accept
///////////////////////////////////////////////////////


event::Attach(eventClientMessage, AcceptMonitor);
function AcceptMonitor (%cl, %msg) {

	%joinparty = Match::String(%msg, "* has invited you to join his/her party.");
	%invperson = Match::Result(0);

	if (%joinparty) {
		if ($Accepting == "1") {
		$tehpartay = %invperson;
		$tehpartay = getClientByName($tehpartay);
		remoteEval(2048, ScoresOn);
		schedule("clientMenuSelect(\"partyoptions\");", 0.5);
		schedule("clientMenuSelect(\"acceptinv " @ $tehpartay @ "\");", 1.0);
		schedule("say(0, \"#zone Thanks~wthanks\");", 1.3);
		schedule("remoteEval(2048, lmsg, thanks);", 1.3);
		}
	}

} // end AcceptMonitor






if ($Accepting == "") {
	$Accepting = 1;
	echo("");
	echo("");
	echo("Auto-accepting party invites.");
	echo("");
	echo("");
} else if ($Accepting == "1") {
	$Accepting = "";
	echo("");
	echo("");
	echo("No longer auto-accepting party invites.");
	echo("");
	echo("");
}

