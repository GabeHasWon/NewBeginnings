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

        internal static void PrintBGDescriptions()
        {
            string log = "";
            foreach (var item in PlayerBackgroundDatabase.playerBackgroundDatas)
                log += item.Name + ": " + item.Description + "\n";

            ModLoader.GetMod("NewBeginnings").Logger.Debug(log);
        }
    }
}