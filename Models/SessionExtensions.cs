using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Project_Equinox.Models;


namespace Project_Equinox.Models
{
 
    public static class SessionExtensions
    {
        // Session keys
        public const string FILTER_CLUB_ID = "FilterClubId";
        public const string FILTER_CATEGORY_ID = "FilterCategoryId";

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T)! : JsonSerializer.Deserialize<T>(value)!;
        }

        // Helper methods for filter values using ViewModel
        public static void SetFilterValues(this ISession session, ClassFilterViewModel vm)
        {
            session.SetInt32(FILTER_CLUB_ID, vm.ClubId);
            session.SetInt32(FILTER_CATEGORY_ID, vm.CategoryId);
        }

        public static ClassFilterViewModel GetFilterValues(this ISession session)
        {
            var clubId = session.GetInt32(FILTER_CLUB_ID) ?? 0;
            var categoryId = session.GetInt32(FILTER_CATEGORY_ID) ?? 0;
            return new ClassFilterViewModel { ClubId = clubId, CategoryId = categoryId };
        }

        public static void ClearFilterValues(this ISession session)
        {
            session.Remove(FILTER_CLUB_ID);
            session.Remove(FILTER_CATEGORY_ID);
        }
    }
}
