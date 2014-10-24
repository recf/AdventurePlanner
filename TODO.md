TODOs
=====

* Add / Delete buttons for Ability Score Improvements
* :bug: Level Plan: Ability Score Improvements will throw if you try to add the
  same Ability more than once. Look into filtering available options based on
  what has already been selected. I should do the same thing for New Skill
  Proficiencies, but that shouldn't actually throw, so it's less important.
* :question: Modify ExtractSourceTodos.ps1 to read XML comments.
* For the expanders in LevelPlanView.xaml, I'd like to have the buttons in the
  header. Research Attached Properties (local:Expander.HeaderCommands) and
  overriding the HeaderTemplate style, based on the Mahapps style.


Source TODOs
============

* \AdventurePlanner.UI\MarkdownExtensions.cs
    * 39 To paragraph/subheader?
* \AdventurePlanner.UI\ViewModels\CharacterPlanViewModel.cs
    * 37 Consider: Instead of calling AddLevel on create, instead call SetFromPlan() with initial state.
* \AdventurePlannter.Core.Tests\CharacterPlanTests.cs
    * 21 Figure out how I want to test multiple cases (snapshot at different levels, etc).
