using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventurePlanner.Core.Snapshots
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
