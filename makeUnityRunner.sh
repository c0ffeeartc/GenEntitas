#!/bin/bash
dir=$(cd -P -- "$(dirname -- "$0")" && pwd -P)
cd $dir
rm -r ./GenEntitasUnity/*
mkdir -p ./GenEntitasUnity/Editor/Libraries
cp -R ./ComponentsLib/Components/ ./GenEntitasUnity/Editor/Components
cp -R ./Generated/Sources/ ./GenEntitasUnity/Editor/Generated
cp -R ./GenEntitasLib/Custom/ ./GenEntitasUnity/Editor/Systems

cp -R ./packages/Sprache.2.1.2/lib/net45 ./GenEntitasUnity/Editor/Libraries/Sprache_net45
cp -R ./GenEntitasLang/Sources/ ./GenEntitasUnity/Editor/GenEntitasLang
cp -R ./GenEntitasSettings/Sources/ ./GenEntitasUnity/Editor/Settings

cp ./GenEntitas/Custom/Runner.cs ./GenEntitasUnity/Editor/Runner.cs
cp ./UnityRunner/GenEntitasUnityRunner.cs ./GenEntitasUnity/Editor/GenEntitasUnityRunner.cs
