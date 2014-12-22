Properties {
    $config = "Debug"
    $build_dir = Split-Path $psake.build_script_file
    $bin_ui = "$build_dir\AdventurePlanner.UI\bin\$config"
}

TaskSetup {
    $env:path += ";$bin_ui"
}

Task RunUi {
    exec { AdventurePlanner.UI.exe }
}
