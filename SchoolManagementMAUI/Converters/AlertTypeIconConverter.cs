using SchoolManagementMAUI.Models;
using System.Globalization;

namespace SchoolManagementMAUI.Converters
{
 
    public class AlertTypeIconConverter : IValueConverter
    {
        private static readonly Dictionary<string, string> FontIconMapping = new() // FontAwesome Unicode
        {
            // Alertas principais 
            { "GradePosted", "\uf15c" },      // fa-file-text
            { "AddedToClass", "\uf19d" },     // fa-graduation-cap
            { "StatusChanged", "\uf021" },    // fa-refresh
            { "RemovedFromClass", "\uf503" }, // fa-user-minus
            { "ClassClosed", "\uf023" },      // fa-lock
            { "ExcludedByAbsences", "\uf071" }, // fa-exclamation-triangle
            { "GeneralNotification", "\uf0f3" }, // fa-bell padrão
            
            // Status de aprovação/reprovação
            { "Approved", "\uf00c" },         // fa-check
            { "Failed", "\uf00d" }            // fa-times
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string alertType)
            {
                return FontIconMapping.TryGetValue(alertType, out var icon)
                    ? icon
                    : "\uf0f3"; // fa-bell (padrão)
            }

            if (value is AlertType enumAlertType)
            {
                return FontIconMapping.TryGetValue(enumAlertType.ToString(), out var icon)
                    ? icon
                    : "\uf0f3"; 
            }

            return "\uf0f3"; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
