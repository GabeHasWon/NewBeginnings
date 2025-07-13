using Microsoft.Xna.Framework;
using NewBeginnings.Common.PlayerBackgrounds.Containers;
using NewBeginnings.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NewBeginnings.Common.PlayerBackgrounds;

/// <summary>Handles player-related background functions (i.e. inventory).</summary>
internal class PlayerBackgroundPlayer : ModPlayer
{
    public PlayerBackgroundData BackgroundData = new();
    public CustomOriginData CustomOriginData = null;

    private readonly Dictionary<Guid, Point16> _originSpawns = [];

    internal string bgName = "";

    public void SetBackground(PlayerBackgroundData data)
    {
        BackgroundData = data;
        bgName = data.Identifier;
    }

    //Save / Load data for the player's origin name, custom data & spawn (if any)
    public override void SaveData(TagCompound tag)
    {
        tag.Add("bgName", bgName);

        if (CustomOriginData is not null && bgName == "Custom")
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
        bgName = tag.GetString("bgName");
        CustomOriginData = null;

        if (bgName == "Custom" && tag.TryGet<TagCompound>("customData", out var data))
        {
            CustomOriginData = CustomOriginData.Empty;
            CustomOriginData.LoadData(data);
        }

        SetBackgroundBasedOnName();

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

    internal void SetBackgroundBasedOnName()
    {
        if (PlayerBackgroundDatabase.playerBackgroundDatas.Any(x => x.Identifier == bgName))
            BackgroundData = PlayerBackgroundDatabase.playerBackgroundDatas.FirstOrDefault(x => x.Identifier == bgName);
        else if (bgName is not null && bgName != string.Empty)
        {
            if (bgName == "Custom")
                BackgroundData = Custom.GetCustomBackground(Player);
            else
                BackgroundData = new PlayerBackgroundData("Mods.NewBeginnings.Origins.Unloaded", "Unloaded", null, null);
        }
    }

    public override void OnEnterWorld()
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
            return;

        ModPacket packet = ModContent.GetInstance<NewBeginnings>().GetPacket(6);
        packet.Write(NewBeginnings.MessageID.SyncPlayerOrigin);
        packet.Write(bgName);
        packet.Write((byte)Player.whoAmI);
        packet.Send();
    }

    public bool HasBG() => bgName is not null and not "";
    public bool HasBG(string name) => HasBG() && bgName == name;
    public void SetOriginSpawn(Point16 point) => _originSpawns.TryAdd(Main.ActiveWorldFileData.UniqueId, point);
    public override void Load() => On_Player.Spawn += HijackSpawn;

    private static void HijackSpawn(On_Player.orig_Spawn orig, Player self, PlayerSpawnContext context)
    {
        orig(self, context);

        PlayerBackgroundPlayer plr = self.GetModPlayer<PlayerBackgroundPlayer>();

        if (!plr._originSpawns.TryGetValue(Main.ActiveWorldFileData.UniqueId, out _))
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<NewBeginnings>().GetPacket(6);
                packet.Write(NewBeginnings.MessageID.RequestPlayerSpawn);
                packet.Write(plr.bgName);
                packet.Write((byte)self.whoAmI);
                packet.Send();
            }
            else
                PlayerBackgroundWorld.SetOriginSpawn(self);
        }

        if (plr._originSpawns.TryGetValue(Main.ActiveWorldFileData.UniqueId, out var spawn) && self.SpawnX == -1 || self.SpawnY == -1)
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

            if (CustomOriginData is not null)
            {
                mana.Base += CustomOriginData.mana;
            }
        }
    }
}
