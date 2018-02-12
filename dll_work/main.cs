using System;
using ECEServer;
using System.IO;
using System.Threading;

public class ChatMain {


	public static void Main(String[] args) {
		CommServer server = new CommServer("192.168.7.1", 9009, 9008, 9007);
		// server.OnBytesReceived += RespondToIncoming;

		// TEST FOR SENGIND A FILE TO BBB
		// the next three lines require a temp.txt file to be
		// present in the local directory
		// string path = Directory.GetCurrentDirectory();
		// path = path + "/temp.txt";
		// server.sendFile(path);

		// 1 <= deviceID <= 4
		int deviceID = 1;
		// server.turnPower(true, 1);

		// TEST FOR RECIEVE BYTES
		// byte[] things = server.receiveBytes(deviceID);
		// for(int i = 0; i < things.Length; i++) {
			// Console.WriteLine(things[i].ToString("X"));
		// }

		// TEST FOR SEND BYTES
		// Note that the actual function you would use would
		// be server.sendBytes(int id, byte[] data) 
		// where id is the emm'd device id and data 
		// // is the data to be sent to the emm'd device
		// Console.WriteLine("TESTING SEND BYTES...");
		// server.testSendBytes(deviceID);
		// Console.WriteLine("SEND BYTES TESTING COMPLETE");
		Console.WriteLine("Testing Throughput:")
		byte[] data = new byte[32] {0x20, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 
									0xEE, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38,
									0x39, 0xAA, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
									0x49, 0x50};
		server.sendBytes(1,data);
		Console.WriteLine("data sent")

		// TEST GETVID()
		Console.WriteLine("Getting VID...");
		// Console.WriteLine(server.getVID(deviceID));

		// TEST GETPID()
		Console.WriteLine("Getting PID");
		// Console.WriteLine(server.getPID(deviceID));
		System.Threading.Thread.Sleep(2000);
		// server.turnPower(false, 1);

		server.disconnect();
	}

	public static void RespondToIncoming(object sender, ECEServer.ByteEventArgs args) {
		Console.WriteLine("!Receiving bytes events triggered!");
		Console.WriteLine("Data came from device: " + args.device);
	}
}