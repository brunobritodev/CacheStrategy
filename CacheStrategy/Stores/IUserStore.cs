using CacheStrategy.Models;
using System.Collections.Generic;

namespace CacheStrategy.Stores
{
    public interface IUserStore
    {
        IEnumerable<User> List();
        User Get(int userid);
    }
}
