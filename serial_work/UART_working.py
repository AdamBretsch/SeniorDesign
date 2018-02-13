#!/usr/bin/env python
import Adafruit_BBIO.UART as UART
import serial
import time
import binascii

UART.setup("UART2")
ser = serial.Serial(port = "/dev/ttyS2", baudrate = 9600)
ser.close()
ser.open()

# Teensy 1 ----- 22, 0 ------ 21

values = ''.join(chr(x) for x in [0xE2,0x11,0x20,0x00,0x00,0x23])
if ser.isOpen():
    print "Serial is Open!"
    i = 0
    for i in range(0,6):
        ser.write(values[i])
    time.sleep(1)
    out = ''
    hexout = ''
    
    while ser.inWaiting() >= 1 and len(hexout) < 192:
        out = ser.read(2)
        hexout = hexout + out + "\n"
    print hexout
ser.close()
f = open("/home/debian/SeniorDesign/teensyTransfer/pyWrite/DataFromTennsyFromPort2.txt","a+")
f.write(hexout + "\r\n")
f.close()

