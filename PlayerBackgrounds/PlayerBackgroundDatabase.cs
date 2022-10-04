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

        public static void Populate(Mod mod)
        {
            LoadBackgroundIcons();

            playerBackgroundDatas.Clear();

            LoadAllBackgrounds();
        }

        private static void LoadAllBackgrounds()
        {
            AddNewBG("Purist", "Purist", "The normal Terraria experience.", 100, 0, default, false);
            AddNewBG("Demoman", "Demolitionist", "Hurl explosives at ore, enemies, or yourself!", 100, 0, default, true, (ItemID.Dynamite, 1), (ItemID.Bomb, 5), (ItemID.Grenade, 10));
            AddNewBG("Fisherman", "Fisherman", $"Slimes want me, fish fear me...", 100, 0, (ItemID.AnglerHat, 0, 0), true, (ItemID.ReinforcedFishingPole, 1), (ItemID.CanOfWorms, 3), (ItemID.HighTestFishingLine, 1));
            AddNewBG("Bookworm", "Bookworm", $"Mind over matter. The best way to fight is with a sharpened mind!", 100, 0, (0, 0, 0), true, (ItemID.CordageGuide, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));
            AddNewBG("Boomer", "Boomer", $"Back in my day...", 100, 0, (0, 0, 0), true, (ItemID.LawnMower, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));
            AddNewBG("Random", "Default", "Choose a random background.", 100, 0, (0, 0, 0), false);
        }

        /// <summary>
        /// Autoloads every texture in PlayerBackgrounds/Textures/
        /// </summary>
        private static void LoadBackgroundIcons()
        {
            var assets = ModContent.GetInstance<NewBeginnings>().Assets.GetLoadedAssets();
            var realIcons = assets.Where(x => x is Asset<Texture2D> && x.Name.StartsWith("PlayerBackgrounds\\Textures\\")).ToList();

            foreach (var item in realIcons)
                backgroundIcons.Add(item.Name["PlayerBackgrounds\\Textures\\".Length..], item as Asset<Texture2D>);
        }

        private static void AddNewBG(string name, string tex, string desc, int maxLife, int startMana, (int, int, int) armor, bool addInvToDesc = true, params(int type, int stack)[] inv)
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

            var data = new PlayerBackgroundData(name, tex, desc, maxLife, startMana, armor, inv);
            playerBackgroundDatas.Add(data);
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