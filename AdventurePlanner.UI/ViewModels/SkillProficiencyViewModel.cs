using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Meta;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class SkillProficiencyViewModel : DirtifiableObject
    {
        private Skill _skill;

        public Skill Skill
        {
            get { return _skill; }
            set { this.RaiseAndSetIfChanged(ref _skill, value); }
        }

        private bool _proficient;

        public bool IsProficient 
        {
            get { return _proficient; }
            set { this.RaiseAndSetIfChanged(ref _proficient, value); }
        }
    }
}
