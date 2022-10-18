using Mono.Cecil.Cil;
using MonoMod.Cil;
using NewBeginnings.Common.PlayerBackgrounds;
using Terraria;

namespace NewBeginnings.Common.Edits
{
    internal class CharNameEdit
    {
        public static void Load()
        {
            IL.Terraria.GameContent.UI.Elements.UICharacterListItem.DrawSelf += UICharacterListItem_DrawSelf;
        }

        private static void UICharacterListItem_DrawSelf(ILContext il)
        {
            ILCursor c = new(il);

            if (!c.TryGotoNext(MoveType.After, x => x.MatchStloc(4)))
                return;

            c.Emit(OpCodes.Ldloc_S, (byte)4);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit<Terraria.GameContent.UI.Elements.UICharacterListItem>(OpCodes.Ldfld, "_data");
            c.Emit<Terraria.IO.PlayerFileData>(OpCodes.Callvirt, "get_Player");

            c.EmitDelegate((string input, Player player) =>
            {
                string name = player.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData.Name;
                if (name is not null)
                    return input + $" [c/8B8B8B:the {name}]";
                return input;
            });

            c.Emit(OpCodes.Stloc_S, (byte)4);
        }
    }
}
