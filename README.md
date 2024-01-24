# CSync <img align="right" width="128" height="128" src="https://media.discordapp.net/attachments/974491955864150046/1199740908879491162/CSync.png?ex=65c3a4ca&is=65b12fca&hm=1fd441d8eec89e22e16ab5963ec80244366a52aa0c12cdb01b069071bd1140d1&=&format=webp&quality=lossless&width=671&height=671">
A BepInEx configuration file syncing library.<br>
This library will help you force clients to have the same settings as the host!

## Features
- Can serialize a `ConfigEntry` with a drop-in replacement.
- No seperate config file system, retains BepInEx support.
- Uses `DataContractSerializer`, a fast and safer alternative to `BinaryFormatter`.
- Provides helpful extension methods.

## Setup
1. Download and extract BepInEx v5 into your game directory.
2. Drop the `CSync.dll` into the `../BepInEx/plugins` folder.
3. In your mod project, add an **Assembly Reference** to the aformentioned DLL.

## Usage
#### 1. Enable serialization.
To begin, we will create a new Config class that will inherit from `SyncedInstance`.<br>
We must then add the `[DataContract]` attribute for this to be synced with clients.

```cs
[DataContract]
public class Config : SyncedInstance<Config>
```

#### 2. Create/modify entries.
Within the Config class, we can now begin writing out our config entries that we want to be synchronized.<br>
We must also mark them with the `[DataMember]` attribute for the serializer to recognize them.

```cs
[DataContract]
public class Config : SyncedInstance<Config> {
    public ConfigEntry<bool> PLUGIN_ENABLED { get; private set; }

    [DataMember] public SyncedEntry<float> MOVEMENT_SPEED { get; private set; }
    [DataMember] public SyncedEntry<float> CLIMB_SPEED { get; private set; }
}
```

#### 3. BepInEx binding.<br>
Before binding, we will add the following line at the top of the constructor.
```cs
InitInstance(this);
```

We can now bind our entries to the BepInEx config file like usual, however we will use the dedicated `BindSyncedEntry` extension method provided by CSync.

```cs
public Config(ConfigFile cfg) {
    InitInstance(this);

    MOVEMENT_SPEED = cfg.BindSyncedEntry("Movement", "fMovementSpeed", 4.1f,
        "The base speed at which the player moves. This is NOT a multiplier."
    );

    CLIMB_SPEED = cfg.BindSyncedEntry("Movement", "fClimbSpeed", 3.9f,
        "The base speed at which the player climbs. This is NOT a multiplier."
    );
}
```

#### 4. Add synchronization methods.
Finally, we will place the following methods within the class, replacing any instances of `Plugin.GUID` with our own.<br>
Alternatively, make sure you have a `GUID` property in your `Plugin.cs` file.

Now when we reference `.Value` on an entry, it will be the same as whatever the host has set!

```cs
internal static void RequestSync() {
    if (!IsClient) return;

    using FastBufferWriter stream = new(IntSize, Allocator.Temp);

    // Method `OnRequestSync` will then get called on host.
    stream.SendMessage($"{Plugin.GUID}_OnRequestConfigSync");
}

internal static void OnRequestSync(ulong clientId, FastBufferReader _) {
    if (!IsHost) return;

    Plugin.Logger.LogDebug($"Config sync request received from client: {clientId}");

    byte[] array = SerializeToBytes(Instance);
    int value = array.Length;

    using FastBufferWriter stream = new(value + IntSize, Allocator.Temp);

    try {
        stream.WriteValueSafe(in value, default);
        stream.WriteBytesSafe(array);

        stream.SendMessage($"{Plugin.GUID}_OnReceiveConfigSync", clientId);
    } catch(Exception e) {
        Plugin.Logger.LogError($"Error occurred syncing config with client: {clientId}\n{e}");
    }
}

internal static void OnReceiveSync(ulong _, FastBufferReader reader) {
    if (!reader.TryBeginRead(IntSize)) {
        Plugin.Logger.LogError("Config sync error: Could not begin reading buffer.");
        return;
    }

    reader.ReadValueSafe(out int val, default);
    if (!reader.TryBeginRead(val)) {
        Plugin.Logger.LogError("Config sync error: Host could not sync.");
        return;
    }

    byte[] data = new byte[val];
    reader.ReadBytesSafe(ref data, val);

    try {
        SyncInstance(data);
    } catch(Exception e) {
        Plugin.Logger.LogError($"Error syncing config instance!\n{e}");
    }
}
```

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
