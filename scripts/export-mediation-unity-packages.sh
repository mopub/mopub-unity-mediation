#!/usr/bin/env bash
my_dir="$(dirname "$0")"
source "$my_dir/validate.sh"

# ==================================================== #
# Exports each of the third-party network adapters     #
# ==================================================== #

UNITY_BIN=/Applications/Unity/Unity.app/Contents/MacOS/Unity
PROJECT_PATH="`pwd`/unity-sample-app"
OUT_DIR="`pwd`/unity-packages"
SUPPORT_LIBS=( "AdColony" "AOL" "AdMob" "Chartboost" "AudienceNetwork" "UnityAds" "Vungle" )


# Generate Unity packages for all networks
for SUPPORT_LIB in "${SUPPORT_LIBS[@]}"
do
    echo "Processing ${SUPPORT_LIB}..."

    IOS_ADAPTER_FOLDER="mopub-ios-mediation/${SUPPORT_LIB}"
    IOS_EXPORT_FOLDER="Assets/MoPub/Editor/Support/${SUPPORT_LIB}"
    SUPPORT_LIB_LOWERCASE=`echo "${SUPPORT_LIB}" | tr '[:upper:]' '[:lower:]'`
    ANDROID_ADAPTER_JAR="mopub-android-mediation/libs/${SUPPORT_LIB_LOWERCASE}-*.jar"
    ANDROID_EXPORT_JAR="Assets/Plugins/Android/mopub-support/libs/${SUPPORT_LIB}/mopub-${SUPPORT_LIB_LOWERCASE}-custom-events.jar"

    # Delete existing adapters
    echo "Removing existing adapters..."
    rm -r $IOS_EXPORT_FOLDER 2> /dev/null
    rm $ANDROID_EXPORT_JAR 2> /dev/null

    # # Copy over new adapters
    echo "Copying new adapters..."
    cp -r "${IOS_ADAPTER_FOLDER}" "${PROJECT_PATH}/${IOS_EXPORT_FOLDER}"
    validate
    cp $ANDROID_ADAPTER_JAR "${PROJECT_PATH}/${ANDROID_EXPORT_JAR}"
    validate

    # Generate Unity package
    echo "Exporting Unity package..."
    echo "IOS_EXPORT_FOLDER: ${IOS_EXPORT_FOLDER}"
    DEST_PACKAGE="${OUT_DIR}/${SUPPORT_LIB}Support.unitypackage"
    $UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile -exportPackage ${IOS_EXPORT_FOLDER} $ANDROID_EXPORT_JAR $DEST_PACKAGE
    validate
    echo "Exported ${SUPPORT_LIB} (iOS: ${IOS_EXPORT_FOLDER} | Android: ${ANDROID_EXPORT_JAR}) to ${DEST_PACKAGE}"
done
