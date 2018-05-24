#!/usr/bin/env bash
my_dir="$(dirname "$0")"
source "$my_dir/validate.sh"

# ==================================================== #
# Exports each of the third-party network adapters     #
# ==================================================== #

# NOTE: this script requires the mopub-unity-sdk repository to be present as a sibling of this one

UNITY_BIN=/Applications/Unity/Unity.app/Contents/MacOS/Unity
PROJECT_PATH="`pwd`/../mopub-unity-sdk/unity-sample-app"
OUT_DIR="`pwd`/unity-packages"
IOS_MEDIATION_DIR="mopub-ios-mediation"
ANDROID_MEDIATION_DIR="mopub-android-mediation"
SUPPORT_LIBS=( "AdColony" "AdMob" "AppLovin" "Chartboost" "FacebookAudienceNetwork" "Flurry" "IronSource" "OnebyAOL" "Tapjoy" "UnityAds" "Vungle" "IronSource")

# Check sample app under mopub-unity-sdk exists
ls $PROJECT_PATH 1> /dev/null
validate "Unity SDK sample app not found; is the mopub-unity-sdk repository a sibling of this one?\nExpected it to be at: ${PROJECT_PATH}"

# Update mediation submodules
cd $IOS_MEDIATION_DIR
git checkout master
git pull origin master
cd ..
cd $ANDROID_MEDIATION_DIR
git checkout master
git pull origin master
cd ..

# Delete existing packages
rm $OUT_DIR/* 2> /dev/null

# Generate Unity packages for all networks
for SUPPORT_LIB in "${SUPPORT_LIBS[@]}"
do
    echo "Processing ${SUPPORT_LIB}..."

    # Gather necessary values
    NETWORK_ADAPTERS_NAME="MoPub-${SUPPORT_LIB}-Adapters"
    NETWORK_ADAPTERS_NAME_LOWERCASE=`echo "${NETWORK_ADAPTERS_NAME}" | tr '[:upper:]' '[:lower:]'`
    IOS_ADAPTER_DIR="${IOS_MEDIATION_DIR}/${SUPPORT_LIB}"
    IOS_EXPORT_DIR="Assets/Plugins/iOS/${NETWORK_ADAPTERS_NAME}"
    IOS_PODSPEC_FILE="${IOS_ADAPTER_DIR}/MoPub-${SUPPORT_LIB}-PodSpecs/${NETWORK_ADAPTERS_NAME}.podspec"
    IOS_ADAPTER_VERSION=`less $IOS_PODSPEC_FILE | grep s.version | sed "s/^.*'\([.0-9]*\)'.*/\1/"`
    ANDROID_ADAPTER_JAR="${ANDROID_MEDIATION_DIR}/libs/${NETWORK_ADAPTERS_NAME_LOWERCASE}-*.jar"
    ANDROID_EXPORT_DIR="Assets/Plugins/Android/mopub-support/libs/${NETWORK_ADAPTERS_NAME}"
    ANDROID_ADAPTER_VERSION=`echo $ANDROID_ADAPTER_JAR|sed "s/^.*${NETWORK_ADAPTERS_NAME_LOWERCASE}-\([.0-9]*[^\.jar]\).*/\1/"`

    # Delete existing adapters
    echo "Removing existing adapters..."
    rm -r "${PROJECT_PATH}/${IOS_EXPORT_DIR}" 2> /dev/null
    rm "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}/"* 2> /dev/null

    # Copy over new adapters
    echo "Copying new adapters..."
    mkdir -p "${PROJECT_PATH}/${IOS_EXPORT_DIR}"
    cp -r "${IOS_ADAPTER_DIR}" "${PROJECT_PATH}/${IOS_EXPORT_DIR}"
    validate
    mkdir -p "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}"
    cp $ANDROID_ADAPTER_JAR "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}/${NETWORK_ADAPTERS_NAME_LOWERCASE}-${ANDROID_ADAPTER_VERSION}.jar"
    validate

    # Generate Unity package
    echo "Exporting Unity package..."
    echo "IOS_EXPORT_DIR: ${IOS_EXPORT_DIR}"
    DEST_PACKAGE="${OUT_DIR}/${NETWORK_ADAPTERS_NAME}-Android.${ANDROID_ADAPTER_VERSION}-iOS.${IOS_ADAPTER_VERSION}.unitypackage"
    $UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile -exportPackage $IOS_EXPORT_DIR $ANDROID_EXPORT_DIR $DEST_PACKAGE
    validate
    echo "Exported ${NETWORK_ADAPTERS_NAME} (iOS: ${IOS_EXPORT_DIR} | Android: ${ANDROID_EXPORT_DIR}) to ${DEST_PACKAGE}"
done

git add .
GREEN='\033[0;32m'
NC='\033[0m' # No Color
echo -e "${GREEN}DONE EXPORTING UNITY PACKAGES!${NC}"
echo "Test adapter updates via the updated sample app (under mopub-unity-sdk/) then commit and push these changes."
