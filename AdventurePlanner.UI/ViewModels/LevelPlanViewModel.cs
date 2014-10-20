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
        private static string[] AbilityScoreProperties = new[]
        {
            "IncreaseStr",
            "IncreaseDex",
            "IncreaseCon",
            "IncreaseInt", 
            "IncreaseWis", 
            "IncreaseCha"
        };

        public LevelPlanViewModel()
        {
            NewSkillProficiencies = new ReactiveList<SkillProficiencyViewModel>() { ChangeTrackingEnabled = true };

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

        #region Ability Score increases

        private int _increaseStr;

        public int IncreaseStr
        {
            get { return _increaseStr; }
            set { this.RaiseAndSetIfChanged(ref _increaseStr, value); }
        }

        private int _increaseDex;

        public int IncreaseDex
        {
            get { return _increaseDex; }
            set { this.RaiseAndSetIfChanged(ref _increaseDex, value); }
        }

        private int _increaseCon;

        public int IncreaseCon
        {
            get { return _increaseCon; }
            set { this.RaiseAndSetIfChanged(ref _increaseCon, value); }
        }

        private int _increaseInt;

        public int IncreaseInt
        {
            get { return _increaseInt; }
            set { this.RaiseAndSetIfChanged(ref _increaseInt, value); }
        }

        private int _increaseWis;

        public int IncreaseWis
        {
            get { return _increaseWis; }
            set { this.RaiseAndSetIfChanged(ref _increaseWis, value); }
        }

        private int _increaseCha;

        public int IncreaseCha
        {
            get { return _increaseCha; }
            set { this.RaiseAndSetIfChanged(ref _increaseCha, value); }
        }

        #endregion

        private int _setProficiencyBonus;

        public int SetProficiencyBonus
        {
            get { return _setProficiencyBonus; }
            set { this.RaiseAndSetIfChanged(ref _setProficiencyBonus, value); }
        }

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
