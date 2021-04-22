using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace CDEApp.Models
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message) //send email to user that is not register in project
        {
            var _message = new MimeMessage();

            _message.From.Add(new MailboxAddress("Администрация сайта", "cdeapplicationproject@gmail.com")); //add email information 
            _message.To.Add(new MailboxAddress("", email));
            _message.Subject = subject;
            _message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false); //connection to mail service
                await client.AuthenticateAsync("cdeapplicationproject@gmail.com", "CDE123App"); //authentication to mail account
                await client.SendAsync(_message); //send message

                await client.DisconnectAsync(true); //disconntection from mail service
            }
        }
    }
}
