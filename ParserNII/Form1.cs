using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using ParserNII.DataStructures;
using ZedGraph;

namespace ParserNII
{
    public partial class Form1 : Form
    {
        private List<TextBox> textBoxes;
        private List<CheckBox> checkBoxes;
        private Dictionary<string, TextBox> uidNames;
        private List<Panel> panels;
        private Dictionary<string, int> DisplayedParamNames;
        private List<ZedGraphControl.PointValueHandler> pointEventHandlers = new List<ZedGraphControl.PointValueHandler>();
        private readonly Config config;
        private LineObj verticalLine;


        public Form1()
        {
            InitializeComponent();
            button1.Visible = false;
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Environment.CurrentDirectory + "/config.json"));
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] fileBytes = new byte[stream.Length];
                stream.Read(fileBytes, 0, (int)stream.Length);
                label6.Text = (stream.Length / 1024).ToString();
                var parser = Path.GetExtension(ofd.FileName) == ".dat" ? (Parser)new DatFileParser() : new BinFileParser();
                List<DataFile> result = parser.Parse(fileBytes);

                foreach (var pointEventHandler in pointEventHandlers)
                {
                    zedGraphControl1.PointValueEvent -= pointEventHandler;
                }

                pointEventHandlers = new List<ZedGraphControl.PointValueHandler>();

                List<XDate> xValues;
                if (Path.GetExtension(ofd.FileName) == ".dat")
                {
                    xValues = result.Select(r => new XDate(DateTimeOffset.FromUnixTimeSeconds((uint)r.Data["Время в “UNIX” формате"].OriginalValue).AddHours(3).DateTime)).ToList();

                    label2.Text = result[0].Data["Тип метки"].DisplayValue;
                    label4.Text = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss");
                    label7.Text = result[0].Data["Тип локомотива"].DisplayValue;
                    label10.Text = TrainNames.NamesDictionary[(byte)result[0].Data["Тип локомотива"].OriginalValue];
                    label12.Text = result[0].Data["№ тепловоза"].DisplayValue;
                }
                else
                {
                    xValues = result.Select(r => new XDate(DateTimeOffset.FromUnixTimeMilliseconds((long)r.Data["Время в “UNIX” формате"].OriginalValue).AddHours(3).DateTime)).ToList();
                    string[] nameParams = Path.GetFileName(ofd.FileName).Split('_', '-');
                    label10.Text = nameParams[2];
                    label12.Text = nameParams[3];
                    label7.Text = nameParams[4];
                }

                label40.Text = result.First().Data["Время в “UNIX” формате"].DisplayValue + " - " + result.Last().Data["Время в “UNIX” формате"].DisplayValue;

                var arrayResult = parser.ToArray(result);

                DisplayPanelElements(result[0]);
                Drawer drawer = new Drawer(zedGraphControl1);

                var keys = result[0].Data.Keys.Where(k => result[0].Data[k].Display).ToArray();

                for (int i = 0; i < keys.Length; i++)
                {
                    drawer.DrawGraph(xValues,
                        arrayResult.Data[keys[i]].Select(d => d.ChartValue).ToList(),
                        keys[i],
                        Drawer.GetColor(i));

                    panels[DisplayedParamNames[keys[i]]].BackColor = Drawer.GetColor(i);
                }

                verticalLine = drawer.CrateVerticalLine();

                zedGraphControl1.IsShowPointValues = true;

                pointEventHandlers.Add((pointSender, graphPane, curve, pt) =>
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        uidNames[keys[i]].Text = result[pt].Data[keys[i]].DisplayValue;
                    }

                    verticalLine.Location.X = xValues[pt];
                    verticalLine.Location.X1 = xValues[pt];
                    zedGraphControl1.Refresh();
                    return "";
                });

                zedGraphControl1.PointValueEvent += pointEventHandlers.Last();


                drawer.Refresh();

                stream.Close();
            }
        }

        private void DisplayPanelElements(DataFile data)
        {
            panel3.Controls.Clear();
            DisplayedParamNames = new Dictionary<string, int>();
            textBoxes = new List<TextBox>();
            checkBoxes = new List<CheckBox>();
            panels = new List<Panel>();
            uidNames = new Dictionary<string, TextBox>();
            var keys = data.Data.Keys.Where(k => data.Data[k].Display).ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                DisplayedParamNames.Add(keys[i], i);

                var textBox = new TextBox();
                textBoxes.Add(textBox);
                panel3.Controls.Add(textBox);
                textBox.Location = new Point(2, 4 + i * 26);
                textBox.Name = $"textBox{i}";
                textBox.ReadOnly = true;
                textBox.Size = new Size(82, 20);
                textBox.TabIndex = 30 + i;

                var checkBox = new CheckBox();
                panel3.Controls.Add(checkBox);
                checkBox.AutoSize = true;
                checkBox.Location = new Point(108, 6 + i * 26);
                checkBox.Name = $"checkBox{i}";
                checkBox.Size = new Size(80, 17);
                checkBox.TabIndex = 0;
                checkBox.Text = keys[i];
                checkBox.UseVisualStyleBackColor = true;
                checkBoxes.Add(checkBox);
                checkBox.Checked = true;
                int index = i;

                checkBox.CheckedChanged += (object sender, EventArgs e) =>
                {
                    if (zedGraphControl1.GraphPane.CurveList[index].IsVisible != checkBox.Checked)
                    {
                        zedGraphControl1.GraphPane.CurveList[index].IsVisible = checkBox.Checked;

                        if (checkBox.Checked)
                        {
                            zedGraphControl1.AxisChange();
                            zedGraphControl1.Refresh();
                        }
                        else
                        {
                            zedGraphControl1.Refresh();
                        }
                    }
                };

                var panel = new Panel();
                panel3.Controls.Add(panel);
                panel.Location = new Point(88, 6 + i * 26);
                panel.Name = $"panel{i}";
                panel.Size = new Size(17, 17);
                panel.TabIndex = 0;
                panels.Add(panel);

                uidNames.Add(keys[i], textBoxes[i]);
            }

            button1.Visible = true;
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.CurveList.ForEach(c => c.IsVisible = false);

            zedGraphControl1.Refresh();

            checkBoxes.ForEach(cb => cb.Checked = false);
        }
    }
}
