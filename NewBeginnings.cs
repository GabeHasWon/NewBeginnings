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
    }
}