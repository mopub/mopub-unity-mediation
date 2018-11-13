#!/usr/bin/env bash
my_dir="$(dirname "$0")"
source "$my_dir/validate.sh"

# ==================================================== #
# Exports each of the third-party network adapters     #
# ==================================================== #

# NOTE: this script requires the mopub-unity-sdk repository to be present as a sibling of this one

# TODO: revert to root Unity
UNITY_BIN=/Applications/Unity/2018.2.3f1/Unity.app/Contents/MacOS/Unity

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
LOG_PREFIX="[MoPub Script] "

# Check sample app under mopub-unity-sdk exists
ls $PROJECT_PATH 1> /dev/null
validate "Unity SDK sample app not found; is the mopub-unity-sdk repository a sibling of this one?\nExpected it to be at: ${PROJECT_PATH}"

# Display welcome/confirmation message
# Title generated by: http://patorjk.com/software/taag/ (Small and Speed)
WELCOME="
  __  __     ___      _      __  __        _ _      _   _          
 |  \/  |___| _ \_  _| |__  |  \/  |___ __| (_)__ _| |_(_)___ _ _  
 | |\/| / _ \  _/ || | '_ \ | |\/| / -_) _\` | / _\` |  _| / _ \ ' \ 
 |_|  |_\___/_|  \_,_|_.__/ |_|  |_\___\__,_|_\__,_|\__|_\___/_||_| 
_____  __      __________             __________                           _____ 
__  / / /_________(_)_  /_____  __    ___  ____/___  ________________________  /_
_  / / /__  __ \_  /_  __/_  / / /    __  __/  __  |/_/__  __ \  __ \_  ___/  __/
/ /_/ / _  / / /  / / /_ _  /_/ /     _  /___  __>  < __  /_/ / /_/ /  /   / /_  
\____/  /_/ /_//_/  \__/ _\__, /      /_____/  /_/|_| _  .___/\____//_/    \__/  
                         /____/                       /_/                       
"
echo "${WELCOME}"
printf "This script will generate a new Unity package for the desired Network.\n\n"
echo "Select one of the options below to continue..."

