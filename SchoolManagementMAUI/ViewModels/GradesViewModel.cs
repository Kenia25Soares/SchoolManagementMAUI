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
    public partial class GradesViewModel : ObservableObject
    {
        private readonly IGradesService _gradesService;
        private readonly IUserSession _userSession;

        public GradesViewModel(IGradesService gradesService, IUserSession userSession)
        {
            _gradesService = gradesService;
            _userSession = userSession;
        }

        [ObservableProperty]
        private ObservableCollection<Grade> grades = new();

        [ObservableProperty]
        private string studentName;

        [ObservableProperty]
        private double totalAverage;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool isClassClosed;

        [RelayCommand]
        public async Task LoadGradesAsync()
        {
            if (!_userSession.IsLoggedIn)
            {
                Message = "Você precisa estar autenticado.";
                return;
            }

            var studentId = _userSession.CurrentUser?.Id;
            var token = _userSession.CurrentUser?.Token;

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(token))
            {
                Message = "Token ou ID inválido.";
                return;
            }

            try
            {
                var response = await _gradesService.GetGradesAsync(studentId, token);

                if (response == null || response.Count == 0)
                {
                    Message = "Nenhuma nota encontrada.";
                    Grades = new ObservableCollection<Grade>();
                    return;
                }

                Grades = new ObservableCollection<Grade>(response);

                StudentName = _userSession.CurrentUser.FullName;
                TotalAverage = Grades.Average(g => g.WeightedAverage);
                IsClassClosed = Grades.Any(g => g.FailedDueToAbsences);
                Message = string.Empty;
            }
            catch (Exception ex)
            {
                Message = "Erro ao carregar as notas.";
                Console.WriteLine(ex.Message);
            }
        }
    }
}
