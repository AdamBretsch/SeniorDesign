#!/usr/bin/env python
import Adafruit_BBIO.UART as UART
import serial
import time
import binascii

UART.setup("UART2")
# Teensy 1 ----- 22, 0 ------ 21
ser = serial.Serial(port = "/dev/ttyS2", baudrate = 38400)
ser.close()
ser.open()

values = ''.join(chr(x) for x in [0xE2,0x11,0x2,0x00,0x00,0x23])

f = open("FileToReadForPort2.txt","r+")
contents = f.readlines()





if ser.isOpen():
    print "Serial is Open!"
    i = 0;
    for i in range(0,6):
        ser.write(values[i])
    time.sleep(1)
    out = ''
    hexout = ''
   # while True:
    #    bytesToRead = ser.inWaiting()
     #   out = ser.read(bytesToRead)
      #  print out
       # break
    while ser.inWaiting() >= 1:
        out = ser.read(1)
        hexout = hexout + out
    print hexout
ser.close()

f = open("DataFromTennsyFromPort2.txt","a+")
f.write(hexout + "\r\n")
f.close()

