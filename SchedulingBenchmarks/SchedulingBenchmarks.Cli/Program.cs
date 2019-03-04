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
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", null),
                new MenuItem ("_Close", "", () => { }),
                new MenuItem ("_Quit", "", () => {  })
            }),
            new MenuBarItem ("_Edit", new MenuItem [] {
                new MenuItem ("_Copy", "", null),
                new MenuItem ("C_ut", "", null),
                new MenuItem ("_Paste", "", null)
            })
        });
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

            var tableView = new ScheduleTableView(60, 20)
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

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
            win.Add(
                // The ones with my favorite layout system
                //login, password, loginText, passText,

                //    // The ones laid out like an australopithecus, with absolute positions:
                //    new CheckBox(3, 6, "Remember me"),
                //    new RadioGroup(3, 8, new[] { "_Personal", "_Company" }),
                //    new Button(3, 14, "Ok"),
                //    new Button(10, 14, "Cancel"),
                //    new Label(3, 18, "Press ESC and 9 to activate the menubar"),
                    
                tableView);

            Application.Run();
        }
    }

    class ScheduleTableView : View
    {
        private readonly Random _random;
        private char[][] _buffer;
        private readonly int _width;
        private readonly int _height;
        private int _fixedColumns = 0;

        public ScheduleTableView(int width, int height)
        {
            _width = width;
            _height = height;
            _random = new Random();

            _buffer = CreateBuffer();

            var token = Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(1000 / 25), _ =>
            {
                var _10Percent = ((_width - _fixedColumns) * _height) / 10;
                for (int i = 0; i < _10Percent; i++)
                {
                    _buffer[_random.Next(_height)][_random.Next(_fixedColumns, _width)] = (char)_random.Next(32, 127);
                }

                SetNeedsDisplay();
                return true;
            });
        }

        public override void Redraw(Rect region)
        {            
            Driver.SetAttribute(ColorScheme.Normal);

            for (int i = 0; i < _height; i++)
            {
                Move(0, i);
                Driver.AddStr(new string(_buffer[i]));
            }
        }

        private char[][] CreateBuffer()
        {
            var buffer = new char[_height][];

            for (int i = 0; i < _height; i++)
            {
                buffer[i] = new char[_width];

                for (int j = 0; j < _width; j++)
                {
                    buffer[i][j] = (char)_random.Next(32, 127);
                }
            }

            return buffer;
        }

        public void SetColumn(char[] values, int columnIndex)
        {
            if (columnIndex >= _width) return;

            _fixedColumns++;
            var animationLength = 1000 / 60;
            var animationCount = 0;

            var token = Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(animationLength), mainLoop =>
            {
                if (animationCount > animationLength)
                {
                    for (int i = 0; i < _height; i++)
                    {
                        _buffer[i][columnIndex] = values[i];
                    }

                    //mainLoop.RemoveTimeout(token);
                    return false;
                }

                var rowIndex = _random.Next(_height);
                _buffer[rowIndex][columnIndex] = values[rowIndex];
                animationCount++;

                return true;
            });
        }

        public override bool ProcessColdKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Space)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(Foo);
                return false;
            }

            return true;
        }

        private void Foo(object arg)
        {
            for (int j = 0; j < _width; j++)
            {
                var values = new char[_height];

                for (int i = 0; i < _height; i++)
                {
                    var rand = _random.NextDouble();
                    var rounded = Math.Round(rand);
                    var isZero = 1.0 - rounded > double.Epsilon;


                    values[i] = isZero ? ' ' : 'D';
                }

                Application.MainLoop.Invoke(() => SetColumn(values, j));

                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
