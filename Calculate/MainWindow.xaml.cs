using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.SqlServer.Server;


namespace Calculate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// СДЕЛАТЬ TOOLTIPE =NULL КОГДА ВСЕ ПОЛЯ ВЕРНЫ
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        public static String form;

        public MainWindow()
        {
            InitializeComponent();
           
            UpdateData update = new UpdateData();
            Combo.ItemsSource = update.Updates();
            



        }
        public void ChangeTrig(TextBox name, String letter)
        {
            string sin = Math.Sin(Math.PI * Convert.ToDouble(name.Text.Trim()) / 180).ToString();
            form = form.Replace("sin(" + letter + ")", sin);

            string cos = Math.Cos(Math.PI * Convert.ToDouble(name.Text.Trim()) / 180).ToString();
            form = form.Replace("cos(" + letter + ")", cos);

            string tg = Math.Tan(Math.PI * Convert.ToDouble(name.Text.Trim()) / 180).ToString();
            form = form.Replace("tg(" + letter + ")", tg);

        }
        public void Log(TextBox name, String letter)
        {
            for (double i = 0; i < 100; i+=0.01)
            {
                if (form.Contains("Log" + i + "(" + letter + ")"))
                {
                    string log = Math.Log(Convert.ToDouble(name.Text.Trim()), i).ToString();
                    form = form.Replace("Log" + i + "(" + letter + ")", log);
                }


                if (form.Contains("Log" + letter + "(" + i + ")"))
                {
                    string log = Math.Log( i, Convert.ToDouble(name.Text.Trim())).ToString();
                    form = form.Replace("Log" + letter + "(" + i + ")", log);
                }


            }
        }
       public void Pow( TextBox name, String letter)
 {
     for (double i = 0; i < 100; i += 0.01)
     {
         if (form.Contains( "("+letter + "^" + i+")" ))
         {
             string pow = Math.Pow(Convert.ToDouble(name.Text.Trim()), i).ToString();
             form=form.Replace("(" + letter + "^" + i + ")", pow);
         }
         if (form.Contains("(" + i + "^" + letter + ")"))
         {
             string pow = Math.Pow(i, Convert.ToDouble(name.Text.Trim())).ToString();
             form = form.Replace("(" + i + "^" + letter + ")", pow);
         }
         
     }
  
 }

        private void New_Calc(object sender, RoutedEventArgs e)
        {
            SecondWindow SechWindow = new SecondWindow();
            SechWindow.Show();
            Close();
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Text_A.Text = "";
            Text_B.Text = "";
            Text_C.Text = "";
            Text_A.Background = Brushes.Transparent;
            Text_B.Background = Brushes.Transparent;
            Text_C.Background = Brushes.Transparent;
            Form.Text = "";
            Rez.Text = "";
            string targetName = Combo.SelectedItem as string;

            string connectionString = "Data Source=data.db";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM Calcs WHERE name= @targetName; ";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@targetName", targetName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            form = reader.GetString(2);
                            Form.Text = form;
                            if (Form.Text.Contains("A") & !Form.Text.Contains("B") & !Form.Text.Contains("C"))
                            {
                                Text_A.IsEnabled = true;
                                Text_B.IsEnabled = false;
                                Text_C.IsEnabled = false;
                            }
                            if (Form.Text.Contains("A") & Form.Text.Contains("B") & !Form.Text.Contains("C"))
                            {
                                Text_A.IsEnabled = true;
                                Text_B.IsEnabled = true;
                                Text_C.IsEnabled = false;
                            }
                            if (Form.Text.Contains("A") & Form.Text.Contains("B") & Form.Text.Contains("C"))
                            {
                                Text_A.IsEnabled = true;
                                Text_B.IsEnabled = true;
                                Text_C.IsEnabled = true;
                            }

                        }
                    }
                }

                connection.Close();
            }















        }

        private void Calc(object sender, RoutedEventArgs e)
        {
            string targetName = Combo.SelectedItem as string;

            Text_A.Background = Brushes.Transparent;
            Text_B.Background = Brushes.Transparent;
            Text_C.Background = Brushes.Transparent;
            Rez.Text = "";


            form = Form.Text;
            if (Form.Text.Contains("A") & !Form.Text.Contains("B") & !Form.Text.Contains("C"))
            {
                if (Text_A.Text.Trim() == "")
                {
                    Text_A.ToolTip = "Парамаетр A не введен";
                    Text_A.Background = Brushes.Pink;
                }

                else
                {
                    Text_A.Background = Brushes.Transparent;
                    Text_A.ToolTip = null;
                }

                if (Text_A.Background == Brushes.Transparent)
                {
                   

                    ChangeTrig(Text_A, "A");
                    Pow(Text_A, "A");
                    Log(Text_A, "A");

                    form = form.Replace("A", Text_A.Text.Trim().ToString());

                    form = form.Replace(",", ".");


                    string value = new DataTable().Compute(form, "").ToString();

                    Rez.Text = Math.Round(Double.Parse(value), 2).ToString();
                }





            }
            if (Form.Text.Contains("A") & Form.Text.Contains("B") & !Form.Text.Contains("C"))
            {
                if (Text_A.Text.Trim() == "")
                {
                    Text_A.ToolTip = "Парамаетр A не введен";
                    Text_A.Background = Brushes.Pink;
                }

                else
                {
                    Text_A.Background = Brushes.Transparent;
                    Text_A.ToolTip = null;
                }
                if (Text_B.Text.Trim() == "")
                {
                    Text_B.ToolTip = "Парамаетр B не введен";
                    Text_B.Background = Brushes.Pink;
                }

                else
                {
                    Text_B.Background = Brushes.Transparent;
                    Text_B.ToolTip = null;

                }



                if (Text_A.Background == Brushes.Transparent & Text_B.Background == Brushes.Transparent)
                {
                   


                    ChangeTrig(Text_A, "A");
                    ChangeTrig(Text_B, "B");

                   



                    form = form.Replace("A", Text_A.Text.Trim().ToString());

                    form = form.Replace("B", Text_B.Text.Trim().ToString());

                    form = form.Replace(",", ".");
                    string value_2 = new DataTable().Compute(form, "").ToString();


                    Rez.Text = Math.Round(Convert.ToDouble(value_2), 2).ToString();
                }

            }









            if (Form.Text.Contains("A") & Form.Text.Contains("B") & Form.Text.Contains("C"))
            {
                if (Text_A.Text.Trim() == "")
                {
                    Text_A.ToolTip = "Парамаетр A не введен";
                    Text_A.Background = Brushes.Pink;
                }

                else
                {
                    Text_A.Background = Brushes.Transparent;


                    Text_A.ToolTip = null;
                }
                if (Text_B.Text.Trim() == "")
                {
                    Text_B.ToolTip = "Парамаетр B не введен";
                    Text_B.Background = Brushes.Pink;
                }

                else
                {
                    Text_B.Background = Brushes.Transparent;

                    Text_B.ToolTip = null;
                }
                if (Text_C.Text.Trim() == "")
                {
                    Text_C.ToolTip = "Парамаетр C не введен";
                    Text_C.Background = Brushes.Pink;
                }

                else
                {
                    Text_C.Background = Brushes.Transparent;

                    Text_C.ToolTip = null;
                }

                if (Text_A.Background == Brushes.Transparent & Text_B.Background == Brushes.Transparent & Text_C.Background == Brushes.Transparent)
                {
                    if (Text_A.Text.EndsWith(",") == true)
                    {
                        Text_A.Text += "0";
                    }
                    if (Text_B.Text.EndsWith(",") == true)
                    {
                        Text_B.Text += "0";
                    }
                    if (Text_C.Text.EndsWith(",") == true)
                    {
                        Text_C.Text += "0";
                    }

                

                    ChangeTrig(Text_A, "A");
                    ChangeTrig(Text_B, "B");
                    ChangeTrig(Text_C, "C");




                    form = form.Replace("A", Text_A.Text.Trim().ToString());

                    form = form.Replace("B", Text_B.Text.Trim().ToString());

                    form = form.Replace("C", Text_C.Text.Trim().ToString());



                    form = form.Replace(",", ".");

                    string value_3 = new DataTable().Compute(form, "").ToString();

                    Rez.Text = Math.Round(Convert.ToDouble(value_3), 2).ToString();

                }

            }








    

        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            Text_A.Text = "";
            Text_B.Text = "";
            Text_C.Text = "";
            Text_A.Background = Brushes.Transparent;
            Text_A.ToolTip = null;
            Text_B.ToolTip = null;
            Text_C.ToolTip = null;
            Text_B.Background = Brushes.Transparent;
            Text_C.Background = Brushes.Transparent;

            Rez.Text = "";

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            double val;
            if (textBox.Text == "" && e.Text == "-")
            {
                e.Handled = false; 
            }
            else if (e.Text == "-")
            {
                e.Handled = true; 
            }
            else
            {
              e.Handled = !double.TryParse(fullText, out val); 
            }
        }




    }
}
