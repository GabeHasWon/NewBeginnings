using System;
using System.Collections.Generic;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal struct DelegateData
    {
        public Func<bool> ClearCondition;
        public Action<List<GenPass>> ModifyWorldGenTasks;

        /// <summary>
        /// Default delegates ("do nothing").
        /// </summary>
        public DelegateData()
        {
            ClearCondition = () => true;
            ModifyWorldGenTasks = (_) => { };
        }

        /// <summary>Allows the use of conditions for an origin and a hook-like additional worldgen delegate for ease-of-use.</summary>
        /// <param name="clear">Condition of the origin. Returns true by default.</param>
        /// <param name="modify">Allows you to modify the genpass list of an incoming world, allowing you to add or remove passes as you please.</param>
        public DelegateData(Func<bool> clear = null, Action<List<GenPass>> modify = null)
        {
            ClearCondition = clear ?? (() => true);
            ModifyWorldGenTasks = modify ?? ((_) => { });
        }
    }
}
