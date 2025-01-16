using System.Text;

namespace MailDump.App;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        Application.Run(new MainForm());
    }    
}