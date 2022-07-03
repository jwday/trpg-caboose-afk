include("presto\\Match.cs");
include("presto\\Event.cs");
include("presto\\Schedule.cs");
include("convertTimeFormat.cs");
include("clearMineStats.cs");
include("100.cs");
$fastmine::override::ON = false; //Default to OFF every time Normal Siayer Mining is initiated


// Modify DeusChatbind.cs upon starting Tribes ////////////////////////////////////
Menu::New(MenuA, "Auto Mining");
	Menu::AddChoice(MenuA, "tXin_ Auto Mining: Quit = TRUE", "Mining::Start(TRUE);");
	Menu::AddChoice(MenuA, "fXin_ Auto Mining: Quit = FALSE", "Mining::Start(FALSE);");
	Menu::AddChoice(MenuA, "^----------------------------------", "blank();");
	Menu::AddChoice(MenuA, "1Deus Auto Mining: Quit = TRUE", "Mining::Start(TRUE, old);");
	Menu::AddChoice(MenuA, "2Deus Auto Mining: Quit = FALSE", "Mining::Start(FALSE, old);");
	Menu::AddChoice(MenuA, "zStop Auto Mining", "Mining::Stop();");
	Menu::AddChoice(MenuA, "*----------------------------------", "blank();");
    Menu::AddChoice(MenuA, "cCaboose Mining, Normal (C)", "cabmining::choose::normal();");
	Menu::AddChoice(MenuA, "vCaboose Mining, Fast (V)", "cabmining::choose::fast();");
    Menu::AddChoice(MenuA, "oOverride Fast Mine Safety (O)", "cabmining::choose::fast::override::toggle();");
	Menu::AddChoice(MenuA, "xStop Caboose Mining (X)", "stopcabmining();");
    Menu::AddChoice(MenuA, "-----------------------------------", "blank();");
    Menu::AddChoice(MenuA, "sCaboose Mining Stats (S)", "mineStats::Recall();");
    Menu::AddChoice(MenuA, "aAnnounce Mining Stats (A)", "mineStats::Announce();");
    Menu::AddChoice(MenuA, "lAnnounce Fake Mining Stats (L)", "mineStats::Fake::Announce();");
    Menu::AddChoice(MenuA, "= - - - - - - - - - - - -", "blank();");
    Menu::AddChoice(MenuA, "bCaboose Mining Breakdown (B)", "mineStats::Breakdown();");
    Menu::AddChoice(MenuA, "nAnnounce Mining Breakdown (N)", "mineStats::Announce::Breakdown();");
    Menu::AddChoice(MenuA, "kAnnounce Fake Breakdown (K)", "mineStats::Fake::Announce::Breakdown();");
    Menu::AddChoice(MenuA, "+----------------------------------", "blank();");
    Menu::AddChoice(MenuA, "4Auto Retrieve/Sell Gems (4)", "startautobuysellloop();");
    Menu::AddChoice(MenuA, "5Stop All Auto Buy/Sell (5)", "stopallbuysell();");
    Menu::AddChoice(MenuA, "_----------------------------------", "blank();");
    Menu::AddChoice(MenuA, "0Clear Mine Stats (0)", "arm::clearMineStats();");
    Menu::AddChoice(MenuA, "3Disarm Clear Mine Stats (3)", "disarm::clearMineStats();");
    Menu::AddChoice(MenuA, "&----------------------------------", "blank();");
    Menu::AddChoice(MenuA, "pToggle Spam Coin Packs (p)", "spam::ToggleCoinPacks();");

// End Modify DeusChatbind.cs /////////////////////////////////////////////////////




// Caboose Mining Mode Select /////////////////////////////////////////////////////
function cabmining::choose::normal() {
    $fastswing = 0; //False
    $mining::mode = "Normal";
    startcabmining();
}

function cabmining::choose::fast() {
    $fastswing = 1; //True
    $mining::mode = "Fast";
    startcabmining();
}

function cabmining::choose::fast::override::toggle() {
    if(!$fastmine::override::ON) {
        $fastswing = 1;
        $fastmine::override::ON = true;
        $override::mode = "ON";
        startloop1();
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Fastmine Safeguard Override <f2>" @$override::mode , 1);
    }
    else if($fastmine::override::ON) {
        $fastswing = 0; //False
        $loop1 = false;
        $loop2 = false;
        schedule("equip();", 1);
        $fastmine::override::ON = false;
        $override::mode = "OFF";
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Fastmine Safeguard Override <f2>" @$override::mode , 1);
        schedule("Client::centerPrint(\"\", 1);", 5);
    }

    
}
// End Caboose Mining Mode Select ////////////////////////////////////////////////




