using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Data.Common;
using endureSDF;
using System.Security.Cryptography;

namespace linqtsql
{
    public partial class Form1 : Form
    {

        private AddRoute form1 = new AddRoute();
        private AddRun form2 = new AddRun();
        private ChartForm form3 = new ChartForm(null);
        private ImageConverter convertor = new ImageConverter();
        private OpenFileDialog ofd = new OpenFileDialog() { Filter = "Images|*.bmp;*.jpg;*.jpeg;*.gif" };
        private Image image;
        private Point imageOffset;
        private bool vb, hb;

        private Route selectedRoute;

        private User currentUser;

        public Form1(User user)
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            currentUser = user;
        }



        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Route r in Model.Context.Routes)
                if (r.Runs.Count == 0)
                    Model.Context.Routes.DeleteOnSubmit(r);

            Model.Context.SubmitChanges();
            Model.Context.Connection.Close();

        }

        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (image != null)
                e.Graphics.DrawImage(image, imageOffset);
        }

        private Dictionary<string, Route> d_routes = new Dictionary<string, Route>();



        private void Form1_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (Route r in Model.Context.Routes)
            {
                comboBox1.Items.Add(r.Name);
                d_routes.Add(r.Name, r);

            }

            /* var runs = Model.Context.Runs.Where(r => r.UID == currentUser.ID).ToList();
             if (runs != null)
                 sum = runs.Sum(s => s.Route.Length);*/
            var runs = currentUser.Runs;
            if (runs != null)
                sum = runs.Sum(s => s.Route.Length);
            double result = watch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
            result++;
        }

        private float AVGSpeed(float distance, float minutes)
        {
            return distance / minutes * 60;
        }

        private string floatToMinutes(float minutes)
        {
            int full = (int)minutes;
            if (minutes < 60)
                return full.ToString().PadLeft(2, '0') + ":" + ((int)Math.Round(60 * (minutes - full))).ToString().PadLeft(2, '0');
            int hours = full / 60;
            full %= 60;
            minutes -= hours * 60;
            return hours.ToString() + ":" + full.ToString().PadLeft(2, '0') + ":" + ((int)Math.Round(60 * (minutes - full))).ToString().PadLeft(2, '0');
        }

        private string TimeAndSpeed(float distance, float minutes)
        {
            return floatToMinutes(minutes) + " [ " + AVGSpeed(distance, minutes).ToString("F2") + " км/год ]";
        }

        float sum;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {

                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

                //  Route r = Model.Context.Routes.First(rt => rt.Name == comboBox1.SelectedItem.ToString());
                Route r = d_routes[comboBox1.SelectedItem.ToString()];
                selectedRoute = r;
                double result = watch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;


                LoadPicture(selectedRoute);

                label2.Text = "Довжина  " + r.Length.ToString("F3") + " км";

                RefreshGrid();


                pictureBox1.Invalidate();
            }
        }

        private Dictionary<string, Image> imageCache = new Dictionary<string, Image>();


        private void LoadPicture(Route r)
        {


            if (imageCache.ContainsKey(r.Name))
            {
                image = imageCache[r.Name];
            }
            else if (r.Image != null)
            {
                image = (Image)convertor.ConvertFrom(r.Image.ToArray());
                imageCache.Add(r.Name, image);
            }

            if (image != null)
            {
                hb = image.Width > pictureBox1.Width;
                vb = image.Height > pictureBox1.Height;
                if (hb)
                {
                    if (vb)
                        imageOffset = Point.Empty;
                    else
                        imageOffset = new Point(0, (pictureBox1.Height - image.Height) / 2);
                }
                else if (vb)
                {
                    imageOffset = new Point((pictureBox1.Width - image.Width) / 2, 0);
                }
                else
                {
                    imageOffset = new Point((pictureBox1.Width - image.Width) / 2, (pictureBox1.Height - image.Height) / 2);
                }
            }
            else
            {
                imageOffset = Point.Empty;
            }


        }



        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (selectedRoute != null && ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] bts = File.ReadAllBytes(ofd.FileName);
                    selectedRoute.Image = new System.Data.Linq.Binary(bts);
                }
            }
            else
            {
                selectedRoute.Image = null;
            }
            Model.Context.SubmitChanges();
            LoadPicture(selectedRoute);
            pictureBox1.Invalidate();
        }

        private bool drag = false;
        private Point lastPos;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (image != null)
            {
                drag = true;
                lastPos = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                if (hb)
                {
                    int newX = imageOffset.X + (e.X - lastPos.X);
                    if (newX < 0)
                        newX = Math.Max(newX, pictureBox1.Width - image.Width);
                    else
                        newX = 0;
                    imageOffset.X = newX;
                }
                if (vb)
                {
                    int newY = imageOffset.Y + (e.Y - lastPos.Y);
                    if (newY < 0)
                        newY = Math.Max(newY, pictureBox1.Height - image.Height);
                    else
                        newY = 0;
                    imageOffset.Y = newY;
                }

                lastPos = e.Location;
                pictureBox1.Invalidate();
            }

        }

        private bool fdrag = false;
        private Point offset = Point.Empty;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Y < 66)
            {
                offset = e.Location;
                fdrag = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            fdrag = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (fdrag)
            {
                Left += e.X - offset.X;
                Top += e.Y - offset.Y;
            }
            base.OnMouseMove(e);
        }

        private Font header = new Font("Book Antiqua", 36);

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            e.Graphics.FillRectangle(Brushes.White, 0, 0, Width - 1, 65);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, 65);
            e.Graphics.DrawImage(endureSDF.Properties.Resources.lil, 13, 3);
            SizeF szf = e.Graphics.MeasureString("Endurance", header);
            //e.Graphics.DrawString("Endurance", header, Brushes.Black, 84 + (button6.Left - 77 - szf.Width) / 2, (65 - szf.Height) / 2 + 1);
            e.Graphics.DrawString("Endurance", header, Brushes.Black, (Width - szf.Width) / 2, (65 - szf.Height) / 2 + 1);
            base.OnPaint(e);
        }

        protected override void OnShown(EventArgs e)
        {
            Owner.Hide();
            base.OnShown(e);
        }

        private void RefreshGrid()
        {
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();

            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            var runs = selectedRoute.Runs.Where(r => r.UID == currentUser.ID);

            double result = watch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
            int count = runs.Count();

            string time = result.ToString("f3") + " ";
            watch.Reset();
            watch.Start();
            if (runs != null && count > 0)
            {
                foreach (var run in runs)
                {
                    string date = run.Date.Date.ToShortDateString() + " " + run.Date.Hour.ToString().PadLeft(2, '0') + ":" + run.Date.Minute.ToString().PadLeft(2, '0');
                    dataGridView1.Rows.Add(date, floatToMinutes(run.Time), run.AverageSpeed.ToString("F2"), run.Description ?? " ");
                }
                result = watch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
                time += result.ToString("f3") + " ";
                watch.Reset();
                watch.Start();
                float max = runs.Max(rt => rt.Time);
                float min = runs.Min(rt => rt.Time);
                float avg = runs.Average(rt => rt.Time);
                result = watch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
                time += result.ToString("f3") + " ";

                label3.Text = "Пробіг  " + (count * selectedRoute.Length).ToString("F2") + " / " + sum.ToString("F2") + " км";
                label4.Text = "Найгірший час:   " + TimeAndSpeed(selectedRoute.Length, max);
                label5.Text = "Найкращий час: " + TimeAndSpeed(selectedRoute.Length, min);
                label6.Text = "Середній час: " + TimeAndSpeed(selectedRoute.Length, avg);
                label7.Text = "Кількість: " + count;
                //label7.Text = time;
            }
            else
            {
                label3.Text = "";
                label4.Text = "Найгірший час:   ";
                label5.Text = "Найкращий час: ";
                label6.Text = "Середній час: ";
                label7.Text = "Кількість: ";
            }


        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (form1.ShowDialog() == DialogResult.OK)
            {


                int rc = Model.Context.Routes.Count() - 1;
                var route = Model.Context.Routes.Skip(rc).First();
                comboBox1.Items.Add(route.Name);
                d_routes.Add(route.Name, route);
                var runs = Model.Context.Runs.Where(r => r.UID == currentUser.ID).ToList();
                if (runs != null)
                    sum = runs.Sum(s => s.Route.Length);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                form2.Route = selectedRoute;
                form2.CurrentUser = currentUser;
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    var runs = Model.Context.Runs.Where(r => r.UID == currentUser.ID).ToList();
                    if (runs != null)
                        sum = runs.Sum(s => s.Route.Length);
                    RefreshGrid();

                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    DataGridViewCell dgc = dataGridView1.SelectedCells[0];

                    Run r = selectedRoute.Runs.Where(rr => rr.UID == currentUser.ID).Skip(dgc.RowIndex).First();
                    Model.Context.Runs.DeleteOnSubmit(r);
                    Model.Context.SubmitChanges();
                    var runs = Model.Context.Runs.Where(rr => rr.UID == currentUser.ID).ToList();
                    if (runs != null)
                        sum = runs.Sum(s => s.Route.Length);

                    RefreshGrid();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form3.CurrentUser = currentUser;
            form3.ShowDialog();
        }





    }
}
