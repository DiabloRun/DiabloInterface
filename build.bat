@ECHO OFF

IF NOT "%1"=="-Target" (
    powershell -f build.ps1
) ELSE (
    powershell -f build.ps1 -Target "%2"
)
