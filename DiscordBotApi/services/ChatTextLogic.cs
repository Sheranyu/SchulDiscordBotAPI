using DiscordBotApi.ApiModels;
using DiscordBotApi.Models;

namespace DiscordBotApi.services
{
    public class ChatTextLogic
    {
        private readonly PostgresContext db;

        public ChatTextLogic(PostgresContext db)
        {
            this.db = db;
        }
        public bool CheckAktivierer()
        {
            var ischeck = db.SchulBotConfigs.Select(x => x.Messagelisten).Single();
            if (ischeck != null)
            {
                return (bool)ischeck;
            }
            else
            {
                return false;
            }
        }
        public bool CheckCustomMemeactive()
        {
            var ischeck = db.SchulBotConfigs.Select(x => x.GetRandomMeme).Single();
            return ischeck;

        }
        public void UpdateCheckaktivierer(BotConfigModel config)
        {
            try
            {
                var configdata = db.SchulBotConfigs.Find(config.UserID);

                if (configdata != null)
                {
                    configdata.Messagelisten = config.Allowed;
                    db.SchulBotConfigs.Update(configdata);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("Keine Daten gefunden");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Update zur Datenbank gefailt");
            }


        }

        public void Updatememeaktivierer(BotConfigModel config)
        {
            try
            {
                var configdata = db.SchulBotConfigs.Find(config.UserID);

                if (configdata != null)
                {
                    configdata.GetRandomMeme = config.Allowed;
                    db.SchulBotConfigs.Update(configdata);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("Keine Daten gefunden");
                }
            }
            catch (Exception)
            {
                throw new Exception("Update zur Datenbank gefailt");
            }

        }


    }
}
