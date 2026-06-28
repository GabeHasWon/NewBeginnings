using Terraria.ModLoader;

namespace NewBeginnings.Common.Crossmod
{
    internal class RussianTranslateCompat : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("CalamityRuTranslate", out Mod tru))
            {
                tru.Call("AddFeminineItems", Mod, new[] { "RustyCutlass", "WornSpellbook" });
            }
        }
    }
}
