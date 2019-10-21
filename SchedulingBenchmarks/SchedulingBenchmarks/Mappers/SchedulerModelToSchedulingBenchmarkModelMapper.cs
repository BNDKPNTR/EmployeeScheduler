using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assignment = SchedulingBenchmarks.SchedulingBenchmarksModel.Assignment;
using SchedulerAssignment = SchedulingBenchmarks.Models.Assignment;

namespace SchedulingBenchmarks.Mappers
{
    class SchedulerModelToSchedulingBenchmarkModelMapper
    {
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;
        private readonly SchedulerModel _schedulerModel;

        public SchedulerModelToSchedulingBenchmarkModelMapper(SchedulerModel model, SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
            _schedulerModel = model ?? throw new ArgumentNullException(nameof(model));

        }

        public static SchedulingBenchmarkModel MapToSchedulingBenchmarkModel(SchedulerModel model, SchedulingBenchmarkModel schedulingBenchmarkModel)
            => new SchedulerModelToSchedulingBenchmarkModelMapper(model, schedulingBenchmarkModel).MapToInputModel();

        private SchedulingBenchmarkModel MapToInputModel()
        {
            var people = _schedulerModel.People.ToDictionary(p => p.ExternalId);

            foreach (var employee in _schedulingBenchmarkModel.Employees)
            {
                var person = people[employee.Id];

                foreach (var schedulerAssignment in person.Assignments.AllRounds.Values)
                {
                    var assignment = new Assignment
                    {
                        Day = schedulerAssignment.Day,
                        ShiftId = schedulerAssignment.Shift.Id,
                        PersonId = schedulerAssignment.Person.ExternalId
                    };

                    // Az Add() metódussal kényszerítsük ki, hogy hiba keletkezzen, ha egy adott napra több beosztása is lenne egy dolgozónak
                    employee.Assignments.Add(assignment.Day, assignment);
                }
            }

            return _schedulingBenchmarkModel;
        }
    }
}
