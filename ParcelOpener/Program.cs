using Modbus.Device;
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
        ushort registerValue;
        do
        {
            var result = ushort.TryParse(Console.ReadLine(), out registerValue);

            if (result == false)
                Console.WriteLine("Enter valid value:");
        } while (registerValue == 0);

        var serialPort = new SerialPort(deviceAddress)
        {
            BaudRate = 9600,
            DataBits = 8,
            StopBits = StopBits.One,
            Parity = Parity.None,
        };
        serialPort.Open();

        var master = ModbusSerialMaster.CreateRtu(serialPort);

        if (string.IsNullOrWhiteSpace(deviceAddress))
        {
            Console.WriteLine("deviceAddress is not valid");
            Console.ReadLine();
            return;
        }

        var slaveId = byte.Parse(deviceAddress.Replace("COM", string.Empty));

        master.WriteSingleRegister(slaveId, registerAddress, registerValue);


        Console.WriteLine("Value written\nCheck status");


        Console.WriteLine("Enter lock number to check status:");
        _ = int.TryParse(Console.ReadLine(), out var lockNumber);
        GetLockStatus(slaveId, registerAddress, lockNumber, master);
    }

    public static bool GetLockStatus(byte slaveId, ushort registerAddress, int lockNumber, IModbusSerialMaster modbus)
    {
        var readResult = modbus.ReadHoldingRegisters(slaveId, registerAddress, 1);
        if (readResult == null)
            return false;

        var mask = 1 << lockNumber;
        return (readResult[0] & mask) == 0;
    }
}
