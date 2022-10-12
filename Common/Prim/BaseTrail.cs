using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.Prim.Components.Positions;
using System.Collections.Generic;
using Terraria;

namespace NewBeginnings.Common.Prim
{
    internal abstract class BaseTrail
    {
        public readonly float MaxLength;

        public Entity Parent { get; private set; }
        public float CurrentLength { get; set; }

        internal List<Vector2> vertices = new List<Vector2>();

        private ITrailPosition _position;
        private bool _dying = false;

        public BaseTrail(Entity parent, float maxLength)
        {
            Parent = parent;
            MaxLength = maxLength;
        }

        public void Update()
        {
            _position.InsertNextPosition(this);

            if (!Parent.active)
                Kill();
        }

        public virtual void Kill()
        {
            _dying = true;
        }

        public abstract void Draw(Effect effect, BasicEffect basicEffect, GraphicsDevice device);
    }
}
