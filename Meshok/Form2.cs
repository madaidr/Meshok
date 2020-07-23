using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Meshok
{
    public partial class Form2 : Form
    {
        private string writePath = @"href.txt";
        private string writePath2 = @"href2.txt";
        public Form2()
        {
            InitializeComponent();
            Tabel_update();
            //Thread thread = new Thread(Tabel_update);
            //thread.Start();

        }
        private void DataGridView1_DoubleClick(object sender, EventArgs e) //если нажать два раза на ссылку, то можно поменять расположение
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 1)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK) { dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value = dialog.FileName.ToString(); }
            }

        }

        private async void button1_Click(object sender, EventArgs e) //Сохранение настроек
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8))
                {
                    Console.WriteLine("Сохранено строк: " + (dataGridView1.RowCount - 1));
                    for (int i = 0; i < dataGridView1.RowCount-1; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value != null || (dataGridView1.Rows[i].Cells[1].Value != null))
                        {
                            await sw.WriteLineAsync(dataGridView1.Rows[i].Cells[0].Value.ToString());
                            await sw.WriteLineAsync(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        }

                    }
                }
                using (StreamWriter sw = new StreamWriter(writePath2, false, System.Text.Encoding.UTF8))
                {
                    if (TextBox1.Text != null || (TextBox2.Text != null))
                    {
                        await sw.WriteLineAsync(TextBox1.Text);
                        await sw.WriteLineAsync(TextBox2.Text);
                    }


                }
                Console.WriteLine("Запись выполнена");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        public string Ooo(String c)
        {
            String gok = "Не найдено";
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString().Contains(c))
                    {
                        gok = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        break;
                    }
                }
            }
            return gok;
        }
        public int Ooa() //возвращает кол-во строк в таблице
        {
            return dataGridView1.RowCount-1;
        }
        public string Oob(int b)
        {
            return dataGridView1.Rows[b].Cells[0].Value.ToString();
        }
        private void Tabel_update()
        {
            try // если нет файла, то создает
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.UTF8))
                {

                }
                using (StreamWriter sw = new StreamWriter(writePath2, true, System.Text.Encoding.UTF8))
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dataGridView1.ColumnCount = 2;
            if (System.IO.File.ReadAllLines(writePath).Length >= 1)
            {
                dataGridView1.RowCount = System.IO.File.ReadAllLines(writePath).Length / 2 + 1;


                Console.WriteLine(dataGridView1.RowCount - 1);
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)// заполняет таблицу 
                {
                    dataGridView1.Rows[i].Cells[0].Value = File.ReadAllLines(writePath)[i * 2];
                    dataGridView1.Rows[i].Cells[1].Value = File.ReadAllLines(writePath)[(i * 2) + 1];
                }
            }
            if (System.IO.File.ReadAllLines(writePath2).Length >= 2)
            {
                TextBox1.Text = File.ReadAllLines(writePath2)[0];
                TextBox2.Text = File.ReadAllLines(writePath2)[1];
            }
        }

    }
}
