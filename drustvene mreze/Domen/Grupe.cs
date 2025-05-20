namespace drustvene_mreze.Domen
{
    public class Grupe
    {
        public int id {  get; set; }
        public string naziv { get; set; }
        public DateTime datumOsnivanja { get; set; }

        public Grupe(int id, string naziv, DateTime datumOsnivanja)
        {
            this.id = id;
            this.naziv = naziv;
            this.datumOsnivanja = datumOsnivanja;
        }

    }
}
