namespace NewBeginnings.PlayerBackgrounds
{
    internal struct ExtraBackgroundData
    {
        public int SpecialFirstNPCType;
        public int SpecialSpawnX;
        public int SpecialSpawnY;

        public ExtraBackgroundData(int npcType, int x, int y)
        {
            SpecialFirstNPCType = npcType;
            SpecialSpawnX = x;
            SpecialSpawnY = y;
        }
    }
}