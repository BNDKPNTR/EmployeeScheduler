using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    class SchedulerModelMapper
    {
        private readonly Dto.InputModel _input;
        private readonly SchedulerModel _model;
        private readonly Dictionary<int, Person> _people;
        private readonly Dictionary<int, Dto.Activity> _activityDtos;

        public SchedulerModelMapper(SchedulerModel model, Dto.InputModel input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _people = _model.People.ToDictionary(p => p.Id);
            _activityDtos = _input.Demands.Select(d => d.Activity).Distinct().ToDictionary(a => a.Id);
        }

        public static Dto.InputModel MapToInputModel(SchedulerModel model, Dto.InputModel input) => new SchedulerModelMapper(model, input).MapToInputModel();

        private Dto.InputModel MapToInputModel()
        {
            foreach (var personDto in _input.People)
            {
                var person = _people[personDto.Id];
                personDto.Assignments = MapAndMergeAssignments(person.Assignments, personDto);
            }

            return _input;
        }

        private List<Dto.Assignment> MapAndMergeAssignments(Dictionary<int, Assignment> assignments, Dto.Person personDto)
        {
            var result = new List<Dto.Assignment>();
            var comparer = EqualityComparer.Create<Assignment>((a, b) => a.TimeSlot + 1 == b.TimeSlot && a.Activity == b.Activity);
            var orderedAssignments = assignments.Values.OrderBy(a => a.TimeSlot).ToList();
            var assignmentGroup = new List<Assignment>();

            if (orderedAssignments.Count > 0)
            {
                assignmentGroup.Add(orderedAssignments[0]);
            }

            for (int i = 1; i < orderedAssignments.Count; i++)
            {
                var previousAssignment = assignmentGroup[assignmentGroup.Count - 1];
                var currentAssignment = orderedAssignments[i];

                if (comparer.Equals(previousAssignment, currentAssignment))
                {
                    assignmentGroup.Add(currentAssignment);
                }
                else
                {
                    result.Add(MapToAssignmentDto(assignmentGroup, _activityDtos[previousAssignment.Activity.Id], personDto));

                    assignmentGroup.Clear();
                    assignmentGroup.Add(currentAssignment);
                }
            }

            if (assignmentGroup.Count > 0)
            {
                result.Add(MapToAssignmentDto(assignmentGroup, _activityDtos[assignmentGroup[0].Activity.Id], personDto));
            }

            return result;
        }

        private Dto.Assignment MapToAssignmentDto(List<Assignment> assignments, Dto.Activity activityDto, Dto.Person personDto)
        {
            var minTimeSlot = assignments[0].TimeSlot;
            var maxTimeSlot = assignments[assignments.Count - 1].TimeSlot;

            return new Dto.Assignment
            {
                Start = _model.Calendar.TimeSlotIndexToDateTime(minTimeSlot),
                End = _model.Calendar.TimeSlotIndexToDateTime(maxTimeSlot + 1),
                Person = personDto,
                Activity = activityDto
            };
        }
    }
}
