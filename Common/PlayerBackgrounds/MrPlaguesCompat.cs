using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NewBeginnings.Common.Edits;
using NewBeginnings.Common.UI;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NewBeginnings.Common.PlayerBackgrounds;

internal class MrPlaguesCompat
{
    public static LocalizedText NoticeText;
    public static FieldInfo MrPlaguesInternalPlayerField;

    static bool hover = false;

    static Vector2 OpenTextPosition => new(Main.screenWidth / 2f, Main.screenHeight / 2f + ModContent.GetInstance<NewBeginningsConfig>().MrPlaguesButtonOffsetY);

    internal static void AddCharCreationDetour()
    {
        if (ModLoader.TryGetMod("MrPlagueRaces", out Mod mod))
        {
            NoticeText = Language.GetText("Mods.NewBeginnings.MrPlaguesCompatLine");
            var charUI = mod.Code.GetType("MrPlagueRaces.Common.UI.States.MrPlagueUICharacterCreation");
            var method = charUI.GetMethod("FinishCreatingCharacter", BindingFlags.Instance | BindingFlags.NonPublic);
            MonoModHooks.Add(method, CharCreationHijackSaveDetour.MrPlaguesHookFinishCharacter);
        }
    }

    internal static void PostSetupContent()
    {
        if (ModLoader.TryGetMod("MrPlagueRaces", out Mod mod)) //Authentic Races compat
        {
            var charUI = mod.Code.GetType("MrPlagueRaces.Common.UI.States.MrPlagueUICharacterCreation");

            MrPlaguesInternalPlayerField = charUI.GetField("_player", BindingFlags.Instance | BindingFlags.NonPublic);

            On_UserInterface.Draw += UserInterface_Draw;
        }
    }

    private static Rectangle OpenTextBounds()
    {
        Vector2 size = FontAssets.MouseText.Value.MeasureString(NoticeText.Value);
        return new Rectangle((int)(OpenTextPosition.X - size.X / 2f), (int)(OpenTextPosition.Y - size.Y / 2), (int)size.X, (int)size.Y);
    }

    private static void UserInterface_Draw(On_UserInterface.orig_Draw orig, UserInterface self, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, GameTime time)
    {
        orig(self, spriteBatch, time);

        if (self.CurrentState is not null && self.CurrentState.GetType().FullName == "MrPlagueRaces.Common.UI.States.MrPlagueUICharacterCreation")
        {
            var bounds = OpenTextBounds();
            hover = bounds.Contains(Main.MouseScreen.ToPoint());
            bool clickingOnThis = Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.O) || hover && Main.mouseLeft;

            Color col = hover ? Color.Gray : Color.White;
            var pos = OpenTextPosition;
            var font = FontAssets.MouseText.Value;

            if (clickingOnThis)
            {
                var plr = MrPlaguesInternalPlayerField.GetValue(Main.MenuUI.CurrentState) as Player;
                var mrPlague = Main.MenuUI.CurrentState;

                if (!plr.GetModPlayer<PlayerBackgroundPlayer>().HasBG())
                    plr.GetModPlayer<PlayerBackgroundPlayer>().SetBackground(PlayerBackgroundDatabase.playerBackgroundDatas.First());

                Main.MenuUI.SetState(new UIOriginSelection(plr, (evt, listeningElement) => Main.MenuUI.SetState(mrPlague)));
            }

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, NoticeText.Value, pos, col, Color.Black, 0f, bounds.Size() / 2f, Vector2.One);
        }
    }

    internal static void Unload()
    {
        if (ModLoader.HasMod("MrPlagueRaces")) //Authentic Races compat
        {
            NoticeText = null;
            MrPlaguesInternalPlayerField = null;
        }
    }
}
