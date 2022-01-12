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

        public bool Equals(AsyncResolverConfig other)
        {
            bool source; bool destination;
            if (this.SourceMemberInfo != null && other.SourceMemberInfo != null)
                source = this.SourceMemberInfo == other.SourceMemberInfo;
            else
                source =  this.SourceMemberGetter.Equals(other.SourceMemberGetter);
            destination = this.DestinationMemberInfo == other.DestinationMemberInfo;
            return source && destination;
        }
    }
}