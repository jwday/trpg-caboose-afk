// Caboose Mining v1.03
// An alternative to Deus and Xin_ mining for Tribes RPG
// rev. March 22nd, 2014

// Requires Presto for Match::string();, Event::attach();, and Schedule();
// Requires DeusRPG Pack for DeusRPG::FetchData() and proper mining menu integration
// Requires Cowboy HUD (or equivalent) to retain permanent access to storage
// Also requires convertTimeFormat.cs to properly format the timer output

// Features include:
// -- Auto-store Thorr's Hammer if someone you don't like joins the game
// -- Auto-retrieve Thorr's Hammer if the person/people you don't like leaves/leave the game
// -- Auto-store gems
// -- Record time spent mining
// -- Record value of gems put in storage
// -- Calculate and display total gem value mined
// -- Calculate and display your net worth (based on gems stored, coins on you, and coins in bank)
// -- Calculate and display your mining rate between gem storage sessions
// -- Calculate and display your overall average mining rate
// -- Menu modifications to DeusChatbind.cs to see and/or announce your mining stats and current auto-store option
// -- Ability to toggle auto-store on/off from the menu

// To install:
// Place CabooseMining103.cs and convertTimeFormat.cs into your tribes/config folder
// Add the line "exec("CabooseMining103.cs");" to your autoexec.cs file

// Credit where credit is due:
// -- A lot of functions, logic, and syntax were borrowed and expanded upon from Taurik's Script Pack
// -- rJ and DeathAdder were largely responsible for helping me understand the way Tribes code functions (especially when it came to arrays)
// -- Particle, for letting me use his server to test my scripts and provide a platform upon which to self-educate
///////////////////////////////////////////////////////////////////////////////////




// Update Log /////////////////////////////////////////////////////////////////////
//
// v1.03 (March 22nd, 2014) - Added extra functions to keep track of stats/announce stats. Fixed some small annoyances with auto-retreive Thorr's Hammer. Changed the way time is kept. Changed menu layout.
//
// v1.02 (March 22nd, 2014) - v1.01 still didn't work due to the fact that Caboose is a moron and doesn't know how to code. Switched order of events in Mining Menu to Modify DeusChatbind.cs
//
// v1.01 (March 21st, 2014) - Delayed executing of mining menu updater to avoid conflicts with DeusChatbind.cs
//
// v1.00 (March 13th, 2014) - First public release
//
// End Update Log /////////////////////////////////////////////////////////////////




// Caboose Mining Options /////////////////////////////////////////////////////////
//
// This option will set the default status of the autostore routine every time you join the game. It can be toggled on/off in-game through the mining menu.
// As you probably guessed, "true" = enabled; "false" = disabled.
// This option is tied to player join/drop notifications. The script will only notify you if a player joins or drops if it is set to "true"

$autostore::thorrhammer = true; //Default option. Can be toggled in-game.

// Players You Want Caboose Mining to Protect Your Thorr's Hammer From ////////////
$blacklist::player[0] = "Astal";
$blacklist::player[1] = "Shorty";
$blacklist::player[2] = "Focus";
$blacklist::player[3] = "NFY";
$blacklist::player[4] = "Sako";
$blacklist::player[5] = "";
// End Blacklisted Players ////////////////////////////////////////////////////////

// End Caboose Mining Options /////////////////////////////////////////////////////




///////////////////////////////////////////////////////////////////////////////////
// BEGIN CABOOSE MINING ///////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////

