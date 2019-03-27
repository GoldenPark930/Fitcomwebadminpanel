using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace LinksMediaCorpBusinessLayer
{

    public static class SortExtension
    {
        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource,Tkey>(this IEnumerable<TSource> source, Func<TSource,Tkey> keySelector, bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, Tkey>(this IQueryable<TSource> source, Expression<Func<TSource,Tkey>> keySelector, bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }
    }
}
