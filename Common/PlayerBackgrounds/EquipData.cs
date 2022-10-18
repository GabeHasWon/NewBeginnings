namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal struct EquipData
    {
        public int Head;
        public int Body;
        public int Legs;

        public int[] Accessories;

        public EquipData(int head = 0, int body = 0, int legs = 0, params int[] acc)
        {
            Head = head;
            Body = body;
            Legs = legs;
            Accessories = acc;
        }

        /// <summary>For origins that only need 1 accessory and no armor</summary>
        public static EquipData SingleAcc(int acc) => new EquipData(0, 0, 0, acc);

        /// <summary>For origins that need 1 accessory and less than a full set of armor, such as: <code>AccFirst(ItemID.Aglet, ItemID.Goggles)</code></summary>
        public static EquipData AccFirst(int acc, int head = 0, int body = 0, int legs = 0) => AccFirst(new int[] { acc }, head, body, legs);

        /// <summary>For origins that need more than one accessory and less than a full set of armor, such as: <code>AccFirst(new int[] { ItemID.Aglet, ItemID.HermesBoots }, ItemID.Goggles)</code></summary>
        public static EquipData AccFirst(int[] acc, int head = 0, int body = 0, int legs = 0) => new EquipData(head, body, legs, acc);
    }
}
