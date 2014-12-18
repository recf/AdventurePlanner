using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using AdventurePlanner.Core;
using AdventurePlanner.Core.Domain;
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
            
            AddArmor = ReactiveCommand.CreateAsyncObservable(_ => AddArmorImpl());
            RemoveArmor = ReactiveCommand.CreateAsyncObservable(vm => RemoveArmorImpl((ArmorViewModel)vm));

            AddWeapon = ReactiveCommand.CreateAsyncObservable(_ => AddWeaponImpl());
            RemoveWeapon = ReactiveCommand.CreateAsyncObservable(vm => RemoveWeaponImpl((WeaponViewModel)vm));

            ClassPlan = new ClassPlanViewModel();
            Monitor(ClassPlan);

            LevelPlans = new ReactiveList<LevelPlanViewModel>();
            Monitor(LevelPlans);

            Armor = new ReactiveList<ArmorViewModel>();
            Monitor(Armor);

            Weapons = new ReactiveList<WeaponViewModel>();
            Monitor(Weapons);

            var saveLoad = Observable.Merge(Load, Save);
            saveLoad.Subscribe(_ => MarkClean());

            Dirtied.Select(_ => GetMarkdownString()).ToProperty(this, x => x.SnapshotAsText, out _snapShotAsText);

            SetFromPlan(new CharacterPlan
            {
                ClassPlan = new ClassPlan()
            });
            MarkClean();
        }

        public ReactiveCommand<Unit> Load { get; private set; }

        public ReactiveCommand<Unit> Save { get; private set; }

        public ReactiveCommand<Unit> SaveSnapshot { get; private set; }

        public ReactiveCommand<LevelPlanViewModel> AddLevel { get; private set; }

        public ReactiveCommand<ArmorViewModel> AddArmor { get; set; }

        public ReactiveCommand<ArmorViewModel> RemoveArmor { get; set; }

        public ReactiveCommand<WeaponViewModel> AddWeapon { get; set; }

        public ReactiveCommand<WeaponViewModel> RemoveWeapon { get; set; }

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

        public ClassPlanViewModel ClassPlan { get; private set; }

        public IReactiveList<LevelPlanViewModel> LevelPlans { get; private set; }

        public IReactiveList<ArmorViewModel> Armor { get; private set; }

        public IReactiveList<WeaponViewModel> Weapons { get; private set; }

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

        // TODO: AddArmorImpl, etc are small enough that they would probably better inline.
        private IObservable<ArmorViewModel> AddArmorImpl()
        {
            var armorVm = new ArmorViewModel();
            Armor.Add(armorVm);

            return Observable.Return(armorVm);
        }

        private IObservable<ArmorViewModel> RemoveArmorImpl(ArmorViewModel armorVm)
        {
            Armor.Remove(armorVm);

            return Observable.Return(armorVm);
        }

        private IObservable<WeaponViewModel> AddWeaponImpl()
        {
            var weaponVm = new WeaponViewModel();
            Weapons.Add(weaponVm);

            return Observable.Return(weaponVm);
        }

        private IObservable<WeaponViewModel> RemoveWeaponImpl(WeaponViewModel weaponVm)
        {
            Weapons.Remove(weaponVm);

            return Observable.Return(weaponVm);
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

                ClassPlan = new ClassPlan
                {
                    ClassName = ClassPlan.ClassName,

                    ArmorProficiencies = ClassPlan.ArmorProficiencies.Split(',').Select(s => s.Trim()).ToArray(),
                    WeaponProficiencies = ClassPlan.WeaponProficiencies.Split(',').Select(s => s.Trim()).ToArray(),
                    ToolProficiencies = ClassPlan.ToolProficiencies.Split(',').Select(s => s.Trim()).ToArray(),

                    SaveProficiencies = ClassPlan.SaveProficiencies
                        .Where(s => s.IsProficient)
                        .Select(s => s.Ability.Abbreviation).ToArray(),

                    SkillProficiencies = ClassPlan.SkillProficiencies
                        .Where(s => s.IsProficient)
                        .Select(s => s.Skill.SkillName).ToArray(),
                },
            };

            plan.LevelPlans = LevelPlans.Select(view => new LevelPlan
            {
                Level = view.Level,

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

            plan.Armor = Armor.Select(view => new Armor
            {
                Name = view.ArmorName,
                ArmorClass = view.ArmorClass,
                ProficiencyGroup = view.ProficiencyGroup,
                MaximumDexterityModifier = view.MaximumDexterityModifier
            }).ToList();

            plan.Weapons = Weapons.Select(view => new Weapon
            {
                Name = view.Name,
                ProficiencyGroup = view.ProficiencyGroup,
                DamageDice = view.DamageDice,
                DamageType = view.DamageType,

                NormalRange = view.NormalRange,
                MaximumRange = view.MaximumRange,

                HasAmmunition = view.HasAmmunition,
                IsLight = view.IsLight
            }).ToList();

            return plan;
        }

        private void SetFromPlan(CharacterPlan plan)
        {
            EnsureTwentyLevels(plan);

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

            ClassPlan.ClassName = plan.ClassPlan.ClassName;

            ClassPlan.ArmorProficiencies = string.Join(", ", plan.ClassPlan.ArmorProficiencies ?? new string[0]);
            ClassPlan.WeaponProficiencies = string.Join(", ", plan.ClassPlan.WeaponProficiencies ?? new string[0]);
            ClassPlan.ToolProficiencies = string.Join(", ", plan.ClassPlan.ToolProficiencies ?? new string[0]);

            var saveProfs = plan.ClassPlan.SaveProficiencies ?? new string[0];
            foreach (var saveProfVm in ClassPlan.SaveProficiencies)
            {
                saveProfVm.IsProficient = saveProfs.Contains(saveProfVm.Ability.Abbreviation);
            }

            var skillProfs = plan.ClassPlan.SkillProficiencies ?? new string[0]; 
            foreach (var skillProfVm in ClassPlan.SkillProficiencies)
            {
                skillProfVm.IsProficient = skillProfs.Contains(skillProfVm.Skill.SkillName);
            }

            LevelPlans.Clear();
            foreach (var lp in plan.LevelPlans)
            {
                var levelPlanVm = new LevelPlanViewModel
                {
                    Level = lp.Level,

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

                foreach (var feature in lp.FeaturePlans ?? new FeaturePlan[0])
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

            Armor.Clear();
            foreach (var armor in plan.Armor ?? new Armor[0])
            {
                Armor.Add(new ArmorViewModel()
                {
                    ArmorName = armor.Name,
                    ArmorClass = armor.ArmorClass,
                    ProficiencyGroup = armor.ProficiencyGroup,
                    MaximumDexterityModifier = armor.MaximumDexterityModifier
                });
            }

            Weapons.Clear();
            foreach (var weapon in plan.Weapons ?? new Weapon[0])
            {
                Weapons.Add(new WeaponViewModel
                {
                    Name = weapon.Name,
                    ProficiencyGroup = weapon.ProficiencyGroup,
                    DamageDice = weapon.DamageDice,
                    DamageType = weapon.DamageType,

                    NormalRange = weapon.NormalRange,
                    MaximumRange = weapon.MaximumRange,

                    HasAmmunition = weapon.HasAmmunition,
                    IsLight = weapon.IsLight
                });
            }

            // Set snapshot level last so that the SnapshotMarkdown property is 
            // set correctly. Otherwise it will try to render at a level that 
            // isn't in the list yet.
            SnapshotLevel = plan.SnapshotLevel;
        }

        private void EnsureTwentyLevels(CharacterPlan plan)
        {
            if (plan.LevelPlans == null)
            {
                plan.LevelPlans = new List<LevelPlan>();
            }

            for (var i = 1; i <= 20; i++)
            {
                if (plan.LevelPlans.Any(lp => lp.Level == i))
                {
                    continue;
                }

                plan.LevelPlans.Add(new LevelPlan { Level = i });
            }
        }

        #endregion

        private string GetMarkdownString()
        {
            var snapshot = GetPlan().ToSnapshot(SnapshotLevel);

            return snapshot.ToText();
        }
    }
}
