include("presto\\Match.cs");
include("presto\\Event.cs");
	
	// echo("");
	// echo("");
	// echo(">> Type start::buyitem(ItemNameHere); to begin auto-buying from storage.");
	// echo("");
	// echo(">> Type stop::buyitem(); to stop (otherwise, will auto-stop when complete).");
	// echo("");
	// echo("");
	// echo("");
	// echo(">> Type start::sellitem(ItemNameHere); to begin auto-selling to merchant.");
	// echo("");
	// echo(">> Type stop::sellitem(); to stop (otherwise, will auto-stop when complete).");
	// echo("");
	// echo("");
	// echo("");
	// echo("");
    // echo("");
	// echo("");
	// echo(">> Type start::buyparchment(); to begin auto-buying Parchment.");
	// echo("");
	// echo(">> Type stop::buyparchment(); to stop (this is necessary; function will not auto-stop).");
	// echo("");
	// echo("");
	// echo("");
	// echo(">> Type start::buydust(); to begin auto-buying Magic Dust.");
	// echo("");
	// echo(">> Type stop::buydust(); to stop (this is necessary; function will not auto-stop).");
	// echo("");
	// echo("");



//All stop
function stopallbuysell() {
    $autosellallgems::on = false;
    $autobuyallgems::on = false;
    $buyspam = false;
    stop::sellitem();
    stop::buyitem();
    $autoLoopOn = false;
}




//Auto sell gems loop

$maxcarry[Diamond] = 3000;  						// Sells at $2652
$maxcarry[Emerald] = $maxcarry[Diamond]*1.2;  		// Sells at $1552
$maxcarry[Titanium] = $maxcarry[Emerald]*1.2;		// Sells at $1331 
$maxcarry[Silver] = $maxcarry[Titanium]*1.2;		// Sells at $664
$maxcarry[Sapphire] = $maxcarry[Silver]*1.2;		// Sells at $469
$maxcarry[Topaz] = $maxcarry[Sapphire]*1.2;			// Sells at $257
$maxcarry[Iron] = $maxcarry[Topaz]*1.2;				// Sells at $232
$maxcarry[Turquoise] = $maxcarry[Iron]*1.2;			// Sells at $136


function startautobuysellloop() { //Be at banker (facing merchant) to open storage
    $gem::sell::position = 0;
    schedule("autobuyallgems();", 1);
    $autoLoopOn = true;

    $ranOutOf::[Diamond] = false;
    $ranOutOf::[Emerald] = false;
    $ranOutOf::[Titanium] = false;
    $ranOutOf::[Silver] = false;
    $ranOutOf::[Sapphire] = false;
    $ranOutOf::[Topaz] = false;
    $ranOutOf::[Iron] = false;
    $ranOutOf::[Turquoise] = false;
}

function autobuyallgems() {
    $autobuyallgems::on = true;

    if($gem::sell::position == 0 && $autoLoopOn) {
        $gem::sell::position = 1;
        schedule("say(0,\"#say hi\");", 0);
        schedule("say(0,\"#say storage\");", 2);
        schedule("autobuyallgems::loop();", 4);
        Schedule("postAction(2048, IDACTION_MOVEFORWARD, 1.000000);", 5);
        Schedule("postAction(2048, IDACTION_MOVEFORWARD, 0.000000);", 8);
    }
}

function autobuyallgems::loop() {
    if($autoLoopOn) {
        $itemCountBeginning = 0;
        $itemCountBeginning = -1; //So that they do not equal each other when it comes time to loop autobuy
        echo("");
        echo(">>> $ranoutof::[Diamond] = " @$ranoutof::[Diamond]);
        echo(">>> $ranoutof::[Emerald] = " @$ranoutof::[Emerald]);
        echo(">>> $ranoutof::[Sapphire] = " @$ranoutof::[Sapphire]);
        echo(">>> $ranoutof::[Topaz] = " @$ranoutof::[Topaz]);
        echo(">>> $ranoutof::[Turquoise] = " @$ranoutof::[Turquoise]);
        echo("");
    
        if(getItemCount("Diamond") < $maxcarry[Diamond] && !$ranOutOf::[Diamond]) {
            start::buyitem("Diamond");
        }
        else if(getItemCount("Emerald") < $maxcarry[Emerald] && !$ranOutOf::[Emerald]) {
            start::buyitem("Emerald");
        }
        //else if(getItemCount("Titanium") < $maxcarry[Titanium] && !$ranOutOf::[Titanium]) {
        //    start::buyitem(Titanium);
        //}
        //else if(getItemCount("Silver") < $maxcarry[Silver] && !$ranOutOf::[Silver]) {
        //    start::buyitem(Silver);
        //}
        else if(getItemCount("Sapphire") < $maxcarry[Sapphire] && !$ranOutOf::[Sapphire]) {
            start::buyitem("Sapphire");
        }
        else if(getItemCount("Topaz") < $maxcarry[Topaz] && !$ranOutOf::[Topaz]) {
            start::buyitem("Topaz");
        }
        //else if(getItemCount("Iron") < $maxcarry[Iron] && !$ranOutOf::[Iron]) {
        //    start::buyitem(Iron);
        //}
        else if(getItemCount("Turquoise") < $maxcarry[Turquoise] && !$ranOutOf::[Turquoise]) {
            start::buyitem(Turquoise);
        }
        else if($ranOutOf::[Diamond] && $ranOutOf::[Emerald] && $ranOutOf::[Sapphire] && $ranOutOf::[Topaz] && $ranOutOf::[Turquoise]) {
            stopallbuysell();
        }
        else {
            $autobuyallgems::on = false;
            schedule("autosellallgems();", 1);
        }
    }
}




