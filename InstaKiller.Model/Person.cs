using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaKiller.Model
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public Int32 PasswordHash { get; set; }
        public string About { get; set; }
        public List<Person> Subscribers { get; set; } 
        public List<Person> Subscriptions { get; set; }

        public Person()
        {
            Subscribers = new List<Person>();
            Subscriptions = new List<Person>();
        }
    }
}
