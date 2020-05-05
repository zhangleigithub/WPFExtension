using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFExtension.Converter
{
    /// <summary>
    /// HorizontalScrollBarValueConverter
    /// </summary>
    public class HorizontalScrollBarValueConverter : IValueConverter
    {
        /// <summary>
        /// 源到目标
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns>目标值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * -1;
        }

        /// <summary>
        /// 目标到源
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns>源值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * -1;
        }
    }
}
