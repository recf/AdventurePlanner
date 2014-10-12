using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

            var abilityScores = snapshot.Abilities.Values.Select(
                a => new { Ability = a.Abbreviation, a.Score, a.Modifier }).ToMarkdownTable();

            // TODO: To paragraph/subheader?
            var profBonus = new Dictionary<string, object>
            {
                { "Proficiency Bonus", snapshot.ProficiencyBonus }
            }.ToMarkdownBulletedList();

            var skills = snapshot.Skills.Values.Select(
                s => new
                {
                    Prof = s.IsProficient.ToMarkdownCheckbox(),
                    Mod = s.Modifier,
                    Skill = string.Format("{0} ({1})", s.SkillName, s.Ability.Abbreviation),
                    Notes = string.Empty
                }).ToMarkdownTable();

            container.Append(header);
            container.Append(infoBlock);
            container.Append(abilityScores);
            container.Append(profBonus);
            container.Append(skills);

            return container;
        }

        public static string ToMarkdownCheckbox(this bool check)
        {
            return check ? "[x]" : "[ ]";
        }

        public static BulletedList ToMarkdownBulletedList(this Dictionary<string, object> dict)
        {
            return dict.ToMarkdownBulletedList(kvp => string.Format("{0}: {1}", kvp.Key, kvp.Value));
        }
    }
}
