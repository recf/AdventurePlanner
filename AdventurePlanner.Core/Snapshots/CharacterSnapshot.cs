using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventurePlanner.Core.Snapshots
{
    public class CharacterSnapshot
    {
        public string Name { get; set; }

        public string Race { get; set; }

        public int Speed { get; set; }

        public string Alignment { get; set; }

        public string Background { get; set; }

        #region Appearance

        public int Age { get; set; }

        public int? HeightFeet { get; set; }

        public int HeightInches { get; set; }

        public int Weight { get; set; }

        public string EyeColor { get; set; }

        public string HairColor { get; set; }

        public string SkinColor { get; set; }

        #endregion

        public int CharacterLevel
        {
            get { return Classes.Sum(kvp => kvp.Value); }
        }

        public IDictionary<string, int> Classes { get; set; }

        #region Ability Scores

        public AbilitySnapshot StrScore { get; private set; }

        public AbilitySnapshot DexScore { get; private set; }

        public AbilitySnapshot ConScore { get; private set; }

        public AbilitySnapshot IntScore { get; private set; }

        public AbilitySnapshot WisScore { get; private set; }

        public AbilitySnapshot ChaScore { get; private set; }

        #endregion

        public int ProficiencyBonus { get; set; }

        public CharacterSnapshot()
        {
            StrScore = new AbilitySnapshot();
            DexScore = new AbilitySnapshot();
            ConScore = new AbilitySnapshot();
            IntScore = new AbilitySnapshot();
            WisScore = new AbilitySnapshot();
            ChaScore = new AbilitySnapshot();
        }
    }
}
