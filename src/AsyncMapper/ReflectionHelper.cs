using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<Type> GetConstructorArguments(Type type)
        {
            var constructors = type.GetConstructors().Where(c => c.GetParameters().Length != 0);
            // has no parameters
            if (constructors.Count() == 0) return new List<Type>();
            // has more than one constructor with parameters
            else if (constructors.Count() != 1) throw new ArgumentException($"{type.Name} Has more than one constructor with parameters");
            // has exactly one constructor with parameters
            ParameterInfo[] pList = constructors.First().GetParameters();
            return pList.Select(p => p.ParameterType).ToList();
        }
    }
}