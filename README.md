# CSync <img align="right" width="128" height="128" src="https://media.discordapp.net/attachments/974491955864150046/1199740908879491162/CSync.png?ex=65c3a4ca&is=65b12fca&hm=1fd441d8eec89e22e16ab5963ec80244366a52aa0c12cdb01b069071bd1140d1&=&format=webp&quality=lossless&width=671&height=671">
A BepInEx configuration file syncing library.<br>
This library will help you force clients to have the same settings as the host!

## Features
- Can serialize a `ConfigEntry` with a drop-in replacement. (`SyncedEntry`)
- No seperate config file system, retains BepInEx support.
- Uses `DataContractSerializer`, a fast and safer alternative to `BinaryFormatter`.
- Provides helpful extension methods.

## Setup
1. Download and extract BepInEx v5 into your game directory.
2. Drop the `CSync.dll` into the `../BepInEx/plugins` folder.
3. In your mod project, add an **Assembly Reference** to the aformentioned DLL.

## Usage
See the Thunderstore [wiki](https://thunderstore.io/c/lethal-company/p/Owen3H/CSync/wiki/) for a guide to using CSync.

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
