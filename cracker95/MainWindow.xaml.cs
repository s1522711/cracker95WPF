using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cracker95
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static string GenerateCdKey()
        {
            // initialise needed vars
            Random r = new Random();
            string CdCDBox1, CdCDBox2;
            CdCDBox1 = string.Empty;
            CdCDBox2 = "1";

            while (CdCDBox1 == "333" && CdCDBox1 == "444" && CdCDBox1 == "555" && CdCDBox1 == "666" && CdCDBox1 == "777" && CdCDBox1 == "888" && CdCDBox1 == "999") // generate box 1
            {
                CdCDBox1 = r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString();
            }

            while (CdCDBox2.ToString().Select(digit => int.Parse(digit.ToString())).ToArray().Sum() % 7 != 0) // generate box 2
            {
                // keyInputBox.Text = keyInputBox.Text + CdCDBox2 + "\n";
                CdCDBox2 = r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(1, 7).ToString();
            }

            return CdCDBox1 + "-" + CdCDBox2 + "\n"; // output generated CD key
        }

        private static string GenerateOemKey()
        {
            // initialise needed vars
            Random r = new Random();
            string OemCDBox1P1, OemCDBox1P2, OemCDBox2, OemBox3;
            OemCDBox1P1 = string.Empty;
            OemCDBox1P2 = string.Empty;
            OemCDBox2 = "1";
            OemBox3 = string.Empty;

            // generate both parts of box 1
            OemCDBox1P1 = r.Next(100, 365).ToString();
            OemCDBox1P2 = r.Next(95, 99).ToString();

            while (OemCDBox2.ToString().Select(digit => int.Parse(digit.ToString())).ToArray().Sum() % 7 != 0) // generate box 2
            {
                // keyInputBox.Text = keyInputBox.Text + CdCDBox2 + "\n";
                OemCDBox2 = "0" + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(1, 7).ToString();
            }

            OemBox3 = r.Next(10000, 99999).ToString(); // generate last 5 digits of key (box 5)

            return OemCDBox1P1 + OemCDBox1P2 + "-OEM-" + OemCDBox2 + "-" + OemBox3 + "\n"; // output generated OEM key
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (keyOutputBox.Text.Count(x => x == '\n') == 13)
            {
                keyOutputBox.Text = "";
            }
            if (oemRadio.IsChecked == true)
            {
                if (keyOutputBox.Text.Contains("start"))
                {
                    keyOutputBox.Text = "";
                    keyOutputBox.FontFamily = new FontFamily("Segoe UI");
                    keyOutputBox.FontSize = 12;
                }
                keyOutputBox.Text += GenerateOemKey();
            }
            else if (cdKeyRadio.IsChecked == true)
            {
                if (keyOutputBox.Text.Contains("start"))
                {
                    keyOutputBox.Text = "";
                    keyOutputBox.FontFamily = new FontFamily("Segoe UI");
                    keyOutputBox.FontSize = 12;
                }
                keyOutputBox.Text += GenerateCdKey();
            }
            else
            {
                keygenErrorBackground.Visibility = Visibility.Visible;
                keygenErrorText.Visibility = Visibility.Visible;
                keygenErrorBtn.Visibility = Visibility.Visible;
            }
        }

        private void keygenErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            keygenErrorBackground.Visibility = Visibility.Hidden;
            keygenErrorText.Visibility = Visibility.Hidden;
            keygenErrorBtn.Visibility = Visibility.Hidden;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            keyOutputBox.Text = "Click the button to start";
            keyOutputBox.FontFamily = new FontFamily("Segoe UI Light");
            keyOutputBox.FontSize = 24;
        }

        private void keyInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (keyInputBox.Text == "")
            {
                validatorTextboxPlaceholder.Text = "Enter a key to validate";
                validatorTextboxPlaceholder.Foreground = Brushes.Gray;
                validatorTextboxPlaceholder.FontSize = 30;
                validatorTextboxPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                validatorTextboxPlaceholder.Visibility = Visibility.Hidden;
            }

            if (keyInputBox.Text.Length == 23 || keyInputBox.Text.Length == 11)
            {
                keyInputBox.Foreground = Brushes.Black;
            }
            else
            {
                keyInputBox.Foreground = Brushes.Red;
            }
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (keyInputBox.Text.ToLower().Contains("oem")) // check if key is oem key
                {
                    // 01234567890123456789012
                    // AAABB-OEM-CCCCCCC-DDDDD
                    // initialising needed vars
                    string OemCDBox1P1, OemCDBox1P2, OemCDBox2, OemBox3, OemErrors;
                    OemCDBox1P1 = keyInputBox.Text[0].ToString() + keyInputBox.Text[1].ToString() + keyInputBox.Text[2].ToString();
                    OemCDBox1P2 = keyInputBox.Text[3].ToString() + keyInputBox.Text[4].ToString();
                    OemCDBox2 = keyInputBox.Text[10].ToString() + keyInputBox.Text[11].ToString() + keyInputBox.Text[12].ToString() + keyInputBox.Text[13].ToString() + keyInputBox.Text[14].ToString() + keyInputBox.Text[15].ToString() + keyInputBox.Text[16].ToString();
                    OemBox3 = keyInputBox.Text[18].ToString() + keyInputBox.Text[19].ToString() + keyInputBox.Text[20].ToString() + keyInputBox.Text[21].ToString() + keyInputBox.Text[22].ToString();

                    Int32 ValidityScore = 0;
                    OemErrors = "";

                    if (001 <= int.Parse(OemCDBox1P1) && int.Parse(OemCDBox1P1) <= 366) { ValidityScore += 1; } // check if first 3 chars of box 1 are allowed
                    else { OemErrors += "First 3 digits of the first box are not between 001 and 366, "; }

                    if (OemCDBox1P2 == "95" || OemCDBox1P2 == "96" || OemCDBox1P2 == "97" || OemCDBox1P2 == "98" || OemCDBox1P2 == "99" || OemCDBox1P2 == "00" || OemCDBox1P2 == "01" || OemCDBox1P2 == "02" || OemCDBox1P2 == "03") { ValidityScore += 1; } // check if the last 2 chars in box 1 are allowed
                    else { OemErrors += "Last 2 digits of the first box are not 95,96 ,97,98,99,00,01,02 or 03, "; }

                    if (keyInputBox.Text[5].ToString() == "-" && keyInputBox.Text[9].ToString() == "-" && keyInputBox.Text[17].ToString() == "-") { ValidityScore += 1; } // check if the dashes are dashes
                    else { OemErrors += "Characters 6, 10 and 18 are not dashes, "; }

                    if (OemCDBox2.ToString().Select(digit => int.Parse(digit.ToString())).ToArray().Sum() % 7 == 0) { ValidityScore += 1; } // check sum of digits in box 2
                    else { OemErrors += "the sum of the characters in the 2nd box isnt divisible by 7, "; }

                    if (OemCDBox2[0].ToString() == "0") { ValidityScore += 1; } // check if first char in box 2 is 0
                    else { OemErrors += "The first digit of the 2nd box must be 0, "; }

                    if (keyInputBox.Text.Length == 23) { ValidityScore += 1; } // check if key is 23 chars long
                    else { OemErrors += "The key must be 23 characters long"; }

                    validatorTextboxPlaceholder.Visibility = Visibility.Visible;
                    keyInputBox.Text = "";
                    if (ValidityScore == 6) // check if enough validity checks passed
                    {
                        validatorTextboxPlaceholder.Foreground = Brushes.Green;
                        validatorTextboxPlaceholder.Text = "OEM Key is valid!";
                    }
                    else
                    {
                        validatorTextboxPlaceholder.Foreground = Brushes.Red;
                        validatorTextboxPlaceholder.Text = "OEM Key is not valid: " + OemErrors;
                    }
                }
                else // cd key checks
                {
                    // initialising most of the needed vars
                    string CDBox1 = keyInputBox.Text[0].ToString() + keyInputBox.Text[1].ToString() + keyInputBox.Text[2].ToString();
                    string CDBox2 = keyInputBox.Text[4].ToString() + keyInputBox.Text[5].ToString() + keyInputBox.Text[6].ToString() + keyInputBox.Text[7].ToString() + keyInputBox.Text[8].ToString() + keyInputBox.Text[9].ToString() + keyInputBox.Text[10].ToString();
                    validatorTextboxPlaceholder.Text = CDBox1 + CDBox2;
                    int ValidityScore = 0;
                    string KeyErrors = "";

                    if (CDBox1 != "333" && CDBox1 != "444" && CDBox1 != "555" && CDBox1 != "666" && CDBox1 != "777" && CDBox1 != "888" && CDBox1 != "999") // check if the first box isnt equal to blacklisted values
                    {
                        ValidityScore += 1;
                    }
                    else { KeyErrors += "The Digits In The First Box Cant Be 333, 444, 555, 666, 777, 888 or 999, "; }

                    if (CDBox2.ToString().Select(digit => int.Parse(digit.ToString())).ToArray().Sum() % 7 == 0) // check sum of digits in second box
                    {
                        ValidityScore += 1;
                    }
                    else { KeyErrors += "Sum of Digits In Key Isnt Devisable By 7, "; }

                    if (int.Parse(CDBox2[6].ToString()) != 0 && int.Parse(CDBox2[6].ToString()) < 8) // check if last digit is higher than 7 or is 0
                    {
                        ValidityScore += 1;
                    }
                    else { KeyErrors += "The last digit of the key must be lower than 8 and cant be 0, "; }


                    if (keyInputBox.Text.Length == 11) // check key length
                    {
                        ValidityScore += 1;
                    }
                    else { KeyErrors += "Key lenght isnt 11"; }

                    validatorTextboxPlaceholder.Visibility = Visibility.Visible;
                    keyInputBox.Text = "";
                    if (ValidityScore == 4)
                    {
                        validatorTextboxPlaceholder.Foreground = Brushes.Green;
                        validatorTextboxPlaceholder.Text = "CD Key Valid!";
                    } // check if enough validity checks have been passed
                    else
                    {
                        validatorTextboxPlaceholder.Foreground = Brushes.Red;
                        validatorTextboxPlaceholder.FontSize = 12;
                        validatorTextboxPlaceholder.Text = "CD Key Invalid: " + KeyErrors;
                    }
                }
            }
            catch (Exception IndexOutOfRangeException)
            {
                Console.WriteLine(IndexOutOfRangeException);
                validatorTextboxPlaceholder.Visibility = Visibility.Visible;
                keyInputBox.Text = "";
                validatorTextboxPlaceholder.Foreground = Brushes.Red;
                validatorTextboxPlaceholder.Text = "Key length must be 11 for CD Keys and 23 for OEM Keys";
            }
        }
    }
}