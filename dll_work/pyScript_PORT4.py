#!/usr/bin/env python
import Adafruit_BBIO.UART as UART
import serial
import thread
import time
import binascii
import os

from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

# Teensy port 1 0 ----- 13, 1 ---- 11



class Watcher:
    DIRECTORY_TO_WATCH = "/home/debian/SeniorDesign/teensyTransfer/csWrite/PORT_4"
    
    def __init__(self):
        self.observer = Observer()
        
    def run(self):
        event_handler = Handler()
        # flag = 0
        flag = True
        self.observer.schedule(event_handler, self.DIRECTORY_TO_WATCH,recursive = True)
        print "watchdog_port4 started"
        try: 
            self.observer.start()
            time.sleep(5)
            while flag:
                serialReceive(ser)
                time.sleep(40)

        except KeyboardInterrupt:
                self.observer.stop()
                print "Exiting"
                ser.close()
                
                
        self.observer.join()
        
        
class Handler(FileSystemEventHandler):
    @staticmethod
    def on_any_event(event):
        if(event.is_directory):
            return None;
        elif event.event_type == 'created' or event.event_type == 'modified' :
            print "Received changed event - %s." % event.src_path
            
            f = open(event.src_path,"r+")
            contents = f.readlines()
            for k in range(0,len(contents)):
                ser.write(contents[k])
            ser.close()
            ser.open()
            time.sleep(5)
        # writing the information to teensy
        
def serialReceive(ser):
    if ser.isOpen():
        ser.flush()
        out = ''
        hexout = ''
        flag = True;
        while ser.inWaiting() >= 1 and len(hexout) < 192:
            out = ser.read(2)
            hexout = hexout + out + "\n"

    f = open("/home/debian/SeniorDesign/teensyTransfer/pyWrite/PORT_4/DataFromTennsyFromPort4.txt","w")
    f.write(hexout + "\r\n")
    f.close()
    ser.close()
    ser.open()
    
if __name__ == '__main__':
    UART.setup("UART4")
    ser = serial.Serial(port = "/dev/ttyS4", baudrate = 38400)
    time.sleep(2)
    w = Watcher()
    w.run()
    ser.close()
    
    
    # serial will only read information once but we can add a loop for that easily.
    
    

