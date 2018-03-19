using System;
using System.IO.Ports;
using System.Threading;

namespace CSSerialLibrary {
  	public class Teensy {
		private SerialPort serialport;
		public static bool toContinue;
        public static byte[] receivedData = new byte[64];
        private static Teensy t;
    

		public static void Main(String[] args) {
		    t = new Teensy("/dev/ttyS2", 9600, DataReceivedEventHandler);
		    String message;
			Thread readThread = new Thread(Read);
			toContinue = true;
			readThread.Start();
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
			Console.WriteLine("serial started: ");
			// t.Write(newdata);
			
			Console.WriteLine("type in 'quit' to exit");
			while(toContinue){
			    message = Console.ReadLine();
			    if(message == "quit"){
			        toContinue = false;
			    }
			    else{
			        // t.Write(newdata);
			    }
			}
			readThread.Abort();
			t.serialport.Close();
		}
		private static void Read(){
		    Console.WriteLine("Received Data ready:");
		    while(toContinue){
		        Console.WriteLine(t.serialport.Read(receivedData,0,1));
		    }
		    Console.WriteLine("Here it goes:");
		    
		}
// 		private static void SerialDataReceivedEventHandler( object sender, SerialDataReceivedEventArgs e) {
// 		    Console.WriteLine("here");
// 	        SerialPort sport = (SerialPort) sender;
// // 	        byte[] data = new byte[64];
// // 			sport.Read(data, 0, 64);
// // 	        Console.WriteLine("Data Received");
// // 	        for (int i = 0; i < data.Length; i++) {
// // 	        	Console.WriteLine("Data[{0}] = " + data[i].ToString("X2"), i);
// // 	        }
// 			Console.WriteLine(sport.ReadExisting());
			
// 	    }

		public Teensy(string port, int baudrate, Action<object, SerialDataReceivedEventArgs> method) {
			serialport = new SerialPort(port, baudrate);
			serialport.RtsEnable = true;
// 			serialport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedEventHandler);
		    serialport.Open();
		}
		
		private static void DataReceivedEventHandler( object sender, SerialDataReceivedEventArgs e) {
		    Console.WriteLine("here");
	        SerialPort sport = (SerialPort) sender;
// 	        byte[] data = new byte[64];
// 			sport.Read(data, 0, 64);
// 	        Console.WriteLine("Data Received");
// 	        for (int i = 0; i < data.Length; i++) {
// 	        	Console.WriteLine("Data[{0}] = " + data[i].ToString("X2"), i);
// 	        }
			String indata = sport.ReadExisting();
			Console.Write(indata);
			
	    }
		
		public void Write(byte[] data) {
			serialport.Write(data, 0, data.Length);
		}
	}
}