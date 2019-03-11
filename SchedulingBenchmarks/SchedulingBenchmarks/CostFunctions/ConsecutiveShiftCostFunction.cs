using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class ConsecutiveShiftCostFunction : CostFunctionBase
    {
        private readonly double _underMinConsecutiveShiftCount;

        public ConsecutiveShiftCostFunction()
        {
            _underMinConsecutiveShiftCount = DefaultCost / 9.0;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (!CanWorkMinConsecutiveShiftsInFuture(person, day)) return MaxCost;

            if (person.State.ConsecutiveShiftCount + 1 > person.WorkSchedule.MaxConsecutiveShifts) return MaxCost;
            if (person.State.ConsecutiveShiftCount < person.WorkSchedule.MinConsecutiveShifts) return _underMinConsecutiveShiftCount;

            return DefaultCost;
        }

        private bool CanWorkMinConsecutiveShiftsInFuture(Person person, int day)
        {
            if (person.State.ConsecutiveShiftCount == 0)
            {
                var length = Math.Min(day + person.WorkSchedule.MinConsecutiveShifts, person.Availabilities.Length);
                for (int i = day + 1; i < length; i++)
                {
                    if (!person.Availabilities[i])
                    {
                        return false;
                    }

                    if (IsWeekend(i) && person.State.WorkedWeekendCount >= person.WorkSchedule.MaxWorkingWeekendCount)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsWeekend(int day)
        {
            var dayOfWeek = day % 7;

            return dayOfWeek == 5 || dayOfWeek == 6;
        }
    }
}
