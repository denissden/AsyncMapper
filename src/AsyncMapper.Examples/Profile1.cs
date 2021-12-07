using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncMapper;

namespace AsyncMapper.Examples
{
    public class Profile1 : AsyncProfile
    {
        public Profile1()
        {
            CreateAsyncMap<From1, To1>()
                .AddAsyncResolver<Resolver1Async, string>(to => to.StringValue)
                .EndAsyncConfig()
                .ForMember(to => to.StringValue, opt => opt.MapFrom<Resolver1>());
        }
    }
}
