TODOs
=====

Source TODOs
============

* \AdventurePlanner.Core\Planning\CharacterLevelPlan.cs
    * 14 Convert Ability Score increases to to a dictionary (to map to snapshot)
* \AdventurePlanner.UI\MarkdownExtensions.cs
    * 39 To paragraph/subheader?
* \AdventurePlanner.UI\ViewModels\CharacterPlanViewModel.cs
    * 31 Bug: marking dirty on modify level plans means we are not marked clean on load.
    * 67 Bug: Snapshot is loading as whatever is in the file for the slider, but always as 1 in the markdown.
* \AdventurePlannter.Core.Tests\CharacterPlanTests.cs
    * 21 Figure out how I want to test multiple cases (snapshot at different levels, etc).
