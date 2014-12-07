using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyhedral;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class WeaponViewModel : DirtifiableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        private string _proficiencyGroup;

        public string ProficiencyGroup
        {
            get { return _proficiencyGroup; }
            set { this.RaiseAndSetIfChanged(ref _proficiencyGroup, value); }
        }

        private bool _hasAmmunition;

        public bool HasAmmunition
        {
            get { return _hasAmmunition; }
            set { this.RaiseAndSetIfChanged(ref _hasAmmunition, value); }
        }

        private bool _isLight;

        public bool IsLight
        {
            get { return _isLight; }
            set { this.RaiseAndSetIfChanged(ref _isLight, value); }
        }

        private DiceRoll _damageDice;

        public DiceRoll DamageDice
        {
            get { return _damageDice; }
            set { this.RaiseAndSetIfChanged(ref _damageDice, value); }
        }

        private string _damageType;

        public string DamageType
        {
            get { return _damageType; }
            set { this.RaiseAndSetIfChanged(ref _damageType, value); }
        }

        private int? _normalRange;

        public int? NormalRange
        {
            get { return _normalRange; }
            set { this.RaiseAndSetIfChanged(ref _normalRange, value); }
        }

        private int? _maximumRange;

        public int? MaximumRange
        {
            get { return _maximumRange; }
            set { this.RaiseAndSetIfChanged(ref _maximumRange, value); }
        }
    }
}
