using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.PlayerBackgrounds
{
    internal static class PlayerBackgroundDatabase
    {
        public static List<PlayerBackgroundData> playerBackgroundDatas = new();
        public static Dictionary<string, Asset<Texture2D>> backgroundIcons = new();

        public static void Populate()
        {
            LoadBackgroundIcons();

            playerBackgroundDatas.Clear();
            LoadAllBackgrounds();
        }

        private static void LoadAllBackgrounds()
        {
            AddNewBG("Purist", "Purist", "The normal Terraria experience.", 100, 20, default, false);
            AddNewBG("Demoman", "Demolitionist", "Hurl explosives at ore, enemies, or yourself!", 100, 20, default, true, (ItemID.Dynamite, 1), (ItemID.Bomb, 5), (ItemID.Grenade, 10));
            AddNewBG("Fisherman", "Fisherman", "Slimes want me, fish fear me...", 100, 20, (ItemID.AnglerHat, 0, 0), new int[] { ItemID.HighTestFishingLine }, true, (ItemID.ReinforcedFishingPole, 1), (ItemID.CanOfWorms, 3));
            AddNewBG("Bookworm", "Bookworm", "Mind over matter. The best way to fight is with a sharpened mind!", 100, 20, (0, 0, 0), true, (ItemID.CordageGuide, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));
            AddNewBG("Boomer", "Boomer", "Back in my day...", 100, 20, (ItemID.Sunglasses, 0, 0), true, (ItemID.LawnMower, 1), (ItemID.BBQRibs, 2), (ItemID.GrilledSquirrel, 1));
            AddNewBG("Random", "Default", "Choose a random background.", 100, 20, (0, 0, 0), false);
        }

        /// <summary>
        /// Autoloads every texture in PlayerBackgrounds/Textures/.
        /// </summary>
        private static void LoadBackgroundIcons()
        {
            var assets = ModContent.GetInstance<NewBeginnings>().Assets.GetLoadedAssets();
            var realIcons = assets.Where(x => x is Asset<Texture2D> && x.Name.StartsWith("PlayerBackgrounds\\Textures\\")).ToList();

            foreach (var item in realIcons)
                backgroundIcons.Add(item.Name["PlayerBackgrounds\\Textures\\".Length..], item as Asset<Texture2D>);
        }

        /// <summary>Adds a new background with the given info.</summary>
        /// <param name="name">Name of the background; will be used in the UI and player title.</param>
        /// <param name="tex">Name of the texture for the icon; i.e. Bookworm. Must be an image in PlayerBackgrounds/Textures/.</param>
        /// <param name="desc">Description of the background to be shown on the char creation UI.</param>
        /// <param name="maxLife">Max life of the player who uses this background.</param>
        /// <param name="startMana">Max mana of the player who uses this background.</param>
        /// <param name="armor">Set of armor for the player; (head, body, legs) respectively.</param>
        /// <param name="addInvToDesc">Whether the backgrounds inventory and accessories are to be shown in the description.</param>
        /// <param name="inv"></param>
        private static void AddNewBG(string name, string tex, string desc, int maxLife, int startMana, (int head, int body, int legs) armor, bool addInvToDesc = true, params (int type, int stack)[] inv)
        {
            if (addInvToDesc)
                ExpandDesc(inv, ref desc);

            var data = new PlayerBackgroundData(name, tex, desc, maxLife, startMana, armor, inv);
            playerBackgroundDatas.Add(data);
        }

        /// <summary>Same as <see cref="AddNewBG(string, string, string, int, int, (int head, int body, int legs), bool, (int type, int stack)[])"/>, but also has accessories.</summary>
        /// <param name="accessories">Accessories the player will have. Must be less than <see cref="Player.InitialAccSlotCount"/> (5).</param>
        private static void AddNewBG(string name, string tex, string desc, int maxLife, int startMana, (int head, int body, int legs) armor, int[] accessories, bool addInvToDesc = true, params (int type, int stack)[] inv)
        {
            if (addInvToDesc)
                ExpandDesc(inv, ref desc, accessories);

            var data = new PlayerBackgroundData(name, tex, desc, maxLife, startMana, accessories, armor, inv);
            playerBackgroundDatas.Add(data);
        }

        /// <summary>Automatically writes everything in <paramref name="inv"/> and <paramref name="accessories"/> (if there are any accessories) to the description.</summary>
        /// <param name="inv"></param>
        /// <param name="desc"></param>
        /// <param name="accessories"></param>
        private static void ExpandDesc((int type, int stack)[] inv, ref string desc, int[] accessories = null)
        {
            if (inv.Length > 0)
                desc += "\n";

            foreach (var (type, stack) in inv) //Add every item in inv to the description
            {
                string itemText = $"[i/s{stack}:{type}]";
                if (stack <= 1)
                    itemText = $"[i:{type}]";

                desc += itemText;
            }

            if (accessories is not null)
            {
                foreach (var type in accessories) //Add every item in accessories to the description
                    desc += $"[i:{type}]";
            }
        }
    }
}

/*
Knight: Ready your sword! Charge fourth with tough armor and more health!
Huntsman: Steady. Aim. Fire! Barrage your foes with a shower of arrows!
Wizard: Dawn your hat and conjure magics to blast your enemies with a barrage of spells!
+Magician: A wizard hat wasn't enough? Impress your enemy with a slew of magical attacks! 
Beastmaster: Tame ferocious creatures and command an army into battle!
Noble: Begin with deepened pockets and an all or nothing attitude. Spare no expense on luxuries.
Shinobi: Hone your blade and stay light on your feet. They won't know what hit them.
Trailblazer: No time to explain, places to go, things to see!
Adventurer: Always come prepared. Use your arsenal to traverse all terrain!
Acrobat: Dodge and weave around your enemies to gain the upper hand!
Fisherman: Slimes want me, fish fear me...
Farmer: It's not much, but it's honest work.
Spelunker: The caves call and you answer. That ore won't mine itself you know.
Demoman: Hurl explosives at ore, enemies, or anything that moves! You've got more to spare.
Builder: Kick off your adventure with some building instead! Fighting is overrated anyways.
Thief: Save some extra change with every purchase, every copper coin is a coin worth saving.
Bookworm: Mind over matter. The best way to fight is with a sharpened mind!
Firestarter: Mmph mmmph mph-mph mmph mmmmph!
Tiger: Climb walls and ambush your enemies before they know what's going on!
Druid: Channel the wrath of nature to aid you in your adventure!
Deprived: The lowest of the low. Not even bunnies look your way.
Pirate: Plunder your enemies with a barrage of canonballs and sail the seas for treasure!
Zoomer: You are zoomer. 
Boomer: I am boomer.
*/