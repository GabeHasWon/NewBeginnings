using System;
using System.IO;
using System.Linq;
using Terraria.ModLoader.IO;

namespace NewBeginnings.Common.UnlockabilitySystem;

#nullable enable

/// <summary>
/// Class used to save/load data regarding unlocks.
/// </summary>
internal class UnlockabilityIO
{
    public const string SaveName_Legacy = "nb_unlocks";
    public const string SaveName = "NewBeginningsUnlocks";

    public const int EncryptionKey = 8;

    private static readonly char[] Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ".ToCharArray();

    public static void LoadData()
    {
        UnlockSaveData.Populate();

        string filePath = SecureGetSavePath(true, out bool hasLegacy);

        if (hasLegacy)
            LoadLegacy(filePath);
        else
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                return;
            }

            TagCompound compound = TagIO.FromFile(filePath, true);
            string[] unlocks = compound.GetString("unlocks").Split(',');

            foreach (string unlock in UnlockSaveData.achievementsByName.Keys)
                if (unlocks.Any(x => x.Equals(unlock, StringComparison.OrdinalIgnoreCase)))
                    UnlockSaveData.Complete(unlock, null, true, true);
        }
    }

    private static bool LoadLegacy(string filePath)
    {
        // Define a scope for the "used" files here, so we can delete it safely after they're disposed
        {
            using FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
            using StreamReader reader = new(file);
            string text = reader.ReadToEnd();

            if (text == string.Empty)
                return false;

            string[] keys = text.Split(',');

            foreach (string item in keys)
                if (item != string.Empty)
                    UnlockSaveData.Complete(Decrypt(item), null, true, true);
        }

        // Replace old file with new
        File.Delete(filePath);

        foreach (string unlock in UnlockSaveData.achievementsByName.Keys)
            if (UnlockSaveData.achievementsByName[unlock].Unlocked)
                QuickSave(unlock);

        return true;
    }

    private static string SecureGetSavePath(bool allowLegacy, out bool hasLegacy)
    {
        string path = Directory.GetCurrentDirectory();
        path = Path.Combine(path, "ModSaveData");
        Directory.CreateDirectory(path);
        string filePath = Path.Combine(path, $"{SaveName_Legacy}.txt");

        if (Path.Exists(filePath) && allowLegacy)
            hasLegacy = true;
        else
        {
            hasLegacy = false;
            filePath = Path.Combine(path, $"{SaveName}.txt");
        }

        return filePath;
    }

    /// <summary>
    /// Safely adds a given achievement key to the save data. <paramref name="key"/> can be null if the only goal is to create the file.
    /// </summary>
    internal static void QuickSave(string? key)
    {
        string save = string.Empty;

        if (key != null && UnlockSaveData.achievementsByName[key].Unlocked)
            save += key + ",";

        string filePath = SecureGetSavePath(false, out _);
        TagCompound tag = [];
        tag.Add("unlocks", save);
        TagIO.ToFile(tag, filePath, true);
    }

    private static string Decrypt(string orig)
    {
        string str = "";

        foreach (char c in orig)
        {
            int origIndex = Array.IndexOf(Characters, c);
            origIndex -= EncryptionKey;

            if (origIndex < 0)
                origIndex += Characters.Length;

            str += Characters[origIndex];
        }

        return str;
    }
}
