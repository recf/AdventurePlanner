using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Snapshots;
using MarkdownLog;

namespace AdventurePlanner.UI
{
    public static class MarkdownExtensions
    {
        public static MarkdownContainer ToMarkdownCharacterSheet(this CharacterSnapshot snapshot)
        {
            var container = new MarkdownContainer();

            var header = snapshot.Name.ToMarkdownHeader();

            var classes = snapshot.Classes.Select(kvp => string.Format("{0} {1}", kvp.Key, kvp.Value));

            var infoBlock = new Dictionary<string, object>
            {
                { "Character level", snapshot.CharacterLevel },
                { "Class levels", string.Join(", ", classes) },
                { "Race", snapshot.Race },
                { "Alignment", snapshot.Alignment },
                { "Age", snapshot.Age },
                { "Weight", string.Format("{0} lbs.", snapshot.Weight) },
                { "Height", string.Format("{0}'{1}\"", snapshot.HeightFeet, snapshot.HeightInches) },
                { "Eyes", snapshot.EyeColor },
                { "Skin", snapshot.SkinColor },
                { "Hair", snapshot.HairColor },
            }.ToMarkdownBulletedList();

            var abilityScores = new[]
            {
                new { Ability = "STR", snapshot.StrScore.Score, Mod = snapshot.StrScore.Modifier },
                new { Ability = "DEX", snapshot.DexScore.Score, Mod = snapshot.DexScore.Modifier },
                new { Ability = "CON", snapshot.ConScore.Score, Mod = snapshot.ConScore.Modifier },
                new { Ability = "INT", snapshot.IntScore.Score, Mod = snapshot.IntScore.Modifier },
                new { Ability = "WIS", snapshot.WisScore.Score, Mod = snapshot.WisScore.Modifier },
                new { Ability = "CHA", snapshot.ChaScore.Score, Mod = snapshot.ChaScore.Modifier },
            }.ToMarkdownTable();

            var profBonus = new Dictionary<string, object>
            {
                { "Proficiency Bonus", snapshot.ProficiencyBonus }
            }.ToMarkdownBulletedList();

            container.Append(header);
            container.Append(infoBlock);
            container.Append(abilityScores);
            container.Append(profBonus);

            return container;
        }

        public static BulletedList ToMarkdownBulletedList(this Dictionary<string, object> dict)
        {
            return dict.ToMarkdownBulletedList(kvp => string.Format("{0}: {1}", kvp.Key, kvp.Value));
        }
    }
}
