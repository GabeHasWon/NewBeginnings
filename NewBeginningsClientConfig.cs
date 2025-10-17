using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace NewBeginnings;

internal class NewBeginningsClientConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(300f)]
    [Range(0f, 1600f)]
    public float MrPlaguesButtonOffsetY { get; set; }

    [DefaultValue(false)]
    [ReloadRequired]
    public bool HidePlayerOrigins { get; set; }

    public override void OnChanged()
    {
        if (MrPlaguesButtonOffsetY > Main.screenHeight / 2)
            MrPlaguesButtonOffsetY = Main.screenHeight / 2;
    }
}
