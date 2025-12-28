using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace NewBeginnings.Common.UnlockabilitySystem.Achievements;

public class RenewedAchievement : ModAchievement
{
    internal static CustomFlagCondition Condition = null;

    public override void SetStaticDefaults() => Condition = AddCondition();
}
