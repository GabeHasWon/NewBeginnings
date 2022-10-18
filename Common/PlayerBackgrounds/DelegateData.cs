using System;
using System.Collections.Generic;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal struct DelegateData
    {
        public Func<bool> ClearCondition;
        public Action<List<GenPass>> ModifyWorldGenTasks;

        public DelegateData()
        {
            ClearCondition = () => true;
            ModifyWorldGenTasks = (_) => { };
        }

        public DelegateData(Func<bool> clear = null, Action<List<GenPass>> modify = null)
        {
            ClearCondition = clear ?? (() => true);
            ModifyWorldGenTasks = modify ?? ((_) => { });
        }
    }
}
