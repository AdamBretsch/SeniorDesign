using System ; 
using System.Net;
using System.Net.Sockets ; 
using System.IO ; 
using System.Diagnostics;
using System.Collections;
using System.Security.Permissions;
using System.Globalization;
using System.Threading;
using CSSerialLibrary;

public class ClientSocket {
    
    protected NetworkStream networkStream; 
    protected NetworkStream sendStream;
    protected NetworkStream getStream;

    protected StreamReader streamreader; 
    protected StreamWriter streamwriter;
    // protected StreamWriter sendwriter;
    protected StreamReader getreader;
    protected static ClientSocket cs;
    
    protected FileSystemWatcher watcher;

    protected string servermessage = "";
    protected byte[] bytes =  new byte[32] {0x20, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50};
    
    public static void Main(string[] args) {
        ClientSocket cis = new ClientSocket();
    }

    [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
    private void configureWatcher() {
        string path = "/home/debian/SeniorDesign/teensyTransfer/pyWrite";
        watcher = new FileSystemWatcher(path);
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        watcher.Filter = "*.txt";
        // watcher.Changed += new FileSystemEventHandler(OnFileChanged);
        watcher.Created += new FileSystemEventHandler(OnFileChanged);
        watcher.EnableRaisingEvents = true;
        Teensy t = new Teensy("/dev/ttyO2", 9600, SerialDataReceivedEventHandler);
        
    }
    	private static void SerialDataReceivedEventHandler( object sender, SerialDataReceivedEventArgs e) {
	        SerialPort sport = (SerialPort) sender;
	        byte[] data = new byte[64];
			sport.Read(data, 0, 64);
	        Console.WriteLine("Data Received");
	        for (int i = 0; i < data.Length; i++) {
	        	Console.WriteLine("Data[{0}] = " + data[i].ToString("X2"), i);
	        }
	    }
    
    [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
    public ClientSocket() { 
        TcpClient server;
        TcpClient sendServer;
        TcpClient getServer;
        bool status = true ; 
        try { 
        // the 192 address is for ethernet over usb
        server = new TcpClient("192.168.7.1", 9009);
        sendServer = new TcpClient("192.168.7.1", 9008);
        getServer = new TcpClient("192.168.7.1", 9007);
        // the 10 address is for the ethernet only connection
        // server = new TcpClient("10.42.0.1", 9009);
        Console.WriteLine("Connected to Server");
        } catch { 
            Console.WriteLine("Failed to Connect to server"); 
            return ; 
        } 
        networkStream = server.GetStream(); 
        sendStream = sendServer.GetStream();
        getStream = getServer.GetStream();

        streamreader = new StreamReader(networkStream) ; 
        streamwriter = new StreamWriter(networkStream) ;
        // sendwriter = new StreamWriter(sendStream);
        getreader = new StreamReader(getStream);
        Console.WriteLine("Streams Created");
        configureWatcher();
        ClientSocket.cs = this;
        try 
        { 
            // this while loop recieves commands for the duration
            // that the beaglebone is running
            while(status) 
            { 
                servermessage = getData(); 
                
                switch (servermessage) {
                    case "VID:":
                        getVID();
                        break;
                    case "PID:":
                        getPID();
                        break;
                    case "DEVICES:":
                        getDevices();
                        break;
                    case "TransmitReady:":
                        saveFile();
                        break;
                    case "GOTBYTE:":
                        receiveBytes();
                        break;
                    case "POWER:":
                        turnPower();
                        break;
                    case "RELAY:":
                        relay();
                        break;
                    case "medic:":
                        getMedData();
                        break;                        
                    case "bye":
                        status = false;
                        break;
                    case "red":
                        runScript("usr0.sh");
                        break;
                    case "blue":
                        runScript("usr1.sh");
                        break;
                    case "green":
                        runScript("usr2.sh");
                        break;
                    case "watchdogs":
                        Thread th = new Thread(watchdog);
                        th.Start();
                        break;
                        
                    default:
                        Console.WriteLine("Host: " + servermessage);
                        break;
                }
            } 
        } 
        catch
        { 
            Console.WriteLine("Exception reading from the server") ; 
        } 
        streamreader.Close(); 
        getreader.Close();
        streamwriter.Close();
        // sendwriter.Close(); 
        networkStream.Close();
        // sendStream.Close();
        getStream.Close(); 
    }

    protected string getData() {
        return streamreader.ReadLine();
    }

    protected void getMedData() {
        runScript("medicalReader.py");
        string path = Directory.GetCurrentDirectory() + "/usbOut.txt";
        sendFile(path);
    }
    
    protected void getDevices() {
        ArrayList devices = new ArrayList();
        devices.Add(1);
        // need a real way to get devices here
        foreach(var item in devices) {
            sendData("" + item);
        }
        sendData("TransmitOver:");
    }

    protected void getPID() {
        // int device = int.Parse(getData());
        string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + @"/vid.txt");
        // need to get real pid here
        sendData(lines[1]);
    }
    
    protected void getVID() {
        // int device = int.Parse(getData());
        string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + @"/vid.txt");
        // need to get real vid here
        sendData(lines[0]);
    }

