using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    /// <summary>Handles player-related background functions (i.e. inventory).</summary>
    internal class PlayerBackgroundPlayer : ModPlayer
    {
        public PlayerBackgroundData BackgroundData = new PlayerBackgroundData();

        private string _bgName = "";

        //BG specific stuff
        public void SetBackground(PlayerBackgroundData data)
        {
            BackgroundData = data;
            _bgName = data.Name;
        }

        //Save / Load data for the player's origin name
        public override void SaveData(TagCompound tag) => tag.Add("bgName", _bgName);

        public override void LoadData(TagCompound tag)
        {
            _bgName = tag.GetString("bgName");

            BackgroundData = PlayerBackgroundDatabase.playerBackgroundDatas.FirstOrDefault(x => x.Name == _bgName);
        }

        public bool HasBG() => _bgName != "";

        //Misc tMod Hooks
        public override void OnEnterWorld(Player player)
        {
            if (BackgroundData.Name != "Purist")
                UnlockabilitySystem.UnlockSaveData.Complete("Beginner");
        }
    }
}
