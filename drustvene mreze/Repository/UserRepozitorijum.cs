using drustvene_mreze.Domen;

namespace drustvene_mreze.Repository
{
    public class UserRepozitorijum
    {
        private readonly string filePath = "data/korisnici.csv";

        public List<User> GetAll()
        {
            if (!File.Exists(filePath)) return new List<User>();

            return File.ReadAllLines(filePath)
                .Select(line => line.Split(','))
                .Select(parts => new User
                {
                    Id = int.Parse(parts[0]),
                    KorisnickoIme = parts[1],
                    Ime = parts[2],
                    Prezime = parts[3],
                    DatumRodjenja = DateTime.Parse(parts[4])
                }).ToList();
        }

        public User? GetById(int id) => GetAll().FirstOrDefault(u => u.Id == id);

        public void Add(User user)
        {
            var users = GetAll();
            users.Add(user);
            SaveAll(users);
        }

        public void Update(User user)
        {
            var users = GetAll();
            var index = users.FindIndex(u => u.Id == user.Id);
            if (index != -1)
            {
                users[index] = user;
                SaveAll(users);
            }
        }

        private void SaveAll(List<User> users)
        {
            var lines = users.Select(u =>
                $"{u.Id},{u.KorisnickoIme},{u.Ime},{u.Prezime},{u.DatumRodjenja:yyyy-MM-dd}");
            File.WriteAllLines(filePath, lines);
        }
    }
}