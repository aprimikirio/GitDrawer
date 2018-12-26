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
using GitDrawer.ParserCore;
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

            Parser parser = new Parser("aprimikirio", DateTime.Now);
            parser.OnCommits += Commits;
            parser.OnNewData += ConsoleAddText;
            parser.OnError += ConsoleAddText;

            GitWorker worker = new GitWorker("C:\\Program Files\\Git");
            ConsoleAddText(worker.GitCommand("D:\\Projects\\Parser", " add *"));
            ConsoleAddText(worker.GitCommand("D:\\Projects\\Parser", " commit -m \"test\""));
            ConsoleAddText(worker.GitCommand("D:\\Projects\\Parser", " push origin master"));
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
