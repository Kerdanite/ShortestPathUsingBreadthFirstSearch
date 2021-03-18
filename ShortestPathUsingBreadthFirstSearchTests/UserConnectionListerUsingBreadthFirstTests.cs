using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ShortestPathUsingBreadthFirstSearch.Tests
{
    [TestFixture]
    public class UserConnectionListerUsingBreadthFirstTests
    {
        [Test]
        public void GetConnectionWith2UserWithoutFriendsReturnEmptyList()
        {
            var user1 = CreateUser();
            var user2 = CreateUser();

            var connectionLister = GetNewConnectionLister(user1, user2, 5);
            var result = connectionLister.GetConnectionList();

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetConnectionWith2UserDirectlyFriendsReturnListOf2List()
        {
            var user1 = CreateUser();
            var user2 = CreateUser();
            AddMutualFriendShipBetweenUsers(user1, user2);
            var expected = new List<IUser> {user1, user2};

            var connectionLister = GetNewConnectionLister(user1, user2, 5);
            var result = connectionLister.GetConnectionList();

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Test_Get_GetConnectionList_With_Users_Having_Connection()
        {
            var user1 = CreateUser();
            var user2 = CreateUser();
            var users = CreateManyUsers(4).ToArray();

            AddMutualFriendShipBetweenUsers(user1, users[0]);
            AddMutualFriendShipBetweenUsers(users[0], users[1]);
            AddMutualFriendShipBetweenUsers(users[1], users[2]);
            AddMutualFriendShipBetweenUsers(users[2], users[3]);
            AddMutualFriendShipBetweenUsers(users[3], user2);

            var expectedConnection = new List<IUser> { user1 };
            expectedConnection.AddRange(users);
            expectedConnection.Add(user2);

            var connectionLister = GetNewConnectionLister(user1, user2, 6);
            var result = connectionLister.GetConnectionList();

            CollectionAssert.AreEqual(expectedConnection, result);
        }

        [Test]
        public void Test_Get_GetConnectionList_With_Users_HavingMoreConnectionThanMaxDepth()
        {
            var user1 = CreateUser();
            var user2 = CreateUser();
            var users = CreateManyUsers(4).ToArray();

            AddMutualFriendShipBetweenUsers(user1, users[0]);
            AddMutualFriendShipBetweenUsers(users[0], users[1]);
            AddMutualFriendShipBetweenUsers(users[1], users[2]);
            AddMutualFriendShipBetweenUsers(users[2], users[3]);
            AddMutualFriendShipBetweenUsers(users[3], user2);

            var connectionLister = GetNewConnectionLister(user1, user2, 5);
            var result = connectionLister.GetConnectionList();

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Test_Get_GetConnectionList_Should_Return_Shortest_Connection()
        {
            var user1 = CreateUser();
            var user2 = CreateUser();
            var users = CreateManyUsers(4).ToArray();

            AddMutualFriendShipBetweenUsers(user1, users.First());
            AddMutualFriendShipBetweenUsers(users.First(), users[1]);
            AddMutualFriendShipBetweenUsers(users[1], users[2]);
            AddMutualFriendShipBetweenUsers(users[2], users[3]);
            AddMutualFriendShipBetweenUsers(users[3], user2);

            var bestFriend = CreateUser();
            var coolFriend = CreateUser();
            AddMutualFriendShipBetweenUsers(user1, bestFriend);
            AddMutualFriendShipBetweenUsers(bestFriend, coolFriend);
            AddMutualFriendShipBetweenUsers(coolFriend, user2);

            var expectedConnection = new List<IUser> { user1, bestFriend, coolFriend, user2 };

            var connectionLister = GetNewConnectionLister(user1, user2, 10);
            var result = connectionLister.GetConnectionList();

            CollectionAssert.AreEqual(expectedConnection, result);
        }

        private IEnumerable<IUser> CreateManyUsers(int userToCreate)
        {
            return Enumerable.Range(1, userToCreate).Select(s => CreateUser());
        }


        private IUserConnectionLister GetNewConnectionLister(IUser startUser, IUser endUser, uint maxDepthSearch)
        {
            return new UserConnectionListerUsingBreadthFirst(startUser, endUser, maxDepthSearch);
        }

        private IUser CreateUser()
        {
            var id = Guid.NewGuid();
            var user1 = new User(id.ToString(), $"{id}_name");
            return user1;
        }

        private void AddMutualFriendShipBetweenUsers(IUser user1, IUser user2)
        {
            user1.AddFriend(user2);
            user2.AddFriend(user1);
        }
    }
}