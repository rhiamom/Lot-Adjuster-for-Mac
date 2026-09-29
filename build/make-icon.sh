#!/bin/bash
###############################################################################
#  Build build/AppIcon.icns from Mootilda's own LotAdjuster.ico (her program
#  icon, <ApplicationIcon> in LotExpander.csproj). It holds one 48x48 image,
#  so larger sizes are upscaled and look soft at Dock size.
#
#  Usage:  build/make-icon.sh [path to her LotAdjuster.ico]
###############################################################################
set -euo pipefail
cd "$(dirname "$0")"
ICO="${1:-$HOME/Library/Developer/MTS_Mootilda_1047111_LotAdjusterSource/LotAdjuster.ico}"
TMP="$(mktemp -d)"; trap 'rm -rf "$TMP"' EXIT
sips -s format png "$ICO" --out "$TMP/src.png" >/dev/null
SET="$TMP/AppIcon.iconset"; mkdir "$SET"
for s in 16 32 128 256 512; do
    sips -z $s $s "$TMP/src.png" --out "$SET/icon_${s}x${s}.png" >/dev/null
    d=$((s * 2))
    sips -z $d $d "$TMP/src.png" --out "$SET/icon_${s}x${s}@2x.png" >/dev/null
done
iconutil -c icns "$SET" -o AppIcon.icns
echo "Wrote $(pwd)/AppIcon.icns"
