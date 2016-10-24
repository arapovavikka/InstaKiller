using System;
using System.Collections.Generic;

namespace InstaKiller.Model
{
    public class Photo
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime TimeDate { get; set; }
        public List<Person> UsersThatLike { get; set; }

        public Photo()
        {
            UsersThatLike = new List<Person>();
        }
    }
}
