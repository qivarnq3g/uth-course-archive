namespace _066206005044_BT43_FileTransferClientGUITCP;

partial class Form1
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        labelTitle = new Label();
        labelServerIp = new Label();
        textServerIp = new TextBox();
        labelPort = new Label();
        numericPort = new NumericUpDown();
        labelSendFile = new Label();
        textSendFile = new TextBox();
        buttonBrowseSend = new Button();
        buttonSend = new Button();
        progressSend = new ProgressBar();
        labelProgress = new Label();
        labelStatus = new Label();
        ((System.ComponentModel.ISupportInitialize)numericPort).BeginInit();
        SuspendLayout();
        labelTitle.AutoSize = true;
        labelTitle.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelTitle.Location = new Point(21, 20);
        labelTitle.Name = "labelTitle";
        labelTitle.Size = new Size(421, 37);
        labelTitle.TabIndex = 0;
        labelTitle.Text = "BT4.3 - TCP File Transfer Client";
        labelServerIp.AutoSize = true;
        labelServerIp.Location = new Point(27, 82);
        labelServerIp.Name = "labelServerIp";
        labelServerIp.Size = new Size(57, 15);
        labelServerIp.TabIndex = 1;
        labelServerIp.Text = "Server IP";
        textServerIp.Location = new Point(97, 78);
        textServerIp.Name = "textServerIp";
        textServerIp.Size = new Size(154, 23);
        textServerIp.TabIndex = 2;
        textServerIp.Text = "127.0.0.1";
        labelPort.AutoSize = true;
        labelPort.Location = new Point(272, 82);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(31, 15);
        labelPort.TabIndex = 3;
        labelPort.Text = "Port";
        numericPort.Location = new Point(318, 78);
        numericPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numericPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericPort.Name = "numericPort";
        numericPort.Size = new Size(95, 23);
        numericPort.TabIndex = 4;
        numericPort.Value = new decimal(new int[] { 9100, 0, 0, 0 });
        labelSendFile.AutoSize = true;
        labelSendFile.Location = new Point(27, 132);
        labelSendFile.Name = "labelSendFile";
        labelSendFile.Size = new Size(50, 15);
        labelSendFile.TabIndex = 5;
        labelSendFile.Text = "File gui";
        textSendFile.Location = new Point(27, 153);
        textSendFile.Name = "textSendFile";
        textSendFile.ReadOnly = true;
        textSendFile.Size = new Size(386, 23);
        textSendFile.TabIndex = 6;
        buttonBrowseSend.Location = new Point(425, 150);
        buttonBrowseSend.Name = "buttonBrowseSend";
        buttonBrowseSend.Size = new Size(109, 28);
        buttonBrowseSend.TabIndex = 7;
        buttonBrowseSend.Text = "Chon File";
        buttonBrowseSend.UseVisualStyleBackColor = true;
        buttonBrowseSend.Click += buttonBrowseSend_Click;
        buttonSend.Location = new Point(27, 199);
        buttonSend.Name = "buttonSend";
        buttonSend.Size = new Size(507, 41);
        buttonSend.TabIndex = 8;
        buttonSend.Text = "Gui File";
        buttonSend.UseVisualStyleBackColor = true;
        buttonSend.Click += buttonSend_Click;
        progressSend.Location = new Point(27, 284);
        progressSend.Name = "progressSend";
        progressSend.Size = new Size(507, 28);
        progressSend.TabIndex = 9;
        labelProgress.AutoSize = true;
        labelProgress.Location = new Point(27, 262);
        labelProgress.Name = "labelProgress";
        labelProgress.Size = new Size(93, 15);
        labelProgress.TabIndex = 10;
        labelProgress.Text = "Tien do gui file";
        labelStatus.AutoSize = true;
        labelStatus.Location = new Point(27, 331);
        labelStatus.Name = "labelStatus";
        labelStatus.Size = new Size(123, 15);
        labelStatus.TabIndex = 11;
        labelStatus.Text = "Status: Disconnected";
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(569, 382);
        Controls.Add(labelStatus);
        Controls.Add(labelProgress);
        Controls.Add(progressSend);
        Controls.Add(buttonSend);
        Controls.Add(buttonBrowseSend);
        Controls.Add(textSendFile);
        Controls.Add(labelSendFile);
        Controls.Add(numericPort);
        Controls.Add(labelPort);
        Controls.Add(textServerIp);
        Controls.Add(labelServerIp);
        Controls.Add(labelTitle);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "066206005044_BT43_FileTransferClientGUITCP";
        ((System.ComponentModel.ISupportInitialize)numericPort).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label labelTitle;
    private Label labelServerIp;
    private TextBox textServerIp;
    private Label labelPort;
    private NumericUpDown numericPort;
    private Label labelSendFile;
    private TextBox textSendFile;
    private Button buttonBrowseSend;
    private Button buttonSend;
    private ProgressBar progressSend;
    private Label labelProgress;
    private Label labelStatus;
}
