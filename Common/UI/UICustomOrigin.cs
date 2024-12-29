using Microsoft.Xna.Framework;
using NewBeginnings.Content.Items.Icons;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace NewBeginnings.Common.UI;

internal class UICustomOrigin : UIState
{
    private readonly Player _player;
    private readonly MouseEvent _return;

    private int RealLife => (int)MathHelper.Clamp(_maxLife * 400, 20, 400);
    private int RealMana => (int)(_maxMana * 200);

    private float _maxLife = 0;
    private float _maxMana = 0;
    private float _stars = 0;

    private UIColoredSlider _lifeSlider = null;
    private UIColoredSlider _manaSlider = null;

    public UICustomOrigin(Player player, MouseEvent returnAction)
    {
        _player = player;
        _return = returnAction;

        Setup();
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
            Width = StyleDimension.FromPixelsAndPercent(0, 0.75f),
            Height = StyleDimension.FromPixelsAndPercent(-44, 1f),
            VAlign = 1f
        };
        panel.Append(grid);
    }

    public void OnClickItem(UIItemContainer container)
    {

    }

    internal static bool InvalidItemToAdd(Item it) => it.createTile >= TileID.Dirt || it.createWall > WallID.None || it.questItem || it.ammo > AmmoID.None && it.damage > 15 
        || it.damage > 15 || it.defense > 5 || ItemID.Sets.BossBag[it.type] || ItemID.Sets.ItemsThatShouldNotBeInInventory[it.type] || it.type <= ItemID.None
        || ItemID.Sets.Deprecated[it.type] || it.pick > 80 || it.axe > 15 || it.hammer > 50 || it.wingSlot > 0 || it.type == ItemID.BoringBow || ItemID.Sets.IsAPickup[it.type]
        || it.type == ItemID.ManaCloakStar;

    private void BuildSliders(UIPanel panel)
    {
        _lifeSlider = MakeSlider(() => _maxLife, x => _maxLife = x, x => Color.Lerp(Color.White, Color.Red, x));
        _lifeSlider.Width = StyleDimension.FromPixels(40);
        _lifeSlider.Height = StyleDimension.FromPixels(12);
        _lifeSlider.Left = StyleDimension.FromPixels(192);
        _lifeSlider.Top = StyleDimension.FromPixels(0);

        var healthText = new UIText("Health", 1f)
        {
            Left = StyleDimension.FromPercent(1),
            Top = StyleDimension.FromPixels(8),
            VAlign = 0.5f,
        };

        healthText.OnUpdate += (_) => healthText.SetText($"Life ({RealLife:#0}/400)");
        _lifeSlider.Append(healthText);
        panel.Append(_lifeSlider);

        _manaSlider = MakeSlider(() => _maxMana, x => _maxMana = x, (x) => Color.Lerp(Color.White, Color.Blue, x));
        _manaSlider.Width = StyleDimension.FromPixels(40);
        _manaSlider.Height = StyleDimension.FromPixels(0);
        _manaSlider.Left = StyleDimension.FromPixels(518);

        var manaText = new UIText("Mana 0/200", 1f)
        {
            Left = StyleDimension.FromPixelsAndPercent(7, 1),
            Top = StyleDimension.FromPixels(8),
        };

        manaText.OnUpdate += (_) => manaText.SetText($"Mana ({RealMana:#0}/200)");
        _manaSlider.Append(manaText);
        panel.Append(_manaSlider);
    }

    private static UIColoredSlider MakeSlider(Func<float> returnFunction, Action<float> updateValue, Func<float, Color> colorMod) 
        => new(LocalizedText.Empty, returnFunction, updateValue, () =>
        {
            float value = UILinksInitializer.HandleSliderHorizontalInput(returnFunction.Invoke(), 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
            updateValue(value);
        }, colorMod, Color.Transparent);

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
