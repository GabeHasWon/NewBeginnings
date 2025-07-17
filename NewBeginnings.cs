using Microsoft.Xna.Framework;
using NewBeginnings.Common.Crossmod;
using NewBeginnings.Common.Edits;
using NewBeginnings.Common.PlayerBackgrounds;
using NewBeginnings.Common.UnlockabilitySystem;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace NewBeginnings;

public class NewBeginnings : Mod
{
    public static class MessageID
    {
        public const byte SyncPlayerOrigin = 0;
        public const byte RequestPlayerSpawn = 1;
    }

    public UIState UnlockUI;

    public override void Load()
    {
        UnlockabilityIO.LoadData();
        CharCreationEdit.Load();
    }

    public override void PostSetupContent()
    {
        MrPlaguesCompat.PostSetupContent();
        PlayerBackgroundDatabase.Populate();
    }

    public override void Unload() => MrPlaguesCompat.Unload();
    public override object Call(params object[] args) => OriginCalls.Call(args);

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        byte type = reader.ReadByte();

        if (type == MessageID.SyncPlayerOrigin)
        {
            string origin = reader.ReadString();
            byte who = reader.ReadByte();

            if (Main.dedServ)
            {
                ModPacket packet = ModContent.GetInstance<NewBeginnings>().GetPacket(6);
                packet.Write(MessageID.SyncPlayerOrigin);
                packet.Write(origin);
                packet.Write(who);
                packet.Send(-1, who);
            }

            Main.player[who].GetModPlayer<PlayerBackgroundPlayer>().bgName = origin;
            Main.player[who].GetModPlayer<PlayerBackgroundPlayer>().SetBackgroundBasedOnName();
        }
        else if (type == MessageID.RequestPlayerSpawn)
        {
            string origin = reader.ReadString();
            byte who = reader.ReadByte();

            Player player = Main.player[who];

            if (player.GetModPlayer<PlayerBackgroundPlayer>().bgName == "")
            {
                player.GetModPlayer<PlayerBackgroundPlayer>().bgName = origin;
                player.GetModPlayer<PlayerBackgroundPlayer>().SetBackgroundBasedOnName();
            }

            if (Main.dedServ)
            {
                Point16 pos = PlayerBackgroundWorld.SetOriginSpawn(Main.player[who]);

                ModPacket packet = ModContent.GetInstance<NewBeginnings>().GetPacket(6);
                packet.Write(MessageID.RequestPlayerSpawn);
                packet.Write(origin);
                packet.Write(who);
                packet.Write(pos.X);
                packet.Write(pos.Y);
                packet.Send();
            }
            else
            {
                int x = reader.ReadInt16();
                int y = reader.ReadInt16();

                if (x > 0 && y > 0)
                {
                    player.GetModPlayer<PlayerBackgroundPlayer>().SetOriginSpawn(new Point16(x, y));
                    player.Center = new Vector2(x, y).ToWorldCoordinates();
                }
            }
        }
    }
}