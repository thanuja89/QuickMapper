using System.Reflection;

namespace QuckMapper.Core
{
    /// <summary>
    /// Accessor Property Pair
    /// </summary>
    public class AccessorPropertyPair
    {
        /// <summary>
        /// Accessor of Member
        /// </summary>
        public MethodInfo Accessor { get; set; }

        /// <summary>
        /// Member
        /// </summary>
        public MemberInfo Member { get; set; }
    }
}
