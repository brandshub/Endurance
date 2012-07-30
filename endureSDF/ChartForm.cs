using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using linqtsql;

namespace endureSDF
{
    public partial class ChartForm : Form
    {
        private User current;
        private List<Run> runs;
        private Dictionary<string, List<Run>> sortedRuns;
        private IEnumerable<IGrouping<string, Run>> grouped;
        private float minVal = float.MaxValue;
        private float maxVal = float.MinValue;
        private List<Route> urRoutes;


        public ChartForm(User user)
        {
            InitializeComponent();
            current = user;
        }

        public User CurrentUser { get { return current; } set { current = value; } }
        public bool GlobalStats { get { return monthes_LB.SelectedIndices.Count == 0 && routes_LB.SelectedItems.Count == 0; } }


        protected override void OnLoad(EventArgs e)
        {
            monthes_LB.Items.Clear();


            runs = Model.Context.Runs.Where(r => r.UID == current.ID).ToList();
            runs.Sort((r1, r2) => r1.Date.CompareTo(r2.Date));


            grouped = runs.GroupBy(r => r.Date.Month.ToString().PadLeft(2, '0') + "." + r.Date.Year);
            sortedRuns = new Dictionary<string, List<Run>>();

            routes_LB.Items.Clear();


            urRoutes = runs.Select(r => r.Route).Distinct().ToList();


            foreach (var item in grouped)
            {
                List<Run> temp = item.ToList();
                temp.Sort((r1, r2) => r1.Date.CompareTo(r2.Date));
                sortedRuns.Add(item.Key, temp);
            }


            monthes_LB.Items.AddRange(grouped.Select(g => g.Key).ToArray());
            routes_LB.Items.AddRange(urRoutes.Select(r => r.Name).ToArray());

            pens = new Pen[monthes_LB.Items.Count];
            for (int i = 0; i < pens.Length; i++)
                pens[i] = new Pen(Color.FromKnownColor(kcolors[i % kcolors.Length]), 2);

            base.OnLoad(e);

            listBox1_SelectedIndexChanged(null, null);
        }



        private List<List<Run>> selected;
        private bool everything = false;


        private List<Run> FilterRoutes(List<Run> list)
        {
            if (routes_LB.SelectedItems.Count == 0)
            {
                list.Sort((r1, r2) => r1.Date.CompareTo(r2.Date));
                return list;
            }

            List<Run> filtered = new List<Run>();
            foreach (var name in routes_LB.SelectedItems)
                filtered.AddRange(list.Where(r => r.Route.Name == name.ToString()));
            filtered.Sort((r1, r2) => r1.Date.CompareTo(r2.Date));
            return filtered;
        }

        private List<List<Run>> GetSingleBounds()
        {
            List<List<Run>> runs = new List<List<Run>>();

            if (routes_LB.SelectedItems.Count == 0)
            {
                var years = this.runs.GroupBy(r => r.Date.Year);
                if (avgSpeed_RB.Checked)
                {
                    minVal = this.runs.Min(r => r.AverageSpeed) - 0.5f;
                    maxVal = this.runs.Average(r => r.AverageSpeed) + 0.5f;
                }
                else if (speed_RB.Checked)
                {
                    minVal = this.runs.Min(r => r.AverageSpeed);
                    maxVal = this.runs.Max(r => r.AverageSpeed);
                }
                else if (distance_RB.Checked)
                {
                    float maxV = float.MinValue;
                    foreach (var item in years)
                    {
                        maxV = item.Sum(r => r.Route.Length);
                        if (maxV > maxVal)
                            maxVal = maxV;
                    }
                }
                foreach (var item in years)
                    runs.Add(item.ToList());
            }
            else
            {
                runs.Add(FilterRoutes(this.runs));
                if (avgSpeed_RB.Checked)
                {
                    minVal = runs[0].Min(r => r.AverageSpeed) - 0.5f;
                    maxVal = runs[0].Average(r => r.AverageSpeed) + 0.5f;
                }
                else if (speed_RB.Checked)
                {
                    minVal = runs[0].Min(r => r.AverageSpeed);
                    maxVal = runs[0].Max(r => r.AverageSpeed);
                }
                else if (distance_RB.Checked)
                {
                    maxVal = runs[0].Sum(r => r.Route.Length);
                }
            }


            return runs;
        }

