using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Examples
{
    public class SomeService : ISomeService
    {
        private readonly Func<int, int> _modifier;

        public SomeService(Func<int, int> modifier)
        {
            _modifier = modifier;
        }

        public int Modify(int value)
        {
            return _modifier(value);
        }
    }
}
