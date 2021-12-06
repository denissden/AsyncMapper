using System;
using System.Reflection;

namespace AsyncMapper
{
    public class AsyncResolverConfig
    {
        public Delegate MemberGetter { get; set; }
        public MemberInfo ToMemberInfo { get; set; }
        public Type ResolverType { get; set; }
    }
}