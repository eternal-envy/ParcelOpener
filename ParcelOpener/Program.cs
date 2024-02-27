using EasyModbus;
using System.IO.Ports;

namespace ParcelOpener;

public class Program
{
    protected static void Main(string[] args)
    {
        Console.WriteLine("Enter device address (COM1):");
        var deviceAddress = Console.ReadLine();
        Console.WriteLine("Enter coil address:");
        var coilAddress = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter value for coil (0 or 1):");
        int coilValue;
        do
        {
            var result = int.TryParse(Console.ReadLine(), out coilValue);

            if (result == false)
                Console.WriteLine("Enter valid value:");
        } while (coilValue == 0 || coilValue == 1);

        var modbusClient = new ModbusClient(deviceAddress)
        {
            Baudrate = 9600,
            StopBits = StopBits.Two,
            Parity = Parity.None
        };

        modbusClient.WriteSingleCoil(coilAddress, coilValue == 1);

        Console.WriteLine("Value written");
    }
}