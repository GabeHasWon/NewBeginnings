using NewBeginnings.PlayerBackgrounds;
using Terraria.ModLoader;

namespace NewBeginnings
{
	public class NewBeginnings : Mod
	{
        public override void Load()
        {
            CharCreationEdit.Load();
            PlayerBackgroundDatabase.Populate();
        }
    }
}