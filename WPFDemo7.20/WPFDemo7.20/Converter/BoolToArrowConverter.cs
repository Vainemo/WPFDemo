using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFDemo7._20.Converter
{
    public class BoolToArrowConverter : IValueConverter
    {
        //模型向界面转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value!=null&&bool.Parse(value.ToString()))
            {
                return "⬆";
            }
            return "⬇";
        }

        //界面向模型转换
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
