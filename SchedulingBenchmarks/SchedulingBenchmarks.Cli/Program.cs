using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SchedulingBenchmarks.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            var win = new Window("MyApp")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(win);

            var menu = CreateMenuBar();
            top.Add(menu);

            Application.Run();
        }

        private static MenuBar CreateMenuBar()
        {
            var menuitems = new MenuItem[24];
            for (int i = 0; i < menuitems.Length; i++)
            {
                int j = i + 1;
                menuitems[i] = new MenuItem($"_{j}", "", () => OnInstanceSelected(j, Application.Top));
            }

            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", null),
                new MenuItem ("_Close", "", () => { }),
                new MenuItem ("_Quit", "", () => {  })
            }),
            new MenuBarItem ("_Instances", menuitems)
            });

            return menu;
        }

        private static void OnInstanceSelected(int instanceNumber, Toplevel top)
        {
            var viewModel = new ScheduleTableViewModel(instanceNumber);
            var view = new ScheduleTableView(viewModel)
            {
                X = 2,
                Y = 2,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var newTop = new Toplevel(top.Frame);
            var window = new Window($"Instance {instanceNumber}")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            //var frame = new Rect(top.Frame.X, top.Frame.Y, top.Frame.Width - 10, top.Frame.Height - 10);

            //var scrollViewer = new ScrollView(frame)
            //{
            //    X = 0,
            //    Y = 0,
            //    Width = Dim.Fill() - 10,
            //    Height = Dim.Fill() - 10
            //};

            //scrollViewer.ShowHorizontalScrollIndicator = true;
            //scrollViewer.ShowVerticalScrollIndicator = true;
            //scrollViewer.Add(view);

            window.Add(view);

            newTop.Add(CreateMenuBar(), window);
            Application.Run(newTop);
        }
    }

    class ScheduleTableViewModel
    {
        private readonly int _instanceNumber;
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;
        private readonly ScheduleBenchmarkModelFrameStringPrinter _printer;
        private readonly AlgorithmResult _result;

        public int DayColumnWidth => _printer.DayColumnWidth;
        public int DayCount => _schedulingBenchmarkModel.Duration;
        public int EmployeeCount => _schedulingBenchmarkModel.Employees.Length;
        public int PersonColumnWidth => _printer.PersonColumnWidth;

        public AlgorithmResult AlgorithmResult => _result;

        public ScheduleTableViewModel(int instanceNumber)
        {
            _instanceNumber = instanceNumber;
            var dto = SchedulingBenchmarkInstanceReader.FromXml(_instanceNumber);
            _schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);
            _printer = new ScheduleBenchmarkModelFrameStringPrinter(_schedulingBenchmarkModel);
            _result = AlgorithmRunner.GetResult(_instanceNumber);
        }

        public string GetFrame()
        {
            _printer.Print();
            return _printer.ToString();
        }

        public string Run()
        {
            var printer = new ScheduleBenchmarkModelStringPrinter(_result.Result);
            printer.Print();
            return printer.ToString();
        }

        public string[][] GetAssignmentsForDays()
        {
            var result = new string[_schedulingBenchmarkModel.Duration][];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new string[_schedulingBenchmarkModel.Employees.Length];

                for (int j = 0; j < _schedulingBenchmarkModel.Employees.Length; j++)
                {
                    if (_result.Result.Employees[j].Assignments.TryGetValue(i, out var assignment))
                    {
                        result[i][j] = assignment.ShiftId;
                    }
                    else
                    {
                        result[i][j] = " ";
                    }
                }
            }

            return result;
        }
    }

    class BasicScheduleTableView : View
    {
        private readonly ScheduleTableViewModel _viewModel;
        private string[] _buffer;

        public BasicScheduleTableView(ScheduleTableViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _buffer = _viewModel.GetFrame().Split(Environment.NewLine);
        }

        public override bool ProcessColdKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Space)
            {
                var result = _viewModel.Run();

                _buffer = result.Split(Environment.NewLine);

                SetNeedsDisplay();
            }

            return true;
        }
        
        public override void Redraw(Rect region)
        {
            Driver.SetAttribute(ColorScheme.Normal);

            for (int i = 0; i < _buffer.Length; i++)
            {
                Move(0, i);
                Driver.AddStr(_buffer[i]);
            }
        }
    }

    class ScheduleTableView : View
    {
        private readonly ScheduleTableViewModel _viewModel;
        private readonly Random _random;
        private char[][] _charBuffer;
        private readonly int _tableWidth;
        private readonly int _tableHeight;
        private readonly int _tableX;
        private readonly int _tableY;
        private int _fixedColumns = 0;
        private string[] _buffer;

        public ScheduleTableView(ScheduleTableViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _tableX = _viewModel.PersonColumnWidth;
            _tableY = 2;
            _tableHeight = _tableY + _viewModel.EmployeeCount;
            _tableWidth = _tableX + _viewModel.DayColumnWidth * _viewModel.DayCount;
            _random = new Random();

            _charBuffer = CreateBuffer();

            var token = Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(1000 / 25), _ =>
            {
                var _10Percent = (((_tableWidth - _tableX) - _fixedColumns) * (_tableHeight - _tableY)) / 10;
                for (int i = 0; i < _10Percent; i++)
                {
                    var j = _random.Next(_tableY, _tableHeight);
                    var k = _random.Next(_tableX + _fixedColumns, _tableWidth);
                    _charBuffer[j][k] = (char)_random.Next(32, 127);
                }

                SetNeedsDisplay();
                return true;
            });
        }

        public override void Redraw(Rect region)
        {
            if (_buffer != null)
            {
                DrawFinalTable();
                return;
            }

            Driver.SetAttribute(ColorScheme.Normal);

            for (int i = 0; i < _charBuffer.Length; i++)
            {
                Move(0, i);
                Driver.AddStr(new string(_charBuffer[i]));
            }
        }

        private void DrawFinalTable()
        {
            Driver.SetAttribute(ColorScheme.Normal);

            for (int i = 0; i < _buffer.Length; i++)
            {
                Move(0, i);
                Driver.AddStr(_buffer[i]);
            }
        }


        private char[][] CreateBuffer()
        {
            var frame = _viewModel.GetFrame().Split(Environment.NewLine);
            var buffer = new char[frame.Length][];

            for (int i = 0; i < frame.Length; i++)
            {
                buffer[i] = frame[i].ToCharArray();
            }

            for (int i = _tableY; i < _tableHeight; i++)
            {
                for (int j = _tableX; j < _tableWidth; j++)
                {
                    buffer[i][j] = (char)_random.Next(32, 127);
                }
            }

            return buffer;
        }

        public void SetColumn(string[] values, int day)
        {
            if (day >= _tableWidth) return;

            Interlocked.Add(ref _fixedColumns, _viewModel.DayColumnWidth);
            
            var animationLength = 1000 / 60;
            var animationCount = 0;

            while (animationCount < animationLength)
            {
                Thread.Sleep(5);
                string shiftId;
                string shift;

                if (animationCount == animationLength - 1)
                {
                    for (int i = _tableY; i < _tableHeight; i++)
                    {
                        shiftId = values[i - _tableY];
                        shift = $"{shiftId}{new string(' ', _viewModel.DayColumnWidth - shiftId.Length)}";
                        for (int j = 0; j < shift.Length; j++)
                        {
                            _charBuffer[i][_tableX + (day * _viewModel.DayColumnWidth) + j] = shift[j];
                        }
                    }

                    return;
                }

                var rowIndex = _random.Next(_tableY, _tableHeight);
                shiftId = values[rowIndex - _tableY];
                shift = $"{shiftId}{new string(' ', _viewModel.DayColumnWidth - shiftId.Length)}";
                for (int j = 0; j < shift.Length; j++)
                {
                    _charBuffer[rowIndex][_tableX + (day * _viewModel.DayColumnWidth) + j] = shift[j];
                }

                animationCount++;
            }
        }

        public override bool ProcessColdKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Space)
            {
                ThreadPool.QueueUserWorkItem(LoadAssignments);
                return false;
            }

            return true;
        }

        private void LoadAssignments(object arg)
        {
            var assignmentsByDay = _viewModel.GetAssignmentsForDays();

            for (int i = 0; i < assignmentsByDay.Length; i++)
            {
                SetColumn(assignmentsByDay[i], i);
            }

            Application.MainLoop.Invoke(() =>
            {
                //var result = _viewModel.Run();
                var result = PrintResultToConsole(_viewModel.AlgorithmResult);
                _buffer = result.Split(Environment.NewLine);
                this.Clear();
                SetNeedsDisplay();
            });
        }

        private static string PrintResultToConsole(AlgorithmResult algorithmResult, params string[] additionalInformation)
        {
            var builder = new StringBuilder();
            var separator = new string('-', 80);
            var res = algorithmResult.Result.ToFormattedString();
            builder.Append(res);
            builder.AppendLine();
            builder.AppendLine();

            builder.AppendLine($"Name: {algorithmResult.Name}");
            builder.AppendLine($"Penalty: {algorithmResult.Penalty}");

            if (!algorithmResult.Feasible)
            {
                builder.AppendLine("Feasibility: INFEASIBLE");
            }

            if (algorithmResult.Duration != default)
            {
                builder.AppendLine($"Duration: {algorithmResult.Duration}");
            }

            if (additionalInformation.Length > 0)
            {
                foreach (var text in additionalInformation.Where(x => !string.IsNullOrEmpty(x)))
                {
                    builder.AppendLine(text);
                }
            }

            builder.AppendLine();

            if (algorithmResult.FeasibilityMessages?.Count > 0)
            {
                builder.AppendLine();

                foreach (var message in algorithmResult.FeasibilityMessages)
                {
                    builder.AppendLine(message);
                }
            }

            return builder.ToString();
        }
    }
}
