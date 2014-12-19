using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Domain;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class FeaturePlanViewModel : DirtifiableObject
    {
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

        private bool _selected;

        public bool IsSelected
        {
            get { return _selected; }
            set { this.RaiseAndSetIfChanged(ref _selected, value); }
        }
    }
}
