using Book_Store_Core.Models;
using System.Net.Mail;
using System.Net;

namespace Book_Store_API.Helper
{
    public class EmailService
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mohammadaljad.1234@gmail.com", "nrur egsy ryus fvrg");
            client.Send("mohammadaljad.1234@gmail.com", email.Recivers, email.Subject, email.Body);
        }
    }
}
