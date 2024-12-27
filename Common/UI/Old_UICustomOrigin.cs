using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Content.Items.Icons;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace NewBeginnings.Common.UI;

[Obsolete("Requires a LOT of work to get right, completely unbalanceable. Defer to later, if ever.")]
internal class Old_UICustomOrigin : UIState
{
    private Player _player;
    private readonly MouseEvent _return;

    private int RealLife => (int)MathHelper.Clamp(_maxLife * 400, 20, 400);
    private int RealMana => (int)(_maxMana * 200);

    private float _maxLife = 0;
    private float _maxMana = 0;
    private float _stars = 0;

    public Old_UICustomOrigin(Player player, MouseEvent returnAction)
    {
        _player = player;
        _return = returnAction;

        Setup();
    }

    private void Setup()
    {
        RemoveAllChildren();

        UIElement mainElement = new UIElement
        {
            Width = StyleDimension.FromPixels(800),
            Height = StyleDimension.FromPixels(300),
            Top = StyleDimension.FromPixels(220f),
            HAlign = 0.5f,
            VAlign = 0f
        };
        mainElement.SetPadding(0f);
        Append(mainElement);

        Color panelColor = new Color(33, 43, 79) * 0.8f;

        UIPanel panel = new UIPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f),
            Top = StyleDimension.FromPixels(50f),
            BackgroundColor = panelColor
        };

        mainElement.Append(panel);

        UIImageButton closeButton = new UIImageButton(ModContent.Request<Texture2D>("NewBeginnings/Assets/Textures/UI/OriginBack"))
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixelsAndPercent(-40, 1f),
            Top = StyleDimension.FromPixelsAndPercent(0, 0)
        };

        closeButton.OnLeftClick += _return;
        panel.Append(closeButton);

        UIOriginSelection.AddCharacterPreview(panel, _player);

        string str = GetSplash();
        UIText text = new UIText(str, 1 - (str.Length / 200f))
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-6),
        };

        text.OnUpdate += (UIElement affectedElement) => { text.TextColor = Main.MouseTextColorReal; };
        panel.Append(text);

        AddStatsCounters(panel);
        BuildSliders(panel);
        BuildItemGrid(panel);
    }

    private void BuildItemGrid(UIPanel panel)
    {
        UIList itemGrid = new UIList()
        {
            Left = StyleDimension.FromPercent(0.5f),
            Width = StyleDimension.FromPixelsAndPercent(-8, 0.5f),
            Height = StyleDimension.FromPixelsAndPercent(-64, 1f),
            Top = StyleDimension.FromPixels(60),
            ListPadding = 20f,
            PaddingTop = 16,
        };

        var scrollBar = new UIScrollbar() //Scrollbar for above list
        {
            HAlign = 1f,
            Height = StyleDimension.FromPixelsAndPercent(-64, 1f),
            Width = StyleDimension.FromPixels(16),
            Top = StyleDimension.FromPixels(60),
        };

        itemGrid.SetScrollbar(scrollBar);
        panel.Append(scrollBar);

        List<(int type, UIColoredImageButton button)> buttons = new List<(int, UIColoredImageButton)>();

        for (int i = 1; i < ItemLoader.ItemCount; ++i)
        {
            if (InvalidItemToAdd(ContentSamples.ItemsByType[i]))
                continue;

            Main.instance.LoadItem(i);
            UIColoredImageButton button = new UIColoredImageButton(TextureAssets.Item[i])
            {
                Width = StyleDimension.FromPixels(32),
                Height = StyleDimension.FromPixels(32),
                Left = StyleDimension.FromPixels(16)
            };

            itemGrid.Add(button);
            buttons.Add((i, button));
        }

        itemGrid.ManualSortMethod = (list) => list.Sort((self, other) =>
        {
            int mySortPriority = buttons.Find(x => x.button == self).type; //Find priority by finding the value that has the given button as Item2
            int otherSortPriority = buttons.Find(x => x.button == other).type; //for both the current and next button

            return mySortPriority.CompareTo(otherSortPriority);
        });
        itemGrid.UpdateOrder();

        panel.Append(itemGrid);
    }

    private bool InvalidItemToAdd(Item item)
    {
        return item.createTile >= TileID.Dirt || item.createWall > WallID.None || item.questItem || (item.ammo > AmmoID.None && item.damage > 8) || item.damage > 15 || item.defense > 5 || ItemID.Sets.BossBag[item.type] || 
            ItemID.Sets.Deprecated[item.type] || item.pick > 80 || item.axe > 15 || item.hammer > 50 || item.wingSlot > 0;
    }

    private void BuildSliders(UIPanel panel)
    {
        UIColoredSlider lifeSlider = MakeSlider(() => _maxLife, x => _maxLife = x, (x) => Color.Lerp(Color.White, Color.Red, x));
        lifeSlider.Width = StyleDimension.FromPixels(60);
        lifeSlider.Height = StyleDimension.FromPixels(12);
        lifeSlider.HAlign = 0.2f;
        lifeSlider.Top = StyleDimension.FromPixels(32);

        lifeSlider.Append(new UIText("Health", 1f)
        {
            Left = StyleDimension.FromPercent(1),
            Top = StyleDimension.FromPixels(8),
            VAlign = 0.5f,
        });
        panel.Append(lifeSlider);

        UIColoredSlider manaSlider = MakeSlider(() => _maxMana, x => _maxMana = x, (x) => Color.Lerp(Color.White, Color.Blue, x));
        manaSlider.Width = StyleDimension.FromPixels(60);
        manaSlider.Height = StyleDimension.FromPixels(12);
        manaSlider.HAlign = 0.2f;
        manaSlider.Top = StyleDimension.FromPixels(54);

        manaSlider.Append(new UIText("Mana", 1f)
        {
            Left = StyleDimension.FromPercent(1),
            Top = StyleDimension.FromPixels(8),
            VAlign = 0.5f,
        });
        panel.Append(manaSlider);
    }

    private delegate float ReturnValueDelegate();

    private UIColoredSlider MakeSlider(Func<float> returnFunction, Action<float> updateValue, Func<float, Color> colorMod)
    {
        return new UIColoredSlider(LocalizedText.Empty, () => returnFunction.Invoke(), updateValue, () =>
        {
            float value = UILinksInitializer.HandleSliderHorizontalInput(returnFunction.Invoke(), 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
            updateValue(value);
        }, colorMod, Color.Transparent);
    }

    private void AddStatsCounters(UIPanel panel)
    {
        var stats = new UIText(GetStatsText(RealLife, RealMana, (int)_stars))
        {
            Top = StyleDimension.FromPixels(4),
            Width = StyleDimension.FromPixels(4),
            HAlign = 0f,
            Height = StyleDimension.FromPixels(44),
            IsWrapped = false,
            MarginTop = 8
        };
        stats.OnUpdate += (self) => stats.SetText(GetStatsText(RealLife, RealMana, (int)_stars));
        panel.Append(stats);
    }

    private static string GetStatsText(int health, int mana, int starsCount)
    {
        string stats = $"[i:{ItemID.Heart}]{health} [i:{ItemID.Star}]{mana}";
        string stars = string.Empty;

        for (int i = 0; i < 5; ++i)
            stars += starsCount > i ? $"[i:{ModContent.ItemType<LightStar>()}]" : $"[i:{ModContent.ItemType<DimStar>()}]";

        return stars + "   " + stats;
    }

    private string GetSplash()
    {
        int random = Main.rand.Next(4);

        return random switch
        {
            0 => "The real challenge is to start without items!",
            1 => "Asbestos free since 1979!",
            2 => "If you give yourself the Celestial Sigil you can win instantly!",
            _ => "Calamiter? I hardly know her!"
        };
    }
}
