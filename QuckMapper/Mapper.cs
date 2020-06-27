using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace QuickMapper.Core
{
    /// <summary>
    /// Mapper
    /// </summary>
    /// <typeparam name="TSource">Source</typeparam>
    /// <typeparam name="TTarget">Target</typeparam>
    public static class Mapper<TSource, TTarget> 
        where TSource : class 
        where TTarget : class
    {
        /// <summary>
        /// Cached delegate
        /// </summary>
        private static MappingDelegate<TSource, TTarget> _delegate;

        /// <summary>
        /// Maps a object of type TSource to and object of type TTarget
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="dest">Target</param>
        /// <returns>Target</returns>
        public static TTarget MapTo(TSource source, TTarget dest)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            if (dest is null)
                throw new System.ArgumentNullException(nameof(dest));

            if (_delegate != null)
                return _delegate(source, dest);
            
            var getters = typeof(TSource)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new AccessorPropertyPair(p.GetGetMethod(), p)).ToList();

            var sourceFields = typeof(TSource)
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Select(f => new AccessorPropertyPair(default, f)).ToList();

            if (getters.Count == 0 && sourceFields.Count == 0)
                return dest;

            var setters = typeof(TTarget)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new AccessorPropertyPair(p.GetSetMethod(), p)).ToList();

            var destFields = typeof(TSource)
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Select(f => new AccessorPropertyPair(default, f)).ToList();

            if (setters.Count == 0 && destFields.Count == 0)
                return dest;

            var matches = new Dictionary<MemberInfo, MemberInfo>();

            foreach (var setter in setters.Union(sourceFields))
            {
                var matchingGetter = getters.Union(destFields)
                    .FirstOrDefault(g => g.Member.Name == setter.Member.Name 
                        && g.Member.GetType() == setter.Member.GetType());

                if (matchingGetter != default)
                    matches[setter.Accessor ?? setter.Member] = matchingGetter.Accessor ?? matchingGetter.Member;
            }

            if (matches.Count == 0)
                return dest;

            var del = DelegateFactory.Create<TSource, TTarget>(matches);

            // Write to the field and flush the cache
            Volatile.Write(ref _delegate, del);

            var val = _delegate(source, dest);

            return val;
        }
    }
}
