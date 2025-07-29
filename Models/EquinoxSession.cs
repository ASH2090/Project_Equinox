using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Project_Equinox.Models
{
    public static class EquinoxSession
    {
        // Constants for session keys - moved from controller for cleaner code
        private const string FILTER_CLUB_ID = "FilterClubId";
        private const string FILTER_CATEGORY_ID = "FilterCategoryId";

        /// <summary>
        /// Saves the selected club filter value
        /// </summary>
        public static void SaveSelectedClub(this ISession session, int clubId)
        {
            session.SetInt32(FILTER_CLUB_ID, clubId);
        }

        /// <summary>
        /// Saves the selected class category filter value
        /// </summary>
        public static void SaveSelectedClassCategory(this ISession session, int categoryId)
        {
            session.SetInt32(FILTER_CATEGORY_ID, categoryId);
        }

        /// <summary>
        /// Saves both club and category filter values from a view model
        /// </summary>
        public static void SaveFilterValues(this ISession session, ClassFilterViewModel vm)
        {
            session.SaveSelectedClub(vm.ClubId);
            session.SaveSelectedClassCategory(vm.CategoryId);
        }

        /// <summary>
        /// Gets the selected club filter value
        /// </summary>
        public static int GetSelectedClub(this ISession session)
        {
            return session.GetInt32(FILTER_CLUB_ID) ?? 0;
        }

        /// <summary>
        /// Gets the selected class category filter value
        /// </summary>
        public static int GetSelectedClassCategory(this ISession session)
        {
            return session.GetInt32(FILTER_CATEGORY_ID) ?? 0;
        }

        /// <summary>
        /// Gets all filter values as a view model
        /// </summary>
        public static ClassFilterViewModel GetFilterValues(this ISession session)
        {
            return new ClassFilterViewModel 
            { 
                ClubId = session.GetSelectedClub(),
                CategoryId = session.GetSelectedClassCategory()
            };
        }

        /// <summary>
        /// Clears all filter values from the session
        /// </summary>
        public static void ClearAllFilters(this ISession session)
        {
            session.Remove(FILTER_CLUB_ID);
            session.Remove(FILTER_CATEGORY_ID);
        }

        /// <summary>
        /// Checks if any filters are currently set
        /// </summary>
        public static bool HasActiveFilters(this ISession session)
        {
            return session.GetSelectedClub() != 0 || session.GetSelectedClassCategory() != 0;
        }

        // Generic helper methods for other session operations if needed
        private static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        private static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T)! : JsonSerializer.Deserialize<T>(value)!;
        }
    }
}
