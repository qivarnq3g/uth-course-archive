namespace BaitapUSCLN_GUI;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    static long findGcdBySubtraction(long a, long b)
    {
        while (a != b)
        {
            if (a > b)
            {
                a = a - b;
            }
            else
            {
                b = b - a;
            }
        }

        return a;
    }

    void calculateButton_Click(object? sender, EventArgs e)
    {
        string inputA = txtA.Text.Trim();
        string inputB = txtB.Text.Trim();

        if (inputA.Length == 0)
        {
            MessageBox.Show("Vui long nhap a.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtA.Focus();
            return;
        }

        if (inputB.Length == 0)
        {
            MessageBox.Show("Vui long nhap b.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtB.Focus();
            return;
        }

        for (int i = 0; i < inputA.Length; i++)
        {
            if (!char.IsDigit(inputA[i]))
            {
                MessageBox.Show("a chi duoc gom cac chu so.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA.Focus();
                return;
            }
        }

        for (int i = 0; i < inputB.Length; i++)
        {
            if (!char.IsDigit(inputB[i]))
            {
                MessageBox.Show("b chi duoc gom cac chu so.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtB.Focus();
                return;
            }
        }

        if (!long.TryParse(inputA, out long a) || a <= 0)
        {
            MessageBox.Show("a khong hop le.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtA.Focus();
            return;
        }

        if (!long.TryParse(inputB, out long b) || b <= 0)
        {
            MessageBox.Show("b khong hop le.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtB.Focus();
            return;
        }

        long uscln = findGcdBySubtraction(a, b);

        txtUscln.Text = uscln.ToString();
    }

    void closeButton_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
