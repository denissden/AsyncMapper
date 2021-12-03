using AutoMapper;
using System.ComponentModel;
using System.Text.Json;
using System;
using AsyncMapper;

namespace AsyncMapper.Examples {
    public partial class Program
    {
        public static void Main()
        {
            var conf = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<From1, To1>()
                    .ForMember(to => to.StringValue, opt => opt.MapFrom<Resolver1>());
                cfg.CreateMap<From2, To2>()
                    .IncludeBase<From1, To1>()
                    .ForMember(to => to.IntValue2, opt => opt.MapFrom<Resolver2>());
            });
            //conf.AssertConfigurationIsValid();

            var asyncConf = new AsyncMapperConfiguration(cfg =>
            {
                cfg.CreateAsyncMap<From1, To1>()
                    .AddAsyncResolver<string, Resolver1Async>(to => to.StringValue);
            });

            var mapper = conf.CreateMapper();

            var f1 = new From1(12, "fddfwf");

            Console.WriteLine(f1);

            var t1 = mapper.Map<To1>(f1);

            Console.WriteLine(t1);

            var f2 = new From2(10, 1, "asdf");

            Console.WriteLine(f2);

            var t2 = mapper.Map<To2>(f2);

            Console.WriteLine(t2);
        }
    }
}