// ................................................ ..........................._,-~"¯¯"~-,
// .................................................. ................__„-~"¯¯:::,-~~-,_::::"-
// .................................................. ..........„~"¯::::::::::::::"::::::::::::::::::::::\
// .................................................. .__„„„-"::::::::::::::::::::::::::::::::::::::::::::::"~-,
// ..........................................__-~"::,-':::::::::::::::::::::::::::::::::::::::::::::::::::::::: ::::~-,
// ..........................._______~"___-~"::::::::::::::::::::::::::::::::::::::::::::: ::: :: :::::::::::"-,
// ......................,~"::::::::::::::¯¯::::::::: ::::::::::::::::::::::::: :::::::::::::::::::::::::::::::::::::::::,: |
// ....................:/:::::::::::::::::__-~":::::::::::::::::::::::::::::::::::::::::::::::::_,-~":'\'-,:\:|:\|::\|\::\:|
// ...................,'::::::::,-~~"~"_::',::|::::::::::::::::::::::::::::::::::: :: :::,~ ':\'-,::',"-\::'':"::::::::\|:|/
// ..............._,-'"~----":::/,~"¯"-:|::|::|:::::::::::::::::::::::::::::::::::,~"::\'-,:\;;'-';;;;;;;;;;;,-'::\::|/
// ............,-'::::::::::::::::'-\~"O¯_/::,'::|:::::::::::::::::::::::::::::::::,-',::\'-,:|::";;;;;;;;;;;;,-':\:'-,::\
// ............|:::::::::::::::::-,_'~'::::,-'::,':::::::::::::::::::::::::::::,-':\'-,:\'-,';;';;;;;;;;;;;;;,-':\:::'\-,|''
// ............|::,-~"::::::::::::::"~~":::,-'::::::::::::::::::::::::_,-~':\'-,|:"'";;;;;;;;;;;;;;,-'¯::'-,:',\|
// .........../::/::::::::::::::::::::::::::::::::::::::::::::_,„-~"¯\:\'-,|;''-';;;;;;;;;;;;;;;;;;,-'--,::\-:\:\|
// ........./::::|:::::::::::::::::::::::::::::::::::::::::,-';;'-';;;;',/;\/;;;;;;;;;;;;;;;;;;;;,-,|:::\-,:|\|..\|
// ......./:::::::\:::::::::::::::::::::::::::::::::::::,-';;;;;;;;;;;;;;;;;;;;;;;;;;;,-~'''("-,\:::|\:|::''
// ......,':::::::,'::::::::::::::::::::::::::::::::: :,-'/;;;;;;;;;;;;;;;;;;;;;;;;;,--'::::::/"~'
// .....,'::::::::|:::::::::::::::::::::::::::::,„-~"::|;;;;;;;;;;;;;;;;;;;;;,-'::::::::,'::::/
// ..../:::::::::|:::::::::::::„---~~""¯¯¯::',:::::,';;;;;;;;;;;;;;;;;;;,'::::::::: :: |_,-'
// ..,'::::::::::::",:,-~"¯::::::::"-,::::::::::|:::/;;;;;;;;;;;;;;;;;;;,':::::::|::::,'
// ./:::::::::::::::|:::::::::::::::::::"-,:::::::\:::|¯¯¯"""~-,~,_/::::::::,':::/
// ::::::::::::::::::::::::::::::::::::::"~-,_::|::\: : : : : : |: : \::::::::/:/ mine ALL the gems!
// ::::::::::::::::::::::::::::::",:::::::::::::"-':::\: : : : : : |: : :\::::::\
// ::::::::::::::::::::::::::::::::",:::::::::::::: ::::\: : : : : : \: : : |:::::;;\
// ::::::::::::::::::"-,:::::::::::::::",:::::::::::::::/|\ ,: : : : : : : |::::,'/|::::|
// :::::::::::::::::::::"-,:::::::::::::::"-,_::::::::::\|:/|,: : : : : : : |::: |'-,/|:::|
// ::::::::::::::::::::::::"~-,_::::::::::::::"~-,_:::"-,/|/\::::::::::: \::: \"-/|::|
// :::::::::::::::::::::::::::::::"~-,__:::::::::::',"-,:::"_|/\:|\: : : : \::\":/|\|
// ::::::::::::::::::::::::::::::::::::::::"~-,_:::::\:::\:::"~/_:|:|\: : : '-,\::"::,'\
// :::::::::::::::::::::::::::::::::::::::::::::::"-,_:'-,::\:::::::"-,|:||\,-, : '-,\:::|-'-„
// :::::::::::::::::::::::::::::::::::::::::::::::::: ::,-,'"-:"~,:::::"/_/::|-/\--';;\:::/: ||\-,
// :::::::::::::::::::::::::::::::::::::::::::::::::: :/...'-,::::::"~„::::"-,/_:|:/\:/|/|/|_/:|
// :::::::::::::::::::::::::::::::::::::::::::::::::: |......"-,::::::::"~-:::::""~~~"¯:::|
// :::::::::::::::::::::::::::::::::::::::::::::::::: |........."-,_::::::::::::::::::::::::::::/
// :::::::::::::::::::::::::::::::::::::::::::::::::\ .............."~--„_____„„-~~"

///////////////////////////////////////////////////////////////////////////////////

include("convertTimeFormat.cs");
include("presto\\Match.cs");
include("presto\\Event.cs");
include("presto\\Schedule.cs");

// Toggle Autostore Hammer On/Off /////////////////////////////////////////////////
function autostore::toggle() {
    if($autostore::thorrhammer == true) {
        $autostore::thorrhammer = false;
        %autostore::status = "OFF";
        cabmining::menu::update(%autostore::status);
    }

    else if($autostore::thorrhammer == false) {
        $autostore::thorrhammer = true;
        %autostore::status = "ON";
        cabmining::menu::update(%autostore::status);
    }

    Client::centerPrint("<jc><f0>Caboose Mining: <f1>Auto-store <f2>Thorr's Hammer <f1>is <f2>" @%autostore::status, 1);
    schedule("Client::centerPrint(\"\", 1);", 5);
}
// End Toggle Autostore ///////////////////////////////////////////////////////////




// Modify DeusChatbind.cs upon starting Tribes ////////////////////////////////////
//
// v1.02 Update: Caboose is a moron and doesn't know howto properly code stuff

