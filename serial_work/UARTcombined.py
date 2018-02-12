#!/usr/bin/env python
import Adafruit_BBIO.UART as UART
import serial
import thread
import time
import binascii
import keyboard

from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler
# Teensy 1 ----- 22, 0 ------ 21

class Watcher:
    DIRECTORY_TO_WATCH = "/home/debian/SeniorDesign/teensyTransfer/csWrite"
    
    def __init__(self):
        self.observer = Observer()
        
    def run(self):
        event_handler = Handler()
        self.observer.schedule(event_handler, self.DIRECTORY_TO_WATCH,recursive = True)
        print "watchdog started"
        self.observer.start()
        try:
            while True:
                message = raw_input("type in 'stop/pause watching' to stop \n" )
                
                if(message == "pause watching"):
                    self.observer.stop()
                    time.sleep(10)
                    self.observer.start()
                elif(message == "stop watching"):
                    self.observer.stop()
                    break
                    
        except KeyboardInterrupt:
                self.observer.stop()
                print "Exiting"
                
        self.observer.join()
        
    def stop(self):
        self.observer.stop()


class Handler(FileSystemEventHandler):
    @staticmethod
    def on_any_event(event):
        if(event.is_directory):
            return None;
        elif event.event_type == 'created' or event.event_type == 'modified' :
            print "Received changed event - %s." % event.src_path
            
            f = open(event.src_path,"r+")
            contents = f.readlines()
            for i in range(0,len(contents)):
                ser.write(contents[i])
                print contents[i]
            time.sleep(1)
        # writing the information to teensy
            
def serialReceive(ser):
    if ser.isOpen():
        print "Serial is Open!"
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

if __name__ == '__main__':
    UART.setup("UART2")
    ser = serial.Serial(port = "/dev/ttyS2", baudrate = 38400)
    w = Watcher()
    w.run()
    # thread.start_new_thread(serialReceive(),(ser))
    serialReceive(ser);
    
    # serial will only read information once but we can add a loop for that easily.
    
    
