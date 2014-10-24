using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Meta;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class AbilityScoreImprovementViewModel : DirtifiableObject
    {
        public AbilityScoreImprovementViewModel()
        {
            AvailableOptions = new ReactiveList<Ability>();
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

        private bool _selected;

        public bool IsSelected
        {
            get { return _selected; }
            set { this.RaiseAndSetIfChanged(ref _selected, value); }
        }

        public ReactiveList<Ability> AvailableOptions { get; private set; }
    }
}
