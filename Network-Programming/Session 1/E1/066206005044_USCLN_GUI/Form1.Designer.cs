namespace BaitapUSCLN_GUI;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblA = new Label();
        txtA = new TextBox();
        lblB = new Label();
        txtB = new TextBox();
        lblUscln = new Label();
        txtUscln = new TextBox();
        btnTinh = new Button();
        btnDong = new Button();
        SuspendLayout();
        lblA.AutoSize = true;
        lblA.Location = new Point(28, 31);
        lblA.Name = "lblA";
        lblA.Size = new Size(15, 20);
        lblA.TabIndex = 0;
        lblA.Text = "a";
        txtA.Location = new Point(88, 28);
        txtA.MaxLength = 12;
        txtA.Name = "txtA";
        txtA.Size = new Size(130, 27);
        txtA.TabIndex = 1;
        txtA.Text = "";
        lblB.AutoSize = true;
        lblB.Location = new Point(28, 75);
        lblB.Name = "lblB";
        lblB.Size = new Size(15, 20);
        lblB.TabIndex = 2;
        lblB.Text = "b";
        txtB.Location = new Point(88, 72);
        txtB.MaxLength = 12;
        txtB.Name = "txtB";
        txtB.Size = new Size(130, 27);
        txtB.TabIndex = 3;
        txtB.Text = "";
        lblUscln.AutoSize = true;
        lblUscln.Location = new Point(28, 119);
        lblUscln.Name = "lblUscln";
        lblUscln.Size = new Size(53, 20);
        lblUscln.TabIndex = 4;
        lblUscln.Text = "USCLN";
        txtUscln.Location = new Point(88, 116);
        txtUscln.Name = "txtUscln";
        txtUscln.ReadOnly = true;
        txtUscln.Size = new Size(130, 27);
        txtUscln.TabIndex = 5;
        btnTinh.Location = new Point(52, 162);
        btnTinh.Name = "btnTinh";
        btnTinh.Size = new Size(64, 30);
        btnTinh.TabIndex = 6;
        btnTinh.Text = "Tinh";
        btnTinh.UseVisualStyleBackColor = true;
        btnTinh.Click += calculateButton_Click;
        btnDong.Location = new Point(154, 162);
        btnDong.Name = "btnDong";
        btnDong.Size = new Size(64, 30);
        btnDong.TabIndex = 7;
        btnDong.Text = "Dong";
        btnDong.UseVisualStyleBackColor = true;
        btnDong.Click += closeButton_Click;
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(254, 216);
        Controls.Add(btnDong);
        Controls.Add(btnTinh);
        Controls.Add(txtUscln);
        Controls.Add(lblUscln);
        Controls.Add(txtB);
        Controls.Add(lblB);
        Controls.Add(txtA);
        Controls.Add(lblA);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tim USCLN";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblA;
    private TextBox txtA;
    private Label lblB;
    private TextBox txtB;
    private Label lblUscln;
    private TextBox txtUscln;
    private Button btnTinh;
    private Button btnDong;
}
