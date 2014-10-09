using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class CharacterLevelPlanViewModel : ReactiveObject
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

        public CharacterLevelPlanViewModel()
        {
            var abilityScoreChanged = Changed.Where(e => AbilityScoreProperties.Contains(e.PropertyName));
            
            abilityScoreChanged.Select(_ => (IncreaseStr +
                                             IncreaseDex +
                                             IncreaseCon +
                                             IncreaseInt +
                                             IncreaseWis +
                                             IncreaseCha) > 0)
                .ToProperty(this, x => x.HasAbilityScoreIncreases, out _hasAbilityScoreIncreases);
        }

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

        private readonly ObservableAsPropertyHelper<bool> _hasAbilityScoreIncreases;

        public bool HasAbilityScoreIncreases
        {
            get { return _hasAbilityScoreIncreases.Value; }
        }

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

    }
}
