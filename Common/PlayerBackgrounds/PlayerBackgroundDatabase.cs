using Microsoft.Xna.Framework.Graphics;
using NewBeginnings.Common.Crossmod;
using NewBeginnings.Common.PlayerBackgrounds.Containers;
using NewBeginnings.Content.Items;
using NewBeginnings.Content.Items.Tools;
using NewBeginnings.Content.Items.Vanity;
using NewBeginnings.Content.Items.Weapon;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds;

internal static class PlayerBackgroundDatabase
{
    internal static List<PlayerBackgroundData> playerBackgroundDatas = [];
    internal static Dictionary<string, Asset<Texture2D>> backgroundIcons = [];

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
        AddNewBGItemlessDesc("Purist", "Purist", null, new MiscData(sortPriority: 12));
        AddNewBG("Knight", "Knight", new EquipData(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves), new MiscData(swordReplace: ItemID.SilverBroadsword, sortPriority: 11, stars: 1));
        AddNewBG("Huntsman", "Huntsman", EquipData.SingleAcc(ItemID.HunterCloak), new MiscData(sortPriority: 11, stars: 1), (ItemID.GoldBow, 1), (ItemID.EndlessQuiver, 1));
        AddNewBG("Wizard", "Wizard", new EquipData(ItemID.WizardHat, ItemID.TopazRobe, 0), new MiscData(80, 60, swordReplace: ItemID.TopazStaff, sortPriority: 11, stars: 1));
        AddNewBG("Beastmaster", "Beastmaster", new MiscData(80, sortPriority: 11, stars: 1), (ItemID.SlimeStaff, 1), (ItemID.BlandWhip, 1));
        AddNewBG("Shinobi", "Shinobi", EquipData.SingleAcc(ItemID.Tabi), new MiscData(80, stars: 1), (ItemID.Katana, 1));
        AddNewBG("Alchemist", "Alchemist", (ItemID.AlchemyTable, 1), (ItemID.BottledWater, 50), (ItemID.HerbBag, 12));
        AddNewBG("Demoman", "Demolitionist", new MiscData(npcType: NPCID.Demolitionist), (ItemID.Dynamite, 2), (ItemID.Bomb, 10), (ItemID.Grenade, 15));
        AddNewBG("Boomer", "Boomer", new EquipData(ItemID.Sunglasses), (ItemID.LawnMower, 1), (ItemID.BBQRibs, 2), (ItemID.GrilledSquirrel, 1));
        AddNewBG("Zoomer", "Zoomer", new EquipData(ItemID.Goggles), new MiscData(40, stars: 4), (ItemID.CellPhone, 1));
        AddNewBG("Tiger", "Tiger", new EquipData(ItemID.CatEars, 0, ItemID.FoxTail, ItemID.TigerClimbingGear), new MiscData(stars: 1), (ItemID.BladedGlove, 1));
        AddNewBG("Builder", "Builder", new EquipData(0, 0, 0, ItemID.PortableStool, ItemID.ArchitectGizmoPack), new MiscData(npcType : NPCID.Painter), (ItemID.Wood, 500), (ItemID.StoneBlock, 500));
        AddNewBG("Thief", "Thief", new EquipData(0, 0, 0, ItemID.LuckyCoin), new MiscData(stars: 2), (ItemID.PlatinumShortsword, 1), (ItemID.PoisonedKnife, 300)); //replace plat shortsword with thief's dagger
        AddNewBG("Firestarter", "Firestarter", new MiscData(stars: 2), (ItemID.WandofSparking, 1), (ItemID.MolotovCocktail, 300), (ItemID.FlareGun, 1), (ItemID.Flare, 50), (ItemID.Torch, 100));
        
        AddNewBG("Chef", "Chef", new EquipData(ItemID.ChefHat, ItemID.ChefShirt, ItemID.ChefPants), 
            (ItemID.BloodyMachete, 1), (ItemID.CookingPot, 1), (ItemID.CookedFish, 3), (ItemID.Apple, 3), (ItemID.CookedShrimp, 1), (ItemID.FruitSalad, 1));

        playerBackgroundDatas.Add(new Pirate());
        
