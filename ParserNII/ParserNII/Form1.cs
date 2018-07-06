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
                if (Path.GetExtension(ofd.FileName) == ".dat"){
                    DatFileParser parser = new DatFileParser();
                    var result = parser.Parse(stream);
                    label2.Text = result[0].LabelType.ToString();
                    DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(result[0].UnixTime);
                    label4.Text = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss");
                    label7.Text = result[0].LocomotiveType.ToString();
                    label6.Text = (stream.Length / 1024).ToString();
                    label10.Text = result[0].LocomotiveSection.ToString();
                    label12.Text = result[0].LocomotiveNumber.ToString();

                    panel1.Text = result[0].ColdWaterCircuitTemperature.ToString();
                }
                else
                {
                    BinFileParser parser = new BinFileParser();
                    var result = parser.Parse(stream);
                    foreach (var binFile in result)
                    {
                        var date = DateTimeOffset.FromUnixTimeSeconds(binFile.Date);
                    }

                }
              
                stream.Close();
            }
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}

