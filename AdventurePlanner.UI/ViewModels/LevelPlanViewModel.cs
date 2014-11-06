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

            NewSaveProficiencies = new ReactiveList<SaveProficiencyViewModel>() { ChangeTrackingEnabled = true };
            Monitor(NewSaveProficiencies);

            NewSkillProficiencies = new ReactiveList<SkillProficiencyViewModel>() { ChangeTrackingEnabled = true };
            Monitor(NewSkillProficiencies);

            NewFeatures = new ReactiveList<FeaturePlanViewModel>();
            Monitor(NewFeatures);

            // Connect commands
            AddAbilityScoreImprovement = ReactiveCommand.CreateAsyncObservable(_ => AddAbilityScoreImprovementImpl());
            RemoveSelectedAbilityScoreImprovements =
                ReactiveCommand.CreateAsyncObservable(_ => RemoveSelectedAbilityScoreImprovementsImpl());

            AddSkillProficiency = ReactiveCommand.CreateAsyncObservable(_ => AddSkillProficiencyImpl());
            RemoveSelectedSkillProficiencies = ReactiveCommand.CreateAsyncObservable(_ => RemoveSelectedSkillProficienciesImpl());
            
            AddSaveProficiency = ReactiveCommand.CreateAsyncObservable(_ => AddSaveProficiencyImpl());
            RemoveSelectedSaveProficiencies = ReactiveCommand.CreateAsyncObservable(_ => RemoveSelectedSaveProficienciesImpl());

            AddFeature = ReactiveCommand.CreateAsyncObservable(_ => AddFeatureImpl());
            RemoveSelectedFeatures = ReactiveCommand.CreateAsyncObservable(_ => RemoveSelectedAddFeatureImpl());
        }

        public ReactiveCommand<AbilityScoreImprovementViewModel> AddAbilityScoreImprovement { get; private set; }

        public ReactiveCommand<IList<AbilityScoreImprovementViewModel>> RemoveSelectedAbilityScoreImprovements
        {
            get;
            private set;
        }

        public ReactiveCommand<SaveProficiencyViewModel> AddSaveProficiency { get; private set; }

        public ReactiveCommand<IList<SaveProficiencyViewModel>> RemoveSelectedSaveProficiencies { get; private set; }

        public ReactiveCommand<SkillProficiencyViewModel> AddSkillProficiency { get; private set; }

        public ReactiveCommand<IList<SkillProficiencyViewModel>> RemoveSelectedSkillProficiencies { get; private set; }

        public ReactiveCommand<FeaturePlanViewModel> AddFeature { get; private set; }

        public ReactiveCommand<IList<FeaturePlanViewModel>> RemoveSelectedFeatures { get; private set; }

        #region Data Properties

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

        public ReactiveList<AbilityScoreImprovementViewModel> AbilityScoreImprovements { get; private set; }

        public ReactiveList<SaveProficiencyViewModel> NewSaveProficiencies { get; private set; }

        public ReactiveList<SkillProficiencyViewModel> NewSkillProficiencies { get; private set; }

        public ReactiveList<FeaturePlanViewModel> NewFeatures { get; private set; }

        #endregion

        #region Command Implementations
        
        private IObservable<AbilityScoreImprovementViewModel> AddAbilityScoreImprovementImpl()
        {
            var asiVm = new AbilityScoreImprovementViewModel();
            asiVm.AvailableOptions.AddRange(Ability.All);

            AbilityScoreImprovements.Add(asiVm);

            return Observable.Return(asiVm);
        }

        private IObservable<IList<AbilityScoreImprovementViewModel>> RemoveSelectedAbilityScoreImprovementsImpl()
        {
            var selected = AbilityScoreImprovements.Where(asi => asi.IsSelected).ToList();

            foreach (var asi in selected)
            {
                AbilityScoreImprovements.Remove(asi);
            }

            return Observable.Return(selected);
        }

        private IObservable<SaveProficiencyViewModel> AddSaveProficiencyImpl()
        {
            var saveProfVm = new SaveProficiencyViewModel();

            saveProfVm.AvailableOptions.AddRange(Ability.All);

            NewSaveProficiencies.Add(saveProfVm);

            return Observable.Return(saveProfVm);
        }

        private IObservable<IList<SaveProficiencyViewModel>> RemoveSelectedSaveProficienciesImpl()
        {
            var selected = NewSaveProficiencies.Where(s => s.IsSelected).ToList();

            foreach (var sprof in selected)
            {
                NewSaveProficiencies.Remove(sprof);
            }

            return Observable.Return(selected);
        }

        private IObservable<SkillProficiencyViewModel> AddSkillProficiencyImpl()
        {
            var skillProf = new SkillProficiencyViewModel();

            skillProf.AvailableOptions.AddRange(Skill.All);

            NewSkillProficiencies.Add(skillProf);

            return Observable.Return(skillProf);
        }

        private IObservable<IList<SkillProficiencyViewModel>> RemoveSelectedSkillProficienciesImpl()
        {
            var selected = NewSkillProficiencies.Where(s => s.IsSelected).ToList();

            foreach (var sprof in selected)
            {
                NewSkillProficiencies.Remove(sprof);
            }

            return Observable.Return(selected);
        }

        private IObservable<FeaturePlanViewModel> AddFeatureImpl()
        {
            var featureVm = new FeaturePlanViewModel();
            featureVm.AvailableSkills.AddRange(Skill.All);

            NewFeatures.Add(featureVm);

            return Observable.Return(featureVm);
        }

        private IObservable<IList<FeaturePlanViewModel>> RemoveSelectedAddFeatureImpl()
        {
            var selected = NewFeatures.Where(f => f.IsSelected).ToList();

            foreach (var feature in selected)
            {
                NewFeatures.Remove(feature);
            }

            return Observable.Return(selected);
        }
        #endregion
    }
}
