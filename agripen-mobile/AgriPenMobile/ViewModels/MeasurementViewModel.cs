using AgriPenMobile.API;
using AgriPenMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AgriPenMobile.ViewModels;

public partial class MeasurementViewModel : ObservableObject
{
    private const int MaxObservations = 60;

    private readonly IAgriAPI _api;
    private readonly IAgriBluetoothLogger _blueLogger;
    
    private Location? _location;
    private double _processed = 0;
    public List<string> _photos = new();

    [ObservableProperty]
    private string status = "Siap.";
    [ObservableProperty]
    private string photoStatus = "Tidak ada foto.";
    [ObservableProperty]
    private bool isScanEnabled = true;
    [ObservableProperty]
    public ObservableCollection<TelemetryViewModel> measurements  = new();

    public MeasurementViewModel(IAgriBluetoothLogger blueLogger, IAgriAPI api)
    {
        _api = api;
        _blueLogger = blueLogger;
        _blueLogger.MaxObservations = MaxObservations;
        _blueLogger.OnData += Logger_OnData;
        _blueLogger.OnCompleted += Logger_OnCompleted;

        FillZero();
    }

    private void FillZero()
    {
        Measurements.Clear();
        Measurements.Add(new()
        {
            Name = "Suhu udara",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Kelembapan udara",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Heat index",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Kelembapan tanah",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Pencahyaan matahari",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Latitude",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Longitude",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Altitude",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Akurasi horizontal",
            Value = 0
        });
        Measurements.Add(new()
        {
            Name = "Akurasi vertikal",
            Value = 0
        });
    }

    [RelayCommand]
    private async Task TakePhotoAsync()
    {
        try
        {
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
            using var sourceStream = await photo.OpenReadAsync();
            using var localFileStream = File.OpenWrite(localFilePath);
            await sourceStream.CopyToAsync(localFileStream);

            // show image
            _photos.Add(localFilePath);

            PhotoStatus = $"{_photos.Count} foto dipilih";
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Galat", ex.Message, "Kembali");
        }
    }

    [RelayCommand]
    private async Task ScanAsync()
    {
        try
        {
            await Task.Yield();
            Status = "Menyiapkan...";
            IsScanEnabled = false;

            // check for permissions
            var grantStatus = await Permissions.RequestAsync<AgriBluetoothermissions>();
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
            _location = await geo.GetLocationAsync();
            Measurements[5].Value = _location.Latitude;
            Measurements[6].Value = _location.Longitude;
            Measurements[7].Value = _location.Altitude ?? 0;
            Measurements[8].Value = _location.Accuracy ?? 0;
            Measurements[9].Value = _location.VerticalAccuracy ?? 0;

            // start bluetooth data collection
            _processed = 0;
            await _blueLogger.Start();
        }
        catch (Exception ex)
        {
            Status = "Siap.";
            IsScanEnabled = true;
            await Application.Current.MainPage.DisplayAlert("Gagal!", ex.Message, "OK");
        }
    }

    private void Logger_OnData(SensorData obj)
    {
        _processed++;
        Measurements[0].Value = obj.AirTemperature;
        Measurements[1].Value = obj.AirHumidity;
        Measurements[3].Value = obj.AirHeatIndex;
        Measurements[2].Value = obj.SunIllumination;
        Measurements[4].Value = obj.SoilMoisture;

        Status = $"{MaxObservations - _processed} dari {MaxObservations}...";
    }

    private void Logger_OnCompleted()
    {
        Status = "Mengirim data...";

        Task.Run(async () =>
        {
            try
            {
                // send telemetry
                var data = _blueLogger.Data.Select(x => x.ToTelemetry()).ToList();
                var result = await _api.TelemetryAsync(data, _location.ToGpsData());

                // upload pictures
                await _api.TelemetryImageAsync(result.Id, _photos);

                // set ready
                Status = "Data tersimpan!";

                // delete all files
                foreach (var item in _photos)
                {
                    File.Delete(item);
                }
            }
            catch (Exception ex)
            {
                Status = "Data gagal disimpan.";
            }

            IsScanEnabled = true;
        });
    }
}
