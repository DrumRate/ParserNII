using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ParserNII.DataStructures;

namespace ParserNII
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                DatFileParser parser = new DatFileParser();
                var result = parser.Parse(stream);
                label2.Text = result.LabelType.ToString();
                DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(result.UnixTime);
                label4.Text = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss");
                label7.Text = result.LocomotiveType.ToString();
                label6.Text = (stream.Length / 1024).ToString();
                label10.Text = result.LocomotiveSection.ToString();
                label12.Text = result.LocomotiveNumber.ToString();

                panel1.Text = result.ColdWaterCircuitTemperature.ToString();
                stream.Close();
            }
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}

