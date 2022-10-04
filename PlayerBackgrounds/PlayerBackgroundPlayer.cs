using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NewBeginnings.PlayerBackgrounds
{
    /// <summary>Handles player-related background functions (i.e. inventory).</summary>
    internal class PlayerBackgroundPlayer : ModPlayer
    {
        public PlayerBackgroundData BackgroundData = new PlayerBackgroundData();

        private string _bgName = "";

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            var items = new List<Item>();

            if (mediumCoreDeath || BackgroundData.Inventory is null)
                return items;

            foreach (var (type, stack) in BackgroundData.Inventory)
            {
                Item newItem = new(type)
                {
                    stack = stack
                };
                items.Add(newItem);
            }

            return items;
        }

        public void SetBackground(PlayerBackgroundData data)
        {
            BackgroundData = data;
            _bgName = data.Name;
        }

        //Save / Load data for if the player is just created
        public override void SaveData(TagCompound tag)
        {
            tag.Add("bgName", _bgName);
        }

        public override void LoadData(TagCompound tag)
        {
            _bgName = tag.GetString("bgName");

            BackgroundData = PlayerBackgroundDatabase.playerBackgroundDatas.FirstOrDefault(x => x.Name == _bgName);
        }
    }
}
