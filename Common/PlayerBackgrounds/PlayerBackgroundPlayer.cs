using Microsoft.Xna.Framework;
using NewBeginnings.Common.PlayerBackgrounds.Containers;
using NewBeginnings.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NewBeginnings.Common.PlayerBackgrounds;

/// <summary>Handles player-related background functions (i.e. inventory).</summary>
internal class PlayerBackgroundPlayer : ModPlayer
{
    public PlayerBackgroundData BackgroundData = new();
    public CustomOriginData CustomOriginData = null;

    private readonly Dictionary<Guid, Point16> _originSpawns = [];

    private string _bgName = "";

    //BG specific stuff
    public void SetBackground(PlayerBackgroundData data)
    {
        BackgroundData = data;
        _bgName = data.Identifier;
    }

    //Save / Load data for the player's origin name, custom data & spawn (if any)
    public override void SaveData(TagCompound tag)
    {
        tag.Add("bgName", _bgName);

        if (CustomOriginData is not null && _bgName == "Custom")
        {
            TagCompound customData = [];
            CustomOriginData.SaveData(customData);
            tag.Add("customData", customData);
        }

        if (_originSpawns.Count > 0)
        {
            tag.Add("originSpawnCount", _originSpawns.Count);
            int count = 0;
            
            foreach (KeyValuePair<Guid, Point16> pair in _originSpawns)
            {
                tag.Add("spawnKey" + count, pair.Key.ToByteArray());
                tag.Add("spawnValue" + count, pair.Value);
                count++;
            }
        }
    }

    public override void LoadData(TagCompound tag)
    {
        _bgName = tag.GetString("bgName");
        CustomOriginData = null;

        if (_bgName == "Custom" && tag.TryGet<TagCompound>("customData", out var data))
        {
            CustomOriginData = CustomOriginData.Empty;
            CustomOriginData.LoadData(data);
        }

        if (PlayerBackgroundDatabase.playerBackgroundDatas.Any(x => x.Identifier == _bgName))
            BackgroundData = PlayerBackgroundDatabase.playerBackgroundDatas.FirstOrDefault(x => x.Identifier == _bgName);
        else if (_bgName is not null && _bgName != string.Empty)
        {
            if (_bgName == "Custom")
                BackgroundData = Custom.GetCustomBackground(Player);
            else
                BackgroundData = new PlayerBackgroundData("Mods.NewBeginnings.Origins.Unloaded", "Unloaded", null, null);
        }

        if (tag.TryGet("originSpawnCount", out int count))
        {
            for (int i = 0; i < count; ++i)
            {
                Guid key = new(tag.GetByteArray("spawnKey" + i));
                Point16 value = tag.Get<Point16>("spawnValue" + i);
                _originSpawns.Add(key, value);
            }
        }
    }

    public bool HasBG() => _bgName is not null and not "";
    public bool HasBG(string name) => HasBG() && _bgName == name;
    public void SetOriginSpawn(Point16 point) => _originSpawns.Add(Main.ActiveWorldFileData.UniqueId, point);

    //Misc tMod Hooks
    public override void Load() => On_Player.Spawn += HijackSpawn;

    private void HijackSpawn(On_Player.orig_Spawn orig, Player self, PlayerSpawnContext context)
    {
        orig(self, context);

        if (self.GetModPlayer<PlayerBackgroundPlayer>()._originSpawns.TryGetValue(Main.ActiveWorldFileData.UniqueId, out Point16 spawn) && (self.SpawnX == -1 || self.SpawnY == -1))
        {
            self.SpawnX = spawn.X;
            self.SpawnY = spawn.Y;

            self.Center = new Vector2(self.SpawnX, self.SpawnY).ToWorldCoordinates();
            self.fallStart = (int)(self.Center.Y / 16f);
        }
    }

    public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
    {
        health = StatModifier.Default;
        mana = StatModifier.Default;

        if (HasBG()) //This adjusts health to be according to the origin.
        {
            int maxLife = BackgroundData.Misc.MaxLife;
            var scaling = ModContent.GetInstance<NewBeginningsConfig>().HealthScaling;

            if (scaling == NewBeginningsConfig.Scaling.Scaled)
            {
                float factor = 1 - Player.ConsumedLifeCrystals / (float)Player.LifeCrystalMax;
                float modifier = (int)((100 - maxLife) * factor);
                health.Base -= modifier;
            }
            else if (scaling == NewBeginningsConfig.Scaling.Relative)
            {
                float factor = Player.ConsumedLifeCrystals / (float)Player.LifeCrystalMax;
                health.Base = MathHelper.Lerp(maxLife - 100, maxLife * 4 - 400, factor);
            }
            else
                health.Base -= 100 - maxLife;
        }
    }
}
