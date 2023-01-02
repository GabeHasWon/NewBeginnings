using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.PlayerBackgrounds.Containers;
using NewBeginnings.Content.Items;
using ReLogic.Content;
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

            AddNewBGItemlessDesc("Purist", "Purist", "The normal Terraria experience.", "", null, new MiscData(sortPriority: 11));
            AddNewBG("Knight", "Knight", "A noble warrior, clad in iron.", "Starts with a full set of iron armor and a silver broadsword.", new EquipData(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves), new MiscData(sortPriority: 11, stars: 1), (ItemID.SilverBroadsword, 1));
            AddNewBG("Huntsman", "Huntsman", "Steady hands and keen eyes, a master of the hunt.", "Starts with a bow, an endless quiver and a hunter cloak.", EquipData.SingleAcc(ItemID.HunterCloak), new MiscData(sortPriority: 11, stars: 1), (ItemID.GoldBow, 1), (ItemID.EndlessQuiver, 1));
            AddNewBG("Wizard", "Wizard", "An apprentice wizard with an affinity for the arcane.", "Starts with a wizard hat, a sapphire staff and a sapphire robe, with additional mana but lower max health.", new EquipData(ItemID.WizardHat, ItemID.SapphireRobe, 0), new MiscData(80, 60, sortPriority: 11, stars: 1), (ItemID.SapphireStaff, 1));
            AddNewBG("Beastmaster", "Beastmaster", "Raised in the woodlands, they summon beasts to aid their journey.", "Lower max health but an immediate slime staff and leather whip.", new MiscData(80, sortPriority: 11, stars: 2), (ItemID.SlimeStaff, 1), (ItemID.BlandWhip, 1));
            AddNewBG("Shinobi", "Shinobi", "A deadly mercenary assassin from the east. Fast, nimble, and with lethal efficiency.", "Start with a tabi and a katana but 80 max health.", EquipData.SingleAcc(ItemID.Tabi), new MiscData(80, stars: 1), (ItemID.Katana, 1));
            AddNewBG("Alchemist", "Alchemist", "A master of nature and chemicals; prepared for anything.", "Start with an alchemy table, 50 bottled water and 12 herb bags.", (ItemID.AlchemyTable, 1), (ItemID.BottledWater, 50), (ItemID.HerbBag, 12));
            AddNewBG("Demoman", "Demolitionist", "Hurl explosives at ore, enemies, or yourself!", "Starts with 1 dynamite, 5 bombs, and 10 grenades.", (ItemID.Dynamite, 1), (ItemID.Bomb, 5), (ItemID.Grenade, 10));
            AddNewBG("Boomer", "Boomer", "Back in my day...", "Starts with a lawn mower, sunglasses, BBQ ribs and a grilled squirrel.", new EquipData(ItemID.Sunglasses), (ItemID.LawnMower, 1), (ItemID.BBQRibs, 2), (ItemID.GrilledSquirrel, 1));
            AddNewBG("Zoomer", "Zoomer", "Terreddit post is popping off today boutta frag some slimes fr fr", "Starts with goggles, a cell phone and 40 max health.", new EquipData(ItemID.Goggles), new MiscData(40, stars: 4), (ItemID.CellPhone, 1));
            AddNewBG("Tiger", "Tiger", "Lightly more feral than other Terrarians, but not as much as you'd think!", "Starts with cat ears, a fox tail, tiger climbing gear, and a bladed glove.", new EquipData(ItemID.CatEars, 0, ItemID.FoxTail, ItemID.TigerClimbingGear), new MiscData(stars: 1), (ItemID.BladedGlove, 1));
            
            AddNewBG("Builder", "Builder", "A renowned architect from the cities.", "Starts with a portable stool, an architect gizmo pack, and the Painter instead of the Guide in new worlds.", new EquipData(0, 0, 0, ItemID.PortableStool, ItemID.ArchitectGizmoPack), new MiscData(npcType : NPCID.Painter), 
                (ItemID.Wood, 500), (ItemID.StoneBlock, 500));
            
            AddNewBG("Thief", "Thief", "Petty thief with a penchant for pickpocketing.", "Starts with a lucky coin, platinum shortsword and 300 poisoned knives.", new EquipData(0, 0, 0, ItemID.LuckyCoin), new MiscData(stars: 2), 
                (ItemID.PlatinumShortsword, 1), (ItemID.PoisonedKnife, 300)); //replace plat shortsword with thief's dagger
            
            AddNewBG("Firestarter", "Firestarter", "Some people just want to watch the world burn.", "Starts with a magma stone, 300 molotov cocktails, a flare gun with 50 flares, and 100 torches.", new EquipData(0, 0, 0, ItemID.MagmaStone), new MiscData(stars: 2), 
                (ItemID.MolotovCocktail, 300), (ItemID.FlareGun, 1), (ItemID.Flare, 50), (ItemID.Torch, 100));
            
            AddNewBG("Pirate", "Pirate", "Aye. A real pirate knows when land can be plundered, and plunder they shall!", "Starts with an eye patch, a gold ring, a sextant, a rusty cutlass, a keg and 200 sails.", 
                new EquipData(ItemID.EyePatch, 0, 0, ItemID.GoldRing, ItemID.Sextant, 1), new MiscData(stars: 2), (ModContent.ItemType<RustyCutlass>(), 1), (ItemID.Keg, 1), (ItemID.Sail, 200));
            
            AddNewBG("Deprived", "Deprived", "A forgotten warrior carrying long-lost tools.", "Starts with 80 max health, a deprived blade, a deprived lantern and a healing potion.", null, new MiscData(80, swordReplace: ModContent.ItemType<DeprivedBlade>(), stars: 2), 
                (ModContent.ItemType<DeprivedLantern>(), 1), (ItemID.HealingPotion, 3));

            AddNewBG("Lumberjack", "Lumberjack", "A humble lumberjack and his trusty axe.", "Starts with Lucy the Axe, a sawmill, 12 apples, and 300 of normal, boreal, palm, ebon, shade, rich mahogany, and dynasty wood.", 
                new MiscData(stars: 1), (ItemID.LucyTheAxe, 1), (ItemID.Sawmill, 1), (ItemID.Apple, 12), (ItemID.Wood, 300), (ItemID.BorealWood, 300), 
                (ItemID.PalmWood, 300), (ItemID.Ebonwood, 300), (ItemID.Shadewood, 300), (ItemID.RichMahogany, 300), (ItemID.DynastyWood, 300));

            AddNewBG("Nobleman", "Nobleman", "A hard worker if you think counting money is hard. NOTE: Can only be played in Mediumcore or Hardcore.", 
                "Starts with a diamond ring, a gold watch, 20 max health, and a platinum coin - in mediumcore or hardcore only.",
                EquipData.AccFirst(new int[] { ItemID.DiamondRing, ItemID.GoldWatch }), new MiscData(20, stars: 5), new DelegateData(modifyCreation: (plr) =>
            {
                if (plr.difficulty == PlayerDifficultyID.SoftCore || plr.difficulty == PlayerDifficultyID.Creative)
                    plr.difficulty = PlayerDifficultyID.MediumCore;
            }), (ItemID.PlatinumCoin, 1));

            AddNewBG("Fisherman", "Fisherman", "Slimes want me, fish fear me...", "Starts with a high test fishing line, angler hat, a reinforced fishing pole, 3 cans of worms, and the Angler instead of the Guide.",
                EquipData.AccFirst(ItemID.HighTestFishingLine, ItemID.AnglerHat), new MiscData(npcType: NPCID.Angler), (ItemID.ReinforcedFishingPole, 1), (ItemID.CanOfWorms, 3));

            AddNewBG("Trailblazer", "Trailblazer", "No time to explain. They have places to go, things to see!", "Starts with hermes boots, an aglet, and an anklet of the wind.", 
                EquipData.AccFirst(new int[] { ItemID.HermesBoots, ItemID.Aglet, ItemID.AnkletoftheWind }), new MiscData(stars: 1)); //Needs the winged helmet vanity

            playerBackgroundDatas.Add(new Adventurer());

            AddNewBG("Farmer", "Farmer", "It ain't much, but it's honest work.", "Starts with a summer hat, a sickle, 200 hay, and 12 daybloom, blinkroot, moonglow, waterleaf, shiverthorn, deathweed, and fireblossom seeds.", 
                new EquipData(ItemID.SummerHat), new MiscData(stars: 2), (ItemID.Sickle, 1), (ItemID.Hay, 200), (ItemID.DaybloomSeeds, 12), (ItemID.BlinkrootSeeds, 12), (ItemID.MoonglowSeeds, 12), (ItemID.WaterleafSeeds, 12), 
                (ItemID.ShiverthornSeeds, 12), (ItemID.DeathweedSeeds, 12), (ItemID.FireblossomSeeds, 12)); //Gonna need a custom straw hat vanity item to replace the summer hat. 

            AddNewBG("Spelunker", "Spelunker", "The caves call and they answer. Those ores aren't gonna mine themselves!", "Starts with an ancient chisel, mining helmet, a gold pickaxe, 15 bombs and 10 spelunker potions.", 
                EquipData.AccFirst(ItemID.AncientChisel, ItemID.MiningHelmet), new MiscData(stars: 2), (ItemID.GoldPickaxe, 1), (ItemID.Bomb, 15), (ItemID.SpelunkerPotion, 10));

            AddNewBG("Bookworm", "Bookworm", "Mind over matter. The best way to fight is with a sharpened mind!", "Starts with a worn spellbook, a guide to plant fiber cordage, 8 books and a guide to critter companionship.", 
                new MiscData(stars: 2), (ModContent.ItemType<WornSpellbook>(), 1), (ItemID.CordageGuide, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));

            AddNewBG("Druid", "Druid", "A herald of nature, engaged with keeping the world alive and healthy!", "Starts with a guide to plant fiber cordage, a vilethorn, a staff of regrowth, 3 herb bags, and 10 clay pots.", 
                EquipData.SingleAcc(ItemID.CordageGuide), (ItemID.Vilethorn, 1), (ItemID.StaffofRegrowth, 1), (ItemID.HerbBag, 3), (ItemID.ClayPot, 10));

            playerBackgroundDatas.Add(new Slayer());

            AddNewBG("Alternate", "Alternate", "Perhaps, if things had been a little different, this'd be a purist.", "Starts with tin tools instead of copper\nUnlocked by using your first origin.", null, 
                new MiscData(swordReplace: ItemID.TinShortsword, pickReplace: ItemID.TinPickaxe, axeReplace: ItemID.TinAxe), new DelegateData(() => UnlockabilitySystem.UnlockSaveData.Unlocked("Beginner")));

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
        /// <param name="flavour">Description of the background to be shown on the char creation UI.</param>
        /// <param name="equipData">The armor and accessories the player will wear.</param>
        /// <param name="addInvToDesc">Whether the backgrounds inventory and accessories are to be shown in the description.</param>
        private static void AddNewBG(string name, string tex, string flavour, string description,  EquipData? equipData = null, MiscData? miscData = null, params (int type, int stack)[] inv)
        {
            var data = new PlayerBackgroundData(name, tex, flavour, description, equipData, miscData, inv);
            playerBackgroundDatas.Add(data);
        }

        private static void AddNewBG(string name, string tex, string flavour, string description, EquipData? equipData = null, MiscData? miscData = null, DelegateData? delegateData = null, params (int type, int stack)[] inv)
        {
            var data = new PlayerBackgroundData(name, tex, flavour, description, equipData, miscData, inv);
            data.Delegates = delegateData ?? new DelegateData();
            playerBackgroundDatas.Add(data);
        }

        private static void AddNewBG(string name, string tex, string flavour, string description, EquipData? equipData = null, params (int type, int stack)[] inv) => AddNewBG(name, tex, flavour, description, equipData, null, inv);
        private static void AddNewBG(string name, string tex, string flavour, string description, MiscData? miscData = null, params (int type, int stack)[] inv) => AddNewBG(name, tex, flavour, description, null, miscData, inv);
        private static void AddNewBG(string name, string tex, string flavour, string description, params (int type, int stack)[] inv) => AddNewBG(name, tex, flavour, description, null, null, inv);

        private static void AddNewBGItemlessDesc(string name, string tex, string flavour, string description, EquipData? equipData = null, MiscData? miscData = null, params (int type, int stack)[] inv)
        {
            var data = new PlayerBackgroundData(name, tex, flavour, description, equipData, miscData, inv);
            playerBackgroundDatas.Add(data);
        }
    }
}