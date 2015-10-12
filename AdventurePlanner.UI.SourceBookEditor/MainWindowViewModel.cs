using System.Security.RightsManagement;
using AdventurePlanner.UI.SourceBookEditor.SourceBooks;

namespace AdventurePlanner.UI.SourceBookEditor
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            CurrentViewModel = new SourceBookDetailViewModel();
        }

        public object CurrentViewModel { get; set; }
    }
}