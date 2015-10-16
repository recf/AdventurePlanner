using System.Runtime.Serialization;
using AdventurePlanner.Domain;

namespace AdventurePlanner.UI.SourceBookEditor.DataAccess
{
    [DataContract]
    public class SourceBookDto : IDataTrasferObject<SourceBook>
    {
        [DataMember(Name = "identifier", IsRequired = true)]
        public string Identifier { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        #region Implementation of IDataTrasferObject<SourceBook>

        public void SetFromDomain(SourceBook domainObject)
        {
            Identifier = domainObject.Identifier;
            Name = domainObject.Name;
        }

        public SourceBook GetDomainObject()
        {
            return new SourceBook(Identifier, Name);
        }

        #endregion
    }

    public interface IDataTrasferObject<T>
    {
        void SetFromDomain(T domainObject);
        T GetDomainObject();
    }
}