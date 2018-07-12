using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ParserNII.DataStructures;
using ZedGraph;

namespace ParserNII
{
    public partial class Form1 : Form
    {
        private List<string> paramNames;
        private List<TextBox> textBoxes;
        private List<CheckBox> checkBoxes;
        private Dictionary<string, TextBox> uidNames;
        private List<Panel> panels;
        private List<string> DisplayedParamNames;
        private List<ZedGraphControl.PointValueHandler> pointEventHandlers = new List<ZedGraphControl.PointValueHandler>();


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
                Stream stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var parser = Path.GetExtension(ofd.FileName) == ".dat" ? (Parser)new DatFileParser() : new BinFileParser();
                List<DataFile> result = parser.Parse(stream);
                

                foreach (var pointEventHandler in pointEventHandlers)
                {
                    zedGraphControl1.PointValueEvent -= pointEventHandler;
                }

                pointEventHandlers = new List<ZedGraphControl.PointValueHandler>();

                List<DateTimeOffset> xValues;
                if (Path.GetExtension(ofd.FileName) == ".dat")
                {
                    xValues = result.Select(r => DateTimeOffset.FromUnixTimeSeconds((uint)r.Data["Время в “UNIX” формате"].OriginalValue).AddHours(3)).ToList();

                    label2.Text = result[0].Data["Тип метки"].DisplayValue;
                    label4.Text = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss");
                    label7.Text = result[0].Data["Тип локомотива"].DisplayValue;
                    label6.Text = (stream.Length / 1024).ToString();
                    label10.Text = result[0].Data["Секция локомотива"].DisplayValue;
                    label12.Text = result[0].Data["№ тепловоза"].DisplayValue;

                    label40.Text = result.First().Data["Время в “UNIX” формате"].DisplayValue + " - " + result.Last().Data["Время в “UNIX” формате"].DisplayValue;
                }
                else
                {
                    xValues = result.Select(r => DateTimeOffset.FromUnixTimeMilliseconds((long)r.Data["Время в “UNIX” формате"].OriginalValue).AddHours(3)).ToList();
                }

                var arrayResult = parser.ToArray(result);

                DisplayPanelElements(result[0]);



                Drawer.Initialize(zedGraphControl1);
                var keys = result[0].Data.Keys.Where(k => result[0].Data[k].Display).ToArray();

                for (int i = 0; i < keys.Length; i++)
                {
                    Drawer.DrawGraph(zedGraphControl1, xValues,
                        arrayResult.Data[keys[i]].Select(d => d.ChartValue).ToList(),
                        keys[i],
                        Drawer.GetColor(i));

                    panels[DisplayedParamNames.IndexOf(keys[i])].BackColor = Drawer.GetColor(i);
                }

                zedGraphControl1.IsShowPointValues = true;

                pointEventHandlers.Add((pointSender, graphPane, curve, pt) =>
                {

                    for (int i = 0; i < keys.Length; i++)
                    {
                        uidNames[keys[i]].Text = result[pt].Data[keys[i]].DisplayValue;
                    }

                    return "";

                });
                zedGraphControl1.PointValueEvent += pointEventHandlers.Last();

                stream.Close();
            }
        }

        private void DisplayPanelElements(DataFile data)
        {
            panel3.Controls.Clear();
            DisplayedParamNames = new List<string>();
            textBoxes = new List<TextBox>();
            checkBoxes = new List<CheckBox>();
            panels = new List<Panel>();
            uidNames = new Dictionary<string, TextBox>();
            var keys = data.Data.Keys.Where(k => data.Data[k].Display).ToArray();

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
                    zedGraphControl1.GraphPane.CurveList[index].IsVisible = checkBox.Checked;
                    zedGraphControl1.Refresh();
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

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.Checked = false;
            }
        }
    }
}

