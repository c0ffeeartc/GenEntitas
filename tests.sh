#!/bin/bash
clear
#msbuild /property:Configuration=Release /verbosity:minimal CodeGen.sln
mono Tests/bin/Release/Tests.exe $@
