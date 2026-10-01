namespace _066206005044_BT42_TimeClientGUIUDP;

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
        labelIp = new Label();
        textIp = new TextBox();
        labelPort = new Label();
        numericPort = new NumericUpDown();
        labelRequest = new Label();
        comboRequest = new ComboBox();
        buttonSendRequest = new Button();
        buttonExit = new Button();
        ((System.ComponentModel.ISupportInitialize)numericPort).BeginInit();
        SuspendLayout();
        labelTitle.AutoSize = true;
        labelTitle.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelTitle.Location = new Point(45, 28);
        labelTitle.Name = "labelTitle";
        labelTitle.Size = new Size(286, 45);
        labelTitle.TabIndex = 0;
        labelTitle.Text = "Yeu cau tu server";
        labelIp.AutoSize = true;
        labelIp.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelIp.Location = new Point(48, 108);
        labelIp.Name = "labelIp";
        labelIp.Size = new Size(41, 37);
        labelIp.TabIndex = 1;
        labelIp.Text = "IP";
        textIp.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        textIp.Location = new Point(108, 103);
        textIp.Name = "textIp";
        textIp.Size = new Size(220, 43);
        textIp.TabIndex = 2;
        textIp.Text = "127.0.0.1";
        labelPort.AutoSize = true;
        labelPort.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelPort.Location = new Point(386, 108);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(74, 37);
        labelPort.TabIndex = 3;
        labelPort.Text = "Port";
        numericPort.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numericPort.Location = new Point(477, 103);
        numericPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numericPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericPort.Name = "numericPort";
        numericPort.Size = new Size(128, 43);
        numericPort.TabIndex = 4;
        numericPort.Value = new decimal(new int[] { 9000, 0, 0, 0 });
        labelRequest.AutoSize = true;
        labelRequest.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelRequest.Location = new Point(48, 184);
        labelRequest.Name = "labelRequest";
        labelRequest.Size = new Size(122, 37);
        labelRequest.TabIndex = 5;
        labelRequest.Text = "Yeu cau";
        comboRequest.DropDownStyle = ComboBoxStyle.DropDownList;
        comboRequest.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        comboRequest.FormattingEnabled = true;
        comboRequest.Items.AddRange(new object[] { "1 - Gio", "2 - Ngay", "3 - Ngay + Gio" });
        comboRequest.Location = new Point(186, 179);
        comboRequest.Name = "comboRequest";
        comboRequest.Size = new Size(420, 45);
        comboRequest.TabIndex = 6;
        buttonSendRequest.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        buttonSendRequest.Location = new Point(64, 286);
        buttonSendRequest.Name = "buttonSendRequest";
        buttonSendRequest.Size = new Size(230, 72);
        buttonSendRequest.TabIndex = 7;
        buttonSendRequest.Text = "Gui yeu cau";
        buttonSendRequest.UseVisualStyleBackColor = true;
        buttonSendRequest.Click += buttonSendRequest_Click;
        buttonExit.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        buttonExit.Location = new Point(385, 286);
        buttonExit.Name = "buttonExit";
        buttonExit.Size = new Size(220, 72);
        buttonExit.TabIndex = 8;
        buttonExit.Text = "Thoat";
        buttonExit.UseVisualStyleBackColor = true;
        buttonExit.Click += buttonExit_Click;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(672, 404);
        Controls.Add(buttonExit);
        Controls.Add(buttonSendRequest);
        Controls.Add(comboRequest);
        Controls.Add(labelRequest);
        Controls.Add(numericPort);
        Controls.Add(labelPort);
        Controls.Add(textIp);
        Controls.Add(labelIp);
        Controls.Add(labelTitle);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "UDP Client";
        FormClosing += formMain_FormClosing;
        Load += formMain_Load;
        ((System.ComponentModel.ISupportInitialize)numericPort).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label labelTitle;
    private Label labelIp;
    private TextBox textIp;
    private Label labelPort;
    private NumericUpDown numericPort;
    private Label labelRequest;
    private ComboBox comboRequest;
    private Button buttonSendRequest;
    private Button buttonExit;
}
