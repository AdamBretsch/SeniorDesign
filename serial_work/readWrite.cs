using System ; 
using System.IO ; 
using System.Security.Permissions;
using System.Globalization;
public class TrialEventHandler {

	public static void Main(string[] args) {

        // this block sets up the testing data arrays
        // byte[] data = new byte[32] {0x20, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0xEE, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0xAA, 0x41, 0x42, 0x43, 0x44, 0x45, 						0x46, 0x47, 0x48, 0x49, 0x50};
        // string[] stringData = new string[32];
        // for(int i = 0; i < data.Length; i++) {
        //     stringData[i] = data[i].ToString("X");
        // }

        // start the file watcher
        TrialEventHandler th = new TrialEventHandler();

        // The below line is the test that I used to test the functionality of the watcher
		// System.IO.File.WriteAllLines(Directory.GetCurrentDirectory() + @"/trial.txt", stringData);

		Console.WriteLine("waiting now for an event");

		while(true);
    }

    [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
    public TrialEventHandler() {
        // TODO: Add "/folder" to string path if you want this to watch the /folder directory
    	string path = "/home/debian/SeniorDesign/teensyTransfer/pyWrite/";
    	FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = path;
        /* Watch for changes in LastAccess and LastWrite times, and     
           the renaming of files or directories. */
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        // Only watch text files.
        watcher.Filter = "*.txt";

        // Add event handlers.
        watcher.Changed += new FileSystemEventHandler(reaction);
        // Begin watching.
        watcher.EnableRaisingEvents = true;

    }

    private void reaction(object source, FileSystemEventArgs e) {
    	Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        string[] stringData = System.IO.File.ReadAllLines(e.FullPath);
        byte[] data = new byte[stringData.Length];
        Console.WriteLine("here: " + stringData.Length);

        for(int i = 0; i < stringData.Length; i++) {
            data[i] = Byte.Parse(stringData[i].Trim(), NumberStyles.AllowHexSpecifier);
            Console.WriteLine("string:" + stringData[i]);
            Console.WriteLine("byte:" + data[i].ToString("X"));
        }
    }
}
