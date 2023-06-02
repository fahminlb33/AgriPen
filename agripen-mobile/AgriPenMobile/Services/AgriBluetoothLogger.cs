using Android.Bluetooth;
using Android.Content;
using Java.Util;
using System.Diagnostics;
using System.Text;

namespace AgriPenMobile.Services;

public interface IAgriBluetoothLogger
{
    event Action<SensorData> OnData;
    event Action OnCompleted;

    int MaxObservations { get; set; }
    List<SensorData> Data { get; }

    void Dispose();
    Task Start();
    void Stop();
}

public class AgriBluetoothLogger : IDisposable, IAgriBluetoothLogger
{
    private BluetoothSocket? _socket;
    private CancellationTokenSource _ct;

    public event Action<SensorData> OnData;
    public event Action OnCompleted;

    public int MaxObservations { get; set; }
    public List<SensorData> Data { get; private set; }

    public AgriBluetoothLogger()
    {
    }

    public async Task Start()
    {
        // create new list and clear cancellation token
        Data = new();
        _ct?.Dispose();
        _ct = new CancellationTokenSource();

        // get bluetooth adapter
        
        var adapter = BluetoothAdapter.DefaultAdapter;
        //var manager = (BluetoothManager) Platform.AppContext.GetSystemService(Context.BluetoothService);
        //var adapter = manager.Adapter;
        if (adapter == null)
        {
            throw new Exception("Tidak dapat mengakses Bluetooth. Pastikan Anda sudah memberikan izin akses.");
        }

        // find AgriPen device
        var device = adapter.BondedDevices.FirstOrDefault(x => x.Name == "AgriPen");
        if (device == null)
        {
            throw new Exception("Perangkat AgriPen belum disandingkan dengan perangkat ini.");
        }

        // create socket
        _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
        await _socket.ConnectAsync();

        // start reading
        ThreadPool.QueueUserWorkItem(o => ReadBluetoothStream((CancellationToken)o), _ct.Token);
    }

    public void Stop()
    {
        _ct?.Cancel();
    }


    private void ReadBluetoothStream(CancellationToken ct)
    {
        // create buffer and read stream
        var buffer = "";
        using var inputStream = _socket.InputStream;
        
        // while the job is not done...
        while (!ct.IsCancellationRequested && Data.Count < MaxObservations)
        {
            // if there is no data, skip
            if (!inputStream.IsDataAvailable()) continue;

            // read from stream
            var buf = new byte[100];
            var readBytes = inputStream.Read(buf);

            // encode to string
            buffer += Encoding.ASCII.GetString(buf, 0, readBytes);

            // check if the buffer has completed
            if (buffer.Contains(Environment.NewLine))
            {
                // parse the data
                var data = SensorData.Parse(buffer);
                Data.Add(data);

                // clear buffer
                buffer = "";

                // raise event
                RaiseOnData(data);
            }
        }

        // raise event
        RaiseOnCompleted();
    }

    private void RaiseOnData(SensorData data)
    {
        OnData?.Invoke(data);
    }

    private void RaiseOnCompleted()
    {
        OnCompleted?.Invoke();
    }

    public void Dispose()
    {
        _ct?.Dispose();
    }
}
