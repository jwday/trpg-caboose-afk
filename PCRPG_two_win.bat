@ECHO OFF
SETLOCAL ENABLEDELAYEDEXPANSION
TITLE Infinite Double Spawn PCRPG

set /A restarts=0

:SPAWN
	:: CHECK IF IT CRASHED BUT IS STILL RUNNING IN THE BG

	FOR /F %%b in ('TASKLIST ^| FIND /I /C "T1Vista.exe"') DO SET count=%%b
	:: ECHO No. of instances: %count%

	FOR /F %%a in ('TASKLIST /FI "STATUS eq UNKNOWN" ^| FIND /I /C "T1Vista.exe"') DO SET countBroken=%%a
	:: ECHO No. of broken instances: %countBroken%

	FOR /F "tokens=2" %%K in ('TASKLIST /FI "IMAGENAME eq T1Vista.exe" /FI "STATUS eq UNKNOWN" /FO LIST ^| FINDSTR /B "PID:"') DO (
		SET brokenPID=%%K
		ECHO Broken instance PID: !brokenPID!
		ECHO Killing !brokenPID!....
		TASKKILL /F /PID !brokenPID!
	)	
	CLS
	IF %count% EQU 0 (
		ECHO 0 instances running. Starting 2x T1Vista.exe...
		START "" "D:\Program Files\Dynamix\TRIBES\T1Vista.exe" -mod RPG +connect tribes.pcrpg.org:28001
		@TIMEOUT /T 5 /NOBREAK>NUL
		START "" "D:\Program Files\Dynamix\TRIBES\T1Vista.exe" -mod RPG +connect tribes.pcrpg.org:28001
		set /A restarts+=2
	)
	
	IF %count% EQU 1 (
		ECHO %restarts% restarts since script execution.
		ECHO 1 instance running. Starting 1x T1Vista.exe...
		START "" "D:\Program Files\Dynamix\TRIBES\T1Vista.exe" -mod RPG +connect tribes.pcrpg.org:28001
		set /A restarts+=1
	)

	IF %count% EQU 2 (
		ECHO %restarts% restarts since script execution.
		ECHO 2 instances running. Checking again in 30 seconds...
	)

	IF %count% GTR 2 (
		ECHO More than two instances have been found. UH OH.
	)

	ECHO Press CTRL+C to quit
	@TIMEOUT /T 30 /NOBREAK>NUL
	GOTO SPAWN
