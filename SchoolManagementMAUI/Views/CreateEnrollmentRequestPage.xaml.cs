using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class CreateEnrollmentRequestPage : ContentPage
{
    private readonly CreateEnrollmentRequestViewModel _viewModel;

    public CreateEnrollmentRequestPage(CreateEnrollmentRequestViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public void Initialize(AvailableSubjectSummary subject)
    {
        _viewModel.SetSelectedSubject(subject);
    }
}