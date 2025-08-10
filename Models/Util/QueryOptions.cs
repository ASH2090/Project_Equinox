using System.Linq.Expressions;

namespace Project_Equinox.Models.Util
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, bool>>? Where { get; set; }
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? ThenOrderBy { get; set; }
        public bool OrderByDirection { get; set; } = true; // true for ascending, false for descending
        public string Includes { get; set; } = "";
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
        public bool HasThenOrderBy => ThenOrderBy != null;
        public bool HasPaging => PageNumber > 0 && PageSize > 0;

        public QueryOptions()
        {
            PageNumber = 1;
            PageSize = 999999; // Default to show all if not set
        }

        public QueryOptions<T> Include(string navigationProperty)
        {
            if (string.IsNullOrEmpty(Includes))
                Includes = navigationProperty;
            else
                Includes += $",{navigationProperty}";
            return this;
        }

        public QueryOptions<T> Sort(Expression<Func<T, object>> orderBy, bool ascending = true)
        {
            OrderBy = orderBy;
            OrderByDirection = ascending;
            return this;
        }

        public QueryOptions<T> Filter(Expression<Func<T, bool>> where)
        {
            Where = where;
            return this;
        }

        public QueryOptions<T> Page(int pageNumber, int pageSize = 10)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            return this;
        }
    }
}
