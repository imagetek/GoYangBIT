using System.Diagnostics;
using System.IO;
using System.Text;

namespace SSCommonNET
{
	public static class PowerShellService
	{
		private static readonly string _currentDir = Directory.GetCurrentDirectory();
		private static readonly string _registerTaskFileName = "SSAgentRegistration.ps1";
		private static readonly string _enableTaskFileName = "EnableSSAgent.ps1";
		private static readonly string _disableTaskFileName = "DisableSSAgent.ps1";
		private static readonly string _checkTaskStatusFileName = "SSAgentStatus.ps1";
		private static readonly string _unregisterTaskFileName = "UnregisterSSAgent.ps1";
		private static readonly string _taskName = "SSAgent";
		private static readonly StringBuilder _error = new StringBuilder();
		private static readonly StringBuilder _result = new StringBuilder();
		//public static void RegisterSSAgent()
		//{			
		//	CreateAndRunProcess(PsScriptType.Register);
		//}

		//public static void DisableSSAgent()
		//{
		//	CreateAndRunProcess(PsScriptType.Disable);
		//}

		//public static void UnregisterSSAgent()
		//{
		//	CreateAndRunProcess(PsScriptType.Unregister);
		//}

		//public static void EnableSSAgent()
		//{
		//	CreateAndRunProcess(PsScriptType.Enable);
		//}

		//public static void CheckSSAgentStatus()
		//{
		//	CreateAndRunProcess(PsScriptType.CheckStatus);
		//}

		public static void CreateAndRunProcess(PsScriptType psScriptType)
		{
			ClearMessages();
			GetArgsAndEvent(psScriptType, out string arguments, out EventHandler? exitEvent);
			StartProcess(arguments, exitEvent);
		}

		private static void StartProcess(string arguments, EventHandler? exitEvent)
		{
			var startInfo = new ProcessStartInfo()
			{
				FileName = "powershell.exe",
				//Arguments = $"-NoProfile -ExecutionPolicy ByPass -File \"{ps1File}\"",
				Arguments = arguments,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				WindowStyle = ProcessWindowStyle.Hidden,
			};
			Process process = new()
			{
				StartInfo = startInfo,
				EnableRaisingEvents = true,
			};
			process.Exited += exitEvent;
			process.ErrorDataReceived += Process_ErrorDataReceived;
			process.OutputDataReceived += Process_OutputDataReceived;
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			//var result = process.StandardOutput.ReadToEnd();
			//Console.WriteLine(result);
			process.WaitForExit();
		}

		private static void GetArgsAndEvent(PsScriptType psScriptType, out string arguments, out EventHandler? exitEvent)
		{
			arguments = string.Empty;
			exitEvent = null;
			switch (psScriptType)
			{
				case PsScriptType.Register:
					exitEvent = RegisterProcess_Exited;
					arguments = CreateArgument(psScriptType, _registerTaskFileName);
					break;
				case PsScriptType.Enable:
					exitEvent = EnableSSAgentProcess_Exited;
					arguments = CreateArgument(psScriptType, _enableTaskFileName);
					break;
				case PsScriptType.Disable:
					exitEvent = DisableSSAgentProcess_Exited;
					arguments = CreateArgument(psScriptType, _disableTaskFileName);
					break;
				case PsScriptType.CheckStatus:
					exitEvent = StatusCheckProcess_Exited;
					arguments = CreateArgument(psScriptType, _checkTaskStatusFileName);
					break;
				case PsScriptType.Unregister:
					exitEvent = UnregisterSSAgentProcess_Exited;
					arguments = CreateArgument(psScriptType, _unregisterTaskFileName);
					break;
			}
		}

		private static void ClearMessages()
		{
			_error.Clear();
			_result.Clear();
		}

		private static void RegisterProcess_Exited(object? sender, EventArgs e)
		{
			if (_error.Length > 0)
			{
				OnTaskNotRegisteredEvent?.Invoke();
				OnTaskErrorEvent?.Invoke($"Failed to register service: {_error}.\n Please register task manually.");
			}
			else if (_result.Length > 0)
			{
				if (_result.ToString().Contains(_taskName, StringComparison.InvariantCultureIgnoreCase) && _result.ToString().Contains("ready", StringComparison.InvariantCultureIgnoreCase))
				{
					OnTaskEnabledEvent?.Invoke();
					OnTaskRegisteredEvent?.Invoke();
				}
			}
		}

