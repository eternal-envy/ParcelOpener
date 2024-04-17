using EasyModbus;
using System.IO.Ports;

namespace ParcelOpener;

public class Program
{
    protected static void Main(string[] args)
    {
        Console.WriteLine("Open lock");
        const int registerAddress = 1005;
        Console.WriteLine("Enter device address (COM1):");
        var deviceAddress = Console.ReadLine();
        Console.WriteLine("Enter value to write to register:");
        int registerValue;
        do
        {
            var result = int.TryParse(Console.ReadLine(), out registerValue);

            if (result == false)
                Console.WriteLine("Enter valid value:");
        } while (registerValue == 0 || registerValue == 1);

        var modbusClient = new ModbusClient(deviceAddress)
        {
            Baudrate = 9600,
            StopBits = StopBits.Two,
            Parity = Parity.None
        };

        modbusClient.WriteSingleRegister(registerAddress, registerValue);

        Console.WriteLine("Value written\nCheck status");

        GetLockStatuses(deviceAddress, registerAddress);
    }

    private List<bool> GetLocksStatuses(int deviceAddress, int registerAddress)
        {
            var readResult = Read(SlaveId, OpenLockRegisterAddress, 1);
            if (readResult == null)
                return null;

            var result = new List<bool>();
            var mask = 1;
            for (byte i = 0; i < 16; i++)
            {
                result.Add((readResult[0] & mask) == 0);
                mask <<= 1;
            }
            return result;
        }
}
