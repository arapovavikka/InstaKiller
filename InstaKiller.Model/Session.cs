using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaKiller.Model
{
    public class Session
    {
        public Guid Id { get; set; }
        public long UserIp { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public long Token { get; set; }
    }
}
