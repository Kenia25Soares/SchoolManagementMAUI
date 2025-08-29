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
    public partial class SubjectsListViewModel : ObservableObject
    {
        private readonly IPublicCatalogService _service;

        [ObservableProperty] private bool isBusy;
        public ObservableCollection<PublicSubject> Subjects { get; } = new();

        public SubjectsListViewModel(IPublicCatalogService service)
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
                Subjects.Clear();
                var list = await _service.GetSubjectsAsync();
                foreach (var s in list) Subjects.Add(s);
            }
            finally { IsBusy = false; }
        }
    }
}