// Caboose Mining Controls //////////////////////////////////////////////////////
function startcabmining() {
    if($mineOn) { //If you switch mining styles without stopping first
        mineStats::Save();
    }
    else {
        exec("mineStats.cs");
        $mine::time::round::partial = $mineStats::time::round::partial; //Every "round" is the period of time it takes to get 500 Diamonds. It starts over after the sell loop finishes
        $mine::time::total = $mineStats::time::total;
        $mine::time::round::beginning = getSimTime();
    }


    //$alt::mine::time::seconds = $mineStats::alt::time::seconds;
    //$alt::mine::time::minutes = $mineStats::alt::time::minutes;
    //$alt::mine::time::hours = $mineStats::alt::time::hours;

    $itemized::gem::total[Diamond] = $mineStats::itemized::gem::total[Diamond];
    $itemized::gem::total[Emerald] = $mineStats::itemized::gem::total[Emerald];
    $itemized::gem::total[Titanium] = $mineStats::itemized::gem::total[Titanium];
    $itemized::gem::total[Silver] = $mineStats::itemized::gem::total[Silver];
    $itemized::gem::total[Sapphire] = $mineStats::itemized::gem::total[Sapphire];
    $itemized::gem::total[Topaz] = $mineStats::itemized::gem::total[Topaz];
    $itemized::gem::total[Iron] = $mineStats::itemized::gem::total[Iron];
    $itemized::gem::total[Turquoise] = $mineStats::itemized::gem::total[Turquoise];

    $gem::total = $mineStats::gem::total;

    $mineOn = true;
    $alreadySellingGems = false;
	$position = 1;
    stopslots();

    if(getItemCount("Thorr's Hammer") > 0) {
        use("Thorr's Hammer");
    }
    else if(getItemCount("Pick Axe") > 0) {
        use("Pick Axe");
    }
    else {
        schedule::cancel("Client::centerPrint(\"\", 1);");
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>No mining tool, dumbass!", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 20);
        break;
    }

    startloop1();
    setHudTimer($mineStats::time::total);
    schedule::cancel("Client::centerPrint(\"\", 1);");
    Client::centerPrint("<jc><f0>Caboose Mining: <f1>Started<nl><f0>Mode: <f1>" @$mining::mode@ "<nl><f0>Total mining time: <f2>" @convertTimeFormat($mineStats::time::total)@ "<nl><f0>Total gem value: <f1>$ <f2>" @$mineStats::gem::total@ "<nl><f0>Overall mining rate: <f1>$ <f2>" @$mineStats::rate::average@ "<f1>/second.", 1);
    schedule::add("Client::centerPrint(\"\", 1);", 20);
    Schedule::Add("safeguard();", 15);
}

function pausecabmining() {
    mineStats::Save();
	$loop1 = false;
   	$loop2 = false;
	$miningMove = false;
    Schedule::cancel("mineWalk();");
    Schedule::cancel("safeguard();");
    postAction(2048, IDACTION_BREAK1, 1);
    $DeusRPG::toggleFire = true;
}

function stopcabmining() {
    if($mineOn) {
        $mine::time::round::end = getSimTime(); //If and when you stop Caboose Mining without going through a storage loop
        $mine::time::round::partial = $mine::time::round::end - $mine::time::round::beginning;
        $mine::time::total = $mine::time::total + $mine::time::round::partial;
        mineStats::Save();

	    $loop1 = false;
   	    $loop2 = false;
	    $miningMove = false;
        $mineOn = false;
        postAction(2048, IDACTION_BREAK1, 1);
        $DeusRPG::toggleFire = true;
        Schedule::cancel("mineWalk();");
        Schedule::cancel("safeguard();");
        schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
        schedule("equip();", 2);
        schedule::cancel("Client::centerPrint(\"\", 1);");
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Stopped<nl><f0>Total mining time: <f2>" @convertTimeFormat($mineStats::time::total)@ "<nl><f0>Total gem value: <f2>" @$mineStats::gem::total@ " <f1>coins<nl><f0>Overall mining rate: <f1>$ <f2>" @$mineStats::rate::average@ "<f1>/second.", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 20);
    }
}
// End Cabose Mining Controls //////////////////////////////////////////////////




// Initialize Caboose Mining Loop /////////////////////////////////////////////
function startloop1() {
    if($mineON) {
	    $loop1 = true;
	    $miningMove = true;
        $foundgems = false;
        $DeusRPG::toggleFire = false;
        postAction(2048, IDACTION_FIRE1, 1);

        if($fastswing == 1) {
	        blah1();
        }
        
        Schedule::Add("mineWalk();", 3);
    }
}
// End Initialize Caboose Mining Loop /////////////////////////////////////////




// Auto-equip for Fastswing ///////////////////////////////////////////////////
event::Attach(eventClientMessage, fastMineLoop);

function fastMineLoop(%client, %msg) {
	if ($mineON && $fastswing) {
		if(Match::String(%msg, "You equipped Wind Walkers.")) {
			schedule::cancel("blah1safety");
			schedule::add("remoteEval(2048,useItem,22); echo(\"*** UNEQUIP ***\");", 0.9); // Unequip Wind Walkers
			schedule::add("blah1(); echo(\"*** BLAH1 SAFETY ADDED ***\");", 5, "blah1safety");
			// echo(">>> EQUIPPED");
		}
		else if(Match::String(%msg, "You unequipped Wind Walkers.")) {
			schedule::cancel("blah1safety");
			schedule::add("remoteEval(2048,useItem,21); echo(\"*** EQUIP ***\");", 0.9); // Equip Wind Walkers
			schedule::add("blah1(); echo(\"*** BLAH1 SAFETY ADDED ***\");", 5, "blah1safety");
			// echo(">>> UNEQUIPPED");
		}
	}
}

function blah1() { //Wind walkers
	if($loop1) {
		schedule::add("equip(); echo(\"*** EQUIP BLAH1 ***\");", 0, "equipWWs");
		schedule::add("unequip(); echo(\"*** UNEQUIP BLAH1 ***\");", 1, "unequipWWs");
        schedule::add("blah1(); echo(\"*** BLAH1 SAFETY ADDED FROM BLAH1***\");", 5, "blah1safety");
    }
}

