using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using System.Net.Mail;
namespace ProjetosViniciusVieiraMota.Services

{
    public class SmtpService
    {

        public void Main(string adress)
        {
            Execute(adress).Wait();
        }

        static async Task Execute(string adress)
        {
            string poem = "Roses are red,\n" +
                "Violets are blue,\n" +
                "Sugar is sweet,\n" +
                "And so are you.";

            Console.WriteLine("executando");
            Console.WriteLine(adress);
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("viniciusvm1704@gmail.com", "Travelling Poet");
            var subject = " A poem for you!!";
            var to = new EmailAddress(adress, "Most prestigious of guests");
            var plainTextContent = poem;
            var htmlContent = poem;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
