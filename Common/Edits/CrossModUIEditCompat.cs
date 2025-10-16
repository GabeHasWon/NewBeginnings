using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NewBeginnings.Common.PlayerBackgrounds;
using NewBeginnings.Common.UI;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NewBeginnings.Common.Edits;

internal class CrossModUIEditCompat
{
    private const string PlaguesCharUIName = "MrPlagueRaces.Common.UI.States.MrPlagueUICharacterCreation";
    private const string OnePieceCharUIName = "GumGum.UI.States.OnePieceUICharacterCreation";
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
    
    public static LocalizedText NoticeText;
    public static FieldInfo MrPlaguesInternalPlayerField;
    public static FieldInfo OnePieceInternalPlayerField;

    static bool hover = false;

    static Vector2 OpenTextPosition => new(Main.screenWidth / 2f, Main.screenHeight / 2f + ModContent.GetInstance<NewBeginningsClientConfig>().MrPlaguesButtonOffsetY);

    internal static void AddCharCreationDetour()
    {
        NoticeText = Language.GetText("Mods.NewBeginnings.MrPlaguesCompatLine");

        if (ModLoader.TryGetMod("MrPlagueRaces", out Mod mod))
        {
            var charUI = mod.Code.GetType(PlaguesCharUIName);
            var method = charUI.GetMethod("FinishCreatingCharacter", Flags);
            MonoModHooks.Add(method, CharCreationHijackSaveDetour.CrossmodFinishHookCharacter);
        }

        if (ModLoader.TryGetMod("GumGum", out Mod onePiece))
        {
            var charUI = onePiece.Code.GetType(OnePieceCharUIName);
            var method = charUI.GetMethod("FinishCreatingCharacter", Flags);
            MonoModHooks.Add(method, CharCreationHijackSaveDetour.CrossmodFinishHookCharacter);
        }
    }

    internal static void PostSetupContent()
    {
        if (ModLoader.TryGetMod("MrPlagueRaces", out Mod mod)) //Authentic Races compat
        {
            var charUI = mod.Code.GetType(PlaguesCharUIName);
            MrPlaguesInternalPlayerField = charUI.GetField("_player", Flags);
        }

        if (ModLoader.TryGetMod("GumGum", out Mod onePiece)) //Authentic Races compat
        {
            var charUI = onePiece.Code.GetType(OnePieceCharUIName);
            OnePieceInternalPlayerField = charUI.GetField("_player", Flags);
        }

        On_UserInterface.Draw += UserInterface_Draw;
    }

    private static Rectangle OpenTextBounds()
    {
        Vector2 size = FontAssets.MouseText.Value.MeasureString(NoticeText.Value);
        return new Rectangle((int)(OpenTextPosition.X - size.X / 2f), (int)(OpenTextPosition.Y - size.Y / 2), (int)size.X, (int)size.Y);
    }

    private static void UserInterface_Draw(On_UserInterface.orig_Draw orig, UserInterface self, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, GameTime time)
    {
        orig(self, spriteBatch, time);

        if (self.CurrentState is not null && self.CurrentState.GetType().FullName is PlaguesCharUIName or OnePieceCharUIName)
        {
            var bounds = OpenTextBounds();
            hover = bounds.Contains(Main.MouseScreen.ToPoint());
            bool clickingOnThis = Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.O) || hover && Main.mouseLeft;

            Color col = hover ? Color.Gray : Color.White;
            var pos = OpenTextPosition;
            var font = FontAssets.MouseText.Value;

            if (clickingOnThis)
            {
                FieldInfo fieldToUse = self.CurrentState.GetType().FullName is PlaguesCharUIName ? MrPlaguesInternalPlayerField : OnePieceInternalPlayerField;
                var plr = fieldToUse.GetValue(Main.MenuUI.CurrentState) as Player;
                var state = Main.MenuUI.CurrentState;

                if (!plr.GetModPlayer<PlayerBackgroundPlayer>().HasBG())
                    plr.GetModPlayer<PlayerBackgroundPlayer>().SetBackground(PlayerBackgroundDatabase.playerBackgroundDatas.First());

                Main.MenuUI.SetState(new UIOriginSelection(plr, (evt, listeningElement) => Main.MenuUI.SetState(state)));
            }

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, NoticeText.Value, pos, col, Color.Black, 0f, bounds.Size() / 2f, Vector2.One);
        }
    }

    internal static void Unload()
    {
        NoticeText = null;
        MrPlaguesInternalPlayerField = null;
        OnePieceInternalPlayerField = null;
    }
}
