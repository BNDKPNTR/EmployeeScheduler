using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class AssignmentsCollection : ICloneable
    {
        private Dictionary<int, Assignment> _allRounds;
        private Dictionary<int, Assignment> _latestRound;
        private Dictionary<int, Assignment> _previousAssignments;

        public IReadOnlyDictionary<int, Assignment> AllRounds => _allRounds;
        public IReadOnlyDictionary<int, Assignment> LatestRound => _latestRound;
        public IReadOnlyDictionary<int, Assignment> PreviousAssignments => _previousAssignments;

        public AssignmentsCollection()
        {
            _allRounds = new Dictionary<int, Assignment>();
            _previousAssignments = new Dictionary<int, Assignment>();
            _latestRound = new Dictionary<int, Assignment>();
        }

        public Assignment Add(int day, Assignment assignment)
        {
            // Let the dictionaries throw and exception if an assigment already exists for the given day
            _allRounds.Add(day, assignment);
            _latestRound.Add(day, assignment);

            return assignment;
        }

        public void Remove(int day)
        {
            _allRounds.Remove(day);
            _latestRound.Remove(day);
            _previousAssignments.Remove(day);
        }

        public void StartNewRound()
        {
            foreach (var item in _latestRound)
            {
                _previousAssignments.Add(item.Key, item.Value);
            }

            _latestRound = new Dictionary<int, Assignment>();
        }

        public AssignmentsCollection Clone()
        {
            return new AssignmentsCollection
            {
                _allRounds = new Dictionary<int, Assignment>(_allRounds),
                _latestRound = new Dictionary<int, Assignment>(_latestRound),
                _previousAssignments = new Dictionary<int, Assignment>(_previousAssignments)
            };
        }

        object ICloneable.Clone() => Clone();

        public void Restore(AssignmentsCollection collection)
        {
            _allRounds = collection._allRounds;
            _latestRound = collection._latestRound;
            _previousAssignments = collection._previousAssignments;
        }
    }
}
