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

            NewFeatures = new ReactiveList<FeaturePlanViewModel>();
            Monitor(NewFeatures);

            // Connect commands
            AddAbilityScoreImprovement = ReactiveCommand.CreateAsyncObservable(_ => AddAbilityScoreImprovementImpl());
            RemoveSelectedAbilityScoreImprovements =
                ReactiveCommand.CreateAsyncObservable(_ => RemoveSelectedAbilityScoreImprovementsImpl());
            
            AddFeature = ReactiveCommand.CreateAsyncObservable(_ => AddFeatureImpl());
            RemoveSelectedFeatures = ReactiveCommand.CreateAsyncObservable(_ => RemoveSelectedAddFeatureImpl());
        }

        public ReactiveCommand<AbilityScoreImprovementViewModel> AddAbilityScoreImprovement { get; private set; }

        public ReactiveCommand<IList<AbilityScoreImprovementViewModel>> RemoveSelectedAbilityScoreImprovements
        {
            get;
            private set;
        }

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

        private ClassPlanViewModel _classPlan;

        public ClassPlanViewModel ClassPlan
        {
            get { return _classPlan; }
            set { this.RaiseAndSetIfChanged(ref _classPlan, value); }
        }

        public IReactiveList<ClassPlanViewModel> AvailableClassPlans { get; set; }

        private int _setProficiencyBonus;

        public int SetProficiencyBonus
        {
            get { return _setProficiencyBonus; }
            set { this.RaiseAndSetIfChanged(ref _setProficiencyBonus, value); }
        }

        public ReactiveList<AbilityScoreImprovementViewModel> AbilityScoreImprovements { get; private set; }

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
