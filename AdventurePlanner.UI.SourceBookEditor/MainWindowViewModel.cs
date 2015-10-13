using AdventurePlanner.UI.SourceBookEditor.SourceBooks;
using Microsoft.Practices.Unity;

namespace AdventurePlanner.UI.SourceBookEditor
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel(IUnityContainer container)
        {
            CurrentViewModel = container.Resolve<SourceBookDetailViewModel>();
        }

        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { Set(ref _currentViewModel, value); }
        }
    }
}