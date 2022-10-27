using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.UnlockabilitySystem;
using NewBeginnings.Content.Items;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;


namespace NewBeginnings.Common.PlayerBackgrounds
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
            //FORMATTING: Try keeping single line backgrounds clumped together
            //Add new lines to make sure all very long backgrounds fit visibly on the screen at once, and keep them seperated

            AddNewBGItemlessDesc("Purist", "Purist", "The normal Terraria experience.", null, null);
            AddNewBG("Knight", "Knight", "A noble warrior, clad in iron.", new EquipData(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves), null, (ItemID.SilverBroadsword, 1));
            AddNewBG("Huntsman", "Huntsman", "Steady hands and keen eyes, a master of the hunt.", EquipData.SingleAcc(ItemID.HunterCloak), (ItemID.GoldBow, 1), (ItemID.EndlessQuiver, 1));
            AddNewBG("Wizard", "Wizard", "An apprentice wizard with an affinity for the arcane.", new EquipData(ItemID.WizardHat, ItemID.SapphireRobe, 0), new MiscData(80, 60), (ItemID.SapphireStaff, 1));
            AddNewBG("Beastmaster", "Beastmaster", "Raised in the woodlands, they summon beasts to aid their journey.", new MiscData(60), (ItemID.SlimeStaff, 1), (ItemID.BlandWhip, 1));
            AddNewBG("Shinobi", "Shinobi", "A deadly mercenary assassin from the east. Fast, nimble, and with lethal efficiency", EquipData.SingleAcc(ItemID.Tabi), new MiscData(80), (ItemID.Katana, 1));
            AddNewBG("Alchemist", "Alchemist", "Tentative", (ItemID.AlchemyTable, 1), (ItemID.BottledWater, 50), (ItemID.HerbBag, 12));
            AddNewBG("Demoman", "Demolitionist", "Hurl explosives at ore, enemies, or yourself!", (ItemID.Dynamite, 1), (ItemID.Bomb, 5), (ItemID.Grenade, 10));
            AddNewBG("Boomer", "Boomer", "Back in my day...", new EquipData(ItemID.Sunglasses), (ItemID.LawnMower, 1), (ItemID.BBQRibs, 2), (ItemID.GrilledSquirrel, 1));
            AddNewBG("Zoomer", "Boomer", "Terreddit post is popping off today\nboutta frag some slimes fr fr", new EquipData(ItemID.Goggles), new MiscData(40), (ItemID.CellPhone, 1));
            AddNewBG("Tiger", "Boomer", "Lightly more feral than other Terrarians, but not as much as you'd think!", new EquipData(ItemID.CatEars, 0, ItemID.FoxTail, ItemID.TigerClimbingGear), null, (ItemID.BladedGlove, 1));
            AddNewBG("Builder", "Builder", "A renowned architect from the cities.", new EquipData(0, 0, 0, ItemID.PortableStool, ItemID.ArchitectGizmoPack), new MiscData(npcType : NPCID.Painter), (ItemID.Wood, 500), (ItemID.StoneBlock, 500));
            AddNewBG("Lumberjack", "Lumberjack", "A humble lumberjack and his trusty axe", (ItemID.LucyTheAxe, 1), (ItemID.Sawmill, 1), (ItemID.Apple, 12), (ItemID.Wood, 300), (ItemID.BorealWood, 300), (ItemID.PalmWood, 300), (ItemID.Ebonwood, 300), (ItemID.Shadewood, 300), (ItemID.RichMahogany, 300), (ItemID.DynastyWood, 300));
            AddNewBG("Thief", "Thief", "Petty thief with a penchant for pickpocketing and poisoning", new EquipData(0, 0, 0, ItemID.LuckyCoin), (ItemID.PlatinumShortsword, 1), (ItemID.PoisonedKnife, 300)); //replace plat shortsword with thief's dagger
            AddNewBG("Firestarter", "Firestarter", "Some people just want to watch the world burn", new EquipData(0, 0, 0, ItemID.MagmaStone), (ItemID.MolotovCocktail, 300), (ItemID.FlareGun, 1), (ItemID.Flare, 50), (ItemID.Torch, 100));
            AddNewBG("Pirate", "Pirate", "Later",  new EquipData(ItemID.EyePatch, 0, 0, ItemID.GoldRing, ItemID.Sextant, 1), (ModContent.ItemType<RustyCutlass>(), 1), (ItemID.Keg, 1), (ItemID.Sail, 200));

            AddNewBG("Fisherman", "Fisherman", "Slimes want me, fish fear me...", EquipData.AccFirst(ItemID.HighTestFishingLine, ItemID.AnglerHat), 
                new MiscData(npcType: NPCID.Angler), (ItemID.ReinforcedFishingPole, 1), (ItemID.CanOfWorms, 3));

            AddNewBG("Trailblazer", "Trailblazer", "No time to explain. They have places to go, things to see", 
                EquipData.AccFirst(new int[] { ItemID.HermesBoots, ItemID.Aglet, ItemID.AnkletoftheWind })); //Needs the winged helmet vanity

            AddNewBG("Adventurer", "Adventurer", "Tentative", new EquipData(ItemID.ArchaeologistsHat, ItemID.ArchaeologistsJacket, ItemID.ArchaeologistsPants),
                (ItemID.GrapplingHook, 1), (ItemID.Torch, 100), (ItemID.TrapsightPotion, 5), (ItemID.SpelunkerPotion, 5));

            AddNewBG("Farmer", "Farmer", "It ain't much, but it's honest work", new EquipData(ItemID.SummerHat), 
                (ItemID.Sickle, 1), (ItemID.Hay, 200), (ItemID.DaybloomSeeds, 12), (ItemID.BlinkrootSeeds, 12), (ItemID.MoonglowSeeds, 12), (ItemID.WaterleafSeeds, 12), 
                (ItemID.ShiverthornSeeds, 12), (ItemID.DeathweedSeeds, 12), (ItemID.FireblossomSeeds, 12)); //Gonna need a custom straw hat vanity item to replace the summer hat. 

            AddNewBG("Spelunker", "Spelunker", "The caves call and they answer. Those ores aren't gonna mine themselves", EquipData.AccFirst(ItemID.AncientChisel, ItemID.MiningHelmet), 
                (ItemID.GoldPickaxe, 1), (ItemID.Bomb, 15), (ItemID.SpelunkerPotion, 10));

            AddNewBG("Bookworm", "Bookworm", "Mind over matter. The best way to fight is with a sharpened mind!", 
                (ModContent.ItemType<WornSpellbook>(), 1), (ItemID.CordageGuide, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));

            AddNewBG("Druid", "Boomer", "A herald of nature, engaged with keeping the world alive and healthy!", EquipData.SingleAcc(ItemID.CordageGuide),
                (ItemID.Vilethorn, 1), (ItemID.StaffofRegrowth, 1), (ItemID.HerbBag, 3), (ItemID.ClayPot, 10));

            AddNewBG("Slayer", "Slayer", "Rip and tear until it is done", null, new MiscData(140, 20), new DelegateData(null, null, () => true, () =>
            {
                const int Offset = 400;

                int x = WorldGen.genRand.NextBool() ? Offset : Main.maxTilesX - Offset;
                int y = Main.maxTilesY - 160;
                bool success = false;

                while (!success)
                {
                    for (int i = y; i < Main.maxTilesY - 40; i++)
                    {
                        bool validOpening = true;
                        for (int j = 0; j < 3; ++j)
                            if (WorldGen.SolidTile(x, i - j - 1) || WorldGen.SolidTile(x + 1, i - j - 1))
                                validOpening = false;

                        if (validOpening && WorldGen.SolidTile(x, i) && !Collision.LavaCollision(new Vector2(x - 1, i - 3) * 16, 3 * 16, 3 * 16))
                        {
                            success = true;
                            y = i - 3;
                            break;
                        }
                    }

                    if (!success)
                        x += (x < Main.maxTilesX / 2) ? 2 : -2;
                }

                return new Terraria.DataStructures.Point16(x, y);
            }), (ItemID.Boomstick, 1), (ItemID.EndlessMusketPouch, 1));

            AddNewBG("Accursed", "Accursed", "Starts with hardmode already enabled. Good luck!", new EquipData(ItemID.PearlwoodHelmet, ItemID.PearlwoodBreastplate, ItemID.PearlwoodGreaves),
                new MiscData(swordReplace: ItemID.GoldBroadsword, pickReplace: ItemID.GoldPickaxe, axeReplace: ItemID.GoldAxe, npcType: NPCID.Dryad),
                new DelegateData(
                    () => UnlockSaveData.Unlocked("Accursed"), //Unlocked by having the Accursed unlock completed
                    (list) => //Spawns hardmode in a new world
                    {
                        list.Add(new PassLegacy("Early Hardmode", (GenerationProgress p, GameConfiguration config) =>
                        {
                            p.Message = "Generating hardmode";
                            WorldGen.smCallBack(null);

                            Main.hardMode = true;
                        }
                    ));
                }));

            AddNewBGItemlessDesc("Random", "Default", "Choose a random background.", null, null); //Keep this as the last bg for functionality reasons
        }

        /// <summary>
        /// Autoloads every texture in PlayerBackgrounds/Textures/.
        /// </summary>
        private static void LoadBackgroundIcons()
        {
            const string AssetPath = "Assets/Textures/BackgroundIcons/";

            var assets = ModContent.GetInstance<NewBeginnings>().Assets.GetLoadedAssets();

            var mod = ModContent.GetInstance<NewBeginnings>();
            var realIcons = mod.GetFileNames().Where(x => x.StartsWith(AssetPath) && x.EndsWith(".rawimg"));

            foreach (var item in realIcons)
            {
                backgroundIcons.Add(item[AssetPath.Length..].Replace(".rawimg", string.Empty), mod.Assets.Request<Texture2D>(item.Replace(".rawimg", string.Empty)));
            }
        }

        /// <summary>Adds a new background with the given info.</summary>
        /// <param name="name">Name of the background; will be used in the UI and player title.</param>
        /// <param name="tex">Name of the texture for the icon; i.e. Bookworm. Must be an image in PlayerBackgrounds/Textures/.</param>
        /// <param name="desc">Description of the background to be shown on the char creation UI.</param>
        /// <param name="equipData">The armor and accessories the player will wear.</param>
        /// <param name="addInvToDesc">Whether the backgrounds inventory and accessories are to be shown in the description.</param>
        private static void AddNewBG(string name, string tex, string desc, EquipData? equipData = null, MiscData? miscData = null, params (int type, int stack)[] inv)
        {
            ExpandDesc(inv, ref desc, equipData?.Accessories);
            var data = new PlayerBackgroundData(name, tex, desc, equipData, miscData, inv);
            playerBackgroundDatas.Add(data);
        }

        private static void AddNewBG(string name, string tex, string desc, EquipData? equipData = null, MiscData? miscData = null, DelegateData? delegateData = null, params (int type, int stack)[] inv)
        {
            ExpandDesc(inv, ref desc, equipData?.Accessories);
            var data = new PlayerBackgroundData(name, tex, desc, equipData, miscData, inv);
            data.Delegates = delegateData ?? new DelegateData();
            playerBackgroundDatas.Add(data);
        }

        private static void AddNewBG(string name, string tex, string desc, EquipData? equipData = null, params (int type, int stack)[] inv) => AddNewBG(name, tex, desc, equipData, null, inv);
        private static void AddNewBG(string name, string tex, string desc, MiscData? miscData = null, params (int type, int stack)[] inv) => AddNewBG(name, tex, desc, null, miscData, inv);
        private static void AddNewBG(string name, string tex, string desc, params (int type, int stack)[] inv) => AddNewBG(name, tex, desc, null, null, inv);

        private static void AddNewBGItemlessDesc(string name, string tex, string desc, EquipData? equipData = null, MiscData? miscData = null, params (int type, int stack)[] inv)
        {
            var data = new PlayerBackgroundData(name, tex, desc, equipData, miscData, inv);
            playerBackgroundDatas.Add(data);
        }

        /// <summary>Automatically writes everything in <paramref name="inv"/> and <paramref name="accessories"/> (if there are any accessories) to the description.</summary>
        /// <param name="inv"></param>
        /// <param name="desc"></param>
        /// <param name="accessories"></param>
        private static void ExpandDesc((int type, int stack)[] inv, ref string desc, int[] accessories = null)
        {
            return;
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