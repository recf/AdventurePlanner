using System;
using System.IO;
using System.Linq;
using AdventurePlanner.Core;
using AdventurePlanner.Core.Planning;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Build
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bin = AppDomain.CurrentDomain.BaseDirectory;

            var solutionDir = Path.GetFullPath(Path.Combine(bin, "../../../"));

            var schemaDirectory = Path.Combine(solutionDir, "docs", "schemas");
            var examplesDirectory = Path.Combine(solutionDir, "docs", "examples");

            CleanDirectory(schemaDirectory);
            GenerateSchema(schemaDirectory, typeof(CharacterPlan));

            CreateSnapshots(examplesDirectory);
        }

        private static void CreateSnapshots(string examplesDirectory)
        {
            foreach (var planFile in Directory.EnumerateFiles(examplesDirectory, "*.apc"))
            {
                var planFileBaseName = Path.GetFileNameWithoutExtension(planFile);
                var plan = JsonConvert.DeserializeObject<CharacterPlan>(File.ReadAllText(planFile));

                var numLevelPlans = plan.LevelPlans.Count;
                var levelsToSnapshot = from n in new[] { 1, 5, 10, 20 }
                                       where n <= numLevelPlans
                                       select n;

                foreach (var levelNum in levelsToSnapshot)
                {
                    var snapshot = plan.ToSnapshot(levelNum);

                    var fileName = Path.Combine(
                        examplesDirectory,
                        string.Format("{0}-at-level-{1}.txt", planFileBaseName, levelNum));

                    File.WriteAllText(fileName, snapshot.ToText());
                }
            }
        }

        private static void CleanDirectory(string directory)
        {
            Directory.EnumerateFiles(directory).ToList().ForEach(File.Delete);
        }

        private static void GenerateSchema(string docsDirectory, Type type)
        {
            var generator = new JsonSchemaGenerator();

            var schema = generator.Generate(type);

            var writer = new StringWriter();
            var jsonTextWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented };

            schema.WriteTo(jsonTextWriter);

            var schemaName = schema.Title.Replace(' ', '-').ToLowerInvariant();
            var schemaPath = Path.Combine(docsDirectory, schemaName + ".schema.json");

            File.WriteAllText(schemaPath, writer.ToString());
        }
    }
}
