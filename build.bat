@ECHO OFF

IF NOT "%1"=="-Target" (
    powershell -f build.ps1 -PlatformTarget "Any CPU"
    powershell -f build.ps1 -PlatformTarget "x64"
    powershell -f build.ps1 -PlatformTarget "x86"
) ELSE (
    powershell -f build.ps1 -Target "%2" -PlatformTarget "Any CPU"
    powershell -f build.ps1 -Target "%2" -PlatformTarget "x64"
    powershell -f build.ps1 -Target "%2" -PlatformTarget "x86"
)
