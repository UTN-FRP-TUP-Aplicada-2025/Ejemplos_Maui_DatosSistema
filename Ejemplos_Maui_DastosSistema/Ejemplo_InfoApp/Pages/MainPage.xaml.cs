using Ejemplo_InfoApp.Services;

namespace Ejemplo_InfoApp.Pages;

public partial class MainPage : ContentPage
{
    readonly InfoSystemService _statusReport;

    private string _networkType = string.Empty;
    private string _deviceId = string.Empty;
    private string _memoryInfo= string.Empty;
    private string _deviceInfo = string.Empty;
    private string _systemLoad = string.Empty;
    private string _runningProcesses = string.Empty;


    public string NetworkType
    {
        get => _networkType;
        set
        {
            if (_networkType != value)
            {
                _networkType = value;
                OnPropertyChanged();
            }
        }
    }

    public string DeviceId
    {
        get => _deviceId;
        set
        {
            if (_deviceId != value)
            {
                _deviceId = value;
                OnPropertyChanged();
            }
        }
    }

    public string MemoryInfo
    {
        get => _memoryInfo;
        set
        {
            if (_memoryInfo != value)
            {
                _memoryInfo = value;
                OnPropertyChanged();
            }
        }
    }

    public string DeviceInfo
    {
        get => _deviceInfo;
        set
        {
            if (_deviceInfo != value)
            {
                _deviceInfo = value;
                OnPropertyChanged();
            }
        }
    }

    public string SystemLoad
    {
        get => _systemLoad;
        set
        {
            if (_systemLoad != value)
            {
                _systemLoad = value;
                OnPropertyChanged();
            }
        }
    }

    public string RunningProcesses
    {
        get => _runningProcesses;
        set
        {
            if (_runningProcesses != value)
            {
                _runningProcesses = value;
                OnPropertyChanged();
            }
        }
    }

    string logs;
    public string Logs
    {
        get {
            return logs;
        }
        set 
        {
            if (logs != value)
            {
                logs = value;
                OnPropertyChanged();
            }
        }
    }
    

    public MainPage(InfoSystemService statusReport)
    {
        InitializeComponent();
        _statusReport = statusReport;

        BindingContext = this;

#if ANDROID
        var diagnostics = FileLoggerService.Instance?.GetLogDiagnostics() ?? "Servicio NULL";
        var logs = FileLoggerService.Instance?.GetLogContent() ?? "No hay logs";

        // Mostrar tanto diagnósticos como logs
        Logs = $"{diagnostics}\n\n=== CONTENIDO DEL LOG ===\n{logs}";
#endif
    }

    private void BtnNetworkType_Clicked(object sender, EventArgs e)
    {
        NetworkType = _statusReport.GetNetworkType();
    }

    private void BtnDeviceId_Clicked(object sender, EventArgs e)
    {
        DeviceId= _statusReport.GetDeviceId();
    }

    private void BtnMemoryInfo_Clicked(object sender, EventArgs e)
    {
        MemoryInfo = _statusReport.GetMemoryInfoText();
    }

    private void BtnDeviceInfo_Clicked(object sender, EventArgs e)
    {
        DeviceInfo = _statusReport.GetDeviceInfoText();
    }

    private void BtnSystemLoad_Clicked(object sender, EventArgs e)
    {
        SystemLoad = _statusReport.GetSystemLoadText();
    }

    private void BtnRunningProcesses_Clicked(object sender, EventArgs e)
    {
        RunningProcesses = _statusReport.GetRunningProcessesText();
    }
    

    async private void BtnThrows_Clicked(object sender, EventArgs e)
    {
        //  await Shell.Current.GoToAsync("/ThrowExceptionPage");
        await Shell.Current.GoToAsync("ThrowExceptionPage");
    }
    
}
