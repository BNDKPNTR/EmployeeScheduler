using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    class StateCalculator
    {
        private readonly WorkedOnWeekedStateCalculator _workedOnWeekedStateCalculator;
        private readonly TotalWorkTimeStateCalculator _totalWorkTimeStateCalculator;
        private readonly ConsecutiveShiftCountStateCalculator _consecutiveShiftCountStateCalculator;
        private readonly DayOffCountStateCalculator _dayOffCountStateCalculator;
        private readonly WorkedWeekendCountStateCalculator _workedWeekendCountStateCalculator;
        private readonly ShiftWorkedCountStateCalculator _shiftWorkedCountStateCalculator;

        public StateCalculator(Range schedulePeriod, Calendar calendar)
        {
            _workedOnWeekedStateCalculator = new WorkedOnWeekedStateCalculator(calendar);
            _totalWorkTimeStateCalculator = new TotalWorkTimeStateCalculator();
            _consecutiveShiftCountStateCalculator = new ConsecutiveShiftCountStateCalculator();
            _dayOffCountStateCalculator = new DayOffCountStateCalculator();
            _workedWeekendCountStateCalculator = new WorkedWeekendCountStateCalculator(schedulePeriod, calendar);
            _shiftWorkedCountStateCalculator = new ShiftWorkedCountStateCalculator();
        }

        public void RefreshState(Person person, int day)
        {
            person.State.WorkedOnWeeked = _workedOnWeekedStateCalculator.CalculateState(person, day);
            person.State.TotalWorkTime = _totalWorkTimeStateCalculator.CalculateState(person, day);
            person.State.ConsecutiveShiftCount = _consecutiveShiftCountStateCalculator.CalculateState(person, day);
            person.State.DayOffCount = _dayOffCountStateCalculator.CalculateState(person, day);
            person.State.WorkedWeekendCount = _workedWeekendCountStateCalculator.CalculateState(person, day);
            person.State.ShiftWorkedCount = _shiftWorkedCountStateCalculator.CalculateState(person, day);
        }

        public void InitializeState(Person person)
        {
            person.State.WorkedOnWeeked = _workedOnWeekedStateCalculator.InitializeState(person);
            person.State.TotalWorkTime = _totalWorkTimeStateCalculator.InitializeState(person);
            person.State.ConsecutiveShiftCount = _consecutiveShiftCountStateCalculator.InitializeState(person);
            person.State.DayOffCount = _dayOffCountStateCalculator.InitializeState(person);
            person.State.WorkedWeekendCount = _workedWeekendCountStateCalculator.InitializeState(person);
            person.State.ShiftWorkedCount = _shiftWorkedCountStateCalculator.InitializeState(person);
        }
    }
}