        AddNewBG("Deprived", "Deprived", null, new MiscData(80, swordReplace: ModContent.ItemType<DeprivedBlade>(), stars: 2), (ModContent.ItemType<DeprivedLantern>(), 1), (ItemID.HealingPotion, 3));

        AddNewBG("Lumberjack", "Lumberjack", new EquipData(0, ModContent.ItemType<LumberjackFlannel>()), new MiscData(axeReplace: ModContent.ItemType<LumberjackAxe>(), stars: 1), (ItemID.Sawmill, 1), (ItemID.Apple, 12), (ItemID.Wood, 300), (ItemID.BorealWood, 300), 
            (ItemID.PalmWood, 300), (ItemID.Ebonwood, 300), (ItemID.Shadewood, 300), (ItemID.RichMahogany, 300), (ItemID.DynastyWood, 300));

        AddNewBG("Nobleman", "Nobleman",
            EquipData.AccFirst([ItemID.DiamondRing, ItemID.GoldWatch]), new MiscData(20, stars: 5, npcType: NPCID.TaxCollector), new DelegateData(modifyCreation: (plr) =>
        {
            if (plr.difficulty is PlayerDifficultyID.SoftCore or PlayerDifficultyID.Creative)
                plr.difficulty = PlayerDifficultyID.MediumCore;
        }), (ItemID.PlatinumCoin, 1));
        
        AddNewBG("Fisherman", "Fisherman", EquipData.AccFirst(ItemID.HighTestFishingLine, ItemID.AnglerHat), new MiscData(npcType: NPCID.Angler), (ItemID.ReinforcedFishingPole, 1), (ItemID.CanOfWorms, 3));
        AddNewBG("Trailblazer", "Trailblazer", EquipData.AccFirst([ItemID.HermesBoots, ItemID.Aglet, ItemID.AnkletoftheWind], ModContent.ItemType<TrailblazerHelmet>()), new MiscData(stars: 1));

        playerBackgroundDatas.Add(new Adventurer());

        AddNewBG("Farmer", "Farmer", new EquipData(ItemID.SummerHat), new MiscData(stars: 2), (ItemID.Sickle, 1), (ItemID.Hay, 100), (ItemID.DaybloomSeeds, 12), (ItemID.BlinkrootSeeds, 12), (ItemID.MoonglowSeeds, 12), (ItemID.WaterleafSeeds, 12), 
            (ItemID.ShiverthornSeeds, 12), (ItemID.DeathweedSeeds, 12), (ItemID.FireblossomSeeds, 12), (ItemID.BlinkrootPlanterBox, 5), (ItemID.CorruptPlanterBox, 5), (ItemID.CrimsonPlanterBox, 5), 
            (ItemID.DayBloomPlanterBox, 5), (ItemID.FireBlossomPlanterBox, 5), (ItemID.MoonglowPlanterBox, 5), (ItemID.ShiverthornPlanterBox, 5), (ItemID.WaterleafPlanterBox, 5)); //Gonna need a custom straw hat vanity item to replace the summer hat. 

        AddNewBG("Spelunker", "Spelunker", EquipData.AccFirst(ItemID.AncientChisel, ItemID.MiningHelmet), new MiscData(stars: 2), (ItemID.GoldPickaxe, 1), (ItemID.Bomb, 10), (ItemID.SpelunkerPotion, 10), (ItemID.Minecart, 1));
        AddNewBG("Bookworm", "Bookworm", new MiscData(stars: 2), (ModContent.ItemType<WornSpellbook>(), 1), (ItemID.CordageGuide, 1), (ItemID.Book, 8), (ItemID.DontHurtCrittersBook, 1));
        AddNewBG("Druid", "Druid", EquipData.SingleAcc(ItemID.CordageGuide), (ItemID.Vilethorn, 1), (ItemID.StaffofRegrowth, 1), (ItemID.HerbBag, 3), (ItemID.ClayPot, 10));
        AddNewBG("Painter", "Painter", null, new MiscData(swordReplace: ItemID.PainterPaintballGun, npcType: NPCID.Painter), (ItemID.PaintScraper, 1), (ItemID.PaintSprayer, 1), (ItemID.Paintbrush, 1), (ItemID.PaintRoller, 1));
        AddNewBG("Australian", "Australian", new EquipData(ItemID.CowboyHat, ItemID.CowboyJacket, ItemID.CowboyPants), new MiscData(swordReplace: ModContent.ItemType<KylieBoomerang>()));

