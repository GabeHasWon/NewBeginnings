using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;

namespace NewBeginnings.Content.Items.Icons;

internal class DimStar : ModItem
{
    public override void SetStaticDefaults() => Tooltip.SetDefault("You shouldn't see this - unless you're CHEATING (or on the git!)");

    public override void SetDefaults()
    {
        Item.Size = new Vector2(14, 14);
    }
}

internal class LightStar : ModItem
{
    public override void SetStaticDefaults() => Tooltip.SetDefault("You shouldn't see this - unless you're CHEATING (or on the git!)");

    public override void SetDefaults()
    {
        Item.Size = new Vector2(14, 14);
    }
}
