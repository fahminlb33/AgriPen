namespace AgriPenMobile.Services;

public class AgriPlatformPermissions : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions
    {
        get
        {
            return new List<(string androidPermission, bool isRuntime)>
            {
                (Android.Manifest.Permission.Internet, true),
                (Android.Manifest.Permission.AccessNetworkState, true),

                (Android.Manifest.Permission.ChangeWifiState, true),
                (Android.Manifest.Permission.AccessWifiState, true),
                (Android.Manifest.Permission.NearbyWifiDevices, true),

                (Android.Manifest.Permission.AccessFineLocation, true),
                (Android.Manifest.Permission.AccessCoarseLocation, true),
                (Android.Manifest.Permission.AccessBackgroundLocation, true),

                (Android.Manifest.Permission.Bluetooth, true),
                (Android.Manifest.Permission.BluetoothAdmin, true),
                (Android.Manifest.Permission.BluetoothScan, true),
                (Android.Manifest.Permission.BluetoothConnect, true),
                (Android.Manifest.Permission.BluetoothAdvertise, true),

                (Android.Manifest.Permission.Camera, true),
                (Android.Manifest.Permission.ReadMediaImages, true),
                (Android.Manifest.Permission.ReadExternalStorage, true),
                (Android.Manifest.Permission.WriteExternalStorage, true),
            }.ToArray();
        }
    }
}
