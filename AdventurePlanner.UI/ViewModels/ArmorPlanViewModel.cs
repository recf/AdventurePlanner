using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class ArmorPlanViewModel : DirtifiableObject
    {
        private int _armorClass  = 0;

        public int ArmorClass
        {
            get { return _armorClass; }
            set { this.RaiseAndSetIfChanged(ref _armorClass, value); }
        }

        private int? _maximumDexterityModifier;

        public int? MaximumDexterityModifier
        {
            get { return _maximumDexterityModifier; }
            set { this.RaiseAndSetIfChanged(ref _maximumDexterityModifier, value); }
        }

        private string _armorName;

        public string ArmorName
        {
            get { return _armorName; }
            set { this.RaiseAndSetIfChanged(ref _armorName, value); }
        }
        
        private string _proficiencyGroup;

        public string ProficiencyGroup
        {
            get { return _proficiencyGroup; }
            set { this.RaiseAndSetIfChanged(ref _proficiencyGroup, value); }
        }
    }
}
