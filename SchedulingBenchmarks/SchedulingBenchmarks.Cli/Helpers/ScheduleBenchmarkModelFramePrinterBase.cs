using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.Cli
{

    class ScheduleBenchmarkModelFrameStringPrinter : ScheduleBenchmarkModelFramePrinterBase
    {
        private readonly StringBuilder _builder;

        public ScheduleBenchmarkModelFrameStringPrinter(SchedulingBenchmarkModel schedulingBenchmarkModel) : base(schedulingBenchmarkModel)
        {
            _builder = new StringBuilder();
        }

        protected override void Write(string value) => _builder.Append(value);

        public override string ToString() => _builder.ToString();
    }

    abstract class ScheduleBenchmarkModelFramePrinterBase
    {
        private const char HorizontalSeparator = '_';
        private const char VerticalSeparator = '|';

        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;
        private readonly Dictionary<string, TimeSpan> _shiftLengths;
        private readonly int _personColumnWidth;
        private readonly int _dayColumnWidth;

        public int DayColumnWidth => _dayColumnWidth;
        public int PersonColumnWidth => _personColumnWidth;

        protected ScheduleBenchmarkModelFramePrinterBase(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));

            _shiftLengths = _schedulingBenchmarkModel.Shifts.ToDictionary(s => s.Id, s => s.Duration);
            _personColumnWidth = _schedulingBenchmarkModel.Employees.Max(e => e.Id.Length) + 2;

            var maxDayNumberWidth = _schedulingBenchmarkModel.Duration.ToString().Length;
            var maxShiftIdWidth = _schedulingBenchmarkModel.Shifts.Max(s => s.Id.Length);
            _dayColumnWidth = Math.Max(maxDayNumberWidth, maxShiftIdWidth) + 1;
        }

        protected abstract void Write(string value);

        protected virtual void BeforeWeekendPrintStart() { }

        protected virtual void AfterWeekendPrintEnd() { }

        public void Print()
        {
            PrintTableHeader();

            foreach (var employee in _schedulingBenchmarkModel.Employees)
            {
                PrintEmployeeId(employee);

                foreach (var day in SchedulePeriod())
                {
                    PrintEmployeeAssignment(employee, day);
                }

                PrintTotalWorkedMinutes(employee);
                WriteLine();
            }

            PrintTableFooter();
        }

        private void PrintTableHeader()
        {
            var emptyPersonColumn = new string(' ', _personColumnWidth);
            var fullWidthHorizontalSeparator = new string(HorizontalSeparator, _schedulingBenchmarkModel.Duration * _dayColumnWidth);

            Write(emptyPersonColumn);

            foreach (var day in SchedulePeriod())
            {
                var dayString = day.ToString();
                Write($"{dayString}{new string(' ', _dayColumnWidth - dayString.Length)}");
            }

            WriteLine();
            WriteLine($"{emptyPersonColumn}{fullWidthHorizontalSeparator}");
        }

        private void PrintTableFooter()
        {
            var assignmentsOnDays = CreateAssignmentsOnDaysArray();

            var emptyPersonColumn = new string(' ', _personColumnWidth - 1);
            var fullWidthHorizontalSeparator = new string(HorizontalSeparator, _schedulingBenchmarkModel.Duration * _dayColumnWidth);

            WriteLine($"{emptyPersonColumn}{VerticalSeparator}{fullWidthHorizontalSeparator}{VerticalSeparator}");
            //Write(new string(' ', _personColumnWidth));

            //foreach (var day in SchedulePeriod())
            //{
            //    var infeasibleDemandCount = _schedulingBenchmarkModel.Demands[day].Sum(d => d.MinEmployeeCount) - assignmentsOnDays[day];
            //    var infeasibleDemandCountString = infeasibleDemandCount.ToString();
            //    Write($"{infeasibleDemandCountString}{new string(' ', _dayColumnWidth - infeasibleDemandCountString.Length)}");
            //}
        }

        private void PrintEmployeeId(Employee employee) => Write($"{employee.Id}{new string(' ', _personColumnWidth - employee.Id.Length - 2)} {VerticalSeparator}");

        private void PrintEmployeeAssignment(Employee employee, int day)
        {
            if (employee.Assignments.TryGetValue(day, out var assignment))
            {
                Write($"{assignment.ShiftId}{new string(' ', _dayColumnWidth - assignment.ShiftId.Length)}");
            }
            else
            {
                Write(new string(' ', _dayColumnWidth));
            }
        }

        private void PrintTotalWorkedMinutes(Employee employee)
        {
            //var workedMinutes = employee.Assignments.Values.Sum(a => _shiftLengths[a.ShiftId].TotalMinutes);
            //var formattedWorkedMinutes = string.Empty;
            //if (employee.Contract.MinTotalWorkTime <= workedMinutes && workedMinutes <= employee.Contract.MaxTotalWorkTime)
            //{
            //    formattedWorkedMinutes = "0";
            //}
            //else if (workedMinutes < employee.Contract.MinTotalWorkTime)
            //{
            //    formattedWorkedMinutes = $"-{employee.Contract.MinTotalWorkTime - workedMinutes}";
            //}
            //else if (workedMinutes > employee.Contract.MaxTotalWorkTime)
            //{
            //    formattedWorkedMinutes = $"+{workedMinutes - employee.Contract.MaxTotalWorkTime}";
            //}

            //Write($"{VerticalSeparator} {formattedWorkedMinutes}");
            Write(VerticalSeparator.ToString());
        }

        private void WriteLine() => Write(Environment.NewLine);

        private void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        private IEnumerable<int> SchedulePeriod()
        {
            for (int i = 0; i < _schedulingBenchmarkModel.Duration; i++)
            {
                if (IsSaturday(i)) BeforeWeekendPrintStart();

                yield return i;

                if (IsSunday(i)) AfterWeekendPrintEnd();
            }

            if (IsSunday(_schedulingBenchmarkModel.Duration)) AfterWeekendPrintEnd();

            bool IsSaturday(int day) => day % 7 == 5;
            bool IsSunday(int day) => day % 7 == 6;
        }

        private int[] CreateAssignmentsOnDaysArray()
        {
            var assignmentsOnDays = new int[_schedulingBenchmarkModel.Duration];
            var assignmentsOnDaysDictionary = _schedulingBenchmarkModel.Employees
                .SelectMany(e => e.Assignments.Values)
                .GroupBy(a => a.Day)
                .ToDictionary(g => g.Key, g => g.Count());

            for (int i = 0; i < assignmentsOnDays.Length; i++)
            {
                assignmentsOnDaysDictionary.TryGetValue(i, out assignmentsOnDays[i]);
            }

            return assignmentsOnDays;
        }
    }
}