        private void GetMultiplyBounds()
        {
            float max = float.MinValue;
            float min = float.MaxValue;
            minVal = min;
            maxVal = max;

            selected = new List<List<Run>>();
            everything = false;
            if (avgSpeed_RB.Checked)
            {
                foreach (var item in monthes_LB.SelectedItems)
                {
                    List<Run> temp = FilterRoutes(sortedRuns[item.ToString()]);
                    if (temp.Count == 0)
                    {
                        selected.Add(new List<Run>());
                        continue;
                    }
                    min = temp.Min(r => r.AverageSpeed);
                    if (minVal > min)
                        minVal = min;
                    max = temp.Average(r => r.AverageSpeed);
                    if (maxVal < max)
                        maxVal = max;

                    selected.Add(temp);
                }
                minVal -= 0.5f;
                maxVal += 0.5f;
            }
            else if (speed_RB.Checked)
            {
                foreach (var item in monthes_LB.SelectedItems)
                {
                    List<Run> temp = FilterRoutes(sortedRuns[item.ToString()]);
                    if (temp.Count == 0)
                    {
                        selected.Add(new List<Run>());
                        continue;
                    }
                    min = temp.Min(r => r.AverageSpeed);
                    if (minVal > min)
                        minVal = min;
                    max = temp.Max(r => r.AverageSpeed);
                    if (maxVal < max)
                        maxVal = max;
                    selected.Add(temp);
                }
                minVal -= 0.5f;
                maxVal += 0.5f;
            }
            else
            {
                foreach (var item in monthes_LB.SelectedItems)
                {
                    List<Run> temp = FilterRoutes(sortedRuns[item.ToString()]);
                    if (temp.Count == 0)
                    {
                        selected.Add(new List<Run>());
                        continue;
                    }
                    max = temp.Sum(r => r.Route.Length);
                    if (maxVal < max)
                        maxVal = max;
                    selected.Add(temp);
                }
                maxVal *= 1.1f;
                minVal = 0;

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (monthes_LB.SelectedIndices.Count == 0)
            {
                selected = new List<List<Run>>();
                selected.AddRange(GetSingleBounds());
                everything = true;
            }
            else
            {
                GetMultiplyBounds();
                everything = false;
            }
            canvas.Invalidate();
        }


        private int legendWidth = 100;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            if (selected != null)
            {

                int daycount;
                float dayW;
                float zoom = (canvas.Height) / (maxVal - minVal);
                float avgSpeed = 0;
                int n = 0;
                if (everything)
                {

                    foreach (var item in selected)
                    {
                        PointF[] points;
                        if (avgSpeed_RB.Checked)
                            points = GetPointsAverage(item, minVal, maxVal);
                        else if (speed_RB.Checked)
                            points = GetPointsNormal(item, minVal, maxVal, out avgSpeed);
                        else
                            points = GetDistance(item, maxVal);


                        if (points.Length == 1)
                            g.DrawLine(pens[n], points[0].X, points[0].Y, canvas.Width - legendWidth, points[0].Y);
                        else
                            g.DrawLines(pens[n], points);


                        if (speed_RB.Checked && avg_CB.Checked)
                            g.DrawLine(new Pen(pens[n].Color, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot }, 0, avgSpeed, canvas.Width - legendWidth, avgSpeed);

                        if (dots_CB.Checked)
                            for (int i = 0; i < points.Length; i++)
                                g.FillEllipse(new SolidBrush(Color.FromKnownColor(kcolors[n])), points[i].X - 2.5f, points[i].Y - 2.5f, 5, 5);
                        n++;
                    }

                    DateTime current;
                    if (!GlobalStats)
                        current = runs[0].Date;
                    else
                        current = new DateTime(2010, 1, 1);

                    g.DrawString(current.Day.ToString().PadLeft(2, '0') + "." + current.Month.ToString().PadLeft(2, '0'), font, Brushes.Black, 2, canvas.Height - 20);

                    double d6;
                    if (!GlobalStats)
                        d6 = (runs[runs.Count - 1].Date - current).TotalDays / 30;
                    else
                        d6 = 364 / 30f;

                    dayW = (canvas.Width - legendWidth) / 30.0f;

                    current = current.AddDays(d6 * 5);

                    for (int i = 5; i <= 30; i += 5)
                    {
                        g.DrawLine(Pens.Gray, i * dayW, 0, i * dayW, canvas.Height);
                        g.DrawString(current.Day.ToString().PadLeft(2, '0') + "." + current.Month.ToString().PadLeft(2, '0'), font, Brushes.Black, i * dayW - 20, canvas.Height - 20);
                        current = current.AddDays(d6 * 5);
                    }

                }
                else
                {
                    daycount = 31;
                    dayW = ((float)canvas.Width - legendWidth) / daycount;
                    foreach (var runCollection in selected)
                    {
                        if (runCollection.Count > 0)
                        {
                            PointF[] points;
                            if (avgSpeed_RB.Checked)
                                points = GetPointsAverage(runCollection, minVal, maxVal);
                            else if (speed_RB.Checked)
                                points = GetPointsNormal(runCollection, minVal, maxVal, out avgSpeed);
                            else
                                points = GetDistance(runCollection, maxVal);

                            if (points.Length == 1)
                                g.DrawLine(pens[monthes_LB.SelectedIndices[n]], points[0].X, points[0].Y, canvas.Width - legendWidth, points[0].Y);
                            else
                            {
                                g.DrawLines(pens[monthes_LB.SelectedIndices[n]], points);
                                if (speed_RB.Checked && avg_CB.Checked)
                                {
                                    g.DrawLine(new Pen(pens[monthes_LB.SelectedIndices[n]].Color, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot }, 0, avgSpeed, canvas.Width - legendWidth, avgSpeed);
                                }
                            }

                            if (dots_CB.Checked)
                                for (int i = 0; i < points.Length; i++)
                                    g.FillEllipse(new SolidBrush(Color.FromKnownColor(kcolors[monthes_LB.SelectedIndices[n]])), points[i].X - 3, points[i].Y - 3, 6, 6);
                        }
                        n++;
                    }

                    g.DrawString("0", font, Brushes.Black, 2, canvas.Height - 20);

                    for (int i = 5; i <= 30; i += 5)
                    {
                        g.DrawLine(Pens.Gray, i * dayW, 0, i * dayW, canvas.Height);
                        g.DrawString(i.ToString(), font, Brushes.Black, i * dayW - 6, canvas.Height - 20);
                    }
                }

                int lb = (int)Math.Floor(minVal);
                int ub = (int)Math.Ceiling(maxVal);
                if (!distance_RB.Checked)
                {

                    for (int i = lb + 1; i <= ub; i++)
                    {
                        float y = canvas.Height - (i - minVal) * zoom;
                        g.DrawLine(Pens.Gray, 0, y, canvas.Width - legendWidth, y);
                        g.DrawString(i.ToString(), font, Brushes.Black, 3, y - 25);
                        y = canvas.Height - (i - 0.5f - minVal) * zoom;
                        g.DrawLine(Pens.Gray, 0, y, canvas.Width - legendWidth, y);
                        g.DrawString((i - 0.5).ToString("F1"), font, Brushes.Black, 3, y - 25);
                    }
                }
                else
                {
                    int dd = 3;
                    while ((ub - minVal) / dd > 18)
                        dd *= 2;

                    for (int i = 0; i <= ub; i += dd)
                    {
                        float y = canvas.Height - (i - minVal) * zoom;
                        g.DrawLine(Pens.Gray, 0, y, canvas.Width - legendWidth, y);
                        g.DrawString(i.ToString(), font, Brushes.Black, 3, y);
                    }
                }

                g.DrawRectangle(new Pen(Color.Black, 2), 1, 1, canvas.Width - legendWidth - 2, canvas.Height - 2);
                g.DrawRectangle(new Pen(Color.Black, 2), canvas.Width - legendWidth - 1, 1, legendWidth, canvas.Height - 2);

                int lineWidth = 18;
                int perLine = 16;
                int upperOffset = 30;

                if (everything)
                {
                    g.DrawLine(pens[0], canvas.Width - legendWidth + 4, upperOffset, canvas.Width - legendWidth + lineWidth + 4, upperOffset);
                    for (int i = 0; i < selected.Count; i++)
                    {
                        int y = upperOffset + perLine * i;
                        g.DrawLine(pens[i], canvas.Width - legendWidth + 4, y, canvas.Width - legendWidth + lineWidth + 4, y);
                        g.DrawString(selected[i][0].Date.Year.ToString(), font, Brushes.Black, canvas.Width - legendWidth + lineWidth + 9, y - 9);
                    }


                    //g.DrawString("Усі", font, Brushes.Black, pictureBox1.Width - legendWidth + lineWidth + 9, upperOffset - 9);
                }
                else
                {
                    for (int i = 0; i < selected.Count; i++)
                    {
                        int y = upperOffset + perLine * i;
                        g.DrawLine(pens[monthes_LB.SelectedIndices[i]], canvas.Width - legendWidth + 4, y, canvas.Width - legendWidth + lineWidth + 4, y);
                        g.DrawString(monthes_LB.SelectedItems[i].ToString(), font, Brushes.Black, canvas.Width - legendWidth + lineWidth + 9, y - 9);
                    }
                }
            }

            double result = watch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
            Text = result.ToString();
        }



