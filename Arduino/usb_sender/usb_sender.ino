 /* Basic Raw HID Example
   Teensy can send/receive 64 byte packets with a
   dedicated program running on a PC or Mac.

   You must select Raw HID from the "Tools > USB Type" menu

   Optional: LEDs should be connected to pins 0-7,
   and analog signals to the analog inputs.

   This example code is in the public domain.
*/

// include the library code:
#include <LiquidCrystal.h>

const int rs = 11, en = 10, d4 = 5, d5 = 4, d6 = 3, d7 = 2;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);
const int ledPin = 13;

void setup() {
  
  // set up the LCD's number of columns and rows:
  lcd.begin(16, 2);
  // Print a message to the LCD.
  lcd.print("USB Data:");
  // start led
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, HIGH);   // set the LED on
  delay(1000);                  // wait for a second
  digitalWrite(ledPin, LOW);    // set the LED off
}

// RawHID packets are always 64 bytes
byte buffer[64];
elapsedMillis msUntilNextSend;
unsigned int packetCount = 0;

void loop() {
  int n;
  byte sent;
  n = RawHID.recv(buffer, 0); // 0 timeout = do not wait
  if (n > 0) {
    // the computer sent a message.  Display the bits
    // of the first byte on pin 0 to 7.  Ignore the
    // other 63 bytes!
    digitalWrite(ledPin, HIGH);   // set the LED on
    delay(100);                  // wait for a second
    digitalWrite(ledPin, LOW);    // set the LED off
    sent = buffer[0];
    lcd.setCursor(0, 1);
    lcd.print(buffer[0]);
  }
  // every 2 seconds, send a packet to the computer
  if (msUntilNextSend > 2000) {
    msUntilNextSend = msUntilNextSend - 2000;
    // first 2 bytes are a signature
    buffer[0] = 0xAB;
    buffer[1] = 0xCD;
    buffer[3] = sent;
    
    String data =  "E2 00 00 32 80 00 00 00 00 01 00 2A 50 79 00 26 80 00 00 00 80 00 80 00 00 00 00 00 00 00 00 80 00 00 00 08 00 60 19 00 01 36 3B 32 40 00 00 01 01 00 00 00 00 00";
    int b = 0;
    for (int i=0; i < data.length(); i=i+3){
      String dataAt = data.substring(i,i+1);
      buffer[b] = dataAt.toInt();
      b++;
    }
    // and put a count oflkjuuu packets sent at the end
    buffer[62] = highByte(packetCount);
    buffer[63] = lowByte(packetCount);
    // actually send the packet
    n = RawHID.send(buffer, 100);
    if (n > 0) {
      packetCount = packetCount + 1;
    } 
  }
}
