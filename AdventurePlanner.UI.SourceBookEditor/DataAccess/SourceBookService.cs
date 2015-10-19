using System;
using System.IO;
using System.Runtime.Serialization.Json;
using AdventurePlanner.Domain;

namespace AdventurePlanner.UI.SourceBookEditor.DataAccess
{
    public class SourceBookService
    {
        public void Save(SourceBook sourceBook, string filePath)
        {
            var dto = new SourceBookDto();
            dto.SetFromDomainObject(sourceBook);

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                var serializer = new DataContractJsonSerializer(typeof (SourceBookDto));
                serializer.WriteObject(stream, dto);
            }
        }

        public SourceBook Open(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var serializer = new DataContractJsonSerializer(typeof(SourceBookDto));
                var dto = serializer.ReadObject(stream) as SourceBookDto;
                return dto.GetDomainObject();
            }
        }
    }
}