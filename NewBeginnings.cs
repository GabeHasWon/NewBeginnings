using NewBeginnings.Common.Edits;
using NewBeginnings.Common.PlayerBackgrounds;
using NewBeginnings.Common.UnlockabilitySystem;
using Terraria.ModLoader;
using Terraria.UI;

namespace NewBeginnings
{
    public class NewBeginnings : Mod
	{
        public UIState UnlockUI;

        public override void Load()
        {
            UnlockabilityIO.LoadData();

            CharCreationEdit.Load();
            PlayerBackgroundDatabase.Populate();
        }

        public override void PostSetupContent() => MrPlaguesCompat.PostSetupContent();
        public override void Unload() => MrPlaguesCompat.Unload();

        internal static void PrintBGDescriptions()
        {
            string log = "";
            foreach (var item in PlayerBackgroundDatabase.playerBackgroundDatas)
                log += item.Name + ": " + item.Flavour + "\n";

            ModLoader.GetMod("NewBeginnings").Logger.Debug(log);
        }
    }
}