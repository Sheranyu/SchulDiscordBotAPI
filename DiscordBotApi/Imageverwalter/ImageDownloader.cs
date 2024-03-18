using DiscordBotApi.Models;
using System.Net.Http;
using System;
using DiscordBotApi.ApiModels;

namespace DiscordBotApi.Imageverwalter
{
    public class ImageDownloader
    {
        private readonly HttpClient httpClient;

        public ImageDownloader(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task PictureDownloader(CustomMemePictureModel model, int picturename)
        {
            var response = await httpClient.GetAsync(model.url);
            if (response.IsSuccessStatusCode)
            {

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{model.DiscordGuildID}", model.MemeName, $"{picturename}.png");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{model.MemeName}", model.MemeName));
                }
                await Console.Out.WriteLineAsync(filePath);
                using (var fileStream = File.Create(filePath))
                {
                    // Daten aus der Antwort in die Datei kopieren
                    try
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                    catch (Exception)
                    {

                        await DeleteFileAsync(filePath);
                    }
                }
            }
            else
            {
                await Console.Out.WriteLineAsync("\n\n\n" + response.ToString() + "\n\n\n");
            }

        }
        public async Task DeleteFileAsync(string pfad)
        {
            File.Delete(pfad);
            await Task.CompletedTask;
        }
    }
}
