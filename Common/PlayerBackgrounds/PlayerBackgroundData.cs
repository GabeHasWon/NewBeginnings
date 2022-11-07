using System;
using Terraria;
using Terraria.ID;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    /// <summary>Info struct for a given player background.</summary>
    internal struct PlayerBackgroundData
    {
        public string Name;
        public string Texture;
        public string Description;
        public (int type, int stack)[] Inventory;
        public EquipData Equip;
        public MiscData Misc;
        public DelegateData Delegates;

        public PlayerBackgroundData(string name, string texName, string desc, EquipData? equips, MiscData? misc, params (int, int)[] inv)
        {
            Name = name;
            Texture = texName;
            Description = desc;
            Inventory = inv;

            Equip = equips ?? new EquipData(0, 0, 0);
            Misc = misc ?? new MiscData(100, 20, -1, -1, -1);
            Delegates = new();
        }

        public void ApplyToPlayer(Player player)
        {
            ApplyStats(player);
            ApplyAccessories(player);
            ApplyArmor(player);
            ApplyInventory(player);
            ApplyItemReplacements(player);
        }

        private void ApplyItemReplacements(Player player)
        {
            if (Misc.CopperShortswordReplacement != -1)
                player.inventory[0] = new Item(Misc.CopperShortswordReplacement);

            if (Misc.CopperPickaxeReplacement != -1)
                player.inventory[1] = new Item(Misc.CopperPickaxeReplacement);

            if (Misc.CopperAxeReplacement != -1)
                player.inventory[2] = new Item(Misc.CopperAxeReplacement);
        }

        public void ApplyStats(Player player)
        {
            if (Misc.MaxLife > 20)
                player.statLifeMax = Misc.MaxLife;

            if (Misc.AdditionalMana >= 0)
                player.statManaMax = Misc.AdditionalMana;
        }

        public void ApplyAccessories(Player player)
        {
            if (Equip.Accessories.Length > Player.InitialAccSlotCount)
                throw new Exception("Inventory is too big. Fix it.");

            for (int i = 0; i < Player.InitialAccSlotCount; ++i)
                player.armor[3 + i] = new Item(i < Equip.Accessories.Length ? Equip.Accessories[i] : 0);
        }

        public void ApplyArmor(Player player)
        {
            if (Equip.Head < 0 || Equip.Body < 0 || Equip.Legs < 0)
                throw new Exception("Uh oh! Negative armor IDs!");

            player.armor[0] = new Item(Equip.Head);
            player.armor[1] = new Item(Equip.Body);
            player.armor[2] = new Item(Equip.Legs);
        }

        private void ApplyInventory(Player player)
        {
            if (Inventory.Length > player.inventory.Length)
                throw new Exception("Inventory is too big. Fix it.");

            int offset = player.creativeGodMode ? 10 : 3;

            for (int i = 0; i < Inventory.Length; ++i)
            {
                player.inventory[i + offset] = new Item(Inventory[i].type);
                player.inventory[i + offset].stack = Inventory[i].stack;
            }
        }

        public int DisplayItemCount()
        {
            int total = Inventory.Length + Equip.Accessories.Length;

            if (Misc.CopperShortswordReplacement != -1) 
                total++;

            if (Misc.CopperPickaxeReplacement != -1)
                total++;

            if (Misc.CopperAxeReplacement != -1)
                total++;

            return total;
        }
    }
}
