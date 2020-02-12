#!/usr/bin/env bash
#
# This script clones or pulls all Virto 3 modules.
# If you want to combine it with link_mods.sh run this script in the directory that contains your clone of https://github.com/rb2-bv/vc-platform
#

if [ -d .git ]; then
  echo "Refusing to clone repositories, you are already in a git repository"
  exit 1
fi;


wget -O modules_v3.json https://raw.githubusercontent.com/VirtoCommerce/vc-modules/master/modules_v3.json
for repo in $(cat modules_v3.json | jq '.[].Versions[].PackageUrl' | awk -F[/:] '{print $6}'); do
    if [ -d $repo ]; then
        echo "Pulling latest $repo"
        git -C $repo pull
    else
        git clone https://github.com/VirtoCommerce/$repo
        git -C $repo checkout release/3.0.0
    fi;
done
