using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;

namespace ControlDeInventarios.mvc.Utils
{
    public class EmailSender
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Leer la configuración SMTP del archivo web.config
                var smtpSection = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;

                if (smtpSection == null)
                {
                    throw new Exception("No se encontró la sección de configuración SMTP en el archivo web.config.");
                }

                // Crear el mensaje de correo
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpSection.Network.UserName);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true; // Cambia a false si no envías HTML

                // Configurar el cliente SMTP
                SmtpClient smtpClient = new SmtpClient(smtpSection.Network.Host, smtpSection.Network.Port);
                smtpClient.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                smtpClient.EnableSsl = smtpSection.Network.EnableSsl;

                // Enviar el correo
                smtpClient.Send(mailMessage);

                Console.WriteLine("Correo enviado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}