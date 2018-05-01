#!/bin/bash
# Startup script for mediacal device emulation
echo 60 > /sys/class/gpio/export
echo out > /sys/class/gpio/gpio60/direction

echo 48 > /sys/class/gpio/export
echo out > /sys/class/gpio/gpio48/direction

echo 49 > /sys/class/gpio/export
echo out > /sys/class/gpio/gpio49/direction

echo 115 > /sys/class/gpio/export
echo out > /sys/class/gpio/gpio115/direction

