using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace XmlSettings {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void FillCombo() {
            foreach (var item in PrinterSettings.InstalledPrinters) {
                comboBox1.Items.Add(item);
                comboBox2.Items.Add(item);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            FillCombo();
        }

        private void button1_Click(object sender, EventArgs e) {
            Settings.Printer2 = Convert.ToString(comboBox2.SelectedItem);
            Settings.Printer1 = Convert.ToString(comboBox1.SelectedItem);
            Settings.DesignName = "Design 1";
            Settings.Preview = checkBox1.Checked;
            Settings.SaveSettings();
        }

        private void button2_Click(object sender, EventArgs e) {
            //Settings settings = new Settings();
            Settings.GetSettings();
            comboBox1.SelectedItem = Settings.Printer1;
            comboBox2.SelectedItem = Settings.Printer2;
            checkBox1.Checked = Settings.Preview;
        }
    }
}