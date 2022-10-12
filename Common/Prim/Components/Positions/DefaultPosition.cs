using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;

namespace NewBeginnings.Common.Prim.Components.Positions
{
    internal class DefaultPosition : ITrailPosition
    {
        public Vector2 GetNextPosition(Entity entity) => entity.Center;

        public void InsertNextPosition(BaseTrail trail)
        {
            if (trail.vertices.Count == 0)
            {
                trail.vertices.Add(GetNextPosition(trail.Parent));
                return;
            }

            Vector2 next = GetNextPosition(trail.Parent);
            float distance = Vector2.Distance(next, trail.vertices[0]);
            trail.vertices.Insert(0, next);

            //If adding the next point is too much
            if (trail.CurrentLength + distance > trail.MaxLength)
                TrimToLength(trail, trail.MaxLength);
            else
                trail.CurrentLength += distance;
        }

        /// <summary>STOLEN from Spirit Mod's implementation directly.</summary>
        /// <param name="length"></param>
        private static void TrimToLength(BaseTrail trail, float length)
        {
            if (trail.vertices.Count == 0) return;

            trail.CurrentLength = length;

            int firstPointOver = -1;
            float newLength = 0;

            for (int i = 1; i < trail.vertices.Count; i++)
            {
                newLength += Vector2.Distance(trail.vertices[i], trail.vertices[i - 1]);
                if (newLength > length)
                {
                    firstPointOver = i;
                    break;
                }
            }

            if (firstPointOver == -1) return;

            //get new end point based on remaining distance
            float leftOverLength = newLength - length;
            Vector2 between = trail.vertices[firstPointOver] - trail.vertices[firstPointOver - 1];
            float newPointDistance = between.Length() - leftOverLength;
            between.Normalize();

            int toRemove = trail.vertices.Count - firstPointOver;
            trail.vertices.RemoveRange(firstPointOver, toRemove);

            trail.vertices.Add(trail.vertices.Last() + between * newPointDistance);
        }

    }
}
