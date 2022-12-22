using Opc.UaFx;
using Opc.UaFx.Client;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

public class Program
{
    static string[] Links = File.ReadAllLines("Links.txt");
    static string OpcConnectionString = Links[1];
    static string DeviceConnectionString = Links[3];

    public static DateTime maintenanceDate = DateTime.MinValue;

    static async Task Main(string[] arguments)
    {
        using var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
        await deviceClient.OpenAsync();
        var device = new IoTDevice(deviceClient);

        await device.InitializeHandlers();

        Opcdevice opcDevice = new Opcdevice();

        Opcdevice.StartConnection();

        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync())
        {
            Console.WriteLine(DateTime.Now);
            await IoTDevice.SendTelemetry(Opcdevice.client);
        }

        Opcdevice.EndConnection();
        Console.ReadLine();
    }
}