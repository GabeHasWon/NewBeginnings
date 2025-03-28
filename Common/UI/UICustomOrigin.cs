﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NewBeginnings.Common.PlayerBackgrounds;
using NewBeginnings.Common.PlayerBackgrounds.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace NewBeginnings.Common.UI;

internal class UICustomOrigin : UIState
{
    private static Item AirItem
    {
        get
        {
            var item = new Item(0);
            item.TurnToAir();
            return item;
        }
    }

    private readonly Player _player;
    private readonly MouseEvent _return;

    private int RealLife => (int)MathHelper.Clamp(_maxLife * 400, 20, 400);
    private int RealMana => (int)(_maxMana * 200);

    private readonly HashSet<int> _accIds = [];
    private readonly List<Item> _hotbar = [];

    private float _maxLife = 0;
    private float _maxMana = 0;
    private bool _setData = false;

    private UIColoredSlider _lifeSlider = null;
    private UIColoredSlider _manaSlider = null;

    private UIItemContainer _helmetSlot = null;
    private UIItemContainer _bodySlot = null;
    private UIItemContainer _legsSlot = null;
    private UIList _hotbarList = null;
    private UIList _accList = null;

    public UICustomOrigin(Player player, MouseEvent returnAction)
    {
        _player = player;
        _return = returnAction;

        if (player.GetModPlayer<PlayerBackgroundPlayer>().CustomOriginData is not null)
            _setData = true;

        Setup();
    }

    internal static string Localize(string key) => Language.GetTextValue("Mods.NewBeginnings.UI.CustomOrigin." + key);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            _return.Invoke(null, null);

        _player.armor[0] = _helmetSlot.Item;
        _player.armor[1] = _bodySlot.Item;
        _player.armor[2] = _legsSlot.Item;
        _player.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData = new Custom();

        ref var customData = ref _player.GetModPlayer<PlayerBackgroundPlayer>().CustomOriginData;
        customData ??= CustomOriginData.Empty;

        customData.Helmet = new ItemPair(_helmetSlot.Item.type, 1);
        customData.Body = new ItemPair(_bodySlot.Item.type, 1);
        customData.Legs = new ItemPair(_legsSlot.Item.type, 1);

        customData.life = RealLife;
        customData.mana = RealMana;

        int count = 0;

        foreach (int id in _accIds)
        {
            customData.Accessories[count] = new ItemPair(id, 1);
            _player.armor[3 + count++] = ContentSamples.ItemsByType[id];
        }

        for (int i = 3 + count; i < 8; ++i)
            _player.armor[i] = AirItem;

        for (int i = 0; i < _hotbar.Count; ++i)
            customData.Hotbar[i] = new(_hotbar[i].type, _hotbar[i].stack);

