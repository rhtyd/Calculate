using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Calculate
{
    /// <summary>
    /// Логика взаимодействия для SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        ApplicationContext db;
        public SecondWindow()
        {
            InitializeComponent();
            db = new ApplicationContext();
            UpdateData update = new UpdateData();
            Combo.ItemsSource = update.Updates();
        }

        private void Close(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();

        }

        private void Add(object sender, RoutedEventArgs e)
        {
            string name = Name.Text.Trim();
            string form = Form.Text.Trim();

            Name.Background = Brushes.Transparent;

            Form.Background = Brushes.Transparent;



            if (name == "") {
                Name.ToolTip = "Имя не введено";
               Name.Background = Brushes.Pink;
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=data.db"))
                {
                    connection.Open();

                    string targetName = Name.Text.Trim();
                    string selectQuery = "SELECT COUNT(*) FROM Calcs WHERE name = @targetName";

                    // Create a command with the SELECT statement and connection
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        // Set the parameter value for the record you want to check
                        command.Parameters.AddWithValue("@targetName", targetName);

                        // Execute the SELECT statement and get the result
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Check if the count is greater than 0
                        if (count > 0)
                        {
                            Name.ToolTip = "Такое имя уже существует";
                            Name.Background = Brushes.Pink;

                        }
                        else
                        {
                            Name.Background = Brushes.Transparent;
                     
                            Name.ToolTip = null;
                        }
                    }


                    connection.Close();
                }
            }

            if (form == "")
            {
                Form.ToolTip = "Формула не введена";
                Form.Background = Brushes.Pink;
            } 
            else
            {
                Form.Background = Brushes.Transparent;

                Form.ToolTip = null;
            }
           
           
           
              
                   






            if (Name.Background == Brushes.Transparent &
              Form.Background == Brushes.Transparent 
            )
            {
                Name.Background = Brushes.PaleGreen;
                Form.Background = Brushes.PaleGreen;

                Calc calc = new Calc(name, form);
                db.Calcs.Add(calc);
                db.SaveChanges();
                //List<Calc> calcs = db.Calcs.ToList();

                //List<string> str = new List<string>();
                //foreach (Calc calc_1 in calcs)
                //{
                //    str.Add(calc_1.name);
                //}
                //  Combo.ItemsSource = str;
                UpdateData update = new UpdateData();

                Combo.ItemsSource = update.Updates();
            }
           
        }











        private void Delete(object sender, RoutedEventArgs e)
        {


       

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=data.db"))
            {
                connection.Open();
                 string targetName = Combo.SelectedItem as string;
                string deleteQuery = $"DELETE FROM Calcs WHERE name =@targetName;";
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                 command.Parameters.AddWithValue("@targetName", targetName);
                  command.ExecuteNonQuery();
                    UpdateData update = new UpdateData();
                    Combo.ItemsSource = update.Updates();
                }
                              
                connection.Close();
            }

        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Name.Background = Brushes.Transparent;
            Name.Text = "";
            Form.Text = "";
            Form.Background = Brushes.Transparent;


        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-3]+");
            e.Handled = regex.IsMatch(e.Text);
        }









    }
    }

