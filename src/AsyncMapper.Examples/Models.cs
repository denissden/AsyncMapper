using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Examples
{
    public class Group
    {
        public List<string> people { get; set; }
        public string leader { get; set; }
        public List<Task> tasks { get; set; }
    }

    public class PartialTask
    {
        public int id { get; set; }
        public bool finished { get; set; }
        public string responsible_login { get; set; }
    }

    public class Task : PartialTask
    {
        public string responsible_full_name { get; set; }
        public string description { get; set; }
    }

    public class Person
    {
        public string name { get; set; }
        public string login { get; set; }
        public string phone { get; set; }
    }

    public class Worker : Person
    {
        public string address { get; set; }
        public Job job { get; set; }
    }

    public class Job
    {
        public string name { get; set; }
        public DateTime employment_date { get; set; }
    }
}
