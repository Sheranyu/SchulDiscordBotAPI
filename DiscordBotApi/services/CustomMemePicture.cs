using DiscordBotApi.ApiModels;
using DiscordBotApi.Models;
using System.Net;
using System;
using Supabase;
using DiscordBotApi.Imageverwalter;

namespace DiscordBotApi;

public class CustomMemePicture : ICustomMemePicture
{
    private readonly Client sbclient;
    private readonly ImageDownloader imageDownloader;
    private readonly Random random;

    public CustomMemePicture(Client sbclient, ImageDownloader imageDownloader)
    {
        this.sbclient = sbclient;
        this.imageDownloader = imageDownloader;
        random = new Random();
    }
    public async Task<string> GetCustomMemeURL(CustomMemePictureModel bilddata)
    {
        List<Supabase.Storage.FileObject> bucket;
        try
        {
            bucket = await sbclient.Storage.From(bilddata.BucketName).List($"Images/{bilddata.DiscordGuildID}/{bilddata.MemeName}");
        }
        catch (Exception)
        {
            throw new Exception();
        }

        bucket = bucket.Where(x => x.Name != ".emptyFolderPlaceholder").ToList();

        int rndzahl = random.Next(0, bucket.Count);
        await Console.Out.WriteLineAsync(rndzahl.ToString());
        string name = bucket[rndzahl].Name;
        return sbclient.Storage.From("MemeBucket").GetPublicUrl($"Images/{bilddata.DiscordGuildID}/{bilddata.MemeName}/{name}");
    }

    public async Task InsertCustomMemeURL(CustomMemePictureModel bilddata)
    {
        int highestNumber = 0;
        try
        {
            var Bilderliste = await sbclient.Storage.From(bilddata.BucketName).List($"Images/{bilddata.DiscordGuildID}/{bilddata.MemeName}");

            foreach (var item in Bilderliste)
            {
                if (item.Name == ".emptyFolderPlaceholder") continue;
                var filename = Path.GetFileNameWithoutExtension(item.Name);
                highestNumber = int.Parse(filename);
            }
            //immer um 1 erhöhen ähnlich wie auto increment
            highestNumber++;

            await imageDownloader.PictureDownloader(bilddata, highestNumber);

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{bilddata.DiscordGuildID}", bilddata.MemeName, $"{highestNumber}.png");

            await sbclient.Storage.From(bilddata.BucketName).Upload(filePath, $"Images/{bilddata.DiscordGuildID}/{bilddata.MemeName}/{highestNumber}.png");
            await imageDownloader.DeleteFileAsync(filePath);

        }
        catch (Exception ex)
        {
            throw new Exception($"irgendwas ist beim einfügen schiefgegangen: {ex}");
        }
    }
}
