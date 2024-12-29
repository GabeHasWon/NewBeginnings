using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
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

    private readonly UIList _list = null;
    private readonly UIEditableText _inputText = null;
    private readonly UIButton<string> _sortButton = null;
    private readonly Action<UIItemContainer> _clickItem = null;

    private SortMode _mode = SortMode.Chaos;

    public UISearchItemGrid(Action<UIItemContainer> onClick)
    {
        _clickItem = onClick;

        var panel = new UIPanel()
        {
            Width = StyleDimension.FromPixelsAndPercent(-180, 1),
            Height = StyleDimension.FromPixels(36),
        };

        Append(panel);

        _inputText = new UIEditableText(InputType.Text, "Search for an item - enter to filter")
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
            Top = StyleDimension.FromPixels(-8),
            VAlign = 0.5f
        };

        panel.Append(_inputText);

        _sortButton = new UIButton<string>("Sort: Chaos")
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
            Height = StyleDimension.FromPixelsAndPercent(-40, 1f),
            HAlign = 1f,
            VAlign = 1f,
        };

        _list.SetScrollbar(bar);
        Append(bar);

        PopulateGrid();
    }

    private void ClickSortButton(UIMouseEvent evt, UIElement listeningElement)
    {
        _mode++;

        if (_mode > SortMode.ID)
            _mode = SortMode.Chaos;

        (listeningElement as UIButton<string>).SetText("Sort: " + _mode);

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

        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            RedoList();
    }

    private void RedoList()
    {
        _list.Clear();
        PopulateGrid();
        Recalculate();
        _list.UpdateOrder();
    }

    private void PopulateGrid()
    {
        for (int i = 1; i < ItemLoader.ItemCount; ++i)
        {
            Item item = new(i);

            bool notSearch = _inputText.currentValue == string.Empty;

            if (UICustomOrigin.InvalidItemToAdd(item) || !notSearch && !item.Name.Contains(_inputText.currentValue, StringComparison.OrdinalIgnoreCase))
                continue;

            var image = new UIItemContainer(item);
            image.OnLeftClick += Image_OnLeftClick;
            _list.Add(image);

            image.Append(new UIText(item.Name) { VAlign = 0.5f, Left = StyleDimension.FromPixels(46) });
        }
    }

    private void Image_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
    {
        var container = listeningElement as UIItemContainer;
        _clickItem(container);
    }
}
