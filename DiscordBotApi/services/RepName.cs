using DiscordBotApi.CustomException;
using DiscordBotApi.Models;
using DiscordBotApi.services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DiscordBotApi.services
{
    public class RepName : IRepName
    {
        private readonly PostgresContext db;
        private readonly ILogger<RepName> logger;

        public RepName(PostgresContext db, ILogger<RepName> logger)
        {
            this.db = db;
            this.logger = logger;
        }
        public void ChangeMemeBeschreibungbyName(string memename)
        {
            throw new NotImplementedException();
        }

        public MemeName CreateMemeNamemitBeschreibung(string Beschreibung, string memename)
        {
            var namelowercase = memename.ToLower();
            var beschreibunlowercase = Beschreibung.ToLower();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var memenamedata = new MemeName
                    {
                        Name = namelowercase,
                        CreatedAt = DateTime.UtcNow,
                        //TextInhalts = new TextInhalt[]
                        //{
                        //    new() { Text = beschreibunlowercase, CreatedAt = DateTime.UtcNow}
                        //}
                    };

                    db.MemeNames.Add(memenamedata);
                    db.SaveChanges(); // Speichere MemeName

                    var newbeschreibung = new TextInhalt
                    {
                        Text = beschreibunlowercase,
                        CreatedAt = DateTime.UtcNow,
                        NamesId = memenamedata.Id,
                    };

                    db.TextInhalts.Add(newbeschreibung);
                    db.SaveChanges(); // Speichere TextInhalt

                    transaction.Commit();
                    return memenamedata;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Log(logLevel: LogLevel.Error, ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }

        public MemeName CreateMemeNameOnly(string memename)
        {
            memename = memename.ToLower();
            try
            {
                var memenamedata = new MemeName
                {
                    Name = memename,
                    CreatedAt = DateTime.UtcNow,
                };

                db.MemeNames.Add(memenamedata);
                db.SaveChanges(); // Speichere MemeName
                return memenamedata;
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

        public TextInhalt[] GetBeschreibungfromMemeNames(string memeName)
        {
            var beschreibungdata = db.TextInhalts.Where(x => x.Names.Name == memeName).ToArray();

            if (beschreibungdata is not null)
            {
                return beschreibungdata;
            }
            else
            {
                return [];
            }
        }

        public MemeName GetMemeName(string memeName)
        {
            throw new NotImplementedException();
        }

        //nur ein test
        public static readonly Func<PostgresContext, MemeName[]> MemeNameArray = EF.CompileQuery(
            (PostgresContext context) => context.MemeNames.ToArray());
        public MemeName[] GetMeme_List()
        {
            var arraydata = db.MemeNames.ToArray();
            if (arraydata is not null) return arraydata;
            else
            {
                return [];
            }
        }

        public MemeName GetMeme_NamesbyName(string name)
        {
            var memenames = db.MemeNames.SingleOrDefault(x => x.Name == name);
            if (memenames is not null)
            {
                return memenames;
            }
            else
            {
                throw new Exception("No user found");
            }
        }

        public void UpdateMemeNamebyName(string memename)
        {
            throw new NotImplementedException();
        }

        public MemeName DeleteMemeNameByID(int id)
        {
            try
            {
                var memeitem = db.MemeNames.Remove(db.MemeNames.First(x => x.Id == id));
                
                MemeName memeItem = memeitem.Entity;
                db.SaveChanges();

                return memeItem;
            }
            catch (Exception)
            {

                throw new Exception("Fehler beim Löschen des MemeName-Eintrags.");
            }
        }

        public void UpdateBeschreibung(int beschreibungid, string changedtext)
        {
            // LINQ-Abfrage, um den Textinhalt basierend auf der beschreibungid abzurufen
            var inhalt = (from textinhalt in db.TextInhalts
                         where textinhalt.IdText == beschreibungid
                         select textinhalt).FirstOrDefault();

            // Überprüfen, ob der Textinhalt gefunden wurde, bevor Sie ihn aktualisieren
            if (inhalt != null)
            {
                // Änderungen am Textinhalt vornehmen
                inhalt.Text = changedtext;
                // Aktualisieren Sie den Textinhalt in der Datenbank
                db.TextInhalts.Update(inhalt);
                // Speichern Sie die Änderungen in der Datenbank
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Keinen Beschreibung gefunden");
            }
        }

        public void DeleteBeschreibung(int beschreibungid)
        {
            var inhalt = (from textinhalt in db.TextInhalts
                         where textinhalt.IdText == beschreibungid
                         select textinhalt).FirstOrDefault();
            if (inhalt != null)
            {
                db.TextInhalts.Remove(inhalt);
                db.SaveChanges();
            }
            else
            {
                throw new Nohtingfound("Keinen Beschreibung gefunden");
            }
        }
    }
}
