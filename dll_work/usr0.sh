#!/bin/bash
# toggles led usr0 
cd /sys/class/gpio/gpio60
if grep -q "0" value
 then 
  echo 1 > value
 else
  echo 0 > value
fi
echo "Teensy 1 toggled"

