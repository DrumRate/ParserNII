using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ParserNII.DataStructures;
using ZedGraph;
using Label = System.Windows.Forms.Label;

namespace ParserNII
{
    public partial class Form1 : Form
    {
        private List<string> paramNames = new List<string>();
        private List<TextBox> textBoxes = new List<TextBox>();
        private List<CheckBox> checkBoxes = new List<CheckBox>();
        private Dictionary<string, TextBox> uidNames = new Dictionary<string, TextBox>();
        private List<Panel> panels = new List<Panel>();
        private List<string> DisplayedParamNames;
        private List<LineItem> linesClones = new List<LineItem>();
        

        public Form1()
        {
            InitializeComponent();
            paramNames = new List<string>
            {
                { "Температура контура охлаждения" },
                { "Табельный номер"},
                { "Скорость по GPS"},
                { "Широта"},
                { "Долгота"},
                { "Обороты дизеля"},
                { "Коэффициент по оборотам"},
                { "Коэффициент по оборотам дизеля"},
                { "Давление в топливной системе"},
                { "Давление в масляной системе"},
                { "Давление в масляной системе 2"},
                { "Давление наддувочного воздуха"},
                { "Обороты турбокомпрессора"},
                { "Температура воды на выходе дизеля"},
                { "Температура масла на выходе масла"},
                { "Сила тока главного генератора"},
                { "Коэффициент по току"},
                { "Напряжение главного генератора"},
                { "Коэффициент по напряжению"},
                { "Мощность главного генератора" },
                { "Позиция контроллера машиниста" },
                { "ДУТ поплавковый"},
                { "Объем топлива левый" },
                { "Объем топлива правый" },
                { "Объем топлива секундный" },
                { "Объем топлива средний" },
                { "Объем топлива" },
                { "Температура топлива левая" },
                { "Температура топлива правая" },
                { "Температура топлива" },
                { "Масса топлива" },
                { "Объем экипированного топлива"},
                { "Признак экипировки" },
                { "Температура окружающего воздуха"},
                { "Давление в тормозной магистрали"},
                { "ЭПК"},
                { "Код позиции"},
                { "Код АЛСН"},
                { "Напряжение АКБ"},
                { "Ток зар./разр. АКБ"},
                { "Контрольный режим"},
                { "Связь с РМ РКД" },
                { "Связь с ДУТ левая" },
                { "Связь с ДУТ правая" },
                { "Связь с ДМ" },
                { "Связь с БДП" },
                { "Данные GPS валидны" },
                { "Подключение к ЕСМБС" },
                { "Связь с МЭК" },
                { "Работа САЗДТ" },
                { "Температура ТС ДУТ левая"},
                { "Температура ТС ДУТ правая" },
                { "Плотность топлива текущая"},
                { "Плотность топлива при 20 С"},
                { "Температура контура масла"},
                { "Напряжение ИБП" },
                { "Коэффициент по ТК" },
                { "Объем экипировки" },
                { "Смещение ДУТ левого" },
                { "Смещение ДУТ правого" },
                { "Температура воды холодного контура"},
                { "Плотность топлива при экипировке"},
                { "Версия БИ"},
            };


        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
                if (Path.GetExtension(ofd.FileName) == ".dat")
                {
                    DatFileParser parser = new DatFileParser();
                    var result = parser.Parse(stream);
                    label2.Text = result[0].data["Тип метки"].DisplayValue;
                    //DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(result[0].UnixTime);
                    label4.Text = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss");
                    label7.Text = result[0].data["Тип локомотива"].DisplayValue;
                    label6.Text = (stream.Length / 1024).ToString();
                    label10.Text = result[0].data["Секция локомотива"].DisplayValue;
                    label12.Text = result[0].data["№ тепловоза"].DisplayValue;

                    label40.Text = result.First().data["Время в “UNIX” формате"].DisplayValue + " - " + result.Last().data["Время в “UNIX” формате"].DisplayValue;

                    var arrayResult = parser.ToArray(result);

                    DisplayPanelElements(result[0]);

                    var xValues = result.Select(r => DateTimeOffset.FromUnixTimeSeconds((uint)r.data["Время в “UNIX” формате"].OriginalValue).AddHours(3)).ToList();

                    Drawer.Initialize(zedGraphControl1);
                    var keys = result[0].data.Keys.Where(k => result[0].data[k].Display).ToArray();

                    for (int i = 0; i < keys.Length; i++)
                    {
                        Drawer.DrawGraph(zedGraphControl1, xValues,
                            arrayResult.data[keys[i]].Select(d => d.ChartValue).ToList(),
                            keys[i],
                            Drawer.GetColor(i));

                        linesClones.Add(((LineItem)zedGraphControl1.GraphPane.CurveList.Last()).Clone());

                        panels[DisplayedParamNames.IndexOf(keys[i])].BackColor = Drawer.GetColor(i);
                    }

                    zedGraphControl1.IsShowPointValues = true;
                    zedGraphControl1.PointValueEvent += (pointSender, graphPane, curve, pt) =>
                    {

                        for (int i = 0; i < keys.Length; i++)
                        {
                            uidNames[keys[i]].Text = result[pt].data[keys[i]].DisplayValue;
                        }

                        return "";

                    };

                }
                else
                {
                    BinFileParser parser = new BinFileParser();
                    var result = parser.Parse(stream);
                    foreach (var binFile in result)
                    {
                        var date = DateTimeOffset.FromUnixTimeMilliseconds(binFile.Date);
                    }

                    var dictionaryResult = parser.ToDictionary(result);

                }

                stream.Close();
            }
        }

        private void DisplayPanelElements(DataFile data)
        {
            DisplayedParamNames = new List<string>();

            var keys = data.data.Keys.Where(k => data.data[k].Display).ToArray();

            for (int i = 0; i < keys.Length; i++)
            {

                DisplayedParamNames.Add(keys[i]);

                var textBox = new TextBox();
                textBoxes.Add(textBox);
                panel3.Controls.Add(textBox);
                textBox.Location = new Point(4, 4 + i * 26);
                textBox.Name = $"textBox{i}";
                textBox.ReadOnly = true;
                textBox.Size = new Size(82, 20);
                textBox.TabIndex = 30 + i;

                var checkBox = new CheckBox();
                panel3.Controls.Add(checkBox);
                checkBox.AutoSize = true;
                checkBox.Location = new Point(110, 6 + i * 26);
                checkBox.Name = $"checkBox{i}";
                checkBox.Size = new Size(80, 17);
                checkBox.TabIndex = 0;
                checkBox.Text = DisplayedParamNames[i];
                checkBox.UseVisualStyleBackColor = true;
                checkBoxes.Add(checkBox);
                checkBox.Checked = true;
                int index = i;
                
                checkBox.CheckedChanged += (object sender, EventArgs e) =>
                {
                    
                    if (!checkBox.Checked)
                    {
                        zedGraphControl1.GraphPane.CurveList[index].Clear();
                        zedGraphControl1.Refresh();
                    }
                    else
                    {
                        zedGraphControl1.GraphPane.CurveList[index] = linesClones[index];
                        zedGraphControl1.Refresh();
                    }
                };

                var panel = new Panel();
                panel3.Controls.Add(panel);
                panel.Location = new Point(90, 6 + i * 26);
                panel.Name = $"panel{i}";
                panel.Size = new Size(17, 17);
                panel.TabIndex = 0;
                panels.Add(panel);

                uidNames.Add(DisplayedParamNames[i], textBoxes[i]);

            }
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

