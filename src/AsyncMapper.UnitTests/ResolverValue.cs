using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AsyncMapper.UnitTests
{
    public class ResolverValue : TestConfigurationBase
    {
        IAsyncMapper _mapper;
        public class A
        {
            public int id;
            public string email;
            public string phone;
        }

        public class B
        {
            public int id;
            public string name;
            public string email;
            public string phone;
        }

        static Dictionary<int, string> NameData = new()
        {
            { 1, "Andrew" },
            { 2, "Mary" },
            { 3, "Dmitry" },
            { 4, "Deniss" }
        };

        public class IdToNameResolver : IAsyncValueResolver<A, B, string>
        {
            public async Task<string> Resolve(A source, B destination)
            {
                await Task.Delay(100);
                return await Task.FromResult(NameData[source.id]);
            }
        }

        public class PhoneFormatResolver : IAsyncMemberValueResolver<A, B, string, string>
        {
            public async Task<string> Resolve(A source, B destination, string sourceMember)
            {
                await Task.Delay(100);
                return await Task.FromResult(String.Format("{0:(###)###-##-##}", Convert.ToInt32(sourceMember)));
            }
        }

        protected override AsyncMapperConfiguration Configuration => new AsyncMapperConfiguration(cfg =>
        {
            cfg.CreateAsyncMap<A, B>()
                .ForMemberAsync(to => to.name, opt => opt.AddResolver<IdToNameResolver>())
                .ForMemberAsync(to => to.phone, opt => opt.AddMemberResolver<PhoneFormatResolver, string>(from => from.phone));
        });

        protected override void Initialize()
        {
            _mapper = Configuration.CreateAsyncMapper();
        }

        async Task<B> MapManual(A from)
        {
            return new B()
            {
                id = from.id,
                email = from.email,
                name = await new IdToNameResolver().Resolve(from, null),
                phone = await new PhoneFormatResolver().Resolve(from, null, from.phone),
            };
        }

        [Theory]
        [InlineData(1, "1234567890", "test@test")]
        [InlineData(2, "0987654321", "another@mail")]
        [InlineData(3, "1234567890", null)]
        [InlineData(4, "1234567890", "")]
        public async Task Should_map_correct(int id, string phone, string email)
        {
            A from = new A() { id = id, phone = phone, email = email };
            var expected = await MapManual(from);
            var actual = await _mapper.Map<B>(from);
            Assert.Equal(expected.id, actual.id);
            Assert.Equal(expected.name, actual.name);
            Assert.Equal(expected.phone, actual.phone);
            Assert.Equal(expected.email, actual.email);
        }
    }
}
