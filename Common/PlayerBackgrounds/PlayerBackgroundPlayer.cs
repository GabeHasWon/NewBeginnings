using Microsoft.Xna.Framework;
using System.Linq;
using System.Numerics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

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
            _bgName = data.Identifier;
        }

        //Save / Load data for the player's origin name
        public override void SaveData(TagCompound tag) => tag.Add("bgName", _bgName);

        public override void LoadData(TagCompound tag)
        {
            _bgName = tag.GetString("bgName");

            if (PlayerBackgroundDatabase.playerBackgroundDatas.Any(x => x.Identifier == _bgName))
                BackgroundData = PlayerBackgroundDatabase.playerBackgroundDatas.FirstOrDefault(x => x.Identifier == _bgName);
            else if (_bgName is not null && _bgName != string.Empty)
                BackgroundData = new PlayerBackgroundData(_bgName, "Unloaded", null, null);
        }

        public bool HasBG() => _bgName != "" && _bgName != null;
        public bool HasBG(string name) => HasBG() && _bgName == name;

        //Misc tMod Hooks
        public override void OnEnterWorld()
        {
            if (HasBG() && BackgroundData.Identifier != "Purist") //Unlock Beginner/Alternate if the player has an origin
                UnlockabilitySystem.UnlockSaveData.Complete("Beginner");
        }

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;

            if (HasBG()) //This adjusts health to be according to the origin.
            {
                int maxLife = BackgroundData.Misc.MaxLife;
                var scaling = ModContent.GetInstance<NewBeginningsConfig>().HealthScaling;

                if (scaling == NewBeginningsConfig.Scaling.Scaled)
                {
                    float factor = 1 - (Player.ConsumedLifeCrystals / (float)Player.LifeCrystalMax);
                    float modifier = (int)((100 - maxLife) * factor);
                    health.Base -= modifier;
                }
                else if (scaling == NewBeginningsConfig.Scaling.Relative)
                {
                    float factor = Player.ConsumedLifeCrystals / (float)Player.LifeCrystalMax;
                    health.Base = MathHelper.Lerp(maxLife - 100, (maxLife * 4) - 400, factor);
                }
                else
                    health.Base -= 100 - maxLife;
            }
        }
    }
}