        private PointF[] GetPointsNormal(List<Run> runs, float min, float max, out float avgValue)
        {
            PointF[] array = new PointF[runs.Count];
            runs.Sort((r1, r2) => r1.Date.CompareTo(r2.Date));
            float yzoom = canvas.Height / (max - min);
            DateTime d0, d1;
            float xzoom;

            if (monthes_LB.SelectedItems.Count == 0 && routes_LB.SelectedItems.Count == 0)
            {
                d0 = new DateTime(runs[0].Date.Year, 1, 1);
                d1 = new DateTime(d0.Year, 12, 31);
                xzoom = (canvas.Width - legendWidth) / 365f;
            }
            else if (runs[0].Date.Year == runs[runs.Count - 1].Date.Year && runs[0].Date.Month == runs[runs.Count - 1].Date.Month)
            {
                d0 = new DateTime(runs[0].Date.Year, runs[0].Date.Month, 1);
                xzoom = (canvas.Width - legendWidth) / 31.0f;
            }

            else
            {
                d0 = runs[0].Date;
                d1 = runs[runs.Count - 1].Date;
                xzoom = (canvas.Width - legendWidth) / (float)(d1 - d0).TotalDays;
            }


            for (int i = 0; i < runs.Count; i++)
                array[i] = new PointF((float)(runs[i].Date - d0).TotalDays * xzoom, canvas.Height - yzoom * (runs[i].AverageSpeed - min));
            avgValue = canvas.Height - yzoom * (runs.Average(r => r.AverageSpeed - min));
            return array;
        }

