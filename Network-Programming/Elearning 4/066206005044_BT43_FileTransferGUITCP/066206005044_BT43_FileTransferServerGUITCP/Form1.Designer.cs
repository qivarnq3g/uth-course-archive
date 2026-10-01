namespace _066206005044_BT43_FileTransferServerGUITCP;

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
        labelPort = new Label();
        numericPort = new NumericUpDown();
        buttonStart = new Button();
        buttonStop = new Button();
        labelReceivePath = new Label();
        textReceivePath = new TextBox();
        buttonBrowseReceive = new Button();
        progressReceive = new ProgressBar();
        labelProgress = new Label();
        labelStatus = new Label();
        ((System.ComponentModel.ISupportInitialize)numericPort).BeginInit();
        SuspendLayout();
        labelTitle.AutoSize = true;
        labelTitle.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelTitle.Location = new Point(22, 20);
        labelTitle.Name = "labelTitle";
        labelTitle.Size = new Size(433, 37);
        labelTitle.TabIndex = 0;
        labelTitle.Text = "BT4.3 - TCP File Transfer Server";
        labelPort.AutoSize = true;
        labelPort.Location = new Point(27, 84);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(31, 15);
        labelPort.TabIndex = 1;
        labelPort.Text = "Port";
        numericPort.Location = new Point(74, 82);
        numericPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numericPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericPort.Name = "numericPort";
        numericPort.Size = new Size(95, 23);
        numericPort.TabIndex = 2;
        numericPort.Value = new decimal(new int[] { 9100, 0, 0, 0 });
        buttonStart.Location = new Point(191, 77);
        buttonStart.Name = "buttonStart";
        buttonStart.Size = new Size(118, 32);
        buttonStart.TabIndex = 3;
        buttonStart.Text = "Start Service";
        buttonStart.UseVisualStyleBackColor = true;
        buttonStart.Click += buttonStart_Click;
        buttonStop.Location = new Point(321, 77);
        buttonStop.Name = "buttonStop";
        buttonStop.Size = new Size(118, 32);
        buttonStop.TabIndex = 4;
        buttonStop.Text = "Stop Service";
        buttonStop.UseVisualStyleBackColor = true;
        buttonStop.Click += buttonStop_Click;
        labelReceivePath.AutoSize = true;
        labelReceivePath.Location = new Point(27, 139);
        labelReceivePath.Name = "labelReceivePath";
        labelReceivePath.Size = new Size(99, 15);
        labelReceivePath.TabIndex = 5;
        labelReceivePath.Text = "File nhan (output)";
        textReceivePath.Location = new Point(27, 160);
        textReceivePath.Name = "textReceivePath";
        textReceivePath.ReadOnly = true;
        textReceivePath.Size = new Size(412, 23);
        textReceivePath.TabIndex = 6;
        buttonBrowseReceive.Location = new Point(451, 158);
        buttonBrowseReceive.Name = "buttonBrowseReceive";
        buttonBrowseReceive.Size = new Size(109, 27);
        buttonBrowseReceive.TabIndex = 7;
        buttonBrowseReceive.Text = "Chon File";
        buttonBrowseReceive.UseVisualStyleBackColor = true;
        buttonBrowseReceive.Click += buttonBrowseReceive_Click;
        progressReceive.Location = new Point(27, 245);
        progressReceive.Name = "progressReceive";
        progressReceive.Size = new Size(533, 28);
        progressReceive.TabIndex = 8;
        labelProgress.AutoSize = true;
        labelProgress.Location = new Point(27, 223);
        labelProgress.Name = "labelProgress";
        labelProgress.Size = new Size(103, 15);
        labelProgress.TabIndex = 9;
        labelProgress.Text = "Tien do nhan file";
        labelStatus.AutoSize = true;
        labelStatus.Location = new Point(27, 292);
        labelStatus.Name = "labelStatus";
        labelStatus.Size = new Size(123, 15);
        labelStatus.TabIndex = 10;
        labelStatus.Text = "Status: Disconnected";
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(594, 346);
        Controls.Add(labelStatus);
        Controls.Add(labelProgress);
        Controls.Add(progressReceive);
        Controls.Add(buttonBrowseReceive);
        Controls.Add(textReceivePath);
        Controls.Add(labelReceivePath);
        Controls.Add(buttonStop);
        Controls.Add(buttonStart);
        Controls.Add(numericPort);
        Controls.Add(labelPort);
        Controls.Add(labelTitle);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "066206005044_BT43_FileTransferServerGUITCP";
        FormClosing += formMain_FormClosing;
        Load += formMain_Load;
        ((System.ComponentModel.ISupportInitialize)numericPort).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label labelTitle;
    private Label labelPort;
    private NumericUpDown numericPort;
    private Button buttonStart;
    private Button buttonStop;
    private Label labelReceivePath;
    private TextBox textReceivePath;
    private Button buttonBrowseReceive;
    private ProgressBar progressReceive;
    private Label labelProgress;
    private Label labelStatus;
}
