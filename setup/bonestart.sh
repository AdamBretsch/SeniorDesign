#!/bin/bash

echo "Configuring for BeagleBone"
cd ~/SeniorDesign/setup
./ipMasquerade.sh enp0s3
./firstssh.sh
git config --global user.email "email@example.com"
git config --global user.name "Example Name"