        private PointF[] GetPointsAverage(List<Run> runs, float min, float max)
        {
            PointF[] array = new PointF[runs.Count];
            runs.Sort((r1, r2) => r1.Date.CompareTo(r2.Date));
            float yzoom = canvas.Height / (max - min);
            DateTime d0, d1;
            float xzoom;


            if (monthes_LB.SelectedItems.Count == 0 && routes_LB.SelectedItems.Count == 0)
            {
                d0 = new DateTime(runs[0].Date.Year, 1, 1);
                d1 = new DateTime(d0.Year, 12, 31);
                xzoom = (canvas.Width - legendWidth) / 365f;
            }
            else if (runs[0].Date.Year == runs[runs.Count - 1].Date.Year && runs[0].Date.Month == runs[runs.Count - 1].Date.Month)
            {
                d0 = new DateTime(runs[0].Date.Year, runs[0].Date.Month, 1);
                xzoom = (canvas.Width - legendWidth) / 31.0f;
            }
            else
            {
                d0 = runs[0].Date;
                d1 = runs[runs.Count - 1].Date;
                xzoom = (canvas.Width - legendWidth) / (float)(d1 - d0).TotalDays;
            }
            float sum = 0;
            for (int i = 0; i < runs.Count; i++)
            {
                sum += runs[i].AverageSpeed;
                array[i] = new PointF((float)(runs[i].Date - d0).TotalDays * xzoom, canvas.Height - yzoom * (sum / (i + 1) - min));
            }
            return array;
        }