function blah2() { //Cheetaur's paws
	if($loop2) {
        schedule::add("equip2();", 0);
	    schedule::add("unequip2();", 1);
        schedule::add("blah2();", 2);
    }
}

function equip() {
	schedule::add("remoteEval(2048,useItem,21);", 0); // Equip Wind Walkers
	// schedule::add("remoteEval(2048,useItem,187);", 0.9); // Equip Elven Robe
}
function unequip() { 
	schedule::add("remoteEval(2048,useItem,22);", 0); // Unequip Wind Walkers
	// schedule::add("remoteEval(2048,useItem,188);", 0.9); // Unequip Elven Robe
}

function equip2() { schedule("remoteEval(2048,useItem,11);", 0); } //11 for Cheetaur's Paws, 21 for Wind Walkers
function unequip2() { schedule("remoteEval(2048,useItem,12);", 0); } //12 for Cheetaur's Paws, 22 for Wind Walkers
// End Auto-equip for Fastswing ///////////////////////////////////////////////
function equipSweep() {
	for(%i = 0; %i<401; %i++) {
		%time = %i*0.5 + 0.5;
		schedule::add("remoteEval(2048, useItem, \"" @ %i @ "\");", %time);
		schedule::add("echo(\"" @ %i @ "\");", %time);

		// schedule("say(0, \"" @ %msg @ "\");", 0.5);
		// schedule("say(0, \"" @ escapeString(%msg) @ "\");", 1);
	}
}

// 187 Elven robe
//189
//191

// Caboose Mining Timekeeper /////////////////////////////////////////////////
//function mining::timekeeper() {
//    if($mineOn) {
//        $mine::time = $mine::time++;
//        $mine::time::round = $mine::time::round++;

//        $mine::time::string = convertTimeFormat($mine::time);
//        $mine::time::round::string = convertTimeFormat($mine::time::round);

//        // My alternative timekeeping method
//        $alt::mine::time::seconds++;

//        if($alt::mine::time::seconds == 60) {
//            $alt::mine::time::seconds = 0;
//            $alt::mine::time::minutes++;
//        }

//        if($alt::mine::time::minutes == 60) {
//            $alt::mine::time::minutes = 0;
//            $alt::mine::time::hours++;
//        }

//        $alt::mine::time::string = $alt::mine::time::hours@ ":" @$alt::mine::time::minutes@ ":" @$alt::mine::time::seconds;
//        // End my alternative timekeeping method

//        schedule("mining::timekeeper();", 1);
//    }
//}
// End Caboose Mining Timekeeper /////////////////////////////////////////////




// Caboose Mining Loop ///////////////////////////////////////////////////////
event::Attach(eventClientMessage, mineLoop);

function mineLoop(%client, %msg) {
    if($mineOn && $miningMove) {
    	$foundgems = Match::String(%msg, "You found *");

	    if($foundgems) {
            if($alreadySellingGems == false) {
                schedule("gemstore();", 0.5); //Delay to make sure all gem counts are up to date before initiating gemstore();
            }
            $safeguardRecover = false;
	    	Schedule::Cancel("mineWalk();");
	    	Schedule::Cancel("safeguard();");
            if($fastswing == 1) {
                Schedule::Add("mineWalk();", 2);
            }
            else {
                Schedule::Add("mineWalk();", 4);
            }
            Schedule("$foundgems = false;", 2);
            Schedule::Add("safeguard();", 15);
        }

        if($safeguardRecover) {
            $safeguardRecover = false;
            Schedule::Add("mineWalk();", 4);
            Schedule::Add("safeguard();", 15);
        }
    }
}
// End Caboose Mining Loop ///////////////////////////////////////////////////




// Caboose Mining Movement Control ///////////////////////////////////////////
function mineWalk() {
    if($mineOn && $miningMove) {
	    if($position == 1) {
	  	    Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.3);
            if($fastswing == 1) {
                Schedule::Add("mineWalk();", 1.5);
            }
            else {
                Schedule::Add("mineWalk();", 3);
            }
		    $position = 4;
            return;
	    }

        if($position == 4) {
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 0.4);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.4);
            if($fastswing == 1) {
                Schedule::Add("mineWalk();", 1.5);
            }
            else {
                Schedule::Add("mineWalk();", 3);
            }
		    $position = 3;
            return;
	    }

        if($position == 3) {
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 0.4);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.4);
            if($fastswing == 1) {
                Schedule::Add("mineWalk();", 1.5);
            }
            else {
                Schedule::Add("mineWalk();", 3);
            }
		    $position = 2;
            return;
	    }

        if($position == 2) {
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 0.4);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.4);
            if($fastswing == 1) {
                Schedule::Add("mineWalk();", 1.5);
            }
            else {
                Schedule::Add("mineWalk();", 3);
            }
		    $position = 1;
            return;
	    }
    }
}
// End Caboose Mining Movement Control ///////////////////////////////////////




// Caboose Mining Safeguard //////////////////////////////////////////////////
function safeguard() {
    if($mineOn && $miningMove) {
        schedule::cancel("Client::centerPrint(\"\", 1);");
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Initiating safeguard routine", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 5);
        pausecabmining();

        Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 1);
        Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 3);

        Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.00000);", 4);
        Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 4.5);

        Schedule("startloop1();", 5);
        Schedule::Add("safeguard();", 25);

        $position = 1;
        $safeguardRecover = true;
    }

    if($mineOn && $fastswing == 1) {
        onClientDrop(); //Check every time safeguard initiates to verify you are, in fact, alone
    }
}
// End Caboose Mining Safeguard //////////////////////////////////////////////




