using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GitDrawer.GitCore;

namespace GitDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /*
            Parser parser = new Parser("aprimikirio", DateTime.Now);
            parser.OnCommits += Commits;
            parser.OnNewData += ConsoleAddText;
            parser.OnError += ConsoleAddText;
            */

            DrawGrid(50, 30);

            //GitWorker worker = new GitWorker("C:\\Program Files\\Git", "D:\\Projects\\GitDrawer");
        }

        private void DrawGrid(double x = 0, double y = 0, double edge = 10, double gap = 2, int width = 53, int height = 7)
        {for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Rectangle dayRectangle = new Rectangle();
                    dayRectangle.Width = edge;
                    dayRectangle.Height = edge;
                    dayRectangle.VerticalAlignment = VerticalAlignment.Top;
                    dayRectangle.HorizontalAlignment = HorizontalAlignment.Left;
                    dayRectangle.Fill = Brushes.Green;
                    dayRectangle.Triggers.Add( new EventTrigger());

                    CalendarCanvas.Children.Add(dayRectangle);
                    Canvas.SetLeft(dayRectangle, x + (edge + gap) * i);
                    Canvas.SetTop(dayRectangle, y + (edge + gap) * j);
                }
            }
        }

        private void ConsoleAddText(string message)
        {
            Console.Text += "\n" + message + "\n" + new String('-', 15);
        }

        private void Commits(int count)
        {
            Console.Text += "\n(" + count + ")\n" + new String('-', 15);
        }
    }
}
