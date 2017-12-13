#!/usr/bin/env python
import usb.core
import usb.util

# find our device
# usb drive
#dev = usb.core.find(idVendor=0x0781, idProduct=0x5583)
# mouse dongle
dev = usb.core.find(idVendor=0x046d, idProduct=0xc52f)
# medical device
#dev = usb.core.find(idVendor=0x173a, idProduct=0x21d5)

# was it found?
if dev is None:
    raise ValueError('Device not found')
interface = 0
if dev.is_kernel_driver_active(interface) is True:
  # tell the kernel to detach
  dev.detach_kernel_driver(interface)
  # claim the device
  usb.util.claim_interface(dev, interface)

# get an endpoint instance
cfg = dev.get_active_configuration()
intf = cfg[(0,0)]

ep = usb.util.find_descriptor(
    intf,
    # match the first OUT endpoint
    custom_match = \
    lambda e: \
        usb.util.endpoint_direction(e.bEndpointAddress) == \
        usb.util.ENDPOINT_OUT)

print(ep)
#print(dev.get_active_configuration())
assert ep is not None

# write the data
print("attemping to write to device")
ep.write('test')
