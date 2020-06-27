using System.Reflection;

namespace QuickMapper.Core
{
    /// <summary>
    /// Accessor Property Pair
    /// </summary>
    public readonly struct AccessorPropertyPair
    {
        /// <summary>
        /// Accessor of Member
        /// </summary>
        public readonly MethodInfo Accessor;

        /// <summary>
        /// Member
        /// </summary>
        public readonly MemberInfo Member;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accessor">Accessor of Member</param>
        /// <param name="member">Member</param>
        public AccessorPropertyPair(MethodInfo accessor, MemberInfo member)
        {
            Accessor = accessor;
            Member = member;
        }


        public bool Equals(AccessorPropertyPair other)
        {
            return Equals(Accessor, other.Accessor) && Equals(Member, other.Member);
        }

        public override bool Equals(object obj)
        {
            return obj is AccessorPropertyPair other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Accessor != null ? Accessor.GetHashCode() : 0) * 397) ^ (Member != null ? Member.GetHashCode() : 0);
            }
        }

        public static bool operator ==(AccessorPropertyPair p, AccessorPropertyPair q)
            => p.Accessor == q.Accessor && p.Member == q.Member;

        public static bool operator !=(AccessorPropertyPair p, AccessorPropertyPair q) => !(p == q);
    }
}
