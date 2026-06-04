using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                Clipboard.SetText(textBox1.Text);

                MessageBox.Show("Текст успешно скопирован в буфер обмена!");
            }
            else
            {
                MessageBox.Show("Введите сначала текст в поле ввода.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string textFromClipboard = Clipboard.GetText();
                textBox1.Text = textFromClipboard;


                File.WriteAllText("stepka.txt", textFromClipboard);
                MessageBox.Show("Текст извлечен и успешно записан в файл stepka.txt");
            }
            else
            {
                MessageBox.Show("В буфере обмена нет текста! Скопируйте что-нибудь.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
