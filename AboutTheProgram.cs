using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Тест_OpenTK
{
    public partial class AboutTheProgram : Form
    {
        public AboutTheProgram()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutTheProgram_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "Кузнецов C# Уровень 2. Задание 1. Динамическая 2D графика на OpenGL. V1.0" + Environment.NewLine +
    //1.Добавить свои объекты в иерархию объектов, чтобы получился красивый задний фон, похожий на полет в звездном пространстве.
    //2. *Заменить кружочки картинками, используя метод DrawImage.
    "Программа создана учеником Кузнецовым В.В.GeekBrains под предводительством Сергея Камянецкого и непосредственным содействием Антоном Алиевым." + Environment.NewLine +
    "" + Environment.NewLine +
    "Copyright © Все права защищены согласно статье 1271 ГК РФ, а также ГОСТ Р 7.0.1 - 2003.";
        }
    }
}
