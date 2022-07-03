function autoremort() {
	$hitCheck = false;

	Schedule::Cancel("getLVL();", 1);
	Schedule("stop::equiploop();", 2);
	Schedule::Cancel("castDrift();", 3);

	Schedule("Stop::AutoCast();", 4);
	Schedule("DeusRPGPack::func14();", 5);
	Schedule("$current::RL = DeusRPG::FetchData(\"RemortStep\");", 17);
	Schedule("$current::RL = DeusRPG::FetchData(\"RemortStep\");", 18);
	Schedule("$current::RL = DeusRPG::FetchData(\"RemortStep\");", 19);

	Schedule("say(0,\"Time to remort to RL \" @ $current::RL+1 @ \"!\");", 20);
	Schedule("say(1,\"#cast remort\");", 21);
}

function autoskills() {
    if($ultimateAFK) {
        %i = 0.5;  // Timing interval. Increase this if steps get skipped.
        $currently::autoskilling = true;

        schedule("remoteeval(2048, ScoresOn);", %i++);  // Open Tab menu
        schedule("ClientMenuSelect(sp);", %i++); // Select 'Skill Points' submenu

        schedule("say(1,\"30\");",%i++); //30 into Slashing
        schedule("ClientMenuSelect(\"1 1\");",%i++);
        schedule("say(1,\"7\");",%i++); //7 more into Slashing in case of KLS
        schedule("ClientMenuSelect(\"1 1\");",%i++);
        schedule("say(1,\"10\");",%i++); //10 into Weght Capacity
        schedule("ClientMenuSelect(\"5 1\");",%i++);
        schedule("say(1,\"40\");",%i++); //40 into Offensive Casting
        schedule("ClientMenuSelect(\"10 2\");",%i++);
        schedule("say(1,\"5\");",%i++); //5 into Healing
        schedule("ClientMenuSelect(\"13 3\");",%i++);
        schedule("say(1,\"5\");",%i++); //5 into Endurance
        schedule("ClientMenuSelect(\"15 3\");",%i++);
        schedule("say(1,\"15\");",%i++); //15 into Speech
        schedule("ClientMenuSelect(\"18 3\");",%i++);
        schedule("say(1,\"20\");",%i++); //20 into Sense Heading
        schedule("ClientMenuSelect(\"19 4\");",%i++);
        schedule("say(1,\"30\");",%i++); //30 into Energy
        schedule("ClientMenuSelect(\"20 4\");",%i++);
        schedule("say(1,\"5\");",%i++); //5 into Neutral Casting
        schedule("ClientMenuSelect(\"22 4\");",%i++);
        schedule("remoteeval(2048, ScoresOff);", %i++);
        schedule("say(1,\"#say Checking weight...\");",%i++);
        schedule("preTransportCheck();", %i++);
    }
}

function preTransportCheck() {
    $maxWeight = DeusRPG::FetchData("MaxWeight");
    $currentWeight = DeusRPG::FetchData("Weight");

    if($currentWeight >= 0.85*$maxWeight && $currentWeight < $maxWeight) {
        schedule("say(1,\"#say I'm getting close to being overweight! I better drop all these coins off at Jaten Outpost before heading to Tenebrous Caves.\");", 1);
        $intending::to::transport::jaten = true;
        schedule::add("cast::transport();", 2);
    }
    else if($currentWeight >= $maxWeight) {
        schedule("say(1,\"#say I'm at my maximum weight capacity! Let's add a little more SP into Weight Capacity then go deposit my coins.\");", 1);
        schedule("remoteeval(2048, ScoresOn);", 2);
        schedule("ClientMenuSelect(sp);", 3);
        schedule("say(1,\"#say 5\");", 3); //5 into Weght Capacity
        schedule("ClientMenuSelect(\"5 1\");", 4);
        schedule("remoteeval(2048, ScoresOff);", 5);
        $intending::to::transport::jaten = true;
        schedule::add("cast::transport();", 6);
    }
    else {
        $intending::to::transport::jaten = false;
        schedule("say(1,\"#say Weight looks fine. Heading to Tenebrous Caves.\");", 1);
        schedule::add("cast::transport();", 2);
    }
}