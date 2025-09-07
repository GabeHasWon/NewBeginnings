using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.PlayerBackgrounds;
using System.Collections.Generic;
using NewBeginnings.Content.Items.Icons;
using System.Linq;
using Terraria.Localization;
using NewBeginnings.Common.UnlockabilitySystem;
using NewBeginnings.Common.PlayerBackgrounds.Containers;
using NewBeginnings.Common.Crossmod;

namespace NewBeginnings.Common.UI;

internal class UIOriginSelection : UIState
{
    public enum SortMode
    {
        Default,
        Difficulty,
        DifficultyReverse,
        Alphabetically,
        AlphabeticallyReverse,
        Mod,
        ModReverse
    }

    private const int TotalWidth = 800;
    private const int BackgroundListWidth = 220;

    private static SortMode _sortMode = SortMode.Default;

    private readonly Player _player;
    private readonly MouseEvent _return;

    private UIText _statsText;
    private UIText _descFlavour;
    private UIText _descText;
    private UIElement _itemContainer;
    private UIImageButton _sortButton;

    private static float BaseHeight => 554f / 1017 * Main.screenHeight;

    public UIOriginSelection(Player player, MouseEvent returnAction)
    {
        _player = player;
        _return = returnAction;

        Setup();
    }

    private void Setup()
    {
        _sortMode = SortMode.Default;

        RemoveAllChildren();

        var mainElement = new UIElement
        {
            Width = StyleDimension.FromPixels(TotalWidth),
            Height = StyleDimension.FromPixels(BaseHeight),
            Top = StyleDimension.FromPixels(220f),
            HAlign = 0.5f,
            VAlign = 0f
        };
        mainElement.SetPadding(0f);
        Append(mainElement);

        Color panelColor = new Color(33, 43, 79) * 0.8f;

        var panel = new UIPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f),
            Top = StyleDimension.FromPixels(50f),
            BackgroundColor = panelColor
        };

        mainElement.Append(panel);

        UIImageButton closeButton = new(ModContent.Request<Texture2D>("NewBeginnings/Assets/Textures/UI/OriginBack"))
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixelsAndPercent(-40, 1f),
            Top = StyleDimension.FromPixelsAndPercent(0, 0)
        };

        closeButton.OnLeftClick += _return;
        panel.Append(closeButton);

        _sortButton = new UIImageButton(ModContent.Request<Texture2D>("NewBeginnings/Assets/Textures/UI/OriginSort"))
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(6),
        };

        _sortButton.Append(new UIText(Language.GetText("Mods.NewBeginnings.UI.SortBy.Line").WithFormatArgs(Language.GetText("Mods.NewBeginnings.UI.SortBy." + _sortMode)), 0.8f)
        {
            VAlign = 0.5f,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f),
            Left = StyleDimension.FromPixels(32),
            Top = StyleDimension.FromPixels(6),
        });

        panel.Append(_sortButton);

        AddCharacterPreview(panel, _player);

        var descriptionPanel = new UIPanel
        {
            Width = StyleDimension.FromPixelsAndPercent(-BackgroundListWidth - 10, 1f),
            HAlign = 1f,
            Height = StyleDimension.FromPixelsAndPercent(-40, 1f),
            Top = StyleDimension.FromPixels(30),
            BackgroundColor = panelColor
        };
        panel.Append(descriptionPanel);

        BuildDescription(descriptionPanel);

        var originsListPanel = new UIPanel
        {
            Width = StyleDimension.FromPixels(BackgroundListWidth),
            HAlign = 0,
            Height = StyleDimension.FromPixelsAndPercent(-40, 1f),
            Top = StyleDimension.FromPixels(30),
            BackgroundColor = panelColor
        };
        panel.Append(originsListPanel);

        BuildBackgroundSelections(originsListPanel);

        string str = GetSplashText();
        UIText text = new(str, 1 - str.Length / 200f)
        {
            HAlign = 0.88f,
            Top = StyleDimension.FromPixels(10),
        };

        text.OnUpdate += (UIElement affectedElement) => text.TextColor = Main.MouseTextColorReal;
        panel.Append(text);
    }

    private static string GetSplashText()
    {
        const int OurMax = 30;

        int random = Main.rand.Next(OurMax + OriginCalls._crossModSplashTexts.Count);
        return (random >= OurMax ? OriginCalls._crossModSplashTexts[random - OurMax] : Language.GetText("Mods.NewBeginnings.UI.Splash." + random)).Value;
    }

    internal static void AddCharacterPreview(UIPanel panel, Player player)
    {
        UICharacter element = new(player, animated: true, hasBackPanel: false, 1.5f)
        {
            Width = StyleDimension.FromPixels(80f),
            Height = StyleDimension.FromPixelsAndPercent(80f, 0f),
            Top = StyleDimension.FromPixelsAndPercent(-80, 0f),
            VAlign = 0f,
            HAlign = 0.5f
        };

        element.OnUpdate += (UIElement uiChar) => player.gravDir = player.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Australian") ? -1 : 1;
        panel.Append(element);
    }

    private void BuildDescription(UIPanel panel)
    {
        var list = new UIList() //List for use in the description
        {
            Width = StyleDimension.FromPercent(0.98f),
            Height = StyleDimension.FromPixelsAndPercent(0, 1f),
            PaddingLeft = 8,
            PaddingRight = 8,
            ListPadding = -20
        };
        panel.Append(list);

        var scrollBar = new UIScrollbar() //Scrollbar for above list
        {
            HAlign = 1f,
            Height = StyleDimension.FromPixelsAndPercent(-8, 1f),
            Top = StyleDimension.FromPixels(4),
        };

        list.SetScrollbar(scrollBar);
        panel.Append(scrollBar);

        var bgData = _player.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;
        _statsText = new UIText(GetStatsText(bgData))
        {
            Top = StyleDimension.FromPercent(0.1f),
            Width = StyleDimension.FromPixelsAndPercent(-8, 1f),
            Height = StyleDimension.FromPixels(44),
            IsWrapped = false,
            MarginTop = 8
        };
        list.Add(_statsText);

        _descFlavour = new UIText(bgData.Flavour)
        {
            Top = StyleDimension.FromPercent(0.1f),
            Width = StyleDimension.FromPixelsAndPercent(-8, 1f),
            Height = StyleDimension.FromPixels(10),
            IsWrapped = true,
            MarginTop = 8
        };
        list.Add(_descFlavour);

        _itemContainer = new();
        SetItemList(bgData, true);
        list.Add(_itemContainer);

        _descText = new UIText(bgData.Description, 0.9f)
        {
            TextColor = Color.Gray,
            Top = StyleDimension.FromPercent(0.1f),
            Width = StyleDimension.FromPixelsAndPercent(-8, 1f),
            Height = StyleDimension.FromPixels(10),
            IsWrapped = true,
            MarginTop = 8
        };
        list.Add(_descText);
    }

    private void SetItemList(PlayerBackgroundData data, bool resetItemContainer = false)
    {
        var descItemHeight = StyleDimension.FromPixels(data.DisplayItemCount() == 0 ? 0 : 54 + 32 * (data.DisplayItemCount() / 12f));

        if (resetItemContainer) //Creates a new item container
        {
            _itemContainer = new UIElement()
            {
                Width = StyleDimension.FromPercent(1),
                Height = descItemHeight,
                Top = StyleDimension.FromPixels(-20),
                HAlign = 0.5f,
            };
        }
        else //Adjusts container to fit new height
        {
            _itemContainer.Height = descItemHeight;
            _itemContainer.HAlign = 0.5f;
            _itemContainer.Recalculate();
        }

        int offset = 0;
        float yOffset = 0;
        int id = 0;
        List<(int type, int stack)> items = [];
        (int sword, int pick, int axe) = (data.Misc.CopperShortswordReplacement, data.Misc.CopperPickaxeReplacement, data.Misc.CopperAxeReplacement);

        if (sword != -1)
            items.Add((sword, 1));

        if (pick != -1)
            items.Add((pick, 1));

        if (axe != -1)
            items.Add((axe, 1));

        (int head, int body, int legs) = (data.Equip.Head, data.Equip.Body, data.Equip.Legs);

        if (head > ItemID.None)
            items.Add((head, 1));

        if (body > ItemID.None)
            items.Add((body, 1));

        if (legs > ItemID.None)
            items.Add((legs, 1));

        foreach (int item in data.Equip.Accessories)
            items.Add((item, 1));

        items.AddRange([.. data.Inventory]);

        foreach (var (type, stack) in items)
        {
            Item item = new(type) { stack = stack, scale = 0.85f };

            var slotBG = new UIImage(TextureAssets.InventoryBack) //Slot background
            {
                Left = StyleDimension.FromPixels(-6 + offset * 42),
                Top = StyleDimension.FromPixels(-2 + yOffset * 38),
                ImageScale = 0.77f,
            };

            var icon = new UIItemIcon(item, false) //Item itself
            {
                Left = StyleDimension.FromPixels(10),
                Top = StyleDimension.FromPixels(9),
            };

            if (stack > 1)
            {
                var stackUI = new UIText(stack.ToString(), 0.65f, false)
                {
                    Left = StyleDimension.FromPixels(4),
                    Top = StyleDimension.FromPixels(18)
                };

                icon.Append(stackUI);
            }

            slotBG.Append(icon);

            float width = FontAssets.ItemStack.Value.MeasureString(item.HoverName).X * 0.8f;

            UIText name = new(item.HoverName, 0.8f)
            {
                Width = StyleDimension.FromPixels(42),
                Top = StyleDimension.FromPixels(-10),
                HAlign = 0.5f,
                DynamicallyScaleDownToWidth = true,
            };

            if (id == 0 || id % 5 == 1)
            {
                name.Top = StyleDimension.FromPixels(-10);
                name.HAlign = 0f;
                name.Left = StyleDimension.FromPixels(6);
            }
            else if (id % 5 == 0)
            {
                name.Top = StyleDimension.FromPixels(-10);
                name.HAlign = 1f;
            }

            name.DynamicallyScaleDownToWidth = true;

            icon.OnMouseOver += (UIMouseEvent evt, UIElement listeningElement) => slotBG.Append(name);
            icon.OnMouseOut += (UIMouseEvent evt, UIElement listeningElement) => slotBG.RemoveChild(name);

            _itemContainer.Append(slotBG);

            offset++;
            if (10 + offset * 32 > TotalWidth - BackgroundListWidth + 20) //ew hardcoding but nothing works
            {
                offset = 0;
                yOffset++;
            }

            id++;
        }
    }

    private static string GetStatsText(PlayerBackgroundData bgData)
    {
        string stats = $"[i:{ItemID.Heart}]{bgData.Misc.MaxLife} [i:{ItemID.Star}]{bgData.Misc.AdditionalMana}";
        string stars = string.Empty;

        for (int i = 0; i < 5; ++i)
            stars += bgData.Misc.Stars > i ? $"[i:{ModContent.ItemType<LightStar>()}]" : $"[i:{ModContent.ItemType<DimStar>()}]";

        return stars + "   " + stats;
    }

    /// <summary>Builds the origin list and buttons.</summary>
    private void BuildBackgroundSelections(UIPanel container)
    {
        UIList allBGButtons = new() //List of all background buttons
        {
            Width = StyleDimension.FromPercent(1),
            Height = StyleDimension.FromPixelsAndPercent(0, 1f),
            ListPadding = 4,
        };

        container.Append(allBGButtons);

        UIScrollbar scroll = new() //Scrollbar for above list
        {
            HAlign = 1f,
            Height = StyleDimension.FromPixelsAndPercent(-8, 1f),
            Top = StyleDimension.FromPixels(4)
        };

        allBGButtons.SetScrollbar(scroll);
        container.Append(scroll);

        List<(PlayerBackgroundData data, UIColoredImageButton button)> buttons = [];

        foreach (var item in PlayerBackgroundDatabase.playerBackgroundDatas) //Adds every background into the list as a button
        {
            if (!item.Delegates.ClearCondition())
                continue;

            var asset = PlayerBackgroundDatabase.backgroundIcons[PlayerBackgroundDatabase.backgroundIcons.ContainsKey(item.Identifier) ? item.Identifier : "Default"];
            UIColoredImageButton currentBGButton = new(asset)
            {
                Width = StyleDimension.FromPercent(0.9f),
                Height = StyleDimension.FromPixels(36),
                Left = StyleDimension.FromPixels(-64),
                Top = StyleDimension.FromPixels(8),
                MarginRight = 4f,
                MarginTop = 4f,
            };
            currentBGButton.SetColor(Color.Gray);

            currentBGButton.OnLeftMouseDown += (UIMouseEvent evt, UIElement listeningElement) => //Click event
                BackgroundButtonClick(allBGButtons, item, currentBGButton);

            currentBGButton.OnMouseOver += (UIMouseEvent evt, UIElement listeningElement) =>
            {
                item.ApplyArmor(_player);
                item.ApplyAccessories(_player, true);

                currentBGButton.SetColor(new Color(220, 220, 220));
            };

            currentBGButton.OnMouseOut += (UIMouseEvent evt, UIElement listeningElement) =>
            {
                var bgData = _player.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;

                if (!_player.GetModPlayer<PlayerBackgroundPlayer>().HasBG() || bgData.Name != item.Name)
                {
                    if (bgData.Identifier == "Custom")
                        bgData = Custom.GetCustomBackground(_player, true);

                    bgData.ApplyArmor(_player);
                    bgData.ApplyAccessories(_player, true);
                    _player.GetModPlayer<PlayerBackgroundPlayer>().SetBackground(bgData); //Sets the player's background.

                    currentBGButton.SetColor(Color.Gray);
                }
                else
                    currentBGButton.SetColor(Color.White);
            };

            float textSize = 1.2f;

            if (LanguageManager.Instance.ActiveCulture.Name == "ru-RU")
            {
                textSize = 1f;
            }

            UIText bgName = new(item.Name, textSize) // Background's name
            {
                HAlign = 0f,
                VAlign = 0.5f,
                Left = StyleDimension.FromPixels(114)
            };

            currentBGButton.Append(bgName);
            buttons.Add((item, currentBGButton));
        }

        foreach (var (_, button) in buttons)
            allBGButtons.Add(button);

        if (UnlockSaveData.Unlocked("Renewed"))
            AddCustomBGButton(allBGButtons, buttons);

        SetSort(allBGButtons, buttons, _sortButton);
    }

    private void AddCustomBGButton(UIList allBGButtons, List<(PlayerBackgroundData, UIColoredImageButton button)> buttons)
    {
        var asset = PlayerBackgroundDatabase.backgroundIcons["Custom"];
        UIColoredImageButton customBGButton = new(asset)
        {
            Width = StyleDimension.FromPercent(0.9f),
            Height = StyleDimension.FromPixels(36),
            Left = StyleDimension.FromPixels(-64),
            Top = StyleDimension.FromPixels(8),
            MarginRight = 4f,
            MarginTop = 4f,
        };
        customBGButton.SetColor(Color.Gray);
        customBGButton.OnLeftClick += CurrentBGButton_OnClick;
        allBGButtons.Add(customBGButton);

        UIText bgName = new(Language.GetTextValue("Mods.NewBeginnings.Origins.Custom.DisplayName"), 1.2f) //Background's name
        {
            HAlign = 0f,
            VAlign = 0.5f,
            Left = StyleDimension.FromPixels(114)
        };
        customBGButton.Append(bgName);

        // Dummy data for the random since it doesn't strictly have a useful data; solely sets stuff so that everything is optimal for sorting (and doesn't crash).
        PlayerBackgroundData data = new()
        {
            Misc = new(sortPriority: -500, stars: 500),
            Name = Language.GetText("Mods.NewBeginnings.Origins.Random.DisplayName")
        };
        buttons.Add((data, customBGButton));
    }

    private void CurrentBGButton_OnClick(UIMouseEvent evt, UIElement listeningElement) => 
        Main.MenuUI.SetState(new UICustomOrigin(_player, (evt, listeningElement) => Main.MenuUI.SetState(this)));

    private PlayerBackgroundData BackgroundButtonClick(UIList allBGButtons, PlayerBackgroundData item, UIColoredImageButton currentBGButton)
    {
        PlayerBackgroundData useData = item.Identifier == "Random" ? GetValidRandomOrigin() : item; //Hardcoding for random, sucks but eh
        _descFlavour.SetText(item.Identifier != "Random" ? useData.Flavour : item.Flavour); //Changes the UIText's value to use the bg's description
        _descText.SetText(item.Identifier != "Random" ? useData.Description : item.Description); //Changes the UIText's value to use the bg's description

        if (item.Identifier != "Random")
            _statsText.SetText(GetStatsText(useData));
        else
            _statsText.SetText($"[i:{ItemID.Heart}]??? [i:{ItemID.Star}]???");

        _itemContainer.RemoveAllChildren();

        if (item.Identifier != "Random")
            SetItemList(useData);

        item.ApplyArmor(_player);
        item.ApplyAccessories(_player, true);
        _player.GetModPlayer<PlayerBackgroundPlayer>().SetBackground(useData); //...and sets it.

        foreach (var button in allBGButtons.Where(x => x is UIColoredImageButton))
            (button as UIColoredImageButton).SetColor(Color.Gray);

        currentBGButton.SetColor(Color.White); //"Selects" the button visually.
        return item;
    }

    private static PlayerBackgroundData GetValidRandomOrigin()
    {
        var list = PlayerBackgroundDatabase.playerBackgroundDatas.SkipLast(1).ToList();
        var origin = Main.rand.Next(list);

        while (!origin.Delegates.ClearCondition.Invoke())
            origin = Main.rand.Next(list);

        return origin;
    }

    private static void SetSort(UIList allBGButtons, List<(PlayerBackgroundData data, UIColoredImageButton button)> buttons, UIImageButton sortButton)
    {
        ResortBGButtons(allBGButtons, buttons);

        sortButton.OnLeftClick += (evt, listeningElement) =>
        {
            _sortMode++;

            if (_sortMode > SortMode.AlphabeticallyReverse)
                _sortMode = SortMode.Default;

            (sortButton.Children.First() as UIText).SetText(Language.GetText("Mods.NewBeginnings.UI.SortBy.Line")
                .WithFormatArgs(Language.GetText("Mods.NewBeginnings.UI.SortBy." + _sortMode)));
            string path = "NewBeginnings/Assets/Textures/UI/";

            sortButton.SetImage(ModContent.Request<Texture2D>(path + _sortMode switch
            {
                SortMode.Default => "OriginSort",
                SortMode.Difficulty => "OriginSortStar",
                SortMode.DifficultyReverse => "OriginSortDifficultyReverse",
                SortMode.Alphabetically => "OriginSortAlphabet",
                SortMode.AlphabeticallyReverse => "OriginSortAlphabetRev",
                SortMode.Mod => "OriginSortMod",
                _ => "OriginSortModRev"
            }));

            sortButton.Width = StyleDimension.FromPixels(32);
            sortButton.Height = StyleDimension.FromPixels(32);

            ResortBGButtons(allBGButtons, buttons);

            sortButton.Recalculate();
        };
    }

    private static void ResortBGButtons(UIList allBGButtons, List<(PlayerBackgroundData data, UIColoredImageButton button)> buttons)
    {
        //This is some of the ugliest nonsense I've ever written
        allBGButtons.ManualSortMethod = (list) => list.Sort((self, other) =>
        {
            var selfData = buttons.Find(x => x.button == self).data; // Find priority by finding the value that has the given button as Item2
            var otherData = buttons.Find(x => x.button == other).data; // for both the current and next button

            if (_sortMode is SortMode.Mod or SortMode.ModReverse)
            {
                int mySortPriority = selfData.Misc.SortPriority; 
                int otherSortPriority = otherData.Misc.SortPriority;

                return otherSortPriority.CompareTo(mySortPriority);
            }
            else if (_sortMode is SortMode.Difficulty or SortMode.DifficultyReverse)
            {
                int mySortPriority = selfData.Misc.Stars;
                int otherSortPriority = otherData.Misc.Stars;
                int sort = mySortPriority.CompareTo(otherSortPriority);

                if (_sortMode == SortMode.DifficultyReverse)
                    sort *= -1;

                return sort;
            }
            else if (_sortMode is SortMode.Alphabetically or SortMode.AlphabeticallyReverse)
            {
                string myName = selfData.Name.Value;
                string otherName = otherData.Name.Value;
                int sort = myName.CompareTo(otherName);

                if (_sortMode == SortMode.AlphabeticallyReverse)
                    sort *= -1;

                return sort;
            }
            else
            {
                int mySortPriority = selfData.Misc.SortPriority;
                int otherSortPriority = otherData.Misc.SortPriority;

                return otherSortPriority.CompareTo(mySortPriority);
            }
        });

        allBGButtons.UpdateOrder(); //Reorder the list according to the above manual sort method
    }
}
