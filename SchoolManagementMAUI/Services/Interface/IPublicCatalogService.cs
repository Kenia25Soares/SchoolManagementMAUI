using SchoolManagementMAUI.Models;

namespace SchoolManagementMAUI.Services.Interface
{
    public interface IPublicCatalogService
    {
        Task<IReadOnlyList<Course>> GetCoursesAsync();
        Task<IReadOnlyList<StudentClass>> GetClassesAsync(int? courseId = null, string? year = null, string? shift = null);
        Task<IReadOnlyList<PublicSubject>> GetSubjectsAsync();
        Task<IReadOnlyList<PublicSubject>> GetCourseSubjectsAsync(int courseId);
        Task<Course?> GetCourseAsync(int id);
        Task<StudentClass?> GetClassAsync(int id);
    }
}
