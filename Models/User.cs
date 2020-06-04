using System;
using System.Collections.Generic;

namespace TestTask.Models
{
    public partial class User
    {
        public uint Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int Amount { get; set; }
    }
}
