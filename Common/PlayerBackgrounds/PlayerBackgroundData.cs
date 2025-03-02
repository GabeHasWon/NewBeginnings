using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace NewBeginnings.Common.PlayerBackgrounds;

/// <summary>
/// Info struct for a given player background. Contains all necessary information as direct values;<br/>
/// All non-required values are in the <see cref="EquipData"/>, <see cref="MiscData"/> and <see cref="DelegateData"/> structs.
/// </summary>
internal struct PlayerBackgroundData(string langKey, string identifier, EquipData? equips, MiscData? misc, params (int, int)[] inv)
{
    /// <summary>Name of the background, for use in UI and for internal keys.</summary>
    public LocalizedText Name = Language.GetText(langKey + ".DisplayName");

    /// <summary>Key used to get the given asset from <see cref="PlayerBackgroundDatabase.backgroundIcons"/>, and is used as an internal name. 
    /// Defaults to "Default".</summary>
    public string Identifier = identifier;

    /// <summary>Flavour text of the background in the character creation UI.</summary>
    public LocalizedText Flavour = Language.GetText(langKey + ".Flavor");

    /// <summary>Description of the background in the character creation UI.</summary>
    public LocalizedText Description = Language.GetText(langKey + ".Description");

    /// <summary>Ordered list of every item type and stack size in the inventory. Is ADDED to, rather than REPLACING, the inventory.</summary>
    public (int type, int stack)[] Inventory = inv ?? Array.Empty<(int, int)>();

    /// <summary>Contains all equipment related info, like armor and accessories.</summary>
    public EquipData Equip = equips ?? new EquipData(0, 0, 0);

    /// <summary>Contains miscellaneous info, such as starting stats, background UI priority, and base copper set replacements.</summary>
    public MiscData Misc = misc ?? new MiscData(100, 20, -1, -1, -1);

    /// <summary>Contains what are functionally hooks, such as modifying worldgen and modifying the player right before they're saved after player creation.</summary>
    public DelegateData Delegates = new();

    /// <summary>
    /// Applies changes to the player.<br/>
    /// Note that health changes are applied in <see cref="PlayerBackgroundPlayer.ModifyMaxStats"/> now.<br/>
    /// The <see cref="Custom"/>
    /// </summary>
    /// <param name="player"></param>
    public readonly void ApplyToPlayer(Player player)
    {
        player.ConsumedManaCrystals = Misc.AdditionalMana / 20 - 1;

        ApplyAccessories(player);
        ApplyArmor(player);
        ApplyItemReplacements(player);
        ApplyInventory(player);
    }

    private readonly void ApplyItemReplacements(Player player)
    {
        if (Misc.CopperShortswordReplacement != -1)
            player.inventory[0] = new Item(Misc.CopperShortswordReplacement);

        if (Misc.CopperPickaxeReplacement != -1)
            player.inventory[1] = new Item(Misc.CopperPickaxeReplacement);

        if (Misc.CopperAxeReplacement != -1)
            player.inventory[2] = new Item(Misc.CopperAxeReplacement);
    }

    public readonly void ApplyAccessories(Player player, bool forced = false)
    {
        if (Equip.Accessories is null)
            return;

        if (Equip.Accessories.Length > Player.InitialAccSlotCount)
            throw new Exception("Accessories is too big. Fix it.");

        if (!forced)
        {
            if (Equip.Accessories.Length == 0)
                return;

            int offset = player.difficulty == PlayerDifficultyID.Creative ? 1 : 0;

            for (int i = 0; i < Player.InitialAccSlotCount; ++i)
            {
                if (i >= Equip.Accessories.Length || i + 3 >= 8)
                    break;

                player.armor[3 + i + offset] = new Item(Equip.Accessories[i]);
            }
        }
        else
            for (int i = 0; i < Player.InitialAccSlotCount; ++i)
                player.armor[3 + i] = new Item(i < Equip.Accessories.Length ? Equip.Accessories[i] : 0);
    }

    public readonly void ApplyArmor(Player player)
    {
        if (Equip.Head < 0 || Equip.Body < 0 || Equip.Legs < 0)
            throw new Exception("Uh oh! Negative armor IDs!");

        player.armor[0] = new Item(Equip.Head);
        player.armor[1] = new Item(Equip.Body);
        player.armor[2] = new Item(Equip.Legs);
    }

    private readonly void ApplyInventory(Player player)
    {
        if (Inventory is null || Inventory.Length == 0)
            return;

        if (Inventory.Length > player.inventory.Length)
            throw new Exception("Inventory is too big. Fix it.");

        int offset = 0;

        for (int i = 0; i < Inventory.Length; ++i)
        {
            if (!player.inventory[offset].IsAir) // Skip current item if it's occupied by (presumably) another mod's items beforehand
            {
                i--;
                offset++;
                continue;
            }

            player.inventory[offset] = new Item(Inventory[i].type)
            {
                stack = Inventory[i].stack
            };

            offset++;
        }
    }

    /// <summary>Counts the number of items that will be displayed in the character creation UI.</summary>
    public readonly int DisplayItemCount()
    {
        int total = Inventory.Length + Equip.Accessories.Length;

        if (Misc.CopperShortswordReplacement != -1) 
            total++;

        if (Misc.CopperPickaxeReplacement != -1)
            total++;

        if (Misc.CopperAxeReplacement != -1)
            total++;

        if (Equip.Head != ItemID.None)
            total++;

        if (Equip.Body != ItemID.None)
            total++;

        if (Equip.Legs != ItemID.None)
            total++;

        return total;
    }
}
