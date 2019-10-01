using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.Schedulers;
using SchedulingBenchmarks.StateCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks
{
    class SchedulerAlgorithm
    {
        private readonly SchedulerModel _model;
        private readonly StateCalculator _stateCalculator;
        private readonly WorkEligibilityChecker _workEligibilityChecker;
        private readonly CostFunctionBase _costFunction;
        private readonly SchedulerBase[] _schedulers;

        public SchedulerAlgorithm(SchedulerModel model)
        {
            _model = model;
            _stateCalculator = new StateCalculator(_model.SchedulePeriod, _model.Calendar);
            _workEligibilityChecker = new WorkEligibilityChecker(_model);
            _costFunction = CreateCompositeCostFunction(_model);

            _schedulers = new SchedulerBase[]
            {
                //new MinDemandsScheduler(_model, _costFunction, _workEligibilityChecker, _stateCalculator),
                //new WeekendScheduler(_model, _costFunction, _workEligibilityChecker, _stateCalculator),
                new AllDemandsScheduler(_model, _costFunction, _stateCalculator, _workEligibilityChecker),
                new UntilMinTotalWorkTimeScheduler(_model, _stateCalculator, _workEligibilityChecker, _costFunction),
                new ReplaceSingleWeekendsWithDoubleWeekendsScheduler(_model, _stateCalculator, _workEligibilityChecker, _costFunction),
                new RemoveUnderMinConsecutiveShiftsScheduler(_model, _costFunction, _workEligibilityChecker),
            };
        }

        public void Run()
        {
            foreach (var scheduler in _schedulers)
            {
                scheduler.Run();
            }
        }

        private CompositeCostFunction CreateCompositeCostFunction(SchedulerModel model)
        {
            var (maxShiftOffRequestWeight, maxShiftOnRequestWeight) = ShiftRequestCostFunction.GetMaxWeights(model);

            var costFunctions = new CostFunctionBase[]
            {
                new WeekendWorkCostFunction(_model.Calendar),
                new TotalWorkTimeCostFunction(_model, _workEligibilityChecker, _stateCalculator),
                new ShiftRequestCostFunction(maxShiftOffRequestWeight, maxShiftOnRequestWeight),
                new ConsecutiveShiftCostFunction(_model.Calendar),
                new DayOffCostFunction(),
                new ValidShiftCostFunction(),
                new MaxShiftCostFunction(),
                new MinRestTimeCostFunction()
            };

            return new CompositeCostFunction(costFunctions);
        }
    }
}
