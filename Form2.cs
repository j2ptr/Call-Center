using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CallCenter
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = Properties.Settings.Default.Server;
            textBox2.Text = Properties.Settings.Default.Username;
            textBox3.Text = Properties.Settings.Default.Password;
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Server = textBox1.Text;
            Properties.Settings.Default.Username = textBox2.Text;
            Properties.Settings.Default.Password = textBox3.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