        private PointF[] GetDistance(List<Run> runs, float max)
        {
            if (runs.Count > 1)
            {
                PointF[] array = new PointF[runs.Count];
                float yzoom = canvas.Height / max;
                DateTime d0, d1;
                float xzoom;

                if (monthes_LB.SelectedItems.Count == 0 && routes_LB.SelectedItems.Count == 0)
                {
                    d0 = new DateTime(runs[0].Date.Year, 1, 1);
                    d1 = new DateTime(d0.Year, 12, 31);
                    xzoom = (canvas.Width - legendWidth) / 365f;
                }
                else if (runs[0].Date.Year == runs[runs.Count - 1].Date.Year && runs[0].Date.Month == runs[runs.Count - 1].Date.Month)
                {
                    d0 = new DateTime(runs[0].Date.Year, runs[0].Date.Month, 1);
                    xzoom = (canvas.Width - legendWidth) / 31.0f;
                }
                else
                {
                    d0 = runs[0].Date;
                    d1 = runs[runs.Count - 1].Date;
                    xzoom = (canvas.Width - legendWidth) / (float)(d1 - d0).TotalDays;
                }
                float sum = 0;
                for (int i = 0; i < runs.Count; i++)
                {
                    sum += runs[i].Route.Length;
                    array[i] = new PointF((float)(runs[i].Date - d0).TotalDays * xzoom, canvas.Height - yzoom * sum);
                }
                return array;
            }
            else
            {
                PointF[] array = new PointF[2];
                float yzoom = canvas.Height / max;
                array[0] = new PointF(0, canvas.Height);
                array[1] = new PointF(canvas.Width - legendWidth, canvas.Height - yzoom * runs[0].Route.Length);
                return array;

            }
        }

        private Font font = new Font("ArialUnicodeMS", 10, FontStyle.Bold);
        private Random r_color = new Random();

        //private KnownColor[] kcolors = (KnownColor[])Enum.GetValues(typeof(KnownColor));

        private KnownColor[] kcolors = 
        { 
          KnownColor.Red,KnownColor.Blue,KnownColor.Green,
          KnownColor.Purple,KnownColor.Orange,KnownColor.Brown,
          KnownColor.SkyBlue,KnownColor.Pink,KnownColor.LimeGreen,
          KnownColor.Magenta,KnownColor.DarkBlue,KnownColor.DarkGray
        };

        private Pen[] pens;

        public Pen RandomColor()
        {
            return new Pen(Color.FromKnownColor(kcolors[r_color.Next(kcolors.Length)]), 2);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (monthes_LB.SelectedIndices.Count == 0)
            {
                selected = new List<List<Run>>();
                selected.AddRange(GetSingleBounds());
            }
            else
            {
                GetMultiplyBounds();
            }
            avg_CB.Visible = speed_RB.Checked;
            canvas.Invalidate();
        }



        private void PbInvalidation(object sender, EventArgs e)
        {
            canvas.Invalidate();
        }


    }
}
