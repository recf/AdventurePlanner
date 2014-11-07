using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Meta;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class SaveProficiencyViewModel : DirtifiableObject
    {
        private Ability _ability;

        public Ability Ability
        {
            get { return _ability; }
            set { this.RaiseAndSetIfChanged(ref _ability, value); }
        }
        
        private bool _proficient;

        public bool IsProficient
        {
            get { return _proficient; }
            set { this.RaiseAndSetIfChanged(ref _proficient, value); }
        }
    }
}
