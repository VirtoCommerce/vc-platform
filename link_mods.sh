#!/usr/bin/env bash
#
# This script builds, publishes and symlinks all Virto modules.
#

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
cd $SCRIPT_DIR

mkdir -p src/VirtoCommerce.Platform.Web/Modules

for d in ../vc-module-* ; do
    if [ -d $d/src ]; then
        WEB_MOD=$(basename $(ls -d $d/src/*Web))
        SOURCE_MOD_PATH=../../../../$d/src/$WEB_MOD
    else
        WEB_MOD=$(basename $(ls -d $d/*Web))
        SOURCE_MOD_PATH=../../../../$d/$WEB_MOD
    fi;
    DEST_MOD_PATH=src/VirtoCommerce.Platform.Web/Modules/$(basename $d)

    mkdir -p $DEST_MOD_PATH && pushd $DEST_MOD_PATH
    if [ -f $SOURCE_MOD_PATH/package.json ]; then
        npm --prefix $SOURCE_MOD_PATH ci && npm run --prefix $SOURCE_MOD_PATH webpack:build
    fi;
    dotnet publish $(ls $SOURCE_MOD_PATH/*.csproj)
    ln -sf $SOURCE_MOD_PATH/module.manifest module.manifest
    ln -sf $SOURCE_MOD_PATH/bin/Debug/netcoreapp3.1 bin
    if [ -d $SOURCE_MOD_PATH/dist ]; then ln -sf $SOURCE_MOD_PATH/dist dist; fi
    if [ -d $SOURCE_MOD_PATH/Localizations ]; then ln -sf $SOURCE_MOD_PATH/Localizations Localizations; fi
    if [ -d $SOURCE_MOD_PATH/Scripts ]; then ln -sf $SOURCE_MOD_PATH/Scripts Scripts; fi
    if [ -d $SOURCE_MOD_PATH/Content ]; then ln -sf $SOURCE_MOD_PATH/Content Content; fi
    popd
done
