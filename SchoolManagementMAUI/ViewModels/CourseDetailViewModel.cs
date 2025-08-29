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
    public partial class CourseDetailViewModel : ObservableObject
    {
        private readonly IPublicCatalogService _service;

        [ObservableProperty] private int courseId;
        [ObservableProperty] private string? courseName;
        [ObservableProperty] private bool isBusy;

        public ObservableCollection<PublicSubject> Subjects { get; } = new();
        public ObservableCollection<StudentClass> Classes { get; } = new();

        public CourseDetailViewModel(IPublicCatalogService service)
        {
            _service = service;
        }

        public void Initialize(int courseId, string? courseName = null)
        {
            CourseId = courseId;
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
                Classes.Clear();
                if (string.IsNullOrWhiteSpace(CourseName))
                {
                    var course = await _service.GetCourseAsync(CourseId);
                    CourseName = course?.Name ?? $"Course {CourseId}";
                }
                var subs = await _service.GetCourseSubjectsAsync(CourseId);
                foreach (var s in subs) Subjects.Add(s);
                var cls = await _service.GetClassesAsync(CourseId);
                foreach (var c in cls) Classes.Add(c);
            }
            finally { IsBusy = false; }
        }
    }
}
