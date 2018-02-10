#!/usr/bin/env bash
my_dir="$(dirname "$0")"

# Exports each of the third-party network adapters.
# NOTE: the mopub-unity-sdk must be cloned as a sibling of this repository.
# TODO: create unity project and copy files from submodules into correct directories.

OUT_DIR="`pwd`/unity-packages"
UNITY_BIN=/Applications/Unity/Unity.app/Contents/MacOS/Unity
DEST_PACKAGE="$OUT_DIR/$PACKAGE_NAME.unitypackage"
PROJECT_PATH="`pwd`/../mopub-unity-sdk/unity/MoPubUnityPlugin"


SUPPORT_LIBS=( "AdColony" "AdMob" "Chartboost" "Facebook" "UnityAds" "Vungle" )

for SUPPORT_LIB in "${SUPPORT_LIBS[@]}"
do
    IOS_EXPORT_FOLDERS_SUPPORT="Assets/MoPub/Editor/Support/$SUPPORT_LIB"
    ANDROID_EXPORT_FOLDERS_SUPPORT="Assets/Plugins/Android/mopub-support/libs/$SUPPORT_LIB"
    DEST_PACKAGE="$OUT_DIR/${SUPPORT_LIB}Support.unitypackage"

    $UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile $EXPORT_LOG -exportPackage $IOS_EXPORT_FOLDERS_SUPPORT $ANDROID_EXPORT_FOLDERS_SUPPORT $DEST_PACKAGE
    echo "Exported $SUPPORT_LIB (iOS: $IOS_EXPORT_FOLDERS_SUPPORT | Android: $ANDROID_EXPORT_FOLDERS_SUPPORT) to $DEST_PACKAGE"
done

# Millennial
MM_IOS_EXPORT_FOLDERS_SUPPORT="Assets/MoPub/Editor/Support/Millennial"
MM_ANDROID_EXPORT_FOLDERS_SUPPORT="Assets/Plugins/Android/mopub-support/libs/Millennial"
MM_ANDROID_SDK_FOLDER="Assets/Plugins/Android/mopub-support/libs/Millennial"
MM_DEST_PACKAGE="$OUT_DIR/MillennialSupport.unitypackage"

$UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile $EXPORT_LOG -exportPackage $MM_IOS_EXPORT_FOLDERS_SUPPORT $MM_ANDROID_EXPORT_FOLDERS_SUPPORT $MM_ANDROID_SDK_FOLDER $MM_DEST_PACKAGE
echo "Exported Millennial (iOS: $MM_IOS_EXPORT_FOLDERS_SUPPORT | Android: $MM_ANDROID_EXPORT_FOLDERS_SUPPORT | MM Android SDK: $MM_ANDROID_SDK_FOLDER) to $MM_DEST_PACKAGE"