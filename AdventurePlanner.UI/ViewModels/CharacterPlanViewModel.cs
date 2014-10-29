using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using AdventurePlanner.Core.Meta;
using AdventurePlanner.Core.Planning;
using MarkdownLog;
using Microsoft.Win32;
using Newtonsoft.Json;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class CharacterPlanViewModel : DirtifiableObject
    {
        public const string FileDialogFilter = "Adventure Planner Character files (*.apc)|*.apchar";

        public CharacterPlanViewModel()
            : base("BackingFile", "SnapshotAsMarkdown")
        {
            var canSave = this.ObservableForProperty(x => x.IsDirty).Select(y => y.Value);

            Load = ReactiveCommand.CreateAsyncObservable(_ => LoadImpl());
            Save = ReactiveCommand.CreateAsyncObservable(canSave, _ => SaveImpl());
            AddLevel = ReactiveCommand.CreateAsyncObservable(_ => AddLevelImpl());

            LevelPlans = new ReactiveList<LevelPlanViewModel> { ChangeTrackingEnabled = true };
            Monitor(LevelPlans);

            var saveLoad = Observable.Merge(Load, Save);
            saveLoad.Subscribe(_ => MarkClean());

            Dirtied.Select(_ => GetMarkdownString()).ToProperty(this, x => x.SnapshotAsMarkdown, out _snapShotAsMarkdown);

            // TODO: Consider: Instead of calling AddLevel on create, instead call SetFromPlan() with initial state.
            AddLevel.Execute(null);
            MarkClean();
        }

        public ReactiveCommand<Unit> Load { get; private set; }

        public ReactiveCommand<Unit> Save { get; private set; }

        public ReactiveCommand<LevelPlanViewModel> AddLevel { get; private set; }

        #region Bookkeeping Properties

        private string _backingFile;

        public string BackingFile
        {
            get { return _backingFile; }
            private set { this.RaiseAndSetIfChanged(ref _backingFile, value); }
        }

        #endregion

        #region Level Snapshot properties

        private int _snapshotLevel = 0;

        public int SnapshotLevel
        {
            get { return _snapshotLevel; }
            set { this.RaiseAndSetIfChanged(ref _snapshotLevel, value); }
        }

        private readonly ObservableAsPropertyHelper<string> _snapShotAsMarkdown;

        public string SnapshotAsMarkdown
        {
            get { return _snapShotAsMarkdown.Value; }
        }

        #endregion

        #region Data Properties

        private string _characterName;

        public string CharacterName
        {
            get { return _characterName; }
            set { this.RaiseAndSetIfChanged(ref _characterName, value); }
        }

        private string _race;

        public string Race
        {
            get { return _race; }
            set { this.RaiseAndSetIfChanged(ref _race, value); }
        }

        private int _speed;

        public int Speed
        {
            get { return _speed; }
            set { this.RaiseAndSetIfChanged(ref _speed, value); }
        }

        private int _age;

        public int Age
        {
            get { return _age; }
            set { this.RaiseAndSetIfChanged(ref _age, value); }
        }

        private int _heightFeet;

        public int HeightFeet
        {
            get { return _heightFeet; }
            set { this.RaiseAndSetIfChanged(ref _heightFeet, value); }
        }

        private int _heightInches;

        public int HeightInches
        {
            get { return _heightInches; }
            set { this.RaiseAndSetIfChanged(ref _heightInches, value); }
        }

        private int _weight;

        public int Weight
        {
            get { return _weight; }
            set { this.RaiseAndSetIfChanged(ref _weight, value); }
        }

        private string _eyeColor;

        public string EyeColor
        {
            get { return _eyeColor; }
            set { this.RaiseAndSetIfChanged(ref _eyeColor, value); }
        }

        private string _hairColor;

        public string HairColor
        {
            get { return _hairColor; }
            set { this.RaiseAndSetIfChanged(ref _hairColor, value); }
        }

        private string _skinColor;

        public string SkinColor
        {
            get { return _skinColor; }
            set { this.RaiseAndSetIfChanged(ref _skinColor, value); }
        }

        private string _characterBackground;

        public string CharacterBackground
        {
            get { return _characterBackground; }
            set { this.RaiseAndSetIfChanged(ref _characterBackground, value); }
        }

        public IReactiveList<LevelPlanViewModel> LevelPlans { get; private set; }

        #endregion

        #region Command Implementations

        private IObservable<Unit> LoadImpl()
        {
            var dialog = new OpenFileDialog
            {
                Filter = FileDialogFilter
            };

            var result = dialog.ShowDialog();

            if (result != true)
            {
                return Observable.Never(Unit.Default);
            }

            var contents = File.ReadAllText(dialog.FileName);
            var plan = JsonConvert.DeserializeObject<CharacterPlan>(contents);

            SetFromPlan(plan);

            BackingFile = dialog.FileName;

            return Observable.Return(Unit.Default);
        }

        private IObservable<Unit> SaveImpl()
        {
            if (BackingFile == null)
            {
                var dialog = new SaveFileDialog
                {
                    Filter = FileDialogFilter
                };

                var result = dialog.ShowDialog();

                if (result != true)
                {
                    return Observable.Never(Unit.Default);
                }

                BackingFile = dialog.FileName;
            }

            var plan = GetPlan();

            var contents = JsonConvert.SerializeObject(plan, Formatting.Indented);

            File.WriteAllText(BackingFile, contents);

            return Observable.Return(Unit.Default);
        }

        private IObservable<LevelPlanViewModel> AddLevelImpl()
        {
            var maxLevel = LevelPlans.LastOrDefault();

            LevelPlanViewModel nextLevel;

            if (maxLevel == null)
            {
                nextLevel = new LevelPlanViewModel
                {
                    Level = 1,
                    SetProficiencyBonus = 2
                };

                foreach (var ability in Ability.All)
                {
                    var abilityVm = new AbilityScoreImprovementViewModel { Ability = ability, Improvement = 10 };
                    abilityVm.AvailableOptions.AddRange(Ability.All);

                    nextLevel.AbilityScoreImprovements.Add(abilityVm);
                }
            }
            else
            {
                nextLevel = new LevelPlanViewModel
                {
                    Level = maxLevel.Level + 1,
                    ClassName = maxLevel.ClassName,

                    SetProficiencyBonus = maxLevel.SetProficiencyBonus
                };
            }

            LevelPlans.Add(nextLevel);

            SnapshotLevel = nextLevel.Level;

            return Observable.Return(nextLevel);
        }

        #endregion

        #region Conversion VM <-> M

        private CharacterPlan GetPlan()
        {
            var plan = new CharacterPlan
            {
                SnapshotLevel = SnapshotLevel,

                Name = CharacterName,
                Race = Race,
                Speed = Speed,
                Alignment = "TN",

                Age = Age,
                HeightFeet = HeightFeet,
                HeightInches = HeightInches,
                Weight = Weight,

                EyeColor = EyeColor,
                HairColor = HairColor,
                SkinColor = SkinColor,

                Background = CharacterBackground,

                LevelPlans = LevelPlans.Select(view => new LevelPlan
                {
                    Level = view.Level,
                    ClassName = view.ClassName,

                    AbilityScoreImprovements = view.AbilityScoreImprovements
                        .Where(asi => asi.Ability != null)
                        .ToDictionary(asi => asi.Ability.Abbreviation, asi => asi.Improvement),

                    SetProficiencyBonus = view.SetProficiencyBonus,

                    NewSaveProficiencies = view.NewSaveProficiencies
                        .Where(s => s.Ability != null)
                        .Select(s => s.Ability.Abbreviation).ToArray(),

                    NewSkillProficiencies = view.NewSkillProficiencies
                        .Where(s => s.Value != null)
                        .Select(s => s.Value.SkillName).ToArray(),

                    FeaturePlans = view.NewFeatures
                        .Where(f => !string.IsNullOrWhiteSpace(f.FeatureName))
                        .Select(f => new FeaturePlan
                        {
                            Name = f.FeatureName,
                            Description = f.Description
                        }).ToArray(),
                }).ToList()
            };

            return plan;
        }

        private void SetFromPlan(CharacterPlan plan)
        {
            CharacterName = plan.Name;
            Race = plan.Race;
            Speed = plan.Speed;

            Age = plan.Age;
            HeightFeet = plan.HeightFeet;
            HeightInches = plan.HeightInches;
            Weight = plan.Weight;
            EyeColor = plan.EyeColor;
            HairColor = plan.HairColor;
            SkinColor = plan.SkinColor;

            CharacterBackground = plan.Background;

            LevelPlans.Clear();
            foreach (var lp in plan.LevelPlans)
            {
                var levelPlanVm = new LevelPlanViewModel
                {
                    Level = lp.Level,
                    ClassName = lp.ClassName,

                    SetProficiencyBonus = lp.SetProficiencyBonus
                };

                foreach (var kvp in lp.AbilityScoreImprovements ?? new Dictionary<string, int>())
                {
                    var asiVm = new AbilityScoreImprovementViewModel();
                    asiVm.AvailableOptions.AddRange(Ability.All);
                    asiVm.Ability = asiVm.AvailableOptions.First(a => a.Abbreviation == kvp.Key);
                    asiVm.Improvement = kvp.Value;

                    levelPlanVm.AbilityScoreImprovements.Add(asiVm);
                }

                foreach (var abilityAbbr in lp.NewSaveProficiencies ?? new string[0])
                {
                    var saveProfVm = new SaveProficiencyViewModel();
                    saveProfVm.AvailableOptions.AddRange(Ability.All);
                    saveProfVm.Ability = saveProfVm.AvailableOptions.First(a => a.Abbreviation == abilityAbbr);

                    levelPlanVm.NewSaveProficiencies.Add(saveProfVm);
                }

                foreach (var skillProf in lp.NewSkillProficiencies)
                {
                    var skillProfVm = new SkillProficiencyViewModel();
                    skillProfVm.AvailableOptions.AddRange(Skill.All);
                    skillProfVm.Value = skillProfVm.AvailableOptions.First(s => s.SkillName == skillProf);

                    levelPlanVm.NewSkillProficiencies.Add(skillProfVm);
                }

                foreach (var feature in lp.FeaturePlans)
                {
                    var featureVm = new FeaturePlanViewModel()
                    {
                        FeatureName = feature.Name,
                        Description = feature.Description
                    };
                    
                    levelPlanVm.NewFeatures.Add(featureVm);
                }

                LevelPlans.Add(levelPlanVm);
            }

            // Set snapshot level last so that the SnapshotMarkdown property is 
            // set correctly. Otherwise it will try to render at a level that 
            // isn't in the list yet.
            SnapshotLevel = plan.SnapshotLevel;
        }

        #endregion

        private string GetMarkdownString()
        {
            var snapshot = GetPlan().ToSnapshot(SnapshotLevel);

            return snapshot.ToMarkdownCharacterSheet().ToMarkdown();
        }
    }
}
