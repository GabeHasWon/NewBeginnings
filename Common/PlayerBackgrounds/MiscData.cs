namespace NewBeginnings.Common.PlayerBackgrounds;

/// <summary>Defines a bunch of mostly esoteric content, such as the npc type that replaces the guide or what items replace the base set (if any).
/// Defaults to either default stats (with <paramref name="life"/> and <paramref name="mana"/>) or no override (with everything else).</summary>
/// <param name="life">Starting max life of the background. Defaults to base 100. Cannot be lower than 20.</param>
/// <param name="mana">Starting max mana of the background. Defaults to 20.</param>
/// <param name="npcType">NPC type that replaces the guide in a new world.</param>
/// <param name="sword">Item type that replaces the copper shortsword (slot 0).</param>
/// <param name="pick">Item type that replaces the copper pickaxe (slot 1).</param>
/// <param name="axe">Item type that replaces the copper axe (slot 2).</param>
/// <param name="sortPriority">Sort priority in the character creation UI. Higher number = higher on the list.</param>
/// <param name="stars">Star count of the origin. Defaults to 3 (neutral).</param>
/// <param name="mod">The mod that adds this origin.</param>
internal readonly struct MiscData(int life = 100, int mana = 20, int npcType = -1, int sword = -1, int pick = -1, int axe = -1, int sortPriority = 10, int stars = 3)
{
    public readonly int SpecialFirstNPCType = npcType;
    public readonly int MaxLife = life;
    public readonly int AdditionalMana = mana;
    public readonly int CopperShortswordReplacement = sword;
    public readonly int CopperPickaxeReplacement = pick;
    public readonly int CopperAxeReplacement = axe;
    public readonly int SortPriority = sortPriority;
    public readonly int Stars = stars;
}