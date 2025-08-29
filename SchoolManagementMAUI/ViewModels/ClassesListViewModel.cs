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
    public partial class CourseWithClasses
    {
        public Course Course { get; set; }
        public ObservableCollection<StudentClass> Classes { get; set; } = new();
    }

    public partial class ClassesListViewModel : ObservableObject
    {
        private readonly IPublicCatalogService _service;

        [ObservableProperty] private bool isBusy;
        public ObservableCollection<CourseWithClasses> CoursesWithClasses { get; } = new();

        public ClassesListViewModel(IPublicCatalogService service)
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
                CoursesWithClasses.Clear();

                // Get all courses first
                var courses = await _service.GetCoursesAsync();
                var courseDict = courses.ToDictionary(c => c.Id, c => c);

                // Get all classes
                var allClasses = await _service.GetClassesAsync();

                // Group classes by course
                var groupedClasses = allClasses.GroupBy(c => c.CourseId);

                foreach (var group in groupedClasses)
                {
                    var courseId = group.Key;
                    if (courseDict.TryGetValue(courseId, out var course))
                    {
                        var courseWithClasses = new CourseWithClasses { Course = course };
                        foreach (var cls in group)
                        {
                            courseWithClasses.Classes.Add(cls);
                        }
                        CoursesWithClasses.Add(courseWithClasses);
                    }
                }
            }
            finally { IsBusy = false; }
        }
    }
}