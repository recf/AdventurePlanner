using System;

namespace AdventurePlanner.Domain
{
    public class AbilityScore
    {
        public AbilityScore(string abbreviation, string name)
        {
            // TODO: :question: make these actual ability objects?
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
