using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class AssignmentsCollection
    {
        private readonly Dictionary<int, Assignment> _allRounds;
        private Dictionary<int, Assignment> _latestRound;

        public IReadOnlyDictionary<int, Assignment> AllRounds => _allRounds;
        public IReadOnlyDictionary<int, Assignment> LatestRound => _latestRound;

        public AssignmentsCollection()
        {
            _allRounds = new Dictionary<int, Assignment>();
        }

        public void Add(int day, Assignment assignment)
        {
            // Let the dictionaries throw and exception if an assigment already exists for the given day
            _allRounds.Add(day, assignment);
            _latestRound.Add(day, assignment);
        }

        public void StartNewRound() => _latestRound = new Dictionary<int, Assignment>();
    }
}
