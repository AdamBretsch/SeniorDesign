using System ; 
using System.Net;
using System.Net.Sockets ; 
using System.IO ; 
using System.Diagnostics;
using runGnomeTerminal;
public class ClientSocket1 
{ 
    static void Main(string[] args) 
    { 
        TcpClient socketForServer ; 
        bool status = true ; 
        try { 
	    IPAddress ipa = IPAddress.Parse("192.168.7.3");
	    IPEndPoint ep = new IPEndPoint(ipa, 8100);
            socketForServer = new TcpClient(ep) ; 
            Console.WriteLine("Connected to Server") ; 
        } catch { 
            Console.WriteLine("Failed to Connect to server{0}:999","localhost") ; 
            return ; 
        } 
        NetworkStream networkStream = socketForServer.GetStream() ; 
        StreamReader streamreader = new StreamReader(networkStream) ; 
        StreamWriter streamwriter = new StreamWriter(networkStream) ; 
        try 
        { 
            // string clientmessage="" ; 
            string servermessage="" ; 
            while(status) 
            { 
                servermessage = streamreader.ReadLine() ; 
                Console.WriteLine("Host:"+servermessage) ;

                if((servermessage == "bye") || (servermessage == "BYE")) 
                { 
                    status = false; 
                } 

                if((servermessage == "red") || (servermessage == "RED")) 
                { 
                    Console.WriteLine("Toggling red led");
                    MainClass.ExecuteCommand("gnome-terminal -x bash -ic 'cd $HOME; ./usr0h.sh; exit; bash;'");
                } 
                if((servermessage == "blue") || (servermessage == "BLUE")) 
                { 
                    Console.WriteLine("Toggling blue led");
                    MainClass.ExecuteCommand("gnome-terminal -x bash -ic 'cd $HOME; ./usr1h.sh; exit; bash;'");
                } 
                if((servermessage == "green") || (servermessage == "GREEN")) 
                { 
                    Console.WriteLine("Toggling green led");
                    MainClass.ExecuteCommand("gnome-terminal -x bash -ic 'cd $HOME; ./usr2h.sh; exit; bash;'");
                } 
            } 
        } 
        catch
        { 
            Console.WriteLine("Exception reading from the server") ; 
        } 
        streamreader.Close() ; 
        networkStream.Close() ; 
        streamwriter.Close() ; 
    } 
} 



namespace runGnomeTerminal

    {

        class MainClass

        {

            public static void ExecuteCommand(string command)

            {

                Process proc = new System.Diagnostics.Process ();

                proc.StartInfo.FileName = "/bin/bash";

                proc.StartInfo.Arguments = "-c \" " + command + " \"";

                proc.StartInfo.UseShellExecute = false; 

                proc.StartInfo.RedirectStandardOutput = true;

                proc.Start();



                while (!proc.StandardOutput.EndOfStream) {

                    Console.WriteLine (proc.StandardOutput.ReadLine ());

                }

            }



            // public static void Run()

            // {

            //     ExecuteCommand("gnome-terminal -x bash -ic 'cd $HOME; ./toggleRed.sh; bash;'");

            // }





        }

    }
