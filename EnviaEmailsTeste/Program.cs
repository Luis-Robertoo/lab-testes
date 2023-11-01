// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Mail;

Console.WriteLine("Hello, World!");


var client = new SmtpClient 
{
    Host = "smtp.sendgrid.net",
    Port = 587,
    Credentials = new NetworkCredential("apikey", "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"),
    EnableSsl = false

};

var reme = new MailAddress("luis.r.s14@hotmail.com");
var dest = new MailAddress("luis.r.s12@hotmail.com");

//:\Projetos\Luis\CSHARP\TestarAsParadas\EnviaEmailsTeste\template - email.html


string bodyHtmlTemplate = File.ReadAllText("../../../template-email.html");

bodyHtmlTemplate = bodyHtmlTemplate.Replace("{{codigo}}", $"{Guid.NewGuid().ToString()}");

var mensagemDeEmail = new MailMessage(reme, dest);
mensagemDeEmail.Subject = "titulo aqui";
mensagemDeEmail.IsBodyHtml = true;
mensagemDeEmail.Body = bodyHtmlTemplate;

client.Send(mensagemDeEmail);