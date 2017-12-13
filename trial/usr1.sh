#!/bin/bash
# toggles led usr1 
cd /sys/class/leds/beaglebone\:green\:usr1/
echo none > trigger
if grep -q "0" brightness
 then 
  echo 1 > brightness
 else
  echo 0 > brightness
fi
echo "LED usr0 toggled"

