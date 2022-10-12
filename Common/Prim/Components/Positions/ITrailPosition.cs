using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace NewBeginnings.Common.Prim.Components.Positions
{
    internal interface ITrailPosition
    {
        Vector2 GetNextPosition(Entity entity);
        void InsertNextPosition(BaseTrail trail);
    }
}
