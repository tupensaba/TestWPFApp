using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TestWPFApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Configurator { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void createFile_Click(object sender, RoutedEventArgs e)
        {
            if(textField.Text != "")
            {
                SaveToFile();
            }
            textField.Text = "";
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            bool? res = ofd.ShowDialog();
            if (res != false)
            {
                Stream myStream;
                if ((myStream = ofd.OpenFile()) != null)
                {
                    string file_name = ofd.FileName;
                    string file_text = File.ReadAllText(file_name);
                    textField.Text = file_text;
                }
            }
            
        }



        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TimesNewRomanFont_Click(object sender, RoutedEventArgs e)
        {
            textField.FontFamily = new FontFamily("Times New Roman");
            VerdanaFont.IsChecked = false;
        }

        private void Verdana_Click(object sender, RoutedEventArgs e)
        {
            textField.FontFamily = new FontFamily("Verdana");
            TimesNewRomanFont.IsChecked = false;
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            textField.Background = Brushes.Red;
            Blue.IsChecked = false;

        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            textField.Background = Brushes.Blue;
            Red.IsChecked = false;
        }

        private void colorText_Click(object sender, RoutedEventArgs e)
        {

        }

        private void redText_Click(object sender, RoutedEventArgs e)
        {
            textField.Foreground = Brushes.Red;
            blueText.IsChecked = false;
        }

        private void blueText_Click(object sender, RoutedEventArgs e)
        {
            textField.Foreground = Brushes.Blue;
            redText.IsChecked = false;
        }

        private void btnCopyText_Click(object sender, RoutedEventArgs e)
        {
                      
        }

        private void selectFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = selectFontSize.SelectedItem.ToString();

            fontSize = fontSize.Substring(fontSize.Length - 2);

            switch (fontSize)
            {
                case "10":
                    textField.FontSize = 10;
                    break;
                case "14":
                    textField.FontSize = 14;
                    break;
                case "16":
                    textField.FontSize = 16;
                    break;
                case "20":
                    textField.FontSize = 20;
                    break;
                case "24":
                    textField.FontSize = 24;
                    break;
                case "48":
                    textField.FontSize = 48;
                    break;
            }


        }
        private void SaveToFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            
            bool? res = sfd.ShowDialog();

            if (res != false)
            {
                using (Stream s = File.Open(sfd.FileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(textField.Text);
                    }
                }
            }




        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            SqlConnection sql = new SqlConnection(connectionString);

            try
            {
                if(sql.State == System.Data.ConnectionState.Closed)
                {
                    sql.Open();
                }

                string query = "SELECT COUNT(1) FROM Users WHERE login=@login AND password=@pass";
                
                SqlCommand sqlcom = new SqlCommand(query, sql);
                sqlcom.CommandType =System.Data.CommandType.Text;
                sqlcom.Parameters.Add("@login", login.Text);
                sqlcom.Parameters.Add("@pass", password.Password);

                int countUser = Convert.ToInt32(sqlcom.ExecuteScalar());
                if(countUser == 0)
                {
                    query = "INSERT INTO Users(login , password) VALUES (@login , @pass)";

                    SqlCommand com = new SqlCommand(query, sql);
                    com.CommandType = System.Data.CommandType.Text;
                    com.Parameters.Add("@login", login.Text);
                    com.Parameters.Add("@pass", password.Password);

                    com.ExecuteNonQuery();
                    MessageBox.Show("Вы зарегестрированы");


                }
                else
                {
                    MessageBox.Show("Вы авторизованы");
                }
                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                sql.Close();
            }
        }
    }
}
    


