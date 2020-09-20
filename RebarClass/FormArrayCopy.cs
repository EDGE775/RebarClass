using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarClass
{
    public partial class FormArrayCopy : Form
    {
        public int CountX;
        public double StepX;
        public int CountY;
        public double StepY;

        public FormArrayCopy()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int.TryParse(textBoxCountX.Text, out CountX);
            int.TryParse(textBoxCountY.Text, out CountY);
            double.TryParse(textBoxStepX.Text, out StepX);
            double.TryParse(textBoxStepY.Text, out StepY);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
