using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace AdventurePlanner.UI
{
    public class CharacterPlanViewModel : ReactiveObject
    {
        public CharacterPlanViewModel()
        {
            var canSaveOrReload = this.ObservableForProperty(x => x.IsDirty).Select(y => y.Value);

            Save = ReactiveCommand.CreateAsyncObservable(canSaveOrReload, _ => SaveImpl());
            Reload = ReactiveCommand.CreateAsyncObservable(canSaveOrReload, _ => ReloadImpl());

            var dataPropertyChanged = Changed.Where(e => e.PropertyName != "IsDirty");
            
            Observable.Merge(
                Save.Select(_ => false),
                Reload.Select(_ => false),
                dataPropertyChanged.Select(_ => true)).ToProperty(this, x => x.IsDirty, out _dirty);
        }

        private readonly ObservableAsPropertyHelper<bool> _dirty;

        public bool IsDirty
        {
            get { return _dirty.Value; }
        }

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

        public ReactiveCommand<Unit> Save { get; private set; }

        public IObservable<Unit> SaveImpl()
        {
            return Observable.Return(Unit.Default);
        }

        public ReactiveCommand<Unit> Reload { get; private set; }

        public IObservable<Unit> ReloadImpl()
        {
            return Observable.Return(Unit.Default);
        }
    }
}
