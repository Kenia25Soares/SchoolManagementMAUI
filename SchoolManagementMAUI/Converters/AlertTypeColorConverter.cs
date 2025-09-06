using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SchoolManagementMAUI.Converters
{
    public class AlertTypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string alertType)
            {
                return alertType switch
                {
                    // Alertas principais 
                    "GradePosted" => Colors.Green,
                    "AddedToClass" => Colors.Blue,
                    "StatusChanged" => Colors.Orange,
                    "RemovedFromClass" => Colors.Orange,
                    "ClassClosed" => Colors.Gray,
                    "ExcludedByAbsences" => Colors.Red,
                    "GeneralNotification" => Colors.Gray,
                    
                    // Status de aprovação/reprovação
                    "Approved" => Colors.Green,
                    "Failed" => Colors.Red,
                    
                    // Padrão para tipos não reconhecidos
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}