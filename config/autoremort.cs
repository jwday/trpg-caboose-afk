include("runFunctions.cs");
include("presto\\event.cs");
include("convertTimeFormat.cs");
include("autostore.cs");

$track::target = "Backpack";


function stopAutoAFK() {
    %i = 0.5;

	$autoPlacePack::ongoing = false;
	$autoPlacePack::coarseCorrecting = false;
	$autoPlacePack::verifyLocation = false;
    $timer::on = false;
    $ultimateAFK = false;
    $calibrating = false;
    $determine::tene::position = false;
    $determine::jaten::position = false; 
    $check1 = false;
    $check2 = false;
    $check3 = false;
    $DeusRPG::toggleFire = true; //For some reason, Deus equates "true" with "not firing".
    $hitCheck = false;
    Stop::AutoCast();
	$KLS::override::ON = false;
    stopslots();
    postAction(2048, IDACTION_BREAK1, 1);
    client::centerPrint("Stopping all autoAFK functions...");
	Schedule::Cancel("cast::transport();", 0);
    Schedule::Cancel("say(1,\"#cast transport tene\");", %i++);
    Schedule::Cancel("say(1,\"#cast transport jaten\");", %i++);
    Schedule::Cancel("preTransportCheck();", %i++);
    Schedule::Cancel("castDrift();", %i++);
    Schedule::Cancel("AFKCasting();", %i++);
    Schedule::Cancel("getLVL();", %i++);
    Schedule::Cancel("triangulate");
    Schedule::Cancel("waypoint::check1();", %i++);
    Schedule::Cancel("waypoint::check2();", %i++);
    Schedule::Cancel("waypoint::check3();", %i++);
	

    Client::centerPrint("All autoAFK functions stopped. I hope...");
    schedule("Client::centerPrint(\"\", 1);", 20);
}

function startAutoAFK() {
    $ultimateAFK = true;
    if($timer::on == false) {
        $timer::on = true;
        remort::timer();
    }
    // $determine::tene::position = true;
    Schedule("StartAttacking();", 1);
    startslots();
}

function getLVL() {
    %i = 0.5;
    Schedule("say(1,\"#say Checking level...\");", %i);
    Schedule("$current::lvl = DeusRPG::FetchData(\"LVL\");", %i++); //Run 5 times because it's not very reliable
    Schedule("$current::lvl = DeusRPG::FetchData(\"LVL\");", %i++);
    Schedule("$current::lvl = DeusRPG::FetchData(\"LVL\");", %i++);
    Schedule("$current::lvl = DeusRPG::FetchData(\"LVL\");", %i++);
    Schedule("$current::lvl = DeusRPG::FetchData(\"LVL\");", %i++);
    Schedule("checkLVL();", %i++);
}



function checkLVL() {
    if($current::lvl <= 20 && $ultimateAFK == true) {
        Schedule("say(1,\"#say I'm not even close to level 101 yet. Rechecking in 30 minutes.\");", 1);
        Schedule::Cancel("getLVL();", 2);
        Schedule::Add("getLVL();", 1800);
    }
    else if($current::lvl <= 50 && $current::lvl > 20 && $ultimateAFK == true) {
        Schedule("say(1,\"#say I'm not even close to level 101 yet. Rechecking in 20 minutes.\");", 1);
        Schedule::Cancel("getLVL();", 2);
        Schedule::Add("getLVL();", 1200);
    }
    else if($current::lvl <= 90 && $current::lvl > 50 && $ultimateAFK == true) {
        Schedule("say(1,\"#say I'm not close to level 101 yet. Rechecking in 5 minutes.\");", 1);
        Schedule::Cancel("getLVL();", 2);
        Schedule::Add("getLVL();", 300);
    }
    else if($current::lvl <= 95 && $current::lvl > 90 && $ultimateAFK == true) {
        Schedule("say(1,\"#say I'm almost level 101, but I'm not there yet. Rechecking in 2 minutes.\");", 1);
        Schedule::Cancel("getLVL();", 2);
        Schedule::Add("getLVL();", 120);
    }
    else if($current::lvl <= 100 && $current::lvl > 95 && $ultimateAFK == true) {
        Schedule("say(1,\"#say I'm really close to level 101, but I'm not there yet. Rechecking in 30 seconds.\");", 1);
        Schedule::Cancel("getLVL();", 2);
        Schedule::Add("getLVL();", 30);
    }
    else if($current::lvl >= 101 && $ultimateAFK == true) {
        Schedule::Cancel("getLVL();", 1);
        Schedule("stop::equiploop();", 2);
        Schedule::Cancel("castDrift();", 3);
        $hitCheck = false;
        Schedule("Stop::AutoCast();", 4);
        Schedule("DeusRPGPack::func14();", 5);
        // if($KLS::override::ON) {
        //     Schedule("KLS::Override();", 6);
        // }
		Schedule("$current::RL = DeusRPG::FetchData(\"RemortStep\");", 17);
		Schedule("$current::RL = DeusRPG::FetchData(\"RemortStep\");", 18);
		Schedule("$current::RL = DeusRPG::FetchData(\"RemortStep\");", 19);
        Schedule("say(0,\"Time to remort to RL \" @ $current::RL+1 @ \"!\");", 20);
        Schedule("say(1,\"#cast remort\");", 21);
        Schedule::Add("getLVL();", 30);
    }
}


event::Attach(eventClientMessage, remortCatch);

function remortCatch(%client, %msg) {
    %remorted = Match::String(%msg, "Welcome to Remort Level *!");

    if(%remorted) {
        timer::stop(%remorted);
        Schedule::Cancel("getLVL();", 1);
        autoskills();
    }
}
        