function cabmining::menu::update(%autostore::status) {
    Menu::New(MenuA, "Auto Mining");
	    Menu::AddChoice(MenuA, "tXin_ Auto Mining: Quit = TRUE", "Mining::Start(TRUE);");
	    Menu::AddChoice(MenuA, "fXin_ Auto Mining: Quit = FALSE", "Mining::Start(FALSE);");
	    Menu::AddChoice(MenuA, "^----------------------------------", "blank();");
	    Menu::AddChoice(MenuA, "1Deus Auto Mining: Quit = TRUE", "Mining::Start(TRUE, old);");
	    Menu::AddChoice(MenuA, "2Deus Auto Mining: Quit = FALSE", "Mining::Start(FALSE, old);");
	    Menu::AddChoice(MenuA, "zStop Auto Mining", "Mining::Stop();");
        Menu::AddChoice(MenuA, "-----------------------------------", "blank();");
        Menu::AddChoice(MenuA, "cCaboose Mining Menu", "Menu::Display(MenuCabMine);");

    Menu::New(MenuCabMine, "Caboose Mining Menu");
        Menu::AddChoice(MenuCabMine, "cCaboose Mining, Normal (C)", "startcabmining();");
	    Menu::AddChoice(MenuCabMine, "xStop Caboose Mining (X)", "stopcabmining();");
        Menu::AddChoice(MenuCabMine, "+----------------------------------", "blank();");
        Menu::AddChoice(MenuCabMine, "sCaboose Mining Stats (S)", "mineStats::Recall();");
        Menu::AddChoice(MenuCabMine, "aAnnounce Mining Stats (A)", "mineStats::Announce();");
        Menu::AddChoice(MenuCabMine, "- - - - - - - - - - - - - -", "blank();");
        Menu::AddChoice(MenuCabMine, "bCaboose Mining Breakdown (B)", "mineStats::Breakdown();");
        Menu::AddChoice(MenuCabMine, "nAnnounce Mining Breakdown (N)", "mineStats::Announce::Breakdown();");
        Menu::AddChoice(MenuCabMine, "*----------------------------------", "blank();");
        Menu::AddChoice(MenuCabMine, "lShow blacklisted Players (L)", "show::blacklist();");
}

if($autostore::thorrhammer == true) {
    %autostore::status = "ON";
    cabmining::menu::update(%autostore::status);
}

else if($autostore::thorrhammer == false) {
    %autostore::status = "OFF";
    cabmining::menu::update(%autostore::status);
}
// End Modify DeusChatbind.cs /////////////////////////////////////////////////////




// This will overide the default onClientJoin/ChangeTeam/Drop script. That means if you have anything else tied to it, it will not work. Sorry, but event::clientJoin() sucks hairy donkey dick, and this was the only workaround I could come up with.
function onClientJoin(%client) {
    caboose::onClientJoin(%client);
}

function onClientChangeTeam(%client, %team) {
    caboose::onClientChangeTeam(%client, %team);
}

function onClientDrop(%client) {
    caboose::onClientDrop(%client);
}
// End onClientJoin/ChangeTeam/Drop override ////////////////////////////////////




// Caboose Mining Controls //////////////////////////////////////////////////////
function startcabmining() {
    //$RPG::PlayerName = Client::GetName(GetManagerID()); //Taken from Cody/Corona/Tokath/Taurik/Coddman. All credit goes to him, and may the wind be at his back wherever he may be
    $RPG::StatsName = "mineStats-" @ Client::GetName(GetManagerID()) @ ".cs"; //In case you have multiple characters mining. Also taken from Cody.
    exec($RPG::StatsName); //Also taken from Cody.

    $mine::time::round::partial = $mineStats::time::round::partial; //Every "round" is the period of time it takes to get 500 Diamonds. It starts over after the sell loop finishes
    $mine::time::total = $mineStats::time::total;
    $mine::time::round::beginning = getSimTime();

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

    if($autostore::thorrhammer) {
        $autostore::thorrhammer::string = "Enabled";
    }
    else {
        $autostore::thorrhammer::string = "Disabled";
    }

    if(getItemCount("Thorr's Hammer") > 0) {
        use("Thorr's Hammer");
        $mining::tool = "thorrhammer";
    }
    else if(getItemCount("Pick Axe") > 0) {
        use("Pick Axe");
        $mining::tool = "Pick";
    }
    else if(getItemCount("Hammer Pick") > 0) {
        use("Hammer Pick");
        $mining::tool = "Pick"; //Use same timing scheme for Pick Axe and Hammer Pick even though Hammer Pick is a little slower. Might cause some mucking up, but who in their right mind uses the Hammer Pick to mine anyway?
    }
    else {
        schedule::cancel("Client::centerPrint(\"\", 1);");
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>No mining tool, dumbass!", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 20);
        break;
    }

    startloop();
    setHudTimer($mineStats::time::total);
    schedule::cancel(cleartag);
    Client::centerPrint("<jc><f0>Caboose Mining: <f1>Started<nl><f0>Total mining time: <f2>" @convertTimeFormat($mineStats::time::total)@ "<nl><f0>Total gem value: <f1>$ <f2>" @$mineStats::gem::total@ "<nl><f0>Overall mining rate: <f1>$ <f2>" @$mineStats::rate::average@ "<f1>/second.", 1);
    schedule::add("Client::centerPrint(\"\", 1);", 20, cleartag);
    Schedule::Add("safeguard();", 15);
}

function pausecabmining() {
    if($mineOn) {
        mineStats::Save();
	    $miningMove = false;
        Schedule::cancel("mineWalk();");
        Schedule::cancel("safeguard();");
        postAction(2048, IDACTION_BREAK1, 1);
        $DeusRPG::toggleFire = true;
    }
}

function stopcabmining() {
    if($mineOn) {
        $mine::time::round::end = getSimTime(); //If and when you stop Caboose Mining without going through a storage loop
        $mine::time::round::partial = $mine::time::round::end - $mine::time::round::beginning;
        $mine::time::total = $mine::time::total + $mine::time::round::partial;
        mineStats::Save();

	    $miningMove = false;
        $mineOn = false;
        postAction(2048, IDACTION_BREAK1, 1);
        $DeusRPG::toggleFire = true;
        Schedule::cancel("mineWalk();");
        Schedule::cancel("safeguard();");
        schedule("postAction(2048, IDACTION_BREAK1, 1);", 1);
        schedule::cancel(cleartag);
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Stopped<nl><f0>Total mining time: <f2>" @convertTimeFormat($mineStats::time::total)@ "<nl><f0>Total gem value: <f2>" @$mineStats::gem::total@ " <f1>coins<nl><f0>Overall mining rate: <f1>$ <f2>" @$mineStats::rate::average@ "<f1>/second.", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 20, cleartag);
    }
}
// End Cabose Mining Controls //////////////////////////////////////////////////




