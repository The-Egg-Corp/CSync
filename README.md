> [!WARNING]
> **I will no longer affiliate with Thunderstore or the Lethal Modding Discord.<br>
> Further updates will only be [here](https://github.com/Owen3H/CSync/releases). I will not support any forks.**
> 
> **You are free to join my [discord](https://discord.gg/CMyTmUMP2P) if you would like to chat.**

# CSync <img align="right" width="128" height="128" src="https://gcdn.thunderstore.io/live/repository/icons/Owen3H-CSync-1.0.8.png.128x128_q95.png">
An easy-to-implement configuration file syncing library.<br>
**CSync** is game-independent and should work for any **BepInEx** supported game!

The [wiki](https://github.com/Owen3H/CSync/wiki) should cover all you need to start using this library.

> [!IMPORTANT]
> - This is **NOT** a standalone mod, it is intended for mod developers and does nothing on its own!<br>
> - This does **NOT** edit or replace config files directly, everything is done in-memory.<br>
> - It will **NOT** sync configs from mods that aren't dependent upon it.<br>
> - CSync uses Unity's [Named Messages](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/custom-messages/#name-message-example) to avoid **Netcode Patcher** and **NetworkBehaviour**.

## Features
- Can serialize a `ConfigEntry` with a drop-in replacement. (`SyncedEntry`)
- No seperate config file system, retains BepInEx support.
- Uses `DataContractSerializer`, a fast and safer alternative to `BinaryFormatter`.
- Provides helpful extension methods.

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
