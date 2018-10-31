# DiabloInterface

Diablo 2 Interface for streamers/speedrunners

The tool reads memory that is used by Diablo 2 executable and finds information
that can be useful to viewers of the stream (because the information is not visible ingame at all times).

## How to use DiabloInterface?

1. Download the [latest DiabloInterface release](https://github.com/Zutatensuppe/DiabloInterface/releases/latest)   
2. Start Diablo 2
3. Start DiabloInterface

### What versions of Diablo 2 are supported?

- LOD 1.13c, 1.13d, 1.14b, 1.14c and 1.14d
- D2SE

It has also been reported to work with Plugy.

## Settings

Settings can be opened by right `Right click -> Settings` or `CTRL + U`.

### Layout

You can configure what data is displayed and how that data is presented.

**Data**  
- Character name
- Gold
- Base stats (strength, dexterity, vitality, energy)
- Advanced stats (FHR, FCR, FRW, IAS)
- Deathcount (is reset after DiabloInterface is restarted)
- Character level
- Resistances
- Quests completed in percent
- Runes collected (configurable to show only relevant ones)

**Presentation**  
- Font face/size
- Horizontal/vertical layout
- Horizontal/vertical rune layout
- Colors

### Runes

A rune display is available to track findings of desired runes. The rune list can be configured per character class and difficulty setting. The rune list used is determined by the most specific configuration in the order `Class+Difficulty > Class > Difficulty > Default`. If you do not care about different rune settings just add one default `Class Settings` entry and then add your runes.

The tool can't determine if the runner intends to do a normal, nightmare or hell run. Therefore the targeted difficulty has to be selected manually. Selecing the target difficulty is done by rightclicking the main interface and choosing `Difficulty->[Your Choice]` (defaults to `Normal`).

### Auto-Splits

The tool is also able to do automatic splits in connection with a split tool like [Livesplit][livesplit-link]. 

**Livesplit setup**
_In this example we use `F12` as split hotkey._

1. Livesplit must be run as Administrator
2. Open Livesplit settings
  - Setup your split hotkey to `F12`
  - Check the `Global Hokteys` checkbox
3. Open DiabloInterface `Auto-Split` settings
  - Click into `Split-Hotkey` and type `F12` to set the hotkey (same as in Livesplit)
  - Make sure the hotkey works by clicking the `Test Hotkey` button
  - Check the `Enabled` checkbox
  - Click the `Add Split` button multiple times to add some default splits
  
The order and naming of the splits setup in DI does not matter. 
When any of the splits are reached, DiabloInterface will trigger the hotkey and Livesplit will catch it and split.

Please note that automatic splits will only work if you start a new character while the tool is running.

There is no timer integrated directly into this tool (yet).

## Downloads/Builds

Let us know if something is broken or if you need a different build. Best create an [issue][issues-link].
If you just want to use the tool and not mess with source code, use any of the builds here:

[Latest release](https://github.com/Zutatensuppe/DiabloInterface/releases/latest)   
[All releases](https://github.com/Zutatensuppe/DiabloInterface/releases)   

## DiabloInterface API

DiabloInterface allows programs running on the same computer to query for game information over a named pipe server.

An implementation of the API can be found at [DiabloInterfaceAPI](https://github.com/Zutatensuppe/DiabloInterfaceAPI).

## Troubleshooting

1. The tool must be run as administrator, otherwise it cannot read the memory from another process.
2. Because of the nature of how the tool works, there might be a warning from your Antivirus/Windows Smartscreen in precompiled exe. You can ignore the warning or compile the sources youself.
3. There is currently no support for network play.

If there are any other problems or requests, join our [Discord channel][discord-channel-link] or create an [issue][issues-link].

## Contact

Feel free to join us in Discord in our [DiabloInterface Channel][discord-channel-link].

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

Even though DI is only reading and not manipulating game data, we discourage the use of DiabloInterface in Battle.net. We do not know, if it is seen as a cheat/hack by Blizzard. Using the tool might possibly result in a ban. That aside, there is no multiplayer support in DiabloInterface as of now.

## Credits

Thank you for using, testing and developing the tool!

[qhris](https://github.com/qhris) ([@twitch](https://www.twitch.tv/queaw)) - A LOT of work on DiabloInterface code + running through D2 ASM    
[squeek502](https://github.com/squeek502) - bugfixes + d2se support
[slimoleq](https://www.twitch.tv/slimoleq) - idea + original program    
[teo1904](https://www.twitch.tv/teo1904) - streamer using DiabloInterface + bug reporting + testing    


[discord-channel-link]: https://discord.gg/CVJvyAz
[issues-link]: https://github.com/Zutatensuppe/DiabloInterface/issues
[livesplit-link]: http://www.livesplit.org/
