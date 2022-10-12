using System;
using System.IO;

namespace NewBeginnings.Common.UnlockabilitySystem
{
    internal class UnlockabilityIO
    {
        public const string SaveName = "nb_unlocks";

        public const int EncryptionKey = 8;

        public static void LoadData()
        {
            UnlockSaveData.Populate();

            string filePath = SecureGetSavePath();

            FileStream file = File.Open(filePath, FileMode.Create);
            using StreamReader reader = new(file);

            if (reader.Equals(string.Empty))
                return;

            string text = reader.ReadToEnd();
            string[] keys = text.Split(' ');

            foreach (var item in keys)
            {
            }
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
                save += Encrypt(key) + " ";

            string filePath = SecureGetSavePath();
        }

        private static string Encrypt(string orig)
        {
            string compare = orig.ToLower();
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            string str = "";

            foreach (char c in compare)
            {
                int origIndex = Array.IndexOf(chars, c);
                origIndex += EncryptionKey;

                if (origIndex >= chars.Length)
                    origIndex -= chars.Length;

                str += chars[origIndex];
            }

            return str;
        }

        private static string Decrypt(string orig)
        {
            string compare = orig.ToLower();
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            string str = "";

            foreach (char c in compare)
            {
                int origIndex = Array.IndexOf(chars, c);
                origIndex -= EncryptionKey;

                if (origIndex < 0)
                    origIndex += chars.Length;

                str += chars[origIndex];
            }

            return str;
        }
    }
}