        _player.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData = new Custom();
    }

    private void Setup()
    {
        var panel = new UIPanel()
        {
            Width = StyleDimension.FromPixels(720),
            Height = StyleDimension.FromPixels(460),
            HAlign = 0.5f,
            VAlign = 0.65f,
        };
        Append(panel);

        panel.Append(new UIText(GetSplash()) { VAlign = 1f, Top = StyleDimension.FromPixels(34), HAlign = 0.5f });

        BuildSlots(panel);
        BuildSliders(panel);

        var returnButton = new UIButton<string>("x")
        {
            Width = StyleDimension.FromPixels(40),
            Height = StyleDimension.FromPixels(40)
        };

        returnButton.OnLeftClick += (_, _) => _return.Invoke(null, null);
        panel.Append(returnButton);

        UIOriginSelection.AddCharacterPreview(panel, _player);

        var grid = new UISearchItemGrid(OnClickItem)
        {
            Width = StyleDimension.FromPixelsAndPercent(-10, 0.75f),
            Height = StyleDimension.FromPixelsAndPercent(-44, 1f),
            VAlign = 1f
        };
        panel.Append(grid);

        if (_setData)
        {
            ref var customData = ref _player.GetModPlayer<PlayerBackgroundPlayer>().CustomOriginData;

            _helmetSlot.OverrideItem(new(customData.Helmet.Id));
            _bodySlot.OverrideItem(new(customData.Body.Id));
            _legsSlot.OverrideItem(new(customData.Legs.Id));

            for (int i = 0; i < customData.Hotbar.Length; i++)
            {
                ItemPair pair = customData.Hotbar[i];

                if (pair.Id != ItemID.None && pair.Stack != 0)
                    OnClickItem(new UIItemContainer(new Item(pair.Id, pair.Stack)));
            }

            for (int i = 0; i < customData.Accessories.Length; i++)
            {
                ItemPair pair = customData.Accessories[i];

                if (pair.Id != ItemID.None && pair.Stack != 0)
                    OnClickItem(new UIItemContainer(new Item(pair.Id, pair.Stack)));
            }
        }
    }

    private void BuildSlots(UIPanel panel)
    {
        UIPanel hotbarPanel = new()
        {
            Width = StyleDimension.FromPixels(64),
            Height = StyleDimension.FromPixelsAndPercent(-44, 1),
            Left = StyleDimension.FromPixels(516),
            VAlign = 1f,
        };

        hotbarPanel.Append(new UIText(Localize("Inv")) { Top = StyleDimension.FromPixels(-20) });

        panel.Append(hotbarPanel);

        _hotbarList = new UIList()
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
        };

        hotbarPanel.Append(_hotbarList);

        UIPanel accPanel = new()
        {
            Width = StyleDimension.FromPixels(54),
            Height = StyleDimension.FromPixelsAndPercent(-44, 1),
            Left = StyleDimension.FromPixels(638),
            VAlign = 1f,
        };

        accPanel.Append(new UIText(Localize("Acc")) { Top = StyleDimension.FromPixels(-20) });
        panel.Append(accPanel);

        _accList = new UIList()
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
        };

        accPanel.Append(_accList);

        _helmetSlot = new UIItemContainer(AirItem)
        {
            Left = StyleDimension.FromPixels(584),
            Top = StyleDimension.FromPixels(64)
        };

        _helmetSlot.Append(new UIText(Localize("Helmet")) { Top = StyleDimension.FromPixels(-20) });
        _helmetSlot.OnLeftClick += (_, self) => _helmetSlot.OverrideItem(AirItem);
        panel.Append(_helmetSlot);

        _bodySlot = new UIItemContainer(AirItem)
        {
            Left = StyleDimension.FromPixels(584),
            Top = StyleDimension.FromPixels(120)
        };

        _bodySlot.OnLeftClick += (_, self) => _bodySlot.OverrideItem(AirItem);
        _bodySlot.Append(new UIText(Localize("Body")) { Top = StyleDimension.FromPixels(-20) });
        panel.Append(_bodySlot);

        _legsSlot = new UIItemContainer(AirItem)
        {
            Left = StyleDimension.FromPixels(584),
            Top = StyleDimension.FromPixels(176)
        };

        _legsSlot.OnLeftClick += (_, _) => _legsSlot.OverrideItem(AirItem);
        _legsSlot.Append(new UIText(Localize("Legs")) { Top = StyleDimension.FromPixels(-20) });
        panel.Append(_legsSlot);
    }

    public void OnClickItem(UIItemContainer container)
    {
        UIItemContainer newContainer = new(new(container.Item.type));

        if (container.Item.headSlot > -1)
            _helmetSlot.OverrideItem(newContainer.Item);
        else if (container.Item.bodySlot > -1)
            _bodySlot.OverrideItem(newContainer.Item);
        else if (container.Item.legSlot > -1)
            _legsSlot.OverrideItem(newContainer.Item);
        else if (container.Item.accessory && !_accIds.Contains(newContainer.Item.type) && _accList.Count < 5)
        {
            newContainer.OnLeftClick += RemoveItemFromAcc;
            _accList.Add(newContainer);
            _accIds.Add(container.Item.type);
        }
        else if (_hotbarList.Count < 10)
        {
            newContainer.OnLeftClick += RemoveItemFromList;
            newContainer.SetCanStack(true);

            _hotbarList.Add(newContainer);
            _hotbar.Add(newContainer.Item);
        }
    }

    private void RemoveItemFromList(UIMouseEvent evt, UIElement listeningElement)
    {
        _hotbarList.Remove(listeningElement);
        _hotbar.Remove(_hotbar.First(x => x.type == (listeningElement as UIItemContainer).Item.type));
    }

    private void RemoveItemFromAcc(UIMouseEvent evt, UIElement listeningElement)
    {
        _accList.Remove(listeningElement);
        _accIds.Remove((listeningElement as UIItemContainer).Item.type);
    }

    internal static bool InvalidItemToAdd(Item it) => it.createTile >= TileID.Dirt || it.createWall > WallID.None || it.questItem || it.ammo > AmmoID.None && it.damage > 15 
        || it.damage > 15 || it.defense > 3 || ItemID.Sets.BossBag[it.type] || ItemID.Sets.ItemsThatShouldNotBeInInventory[it.type] || it.type <= ItemID.None
        || it.pick > 80 || it.axe > 15 || it.hammer > 50 || it.wingSlot > 0 || it.type == ItemID.BoringBow || ItemID.Sets.IsAPickup[it.type]
        || it.type == ItemID.ManaCloakStar || it.mountType != -1;

    private void BuildSliders(UIPanel panel)
    {
        _lifeSlider = MakeSlider(() => _maxLife, x => _maxLife = x, x => Color.Lerp(Color.White, Color.Red, x));
        _lifeSlider.Width = StyleDimension.FromPixels(40);
        _lifeSlider.Height = StyleDimension.FromPixels(12);
        _lifeSlider.Left = StyleDimension.FromPixels(192);
        _lifeSlider.Top = StyleDimension.FromPixels(0);

        var healthText = new UIText(Language.GetText("Mods.NewBeginnings.UI.CustomOrigin.Health").Format(RealLife.ToString("#0")), 1f)
        {
            Left = StyleDimension.Fill,
            Top = StyleDimension.FromPixels(8),
            VAlign = 0.5f,
        };

        healthText.OnUpdate += (_) => healthText.SetText(Language.GetText("Mods.NewBeginnings.UI.CustomOrigin.Health").Format(RealLife.ToString("#0")));
        _lifeSlider.Append(healthText);
        panel.Append(_lifeSlider);

        _manaSlider = MakeSlider(() => _maxMana, x => _maxMana = x, (x) => Color.Lerp(Color.White, Color.Blue, x));
        _manaSlider.Width = StyleDimension.FromPixels(40);
        _manaSlider.Height = StyleDimension.FromPixels(0);
        _manaSlider.Left = StyleDimension.FromPixels(518);

        var manaText = new UIText(Language.GetText("Mods.NewBeginnings.UI.CustomOrigin.Mana").Format(RealMana.ToString("#0")), 1f)
        {
            Left = StyleDimension.FromPixelsAndPercent(7, 1),
            Top = StyleDimension.FromPixels(8),
        };

        manaText.OnUpdate += (_) => manaText.SetText(Language.GetText("Mods.NewBeginnings.UI.CustomOrigin.Mana").Format(RealMana.ToString("#0")));
        _manaSlider.Append(manaText);
        panel.Append(_manaSlider);
    }

    private static UIColoredSlider MakeSlider(Func<float> returnFunction, Action<float> updateValue, Func<float, Color> colorMod) 
        => new(LocalizedText.Empty, returnFunction, updateValue, () =>
        {
            float value = UILinksInitializer.HandleSliderHorizontalInput(returnFunction.Invoke(), 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
            updateValue(value);
        }, colorMod, Color.Transparent);

    private static string GetSplash()
    {
        int random = Main.rand.Next(16);
        return Localize("Splashes." + random);
    }
}
