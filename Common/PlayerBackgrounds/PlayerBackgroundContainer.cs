using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    /// <summary>
    /// Wrapper class for backgrounds that merit their own file entirely, such as the Slayer with its long worldgen code.<br/>
    /// Use this sparingly, and only for large backgrounds that merit it.
    /// </summary>
    internal abstract class PlayerBackgroundContainer
    {
        /// <inheritdoc cref="PlayerBackgroundData.Name"/>
        public virtual string Name => GetType().Name;

        /// <inheritdoc cref="PlayerBackgroundData.Texture"/>
        public virtual string Texture => GetType().Name;

        /// <inheritdoc cref="PlayerBackgroundData.Flavour"/>
        public virtual string Flavour => "TBD";

        /// <inheritdoc cref="PlayerBackgroundData.Description"/>
        public virtual string Description => "TBD";

        /// <inheritdoc cref="PlayerBackgroundData.Inventory"/>
        public virtual (int type, int stack)[] Inventory => Array.Empty<(int type, int stack)>();

        /// <inheritdoc cref="PlayerBackgroundData.Equip"/>
        public virtual EquipData Equip => new(0, 0, 0);

        /// <inheritdoc cref="PlayerBackgroundData.Misc"/>
        public virtual MiscData Misc => new(100, 20);

        public virtual bool ClearCondition() => true;
        public virtual void ModifyWorldGenTasks(List<GenPass> list) { }
        public virtual void ModifyPlayerCreation(Player player) { }

        public virtual bool HasSpecialSpawn() => false;
        public virtual Point16 GetSpawnPosition() => Point16.Zero;

        /// <summary>Automatically converts a given PlayerBackgroundContainer into a PlayerBackgroundData for ease of use.</summary>
        public static implicit operator PlayerBackgroundData(PlayerBackgroundContainer container)
        {
            var data = new PlayerBackgroundData(container.Name, container.Texture, container.Flavour, container.Description, container.Equip, container.Misc, container.Inventory)
            {
                Delegates = new DelegateData(container.ClearCondition, container.ModifyWorldGenTasks, container.HasSpecialSpawn, container.GetSpawnPosition, container.ModifyPlayerCreation)
            };
            return data;
        }
    }
}
