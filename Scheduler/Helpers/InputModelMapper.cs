using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Scheduler
{
    class InputModelMapper
    {
        private readonly Dto.InputModel _inputModel;
        private readonly Dictionary<int, Activity> _activities;

        private InputModelMapper(Dto.InputModel inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _activities = new Dictionary<int, Activity>();
        }

        public static SchedulerModel MapToScheduleModel(Dto.InputModel inputModel) => new InputModelMapper(inputModel).MapToScheduleModel();

        private SchedulerModel MapToScheduleModel()
        {
            var model = new SchedulerModel();
            model.Calendar = new Calendar(_inputModel.SchedulePeriodStart, _inputModel.TimeSlotLengthInMinutes);
            model.SchedulePeriod = Range.Of(
                start: model.Calendar.DateTimeToTimeSlotIndex(_inputModel.SchedulePeriodStart),
                end: model.Calendar.DateTimeToTimeSlotIndex(_inputModel.SchedulePeriodEnd) - 1);

            model.People = MapPeople(model.Calendar, model.SchedulePeriod);
            model.AllDemands = MapDemands(model.Calendar, model.SchedulePeriod);

            return model;
        }

        private List<Person> MapPeople(Calendar calendar, Range schedulePeriod)
        {
            var people = new List<Person>();

            foreach (var personDto in _inputModel.People)
            {
                var person = new Person(
                    personDto.Id,
                    personDto.Name,
                    CreateState(),
                    MapAvailabilities(personDto.Availabilities, calendar, schedulePeriod),
                    new Dictionary<int, Assignment>());

                people.Add(person);
            }

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

        private TimeSlotDependentCollection<bool> MapAvailabilities(List<Dto.Availability> availabilities, Calendar calendar, Range schedulePeriod)
        {
            var availableRanges = availabilities
                .Select(a => Range.Of(start: calendar.DateTimeToTimeSlotIndex(a.Start), end: calendar.DateTimeToTimeSlotIndex(a.End) - 1))
                .Normalize()
                .IntersectWith(schedulePeriod)
                .ToList();

            var notAvailabilableRanges = schedulePeriod.Except(availableRanges);
            var availabilityRanges = availableRanges.Select(range => (range, true))
                .Concat(notAvailabilableRanges.Select(range => (range, false)))
                .OrderBy(r => r.range.Start)
                .ToArray();

            return new TimeSlotDependentCollection<bool>(availabilityRanges);
        }

        private TimeSlotDependentCollection<List<Demand>> MapDemands(Calendar calendar, Range schedulePeriod)
        {
            var demands = MapIntersectingDemands(calendar, schedulePeriod).ToList();

            var rangesWithDemands = CreateDemandLists(demands, schedulePeriod)
                .Select(list => (range: list.Skip(1).Aggregate(list.First().Period, (unionSoFar, d) => unionSoFar.Union(d.Period)), value: list))
                .ToList();

            var emptyList = new List<Demand>(0);
            var rangesWithoutDemands = schedulePeriod
                .Except(rangesWithDemands.Select(x => x.range))
                .Select(r => (range: r, value: emptyList))
                .ToList();

            var demandRanges = rangesWithDemands.Concat(rangesWithoutDemands)
                .OrderBy(x => x.range.Start)
                .ToArray();

            return new TimeSlotDependentCollection<List<Demand>>(demandRanges);
        }

        private IEnumerable<Demand> MapIntersectingDemands(Calendar calendar, Range schedulePeriod)
        {
            foreach (var dto in _inputModel.Demands)
            {
                var period = Range.Of(start: calendar.DateTimeToTimeSlotIndex(dto.Start), end: calendar.DateTimeToTimeSlotIndex(dto.End) - 1);
                if (period.Overlaps(schedulePeriod))
                {
                    yield return new Demand(period.Intersect(schedulePeriod), MapActivity(dto.Activity), dto.RequiredPersonCount);
                }
            }
        }

        private IEnumerable<List<Demand>> CreateDemandLists(List<Demand> demands, Range schedulePeriod)
        {
            var demandLists = new List<Demand>[schedulePeriod.Length];

            for (int i = 0; i < demandLists.Length; i++)
            {
                demandLists[i] = new List<Demand>();
            }

            foreach (var demand in demands)
            {
                foreach (var timeSlot in demand.Period)
                {
                    demandLists[timeSlot].Add(demand);
                }
            }

            var comparer = EqualityComparer.Create<List<Demand>>((a, b) => DemandListsAreEqual(a, b));

            demandLists = demandLists
                .Where(list => list.Count != 0)
                .Distinct(comparer)
                .ToArray();

            return demandLists;

            bool DemandListsAreEqual(List<Demand> a, List<Demand> b)
            {
                if (a.Count != b.Count) return false;

                var distinctAs = new HashSet<Demand>(a);

                foreach (var demand in b)
                {
                    if (distinctAs.Contains(demand))
                    {
                        distinctAs.Remove(demand);
                    }
                    else
                    {
                        return false;
                    }
                }

                return distinctAs.Count == 0;
            }
        }

        private Activity MapActivity(Dto.Activity dto)
        {
            if (!_activities.TryGetValue(dto.Id, out var activity))
            {
                _activities[dto.Id] = activity = new Activity(dto.Id, dto.Name);
            }

            return activity;
        }
    }
}
