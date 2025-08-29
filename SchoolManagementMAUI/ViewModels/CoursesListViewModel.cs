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
    public partial class CourseWithSubjects
    {
        public Course Course { get; set; }
        public ObservableCollection<PublicSubject> Subjects { get; set; } = new();
        public bool IsExpanded { get; set; } = false;
    }

    public partial class CoursesListViewModel : ObservableObject
    {
        private readonly IPublicCatalogService _service;

        [ObservableProperty] private bool isBusy;
        public ObservableCollection<CourseWithSubjects> CoursesWithSubjects { get; } = new();

        public CoursesListViewModel(IPublicCatalogService service)
        {
            _service = service;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                CoursesWithSubjects.Clear();
                var courses = await _service.GetCoursesAsync();
                foreach (var course in courses)
                {
                    var courseWithSubjects = new CourseWithSubjects { Course = course };
                    var subjects = await _service.GetCourseSubjectsAsync(course.Id);
                    foreach (var subject in subjects)
                    {
                        courseWithSubjects.Subjects.Add(subject);
                    }
                    CoursesWithSubjects.Add(courseWithSubjects);
                }
            }
            finally { IsBusy = false; }
        }
    }
}
