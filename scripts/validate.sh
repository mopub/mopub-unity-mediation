#!/usr/bin/env bash

# Ensures the previous command completed successfully otherwise prints given message and exits
function validate {
  if [[ $? -ne 0 ]]; then
    RED='\033[0;31m'
    NC='\033[0m' # No Color
    # Use argument as message or default message if no argument was given
    msg=${1:-Something went wrong, scroll up for details.}
    echo -e "\n${RED}FAILED:" $msg"${NC}\n"
    exit 1
  fi
}

# Ensures the script is being run on the project root
function ensure_project_root {
  ls unity-packages > /dev/null 2>&1 /dev/null
  validate "Ensure this script is running from the project root directory."
}

# Scripts that use validate.sh should run from the root directory
ensure_project_root
