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
	    server = new TcpClient("10.0.2.15", 9009);
	    // the 10 address is for the ethernet only connection
	    // server = new TcpClient("10.42.0.1", 9009);
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
                    case "TransmitReady:":
                        saveFile();
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
    
    protected void getVID() {
        string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + @"/vid.txt");
        sendData(lines[0]);
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
} 
