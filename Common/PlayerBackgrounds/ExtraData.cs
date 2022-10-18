namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal struct MiscData
    {
        public int SpecialFirstNPCType;
        public int SpecialSpawnX;
        public int SpecialSpawnY;
        public int MaxLife;
        public int AdditionalMana;

        public MiscData(int life = 100, int mana = 20, int x = -1, int y = -1, int npcType = -1)
        {
            SpecialFirstNPCType = npcType;
            SpecialSpawnX = x;
            SpecialSpawnY = y;
            MaxLife = life;
            AdditionalMana = mana;
        }
    }
}