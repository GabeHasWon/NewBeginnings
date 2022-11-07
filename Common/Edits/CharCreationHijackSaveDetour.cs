using NewBeginnings.Common.PlayerBackgrounds;
using Terraria;
using Terraria.GameContent.UI.States;

namespace NewBeginnings.Common.Edits
{
    internal class CharCreationHijackSaveDetour
    {
        public static void Load()
        {
            On.Terraria.GameContent.UI.States.UICharacterCreation.SetupPlayerStatsAndInventoryBasedOnDifficulty += HijackPlayerCreationDefaults;
        }

        private static void HijackPlayerCreationDefaults(On.Terraria.GameContent.UI.States.UICharacterCreation.orig_SetupPlayerStatsAndInventoryBasedOnDifficulty orig, UICharacterCreation self)
        {
            orig(self);

            Player p = CharCreationEdit.InternalPlayerField.GetValue(self) as Player;
            PlayerBackgroundData data = p.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;

            data.ApplyToPlayer(p);
            data.Delegates.ModifyPlayerCreation(p);            
        }
    }
}
