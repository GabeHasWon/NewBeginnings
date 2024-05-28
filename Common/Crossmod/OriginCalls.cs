using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.PlayerBackgrounds;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Terraria;
using Terraria.ModLoader;

namespace NewBeginnings.Common.Crossmod;

internal static class OriginCalls
{
    internal static List<PlayerBackgroundData> _crossModDatas = new();

    internal static object Call(object[] args)
    {
        if (args[0] is not string type)
            return null;

        type = type.ToLower();

        if (type == "addorigin")
            return AddOrigin(args[1..]);
        else if (type == "setoriginicon")
            return SetOriginIcon(args[1..]);
        else if (type == "equipdata")
            return StructBuilders.MakeEquipData(args[1..]);
        else if (type == "miscdata")
            return StructBuilders.MakeMiscData(args[1..]);
        else if (type == "delegatedata")
            return StructBuilders.MakeDelegateData(args[1..]);
        else if (type == "shortaddorigin")
            return AddShortOrigin(args[1..]);
        else if (type == "playerhasorigin")
            return PlayerHasOrigin(args[1..]);
        else if (type == "removeorigin")
            return RemoveOrigin(args[1..]);

        return null;
    }

    internal static object RemoveOrigin(object[] objects)
    {
        if (objects[0] is not string id)
            return ThrowOrReturn("objects[0] is not a string!");

        if (objects[1] is not Mod mod)
            return ThrowOrReturn("objects[1] is not a Mod!");

        if (_crossModDatas.Any(x => x.Identifier == id))
        {
            _crossModDatas.Remove(_crossModDatas.First(x => x.Identifier == id));
            ModContent.GetInstance<NewBeginnings>().Logger.Warn($"{mod.DisplayNameClean} removed \"{id}\" cross mod origin from New Beginnings!");
        }
        else if (PlayerBackgroundDatabase.playerBackgroundDatas.Any(x => x.Identifier == id))
        {
            PlayerBackgroundDatabase.playerBackgroundDatas.Remove(PlayerBackgroundDatabase.playerBackgroundDatas.First(x => x.Identifier == id));
            ModContent.GetInstance<NewBeginnings>().Logger.Warn($"{mod.DisplayNameClean} removed \"{id}\" in-house origin from New Beginnings!");
        }
        else
        {
            ModContent.GetInstance<NewBeginnings>().Logger.Info($"Identifier {id} does not correspond to any existing origin.");
            return false;
        }

        return true;
    }

    private static bool PlayerHasOrigin(object[] objects)
    {
        if (objects[0] is not Player player)
            return ThrowOrReturn("objects[0] is not Player!");

        if (objects[1] is not string bgName)
            return ThrowOrReturn("objects[1] is not string!");

        return player.GetModPlayer<PlayerBackgroundPlayer>().HasBG(bgName);
    }

    private static object AddShortOrigin(object[] objects)
    {
        AddBasicOrigin(objects, out var _, out var texName, out var name, out var inventory);

        if (objects[4] is not EquipData equip)
            return ThrowOrReturn("objects[4] (equip) is not EquipData!");

