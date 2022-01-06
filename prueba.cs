using System;
using System.IO.Ports;
using System.Threading;

class Prueba
{
	public static void Main()
	{
		SerialPort port = new SerialPort("/dev/ttyUSB0", 115200);
		port.Open();
		// Reseteo el Arduino
		port.DtrEnable = false;
		Thread.Sleep(1000);
		port.DiscardInBuffer();
		port.DtrEnable = true;

		Thread.Sleep(3000);
		
		while(!Console.KeyAvailable)
		{
			port.Write(" ");
			Console.WriteLine(port.ReadLine());
		}

		port.Dispose();
	}
}
