using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Eshopper_website.Utils.Extension
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static void SetDecimal(this ISession session, string key, decimal value)
        {
            session.SetString(key, value.ToString());
        }

        public static decimal GetDecimal(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? 0 : decimal.Parse(value);
        }
    }
} 