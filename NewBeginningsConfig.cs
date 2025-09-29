using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace NewBeginnings;

public class NewBeginningsConfig : ModConfig
{
    public enum Scaling : byte
    {
        Scaled,
        Flat,
        Relative
    }

    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(Scaling.Scaled)]
    public Scaling HealthScaling { get; set; }

    [DefaultValue(300)]
    [Range(0, 1600)]
    public float MrPlaguesButtonOffsetY { get; set; }

    public override void OnChanged()
    {
        if (MrPlaguesButtonOffsetY > Main.screenHeight / 2)
            MrPlaguesButtonOffsetY = Main.screenHeight / 2;
    }
}