using System;
using System.Reflection;

namespace AsyncMapper
{
    public class AsyncResolverConfig
    {
        public Delegate SourceMemberGetter { get; set; }
        public MemberInfo DestinationMemberInfo { get; set; }
        public MemberInfo SourceMemberInfo { get; set; }
        public Type ResolverType { get; set; }
        public Type MemberType { get; set; }
        public Type MemberTaskType { get; set; }
    }
}