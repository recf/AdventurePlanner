using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdventurePlanner.Core.Planning;
using Microsoft.Win32;
using Newtonsoft.Json;
using ReactiveUI;

namespace AdventurePlanner.UI
{
    public class CharacterPlanViewModel : ReactiveObject
    {
        public CharacterPlanViewModel()
        {
            var canSaveOrReload = this.ObservableForProperty(x => x.IsDirty).Select(y => y.Value);

            Load = ReactiveCommand.CreateAsyncObservable(_ => LoadImpl());
            Save = ReactiveCommand.CreateAsyncObservable(canSaveOrReload, _ => SaveImpl());
            Reload = ReactiveCommand.CreateAsyncObservable(canSaveOrReload, _ => ReloadImpl());

            var dataPropertyChanged = Changed.Where(e => e.PropertyName != "IsDirty");

            Observable.Merge(
                Load.Select(_ => true),
                Save.Select(b => !b),
                Reload.Select(b => !b),
                dataPropertyChanged.Select(_ => true)).ToProperty(this, x => x.IsDirty, out _dirty);
        }

        public ReactiveCommand<Unit> Load { get; private set; }

        public ReactiveCommand<bool> Save { get; private set; }

        public ReactiveCommand<bool> Reload { get; private set; }

        #region Bookkeeping Properties

        private readonly ObservableAsPropertyHelper<bool> _dirty;

        public bool IsDirty
        {
            get { return _dirty.Value; }
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

        private IObservable<Unit> LoadImpl()
        {
            return Observable.Return(Unit.Default);
        }

        private IObservable<bool> SaveImpl()
        {
            var saved = false;
            var dialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Adventure Planner Character files (*.apchar)|*.apchar"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var plan = GetPlan();

                var contents = JsonConvert.SerializeObject(plan, Formatting.Indented);

                File.WriteAllText(dialog.FileName, contents);
                saved = true;
            }

            return Observable.Return(saved);
        }
        
        private IObservable<bool> ReloadImpl()
        {
            return Observable.Return(true);
        }

        #endregion

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
    }
}
