using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Drawing;
using Mazer.Enums;
using Microsoft.CSharp;
using System.IO;
using Microsoft.Win32;
using MazeLibrary;
using System.Diagnostics;
using Mazer.Model;
using System.Collections.ObjectModel;
using MazeLibrary.Events;
using MazeLibrary.Algorithms;
using Mazer.Helpers;
using Mazer.ViewModels;
using System.Windows.Threading;

namespace Mazer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MazeGeneratorWindow : Window
    {
        //_gridStorage stores all generated grids so th user can see them whereas _grid stores only the current / latest one
        private MazeGeneratorWindowViewModel _mazeGeneratorWindowViewModel;
        private MazeLibrary.Grid _grid;
        private List<MazeLibrary.Grid> _gridStorage = new List<MazeLibrary.Grid>();

        public MazeGeneratorWindow()
        {
            InitializeComponent();

            _mazeGeneratorWindowViewModel = new MazeGeneratorWindowViewModel();
            DataContext = _mazeGeneratorWindowViewModel;

            //Setting up buttons that require a grid maze to be inactive before a maze has been created. 
            //Wilsons algorithm box is inactive as it hasn't been implemented yet
            btnSaveImage.IsEnabled = false;
            btnSaveAsMaz.IsEnabled = false;
            btnSaveResults.IsEnabled = false;
            WilsonsAlgorithmBox.IsEnabled = false;


            //Setting default values for textboxes and sliders
            _mazeGeneratorWindowViewModel.Delay = 1;
            _mazeGeneratorWindowViewModel.Width = 32;
            _mazeGeneratorWindowViewModel.Height = 30;
            _mazeGeneratorWindowViewModel.WallSize = 25;
            _mazeGeneratorWindowViewModel.WallThickness = 2;
            hideGridLinesBox.IsChecked = true;
            _mazeGeneratorWindowViewModel.HideGridLinesSelected = true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //Closes window and returns control to the startup screen
            Close();
        }

        private async void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            //Code validating input, by stopping a crash when non-usable values are entered
            if (txtbxMazeHeight.Text == "" || txtbxMazeWidth.Text == "" || txtbxMazeHeight.Text == "0" || txtbxMazeWidth.Text == "0")
            {
                MessageBox.Show("Both height and width inputs must me a number greater than zero!");
                return;
            }


            //delay gets the value from the view model
            int delay = _mazeGeneratorWindowViewModel.Delay;

            //User should not be able to edit the delay otherwise an unfair time would be given for the generated maze
            sldDelay.IsEnabled = false;
            txtDelay.IsEnabled = false;
            btnGenerate.IsEnabled = false;

            btnSaveImage.IsEnabled = false;
            btnSaveAsMaz.IsEnabled = false;
            btnSaveResults.IsEnabled = false;

            hideHighlightBox.IsEnabled = false;
            hideGridLinesBox.IsEnabled = false;
            hideWallAndPathUpdatesBox.IsEnabled = false;

            _gridStorage = new List<MazeLibrary.Grid>();

            //Clears view model of data grid data for new run
            _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Clear();

            //Code ensuring for most reasonable maze dimensions the maze will be drawn within the bounds of the canvas
            _mazeGeneratorWindowViewModel.WallSize = 25;
            while (_mazeGeneratorWindowViewModel.WallSize * _mazeGeneratorWindowViewModel.Width >= generatorCanvas.ActualWidth ||
              _mazeGeneratorWindowViewModel.WallSize * _mazeGeneratorWindowViewModel.Height >= generatorCanvas.ActualHeight)
            {
                _mazeGeneratorWindowViewModel.WallSize--;
                if (_mazeGeneratorWindowViewModel.WallSize <= 1) { break; }
            }

            //Creates new grid of specified dimensions
            _grid = new MazeLibrary.Grid("", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
            DisplayMaze(_grid);

            //Sets all timing data to null for the new run
            AlgorithmMetric binaryTreeAlgorithmAlgorithmMetric = null;
            AlgorithmMetric drunkardsWalkAlgorithmAlgorithmMetric = null;
            AlgorithmMetric huntAndKillAlgorithmAlgorithmMetric = null;
            AlgorithmMetric kruskalsAlgorithmAlgorithmMetric = null;
            AlgorithmMetric primsAlgorithmAlgorithmMetric = null;
            AlgorithmMetric recursiveBackTrackerAlgorithmAlgorithmMetric = null;
            AlgorithmMetric recursiveDivisionAlgorithmAlgorithmMetric = null;
            AlgorithmMetric sidewinderAlgorithmAlgorithmMetric = null;
            AlgorithmMetric simpleRectangleAlgorithmAlgorithmMetric = null;
            AlgorithmMetric wilsonsAlgorithmAlgorithmMetric = null;

            //Booleans store value in checkboxes so generation can't be confused if the user clicks a
            //new algorithm box before it has finished making mazes
            bool doBinaryTreeAlgorithm = BinaryTreeBox.IsChecked.Value;
            bool doDrunkardsWalkAlgorithm = DrunkardsWalkBox.IsChecked.Value;
            bool doHuntAndKillAlgorithm = HuntandKillBox.IsChecked.Value;
            bool doKruskalsAlgorithm = KruskalsBox.IsChecked.Value;
            bool doPrimsAlgorithm = PrimsBox.IsChecked.Value;
            bool doRecursiveBackTrackerAlgorithm = RecursiveBackTrackerBox.IsChecked.Value;
            bool doRecursiveDivisionAlgorithm = RecursiveDivisionBox.IsChecked.Value;
            bool doSidewinderAlgorithm = SidewinderBox.IsChecked.Value;
            bool doSimpleRectangleAlgorithm = SimpleRectangleBox.IsChecked.Value;
            bool doWilsonsAlgorithm = WilsonsAlgorithmBox.IsChecked.Value;

            //Simple method of hiding the guide path if that checkbox is checked
            if (_mazeGeneratorWindowViewModel.HideGridLinesSelected)
            {
                GuidePath.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                GuidePath.Visibility = System.Windows.Visibility.Visible;
            }
            // if (!_mazeGeneratorWindowViewModel.HideGridLinesSelected) ;

            //Allows the displaying of a maze to be on one thread while the algorithms still create the maze on another. 
            //(Algorithms therefore run in the background on one thread, firing off events to update the ui on the main thread with new information) 
            await Task.Run(() =>
            {
                Stopwatch stopwatch = new Stopwatch();

                //The guide for the maze is always the same, as every maze is made with the same dimensions
                DisplayGuide(_grid);

                //All algorithm's if-checked statements follow the same procedure. I will therefore only comment on the binary Tree algorithm

                #region Binary Tree

                if (doBinaryTreeAlgorithm)
                {
                    //_grid is set to a new grid named this algorithm
                    _grid = new MazeLibrary.Grid("Grid: BinaryTree", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);

                    //Updates a label with the currently running algorithm
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    //Stopwatch is reset and started and BinaryTreeAlgorithm is created with the required delay. Whenever a wall created or passage created event 
                    //is called in the code it creates a new handle for it on another thread. There it updates the ui canvas. After the maze is created on 
                    //_grid the stopwatch is stopped
                    stopwatch.Reset();
                    stopwatch.Start();

                    //Initaliates a new BinaryTreeAlgorithm
                    BinaryTreeAlgorithm binaryTreeAlgorithm = new BinaryTreeAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        //Only if the hide wall and path updates checkbox is not checked does it handle the event calls
                        binaryTreeAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        binaryTreeAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    //Only if the hide highlight updates checkbox is not checked does it handle the event calls
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) binaryTreeAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;

                    //Genrerates the maze on _grid
                    binaryTreeAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    //Add the grid to the storage. As all algorithms are ordered the same as they appear in the data grid,
                    //the row selected in the data grid directly corasponds to the index () in the _gridStorage list (Where both indexes start at zero)
                    _gridStorage.Add(_grid);

                    //Creates and stores all data recorded for use in the creation of the histogram 
                    binaryTreeAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "BinaryTree",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Drunkards Walk

                if (doDrunkardsWalkAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: DrunkardsWalk", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    DrunkardsWalkAlgorithm drunkardsWalkAlgorithm = new DrunkardsWalkAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        drunkardsWalkAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        drunkardsWalkAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) drunkardsWalkAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    drunkardsWalkAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    drunkardsWalkAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "DrunkardsWalk",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Hunt and Kill

                if (doHuntAndKillAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: HuntandKill", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    HuntAndKillAlgorithm huntAndKillAlgorithm = new HuntAndKillAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        huntAndKillAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        huntAndKillAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) huntAndKillAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    huntAndKillAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    huntAndKillAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "HuntandKill",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Kruskals

                if (doKruskalsAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: Kruskals", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    KruskalsAlgorithm kruskalsAlgorithm = new KruskalsAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        kruskalsAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        kruskalsAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) kruskalsAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    kruskalsAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    kruskalsAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "Kruskals",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Prims

                if (doPrimsAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: Prims", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    PrimsAlgorithm primsAlgorithm = new PrimsAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        primsAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        primsAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) primsAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    primsAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    primsAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "Prims",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Recursive Back Tracker

                if (doRecursiveBackTrackerAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: RecursiveBackTracker", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    RecursiveBackTrackerAlgorithm recursiveBackTrackerAlgorithm = new RecursiveBackTrackerAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        recursiveBackTrackerAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        recursiveBackTrackerAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) recursiveBackTrackerAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    recursiveBackTrackerAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    recursiveBackTrackerAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "RecursiveBackTracker",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Recursive Division

                if (doRecursiveDivisionAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: RecursiveDivision", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    RecursiveDivisionAlgorithm recursiveDivisionAlgorithm = new RecursiveDivisionAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        recursiveDivisionAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        recursiveDivisionAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) recursiveDivisionAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    recursiveDivisionAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    recursiveDivisionAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "RecursiveDivision",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Sidewinder

                if (doSidewinderAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: Sidewinder", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    SidewinderAlgorithm sidewinderAlgorithm = new SidewinderAlgorithm(delay);
                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        sidewinderAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        sidewinderAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }
                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) sidewinderAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    sidewinderAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    sidewinderAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "Sidewinder",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Simple Rectangle

                if (doSimpleRectangleAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: SimpleRectangle", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    SimpleRectangleAlgorithm simpleRectangleAlgorithm = new SimpleRectangleAlgorithm(delay);

                    if (!_mazeGeneratorWindowViewModel.HideWallAndPathUpdatesSelected)
                    {
                        simpleRectangleAlgorithm.RaiseCellWallCreatedEvent += HandleCellWallCreatedEvent;
                        simpleRectangleAlgorithm.RaiseCellPassageCreatedEvent += HandleCellPassageCreatedEvent;
                    }

                    if (!_mazeGeneratorWindowViewModel.HideHighlightSelected) simpleRectangleAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    simpleRectangleAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    simpleRectangleAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "SimpleRectangle",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                #region Wilsons

                if (doWilsonsAlgorithm)
                {
                    _grid = new MazeLibrary.Grid("Grid: WilsonsAlgorithm", _mazeGeneratorWindowViewModel.Width, _mazeGeneratorWindowViewModel.Height);
                    _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

                    stopwatch.Reset();
                    stopwatch.Start();
                    WilsonsAlgorithm.Generate(_grid);
                    stopwatch.Stop();

                    _gridStorage.Add(_grid);

                    wilsonsAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "WilsonsAlgorithm",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RelativeComplexity = GetComplexity(_grid),
                    };
                }

                #endregion

                //As no maze is being generated now the label is set to empty
                _mazeGeneratorWindowViewModel.AlgorithimLabel = "";

                //Clear all cells that are currently highlighted
                ClearHighlight();

                //To ensure a maze is always displayed I force another diplaying of the latest grid here
                DisplayMaze(_grid);
            });

            //Adds only the algorithm times from the algorithms that created a maze to an AlgorithmMetricCollection as a storage for
            //all the algorithms used
            if (binaryTreeAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(binaryTreeAlgorithmAlgorithmMetric);
            }
            if (drunkardsWalkAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(drunkardsWalkAlgorithmAlgorithmMetric);
            }
            if (huntAndKillAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(huntAndKillAlgorithmAlgorithmMetric);
            }
            if (kruskalsAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(kruskalsAlgorithmAlgorithmMetric);
            }
            if (primsAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(primsAlgorithmAlgorithmMetric);
            }
            if (recursiveBackTrackerAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(recursiveBackTrackerAlgorithmAlgorithmMetric);
            }
            if (recursiveDivisionAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(recursiveDivisionAlgorithmAlgorithmMetric);
            }
            if (sidewinderAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(sidewinderAlgorithmAlgorithmMetric);
            }
            if (simpleRectangleAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(simpleRectangleAlgorithmAlgorithmMetric);
            }
            if (wilsonsAlgorithmAlgorithmMetric != null)
            {
                _mazeGeneratorWindowViewModel.AlgorithmMetricCollection.Add(wilsonsAlgorithmAlgorithmMetric);
            }

            //As everything has finished and there are now accessable mazes all sliders and buttons are now enabled
            sldDelay.IsEnabled = true;
            txtDelay.IsEnabled = true;
            btnGenerate.IsEnabled = true;

            btnSaveImage.IsEnabled = true;
            btnSaveAsMaz.IsEnabled = true;
            btnSaveResults.IsEnabled = true;

            hideHighlightBox.IsEnabled = true;
            hideGridLinesBox.IsEnabled = true;
            hideWallAndPathUpdatesBox.IsEnabled = true;
            ClearHighlight();
        }

        private void ClearHighlight()
        {
            //Resets all highlighting
            _mazeGeneratorWindowViewModel.HighlightPathData = Geometry.Parse("");
        }

        void HandleCellPassageCreatedEvent(object sender, CellPassageCreatedEventArgs e)
        {
            //Displays the current _grid in whatever state of completeness it is in
            DisplayMaze(e.Grid);
        }

        void HandleCellWallCreatedEvent(object sender, CellWallCreatedEventArgs e)
        {
            //Displays the current _grid in whatever state of completeness it is in
            DisplayMaze(e.Grid);
        }

        void HandleHighlightCellEvent(object sender, HighlightPathAddedNewCellEventArgs e)
        {
            //Displays the highlight list of cells
            DisplayHighlight(e.Grid, e.HighlightCells);
        }

        private void DisplayGuide(MazeLibrary.Grid _grid)
        {
            //Runs this code at the same time as running the rest of the program
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
            {
                int width = _grid.Width;
                int height = _grid.Height;
                int wallSize = _mazeGeneratorWindowViewModel.WallSize;

                //Creates the guide path with the pre-created data
                string guidePath = PathHelper.GetGuidePath(width, height, wallSize);
                _mazeGeneratorWindowViewModel.GuidePathData = Geometry.Parse(guidePath);
            });
        }

        private void DisplayMaze(MazeLibrary.Grid grid)
        {
            //Runs this code at the same time as running the rest of the program
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
            {
                int width = grid.Width;
                int height = grid.Height;
                int wallSize = _mazeGeneratorWindowViewModel.WallSize;

                //Creates the maze path with the pre-created data
                string mazePath = PathHelper.GetMazeGeneratorPath(grid, wallSize);
                _mazeGeneratorWindowViewModel.MazePathData = Geometry.Parse(mazePath);
            });
        }

        private void DisplayHighlight(MazeLibrary.Grid grid, List<Cell> highlightCells)
        {
            //Runs this code at the same time as running the rest of the program
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
            {
                int width = _grid.Width;
                int height = _grid.Height;
                int wallSize = _mazeGeneratorWindowViewModel.WallSize;
                int wallThickness = _mazeGeneratorWindowViewModel.WallThickness;

                //Creates a highlight at each of the passed cells
                string highlightCellPath = PathHelper.GetHighlightCellPath(wallSize, wallThickness, highlightCells);
                _mazeGeneratorWindowViewModel.HighlightPathData = Geometry.Parse(highlightCellPath);
            });
        }

        private double GetComplexity(MazeLibrary.Grid _grid)
        {
            double onePassageWay = 0;
            double twoPassageWays = 0;
            double threePassageWays = 0;
            double fourPassageWays = 0;

            for (int y = 0; y < _grid.Height; y++)
            {
                for (int x = 0; x < _grid.Width; x++)
                {
                    int passageCounter = 0;
                    Cell currentCell = _grid.GetCell(x, y);
                    for (int i = 0; i < 4; i++)
                    {
                        CellEdge cellEdge = currentCell.GetEdge((Direction)i);
                        if (cellEdge is CellPassage)
                        {
                            passageCounter++;
                        }
                    }
                    switch (passageCounter)
                    {
                        case 1:
                            onePassageWay++;
                            break;
                        case 2:
                            twoPassageWays++;
                            break;
                        case 3:
                            threePassageWays++;
                            break;
                        case 4:
                            fourPassageWays++;
                            break;
                    }
                }
            }

            double value = ((twoPassageWays + threePassageWays + fourPassageWays) / onePassageWay); //lowness is goodness
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private void txtbxMazeWidth_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Handles key presses which are allowed in the textboxes. This is the form of validating input I chose to implement
            e.Handled = true;

            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.Back:
                case Key.Tab:
                case Key.Left:
                case Key.Right:
                case Key.Delete:
                    e.Handled = false;
                    break;
            }
        }

        private void txtbxMazeHeight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Handles key presses which are allowed in the textboxes. This is the form of validating input I chose to implement
            e.Handled = true;

            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.Back:
                case Key.Tab:
                case Key.Left:
                case Key.Right:
                case Key.Delete:
                    e.Handled = false;
                    break;
            }
        }

        private void btnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            //Saves the currently displayed maze as an image in the SaveImageHelper class
            SaveImageHelper.SavePngImage(generatorCanvas, "Generated");
        }

        private void btnSaveGridAsMazFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string applicationFolder = "Mazer Save Files";
            //Gets the path for the myDocuments path for whatever computer this program is on.
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //combines these paths
            string applicationPath = System.IO.Path.Combine(myDocuments, applicationFolder, "Generated Mazes (.maz)");
            //Sees if this path already exists
            if (!Directory.Exists(applicationPath))
            {
                Directory.CreateDirectory(applicationPath);
            }
            //All this provides the usual "Save As" function of any microsoft application.
            saveFileDialog.Filter = "maz (*.maz)|*.maz|All Files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.Title = "Save Mazer text File";
            saveFileDialog.InitialDirectory = applicationPath;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.DefaultExt = "maz";

            //dialogResult can be true, false or null (just in case the dialog returns null if it is closed, which would otherwise chrash the program at this line)
            bool? dialogResult = saveFileDialog.ShowDialog();

            //If what the user enters is Save (OK) will save the file.
            if (dialogResult == true)
            {
                //Using is here to close the stream writer in case if forget to close it as a good example of defensive coding
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, true))
                {
                    //Writes the first line of the text file with the grid's name, width and height. It seporates 
                    //these with vertical bars so the reader in the solver window can split on each part
                    writer.WriteLine(String.Format("{0}|{1}|{2}", _grid.Name, _grid.Width, _grid.Height));

                    //Steps through every cell...
                    for (int y = 0; y < _grid.Height; y++)
                    {
                        for (int x = 0; x < _grid.Width; x++)
                        {
                            Cell currentCell = _grid.GetCell(x, y);


                            //CellEdge edgeNorth = currentCell.GetEdge(Direction.North);
                            //CellEdge edgeEast = currentCell.GetEdge(Direction.East);
                            //CellEdge edgeSouth = currentCell.GetEdge(Direction.South);
                            //CellEdge edgeWest = currentCell.GetEdge(Direction.West);

                            //...getting each edge and...
                            CellEdge[] currentCellEdges = new CellEdge[] { currentCell.GetEdge(Direction.North),
                        currentCell.GetEdge(Direction.East),                                   
                        currentCell.GetEdge(Direction.South),                                  
                        currentCell.GetEdge(Direction.West)};

                            //...determines what edge type they are, writing the correct string in to represent that spot
                            StringBuilder line = new StringBuilder();
                            foreach (CellEdge ce in currentCellEdges)
                            {
                                if (ce is CellPassage)
                                {
                                    writer.Write("P");
                                }
                                else if (ce is CellWall)
                                {
                                    writer.Write("W");
                                }
                                else
                                {
                                    writer.Write("-");
                                }
                            }

                            //Every four strings added starts a new line
                            writer.WriteLine();
                        }
                    }
                }
            }
        }

        private void generatorCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Code for zooming into the canvas with the mousewheel
            _mazeGeneratorWindowViewModel.MouseWheel += (e.Delta / 100);
        }

        private void btnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            //Adds each of the used algorithm's names and times to the algorithmMetric
            Dictionary<string, long> data = new Dictionary<string, long>();
            foreach (AlgorithmMetric algorithmMetric in _mazeGeneratorWindowViewModel.AlgorithmMetricCollection)
            {
                data.Add(algorithmMetric.AlgorithmName, algorithmMetric.ElapsedMilliseconds);
            }

            //Displays the results window with this data and the title of which window it came from
            ResultsWindow resultsWindow = new ResultsWindow();
            resultsWindow.Owner = this;
            resultsWindow.ResultsWindowViewModel.Data = data;
            resultsWindow.ResultsWindowViewModel.SourceName = "Generator Results";
            resultsWindow.ShowDialog();
        }

        private void Row_ClickedGenerator(object sender, MouseButtonEventArgs e)
        {
            //Code for getting the correct index in _gridStorage corrasponding to the index of the row clicked
            DataGridRow row = (DataGridRow)ItemsControl.ContainerFromElement((DataGrid)sender, (DependencyObject)e.OriginalSource);

            //If nothing is clicked do nothing
            if (row == null) return;

            //_grid gets the required maze grid
            _grid = _gridStorage[row.GetIndex()];

            //Displays the grid's name
            _mazeGeneratorWindowViewModel.AlgorithimLabel = string.Format("Generating ({0})", _grid.Name);

            //Displays the new grid
            DisplayMaze(_grid);

        }
    }
}
