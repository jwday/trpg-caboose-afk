echo("");
echo("///// SAFEGUARD /////");
echo("");
echo("Type startguard(); to activate safeguard routine");
echo("Type stopguard(); to deactivate safeguard routine");
echo("");

function startguard() {
	echo("");
	echo("Beginning auto-mining safeguard routine");
	echo("");
	$Safe = true;
	guardloop();
	}

function guardloop() {
	if($Safe)
    schedule("Mining::Stop();",0);

	schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 1);
	schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 4);

	schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 5);
	schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 5.65);

    schedule("Mining::Start(FALSE);",7);

	schedule("guardloop();", 300);
	}

function stopguard() {
	$Safe = false;
	echo("");
	echo("Ending auto-mining safeguard routine");
	echo("");
	}