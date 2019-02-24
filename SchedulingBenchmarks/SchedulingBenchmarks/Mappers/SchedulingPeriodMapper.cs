using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    class SchedulingPeriodMapper
    {
        private readonly Dto.SchedulingPeriod _inputModel;
        private readonly Dictionary<int, Activity> _activities;

        private SchedulingPeriodMapper(Dto.SchedulingPeriod inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _activities = new Dictionary<int, Activity>();
        }

        public static SchedulerModel MapToScheduleModel(Dto.SchedulingPeriod inputModel) 
            => new SchedulingPeriodMapper(inputModel).MapToScheduleModel();

        private SchedulerModel MapToScheduleModel()
        {
            var model = new SchedulerModel();
            

            return model;
        }

        private List<Person> MapPeople(Calendar calendar, Range schedulePeriod)
        {
            var people = new List<Person>();

           

            return people;
        }

        private State CreateState()
        {
            return new State
            {
                TimeSlotsWorked = 0,
                TimeSlotsWorkedToday = 0,
                WorkedDaysInMonthCount = 0,
                DailyWorkStartCounts = ImmutableDictionary<int, int>.Empty
            };
        }
    }
}
