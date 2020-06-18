using Bogus;
using CacheStrategy.Models;
using System.Collections.Generic;
using System.Linq;

namespace CacheStrategy.Stores
{
    public class CarStore : ICarStore
    {
        public List<Car> CarStorage { get; set; }

        public CarStore()
        {
            var id = 0;
            var testOrders = new Faker<Car>()
                .RuleFor(o => o.Id, f => ++id)
                .RuleFor(o => o.Year, f => f.Random.Int(1950, 2020))
                .RuleFor(o => o.Brand, f => f.Vehicle.Manufacturer())
                .RuleFor(o => o.Model, f => f.Vehicle.Model());
            CarStorage = testOrders.Generate(10);
        }

        public IEnumerable<Car> List()
        {
            return CarStorage;
        }

        public Car Get(int userid)
        {
            return CarStorage.FirstOrDefault(f => f.Id == userid);
        }
    }
}