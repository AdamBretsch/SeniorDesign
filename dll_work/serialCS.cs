using System;
using System.IO.Ports;

namespace CSSerialLibrary {
	public class Teensy {
		private SerialPort serialport;

		public static void Main(String[] args) {
			Teensy t = new Teensy("ttyS2", 9600, SerialDataReceivedEventHandler);
			byte[] newdata = new byte[64] 
				{
					0xAA, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
					0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
					0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
					0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
					0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
					0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
					0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
					0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xEE,
				};
			while (Console.ReadLine() != "send");
			t.Write(newdata);
			while (Console.ReadLine() != "off");
			t.serialport.Close();
		}

		private static void SerialDataReceivedEventHandler( object sender, SerialDataReceivedEventArgs e) {
	        SerialPort sport = (SerialPort) sender;
	        // string indata = sp.ReadExisting();
	        byte[] data = new byte[64];
			sport.Read(data, 0, 64);
	        Console.WriteLine("Data Received");
	        // Console.Write(indata);
	        for (int i = 0; i < data.Length; i++) {
	        	Console.WriteLine("Data[{0}] = " + data[i].ToString("X2"), i);
	        }
	    }

		public Teensy(string port, int baudrate, Action<object, SerialDataReceivedEventArgs> method) {
			serialport = new SerialPort(port, baudrate);
			serialport.Open();
			// port.DiscardInBuffer();
			// port.DiscardOutBuffer();
			serialport.DataReceived += new SerialDataReceivedEventHandler(method);
		}

		public void Write(byte[] data) {
			serialport.Write(data, 0, data.Length);
		}
	}
}