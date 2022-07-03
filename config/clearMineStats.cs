$clear::run::times = 0;
$mineStatsClear::confirm = false;

function arm::clearMineStats() {
    if($clear::run::times == 1) {
        $mineStatsClear::confirm = true;
        clearMineStats();
    }
    else {
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Do you wish to <f2>CLEAR <f1>your mining stats?<nl>Initialize this command again to <f2>CONFIRM CLEAR.<nl><f1>Otherwise, run the <f2>DISARM <f1>command to disarm clear function.", 1);
        $clear::run::times++;
    }
}

function disarm::clearMineStats() {
    if($clear::run::times == 1) {
        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Mine Stats clear function has been <f2>DISARMED", 1);
        schedule("Client::centerPrint(\"\", 1);", 5);
        $clear::run::times = 0;
    }
}

function clearMineStats() {
    if($mineStatsClear::confirm) {
        exec("mineStats.cs");

        $mineStats::gem::total = "0";
        $mineStats::itemized::gem::total::percentageDiamond = "0";
        $mineStats::itemized::gem::total::percentageEmerald = "0";
        $mineStats::itemized::gem::total::percentageIron = "0";
        $mineStats::itemized::gem::total::percentageSapphire = "0";
        $mineStats::itemized::gem::total::percentageSilver = "0";
        $mineStats::itemized::gem::total::percentageTitanium = "0";
        $mineStats::itemized::gem::total::percentageTopaz = "0";
        $mineStats::itemized::gem::total::percentageTurquoise = "0";
        $mineStats::itemized::gem::totalDiamond = "0";
        $mineStats::itemized::gem::totalEmerald = "0";
        $mineStats::itemized::gem::totalIron = "0";
        $mineStats::itemized::gem::totalSapphire = "0";
        $mineStats::itemized::gem::totalSilver = "0";
        $mineStats::itemized::gem::totalTitanium = "0";
        $mineStats::itemized::gem::totalTopaz = "0";
        $mineStats::itemized::gem::totalTurquoise = "0";
        $mineStats::rate::average = "0";
        $mineStats::round::gem::total = "0";
        $mineStats::round::rate = "0";
        $mineStats::time::last::round = "";
        $mineStats::time::round::partial = "0";
        $mineStats::time::to::billion = "0";
        $mineStats::time::total = "0";

        $itemized::gem::total[Diamond] = 0;
        $itemized::gem::total[Emerald] = 0;
        $itemized::gem::total[Titanium] = 0;
        $itemized::gem::total[Silver] = 0;
        $itemized::gem::total[Sapphire] = 0;
        $itemized::gem::total[Topaz] = 0;
        $itemized::gem::total[Iron] = 0;
        $itemized::gem::total[Turquoise] = 0;

        $round::gem::total = 0;
        $gem::total = 0;

        $itemized::gem::total::percentage[Diamond] = 0;
        $itemized::gem::total::percentage[Emerald] = 0;
        $itemized::gem::total::percentage[Titanium] = 0;
        $itemized::gem::total::percentage[Silver] = 0;
        $itemized::gem::total::percentage[Sapphire] = 0;
        $itemized::gem::total::percentage[Topaz] = 0;
        $itemized::gem::total::percentage[Iron] = 0;
        $itemized::gem::total::percentage[Turquoise] = 0;

        $mine::time::round = 0;
        $mine::time::total = 0;

        $mine::round::rate = 0;
        $mine::total::rate::average = 0;
        $mine::time::to::billion = 0;

        $mine::time::last::round = 0;
        $mine::time::round = 0;
        $mine::time::round::partial = 0;
        $time::at::round::beginning = 0;

        $itemized::round::gem::total[Diamond] = 0;
        $itemized::round::gem::total[Emerald] = 0;
        $itemized::round::gem::total[Titanium] = 0;
        $itemized::round::gem::total[Silver] = 0;
        $itemized::round::gem::total[Sapphire] = 0;
        $itemized::round::gem::total[Topaz] = 0;
        $itemized::round::gem::total[Iron] = 0;            
        $itemized::round::gem::total[Turquoise] = 0;

        export("$mineStats::*", "config\\mineStats.cs");

        $mineStatsClear::confirm = false;
        $clear::run::times = 0;

        Client::centerPrint("<jc><f0>Caboose Mining: <f1>Mine Stats <f2>CLEARED", 1);
        schedule("Client::centerPrint(\"\", 1);", 5);
    }
}