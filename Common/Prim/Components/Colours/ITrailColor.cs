using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NewBeginnings.Common.Prim.Components.Colours
{
    internal interface ITrailColor
    {
        Color ColorAt(float factor, float trailSize, List<Vector2> vertices);
    }
}