// Initialize Caboose Mining Loop /////////////////////////////////////////////
function startloop() {
    if($mineON) {
	    $miningMove = true;
        $foundgems = false;
        schedule("postAction(2048, IDACTION_FIRE1, 1);", 0);
        Schedule::Add("mineWalk();", 3);
    }
}
// End Initialize Caboose Mining Loop /////////////////////////////////////////




// Caboose Mining Loop ///////////////////////////////////////////////////////
event::Attach(eventClientMessage, mineLoop);

function mineLoop(%client, %msg) {
    if($mineOn && $miningMove) {
    	$foundgems = Match::String(%msg, "You found *");

	    if($foundgems) {
            if($alreadySellingGems == false) { //This is to make sure the gem counter doesn't accidentally double up the count
                schedule("gemstore();", 0.5); //Check if you have 500 Diamonds or if you have Keldrinite every time to strike the spike. Delay to make sure all gem counts are up to date before initiating gemstore();
            }
            $safeguardRecover = false;
	    	Schedule::Cancel("mineWalk();");
	    	Schedule::Cancel("safeguard();");
            if($mining::tool == "thorrhammer") {
                Schedule::Add("mineWalk();", 4);
            }
            else {
                Schedule::Add("mineWalk();", 2);
            }
            Schedule("$foundgems = false;", 2);
            Schedule::Add("safeguard();", 30);
        }

        if($safeguardRecover) {
            $safeguardRecover = false;
            Schedule::Add("mineWalk();", 4);
            Schedule::Add("safeguard();", 30);
        }
    }
}
// End Caboose Mining Loop ////////////////////////////////////////////////////




// Caboose Mining Movement Control ///////////////////////////////////////////
function mineWalk() {
    if($mineOn && $miningMove) {
	    if($position == 1) {
	  	    Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.3);
            if($mining::tool == "thorrhammer") {
                Schedule::Add("mineWalk();", 4);
            }
            else {
                Schedule::Add("mineWalk();", 2);
            }
		    $position = 4;
            return;
	    }

        if($position == 4) {
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 0.4);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.4);
            if($mining::tool == "thorrhammer") {
                Schedule::Add("mineWalk();", 4);
            }
            else {
                Schedule::Add("mineWalk();", 2);
            }
		    $position = 3;
            return;
	    }

        if($position == 3) {
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 0.4);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.4);
            if($mining::tool == "thorrhammer") {
                Schedule::Add("mineWalk();", 4);
            }
            else {
                Schedule::Add("mineWalk();", 2);
            }
		    $position = 2;
            return;
	    }

        if($position == 2) {
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 0);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 0);
		    Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 0.4);
            Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 0.4);
            if($mining::tool == "thorrhammer") {
                Schedule::Add("mineWalk();", 4);
            }
            else {
                Schedule::Add("mineWalk();", 2);
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
        schedule::cancel(cleartag);
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Initiating safeguard routine", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 5, cleartag);
        pausecabmining();

        Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 1);
        Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 3);

        Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.00000);", 4);
        Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 4.5);

        Schedule("startloop();", 5);
        Schedule::Add("safeguard();", 25);

        $position = 1;
        $safeguardRecover = true;
    }
}
// End Caboose Mining Safeguard //////////////////////////////////////////////




// Caboose Mining Gem Storage ////////////////////////////////////////////////
function gemstore() { //Based only on Diamond or Keldrinite count
    if($mineOn && $miningMove) {
        if(getItemCount("Keldrinite") > 0) {
            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Caboose Mining: <f1>Lets save this piece of <f2>KELDRINITE <f1>you found!", 1);
            schedule("sellLoop();", 1);
            schedule::add("Client::centerPrint(\"\", 1);", 10, cleartag);
        }

        else if(getItemCount("Diamond") > 500) {
            $alreadySellingGems = true;
            $round::gem::total = 0;
            $mine::time::round::end = getSimTime(); //This ends the timer for the previous round of mining

            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Caboose Mining: <f1>Storing gems", 1);
            schedule("sellLoop();", 1);
        }
    }
}

