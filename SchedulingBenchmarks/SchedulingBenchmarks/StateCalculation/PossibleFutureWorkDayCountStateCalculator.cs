using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class PossibleFutureWorkDayCountStateCalculator : StateCalculatorBase<int>
    {
        public override int CalculateState(Person person, StateTriggers triggers, int day)
        {
            if (triggers.WorkedOnPreviousDay)
            {
                return person.State.PossibleFutureWorkDayCount - 1;
            }

            return CalculatePossibleFutureWorkDayCount(person, day);
        }

        public override int InitializeState(Person person)
        {
            return CalculatePossibleFutureWorkDayCount(person, 0);
        }

        private int CalculatePossibleFutureWorkDayCount(Person person, int day)
        {
            var possibleFutureWorkDayCount = 0;
            var length = day + person.WorkSchedule.MaxConsecutiveWorkDays;

            for (int i = day; i < length; i++)
            {
                if (i < person.Availabilities.Length)
                {
                    if (person.Availabilities[i])
                    {
                        possibleFutureWorkDayCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    possibleFutureWorkDayCount++;
                }
            }

            return possibleFutureWorkDayCount;
        }
    }
}
