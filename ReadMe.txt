# SeniorDesign
Senior design for team 14, Rose-Hulman, 2017

To setup, see Beaglebone guide google doc: 
https://docs.google.com/document/d/1Vx9MXgEDKVhbmT5UgRsDel0x_IPPHrwFCm7yPbT5DzI/edit?usp=sharing

# Directories
There are two main directories, ./dll_work and ./teensyTransfer.
./dll_work has all the files up to date. This is the working directory for all the latest code versions.
./teensyTransfer/pyWrite is used for the Python script to write information to used to transfer information between it and the DLL.
./setup is used for initial set up of the BeagleBone image.
./serial_work was used for debugging UART communication. It is not being used in the latest version.
./usb_work is left over work from trying to make the usb client on the BBB. It is out of date and not currently used.
exportGPIO.sh is a script which exports all the serial ports so that they are able to be controlled. This needs to be run to allow the GPIO ports to function properly
