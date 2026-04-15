namespace Ejemplo_ReportSystemData.Pages;

public partial class WebViewPage : ContentPage
{
	public WebViewPage()
	{
		InitializeComponent();

        webview.Source= "https://GeometriaFernando.somee.com";
	}

    private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        Task.Delay(10000).GetAwaiter().GetResult();
    }

    private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {

    }
}