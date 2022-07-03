//If you can't get my stuff working.. try this autoexec.cs
//Just copy it to your TRIBES\config\
//And over-write/back-up your old one.
echo(">> Loading Config Autoexec.cs");
$console::logmode = 0;	//Set this to 1 to log all your msg that go in thru your console..(This can slow you down bad when it gets big... not good to have one with a slow machine)
editActionMap("playmap.sae");
exec("presto\\Install.cs");
Include("cowboy\\CmdHUD.cs");
Include("Autoload.cs");

exec("DeusRPGPack\\DeusInstall.cs");
Include("DeusRPGPack\\BattleHUD\\Install.cs");
Include("DeusRPGPack\\DeusAdminExporter.cs");

if(!$error || $DeusInstallLoaded) {
	echo(">> Loaded Autoexec.cs No errors found.");
}
else {
	echo(">> Loaded Autoexec.cs But with errors.");
}


exec("HocusFocus.cs");
exec("WinampFunctions.cs");
exec("dustcalc.cs");
exec("autoinvite.cs");
exec("autoparty.cs");
exec("autotags.cs");
// exec("collect.cs");
// exec("autostore.cs");
// exec("crashAlarm.cs");
// exec("bashAlarm.cs");
exec("autostore.cs");

if($PCFG::Name == "Caboose") { //If Caboose is selected
    exec("CabooseAFK\\cabooseAFK_main.cs"); //Run ONLY for Caboose
}

if($PCFG::Name == "SIayer") { //If SIayer is selected
    exec("SiayerMining.cs"); //Run ONLY for SIayer
}

else {
	// Do nothing
}

exec("autoPickChar.cs");



// For AutoKLS (autoremort.cs) and AutoFastswing (autoequip.cs)
function onClientJoin(%client) {
    my::onClientJoin(%client);
}

function onClientChangeTeam(%client, %team) {
    my::onClientChangeTeam(%client, %team);
}

function onClientDrop(%client) {
    my::onClientDrop(%client);
}


bindCommand(mouse0, zaxis0, TO, "nextWeapon();"); //Wheel forward	May or may not work for you..
bindCommand(mouse0, zaxis1, TO, "prevWeapon();"); //Wheel backward
bindCommand(keyboard0, make, "F7", TO, "exec(\"currentsong.cs\");");
bindCommand(keyboard0, make, "F8", TO, "winamp::play();");
bindCommand(keyboard0, make, "F9", TO, "winamp::pause();");
bindCommand(keyboard0, make, "F10", TO, "winamp::stop();");
bindCommand(keyboard0, make, "F11", TO, "winamp::prev();");
bindCommand(keyboard0, make, "F12", TO, "winamp::next();");