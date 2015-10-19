using System.ComponentModel.DataAnnotations;
using AdventurePlanner.Domain;

namespace AdventurePlanner.UI.SourceBookEditor.SourceBooks
{
    public class SourceBookModel : ValidatableBindableBase, IDomainRepresentationObject<SourceBook>
    {
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

        public void SetFromDomainObject(SourceBook domainObject)
        {
            Identifier = domainObject.Identifier;
            Name = domainObject.Name;
        }

        public SourceBook GetDomainObject()
        {
            return new SourceBook(Identifier, Name);
        }
    }
}