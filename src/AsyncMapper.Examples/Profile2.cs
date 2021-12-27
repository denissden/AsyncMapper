using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Examples
{
    public class Profile2 : AsyncProfile
    {
        public Profile2()
        {
            /*CreateAsyncMap<From2, To2>()
                //.ForMember(to => to.StringValue, o => o.AddResolver<Resolver1Async>())
                .ForMember(to => to.IntValue2, o => o.AddMemberResolver<Resolver2Async, int>(from => from.IntValue2))
                .EndAsyncConfig()
                .IncludeBase<From1, To1>();*/

            /*CreateMap<From2, To2>()
                .ForMember(to => to.StringValue, o => o.MapFrom<Resolver1>())
                .ForMember(to => to.IntValue2, o => o.MapFrom<Resolver2>());*/
            //.IncludeBase<From1, To1>();
        }
    }
}
