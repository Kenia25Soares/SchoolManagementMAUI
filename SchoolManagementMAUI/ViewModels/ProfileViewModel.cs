using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IUserSession _userSession;
        private readonly IProfileService _profileService;

        [ObservableProperty] private string? fullName;
        [ObservableProperty] private string? phoneNumber;
        [ObservableProperty] private string? email;
        [ObservableProperty] private DateTime? dateOfBirth;
        [ObservableProperty] private string? address;
        [ObservableProperty] private string? profilePictureUrl;
        [ObservableProperty] private string? officialPhotoUrl;
        [ObservableProperty] private bool isBusy;

        private Stream? _newProfilePicture;
        private string? _newProfilePictureFileName;
        private string? _newProfilePictureContentType;
        private Stream? _newOfficialPhoto;
        private string? _newOfficialPhotoFileName;
        private string? _newOfficialPhotoContentType;

        public ProfileViewModel(IUserSession userSession, IProfileService profileService)
        {
            _userSession = userSession;
            _profileService = profileService;
        }


        [RelayCommand]
        public async Task LoadAsync()
        {
            if (_userSession.CurrentUser == null || string.IsNullOrEmpty(_userSession.CurrentUser.Token)) return;
            try
            {
                IsBusy = true;
                var profile = await _profileService.GetStudentProfileFullAsync(_userSession.CurrentUser.Token);
                if (profile != null)
                {
                    FullName = profile.FullName;
                    PhoneNumber = profile.PhoneNumber;
                    Email = profile.Email;
                    DateOfBirth = profile.DateOfBirth?.DateTime;
                    Address = profile.Address;
                    ProfilePictureUrl = profile.ProfilePictureFullUrl ?? profile.ProfilePictureUrl;
                    OfficialPhotoUrl = profile.OfficialPhotoFullUrl ?? profile.OfficialPhotoUrl;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task PickProfilePictureAsync()
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select profile picture",
                    FileTypes = FilePickerFileType.Images
                });
                if (file != null)
                {
                    _newProfilePicture = await file.OpenReadAsync();
                    _newProfilePictureFileName = file.FileName;
                    _newProfilePictureContentType = file.ContentType;
                }
            }
            catch { }
        }

        // Official photo is read-only in the app per requirements; no picker

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (_userSession.CurrentUser == null || string.IsNullOrEmpty(_userSession.CurrentUser.Token)) return;
            try
            {
                IsBusy = true;
                var data = new ProfileUpdateData
                {
                    FullName = FullName,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    DateOfBirth = DateOfBirth,
                    Address = Address,
                    ProfilePictureStream = _newProfilePicture,
                    ProfilePictureFileName = _newProfilePictureFileName,
                    ProfilePictureContentType = _newProfilePictureContentType,
                    OfficialPhotoStream = _newOfficialPhoto,
                    OfficialPhotoFileName = _newOfficialPhotoFileName,
                    OfficialPhotoContentType = _newOfficialPhotoContentType
                };
                var result = await _profileService.UpdateFullProfileAsync(data, _userSession.CurrentUser.Token);
                if (result.Success && result.User != null)
                {
                    // atualizar sessão 
                    _userSession.CurrentUser.FullName = result.User.FullName;
                    _userSession.CurrentUser.Email = result.User.Email;
                    _userSession.CurrentUser.PhoneNumber = result.User.PhoneNumber;
                    _userSession.CurrentUser.ProfilePictureUrl = result.User.ProfilePictureUrl;
                    _userSession.CurrentUser.ProfilePictureFullUrl = result.User.ProfilePictureFullUrl;
                    await Shell.Current.DisplayAlert("Success", "Profile updated successfully.", "OK");
                    await Shell.Current.GoToAsync("//dashboard");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update profile.", "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

