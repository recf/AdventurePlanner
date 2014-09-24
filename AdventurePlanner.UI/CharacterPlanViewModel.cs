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
            //Save = ReactiveCommand.CreateAsyncObservable(_ => SaveImpl());
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public IReactiveCommand Save { get; private set; }

        //public IObservable<Unit> SaveImpl()
        //{
        //    return Observable.Create();
        //}
    }
}
