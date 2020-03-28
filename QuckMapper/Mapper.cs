using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuckMapper.Core
{
    /// <summary>
    /// Mapper
    /// </summary>
    /// <typeparam name="TSource">Source</typeparam>
    /// <typeparam name="TTarget">Target</typeparam>
    public static class Mapper<TSource, TTarget>
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
            if (_delegate != null)
                return _delegate(source, dest);
            
            var getters = typeof(TSource)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new
                {
                    Getter = p.GetGetMethod(),
                    Property = p
                });

            var setters = typeof(TTarget)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new
                {
                    Setter = p.GetSetMethod(),
                    Property = p
                });

            var matches = new Dictionary<MethodInfo, MethodInfo>();

            foreach (var setter in setters)
            {
                var matchingGetter = getters.FirstOrDefault(g => g.Property.Name == setter.Property.Name && g.Property.GetType() == setter.Property.GetType());

                if (matchingGetter != null)
                    matches[setter.Setter] = matchingGetter.Getter;
            }
           
            _delegate = DelegateFactory.Create<TSource, TTarget>(matches);
            var val = _delegate(source, dest);

            return val;
        }
    }
}
