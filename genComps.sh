#!/bin/bash
dir=$(cd -P -- "$(dirname -- "$0")" && pwd -P)
cd $dir
mono ./GenEntitas/bin/Debug/GenEntitas.exe --dllPaths="./ComponentsLib/bin/Debug/ComponentsLib.dll" --generatePath="./GenEntitasLib/Custom/Sources"
