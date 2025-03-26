using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MalenomDesk
{

    public class ApiService
    {
        private readonly HttpClient client;

        public ApiService()
        {
            client = new HttpClient();
        }

        public async Task<List<Image>> GetData() //GET
        {
            string response = await client.GetStringAsync("https://localhost:44376/api/images/all");
            return JsonConvert.DeserializeObject<List<Image>>(response);
        }

        public async Task SetData(Image image) //POST
        {
            string json = JsonConvert.SerializeObject(image);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://localhost:44376/api/images/add", content);
            if (response.IsSuccessStatusCode)
            {
                image.ID = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                MessageBox.Show("Данные успешно отправлены.");
            }
            else
            {
                MessageBox.Show(response.StatusCode.ToString());
            }
        }

        public async Task UpdateImage(Image image) //PUT
        {
            string json = JsonConvert.SerializeObject(image);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"https://localhost:44376/api/images/update/{image.ID}", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные успешно обновлены.");
            }
            else
            {
                MessageBox.Show(response.StatusCode.ToString());
            }
        }

        public async Task DeleteData(int id) //DELETE
        {
            HttpResponseMessage response = await client.DeleteAsync($"https://localhost:44376/api/images/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные успешно удалены.");
            }
            else
            {
                MessageBox.Show(response.StatusCode.ToString());
            }
        }
    }
}
