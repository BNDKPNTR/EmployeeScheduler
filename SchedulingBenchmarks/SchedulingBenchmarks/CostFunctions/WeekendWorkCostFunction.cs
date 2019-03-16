using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class WeekendWorkCostFunction : CostFunctionBase
    {
        private readonly Calendar _calendar;
        private readonly double _cantWorkOnBothDaysMultiplier;

        public WeekendWorkCostFunction(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
            _cantWorkOnBothDaysMultiplier = DefaultCost * 2;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (!_calendar.IsWeekend(day)) return DefaultCost;

            if (_calendar.IsSaturday(day))
            {
                if (person.State.PossibleFutureWorkDayCount <= 1)
                {
                    return _cantWorkOnBothDaysMultiplier;
                }
            }

            if (_calendar.IsSunday(day))
            {
                if (!person.Assignments.AllRounds.ContainsKey(day - 1))
                {
                    return _cantWorkOnBothDaysMultiplier;
                }
            }

            return DefaultCost;
        }
    }
}
