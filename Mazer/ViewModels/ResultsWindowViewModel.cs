using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazer.ViewModels
{
    public class ResultsWindowViewModel : INotifyPropertyChanged
    {
        //INotifyPropertyChanged is inherited so all the properties in this viewModel can be automatically updated when they change,
        //e.g. a new time has been recorded for an algorithm

        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, long> _data;
        private string _sourceName;

        public Dictionary<string, long> Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    NotifyPropertyChanged("Data");
                }
            }
        }

        public string SourceName
        {
            get { return _sourceName; }
            set
            {
                if (_sourceName != value)
                {
                    _sourceName = value;
                    NotifyPropertyChanged("SourceName");
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
