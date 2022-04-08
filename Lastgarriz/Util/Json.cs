using System.Text;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Lastgarriz.Util
{
    internal static class Json //: IJsonFormatter
    {
        internal static string Serialize<T>(object obj) where T : class
        {
            return JsonSerializer.ToJsonString((T)obj, StandardResolver.AllowPrivateExcludeNullSnakeCase);
        }

        internal static T Deserialize<T>(string strData) where T : class
        {
            byte[] data = Encoding.UTF8.GetBytes(strData);
            return JsonSerializer.Deserialize<T>(data, StandardResolver.AllowPrivateExcludeNullSnakeCase);
        }
    }
}
