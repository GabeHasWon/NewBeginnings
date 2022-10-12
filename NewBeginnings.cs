using NewBeginnings.Common.Edits;
using NewBeginnings.Common.UnlockabilitySystem;
using NewBeginnings.PlayerBackgrounds;
using Terraria.ModLoader;

namespace NewBeginnings
{
    public class NewBeginnings : Mod
	{
        public override void Load()
        {
            UnlockabilityIO.LoadData();

            CharCreationEdit.Load();
            PlayerBackgroundDatabase.Populate();
        }
    }
}