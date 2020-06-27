using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace QuickMapper.Core
{
    /// <summary>
    /// Factory for creating mapping delegates
    /// </summary>
    public static class DelegateFactory
    {
        /// <summary>
        /// Creates mapping delegate for a given source and destination type
        /// </summary>
        /// <typeparam name="TSource">Source Type</typeparam>
        /// <typeparam name="TTarget">Destination Type</typeparam>
        /// <param name="propertyAccessors">Matching property accessors</param>
        /// <returns>MappingDelegate</returns>
        public static MappingDelegate<TSource, TTarget> Create<TSource, TTarget>(Dictionary<MemberInfo, MemberInfo> propertyAccessors)
        {
            var args = new[]
            {
                typeof(TSource),
                typeof(TTarget)
            };

            var method = new DynamicMethod($"Map{nameof(TSource)}To{nameof(TTarget)}"
                , typeof(TTarget), args
                , Assembly.GetExecutingAssembly().Modules.First());

            var gen = method.GetILGenerator();

            foreach (var pair in propertyAccessors)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);

                switch (pair.Value)
                {
                    case MethodInfo sourceMi:
                        gen.Emit(OpCodes.Callvirt, sourceMi);
                        break;
                    default:
                    {
                        if (pair.Key is FieldInfo sourceFi)
                            gen.Emit(OpCodes.Ldfld, sourceFi);
                        break;
                    }
                }

                switch (pair.Key)
                {
                    case MethodInfo targetMi:
                        gen.Emit(OpCodes.Callvirt, targetMi);
                        break;
                    case FieldInfo targetFi:
                        gen.Emit(OpCodes.Stfld, targetFi);
                        break;
                }
            }

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ret);

            var del = (MappingDelegate<TSource, TTarget>)method.CreateDelegate(typeof(MappingDelegate<TSource, TTarget>));

            return del;
        }
    }
}
