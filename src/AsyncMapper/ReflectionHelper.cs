using System;
using System.Reflection;

namespace AsyncMapper
{
    public class ReflectionHelper
    {

        public static object GetMemberValue(MemberInfo memberInfo, object target)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(target);
                    break;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(target);
                    break;
                default:
                    throw new ArgumentException("MemberInfo must be FieldInfo or PropertyInfo");
            }
        }

        public static void SetMemberValue(MemberInfo memberInfo, object target, object value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)memberInfo).SetValue(target, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)memberInfo).SetValue(target, value);
                    break;
                default:
                    throw new ArgumentException("MemberInfo must be FieldInfo or PropertyInfo");
            }
        }
    }
}