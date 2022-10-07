using System;
using Terraria;
using Terraria.ID;

namespace NewBeginnings.PlayerBackgrounds
{
    /// <summary>Info struct for a given player background.</summary>
    internal struct PlayerBackgroundData
    {
        public string Name;
        public string Texture;
        public string Description;
        public int MaxLife;
        public int AdditionalMana;
        public (int head, int body, int legs) Armor = (0, 0, 0);
        public (int type, int stack)[] Inventory;
        public int[] Accessories;

        public PlayerBackgroundData(string name, string texName, string desc, int life = 100, int mana = 20, (int, int, int) armor = default, params (int, int)[] inv)
        {
            Name = name;
            Texture = texName;
            Description = desc;
            MaxLife = life;
            AdditionalMana = mana;
            Armor = armor;
            Inventory = inv;
            Accessories = Array.Empty<int>();
        }

        /// <summary>Constructor with an additional Accessories parameter, for...accessories.</summary>
        /// <param name="accessories">Ordered list of all accessory ItemIDs. Must be less than 6 values long.</param>
        public PlayerBackgroundData(string name, string texName, string desc, int life = 100, int mana = 20, int[] accessories = null, (int, int, int) armor = default, params (int, int)[] inv) 
            : this(name, texName, desc, life, mana, armor, inv)
        {
            Accessories = accessories ?? Array.Empty<int>();
        }

        public void ApplyToPlayer(Player player)
        {
            ApplyStats(player);
            ApplyAccessories(player);
            ApplyArmor(player);
            ApplyInventory(player);
        }

        public void ApplyStats(Player player)
        {
            if (MaxLife > 20)
                player.statLifeMax = MaxLife;

            if (AdditionalMana >= 0)
                player.statManaMax = AdditionalMana;
        }

        public void ApplyAccessories(Player player)
        {
            if (Accessories.Length > Player.InitialAccSlotCount)
                throw new Exception("Inventory is too big. Fix it.");

            for (int i = 0; i < Player.InitialAccSlotCount; ++i)
                player.armor[3 + i] = new Item(i < Accessories.Length ? Accessories[i] : 0);
        }

        public void ApplyArmor(Player player)
        {
            if (Armor.head < 0 || Armor.body < 0 || Armor.legs < 0)
                throw new Exception("Uh oh! Negative armor IDs!");

            player.armor[0] = new Item(Armor.head);
            player.armor[1] = new Item(Armor.body);
            player.armor[2] = new Item(Armor.legs);
        }

        private void ApplyInventory(Player player)
        {
            if (Inventory.Length > player.inventory.Length)
                throw new Exception("Inventory is too big. Fix it.");

            for (int i = 0; i < Inventory.Length; ++i)
            {
                player.inventory[i + 3] = new Item(Inventory[i].type);
                player.inventory[i + 3].stack = Inventory[i].stack;
            }
        }
    }
}
