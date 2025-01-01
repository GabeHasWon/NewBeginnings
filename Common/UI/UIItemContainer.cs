using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.UI;
using Terraria.GameContent.UI.Elements;
using System;

namespace NewBeginnings.Common.UI;

public class UIItemContainer : UIElement
{
    public Item Item { get; private set; }
    public bool CanStack { get; private set; }

    private int _clickStackTimer = 0;
    private int _hoverTimer = 0;
    
    public UIItemContainer(Item item, bool canStack = false)
    {
        Item = item;

        CanStack = canStack && Item.maxStack > 1;

        Width.Set(40, 0);
        Height.Set(32, 0);

        if (CanStack)
            AddStackModification();
    }

    public void SetCanStack(bool canStack)
    {
        CanStack = canStack && Item.maxStack > 1;
        RemoveAllChildren();

        if (CanStack)
            AddStackModification();
    }

    private void AddStackModification()
    {
        var stack = new UIText(Item.stack.ToString())
        {
            Width = StyleDimension.Fill,
            VAlign = 1f,
            HAlign = 0.5f,
            IsWrapped = false,
            DynamicallyScaleDownToWidth = true,
            Top = StyleDimension.FromPixels(6)
        };

        stack.OnUpdate += _ => UpdateStack(stack);
        Append(stack);
    }

    private void UpdateStack(UIText stack)
    {
        _hoverTimer--;
        stack.SetText(Item.stack.ToString());

        if (stack.GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()))
        {
            stack.TextColor = Color.Gray;

            _hoverTimer = 2;
            bool canClick = _clickStackTimer == 0 || _clickStackTimer > 10;
            int mod = _clickStackTimer > 80 ? 50 : 1;

            if (Main.mouseLeft)
            {
                if (canClick)
                    ModifyStack(mod);

                _clickStackTimer++;

            }
            else if (Main.mouseRight)
            {
                if (canClick)
                    ModifyStack(-mod);

                _clickStackTimer++;
            }
            
            if (!Main.mouseLeft && !Main.mouseRight)
                _clickStackTimer = 0;
        }
        else
            stack.TextColor = Color.White;
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        if (_hoverTimer <= 0)
            base.LeftClick(evt);
    }

    private void ModifyStack(int difference) => Item.stack = (int)MathHelper.Clamp(Item.stack + difference, 1, Item.maxStack);
    public void OverrideItem(Item item) => Item = item;

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        Color color = Color.White;

        if (GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()) && _hoverTimer <= 0)
            color = Color.Gray;

        ItemSlot.DrawItemIcon(Item, 31, spriteBatch, GetDimensions().Center(), Item.scale, 32f, color);
    }
}

