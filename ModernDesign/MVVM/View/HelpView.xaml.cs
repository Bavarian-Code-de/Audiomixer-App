using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GSApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for DiscoveryView.xaml
    /// </summary>
    public partial class HelpView : UserControl
    {
        public HelpView()
        {
            InitializeComponent();
        }

        private void Emailtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textboxBorder = (Border)Emailtxt.Template.FindName("TextBorder", Emailtxt);
            var text = (TextBox)Emailtxt.Template.FindName("SearchBar", Emailtxt);
            if (IsValidEmailAdress(text.Text))
            {
                textboxBorder.BorderBrush =  (SolidColorBrush)new BrushConverter().ConvertFrom("#58ACFA");
            }
            else if(text.Text == "")
            {
                textboxBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#58ACFA");

            }
            else
            {
                textboxBorder.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }
        public bool IsValidEmailAdress(string s)
        {
            try
            {
                var mailAddress = new MailAddress(s);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        private void SendMailBtn_Click(object sender, RoutedEventArgs e)
        {
            //MimeMessage emailMessage = new MimeMessage();
            //emailMessage.From.Add(MailboxAddress.Parse("help@gs-electronics.de"));
            //emailMessage.To.Add(MailboxAddress.Parse("help@gs-electronics.de"));
            //emailMessage.Subject = "Support Email from: " + "test";

            //BodyBuilder emailBodyBuilder = new BodyBuilder();
            //emailBodyBuilder.TextBody = "test" + " Says: " + new TextRange(MailMessagetxt.Document.ContentStart, MailMessagetxt.Document.ContentEnd).Text;
            //emailMessage.Body = emailBodyBuilder.ToMessageBody();

            //sendemail(emailMessage);
        }

        private bool CheckMailData()
        {
            return true;
        }
    }
}
