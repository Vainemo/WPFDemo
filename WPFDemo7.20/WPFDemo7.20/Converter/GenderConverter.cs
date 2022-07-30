using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFDemo7._20.Converter
{
  public  class GenderConverter : IValueConverter
    {
        //提供将自定义逻辑应用于绑定的方法。
        //数据-->UI
        //将数据层的数据显示到UI层时做数据类型转换
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null||parameter==null)
            {
                return false;
            }
            return value.ToString() == parameter.ToString();

        }
        //界面点击后所触发的方法
        //UI-->数据
        //返回UI中设置的参数值
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
