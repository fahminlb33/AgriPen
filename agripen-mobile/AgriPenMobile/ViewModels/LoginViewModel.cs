using AgriPenMobile.API;
using AgriPenMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace AgriPenMobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAgriAPI _api;
    private readonly IConfigurationStore _store;

    [ObservableProperty]
    private string username = "";
    [ObservableProperty]
    private string password = "";
    [ObservableProperty]
    private bool isInputEnabled = false;
    [ObservableProperty]
    private string loginText = "Login";
    [ObservableProperty]
    private bool isLoginEnabled = false;

    public LoginViewModel(IAgriAPI api, IConfigurationStore store)
    {
        _api = api;
        _store = store;
    }

    public async Task Reset()
    {
        var savedUser = _store.Username;
        if (string.IsNullOrEmpty(savedUser))
        {
            InternalLogout();
        }
        else
        {
            Username = savedUser;
            Password = "";
            IsInputEnabled = false;
            IsLoginEnabled = true;
            LoginText = "Logout";
        }
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (LoginText == "Login")
        {
            await InternalLogin();
        }
        else
        {
            InternalLogout();
        }
    }

    private async Task InternalLogin()
    {
        try
        {
            // check if username and password is provided
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Login", "Harap isi username dan password.", "OK");
                return;
            }

            // login
            await _api.LoginAsync(Username, Password);

            // disable controls
            IsInputEnabled = false;
            IsLoginEnabled = true;
            LoginText = "Logout";
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Login", "Username/email dan password tidak ditemukan.", "OK");
        }
    }

    private void InternalLogout()
    {
        SecureStorage.Default.RemoveAll();

        Username = "";
        Password = "";
        LoginText = "Login";
        IsInputEnabled = true;
        IsLoginEnabled = true;
    }
}
