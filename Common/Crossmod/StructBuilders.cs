using NewBeginnings.Common.PlayerBackgrounds;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.Crossmod;

internal class StructBuilders
{
    internal static object MakeEquipData(object[] objects)
    {
        if (!OriginCalls.CastInt(objects[0], out int head))
            return OriginCalls.ThrowOrReturn("objects[0] (head) is not an int!");

        if (!OriginCalls.CastInt(objects[1], out int chest))
            return OriginCalls.ThrowOrReturn("objects[1] (chest) is not an int!");

        if (!OriginCalls.CastInt(objects[2], out int legs))
            return OriginCalls.ThrowOrReturn("objects[2] (legs) is not an int!");

        if (objects.Length == 3)
            return new EquipData(head, chest, legs);

        if (objects[3] is not int[] accessories)
            return OriginCalls.ThrowOrReturn("objects[3] (accessories) is not an int[]!");
        return new EquipData(head, chest, legs, accessories);
    }

    /// int life, [optional] int mana, [optional] int npcType, [optional] int swordType, [optional] int pickType, [optional] int axeType, [optional] int sortPriority, [optional] int stars)<br/><br/>
    internal static object MakeMiscData(object[] objects)
    {
        if (!OriginCalls.CastInt(objects[0], out int life))
            return OriginCalls.ThrowOrReturn("objects[0] (life) is not an int!");

        if (objects.Length == 1)
            return new MiscData(life);

        if (!OriginCalls.CastInt(objects[1], out int mana))
            return OriginCalls.ThrowOrReturn("objects[1] (mana) is not an int!");

        if (objects.Length == 2)
            return new MiscData(life, mana);

        if (!OriginCalls.CastInt(objects[2], out int npcType))
            return OriginCalls.ThrowOrReturn("objects[2] (npcType) is not an int!");

        if (objects.Length == 3)
            return new MiscData(life, mana, npcType);

        if (!OriginCalls.CastInt(objects[3], out int swordType))
            return OriginCalls.ThrowOrReturn("objects[2] (swordType) is not an int!");

        if (objects.Length == 4)
            return new MiscData(life, mana, npcType, swordType);

        if (!OriginCalls.CastInt(objects[4], out int pickType))
            return OriginCalls.ThrowOrReturn("objects[2] (pickType) is not an int!");

        if (objects.Length == 5)
            return new MiscData(life, mana, npcType, swordType, pickType);

        if (!OriginCalls.CastInt(objects[5], out int axeType))
            return OriginCalls.ThrowOrReturn("objects[2] (axeType) is not an int!");

        if (objects.Length == 6)
            return new MiscData(life, mana, npcType, swordType, pickType, axeType);

        if (!OriginCalls.CastInt(objects[6], out int sortPriority))
            return OriginCalls.ThrowOrReturn("objects[2] (sortPriority) is not an int!");

        if (objects.Length == 6)
            return new MiscData(life, mana, npcType, swordType, pickType, axeType);

        if (!OriginCalls.CastInt(objects[7], out int stars))
            return OriginCalls.ThrowOrReturn("objects[2] (stars) is not an int!");
        return new MiscData(life, mana, npcType, swordType, pickType, axeType, sortPriority, stars);
    }

    internal static object MakeDelegateData(object[] objects)
    {
        if (objects[0] is not Func<bool> condition)
            return OriginCalls.ThrowOrReturn("objects[0] is not Func<bool>!");

        if (objects.Length == 1)
            return new DelegateData(condition);

        if (objects[1] is not Action<List<GenPass>> modifyWorldGen)
            return OriginCalls.ThrowOrReturn("objects[1] is not Action<List<GenPass>>!");

        if (objects.Length == 2)
            return new DelegateData(condition, modifyWorldGen);

        if (objects[2] is not Func<bool> hasCustomSpawn)
            return OriginCalls.ThrowOrReturn("objects[2] is not Action<List<GenPass>>!");

        if (objects.Length == 3)
            return OriginCalls.ThrowOrReturn("MakeDelegateData accepted a hasCustomSpawn but no actualSpawn func!");

        if (objects[3] is not Func<Point16> actualSpawn)
            return OriginCalls.ThrowOrReturn("objects[3] is not Func<Point16>!");

        if (objects.Length == 4)
            return new DelegateData(condition, modifyWorldGen, hasCustomSpawn, actualSpawn);

        if (objects[4] is not Action<Player> modifyCreation)
            return OriginCalls.ThrowOrReturn("objects[4] is not Func<Point16>!");

        if (objects.Length == 5)
            return new DelegateData(condition, modifyWorldGen, hasCustomSpawn, actualSpawn, modifyCreation);

        return OriginCalls.ThrowOrReturn("objects.Length is greater than 4!");
    }
}