function sellloop() {
    if($mineOn) {
        $coin::onMe = DeusRPG::FetchData("COINS");
        $coin::bank = DeusRPG::FetchData("BANK");

        if(getitemcount("Keldrinite") > 0) {
            say(1,"1");
            schedule("sell(\"Keldrinite\");", 1);
            schedule("sellloop();", 2);
        }

        if(getitemcount("Diamond") > 100) {
            say(1,"100");
            schedule("sell(\"Diamond\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Diamond] = $itemized::round::gem::total[Diamond] + (DeusRPG::FetchData("getsellcost Diamond")*100);
        }

        else if(getitemcount("Emerald") > 100) {
            say(1,"100");
            schedule("sell(\"Emerald\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Emerald] = $itemized::round::gem::total[Emerald] + (DeusRPG::FetchData("getsellcost Emerald")*100);
        }

        else if(getitemcount("titanium") > 100) {
            say(1,"100");
            schedule("sell(\"titanium\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Titanium] = $itemized::round::gem::total[Titanium] + (DeusRPG::FetchData("getsellcost Titanium")*100);
        }

        else if(getitemcount("silver") > 100) {
            say(1,"100");
            schedule("sell(\"silver\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Silver] = $itemized::round::gem::total[Silver] + (DeusRPG::FetchData("getsellcost Silver")*100);
        }

        else if(getitemcount("sapphire") > 100) {
            say(1,"100");
            schedule("sell(\"sapphire\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Sapphire] = $itemized::round::gem::total[Sapphire] + (DeusRPG::FetchData("getsellcost Sapphire")*100);
        }
    
        else if(getitemcount("topaz") > 100) {
            say(1,"100");
            schedule("sell(\"topaz\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Topaz] = $itemized::round::gem::total[Topaz] + (DeusRPG::FetchData("getsellcost Topaz")*100);
        }
    
        else if(getitemcount("iron") > 100) {
            say(1,"100");
            schedule("sell(\"iron\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Iron] = $itemized::round::gem::total[Iron] + (DeusRPG::FetchData("getsellcost Iron")*100);
        }
    
        else if(getitemcount("turquoise") > 100) {
            say(1,"100");
            schedule("sell(\"turquoise\");", 1);
            schedule("sellloop();", 2);
            $itemized::round::gem::total[Turquoise] = $itemized::round::gem::total[Turquoise] + (DeusRPG::FetchData("getsellcost Turquoise")*100);
        }

        else {
            $alreadySellingGems = false;

            //Mined gems itemized by coin value
            $itemized::gem::total[Diamond] = $itemized::gem::total[Diamond] + $itemized::round::gem::total[Diamond];
            $itemized::gem::total[Emerald] = $itemized::gem::total[Emerald] + $itemized::round::gem::total[Emerald];
            $itemized::gem::total[Titanium] = $itemized::gem::total[Titanium] + $itemized::round::gem::total[Titanium];
            $itemized::gem::total[Silver] = $itemized::gem::total[Silver] + $itemized::round::gem::total[Silver];
            $itemized::gem::total[Sapphire] = $itemized::gem::total[Sapphire] + $itemized::round::gem::total[Sapphire];
            $itemized::gem::total[Topaz] = $itemized::gem::total[Topaz] + $itemized::round::gem::total[Topaz];
            $itemized::gem::total[Iron] = $itemized::gem::total[Iron] + $itemized::round::gem::total[Iron];
            $itemized::gem::total[Turquoise] = $itemized::gem::total[Turquoise] + $itemized::round::gem::total[Turquoise];

            //Calculate total for this round and overall
            $round::gem::total = $itemized::round::gem::total[Diamond] + $itemized::round::gem::total[Emerald] + $itemized::round::gem::total[Titanium] + $itemized::round::gem::total[Silver] + $itemized::round::gem::total[Sapphire] + $itemized::round::gem::total[Topaz] + $itemized::round::gem::total[Iron] + $itemized::round::gem::total[Turquoise];
            $gem::total = $gem::total + $round::gem::total;

            //Mined gems itemized by percentage of total value
            $itemized::gem::total::percentage[Diamond] = round(100*($itemized::gem::total[Diamond]/$gem::total));
            $itemized::gem::total::percentage[Emerald] = round(100*($itemized::gem::total[Emerald]/$gem::total));
            $itemized::gem::total::percentage[Titanium] = round(100*($itemized::gem::total[Titanium]/$gem::total));
            $itemized::gem::total::percentage[Silver] = round(100*($itemized::gem::total[Silver]/$gem::total));
            $itemized::gem::total::percentage[Sapphire] = round(100*($itemized::gem::total[Sapphire]/$gem::total));
            $itemized::gem::total::percentage[Topaz] = round(100*($itemized::gem::total[Topaz]/$gem::total));
            $itemized::gem::total::percentage[Iron] = round(100*($itemized::gem::total[Iron]/$gem::total));
            $itemized::gem::total::percentage[Turquoise] = round(100*($itemized::gem::total[Turquoise]/$gem::total));

            //Timekeeper
            $mine::time::round = ($mine::time::round::end - $mine::time::round::beginning) + $mine::time::round::partial;
            $mine::time::total = $mine::time::total + $mine::time::round;
            setHudTimer($mine::time::total);

            //Calculate round rate, total rate (average), and estimated time to get $1 Billion coins
            $mine::round::rate = $round::gem::total/$mine::time::round;
            $mine::total::rate::average = $gem::total/$mine::time::total;
            $mine::time::for::billion = round((1000000000/$mine::total::rate::average)/3600); //Output in rounded hours

            //Calculate your net worth and determine your next goal (in orders of Billions)
            $coin::net::worth = $gem::total + $coin::onMe + $coin::bank;
            $net::worth::goal::int = floor($coin::net::worth/1000000000)+1; //Goal is always the next Billion coin

            //Calculate time remaining to reach your next Billion
            $mine::time::left::to::next::billion = round((($net::worth::goal::int*1000000000 - $coin::net::worth)/$mine::total::rate::average)/3600); //Output in rounded hours

            //Show me the money!
            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Caboose Mining: <f1>You put <f1>$ <f2>" @$round::gem::total@ "<f1> into the bank this round, which took you <f2>" @convertTimeFormat($mine::time::round)@ "<f1>. <nl><jc><f1>You've put <f1>$ <f2>" @$gem::total@ " <f1>worth of gems in storage over the course of <f2>" @convertTimeFormat($mine::time::total)@ "<nl><jc><f0>Net worth: <f1>$ <f2>" @$coin::net::worth@ "<nl><jc><f0>Mining rate (this round): <f1>$ <f2>" @$mine::round::rate@ "<f1>/sec<nl><jc><f0>Overall mining rate: <f1>$ <f2>" @$mine::total::rate::average@ "<f1>/sec<nl><jc><f1>At this rate, it would take <f2>" @$mine::time::for::billion@ " <f1>hours to get $1 Billion worth of gems<nl>At the rate you're going, your net worth will be $" @$net::worth::goal::int@ " Billion in <f2>" @$mine::time::left::to::next::billion@ " <f1>hours", 1);
            schedule::add("Client::centerPrint(\"\", 1);", 20, cleartag);

            //Set up variables to be saved to MineStats.cs and reset the counter for the next round
            $mine::time::last::round = $mine::time::round; //Save this round's time in a separate variable for recall purposes
            $mine::time::round = 0;
            $mine::time::round::partial = 0; //Will not become non-zero again until stopmining is called
            $mine::time::round::beginning = getSimTime(); //This begins the timer for the upcoming round of mining

            //Save ALL the stats!
            mineStats::Save();

            //Reset the itemized counters for the next round
            $itemized::round::gem::total[Diamond] = 0; //Clear the round totals to start a new round
            $itemized::round::gem::total[Emerald] = 0;
            $itemized::round::gem::total[Titanium] = 0;
            $itemized::round::gem::total[Silver] = 0;
            $itemized::round::gem::total[Sapphire] = 0;
            $itemized::round::gem::total[Topaz] = 0;
            $itemized::round::gem::total[Iron] = 0;            
            $itemized::round::gem::total[Turquoise] = 0;
        }
    }
}
// End Caboose Mining Gem Storage ////////////////////////////////////////////




// Caboose Mining Stats Tracker //////////////////////////////////////////////
function mineStats::Save() {
    $RPG::PlayerName = Client::GetName(GetManagerID()); //Borrowed from Sneeky/Taurik/Tokath/Coddman/Corona/Cody's afkmodule script. All credit goes to him for this tidbit

    $mineStats::time::round::partial = $mine::time::round::partial; //If you stop mid-round, it saves it as this. Otherwise, this will usually be 0

    $mineStats::time::last::round = $mine::time::last::round; //The time of the previous round
    $mineStats::time::total = $mine::time::total; //Aggregate mining time. For exporting purposes only

    $mineStats::round::gem::total = $round::gem::total;
    $mineStats::gem::total = $gem::total; //Aggregate coin-worth of gems collected. For exporting purposes only. Only be updated at the end of a sell loop.
    $mineStats::coin::net::worth = $mineStats::gem::total + $coin::onMe + $coin::bank;

    $mineStats::round::rate = $mine::round::rate; //The rate of the previous round after gem storage loop
    $mineStats::rate::average = $mine::total::rate::average; //Average (overall) coin rate. Gets updated every sell loop based on aggregate time and coin-worth of gems. For exporting purposes only. Only be updated at the end of a sell loop.
    $mineStats::time::for::billion = $mine::time::for::billion;
    $mineStats::time::left::to::next::billion = $mine::time::left::to::next::billion;

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

    export("$mineStats::*", "config\\mineStats-" @Client::GetName(GetManagerID())@ ".cs");
}

function mineStats::Recall() {
    $RPG::PlayerName = Client::GetName(GetManagerID()); //Also taken from Cody
    $RPG::StatsName = "mineStats-" @ Client::GetName(GetManagerID()) @ ".cs"; //In case you have multiple characters mining. Also taken from Cody.
    exec($RPG::StatsName); //Also taken from Cody.

    $coin::onMe = DeusRPG::FetchData("COINS");
    $coin::bank = DeusRPG::FetchData("BANK");
    schedule::cancel(cleartag);
    Client::centerPrint("<jc><f0>Caboose Mining: <f1>You put <f1>$ <f2>" @$mineStats::round::gem::total@ "<f1> into the bank last round, which took you <f2>" @convertTimeFormat($mineStats::time::last::round)@ "<nl><jc><f1>You've put <f1>$ <f2>" @$mineStats::gem::total@ " <f1>worth of gems in storage over the course of <f2>" @convertTimeFormat($mineStats::time::total)@ "<nl><jc><f0>Net worth: <f1>$ <f2>" @$mineStats::coin::net::worth@ "<nl><jc><f0>Mining rate (last round): <f1>$ <f2>" @$mineStats::round::rate@ "<f1>/sec<nl><jc><f0>Overall mining rate: <f1>$ <f2>" @$mineStats::rate::average@ "<f1>/sec<nl><jc><f1>At this rate, it would take <f2>" @$mineStats::time::for::billion@ " <f1>hours to get $1 Billion worth of gems<nl>At the rate you're going, your net worth will be $" @$net::worth::goal::int@ " Billion in <f2>" @$mineStats::time::left::to::next::billion@ " <f1>hours", 1);
    schedule::add("Client::centerPrint(\"\", 1);", 20, cleartag);
}

function mineStats::Announce() {
    $RPG::PlayerName = Client::GetName(GetManagerID()); //Also taken from Cody
    $RPG::StatsName = "mineStats-" @ Client::GetName(GetManagerID()) @ ".cs"; //In case you have multiple characters mining. Also taken from Cody.
    exec($RPG::StatsName); //Also taken from Cody.

    $coin::onMe = DeusRPG::FetchData("COINS");
    $coin::bank = DeusRPG::FetchData("BANK");
    say(0,"Caboose Mining Stats: I've put " @$mineStats::gem::total@ " coins-worth of gems in storage over the course of " @convertTimeFormat($mineStats::time::total)@ ". My net worth is " @$mineStats::coin::net::worth@ " coins. My average mining rate is " @$mineStats::rate::average@ " coins/sec. At this rate, it would take " @$mine::time::for::billion@ " hours to get 1 Billion coins.", 1);
}

function mineStats::Breakdown() {
    $RPG::PlayerName = Client::GetName(GetManagerID()); //Also taken from Cody
    $RPG::StatsName = "mineStats-" @ Client::GetName(GetManagerID()) @ ".cs"; //In case you have multiple characters mining. Also taken from Cody.
    exec($RPG::StatsName); //Also taken from Cody.

    schedule::cancel(cleartag);
    Client::centerPrint("<jc><f0>Caboose Mining: <f1>Gem Breakdown<nl><f0>-----------------------------------------------------<f1><nl>Diamond: <f1>$ <f2>" @$itemized::gem::total[Diamond]@ " (" @$itemized::gem::total::percentage[Diamond]@ "%)<nl><f1>Emerald: <f1>$ <f2>" @$itemized::gem::total[Emerald]@ " (" @$itemized::gem::total::percentage[Emerald]@ "%)<nl><f1>Titanium: <f1>$ <f2>" @$itemized::gem::total[Titanium]@ " (" @$itemized::gem::total::percentage[Titanium]@ "%)<nl><f1>Silver: <f1>$ <f2>" @$itemized::gem::total[Silver]@ " (" @$itemized::gem::total::percentage[Silver]@ "%)<nl><f1>Sapphire: <f1>$ <f2>" @$itemized::gem::total[Sapphire]@ " (" @$itemized::gem::total::percentage[Sapphire]@ "%)<nl><f1>Topaz: <f1>$ <f2>" @$itemized::gem::total[Topaz]@ " (" @$itemized::gem::total::percentage[Topaz]@ "%)<nl><f1>Iron: <f1>$ <f2>" @$itemized::gem::total[Iron]@ " (" @$itemized::gem::total::percentage[Iron]@ "%)<nl><f1>Turquoise: <f1>$ <f2>" @$itemized::gem::total[Turquoise]@ " (" @$itemized::gem::total::percentage[Turquoise]@ "%)", 1);
    schedule::add("Client::centerPrint(\"\", 1);", 20, cleartag);
}

function mineStats::Announce::Breakdown() {
    %i = 1;

    $RPG::PlayerName = Client::GetName(GetManagerID()); //Also taken from Cody
    $RPG::StatsName = "mineStats-" @ Client::GetName(GetManagerID()) @ ".cs"; //In case you have multiple characters mining. Also taken from Cody.
    exec($RPG::StatsName); //Also taken from Cody.

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
// End Caboose Mining Stats Tracker //////////////////////////////////////////




// Display Blacklist /////////////////////////////////////////////////////////
function show::blacklist() {
    $blacklist::string = "";

    for(%i=0; $blacklist::player[%i] != ""; %i++) {
        %blacklist::player::string = "<nl>" @ $blacklist::player[%i];
        $blacklist::string =  $blacklist::string @ %blacklist::player::string;
    }

    schedule::cancel(cleartag);
    Client::centerPrint("<jc><f0>Blacklisted Players:<f2>" @$blacklist::string, 1);
    schedule::add("client::centerPrint(\"\", 1);", 10, cleartag);
}
// End Display Blacklst //////////////////////////////////////////////////////




// Auto-Store/Retrieve Thorr's Hammer Based on Player Connect/Disconnect /////
function caboose::onClientJoin(%client) {
    cabmine::playerjoins(%client);
}

function caboose::onClientChangeTeam(%client, %team) {
    cabmine::playercitizen(%client, %team);
}

function caboose::onClientDrop(%client) {
    cabmine::playerdrops(%client);
}




function cabmine::playerjoins(%client) {
    if($autostore::thorrhammer) {
        Client::centerPrint("<jc><f0>Caboose Mining: <f2>" @ client::getname(%client) @ " <f1>connected to the game. Will check against the blacklist when they join Citizen.", 1);
        schedule::add("Client::centerPrint(\"\", 1);", 10);
    }
}



function cabmine::playercitizen(%client, %team) {
    if(%team == 0 && $mineOn && $autostore::thorrhammer) {
        for(%i=0; $blacklist::player[%i] != ""; %i++) {
	        if(client::getname(%client) == $blacklist::player[%i] && getItemCount("Thorr's Hammer") > 0) {
                Client::centerPrint("<jc><f0>Caboose Mining: <f2>" @ client::getname(%client) @ " <f1>joined! Storing <f2>Thorr's Hammer <f1>and switching to <f2>Pick Axe.", 1);
                postAction(2048, IDACTION_BREAK1, 1);
                sell("Thorr's Hammer");
                schedule("cabmine::playercitizen(%client, %team);", 1); //Just in case it didn't put Thorr's Hammer away the first time around
                if(getItemCount("Pick Axe") == 0) {
                    schedule("buy(\"Pick Axe\");", 2);
                }
                schedule("use(\"Pick Axe\");", 3);
                schedule("postAction(2048, IDACTION_FIRE1, 1);", 4);
                schedule("Client::centerPrint(\"\", 1);", 10);
            }
	    }
    }
}




function cabmine::playerdrops(%client) { //Check the server for anyone else that is blacklisted.
    if($autostore::thorrhammer) {
        %p = 0;
        %k = 0;
    
        for(%i = 2000; %i <= 2100; %i++) { //Client ID sweep between 2000 and 2100. This is as arbitrary as this function gets.
            if(client::getname(%i) != "") { //If there is a name associated with the client ID
                if(client::getteam(%i) == 0) { //If the client is on team 0 (Citizen), i.e. if they are a player
                    %player::name[%p] = client::getname(%i); //Build an array of players who are still connected to the game
                    %p++;
                }
            }
        }

     //&& %player::name[%i] != client::getname(%client)
        for(%i = 0; %i <= %p-1; %i++) { //Cycle through all the players still connected to the game
            for(%j = 0; $blacklist::player[%j] != ""; %j++) { //Cycle through all the blacklisted players
                if(%player::name[%i] == $blacklist::player[%j]) { //If there's a match, even if it's the one who just dropped
                    %blacklisted::player::name[%k] = %player::name[%i]; //Build an array of blacklisted players who are still connected to the game
                    %k++; //NOTE: This will == 1 when the last blacklisted player leaves the server; ergo, there will be no more blacklisted players connected
                }
            }
        }

        if(%k == 1) {
            %last::player = true; //This will only occur when the person who dropped was the last one on your blacklist, i.e. no more blacklisted players are connected. I had to differentiate this so that it won't auto-buy thorrhammer if just anyone drops. It MUST be a person who was on your blacklist.
        }

        if(%k > 0) {
            %k = %k - 1; //To stay consistent with the number of players connected. This MUST come AFTER the if(%k == 1) check
        }
    
        if($mineOn && %k == 1 && getItemCount("Thorr's Hammer") == 0) { //If there is one blacklisted person still connected...
            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Auto AFK: <f2>" @%blacklisted::player::name[%k-1]@ " <f1>is still connected to the server, and he is blacklisted", 1);
            schedule("Client::centerPrint(\"\", 1);", 10, cleartag);
        }

        else if($mineOn && %k == 2 && getItemCount("Thorr's Hammer") == 0) { //If there are two blacklisted people still connected (separated for grammatical reasons)...
            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Auto AFK: <f2>" @%blacklisted::player::name[%k-2]@ " <f1>and <f2>" @%blacklisted::player::name[%k-1]@ " <f1>are still connected to the server, and they are blacklisted", 1);
            schedule("Client::centerPrint(\"\", 1);", 10, cleartag);
        }

        else if($mineOn && %k > 2 && getItemCount("Thorr's Hammer") == 0) { //If there are more than two blacklisted people still connected...
            %blacklisted::player::name::string = %blacklisted::player::name[0];

            for(%i = 1; %i < %k-1; %i++) {
                %blacklisted::player::name::string = %blacklisted::player::name::string@ "<f1>, <f2>" @%blacklisted::player::name[%i];
            }

            %blacklisted::player::name::string = %blacklisted::player::name::string@ ", <f1>and <f2>" @%blacklisted::player::name[%k-1];

            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Auto AFK: <f2>" @%blacklisted::player::name::string@ " <f1>are still connected to the server, and they are blacklisted", 1);
            schedule("Client::centerPrint(\"\", 1);", 10, cleartag);
        }

        else if($mineOn && %k == 0 && %last::player && getItemCount("Thorr's Hammer") == 0) { //If the last blacklisted player quit and you don't have Thorr's Hammer on you...
            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Auto AFK: <f2>" @client::getname(%client)@ " <f1>dropped!<nl>There are no more blacklisted players are on the server. Retrieving <f2>Thorr's Hammer.", 1);
            postAction(2048, IDACTION_BREAK1, 1);
            buy("Thorr's Hammer");
            schedule("use(\"Thorr's Hammer\");", 2);
            schedule("postAction(2048, IDACTION_FIRE1, 1);", 3);
            schedule("Client::centerPrint(\"\", 1);", 10, cleartag);
        }

        else if($mineOn && %k == 0 && %last::player && getItemCount("Thorr's Hammer") > 0) { //If the last blacklisted player quit and you DO have Thorr's Hammer on you...
            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Auto AFK: <f2>" @client::getname(%client)@ " <f1>dropped!<nl>OMG. You had Thorr's Hammer on you while " @client::getname(%client)@ "was connected! What were you thinking?" , 1);
            schedule("Client::centerPrint(\"\", 1);", 10, cleartag);
        }
        else if($mineOn) { //If anyone quits while you are mining and there were no blacklisted players connected to begin with...
            schedule::cancel(cleartag);
            Client::centerPrint("<jc><f0>Auto AFK: <f2>" @client::getname(%client)@ " <f1>dropped" , 1);
            schedule("Client::centerPrint(\"\", 1);", 10, cleartag);
        }
    }
}
// End Auto-Store/Retrieve Thorr's Hammer ////////////////////////////////////
