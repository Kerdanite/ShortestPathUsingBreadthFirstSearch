using System.Collections.Generic;

namespace Interfaces
{
    public interface IUserConnectionLister
    {
        IEnumerable<IUser> GetConnectionList();
    }
}