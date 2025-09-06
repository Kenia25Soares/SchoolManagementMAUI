using SchoolManagementMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services.Interface
{
    public interface IAlertsService
    {
        Task<AlertsResponse?> GetAllAlertsAsync(string studentId, string token);
        Task<AlertsResponse?> GetUnreadAlertsAsync(string studentId, string token);
        Task<AlertsResponse?> GetRecentAlertsAsync(string studentId, int count, string token);
        Task<UnreadCountResponse?> GetUnreadCountAsync(string studentId, string token);
        Task<bool> MarkAlertAsReadAsync(int alertId, string token);
        Task<bool> MarkMultipleAlertsAsReadAsync(List<int> alertIds, string token);
    }
}
