using MazeLibrary;
using MazeLibrary.Algorithms;
using MazeLibrary.Events;
using Mazer.Helpers;
using Mazer.Model;
using Mazer.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace Mazer
{
    /// <summary>
    /// Interaction logic for MazeSolverWindow.xaml
    /// </summary>
    public partial class MazeSolverWindow : Window
    {
        //_route stores the latest route and _grid stores it's corrasponding grid
        private MazeSolverWindowViewModel _mazeSolverWindowViewModel;
        private MazeLibrary.Grid _grid = null;
        private List<Cell> _route = null;
        private List<List<Cell>> _routeStorage = new List<List<Cell>>();
        private List<List<Cell>> _highlightStorage = new List<List<Cell>>();

        public MazeSolverWindow()
        {
            InitializeComponent();

            _mazeSolverWindowViewModel = new MazeSolverWindowViewModel();
            DataContext = _mazeSolverWindowViewModel;

            //Setting up buttons that require a route through the maze to be inactive before a route has been found. 
            btnSaveAsMaz.IsEnabled = false;
            btnSaveImage.IsEnabled = false;
            btnSaveResults.IsEnabled = false;
            btnSolve.IsEnabled = false;


            //Many algorithm boxes are inactive as they haven't been implemented yet
            BinaryTreeBox.IsEnabled = false;
            HuntandKillBox.IsEnabled = false;
            KruskalsBox.IsEnabled = false;
            PrimsBox.IsEnabled = false;
            RecursiveDivisionBox.IsEnabled = false;
            SidewinderBox.IsEnabled = false;
            SimpleRectangleBox.IsEnabled = false;
            WilsonsAlgorithmBox.IsEnabled = false;

            hideGridLinesBox.IsChecked = true;
            _mazeSolverWindowViewModel.HideGridLinesSelected = true;

            //Set up initial values for the ui window
            _mazeSolverWindowViewModel.Delay = 1;
            _mazeSolverWindowViewModel.WallThickness = 2;
            _mazeSolverWindowViewModel.WallSize = 25;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //Closes window and returns control to the startup screen
            Close();
        }

        private async void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            _routeStorage.Clear();
            _highlightStorage.Clear();

            //Clears previous algorithm and time data stored in the AlgorithmMetricCollection list
            _mazeSolverWindowViewModel.AlgorithmMetricCollection.Clear();

            //...
            ClearHighlight();


            //delay equals the Delay in the view model
            int delay = _mazeSolverWindowViewModel.Delay;

            //Setting buttons to be inactive while the program is busy solving a maze
            sldDelay.IsEnabled = false;
            txtDelay.IsEnabled = false;
            btnSolve.IsEnabled = false;
            btnSaveImage.IsEnabled = false;
            btnSaveAsMaz.IsEnabled = false;
            btnSaveResults.IsEnabled = false;

            hideGridLinesBox.IsEnabled = false;
            hideHighlightBox.IsEnabled = false;

            //Budges the loaded maze so it will fit onto the canvas completely for most reasonable mazes
            _mazeSolverWindowViewModel.WallSize = 25;
            while (_mazeSolverWindowViewModel.WallSize * _grid.Width >= solverCanvas.ActualWidth ||
                   _mazeSolverWindowViewModel.WallSize * _grid.Height >= solverCanvas.ActualHeight)
            {
                _mazeSolverWindowViewModel.WallSize--;
                if (_mazeSolverWindowViewModel.WallSize <= 1)
                {
                    break;
                }
            }

            //Sets all timing data to null for the new run
            AlgorithmMetric aStarAlgorithmAlgorithmMetric = null;
            AlgorithmMetric binaryTreeAlgorithmAlgorithmMetric = null;
            AlgorithmMetric drunkardsWalkAlgorithmAlgorithmMetric = null;
            AlgorithmMetric huntAndKillAlgorithmAlgorithmMetric = null;
            AlgorithmMetric kruskalsAlgorithmAlgorithmMetric = null;
            AlgorithmMetric primsAlgorithmAlgorithmMetric = null;
            AlgorithmMetric recursiveBackTrackerAlgorithmAlgorithmMetric = null;
            AlgorithmMetric recursiveDivisionAlgorithmAlgorithmMetric = null;
            AlgorithmMetric sidewinderAlgorithmAlgorithmMetric = null;
            AlgorithmMetric simpleRectangleAlgorithmAlgorithmMetric = null;
            AlgorithmMetric LeftWallFollowerAlgorithmAlgorithmMetric = null;
            AlgorithmMetric RightWallFollowerAlgorithmAlgorithmMetric = null;
            AlgorithmMetric wilsonsAlgorithmAlgorithmMetric = null;

            //Booleans store value in checkboxes so solving can't be confused if the user clicks a new algorithm 
            //box before it has finished making mazes
            bool doAStarAlgorithm = AStarBox.IsChecked.Value;
            bool doBinaryTreeAlgorithm = BinaryTreeBox.IsChecked.Value;
            bool doDrunkardsWalkAlgorithm = DrunkardsWalkBox.IsChecked.Value;
            bool doHuntAndKillAlgorithm = HuntandKillBox.IsChecked.Value;
            bool doKruskalsAlgorithm = KruskalsBox.IsChecked.Value;
            bool doPrimsAlgorithm = PrimsBox.IsChecked.Value;
            bool doRecursiveBackTrackerAlgorithm = RecursiveBackTrackerBox.IsChecked.Value;
            bool doRecursiveDivisionAlgorithm = RecursiveDivisionBox.IsChecked.Value;
            bool doSidewinderAlgorithm = SidewinderBox.IsChecked.Value;
            bool doSimpleRectangleAlgorithm = SimpleRectangleBox.IsChecked.Value;
            bool doLeftWallFollowerAlgorithm = LeftWallFollowerBox.IsChecked.Value;
            bool doRightWallFollowerAlgorithm = RightWallFollowerBox.IsChecked.Value;
            bool doWilsonsAlgorithm = WilsonsAlgorithmBox.IsChecked.Value;



            //New code to allow a solve start and end point to be selected. If no value is given, defaults to 0,0 and maxSize, maxSize
            Point tempPoint;
            if (solverStartCoordinate.Text == "Start x,y" || solverStartCoordinate.Text == "")
            {
                tempPoint = new Point(0, 0);
            }
            else
            {
                tempPoint = Point.Parse(solverStartCoordinate.Text);
            }


            System.Drawing.Point startCoordinate = new System.Drawing.Point((int)tempPoint.X, (int)tempPoint.Y);

          
            if (solverEndCoordinate.Text == "End x,y" || solverEndCoordinate.Text == "")
            {
                tempPoint = new Point(_grid.Width-1, _grid.Height-1);
            }
            else
            {
                tempPoint = Point.Parse(solverEndCoordinate.Text);
            }
            System.Drawing.Point endCoordinate = new System.Drawing.Point((int)tempPoint.X, (int)tempPoint.Y);

            //These cells are the ones in the algorithm that will be highlighted
            List<Cell> visitedCells = new List<Cell>();

            //Hides gridlines if checkbox is checked
            if (_mazeSolverWindowViewModel.HideGridLinesSelected)
            {
                GuidePath.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                GuidePath.Visibility = System.Windows.Visibility.Visible;
            }

            //Allows the displaying of a maze to be on one thread while the algorithms still create the maze on another. 
            //(Algorithms therefore run in the background on one thread, firing off events to update the ui on the main thread with new information) 
            await Task.Run(() =>
            {
                //The guide for the maze is always the same, as every maze is made with the same dimensions as is the maze so that is displayed as well

                DisplayGuide(_grid);
                DisplayMaze(_grid);

                Stopwatch stopwatch = new Stopwatch();

                //All algorithm's if-checked statements follow the same procedure. I will therefore only comment on the A* algorithm
                #region A*

                if (doAStarAlgorithm)
                {
                    //Starts a new route and displays it, therefore reseting any previous route
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (A*)");
                    //Stopwatch is reset and started and AStarAlgorithm is created with the required delay. Whenever a routeAddingNewCell event 
                    //is called in the code it creates a new handle for it on another thread. There it updates the ui canvas. After the _route has 
                    //been made the stopwatch is stopped
                    stopwatch.Reset();
                    stopwatch.Start();
                    AStarAlgorithm aStar = new AStarAlgorithm(delay);
                    aStar.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    if (!_mazeSolverWindowViewModel.HideHighlightSelected) aStar.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    //_route = aStar.Solve(_grid);
                    //aStar.Solve(ref _grid, out _route, out visitedCells);
                    aStar.Solve(ref _grid, out _route, out visitedCells, startCoordinate, endCoordinate);
                    stopwatch.Stop();

                    // _routeAndHighlightStorage.Add(_route, visitedCells);
                    _routeStorage.Add(_route);
                    _highlightStorage.Add(visitedCells);

                    //Creates and stores all data recorded for use in the creation of the histogram 
                    aStarAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "A*",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Binary Tree

                if (doBinaryTreeAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);

                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Binary Tree)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    BinaryTreeAlgorithm binaryTreeAlgorithm = new BinaryTreeAlgorithm(delay);
                    binaryTreeAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    _route = binaryTreeAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    ////_routeStorage.Add(_route);

                    binaryTreeAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "BinaryTree",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Drunkards Walk

                if (doDrunkardsWalkAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);

                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Drunkard's Walk)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    DrunkardsWalkAlgorithm drunkardsWalkAlgorithm = new DrunkardsWalkAlgorithm(delay);
                    drunkardsWalkAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    if (!_mazeSolverWindowViewModel.HideHighlightSelected) drunkardsWalkAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    // _route = drunkardsWalkAlgorithm.Solve(_grid);
                    drunkardsWalkAlgorithm.Solve(ref _grid, out _route, out visitedCells);
                    stopwatch.Stop();

                    _routeStorage.Add(_route);
                    _highlightStorage.Add(visitedCells);

                    drunkardsWalkAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "DrunkardsWalk",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Hunt and Kill

                if (doHuntAndKillAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Hunt & Kill)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    HuntAndKillAlgorithm huntAndKillAlgorithm = new HuntAndKillAlgorithm(delay);
                    huntAndKillAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    _route = huntAndKillAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    //_routeStorage.Add(_route);

                    huntAndKillAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "HuntandKill",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Kruskals

                if (doKruskalsAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Krustals)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    KruskalsAlgorithm kruskalsAlgorithm = new KruskalsAlgorithm(delay);
                    kruskalsAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    _route = kruskalsAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    //////_routeStorage.Add(_route);

                    kruskalsAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "Kruskals",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Prims

                if (doPrimsAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Prims)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    PrimsAlgorithm primsAlgorithm = new PrimsAlgorithm(delay);
                    primsAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    _route = primsAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    ////_routeStorage.Add(_route);

                    primsAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "Prims",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Recursive Back Tracker

                if (doRecursiveBackTrackerAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Recursive Backtracker)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    RecursiveBackTrackerAlgorithm recursiveBackTrackerAlgorithm = new RecursiveBackTrackerAlgorithm(delay);
                    recursiveBackTrackerAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    if (!_mazeSolverWindowViewModel.HideHighlightSelected) recursiveBackTrackerAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    //  _route = recursiveBackTrackerAlgorithm.Solve(_grid);
                    recursiveBackTrackerAlgorithm.Solve(ref _grid, out _route, out visitedCells);
                    //RecursiveBackTrackerAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    _routeStorage.Add(_route);
                    _highlightStorage.Add(visitedCells);

                    recursiveBackTrackerAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "RecursiveBackTracker",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Recursive Division

                if (doRecursiveDivisionAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Recursive Division)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    RecursiveDivisionAlgorithm recursiveDivisionAlgorithm = new RecursiveDivisionAlgorithm(delay);
                    recursiveDivisionAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    _route = recursiveDivisionAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    //_routeStorage.Add(_route);

                    recursiveDivisionAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "RecursiveDivision",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Sidewinder

                if (doSidewinderAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Sidewinder)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    SidewinderAlgorithm sidewinderAlgorithm = new SidewinderAlgorithm(delay);
                    sidewinderAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    _route = sidewinderAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    //_routeStorage.Add(_route);

                    sidewinderAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "Sidewinder",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region LeftWallFollower

                if (doLeftWallFollowerAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Left Wall Follower)");
                    stopwatch.Reset();
                    stopwatch.Start();
                    WallFollowerAlgorithm wallFollowerAlgorithm = new WallFollowerAlgorithm(delay);
                    wallFollowerAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    if (!_mazeSolverWindowViewModel.HideHighlightSelected) wallFollowerAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    // _route = wallFollowerAlgorithm.SolveLeft2(_grid);
                    wallFollowerAlgorithm.SolveLeft(ref _grid, out _route, out visitedCells);
                    stopwatch.Stop();

                    _routeStorage.Add(_route);
                    _highlightStorage.Add(visitedCells);

                    LeftWallFollowerAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "LeftWallFollower",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region RightWallFollower

                if (doRightWallFollowerAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Right Wall Follower)");
                    stopwatch.Reset();
                    stopwatch.Start();
                    WallFollowerAlgorithm wallFollowerAlgorithm = new WallFollowerAlgorithm(delay);
                    wallFollowerAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    if (!_mazeSolverWindowViewModel.HideHighlightSelected) wallFollowerAlgorithm.RaiseHighlightPathAddedNewCellEvent += HandleHighlightCellEvent;
                    //_route = wallFollowerAlgorithm.SolveRight3(_grid);
                    wallFollowerAlgorithm.SolveRight(ref _grid, out _route, out visitedCells);
                    stopwatch.Stop();

                    _routeStorage.Add(_route);
                    _highlightStorage.Add(visitedCells);

                    RightWallFollowerAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "RightWallFollower",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Simple Rectangle

                if (doSimpleRectangleAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Simple Rectangle)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    SimpleRectangleAlgorithm simpleRectangleAlgorithm = new SimpleRectangleAlgorithm(delay);
                    simpleRectangleAlgorithm.RaiseRoutePathAddedNewCellEvent += HandleRoutePathAddingNewCellEvent;
                    _route = simpleRectangleAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    //_routeStorage.Add(_route);

                    simpleRectangleAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "SimpleRectangle",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                #region Wilsons

                if (doWilsonsAlgorithm)
                {
                    _route = new List<Cell>();
                    DisplayRoute(_grid, _route);
                    _mazeSolverWindowViewModel.AlgorithimLabel = string.Format("Solving (Wilsons)");

                    stopwatch.Reset();
                    stopwatch.Start();
                    WilsonsAlgorithm.Solve(_grid);
                    stopwatch.Stop();

                    //_routeStorage.Add(_route);

                    wilsonsAlgorithmAlgorithmMetric = new AlgorithmMetric
                    {
                        AlgorithmName = "WilsonsAlgorithm",
                        TimeSpan = stopwatch.Elapsed,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    };
                }

                #endregion

                _mazeSolverWindowViewModel.AlgorithimLabel = "";

                //ClearHighlight();
                //To ensure a route is always displayed I force another diplaying of the latest grid here
                DisplayRoute(_grid, _route);
            });

            //Adds only the algorithm times from the algorithms that created a maze to an AlgorithmMetricCollection as a storage for all the algorithms used
            if (aStarAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(aStarAlgorithmAlgorithmMetric);
            }
            if (binaryTreeAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(binaryTreeAlgorithmAlgorithmMetric);
            }
            if (drunkardsWalkAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(drunkardsWalkAlgorithmAlgorithmMetric);
            }
            if (huntAndKillAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(huntAndKillAlgorithmAlgorithmMetric);
            }
            if (kruskalsAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(kruskalsAlgorithmAlgorithmMetric);
            }
            if (primsAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(primsAlgorithmAlgorithmMetric);
            }
            if (recursiveBackTrackerAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(recursiveBackTrackerAlgorithmAlgorithmMetric);
            }
            if (recursiveDivisionAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(recursiveDivisionAlgorithmAlgorithmMetric);
            }
            if (sidewinderAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(sidewinderAlgorithmAlgorithmMetric);
            }
            if (simpleRectangleAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(simpleRectangleAlgorithmAlgorithmMetric);
            }
            if (LeftWallFollowerAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(LeftWallFollowerAlgorithmAlgorithmMetric);
            }
            if (RightWallFollowerAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(RightWallFollowerAlgorithmAlgorithmMetric);
            }
            if (wilsonsAlgorithmAlgorithmMetric != null)
            {
                _mazeSolverWindowViewModel.AlgorithmMetricCollection.Add(wilsonsAlgorithmAlgorithmMetric);
            }

            //As everything has finished and there are now accessable routes all sliders and buttons are now enabled
            sldDelay.IsEnabled = true;
            txtDelay.IsEnabled = true;
            btnSolve.IsEnabled = true;
            btnSaveImage.IsEnabled = true;
            btnSaveAsMaz.IsEnabled = true;
            btnSaveResults.IsEnabled = true;

            hideGridLinesBox.IsEnabled = true;
            hideHighlightBox.IsEnabled = true;
        }



        private void ClearHighlight()
        {
            //Clears all highlight data
            _mazeSolverWindowViewModel.HighlightPathData = Geometry.Parse("");
        }

        void HandleRoutePathAddingNewCellEvent(object sender, RoutePathAddedNewCellEventArgs e)
        {
            //Displays the current _Route in whatever state of completeness it is in
            DisplayRoute(e.Grid, e.RouteCells);
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
                int wallSize = _mazeSolverWindowViewModel.WallSize;

                //Creates the guide path with the pre-created data
                string guidePath = PathHelper.GetGuidePath(width, height, wallSize);
                _mazeSolverWindowViewModel.GuidePathData = Geometry.Parse(guidePath);
            });
        }

        private void DisplayMaze(MazeLibrary.Grid grid)
        {
            //Runs this code at the same time as running the rest of the program
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
            {
                int width = grid.Width;
                int height = grid.Height;
                int wallSize = _mazeSolverWindowViewModel.WallSize;

                //Creates the maze path with the pre-created data and the current grid
                string mazePath = PathHelper.GetMazeGeneratorPath(grid, wallSize);
                _mazeSolverWindowViewModel.MazePathData = Geometry.Parse(mazePath);
            });
        }

        private void DisplayRoute(MazeLibrary.Grid grid, List<Cell> route)
        {
            //Runs this code at the same time as running the rest of the program
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
            {
                int width = grid.Width;
                int height = grid.Height;
                int wallSize = _mazeSolverWindowViewModel.WallSize;

                //Creates the route path with the pre-created data and the current route
                string routePath = PathHelper.GetMazeSolverPath(grid, _mazeSolverWindowViewModel.WallSize, route);
                _mazeSolverWindowViewModel.RoutePathData = Geometry.Parse(routePath);
            });
        }

        private void DisplayHighlight(MazeLibrary.Grid grid, List<Cell> highlightCells)
        {
            //Runs this code at the same time as running the rest of the program
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
            {
                int width = _grid.Width;
                int height = _grid.Height;
                int wallSize = _mazeSolverWindowViewModel.WallSize;
                int wallThickness = _mazeSolverWindowViewModel.WallThickness;

                //Creates a highlight at each of the passed cells
                string highlightCellPath = PathHelper.GetHighlightCellPath(wallSize, wallThickness, highlightCells);
                _mazeSolverWindowViewModel.HighlightPathData = Geometry.Parse(highlightCellPath);
            });
        }

        private void btnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            //Saves the currently displayed maze as an image in the SaveImageHelper class
            SaveImageHelper.SavePngImage(solverCanvas, "Solved");
        }

        private void btnOpenMaze_Click(object sender, RoutedEventArgs e)
        {
            _mazeSolverWindowViewModel.AlgorithmMetricCollection.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string applicationFolder = "Mazer Save Files";
            //Gets the path for the myDocuments path for whatever computer this program is on.
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //combines these paths
            string applicationPath = System.IO.Path.Combine(myDocuments, applicationFolder, "Generated Mazes (.maz)");
            //Sees if this path already exists
            if (!Directory.Exists(applicationPath))//?needed?
            {
                Directory.CreateDirectory(applicationPath);
            }
            //All this provides the usual "Save As" function of any microsoft application.
            openFileDialog.Filter = "maz (*.maz)|*.maz|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.Title = "Open Mazer text File";
            openFileDialog.InitialDirectory = applicationPath;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = "maz";

            //dialogResult can be true, false or null (just in case)
            bool? dialogResult = openFileDialog.ShowDialog();

            //If what the user enters is Save (OK) will save the file.
            if (dialogResult == true)
            {
                //_grid is collected using the filename from the function GetGridFromFile
                string filename = openFileDialog.FileName;
                _grid = MazeLibrary.Grid.GetGridFromFile(filename);

                //Sets dimensions of new grid
                _mazeSolverWindowViewModel.Width = _grid.Width;
                _mazeSolverWindowViewModel.Height = _grid.Height;

                //Budges the loaded maze so it will fit onto the canvas completely for most reasonable mazes
                _mazeSolverWindowViewModel.WallSize = 25;
                while (_mazeSolverWindowViewModel.WallSize * _grid.Width >= solverCanvas.ActualWidth ||
                       _mazeSolverWindowViewModel.WallSize * _grid.Height >= solverCanvas.ActualHeight)
                {
                    _mazeSolverWindowViewModel.WallSize--;
                    if (_mazeSolverWindowViewModel.WallSize <= 1) { break; }
                }


                //Checks if checkbox is checked or not, if so set guide path to be invisable
                if (_mazeSolverWindowViewModel.HideGridLinesSelected)
                {
                    GuidePath.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    GuidePath.Visibility = System.Windows.Visibility.Visible;
                }

                //Builds the guidePath and the mazePath using the dimensions and the new grid for the maze path
                string guidePath = PathHelper.GetGuidePath(_grid.Width, _grid.Height, _mazeSolverWindowViewModel.WallSize);
                _mazeSolverWindowViewModel.GuidePathData = Geometry.Parse(guidePath);
                string mazePath = PathHelper.GetMazeGeneratorPath(_grid, _mazeSolverWindowViewModel.WallSize);
                _mazeSolverWindowViewModel.MazePathData = Geometry.Parse(mazePath);

                //Reset route path data 
                _mazeSolverWindowViewModel.RoutePathData = Geometry.Parse("");

                //Re-enable buttons that can now be run now that there  is a route path
                btnSolve.IsEnabled = true;
                btnSaveImage.IsEnabled = false;
                btnSaveAsMaz.IsEnabled = false;
                btnSaveResults.IsEnabled = false;
            }
        }

        private void btnDeadEnds_Click(object sender, RoutedEventArgs e)
        {
            //Not used anymore
            throw new NotImplementedException();
        }

        private void SolverCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Code for zooming into the canvas with the mousewheel
            _mazeSolverWindowViewModel.MouseWheel += (e.Delta / 100);
        }

        private void btnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            //Adds each of the used algorithm's names and times to the algorithmMetric
            Dictionary<string, long> data = new Dictionary<string, long>();
            foreach (AlgorithmMetric algorithmMetric in _mazeSolverWindowViewModel.AlgorithmMetricCollection)
            {
                data.Add(algorithmMetric.AlgorithmName, algorithmMetric.ElapsedMilliseconds);
            }

            //Displays the results window with this data and the title of which window it came from
            ResultsWindow resultsWindow = new ResultsWindow();
            resultsWindow.Owner = this;
            resultsWindow.ResultsWindowViewModel.Data = data;
            resultsWindow.ResultsWindowViewModel.SourceName = "Solver Results";
            resultsWindow.ShowDialog();
        }

        private void Row_ClickedSolver(object sender, MouseButtonEventArgs e)
        {
            //Merp
            DataGridRow row = (DataGridRow)ItemsControl.ContainerFromElement((DataGrid)sender, (DependencyObject)e.OriginalSource);
            if (row == null) return;

            //Set the current route to the one at the desired index in _routeStorage and the highlight to that index as well
            int index = row.GetIndex();
            DisplayRoute(_grid, _routeStorage[index]);
            DisplayHighlight(_grid, _highlightStorage[index]);

        }
    }
}




