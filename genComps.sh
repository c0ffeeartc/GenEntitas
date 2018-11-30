#!/bin/bash
dir=$(cd -P -- "$(dirname -- "$0")" && pwd -P)
cd $dir
mono ./GenEntitas.Runner.Console/bin/Debug/GenEntitas.exe --SettingsPath=./GenComps.settings
