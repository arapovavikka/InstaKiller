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
        public ulong PasswordHash { get; set; }
        public string About { get; set; }
        public List<Person> Subscribers { get; set; } //TO DO
        public List<Person> Subscriptions { get; set; }
    }
}
