namespace MotoBest.Seeder.JsonModels
{
    using System.Collections.Generic;

    public class ModelJsonModel
    {
        public ModelJsonModel()
        {
            Variations = new HashSet<string>();
        }

        public string Name { get; set; }

        public ICollection<string> Variations { get; set; }
    }
}
