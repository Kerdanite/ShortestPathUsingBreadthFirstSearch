using System.Collections.Generic;

namespace Interfaces
{
    public interface IUser
    {
        string Id { get; }
        string Name { get; }
        IEnumerable<IUser> Friends { get; }

        void AddFriend(IUser friend);
    }
}