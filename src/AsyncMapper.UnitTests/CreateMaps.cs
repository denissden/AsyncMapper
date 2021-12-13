using System;
using Xunit;
using Shouldly;

namespace AsyncMapper.UnitTests
{
    public class CreateMaps : TestBase
    {

        public class C1 { }
        public class C2 { }
        public class C3 { }

        protected override AsyncMapperConfiguration Configuration => new AsyncMapperConfiguration(c => { 
            c.CreateAsyncMap<C1, C2>(); 
            c.CreateAsyncMap<C2, C1>(); 
            c.CreateAsyncMap<C2, C3>(); 
            c.CreateAsyncMap<C3, C1>(); 
        });


        [Fact]
        public void Should_be_4_maps()
        {
            GetMaps().Count.ShouldBe(4);
        }

        [Fact]
        public void Should_throw_ArgumentException()
        {
            Should.Throw<ArgumentException>(() =>
                new AsyncMapperConfiguration(c =>
                {
                    c.CreateAsyncMap<C1, C2>();
                    c.CreateAsyncMap<C1, C2>();
                }));
        }
    }
}
