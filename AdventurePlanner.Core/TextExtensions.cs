using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Snapshots;
using MarkdownLog;

namespace AdventurePlanner.Core
{
    public static class TextExtensions
    {
        public static string ToText(this CharacterSnapshot snapshot)
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

            var abilityScoresHeader = "Ability Scores".ToMarkdownSubHeader();
            var abilityScores = snapshot.Abilities.Values.Select(
                a => new { Ability = a.Abbreviation, a.Score, Mod = a.Modifier }).ToMarkdownTable();

            var savingThrowHeader = "Saving Throws".ToMarkdownSubHeader();
            var savingThrows = snapshot.SavingThrows.Values.Select(
                s => new
                {
                    Prof = s.IsProficient.ToTextCheckbox(),
                    Mod = s.Modifier,
                    Saving_Throw = s.Ability.AbilityName,
                    Notes = string.Empty,
                }).ToMarkdownTable();

            // TODO: To paragraph/subheader?
            var profBonus = new Dictionary<string, object>
            {
                { "Proficiency Bonus", snapshot.ProficiencyBonus }
            }.ToMarkdownBulletedList();


            Func<FeatureSnapshot, string> formatFeature = f => string.Format(
                string.IsNullOrWhiteSpace(f.Description) ? "{0}" : "{0} - {1}",
                f.Name,
                f.Description);

            var skillsHeader = "Skills".ToMarkdownSubHeader();
            var skills = snapshot.Skills.Values.Select(
                s => new
                {
                    Prof = s.IsProficient.ToTextCheckbox(), 
                    Mod = s.Modifier, 
                    Skill = string.Format("{0} ({1})", s.SkillName, s.Ability.Abbreviation), 
                    Notes = string.Join("; ", s.Features.Select(formatFeature))
                }).ToMarkdownTable();

            var proficienciesSubHeader = "Proficiencies".ToMarkdownSubHeader();
            var proficiencies = new Dictionary<string, string>
            {
                { "Armor", string.Join(", ", snapshot.ArmorProficiencies) },
                { "Weapons", string.Join(", ", snapshot.WeaponProficiencies) },
                { "Tools", string.Join(", ", snapshot.ToolProficiencies) },
            }.ToMarkdownBulletedList();

            var featuresSubHeader = "Other Features & Traits".ToMarkdownSubHeader();
            var features = snapshot.Features.OrderBy(f => f.Name).Select(formatFeature).ToMarkdownBulletedList();

            container.Append(header);
            container.Append(infoBlock);

            container.Append(abilityScoresHeader);
            container.Append(abilityScores);

            container.Append(profBonus);

            container.Append(savingThrowHeader);
            container.Append(savingThrows);

            container.Append(skillsHeader);
            container.Append(skills);

            container.Append(proficienciesSubHeader);
            container.Append(proficiencies);

            container.Append(featuresSubHeader);
            container.Append(features);

            return container.ToMarkdown();
        }

        public static string ToTextCheckbox(this bool check)
        {
            return check ? "[x]" : "[ ]";
        }

        public static BulletedList ToMarkdownBulletedList<T>(this Dictionary<string, T> dict)
        {
            return dict.ToMarkdownBulletedList(kvp => string.Format("{0}: {1}", kvp.Key, kvp.Value));
        }
    }
}
