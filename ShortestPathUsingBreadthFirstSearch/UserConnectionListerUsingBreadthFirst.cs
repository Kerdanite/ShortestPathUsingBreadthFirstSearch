using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace ShortestPathUsingBreadthFirstSearch
{
    public class UserConnectionListerUsingBreadthFirst : IUserConnectionLister
    {
        private readonly IUser _startingUser;
        private readonly IUser _targetUser;
        private readonly uint _maxDepth;

        private readonly Queue<IUser> _queue = new Queue<IUser>();
        private readonly HashSet<IUser> _visited = new HashSet<IUser>();
        private IUser _current;

        private readonly IList<KeyValuePair<IUser, IUser>> _visitingLinks = new List<KeyValuePair<IUser, IUser>>();
        private int _currentDepth;
        private int _elementsToDepthIncrease;
        private int _nextElementsToDepthIncrease;


        public UserConnectionListerUsingBreadthFirst(IUser user1, IUser user2, uint maxLevel)
        {
            _startingUser = user1;
            _targetUser = user2;
            _maxDepth = maxLevel;
        }

        public IEnumerable<IUser> GetConnectionList()
        {
            try
            {
                CalculateShortestConnectionUsingBreadthFirst();
                return GetResultListFromVisitingLinks();
            }
            catch (BreadthFirstException)
            {
                return new List<IUser>();
            }
        }

        private IEnumerable<IUser> GetResultListFromVisitingLinks()
        {
            IList<IUser> result = new List<IUser>();

            var current = _targetUser;
            result.Add(current);

            while (current != _startingUser)
            {
                current = _visitingLinks.Single(w => w.Value == current).Key;
                result.Add(current);
            }

            return result.Reverse();
        }

        private void CalculateShortestConnectionUsingBreadthFirst()
        {
            InitBreadthFirstSearch();

            while (_queue.Any())
            {
                _current = _queue.Dequeue();

                if (_current == _targetUser)
                {
                    return;
                }

                var notVisitedFriends = _current.Friends.Where(IsNotVisited);

                VerifyMaxDepthIsReached(notVisitedFriends);
                
                VisitAndEnqueueNotVisitedConnectedNodes(notVisitedFriends);
            }

            throw new BreadthFirstTargetNotFoundException();
        }

        private void InitBreadthFirstSearch()
        {
            _queue.Clear();
            _queue.Enqueue(_startingUser);
            _visited.Add(_startingUser);

            _currentDepth = 0;
            _elementsToDepthIncrease = 1;
            _nextElementsToDepthIncrease = 0;
        }

        private void VisitAndEnqueueNotVisitedConnectedNodes(IEnumerable<IUser> notVisitedFriends)
        {
            foreach (var friend in notVisitedFriends)
            {
                Visit(friend);
                _queue.Enqueue(friend);
            }
        }

        private void VerifyMaxDepthIsReached(IEnumerable<IUser> notVisitedFriends)
        {
            _nextElementsToDepthIncrease += notVisitedFriends.Count();

            _elementsToDepthIncrease--;
            if (_elementsToDepthIncrease == 0)
            {
                _currentDepth++;
                if (_currentDepth >= _maxDepth)
                {
                    throw new BreadthFirstMaxDepthReachedException();
                }

                _elementsToDepthIncrease = _nextElementsToDepthIncrease;
                _nextElementsToDepthIncrease = 0;
            }
        }

        private bool IsNotVisited(IUser friend)
        {
            return !_visited.Contains(friend);
        }

        private void Visit(IUser friend)
        {
            _visited.Add(friend);
            _visitingLinks.Add(new KeyValuePair<IUser, IUser>(_current, friend));
        }
    }

    internal abstract class BreadthFirstException : Exception
    {

    }

    internal class BreadthFirstTargetNotFoundException : BreadthFirstException
    {

    }

    internal class BreadthFirstMaxDepthReachedException : BreadthFirstException
    {

    }
}