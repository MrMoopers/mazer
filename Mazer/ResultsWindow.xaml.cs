using Mazer.ViewModels;
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
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private ResultsWindowViewModel _resultsWindowViewModel;

        public ResultsWindow()
        {
            InitializeComponent();

            //Once intialised creates a new viewModel and fills it with the _resultsWindowViewModel data
            _resultsWindowViewModel = new ResultsWindowViewModel();
            DataContext = _resultsWindowViewModel;
        }

        public ResultsWindowViewModel ResultsWindowViewModel
        {
            get { return _resultsWindowViewModel; }
            set { _resultsWindowViewModel = value; }
        }
    }
}
