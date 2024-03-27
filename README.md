# CSync <img align="right" width="132" height="132" src="https://media.discordapp.net/attachments/1215227926354731029/1217977613176410242/CSyncV69.png?ex=6605fd0b&is=65f3880b&hm=e037a52acf909f9b1605fcaaa67bdb16a41ba194ad04c2cddd675f588d56a4a9&=&format=webp&quality=lossless&width=408&height=408">
An easy-to-implement configuration file syncing library.<br>
The [wiki](https://github.com/Owen3H/CSync/wiki) should cover all you need to start using CSync.

> [!IMPORTANT]
> - This is **NOT** a standalone mod, it is intended for mod developers and does nothing on its own!<br>
> - This does **NOT** edit or replace config files directly, everything is done in-memory.<br>
> - It will **NOT** sync configs from mods that aren't dependent upon it.<br>
> - CSync uses Unity's [Named Messages](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/custom-messages/#name-message-example) to avoid **Netcode Patcher** and **NetworkBehaviour**.

> [!NOTE]
> Using a mirror is not recommended as CSync will continue to be updated.\
> If you have a suggestion or want something fixed, I'm all ears.

## Features
- Can serialize a `ConfigEntry` with a drop-in replacement. (`SyncedEntry`)
- No seperate config file system, retains BepInEx support.
- Uses `DataContractSerializer`, a fast and safer alternative to `BinaryFormatter`.
- Provides helpful extension methods.

## Contact
You can reach me at my [discord](https://discord.gg/CMyTmUMP2P) if you need support or just want to chat! :)

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
