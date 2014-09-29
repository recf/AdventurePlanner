using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AdventurePlanner.Core.Planning;
using Microsoft.Win32;
using Newtonsoft.Json;
using ReactiveUI;

namespace AdventurePlanner.UI
{
    public class CharacterPlanViewModel : ReactiveObject
    {
        private static readonly string[] BookkeepingProperties = { "IsDirty", "BackingFile" };

        public CharacterPlanViewModel()
        {
            var canSave = this.ObservableForProperty(x => x.IsDirty).Select(y => y.Value);

            Load = ReactiveCommand.CreateAsyncObservable(_ => LoadImpl());
            Save = ReactiveCommand.CreateAsyncObservable(canSave, _ => SaveImpl());

            var dataPropertyChanged = Changed.Where(e => !BookkeepingProperties.Contains(e.PropertyName));

            Observable.Merge(
                Load.Select(loaded => !loaded),
                Save.Select(saved => !saved),
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

        #endregion

        #region Command Implementations

        private IObservable<bool> LoadImpl()
        {
            var loaded = false;

            var dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Adventure Planner Character files (*.apchar)|*.apchar"
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
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Adventure Planner Character files (*.apchar)|*.apchar"
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
                Speed = 0,
                Alignment = "TN",
                Background = string.Empty,

                Age = 0,
                HeightFeet = 0,
                HeightInches = 0,
                EyeColor = string.Empty,
                HairColor = string.Empty,
                SkinColor = string.Empty,

                LevelPlans = new List<CharacterLevelPlan>()
            };

            return plan;
        }

        private void SetFromPlan(CharacterPlan plan)
        {
            CharacterName = plan.Name;
            Race = plan.Race;
        }

        #endregion
    }
}
