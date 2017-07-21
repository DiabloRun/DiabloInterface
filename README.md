# DiabloInterface

Diablo 2 Interface for streamers/speedrunners

The tool reads memory that is used by Diablo 2 executable and finds information
that can be useful to viewers of the stream (because the information is not visible ingame at all times).

### Stats
The information that is currently shown in the tool are as follows:
 - Player name
 - Deathcount (is reset after DiabloInterface is restarted)
 - Gold
 - Level
 - Base stats (strength, dexterity, vitality, energy)
 - Advanced stats (FHR, FCR, FRW, IAS)
 - Resistances
 - Runes collected (configurable to show only relevant ones)

### Auto-Splits
The tool is also able to do automatic splits in connection with a split tool like Livesplit. First you need to setup the same hotkey in DiabloInterface as your Start/Split hotkey in split tool. Then you have to setup splits that should be sent to your split tool. The naming and order of splits in DiabloInterface are not relevant, as only a Keypress is sent to the split tool then a split point is reached. There is no timer integrated directly into this tool (yet). Please note that automatic splits will only work if you start a new character while the tool is running.

### Runes

A rune display is available that can track which runes that the runner still needs. The rune list can be configured per character class and difficulty setting. The rune list used is determined by the most specific configuration in the order `Class+Difficulty > Class > Difficulty > Default`.

The tool can't determine if the runner is intending on a normal/nightmare or hell run, therefore the targeted difficulty will have to be selected. Selecing the target difficulty is done by rightclicking the main interface and choosing `Difficulty->[Your Choice]` (defaults to `Normal`).


### DiabloInterface API

DiabloInterface allows programs running on the same computer to query for game information over a named pipe server.

An implementation of the API can be found at [DiabloInterfaceAPI](https://github.com/Zutatensuppe/DiabloInterfaceAPI).

## Downloads/Builds

Let us know if something is broken or if you need a different build. Best create an [issue](https://github.com/Zutatensuppe/DiabloInterface/issues).
If you just want to use the tool and not mess with source code, use any of the builds here:

[Latest release](https://github.com/Zutatensuppe/DiabloInterface/releases/latest)   
[All releases](https://github.com/Zutatensuppe/DiabloInterface/releases)   

**Example config file**    
Put them into same folder as the exe file and rename to settings.conf, or just load via settings menu.

[settings.conf](https://github.com/Zutatensuppe/DiabloInterface/releases/download/v0.3.0/settings.conf)

## Contact

Feel free to join us in Discord in our [DiabloInterface Channel](https://discord.gg/CVJvyAz).

## Troubleshooting

1. The tool must be run as administrator, otherwise it cannot read the memory from another process.
2. Because of the nature of how the tool works, there might be a warning from your Antivirus/Windows Smartscreen in precompiled exe. You can ignore the warning or compile the sources youself.

## Screenshots

**Main window**  
![Main tool window](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/main-win-v0.3.0-1.png)
![Main tool window with other settings](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/main-win-v0.3.0-2.png)

**Settings window**  
![Settings tool window](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/settings-win-v0.3.0-1.png)

**Tool in action**  
[Slimoleq @ Twitch](https://www.twitch.tv/slimoleq)    
![Slimoleq Stream](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/slimoScreen.png)

[Teo1904 @ Twitch](https://www.twitch.tv/teo1904)    
![Teo1904 Stream](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/teoScreen.png)

## Use in Battle.net

Even though DI is only reading and not manipulating game data, we discourage the use of DiabloInterface in Battle.net. We do not know, if it is seen as a cheat/hack by Blizzard. Using the tool might possibly result in a ban.

## Credits

Thank you for using, testing and developing the tool!

[qhris](https://github.com/qhris) ([@twitch](https://www.twitch.tv/queaw)) - A LOT of work on DiabloInterface code + running through D2 ASM    
[slimoleq](https://www.twitch.tv/slimoleq) - idea + original program    
[teo1904](https://www.twitch.tv/teo1904) - streamer using DiabloInterface + bug reporting + testing    
