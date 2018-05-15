#!/bin/bash
dir=$(cd -P -- "$(dirname -- "$0")" && pwd -P)
cd $dir
rm -r ./GenEntitasUnity/*
mkdir -p ./GenEntitasUnity/Editor
cp -R ./ComponentsLib/Components/ ./GenEntitasUnity/Editor/Components
cp -R ./GenEntitasLib/Custom/ ./GenEntitasUnity/Editor/Custom
cp ./GenEntitas/Custom/Runner.cs ./GenEntitasUnity/Editor/Runner.cs
cp ./UnityRunner/GenEntitasUnityRunner.cs ./GenEntitasUnity/Editor/GenEntitasUnityRunner.cs
