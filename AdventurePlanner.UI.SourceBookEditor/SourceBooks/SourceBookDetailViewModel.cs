using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using AdventurePlanner.Domain;
using AdventurePlanner.UI.SourceBookEditor.DataAccess;
using Microsoft.Win32;

namespace AdventurePlanner.UI.SourceBookEditor.SourceBooks
{
    public class SourceBookDetailViewModel : BindableBase
    {
        private SourceBookService _service;

        public SourceBookDetailViewModel(SourceBookService service)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            _service = service;

            OpenCommand = new RelayCommand(OnOpen, CanOpen);
            SaveCommand = new RelayCommand(OnSave, CanSave);

            SourceBookModel = new SourceBookModel();
        }

        public RelayCommand OpenCommand { get; set; }

        private bool CanOpen()
        {
            return true;
        }

        private void OnOpen()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Source Book file|*.apsb,*.json";
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                return;
            }

            var sourceBook = _service.Open(dialog.FileName);
            FilePath = dialog.FileName;
            
            SourceBookModel.SetFromDomainObject(sourceBook);
        }

        public RelayCommand SaveCommand { get; set; }

        private bool CanSave()
        {
            return !SourceBookModel.HasErrors;
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

            _service.Save(SourceBookModel.GetDomainObject(), FilePath);
        }

        private SourceBookModel _sourceBookModel;

        public SourceBookModel SourceBookModel
        {
            get { return _sourceBookModel; }
            set
            {
                Set(ref _sourceBookModel, value);
                value.ErrorsChanged += (sender, args) => { SaveCommand.RaiseCanExecuteChanged(); };
            }
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { Set(ref _filePath, value); }
        }
    }
}