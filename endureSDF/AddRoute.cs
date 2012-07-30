using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using endureSDF;

namespace linqtsql
{
    public partial class AddRoute : Form
    {
        private Route route;
        private byte[] image;

        public AddRoute()
        {
            InitializeComponent();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image = File.ReadAllBytes(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                route = new Route();
                float length;
                if (!float.TryParse(textBox2.Text.Replace('.', ','), out length))
                {
                    MessageBox.Show("Потрібно ввести нормальну довжину маршруту!");
                    return;
                }
                route.Name = textBox1.Text;
                if (!Model.Context.Routes.Any(r => r.Name == textBox1.Text))
                {
                    route.Length = length;
                    if (image != null)
                        route.Image = new System.Data.Linq.Binary(image);
                    Model.Context.Routes.InsertOnSubmit(route);
                    Model.Context.SubmitChanges();
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Маршрут з таким іменем вже існує!");
                }
            }
            else
            {
                MessageBox.Show("Потрібно ввести назву маршруту!");
            }
        }
    }
}
