using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Examples
{
    public class PersonProfile : AsyncProfile
    {
        public PersonProfile()
        {
            CreateAsyncMap<Person, Worker>()
                .ForMember(to => to.job, o => o.AddMemberResolver<LoginToJobResolver, string>(from => from.login))
                .ForMember(to => to.address, o => o.AddMemberResolver<LoginToAddressResolver, string>(from => from.login))
                .EndAsyncConfig()
                .ReverseMap();
        }
    }
}
