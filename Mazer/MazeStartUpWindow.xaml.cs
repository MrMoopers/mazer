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
using System.Windows.Shapes;

namespace Mazer
{
    /// <summary>
    /// Interaction logic for MazeStartUpWindow.xaml
    /// </summary>
    public partial class MazeStartUpWindow : Window
    {
        //Simply gives click events for the three buttons on this window. One for opening the generate window, 
        //one for opening the solve window and one for closing the current window
        public MazeStartUpWindow()
        {
            InitializeComponent();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnGenerateMaze_Click(object sender, RoutedEventArgs e)
        {
            MazeGeneratorWindow mazeGeneratorWindow = new MazeGeneratorWindow();
            mazeGeneratorWindow.ShowDialog();
        }

        private void btnSolveMaze_Click(object sender, RoutedEventArgs e)
        {
            MazeSolverWindow mazeSolverWindow = new MazeSolverWindow();
            mazeSolverWindow.ShowDialog();
        }
    }
}
