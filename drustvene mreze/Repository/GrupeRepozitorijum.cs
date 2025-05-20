using System.Globalization;
using drustvene_mreze.Domen;

namespace drustvene_mreze.Repository
{
    public class GrupeRepozitorijum
    {
        private string Putanja = "data/grupe.csv";

        public static Dictionary<int, Grupe> Podaci;

        public GrupeRepozitorijum()
        {
            if (Podaci == null) 
            {
                Ucitaj();
            }
        }

        private void Ucitaj()
        {
            Podaci = new Dictionary<int, Grupe>();

            string[] linije = File.ReadAllLines(Putanja);
            foreach (string linija in linije)
            {
                string[] vrednosti = linija.Split(",");
                int id = int.Parse(vrednosti[0]);
                string naziv = vrednosti[1];
                DateTime datum = DateTime.ParseExact(vrednosti[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Grupe grupa = new Grupe(id, naziv, datum);
                Podaci[id] = grupa;
            }
        }

        public void Sacuvaj()
        {
            List<string> linije = new List<string>();
            foreach (Grupe grupa in Podaci.Values)
            {
                string linija = $"{grupa.id},{grupa.naziv},{grupa.datumOsnivanja:yyyy-MM-dd}";
                linije.Add(linija);
            }
            File.WriteAllLines(Putanja, linije);
        }
    }
}
