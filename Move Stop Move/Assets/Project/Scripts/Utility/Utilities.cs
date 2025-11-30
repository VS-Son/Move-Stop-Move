using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.Utility
{
    public static class Utilities 
    {
        public static List<T> SortOrder<T>(List<T> list, int amount)
        {
            return list.OrderBy(d => System.Guid.NewGuid()).Take(amount).ToList();
        }
    }
}