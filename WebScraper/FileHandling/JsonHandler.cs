using Newtonsoft.Json;
using WebScraperProject.Logging;

namespace WebScraperProject.Filehandling
{
    public static class JsonHandler
    {
        /// <summary>
        /// Serialize an object to a JSON string
        /// </summary>
        /// <param name="obj">The object to be serialized</param>
        /// <returns>The string representation of the object</returns>
        public static string? Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogType.Error, $"Error while serializing object: {e.Message}");
                return null;
            }
        }
        /// <summary>
        /// Deserialize a JSON string to an object
        /// </summary>
        /// <typeparam name="T">The desired type to get returned</typeparam>
        /// <param name="s">The string representation of the object</param>
        /// <returns>The object</returns>
        public static T? Deserialize<T>(string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogType.Error, $"Error while deserializing object: {e.Message}");
                return default;
            }
        }
    }
}
