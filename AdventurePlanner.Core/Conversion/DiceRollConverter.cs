using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polyhedral;

namespace AdventurePlanner.Core.Conversion
{
    public class DiceRollConverter :JsonConverter
    {
        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var diceRoll = (DiceRoll) value;

            writer.WriteValue(diceRoll.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var input = (string) reader.Value;

            return DiceRoll.Parse(input);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DiceRoll).IsAssignableFrom(objectType);
        }

        #endregion
    }
}