function autosellallgems() {
    $autosellallgems::on = true;

    if($gem::sell::position == 1 && $autoLoopOn) {
        $gem::sell::position = 0;
        schedule("say(0,\"#say hi\");", 2);
        schedule("say(0,\"#say buy\");", 4);
        schedule("autosellallgems::loop();", 6);
        Schedule("postAction(2048, IDACTION_MOVEBACK, 1.000000);", 7);
        Schedule("postAction(2048, IDACTION_MOVEBACK, 0.000000);", 10.5);
    }
}


function autosellallgems::loop() {
    if($autoLoopOn) {
        echo("");
        echo(">>> $ranoutof::[Diamond] = " @$ranoutof::[Diamond]);
        echo(">>> $ranoutof::[Emerald] = " @$ranoutof::[Emerald]);
        echo(">>> $ranoutof::[Sapphire] = " @$ranoutof::[Sapphire]);
        echo(">>> $ranoutof::[Topaz] = " @$ranoutof::[Topaz]);
        echo(">>> $ranoutof::[Turquoise] = " @$ranoutof::[Turquoise]);
        echo("");

        if(getItemCount("Diamond") > 0) {
            start::sellitem(Diamond);
        }
        else if(getItemCount("Emerald") > 0) {
            start::sellitem(Emerald);
        }
        else if(getItemCount("Sapphire") > 0) {
            start::sellitem(Sapphire);
        }
        else if(getItemCount("Topaz") > 0) {
            start::sellitem(Topaz);
        }
        else if(getItemCount("Turquoise") > 0) {
            start::sellitem(Turquoise);
        }
        //else if($ranOutOf::[Diamond] && $ranOutOf::[Emerald] && $ranOutOf::[Sapphire] && $ranOutOf::[Topaz] && $ranOutOf::[Turquoise]) {
        //    stopallbuysell();
        //}
        else {
            $autosellallgems::on = false;
            schedule("say(0,\"#say hi\");", 5);
            schedule("say(0,\"#say deposit\");", 7);
            schedule("say(0,\"#say all\");", 9);
            schedule("autobuyallgems();", 11);
        }
    }
}




//Auto-buy **OR** auto-take from storage
function start::buyitem($buyitem) {
	$buyspam = true;
	loop::buyitem($buyitem);
}	


