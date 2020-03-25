using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabHTTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                Authorization(textBox2.Text, textBox3.Text);
            }
            else
            {
                MessageBox.Show("Введите логин/пароль", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        static void Authorization(string login, string password)
        {
            if (File.Exists("res2.txt"))
            {
                File.Delete("res2.txt");
            }

            var cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                var values = new Dictionary<String, String>
                {
                    { "nick", login },
                    { "pass", password }
                   // { "login_ok", "" }
                };

                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync("https://foiz.ru/aut.php?", content)
                   .GetAwaiter()
                   .GetResult();
                var responseString = response.Content.ReadAsStringAsync();
                Uri uri = new Uri("https://foiz.ru");
                var c = cookieContainer.GetCookies(uri);

                for (int i = 0; i < c.Count; i++)
                {
                    File.AppendAllText("res2.txt", c[i].Name, Encoding.UTF8);
                    File.AppendAllText("res2.txt", "=", Encoding.UTF8);
                    File.AppendAllText("res2.txt", c[i].Value + "\n", Encoding.UTF8);


                }
                if(File.Exists("res2.txt"))
                    Process.Start("res2.txt");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var cookieContainer = new CookieContainer();
            if (File.Exists("res2.txt"))
            {
                string[] cookies = File.ReadAllLines(@"res2.txt", System.Text.Encoding.Default);

                foreach (string cookie1 in cookies)
                    cookieContainer.SetCookies(new Uri("https://foiz.ru"), cookie1);

            }
            Uri uri = new Uri("https://foiz.ru/info.php?link_id=93446");
                var values1 = new Dictionary<String, String>
                {

                };
                var content1 = new FormUrlEncodedContent(values1);
                using (var handler1 = new HttpClientHandler { CookieContainer = cookieContainer })
                using (var client1 = new HttpClient(handler1))
                {
                
                    using (HttpResponseMessage response = await client1.GetAsync(uri))
                    {
                    
                    var s = response.Headers.ToString();
                    richTextBox1.Text = s;
                    using (HttpContent content = response.Content)
                        {
                            string mycontent = await content.ReadAsStringAsync();
                       
                        webBrowser1.DocumentText = mycontent;
                    }
                    
                }
                
                }
            
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri("https://foiz.ru");
                using (HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)))
                {
                    richTextBox1.Text = response.ToString();
                }


            }
            
         
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri("http://unite.md");
                using (HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, url)))
                {
                    richTextBox1.Text = response.ToString();
                }


            }

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var cookieContainer = new CookieContainer();
            if (File.Exists("res2.txt"))
            {
                string[] cookies = File.ReadAllLines(@"res2.txt", System.Text.Encoding.Default);

                foreach (string cookie1 in cookies)
                    cookieContainer.SetCookies(new Uri("https://foiz.ru"), cookie1);

            }
            Uri uri = new Uri("https://foiz.ru/users.php?go&sort=id&DESC");
            var values1 = new Dictionary<String, String>
            {
                { "usearch", textBox2.Text.ToString()},
                { "serch", "2" }

            };
            var content1 = new FormUrlEncodedContent(values1);
            using (var handler1 = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client1 = new HttpClient(handler1))
            {

                using (HttpResponseMessage response = await client1.PostAsync(uri, content1))
                {

                    var s = response.Headers.ToString();
                    richTextBox1.Text = s;
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();

                        webBrowser1.DocumentText = mycontent;
                    }

                }

            }
        }
    }
}
