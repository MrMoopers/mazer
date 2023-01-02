using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazer.Model
{
    public class AlgorithmMetric : INotifyPropertyChanged
    {
        //Class for results histogram window. It requires time an algorithm name and a complexity.
        //INotifyPropertyChanged is inherited so these properties will update if they are ever changed in the running of the program
        public event PropertyChangedEventHandler PropertyChanged;

        private TimeSpan _timeSpan = new TimeSpan();
        private string _algorithmName;
        private long _elapsedMilliseconds;
        private double _relativeComplexity;

        public TimeSpan TimeSpan
        {
            get { return _timeSpan; }
            set
            {
                if (_timeSpan != value)
                {
                    _timeSpan = value;
                    NotifyPropertyChanged("TimeSpan");
                }
            }
        }

        public string AlgorithmName
        {
            get { return _algorithmName; }
            set
            {
                if (_algorithmName != value)
                {
                    _algorithmName = value;
                    NotifyPropertyChanged("AlgorithmName");
                }
            }
        }

        public long ElapsedMilliseconds
        {
            get { return _elapsedMilliseconds; }
            set
            {
                if (_elapsedMilliseconds != value)
                {
                    _elapsedMilliseconds = value;
                    NotifyPropertyChanged("ElapsedMilliseconds");
                }
            }
        }

        public double RelativeComplexity
        {
            get { return _relativeComplexity; }
            set
            {
                if (_relativeComplexity != value)
                {
                    _relativeComplexity = value;
                    NotifyPropertyChanged("RelativeComplexity");
                }
            }
        }

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
