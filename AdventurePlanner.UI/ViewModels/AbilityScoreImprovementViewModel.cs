using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Domain;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class AbilityScoreImprovementViewModel : DirtifiableObject
    {
        public AbilityScoreImprovementViewModel()
        {
        }

        private Ability _ability;

        public Ability Ability
        {
            get { return _ability; }
            set { this.RaiseAndSetIfChanged(ref _ability, value); }
        }

        private int _improvement;
        
        public int Improvement
        {
            get { return _improvement; }
            set { this.RaiseAndSetIfChanged(ref _improvement, value); }
        }
    }
}
