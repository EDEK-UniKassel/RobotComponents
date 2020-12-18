// This file is part of RobotComponents. RobotComponents is licensed 
// under the terms of GNU General Public License as published by the 
// Free Software Foundation. For more information and the LICENSE file, 
// see <https://github.com/RobotComponents/RobotComponents>.

// System Libs
using System;
using System.Windows.Forms;
// Robot Components Libs
using ABB.Robotics.Controllers.IOSystemDomain;

namespace RobotComponents.Gh.Forms
{
    public partial class PickSignalForm : Form
    {
        public static int SignalIndex = 0;
        private static SignalCollection _signals;
        public PickSignalForm()
        {
            InitializeComponent();
        }

        public PickSignalForm(SignalCollection signals)
        {
            InitializeComponent();

            _signals = signals;

            for (int i = 0; i < _signals.Count; i++)
            {
                comboBox1.Items.Add(_signals[i].Name);
            }
        }

        private void PickSignal_Load(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelNameInfo.Text = _signals[comboBox1.SelectedIndex].Name.ToString();
            this.labelValueInfo.Text = _signals[comboBox1.SelectedIndex].Value.ToString();
            this.labelTypeInfo.Text = _signals[comboBox1.SelectedIndex].Type.ToString();
            this.labelMinValueInfo.Text = _signals[comboBox1.SelectedIndex].MinValue.ToString();
            this.labelMaxValueInfo.Text = _signals[comboBox1.SelectedIndex].MaxValue.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SignalIndex = comboBox1.SelectedIndex;
            this.Close();
        }
    }
}
