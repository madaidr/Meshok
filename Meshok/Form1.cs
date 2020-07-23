using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace Meshok
{
    public partial class Form1 : Form
    {
        private string url;
        Form2 newForm2 = new Form2();
        IWebDriver driver;
        private Double chec1, chec2;
        private string num1, num2, opis = "";
        private bool u1, u2, old = true;
        private List<String> files; //массив с файлами
        Excel.Application ex;
        Excel.Workbook workBook;
        Excel.Worksheet workSheet;
        private int last;
        public Form1()
        {
            InitializeComponent();
            //Добавление в U1 номер
            numm();
            //Формат даты
            dateTimePicker1.CustomFormat = "yyyy.MM.dd HH:mm";
            dateTimePicker1.MinDate = DateTime.Now.AddMinutes(10);
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(21);
            //Продолжительность
            for (int i = 0; i <= 20; i++)
            {
                comboBox3.Items.Add((i + 1) + " День");
            }
            //
            //comboBox1.SelectedIndex = 17;
            //comboBox2.SelectedIndex = 17;
            //comboBox3.SelectedIndex = 6;
        }
        public void numm()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            for (int x = 0; x < newForm2.Ooa(); x++)
            {
                comboBox1.Items.Add(newForm2.Oob(x).Trim(new Char[] { 'U' }));
                comboBox2.Items.Add(newForm2.Oob(x));
            }
        }
        //вход под ником
        private void Button1_Click(object sender, EventArgs e)
        {
            IWebElement LogInput;
            IWebElement PasInput;
            driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://meshok.net");
            LogInput = driver.FindElement(By.Name("LOGIN"));
            LogInput.SendKeys("Сюда ваш логин");
            PasInput = driver.FindElement(By.Name("password"));
            PasInput.SendKeys("Пароль" + OpenQA.Selenium.Keys.Enter);
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            driver.Quit();
        }
        //Добавление лота на сайт
        private void Button3_Click(object sender, EventArgs e)
        {
            IWebElement BuyInput, Auk;
            BuyInput = driver.FindElement(By.CssSelector("li#barSell.rr.mm"));
            BuyInput.Click();
            //IJavaScriptExecutor collect = driver as IJavaScriptExecutor;
            //collect.ExecuteScript("loadData('g140','RADIO','type','','0'); setTimeout(() => loadData('g252','RADIO','type','','0'), 1000);");
            string g;
            if (treeView1.SelectedNode != null)
            {
                g = treeView1.SelectedNode.ToolTipText;
            }
            else
            {
                g = "13455";
            }
            //System.Threading.Thread.Sleep(2000);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            string myString = "document.querySelector(\"#bbody > form > table.doted > tbody > tr.r2\").innerHTML = \"<input style='vertical-align: middle;margin-right:2px;' id=123 type='RADIO' name='type' value='" + g + "'>\";";
            js.ExecuteScript(myString);
            //System.Threading.Thread.Sleep(1500);
            js.ExecuteScript("setTimeout(() => loadData(document.getElementById('123').click()), 2000);");
            System.Threading.Thread.Sleep(2000);
            Auk = driver.FindElement(By.Name("sellAuction"));
            Auk.Click();
            //загрузка файлов
            if (files.Count != 0)
            {
                IWebElement detailFrame = driver.FindElement(By.XPath("//*[@id='bbody']/form/table[2]/tbody/tr[2]/td/div/iframe"));
                driver.SwitchTo().Frame(detailFrame);
                IWebElement upl = driver.FindElement(By.Name("up_pic[]"));
                String uploadFilePath = "";
                for (int i = 0; i <= files.Count - 1; i++)
                {
                    uploadFilePath += files[i];
                    if (i < files.Count - 1)
                    {
                        uploadFilePath += "\n ";
                    }
                }
                upl.SendKeys(uploadFilePath);
                driver.SwitchTo().DefaultContent();
            }
            IWebElement nameInput = driver.FindElement(By.Name("name")); //наименование
            nameInput.SendKeys(TextBox1.Text);
            IWebElement opisInput = driver.FindElement(By.Name("descriptionn")); //описание
            opisInput.SendKeys(richTextBox1.Text + "\n " + TextBox9.Text);
            IWebElement pricInput = driver.FindElement(By.Name("min_price")); //начальная цена
            pricInput.SendKeys(TextBox6.Text);
            IWebElement dayInput = driver.FindElement(By.Name("longevity")); //Дни
            var selectElement = new SelectElement(dayInput);
            selectElement.SelectByValue((comboBox3.SelectedIndex + 1).ToString());
            IWebElement sniperInput = driver.FindElement(By.Name("antisniper")); //Анти снайпер
            sniperInput.Click();
            IWebElement tagInput = driver.FindElement(By.Id("ut_tag")); //теги
            tagInput.SendKeys(TextBox3.Text + ",");
            IWebElement stdInput = driver.FindElement(By.Name("start_date")); //начало торгов
            stdInput.SendKeys(dateTimePicker1.Value.ToString("yyyy.MM.dd HH:mm"));

            //Нажатие на дерево вывод console
        }
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listBox1.Items.Add(treeView1.SelectedNode.ToolTipText);
        }
        //Открыть настройки
        private void НастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm2 = new Form2();
            newForm2.Show();
        }
        private void TextBox6_TextChanged(object sender, EventArgs e)
        {

        }
        //ограниичение ввода, только цифры
        private void TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
            if (TextBox6.Text.Length == 8)
            {
                TextBox6.Text = TextBox6.Text.Remove(TextBox6.Text.Length - 1, 1);
                TextBox6.SelectionStart = TextBox6.Text.Length;
            }
        }
        private void TextBox7_TextChanged(object sender, EventArgs e)
        {
            if (TextBox7.Text.Length != 0)
            {
                chec1 = Math.Round(Convert.ToInt32(TextBox7.Text) * 1.3, 0);
                checkBox1.Text = TextBox7.Text + "+ 30% =" + chec1;
            }
            else
            {
                checkBox1.Text = "+ 30%";
            }
        }
        private void TextBox8_TextChanged(object sender, EventArgs e)
        {
            if (TextBox8.Text.Length != 0)
            {
                chec2 = Math.Round(Convert.ToInt32(TextBox8.Text) * 0.65, 0);
                checkBox2.Text = TextBox8.Text + "- 35% =" + chec2;
            }
            else
            {
                checkBox2.Text = "- 35%";
            }
        }
        //ограниичение ввода, только цифры
        private void TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
            if (TextBox7.Text.Length == 8)
            {
                TextBox7.Text = TextBox7.Text.Remove(TextBox7.Text.Length - 1, 1);
                TextBox7.SelectionStart = TextBox7.Text.Length;
            }
        }
        private void TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
            if (TextBox8.Text.Length == 8)
            {
                TextBox8.Text = TextBox8.Text.Remove(TextBox8.Text.Length - 1, 1);
                TextBox8.SelectionStart = TextBox8.Text.Length;
            }
        }
        private void CheckBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                TextBox6.Text = chec1.ToString();
            }
        }
        private void CheckBox2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                TextBox7.Text = chec2.ToString();
            }
        }
        private void CheckBox3_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                TextBox7.Text = TextBox8.Text;
            }
        }
        ////////Замена в excel/////////
        //cut
        private void Exl_cut()
        {
            url = newForm2.Ooo(comboBox2.Text);
            Exl();
            var sourceRange = workSheet.Range["A" + (last - 13) + ":D" + last];
            var destinationRange = workSheet.Range["A" + (last - 12)];
            sourceRange.Cut(destinationRange);
            //
            //Добавление данных
            workSheet.Cells[last - 14, 1] = Convert.ToInt32(workSheet.Cells[last - 15, 1].Text) + 1;
            var oRange = workSheet.get_Range("A" + (last - 14), "E" + (last - 14));
            oRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            workSheet.Cells[last - 14, 3] = TextBox1.Text; //присвоить ячейки наименование
            workSheet.Cells[last - 14, 4] = TextBox7.Text; // присвоить ячейки сумму
            workSheet.Cells[last - 14, 5] = TextBox4.Text; //присвоить прошлый номер
            workBook.Save();
            listBox1.Items.Add("Замена завершена");
        }
        /////////////////////// Метод Excel/////////////////////////////
        private void Exl()
        {
            ex = new Excel.Application();
            ex.Visible = false;
            workBook = ex.Workbooks.Open(url,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing);
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);
            last = workSheet.Cells.Find("*", Missing.Value,
            Missing.Value, System.Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

        }
        //////////////////////////////////////////////////////
        private static DialogResult ShowInputDialog(ref string input, string nameT)
        {
            Size size = new Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = nameT;

            TextBox TextBox = new TextBox();
            TextBox.Size = new Size(size.Width - 10, 23);
            TextBox.Location = new Point(5, 5);
            TextBox.Text = input;
            inputBox.Controls.Add(TextBox);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = TextBox.Text;
            return result;
        }
        // поиск в договоре по U2
        private void Dog_u2(int i)
        {
            listBox1.Items.Add("В договоре найден номер U2");
            DialogResult result = MessageBox.Show(
            "Это оно?\n" + workSheet.Cells[i, 3].Text,
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Yes)
            {
                if (workSheet.Cells[i, 5].Text != "")
                {
                    listBox1.Items.Add("В договоре найден номер U1");
                    TextBox4.Text = workSheet.Cells[i, 5].Text;
                }
                if (workSheet.Cells[i, 5].Text == "")
                {
                    listBox1.Items.Add("В договоре не найден номер U1");
                    DialogResult money = MessageBox.Show(
                    "Вписать номер U1?",
                    "Сообщение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                    if (money == DialogResult.Yes)
                    {
                        string input = TextBox4.Text;
                        ShowInputDialog(ref input, "Прошлый номер");
                        workSheet.Cells[i, 5] = input;
                        workBook.Save();
                    }
                    else { listBox1.Items.Add("Не вписана цена U2"); }
                }
                if (workSheet.Cells[i, 4].Text != "")
                {
                    listBox1.Items.Add("Найдена цена U2");
                    TextBox7.Text = workSheet.Cells[i, 4].Text;
                }
                if (workSheet.Cells[i, 4].Text == "")
                {
                    listBox1.Items.Add("Не найдена цена U2");
                    DialogResult money = MessageBox.Show(
                    "Вписать новую цену?",
                    "Сообщение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                    if (money == DialogResult.Yes)
                    {
                        string input = TextBox7.Text;
                        ShowInputDialog(ref input, "Цена в договоре");
                        workSheet.Cells[i, 4] = input;
                        workBook.Save();
                    }
                    else { listBox1.Items.Add("Не вписана цена U2"); }
                }
            }
            else
            {
            }
        }
        /// поиск в договоре по U1
        private void Dog_u1(int i)
        {
            listBox1.Items.Add("В договоре найден номер U2");
            DialogResult result = MessageBox.Show(
            "Это оно?\n" + workSheet.Cells[i, 3].Text,
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Yes)
            {
                listBox1.Items.Add("В договоре найден номер U1");
                if (workSheet.Cells[i, 1].Text != "")
                {
                    listBox1.Items.Add("В договоре найден номер U2");
                    TextBox5.Text = workSheet.Cells[i, 1].Text;
                }
                if (workSheet.Cells[i, 1].Text == "")
                { listBox1.Items.Add("В договоре не найден номер U2"); }
                if (workSheet.Cells[i, 4].Text != "")
                {
                    listBox1.Items.Add("Найдена цена U2");
                    TextBox7.Text = workSheet.Cells[i, 4].Text;
                }
                if (workSheet.Cells[i, 4].Text == "")
                {
                    listBox1.Items.Add("Не найдена цена U2");
                    DialogResult money = MessageBox.Show(
                    "Вписать новую цену?\n" + TextBox7.Text,
                    "Сообщение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                    if (money == DialogResult.Yes)
                    {
                        string input = TextBox7.Text;
                        ShowInputDialog(ref input, "Цена в договоре");
                        workSheet.Cells[i, 4] = input;
                        workBook.Save();
                    }
                    else { listBox1.Items.Add("Не вписана цена U2"); }
                }
            }
            else
            {
            }
        }
        ///ок
        ////////////////////////////////////////////кнопка обновить//////////////////////////////////////////////////////////////////////////////////////////////
        private void Button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("Нажата кнопка Обновить");
            num1 = TextBox4.Text;
            num2 = TextBox5.Text;
            if (num1.Length != 0)
            {
                u1 = true;
            }
            else { u1 = false; }
            if (num2.Length != 0)
            {
                u2 = true;
            }
            else { u2 = false; }
            url = newForm2.Ooo(comboBox2.Text);
            Exl();
            ////////////////Начало excel 
            if (u1 == true && u2 == true)
            {
                listBox1.Items.Add("u1==true && u2 == true");
                //находим последнию строку
                //находим номер U2
                for (int i = 5; i <= last - 14; i++)
                {
                    if (workSheet.Cells[i, 1].Text == num2) //поиск по num2
                    {
                        Dog_u2(i);
                        break;
                    }
                    else if (workSheet.Cells[i, 5].Text == num1) //поиск по num1
                    {
                        Dog_u1(i);
                        break;
                    }
                    if (i == last - 14)
                    {
                        listBox1.Items.Add("Ничего не найдено");
                        old = false;
                    }
                }
            }
            else if (u1 == true && u2 == false)
            {
                listBox1.Items.Add("u1 == true && u2 == false");
                for (int i = 5; i <= last - 14; i++)
                {
                    if (workSheet.Cells[i, 5].Text == num1) //поиск по num1
                    {
                        Dog_u1(i);
                        break;
                    }
                    if (i == last - 14)
                    {
                        listBox1.Items.Add("Ничего не найдено");
                        old = false;
                    }
                }
            }
            else if (u1 == false && u2 == true)
            {
                listBox1.Items.Add("u1 == false && u2 == true");
                for (int i = 5; i <= last - 14; i++)
                {
                    if (workSheet.Cells[i, 1].Text == num2) //поиск по num1
                    {
                        Dog_u2(i);
                        break;
                    }
                    if (i == last - 14)
                    {
                        listBox1.Items.Add("Ничего не найдено");
                        old = false;
                    }
                }
            }
            else
            {
                old = false;

            }
            workBook.Close(false, Type.Missing, Type.Missing);
            ex.Quit();
            shablon();
            //Вставить, если ничего не нашел///////////////////
            if (old == false)
            {
                listBox1.Items.Add("Не найден в договоре");
                DialogResult money = MessageBox.Show(
                "Вписать новый лот?\n" + TextBox7.Text,
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
                if (money == DialogResult.Yes)
                {
                    //наименование
                    string input = TextBox1.Text;
                    ShowInputDialog(ref input, "Наименование");
                    TextBox1.Text = input;
                    //прошлый номер
                    input = TextBox4.Text;
                    ShowInputDialog(ref input, "Прошлый номер");
                    TextBox4.Text = input;
                    //цена в договоре
                    input = TextBox7.Text;
                    ShowInputDialog(ref input, "Цена в договоре");
                    TextBox7.Text = input;
                    ////
                    listBox1.Items.Add("Происходит замена");
                    //Заменить в Excel файле           
                    Exl_cut();
                    workBook.Close(false, Type.Missing, Type.Missing);
                    ex.Quit();
                    listBox1.Items.Add("Новый лот вписан");
                }
                else { listBox1.Items.Add("Новый лот не вписан"); }
            }


            u1 = false;
            u2 = false;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void shablon()
        {

            if (TextBox4.Text != "")
            {
                listBox1.Items.Add("Поиск в шаблоне");
                listBox1.Items.Add(comboBox1.Text + "/" + TextBox4.Text);
                url = newForm2.TextBox1.Text;
                Exl();
                listBox1.Items.Add("Загрузка");
                int coub = listBox1.Items.Count - 1;
                for (int i = 1; i <= 5500; i++)
                {
                    if (i % 100 == 0)
                    {
                        listBox1.Items.RemoveAt(coub);
                        listBox1.Items.Insert(coub, i.ToString());
                    }

                    if (workSheet.Cells[i, 1].Text == (comboBox1.Text + "/" + TextBox4.Text))
                    {
                        opis = "";
                        listBox1.Items.Add("Найден в Шаблоне U2");
                        Console.WriteLine(workSheet.Cells[i, 5].Text);
                        TextBox1.Text = workSheet.Cells[i, 5].Text; //наименование
                        if (workSheet.Cells[i, 7].Text.Length != 0)
                        {
                            opis += workSheet.Cells[i, 7].Text;
                        }
                        if (workSheet.Cells[i, 9].Text != "")
                        {
                            opis += "\n\n" + workSheet.Cells[i, 9].Text;
                        }
                        if (workSheet.Cells[i, 3].Text != "")
                        {
                            opis += "\n\nАвтор, Художник: " + workSheet.Cells[i, 3].Text;
                        }
                        if (workSheet.Cells[i, 8].Text != "")
                        {
                            opis += "\n\nСостояние: " + workSheet.Cells[i, 8].Text;
                        }
                        richTextBox1.Text = opis;
                        TextBox8.Text = workSheet.Cells[i, 15].Text; //старт u1
                        listBox1.Items.Add("Закончил искать в шаблоне");
                        break;
                    }
                    else
                    {
                        if (i == 5500)
                        {
                            listBox1.Items.Add("Не найден в Шаблоне U2");
                        }
                    }
                }
                workBook.Close(false, Type.Missing, Type.Missing);
                ex.Quit();
            }
        }
        //заполнение номера/коробки
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            num1 = TextBox4.Text;
        }
        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            TextBox9.Text = "\n\n" + comboBox2.Text + "/" + TextBox5.Text + "/" + TextBox2.Text;
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox9.Text = "\n\n" + comboBox2.Text + "/" + TextBox5.Text + "/" + TextBox2.Text;
        }
        //работа со списками номеров договора
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex;
            TextBox9.Text = "\n\n" + comboBox2.Text + "/" + TextBox5.Text + "/" + TextBox2.Text;
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = comboBox1.SelectedIndex;
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //установка времени 
        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Now.AddMinutes(30);
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(21);
        }
        //при наведении файлов
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }
        //кинуть файлы img в list
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            files = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();

            foreach (string file in files)
            {
                listBox1.Items.Add(file);
            }
            pictureBox1.Image = new Bitmap(files[0].ToString());

        }
        int imgbt = 0;
        //нажатие на img
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (files != null && files.Count > 0)
            {
                if (imgbt != files.Count - 1)
                {
                    imgbt += 1;
                    pictureBox1.Image = new Bitmap(files[imgbt].ToString());
                }
                else
                {
                    imgbt = 0;
                    pictureBox1.Image = new Bitmap(files[imgbt].ToString());
                }

            }
            else { pictureBox1.Image = null; }
        }
        //удаление картинки
        private void Button7_Click(object sender, EventArgs e)
        {
            if (files != null)
            {
                if (imgbt != 0 && files.Count > 1)
                {
                    imgbt -= 1;
                    pictureBox1.Image = new Bitmap(files[imgbt].ToString());
                    files.RemoveAt(imgbt + 1);
                }
                else if (imgbt == 0 && files.Count == 1)
                {
                    pictureBox1.Image = null;
                    imgbt = 0;
                    files.RemoveAt(imgbt);
                }
                else if (imgbt == 0 && files.Count > 1)
                {
                    imgbt = files.Count - 1;
                    pictureBox1.Image = new Bitmap(files[imgbt].ToString());
                    files.RemoveAt(0);
                    imgbt -= 1;
                }

            }
            else { pictureBox1.Image = null; }

        }
        //шаблон
        private void ШаблонU2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form3 newForm3 = new Form3();
            //newForm3.Show();
        }
    }
}
