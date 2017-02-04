using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Standard_Assets._2D.Scripts.Game_State
{
    public static class ArxGameSaveHandler
    {
        public static string SaveGamesDirectory
        {
            get
            {
                return Application.persistentDataPath + "/Saves";
            }
        }

        public static List<ArxSaveData> savedGames = new List<ArxSaveData>();

        static ArxGameSaveHandler()
        {
            if (!Directory.Exists(SaveGamesDirectory))
            {
                Directory.CreateDirectory(SaveGamesDirectory);
            }
        }

        public static void Save()
        {
            ArxSaveData.Current.LevelName = SceneManager.GetActiveScene().name;
            SaveAux();
        }

        public static void Save(string levelName)
        {
            ArxSaveData.Current.LevelName = levelName;
            SaveAux();
        }

        public static void Load()
        {
            if (File.Exists(SaveGamesDirectory + "/savedGames.gd"))
            {
                var bf = new BinaryFormatter();
                var file = File.Open(SaveGamesDirectory + "/savedGames.gd", FileMode.Open);
                savedGames = (List<ArxSaveData>)bf.Deserialize(file);
                file.Close();
            }
        }

        private static void SaveAux()
        {
            if (!savedGames.Contains(ArxSaveData.Current))
            {
                savedGames.Add(ArxSaveData.Current);
            }
            var bf = new BinaryFormatter();
            var file = File.Create(SaveGamesDirectory + "/savedGames.gd");
            bf.Serialize(file, savedGames);
            file.Close();
        }
    }
}
