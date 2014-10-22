using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Meta;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class LevelPlanViewModel : DirtifiableObject
    {
        public LevelPlanViewModel()
        {
            AbilityScoreImprovements = new ReactiveList<AbilityScoreImprovementViewModel>()
            {
                ChangeTrackingEnabled = true
            };
            Monitor(AbilityScoreImprovements);

            NewSkillProficiencies = new ReactiveList<SkillProficiencyViewModel>() { ChangeTrackingEnabled = true };
            Monitor(NewSkillProficiencies);
            
            AddSkillProficiency = ReactiveCommand.CreateAsyncObservable(_ => AddSkillProficiencyImpl());
        }
        
        public ReactiveCommand<SkillProficiencyViewModel> AddSkillProficiency { get; private set; }
        
        public string Header
        {
            get
            {
                return (Level == 1) ? "Level 1" : Level.ToString();
            }
        }

        private int _level;

        public int Level
        {
            get { return _level; }
            set { this.RaiseAndSetIfChanged(ref _level, value); }
        }

        private string _className;

        public string ClassName
        {
            get { return _className; }
            set { this.RaiseAndSetIfChanged(ref _className, value); }
        }

        private int _setProficiencyBonus;

        public int SetProficiencyBonus
        {
            get { return _setProficiencyBonus; }
            set { this.RaiseAndSetIfChanged(ref _setProficiencyBonus, value); }
        }

        public ReactiveList<AbilityScoreImprovementViewModel> AbilityScoreImprovements { get; private set; }

        public ReactiveList<SkillProficiencyViewModel> NewSkillProficiencies { get; private set; }
        
        private IObservable<SkillProficiencyViewModel> AddSkillProficiencyImpl()
        {
            var skillProf = new SkillProficiencyViewModel();
            
            skillProf.AvailableOptions.AddRange(Skill.All);

            NewSkillProficiencies.Add(skillProf);

            return Observable.Return(skillProf);
        }
    }
}
