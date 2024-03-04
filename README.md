I WILL NO LONGER AFFILIATE WITH THUNDERSTORE OR THE LETHAL MODDING DISCORD.
PLEASE SEE [RELEASES](https://github.com/Owen3H/CSync/releases)

# CSync <img align="right" width="128" height="128" src="https://gcdn.thunderstore.io/live/repository/icons/Owen3H-CSync-1.0.8.png.128x128_q95.png">
A BepInEx configuration file syncing library.<br>
This library will help you force clients to have the same settings as the host!

> [!IMPORTANT]
> - This is **NOT** a standalone mod, it is intended for mod developers and does nothing on its own!<br>
> - This does **NOT** edit or replace config files directly, everything is done in-memory.<br>
> - It will **NOT** sync configs from mods that aren't dependent upon it.<br>
> - CSync uses Unity's Named Messages to avoid **Netcode Patcher** and **NetworkBehaviour**.

## Features
- Can serialize a `ConfigEntry` with a drop-in replacement. (`SyncedEntry`)
- No seperate config file system, retains BepInEx support.
- Uses `DataContractSerializer`, a fast and safer alternative to `BinaryFormatter`.
- Provides helpful extension methods.

## Setup & Usage
A guide to both setting up and using CSync is available on [Lethal Wiki](https://lethal.wiki/dev/apis/csync).

## License
This project has the `CC BY-NC-SA 4.0` license.<br>
This means the following terms apply:

**Attribution**<br>
If you remix or adapt this project, appropriate credit must be given.<br>
Cloning the repo with intent to contribute is not subject to this.

**NonCommercial**<br>
You may not use this material for commercial purposes.

**ShareAlike**<br>
When remixing, adapting or building upon this material, you must
distribute the new material under the same license as the original.
