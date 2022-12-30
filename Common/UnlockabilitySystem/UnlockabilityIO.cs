using System;
using System.IO;

namespace NewBeginnings.Common.UnlockabilitySystem
{
    /// <summary>
    /// Class used to save/load data regarding unlocks.
    /// </summary>
    internal class UnlockabilityIO
    {
        public const string SaveName = "nb_unlocks";
        public const int EncryptionKey = 8;

        private static readonly char[] Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ".ToCharArray();

        public static void LoadData()
        {
            UnlockSaveData.Populate();

            string filePath = SecureGetSavePath();

            FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
            using StreamReader reader = new(file);

            string text = reader.ReadToEnd();

            if (text == string.Empty)
                return;

            string[] keys = text.Split(',');

            foreach (var item in keys)
                if (item != string.Empty)
                    UnlockSaveData.Complete(Decrypt(item), true, true);
        }

        private static string SecureGetSavePath()
        {
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, "ModSaveData");
            Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, $"{SaveName}.txt");

            if (!File.Exists(filePath)) //If the file data doesn't exist, make an empty file and exit
                File.Create($"{filePath}.txt");
            
            return filePath;
        }

        internal static void QuickSave(string key)
        {
            string save = string.Empty;

            if (UnlockSaveData.achievementsByName[key].Unlocked)
                save += Encrypt(key) + ",";

            string filePath = SecureGetSavePath();
            using StreamWriter writer = new(filePath, append: true);

            writer.Write(save);
        }

        private static string Encrypt(string orig)
        {
            string str = "";

            foreach (char c in orig)
            {
                int origIndex = Array.IndexOf(Characters, c);
                origIndex += EncryptionKey;

                if (origIndex >= Characters.Length)
                    origIndex -= Characters.Length;

                str += Characters[origIndex];
            }

            return str;
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
}
