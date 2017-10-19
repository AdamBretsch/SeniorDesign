#!/bin/bash
echo "Installing vim, updating"
sudo apt install vim
sudo apt-get update
echo "Moving bonestart.sh to ~"
cp ~/SeniorDesign/setup/bonestart.sh ~/
