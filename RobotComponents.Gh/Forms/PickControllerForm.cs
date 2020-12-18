// This file is part of RobotComponents. RobotComponents is licensed 
// under the terms of GNU General Public License as published by the 
// Free Software Foundation. For more information and the LICENSE file, 
// see <https://github.com/RobotComponents/RobotComponents>.

// System Libs
using System;
using System.Windows.Forms;
// ABB Libs
using ABB.Robotics.Controllers;

namespace RobotComponents.Gh.Forms
{
    public partial class PickControllerForm : Form
    {
        public static int StationIndex = 0;
        private static ControllerInfo[] _controllers;

        public PickControllerForm()
        {
            InitializeComponent();
        }

        public PickControllerForm(ControllerInfo[] controllers)
        {
            InitializeComponent();

            _controllers = controllers;
            
            for (int i = 0; i < _controllers.Length; i++)
            {
                comboBox1.Items.Add(_controllers[i].Name);
            }
        }

        private void PickController_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            StationIndex = comboBox1.SelectedIndex;
            this.Close();
        }

        private void ComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            this.labelNameInfo.Text = _controllers[comboBox1.SelectedIndex].Name.ToString();
            this.labelSystemNameInfo.Text = _controllers[comboBox1.SelectedIndex].SystemName.ToString();
            this.labelIPInfo.Text = _controllers[comboBox1.SelectedIndex].IPAddress.ToString();
            this.labelIsVirtualInfo.Text = _controllers[comboBox1.SelectedIndex].IsVirtual.ToString();
            this.labelVersionInfo.Text = _controllers[comboBox1.SelectedIndex].Version.ToString();
        }
    }
}
