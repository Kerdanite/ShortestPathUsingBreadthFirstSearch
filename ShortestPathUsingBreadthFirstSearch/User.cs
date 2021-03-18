using System.Collections.Generic;
using Interfaces;

namespace ShortestPathUsingBreadthFirstSearch
{
    public class User : IUser
    {
        private readonly IList<IUser> _friends;
        public User(string id, string name)
        {
            Id = id;
            Name = name;
            _friends = new List<IUser>();
        }

        public string Id { get; }
        public string Name { get; }
        public IEnumerable<IUser> Friends => _friends;

        public void AddFriend(IUser friend)
        {
            _friends.Add(friend);
        }
    }
}