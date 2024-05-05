# CSync <img align="right" width="132" height="132" src="https://gcdn.thunderstore.io/live/repository/icons/Owen3H-CSync-2.2.4.png.256x256_q95.png">
An easy-to-implement configuration file syncing library.<br>
The [wiki](../../wiki) should cover all you need to start using CSync.

If you have a suggestion or want something fixed, I'm all ears over at my [discord](https://discord.gg/CMyTmUMP2P)! :)

> [!IMPORTANT]
> - This is **NOT** a standalone mod, it is intended for mod developers and does nothing on its own!<br>
> - This does **NOT** edit or replace config files directly, everything is done in-memory.<br>
> - It will **NOT** sync configs from mods that aren't dependent upon it.<br>

## Features
- Can serialize a `ConfigEntry` with a drop-in replacement. (`SyncedEntry`)
- No seperate config file system, retains BepInEx support.
- Uses `DataContractSerializer`, a fast and safer alternative to `BinaryFormatter`.
- Provides helpful extension methods.

## Disclaimer
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
