using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Project_Equinox.Models
{
        public static class CookieHelper
    {
        public const string BOOKED_CLASS_IDS = "BookedClassIds";

        public static void SetCookie<T>(HttpResponse response, string key, T value, int? expireDays = 7)
        {
            var options = new CookieOptions
            {
                Expires = expireDays.HasValue ? DateTime.Now.AddDays(expireDays.Value) : (DateTime?)null,
                IsEssential = true,
                HttpOnly = true,
                Secure = false
            };
            
            try
            {
                response.Cookies.Append(key, JsonSerializer.Serialize(value), options);
            }
            catch
            {
                // Handle serialization errors gracefully
            }
        }

        public static T GetCookie<T>(HttpRequest request, string key)
        {
            try
            {
                var cookie = request.Cookies[key];
                if (string.IsNullOrWhiteSpace(cookie))
                {
                    return default(T)!;
                }

                var result = JsonSerializer.Deserialize<T>(cookie);
                return result == null ? default(T)! : result;
            }
            catch
            {
                return default(T)!;
            }
        }

        public static void RemoveCookie(HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }

        // Helper methods for booked classes
        public static List<int> GetBookedClassIds(HttpRequest request)
        {
            return GetCookie<List<int>>(request, BOOKED_CLASS_IDS) ?? new List<int>();
        }

        public static void SetBookedClassIds(HttpResponse response, List<int> bookedIds)
        {
            SetCookie(response, BOOKED_CLASS_IDS, bookedIds, 7);
        }
    }
}
