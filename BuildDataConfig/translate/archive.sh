#!/usr/bin/env bash
cd $(dirname ${0})

rm -f collect_*.zip

set -x

7z a $(date '+collect_%m%d_%H%M.zip') database/collect*.xls
