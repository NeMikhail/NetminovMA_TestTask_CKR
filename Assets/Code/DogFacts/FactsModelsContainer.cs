using System.Collections.Generic;

namespace DogFacts
{
    public class FactsModelsContainer
    {
        private List<DogFactModel> _dogFactModels;

        public List<DogFactModel> DogFactModels { get => _dogFactModels; set => _dogFactModels = value; }

        FactsModelsContainer()
        {
            _dogFactModels = new List<DogFactModel>();
        }

        public void ClearModels()
        {
            _dogFactModels.Clear();
        }
    }
}
