using System.ComponentModel;
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
}