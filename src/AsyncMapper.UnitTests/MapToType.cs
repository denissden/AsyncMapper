using AsyncMapper.Exceptions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AsyncMapper.UnitTests
{
    public class MapToType : TestConfigurationBase
    {
        IAsyncMapper _mapper;
        public class A
        {
        }

        public class B
        {
        }

        public class C : B
        {
        }

        protected override AsyncMapperConfiguration Configuration => new AsyncMapperConfiguration(cfg =>
        {
            cfg.CreateAsyncMap<A, B>();  
            cfg.CreateAsyncMap<A, C>();  
            cfg.CreateAsyncMap<B, C>();
            cfg.CreateAsyncMap<C, A>();  
        });

        protected override void Initialize()
        {
            _mapper = Configuration.CreateAsyncMapper();
        }

        [Fact]
        public async void A_should_return_C()
        {
            var _actual = await _mapper.Map<C>(new A());
            _actual.ShouldBeOfType<C>();
        }

        [Fact]
        public async void B_should_return_C()
        {
            var _actual = await _mapper.Map<C>(new B());
            _actual.ShouldBeOfType<C>();
        }

        [Fact]
        public void Should_be_task_when_not_awaited()
        {
            var _actual = _mapper.Map<C>(new B());
            _actual.ShouldBeOfType<Task<C>>();
        }

        //[Fact]
        public void Should_throw_mapping_exception_when_not_exists()
        {
            Should.Throw<MappingException>(async () =>
            {
                var _actual = await _mapper.Map<B>(new C());
                _actual.ShouldBeOfType<C>();
            });
        }
    }
}
