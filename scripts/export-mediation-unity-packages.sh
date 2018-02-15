#!/usr/bin/env bash
my_dir="$(dirname "$0")"
source "$my_dir/validate.sh"

# ==================================================== #
# Exports each of the third-party network adapters     #
# ==================================================== #

UNITY_BIN=/Applications/Unity/Unity.app/Contents/MacOS/Unity
PROJECT_PATH="`pwd`/unity-sample-app"
OUT_DIR="`pwd`/unity-packages"
IOS_MEDIATION_DIR="mopub-ios-mediation"
ANDROID_MEDIATION_DIR="mopub-android-mediation"
SUPPORT_LIBS=( "AdColony" "AdMob" "AOL" "AudienceNetwork" "Chartboost" "Flurry" "TapJoy" "UnityAds" "Vungle" )

# Update mediation submodules
cd $IOS_MEDIATION_DIR
git pull origin master
cd ..
cd $ANDROID_MEDIATION_DIR
git pull origin master
cd ..

# Delete existing packages
rm $OUT_DIR/* 2> /dev/null

# Generate Unity packages for all networks
for SUPPORT_LIB in "${SUPPORT_LIBS[@]}"
do
    echo "Processing ${SUPPORT_LIB}..."

    # Gather necessary values
    SUPPORT_LIB_LOWERCASE=`echo "${SUPPORT_LIB}" | tr '[:upper:]' '[:lower:]'`
    IOS_ADAPTER_DIR="${IOS_MEDIATION_DIR}/${SUPPORT_LIB}"
    IOS_EXPORT_DIR="Assets/MoPub/Editor/Support/${SUPPORT_LIB}"
    IOS_PODSPEC_FILE="${IOS_ADAPTER_DIR}/MoPub-${SUPPORT_LIB}-PodSpecs/MoPub-${SUPPORT_LIB}-Adapters.podspec"
    IOS_ADAPTER_VERSION=`less $IOS_PODSPEC_FILE | grep s.version | sed "s/^.*'\([.0-9]*\)'.*/\1/"`
    ANDROID_ADAPTER_JAR="${ANDROID_MEDIATION_DIR}/libs/${SUPPORT_LIB_LOWERCASE}-*.jar"
    ANDROID_EXPORT_DIR="Assets/Plugins/Android/mopub-support/libs/${SUPPORT_LIB}"
    ANDROID_ADAPTER_VERSION=`echo $ANDROID_ADAPTER_JAR|sed "s/^.*${SUPPORT_LIB_LOWERCASE}-\([.0-9]*[^\.jar]\).*/\1/"`

    # Delete existing adapters
    echo "Removing existing adapters..."
    rm -r "${PROJECT_PATH}/${IOS_EXPORT_DIR}" 2> /dev/null
    rm "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}/"* 2> /dev/null

    # Copy over new adapters
    echo "Copying new adapters..."
    cp -r "${IOS_ADAPTER_DIR}" "${PROJECT_PATH}/${IOS_EXPORT_DIR}"
    validate
    cp $ANDROID_ADAPTER_JAR "${PROJECT_PATH}/${ANDROID_EXPORT_DIR}/${SUPPORT_LIB_LOWERCASE}-${ANDROID_ADAPTER_VERSION}.jar"
    validate

    # Generate Unity package
    echo "Exporting Unity package..."
    echo "IOS_EXPORT_DIR: ${IOS_EXPORT_DIR}"
    DEST_PACKAGE="${OUT_DIR}/${SUPPORT_LIB}-Android.${ANDROID_ADAPTER_VERSION}-iOS.${IOS_ADAPTER_VERSION}.unitypackage"
    $UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile -exportPackage $IOS_EXPORT_DIR $ANDROID_EXPORT_DIR $DEST_PACKAGE
    validate
    echo "Exported ${SUPPORT_LIB} (iOS: ${IOS_EXPORT_DIR} | Android: ${ANDROID_EXPORT_DIR}) to ${DEST_PACKAGE}"
done

git add .
echo "DONE EXPORTING UNITY PACKAGES!"
echo "Test adapter updates via the sample app then commit and push these changes."
