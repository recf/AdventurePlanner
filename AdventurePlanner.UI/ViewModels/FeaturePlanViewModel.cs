using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Meta;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class FeaturePlanViewModel : DirtifiableObject
    {
        public FeaturePlanViewModel()
        {
            AvailableSkills = new ReactiveList<Skill>();
        }

        private string _name;

        public string FeatureName
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { this.RaiseAndSetIfChanged(ref _description, value); }
        }

        // TODO: :bug: Cannot be deselected. Add Null Item?
        public ReactiveList<Skill> AvailableSkills { get; private set; }
        
        private Skill _skill;

        public Skill Skill
        {
            get { return _skill; }
            set { this.RaiseAndSetIfChanged(ref _skill, value); }
        }

        private bool _selected;

        public bool IsSelected
        {
            get { return _selected; }
            set { this.RaiseAndSetIfChanged(ref _selected, value); }
        }
    }
}
