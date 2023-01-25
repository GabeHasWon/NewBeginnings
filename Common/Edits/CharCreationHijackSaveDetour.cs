using NewBeginnings.Common.PlayerBackgrounds;
using Terraria;
using Terraria.GameContent.UI.States;

namespace NewBeginnings.Common.Edits
{
    internal class CharCreationHijackSaveDetour
    {
        private static bool FirstSave = false;

        public static void Load()
        {
            On.Terraria.GameContent.UI.States.UICharacterCreation.SetupPlayerStatsAndInventoryBasedOnDifficulty += HijackPlayerCreationDefaults;
            On.Terraria.IO.PlayerFileData.CreateAndSave += PlayerFileData_CreateAndSave;
        }

        private static Terraria.IO.PlayerFileData PlayerFileData_CreateAndSave(On.Terraria.IO.PlayerFileData.orig_CreateAndSave orig, Player player)
        {
            if (FirstSave)
            {
                PlayerBackgroundData data = player.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;

                data.ApplyToPlayer(player);
                data.Delegates.ModifyPlayerCreation(player);
            }

            return orig(player);
        }

        private static void HijackPlayerCreationDefaults(On.Terraria.GameContent.UI.States.UICharacterCreation.orig_SetupPlayerStatsAndInventoryBasedOnDifficulty orig, UICharacterCreation self)
        {
            FirstSave = true;
            orig(self);
            FirstSave = false;
        }
    }
}
