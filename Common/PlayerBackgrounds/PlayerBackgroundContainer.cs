using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    /// <summary>Wrapper class for backgrounds that merit their own file entirely, such as the Slayer with its long worldgen code.</summary>
    internal abstract class PlayerBackgroundContainer
    {
        public virtual string Name => GetType().Name;
        public virtual string Texture => GetType().Name;
        public virtual string Description => "TBD";

        public virtual (int type, int stack)[] Inventory => Array.Empty<(int type, int stack)>();

        public virtual EquipData Equip => new(0, 0, 0);
        public virtual MiscData Misc => new(100, 20);

        public virtual bool ClearCondition() => true;
        public virtual void ModifyWorldGenTasks(List<GenPass> list) { }
        public virtual void ModifyPlayerCreation(Player player) { }

        public virtual bool HasSpecialSpawn() => false;
        public virtual Point16 GetSpawnPosition() => Point16.Zero;

        public static implicit operator PlayerBackgroundData(PlayerBackgroundContainer container)
        {
            var data = new PlayerBackgroundData(container.Name, container.Texture, container.Description, container.Equip, container.Misc, container.Inventory);
            data.Delegates = new DelegateData(container.ClearCondition, container.ModifyWorldGenTasks, container.HasSpecialSpawn, container.GetSpawnPosition);
            return data;
        }
    }
}
