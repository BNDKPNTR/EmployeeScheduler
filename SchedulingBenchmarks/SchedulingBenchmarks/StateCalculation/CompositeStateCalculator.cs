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
        private readonly Action<Person, StateTriggers, int> _refreshState;
        private readonly Action<Person> _initializeState;

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

            _refreshState(person, triggers, day);
        }

        public void InitializeState(Person person)
        {
            _initializeState(person);
        }

        private Action<Person, StateTriggers, int> CreateRefreshStateMethod(Dictionary<string, IStateCalculator> stateCalculators)
        {
            return (Action<Person, StateTriggers, int>)CreateMethod(stateCalculators, nameof(IStateCalculator.CalculateState), typeof(StateTriggers), typeof(int));
        }

        private Action<Person> CreateInitializeStateMethod(Dictionary<string, IStateCalculator> stateCalculators)
        {
            return (Action<Person>)CreateMethod(stateCalculators, nameof(IStateCalculator.InitializeState));
        }

        private Delegate CreateMethod(Dictionary<string, IStateCalculator> stateCalculators, string methodName, params Type[] additionalMethodParamTypes)
        {
            var personMethodParam = Expression.Parameter(typeof(Person));
            var personStateProperty = Expression.PropertyOrField(personMethodParam, nameof(Person.State));
            var methodParams = new[] { personMethodParam }.Concat(additionalMethodParamTypes.Select(t => Expression.Parameter(t))).ToList();

            var methodBody = CreateMethodBody(stateCalculators, methodName, personStateProperty, methodParams);
            
            var lambda = Expression.Lambda(methodBody, methodParams);
            return lambda.Compile();
        }

        /// <summary>
        /// Creates a method body that calls the method with the given name on each stateCalculator instance with the given methodParameteres. It saves the result of 
        /// the method calls to temporary variables then writes back each temporary variable's value to the corresponding property of the person's state object.
        /// 
        /// E.g.:
        /// {
        ///     var p1__temp = p1StateCalculator.methodName(methodParameters);
        ///     var p2__temp = p2StateCalculator.methodName(methodParameters);
        /// 
        ///     state.p1 = p1__temp;
        ///     state.p2 = p2__temp;
        /// }
        /// </summary>
        private BlockExpression CreateMethodBody(Dictionary<string, IStateCalculator> stateCalculators, string methodName, MemberExpression personStateProperty, IEnumerable<Expression> methodParameters)
        {
            var calculateStateExpressions = new List<Expression>();
            var stateAssignmentExpressions = new List<Expression>();
            var variables = new List<ParameterExpression>();

            var stateProperties = typeof(State).GetProperties().ToDictionary(x => x.Name);

            foreach (var (propName, stateCalculator) in stateCalculators)
            {
                var stateCalculatorType = stateCalculator.GetType();
                var matchingProp = stateProperties[propName];

                var stateResultVariable = Expression.Variable(matchingProp.PropertyType, $"{propName}__temp");
                variables.Add(stateResultVariable);

                var stateCalculatorReference = Expression.Constant(stateCalculator, stateCalculatorType);
                var methodOnStateCalculator = stateCalculatorType.GetMethod(methodName);
                var stateCalculatorMethodCall = Expression.Call(stateCalculatorReference, methodOnStateCalculator, methodParameters);
                var variableAssignment = Expression.Assign(stateResultVariable, stateCalculatorMethodCall);

                calculateStateExpressions.Add(variableAssignment);

                var statePropExpression = Expression.PropertyOrField(personStateProperty, propName);
                var stateAssignment = Expression.Assign(statePropExpression, stateResultVariable);

                stateAssignmentExpressions.Add(stateAssignment);
            }

            return Expression.Block(typeof(void), variables, calculateStateExpressions.Concat(stateAssignmentExpressions));
        }
    }
}
