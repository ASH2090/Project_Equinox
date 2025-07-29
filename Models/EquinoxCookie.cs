using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Project_Equinox.Models
{
    public static class EquinoxCookie
    {
        // Constants for cookie keys - moved from controller for cleaner code
        private const string BOOKED_CLASS_IDS = "BookedClassIds";
        private const int BOOKING_EXPIRY_DAYS = 7;

        /// <summary>
        /// Saves a booking for a gym class by adding the class ID to the booked classes list
        /// </summary>
        public static void SaveBooking(HttpResponse response, int classId)
        {
            var bookedIds = GetBookedClasses(GetCurrentRequest(response));
            
            if (!bookedIds.Contains(classId))
            {
                bookedIds.Add(classId);
                SetBookedClasses(response, bookedIds);
            }
        }

        /// <summary>
        /// Removes a booking for a gym class by removing the class ID from the booked classes list
        /// </summary>
        public static void RemoveBooking(HttpResponse response, int classId)
        {
            var bookedIds = GetBookedClasses(GetCurrentRequest(response));
            
            if (bookedIds.Contains(classId))
            {
                bookedIds.Remove(classId);
                SetBookedClasses(response, bookedIds);
            }
        }

        /// <summary>
        /// Gets all booked class IDs for the current user
        /// </summary>
        public static List<int> GetBookedClasses(HttpRequest request)
        {
            return GetCookie<List<int>>(request, BOOKED_CLASS_IDS) ?? new List<int>();
        }

        /// <summary>
        /// Checks if a specific class is already booked
        /// </summary>
        public static bool IsClassBooked(HttpRequest request, int classId)
        {
            var bookedIds = GetBookedClasses(request);
            return bookedIds.Contains(classId);
        }

        /// <summary>
        /// Gets the total count of booked classes
        /// </summary>
        public static int GetBookedClassCount(HttpRequest request)
        {
            var bookedIds = GetBookedClasses(request);
            return bookedIds?.Count ?? 0;
        }

        /// <summary>
        /// Clears all bookings for the current user
        /// </summary>
        public static void ClearAllBookings(HttpResponse response)
        {
            RemoveCookie(response, BOOKED_CLASS_IDS);
        }

        // Private helper methods
        private static void SetBookedClasses(HttpResponse response, List<int> bookedIds)
        {
            SetCookie(response, BOOKED_CLASS_IDS, bookedIds, BOOKING_EXPIRY_DAYS);
        }

        private static void SetCookie<T>(HttpResponse response, string key, T value, int expireDays)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(expireDays),
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

        private static T GetCookie<T>(HttpRequest request, string key)
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

        private static void RemoveCookie(HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }

        // Helper to get HttpRequest from HttpResponse context (for consistency in API)
        private static HttpRequest GetCurrentRequest(HttpResponse response)
        {
            return response.HttpContext.Request;
        }
    }
}
