using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace AsyncMapper
{
    public interface IAsyncMapperConfigurationExpression : IMapperConfigurationExpression
    {
        public IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>();
    }
}