function loop::buyitem($buyitem) {
    if($buyspam == true && getItemCount("Magic Dust") < 400 && getItemCount("Parchment") < 400) { //Buy stuff as long as there isn't more than 500 Parchments or Magic Dust 
        if(getItemCount("Parchment") >= 300 && getItemCount("Parchment") < 400 && $taking::parch::from::storage == true) { //If there are between 400 and 500 parchments in you storage, do this once then break
            $current::parchment = getItemCount("Parchment");
            $buy::parch::amount = 500 - $current::parchment;
            $taking::parch::from::storage = false;
            stop::buyitem();
	        schedule("say(1,$buy::parch::amount);", 0);
		    schedule("buy($buyitem);", 1);
            schedule("start::buydust();", 2);
            break;
        }

        else if(getItemCount("Magic Dust") >= 300 && getItemCount("Magic Dust") < 400 && $taking::dust::from::storage == true) { //If there are between 400 and 500 magic dusts in you storage, do this once then break
            $current::dust = getItemCount("Magic Dust");
            $buy::dust::amount = 400 - $current::dust;
            $taking::dust::from::storage = false;
            stop::buyitem();
	        schedule("say(1,$buy::dust::amount);", 0);
		    schedule("buy($buyitem);", 1);
            schedule("start::selldust();", 2);
            break;
        }

        else if(getItemCount("Magic Dust") >= 400 || getItemCount("Parchment") >= 400 && $taking::parch::from::storage == true) { //Double-check to make sure there isn't more than 400 of either dust or parchments. If there is, break
            $taking::parch::from::storage = false;
            stop::buyitem();
            schedule("say(1,\"Balls! Why am I trying to buy more magic items if I already have 400 (or more) on me? Someone slap me!\");", 0);
            schedule("start::buydust();", 2);
            break;
        }

        else if(getItemCount("Magic Dust") >= 400 || getItemCount("Parchment") >= 400 && $taking::dust::from::storage == true) { //Double-check to make sure there isn't more than 400 of either dust or parchments. If there is, break
            $taking::dust::from::storage = false;
            stop::buyitem();
            schedule("say(1,\"Balls! Why am I trying to buy more magic items if I already have 400 (or more) on me? Someone slap me!\");", 0);
            schedule("start::selldust();", 2);
            break;
        }

        else if($autobuyallgems::on && getItemCount($buyitem) >= $maxcarry[$buyitem]) {
            stop::buyitem();
            schedule("autosellallgems();", 1);
            echo("****** STOPPING AUTOBUYGEMS DUE TO REACHING MAX ALLOWED CARRY ******");
            break;
        }

        else {
            $itemCountBeginning = getItemCount($buyItem);
		    schedule("say(1, \"100\");", 0);
            schedule("buy($buyitem);", 0.7);
		    schedule("say(1, \"100\");", 0.9);
            schedule("buy($buyitem);", 1.6);
		    schedule("say(1, \"100\");", 1.8);
            schedule("buy($buyitem);", 2.5);
            // schedule("$itemCountEnd = getItemCount($buyItem);", 0.6); //Just in case
            schedule("check::item::count($buyitem);", 2.7);
         }
	}
}       

function check::item::count() {
    $itemCountEnd = getItemCount($buyItem);

    if($itemCountBeginning != $itemCountEnd) {
        loop::buyitem($buyitem);
    }
    else {
        echo("");
        echo(">>> No more " @$buyitem@ "s in storage");
        echo("");
        stop::buyitem();
        $ranOutOf::[$buyitem] = true;
        schedule("autosellallgems();", 1);
    }
}


event::Attach(eventClientMessage, buylessthan100);

function buylessthan100(%client, %msg) {
    if(Match::String(%msg, "You only have * of this item.") && $buyspam && !$autobuyallgems::on) {
        %buyamount = Match::Result(0);
        stop::buyitem();
        say(1,%buyamount);
	    schedule("buy($buyitem);", 1);
    }

    else if(Match::String(%msg, "You only have * of this item.") && $buyspam && $autobuyallgems::on) {
        %buyamount = Match::Result(0);
        stop::buyitem();
        say(1,%buyamount);
	    schedule("buy($buyitem);", 1);
        schedule("autosellallgems();", 2);
        $ranOutOf::[$buyitem] = true;
        echo("****** STOPPING AUTOBUYGEMS DUE TO HAVING RUN OUT IN STORAGE ******");
        break;
    }
}


function stop::buyitem() {
	$buyspam = false;
}





//Auto-sell **OR** auto-put in storage
function start::sellitem($sellitem) {
	$sellspam = true;
	loop::sellitem();
}

function loop::sellitem($sellitem) {
	if(getItemCount($sellitem) > 0 && $sellspam == true) {
        %sellamount = getItemCount($sellitem);
        say(1,%sellamount);
		schedule("sell($sellitem);", 0.7);
        schedule("loop::sellitem();", 0.9);
    }

    if(getItemCount($sellitem) == 0 && $sellspam == true) {
        stop::sellitem();
    }
}

function stop::sellitem() {
	$sellspam = false;

    if($autosellallgems::on) {
        schedule("autosellallgems::loop();", 1);
    }
}




//Auto-buy Parchment
function start::buyparchment() {
	$autobuyparchment = true;
    Client::centerPrint("<jc><f0>Autodust: <f1>Auto-buying <f2>Parchment", 1);
	loop::buyparchment();
}	


