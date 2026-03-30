namespace DogFacts
{
    public class DogFactModel
    {
        private string _id;
        private string _name;
        private string _description;

        public string Id { get => _id; }
        public string Name { get => _name; }
        public string Description { get => _description; }

        public DogFactModel(string id, string name, string description)
        {
            _id = id;
            _name = name;
            _description = description;
        }
    }
}
