using NewBeginnings.Common.Crossmod;
using NewBeginnings.Common.Edits;
using NewBeginnings.Common.PlayerBackgrounds;
using NewBeginnings.Common.UnlockabilitySystem;
using Terraria.ModLoader;
using Terraria.UI;

namespace NewBeginnings;

public class NewBeginnings : Mod
{
    public UIState UnlockUI;

    public override void Load()
    {
        UnlockabilityIO.LoadData();
        CharCreationEdit.Load();
    }

    public override void PostSetupContent()
    {
        MrPlaguesCompat.PostSetupContent();
        PlayerBackgroundDatabase.Populate();
    }

    public override void Unload() => MrPlaguesCompat.Unload();

    public override object Call(params object[] args) => OriginCalls.Call(args);
}