        if (objects.Length == 5)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, equip, null, inventory));
            return true;
        }

        if (objects[5] is not MiscData misc)
            return ThrowOrReturn("objects[5] (misc) is not MiscData!");

        if (objects.Length == 6)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, equip, misc, inventory));
            return true;
        }

        if (objects[6] is not DelegateData dele)
            return ThrowOrReturn("objects[6] (misc) is not DelegateData!");

        if (objects.Length == 7)
        {
            var bg = new PlayerBackgroundData(name, texName, equip, misc, inventory)
            {
                Delegates = dele
            };
            _crossModDatas.Add(bg);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Allows other mods to set a given origin icon, or register a new one.<br/><br/>
    /// Overloads:<br/>
    /// SetOriginIcon(Asset[Texture2D] asset, string texName)
    /// </summary>
    private static object SetOriginIcon(object[] objects)
    {
        if (objects[0] is not Asset<Texture2D> asset)
            return false;

        if (objects[1] is not string texName)
            return false;

        TryRegisterTexture(asset, texName, true);
        return true;
    }

    /// <summary>
    /// Allows other mods to add in their own PlayerBackgroundData.<br/><br/>
    /// Overloads: <br/>
    /// AddOrigin(PlayerBackgroundContainer container)<br/><br/>
    /// AddOrigin(PlayerBackgroundData data)<br/><br/>
    /// AddOrigin(Asset[Texture2D] asset, string texName, string name, string flavour, string description, (int, int)[] inventory)<br/><br/>
    /// AddOrigin(Asset[Texture2D] asset, string texName, string name, string flavour, string description, (int, int)[] inventory, int head, int chest, int legs)<br/><br/>
    /// AddOrigin(Asset[Texture2D] asset, string texName, string name, string flavour, string description, (int, int)[] inventory, int head, int chest, int legs, int[] accessories)<br/><br/>
    /// AddOrigin(Asset[Texture2D] asset, string texName, string name, string flavour, string description, (int, int)[] inventory, int head, int chest, int legs, int[] accessories,
    /// int life, int mana = 20, int npcType = -1, int swordType = -1, int pickType = -1, int axeType = -1, int sortPriority = 10, int stars = 3)<br/><br/>
    /// </summary>
    private static bool AddOrigin(object[] objects)
    {
        if (objects[0] is PlayerBackgroundContainer)
        {
            _crossModDatas.Add(objects[0] as PlayerBackgroundContainer);
            return true;
        }

        if (objects[0] is PlayerBackgroundData data)
        {
            _crossModDatas.Add(data);
            return true;
        }

        AddBasicOrigin(objects, out var _, out var texName, out var name, out var inventory);

        if (!CastInt(objects[4], out int head))
            return ThrowOrReturn("objects[4] (head) is not an int!");

        if (!CastInt(objects[5], out int chest))
            return ThrowOrReturn("objects[5] (chest) is not an int!");

        if (!CastInt(objects[6], out int legs))
            return ThrowOrReturn("objects[6] (legs) is not an int!");

        if (objects.Length == 7)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs), null, inventory));
            return true;
        }

        if (objects[7] is not int[] accessories)
            return ThrowOrReturn("objects[7] (accessories) is not an int[]!");

        if (objects.Length == 8)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), null, inventory));
            return true;
        }

        if (!CastInt(objects[8], out int life))
            return ThrowOrReturn("objects[8] (life) is not an int or short!");

        if (objects.Length == 9)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life), inventory));
            return true;
        }

        if (!CastInt(objects[9], out int mana))
            return ThrowOrReturn("objects[9] (mana) is not an int or short!");

        if (objects.Length == 10)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life, mana), inventory));
            return true;
        }

        if (!CastInt(objects[10], out int npcType))
            return ThrowOrReturn("objects[10] (npcType) is not an int or short!");

        if (objects.Length == 11)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType), inventory));
            return true;
        }

        if (!CastInt(objects[11], out int swordType))
            return ThrowOrReturn("objects[11] (swordType) is not an int or short!");

        if (objects.Length == 12)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType, swordType), inventory));
            return true;
        }

        if (!CastInt(objects[12], out int pickType))
            return ThrowOrReturn("objects[12] (pickType) is not an int or short!");

        if (objects.Length == 13)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType, swordType, pickType), inventory));
            return true;
        }

        if (!CastInt(objects[13], out int axeType))
            return ThrowOrReturn("objects[13] (axeType) is not an int or short!");

        if (objects.Length == 14)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType, swordType, pickType, axeType), inventory));
            return true;
        }

        if (!CastInt(objects[14], out int sortPriority))
            return ThrowOrReturn("objects[14] (sortPriorty) is not an int or short!");

        if (objects.Length == 15)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType, swordType, pickType, axeType, sortPriority), inventory));
            return true;
        }

        if (!CastInt(objects[15], out int stars))
            return ThrowOrReturn("objects[15] (stars) is not an int or short!");

        _crossModDatas.Add(new PlayerBackgroundData(name, texName, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType, swordType, pickType, axeType, sortPriority, stars), inventory));
        return true;
    }

    private static bool AddBasicOrigin(object[] objects, out Asset<Texture2D> asset, out string identifier, out string langKey, out (int, int)[] inventory)
    {
        identifier = string.Empty;
        asset = null;
        langKey = string.Empty;
        inventory = Array.Empty<(int, int)>();

        if (objects.Length < 5)
            return ThrowOrReturn("Not enough parameters!");

        if (objects[0] is not Asset<Texture2D>)
            return ThrowOrReturn("objects[0] (asset) is not an Asset<Texture2D>!");
        asset = objects[0] as Asset<Texture2D>;

        if (objects[1] is not string)
            return ThrowOrReturn("objects[1] (identifier) is not a string!");
        identifier = objects[1] as string;

        TryRegisterTexture(asset, identifier);

        if (objects[2] is not string)
            return ThrowOrReturn("objects[2] (langKey) is not a string!");
        langKey = objects[2] as string;

        if (objects[3] is not (int, int)[])
            return ThrowOrReturn("objects[3] (langKey) is not an (int, int)[]!");
        inventory = objects[3] as (int, int)[];

        if (objects.Length == 4)
        {
            _crossModDatas.Add(new PlayerBackgroundData(langKey, identifier, null, null, inventory));
            return true;
        }

        return true;
    }

    internal static bool ThrowOrReturn(string message)
    {
#if DEBUG
        throw new Exception(message);
#else
        return false;
#endif
    }

    internal static bool CastInt(object val, out int value)
    {
        if (val is int ret)
        {
            value = ret;
            return true;
        }

        if (val is short retShort)
        {
            value = retShort;
            return true;
        }

        value = 0;
        return false;
    }

    private static void TryRegisterTexture(Asset<Texture2D> asset, string texName, bool forced = false)
    {
        if (asset is null)
            return;

        if (!PlayerBackgroundDatabase.backgroundIcons.ContainsKey(texName))
            PlayerBackgroundDatabase.backgroundIcons.Add(texName, asset);
        else if (forced)
            PlayerBackgroundDatabase.backgroundIcons[texName] = asset;
    }
}
