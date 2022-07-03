function Forward() { postAction(2048, IDACTION_MOVEFORWARD, 1.000000); }
function Backward() { postAction(2048, IDACTION_MOVEBACK, 1.000000); }
function MoveLeft() { postAction(2048, IDACTION_MOVELEFT, 1.0000000); }
function MoveRight() { postAction(2048, IDACTION_MOVERIGHT, 1.0000000); }

function StopForward() { postAction(2048, IDACTION_MOVEFORWARD, 0.000000); }
function StopBackward() { postAction(2048, IDACTION_MOVEBACK, 0.000000); }
function StopMoveLeft() { postAction(2048, IDACTION_MOVELEFT, 0.000000); }
function StopMoveRight() { postAction(2048, IDACTION_MOVERIGHT, 0.000000); }
function StopTurnLeft() { postAction(2048, IDACTION_TURNLEFT, 0.000000); }
function StopTurnRight() { postAction(2048, IDACTION_TURNRIGHT, 0.000000); }


//Correction turns
function correctionTurnRight() {
    Schedule("ExtremeSlowTurnRight();", 0);
    Schedule("StopTurnRight();", 0.25);
}

function correctionTurnLeft() {
    Schedule("ExtremeSlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 0.25);
}



//Extremely Slow Turns
function ExtremeSlowTurnLeft() { postAction(2048, IDACTION_TURNLEFT, 0.00314159); }
function ExtremeSlowTurnRight() { postAction(2048, IDACTION_TURNRIGHT, 0.00314159); }

function extremeSlowTurnRight45() {
    Schedule("ExtremeSlowTurnRight();", 0);
    Schedule("StopTurnRight();", 8);
}

function extremeSlowTurnLeft45() {
    Schedule("ExtremeSlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 8);
}

function extremeSlowTurnRight90() {
    Schedule("ExtremeSlowTurnRight();", 0);
    Schedule("StopTurnRight();", 16);
}

function extremeSlowTurnLeft90() {
    Schedule("ExtremeSlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 16);
}

function extremeSlowTurnRight135() {
    Schedule("ExtremeSlowTurnRight();", 0);
    Schedule("StopTurnRight();", 24);
}

function extremeSlowTurnLeft135() {
    Schedule("ExtremeSlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 24);
}





//Slow turns

function SlowTurnLeft() { postAction(2048, IDACTION_TURNLEFT, 0.0062904); }
function SlowTurnRight() { postAction(2048, IDACTION_TURNRIGHT, 0.0062904); }

function slowTurnRight45() {
    Schedule("SlowTurnRight();", 0);
    Schedule("StopTurnRight();", 4);
}

function slowTurnLeft45() {
    Schedule("SlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 4);
}

function slowTurnRight90() {
    Schedule("SlowTurnRight();", 0);
    Schedule("StopTurnRight();", 8);
}

function slowTurnLeft90() {
    Schedule("SlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 8);
}

function slowTurnRight135() {
    Schedule("SlowTurnRight();", 0);
    Schedule("StopTurnRight();", 12);
}

function slowTurnLeft135() {
    Schedule("SlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 12);
}

function slowTurnRight180() {
    Schedule("SlowTurnRight();", 0);
    Schedule("StopTurnRight();", 16);
}

function slowTurnLeft180() {
    Schedule("SlowTurnLeft();", 0);
    Schedule("StopTurnLeft();", 16);
}






//Fast turns
function FastTurnLeft() { postAction(2048, IDACTION_TURNLEFT, 0.0125850); }
function FastTurnRight() { postAction(2048, IDACTION_TURNRIGHT, 0.0125850); }

function fastTurnRight45() {
    Schedule("FastTurnRight();", 0);
    Schedule("StopTurnRight();", 2);
}

function fastTurnLeft45() {
    Schedule("FastTurnLeft();", 0);
    Schedule("StopTurnLeft();", 2);
}

function fastTurnRight90() {
    Schedule("FastTurnRight();", 0);
    Schedule("StopTurnRight();", 4);
}

function fastTurnLeft90() {
    Schedule("FastTurnLeft();", 0);
    Schedule("StopTurnLeft();", 4);
}

function fastTurnRight135() {
    Schedule("FastTurnRight();", 0);
    Schedule("StopTurnRight();", 6);
}

function fastTurnLeft135() {
    Schedule("FastTurnLeft();", 0);
    Schedule("StopTurnLeft();", 6);
}

function fastTurnRight180() {
    Schedule("FastTurnRight();", 0);
    Schedule("StopTurnRight();", 8);
}

function fastTurnLeft180() {
    Schedule("FastTurnLeft();", 0);
    Schedule("StopTurnLeft();", 8);
}






//Faster turns

function FasterTurnLeft() { postAction(2048, IDACTION_TURNLEFT, 0.0251700); }
function FasterTurnRight() { postAction(2048, IDACTION_TURNRIGHT, 0.0251700); }

function fasterTurnRight45() {
    Schedule("FasterTurnRight();", 0);
    Schedule("StopTurnRight();", 1);
}

function fasterTurnLeft45() {
    Schedule("FasterTurnLeft();", 0);
    Schedule("StopTurnLeft();", 1);
}

function fasterTurnRight90() {
    Schedule("FasterTurnRight();", 0);
    Schedule("StopTurnRight();", 2);
}

function fasterTurnLeft90() {
    Schedule("FasterTurnLeft();", 0);
    Schedule("StopTurnLeft();", 2);
}

function fasterTurnRight135() {
    Schedule("FasterTurnRight();", 0);
    Schedule("StopTurnRight();", 3);
}

function fasterTurnLeft135() {
    Schedule("FasterTurnLeft();", 0);
    Schedule("StopTurnLeft();", 3);
}

function fasterTurnRight180() {
    Schedule("FasterTurnRight();", 0);
    Schedule("StopTurnRight();", 4);
}

function fasterTurnLeft180() {
    Schedule("FasterTurnLeft();", 0);
    Schedule("StopTurnLeft();", 4);
}







//Fastest turns

function FastestTurnLeft() { postAction(2048, IDACTION_TURNLEFT, 0.0503450); }
function FastestTurnRight() { postAction(2048, IDACTION_TURNRIGHT, 0.0503450); }

function fastestTurnRight45() {
    Schedule("FastestTurnRight();", 0);
    Schedule("StopTurnRight();", 0.5);
}

function fastestTurnLeft45() {
    Schedule("FastestTurnLeft();", 0);
    Schedule("StopTurnLeft();", 0.5);
}

function fastestTurnRight90() {
    Schedule("FastestTurnRight();", 0);
    Schedule("StopTurnRight();", 1);
}

function fastestTurnLeft90() {
    Schedule("FastestTurnLeft();", 0);
    Schedule("StopTurnLeft();", 1);
}

function fastestTurnRight135() {
    Schedule("FastestTurnRight();", 0);
    Schedule("StopTurnRight();", 1.5);
}

function fastestTurnLeft135() {
    Schedule("FastestTurnLeft();", 0);
    Schedule("StopTurnLeft();", 1.5);
}

function fastestTurnRight180() {
    Schedule("FastestTurnRight();", 0);
    Schedule("StopTurnRight();", 2);
}

function fastestTurnLeft180() {
    Schedule("FastestTurnLeft();", 0);
    Schedule("StopTurnLeft();", 2);
}