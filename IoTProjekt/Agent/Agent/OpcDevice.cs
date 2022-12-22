using Opc.UaFx;
using Opc.UaFx.Client;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Opcdevice
{
    public static OpcClient client;

    public static void StartConnection()
    {
        client = new OpcClient(File.ReadAllLines("Links.txt")[1]);
        client.Connect();
        Console.WriteLine("Connection Succes");
    }

    public static void EndConnection()
    {
        client.Disconnect();
    }

    public static async Task Stop(string deviceId)
    {
        Console.WriteLine($"\tDevice shut down {deviceId}\n");
        client.CallMethod($"ns=2;s=Device {deviceId}", $"ns=2;s=Device {deviceId}/EmergencyStop");
        client.WriteNode($"ns=2;s=Device {deviceId}/ProductionRate", OpcAttribute.Value, 0);
        await Task.Delay(1000);
    }

    public static async Task Reset(string deviceId)
    {
        client.CallMethod($"ns=2;s=Device {deviceId}", $"ns=2;s=Device {deviceId}/ResetErrorStatus");
        await Task.Delay(1000);
    }

    public static async Task Maintenance()
    {
        Program.maintenanceDate = DateTime.Now;
        Console.WriteLine($"Device Last Maintenace Date set to: {Program.maintenanceDate}\n");
        await IoTDevice.UpdateTwinValueAsync("LastMaintenanceDate", Program.maintenanceDate);
    }
}