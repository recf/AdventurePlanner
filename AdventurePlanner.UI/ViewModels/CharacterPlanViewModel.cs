using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using AdventurePlanner.Core;
using AdventurePlanner.Core.Meta;
using AdventurePlanner.Core.Planning;
using Microsoft.Win32;
using Newtonsoft.Json;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class CharacterPlanViewModel : DirtifiableObject
    {
        public const string FileDialogFilter = "Adventure Planner Character files (*.apc,*.apchar)|*.apc;*.apchar";

        public CharacterPlanViewModel()
            : base("BackingFile", "SnapshotAsText")
        {
            var canSave = this.ObservableForProperty(x => x.IsDirty).Select(y => y.Value);

            Load = ReactiveCommand.CreateAsyncObservable(_ => LoadImpl());
            Save = ReactiveCommand.CreateAsyncObservable(canSave, _ => SaveImpl());

            SaveSnapshot = ReactiveCommand.CreateAsyncObservable(_ => SaveSnapshotImpl());

            AddClass = ReactiveCommand.CreateAsyncObservable(_ => AddClassImpl());
            AddLevel = ReactiveCommand.CreateAsyncObservable(_ => AddLevelImpl());

            AddArmor = ReactiveCommand.CreateAsyncObservable(_ => AddArmorPlanImpl());
            RemoveArmor = ReactiveCommand.CreateAsyncObservable(armorVm => RemoveArmorImpl((ArmorPlanViewModel)armorVm));

            ClassPlans = new ReactiveList<ClassPlanViewModel>();
            Monitor(ClassPlans);

            LevelPlans = new ReactiveList<LevelPlanViewModel>();
            Monitor(LevelPlans);

            ArmorPlans = new ReactiveList<ArmorPlanViewModel>();
            Monitor(ArmorPlans);

            var saveLoad = Observable.Merge(Load, Save);
            saveLoad.Subscribe(_ => MarkClean());

            Dirtied.Select(_ => GetMarkdownString()).ToProperty(this, x => x.SnapshotAsText, out _snapShotAsText);

            // TODO: Consider: Instead of calling AddLevel on create, instead call SetFromPlan() with initial state.
            AddClass.Execute(null);
            AddLevel.Execute(null);
            MarkClean();
        }

        public ReactiveCommand<Unit> Load { get; private set; }

        public ReactiveCommand<Unit> Save { get; private set; }

        public ReactiveCommand<Unit> SaveSnapshot { get; private set; }

        public ReactiveCommand<ClassPlanViewModel> AddClass { get; private set; }

        public ReactiveCommand<LevelPlanViewModel> AddLevel { get; private set; }

        public ReactiveCommand<ArmorPlanViewModel> AddArmor { get; set; }

        public ReactiveCommand<ArmorPlanViewModel> RemoveArmor { get; set; }

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

        private readonly ObservableAsPropertyHelper<string> _snapShotAsText;

        public string SnapshotAsText
        {
            get { return _snapShotAsText.Value; }
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

        public IReactiveList<ClassPlanViewModel> ClassPlans { get; private set; }

        public IReactiveList<LevelPlanViewModel> LevelPlans { get; private set; }

        public IReactiveList<ArmorPlanViewModel> ArmorPlans { get; private set; } 

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

        private IObservable<Unit> SaveSnapshotImpl()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "AsciiDoc files (*.adoc, *.txt)|*.adoc;*.txt;"
            };

            var result = dialog.ShowDialog();

            if (result != true)
            {
                return Observable.Never(Unit.Default);
            }

            var fileName = dialog.FileName;

            File.WriteAllText(fileName, SnapshotAsText);

            return Observable.Return(Unit.Default);
        }

        private IObservable<ClassPlanViewModel> AddClassImpl()
        {
            var classPlanVm = new ClassPlanViewModel();

            ClassPlans.Add(classPlanVm);

            return Observable.Return(classPlanVm);
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
                    ClassPlan = ClassPlans.First(),
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

                    ClassPlan = maxLevel.ClassPlan,

                    SetProficiencyBonus = maxLevel.SetProficiencyBonus
                };
            }

            nextLevel.AvailableClassPlans = ClassPlans;

            LevelPlans.Add(nextLevel);

            SnapshotLevel = nextLevel.Level;

            return Observable.Return(nextLevel);
        }
        
        private IObservable<ArmorPlanViewModel> AddArmorPlanImpl()
        {
            var armorVm = new ArmorPlanViewModel();
            ArmorPlans.Add(armorVm);

            return Observable.Return(armorVm);
        }

        private IObservable<ArmorPlanViewModel> RemoveArmorImpl(ArmorPlanViewModel armorVm)
        {
            ArmorPlans.Remove(armorVm);

            return Observable.Return(armorVm);
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

                ClassPlans = ClassPlans.Select(view => new ClassPlan
                {
                    ClassName = view.ClassName,

                    ArmorProficiencies = view.ArmorProficiencies.Split(',').Select(s => s.Trim()).ToArray(),
                    WeaponProficiencies = view.WeaponProficiencies.Split(',').Select(s => s.Trim()).ToArray(),
                    ToolProficiencies = view.ToolProficiencies.Split(',').Select(s => s.Trim()).ToArray(),

                    SaveProficiencies = view.SaveProficiencies
                        .Where(s => s.IsProficient)
                        .Select(s => s.Ability.Abbreviation).ToArray(),

                    SkillProficiencies = view.SkillProficiencies
                        .Where(s => s.IsProficient)
                        .Select(s => s.Skill.SkillName).ToArray(),
                }).ToList(),
            };

            plan.LevelPlans = LevelPlans.Select(view => new LevelPlan
            {
                Level = view.Level,

                ClassPlan = plan.ClassPlans.FirstOrDefault(cp => cp.ClassName == view.ClassPlan.ClassName),

                AbilityScoreImprovements = view.AbilityScoreImprovements
                    .Where(asi => asi.Ability != null)
                    .ToDictionary(asi => asi.Ability.Abbreviation, asi => asi.Improvement),

                SetProficiencyBonus = view.SetProficiencyBonus,

                FeaturePlans = view.NewFeatures
                    .Where(f => !string.IsNullOrWhiteSpace(f.FeatureName))
                    .Select(f => new FeaturePlan
                    {
                        Name = f.FeatureName,
                        Description = f.Description,
                        SkillName = f.Skill == null ? null : f.Skill.SkillName
                    }).ToArray(),
            }).ToList();

            plan.ArmorPlans = ArmorPlans.Select(view => new ArmorPlan
            {
                ArmorName = view.ArmorName,
                ArmorClass = view.ArmorClass,
                ProficiencyGroup = view.ProficiencyGroup,
                MaximumDexterityModifier = view.MaximumDexterityModifier
            }).ToList();

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

            ClassPlans.Clear();
            foreach (var lp in plan.ClassPlans)
            {
                var classPlanVm = new ClassPlanViewModel
                {
                    ClassName = lp.ClassName,

                    ArmorProficiencies = string.Join(", ", lp.ArmorProficiencies ?? new string[0]),
                    WeaponProficiencies = string.Join(", ", lp.WeaponProficiencies ?? new string[0]),
                    ToolProficiencies = string.Join(", ", lp.ToolProficiencies ?? new string[0]),
                };

                foreach (var abilityAbbr in lp.SaveProficiencies ?? new string[0])
                {
                    var saveProfVm = classPlanVm.SaveProficiencies.First(spvm => spvm.Ability.Abbreviation == abilityAbbr);
                    saveProfVm.IsProficient = true;
                }

                foreach (var skillProf in lp.SkillProficiencies)
                {
                    var skillProfVm = classPlanVm.SkillProficiencies.First(spvm => spvm.Skill.SkillName == skillProf);
                    skillProfVm.IsProficient = true;
                }

                ClassPlans.Add(classPlanVm);
            }

            LevelPlans.Clear();
            foreach (var lp in plan.LevelPlans)
            {
                var levelPlanVm = new LevelPlanViewModel
                {
                    Level = lp.Level,

                    ClassPlan = ClassPlans.FirstOrDefault(cp => cp.ClassName == lp.ClassPlan.ClassName),

                    SetProficiencyBonus = lp.SetProficiencyBonus,
                };

                foreach (var kvp in lp.AbilityScoreImprovements ?? new Dictionary<string, int>())
                {
                    var asiVm = new AbilityScoreImprovementViewModel();
                    asiVm.AvailableOptions.AddRange(Ability.All);
                    asiVm.Ability = asiVm.AvailableOptions.First(a => a.Abbreviation == kvp.Key);
                    asiVm.Improvement = kvp.Value;

                    levelPlanVm.AbilityScoreImprovements.Add(asiVm);
                }

                foreach (var feature in lp.FeaturePlans)
                {
                    var featureVm = new FeaturePlanViewModel()
                    {
                        FeatureName = feature.Name,
                        Description = feature.Description,
                    };

                    featureVm.AvailableSkills.AddRange(Skill.All);

                    if (!string.IsNullOrWhiteSpace(feature.SkillName))
                    {
                        featureVm.Skill = featureVm.AvailableSkills.First(s => s.SkillName == feature.SkillName);
                    }

                    levelPlanVm.NewFeatures.Add(featureVm);
                }

                LevelPlans.Add(levelPlanVm);
            }

            ArmorPlans.Clear();
            foreach (var armor in plan.ArmorPlans)
            {
                ArmorPlans.Add(new ArmorPlanViewModel()
                {
                    ArmorName = armor.ArmorName,
                    ArmorClass = armor.ArmorClass,
                    ProficiencyGroup = armor.ProficiencyGroup,
                    MaximumDexterityModifier = armor.MaximumDexterityModifier
                });
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

            return snapshot.ToText();
        }
    }
}
