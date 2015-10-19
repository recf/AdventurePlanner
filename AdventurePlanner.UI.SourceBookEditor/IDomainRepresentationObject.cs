namespace AdventurePlanner.UI.SourceBookEditor
{
    public interface IDomainRepresentationObject<T>
    {
        void SetFromDomainObject(T domainObject);
        T GetDomainObject();
    }
}