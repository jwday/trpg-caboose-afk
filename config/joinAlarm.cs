$Focus::Target[0] = "ScreenTest";
$Focus::Target[1] = "Arf";
$Focus::Target[2] = "Master";
$Focus::Target[3] = "Particle";


$Focus::JoinCitizenMessage[0] = "#global hey %target%";
$Focus::JoinCitizenMessage[1] = "#global hey";

function joinalarm::on() {
    $alarm = true;
}

function joinalarm::off() {
    $alarm = false;
}

event::Attach(eventClientJoin, Connected);

function Connected(%client) {
    if ($alarm) {
	    winamp::play();
    }
}

//event::Attach(eventClientChangeTeam, JoinedCitizen);

//function JoinedCitizen(%client) {
//    %client = client::getname(%client);
//        for (%i=0; $Focus::Target[%i] != ""; %i++) {
//        if (%client == $Focus::Target[%i]) {
//            %c = 0;
//            for (%x=0; $Focus::JoinCitizenMessage[%x] != ""; %x++) {
//                %c++;
//            }
//            %rnd = floor(getRandom() * %c);
//            %citizenmessage = String::replace($Focus::JoinCitizenMessage[%rnd], "%target%", %client);
//            schedule("say(0, %citizenmessage);", 5);
//        }
//    }
//}