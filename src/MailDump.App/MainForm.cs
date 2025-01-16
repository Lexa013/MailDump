using System.Diagnostics;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Message = MsgReader.Mime.Message;

namespace MailDump.App;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
    }

    private string _saveFileFolderPath = "";
    private string _tempDirPath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
    

    private void addFileButton_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Multiselect = true;
        openFileDialog.Filter = "Mail files (*.eml)|*.eml";
        openFileDialog.ShowDialog();
        
        foreach (var fileName in openFileDialog.FileNames)
        {
            fileListBox.Items.Add(fileName);
        }
    }
    private void removeFileButton_Click(object sender, EventArgs e)
    {
        for (var i = 0; i < fileListBox.Items.Count; i++)
        {
            if (fileListBox.GetItemChecked(i))
            {
                fileListBox.Items.RemoveAt(i);
                // When removing an item all index are decreased by 1 so we need to counter that 
                i--;
            }
        }
    }
    
    private async void extractButton_Click(object sender, EventArgs e)
    {
        using (var folderBrowser = new FolderBrowserDialog())
        {
            folderBrowser.ShowNewFolderButton = false;
            folderBrowser.Description = "Select destination folder";

            if (folderBrowser.ShowDialog() != DialogResult.OK)
                return;
            
            _saveFileFolderPath = folderBrowser.SelectedPath;

            Console.WriteLine($"Files will be saved to: {_saveFileFolderPath}");

            var tasks = fileListBox.Items.Cast<string>()
                .Select(async file => await ProcessFile(file)).ToList();

            var outputs = await Task.WhenAll(tasks);
            
            foreach (var output in outputs.Where(output => output is not null))
            {
                try
                {
                    File.Move(output!, Path.Combine(_saveFileFolderPath, Path.GetFileName(output!)), true);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to move file: {output}");
                }
            }
            
            MessageBox.Show("Successfully extracted files", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        } 
    }
    
    private async Task<string?> ProcessFile(string filePath)
    {
        FileInfo fileInfo;
        
        try
        {
            fileInfo = new FileInfo(filePath);
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to create file info for: {filePath}");
            return null;
        }
        
        Directory.CreateDirectory(_tempDirPath);
        
        var fileName = Path.GetFileNameWithoutExtension(filePath);

        Process convertProcess = new Process();
        convertProcess.StartInfo.FileName = "java";
        convertProcess.StartInfo.Arguments = $"-jar {Directory.GetCurrentDirectory()}\\toolset\\emailconverter.jar -o \"{_tempDirPath}\\{fileName}.pdf\" \"{filePath}\"";
        convertProcess.StartInfo.WorkingDirectory = $"{Directory.GetCurrentDirectory()}\\toolset";
        convertProcess.StartInfo.CreateNoWindow = true;
        convertProcess.StartInfo.RedirectStandardOutput = true;
        convertProcess.StartInfo.UseShellExecute = false;

        string originalPath = $"{_tempDirPath}\\{fileName}.pdf";
        
        try
        {
            await Task.Run(() =>
            {
                convertProcess.Start();
                convertProcess.WaitForExit();
            });
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to convert eml file to pdf: {fileName}");
            return null;
        }

        Console.WriteLine($"Successfully converted eml file to pdf: {fileName}");
        
        // Read attachements

        Message message;
        try
        {
            message = Message.Load(fileInfo);
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to load .eml file to get attachements: {fileName}");
            return originalPath;
        }
        
        if (message.Attachments is null)
        {
            Console.WriteLine($"No attachments were provided to convert eml file to pdf: {fileName}");
            return originalPath;
        }

        var pdfAttachements = message.Attachments.Where(a => Path.GetExtension(a.FileName) == ".pdf").ToList();

        if (!pdfAttachements.Any())
        {
            Console.WriteLine($"Message contains attachements but none of them are a pdf file: {fileName}");
            return originalPath;
        }
        
        // Attach avery attachement to orginal pdf
        for (var i = 0; i < pdfAttachements.Count; i++)
        {
            var pdfFilePath = $"{_tempDirPath}\\{fileName}_{i}.pdf";

            await File.WriteAllBytesAsync(pdfFilePath, pdfAttachements[i].Body);

            var original = PdfReader.Open(originalPath, PdfDocumentOpenMode.Modify);
            var newPdf = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.Import);
            
            MergePages(newPdf, original);
            
            try
            {
                original.Save(originalPath);
            }
            catch (Exception)
            {
                Console.WriteLine($"Failed to save file to pdf: {fileName}");
            }
            
            original.Dispose();
            newPdf.Dispose();

            try
            {
                File.Delete(pdfFilePath);
            }
            catch (Exception)
            {
                Console.WriteLine($"Failed to delete temp file: {pdfFilePath}");
                return originalPath;
            }
        }
        
        return originalPath;
    }

    private void MergePages(PdfDocument from, PdfDocument to)
    {
        for (var i = 0; i < from.PageCount; i++)
        {
            to.AddPage(from.Pages[i]);
        }
    }
}
