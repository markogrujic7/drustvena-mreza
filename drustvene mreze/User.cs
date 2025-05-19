using System.Text.RegularExpressions;

namespace drustvene_mreze
{
    public class User
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
    }
}
