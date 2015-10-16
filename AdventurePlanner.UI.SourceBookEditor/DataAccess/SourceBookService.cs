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
            dto.SetFromDomain(sourceBook);

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                var serializer = new DataContractJsonSerializer(typeof (SourceBookDto));
                serializer.WriteObject(stream, dto);
            }
        }
    }
}