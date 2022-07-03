echo("");
echo("Type bash::alarmon(); to activate.");
echo("Type bash::alarmoff(); to deactivate.");
echo("");

function bash::alarmon () {
    $alarm = true;
    echo("Bash alarm activated.");
}

function bash::alarmoff () {
    $alarm = false;
    echo("Bash alarm deactivated.");
}

event::Attach(eventClientMessage, bashed);

function bashed(%client, %msg) {
    if($alarm) {
        $Alarm::BashMessage = "Aw snap, %basher% bashed me! Sound the Winamp alarm!";
        %werebashed = Match::String(%msg, "You were bashed by * for * points of damage!");
        %bashname = Match::Result(0);

        if(%werebashed) {
            winamp::play();
		    %bashmessage = String::replace($Alarm::BashMessage, "%basher%", %bashname);
		    say(0, %bashmessage);
        }
    }
}