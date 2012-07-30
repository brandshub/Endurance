using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using endureSDF;

namespace linqtsql
{
    public partial class AddRun : Form
    {
        private Route route;
        public AddRun()
        {
            InitializeComponent();

        }

        public Route Route
        {
            set { route = value; }
        }

        public User CurrentUser { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] strs;
            strs = textBox1.Text.Split(':');
            if (strs.Length == 2)
            {
                int a1 = 0, a2 = 0;
                if (!int.TryParse(strs[0], out a1) || !int.TryParse(strs[1], out a2) || (a1 < 0 || a1 > 23 || a2 < 0 || a2 > 59))
                {
                    MessageBox.Show("Неправильний час!");
                    return;
                }

                DateTime pick = dateTimePicker1.Value;
                DateTime dt = new DateTime(pick.Year, pick.Month, pick.Day, a1, a2, 0);

                strs = textBox2.Text.Split(':');
                if (strs.Length != 2 || !int.TryParse(strs[0], out a1) || !int.TryParse(strs[1], out a2) || (a1 < 0 || a2 < 0 || a2 > 59))
                {
                    MessageBox.Show("Неправильна тривалість бігу!");
                    return;
                }

                float time = a1 + a2 / 60.0f;
                Model.Context.Runs.InsertOnSubmit(new Run { Date = dt, Route = route, Description = textBox3.Text.Substring(0, Math.Min(100, textBox3.Text.Length)), Time = time, UID = CurrentUser.ID });
                Model.Context.SubmitChanges();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Неправильний час!");
            }

        }
    }
}
