using System;
using Xunit;
using Shouldly;

namespace AsyncMapper.UnitTests
{
    public class AddProfiles : TestBase
    {

        public class C1 { }
        public class C2 { }
        public class C3 { }

        public class FirstProfile : AsyncProfile
        {
            public FirstProfile() => CreateAsyncMap<C1, C2>();
        }

        public class FirstReverseProfile : AsyncProfile
        {
            public FirstReverseProfile() => CreateAsyncMap<C2, C1>();
        }

        public class SecondProfile : AsyncProfile
        {
            public SecondProfile() 
            { 
                CreateAsyncMap<C2, C3>(); 
                CreateAsyncMap<C3, C1>(); 
            }
        }

        // this profile has a repeated map <C2, C3>
        public class ThirdProfile : AsyncProfile
        {
            public ThirdProfile()
            {
                CreateAsyncMap<C2, C3>();
            }
        }

        protected override AsyncMapperConfiguration Configuration => new AsyncMapperConfiguration(c => { 
            c.AddAsyncProfile<FirstProfile>(); 
            c.AddAsyncProfile<FirstReverseProfile>(); 
            c.AddAsyncProfile<SecondProfile>(); 
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
                    c.AddAsyncProfile<FirstProfile>();
                    c.AddAsyncProfile<FirstReverseProfile>();
                    c.AddAsyncProfile<SecondProfile>();
                    c.AddAsyncProfile<ThirdProfile>();
                }));
        }
    }
}
