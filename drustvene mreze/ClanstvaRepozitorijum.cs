namespace drustvene_mreze
{
    public class ClanstvoRepozitorijum
    {
        private readonly string filePath = "data/clanstva.csv";

        public List<(int UserId, int GroupId)> GetAll()
        {
            if (!File.Exists(filePath)) return new List<(int, int)>();

            return File.ReadAllLines(filePath)
                .Select(line => line.Split(','))
                .Where(parts => parts.Length == 2)
                .Select(parts => (int.Parse(parts[0]), int.Parse(parts[1])))
                .ToList();
        }

        public List<int> GetUserIdsByGroupId(int groupId)
        {
            return GetAll()
                .Where(pair => pair.GroupId == groupId)
                .Select(pair => pair.UserId)
                .ToList();
        }
    }

}
