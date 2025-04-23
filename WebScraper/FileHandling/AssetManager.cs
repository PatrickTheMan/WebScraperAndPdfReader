using System;
using System.IO;
using System.Linq;
using System.Reflection;
using WebScraperProject.Logging;

namespace WebScraperProject.Filehandling
{
    #region Enums
    public enum AssetType
    {
        PDF
    }
    #endregion
    public static class AssetManager
    {
        #region Variables
        private static string _baseFolderPath => $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\Assets";
        private static Dictionary<AssetType, string> _assetExtentionDic = new()
        {
            {AssetType.PDF, ".pdf"}
        };
        #endregion

        #region Public Get Methods
        /// <summary>
        /// Gets a local asset from the local folder (This is used to store local data that was saved via <see cref="SaveLocalAsset{T}(object)"/>)
        /// </summary>
        /// <typeparam name="T">The type of the asset that is being retrieved (Used as the name for the .txt file)</typeparam>
        /// <returns>The desired asset given by type</returns>
        /// <exception cref="AssetNotFoundException"></exception>
        /// <exception cref="AssetException"></exception>
        public static T? GetLocalAsset<T>()
        {
            string path = $"{_baseFolderPath}\\Local\\{typeof(T)}.txt";
            Logger.Log($"AssetPath: {path}");

            if (!File.Exists(path))
                throw new Exception("Asset not found: " + path);

            try
            {
                string jsonString = File.ReadAllText(path);
                return JsonHandler.Deserialize<T>(jsonString);
            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogType.Error, $"Error while getting asset: {e.Message}");
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Gets a path to an asset via the asset name, uses recursion to find the asset (Will look through all dir under .../Assets)
        /// </summary>
        /// <param name="assetName">The name of the desired file</param>
        /// <param name="filePath">The start position for looking for the asset (Null will use baseFolderPath)</param>
        /// <returns>The path for the desired asset</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="AssetNotFoundException"></exception>
        public static string? GetAssetPath(string assetName, string? filePath = null)
        {
            if (string.IsNullOrEmpty(assetName))
                throw new NullReferenceException();

            // Set base path if not given
            if (string.IsNullOrEmpty(filePath)) 
                filePath = _baseFolderPath;

            // Try and find the asset in the current dir
            string? foundFilePath = Directory.GetFiles(filePath).FirstOrDefault(f => f.Contains(assetName));
            if (!string.IsNullOrEmpty(foundFilePath))
            {
                return foundFilePath;
            }

            // Find all dir in the current dir and use recursion to find the asset
            string[] foundDirPaths = Directory.GetDirectories(filePath);
            foreach (string dirPath in foundDirPaths)
            {
                string? foundPath = GetAssetPath(assetName, dirPath);
                if (!string.IsNullOrEmpty(foundPath))
                    return foundPath;
            }

            if (filePath.Equals(_baseFolderPath))
                throw new Exception($"Asset not found: {filePath}\\{assetName}");
            else
                return null;
        }
        #endregion

        #region Public Save Methods
        /// <summary>
        /// Saves an asset to the local folder (This is used to store local data that can be retrieved via <see cref="GetLocalAsset{T}"/>)
        /// </summary>
        /// <typeparam name="T">The type of the asset that is being saved (Used as the name for the .txt file)</typeparam>
        /// <param name="o">The object that is to be saved as an asset</param>
        /// <returns>The path for the saved asset</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static string SaveLocalAsset<T>(T asset)
        {
            if (asset == null)
                throw new NullReferenceException();

            string path = $"{_baseFolderPath}\\Local\\{typeof(T)}.txt";
            Logger.Log($"AssetPath: {path}");
            string jsonString = JsonHandler.Serialize(asset);
            File.WriteAllText(path, jsonString);
            Logger.Log($"SaveAsset<{typeof(T)}> has been executed");
            return path;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static string SaveAsset(string name, AssetType type, Stream inputStream)
        {
            if (inputStream == null)
                throw new NullReferenceException();

            string path = $"{_baseFolderPath}\\{type}\\{name}{_assetExtentionDic[type]}";
            Logger.Log($"AssetPath: {path}");
            using FileStream outputFileStream = new(path, FileMode.Create);
            inputStream.CopyTo(outputFileStream);
            Logger.Log($"SaveAsset<{type}> has been executed");
            return path;
        }
        #endregion
    }
}
