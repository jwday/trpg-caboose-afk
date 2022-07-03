// Tenebrous Cave Position Function
function determineTenePosition(%pack::direction, %pack::distance) {
	%direction::tene = "North West";
	%pos::A::tene = 1023; //Looking NE, not quite at the top of a hill before dropping down into a valley
	%pos::B::tene = 1054; //Looking NE, not quite at the top of a hill before slowly dropping down the back sidet of a hill, going up another, then down into a valley
	%pos::C::tene = 1069; //Looking NE, see out into a valley with a cliff face on the right
	%pos::D::tene = 1019; //Looking NE, see into a cliff face

	if(%pack::direction == %direction::tene) {
		if(%pack::distance <= $pos::A::tene + 1 && %pack::distance >= $pos::A::tene - 1) {
			schedule::Cancel("trackPack");
			// schedule("say(1,\"#say Tenebrous Cave, Position A\");", 2);
			schedule("move::pos::A::tene();", 1);
		}
		else if(%pack::distance <= $pos::B::tene + 1 && %pack::distance >= $pos::B::tene - 1) {
			schedule::Cancel("trackPack");
			// schedule("say(1,\"#say Tenebrous Cave, Position B\");", 2);
			schedule("move::pos::B::tene();", 1);
		}
		else if(%pack::distance <= $pos::C::tene + 1 && %pack::distance >= $pos::C::tene - 1) {
			schedule::Cancel("trackPack");
			// schedule("say(1,\"#say Tenebrous Cave, Position C\");", 2);
			schedule("move::pos::C::tene();", 1);
		}
		else if(%pack::distance <= $pos::D::tene + 1 && %pack::distance >= $pos::D::tene - 1) {
			schedule::Cancel("trackPack");
			// schedule("say(1,\"#say Tenebrous Cave, Position D\");", 2);
			schedule("move::pos::D::tene();", 1);
		}
		else {
			schedule("say(1,\"#say I'm at Tenebrous Cave, but something's not right. Let's try this again.\");", 1);
			schedule::Cancel("trackPack");
			$intending::to::transport::tene = true;
			schedule::add("cast::transport();", 2);
			$cast::interrupt::check = true;
		}
	}

	$determine::tene::position = false;
}

// Tenebrous Cave Movement Functions
function move::pos::A::tene() {
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

function move::pos::B::tene() {
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

function move::pos::C::tene() {
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

function move::pos::D::tene() {
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

// Tenebrous Cave Waypoint 1
function tene::waypoint::check1(%pack::direction, %pack::distance) {
	schedule::cancel("triangulate");

	if(%pack::distance == 1128) {
		$check1 = false;
		schedule("point2();", 2);
	}
	else if(%pack::distance == 1125) {
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
		schedule("say(1,\"#say I'm off target. I've done this \" @$off::target@ \" times now. Trying again...\");", 1);
		$check1 = false;
		$cast::interrupt::check = true;
		$intending::to::transport::tene = true;
		cast::transport();
	}
}