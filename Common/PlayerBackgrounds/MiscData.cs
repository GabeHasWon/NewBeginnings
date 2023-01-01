namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal readonly struct MiscData
    {
        public readonly int SpecialFirstNPCType;
        public readonly int MaxLife;
        public readonly int AdditionalMana;
        public readonly int CopperShortswordReplacement;
        public readonly int CopperPickaxeReplacement;
        public readonly int CopperAxeReplacement;
        public readonly int SortPriority;
        public readonly int Stars;

        /// <summary>Defines a bunch of mostly esoteric content, such as the npc type that replaces the guide or what items replace the base set (if any).
        /// Defaults to either default stats (with <paramref name="life"/> and <paramref name="mana"/>) or no override (with everything else).</summary>
        /// <param name="life">Starting max life of the background. Defaults to base 100. Cannot be lower than 20.</param>
        /// <param name="mana">Starting max mana of the background. Defaults to 20.</param>
        /// <param name="npcType">NPC type that replaces the guide in a new world.</param>
        /// <param name="swordReplace">Item type that replaces the copper shortsword (slot 0).</param>
        /// <param name="pickReplace">Item type that replaces the copper pickaxe (slot 1).</param>
        /// <param name="axeReplace">Item type that replaces the copper axe (slot 2).</param>
        /// <param name="sortPriority">Sort priority in the character creation UI. Higher number = higher on the list.</param>
        /// <param name="stars">Star count of the origin. Defaults to 3 (neutral).</param>
        public MiscData(int life = 100, int mana = 20, int npcType = -1, int swordReplace = -1, int pickReplace = -1, int axeReplace = -1, int sortPriority = 10, int stars = 3)
        {
            SpecialFirstNPCType = npcType;
            MaxLife = life;
            AdditionalMana = mana;
            CopperShortswordReplacement = swordReplace;
            CopperPickaxeReplacement = pickReplace;
            CopperAxeReplacement = axeReplace;
            SortPriority = sortPriority;
            Stars = stars;
        }
    }
}