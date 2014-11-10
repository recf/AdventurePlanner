using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using AdventurePlanner.Core.Meta;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class ClassPlanViewModel : DirtifiableObject
    {
        public ClassPlanViewModel()
        {
            SaveProficiencies = new ReactiveList<SaveProficiencyViewModel>() { ChangeTrackingEnabled = true };
            Monitor(SaveProficiencies);

            foreach (var ability in Ability.All)
            {
                var saveProfVm = new SaveProficiencyViewModel() { Ability = ability };
                SaveProficiencies.Add(saveProfVm);
            }

            SkillProficiencies = new ReactiveList<SkillProficiencyViewModel>() { ChangeTrackingEnabled = true };
            Monitor(SkillProficiencies);

            foreach (var skill in Skill.All)
            {
                var skillProfVm = new SkillProficiencyViewModel() { Skill = skill };
                SkillProficiencies.Add(skillProfVm);
            }
        }
        
        #region Data Properties

        private readonly ObservableAsPropertyHelper<string> _header;
        
        private string _className;

        public string ClassName
        {
            get { return _className; }
            set { this.RaiseAndSetIfChanged(ref _className, value); }
        }

        private string _armorProficiencies = string.Empty;

        public string ArmorProficiencies
        {
            get { return _armorProficiencies; }
            set { this.RaiseAndSetIfChanged(ref _armorProficiencies, value); }
        }

        private string _weaponProficiencies = string.Empty;

        public string WeaponProficiencies
        {
            get { return _weaponProficiencies; }
            set { this.RaiseAndSetIfChanged(ref _weaponProficiencies, value); }
        }

        private string _toolProficiencies = string.Empty;

        public string ToolProficiencies
        {
            get { return _toolProficiencies; }
            set { this.RaiseAndSetIfChanged(ref _toolProficiencies, value); }
        }
        
        public ReactiveList<SaveProficiencyViewModel> SaveProficiencies { get; private set; }

        public ReactiveList<SkillProficiencyViewModel> SkillProficiencies { get; private set; }

        #endregion
    }
}
