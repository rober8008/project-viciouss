using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Config
    {
        public int Id { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
    }
}
