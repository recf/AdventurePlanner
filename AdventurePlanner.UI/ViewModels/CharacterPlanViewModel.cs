using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using AdventurePlanner.Core.Planning;
using Microsoft.Win32;
using Newtonsoft.Json;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class CharacterPlanViewModel : ReactiveObject
    {
        public const string FileDialogFilter = "Adventure Planner Character files (*.apc)|*.apchar";

        private static readonly string[] BookkeepingProperties = { "IsDirty", "BackingFile" };
        
        public CharacterPlanViewModel()
        {
            var canSave = this.ObservableForProperty(x => x.IsDirty).Select(y => y.Value);

            Load = ReactiveCommand.CreateAsyncObservable(_ => LoadImpl());
            Save = ReactiveCommand.CreateAsyncObservable(canSave, _ => SaveImpl());

            var dataPropertyChanged = Changed.Where(e => !BookkeepingProperties.Contains(e.PropertyName));

            Observable.Merge(
                Load.Where(loaded => loaded).Select(_ => false),
                Save.Where(saved => saved).Select(_ => false),
                dataPropertyChanged.Select(_ => true)).ToProperty(this, x => x.IsDirty, out _dirty);
        }

        public ReactiveCommand<bool> Load { get; private set; }

        public ReactiveCommand<bool> Save { get; private set; }

        #region Bookkeeping Properties

        private readonly ObservableAsPropertyHelper<bool> _dirty;

        public bool IsDirty
        {
            get { return _dirty.Value; }
        }

        private string _backingFile;

        public string BackingFile
        {
            get { return _backingFile; }
            private set { this.RaiseAndSetIfChanged(ref _backingFile, value); }
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

        private string _background;

        public string Background
        {
            get { return _background; }
            set { this.RaiseAndSetIfChanged(ref _background, value); }
        }

        #endregion

        #region Command Implementations

        private IObservable<bool> LoadImpl()
        {
            var loaded = false;

            var dialog = new OpenFileDialog
            {
                Filter = FileDialogFilter
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var contents = File.ReadAllText(dialog.FileName);
                var plan = JsonConvert.DeserializeObject<CharacterPlan>(contents);

                SetFromPlan(plan);

                BackingFile = dialog.FileName;

                loaded = true;
            }

            return Observable.Return(loaded);
        }

        private IObservable<bool> SaveImpl()
        {
            if (BackingFile == null)
            {
                var dialog = new SaveFileDialog
                {
                    Filter = FileDialogFilter
                };

                var result = dialog.ShowDialog();

                if (result == true)
                {
                    BackingFile = dialog.FileName;
                }
                else
                {
                    return Observable.Return(false);
                }
            }

            var plan = GetPlan();

            var contents = JsonConvert.SerializeObject(plan, Formatting.Indented);

            File.WriteAllText(BackingFile, contents);

            return Observable.Return(true);
        }

        #endregion

        #region Conversion VM <-> M

        private CharacterPlan GetPlan()
        {
            var plan = new CharacterPlan
            {
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

                Background = Background,

                LevelPlans = new List<CharacterLevelPlan>()
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

            Background = plan.Background;
        }

        #endregion
    }
}
