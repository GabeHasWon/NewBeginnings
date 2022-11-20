using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.PlayerBackgrounds.Containers;
using NewBeginnings.Content.Items;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;

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
            SortBGDatas();
        }

        private static void LoadAllBackgrounds()
        {
            //FORMATTING: Try keeping single line backgrounds clumped together
            //Add new lines to make sure all very long backgrounds fit visibly on the screen at once, and keep them seperated

            AddNewBGItemlessDesc("Purist", "Purist", "The normal Terraria experience.", null, new MiscData(sortPriority: 11));
            AddNewBG("Knight", "Knight", "A noble warrior, clad in iron.", new EquipData(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves), new MiscData(sortPriority: 11), (ItemID.SilverBroadsword, 1));
            AddNewBG("Huntsman", "Huntsman", "Steady hands and keen eyes, a master of the hunt.", EquipData.SingleAcc(ItemID.HunterCloak), new MiscData(sortPriority: 11), (ItemID.GoldBow, 1), (ItemID.EndlessQuiver, 1));
            AddNewBG("Wizard", "Wizard", "An apprentice wizard with an affinity for the arcane.", new EquipData(ItemID.WizardHat, ItemID.SapphireRobe, 0), new MiscData(80, 60, sortPriority: 11), (ItemID.SapphireStaff, 1));
            AddNewBG("Beastmaster", "Beastmaster", "Raised in the woodlands, they summon beasts to aid their journey.", new MiscData(60, sortPriority: 11), (ItemID.SlimeStaff, 1), (ItemID.BlandWhip, 1));
            AddNewBG("Shinobi", "Shinobi", "A deadly mercenary assassin from the east. Fast, nimble, and with lethal efficiency.", EquipData.SingleAcc(ItemID.Tabi), new MiscData(80), (ItemID.Katana, 1));
            AddNewBG("Alchemist", "Alchemist", "Tentative", (ItemID.AlchemyTable, 1), (ItemID.BottledWater, 50), (ItemID.HerbBag, 12));
            AddNewBG("Demoman", "Demolitionist", "Hurl explosives at ore, enemies, or yourself!", (ItemID.Dynamite, 1), (ItemID.Bomb, 5), (ItemID.Grenade, 10));
            AddNewBG("Boomer", "Boomer", "Back in my day...", new EquipData(ItemID.Sunglasses), (ItemID.LawnMower, 1), (ItemID.BBQRibs, 2), (ItemID.GrilledSquirrel, 1));
            AddNewBG("Zoomer", "Default", "Terreddit post is popping off today boutta frag some slimes fr fr", new EquipData(ItemID.Goggles), new MiscData(40), (ItemID.CellPhone, 1));
            AddNewBG("Tiger", "Default", "Lightly more feral than other Terrarians, but not as much as you'd think!", new EquipData(ItemID.CatEars, 0, ItemID.FoxTail, ItemID.TigerClimbingGear), null, (ItemID.BladedGlove, 1));
            AddNewBG("Builder", "Builder", "A renowned architect from the cities.", new EquipData(0, 0, 0, ItemID.PortableStool, ItemID.ArchitectGizmoPack), new MiscData(npcType : NPCID.Painter), (ItemID.Wood, 500), (ItemID.StoneBlock, 500));
            AddNewBG("Lumberjack", "Lumberjack", "A humble lumberjack and his trusty axe.", (ItemID.LucyTheAxe, 1), (ItemID.Sawmill, 1), (ItemID.Apple, 12), (ItemID.Wood, 300), (ItemID.BorealWood, 300), (ItemID.PalmWood, 300), (ItemID.Ebonwood, 300), (ItemID.Shadewood, 300), (ItemID.RichMahogany, 300), (ItemID.DynastyWood, 300));
            AddNewBG("Thief", "Thief", "Petty thief with a penchant for pickpocketing and poisoning.", new EquipData(0, 0, 0, ItemID.LuckyCoin), (ItemID.PlatinumShortsword, 1), (ItemID.PoisonedKnife, 300)); //replace plat shortsword with thief's dagger
            AddNewBG("Firestarter", "Firestarter", "Some people just want to watch the world burn.", new EquipData(0, 0, 0, ItemID.MagmaStone), (ItemID.MolotovCocktail, 300), (ItemID.FlareGun, 1), (ItemID.Flare, 50), (ItemID.Torch, 100));
            AddNewBG("Pirate", "Pirate", "Aye. A real pirate knows when land can be plundered, and plunder they shall!",  new EquipData(ItemID.EyePatch, 0, 0, ItemID.GoldRing, ItemID.Sextant, 1), (ModContent.ItemType<RustyCutlass>(), 1), (ItemID.Keg, 1), (ItemID.Sail, 200));
            AddNewBG("Deprived", "Default", "Tentative", null, new MiscData(80, swordReplace: ModContent.ItemType<DeprivedBlade>()), (ModContent.ItemType<DeprivedLantern>(), 1), (ItemID.HealingPotion, 3));

            AddNewBG("Nobleman", "Default", "A hard worker if you think counting money is hard. NOTE: Can only be played in Mediumcore or Hardcore.",  EquipData.AccFirst(new int[] { ItemID.DiamondRing, ItemID.GoldWatch }), new MiscData(20), new DelegateData(modifyCreation: (plr) =>
            {
                if (plr.difficulty == PlayerDifficultyID.SoftCore || plr.difficulty == PlayerDifficultyID.Creative)
                    plr.difficulty = PlayerDifficultyID.MediumCore;
            }), (ItemID.PlatinumCoin, 1));

            AddNewBG("Fisherman", "Fisherman", "Slimes want me, fish fear me...", EquipData.AccFirst(ItemID.HighTestFishingLine, ItemID.AnglerHat), 
                new MiscData(npcType: NPCID.Angler), (ItemID.ReinforcedFishingPole, 1), (ItemID.CanOfWorms, 3));

            AddNewBG("Trailblazer", "Trailblazer", "No time to explain. They have places to go, things to see!", 
                EquipData.AccFirst(new int[] { ItemID.HermesBoots, ItemID.Aglet, ItemID.AnkletoftheWind })); //Needs the winged helmet vanity

            AddNewBG("Adventurer", "Adventurer", "Tentative", new EquipData(ItemID.ArchaeologistsHat, ItemID.ArchaeologistsJacket, ItemID.ArchaeologistsPants),
                (ItemID.GrapplingHook, 1), (ItemID.Torch, 100), (ItemID.TrapsightPotion, 5), (ItemID.SpelunkerPotion, 5));

            AddNewBG("Farmer", "Farmer", "It ain't much, but it's honest work.", new EquipData(ItemID.SummerHat), 
                (ItemID.Sickle, 1), (ItemID.Hay, 200), (ItemID.DaybloomSeeds, 12), (ItemID.BlinkrootSeeds, 12), (ItemID.MoonglowSeeds, 12), (ItemID.WaterleafSeeds, 12), 
                (ItemID.ShiverthornSeeds, 12), (ItemID.DeathweedSeeds, 12), (ItemID.FireblossomSeeds, 12)); //Gonna need a custom straw hat vanity item to replace the summer hat. 

            AddNewBG("Spelunker", "Spelunker", "The caves call and they answer. Those ores aren't gonna mine themselves!", EquipData.AccFirst(ItemID.AncientChisel, ItemID.MiningHelmet), 
                (ItemID.GoldPickaxe, 1), (ItemID.Bomb, 15), (ItemID.SpelunkerPotion, 10));

            AddNewBG("Bookworm", "Bookworm", "Mind over matter. The best way to fight is with a sharpened mind!", 
                (ModContent.ItemType<WornSpellbook>(), 1), (ItemID.CordageGuide, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));

            AddNewBG("Druid", "Boomer", "A herald of nature, engaged with keeping the world alive and healthy!", EquipData.SingleAcc(ItemID.CordageGuide),
                (ItemID.Vilethorn, 1), (ItemID.StaffofRegrowth, 1), (ItemID.HerbBag, 3), (ItemID.ClayPot, 10));

            playerBackgroundDatas.Add(new Slayer());
            playerBackgroundDatas.Add(new Accursed());

            AddNewBGItemlessDesc("Random", "Default", "Choose a random background.", null, null);
        }

        private static void SortBGDatas()
        {
            Dictionary<int, List<PlayerBackgroundData>> tempData = new();

            foreach (var item in playerBackgroundDatas)
            {
                if (!tempData.ContainsKey(item.Misc.SortPriority))
                    tempData.Add(item.Misc.SortPriority, new List<PlayerBackgroundData>() { item });
                else
                    tempData[item.Misc.SortPriority].Add(item);
            }

            var keys = tempData.Keys.ToList();
            List<PlayerBackgroundData> newDatas = new();

            while (keys.Count > 0)
            {
                int key = keys.Max();
                newDatas.AddRange(tempData[key]);
                keys.Remove(key);
            }

            playerBackgroundDatas = newDatas;
        }

        /// <summary>
        /// Autoloads every texture in PlayerBackgrounds/Textures/.
        /// </summary>
        private static void LoadBackgroundIcons()
        {
            const string AssetPath = "Assets/Textures/BackgroundIcons/";

            var mod = ModContent.GetInstance<NewBeginnings>();
            var assets = mod.Assets.GetLoadedAssets();
            var realIcons = mod.GetFileNames().Where(x => x.StartsWith(AssetPath) && x.EndsWith(".rawimg"));

            foreach (var item in realIcons)
                backgroundIcons.Add(item[AssetPath.Length..].Replace(".rawimg", string.Empty), mod.Assets.Request<Texture2D>(item.Replace(".rawimg", string.Empty)));
        }

        /// <summary>Adds a new background with the given info.</summary>
        /// <param name="name">Name of the background; will be used in the UI and player title.</param>
        /// <param name="tex">Name of the texture for the icon; i.e. Bookworm. Must be an image in PlayerBackgrounds/Textures/.</param>
        /// <param name="desc">Description of the background to be shown on the char creation UI.</param>
        /// <param name="equipData">The armor and accessories the player will wear.</param>
        /// <param name="addInvToDesc">Whether the backgrounds inventory and accessories are to be shown in the description.</param>
        private static void AddNewBG(string name, string tex, string desc, EquipData? equipData = null, MiscData? miscData = null, params (int type, int stack)[] inv)
        {
            var data = new PlayerBackgroundData(name, tex, desc, equipData, miscData, inv);
            playerBackgroundDatas.Add(data);
        }

        private static void AddNewBG(string name, string tex, string desc, EquipData? equipData = null, MiscData? miscData = null, DelegateData? delegateData = null, params (int type, int stack)[] inv)
        {
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
    }
}