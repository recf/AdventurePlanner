TODOs
=====

* :bug: Level Plan: Ability Score Improvements will throw if you try to add the
  same Ability more than once. Look into filtering available options based on
  what has already been selected. I should do the same thing for New Skill
  Proficiencies, but that shouldn't actually throw, so it's less important.
* :question: Modify ExtractSourceTodos.ps1 to read XML comments.
* For the expanders in LevelPlanView.xaml, I'd like to have the buttons in the
  header. Research Attached Properties (local:Expander.HeaderCommands) and
  overriding the HeaderTemplate style, based on the Mahapps style.
* Need to be able to have features replace older versions of
  themselves. Alternatively, a way to remove features. Probably need the same
  thing when I get to spells, attacks, weapons, etc.
* Convert this file and Snapshot Char Sheet to AsciiDoc
* :question: I liked the Attribute based approach to JSON serialization, but
  seems to be a bit limited compared to custom converters and hand-written
  schemas. Consider switching.

Source TODOs
============

* \AdventurePlanner.Core\Planning\CharacterPlan.cs
    * 63 :question: Consider moving ToSnapshot into an extension method.
* \AdventurePlanner.UI\MarkdownExtensions.cs
    * 50 To paragraph/subheader?
* \AdventurePlanner.UI\ViewModels\CharacterPlanViewModel.cs
    * 37 Consider: Instead of calling AddLevel on create, instead call SetFromPlan() with initial state.
* \AdventurePlanner.UI\ViewModels\FeaturePlanViewModel.cs
    * 34 :bug: Cannot be deselected. Add Null Item?
* \AdventurePlannter.Core.Tests\CharacterPlanTests.cs
    * 21 Figure out how I want to test multiple cases (snapshot at different levels, etc).
