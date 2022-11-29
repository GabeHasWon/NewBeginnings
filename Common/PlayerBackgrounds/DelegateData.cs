using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal struct DelegateData
    {
        public Func<bool> ClearCondition;
        public Action<List<GenPass>> ModifyWorldGenTasks;
        public Func<bool> HasSpecialSpawn;
        public Func<Point16> GetSpawnPosition;
        public Action<Player> ModifyPlayerCreation;

        /// <summary>
        /// Default delegates ("do nothing").
        /// </summary>
        public DelegateData()
        {
            ClearCondition = () => true;
            ModifyWorldGenTasks = (_) => { };
            HasSpecialSpawn = () => false;
            GetSpawnPosition = () => Point16.Zero;
            ModifyPlayerCreation = (_) => { };
        }

        /// <summary>
        /// <para>Allows the use of conditions for an origin and a hook-like additional worldgen delegate for ease-of-use.</para>
        /// <para>With especially large delegates, it's best to make a custom <see cref="PlayerBackgroundContainer"/> child in order to keep the database readable.</para>
        /// </summary>
        /// <param name="clear">Condition of the origin. Returns true by default.</param>
        /// <param name="modify">Allows you to modify the genpass list of an incoming world, allowing you to add or remove passes as you please.</param>
        /// <param name="hasSpawn">Checks if the origin has a special spawn point. Useful for variable spawns.</param>
        /// <param name="spawn">Allows you to dynamically modify the spawn point, such as choosing a random beach to spawn on.</param>
        /// <param name="modifyCreation">Allows you to adjust the player right after the player is actually created and before they're saved.</param>
        public DelegateData(Func<bool> clear = null, Action<List<GenPass>> modify = null, Func<bool> hasSpawn = null, Func<Point16> spawn = null, Action<Player> modifyCreation = null)
        {
            ClearCondition = clear ?? (() => true);
            ModifyWorldGenTasks = modify ?? ((_) => { });
            HasSpecialSpawn = hasSpawn ?? (() => false);
            GetSpawnPosition = spawn ?? (() => Point16.Zero);
            ModifyPlayerCreation = modifyCreation ?? ((_) => { });
        }
    }
}
