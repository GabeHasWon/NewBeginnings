using Terraria.ModLoader;

namespace NewBeginnings.Common.Crossmod;

#nullable enable

/// <summary>
/// Solely used to detect if a player is in a subworld.
/// </summary>
internal class SubworldTooling : ModSystem
{
    public static bool InSubworld => HasSubworld && SubworldLibrary!.Call("Current") is not null;

    public static Mod? SubworldLibrary = null;
    public static bool HasSubworld = false;

    public override void PostSetupContent()
    {
        HasSubworld = false;

        if (ModLoader.TryGetMod("SubworldLibrary", out SubworldLibrary))
            HasSubworld = true;
    }
}
