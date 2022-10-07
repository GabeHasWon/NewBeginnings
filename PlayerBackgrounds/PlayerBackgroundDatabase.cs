using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Content.Items;
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
            AddNewBG("Knight", "Knight", "A noble warrior, clad in iron.", 160, 20, (ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves), true, (ItemID.SilverBroadsword, 1));
            AddNewBG("Huntsman", "Huntsman", $"Steady hands and keen eyes, a master of the hunt.", 100, 20, (0, 0, 0), new int[] { ItemID.HunterCloak }, true, (ItemID.GoldBow, 1), (ItemID.EndlessQuiver, 1));
            AddNewBG("Wizard", "Wizard", $"An apprentice wizard with an affinity for the arcane.", 80, 60, (ItemID.WizardHat, ItemID.SapphireRobe, 0), true, (ItemID.SapphireStaff, 1));
            AddNewBG("Beastmaster", "Beastmaster", $"Raised in the woodlands, they summon beasts to aid their journey.", 60, 20, (0, 0, 0), true, (ItemID.SlimeStaff, 1), (ItemID.BlandWhip, 1));
            AddNewBG("Shinobi", "Shinobi", $"A deadly mercenary assassin from the east. Fast, nimble, and with lethal efficiency", 80, 20, (0, 0, 0), new int[] { ItemID.Tabi }, true, (ItemID.Katana, 1));
            AddNewBG("Trailblazer", "Trailblazer", $"No time to explain. They have places to go, things to see", 100, 20, (0, 0, 0), new int[] { ItemID.HermesBoots, ItemID.Aglet, ItemID.AnkletoftheWind }, true); //Needs the winged helmet vanity
            AddNewBG("Adventurer", "Adventurer", $"Tentative", 100, 20, (ItemID.ArchaeologistsHat, ItemID.ArchaeologistsJacket, ItemID.ArchaeologistsPants), true, (ItemID.GrapplingHook, 1), (ItemID.Torch, 100), (ItemID.TrapsightPotion, 5), (ItemID.SpelunkerPotion, 5));
            AddNewBG("Farmer", "Farmer", $"It ain't much, but it's honest work", 100, 20, (ItemID.SummerHat, 0, 0), true, (ItemID.Sickle, 1), (ItemID.Hay, 200), (ItemID.DaybloomSeeds, 12), (ItemID.BlinkrootSeeds, 12), (ItemID.MoonglowSeeds, 12), (ItemID.WaterleafSeeds, 12), (ItemID.ShiverthornSeeds, 12), (ItemID.DeathweedSeeds, 12), (ItemID.FireblossomSeeds, 12)); //Gonna need a custom straw hat vanity item to replace the summer hat. 
            AddNewBG("Alchemist", "Alchemist", $"Tentative", 100, 20, (0, 0, 0), true, (ItemID.AlchemyTable, 1), (ItemID.BottledWater, 50), (ItemID.HerbBag, 12));
            AddNewBG("Spelunker", "Spelunker", $"The caves call and they answer. Those Ores aren't gonna mine themselves", 100, 20, (ItemID.MiningHelmet, 0, 0), new int[] { ItemID.AncientChisel }, true, (ItemID.GoldPickaxe, 1), (ItemID.Bomb, 15), (ItemID.SpelunkerPotion, 10));
            AddNewBG("Demoman", "Demolitionist", "Hurl explosives at ore, enemies, or yourself!", 100, 20, default, true, (ItemID.Dynamite, 1), (ItemID.Bomb, 5), (ItemID.Grenade, 10));
            AddNewBG("Fisherman", "Fisherman", "Slimes want me, fish fear me...", 100, 20, (ItemID.AnglerHat, 0, 0), new int[] { ItemID.HighTestFishingLine }, true, (ItemID.ReinforcedFishingPole, 1), (ItemID.CanOfWorms, 3));
            AddNewBG("Bookworm", "Bookworm", "Mind over matter. The best way to fight is with a sharpened mind!", 100, 20, (0, 0, 0), true, (ModContent.ItemType<WornSpellbook>(), 1), (ItemID.CordageGuide, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));
            AddNewBG("Boomer", "Boomer", "Back in my day...", 100, 20, (ItemID.Sunglasses, 0, 0), true, (ItemID.LawnMower, 1), (ItemID.BBQRibs, 2), (ItemID.GrilledSquirrel, 1));
            AddNewBG("Random", "Default", "Choose a random background.", 100, 20, (0, 0, 0), false); //Keep this as the last bg for functionality reasons
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
                desc += Environment.NewLine;

            int count = 0;
            foreach (var (type, stack) in inv) //Add every item in inv to the description
            {
                string itemText = $"[i/s{stack}:{type}]";
                if (stack <= 1)
                    itemText = $"[i:{type}]";

                desc += itemText;

                count += itemText.Length;
                if (count > 22)
                {
                    desc += Environment.NewLine;
                    count = 0;
                }
            }

            if (accessories is not null)
            {
                foreach (var type in accessories) //Add every item in accessories to the description
                    desc += $"[i:{type}]";
            }
        }
    }
}