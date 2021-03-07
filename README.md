# DiabloInterface

[![Actions Status](https://github.com/Zutatensuppe/DiabloInterface/workflows/Build/badge.svg)](https://github.com/Zutatensuppe/DiabloInterface/actions)

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
- Plugy
- PD2 (basic support, but autosplitter is not working yet)

## Settings

Settings can be opened by right `Right click -> Config` or `CTRL + U`.

### Layout

You can configure what data is displayed and how that data is presented.

**Data**  
- Character name
- Gold
- Base stats (strength, dexterity, vitality, energy)
- Advanced stats (FHR, FCR, FRW, IAS)
- Character level
- Resistances
- Quests completed in percent
- Magic find
- Attacker takes dmg
- Extra monster gold
- /players X value
- Hardcore/Softcore
- Expansion/Classic
- Counters (tracked while the game is running)
  - Number of deaths (resets with new character creation)
  - Number of games started
  - Number of characters created
- Runes collected (configurable to show only relevant ones)
- Seed of current game
- Life
- Mana

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
_Note: In this example we use `F12` as split hotkey._

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

## HTTP Client

_Note: In case you are just trying to set DiabloInterface up for diablo.run, please follow the instructions at [diablo.run][diablo-run-web] after logging in with your twitch account. You may also want to check out [DiabloRun on github][diablo-run-github] or on [Discord][diablo-run-discord]!_

DiabloInterface can be configured to send out the read data to a url via http request whenever something important was read.

To configure it open the settings (Right click on DiabloInterface, click Config, click HttpClient tab).

`URL` <string>
Url where the request json is sent to, eg. `http://localhost:8123/`
  
`Enable` <bool>
Check to enable the http client

`Headers` <string>
Content that will be sent as part of the request json, regardless of what else is sent `{"Headers": 'content of Headers', ...}`
Regardless of the name, this does not get sent as HTTP headers!

`Status` 
Shows error messages or response of last request

### Play with/Test the http client

You can use a simple php script to test if the http client is working, and to see what it does.

index.php: 
```php
<?php
$body = file_get_contents('php://input');
echo $body; // send request data back to DiabloInterface `Status` textarea
error_log($body); // see the request data on the command line
```

Start up a php server:
```
$ php -S 192.168.1.42:8123
```

Configure the `URL` in DiabloInterface to point to your php server (`http://192.168.1.42:8123`).

Now run around in D2 and do something ;) You should see requests coming in and being logged 
to command line where you started the server, and also in the `Status` textarea of DiabloInterface.


## Twitch integration via diablo.run and Armory extension
_Note: diablo.run Armory extension is made by Borshter._

1. Setup diablo.run (go to https://diablo.run, login, follow the instructions)
2. Install the twitch extension https://dashboard.twitch.tv/extensions/f78avhja8qxf452so9uk915x4evwxa-0.0.1 (or search for diablo.run armory)
3. Follow the instructions of the twitch extension
4. Done 🎉


## Twitch integration via D2ID
_Note: D2ID and twitch extensions are made by [palmettos](https://github.com/palmettos)._ 

D2ID: https://github.com/palmettos/D2ID  
Twitch extension: https://dashboard.twitch.tv/extensions/l0m154prxsmwu1m0tyic1ei0915byb-0.0.4

D2ID (v0.1.4) comes bundled with an outdated version of DiabloInterface. It is old but works out of the box.

How to use a current version of DiabloInterface with D2ID:
  - Launch new version of DiabloInterface
  - Open `Settings`
  - Go to `Misc` tab
  - Change the `Pipe Name` in the `Pipe Server` section to `DiabloInterfaceItems`
  - Done 🎉

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

If there are any other problems, suggestions or requests:
- ask in join our [Discord channel][discord-channel-link]
- create an [issue][issues-link].

## Troubleshooting (dev)

If at build an error like `The underlying connection was closed: An unexpected error occurred on a send` pops up,
try this [solution](https://stackoverflow.com/questions/20445638/nuget-fails-the-underlying-connection-was-closed-an-unexpected-error-occurred).

## Contact

Feel free to join us in Discord in our [DiabloInterface Channel][discord-channel-link].

## Screenshots

**Main window**  
![Main tool window](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/main-win-v0.3.0-1.png)
![Main tool window with other settings](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/main-win-v0.3.0-2.png)

**Tool in action**  
[Slimoleq @ Twitch](https://www.twitch.tv/slimoleq)    
![Slimoleq Stream](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/slimoScreen.png)

[Teo1904 @ Twitch](https://www.twitch.tv/teo1904)    
![Teo1904 Stream](https://github.com/Zutatensuppe/DiabloInterface/raw/master/docs/assets/img/teoScreen.png)

## Use in Battle.net

Even though DI is only reading and not manipulating game data, we discourage the use of DiabloInterface in Battle.net. We do not know, if it is seen as a cheat/hack by Blizzard. Using the tool might possibly result in a ban. That aside, there is no multiplayer support in DiabloInterface as of now.

## Credits

Thanks to everyone using, testing and developing the tool!

[qhris](https://github.com/qhris) ([@twitch](https://www.twitch.tv/queaw)) - A LOT of work on DiabloInterface code + running through D2 ASM    
[squeek502](https://github.com/squeek502) - bugfixes + d2se support
[slimoleq](https://www.twitch.tv/slimoleq) - idea + original program    
[teo1904](https://www.twitch.tv/teo1904) - streamer using DiabloInterface + bug reporting + testing    


[discord-channel-link]: https://discord.gg/CVJvyAz
[issues-link]: https://github.com/Zutatensuppe/DiabloInterface/issues
[livesplit-link]: http://www.livesplit.org/
[diablo-run-web]: https://diablo.run
[diablo-run-github]: https://github.com/DiabloRun
[diablo-run-discord]: https://discord.gg/QMMDR2a
