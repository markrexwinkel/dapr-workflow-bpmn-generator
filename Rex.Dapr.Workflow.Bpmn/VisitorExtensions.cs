using System;
using System.Collections.Generic;

namespace Rex.Bpmn
{
    public static class VisitorExtensions
    {
        public static void Visit<T>(this IEnumerable<T> items, Action<T> visitor)
        {
            foreach(var item in items)
            {
                visitor(item);
            }
        }

        public static void VisitIfNotNull<T>(this T item, Action<T> visitor) where T : class
        {
            if(item != null)
            {
                visitor(item);
            }
        }
    }
}