PS3='Your choice: '
options=(
    "Update Android and iOS Mediation submodules and continue"
    "Continue without updating submodules"
)
select opt in "${options[@]}" "Abort and exit"; do
    case "$REPLY" in
        1 ) printf "\n${LOG_PREFIX}Updating submodules...\n"; break;;
        2 ) printf "\nContinuing without updating submodules...\n"; break;;
        $(( ${#options[@]}+1 )) ) echo "Goodbye!"; exit 0;;
        *) echo "Invalid option. Try another one.";continue;;
    esac
done

if [ $REPLY == 1 ]; then
    # Update mediation submodules
    cd $IOS_MEDIATION_DIR
    git checkout master
    git pull origin master
    cd ..
    cd $ANDROID_MEDIATION_DIR
    git checkout master
    git pull origin master
    cd ..
    printf "${LOG_PREFIX}...done updating submodules!\n"
fi

printf "\nWhich Network do you want to update?\n"
select opt in "${SUPPORT_LIBS[@]}"; do
    if [ $opt ]; then
        NETWORK=$opt
        printf "\nSelected Network: ${NETWORK}\n";break;
    else
        echo "Invalid option. Try another one.";continue;
    fi
done

# Gather necessary values
NETWORK_ADAPTERS_NAME="MoPub-${NETWORK}-Adapters"
NETWORK_ADAPTERS_NAME_LOWERCASE=`echo "${NETWORK_ADAPTERS_NAME}" | tr '[:upper:]' '[:lower:]'`
IOS_ADAPTER_DIR="${IOS_MEDIATION_DIR}/${NETWORK}"
IOS_EXPORT_DIR="${MOPUB_MEDIATION_UNITY_ROOT}/${NETWORK}/Adapter/iOS"
IOS_PODSPEC_FILE="${IOS_ADAPTER_DIR}/MoPub-${NETWORK}-PodSpecs/${NETWORK_ADAPTERS_NAME}.podspec"
IOS_ADAPTER_VERSION=`less $IOS_PODSPEC_FILE | grep s.version | sed "s/^.*'\([.0-9]*\)'.*/\1/"`
IOS_SDK_VERSION=`echo ${IOS_ADAPTER_VERSION%.*}`
ANDROID_ADAPTER_JAR="${ANDROID_MEDIATION_DIR}/libs/${NETWORK_ADAPTERS_NAME_LOWERCASE}-*.jar"
ANDROID_EXPORT_DIR="${MOPUB_MEDIATION_UNITY_ROOT}/${NETWORK}/Adapter/Android"
ANDROID_ADAPTER_VERSION=`echo $ANDROID_ADAPTER_JAR | sed "s/^.*${NETWORK_ADAPTERS_NAME_LOWERCASE}-\([.0-9]*[^\.jar]\).*/\1/"`
ANDROID_SDK_VERSION=`echo ${ANDROID_ADAPTER_VERSION%.*}`
UNITY_SCRIPTS_DIR="${UNITY_MEDIATION_DIR}/${NETWORK}"
UNITY_ADAPTER_CONFIG_FILE="${UNITY_SCRIPTS_DIR}/${NETWORK}AdapterConfig.cs"
UNITY_ADAPTER_DEPS_FILE="${UNITY_SCRIPTS_DIR}/${NETWORK}Dependencies.xml"
UNITY_ADAPTER_VERSION=`less $UNITY_ADAPTER_CONFIG_FILE | grep string\ _version | sed "s/^.*\"\([.0-9]*\)\".*/\1/"`
UNITY_SCRIPTS_EXPORT_DIR="${MOPUB_MEDIATION_UNITY_ROOT}/${NETWORK}/Editor"

if [ -z "$UNITY_ADAPTER_VERSION" ]; then
    echo "FATAL: Unable to read current adapter version from ${UNITY_ADAPTER_CONFIG_FILE}"
    echo "Aborting!"
    exit 1
fi

echo "Current adapter versions:"
printf "> Android:  ${ANDROID_ADAPTER_VERSION}\n"
printf "> iOS:      ${IOS_ADAPTER_VERSION}\n"
printf "> Unity:    ${UNITY_ADAPTER_VERSION}\n"
echo "Which Unity adapter version are you exporting now?"
read -p "New version: " new_version
if [[ $new_version != [.0-9]* ]]; then
    echo "Invalid version number! Aborting."
    exit 1
fi
UNITY_ADAPTER_VERSION=$new_version

printf "\nAbout to export new Unity package for ${NETWORK} with version ${UNITY_ADAPTER_VERSION}\n"
read -p "Press 'Enter' to continue or 'Ctrl-C' to abort."

# Delete existing packages
rm $OUT_DIR/*$NETWORK*

# Update Adapter and SDK version numbers
# TODO: update dependency versions
# sed -i "" -e "s/\"[.0-9]*\"/\"${IOS_SDK_VERSION}\"/g" $UNITY_IOS_SDK_VERSION_FILE
# sed -i "" -e "s/\"[.0-9]*\"/\"${ANDROID_SDK_VERSION}\"/g" $UNITY_ANDROID_SDK_VERSION_FILE

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
cp $UNITY_SCRIPTS_DIR/${NETWORK}Dependencies.xml "${PROJECT_PATH}/${UNITY_SCRIPTS_EXPORT_DIR}/"
validate

# Generate Unity package
echo "Exporting Unity package..."
DEST_PACKAGE="${OUT_DIR}/${NETWORK_ADAPTERS_NAME}-${UNITY_ADAPTER_VERSION}.unitypackage"
$UNITY_BIN -projectPath $PROJECT_PATH -quit -batchmode -logFile -exportPackage $IOS_EXPORT_DIR $ANDROID_EXPORT_DIR $UNITY_SCRIPTS_EXPORT_DIR $DEST_PACKAGE
validate
echo "Exported ${NETWORK_ADAPTERS_NAME} (iOS: ${IOS_EXPORT_DIR} | Android: ${ANDROID_EXPORT_DIR} | Unity: ${UNITY_SCRIPTS_EXPORT_DIR}) to ${DEST_PACKAGE}"

git add .
GREEN='\033[0;32m'
NC='\033[0m' # No Color
echo -e "${GREEN}DONE EXPORTING UNITY PACKAGES!${NC}"
echo "Test adapter updates via the updated sample app (under mopub-unity-sdk/) then commit and push these changes."
