using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Snapshots;

namespace AdventurePlanner.Core
{
    public static class TextExtensions
    {
        public static string ToAsciiDocCheckbox(this bool check)
        {
            return check ? "icon:check-square-o[]" : "icon:square-o[]";
        }

        public static StringBuilder AppendAsciiDocHeader(this StringBuilder builder, string headerText, int level = 1)
        {
            var prefix = new string('=', level);
            builder.AppendLine(string.Format("{0} {1}", prefix, headerText));

            if (level > 1)
            {
                builder.AppendLine();
            }

            return builder;
        }

        public static StringBuilder AppendAsciiDocAttribute(
            this StringBuilder builder, 
            string attribute,
            object value = null)
        {
            return builder.AppendLine(string.Format(":{0}: {1}", attribute, value).Trim());
        }

        public static StringBuilder AppendAsciiDocLabeledList<T>(this StringBuilder builder, Dictionary<string, T> items)
        {
            foreach (var kvp in items)
            {
                builder.AppendLine(string.Format("{0}:: {1}", kvp.Key, kvp.Value.OrEmpty()));
            }

            builder.AppendLine();

            return builder;
        }
        
        private const string TableDelim = "|===";

        public static StringBuilder AppendAsciiDocTable(
            this StringBuilder builder,
            IList<Dictionary<string, object>> items,
            string columnSpec = null)
        {
            if (!items.Any())
            {
                return builder;
            }

            var firstItem = items.First();

            if (string.IsNullOrWhiteSpace(columnSpec))
            {
                columnSpec = string.Format("{0}*", firstItem.Keys.Count);
            }

            builder.AppendLine(string.Format("[cols=\"{0}\", options=\"header\"]", columnSpec));

            builder.AppendLine(TableDelim);

            foreach (var key in firstItem.Keys)
            {
                builder.Append(string.Format("| {0} ", key));
            }
            builder.AppendLine();
            builder.AppendLine();

            foreach (var item in items)
            {
                foreach (var key in item.Keys)
                {
                    var value = item[key].OrEmpty();
                    
                    builder.Append(string.Format("| {0} ", value));
                }
                builder.AppendLine();
            }

            builder.AppendLine(TableDelim);
            builder.AppendLine();

            return builder;
        }

        public static object OrEmpty(this object value)
        {
            return string.IsNullOrWhiteSpace(Convert.ToString(value)) ? "{empty}" : value;
        }

        public static string ToText(this CharacterSnapshot snapshot)
        {
            var builder = new StringBuilder();

            // Header
            builder.AppendAsciiDocHeader(snapshot.Name);
            builder.AppendAsciiDocAttribute("revnumber", snapshot.CharacterLevel);
            builder.AppendAsciiDocAttribute("revdate", DateTime.Today.ToString("yyyy-MM-dd"));

            builder.AppendAsciiDocAttribute("version-label", "Level");
            builder.AppendAsciiDocAttribute("nofooter");
            builder.AppendAsciiDocAttribute("icons", "font");
            builder.AppendLine();

            var classes = snapshot.Classes.Select(kvp => string.Format("{0} {1}", kvp.Key, kvp.Value));

            builder.AppendLine("[horizontal]");
            builder.AppendAsciiDocLabeledList(new Dictionary<string, object>
            {
                { "Class levels", string.Join(", ", classes) }, 
                { "Race", snapshot.Race }, 
                { "Speed", snapshot.Speed },
                { "Alignment", snapshot.Alignment }, 
                { "Age", snapshot.Age }, 
                { "Weight", string.Format("{0} lbs.", snapshot.Weight) }, 
                { "Height", string.Format("{0}'{1}\"", snapshot.HeightFeet, snapshot.HeightInches) }, 
                { "Eyes", snapshot.EyeColor }, 
                { "Skin", snapshot.SkinColor }, 
                { "Hair", snapshot.HairColor }, 
                { "Proficiency Bonus", snapshot.ProficiencyBonus }
            });

            builder.AppendAsciiDocHeader("Ability Scores", 2);

            var abilityScores = snapshot.Abilities.Values.Select(
                a => new Dictionary<string, object>()
                {
                    { "Ability", a.Abbreviation },
                    { "Score", a.Score },
                    { "Mod", a.Modifier },
                }).ToList();

            builder.AppendAsciiDocTable(abilityScores);

            builder.AppendAsciiDocHeader("Saving Throws", 2);

            var savingThrows = snapshot.SavingThrows.Values.Select(
                s => new Dictionary<string, object>()
                {
                    { "Prof", s.IsProficient.ToAsciiDocCheckbox() },
                    { "Mod", s.Modifier },
                    { "Saving Throw", s.Ability.AbilityName },
                }).ToList();

            builder.AppendAsciiDocTable(savingThrows, "1*,a,1*");
            
            Func<FeatureSnapshot, string> formatFeature = f => string.Format(
                string.IsNullOrWhiteSpace(f.Description) ? "{0}" : "{0}:: {1}",
                f.Name,
                f.Description);

            builder.AppendAsciiDocHeader("Skills", 2);

            var skills = snapshot.Skills.Values.Select(
                s => new Dictionary<string, object>()
                {
                    { "Prof", s.IsProficient.ToAsciiDocCheckbox() },
                    { "Mod", s.Modifier },
                    { "Skill", string.Format("{0} ({1})", s.SkillName, s.Ability.Abbreviation) },
                    { "Notes", string.Join(Environment.NewLine, s.Features.Select(formatFeature)) },
                }).ToList();

            builder.AppendAsciiDocTable(skills, "a,2*,a");

            builder.AppendAsciiDocHeader("Proficiencies", 2);

            var proficiencies = new Dictionary<string, string>
            {
                { "Armor", string.Join(", ", snapshot.ArmorProficiencies) },
                { "Weapons", string.Join(", ", snapshot.WeaponProficiencies) },
                { "Tools", string.Join(", ", snapshot.ToolProficiencies) },
            };

            builder.AppendAsciiDocLabeledList(proficiencies);

            builder.AppendAsciiDocHeader("Other Features & Traits", 2);

            var features = snapshot.Features.OrderBy(f => f.Name).ToDictionary(f => f.Name, f => f.Description);

            builder.AppendAsciiDocLabeledList(features);

            builder.AppendAsciiDocHeader("Equipment", 2);

            builder.AppendAsciiDocHeader("Weapons & Attacks", 3);

            var attacks = snapshot.Weapons.SelectMany(w=> w.GetAttacks(snapshot), (w, a) => new Dictionary<string, object>
                {
                    { "Weapon", w.Name },
                    { "Attack", a.Name },
                    { "Attack Bonus", a.AttackModifier },
                    { "Damage", string.Format("{0}/{1}", a.DamageDice, a.DamageType) }
                }).ToList();

            builder.AppendAsciiDocTable(attacks);

            builder.AppendAsciiDocHeader("Armor", 3);

            var armor = snapshot.Armor.Select(
                a => new Dictionary<string, object>
                {
                    { "AC", a.TotalArmorClass },
                    { "Name", a.ArmorName },
                    { "Proficiency Group", string.Format("{0} {1}", a.IsProficient.ToAsciiDocCheckbox(), a.ProficiencyGroup) },
                }).ToList();

            builder.AppendAsciiDocTable(armor);

            return builder.ToString();
        }
    }
}
