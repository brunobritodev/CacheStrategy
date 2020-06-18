using CacheStrategy.Models;
using System.Collections.Generic;

namespace CacheStrategy.Stores
{
    public interface ICarStore
    {
        IEnumerable<Car> List();
        Car Get(int id);
    }
}