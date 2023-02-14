using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.PlayerBackgrounds;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NewBeginnings.Common.Crossmod;

internal class OriginCalls
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

        return null;
    }

    /// <summary>
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
    /// AddOrigin(Asset[Texture2D]? asset, string texName, string name, string flavour, string description, (int, int)[] inventory)<br/>
    /// AddOrigin(Asset[Texture2D]? asset, string texName, string name, string flavour, string description, (int, int)[] inventory, int head, int chest, int legs)<br/>
    /// AddOrigin(Asset[Texture2D]? asset, string texName, string name, string flavour, string description, (int, int)[] inventory, int head, int chest, int legs, int[] accessories)
    /// AddOrigin(Asset[Texture2D]? asset, string texName, string name, string flavour, string description, (int, int)[] inventory, int head, int chest, int legs, int[] accessories, int life, int mana)
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

        if (objects.Length < 5)
            return ThrowOrReturn("Not enough parameters!");

        if (objects[0] is not Asset<Texture2D> asset)
            return ThrowOrReturn("objects[0] is not an Asset<Texture2D>!");

        if (objects[1] is not string texName)
            return ThrowOrReturn("objects[1] is not a string!");

        TryRegisterTexture(asset, texName);

        if (objects[2] is not string name)
            return ThrowOrReturn("objects[2] is not a string!");

        if (objects[3] is not string flavour)
            return ThrowOrReturn("objects[3] is not a string!");

        if (objects[4] is not string description)
            return ThrowOrReturn("objects[4] is not a string!");

        if (objects[5] is not (int, int)[] inventory)
            return ThrowOrReturn("objects[5] is not an (int, int)[]!");

        if (objects.Length == 6)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, null, null, inventory));
            return true;
        }

        if (!CastInt(objects[6], out int head))
            return ThrowOrReturn("objects[6] is not an int!");

        if (!CastInt(objects[7], out int chest))
            return ThrowOrReturn("objects[7] is not an int!");

        if (!CastInt(objects[8], out int legs))
            return ThrowOrReturn("objects[8] is not an int!");

        if (objects.Length == 9 || objects[9] is not int[] accessories)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs), null, inventory));
            return true;
        }

        if (objects.Length == 10)
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), null, inventory));
            return true;
        }

        if (!CastInt(objects[10], out int life))
            return ThrowOrReturn("objects[10] is not an int or short!");

        if (objects.Length == 11 || !CastInt(objects[11], out int mana))
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), new MiscData(life), inventory));
            return true;
        }

        if (objects.Length == 12 || !CastInt(objects[12], out int npcType))
        {
            _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), new MiscData(life, mana), inventory));
            return true;
        }

        _crossModDatas.Add(new PlayerBackgroundData(name, texName, flavour, description, new EquipData(head, chest, legs, accessories), new MiscData(life, mana, npcType), inventory));
        return true;
    }

    private static bool ThrowOrReturn(string message)
    {
#if DEBUG
        throw new Exception(message);
#endif
        return false;
    }

    private static bool CastInt(object val, out int value)
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