function autoskills() {
    if($ultimateAFK) {
        %i = 0.5;
        $currently::autoskilling = true;

        schedule("remoteeval(2048, ScoresOn);", %i++);
        schedule("ClientMenuSelect(sp);", %i++);

//      ClientMenuselect("page 1"); //Pages 1-4
//      ClientMenuSelect("2 1"); //First number is skill 1-23, second number is the page to show after.

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


function cast::transport() { //This is in place because sometimes the script misses the "#cast transport" command. Probably something to do with Presto's "schedule" function.
    if($intending::to::transport::jaten) {
        $intending::to::transport::jaten = false;
        say(1,"#cast transport jaten");
        schedule("%current::lvl = DeusRPG::FetchData(\"Weight\");", 1);
        schedule("%current::lvl = DeusRPG::FetchData(\"Weight\");", 2);
        schedule("%current::lvl = DeusRPG::FetchData(\"Weight\");", 3);
        schedule::add("cast::transport();", 5);
    }

    else {
        say(1,"#cast transport tene");
        schedule("%current::lvl = DeusRPG::FetchData(\"Weight\");", 1);
        schedule("%current::lvl = DeusRPG::FetchData(\"Weight\");", 2);
        schedule("%current::lvl = DeusRPG::FetchData(\"Weight\");", 3);
        schedule::add("cast::transport();", 5);
    }
}


function timer::stop(%remorted) {
    $timer::on = false;
    schedule::cancel("remort::timer();");

    if(%remorted) { //If the timer was stopped due to remorting
        exec("remortTimes.cs");
        %remort::level = DeusRPG::FetchData("REMORTSTEP");
        $time::remort::[%remort::level] = convertTimeFormat($current::remort::time)@ " with " @convertTimeFormat($current::remort::time::KLS)@ " spent using KLS";
        $current::remort::time = 0;
        $current::remort::time::KLS = 0;

        export("$time::remort::*", "config\\remortTimes.cs");
        Client::centerPrint("<jc><f0>Remort time: <f1>" @$time::remort::[%remort::level], 1);
        schedule("Client::centerPrint(\"\", 1);", 20);
    }
}


function timer::start(%current::lvl) {
    $timer::on = true;
    remort::timer();
    setHudTimer($current::remort::time);
}


function remort::timer() {
    if($timer::on) {
        $current::remort::time++;

        if($usingKLS) {
            $current::remort::time::KLS++;
        }

        schedule::add("remort::timer();", 1);
    }
}




event::Attach(eventClientMessage, transporting);

function transporting(%client, %msg) {
    if($ultimateAFK && Match::String(%msg, "Transporting to Tenebrous Cavern")) {
		schedule::cancel("cast::transport();");

        $maxWeight = DeusRPG::FetchData("MaxWeight"); //Verify that you did, in fact, make a deposit
        $currentWeight = DeusRPG::FetchData("Weight");

        if($currentWeight >= 0.85*$maxWeight) {
            Schedule("say(1,\"#say wtf why am i still overweight\");", 1);
            $intending::to::transport::jaten = true;
            // $intending::to::transport::tene = false;
            $determine::tene::position = false;
            $cast::interrupt::check = true;
            schedule::add("cast::transport();", 23);
            break;
        }

        $determine::tene::position = true;
        $determine::jaten::position = false;
        $cast::interrupt::check = false;
        $intending::to::transport::tene = false;
        $currently::autoskilling = false;
        $DeusRPG::toggleFire = true; //"True" means don't fire, for some reason
        $checkpoint::3::tries = 0;

        Schedule::add("triangulate();", 1, "triangulate");
        if($KLS::override::ON = true && $usingKLS = true) {
			Schedule("use(\"Keldrinite Long Sword\");", 2);
		} 
		else {
			Schedule("use(\"Anchet's Sword\");", 2);
		}
        Schedule("use(\"Light Robe\");", 3);
        Schedule::Cancel("preTransportCheck();", 4);
        schedule("timer::start(%current::lvl);", 5);
    }

    if($ultimateAFK && Match::String(%msg, "Transporting to Jaten Outpost")) {
        $determine::jaten::position = true;
        $determine::tene::position = false;
        $cast::interrupt::check = false;
        $intending::to::transport::jaten = false;
        $currently::autoskilling = false;
        $DeusRPG::toggleFire = true; //"True" means don't fire, for some reason
        Schedule("say(1,\"#say Gonna laugh all the way to the bank cuz I'm a fatty...\");", 1);
        Schedule::add("triangulate();", 2, "triangulate");
        Schedule::Cancel("preTransportCheck();", 3);
    }
}


event::Attach(eventClientMessage, cast::interrupt);

function cast::interrupt(%client, %msg) {
    if(Match::String(%msg, "Your casting was interrupted.") && $cast::interrupt::check) {
        Schedule("say(1,\"#say I'm just going to keep trying until it works out.\");", 1);
        $intending::to::transport::tene = true;
        cast::transport();
    }
}





if($track::target == "Backpack") { //Pack must be dropped in the corner where Parscestar normally stands
    $direction::tene = "North West";
    $pos::A::tene = 1023; //Looking NE, not quite at the top of a hill before dropping down into a valley
    $pos::B::tene = 1054; //Looking NE, not quite at the top of a hill before slowly dropping down the back sidet of a hill, going up another, then down into a valley
    $pos::C::tene = 1069; //Looking NE, see out into a valley with a cliff face on the right
    $pos::D::tene = 1019; //Looking NE, see into a cliff face
	// 1019
	// 1054
	// 1023
	// 1069

    $direction::jaten = "North West";
    $pos::A::jaten = 47; //Far left side of front
    $pos::B::jaten = 49; //Left side, out a bit, a hair to the left of plant
    $pos::C::jaten = 39; //Left side, in closer, positioned to the right of plant
    $pos::D::jaten = 35; //Middle, out a bit, directly in front of entrance
}


function triangulate() {
    if($ultimateAFK) {
        // Schedule("say(1,\"#say Tracking \" @ $track::target @ \"...\");", 0);
        Schedule("say(1,\"#trackpack caboose 1\");", 0);
        Schedule::Add("triangulate();", 5, "triangulate");
    }
}

event::Attach(eventClientMessage, sensing);

function sensing(%client, %msg) {
    if($determine::tene::position) {
        if(Match::String(%msg, "Caboose's nearest backpack is " @ $direction::tene @ " of here, * meters away.")) {
            $target::distance = Match::Result(0);

            if($target::distance == $pos::A::tene || $target::distance == $pos::A::tene + 1 || $target::distance == $pos::A::tene - 1) {
                $determine::tene::position = false;
                Schedule::Cancel("triangulate");
                // Schedule("say(1,\"#say Tenebrous Cave, Position A\");", 2);
                Schedule("from::pos::A::tene();", 3);
            }
            else if($target::distance == $pos::B::tene || $target::distance == $pos::B::tene + 1 || $target::distance == $pos::B::tene - 1) {
                $determine::tene::position = false;
                Schedule::Cancel("triangulate");
                // Schedule("say(1,\"#say Tenebrous Cave, Position B\");", 2);
                Schedule("from::pos::B::tene();", 3);
            }
            else if($target::distance == $pos::C::tene || $target::distance == $pos::C::tene + 1 || $target::distance == $pos::C::tene - 1) {
                $determine::tene::position = false;
                Schedule::Cancel("triangulate");
                // Schedule("say(1,\"#say Tenebrous Cave, Position C\");", 2);
                Schedule("from::pos::C::tene();", 3);
            }
            else if($target::distance == $pos::D::tene || $target::distance == $pos::D::tene + 1 || $target::distance == $pos::D::tene - 1) {
                $determine::tene::position = false;
                Schedule::Cancel("triangulate");
                // Schedule("say(1,\"#say Tenebrous Cave, Position D\");", 2);
                Schedule("from::pos::D::tene();", 3);
            }
            else {
                $determine::tene::position = false;
                Schedule("say(1,\"#say I'm at Tenebrous Cave, but something's not right. Let's try this again.\");", 1);
                Schedule::Cancel("triangulate");
                $intending::to::transport::tene = true;
                schedule::add("cast::transport();", 3);
                $cast::interrupt::check = true;
            }
        }
		
		else if(Match::String(%msg, "Caboose doesn't have any dropped backpacks.")) {
			schedule::cancel("triangulate");
			startAutoPlacePack();
		}	
    }

    if($determine::jaten::position) { // For auto-deposit at Jaten script
        if(Match::String(%msg, "Caboose's nearest backpack is " @ $direction::jaten @ " of here, * meters away.")) {
            $target::distance = Match::Result(0);

            if($target::distance == $pos::A::jaten) {
                $determine::jaten::position = false;
                Schedule::Cancel("triangulate");
                Schedule("say(1,\"#say Jaten Outpost, Position A\");", 2);
                Schedule("from::pos::A::jaten();", 3);
            }
            else if($target::distance == $pos::B::jaten) {
                $determine::jaten::position = false;
                Schedule::Cancel("triangulate");
                Schedule("say(1,\"#say Jaten Outpost, Position B\");", 2);
                Schedule("from::pos::B::jaten();", 3);
            }
            else if($target::distance == $pos::C::jaten) {
                $determine::jaten::position = false;
                Schedule::Cancel("triangulate");
                Schedule("say(1,\"#say Jaten Outpost, Position C\");", 2);
                Schedule("from::pos::C::jaten();", 3);
            }
            else if($target::distance == $pos::D::jaten) {
                $determine::jaten::position = false;
                Schedule::Cancel("triangulate");
                Schedule("say(1,\"#say Jaten Outpost, Position D\");", 2);
                Schedule("from::pos::D::jaten();", 3);
            }
            else {
                $determine::jaten::position = false;
                Schedule("say(1,\"#say I'm at Jaten Outpost, but something's not right. Let's try this again.\");", 1);
                Schedule::Cancel("triangulate");
                $intending::to::transport::jaten = true;
                schedule::add("cast::transport();", 2);
                $cast::interrupt::check = true;
            }
        }
    }
}



// Tenebrous Cave Movement Functions

function from::pos::A::tene() {
    schedule("Backward();", 1);
    schedule("MoveRight();", 1);
    schedule("StopBackward();", 6);
    schedule("Backward();", 6.5);
    schedule("StopBackward();", 10);
    schedule("Backward();", 10.25);
    schedule("StopMoveRight();", 20);
    schedule("StopBackward();", 22);
    // schedule("say(1,\"#say Checking for waypoint...\");", 23);
    schedule("$check1 = true;", 24);
    schedule("triangulate();", 25);
}

function from::pos::B::tene() {
    schedule("MoveRight();", 1);
    schedule("StopMoveRight();", 2.75);
    schedule("Backward();", 3);
    schedule("MoveRight();", 3);
    schedule("StopBackward();", 4);
    schedule("Backward();", 4.5);
    schedule("StopMoveRight();", 14);
    schedule("StopBackward();", 16);
    // schedule("say(1,\"#say Checking for waypoint...\");", 17);
    schedule("$check1 = true;", 18);
    schedule("triangulate();", 19);
}

function from::pos::C::tene() {
    schedule("Backward();", 1);
    schedule("MoveLeft();", 1);
    schedule("StopBackward();", 2.6);
    schedule("StopMoveLeft();", 2.6);
    schedule("MoveRight();", 2.75);
    schedule("Backward();", 3);
    schedule("StopBackward();", 9);
    schedule("Backward();", 9.25);
    schedule("StopMoveRight();", 15);
    schedule("StopBackward();", 17);
    // schedule("say(1,\"#say Checking for waypoint...\");", 18);
    schedule("$check1 = true;", 19);
    schedule("triangulate();", 20);
}

function from::pos::D::tene() {
    Schedule("Forward();", 1);
    schedule("MoveRight();", 1);
    schedule("StopForward();", 4.75);
    schedule("StopMoveRight();", 4.75);
    schedule("Backward();", 6);
    schedule("MoveRight();", 6);
    schedule("StopBackward();", 14);
    schedule("Backward();", 14.5);
    schedule("StopMoveRight();", 22);
    schedule("StopBackward();", 24);
    // schedule("say(1,\"#say Checking for waypoint...\");", 25);
    schedule("$check1 = true;", 26);
    schedule("triangulate();", 27);
}


// Jaten Outpost Movement Functions

function from::pos::A::jaten() {
    $intending::to::transport::tene = true;
    $intending::to::transport::jaten = false;
    schedule("Backward();", 1);
    schedule("MoveLeft();", 1);
    schedule("StopBackward();", 8.5);
    schedule("StopMoveLeft();", 8.5);
    schedule("Forward();", 9);
    schedule("MoveLeft();", 9);
    schedule("StopForward();", 10);
    schedule("StopMoveLeft();", 10);
    schedule("say(1,\"#say hi\");", 11);
    schedule("say(1,\"#say deposit\");", 12);
    schedule("say(1,\"#say all\");", 13);
    schedule::add("cast::transport();", 20);
}

function from::pos::B::jaten() {
    $intending::to::transport::tene = true;
    $intending::to::transport::jaten = false;
    schedule("Backward();", 1);
    schedule("MoveLeft();", 2.5);
    schedule("StopBackward();", 10);
    schedule("StopMoveLeft();", 10);
    schedule("Forward();", 11);
    schedule("MoveLeft();", 11);
    schedule("StopForward();", 12);
    schedule("StopMoveLeft();", 12);
    schedule("say(1,\"#say hi\");", 13);
    schedule("say(1,\"#say deposit\");", 14);
    schedule("say(1,\"#say all\");", 15);
    schedule::add("cast::transport();", 20);
}

function from::pos::C::jaten() {
    $intending::to::transport::tene = true;
    $intending::to::transport::jaten = false;
    schedule("Backward();", 1);
    schedule("MoveLeft();", 3.25);
    schedule("StopBackward();", 9);
    schedule("StopMoveLeft();", 9);
    schedule("Forward();", 10);
    schedule("MoveLeft();", 10);
    schedule("StopForward();", 11);
    schedule("StopMoveLeft();", 11);
    schedule("say(1,\"#say hi\");", 12);
    schedule("say(1,\"#say deposit\");", 13);
    schedule("say(1,\"#say all\");", 14);
    schedule::add("cast::transport();", 20);
}

function from::pos::D::jaten() {
    $intending::to::transport::tene = true;
    $intending::to::transport::jaten = false;
    schedule("Backward();", 1);
    schedule("MoveRight();", 1);
    schedule("StopMoveRight();", 3);
    schedule("MoveLeft();", 4);
    schedule("StopBackward();", 10);
    schedule("StopMoveLeft();", 10);
    schedule("Forward();", 11);
    schedule("MoveLeft();", 11);
    schedule("StopForward();", 12);
    schedule("StopMoveLeft();", 12);
    schedule("say(1,\"#say hi\");", 13);
    schedule("say(1,\"#say deposit\");", 14);
    schedule("say(1,\"#say all\");", 15);
    schedule::add("cast::transport();", 20);
}





if($track::target == "Backpack") {
    $waypoint::1 = 1128 + $calibration::factor;
}

event::Attach(eventClientMessage, waypoint::check1);

function waypoint::check1(%client, %msg) {
    if($check1 == true) {
        if(Match::String(%msg, "You sense that " @ $track::target @ " is " @ $direction::tene @ " of here, * meters away.") == 1 || Match::String(%msg, "Caboose's nearest backpack is " @ $direction::tene @ " of here, * meters away.")) {
            $checkpoint::1 = Match::Result(0);
            Schedule::Cancel("triangulate");

            if($checkpoint::1 == $waypoint::1) {
				$check1 = false;
				Schedule("point2();", 2);
				
            }
			else if($checkpoint::1 == 1125) {
				schedule("Forward();", 1);
				schedule("StopForward();", 1.25);
				schedule("MoveRight();", 1.75);
				schedule("StopMoveRight();", 2.35);
				schedule("Backward();", 3);
				schedule("StopBackward();", 5);
				schedule("triangulate();", 5.5);
			}
            else {
                $off::target++;
                Schedule("say(1,\"#say I'm off target. I've done this \" @$off::target@ \" times now. Trying again...\");", 1);
                $check1 = false;
                $cast::interrupt::check = true;
                $intending::to::transport::tene = true;
                cast::transport();
            }
        }
    }
}







function point2() {
    Schedule("say(1,\"#say I'm on target! Continuing on to Waypoint #2...\");", 0);
    $off::target = 0;
    schedule("Forward();", 1);
    schedule("StopForward();", 1.5);
    schedule("Forward();", 2);
    schedule("MoveRight();", 2);
    schedule("StopMoveRight();", 5.7);
    schedule("StopForward();", 6.5);
    //schedule("$check2 = true;", 8);
    //schedule("triangulate();", 8.5);
    schedule("point3();", 8);
}




if($track::target == "Backpack") {
    $waypoint::2 = 1150 + $calibration::factor;
}

event::Attach(eventClientMessage, waypoint::check2);

function waypoint::check2(%client, %msg) {
    if($check2 == true) {
        if(Match::String(%msg, "You sense that " @ $track::target @ " is " @ $direction::tene @ " of here, * meters away.") || Match::String(%msg, "Caboose's nearest backpack is " @ $direction::tene @ " of here, * meters away.")) {
            $checkpoint::2 = Match::Result(0);
            Schedule::Cancel("triangulate();", 0);
            $check2 = false;

            if($checkpoint::2 == $waypoint::2) {
                Schedule("point3();", 2);
            }
            else {
                Schedule("say(1,\"#say I'm off target. Trying again...\");", 2);
                $check2 = false;
                $cast::interrupt::check = true;
                $intending::to::transport::tene = true;
                cast::transport();
            }
        }
    }
}





function point3() {
    $check3 = true;
    Schedule("say(1,\"#say I'm on target! Continuing on to Waypoint #3...\");", 0);
    // schedule("Backward();", 1);
    // schedule("StopBackward();", 1.5);
    // schedule("Backward();", 2);
    // schedule("MoveRight();", 2);
    // schedule("StopMoveRight();", 2.5);
    // schedule("MoveRight();", 3);
    // schedule("StopMoveRight();", 6);
    // schedule("MoveRight();", 6.35);
    // schedule("StopMoveRight();", 8.25);
    // schedule("MoveLeft();", 9);
    // schedule("StopMoveLeft();", 9.5);
    // schedule("StopBackward();", 11.5);
    // schedule("Forward();", 12);
    // schedule("MoveRight();", 12.25);
    // schedule("StopForward();", 12.75);
    // schedule("StopMoveRight();", 12.75);
    // schedule("triangulate();", 14);
	schedule("Backward();", 1);
    schedule("StopBackward();", 1.5);
    schedule("Backward();", 2);
    schedule("MoveRight();", 2);
    schedule("StopMoveRight();", 2.5);
    schedule("MoveRight();", 3.25);
    schedule("StopMoveRight();", 6);
    schedule("MoveRight();", 6.35);
    schedule("StopBackward();", 8);
    schedule("StopMoveRight();", 10.5);
	// Should be at corner of tene and big rock
    schedule("MoveLeft();", 11);
    schedule("Backward();", 11.5);
    schedule("StopMoveLeft();", 11.9);
    schedule("StopBackward();", 12.65);
    // schedule("Forward();", 12);
    // schedule("MoveRight();", 12.25);
    // schedule("StopForward();", 12.75);
    // schedule("StopMoveRight();", 12.75);
    schedule("triangulate();", 13);
}



if($track::target == "Backpack") {
    $waypoint::3 = 1179 + $calibration::factor;
}

event::Attach(eventClientMessage, waypoint::check3);

function waypoint::check3(%client, %msg) {
    if($check3 == true) {
        if(Match::String(%msg, "You sense that " @ $track::target @ " is " @ $direction::tene @ " of here, * meters away.") || Match::String(%msg, "Caboose's nearest backpack is " @ $direction::tene @ " of here, * meters away.")) {
            $checkpoint::3 = Match::Result(0);
            Schedule::Cancel("triangulate");

            if($checkpoint::3 >= $waypoint::3 - 1 && $checkpoint::3 <= $waypoint::3 + 1) {
                Schedule("StartAttacking();", 2);
                $check3 = false;
                $determine::tene::position = false;
            }
            else if($checkpoint::3 <= $waypoint::3 - 2 && $checkpoint::3 >= $waypoint::3 - 6 && $checkpoint::3::tries <= 3) {
                $checkpoint::3::tries++;
                Schedule("say(1,\"#say A bot might have me cornered. Let me back up and try moving again in 10 seconds. If I haven't moved by the third try, I'll just try again from the beginning.\");", 1);
                Schedule("postAction(2048, IDACTION_BREAK1, 1);", 2);
    			Schedule("postAction(2048, IDACTION_FIRE1, 1);", 5);
				Schedule("DeusRPG::AutoCast(\"Powercloud\");", 3);
                Schedule("Backward();", 7);
                Schedule("MoveLeft();", 8);
                Schedule("StopMoveLeft();", 9);
                Schedule("StopBackward();", 16);
                Schedule("Forward();", 16);
                Schedule("MoveRight();", 16.25);
                Schedule("StopMoveRight();", 16.5);
                Schedule("StopForward();", 16.75);
                Schedule("triangulate();", 18);
            }
            else {
                Schedule("say(1,\"#say I'm off target. Trying again...\");", 1);
                $checkpoint::3::tries = 0;
                $check3 = false;
                $determine::tene::position = false;
                $cast::interrupt::check = true;
                $intending::to::transport::tene = true;
                cast::transport();
            }
        }
    }
}        



function StartAttacking() {
    $hitCheck = true;
    $ultimateAFK = true;
	$loop::paws = true;
    $DeusRPG::toggleFire = true;
    // Schedule("say(1,\"#say I'm in place! Starting AFK routine...\");", 0);
    schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
    schedule("postAction(2048, IDACTION_FIRE1, 1);", 4);
    // Schedule("DeusRPG::AutoCast(\"Melt\");", 2);
    Schedule("DeusRPG::AutoCast(\"Powercloud\");", 2);
    Schedule("getLVL();", 3);
    Schedule("startslots();", 4);
	Schedule("start::equiploop();", 5);
    Schedule("KLS::Retrieve();", 6);
    Schedule::Add("castDrift();", 15);
}


event::Attach(eventClientMessage, notHitting); //In case a bot is stuck

function notHitting(%client, %msg) {
    if($ultimateAFK == true && $hitCheck == true) {
        if(Match::String(%msg, "You damaged * for * points of damage!") || Match::String(%msg, "You try to hit *, but miss!") || Match::String(%msg, "You try to hit *, but miss! (1 LCK)")) { //Check to see if you hit someone
            %whoWasHit = Match::Result(0);

            if(%whoWasHit != "yourself" && client::getTeam(GetClientByName(%whoWasHit)) != 0 ) {
                Schedule::Cancel("castDrift();", 1);
				Schedule::Cancel("castDrift();", 2);
				Schedule::Cancel("castDrift();", 3);
                Schedule::Add("castDrift();", 30);
            }
        }
    }
}

function castDrift() {
    if($ultimateAFK == true && $hitCheck == true) {
        $hitCheck = false;
        $castingDrift = true;
        Schedule("say(1,\"#say Looks like I haven't hit anything in a while. Let's try casting Dimension Rift a few times.\");", 1);
        Schedule("Stop::AutoCast();", 2);
        Schedule("DeusRPG::AutoCast(\"Dimensionrift\");", 3);
    }
}

event::Attach(eventClientMessage, backToMelt); 

function backToMelt(%client, %msg) {
    if($ultimateAFK == true && $castingDrift == true) {
        if(Match::String(%msg, "* has died and you gained * experience!")) { //Check to see if you kill the bot
            $hitCheck = true;
            $castingDrift = false;
            Schedule("say(1,\"#say k back to Melt.\");", 1);
            Schedule("Stop::AutoCast();", 2);
            Schedule("DeusRPG::AutoCast(\"Powercloud\");", 3);
            Schedule::Add("castDrift();", 33);
        }
    }
}







//Auto-store and auto-retrieve KLS ////////////////////////////////////////////
$whitelist::player[0] = "Caboose";
$whitelist::player[1] = "SIayer";
$whitelist::player[2] = "Parscestar";
$whitelist::player[3] = "E";
$whitelist::player[4] = "FloPPyPiLLoW";
$whitelist::player[5] = "Leviathan";
$whitelist::player[6] = "vivaxx";
$whitelist::player[7] = "ToTheMoon";
$whitelist::player[8] = "Baphomet";
$whitelist::player[9] = "Dingus";
$whitelist::player[10] = "Behemoth";


function my::onClientJoin(%client) {
    KLS::Join(%client);
}

function my::onClientChangeTeam(%client, %team) {
    KLS::Store(%client, %team);
}

function my::onClientDrop(%client) {
    schedule("KLS::Retrieve(%client);", 1);
}




function KLS::Join(%client) {
    //say(1,"My name is Caboose, and " @ client::getname(%client) @ " connected to the game.");
    Client::centerPrint("<jc><f0>Auto AFK: <f1>" @ client::getname(%client) @ " joined the game.", 1);
    schedule("Client::centerPrint(\"\", 1);", 5);
}


function KLS::Store(%client, %team) {
    if(%team == 0) {
        //say(1,"My name is Caboose, and " @ client::getname(%client) @ " joined team " @ client::getteam(%client) @ ".");
    }

    if($ultimateAFK && %team == 0 && !$KLS::override::ON && getItemCount("Keldrinite Long Sword") > 0 && !$currently::autoskilling && client::getName(%client) != "SIayer") {
        $DeusRPG::toggleFire = true;
        schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
        $usingKLS = false;
        Client::centerPrint("<jc><f0>Auto AFK: <f1>" @ client::getname(%client) @ " joined! Putting KLS in storage.", 1);
        schedule("sell(\"Keldrinite Long Sword\");", 0);
        schedule("KLS::store(%client, %team);", 1); //Won't run if you put away the KLS the first time
        schedule("use(\"Anchet's Sword\");", 2);
		schedule("postAction(2048, IDACTION_BREAK1, 1);", 3);
        schedule("postAction(2048, IDACTION_FIRE1, 1);", 4);
        schedule("Client::centerPrint(\"\", 1);", 10);
    }

    if($ultimateAFK && %team == 0 && getItemCount("Keldrinite Long Sword") > 0 && $currently::autoskilling) {
        schedule("KLS::Store(%client, %team);", 10);
    }
}




function KLS::Retrieve(%client) {
    %p = 0;
    %k = 0;
    
    echo("");
    echo("");
    echo(">>> CONNECTED PLAYERS <<<");
    echo("--------------------------------------------------");
    for(%i = 2000; %i <= 2100; %i++) { //Client ID sweep between 2000 and 2100. This is as arbitrary as this function gets.
        if(client::getname(%i) != "") { //If there is a name associated with the client ID
            if(client::getteam(%i) == 0) { //If the client is on team 0 (Citizen), i.e. if they are a player
                %player::name[%p] = client::getname(%i); //Build an array of players who are still connected to the game
                echo(">> " @%player::name[%p]);
                %p++; //The number of players who are still connected to the game
            }
        }
    }
    //echo(%p);
    echo("");
    
    echo(">>> CONNECTED WHITELISTED PLAYERS <<<");
    echo("-----------------------------------------------------------------------------");
    for(%i = 0; %i <= %p-1; %i++) { //Cycle through all the players still connected to the game
        for(%j = 0; $whitelist::player[%j] != ""; %j++) { //Cycle through all the whitelisted players
            if(%player::name[%i] == $whitelist::player[%j]) { //If the player is on the whitelist
                echo(">> " @$whitelist::player[%j]);
                %k++; //The number of whitelisted players who are connected to the game
            }
        }
    }
    //echo(%k);
    echo("");
    echo("");
    
    if(%p == %k) {
        %safe::for::KLS = true;
    }
    else {
        %safe::for::KLS = false;
    }


    if($ultimateAFK && !%safe::for::KLS && getItemCount("Keldrinite Long Sword") > 0 && !$currently::autoskilling && !$KLS::override::ON) { // NOT safe for use and override is OFF and you are carrying one
        $DeusRPG::toggleFire = false;
        $usingKLS = false;
        Client::centerPrint("<jc><f0>Auto AFK: <f1>Someone is still here! Storing KLS.", 1);
        schedule("sell(\"Keldrinite Long Sword\");", 0);
        //schedule("KLS::Retrieve(%client, %team);", 1); //Won't run if you put away the KLS the first time
        schedule("use(\"Anchet's Sword\");", 2);
        Schedule("DeusRPGPack::func14();", 3);
        Schedule("DeusRPGPack::func14();", 4);
        schedule("Client::centerPrint(\"\", 1);", 10);
    }

    else if($ultimateAFK && !%safe::for::KLS && getItemCount("Keldrinite Long Sword") > 0 && $currently::autoskilling && !$KLS::override::ON) {  // Not safe for use and override is off and you are carrying one but you are currently autoskilling
        schedule("KLS::Retrieve(%client);", 10); //Run again in 10 seconds to store KLS if currently allocating SP
    }

    else if($ultimateAFK && !%safe::for::KLS && getItemCount("Keldrinite Long Sword") == 0 && !$KLS::override::ON) {  // Not safe for use and you're not carrying one and override is OFF
        Client::centerPrint("<jc><f0>Auto AFK: <f1>" @ client::getname(%client) @ " has left the game, but others are still here.", 1);
        schedule("Client::centerPrint(\"\", 1);", 5);
    }

    else if($ultimateAFK && %safe::for::KLS && getItemCount("Keldrinite Long Sword") > 0 && !$currently::autoskilling) { // Safe for use and you're already carrying one
        $DeusRPG::toggleFire = true;
        schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
        $usingKLS = true;
        Client::centerPrint("<jc><f0>Auto AFK: <f1>Looks like no one is here. Using KLS!", 1);
        //schedule("KLS::Retrieve();", 30); //Won't run if you got the KLS the first time
        Schedule("use(\"Keldrinite Long Sword\");", 4);
		schedule("postAction(2048, IDACTION_BREAK1, 1);", 5);
        schedule("postAction(2048, IDACTION_FIRE1, 1);", 6);
        schedule("Client::centerPrint(\"\", 1);", 10);
    }

    else if($ultimateAFK && !%safe::for::KLS && getItemCount("Keldrinite Long Sword") > 0 && !$currently::autoskilling && $KLS::override::ON) { // NOT safe for use but override is on and you already are carrying one
        $DeusRPG::toggleFire = true;
        schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
        $usingKLS = true;
        Client::centerPrint("<jc><f0>Auto AFK: <f1>KLS Override is ON so we're going to keep using it.", 1);
        //schedule("KLS::Retrieve();", 30); //Won't run if you got the KLS the first time
        Schedule("use(\"Keldrinite Long Sword\");", 4);
		schedule("postAction(2048, IDACTION_BREAK1, 1);", 5);
        schedule("postAction(2048, IDACTION_FIRE1, 1);", 6);
        schedule("Client::centerPrint(\"\", 1);", 10);
    }

    else if($ultimateAFK && %safe::for::KLS && getItemCount("Keldrinite Long Sword") == 0 && !$currently::autoskilling) {  // Safe for use regardless of override and you aren't carrying one
        $DeusRPG::toggleFire = true;
        schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
        $usingKLS = true;
        Client::centerPrint("<jc><f0>Auto AFK: <f1>Everyone who I don't trust is gone! Getting KLS from storage.", 1);
        schedule("buy(\"Keldrinite Long Sword\");", 0);
        //schedule("KLS::Retrieve();", 1); //Won't run if you got the KLS the first time
        schedule("use(\"Keldrinite Long Sword\");", 4);
		schedule("postAction(2048, IDACTION_BREAK1, 1);", 5);
        schedule("postAction(2048, IDACTION_FIRE1, 1);", 6);
        schedule("Client::centerPrint(\"\", 1);", 10);
    }

    else if($ultimateAFK && !%safe::for::KLS && getItemCount("Keldrinite Long Sword") == 0 && !$currently::autoskilling && $KLS::override::ON) {  // Not safe for use but override is on and you aren't carrying one
        $DeusRPG::toggleFire = true;
        schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
        $usingKLS = true;
        Client::centerPrint("<jc><f0>Auto AFK: <f1>KLS Override is ON. Getting KLS from storage.", 1);
        schedule("buy(\"Keldrinite Long Sword\");", 0);
        //schedule("KLS::Retrieve();", 1); //Won't run if you got the KLS the first time
        schedule("use(\"Keldrinite Long Sword\");", 4);
		schedule("postAction(2048, IDACTION_BREAK1, 1);", 5);
        schedule("postAction(2048, IDACTION_FIRE1, 1);", 6);
        schedule("Client::centerPrint(\"\", 1);", 10);
    }

    else if($ultimateAFK && %safe::for::KLS && getItemCount("Keldrinite Long Sword") == 0 && $currently::autoskilling) {
        schedule("KLS::Retrieve(%client);", 10); //Run again 10 seconds to get KLS if currently allocating SP
    }
}
// End Auto Store/Retrieve KLS ////////////////////////////////




// KLS Safeguard Override /////////////////////////////////////
// By default, this will ALWAYS be OFF when starting a new session, and/or when remorting
// If you want to use the KLS while non-whitelisted players are on, you will have to reset the override every remort

function KLS::Override() {
    if(!$KLS::override::ON) {
        $KLS::override::ON = true;
        Client::centerPrint("<jc><f0>Auto AFK: <f1>>> KLS Override is <f2>ON <f1><<<", 1);

        if($ultimateAFK && getItemCount("Keldrinite Long Sword") == 1) { //If you already have a KLS in your invo
            $DeusRPG::toggleFire = true;
            $usingKLS = true;
            schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
            schedule("use(\"Keldrinite Long Sword\");", 2);
            schedule("postAction(2048, IDACTION_FIRE1, 1);", 3);
        }

        else if($ultimateAFK && getItemCount("Keldrinite Long Sword") == 0) {
            $DeusRPG::toggleFire = true;
            $usingKLS = true;
            schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
            schedule("buy(\"Keldrinite Long Sword\");", 2);
            schedule("use(\"Keldrinite Long Sword\");", 3);
            schedule("postAction(2048, IDACTION_FIRE1, 1);", 4);
        }

        schedule("Client::centerPrint(\"\", 1);", 20);
    }

    else if($KLS::override::ON) {
        $KLS::override::ON = false;
        schedule("KLS::Retrieve();", 5);
        Client::centerPrint("<jc><f0>Auto AFK: <f1>>> KLS Override is <f2>OFF <f1><<<", 1);
        schedule("Client::centerPrint(\"\", 1);", 20);
    }
}
// End KLS Safeguard Override /////////////////////////////////




// Auto-equip for Fastswing ///////////////////////////////////
function start::equiploop() {
	run::equiploop();
    schedule("postAction(2048, IDACTION_FIRE1, 1);", 0);
    Client::centerPrint("<jc><f0>Caboose Auto-Equip: <f1>On", 1);
    schedule("Client::centerPrint(\"\", 1);", 5);
}

function stop::equiploop() {
	$loop::wws = false;
    $loop::paws = false;
    schedule::cancel("run::equiploop();");
    postAction(2048, IDACTION_BREAK1, 1);
    $DeusRPG::toggleFire = true;
    schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
    schedule("equip();", 2);
    Client::centerPrint("<jc><f0>Caboose Auto-Equip: <f1>Off", 1);
    schedule("Client::centerPrint(\"\", 1);", 5);
    gemStats::Save();
}

function run::equiploop() { //Wind walkers
	if($loop::wws) {
        schedule("equip();", 0);
	    schedule("unequip();", 1);
        schedule::add("run::equiploop();", 2);
    }
	if($loop::paws) { //Cheetaur's Paws
        schedule("equip2();", 0);
	    schedule("unequip2();", 0.9);
        schedule::add("run::equiploop();", 1.8);
    }
}

function equip() { schedule("remoteEval(2048,useItem,21);", 0); } //11 for Cheetaur's Paws, 21 for Wind Walkers
function unequip() { schedule("remoteEval(2048,useItem,22);", 0); } //12 for Cheetaur's Paws, 22 for Wind Walkers

function equip2() { schedule("remoteEval(2048,useItem,11);", 0); } //11 for Cheetaur's Paws, 21 for Wind Walkers
function unequip2() { schedule("remoteEval(2048,useItem,12);", 0); } //12 for Cheetaur's Paws, 22 for Wind Walkers

function equip3() { schedule("remoteEval(2048,useItem,191);", 0); } //191 to equip Phen's Robe
function unequip3() { schedule("remoteEval(2048,useItem,192);", 0); } //192 to unequip Phen's Robe
///////////////////////////////////////////////////////////////////





Menu::New(MenuDeus, "DeusChat Main Menu");
	Menu::AddChoice(MenuDeus, "cCasting Training", "Menu::Display(MenuCast);");
	Menu::AddChoice(MenuDeus, "hHaggling Training", "Menu::Display(MenuH);");
	Menu::AddChoice(MenuDeus, "sSmithingTraining", "Menu::Display(MenuS);");
	Menu::AddChoice(MenuDeus, "xOther Training", "Menu::Display(MenuM);");
	Menu::AddChoice(MenuDeus, "aAutodrop", "Menu::Display(MenuAutodrop);");
	Menu::AddChoice(MenuDeus, "eAutoEnergy", "Menu::Display(MenuAE);");
	Menu::AddChoice(MenuDeus, "rAutoHeal", "Menu::Display(MenuAH);");
	Menu::AddChoice(MenuDeus, "mAuto Mining", "Menu::Display(MenuA);");
	Menu::AddChoice(MenuDeus, "jAuto Jump", "Xin_::JumpToggle();");
	Menu::AddChoice(MenuDeus, "qQuickCast", "Menu::Display(MenuQMM);");
	Menu::AddChoice(MenuDeus, "iInfo Menu", "Menu::Display(MenuMMInfo);");
	Menu::AddChoice(MenuDeus, "nNewbie Guide!", "DeusRPG::toggleGuide();"); //"HUD::ToggleDisplay(DeusNewbieGuide::MAIN);"); //"Menu::Display(MenuNewbieGuide_MAIN);");
	Menu::AddChoice(MenuDeus, "kKeyBind", "DeusRPGHud::toggleDeusKeyBindhud();");
	Menu::AddChoice(MenuDeus, "oOther Options", "Menu::Display(MenuOther);");
	Menu::AddChoice(MenuDeus, "lLinks", "Menu::Display(MenuHTML1);");
    Menu::AddChoice(MenuDeus, "----------------------------", "");
    Menu::AddChoice(MenuDeus, "fAutoAFK Options (F)", "Menu::Display(MenuAutoAFK);");

Menu::New(MenuAutoAFK, "Auto AFK Options");
    Menu::AddChoice(MenuAutoAFK, "sAutoAFK (In Place) (s)", "startAutoAFK();");
    Menu::AddChoice(MenuAutoAFK, "iAutoAFK (Not In Place) (i)", "$ultimateAFK = true; say(1,\"#cast transport tene\");");
    Menu::AddChoice(MenuAutoAFK, "xStop AutoAFK Script (x)", "stopAutoAFK();");
    Menu::AddChoice(MenuAutoAFK, "-----------------------------------", "blank();");
    Menu::AddChoice(MenuAutoAFK, "wStart  Auto-Equip WW's (W)", "$loop::wws = true; $loop::paws = false; start::equiploop();");
    Menu::AddChoice(MenuAutoAFK, "eStart  Auto-Equip Paws (E)", "$loop::wws = false; $loop::paws = true; start::equiploop();");
    Menu::AddChoice(MenuAutoAFK, "qStop Auto-Equip (Q)", "stop::equiploop();");
    Menu::AddChoice(MenuAutoAFK, "+----------------------------------", "blank();");
    Menu::AddChoice(MenuAutoAFK, "kOverride KLS Safeguard (K)", "KLS::Override();");
    Menu::AddChoice(MenuAutoAFK, "aAnnounce Remort Stats (A)", "announce::remort();");


function announce::remort() {
    exec("remortTimes.cs");
    %remort::level = DeusRPG::FetchData("REMORTSTEP");
    %remort::to::announce = $time::remort::[%remort::level - 1];
    say(0, "Auto AFK Stats: I am RL " @%remort::level@ " and my last remort took " @%remort::to::announce);
}