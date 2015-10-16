using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using AdventurePlanner.Domain;
using AdventurePlanner.UI.SourceBookEditor.DataAccess;
using Microsoft.Win32;

namespace AdventurePlanner.UI.SourceBookEditor.SourceBooks
{
    public class SourceBookDetailViewModel : ValidatableBindableBase
    {
        private SourceBookService _service;

        public SourceBookDetailViewModel(SourceBookService service)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            _service = service;

            SaveCommand = new RelayCommand(OnSave, CanSave);

            this.ErrorsChanged += (sender, args) => { SaveCommand.RaiseCanExecuteChanged(); };
        }

        public RelayCommand SaveCommand { get; set; }

        private bool CanSave()
        {
            return !HasErrors;
        }

        private void OnSave()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "Source Book file|*.apsb,*.json";
                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    FilePath = dialog.FileName;
                }
            }

            _service.Save(GetDomainObject(), FilePath);
        }

        private SourceBook GetDomainObject()
        {
            return new SourceBook(Identifier, Name);
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { Set(ref _filePath, value); }
        }

        private string _identifier;

        [Required]
        public string Identifier
        {
            get { return _identifier; }
            set { Set(ref _identifier, value); }
        }

        private string _name;

        [Required]
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
    }
}