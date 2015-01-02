using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Domain;
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
            AddFeature = ReactiveCommand.CreateAsyncObservable(_ => AddFeatureImpl());
            RemoveSelectedFeatures = ReactiveCommand.CreateAsyncObservable(_ => RemoveSelectedAddFeatureImpl());
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

        public ReactiveList<AbilityScoreImprovementViewModel> AbilityScoreImprovements { get; private set; }

        public ReactiveList<FeaturePlanViewModel> NewFeatures { get; private set; }

        #endregion

        #region Command Implementations

        private IObservable<FeaturePlanViewModel> AddFeatureImpl()
        {
            var featureVm = new FeaturePlanViewModel();

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
