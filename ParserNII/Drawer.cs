using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ZedGraph;

namespace ParserNII
{
    public class Drawer
    {
        public static Color GetColor(int i)
        {
            List<Color> colors = new List<Color>();
            colors.Add(Color.Black);
            colors.Add(Color.Maroon);
            colors.Add(Color.Blue);
            colors.Add(Color.BlueViolet);
            colors.Add(Color.Brown);
            colors.Add(Color.Coral);
            colors.Add(Color.Cyan);
            colors.Add(Color.DarkGray);
            colors.Add(Color.DarkGreen);
            colors.Add(Color.DarkMagenta);
            colors.Add(Color.DarkOliveGreen);
            colors.Add(Color.DarkOrange);
            colors.Add(Color.DarkSalmon);
            colors.Add(Color.DarkViolet);
            colors.Add(Color.DeepPink);
            colors.Add(Color.ForestGreen);
            colors.Add(Color.Gray);
            colors.Add(Color.GreenYellow);
            colors.Add(Color.Indigo);
            colors.Add(Color.LimeGreen);
            colors.Add(Color.MediumPurple);
            colors.Add(Color.Olive);
            colors.Add(Color.OrangeRed);
            colors.Add(Color.Tomato);
            colors.Add(Color.YellowGreen);
            colors.Add(Color.Violet);
            return colors[i % colors.Count];
        }

        public static void Initialize(ZedGraphControl control)
        {
            Clear(control);

            GraphPane pane = control.GraphPane;
            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "dd.MM.yyyy HH:mm:ss";

            pane.XAxis.Title.Text = "Дата";

            pane.XAxis.MajorGrid.IsVisible = true;
            pane.XAxis.MajorGrid.DashOn = 10;
            pane.XAxis.MajorGrid.DashOff = 5;
            pane.XAxis.MajorGrid.Color = Color.LightGray;
            pane.XAxis.MajorGrid.IsZeroLine = true;
            control.IsEnableVZoom = false;
            control.IsEnableVPan = false;
            control.IsEnableHPan = false;
            control.IsShowHScrollBar = true;
            control.IsAutoScrollRange = true;
            control.ScrollGrace = 0.01;

        }

        public static void DrawGraph(ZedGraphControl control, List<DateTimeOffset> x, List<double> y, string name, Color color)
        {
            GraphPane pane = control.GraphPane;

            PointPairList list1 = new PointPairList();

            for (int i = 0; i < x.Count; i++)
            {
                var point = new PointPair()
                {
                    X = new XDate(x[i].DateTime),
                    Y = y[i]
                };
                list1.Add(point);
            }

            int yAxis = pane.AddYAxis(name);
            LineItem myCurve = pane.AddCurve(name, list1, color, SymbolType.None);
            myCurve.YAxisIndex = yAxis;
            myCurve.Line.Width = 1.0F;
            myCurve.Line.StepType = StepType.ForwardStep;

            pane.XAxis.Scale.Min = new XDate(x.First().DateTime);
            pane.XAxis.Scale.Max = new XDate(x.Last().DateTime);
            pane.YAxisList[yAxis].Scale.Min = 0;
            pane.YAxisList[yAxis].MajorGrid.IsVisible = true;
            pane.YAxisList[yAxis].MajorGrid.DashOn = 10;
            pane.YAxisList[yAxis].MajorGrid.DashOff = 5;
            pane.YAxisList[yAxis].MajorGrid.Color = Color.LightGray;
            pane.YAxisList[yAxis].MajorGrid.IsZeroLine = false;
            pane.YAxisList[yAxis].IsVisible = false;
            control.GraphPane.Title.IsVisible = false;
            control.GraphPane.Legend.IsVisible = false;
        }

        public static void Refresh(ZedGraphControl control)
        {
            control.RestoreScale(control.GraphPane);
            control.AxisChange();
            control.Invalidate();
        }

        public static void Clear(ZedGraphControl control)
        {
            GraphPane pane = control.GraphPane;

            pane.XAxis.Scale.Min = 0;
            pane.YAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = 0.00001;
            pane.YAxis.Scale.Max = 1.05;

            pane.CurveList.Clear();
            pane.YAxisList.Clear();
            control.Invalidate();
        }
    }
}