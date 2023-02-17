using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.PlayerBackgrounds;
using ReLogic.Content;
using System;
using System.Collections.Generic;

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

        return null;
    }

    private static object AddShortOrigin(object[] objects)
    {
        AddBasicOrigin(objects, out var _, out var texName, out var name, out var flavour, out var description, out var inventory);

        if (objects[6] is not EquipData equip)
            return ThrowOrReturn("objects[6] (equip) is not EquipData!");

        if (objects.Length == 7)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, equip, null, inventory));
            return true;
        }

        if (objects[7] is not MiscData misc)
            return ThrowOrReturn("objects[7] (misc) is not MiscData!");

        if (objects.Length == 8)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, equip, misc, inventory));
            return true;
        }

        if (objects[8] is not DelegateData dele)
            return ThrowOrReturn("objects[7] (misc) is not MiscData!");

        if (objects.Length == 7)
        {
            var bg = new PlayerBackgroundData(name, texName, flavour, description, equip, misc, inventory)
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
    /// int life, [optional] int mana, [optional] int npcType, [optional] int swordType, [optional] int pickType, [optional] int axeType, [optional] int sortPriority, [optional] int stars)<br/><br/>
    /// </summary>
    private static bool AddOrigin(object[] objects)
    {
        if (objects[0] is PlayerBackgroundContainer)
        {
            _crossModDatas.Add(objects[0] as PlayerBackgroundContainer);
            return true;
        }

        if (objects[0] is PlayerBackgroundData)
        {
            _crossModDatas.Add((PlayerBackgroundData)objects[0]);
            return true;
        }

        AddBasicOrigin(objects, out var asset, out var texName, out var name, out var flavour, out var description, out var inventory);

        if (!CastInt(objects[6], out int head))
            return ThrowOrReturn("objects[6] (head) is not an int!");

        if (!CastInt(objects[7], out int chest))
            return ThrowOrReturn("objects[7] (chest) is not an int!");

        if (!CastInt(objects[8], out int legs))
            return ThrowOrReturn("objects[8] (legs) is not an int!");

        if (objects.Length == 9)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs), null, inventory));
            return true;
        }

        if (objects[9] is not int[] accessories)
            return ThrowOrReturn("objects[9] (accessories) is not an int!");

        if (objects.Length == 10)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), null, inventory));
            return true;
        }

        if (!CastInt(objects[10], out int life))
            return ThrowOrReturn("objects[10] (life) is not an int or short!");

        if (objects.Length == 11)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), new MiscData(life), inventory));
            return true;
        }

        if (!CastInt(objects[11], out int mana))
            return ThrowOrReturn("objects[11] (mana) is not an int or short!");

        if (objects.Length == 12)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), new MiscData(life, mana), inventory));
            return true;
        }

        if (!CastInt(objects[12], out int npcType))
            return ThrowOrReturn("objects[12] (npcType) is not an int or short!");

        if (objects.Length == 13)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType), inventory));
            return true;
        }

        if (!CastInt(objects[13], out int swordType))
            return ThrowOrReturn("objects[13] (swordType) is not an int or short!");

        if (objects.Length == 14)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories),
                new MiscData(life, mana, npcType, swordType), inventory));
            return true;
        }

        if (!CastInt(objects[14], out int pickType))
            return ThrowOrReturn("objects[14] (pickType) is not an int or short!");

        if (objects.Length == 15)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), 
                new MiscData(life, mana, npcType, swordType, pickType), inventory));
            return true;
        }

        if (!CastInt(objects[15], out int axeType))
            return ThrowOrReturn("objects[15] (axeType) is not an int or short!");

        if (objects.Length == 16)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), 
                new MiscData(life, mana, npcType, swordType, pickType, axeType), inventory));
            return true;
        }

        if (!CastInt(objects[16], out int sortPriority))
            return ThrowOrReturn("objects[16] (sortPriorty) is not an int or short!");

        if (objects.Length == 17)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), 
                new MiscData(life, mana, npcType, swordType, pickType, axeType, sortPriority), inventory));
            return true;
        }

        if (!CastInt(objects[17], out int stars))
            return ThrowOrReturn("objects[17] (stars) is not an int or short!");

        _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), 
            new MiscData(life, mana, npcType, swordType, pickType, axeType, sortPriority, stars), inventory));
        return true;
    }

    private static bool AddBasicOrigin(object[] objects, out Asset<Texture2D> asset, out string texName, out string name, out string flavour, out string description, out (int, int)[] inventory)
    {
        texName = string.Empty;
        asset = null;
        name = string.Empty;
        flavour = string.Empty;
        description = string.Empty;
        inventory = Array.Empty<(int, int)>();

        if (objects.Length < 5)
            return ThrowOrReturn("Not enough parameters!");

        if (objects[0] is not Asset<Texture2D>)
            return ThrowOrReturn("objects[0] (asset) is not an Asset<Texture2D>!");
        asset = objects[0] as Asset<Texture2D>;

        if (objects[1] is not string)
            return ThrowOrReturn("objects[1] is not a string!");
        texName = objects[1] as string;

        TryRegisterTexture(asset, texName);

        if (objects[2] is not string)
            return ThrowOrReturn("objects[2] is not a string!");
        name = objects[2] as string;

        if (objects[3] is not string)
            return ThrowOrReturn("objects[3] is not a string!");
        flavour = objects[3] as string;

        if (objects[4] is not string)
            return ThrowOrReturn("objects[4] is not a string!");
        description = objects[4] as string;

        if (objects[5] is not (int, int)[])
            return ThrowOrReturn("objects[5] is not an (int, int)[]!");
        inventory = objects[5] as (int, int)[];

        if (objects.Length == 6)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, null, null, inventory));
            return true;
        }
        return true;
    }

    internal static bool ThrowOrReturn(string message)
    {
#if DEBUG
        throw new Exception(message);
#endif
        return false;
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
