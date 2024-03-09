using EasyModbus;
using System.IO.Ports;

namespace ParcelOpener;

public class Program
{
    protected static void Main(string[] args)
    {
        Console.WriteLine("Enter device address (COM1):");
        var deviceAddress = Console.ReadLine();
        Console.WriteLine("Enter register address:");
        var registerAddress = Convert.ToInt32(Console.ReadLine());
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

        Console.WriteLine("Value written");
    }
}