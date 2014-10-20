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

            // TODO: Bug: marking dirty on modify level plans means we are not marked clean on load. Seems related to LevelPlan VMs being Dirtifiable. Need MarkRelatedClean()?
            var levelPlansModified = Observable.Merge<object>(
                LevelPlans.ItemsAdded,
                LevelPlans.ItemsRemoved,
                LevelPlans.ItemChanged);
            levelPlansModified.Subscribe(_ => MarkDirty());

            var saveLoad = Observable.Merge(Load, Save);
            saveLoad.Subscribe(_ => MarkClean());
            
            DataChanged.Select(_ => GetMarkdownString()).ToProperty(this, x => x.SnapshotAsMarkdown, out _snapShotAsMarkdown);
            
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

        // TODO: Bug: SnapshotLevel is loading as whatever is in the file for the slider, but always as 1 in the markdown.
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

        public ReactiveList<LevelPlanViewModel> LevelPlans { get; private set; }

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
                    IncreaseStr = 10,
                    IncreaseDex = 10,
                    IncreaseCon = 10,
                    IncreaseInt = 10,
                    IncreaseWis = 10,
                    IncreaseCha = 10,
                    SetProficiencyBonus = 2
                };
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

                    IncreaseStr = view.IncreaseStr,
                    IncreaseDex = view.IncreaseDex,
                    IncreaseCon = view.IncreaseCon,
                    IncreaseInt = view.IncreaseInt,
                    IncreaseWis = view.IncreaseWis,
                    IncreaseCha = view.IncreaseCha,

                    SetProficiencyBonus = view.SetProficiencyBonus,

                    NewSkillProficiencies = view.NewSkillProficiencies.Where(s => s.Value != null).Select(s => s.Value.SkillName).ToArray()
                }).ToList()
            };

            return plan;
        }

        private void SetFromPlan(CharacterPlan plan)
        {
            SnapshotLevel = plan.SnapshotLevel;

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

                    IncreaseStr = lp.IncreaseStr,
                    IncreaseDex = lp.IncreaseDex,
                    IncreaseCon = lp.IncreaseCon,
                    IncreaseInt = lp.IncreaseInt,
                    IncreaseWis = lp.IncreaseWis,
                    IncreaseCha = lp.IncreaseCha,

                    SetProficiencyBonus = lp.SetProficiencyBonus
                };

                foreach (var skillProf in lp.NewSkillProficiencies)
                {
                    var skillProfVm = new SkillProficiencyViewModel();
                    skillProfVm.AvailableOptions.AddRange(Skill.All);
                    skillProfVm.Value = skillProfVm.AvailableOptions.First(s => s.SkillName == skillProf);
                    levelPlanVm.NewSkillProficiencies.Add(skillProfVm);
                }

                LevelPlans.Add(levelPlanVm);
            }
        }

        #endregion
        
        private string GetMarkdownString()
        {
            var snapshot = GetPlan().ToSnapshot(SnapshotLevel);

            return snapshot.ToMarkdownCharacterSheet().ToMarkdown();
        }
    }
}
