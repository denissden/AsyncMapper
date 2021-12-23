using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace AsyncMapper.UnitTests
{
    public class ResolverMapTime : TestConfigurationBase
    {
        IAsyncMapper _mapper;

        public class A
        {
            public string LowerString1;
            public string LowerString2;
            public string LowerString3;
            public string LowerString4;
            public int SmallerInt1;
            public int SmallerInt2;
            public int SmallerInt3;
            public int SmallerInt4;
            public List<double> DoubleList;
        }

        public class B
        {
            public string UpperString1;
            public string UpperString2;
            public string UpperString3;
            public string UpperString4;
            public int BiggerInt1;
            public int BiggerInt2;
            public int BiggerInt3;
            public int BiggerInt4;
            public List<int> IntList;
        }

        static Random rnd = new Random();

        public class StringResolver : IAsyncMemberValueResolver<A, B, string, string>
        {
            public async Task<string> Resolve(A source, B destination, string sourceMember)
            {
                await Task.Delay(rnd.Next(100, 1000));
                return await Task.FromResult(sourceMember.ToUpper());
            }
        }

        public class IntResolver : IAsyncMemberValueResolver<A, B, int, int>
        {
            public async Task<int> Resolve(A source, B destination, int sourceMember)
            {
                await Task.Delay(rnd.Next(100, 1000));
                return await Task.FromResult(sourceMember * 2);
            }
        }

        public class DoubleToIntResolver : IAsyncValueResolver<A, B, List<int>>
        {
            public async Task<List<int>> Resolve(A source, B destination)
            {
                await Task.Delay(rnd.Next(100, 1000));
                return await Task.FromResult(source.DoubleList.Select(f => (int)f).ToList());
            }
        }

        protected override AsyncMapperConfiguration Configuration => new AsyncMapperConfiguration(cfg =>
        {
            cfg.CreateAsyncMap<A, B>()
                .ForMember(to => to.UpperString1, opt => opt.AddMemberResolver<StringResolver, string>(from => from.LowerString1))
                .ForMember(to => to.UpperString2, opt => opt.AddMemberResolver<StringResolver, string>(from => from.LowerString2))
                .ForMember(to => to.UpperString3, opt => opt.AddMemberResolver<StringResolver, string>(from => from.LowerString3))
                .ForMember(to => to.UpperString4, opt => opt.AddMemberResolver<StringResolver, string>(from => from.LowerString4))
                .ForMember(to => to.BiggerInt1, opt => opt.AddMemberResolver<IntResolver, int>(from => from.SmallerInt1))
                .ForMember(to => to.BiggerInt2, opt => opt.AddMemberResolver<IntResolver, int>(from => from.SmallerInt2))
                .ForMember(to => to.BiggerInt3, opt => opt.AddMemberResolver<IntResolver, int>(from => from.SmallerInt3))
                .ForMember(to => to.BiggerInt4, opt => opt.AddMemberResolver<IntResolver, int>(from => from.SmallerInt4))
                .ForMember(to => to.IntList, opt => opt.AddResolver<DoubleToIntResolver>());
        });

        protected override void Initialize()
        {
            _mapper = Configuration.CreateAsyncMapper();
        }

        public static string RandomString(int length)
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnm1234567890-=";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        A RandomA() =>
            new A()
            {
                LowerString1 = RandomString(rnd.Next(10)),
                LowerString2 = RandomString(rnd.Next(10)),
                LowerString3 = RandomString(rnd.Next(10)),
                LowerString4 = RandomString(rnd.Next(10)),
                SmallerInt1 = rnd.Next(1_000_000),
                SmallerInt2 = rnd.Next(1_000_000),
                SmallerInt3 = rnd.Next(1_000_000),
                SmallerInt4 = rnd.Next(1_000_000),
                DoubleList = new() { 0.34, 1.4352, 4.5, 6.262 }
            };

        // total resolver wait time is around 4 seconds
        // when run in parallel mapping should take ~1 second 
        [Theory]
        //repeat 5 times
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task Should_run_all_resolvers_in_parallel(int _)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var result = await _mapper.Map<B>(RandomA());
            timer.Stop();

            timer.Elapsed.TotalMilliseconds.ShouldBeLessThan(2000);
        }
    }
}
