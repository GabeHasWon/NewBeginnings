using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    /// <summary>
    /// Info struct for a given player background. Contains all necessary information as direct values;<br/>
    /// All non-required values are in the <see cref="EquipData"/>, <see cref="MiscData"/> and <see cref="DelegateData"/> structs.
    /// </summary>
    internal struct PlayerBackgroundData
    {
        /// <summary>Name of the background, for use in UI and for internal keys.</summary>
        public string Name;

        /// <summary>Key used to get the given asset from <see cref="PlayerBackgroundDatabase.backgroundIcons"/>. Defaults to "Default".</summary>
        public string Texture;

        /// <summary>Flavour text of the background in the character creation UI.</summary>
        public string Flavour;

        /// <summary>Description of the background in the character creation UI.</summary>
        public string Description;

        /// <summary>Ordered list of every item type and stack size in the inventory. Is ADDED to, rather than REPLACING, the inventory.</summary>
        public (int type, int stack)[] Inventory;

        /// <summary>Contains all equipment related info, like armor and accessories.</summary>
        public EquipData Equip;

        /// <summary>Contains miscellaneous info, such as starting stats, background UI priority, and base copper set replacements.</summary>
        public MiscData Misc;

        /// <summary>Contains what are functionally hooks, such as modifying worldgen and modifying the player right before they're saved after player creation.</summary>
        public DelegateData Delegates;

        public PlayerBackgroundData(string name, string texName, string flavour, string desc, EquipData? equips, MiscData? misc, params (int, int)[] inv)
        {
            Name = name;
            Texture = texName;
            Flavour = flavour;
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
            player.statLifeMax = Misc.MaxLife > 20 ? Misc.MaxLife : 20;
            player.statManaMax = Misc.AdditionalMana >= 0 ? Misc.AdditionalMana : 0;
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

        /// <summary>Counts the number of items that will be displayed in the character creation UI.</summary>
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
