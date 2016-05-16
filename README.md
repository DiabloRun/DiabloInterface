# DiabloInterface

Diablo 2 Interface for Streamers

The tool reads memory that is used by Diablo 2 executable and finds information 
that can be useful to viewers of the stream (because the information is not visible ingame at all times).

The information that is currently shown in the tool are as follows:
 - Player name
 - Deathcount
 - Gold
 - Base stats (strength, dexterity, vitality, energy)
 - Resistances

**Main window**  
![Main tool window](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/mainWindow.png)

**Settings window**  
![Settings tool window](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/settingsWindow.png)

**Debug window**  
![Settings tool window](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/debugWindow.png)

**Tool in action ([Slimoleq @ Twitch](https://www.twitch.tv/slimoleq))**  
![Slimoleq Stream](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/slimoScreen.png)

## Downloads/Builds

(Let me know if something is broken or if you need a different build.)

If you just want to use the tool and not mess with source code, use any of the builds here:


### Current Version:
Current version allows automatic splits in connection with a split tool like Livesplit. You need to setup the same splits and hotkeys as in your split tool (hotkey in DiabloInterface = Start/Split hotkey in split tool). There is no timer integrated directly into this tool (yet). Please note that automatic splits will only work if you start a new character while the tool is running.

[DiabloInterface_Autosplits_v0.1_.NET4.5.2.exe](https://github.com/Zutatensuppe/DiabloInterface/raw/master/builds/1.14B/DiabloInterface_Autosplits_v0.1_.NET4.5.2.exe) (D2 Patch 1.14B, compiled for .NET 4.5.2)

#### Example Config Files
Put them into same folder as the exe file and rename to settings.conf.

[settings-simple.conf](https://github.com/Zutatensuppe/DiabloInterface/raw/master/builds/1.14B/settings-simple.conf) (basic splits: only game start + bosses)

[settings-extended.conf](https://github.com/Zutatensuppe/DiabloInterface/raw/master/builds/1.14B/settings-extended.conf) (lots of autosplits using Slimoleqs splits)

### Older versions:
[DiabloInterface_.NET4.5.2.exe](https://github.com/Zutatensuppe/DiabloInterface/raw/master/builds/1.14B/DiabloInterface_.NET4.5.2.exe) (D2 Patch 1.14B, compiled for .NET 4.5.2)

## Troubleshooting

1. The tool must be run as administrator, otherwise it cannot read the memory from another process.
2. Because of the nature of how the tool works, there might be a warning from your Antivirus/Windows Smartscreen in precompiled exe. You can ignore the warning or compile the sources youself.
