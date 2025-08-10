using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Project_Equinox.Models.Infrastructure
{
    public static class EquinoxCookie
    {
        private const string BOOKED_CLASS_IDS = "BookedClassIds";
        private const string BOOKING_SESSION_ID = "BookingSessionId";
        private const int BOOKING_EXPIRY_DAYS = 7;

        // Return existing session id or null
        public static string? GetBookingSessionId(HttpRequest request)
        {
            return request.Cookies.TryGetValue(BOOKING_SESSION_ID, out var val) && !string.IsNullOrWhiteSpace(val)
                ? val
                : null;
        }

        // Create one if missing and return it (Ch01–13 minimal cookie options)
        public static string EnsureBookingSessionId(HttpRequest request, HttpResponse response)
        {
            var id = GetBookingSessionId(request);
            if (!string.IsNullOrEmpty(id)) return id!;
            id = Guid.NewGuid().ToString("N");
            response.Cookies.Append(BOOKING_SESSION_ID, id);
            return id;
        }

        // --- Booked class id helpers (UI cookie) ---

        public static void SaveBooking(HttpResponse response, int classId)
        {
            var list = GetBookedClasses(response.HttpContext.Request);
            if (!list.Contains(classId))
            {
                list.Add(classId);
                SetCookie(response, BOOKED_CLASS_IDS, list, BOOKING_EXPIRY_DAYS);
            }
        }

        public static void RemoveBooking(HttpResponse response, int classId)
        {
            var list = GetBookedClasses(response.HttpContext.Request);
            if (list.Remove(classId))
            {
                SetCookie(response, BOOKED_CLASS_IDS, list, BOOKING_EXPIRY_DAYS);
            }
        }

        public static List<int> GetBookedClasses(HttpRequest request)
        {
            return GetCookie<List<int>>(request, BOOKED_CLASS_IDS) ?? new List<int>();
        }

        public static bool IsClassBooked(HttpRequest request, int classId)
        {
            return GetBookedClasses(request).Contains(classId);
        }

        public static int GetBookedClassCount(HttpRequest request)
        {
            return GetBookedClasses(request).Count;
        }

        public static void ClearAllBookings(HttpResponse response)
        {
            response.Cookies.Delete(BOOKED_CLASS_IDS);
        }

        // --- cookie serialization helpers ---

        private static void SetCookie<T>(HttpResponse response, string key, T value, int expireDays)
        {
            response.Cookies.Append(key, JsonSerializer.Serialize(value),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(expireDays) });
        }

        private static T? GetCookie<T>(HttpRequest request, string key)
        {
            if (!request.Cookies.TryGetValue(key, out var s) || string.IsNullOrWhiteSpace(s)) return default;
            try { return JsonSerializer.Deserialize<T>(s); } catch { return default; }
        }
    }
}
