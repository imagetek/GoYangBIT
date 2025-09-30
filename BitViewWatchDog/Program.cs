// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Checking if BitView application is running...");

Process[] allProcesses = Process.GetProcesses();

//allProcesses.ToList().ForEach(p => Console.WriteLine(p.ProcessName));

if (allProcesses.Any(p => p.ProcessName.Equals("BitView", StringComparison.InvariantCultureIgnoreCase)))
{
	WriteToFile("BitView is running");
	Console.WriteLine("BitView is running.");
}
else
{
	WriteToFile("BitView is NOT running");
	Console.WriteLine("BitView is NOT running. Trying to start application...");
	StartBitView();
}

void StartBitView()
{
	//string appPath = "C:\\Users\\bha-SSNS\\Desktop\\EDGAR SOURCE\\BIT Siheung\\siheung-bit\\bin\\Release\\net9.0-windows";
	string appPath = AppDomain.CurrentDomain.BaseDirectory;
	WriteToFile(appPath);
	WriteToFile(Directory.GetCurrentDirectory());
	string appName = "BitView.exe";
	string fullPath = Path.Combine(appPath, appName);

	try
	{
		if (File.Exists(fullPath))
		{
			WriteToFile("Found EXE file and starting process...");
			using (Process process = new Process())
			{
				process.StartInfo.FileName = fullPath;

				process.Start();
				process.PriorityClass = ProcessPriorityClass.High;
				Thread.Sleep(5000);
			}
		}
	}
	catch (Exception ex)
	{
		WriteToFile($"ERROR: {ex.Message}");
	}


}

void WriteToFile(string Message)
{
	string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
	if (!Directory.Exists(path))
	{
		Directory.CreateDirectory(path);
	}
	string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
	using (StreamWriter sw = File.Exists(filepath) ? File.AppendText(filepath) : File.CreateText(filepath))
	{
		sw.WriteLine($"{DateTime.Now.ToLocalTime()}: {Message}");
	}

}