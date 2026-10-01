namespace _066206005044_BT41_SimpleChatGUIUDP;

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
        groupMode = new GroupBox();
        tabMode = new TabControl();
        tabServer = new TabPage();
        buttonStopService = new Button();
        buttonStartService = new Button();
        numericServerPort = new NumericUpDown();
        labelServerPort = new Label();
        tabClient = new TabPage();
        buttonDisconnect = new Button();
        buttonConnect = new Button();
        numericClientPort = new NumericUpDown();
        labelClientPort = new Label();
        comboServerIp = new ComboBox();
        labelServerIp = new Label();
        richLog = new RichTextBox();
        panelRightButtons = new Panel();
        buttonExit = new Button();
        buttonClear = new Button();
        buttonSave = new Button();
        buttonLoad = new Button();
        panelSend = new Panel();
        labelSend = new Label();
        textSend = new TextBox();
        buttonSend = new Button();
        statusStripMain = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        groupMode.SuspendLayout();
        tabMode.SuspendLayout();
        tabServer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numericServerPort).BeginInit();
        tabClient.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numericClientPort).BeginInit();
        panelRightButtons.SuspendLayout();
        panelSend.SuspendLayout();
        statusStripMain.SuspendLayout();
        SuspendLayout();
        labelTitle.AutoSize = true;
        labelTitle.Font = new Font("Segoe UI", 32F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelTitle.Location = new Point(24, 40);
        labelTitle.Name = "labelTitle";
        labelTitle.Size = new Size(478, 59);
        labelTitle.TabIndex = 1;
        labelTitle.Text = "Simple Chat Application";
        groupMode.Controls.Add(tabMode);
        groupMode.Location = new Point(24, 152);
        groupMode.Name = "groupMode";
        groupMode.Size = new Size(952, 115);
        groupMode.TabIndex = 2;
        groupMode.TabStop = false;
        tabMode.Controls.Add(tabServer);
        tabMode.Controls.Add(tabClient);
        tabMode.Location = new Point(6, 10);
        tabMode.Name = "tabMode";
        tabMode.SelectedIndex = 0;
        tabMode.Size = new Size(940, 102);
        tabMode.TabIndex = 0;
        tabMode.SelectedIndexChanged += tabMode_SelectedIndexChanged;
        tabServer.Controls.Add(buttonStopService);
        tabServer.Controls.Add(buttonStartService);
        tabServer.Controls.Add(numericServerPort);
        tabServer.Controls.Add(labelServerPort);
        tabServer.Location = new Point(4, 23);
        tabServer.Name = "tabServer";
        tabServer.Padding = new Padding(3);
        tabServer.Size = new Size(932, 75);
        tabServer.TabIndex = 0;
        tabServer.Text = "Server Mode";
        tabServer.UseVisualStyleBackColor = true;
        buttonStopService.Location = new Point(606, 11);
        buttonStopService.Name = "buttonStopService";
        buttonStopService.Size = new Size(194, 46);
        buttonStopService.TabIndex = 3;
        buttonStopService.Text = "Stop Service";
        buttonStopService.UseVisualStyleBackColor = true;
        buttonStopService.Click += buttonStopService_Click;
        buttonStartService.Location = new Point(392, 11);
        buttonStartService.Name = "buttonStartService";
        buttonStartService.Size = new Size(194, 46);
        buttonStartService.TabIndex = 2;
        buttonStartService.Text = "Start Service";
        buttonStartService.UseVisualStyleBackColor = true;
        buttonStartService.Click += buttonStartService_Click;
        numericServerPort.Location = new Point(187, 20);
        numericServerPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numericServerPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericServerPort.Name = "numericServerPort";
        numericServerPort.Size = new Size(98, 22);
        numericServerPort.TabIndex = 1;
        numericServerPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
        labelServerPort.AutoSize = true;
        labelServerPort.Location = new Point(36, 22);
        labelServerPort.Name = "labelServerPort";
        labelServerPort.Size = new Size(74, 14);
        labelServerPort.TabIndex = 0;
        labelServerPort.Text = "Listen on Port";
        tabClient.Controls.Add(buttonDisconnect);
        tabClient.Controls.Add(buttonConnect);
        tabClient.Controls.Add(numericClientPort);
        tabClient.Controls.Add(labelClientPort);
        tabClient.Controls.Add(comboServerIp);
        tabClient.Controls.Add(labelServerIp);
        tabClient.Location = new Point(4, 23);
        tabClient.Name = "tabClient";
        tabClient.Padding = new Padding(3);
        tabClient.Size = new Size(932, 75);
        tabClient.TabIndex = 1;
        tabClient.Text = "Client Mode";
        tabClient.UseVisualStyleBackColor = true;
        buttonDisconnect.Location = new Point(748, 11);
        buttonDisconnect.Name = "buttonDisconnect";
        buttonDisconnect.Size = new Size(162, 46);
        buttonDisconnect.TabIndex = 5;
        buttonDisconnect.Text = "Disconnect";
        buttonDisconnect.UseVisualStyleBackColor = true;
        buttonDisconnect.Click += buttonDisconnect_Click;
        buttonConnect.Location = new Point(580, 11);
        buttonConnect.Name = "buttonConnect";
        buttonConnect.Size = new Size(162, 46);
        buttonConnect.TabIndex = 4;
        buttonConnect.Text = "Connect";
        buttonConnect.UseVisualStyleBackColor = true;
        buttonConnect.Click += buttonConnect_Click;
        numericClientPort.Location = new Point(467, 20);
        numericClientPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numericClientPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericClientPort.Name = "numericClientPort";
        numericClientPort.Size = new Size(92, 22);
        numericClientPort.TabIndex = 3;
        numericClientPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
        labelClientPort.AutoSize = true;
        labelClientPort.Location = new Point(405, 22);
        labelClientPort.Name = "labelClientPort";
        labelClientPort.Size = new Size(28, 14);
        labelClientPort.TabIndex = 2;
        labelClientPort.Text = "Port";
        comboServerIp.FormattingEnabled = true;
        comboServerIp.Items.AddRange(new object[] { "127.0.0.1", "localhost" });
        comboServerIp.Location = new Point(173, 18);
        comboServerIp.Name = "comboServerIp";
        comboServerIp.Size = new Size(214, 22);
        comboServerIp.TabIndex = 1;
        comboServerIp.Text = "127.0.0.1";
        labelServerIp.AutoSize = true;
        labelServerIp.Location = new Point(36, 22);
        labelServerIp.Name = "labelServerIp";
        labelServerIp.Size = new Size(96, 14);
        labelServerIp.TabIndex = 0;
        labelServerIp.Text = "Connect to Server";
        richLog.Location = new Point(24, 286);
        richLog.Name = "richLog";
        richLog.Size = new Size(786, 316);
        richLog.TabIndex = 3;
        richLog.Text = "";
        panelRightButtons.Controls.Add(buttonExit);
        panelRightButtons.Controls.Add(buttonClear);
        panelRightButtons.Controls.Add(buttonSave);
        panelRightButtons.Controls.Add(buttonLoad);
        panelRightButtons.Location = new Point(826, 286);
        panelRightButtons.Name = "panelRightButtons";
        panelRightButtons.Size = new Size(150, 316);
        panelRightButtons.TabIndex = 4;
        buttonExit.Location = new Point(3, 241);
        buttonExit.Name = "buttonExit";
        buttonExit.Size = new Size(144, 70);
        buttonExit.TabIndex = 3;
        buttonExit.Text = "Exit";
        buttonExit.UseVisualStyleBackColor = true;
        buttonExit.Click += buttonExit_Click;
        buttonClear.Location = new Point(3, 161);
        buttonClear.Name = "buttonClear";
        buttonClear.Size = new Size(144, 70);
        buttonClear.TabIndex = 2;
        buttonClear.Text = "Clear";
        buttonClear.UseVisualStyleBackColor = true;
        buttonClear.Click += buttonClear_Click;
        buttonSave.Location = new Point(3, 81);
        buttonSave.Name = "buttonSave";
        buttonSave.Size = new Size(144, 70);
        buttonSave.TabIndex = 1;
        buttonSave.Text = "Save To File";
        buttonSave.UseVisualStyleBackColor = true;
        buttonSave.Click += buttonSave_Click;
        buttonLoad.Location = new Point(3, 1);
        buttonLoad.Name = "buttonLoad";
        buttonLoad.Size = new Size(144, 70);
        buttonLoad.TabIndex = 0;
        buttonLoad.Text = "Load From File";
        buttonLoad.UseVisualStyleBackColor = true;
        buttonLoad.Click += buttonLoad_Click;
        panelSend.Controls.Add(labelSend);
        panelSend.Controls.Add(textSend);
        panelSend.Controls.Add(buttonSend);
        panelSend.Location = new Point(24, 614);
        panelSend.Name = "panelSend";
        panelSend.Size = new Size(952, 57);
        panelSend.TabIndex = 5;
        labelSend.AutoSize = true;
        labelSend.Location = new Point(10, 17);
        labelSend.Name = "labelSend";
        labelSend.Size = new Size(67, 14);
        labelSend.TabIndex = 0;
        labelSend.Text = "Text to send";
        textSend.Location = new Point(116, 14);
        textSend.Name = "textSend";
        textSend.Size = new Size(686, 22);
        textSend.TabIndex = 1;
        buttonSend.Location = new Point(816, 8);
        buttonSend.Name = "buttonSend";
        buttonSend.Size = new Size(124, 39);
        buttonSend.TabIndex = 2;
        buttonSend.Text = "Send";
        buttonSend.UseVisualStyleBackColor = true;
        buttonSend.Click += buttonSend_Click;
        statusStripMain.ImageScalingSize = new Size(20, 20);
        statusStripMain.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStripMain.Location = new Point(0, 698);
        statusStripMain.Name = "statusStripMain";
        statusStripMain.Size = new Size(1000, 22);
        statusStripMain.TabIndex = 6;
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(40, 17);
        statusLabel.Text = "Status";
        AutoScaleDimensions = new SizeF(7F, 14F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 720);
        Controls.Add(statusStripMain);
        Controls.Add(panelSend);
        Controls.Add(panelRightButtons);
        Controls.Add(richLog);
        Controls.Add(groupMode);
        Controls.Add(labelTitle);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Simple Chat";
        Load += formMain_Load;
        groupMode.ResumeLayout(false);
        tabMode.ResumeLayout(false);
        tabServer.ResumeLayout(false);
        tabServer.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numericServerPort).EndInit();
        tabClient.ResumeLayout(false);
        tabClient.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numericClientPort).EndInit();
        panelRightButtons.ResumeLayout(false);
        panelSend.ResumeLayout(false);
        panelSend.PerformLayout();
        statusStripMain.ResumeLayout(false);
        statusStripMain.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label labelTitle;
    private GroupBox groupMode;
    private TabControl tabMode;
    private TabPage tabServer;
    private TabPage tabClient;
    private Label labelServerPort;
    private NumericUpDown numericServerPort;
    private Button buttonStartService;
    private Button buttonStopService;
    private Label labelServerIp;
    private ComboBox comboServerIp;
    private Label labelClientPort;
    private NumericUpDown numericClientPort;
    private Button buttonConnect;
    private Button buttonDisconnect;
    private RichTextBox richLog;
    private Panel panelRightButtons;
    private Button buttonLoad;
    private Button buttonSave;
    private Button buttonClear;
    private Button buttonExit;
    private Panel panelSend;
    private Label labelSend;
    private TextBox textSend;
    private Button buttonSend;
    private StatusStrip statusStripMain;
    private ToolStripStatusLabel statusLabel;
}
