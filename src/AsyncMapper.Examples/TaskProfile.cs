using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Examples
{
    public class TaskProfile : AsyncProfile
    {
        public TaskProfile()
        {
            CreateAsyncMap<PartialTask, Task>()
                .ForMember(to => to.description, o => o.AddResolver<PartialTaskToTaskResolver>())
                .EndAsyncConfig();
        }
    }
}
