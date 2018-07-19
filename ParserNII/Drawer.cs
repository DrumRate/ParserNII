using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ZedGraph;

namespace ParserNII
{
    public class Drawer
    {
        private readonly ZedGraphControl control;

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

        public Drawer(ZedGraphControl control)
        {
            this.control = control;
            Clear();

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

        public void DrawGraph(List<XDate> x, List<double> y, string name, Color color)
        {
            GraphPane pane = control.GraphPane;

            PointPairList list1 = new PointPairList();

            for (int i = 0; i < x.Count; i++)
            {
                var point = new PointPair()
                {
                    X = x[i],
                    Y = y[i]
                };
                list1.Add(point);
            }

            int yAxis = pane.AddYAxis(name);
            LineItem myCurve = pane.AddCurve(name, list1, color, SymbolType.None);
            myCurve.YAxisIndex = yAxis;
            myCurve.Line.Width = 1.0F;
            myCurve.Line.StepType = StepType.ForwardStep;

            pane.XAxis.Scale.Min = x.First();
            pane.XAxis.Scale.Max = x.Last();
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

        public void Refresh()
        {
            control.RestoreScale(control.GraphPane);
            control.AxisChange();
            control.Invalidate();
        }

        private void Clear()
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

        public LineObj CrateVerticalLine()
        {
            LineObj threshHoldLine = new LineObj(
            Color.Red,
            control.GraphPane.XAxis.Scale.Min,
            0,
            control.GraphPane.XAxis.Scale.Min,
            1);
            threshHoldLine.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
            control.GraphPane.GraphObjList.Add(threshHoldLine);
            return threshHoldLine;
        }
    }
}