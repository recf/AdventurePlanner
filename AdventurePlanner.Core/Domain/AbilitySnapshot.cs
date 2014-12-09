using System;

namespace AdventurePlanner.Core.Domain
{
    public class AbilitySnapshot
    {
        public AbilitySnapshot(string abbreviation, string name)
        {
            Abbreviation = abbreviation;
            AbilityName = name;
        }

        public string Abbreviation { get; private set; }

        public string AbilityName { get; private set; }

        public int Score { get; set; }

        public int Modifier
        {
            get
            {
                var score = (decimal)Score;
                return (int)Math.Floor((score - 10) / 2);
            }
        }
    }
}
