﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using spritz.Properties;
using System.Runtime.InteropServices;


namespace spritz
{
    public partial class Form1 : Form
    {
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        
        private string[] book_text;
        private bool status = false;
        private int word_index = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void update_text(string word)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = 20;
            richTextBox1.SelectionColor = Color.Black;

            if (word.IndexOf("\r\n") > 0) word = word.Remove(word.IndexOf("\r\n"), 2);
            int index = get_colored_letter_number(word);
            richTextBox1.ForeColor = Color.Black;

            richTextBox1.Text = word;
            richTextBox1.SelectionStart = index;
            richTextBox1.SelectionLength = 1;
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.Location = get_point(index);
        }

        private int get_colored_letter_number(string word)
        {
            char[] chars_to_trim = {'.', ',',':',';','!','?','-'};
            if (word.Trim(chars_to_trim).Length >= 18)
                return 5;
            else if (word.Trim(chars_to_trim).Length >= 14)
                return 4;
            else if (word.Trim(chars_to_trim).Length >= 10)
                return 3;
            else if (word.Trim(chars_to_trim).Length >= 6)
                return 2;
            else if (word.Trim(chars_to_trim).Length >= 2)
                return 1;
            else 
                return 0;
        }

        private Point get_point(int index)
        {
            int X = richTextBox1.GetPositionFromCharIndex(index).X;
            int Y = richTextBox1.Location.Y;
            int letter_width = (int)richTextBox1.Font.Size / 2 - 4;
            return new Point(125 - X - letter_width, Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 60 * 1000 / Convert.ToInt32(comboBox1.Text);
            if (status)
            {
                button2.Image = Resources.Symbols_Play_32xLG;
                timer1.Enabled = false;
                status = false;
            }
            else
            {
                button2.Image = Resources.Symbols_Pause_32xLG;
                timer1.Enabled = true;
                status = true;
                button3.Enabled = true;
                button4.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string text = System.IO.File.ReadAllText(openFileDialog1.FileName, Encoding.Default);
                book_text = text.Split(' ');
                button2.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            update_text(book_text[word_index]);
            word_index += 1;
            if (word_index == book_text.Length)
            {
                timer1.Enabled = false;
                button2.Image = Resources.Symbols_Play_32xLG;
                word_index = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool timer_state = timer1.Enabled;
            timer1.Enabled = false;
            if (word_index - 10 < 0)
                word_index = 0;
            else
                word_index -= 10;
            timer1.Enabled = timer_state;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool timer_state = timer1.Enabled;
            timer1.Enabled = false;
            if (word_index + 10 > book_text.Length)
                word_index = book_text.Length - 1;
            else
                word_index += 10;
            timer1.Enabled = timer_state;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                richTextBox1.Font = fontDialog1.Font;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
                return;
            }
            base.OnMouseMove(e);
        }
}
}
