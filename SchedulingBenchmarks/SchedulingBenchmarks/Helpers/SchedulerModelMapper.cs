using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    class SchedulerModelMapper
    {
        private readonly Dto.SchedulingPeriod _input;
        private readonly SchedulerModel _model;
        private readonly Dictionary<int, Person> _people;

        public SchedulerModelMapper(SchedulerModel model, Dto.SchedulingPeriod input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _people = _model.People.ToDictionary(p => p.Id);
        }

        public static Dto.SchedulingPeriod MapToInputModel(SchedulerModel model, Dto.SchedulingPeriod input) => new SchedulerModelMapper(model, input).MapToInputModel();

        private Dto.SchedulingPeriod MapToInputModel()
        {
            throw new NotImplementedException();

            return _input;
        }
    }
}
