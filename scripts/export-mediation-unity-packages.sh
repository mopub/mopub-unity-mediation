#!/usr/bin/env bash
my_dir="$(dirname "$0")"
source "$my_dir/validate.sh"

# ==================================================== #
# Exports each of the third-party network adapters     #
# ==================================================== #

# NOTE: this script requires the mopub-unity-sdk repository to be present as a sibling of this one

UNITY_BIN=/Applications/Unity/Unity.app/Contents/MacOS/Unity

# **********************************************************
# TODO: Revert to public repo before merging to master!
# **********************************************************
PROJECT_PATH="`pwd`/../mopub-unity/unity-sample-app"

MOPUB_MEDIATION_UNITY_ROOT="Assets/MoPub/Mediation"
NETWORK_INSTALLERS_DIR="`pwd`/mopub-unity-adapters"
OUT_DIR="`pwd`/unity-packages"
IOS_MEDIATION_DIR="mopub-ios-mediation"
ANDROID_MEDIATION_DIR="mopub-android-mediation"
UNITY_MEDIATION_DIR="mopub-unity-adapters"
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
# TODO: Read network to update from the command line
for SUPPORT_LIB in "${SUPPORT_LIBS[@]}"
do
    echo "Processing ${SUPPORT_LIB}..."

    # Gather necessary values
    NETWORK_ADAPTERS_NAME="MoPub-${SUPPORT_LIB}-Adapters"
    NETWORK_ADAPTERS_NAME_LOWERCASE=`echo "${NETWORK_ADAPTERS_NAME}" | tr '[:upper:]' '[:lower:]'`
    IOS_ADAPTER_DIR="${IOS_MEDIATION_DIR}/${SUPPORT_LIB}"
    IOS_EXPORT_DIR="${MOPUB_MEDIATION_UNITY_ROOT}/${SUPPORT_LIB}/Adapter/iOS"
    IOS_PODSPEC_FILE="${IOS_ADAPTER_DIR}/MoPub-${SUPPORT_LIB}-PodSpecs/${NETWORK_ADAPTERS_NAME}.podspec"
    IOS_ADAPTER_VERSION=`less $IOS_PODSPEC_FILE | grep s.version | sed "s/^.*'\([.0-9]*\)'.*/\1/"`
    IOS_SDK_VERSION=`echo ${IOS_ADAPTER_VERSION%.*}`
    ANDROID_ADAPTER_JAR="${ANDROID_MEDIATION_DIR}/libs/${NETWORK_ADAPTERS_NAME_LOWERCASE}-*.jar"
    ANDROID_EXPORT_DIR="${MOPUB_MEDIATION_UNITY_ROOT}/${SUPPORT_LIB}/Adapter/Android"
    ANDROID_ADAPTER_VERSION=`echo $ANDROID_ADAPTER_JAR | sed "s/^.*${NETWORK_ADAPTERS_NAME_LOWERCASE}-\([.0-9]*[^\.jar]\).*/\1/"`
    ANDROID_SDK_VERSION=`echo ${ANDROID_ADAPTER_VERSION%.*}`
    UNITY_SCRIPTS_DIR="${UNITY_MEDIATION_DIR}/${SUPPORT_LIB}"
    UNITY_ADAPTER_VERSION_FILE="${UNITY_SCRIPTS_DIR}/${SUPPORT_LIB}AdapterVersion.cs"
    UNITY_ADAPTER_VERSION=`less $UNITY_ADAPTER_VERSION_FILE | grep string\ _name | sed "s/^.*\"\([.0-9]*\)\".*/\1/"`
    UNITY_ANDROID_SDK_VERSION_FILE="${UNITY_SCRIPTS_DIR}/${SUPPORT_LIB}AndroidSDKVersion.cs"
    UNITY_IOS_SDK_VERSION_FILE="${UNITY_SCRIPTS_DIR}/${SUPPORT_LIB}IOSSDKVersion.cs"
    UNITY_SCRIPTS_EXPORT_DIR="${MOPUB_MEDIATION_UNITY_ROOT}/${SUPPORT_LIB}/Editor"

    # Update Adapter and SDK version numbers
    # TODO: Read adapter version bump from command line
    sed -i "" -e "s/\"[.0-9]*\"/\"${IOS_SDK_VERSION}\"/g" $UNITY_IOS_SDK_VERSION_FILE
    sed -i "" -e "s/\"[.0-9]*\"/\"${ANDROID_SDK_VERSION}\"/g" $UNITY_ANDROID_SDK_VERSION_FILE

    # Delete existing adapters
    echo "Removing existing adapters..."
    rm -r "${PROJECT_PATH}/${IOS_EXPORT_DIR}" 2> /dev/null
    rm "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}/"* 2> /dev/null
    rm "${PROJECT_PATH}/${UNITY_SCRIPTS_EXPORT_DIR}/"* 2> /dev/null

    # Copy over new adapters
    echo "Copying new adapters..."
    mkdir -p "${PROJECT_PATH}/${IOS_EXPORT_DIR}"
    cp -r "${IOS_ADAPTER_DIR}" "${PROJECT_PATH}/${IOS_EXPORT_DIR}"
    validate
    mkdir -p "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}"
    cp $ANDROID_ADAPTER_JAR "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}/${NETWORK_ADAPTERS_NAME_LOWERCASE}-${ANDROID_ADAPTER_VERSION}.jar"
    validate
    mkdir -p "${PROJECT_PATH}/${UNITY_SCRIPTS_EXPORT_DIR}"
    cp $UNITY_SCRIPTS_DIR/*.cs "${PROJECT_PATH}/${UNITY_SCRIPTS_EXPORT_DIR}/"
    validate

    # Generate Unity package
    echo "Exporting Unity package..."
    DEST_PACKAGE="${OUT_DIR}/${NETWORK_ADAPTERS_NAME}-${UNITY_ADAPTER_VERSION}.unitypackage"
    $UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile -exportPackage $IOS_EXPORT_DIR $ANDROID_EXPORT_DIR $UNITY_SCRIPTS_EXPORT_DIR $DEST_PACKAGE
    validate
    echo "Exported ${NETWORK_ADAPTERS_NAME} (iOS: ${IOS_EXPORT_DIR} | Android: ${ANDROID_EXPORT_DIR} | Unity: ${UNITY_SCRIPTS_EXPORT_DIR}) to ${DEST_PACKAGE}"
done

git add .
GREEN='\033[0;32m'
NC='\033[0m' # No Color
echo -e "${GREEN}DONE EXPORTING UNITY PACKAGES!${NC}"
echo "Test adapter updates via the updated sample app (under mopub-unity-sdk/) then commit and push these changes."
