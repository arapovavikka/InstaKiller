﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaKiller.Model
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PhotoId { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public List<string> AllHashtags { get; set; }
    }
}
