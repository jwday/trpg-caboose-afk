///////////////////////////////////////////////////////
// Auto Invite Party
///////////////////////////////////////////////////////
//Created by CdWhistler
//It works by inviting people to your party if they say invite in your zone.
//If someone is annoying you by abusing this script just mute them.  You wont autoinvite them then.
//---NEW---
//Keeps track of who is in your party. Run showpartystatus(); in the console to see the list.
//V2.1

$InviteAdvTime = 1000;	//Sets how often you want to advertise in seconds that you have a party.

include("presto\\Match.cs");
include("presto\\Event.cs");

event::Attach(eventClientMessage, PartyMonitor);
function PartyMonitor (%cl, %msg) {
	if ($AcceptingINV == 1) {

		if (%msg == "You have created a new party.") {
			$partyactive = 1; //Enable party.
			schedule("say(0, \"#zone I have created a party, say  | #zone invite |  to be invited.\");", 1.0);
			schedule("advertiseparty();", $InviteAdvTime); //Schedule party advertisement
			deleteVariables("$PartyList*"); //Cleanup the party list.
			$PartyInvited = ""; //Reset the count of party members.
			return;
		}
		else if (%msg == "Your party has been disbanded.") {
			$partyactive = 0; //Disable party.
			schedule("say(0, \"#zone The party has been disbanded.\");", 1.0);
			schedule::Cancel(Advertiseparty); //Stop advertising the party.
			deleteVariables("$PartyList*"); //Cleanup the party list.
			$PartyInvited = ""; //Reset the count of party members.
			return;
		}
		else if (Match::String(%msg, "You have invited * to join your party.") == 1) {
			%iname = Match::Result(0);
			for(%i = 1;%i <= $PartyInvited;%i++) { //Check through the list of names we have.
				if($PartyList[%i, Name] == %iname) {  //If they are already on the list set their status to "Invited".
					$PartyList[%i, Status] = "Invited";
					return;
				}
			}
			$PartyInvited++; //Start the list count or increase it with a new person joining.
			$PartyList[$PartyInvited, Name] = %iname;  //Add their name to the list.
			$PartyList[$PartyInvited, Status] = "Invited";  //Set their status to "Invited"
		}
		else if (Match::String(%msg, "* is no longer in your party.") == 1) {
			%dname = Match::Result(0);
			for(%i;%i <= $PartyInvited;%i++) { //Check through the list of party members.
				if($PartyList[%i, Name] == %dname) {  //Ah found the one we were looking for.
					$PartyList[%i, Status] = "Left";  //Set their status to "Left".
					return;
				}
			}
		}
		else if (Match::String(%msg, "* has joined your party.") == 1) {
			%jname = Match::Result(0);
			for(%i = 1;%i <= $PartyInvited;%i++) { //Check through the list of party members.
				if($PartyList[%i, Name] == %jname) { //Ah found the one we were looking for.
					$PartyList[%i, Status] = "Joined";  //Set their status to "Joined".
					return;
				}
			}
		}
		else if (Match::String(%msg, "* dropped.") == 1) {
      %dropname = Match::Result(0);
      for(%i = 1;%i <= $PartyInvited;%i++) { //Check through the list of party members.
				if($PartyList[%i, Name] == %dropname) { //Ah found the one we were looking for.
					$PartyList[%i, Status] = "Left";  //They got invited and then dropped.
					return;
				}
      } 	
    }
		if ($partyactive == 1) {
			%statusreceived = Match::String(%msg, "[*] * \"*invite*\"");
			%zone           = Match::Result(0);
			%person         = Match::Result(1);
			if (%statusreceived && %zone == "ZONE") {  //Make sure we heard it in zone.
				for(%i = 1;%i <= $PartyInvited;%i++) {  //Check through the list of party members.
					if($PartyList[%i, Name] == %person) {
						if($PartyList[%i, Status] == "Joined" || $PartyList[%i, Status] == "Invited") {
							%NoInvite = 1;
						}
					}
				}
				if(%NoInvite != 1) { //Do not invite if they are already joined or invited.
					remoteEval(2048, ScoresOn);  //Bring up tab menu.
					schedule("clientMenuSelect(\"addparty " @ getClientByName(%person) @ "\");", 1.0);  //Invite them.
				}
			} //End statusreceived check 
		} //End partyactive check
	} //End AcceptingINV check
} //End Partymonitor function

function showpartystatus() {
	if($PartyInvited == "") {
		echo("No Party Status Available");
		return;
	}
	for(%i = 1;%i <= $PartyInvited;%i++) {
		echo($PartyList[%i, Name] @" : " @$PartyList[%i,Status]);
		if($PartyList[%i, Status] != "Left" && $PartyList[%i, Status] != "Invited") %curmembers++;
	}
	echo("Total Current Members: "@%curmembers);
}

function advertiseparty() {
	if($partyactive == 1) {
		say(0, "#zone There is a party made. Say | #zone invite | to be invited.");
		schedule::Add("advertiseparty();", $InviteAdvTime, AdvertiseParty);
	}
}
if ($AcceptingINV == "") {
	$AcceptingINV = 1;
	echo("");
	echo("");
	echo("Auto-inviting players into your party.");
	echo("");
	echo("");
} else if ($AcceptingINV == "1") {
	$AcceptingINV = "";
	echo("");
	echo("");
	echo("No longer auto-inviting players into your party.");
	echo("");
	echo("");
}
