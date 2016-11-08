using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using Site.Models;
using System;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<SendMailOptions> Options;

        public HomeController(IOptions<SendMailOptions> options)
        {
            Options = options;
        }

        public IActionResult Index() => View();

        public IActionResult Host() => View("Host", null);

        public IActionResult Session() => View("Session", null);

        [HttpPost]
        public IActionResult NewSession(string name, string email, string title, string track)
        {
            var vm = new PostResult();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(track))
            {
                vm.Success = false;
                vm.Message = "<p>Todos os campos são obrigatórios!</p>";
            }
            else
            {
                try
                {
                    SendEmail("MCT Summit - Sessao", $"nome: {name}\r\nemail: {email}\r\ntitulo: {title}\r\ntrilha: {track}");

                    vm.Success = true;
                    vm.Message = "<p>Sucesso! Entraremos em contato em breve.</p>";
                }
                catch (Exception)
                {
                    vm.Success = false;
                    vm.Message = "<p>Ops, ocorreu um erro! Estamos de olho, por favor retorne em breve.</p>";
                }
            }

            return View("Session", vm);
        }

        [HttpPost]
        public IActionResult NewHost(string name, string email, string city)
        {
            var vm = new PostResult();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(city))
            {
                vm.Success = false;
                vm.Message = "<p>Todos os campos são obrigatórios!</p>";
            }
            else
            {
                try
                {
                    SendEmail("MCT Summit - Host", $"nome: {name}\r\nemail: {email}\r\ncidade: {city}");

                    vm.Success = true;
                    vm.Message = "<p>Sucesso! Entraremos em contato em breve.</p>";
                }
                catch (Exception)
                {
                    vm.Success = false;
                    vm.Message = "<p>Ops, ocorreu um erro! Estamos de olho, por favor retorne em breve.</p>";
                }
            }

            return View("Host", vm);
        }

        private void SendEmail(string subject, string body)
        {
            var mailgunBaseURL = Options.Value.MailgunBaseURL;
            var mailgunResourceURL = Options.Value.MailgunResourceURL;
            var mailgunAPIKey = Options.Value.MailgunAPIKey;
            var to = Options.Value.EmailTo;

            RestClient client = new RestClient();
            client.BaseUrl = new Uri(mailgunBaseURL);
            client.Authenticator = new HttpBasicAuthenticator("api", mailgunAPIKey);

            RestRequest request = new RestRequest();
            request.AddParameter("from", "Site MCT Summit <andre@calil.com.br>");
            request.Resource = mailgunResourceURL;
            request.AddParameter("to", to);

            request.AddParameter("subject", subject);

            request.AddParameter("text", body);
            request.Method = Method.POST;

            var result = client.Execute(request);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}