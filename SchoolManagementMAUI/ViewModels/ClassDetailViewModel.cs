using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class ClassDetailViewModel : ObservableObject
    {
        private readonly IPublicCatalogService _service;

        [ObservableProperty] private int classId;
        [ObservableProperty] private string? className;
        [ObservableProperty] private int courseId;
        [ObservableProperty] private string? courseName;
        [ObservableProperty] private bool isBusy;

        public ObservableCollection<PublicSubject> Subjects { get; } = new();

        public ClassDetailViewModel(IPublicCatalogService service)
        {
            _service = service;
        }

        public void Initialize(int classId, int courseId, string? className = null, string? courseName = null)
        {
            ClassId = classId;
            CourseId = courseId;
            ClassName = className;
            CourseName = courseName;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                Subjects.Clear();
                if (string.IsNullOrWhiteSpace(ClassName))
                {
                    var cls = await _service.GetClassAsync(ClassId);
                    ClassName = cls?.Name ?? $"Class {ClassId}";
                }
                if (string.IsNullOrWhiteSpace(CourseName))
                {
                    var course = await _service.GetCourseAsync(CourseId);
                    CourseName = course?.Name ?? $"Course {CourseId}";
                }
                var subs = await _service.GetCourseSubjectsAsync(CourseId);
                foreach (var s in subs) Subjects.Add(s);
            }
            finally { IsBusy = false; }
        }
    }
}
