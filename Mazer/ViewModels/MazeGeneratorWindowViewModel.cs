using Mazer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mazer.ViewModels
{
    public class MazeGeneratorWindowViewModel : INotifyPropertyChanged
    {
        //INotifyPropertyChanged is inherited so all the properties in this viewModel can be automatically updated when they change,
        //e.g. algorithm check box is ticked then unticked

        public event PropertyChangedEventHandler PropertyChanged;

        private int _width;
        private int _height;
        private int _wallSize;
        private int _wallThickness;
        private int _delay;
        private Geometry _guidePathData;
        private Geometry _highlightPathData;
        private Geometry _mazePathData;
        private int _mouseWheel;
        private bool _hideHighlightSelected;
        private bool _hideWallAndPathUpdatesSelected;
        private bool _hideGridLinesSelected;
        private string _algorithimLabel;

        public MazeGeneratorWindowViewModel()
        {
            _algorithmMetricCollection = new ObservableCollection<AlgorithmMetric>();
        }

        //Property for building the results histogram
        private ObservableCollection<AlgorithmMetric> _algorithmMetricCollection;

        public ObservableCollection<AlgorithmMetric> AlgorithmMetricCollection
        {
            get { return _algorithmMetricCollection; }
            set
            {
                if (_algorithmMetricCollection != value)
                {
                    _algorithmMetricCollection = value;
                    NotifyPropertyChanged("AlgorithmMetricCollection");
                }
            }
        }

        public int MouseWheel
        {
            get { return _mouseWheel; }
            set
            {
                if (_mouseWheel != value)
                {
                    _mouseWheel = value;
                    NotifyPropertyChanged("MouseWheel");
                }
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    NotifyPropertyChanged("Width");
                }
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    NotifyPropertyChanged("Height");
                }
            }
        }

        public int WallSize
        {
            get { return _wallSize; }
            set
            {
                if (_wallSize != value)
                {
                    _wallSize = value;
                    NotifyPropertyChanged("WallSize");
                }
            }
        }

        public int WallThickness
        {
            get { return _wallThickness; }
            set
            {
                if (_wallThickness != value)
                {
                    _wallThickness = value;
                    NotifyPropertyChanged("WallThickness");
                }
            }
        }

        public int Delay
        {
            get { return _delay; }
            set
            {
                if (_delay != value)
                {
                    _delay = value;
                    NotifyPropertyChanged("Delay");
                }
            }
        }

        public Geometry GuidePathData
        {
            get { return _guidePathData; }
            set
            {
                if (_guidePathData != value)
                {
                    _guidePathData = value;
                    NotifyPropertyChanged("GuidePathData");
                }
            }
        }

        public Geometry HighlightPathData
        {
            get { return _highlightPathData; }
            set
            {
                if (_highlightPathData != value)
                {
                    _highlightPathData = value;
                    NotifyPropertyChanged("HighlightPathData");
                }
            }
        }

        public Geometry MazePathData
        {
            get { return _mazePathData; }
            set
            {
                if (_mazePathData != value)
                {
                    _mazePathData = value;
                    NotifyPropertyChanged("MazePathData");
                }
            }
        }

        public bool HideHighlightSelected
        {
            get { return _hideHighlightSelected; }
            set
            {
                if (_hideHighlightSelected != value)
                {
                    _hideHighlightSelected = value;
                    NotifyPropertyChanged("HideHighlightSelected");
                }
            }
        }

        public bool HideWallAndPathUpdatesSelected
        {
            get { return _hideWallAndPathUpdatesSelected; }
            set
            {
                if (_hideWallAndPathUpdatesSelected != value)
                {
                    _hideWallAndPathUpdatesSelected = value;
                    NotifyPropertyChanged("HideWallAndPathUpdatesSelected");
                }
            }
        }

        public bool HideGridLinesSelected
        {
            get { return _hideGridLinesSelected; }
            set
            {
                if (_hideGridLinesSelected != value)
                {
                    _hideGridLinesSelected = value;
                    NotifyPropertyChanged("HideGridLinesSelected");
                }
            }
        }

        public string AlgorithimLabel
        {
            get { return _algorithimLabel; }
            set
            {
                if (_algorithimLabel != value)
                {
                    _algorithimLabel = value;
                    NotifyPropertyChanged("AlgorithimLabel");
                }
            }
        }

        //Procedure tells the specific property variable to update it's stored value as it has been changed
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
