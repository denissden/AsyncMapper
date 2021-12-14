using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AsyncMapper.UnitTests
{
    public class MapSimpleClass : TestConfigurationBase
    {
        IAsyncMapper _mapper;
        public class A
        {
            public int a;
            public string b;
            public double c;
        }

        public class B
        {
            public string b;
            public float c;

            public bool Equals(B obj)
            {
                return b == obj.b && c.Equals(obj.c);
            }
        }

        protected override AsyncMapperConfiguration Configuration => new AsyncMapperConfiguration(cfg =>
        {
            cfg.CreateAsyncMap<A, B>();
        });

        protected override void Initialize()
        {
            _mapper = Configuration.CreateAsyncMapper();
        }

        B MapManual(A from)
        {
            return new B()
            {
                b = from.b,
                c = (float)from.c
            };
        }

        [Theory]
        [InlineData(1, "some string", 0.362)]
        [InlineData(int.MinValue, "", double.MaxValue)]
        [InlineData(0, null, 2642.73)]
        public async void Should_map_correct(int a, string b, double c)
        {
            A from = new A() { a = a, b = b, c = c };
            var expected = MapManual(from);
            var actual = await _mapper.Map<B>(from);
            expected.Equals(actual).ShouldBe(true);
        }
    }
}
