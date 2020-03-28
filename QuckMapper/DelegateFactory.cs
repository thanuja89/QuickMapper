using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace QuckMapper.Core
{
    /// <summary>
    /// Factory for creating mapping delegates
    /// </summary>
    public static class DelegateFactory
    {
        /// <summary>
        /// Creates mapping delegate for a given source and destination type
        /// </summary>
        /// <typeparam name="S">Source Type</typeparam>
        /// <typeparam name="T">Destination Type</typeparam>
        /// <param name="propertyAccessors">Matching property accessors</param>
        /// <returns>MappingDelegate</returns>
        public static MappingDelegate<S, T> Create<S, T>(Dictionary<MethodInfo, MethodInfo> propertyAccessors)
        {
            var args = new Type[]
            {
                typeof(S),
                typeof(T)
            };

            var method = new DynamicMethod($"Map{nameof(S)}To{nameof(T)}", typeof(T), args, Assembly.GetExecutingAssembly().Modules.First());

            var gen = method.GetILGenerator();

            foreach (var pair in propertyAccessors)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);

                gen.Emit(OpCodes.Callvirt, pair.Value);
                gen.Emit(OpCodes.Callvirt, pair.Key);
            }

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ret);

            var del = (MappingDelegate<S, T>)method.CreateDelegate(typeof(MappingDelegate<S, T>));

            return del;
        }
    }
}
