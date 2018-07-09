using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ParserNII.Attributes;
using ParserNII.DataStructures;
using ParserNII.Extensions;
using ZedGraph;
using Label = System.Windows.Forms.Label;

namespace ParserNII
{
    public partial class Form1 : Form
    {
        private List<string> paramNames = new List<string>();
        private List<TextBox> textBoxes = new List<TextBox>();
        private List<Label> labels = new List<Label>();
        private List<CheckBox> checkBoxes = new List<CheckBox>();
        private Dictionary<string, TextBox> uidNames = new Dictionary<string, TextBox>();
        private List<Panel> panels = new List<Panel>();

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
                {"Температура ТС ДУТ правая" },
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

            for (var i = 0; i < paramNames.Count; i++)
            {
                var textBox = new TextBox();
                textBoxes.Add(textBox);
                panel3.Controls.Add(textBox);
                textBox.Location = new Point(4, 4 + i * 26);
                textBox.Name = $"textBox{i}";
                textBox.ReadOnly = true;
                textBox.Size = new Size(82, 20);
                textBox.TabIndex = 30 + i;

                //var label = new Label();
                //labels.Add(label);
                //panel3.Controls.Add(label);
                //label.AutoSize = true;
                //label.Location = new System.Drawing.Point(120, 11 + i * 26);
                //label.Name = $"label{i}";
                //label.Size = new System.Drawing.Size(156, 13);
                //// label.TabIndex = 29 + i;
                //label.Text = paramNames[i];


                var checkBox = new CheckBox();
                panel3.Controls.Add(checkBox);
                checkBox.AutoSize = true;
                checkBox.Location = new Point(110, 6 + i * 26);
                checkBox.Name = $"checkBox{i}";
                checkBox.Size = new Size(80, 17);
                checkBox.TabIndex = 0;
                checkBox.Text = paramNames[i];
                checkBox.UseVisualStyleBackColor = true;
                checkBoxes.Add(checkBox);

                var panel = new Panel();
                panel3.Controls.Add(panel);
                panel.Location = new Point(90, 6 + i * 26);
                panel.Name = $"panel{i}";
                panel.Size = new Size(17, 17);
                panel.TabIndex = 0;
                panels.Add(panel);

                uidNames.Add(paramNames[i], textBoxes[i]);
            }
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
                    label2.Text = result[0].LabelType.ToString();
                    DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(result[0].UnixTime);
                    label4.Text = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm:ss");
                    label7.Text = result[0].LocomotiveType.ToString();
                    label6.Text = (stream.Length / 1024).ToString();
                    label10.Text = result[0].LocomotiveSection.ToString();
                    label12.Text = result[0].LocomotiveNumber.ToString();
                    DateTimeOffset startTime = DateTimeOffset.FromUnixTimeSeconds(result.First().UnixTime).AddHours(3);
                    DateTimeOffset endTime = DateTimeOffset.FromUnixTimeSeconds(result.Last().UnixTime).AddHours(3);

                    label40.Text = startTime.ToString("dd.MM.yyyy HH:mm:ss") + " - " + endTime.ToString("dd.MM.yyyy HH:mm:ss");

                    var arrayResult = parser.ToArray(result);

                    Type arrayResultType = arrayResult.GetType();
                    FieldInfo[] arrayResultFieldInfo = arrayResultType.GetFields();

                    //for (var i = 0; i < arrayResultFieldInfo.Length; i++)
                    //{
                    //    var fieldInfo = arrayResultFieldInfo[i];
                        
                    //}

                    var yValues = result.Select(r => DateTimeOffset.FromUnixTimeSeconds(r.UnixTime).AddHours(3)).ToList();
                    Color color;
                    Drawer.Initialize(zedGraphControl1);
                    int i = 0;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.FuelTemperature.Select(r => (double)r).ToList(),
                        "Температура топлива",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;

                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.ColdWaterCircuitTemperature.Select(r => (double)r).ToList(),
                        "Температура воды холодного контура",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.CoolingCircuitTemperature.Select(r => (double)r).ToList(),
                        "Температура контура охлаждения",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.DieselSpeed.Select(r => (double)r).ToList(),
                        "Коэффициент по оборотам дизеля",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.LeftDUTOffset.Select(r => (double)r).ToList(),
                        "Смещение ДУТ левого",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.RightDUTOffset.Select(r => (double)r).ToList(),
                        "Смещение ДУТ правого",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.LeftFuelVolume.Select(r => (double)r).ToList(),
                        "Объем топлива левого",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.FuelMass.Select(r => (double)r).ToList(),
                        "Масса топлива",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.MiddleFuelVolume.Select(r => (double)r).ToList(),
                        "Объем топлива средний",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;
                    Drawer.DrawGraph(zedGraphControl1, yValues,
                        arrayResult.FuelTemperature.Select(r => (double)r).ToList(),
                        "Температура топлива",
                        Drawer.GetColor(i));
                    panels[paramNames.IndexOf("Температура топлива")].BackColor = Drawer.GetColor(i);
                    i++;

                    zedGraphControl1.IsShowPointValues = true;
                    zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler((pointSender, graphPane, curve, pt) =>
                    {
                        Type resultType = result[pt].GetType();
                        FieldInfo[] fieldInfo = resultType.GetFields();

                        foreach (FieldInfo field in fieldInfo)
                        {
                            object[] attributes = field.GetCustomAttributes(typeof(ParamNameAttribute), false);

                            if (attributes.Length != 0)
                            {
                                ParamNameAttribute name = (ParamNameAttribute)attributes[0];

                                uidNames[name.Value].Text = field.GetValue(result[pt]).ToString();
                            }
                        }

                        return "";

                    });

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

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}

