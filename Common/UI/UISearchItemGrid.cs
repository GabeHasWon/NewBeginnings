using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.UnlockabilitySystem;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace NewBeginnings.Common.UI;

internal class UISearchItemGrid : UIPanel
{
    private enum SortMode : byte
    {
        Chaos = 0,
        Alphabetical,
        ID,
    }

    private readonly static HashSet<int> alwaysInvalidItems = [];

    private readonly UIList _list = null;
    private readonly UIEditableText _inputText = null;
    private readonly UIButton<string> _sortButton = null;
    private readonly Action<UIItemContainer> _clickItem = null;

    private SortMode _mode = SortMode.Chaos;
    private int _hideTimer = 0;
    private int _timeSinceTyped = 0;
    private string _oldValue = string.Empty;

    public UISearchItemGrid(Action<UIItemContainer> onClick)
    {
        _clickItem = onClick;

        var panel = new UIPanel()
        {
            Width = StyleDimension.FromPixelsAndPercent(-170, 1),
            Height = StyleDimension.FromPixels(36),
        };

        Append(panel);

        _inputText = new UIEditableText(InputType.Text, UICustomOrigin.Localize("Search"))
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
            Top = StyleDimension.FromPixels(-8),
            VAlign = 0.5f
        };

        panel.Append(_inputText);

        _sortButton = new UIButton<string>(UICustomOrigin.Localize("SortBy") + LocalizeMode(_mode))
        {
            Width = StyleDimension.FromPixels(160),
            Height = StyleDimension.FromPixels(36),
            Left = StyleDimension.FromPixels(324)
        };

        _sortButton.OnLeftClick += ClickSortButton;
        Append(_sortButton);

        _list = new()
        {
            Width = StyleDimension.FromPixelsAndPercent(-28, 1),
            Height = StyleDimension.FromPixelsAndPercent(-40, 1f),
            VAlign = 1f,
        };

        Append(_list);

        UIScrollbar bar = new()
        {
            Width = StyleDimension.FromPixels(24),
            Height = StyleDimension.FromPixelsAndPercent(-44, 1f),
            HAlign = 1f,
            VAlign = 1f,
        };

        _list.SetScrollbar(bar);
        Append(bar);

        PopulateList();
    }

    private static string LocalizeMode(SortMode mode) => UICustomOrigin.Localize("Sort." + mode);

    private void ClickSortButton(UIMouseEvent evt, UIElement listeningElement)
    {
        _mode++;

        if (_mode > SortMode.ID)
            _mode = SortMode.Chaos;

        (listeningElement as UIButton<string>).SetText(UICustomOrigin.Localize("SortBy") + LocalizeMode(_mode));

        _list.ManualSortMethod = SortListBy;
        RedoList();
    }

    private void SortListBy(List<UIElement> list)
    {
        if (_mode == SortMode.Chaos)
            list.Sort();
        else if (_mode == SortMode.Alphabetical)
            list.Sort((x, y) => (x as UIItemContainer).Item.Name.CompareTo((y as UIItemContainer).Item.Name));
        else if (_mode == SortMode.ID)
            list.Sort((x, y) => (x as UIItemContainer).Item.type.CompareTo((y as UIItemContainer).Item.type));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_timeSinceTyped == -10)
            RedoList();

        _timeSinceTyped--;

        if (_inputText.currentValue != _oldValue)
            _timeSinceTyped = 0;

        _oldValue = _inputText.currentValue;
    }

    private void RedoList()
    {
        _list.Clear();
        PopulateList();
    }

    private void PopulateList()
    {
        _hideTimer = 3;

        if (alwaysInvalidItems.Count == 0)
        {
            for (int i = 1; i < ItemLoader.ItemCount; ++i)
            {
                Item item = ContentSamples.ItemsByType[i];

                if (item.IsAir || item.type == ItemID.None || ItemID.Sets.Deprecated[i] || ItemID.Sets.ItemsThatShouldNotBeInInventory[i]
                    || item.type == ItemID.ManaCloakStar || item.type == ItemID.BoringBow)
                    alwaysInvalidItems.Add(i);
            }
        }

        List<UIElement> elements = [];

        for (int i = 1; i < ItemLoader.ItemCount; ++i)
        {
            Item item = ContentSamples.ItemsByType[i];

            // Necessary, constant checks that invalidate items
            if (alwaysInvalidItems.Contains(i))
                continue;

            // Skip if we have a restricted custom background
            if (!UnlockSaveData.Unlocked("Terrarian") && UICustomOrigin.InvalidItemToAdd(item))
                continue;

            bool notSearch = _inputText.currentValue == string.Empty;

            // Match search if any search is found
            if (!notSearch && !MatchSearch(item, _inputText.currentValue))
                continue;

            var image = new UIItemContainer(item);
            image.OnLeftClick += Image_OnLeftClick;
            image.Append(new UIText(item.Name) { VAlign = 0.5f, Left = StyleDimension.FromPixels(46) });
            elements.Add(image);
        }

        _list.AddRange(elements);
    }

    private static bool MatchSearch(Item item, string currentValue)
    {
        string value = currentValue;

        if (value.Contains("!au"))
        {
            value = value.Replace("!au", "");

            if (item.headSlot <= -1 && item.bodySlot <= -1 && item.legSlot <= -1 || item.vanity)
                return false;
        }
        else if (value.Contains("!v"))
        {
            value = value.Replace("!v", "");

            if (!item.vanity)
                return false;
        }
        else if (value.Contains("!a"))
        {
            value = value.Replace("!a", "");

            if (item.headSlot <= -1 && item.bodySlot <= -1 && item.legSlot <= -1)
                return false;
        }
        
        if (value.Contains("!c"))
        {
            value = value.Replace("!c", "");

            if (!item.accessory)
                return false;
        }

        if (value.Contains("!w"))
        {
            value = value.Replace("!w", "");

            if (item.damage <= 0)
                return false;
        }

        if (value == string.Empty)
            return true;

        value = value.Trim();
        return item.Name.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    protected override void DrawChildren(SpriteBatch spriteBatch)
    {
        foreach (UIElement element in Elements)
        {
            if (element is UIList && _hideTimer > 0)
                continue;

            element.Draw(spriteBatch);
        }

        _hideTimer--;
    }

    private void Image_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
    {
        var container = listeningElement as UIItemContainer;
        _clickItem(container);
    }
}
