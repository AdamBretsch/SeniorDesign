#!/bin/bash
# toggles rgb leb
RED=false
BLUE=false
GREEN=false
for var in "$@"
do
 if [ "$var" == "r" ]; then
  RED=true
 fi
 if [ "$var" == "g" ]; then
  GREEN=true
 fi
 if [ "$var" == "b" ]; then
  BLUE=true
 fi
done

cd /sys/class/gpio

REDPIN=66
GREENPIN=69
BLUEPIN=45

if [ $RED == true ]; then
 DIR=gpio$REDPIN

 if [ ! -d $DIR ]; then
  echo $REDPIN  > export
 fi

 cd $DIR
 if ! grep -q "out" direction 
 then
  echo out > direction
 fi

 if grep -q "0" value
 then
   echo 1 > value
  else
   echo 0 > value
 fi
 cd ..
 echo "RBG LED red toggled"
fi

if [ $GREEN == true ]; then
 DIR=gpio$GREENPIN

 if [ ! -d $DIR ]; then
  echo $GREENPIN  > export
 fi

 cd $DIR
 if ! grep -q "out" direction 
 then
  echo out > direction
 fi

 if grep -q "0" value
 then
   echo 1 > value
  else
   echo 0 > value
 fi
 cd ..
 echo "RBG LED green toggled"
fi

if [ $BLUE == true ]; then
 DIR=gpio$BLUEPIN

 if [ ! -d $DIR ]; then
  echo $BLUEPIN  > export
 fi

 cd $DIR
 if ! grep -q "out" direction 
 then
  echo out > direction
 fi

 if grep -q "0" value
 then
   echo 1 > value
  else
   echo 0 > value
 fi
 cd ..
 echo "RBG LED blue toggled"
fi
