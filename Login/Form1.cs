using System.Net.Http.Headers;
using System.Net.Security;
using Login.Models;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Net.Http;
using System;

namespace Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sende, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.BaseAddress = new Uri("https://localhost:7270/api/Home/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                LoginUsuario login = new LoginUsuario()
                {
                    Usuario = UsuarioLogin.Text,
                    Pass = PassLogin.Text
                };
                var url = await CreatePersonAsync(login);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        static async Task<Uri> CreatePersonAsync(LoginUsuario login)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sende, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "https://localhost:7270/api/Home/Login", login);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show("Tu usuario y pass no coinciden o no te has registrado.");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                MessageBox.Show("Ingresa los datos correctos");
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show("Bienvenido " + login.Usuario + "!");
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
            return response.Headers.Location;
        }
    }
}
