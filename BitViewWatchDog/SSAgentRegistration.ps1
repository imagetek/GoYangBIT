param([string]$applicationPath)

$action = New-ScheduledTaskAction -Execute "BitViewWatchDog.exe" -WorkingDirectory $applicationPath
$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval (New-TimeSpan -Minutes 1)

Register-ScheduledTask -Action $action -Trigger $trigger -TaskName "SSAgent" -Description "Agent to start BitView if it is not running"