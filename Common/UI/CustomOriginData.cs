using System;
using Terraria.ModLoader.IO;

namespace NewBeginnings.Common.UI;

public readonly record struct ItemPair(int Id, int Stack);

internal class CustomOriginData
{
    public static CustomOriginData Empty => 
        new(
            [EmptyPair, EmptyPair, EmptyPair, EmptyPair, EmptyPair, EmptyPair, EmptyPair, EmptyPair, EmptyPair, EmptyPair], 
            [EmptyPair, EmptyPair, EmptyPair], 
            [EmptyPair, EmptyPair, EmptyPair, EmptyPair, EmptyPair]
        );

    public static readonly ItemPair EmptyPair = new(0, 0);

    public ref ItemPair Helmet => ref Armor[0];
    public ref ItemPair Body => ref Armor[1];
    public ref ItemPair Legs => ref Armor[2];

    public readonly ItemPair[] Hotbar;
    public readonly ItemPair[] Armor;
    public readonly ItemPair[] Accessories;

    public int life = 20;
    public int mana = 0;

    public CustomOriginData(ItemPair[] hotbar, ItemPair[] armor, ItemPair[] accessories)
    {
        Hotbar = hotbar;
        Armor = armor;
        Accessories = accessories;

        if (Hotbar.Length != 10)
            throw new ArgumentException("Hotbar should be 10 items long.");

        if (Armor.Length != 3)
            throw new ArgumentException("Armor should be 3 items long.");

        if (Accessories.Length != 5)
            throw new ArgumentException("Accessories should be 5 items long.");
    }

    public void SaveData(TagCompound tag)
    {
        SavePairArray(Hotbar, "hotbar", tag);
        SavePairArray(Armor, "armor", tag);
        SavePairArray(Accessories, "acc", tag);
    }

    public void LoadData(TagCompound tag)
    {
        LoadPairArray(Hotbar, "hotbar", tag);
        LoadPairArray(Armor, "armor", tag);
        LoadPairArray(Accessories, "acc", tag);
    }

    private static void LoadPairArray(ItemPair[] pair, string name, TagCompound tag)
    {
        int[] types = tag.GetIntArray(name + "Types");
        int[] stacks = tag.GetIntArray(name + "Stacks");

        for (int i = 0; i < types.Length; i++)
            pair[i] = new ItemPair(types[i], stacks[i]);
    }

    private static void SavePairArray(ItemPair[] pair, string name, TagCompound tag)
    {
        int[] types = new int[pair.Length];
        int[] stacks = new int[pair.Length];

        for (int i = 0; i < pair.Length; ++i)
        {
            types[i] = pair[i].Id;
            stacks[i] = pair[i].Stack;
        }

        tag.Add(name + "Types", types);
        tag.Add(name + "Stacks", stacks);
    }
}