		private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(e.Data))
			{
				_error.AppendLine(e.Data);
			}
		}

		private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(e.Data))
			{
				_result.AppendLine(e.Data);
			}
		}

		private static void UnregisterSSAgentProcess_Exited(object? sender, EventArgs e)
		{
			if (_error.Length > 0)
			{
				OnTaskErrorEvent?.Invoke($"Failed to unregister SSAgent: {_error}.\n Please unregister task manually.");
			}
			else
			{
				OnTaskUnregisteredEvent?.Invoke();
			}
		}

		private static void EnableSSAgentProcess_Exited(object? sender, EventArgs e)
		{
			if (_error.Length > 0)
			{
				OnTaskErrorEvent?.Invoke($"Failed to enable service: {_error}.\n Please enable task manually.");
			}
			else if (_result.Length > 0)
			{
				if (_result.ToString().Contains(_taskName, StringComparison.InvariantCultureIgnoreCase) && _result.ToString().Contains("ready", StringComparison.InvariantCultureIgnoreCase))
				{
					OnTaskEnabledEvent?.Invoke();
				}
			}
		}

		

		private static string CreateArgument(PsScriptType psScriptType, string fileName)
		{
			string ps1File = Path.Combine(_currentDir, fileName);
			if (psScriptType == PsScriptType.Register)
			{
				return $"-NoProfile -ExecutionPolicy ByPass -File \"{ps1File}\" -applicationPath \"{_currentDir}\"";
			} 
			else
			{
				return $"-NoProfile -ExecutionPolicy ByPass -File \"{ps1File}\"";
			}			
		}

		private static void DisableSSAgentProcess_Exited(object? sender, EventArgs e)
		{
			if (_error.Length > 0)
			{
				OnTaskErrorEvent?.Invoke($"Failed to disable task: {_error}.\n Please disable task manually.");
			}
			else if (_result.Length > 0)
			{
				if (_result.ToString().Contains(_taskName, StringComparison.InvariantCultureIgnoreCase) && _result.ToString().Contains("disabled", StringComparison.InvariantCultureIgnoreCase))
				{
					OnTaskDisabledEvent?.Invoke();
				}
			}
		}

		private static void StatusCheckProcess_Exited(object? sender, EventArgs e)
		{
			if (_error.Length > 0)
			{
				//MessageBox.Show($"error: ${_error}", "error", MessageBoxButton.OK);
				if (_error.ToString().Contains("MSFT_ScheduledTask 개체가 없습니다") || _error.ToString().Contains("Get-ScheduledTask : No MSFT_ScheduledTask"))
				{
					OnTaskNotRegisteredEvent?.Invoke();
				}
				else
				{
					OnTaskDisabledEvent?.Invoke();
					OnTaskErrorEvent?.Invoke($"Scheduled task status check error: {_error}.");
				}
			}
			else if (_result.Length > 0)
			{
				//MessageBox.Show($"ok: ${_result}", "ok", MessageBoxButton.OK);
				if (_result.ToString().Contains("enabled", StringComparison.InvariantCultureIgnoreCase) || _result.ToString().Contains("ready", StringComparison.InvariantCultureIgnoreCase) || _result.ToString().Contains("running", StringComparison.InvariantCultureIgnoreCase))
				{
					OnTaskEnabledEvent?.Invoke();
				}
				else if (_result.ToString().Contains("disabled", StringComparison.InvariantCultureIgnoreCase))
				{
					OnTaskDisabledEvent?.Invoke();
				}
			}
		}

		public delegate void TaskEnabledHandler();
		public static event TaskEnabledHandler OnTaskEnabledEvent;

		public delegate void TaskDisabledHandler();
		public static event TaskDisabledHandler OnTaskDisabledEvent;

		public delegate void TaskNotRegisteredHandler();
		public static event TaskNotRegisteredHandler OnTaskNotRegisteredEvent;

		public delegate void TaskRegisteredHandler();
		public static event TaskRegisteredHandler OnTaskRegisteredEvent;

		public delegate void TaskUnregisteredHandler();
		public static event TaskUnregisteredHandler OnTaskUnregisteredEvent;

		public delegate void TaskErrorHandler(string error);
		public static event TaskErrorHandler OnTaskErrorEvent;
	}
}

public enum PsScriptType
{
	Register,
	Enable,
	Disable,
	CheckStatus,
	Unregister
}