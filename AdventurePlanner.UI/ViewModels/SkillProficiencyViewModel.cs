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
        public SkillProficiencyViewModel()
        {
            AvailableOptions = new ReactiveList<Skill>();
        }

        private Skill _value;

        public Skill Value
        {
            get { return _value; }
            set { this.RaiseAndSetIfChanged(ref _value, value); }
        }

        public ReactiveList<Skill> AvailableOptions { get; private set; }
    }
}