// Caboose Mining Gem Storage ////////////////////////////////////////////////
function gemstore() { //Based only on Diamond count
    if($mineOn && $miningMove) {
        if(getItemCount("Diamond") > 1000) {
            $alreadySellingGems = true;
            $round::gem::total = 0;
            $mine::time::round::end = getSimTime(); //This ends the timer for the previous round of mining

            if($fastswing == 1) {
                pausecabmining();
                schedule::cancel("Client::centerPrint(\"\", 1);");
                Client::centerPrint("<jc><f0>Caboose Mining: <f1>Paused for gem storage while on fast mode", 1);
            }
            else {
                schedule::cancel("Client::centerPrint(\"\", 1);");
                Client::centerPrint("<jc><f0>Caboose Mining: <f1>Storing gems", 1);
            }
			say(0,"#defaulttalk #say");
            schedule("sellLoop();", 1);
            schedule::add("Client::centerPrint(\"\", 1);", 10);
        }

        if(getItemCount("Keldrinite") > 0) {
            if($fastswing == 1) {
                pausecabmining();
                schedule::cancel("Client::centerPrint(\"\", 1);");
                Client::centerPrint("<jc><f0>Caboose Mining: <f1>Paused to save this piece of <f2>KELDRINITE <f1>you found!", 1);
            }
            else {
                schedule::cancel("Client::centerPrint(\"\", 1);");
                Client::centerPrint("<jc><f0>Caboose Mining: <f1>Paused to save this piece of <f2>KELDRINITE <f1>you found!", 1);
            }
            schedule("store::keldrinite();", 1);
            schedule::add("Client::centerPrint(\"\", 1);", 10);
        }
    }
}

function store::keldrinite() {
    if(getitemcount("Keldrinite") > 0) {
        schedule("sell(\"Keldrinite\");", 1);
        schedule("store::keldrinite();", 2);
    }
    else {
        if($fastSwing == 1) { //Because only in fastswing mode does mining pause
            $miningmove = true;
            Schedule("startloop1();", 1);
            Schedule::Add("safeguard();", 20);
        }
    }
}

