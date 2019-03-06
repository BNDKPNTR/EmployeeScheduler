using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

            // Creates the top-level window to show
            var win = new Window("MyApp")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(win);

            // Creates a menubar, the item "New" has a help menu.
            var menu = CreateMenuBar();
            top.Add(menu);

            var login = new Label("Login: ") { X = 3, Y = 2 };
            var password = new Label("Password: ")
            {
                X = Pos.Left(login),
                Y = Pos.Top(login) + 1
            };
            var loginText = new TextField("")
            {
                X = Pos.Right(password),
                Y = Pos.Top(login),
                Width = 40
            };
            var passText = new TextField("")
            {
                Secret = true,
                X = Pos.Left(loginText),
                Y = Pos.Top(password),
                Width = Dim.Width(loginText)
            };

            //var tableView = new ScheduleTableView(60, 20)
            //{
            //    X = Pos.Center(),
            //    Y = Pos.Center(),
            //    Width = Dim.Fill(),
            //    Height = Dim.Fill()
            //};

            //var button = new Button(10, 5, "headfasdfuidhsfgihadfgiuergnjerdgfheadfasdfuidhsfgihadfgiuergnjerdgfheadfasdfuidhsfgihadfgiuergnjerdgfheadfasdfuidhsfgihadfgiuergnjerdgfheadfasdfuidhsfgihadfgiuergnjerdgfheadfasdfuidhsfgihadfgiuergnjerdgf");

            //var scrollView = new ScrollView(new Rect(0, 0, 90, 25))
            //{
            //    X = 3,
            //    Y = 0,
            //    Width = 80,
            //    Height = 20
            //};

            //scrollView.ShowVerticalScrollIndicator = true;
            //scrollView.ShowHorizontalScrollIndicator = true;

            //scrollView.Add(button);

            // Add some controls, 
            //win.Add(
                // The ones with my favorite layout system
                //login, password, loginText, passText,

                //    // The ones laid out like an australopithecus, with absolute positions:
                //    new CheckBox(3, 6, "Remember me"),
                //    new RadioGroup(3, 8, new[] { "_Personal", "_Company" }),
                //    new Button(3, 14, "Ok"),
                //    new Button(10, 14, "Cancel"),
                //    new Label(3, 18, "Press ESC and 9 to activate the menubar"),
                    
                //tableView);

            Application.Run();
        }

        private static MenuBar CreateMenuBar()
        {
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", null),
                new MenuItem ("_Close", "", () => { }),
                new MenuItem ("_Quit", "", () => {  })
            }),
            new MenuBarItem ("_Instances", new MenuItem [] {
                new MenuItem ("_1", "", () => OnInstanceSelected(17, Application.Top)),
                //new MenuItem ("C_ut", "", null),
                //new MenuItem ("_Paste", "", null)
            })
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

            //for (int i = 0; i < _result.Result.Employees.Length; i++)
            //{
            //    var employee = _result.Result.Employees[i];

            //    foreach (var assignment in employee.Assignments.Values)
            //    {
            //        result[assignment.Day][i] = assignment.ShiftId;
            //    }
            //}

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
                var _10Percent = ((_tableWidth - _fixedColumns) * _tableHeight) / 10;
                for (int i = 0; i < _10Percent; i++)
                {
                    _charBuffer[_random.Next(_tableY, _tableHeight)][_random.Next(_tableX + _fixedColumns, _tableWidth)] = (char)_random.Next(32, 127);
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

            //_fixedColumns++;

            //for (int i = _tableY; i < _tableHeight; i++)
            //{
            //    for (int j = 0; j < _viewModel.DayColumnWidth; j++)
            //    {
            //        var shift = values[i - _tableY];
            //        _charBuffer[i][_tableX + j] = j < shift.Length ? shift[j] : ' ';
            //    }
            //}



            if (day >= _tableWidth) return;

            _fixedColumns++;
            var animationLength = 1000 / 60;
            var animationCount = 0;

            var token = Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(animationLength), mainLoop =>
            {
                if (animationCount > animationLength)
                {
                    //for (int i = _tableY; i < _tableHeight; i++)
                    //{
                    //    int j = 0;
                    //    while (j < values[i].Length)
                    //    {
                    //        _charBuffer[i][_tableX + day * j] = values[i - _tableY][j];
                    //        j++;
                    //    }

                    //    while (j < _viewModel.DayColumnWidth)
                    //    {
                    //        _charBuffer[i][_tableX + day * j] = ' ';
                    //    }
                    //}

                    for (int i = _tableY; i < _tableHeight; i++)
                    {
                        for (int j = 0; j < _viewModel.DayColumnWidth; j++)
                        {
                            var shift = values[i - _tableY];
                            _charBuffer[i][_tableX + j * day] = j < shift.Length ? shift[j] : ' ';
                        }
                    }

                    return false;
                }

                //var rowIndex = _random.Next(_tableY, _tableHeight);
                ////_charBuffer[rowIndex][day] = values[rowIndex];

                //int k = 0;
                //while (k < values[rowIndex].Length)
                //{
                //    _charBuffer[rowIndex][_tableX + day * k] = values[rowIndex - _tableY][k];
                //    k++;
                //}

                //while (k < _viewModel.DayColumnWidth)
                //{
                //    _charBuffer[rowIndex][_tableX + day * k] = ' ';
                //}

                animationCount++;

                return true;
            });
        }

        public override bool ProcessColdKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Space)
            {

                var result = _viewModel.Run();

                _buffer = result.Split(Environment.NewLine);

                SetNeedsDisplay();


                //System.Threading.ThreadPool.QueueUserWorkItem(LoadAssignments);
                return false;
            }

            return true;
        }

        private void LoadAssignments(object arg)
        {
            var assignmentsByDay = _viewModel.GetAssignmentsForDays();

            //for (int i = 0; i < assignmentsByDay.Length; i++)
            //{
            //    System.Threading.Thread.Sleep(_random.Next(20, 35));

            //    Application.MainLoop.Invoke(() => SetColumn(assignmentsByDay[i], i));
            //}

            Application.MainLoop.Invoke(() => SetColumn(assignmentsByDay[0], 0));
        }

        private void Foo(object arg)
        {
            for (int j = 0; j < _viewModel.DayCount; j++)
            {
                var values = new string[_viewModel.EmployeeCount];

                for (int i = 0; i < _viewModel.EmployeeCount; i++)
                {
                    var rand = _random.NextDouble();
                    var rounded = Math.Round(rand);
                    var isZero = 1.0 - rounded > double.Epsilon;


                    values[i] = isZero ? " " : "D";
                }

                Application.MainLoop.Invoke(() => SetColumn(values, j));

                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
