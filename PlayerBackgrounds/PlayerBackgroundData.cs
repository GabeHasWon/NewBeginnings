using System;
using Terraria;
using Terraria.ID;

namespace NewBeginnings.PlayerBackgrounds
{
    internal struct PlayerBackgroundData
    {
        public string Name;
        public string Texture;
        public string Description;
        public int MaxLife;
        public int AdditionalMana;
        public (int head, int body, int legs) Armor = (0, 0, 0);
        public (int type, int stack)[] Inventory;

        public PlayerBackgroundData(string name, string texName, string desc, int life = 200, int mana = 20, (int, int, int) armor = default, params (int, int)[] inv)
        {
            Name = name;
            Texture = texName;
            Description = desc;
            MaxLife = life;
            AdditionalMana = mana;
            Armor = armor;
            Inventory = inv;
        }

        public void ApplyToPlayer(Player player)
        {
            player.statLifeMax = MaxLife;
            player.statManaMax = AdditionalMana;

            ApplyArmor(player);
            ApplyInventory(player);
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
                player.inventory[i] = new Item(Inventory[i].type);
                player.inventory[i].stack = Inventory[i].stack;
            }
        }
    }
}