    private static void OnFileChanged(object source, FileSystemEventArgs e) {
        Console.WriteLine("Bone.cs says file: " + e.FullPath + " " + e.ChangeType);
        // values read from file will be in stringData
        string[] stringData = System.IO.File.ReadAllLines(e.FullPath);
        string tried;
        byte thing;
        byte[] data = new byte[stringData.Length];

        for (int i = 0; i < stringData.Length; i++) {
            // Console.Write("Line[" + i + "]:. " + stringData[i]);
            tried = stringData[i].Trim();
            if (tried != "") {
                thing = Byte.Parse(stringData[i].Trim(), NumberStyles.AllowHexSpecifier);   
                data[i] = thing;
                // Console.WriteLine("->" + thing.ToString("X"));
            } else {
                Console.WriteLine("->Empty");
                data[i] = 0x00;
            }
        }
        Console.WriteLine("make it work god damn it" + stringData[0]);
        for(int i = 0;i<stringData.Length;i++){
            // prints each byte in original string and compares to the converted byte
            Console.Write(stringData[i] + " : versus : ");
            Console.WriteLine(data[i].ToString("X"));
        }
        // send byte[] as byte[] to control computer
        Console.WriteLine(source.GetType().Name);
        ClientSocket.cs.sendBytes(2,data);
    }
    
    protected void receiveBytes() {
        string device = getData();
        int size = int.Parse(getData());
        byte[] result = new byte[size];
        getStream.Read(result, 0, size);
        // need to do something with received bytes here
        Console.WriteLine("Received Bytes to send to: " + device);
        for(int i = 0; i < size; i++) {
            Console.WriteLine(result[i].ToString("X"));
        }
        sendPyBytes(result);
    }
    
    protected void relay() {
        Console.WriteLine("IN RELAY");
        byte[] thing = new byte[6] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05};
        sendStream.WriteByte(18);
        sendStream.WriteByte(Convert.ToByte(thing.Length));
        sendStream.Write(thing, 0, thing.Length);
    }

    protected void runScript(string shell) {
        Process p = new Process();
        p.StartInfo.FileName = shell;
        p.Start();
        p.WaitForExit();
    }
    
    protected void saveFile() {
        servermessage = getData();
        ArrayList lines = new ArrayList();
        while(servermessage != "TransmitOver:"){
            lines.Add(servermessage);
            servermessage = getData();
        }
        string[] myArr = (string[]) lines.ToArray(typeof(string));
        System.IO.File.WriteAllLines(Directory.GetCurrentDirectory() + @"/temp.txt", myArr);
    }

    protected void sendBytes(int device, byte[] byteArray) {
        // sends device id that had the data originally
        // sendwriter.WriteLine("" + device);
        // sends the length to expect as (string) int
        // sendwriter.WriteLine("" + byteArray.Length);
        // directly send byteArray to control computer
        sendStream.WriteByte(Convert.ToByte(device));
        sendStream.WriteByte(Convert.ToByte(byteArray.Length));
        sendStream.Write(byteArray, 0, byteArray.Length);
    }
    
    protected bool sendData(string data) {
        streamwriter.WriteLine(data) ;
        streamwriter.Flush() ; 
        return true;
    }

    protected bool sendFile(string fileName) {
        Console.WriteLine(fileName);
        if(!File.Exists(fileName)) {
            Console.Write("!FILE NOT FOUND!");
            return false;
        }
        char[] splitOn = {'/'};
        string[] splitName = fileName.Split(splitOn);
        // seperated the path from the aaa.txt we want here
        sendData(splitName[splitName.Length - 1]);
        string[] lines = System.IO.File.ReadAllLines(fileName);
        foreach (string line in lines) {
            sendData(line);
        }
        sendData("TransmitOver:");
        return true;
    }

    protected static void sendPyBytes(byte[] data) {
        string path = "/home/debian/SeniorDesign/teensyTransfer/csWrite/out.txt";
        // string[] converted = new string[data.Length];
        string convert = "";
        for (int i = 0; i < data.Length; i++) {
            convert += data[i].ToString("X") + "\n";
        }
        System.IO.File.WriteAllText(path, convert);
    }

    protected void turnPower() {
        string onOrOff = getData();
        int id = int.Parse(getData());
        // need to actually turn the devices on or off here
        Console.WriteLine("Device " + id + ": " + onOrOff);
        if ((id < 1) || (id > 4)) {
            Console.WriteLine("Invalid device Specified");
        }
        //temporary here
        runScript("usr0.sh");
    }
    
    protected void watchdog() {
        
        new Thread(() => 
        {
            Thread.CurrentThread.IsBackground = true; 
            runScript("pyScript_PORT1.py");
        }).Start();
        
        new Thread(() => 
        {
            Thread.CurrentThread.IsBackground = true; 
            runScript("pyScript_PORT2.py");
        }).Start();

        
    }
} 
