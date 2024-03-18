using DiscordBotApi.Models;

namespace DiscordBotApi.services.Contracts
{
    public interface IRepName
    {
        public MemeName CreateMemeNamemitBeschreibung(string Beschreibung, string memename);
        public MemeName CreateMemeNameOnly(string memename);
        public MemeName GetMemeName(string memeName);
        public void ChangeMemeBeschreibungbyName(string memename);

        public MemeName[] GetMeme_List();
        public MemeName GetMeme_NamesbyName(string name);
        public void UpdateMemeNamebyName(string memename);
        public TextInhalt[] GetBeschreibungfromMemeNames(string memeName);
        public MemeName DeleteMemeNameByID(int id);
        public void UpdateBeschreibung(int beschreibungid, string changedtext);
        public void DeleteBeschreibung(int beschreibungid);
    }
}
