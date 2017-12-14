#!/usr/bin/python

import sys
import usb.core
import usb.util
import time

# medical device
#dev = usb.core.find(idVendor=0x173a, idProduct=0x21d5)
# mouse
dev = usb.core.find(idVendor=0x046d, idProduct=0xc52f)
endpoint = dev[0][(0,0)][0]
# get an endpoint instance
cfg = dev.get_active_configuration()
intf = cfg[(0,0)]
#print("intf: "+intf)
interface = 0
#endpoint = usb.util.find_descriptor(
 #   intf,
    # match the first OUT endpoint
  #  custom_match = \
   # lambda e: \
    #    usb.util.endpoint_direction(e.bEndpointAddress) == \
     #   usb.util.ENDPOINT_OUT)

print(endpoint)
#print(dev.get_active_configuration())
assert endpoint is not None


# if the OS kernel already claimed the device, which is most likely true
# thanks to http://stackoverflow.com/questions/8218683/pyusb-cannot-set-configuration

if dev.is_kernel_driver_active(interface) is True:
  print("detaching kernal driver")
  # tell the kernel to detach
  dev.detach_kernel_driver(interface)
  # claim the device
  usb.util.claim_interface(dev, interface)

text_file = open("usbOut.txt", "w")
collected = 0
goal = 50
while collected < goal :
    try:
        data = dev.read(endpoint.bEndpointAddress,endpoint.wMaxPacketSize)
        collected += 1
        hexData = ""
        for val in data:
            hexData += str(hex(val)[2:]) + " "
        text_file.write(hexData[:-1]+"\n")
        print hexData
    except usb.core.USBError as e:
        data = None
        if e.args == ('Operation timed out',):
            continue

text_file.close()
# release the device
usb.util.release_interface(dev, interface)

# reattach the device to the OS kernel
#dev.attach_kernel_driver(interface)
