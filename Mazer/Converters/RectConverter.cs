using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Mazer.Converters
{
    public class RectConverter : IMultiValueConverter
    {
        //The class here is used to force correct clipping to the canvas when it is being zoomed,
        //if this rectangle for the border wasn't being returned on every change to the canvas size,
        //it would stretch the canvas over the other controls in the window
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Only returns a rectangle if the values are not null, there are two of them and they are set to any value
            //other than the default value for the DependencyProperty
            if (values != null && values.Length == 2 && values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue)
            {
                double width = (double)values[0];
                double height = (double)values[1];
                //Returns the new dimensions
                return new Rect(0, 0, width, height);
            }

            //If the above failed, returns the default value for the DependencyProperty so 
            //I will see there is a problem and then I will resolve it
            return DependencyProperty.UnsetValue;
        }

        //Again a ConvertBack function required for the use of IMultiValueConverter. There is no need to use it in this
        //program though so it will be unused
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
