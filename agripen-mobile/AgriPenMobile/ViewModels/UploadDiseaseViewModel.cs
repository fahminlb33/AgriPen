using AgriPenMobile.API;
using AgriPenMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AgriPenMobile.ViewModels;

public partial class UploadDiseaseViewModel : ObservableObject
{
    private readonly IAgriAPI _api;

    [ObservableProperty]
    public ImageSource selectedImage;
    [ObservableProperty]
    public string status;
    [ObservableProperty]
    public bool isUploadEnabled;
    [ObservableProperty]
    public bool isUploadProgessVisible;

    public UploadDiseaseViewModel(IAgriAPI api)
    {
        _api = api;
    }


    [RelayCommand]
    public async Task SelectUploadAsync()
    {
        IsUploadEnabled = false;
        IsUploadProgessVisible = true;

        try
        {
            // check for permissions
            var grantStatus = await Permissions.RequestAsync<AgriPlatformPermissions>();
            if (grantStatus != PermissionStatus.Granted)
            {
                throw new Exception("Aplikasi AgriPen belum mendapat akses layanan lokasi dan Bluetooth. Silakan perbarui pengaturan sistem Anda.");
            }

            // check for GPS
            var geo = Geolocation.Default;
            if (geo == null)
            {
                throw new Exception("Tidak dapat mengakses layanan lokasi. Pastikan Anda sudah memberikan izin akses.");
            }

            // get location using GPS
            var location = await geo.GetLocationAsync();

            // check if capture is available
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                throw new Exception("Tidak dapat mengakses kamera. Pastikan Anda sudah mengizinkan aplikasi untuk membuka kamera.");
            }

            // capture image
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null)
            {
                throw new Exception("Gambar tidak dipilih.");
            }

            // save the file into local storage
            var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            var sourceStream = await photo.OpenReadAsync();
            var localFileStream = File.Open(localFilePath, FileMode.Create);
            await sourceStream.CopyToAsync(localFileStream);

            // show image
            localFileStream.Seek(0, SeekOrigin.Begin);
            SelectedImage = ImageSource.FromFile(localFilePath);

            // upload image
            localFileStream.Seek(0, SeekOrigin.Begin);
            await _api.DiseaseAnalysisAsync(localFileStream, location.ToGpsData());

            // close
            sourceStream.Close();
            localFileStream.Close();

            // delete
            File.Delete(localFilePath);

            await Application.Current.MainPage.DisplayAlert("Sukses!", "Foto berhasil diunggah! Silakan buka laman AgriPen pada browser.", "Kembali");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Galat", ex.Message, "Kembali");
        }

        IsUploadEnabled = true;
        IsUploadProgessVisible = false;
    }
}
