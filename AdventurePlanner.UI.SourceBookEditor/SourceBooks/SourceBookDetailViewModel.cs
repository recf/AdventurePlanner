using System.ComponentModel;
using System.Windows;

namespace AdventurePlanner.UI.SourceBookEditor.SourceBooks
{
    public class SourceBookDetailViewModel
    {
        public SourceBookDetailViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }
        }

        public string Identifier { get; set; }
        
        public string Name { get; set; }
    }
}