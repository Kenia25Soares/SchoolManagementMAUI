using SchoolManagementMAUI.Models;

namespace SchoolManagementMAUI.Services.Interface
{
    public interface IEnrollmentService
    {
        Task<List<AvailableSubjectSummary>> GetAvailableSubjectsAsync(string studentId, string token);
        Task<bool> CreateEnrollmentRequestAsync(CreateEnrollmentRequest request, string token);
        Task<List<EnrollmentRequestSummary>> GetMyRequestsAsync(string studentId, string token);
    }
}
