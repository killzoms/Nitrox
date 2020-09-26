:: Shell script to execute build and analysis of the Nitrox repository.
:: It requires the SonarScanner for MSBuild.
:: For instructions see https://docs.sonarqube.org/display/SCAN/Analyzing+with+SonarQube+Scanner+for+MSBuild
:: You may need to install additional .NET SDK's and targeting packs for commandline builds.
:: Note: an account with access to the SubnauticaNitrox org on SonarCloud is also required.


@ECHO OFF
IF EXIST "sonar.config.bat" GOTO RunSonar

REM sonar.config.bat does not exist. See sonar.config.example.bat
PAUSE
GOTO END


:RunSonar

CALL "sonar.config.bat"

ECHO Sonar prepare phase
%sonar%SonarScanner.MSBuild.exe begin /k:"%sonarkey%" /o:"%sonarorg%" /d:sonar.host.url="%sonarurl%" /d:sonar.login="%token%" /v:"%version%" /d:sonar.sources="Nitrox/Nitrox.Bootloader,Nitrox/NitroxClient,Nitrox/NitroxLauncher,Nitrox/NitroxModel,Nitrox/NitroxModel.Subnautica,Nitrox/NitroxPatcher,Nitrox/NitroxServer,Nitrox/NitroxServer.Subnautica" /d:sonar.tests="Nitrox/NitroxTest" /d:sonar.cs.vstest.reportsPaths="Nitrox/TestResults"

ECHO Solution build for analysis
MsBuild.exe /t:Rebuild

ECHO Run test coverage tool
"%vsPath%\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" /Logger:trx;LogFileName=NitroxTestResult.trx "NitroxTest\bin\Debug\NitroxTest.dll"

ECHO Sonar processing phase
%sonar%SonarScanner.MSBuild.exe end /d:sonar.login="%token%"

PAUSE


:END
