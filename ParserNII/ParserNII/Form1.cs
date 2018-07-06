using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ParserNII.DataStructures;
using ZedGraph;

namespace ParserNII
{
    public partial class Form1 : Form
    {
        private Dictionary<PointPair, string> pointsDictionary = new Dictionary<PointPair, string>();
        private Dictionary<string, TextBox> uidNames;

        public Form1()
        {
            InitializeComponent();

            uidNames = new Dictionary<string, TextBox>
            {
                { "Табельный номер", textBox1},
                { "Скорость по GPS", textBox1},
                { "Широта", textBox1},
                { "Долгота", textBox1},
                { "Обороты дизеля", textBox1},
                { "Коэффициен по оборотам", textBox1},
                { "Давление в топливной системе", textBox1},
                { "Давление в масляной системе", textBox1},
                { "Давление в масляной системе 2", textBox1},
                { "Давление наддувочного воздуха", textBox1},
                { "Обороты турбокомпрессора", textBox1},
                { "Температура воды на выходе дизеля", textBox1},
                { "Температура масла на выходе масла", textBox1},
                { "Сила тока главного генератора", textBox1},
                { "эффициент по току", textBox1},
                { "Напряжение главного генератора", textBox1},
                { "Коэффициент по напряжению", textBox1},
                { "Мощность главного генератора", textBox1 },
                { "Позиция контроллера машиниста", textBox1 },
                { "ДУТ поплавковый", textBox1},
                { "Объем топлива левый", textBox1 },
                { "Объем топлива правый", textBox1 },
                { "Объем топлива секундный", textBox1 },
                { "Объем топлива", textBox1 },
                { "Плотность топлива", textBox1},
                { "Температура топлива левая", textBox1 },
                { "Температура топлива правая", textBox1 },
                { "Температура топлива", textBox1 },
                { "Масса топлива", textBox1 },
                { "Объем экипированного топлива", textBox1},
                { "Признак экипировки", textBox1 },
                { "Температура окружающего воздуха", textBox1},
                { "Давление в тормозной магистрали", textBox1},
                { "ЭПК", textBox1},
                { "Код позиции", textBox1},
                { "Код АЛСН", textBox1},
                { "Напряжение АКБ", textBox1},
                { "Ток зар./разр. АКБ", textBox1},
                { "Контрольный режим", textBox1},
                { "Версия БИ", textBox1},
                { "Связь с РМ РКД", textBox1 },
                { "Связь с ДУТ левый", textBox1 },
                { "Связь с ДУТ правый", textBox1 },
                { "Связь с ДМ", textBox1 },
                { "Связь с БДП", textBox1 },
                { "Данные GPS валидны", textBox1 },
                { "Подключение к ЕСМБС", textBox1 },
                { "Связь с МЭК", textBox1 },
                { "Работа САЗДТ", textBox1 }
            };
            
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
                    DateTimeOffset startTime = DateTimeOffset.FromUnixTimeSeconds(result.First().UnixTime).AddHours(3);
                    DateTimeOffset endTime = DateTimeOffset.FromUnixTimeSeconds(result.Last().UnixTime).AddHours(3);

                    label40.Text = startTime.ToString("dd.MM.yyyy HH:mm:ss") + " - " + endTime.ToString("dd.MM.yyyy HH:mm:ss");

                    textBox1.Text = result[0].ColdWaterCircuitTemperature.ToString();
                    var points = Drawer.DrawGraph(zedGraphControl1, 
                        result.Select(r => DateTimeOffset.FromUnixTimeSeconds(r.UnixTime).AddHours(3)).ToList(), 
                        result.Select(r => (double)r.FuelTemperature).ToList(),
                        "Температура топлива",
                        "test2",
                        Drawer.GetColor(5));

                    foreach (var point in points)
                    {
                        pointsDictionary.Add(point.Key, point.Value);
                    }
                   
                    zedGraphControl1.IsShowPointValues = true;
                    zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler((pointSender, graphPane, curve, pt) =>
                    {
                        
                        PointPair point = curve[pt];
                        var name = pointsDictionary.Single(p => p.Key.Y == point.Y && p.Key.X == point.X).Value;
                        uidNames[name].Text = point.Y.ToString();

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

