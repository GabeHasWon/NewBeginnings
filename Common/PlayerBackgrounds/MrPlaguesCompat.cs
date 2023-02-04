using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoMod.RuntimeDetour.HookGen;
using NewBeginnings.Common.Edits;
using NewBeginnings.Common.UI;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NewBeginnings.Common.PlayerBackgrounds;

internal class MrPlaguesCompat
{
    const string NoticeText = "Click here to open New Beginning's menu!";

    public static FieldInfo MrPlaguesInternalPlayerField;

    static Point? lastClickLocation = null;
    static bool hover = false;

    static Vector2 OpenTextPosition => new(Main.screenWidth / 2f, (Main.screenHeight / 2f) + 160);

    internal static void AddCharCreationDetour()
    {
        if (ModLoader.TryGetMod("MrPlagueRaces", out Mod mod))
        {
            var charUI = mod.Code.GetType("MrPlagueRaces.Common.UI.States.MrPlagueUICharacterCreation");

            HookEndpointManager.Add(charUI.GetMethod("FinishCreatingCharacter", BindingFlags.Instance | BindingFlags.NonPublic), CharCreationHijackSaveDetour.MrPlaguesHookFinishCharacter);
        }
    }

    internal static void PostSetupContent()
    {
        if (ModLoader.TryGetMod("MrPlagueRaces", out Mod mod)) //Authentic Races compat
        {
            var charUI = mod.Code.GetType("MrPlagueRaces.Common.UI.States.MrPlagueUICharacterCreation");

            MrPlaguesInternalPlayerField = charUI.GetField("_player", BindingFlags.Instance | BindingFlags.NonPublic);
            Main.OnTickForThirdPartySoftwareOnly += Main_OnTickForThirdPartySoftwareOnly;

            On.Terraria.UI.UserInterface.Draw += UserInterface_Draw;
        }
    }

    private static Rectangle OpenTextBounds()
    {
        Vector2 size = FontAssets.ItemStack.Value.MeasureString(NoticeText);

        return new Rectangle((int)(OpenTextPosition.X - (size.X / 2f)), (int)(OpenTextPosition.Y - (size.Y / 2f)), (int)size.X, (int)size.Y);
    }

    private static void UserInterface_Draw(On.Terraria.UI.UserInterface.orig_Draw orig, UserInterface self, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, GameTime time)
    {
        orig(self, spriteBatch, time);

        if (self.CurrentState is not null && self.CurrentState.GetType().FullName == "MrPlagueRaces.Common.UI.States.MrPlagueUICharacterCreation")
        {
            var bounds = OpenTextBounds();
            Color col = hover ? Color.Gray : Color.White;

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, NoticeText, OpenTextPosition - new Vector2(0, 12), col, Color.Black, 0f, bounds.Size() / 2f, Vector2.One);
        }
    }

    internal static void Unload()
    {
        if (ModLoader.TryGetMod("MrPlagueRaces", out Mod mod)) //Authentic Races compat
        {
            MrPlaguesInternalPlayerField = null;
            Main.OnTickForThirdPartySoftwareOnly -= Main_OnTickForThirdPartySoftwareOnly;
        }
    }

    private static void Main_OnTickForThirdPartySoftwareOnly()
    {
        if (Main.mouseLeft && lastClickLocation is null)
            lastClickLocation = Main.MouseScreen.ToPoint();
        else if (!Main.mouseLeft && lastClickLocation is not null)
            lastClickLocation = null;

        bool uiIsValid = Main.MenuUI.CurrentState is not null && Main.MenuUI.CurrentState.GetType().Name == "MrPlagueUICharacterCreation";

        if (uiIsValid)
        {
            hover = OpenTextBounds().Contains(Main.MouseScreen.ToPoint());
            bool keyBind = Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.O);
            bool clickOn = Main.mouseLeft && lastClickLocation is not null && OpenTextBounds().Contains(lastClickLocation.Value);

            if (keyBind || (hover && clickOn))
            {
                var plr = MrPlaguesInternalPlayerField.GetValue(Main.MenuUI.CurrentState) as Player;
                var mrPlague = Main.MenuUI.CurrentState;

                if (!plr.GetModPlayer<PlayerBackgroundPlayer>().HasBG())
                    plr.GetModPlayer<PlayerBackgroundPlayer>().SetBackground(PlayerBackgroundDatabase.playerBackgroundDatas.First());

                Main.MenuUI.SetState(new UIOriginSelection(plr, (UIMouseEvent evt, UIElement listeningElement) => Main.MenuUI.SetState(mrPlague)));
            }
        }
    }
}
