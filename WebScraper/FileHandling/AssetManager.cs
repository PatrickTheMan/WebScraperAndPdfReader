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
