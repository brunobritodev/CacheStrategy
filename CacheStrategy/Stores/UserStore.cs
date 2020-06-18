using Bogus;
using CacheStrategy.Models;
using System.Collections.Generic;
using System.Linq;

namespace CacheStrategy.Stores
{
    public class UserStore : IUserStore
    {
        public List<User> UsersStorage { get; set; }

        public UserStore()
        {
            var id = 0;
            var testOrders = new Faker<User>()
                .RuleFor(o => o.Id, f => ++id)
                .RuleFor(o => o.Age, f => f.Random.Int(18, 30))
                .RuleFor(o => o.Name, f => f.Person.FullName)
                .RuleFor(o => o.Cellphone, f => f.Person.Phone);
            UsersStorage = testOrders.Generate(10);
        }

        public IEnumerable<User> List()
        {
            return UsersStorage;
        }

        public User Get(int userid)
        {
            return UsersStorage.FirstOrDefault(f => f.Id == userid);
        }
    }
}
