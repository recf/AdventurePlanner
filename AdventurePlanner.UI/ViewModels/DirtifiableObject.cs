using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class DirtifiableObject : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _dirty;

        private readonly Subject<bool> _dirtySubject;

        public DirtifiableObject(params string[] derivedProperties)
        {
            _dirtySubject = new Subject<bool>();
            _dirtySubject.ToProperty(this, x => x.IsDirty, out _dirty);
            
            var excludes = derivedProperties.Concat(new[] { "IsDirty" }).ToArray();

            DataChanged = Changed.Where(e => !excludes.Contains(e.PropertyName));
            DataChanged.Subscribe(_ => MarkDirty());
        }

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> DataChanged { get; private set; }

        public bool IsDirty
        {
            get { return _dirty.Value; }
        }

        public void MarkDirty()
        {
            _dirtySubject.OnNext(true);
        }

        public void MarkClean()
        {
            _dirtySubject.OnNext(false);
        }
    }
}