        playerBackgroundDatas.Add(new Bereaved());
        playerBackgroundDatas.Add(new Frozen());
        playerBackgroundDatas.Add(new Slayer());

        AddNewBG("Alternate", "Alternate", null, new MiscData(swordReplace: ItemID.TinShortsword, pickReplace: ItemID.TinPickaxe, axeReplace: ItemID.TinAxe), new DelegateData(() => UnlockabilitySystem.UnlockSaveData.Unlocked("Beginner")));

        playerBackgroundDatas.Add(new Accursed());
        playerBackgroundDatas.Add(new ReallyConfused());
        playerBackgroundDatas.Add(new Inevitable());

        playerBackgroundDatas.AddRange(OriginCalls._crossModDatas);

        AddNewBGItemlessDesc("Random", "Random", null, new MiscData(sortPriority: 0, stars: 0));
    }

    private static void SortBGDatas()
    {
        Dictionary<int, List<PlayerBackgroundData>> tempData = [];

        foreach (var item in playerBackgroundDatas)
        {
            if (!tempData.ContainsKey(item.Misc.SortPriority))
                tempData.Add(item.Misc.SortPriority, [item]);
            else
                tempData[item.Misc.SortPriority].Add(item);
        }

        var keys = tempData.Keys.ToList();
        List<PlayerBackgroundData> newDatas = [];

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

        foreach (string item in realIcons)
            backgroundIcons.Add(item[AssetPath.Length..].Replace(".rawimg", string.Empty), mod.Assets.Request<Texture2D>(item.Replace(".rawimg", string.Empty)));
    }

    /// <summary>Adds a new background with the given info.</summary>
    /// <param name="name">Name of the background; will be used in the UI and player title.</param>
    /// <param name="tex">Name of the texture for the icon; i.e. Bookworm. Must be an image in PlayerBackgrounds/Textures/.</param>
    /// <param name="flavour">Description of the background to be shown on the char creation UI.</param>
    /// <param name="equipData">The armor and accessories the player will wear.</param>
    /// <param name="addInvToDesc">Whether the backgrounds inventory and accessories are to be shown in the description.</param>
    private static void AddNewBG(string langKey, string tex, EquipData? equipData = null, MiscData? miscData = null, params (int type, int stack)[] inv)
    {
        langKey = "Mods.NewBeginnings.Origins." + langKey;
        var data = new PlayerBackgroundData(langKey, tex, equipData, miscData, inv);
        playerBackgroundDatas.Add(data);
    }

    private static void AddNewBG(string langKey, string tex, EquipData? equipData = null, MiscData? miscData = null, DelegateData? delegateData = null, params (int type, int stack)[] inv)
    {
        langKey = "Mods.NewBeginnings.Origins." + langKey;
        var data = new PlayerBackgroundData(langKey, tex, equipData, miscData, inv)
        {
            Delegates = delegateData ?? new DelegateData()
        };
        playerBackgroundDatas.Add(data);
    }

    private static void AddNewBG(string langKey, string tex, EquipData? equipData = null, params (int type, int stack)[] inv) => AddNewBG(langKey, tex, equipData, null, inv);
    private static void AddNewBG(string langKey, string tex, MiscData? miscData = null, params (int type, int stack)[] inv) => AddNewBG(langKey, tex, null, miscData, inv);
    private static void AddNewBG(string langKey, string tex, params (int type, int stack)[] inv) => AddNewBG(langKey, tex, null, null, inv);

    private static void AddNewBGItemlessDesc(string langKey, string tex, EquipData? equipData = null, MiscData? miscData = null, params (int type, int stack)[] inv)
    {
        langKey = "Mods.NewBeginnings.Origins." + langKey;
        var data = new PlayerBackgroundData(langKey, tex, equipData, miscData, inv);
        playerBackgroundDatas.Add(data);
    }
}