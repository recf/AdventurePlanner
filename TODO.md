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
* Some info is only added when you first take a class. Extract that stuff into a
  class plan, and change the class field in the Level Plan UI to a combo box
  that pulls from that list. The ClassPlan class needs the following properties:
  * Class Name
  * Hit Dice
  * Armor/Weapon/Tool Proficiencies
  * Saving Throws
  * Specialization (e.g. Druid Circle, Cleric Domain)
* Spells. While it's possible to find spell books, I think planning ahead for
  spell progression is the more common case, so it should be part of the level
  plan.
* Equipment. Should be separate from the level plan, since equipment changes are
  probably coming from loot or stores, this should be separate from level plan.


Source TODOs
============

* \AdventurePlanner.Core\TextExtensions.cs
    * 50 To paragraph/subheader?
* \AdventurePlanner.Core\Planning\CharacterPlan.cs
    * 67 :question: Consider moving ToSnapshot into an extension method.
    * 93 :poop: This is really clunky
* \AdventurePlanner.UI\ViewModels\CharacterPlanViewModel.cs
    * 44 Consider: Instead of calling AddLevel on create, instead call SetFromPlan() with initial state.
* \AdventurePlanner.UI\ViewModels\ClassPlanViewModel.cs
    * 36 :bug: Not running when loading a file. I ran into this issue before, but can't remember where or how I got around it. Look into dispatchers.
* \AdventurePlanner.UI\ViewModels\FeaturePlanViewModel.cs
    * 34 :bug: Cannot be deselected. Add Null Item?
* \AdventurePlannter.Core.Tests\CharacterPlanTests.cs
    * 21 Figure out how I want to test multiple cases (snapshot at different levels, etc).
