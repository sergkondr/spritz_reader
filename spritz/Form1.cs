using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using spritz.Properties;

namespace spritz
{
    public partial class Form1 : Form
    {
        private string[] book_text;
        private bool status = false;
        private int word_index = 0;

        public Form1()
        {
            InitializeComponent();
            button2.Image = Resources.Symbols_Play_32xLG;
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
            int i = 0;
            switch (word.Trim(chars_to_trim).Length)
            {
                case 1:
                    i = 1;
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                {
                    i = 2;
                    break;
                }
                case 6:
                case 7:
                case 8:
                case 9:
                {
                    i = 3;
                    break;
                }
                case 10:
                case 11:
                case 12:
                case 13:
                {
                    i = 4;
                    break;
                }
                case 14:
                case 15:
                case 16:
                case 17:
                {
                    i = 5;
                    break;
                }
                default:
                {
                    i = 6;
                    break;
                }
            }
            return i - 1;
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
            timer1.Enabled = false;
            if (word_index - 10 < 0)
                word_index = 0;
            else
                word_index -= 10;
            timer1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (word_index + 10 > book_text.Length)
                word_index = book_text.Length - 1;
            else
                word_index += 10;
            timer1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
            }
        }
}
}
