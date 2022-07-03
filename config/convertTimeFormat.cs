//Written by Caboose
//March 12, 2013
//Simply call "convertTimeFormat(%time)" with %time being the input in seconds
//Function will return time in HH:MM:SS format

function convertTimeFormat(%time) {
    if(%time < 10) {
        %time::string = "0:00:0" @%time;
    }

    if(%time >= 10 && %time < 60) {
        %time::string = "0:00:" @%time;
    }

    if(%time >= 60 && %time < 3600) {
        %time::min = floor(%time/60);
        %time::sec = ((%time/60) - %time::min)*60;

        if ((%time::sec - floor(%time::sec)) >= 0.9) {
            %time::sec = round(%time::sec);
        }
        else {
            %time::sec = floor(%time::sec);
        }


        if(%time::min < 10) {
            %time::min::str = "0" @ %time::min;
        }
        else {
            %time::min::str = %time::min;
        }

        if(%time::sec < 10) {
            %time::sec::str = "0" @ %time::sec;
        }
        else {
            %time::sec::str = %time::sec;
        }

        %time::string = "0:" @%time::min::str@ ":" @%time::sec::str;
    }

    if(%time >= 3600) {
        %time::hr = floor(%time/3600);
        %time::min = floor(((%time/3600) - %time::hr)*60);
        %time::sec = ((((%time/3600) - %time::hr)*60) - %time::min)*60;

        if ((%time::sec - floor(%time::sec)) >= 0.9) {
            %time::sec = round(%time::sec);
        }
        else {
            %time::sec = floor(%time::sec);
        }


        %time::hr::str = %time::hr;

        if(%time::min < 10) {
            %time::min::str = "0" @ %time::min;
        }
        else {
            %time::min::str = %time::min;
        }

        if(%time::sec < 10) {
            %time::sec::str = "0" @ %time::sec;
        }
        else {
            %time::sec::str = %time::sec;
        }

        %time::string = %time::hr::str @ ":" @ %time::min::str @ ":" @ %time::sec::str;
    }

    return %time::string;
}