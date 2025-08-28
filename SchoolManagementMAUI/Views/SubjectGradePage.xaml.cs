using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class SubjectGradePage : ContentPage
{
    private readonly SubjectGradeViewModel _viewModel;

    public SubjectGradePage(SubjectGradeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Busca os parâmetros na URL
        var parameters = Shell.Current.CurrentState.Location.ToString();
        if (parameters.Contains("subjectCode=") && parameters.Contains("subjectName="))
        {
            // Extrai os parâmetros da URL
            var subjectCode = ExtractParameter(parameters, "subjectCode");
            var subjectName = ExtractParameter(parameters, "subjectName");

            if (!string.IsNullOrEmpty(subjectCode) && !string.IsNullOrEmpty(subjectName))
            {
                _viewModel.SetSubject(subjectCode, subjectName);
            }
        }
    }

    private string ExtractParameter(string url, string paramName)
    {
        var startIndex = url.IndexOf($"{paramName}=");
        if (startIndex == -1) return string.Empty;

        startIndex += paramName.Length + 1;
        var endIndex = url.IndexOf("&", startIndex);
        if (endIndex == -1) endIndex = url.Length;

        return url.Substring(startIndex, endIndex - startIndex);
    }
}