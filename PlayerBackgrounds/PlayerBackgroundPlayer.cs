using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace NewBeginnings.PlayerBackgrounds
{
    internal class PlayerBackgroundPlayer : ModPlayer
    {
        public PlayerBackgroundData BackgroundData = new PlayerBackgroundData();

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
    }
}
