using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace NewBeginnings.Common.UI;

internal class UISearchItemGrid : UIPanel
{
    private UIGrid _grid = null;
    private UIEditableText _inputText = null;

    public UISearchItemGrid()
    {
        _inputText = new UIEditableText(InputType.Text, "Search for an item")
        {
            Width = StyleDimension.FromPixelsAndPercent(-28, 1),
            Height = StyleDimension.FromPixels(20)
        };

        Append(_inputText);

        _grid = new()
        {
            Width = StyleDimension.FromPixelsAndPercent(-28, 1),
            Height = StyleDimension.FromPixelsAndPercent(-40, 1f),
            VAlign = 1f,
        };

        Append(_grid);

        UIScrollbar bar = new()
        {
            Width = StyleDimension.FromPixels(24),
            Height = StyleDimension.FromPixelsAndPercent(-40, 1f),
            HAlign = 1f,
            VAlign = 1f,
        };

        _grid.SetScrollbar(bar);
        Append(bar);

        PopulateGrid();
    }

    private void PopulateGrid()
    {
        for (int i = 1; i < ItemLoader.ItemCount; ++i)
        {
            Item item = new(i);

            if (UICustomOrigin.InvalidItemToAdd(item))
                continue;

            Main.instance.LoadItem(i);
            var image = new UIItemIcon(item, false);
            _grid.Add(image);
        }
    }
}
