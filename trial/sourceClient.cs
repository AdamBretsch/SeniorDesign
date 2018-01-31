using System ; 
using System.Net;
using System.Net.Sockets ; 
using System.IO ; 
using System.Diagnostics;
using System.Collections;
public class ClientSocket {
    
    protected NetworkStream networkStream; 
    protected StreamReader streamreader; 
    protected StreamWriter streamwriter;
    protected string servermessage = "";
    
    public static void Main(string[] args) {
        ClientSocket cs = new ClientSocket();
    }
    
    public ClientSocket() { 
        TcpClient server; 
        bool status = true ; 
        try { 
	    // the 192 address is for ethernet over usb
<<<<<<< HEAD
	    server = new TcpClient("192.168.7.1", 9009);
	    // the 10 address is for the ethernet only connection
	   // server = new TcpClient("10.42.0.1", 9009);
=======
	    server = new TcpClient("10.0.2.15", 9009);
	    // the 10 address is for the ethernet only connection
	    // server = new TcpClient("10.42.0.1", 9009);
>>>>>>> 5328b01e99996beac745f7019c597ce1c8f90ce0
	    Console.WriteLine("Connected to Server");
        } catch { 
            Console.WriteLine("Failed to Connect to server") ; 
            return ; 
        } 
        networkStream = server.GetStream(); 
        streamreader = new StreamReader(networkStream) ; 
        streamwriter = new StreamWriter(networkStream) ;
        try 
        { 
            while(status) 
            { 
                servermessage = streamreader.ReadLine() ; 
                
                switch (servermessage) {
                    case "medic:":
                        getData();
                        break;
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
                        recieveBytes();
                        break;
                    case "POWER:":
                        turnPower();
                        break;
                    case "RECIEVE:":
                        sendBytes();
                        break;
                    case "medic:":
                        getData();
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
        streamreader.Close() ; 
        streamwriter.Close() ; 
        networkStream.Close() ; 
    }

    protected void getData() {
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
        string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + @"/vid.txt");
        // need to get real pid here
        sendData(lines[0]);
    }
    
    protected void getData() {
        runScript("medicalReader.py");
        string path = Directory.GetCurrentDirectory() + "/usbOut.txt";
        sendFile(path);
    }
    
    protected void getVID() {
        string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + @"/vid.txt");
        // need to get real vid here
        sendData(lines[0]);
    }
    
    protected void recieveBytes() {
        int device = int.Parse(streamreader.ReadLine());
        byte[] result = null;
        BinaryReader binaryreader = new BinaryReader(networkStream);
        // result = binaryreader.Read();
        // need to do something with recieved bytes here
        Console.WriteLine("recieveBytes");
    }

    protected void runScript(string shell) {
        Process p = new Process();
        p.StartInfo.FileName = shell;
        p.Start();
        p.WaitForExit();
    }
    
    protected void saveFile() {
        servermessage = streamreader.ReadLine();
        ArrayList lines = new ArrayList();
        while(servermessage != "TransmitOver:"){
            lines.Add(servermessage);
            servermessage = streamreader.ReadLine();
        }
        string[] myArr = (string[]) lines.ToArray( typeof( string ) );
        System.IO.File.WriteAllLines(Directory.GetCurrentDirectory() + @"/temp.txt", myArr);
    }

    protected void sendBytes() {
        byte[] bytes = null;
        // need to actually get real bytes here
        BinaryWriter binarywriter = new BinaryWriter(networkStream);
        binarywriter.Write(bytes);
    }
    
    protected bool sendData(string data) {
<<<<<<< HEAD
        streamwriter.WriteLine(data) ;
        streamwriter.Flush() ; 
        return true;
    }

=======
            streamwriter.WriteLine(data) ;
            streamwriter.Flush() ; 
            return true;
    }
    
>>>>>>> 5328b01e99996beac745f7019c597ce1c8f90ce0
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
<<<<<<< HEAD

    protected void turnPower() {
        string onOrOff = streamreader.ReadLine();
        int device = int.Parse(streamreader.ReadLine());
        // need to actually turn the devices on or off here
        Console.WriteLine(onOrOff + " " + device);
    }
} 
=======
} 
>>>>>>> 5328b01e99996beac745f7019c597ce1c8f90ce0