function loop::buyparchment() {
	if(getItemCount("Parchment") < 400 && $autobuyparchment) {
	    say(0, "#say hi");
	    schedule("say(0, \"#say yes\");", 1);
	    schedule("loop::buyparchment();", 2);
    }

    if(getItemCount("Parchment") >= 400  && $autobuyparchment == true) {
        $buyparchment = false();
        start::sellitem("Parchment");
        Client::centerPrint("<jc><f0>Autodust: <f1>Paused for <f2>Parchment <f1>storage", 1);
        schedule("start::buyparchment();", 12);
    }

    if(DeusRPG::FetchData("COINS") < 1000000) {
        stop::buyparchment();
        client::centerPrint("<jc><f0>Autodust: <f1>Auto-stopped buying <f2>Parchment <f1>due to a lack of funds", 1);
        start::sellitem("Parchment");
        schedule("client::centerPrint(\"\", 1);", 10);
    }
}


function stop::buyparchment() {
    Client::centerPrint("", 1);
	$autobuyparchment = false;
}




//Auto-buy Magic Dust
function start::buydust() {
	$autobuydust = true;
    Client::centerPrint("<jc><f0>Autodust: <f1>Auto-buying <f2>Magic Dust <f1>from <f2>Parchments <f1>in inventory and storage", 1);
	loop::buydust();
}	


function loop::buydust() {
    if(getItemCount("Parchment") > 0 && $autobuydust == true) { //If you have any Parchment, start buying Magic Dust first
	    say(0, "#say hi");
	    schedule("say(0, \"#say yes\");", 1);
        schedule("loop::buydust();", 2);
    }

    else if(getItemCount("Magic Dust") == 0 && getItemCount("Parchment") == 0 && $autobuydust == true) { //If you have no Parchment and no Dust, start taking Parchment from storage
        $autobuydust = false;
        $taking::parch::from::storage = true;
        start::buyitem("Parchment"); //loop::buyitem() will stop this after 500 parchments have been taken from storage
        Client::centerPrint("<jc><f0>Autodust: <f1>Paused to get <f2>Parchments <f1>from storage", 1);
    }

    else if(getItemCount("Magic Dust") > 0 && getItemCount("Magic Dust") < 400 && getItemCount("Parchment") == 0 && $autobuydust == true) { //If you have no Parchment but some dust, put dust in storage then take out Parchment to sell
        $autobuydust = false;
        start::sellitem("Magic Dust");
        Client::centerPrint("<jc><f0>Autodust: <f1>Paused for <f2>Magic Dust <f1>storage", 1);
        schedule("start::buydust();", 10);
    }

    //I commented this out because I think this is already taken care of in loop::buyitem() above
    //if(getItemCount("Parchment") >= 500 && getItemCount("Magic Dust") < 500 && $autobuydust == false && $dustloop == true) { //Stops getting Parchment from storage after 500 and starts selling it for Magic Dust (As long as you have less than 500 Magic Dust)
    //    stop::buyitem();
    //    $autobuydust = true;
    //    schedule("start::buydust();", 1);
    //}       

    if(getItemCount("Magic Dust") >= 400 && $autobuydust == true) { //If at any time while dustloop is working your Magic Dust count exceeds 400, it'll stop the buy process and store it all
        $autobuydust = false;
        start::sellitem("Magic Dust");
        Client::centerPrint("<jc><f0>Autodust: <f1>Paused for <f2>Magic Dust <f1>storage", 1);
        schedule("start::buydust();", 10);
    }
}


function stop::buydust() {
    Client::centerPrint("", 1);
	$autobuydust = false;
}




//Auto-sell Magic Dust
function start::selldust() {
    $autoselldust = true;
    Client::centerPrint("<jc><f0>Autodust: <f1>Auto-selling <f2>Magic Dust <f1>for EXP", 1);
	loop::selldust();
}

function loop::selldust() {
    if(getItemCount("Magic Dust") > 0 && $autoselldust == true) { //If you have any Magic Dust, sell it first
	    schedule("say(1, \"#say hi\");", 0);
	    schedule("say(1, \"#say yes\");", 1);
        schedule("loop::selldust();", 2);
    }

    if(getItemCount("Magic Dust") == 0 && $autoselldust == true) { //If you have nothing, get dust from storage
        $autoselldust = false;
        $taking::dust::from::storage = true;
        start::buyitem("Magic Dust");
        Client::centerPrint("<jc><f0>Autodust: <f1>Paused to get <f2>Magic Dust <f1>from storage", 1);
    }
}

function stop::selldust() {
    $autoselldust = false;
    schedule::cancel("selldust();",0);
    Client::centerPrint("", 1);
}