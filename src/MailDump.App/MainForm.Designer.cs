namespace MailDump.App;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        fileListBox = new System.Windows.Forms.CheckedListBox();
        addFileButton = new System.Windows.Forms.Button();
        removeFileButton = new System.Windows.Forms.Button();
        extractButton = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // fileListBox
        // 
        fileListBox.FormattingEnabled = true;
        fileListBox.Location = new System.Drawing.Point(12, 12);
        fileListBox.Name = "fileListBox";
        fileListBox.Size = new System.Drawing.Size(335, 148);
        fileListBox.TabIndex = 0;
        // 
        // addFileButton
        // 
        addFileButton.Location = new System.Drawing.Point(353, 12);
        addFileButton.Name = "addFileButton";
        addFileButton.Size = new System.Drawing.Size(147, 31);
        addFileButton.TabIndex = 1;
        addFileButton.Text = "Ajouter";
        addFileButton.UseVisualStyleBackColor = true;
        addFileButton.Click += addFileButton_Click;
        // 
        // removeFileButton
        // 
        removeFileButton.Location = new System.Drawing.Point(353, 49);
        removeFileButton.Name = "removeFileButton";
        removeFileButton.Size = new System.Drawing.Size(147, 31);
        removeFileButton.TabIndex = 2;
        removeFileButton.Text = "Supprimer";
        removeFileButton.UseVisualStyleBackColor = true;
        removeFileButton.Click += removeFileButton_Click;
        // 
        // extractButton
        // 
        extractButton.Location = new System.Drawing.Point(12, 168);
        extractButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
        extractButton.Name = "extractButton";
        extractButton.Size = new System.Drawing.Size(488, 35);
        extractButton.TabIndex = 4;
        extractButton.Text = "Extraire";
        extractButton.UseVisualStyleBackColor = true;
        extractButton.Click += extractButton_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size(507, 214);
        Controls.Add(extractButton);
        Controls.Add(removeFileButton);
        Controls.Add(addFileButton);
        Controls.Add(fileListBox);
        Location = new System.Drawing.Point(15, 15);
        Text = "MailDump";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button extractButton;

    private System.Windows.Forms.Button addFileButton;
    private System.Windows.Forms.Button removeFileButton;

    private System.Windows.Forms.CheckedListBox fileListBox;

    #endregion
}
