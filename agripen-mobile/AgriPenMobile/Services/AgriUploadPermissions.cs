namespace AgriPenMobile.Services;

public class AgriUploadPermissions : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions
    {
        get
        {
            return new List<(string androidPermission, bool isRuntime)>
            {
                (Android.Manifest.Permission.Internet, true),
                (Android.Manifest.Permission.AccessNetworkState, false),

                (Android.Manifest.Permission.Camera, true),
                (Android.Manifest.Permission.ReadMediaImages, false),
                (Android.Manifest.Permission.ReadExternalStorage, false),
                (Android.Manifest.Permission.WriteExternalStorage, false),
            }.ToArray();
        }
    }
}

public class AgriBluetoothermissions : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions
    {
        get
        {
            return new List<(string androidPermission, bool isRuntime)>
            {
                (Android.Manifest.Permission.Internet, true),
                (Android.Manifest.Permission.AccessNetworkState, false),

                (Android.Manifest.Permission.AccessFineLocation, true),
                (Android.Manifest.Permission.AccessCoarseLocation, true),
                (Android.Manifest.Permission.AccessBackgroundLocation, true),

                (Android.Manifest.Permission.Bluetooth, true),
                (Android.Manifest.Permission.BluetoothAdmin, false),
                (Android.Manifest.Permission.BluetoothScan, false),
                (Android.Manifest.Permission.BluetoothConnect, false),
                (Android.Manifest.Permission.BluetoothAdvertise, false),
            }.ToArray();
        }
    }
}
