namespace _066206005044_BT42_TimeServerGUIUDP;

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
        labelPort = new Label();
        numericPort = new NumericUpDown();
        buttonStart = new Button();
        buttonStop = new Button();
        buttonExit = new Button();
        ((System.ComponentModel.ISupportInitialize)numericPort).BeginInit();
        SuspendLayout();
        labelPort.AutoSize = true;
        labelPort.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelPort.Location = new Point(84, 85);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(140, 37);
        labelPort.TabIndex = 0;
        labelPort.Text = "Chon port";
        numericPort.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numericPort.Location = new Point(258, 82);
        numericPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numericPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericPort.Name = "numericPort";
        numericPort.Size = new Size(134, 43);
        numericPort.TabIndex = 1;
        numericPort.Value = new decimal(new int[] { 9000, 0, 0, 0 });
        buttonStart.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        buttonStart.Location = new Point(64, 188);
        buttonStart.Name = "buttonStart";
        buttonStart.Size = new Size(180, 70);
        buttonStart.TabIndex = 2;
        buttonStart.Text = "Chay";
        buttonStart.UseVisualStyleBackColor = true;
        buttonStart.Click += buttonStart_Click;
        buttonStop.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        buttonStop.Location = new Point(296, 188);
        buttonStop.Name = "buttonStop";
        buttonStop.Size = new Size(180, 70);
        buttonStop.TabIndex = 3;
        buttonStop.Text = "Dung";
        buttonStop.UseVisualStyleBackColor = true;
        buttonStop.Click += buttonStop_Click;
        buttonExit.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
        buttonExit.Location = new Point(180, 282);
        buttonExit.Name = "buttonExit";
        buttonExit.Size = new Size(180, 70);
        buttonExit.TabIndex = 4;
        buttonExit.Text = "Thoat";
        buttonExit.UseVisualStyleBackColor = true;
        buttonExit.Click += buttonExit_Click;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(540, 390);
        Controls.Add(buttonExit);
        Controls.Add(buttonStop);
        Controls.Add(buttonStart);
        Controls.Add(numericPort);
        Controls.Add(labelPort);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Time UDP Server";
        FormClosing += formMain_FormClosing;
        Load += formMain_Load;
        ((System.ComponentModel.ISupportInitialize)numericPort).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label labelPort;
    private NumericUpDown numericPort;
    private Button buttonStart;
    private Button buttonStop;
    private Button buttonExit;
}
