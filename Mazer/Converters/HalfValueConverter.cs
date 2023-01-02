using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Mazer.Converters
{
    public class HalfValueConverter : IMultiValueConverter
    {
        //The class here is used to always center the gridline in the center of the canvas.
        //In the WPF code, by giving it the canvas' width or height and the grid's width or height it calculates 
        //the indent required to move the grid to the center of the screen, no matter how the window is moved or resized
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Only centers it if the values are not null, there are two of them and they are set to any value
            //other than the default value for the DependencyProperty
            if (values != null && values.Length == 2 && values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue)
            {
                double canvasLength = (double)values[0];
                double mazeLength = (double)values[1];

                //Here it returns the new position indent, calculated the same for the horizontal or vertical plane
                return (canvasLength - mazeLength) / 2;
            }

            //returns the default value for the DependencyProperty should the above if statement be false, 
            //I will then see there is a problem and can correct it
            return DependencyProperty.UnsetValue;
        }

        //ConvertBack function required for the use of IMultiValueConverter. There is no need to use it in this
        //program though so it will be unused
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
