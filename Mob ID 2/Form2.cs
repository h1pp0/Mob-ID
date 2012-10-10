using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public int xLocation;
        public int yLocation;
        public byte RedBG;
        public byte GreenBG;
        public byte BlueBG;
        public byte AlphaBG;
        public bool BG;
        public byte RedText;
        public byte GreenText;
        public byte BlueText;
        public byte AlphaText;
        public byte top = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        { 
            if (top == 1)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                label3.Enabled = true;
                label4.Enabled = true;
                label5.Enabled = true;
                label6.Enabled = true;
                numericUpDown3.Enabled = true;
                numericUpDown4.Enabled = true;
                numericUpDown5.Enabled = true;
                numericUpDown6.Enabled = true;
            }
            if (checkBox1.Checked == false)
            {
                label3.Enabled = false;
                label4.Enabled = false;
                label5.Enabled = false;
                label6.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
                numericUpDown6.Enabled = false;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                BG = true;
            }
            else
            {
                BG = false;
            }
            xLocation = Convert.ToInt32(numericUpDown1.Value);
            yLocation = Convert.ToInt32(numericUpDown2.Value);
            AlphaBG = Convert.ToByte(numericUpDown3.Value);
            RedBG = Convert.ToByte(numericUpDown4.Value);
            GreenBG = Convert.ToByte(numericUpDown5.Value);
            BlueBG = Convert.ToByte(numericUpDown6.Value);
            AlphaText = Convert.ToByte(numericUpDown7.Value);
            RedText = Convert.ToByte(numericUpDown8.Value);
            GreenText = Convert.ToByte(numericUpDown9.Value);
            BlueText = Convert.ToByte(numericUpDown10.Value);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            MessageBox.Show("" + f.playername);
        }
    }
}
