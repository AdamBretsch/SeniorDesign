#!/usr/bin/env python3
import Adafruit_BBIO.GPIO as GPIO
import time, sys
     
GPIO.setup("P8_7", GPIO.OUT)
GPIO.setup("P8_9", GPIO.OUT)
GPIO.setup("P8_11", GPIO.OUT)
        
if (len(sys.argv) > 1)
	if (sys.arg[1] == "r")
		print("Flashing red")
		GPIO.output("P8_7", GPIO.HIGH)
		time.sleep(0.5)
		GPIO.output("P8_7", GPIO.LOW)
