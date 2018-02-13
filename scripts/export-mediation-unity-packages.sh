#!/usr/bin/env bash
my_dir="$(dirname "$0")"
source "$my_dir/validate.sh"

# ==================================================== #
# Exports each of the third-party network adapters     #
# ==================================================== #

OUT_DIR="`pwd`/unity-packages"
UNITY_BIN=/Applications/Unity/Unity.app/Contents/MacOS/Unity
DEST_PACKAGE="$OUT_DIR/$PACKAGE_NAME.unitypackage"
PROJECT_PATH="`pwd`/unity-sample-app"
SUPPORT_LIBS=( "AdColony" )
#"AdMob" "Chartboost" "Facebook" "UnityAds" "Vungle" )


# Generate Unity packages for all networks except Millennial
for SUPPORT_LIB in "${SUPPORT_LIBS[@]}"
do
    SUPPORT_LIB_LOWERCASE=`echo "$SUPPORT_LIB" | tr '[:upper:]' '[:lower:]'`
    IOS_ADAPTER_FOLDER="mopub-ios-mediation/$SUPPORT_LIB"
    ANDROID_ADAPTER_JAR="mopub-android-mediation/libs/$SUPPORT_LIB_LOWERCASE-*.jar"
    IOS_EXPORT_FOLDER="$PROJECT_PATH/Assets/MoPub/Editor/Support/$SUPPORT_LIB"
    ANDROID_EXPORT_JAR="$PROJECT_PATH/Assets/Plugins/Android/mopub-support/libs/mopub-$SUPPORT_LIB_LOWERCASE-custom-events.jar"

    # Delete existing adapters
    rm -r $IOS_EXPORT_FOLDER 2> /dev/null
    rm -r $ANDROID_EXPORT_FOLDER 2> /dev/null

    # Copy over new adapters
    cp -r "$IOS_ADAPTER_FOLDER" "$IOS_EXPORT_FOLDER"
    validate
    cp $ANDROID_ADAPTER_JAR $ANDROID_EXPORT_JAR
    validate

    # Generate Unity package
    # DEST_PACKAGE="$OUT_DIR/${SUPPORT_LIB}Support.unitypackage"
    # $UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile $EXPORT_LOG -exportPackage $IOS_EXPORT_FOLDER $ANDROID_EXPORT_FOLDER $DEST_PACKAGE
    # echo "Exported $SUPPORT_LIB (iOS: $IOS_EXPORT_FOLDER | Android: $ANDROID_EXPORT_FOLDER) to $DEST_PACKAGE"
done


# Generate Unity package for Millennial
# MM_IOS_EXPORT_FOLDER="Assets/MoPub/Editor/Support/Millennial"
# MM_ANDROID_EXPORT_FOLDER="Assets/Plugins/Android/mopub-support/libs/Millennial"
# MM_ANDROID_SDK_FOLDER="Assets/Plugins/Android/mopub-support/libs/Millennial"
# MM_DEST_PACKAGE="$OUT_DIR/MillennialSupport.unitypackage"
# $UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile $EXPORT_LOG -exportPackage $MM_IOS_EXPORT_FOLDER $MM_ANDROID_EXPORT_FOLDER $MM_ANDROID_SDK_FOLDER $MM_DEST_PACKAGE
# echo "Exported Millennial (iOS: $MM_IOS_EXPORT_FOLDER | Android: $MM_ANDROID_EXPORT_FOLDER | MM Android SDK: $MM_ANDROID_SDK_FOLDER) to $MM_DEST_PACKAGE"
