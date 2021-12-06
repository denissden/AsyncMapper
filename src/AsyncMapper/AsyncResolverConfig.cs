using System;
using System.Reflection;

namespace AsyncMapper
{
    public class AsyncResolverConfig
    {
        public Delegate MemberGetter { get; set; }
        public MemberInfo ToMemberInfo { get; set; }
        public Type ResolverType { get; set; }
        public Type MemberType { get; set; }
        public Type MemberTaskType { get; set; }
    }
}