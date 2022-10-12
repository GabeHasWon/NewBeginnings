using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace NewBeginnings.Common.Prim
{
    internal class EffectLoader : ILoadable
    {
        public static Effect _effect;

        public void Load(Mod mod)
        {
            _effect = ModContent.Request<Effect>("NewBeginnings/Assets/Effects/trailShaders", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }

        public void Unload() { }
    }
}
