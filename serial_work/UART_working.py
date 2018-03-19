#!/usr/bin/env python
import Adafruit_BBIO.UART as UART
import serial
import time
import binascii



ser1 = serial.Serial(port = "/dev/ttyS1", baudrate = 9600)
ser2 = serial.Serial(port = "/dev/ttyS2", baudrate = 9600)
ser3 = serial.Serial(port = "/dev/ttyO4", baudrate = 9600)

# Teensy port 2 0 ------ 21, 1 ---- 22
# Teensy port 1 0 ----- 24, 1 ---- 26
# Teensy port 4 0 -----13 , 1 ---- 11




values_PORT2 = ''.join(chr(x) for x in [0xE2,0x11,0x20,0x00,0x00,0x23])
values_PORT1 = ''.join(chr(x) for x in [0x23,0x00,0x00,0x20,0x11,0xE2])
values_PORT3 = ''.join(chr(x) for x in [0x78,0xf0,0x00,0x00,0x9c,0xE2])


if ser1.isOpen():
    print "Serial port 1 is Open!"
    i = 0
    for i in range(0,6):
        ser1.write(values_PORT1[i])
    time.sleep(1)
    
    out = ''
    hexout = ''
    
    while ser1.inWaiting() >= 1 and len(hexout) < 192:
        out = ser1.read(2)
        hexout = hexout + out + "\n"
    print hexout

f = open("/home/debian/SeniorDesign/teensyTransfer/pyWrite/DataFromTennsyFromPort1.txt","a+")
f.write(hexout + "\r\n")
f.close()

if ser2.isOpen():
    print "Serial port 2 is Open!"
    i = 0
    for i in range(0,6):
        ser2.write(values_PORT2[i])
    time.sleep(1)
    
    out = ''
    hexout = ''
    
    while ser2.inWaiting() >= 1 and len(hexout) < 192:
        out = ser2.read(2)
        hexout = hexout + out + "\n"
    print hexout

f = open("/home/debian/SeniorDesign/teensyTransfer/pyWrite/DataFromTennsyFromPort2.txt","a+")
f.write(hexout + "\r\n")
f.close()

if ser3.isOpen():
    print "Serial port 3 is Open!"
    i = 0
    for i in range(0,6):
        ser3.write(values_PORT3[i])
    time.sleep(1)
    
    out = ''
    hexout = ''
    
    while ser3.inWaiting() >= 1 and len(hexout) < 192:
        out = ser3.read(2)
        hexout = hexout + out + "\n"
    print hexout

f = open("/home/debian/SeniorDesign/teensyTransfer/pyWrite/DataFromTennsyFromPort3.txt","a+")
f.write(hexout + "\r\n")
f.close()