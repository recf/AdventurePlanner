using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Remoting;
using ReactiveUI;

namespace AdventurePlanner.UI.ViewModels
{
    public class DirtifiableObject : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _dirty;

        private readonly Subject<bool> _dirtySubject;

        private readonly Dictionary<DirtifiableObject, IDisposable> _monitored;

        public DirtifiableObject(params string[] derivedProperties)
        {
            _dirtySubject = new Subject<bool>();
            _dirtySubject.ToProperty(this, x => x.IsDirty, out _dirty);

            _monitored = new Dictionary<DirtifiableObject, IDisposable>();
            
            var excludes = derivedProperties.Concat(new[] { "IsDirty" }).ToArray();

            DataChanged = Changed.Where(e => !excludes.Contains(e.PropertyName));
            DataChanged.Subscribe(_ => MarkDirty());

            Dirtied = _dirtySubject.Where(x => x).Select(_ => Unit.Default);
        }

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> DataChanged { get; private set; }
        
        public IObservable<Unit> Dirtied { get; set; }

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

            foreach (var dirtifiable in _monitored.Keys)
            {
                dirtifiable.MarkClean();
            }
        }

        protected void Monitor<T>(IReactiveList<T> dirtifiables) where T : DirtifiableObject
        {
            foreach (var dirtifiable in dirtifiables)
            {
                Monitor(dirtifiable);
            }

            dirtifiables.ItemsAdded.Subscribe(x => { Monitor(x); MarkDirty(); });
            dirtifiables.ItemsRemoved.Subscribe(x => { Unmonitor(x); MarkDirty(); });
        }

        protected void Monitor(DirtifiableObject dirtifiable)
        {
            var sub = dirtifiable.Dirtied.Subscribe(_ => MarkDirty());
            _monitored.Add(dirtifiable, sub);
        }

        protected void Unmonitor(DirtifiableObject dirtifiable)
        {
            var sub = _monitored[dirtifiable];
            sub.Dispose();
            _monitored.Remove(dirtifiable);
        }
    }
}