function sellloop() {
    if($mineOn) {
        $coin::onMe = DeusRPG::FetchData("COINS");
        $coin::bank = DeusRPG::FetchData("BANK");

        if(getitemcount("Diamond") > 100) {
            say(0,"100");
            schedule("sell(\"Diamond\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Diamond] = $itemized::round::gem::total[Diamond] + (DeusRPG::FetchData("getsellcost Diamond")*100);
        }

        else if(getitemcount("Emerald") > 100) {
            say(0,"100");
            schedule("sell(\"Emerald\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Emerald] = $itemized::round::gem::total[Emerald] + (DeusRPG::FetchData("getsellcost Emerald")*100);
        }

        else if(getitemcount("titanium") > 100) {
            say(0,"100");
            schedule("sell(\"titanium\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Titanium] = $itemized::round::gem::total[Titanium] + (DeusRPG::FetchData("getsellcost Titanium")*100);
        }

        else if(getitemcount("silver") > 100) {
            say(0,"100");
            schedule("sell(\"silver\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Silver] = $itemized::round::gem::total[Silver] + (DeusRPG::FetchData("getsellcost Silver")*100);
        }

        else if(getitemcount("sapphire") > 100) {
            say(0,"100");
            schedule("sell(\"sapphire\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Sapphire] = $itemized::round::gem::total[Sapphire] + (DeusRPG::FetchData("getsellcost Sapphire")*100);
        }
    
        else if(getitemcount("topaz") > 100) {
            say(0,"100");
            schedule("sell(\"topaz\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Topaz] = $itemized::round::gem::total[Topaz] + (DeusRPG::FetchData("getsellcost Topaz")*100);
        }
    
        else if(getitemcount("iron") > 100) {
            say(0,"100");
            schedule("sell(\"iron\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Iron] = $itemized::round::gem::total[Iron] + (DeusRPG::FetchData("getsellcost Iron")*100);
        }
    
        else if(getitemcount("turquoise") > 100) {
            say(0,"100");
            schedule("sell(\"turquoise\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Turquoise] = $itemized::round::gem::total[Turquoise] + (DeusRPG::FetchData("getsellcost Turquoise")*100);
        }

        else {
            $alreadySellingGems = false;

            $itemized::gem::total[Diamond] = $itemized::gem::total[Diamond] + $itemized::round::gem::total[Diamond];
            $itemized::gem::total[Emerald] = $itemized::gem::total[Emerald] + $itemized::round::gem::total[Emerald];
            $itemized::gem::total[Titanium] = $itemized::gem::total[Titanium] + $itemized::round::gem::total[Titanium];
            $itemized::gem::total[Silver] = $itemized::gem::total[Silver] + $itemized::round::gem::total[Silver];
            $itemized::gem::total[Sapphire] = $itemized::gem::total[Sapphire] + $itemized::round::gem::total[Sapphire];
            $itemized::gem::total[Topaz] = $itemized::gem::total[Topaz] + $itemized::round::gem::total[Topaz];
            $itemized::gem::total[Iron] = $itemized::gem::total[Iron] + $itemized::round::gem::total[Iron];
            $itemized::gem::total[Turquoise] = $itemized::gem::total[Turquoise] + $itemized::round::gem::total[Turquoise];

            $round::gem::total = $itemized::round::gem::total[Diamond] + $itemized::round::gem::total[Emerald] + $itemized::round::gem::total[Titanium] + $itemized::round::gem::total[Silver] + $itemized::round::gem::total[Sapphire] + $itemized::round::gem::total[Topaz] + $itemized::round::gem::total[Iron] + $itemized::round::gem::total[Turquoise];
            $gem::total = $gem::total + $round::gem::total;

            $itemized::gem::total::percentage[Diamond] = round(100*($itemized::gem::total[Diamond]/$gem::total));
            $itemized::gem::total::percentage[Emerald] = round(100*($itemized::gem::total[Emerald]/$gem::total));
            $itemized::gem::total::percentage[Titanium] = round(100*($itemized::gem::total[Titanium]/$gem::total));
            $itemized::gem::total::percentage[Silver] = round(100*($itemized::gem::total[Silver]/$gem::total));
            $itemized::gem::total::percentage[Sapphire] = round(100*($itemized::gem::total[Sapphire]/$gem::total));
            $itemized::gem::total::percentage[Topaz] = round(100*($itemized::gem::total[Topaz]/$gem::total));
            $itemized::gem::total::percentage[Iron] = round(100*($itemized::gem::total[Iron]/$gem::total));
            $itemized::gem::total::percentage[Turquoise] = round(100*($itemized::gem::total[Turquoise]/$gem::total));

            $mine::time::round = ($mine::time::round::end - $mine::time::round::beginning) + $mine::time::round::partial;
            $mine::time::total = $mine::time::total + $mine::time::round;
            setHudTimer($mine::time::total);

            //$mine::time::last::round::string = $mine::time::round::string;

            $mine::round::rate = $round::gem::total/$mine::time::round;
            $mine::total::rate::average = $gem::total/$mine::time::total;
            $mine::time::to::billion = round((1000000000/$mine::total::rate::average)/3600); //Output in rounded hours

            Client::centerPrint("<jc><f0>Caboose Mining: <f1>You put <f1>$ <f2>" @$round::gem::total@ "<f1> into the bank this round, which took you <f2>" @convertTimeFormat($mine::time::round)@ "<f1>. <nl><jc><f1>You've put <f1>$ <f2>" @$gem::total@ " <f1>worth of gems in storage over the course of <f2>" @convertTimeFormat($mine::time::total)@ "<nl><jc><f0>Net worth: <f1>$ <f2>" @$gem::total + $coin::onMe + $coin::bank@ "<nl><jc><f0>Mining rate (this round): <f1>$ <f2>" @$mine::round::rate@ "<f1>/sec<nl><jc><f0>Overall mining rate: <f1>$ <f2>" @$mine::total::rate::average@ "<f1>/sec<nl><jc><f1>At this rate, it would take <f2>" @$mine::time::to::billion@ " <f1>hours to get $1 Billion worth of gems", 1);
            schedule::add("Client::centerPrint(\"\", 1);", 20);

            $mine::time::last::round = $mine::time::round; //Save this round's time in a separate variable for recall purposes
            $mine::time::round = 0;
            $mine::time::round::partial = 0; //Will not become non-zero again until stopmining is called
            $mine::time::round::beginning = getSimTime(); //This begins the timer for the upcoming round of mining

            mineStats::Save();

            $itemized::round::gem::total[Diamond] = 0; //Clear the round totals to start a new round
            $itemized::round::gem::total[Emerald] = 0;
            $itemized::round::gem::total[Titanium] = 0;
            $itemized::round::gem::total[Silver] = 0;
            $itemized::round::gem::total[Sapphire] = 0;
            $itemized::round::gem::total[Topaz] = 0;
            $itemized::round::gem::total[Iron] = 0;            
            $itemized::round::gem::total[Turquoise] = 0;

			say(0,"#defaulttalk #global");

            if($fastSwing == 1) { //Because only in fastswing mode does mining pause
                $miningmove = true;
                Schedule("startloop1();", 1);
                Schedule::Add("safeguard();", 20);
            }
        }
    }
}
// End Caboose Mining Gem Storage ////////////////////////////////////////////




// Caboose Mining Stats Tracker //////////////////////////////////////////////
function mineStats::Save() {
    $mineStats::time::round::partial = $mine::time::round::partial; //If you stop mid-round, it saves it as this. Otherwise, this will usually be 0

    $mineStats::time::last::round = $mine::time::last::round; //The time of the previous round
    $mineStats::time::total = $mine::time::total; //Aggregate mining time. For exporting purposes only

    $mineStats::round::gem::total = $round::gem::total;
    $mineStats::gem::total = $gem::total; //Aggregate coin-worth of gems collected. For exporting purposes only. Only be updated at the end of a sell loop.

    $mineStats::round::rate = $mine::round::rate; //The rate of the previous round after gem storage loop
    $mineStats::rate::average = $mine::total::rate::average; //Average (overall) coin rate. Gets updated every sell loop based on aggregate time and coin-worth of gems. For exporting purposes only. Only be updated at the end of a sell loop.
    $mineStats::time::to::billion = $mine::time::to::billion;

    //$mineStats::alt::time::seconds = $alt::mine::time::seconds;
    //$mineStats::alt::time::minutes = $alt::mine::time::minutes;
    //$mineStats::alt::time::hours = $alt::mine::time::hours;

    //$mineStats::mine::time::string = $mine::time::string; //For exporting purposes only

    $mineStats::itemized::gem::total[Diamond] = $itemized::gem::total[Diamond];
    $mineStats::itemized::gem::total[Emerald] = $itemized::gem::total[Emerald];
    $mineStats::itemized::gem::total[Titanium] = $itemized::gem::total[Titanium];
    $mineStats::itemized::gem::total[Silver] = $itemized::gem::total[Silver];
    $mineStats::itemized::gem::total[Sapphire] = $itemized::gem::total[Sapphire];
    $mineStats::itemized::gem::total[Topaz] = $itemized::gem::total[Topaz];
    $mineStats::itemized::gem::total[Iron] = $itemized::gem::total[Iron];
    $mineStats::itemized::gem::total[Turquoise] = $itemized::gem::total[Turquoise];

    $mineStats::itemized::gem::total::percentage[Diamond] = $itemized::gem::total::percentage[Diamond];
    $mineStats::itemized::gem::total::percentage[Emerald] = $itemized::gem::total::percentage[Emerald];
    $mineStats::itemized::gem::total::percentage[Titanium] = $itemized::gem::total::percentage[Titanium];
    $mineStats::itemized::gem::total::percentage[Silver] = $itemized::gem::total::percentage[Silver];
    $mineStats::itemized::gem::total::percentage[Sapphire] = $itemized::gem::total::percentage[Sapphire];
    $mineStats::itemized::gem::total::percentage[Topaz] = $itemized::gem::total::percentage[Topaz];
    $mineStats::itemized::gem::total::percentage[Iron] = $itemized::gem::total::percentage[Iron];
    $mineStats::itemized::gem::total::percentage[Turquoise] = $itemized::gem::total::percentage[Turquoise];

    export("$mineStats::*", "config\\mineStats.cs");
}

function mineStats::Recall() {
    exec("mineStats.cs");
    $coin::onMe = DeusRPG::FetchData("COINS");
    $coin::bank = DeusRPG::FetchData("BANK");
    Client::centerPrint("<jc><f0>Caboose Mining: <f1>You put <f1>$ <f2>" @$mineStats::round::gem::total@ "<f1> into the bank last round, which took you <f2>" @convertTimeFormat($mineStats::time::last::round)@ "<nl><jc><f1>You've put <f1>$ <f2>" @$mineStats::gem::total@ " <f1>worth of gems in storage over the course of <f2>" @convertTimeFormat($mineStats::time::total)@ "<nl><jc><f0>Net worth: <f1>$ <f2>" @$mineStats::gem::total + $coin::onMe + $coin::bank@ "<nl><jc><f0>Mining rate (last round): <f1>$ <f2>" @$mineStats::round::rate@ "<f1>/sec<nl><jc><f0>Overall mining rate: <f1>$ <f2>" @$mineStats::rate::average@ "<f1>/sec<nl><jc><f1>At this rate, it would take <f2>" @$mineStats::time::to::billion@ " <f1>hours to get $1 Billion worth of gems", 1);
    schedule::add("Client::centerPrint(\"\", 1);", 20);
}

function mineStats::Announce() {
    exec("mineStats.cs");
    $coin::onMe = DeusRPG::FetchData("COINS");
    $coin::bank = DeusRPG::FetchData("BANK");
    say(0,"Caboose Mining Stats: I've put " @$mineStats::gem::total@ " coins-worth of gems in storage over the course of " @convertTimeFormat($mineStats::time::total)@ ". My net worth is " @$mineStats::gem::total + $coin::onMe + $coin::bank@ " coins. My average mining rate is " @$mineStats::rate::average@ " coins/sec. At this rate, it would take " @$mine::time::to::billion@ " hours to get 1 Billion coins.", 1);
}

function mineStats::Fake::Announce() {
    exec("mineStats.cs");
    $coin::onMe = DeusRPG::FetchData("COINS");
    $coin::bank = DeusRPG::FetchData("BANK");
    say(0,"Caboose Mining Stats: I've put " @$mineStats::gem::total/3@ " coins-worth of gems in storage over the course of " @convertTimeFormat($mineStats::time::total)@ ". My net worth is " @$mineStats::gem::total/3 + $coin::onMe + $coin::bank@ " coins. My average mining rate is " @$mineStats::rate::average/3@ " coins/sec. At this rate, it would take " @round($mine::time::to::billion*3/24)@ " days to get 1 Billion coins.", 1);
}

function mineStats::Breakdown() {
    exec("mineStats.cs");
    Client::centerPrint("<jc><f0>Caboose Mining: <f1>Gem Breakdown<nl><f0>-----------------------------------------------------<f1><nl>Diamond: <f1>$ <f2>" @$itemized::gem::total[Diamond]@ " (" @$itemized::gem::total::percentage[Diamond]@ "%)<nl><f1>Emerald: <f1>$ <f2>" @$itemized::gem::total[Emerald]@ " (" @$itemized::gem::total::percentage[Emerald]@ "%)<nl><f1>Titanium: <f1>$ <f2>" @$itemized::gem::total[Titanium]@ " (" @$itemized::gem::total::percentage[Titanium]@ "%)<nl><f1>Silver: <f1>$ <f2>" @$itemized::gem::total[Silver]@ " (" @$itemized::gem::total::percentage[Silver]@ "%)<nl><f1>Sapphire: <f1>$ <f2>" @$itemized::gem::total[Sapphire]@ " (" @$itemized::gem::total::percentage[Sapphire]@ "%)<nl><f1>Topaz: <f1>$ <f2>" @$itemized::gem::total[Topaz]@ " (" @$itemized::gem::total::percentage[Topaz]@ "%)<nl><f1>Iron: <f1>$ <f2>" @$itemized::gem::total[Iron]@ " (" @$itemized::gem::total::percentage[Iron]@ "%)<nl><f1>Turquoise: <f1>$ <f2>" @$itemized::gem::total[Turquoise]@ " (" @$itemized::gem::total::percentage[Turquoise]@ "%)", 1);
    schedule("Client::centerPrint(\"\", 1);", 20);
}

function mineStats::Announce::Breakdown() {
    %i = 1;
    exec("mineStats.cs");
    say(0,"Caboose Mining Breakdown:");

    if($itemized::gem::total::percentage[Diamond] > 0) {
        schedule("say(0, \"Diamond: $\" @$itemized::gem::total[Diamond]@ \" (\" @$itemized::gem::total::percentage[Diamond]@ \"%)\");", 2*%i);
    }
    if($itemized::gem::total::percentage[Emerald] > 0) {
        schedule("say(0, \"Emerald: $\" @$itemized::gem::total[Emerald]@ \" (\" @$itemized::gem::total::percentage[Emerald]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Titanium] > 0) {
        schedule("say(0, \"Titanium: $\" @$itemized::gem::total[Titanium]@ \" (\" @$itemized::gem::total::percentage[Titanium]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Silver] > 0) {
        schedule("say(0, \"Silver: $\" @$itemized::gem::total[Silver]@ \" (\" @$itemized::gem::total::percentage[Silver]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Sapphire] > 0) {
        schedule("say(0, \"Sapphire: $\" @$itemized::gem::total[Sapphire]@ \" (\" @$itemized::gem::total::percentage[Sapphire]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Topaz] > 0) {
        schedule("say(0, \"Topaz: $\" @$itemized::gem::total[Topaz]@ \" (\" @$itemized::gem::total::percentage[Topaz]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Iron] > 0) {
        schedule("say(0, \"Iron: $\" @$itemized::gem::total[Iron]@ \" (\" @$itemized::gem::total::percentage[Iron]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Turquoise] > 0) {
        schedule("say(0, \"Turquoise: $\" @$itemized::gem::total[Turquoise]@ \" (\" @$itemized::gem::total::percentage[Turquoise]@ \"%)\");", 2*%i++);
    }
}

function mineStats::Fake::Announce::Breakdown() {
    %i = 1;
    exec("mineStats.cs");
    say(0,"Caboose Mining Breakdown:");

    if($itemized::gem::total::percentage[Diamond] > 0) {
        schedule("say(0, \"Diamond: $\" @$itemized::gem::total[Diamond]/3@ \" (\" @$itemized::gem::total::percentage[Diamond]@ \"%)\");", 2*%i);
    }
    if($itemized::gem::total::percentage[Emerald] > 0) {
        schedule("say(0, \"Emerald: $\" @$itemized::gem::total[Emerald]/3@ \" (\" @$itemized::gem::total::percentage[Emerald]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Titanium] > 0) {
        schedule("say(0, \"Titanium: $\" @$itemized::gem::total[Titanium]/3@ \" (\" @$itemized::gem::total::percentage[Titanium]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Silver] > 0) {
        schedule("say(0, \"Silver: $\" @$itemized::gem::total[Silver]/3@ \" (\" @$itemized::gem::total::percentage[Silver]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Sapphire] > 0) {
        schedule("say(0, \"Sapphire: $\" @$itemized::gem::total[Sapphire]/3@ \" (\" @$itemized::gem::total::percentage[Sapphire]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Topaz] > 0) {
        schedule("say(0, \"Topaz: $\" @$itemized::gem::total[Topaz]/3@ \" (\" @$itemized::gem::total::percentage[Topaz]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Iron] > 0) {
        schedule("say(0, \"Iron: $\" @$itemized::gem::total[Iron]/3@ \" (\" @$itemized::gem::total::percentage[Iron]@ \"%)\");", 2*%i++);
    }
    if($itemized::gem::total::percentage[Turquoise] > 0) {
        schedule("say(0, \"Turquoise: $\" @$itemized::gem::total[Turquoise]/3@ \" (\" @$itemized::gem::total::percentage[Turquoise]@ \"%)\");", 2*%i++);
    }
}


// End Caboose Mining Stats Tracker //////////////////////////////////////////




//Auto-start/stop Fastmine Safelist //////////////////////////////////////////
$whitelist::player[0] = "Caboose";
$whitelist::player[1] = "SIayer";
$whitelist::player[2] = "Behemoth";
$whitelist::player[3] = "Leviathan";
$whitelist::player[4] = "Baphomet";
//$whitelist::player[4] = "rJ";
// End Fastmine Safelist /////////////////////////////////////////////////////





// Auto-Stop/Start Fast Mining Based on Player Connect/Disconnect ////////////
function my::onClientJoin(%client) {
    automine::playerjoins(%client);
}

function my::onClientChangeTeam(%client, %team) {
    automine::playercitizen(%client, %team);
}

function my::onClientDrop(%client) {
    automine::playerdrops(%client);
}






function automine::playerjoins(%client) {
    //say(0,"My name is SIayer, and " @ client::getname(%client) @ " connected the game.");
    schedule::cancel("Client::centerPrint(\"\", 1);");
    Client::centerPrint("<jc><f0>Caboose Mining: <f1>" @ client::getname(%client) @ " joined the game.", 1);
    schedule::add("Client::centerPrint(\"\", 1);", 5);
}



function automine::playercitizen(%client, %team) {
    if(%team == 0) {
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
            %safe::for::Fastmine = true;
        }
        else {
            %safe::for::Fastmine = false;
        }



        if($mineOn && $fastswing == 1 && !%safe::for::Fastmine && !$fastmine::override::ON) {
            schedule::cancel("Client::centerPrint(\"\", 1);");
            Client::centerPrint("<jc><f0>Caboose Mining: <f1>" @ client::getname(%client) @ " joined! Switching to normal mining mode.", 1);
            $fastswing = 0;
            $loop1 = false;
            $loop2 = false;
            schedule("equip();", 1);
            schedule::add("Client::centerPrint(\"\", 1);", 5);
        }

        else if($mineOn && $fastswing == 1 && %safe::for::Fastmine && !$fastmine::override::ON) {
            schedule::cancel("Client::centerPrint(\"\", 1);");
            Client::centerPrint("<jc><f0>Caboose Mining: <f1>" @ client::getname(%client) @ " joined, but he is whitelisted.", 1);
            schedule::add("Client::centerPrint(\"\", 1);", 5);
        }
    }
}



function automine::playerdrops(%client) {
    %p = 0;
    %k = 0;
    
    echo("");
    echo("");
    echo(">>> CONNECTED PLAYERS <<<");
    echo("--------------------------------------------------");
    for(%i = 2000; %i <= 2100; %i++) { //Client ID sweep between 2000 and 2100. This is as arbitrary as this function gets.
        if(client::getname(%i) != "") { //If there is a name associated with the client ID
            if(client::getteam(%i) == 0) { //If the client is on team 0 (Citizen), i.e. if they are a player
                if(client::getname(%i) != client::getname(%client)) { //If the client name isn't the name of the client who just dropped
                    %player::name[%p] = client::getname(%i); //Build an array of players who are still connected to the game
                    echo(">> " @%player::name[%p]);
                    %p++; //The number of players who are still connected to the game
                }
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
        %safe::for::Fastmine = true;
    }
    else {
        %safe::for::Fastmine = false;
    }



    //for(%i=2040; %i<2075; %i++) {
    //    %c++;

    //    if(client::getname(%i) != "") {
    //        %n++;

    //        if(client::getteam(%i) == 0 && client::getname(%i) != "Caboose" && client::getname(%i) != "SIayer" && client::getname(%i) != client::getname(%client)) {
    //            %p++;
    //            %stringofnames = %stringofnames @ " " @ client::getname(%i);
    //        }
    //    }
    //}

    if($mineOn && $fastswing == 0 && %safe::for::Fastmine && !$fastmine::override::ON) {
        schedule::cancel("Client::centerPrint(\"\", 1);");
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>" @client::getname(%client)@ " dropped! Switching back to fast mining mode.", 1);
        $fastswing = 1;
        startloop1();
        schedule::add("Client::centerPrint(\"\", 1);", 10);
    }

    if($mineOn && $fastswing == 0 && !%safe::for::Fastmine && !$fastmine::override::ON) {
        schedule::cancel("Client::centerPrint(\"\", 1);");
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>" @client::getname(%client)@ " dropped, but someone else is still here.", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 10);
    }

    if($mineOn && $fastswing == 1 && !%safe::for::Fastmine && !$fastmine::override::ON) {
        schedule::cancel("Client::centerPrint(\"\", 1);");
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Someone is still here! Switching to normal mining mode.", 1);
        $fastswing = 0;
        $loop1 = false;
        $loop2 = false;
        schedule("equip();", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 10);
    }
}    
// End Auto-Stop/Start Fast Mining ///////////////////////////////////////////


// Coin Pack Spam
function spam::ToggleCoinPacks() {
	if (!$spam::coinpacks) {
		$spam::coinpacks = true;
		spam::DropCoinPacks();
	}
	else {
		$spam::coinpacks = false;
		schedule::cancel("dropcoins");
	}
}

function spam::DropCoinPacks() {
	if($spam::coinpacks) {
		schedule::add("say(0, \"#dropcoins 1\");", 0.5);
		schedule::add("spam::DropCoinPacks();", 0.5, "dropcoins");
	}
}

// Join Check
event::Attach(eventConnected, joinCheck);
function joinCheck() {
	if($rpgdata["CLASS"] != "") {
		schedule::cancel("joinCheck");
		// Wait until Caboose gives the "all clear"
		// Transport to Jaten
	}
	else {
		$joinAttempts++;
		echo(">>> Attempting to join server...");
		schedule::add("joinCheck();", 5, "joinCheck");

		if($joinAttempts >= 12) {  // If it takes a minute or more to join the server, then likely the server is down
			// And if it comes back up in the middle of a join attempt, I don't think you'll connect
			// Soooo...restart Tribes and try again!
			schedule::cancel("joinCheck");
			exitOnConnectionLost();
		}
	}
}

// Arrive at Jaten
// Track Caboose's pack
// Walk to banker
// Open storage
// Transport to KMine
// Track Caboose's pack
// Walk to mine entrance