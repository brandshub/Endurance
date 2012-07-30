using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using endureSDF.Properties;
using linqtsql;

namespace endureSDF
{
    partial class Preloader : Form
    {
        public Preloader()
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            backgroundWorker1.RunWorkerAsync();


        }
        public User selectedUser = null;
        private List<User> users;

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Settings.Default.Reload();
            checkBox2.Checked = Settings.Default.saveUP;
            if (Settings.Default.saveUP)
            {
                textBox1.Text = Settings.Default.User;
                textBox2.Text = Settings.Default.PWD;
            }
            label1.Text = "v 2.00";
            foreach (Control c in Controls)
                c.Visible = true;


        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            users = linqtsql.Model.Context.Users.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            selectedUser = users.FirstOrDefault(u => u.Name == textBox1.Text);
            if (selectedUser == null)
            {
                selectedUser = new User() { Name = textBox1.Text, Password = GetHash(textBox2.Text) };
                linqtsql.Model.Context.Users.InsertOnSubmit(selectedUser);
                linqtsql.Model.Context.SubmitChanges();
                Settings.Default.saveUP = checkBox2.Checked;
                if (checkBox2.Checked)
                {
                    Settings.Default.User = textBox1.Text;
                    Settings.Default.PWD = textBox2.Text;
                }
                Settings.Default.Save();
                Close();
            }
            else
            {
                if (selectedUser.Password == GetHash(textBox2.Text))
                {
                    Settings.Default.saveUP = checkBox2.Checked;
                    if (checkBox2.Checked)
                    {
                        Settings.Default.User = textBox1.Text;
                        Settings.Default.PWD = textBox2.Text;
                    }
                    Settings.Default.Save();
                    Close();
                }
                else
                {
                    MessageBox.Show("Такий користувач вже є!");
                }
            }


        }

        private MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        private string tempHash = "";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            User us = users.FirstOrDefault(u => u.Name == textBox1.Text);
            if (us != null)
            {
                pictureBox1.Image = Properties.Resources.ok;
                tempHash = us.Password;

            }
            else
            {
                tempHash = "";
                pictureBox1.Image = Properties.Resources.wr;
            }
            RefreshItems();

        }

        private string GetHash(string text)
        {
            return md5.ComputeHash(Encoding.Default.GetBytes(text)).Aggregate("", (prev, cur) => prev += cur.ToString("x2"));
        }

        private void RefreshItems()
        {
            string inPwd = GetHash(textBox2.Text);
            if (tempHash != "")
            {
                bool right = tempHash == inPwd;
                if (right)
                {
                    button1.BackColor = Color.LightGreen;
                    button1.Text = "Вхід";
                }
                else
                {
                    button1.Text = "Реєстрація";
                    button1.BackColor = Color.LightPink;
                }


                if (right)
                {
                    pictureBox2.Image = Properties.Resources.ok;
                }
                else
                {
                    pictureBox2.Image = Properties.Resources.wr;
                }
            }
            else
            {
                button1.Text = "Реєстрація";
                button1.BackColor = Color.LightPink;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            RefreshItems();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            base.OnPaint(e);

        }

        Form1 form;
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
                DialogResult = DialogResult.Cancel;
            else
            {
                form = new Form1(selectedUser);
                form.ShowDialog(this);

            }
            base.OnClosing(e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Text = "";
            }
            textBox2.UseSystemPasswordChar = !checkBox1.Checked;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
