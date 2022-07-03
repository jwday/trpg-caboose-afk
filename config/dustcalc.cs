event::Attach(eventClientMessage, DustCalcMsg);

function DustCalcMsg(%client, %msg) {
	%pubdustcalc = Match::String(%msg, "* \"dust from * to *\"");
	if(%pubdustcalc) {
		%firstlvl = Match::Result(1);
		%secondlvl = Match::Result(2);
		Schedule("showdustreq("@%firstlvl@","@%secondlvl@");", 1.0);
		return;
	}

	%publvlcalccoins = Match::String(%msg, "* \"dust from * with * coins\"");
	if(%publvlcalccoins) {
		%money = Match::Result(2);
		%firstlvl = Match::Result(1);
		Schedule("showlvlsgainedcoins("@%money@","@%firstlvl@");", 1.0);
		return;
	}	

	%publvlcalcparch = Match::String(%msg, "* \"dust from * with * parchments\"");
	if(%publvlcalcparch) {
		%parch = Match::Result(2);
		%firstlvl = Match::Result(1);
		Schedule("showlvlsgainedparch("@%parch@","@%firstlvl@");", 1.0);
		return;
	}		
}

function showdustreq(%firstlvl,%secondlvl) {
	%startlvl = floor(%firstlvl);
	%endlvl = floor(%secondlvl);
	if(%startlvl < 0) %startlvl = 0;
	if(%endlvl < 0) %endlvl = 0;

	%startexp = (%startlvl - 1 ) * (200 + 50 * ((%startlvl - 1) - 1));
	%endexp = (%endlvl - 1) * (200 + 50 * ((%endlvl -1) - 1));
	%expdiff = %endexp - %startexp;

	%DustNeed = round(%expdiff / 4625);
	
	say(0, "Dusting from level " @ %startlvl @ " to level " @ %endLvl @ " requires " @ %DustNeed @ " parchments. That equals " @ %expdiff @ " EXP, so you better step up your game, son!");
}


function showlvlsgainedcoins(%money,%firstlvl) {
	if(%money < 1000000) {
		say(0, "Thats not enough money for even one parchment you asshat.");
	}
	
	if(%money > 1000000) {
		%mils = %money / 1000000;
		%DustGet = floor(%mils);

		%firstlvl = floor(%firstlvl);
		if(%firstlvl < 0) %firstlvl = 0;

		%startexp = (%firstlvl - 1 ) * (200 + 50 * ((%firstlvl - 1) - 1));
		%endexp = %startexp + (4625 * %DustGet);
		%endlvl = 1 + (0.5 * (sqrt(9 + 2*(%endexp / 25)) - 3));
	
		%endlvl = floor(%endlvl);

	
		say(0, %money @ " coins will bring you from level " @ %firstlvl @ " to level " @ %endlvl @ ".");

	}
}


function showlvlsgainedparch(%parch,%firstlvl) {
		%firstlvl = floor(%firstlvl);
		if(%firstlvl < 0) %firstlvl = 0;

		%startexp = (%firstlvl - 1 ) * (200 + 50 * ((%firstlvl - 1) - 1));
		%endexp = %startexp + (4625 * %parch);
		%endlvl = 1 + (0.5 * (sqrt(9 + 2*(%endexp / 25)) - 3));
	
		%endlvl = floor(%endlvl);

	
		say(0, %parch @ " parchments will bring you from level " @ %firstlvl @ " to level " @ %endlvl @ ".");

}





//Other various questions

event::Attach(eventClientMessage, HaggleCalcMsg);

function HaggleCalcMsg(%client, %msg) {
	%hagglecalc = Match::String(%msg, "* \"how much will a * sell for at max haggle\"");
    if(%hagglecalc) {
        %itemName = Match::Result(1);
        say(0, "You will get " @DeusRPG::FetchData("getsellcost " @%itemName)@ " coins per " @%itemName@ " at max haggle.");
    }
}