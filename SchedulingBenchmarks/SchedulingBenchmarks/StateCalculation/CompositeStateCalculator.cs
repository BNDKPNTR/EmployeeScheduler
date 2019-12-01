using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    class CompositeStateCalculator
    {
        private readonly Action<Person, State, StateTriggers, int> _refreshState;
        private readonly Action<Person, State> _initializeState;

        public CompositeStateCalculator(Range schedulePeriod, Calendar calendar)
        {
            var stateCalculators = new Dictionary<string, IStateCalculator>
            {
                [nameof(State.WorkedOnWeeked)] = new WorkedOnWeekedStateCalculator(calendar),
                [nameof(State.TotalWorkTime)] = new TotalWorkTimeStateCalculator(),
                [nameof(State.ConsecutiveWorkDayCount)] = new ConsecutiveWorkDayCountStateCalculator(),
                [nameof(State.ConsecutiveDayOffCount)] = new ConsecutiveDayOffCountStateCalculator(),
                [nameof(State.WorkedWeekendCount)] = new WorkedWeekendCountStateCalculator(schedulePeriod, calendar),
                [nameof(State.ShiftWorkedCount)] = new ShiftWorkedCountStateCalculator(),
                [nameof(State.PossibleFutureWorkDayCount)] = new PossibleFutureWorkDayCountStateCalculator(),
            };

            _refreshState = CreateRefreshStateMethod(stateCalculators);
            _initializeState = CreateInitializeStateMethod(stateCalculators);
        }

        public void RefreshState(Person person, int day)
        {
            var triggers = new StateTriggers(person, day);
            var newState = new State();

            _refreshState(person, newState, triggers, day);

            person.State = newState;
        }

        public void InitializeState(Person person)
        {
            var newState = new State();

            _initializeState(person, newState);

            person.State = newState;
        }

        private Action<Person, State, StateTriggers, int> CreateRefreshStateMethod(Dictionary<string, IStateCalculator> stateCalculators)
        {
            return (Action<Person, State, StateTriggers, int>)CreateMethod(stateCalculators, nameof(IStateCalculator.CalculateState), typeof(StateTriggers), typeof(int));
        }

        private Action<Person, State> CreateInitializeStateMethod(Dictionary<string, IStateCalculator> stateCalculators)
        {
            return (Action<Person, State>)CreateMethod(stateCalculators, nameof(IStateCalculator.InitializeState));
        }

        private Delegate CreateMethod(Dictionary<string, IStateCalculator> stateCalculators, string methodName, params Type[] additionalMethodParamTypes)
        {
            var personMethodParam = Expression.Parameter(typeof(Person));
            var newStateParam = Expression.Parameter(typeof(State));
            var methodParams = new[] { personMethodParam, newStateParam }.Concat(additionalMethodParamTypes.Select(t => Expression.Parameter(t))).ToList();

            var methodBody = CreateMethodBody(stateCalculators, methodName, newStateParam, methodParams);
            
            var lambda = Expression.Lambda(methodBody, methodParams);
            return lambda.Compile();
        }

        /// <summary>
        /// Creates a method body that calls the method with the given name on each stateCalculator instance with the given methodParameteres. It saves the result of 
        /// the method calls newStateParam's corresponding property.
        /// 
        /// E.g.:
        /// {
        ///     newState.p1 = p1StateCalculator.methodName(methodParameters);
        ///     var newState.p2 = p2StateCalculator.methodName(methodParameters);
        /// }
        /// </summary>
        private BlockExpression CreateMethodBody(Dictionary<string, IStateCalculator> stateCalculators, string methodName, ParameterExpression newStateParam, IEnumerable<Expression> methodParameters)
        {
            var stateAssignmentExpressions = new List<Expression>();

            foreach (var (propName, stateCalculator) in stateCalculators.OrderBy(x => x, new StateCalculatorDependencyComparer()))
            {
                var stateCalculatorType = stateCalculator.GetType();

                var stateCalculatorReference = Expression.Constant(stateCalculator, stateCalculatorType);
                var methodOnStateCalculator = stateCalculatorType.GetMethod(methodName);
                var stateCalculatorMethodCall = Expression.Call(stateCalculatorReference, methodOnStateCalculator, methodParameters);

                var statePropExpression = Expression.PropertyOrField(newStateParam, propName);
                var newStateAssignment = Expression.Assign(statePropExpression, stateCalculatorMethodCall);

                stateAssignmentExpressions.Add(newStateAssignment);
            }

            return Expression.Block(typeof(void), stateAssignmentExpressions);
        }

        private class StateCalculatorDependencyComparer : IComparer<KeyValuePair<string, IStateCalculator>>
        {
            public int Compare(KeyValuePair<string, IStateCalculator> x, KeyValuePair<string, IStateCalculator> y)
            {
                // TODO: Check transient dependencies for DAG
                var x_DependsOn_y = x.Value.StatePropertyDependencies.Contains(y.Key);
                var y_DependsOn_x = y.Value.StatePropertyDependencies.Contains(x.Key);

                if (x_DependsOn_y && y_DependsOn_x)
                {
                    var xStateCalculatorClassName = x.Value.GetType().Name;
                    var yStateCalculatorClassName = y.Value.GetType().Name;
                    
                    throw new Exception($"Both {xStateCalculatorClassName} and {yStateCalculatorClassName} depends on each other");
                }

                return x_DependsOn_y ? 1 : -1;
            }
        }
    }
}
