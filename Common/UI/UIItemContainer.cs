using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;

namespace NewBeginnings.Common.UI;

public class UIItemContainer : UIElement
{
    public Item Item { get; }

    public UIItemContainer(Item item)
    {
        Item = item;
        Width.Set(32f, 0f);
        Height.Set(32f, 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        Color color = Color.White;

        if (GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()))
            color = Color.Gray;

        ItemSlot.DrawItemIcon(Item, 31, spriteBatch, GetDimensions().Center(), Item.scale, 32f, color);
    }
}

