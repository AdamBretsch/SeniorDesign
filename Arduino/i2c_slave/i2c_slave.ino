 /* 
  *  Acts as i2c slave
  *  On bytes recieved: prints first 8 bytes to lcd
  *  
  *  On send request: sends 8 bytes "hello   "
  *  
*/

// include the library code:
#include <LiquidCrystal.h>
#include <Wire.h>

const int rs = 11, en = 10, d4 = 5, d5 = 4, d6 = 3, d7 = 2;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);
const int ledPin = 13;


void setup() {
  Wire.begin(9);                // join i2c bus with address #9
  Wire.onReceive(receiveEvent); // register event
  Wire.onRequest(requestEvent); // register event
  
  // set up the LCD's number of columns and rows:
  lcd.begin(16, 2);
  // Print a message to the LCD.
  lcd.print("i2c slave id: 9");
  // start led
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, HIGH);   // set the LED on
  delay(1000);                  // wait for a second
  digitalWrite(ledPin, LOW);    // set the LED off
}


elapsedMillis msUntilNextSend;
unsigned int packetCount = 0;

void loop() {
  delay(100);
}

// function that executes whenever data is received from master
// this function is registered as an event, see setup()
void receiveEvent(int howMany)
{
  int cursor = 0;
  digitalWrite(ledPin, HIGH);       // briefly flash the LED
  while(Wire.available() > 0) {  // loop through all
    if (!(cursor >= 16)){ // only prints first 16 characters to prevent lcd overflow
      char c = Wire.read();        // receive byte as a character
      lcd.setCursor(0, cursor+1);
      lcd.print(c);             // print the char
    }
  }
  lcd.setCursor(0, 1);
  digitalWrite(ledPin, LOW);
}


// function that executes whenever data is requested by master
// this function is registered as an event, see setup()
void requestEvent()
{
  digitalWrite(ledPin, HIGH);  // briefly flash the LED
  Wire.write("hello   ");     // respond with message of 8 bytes
                            // as expected by master
  digitalWrite(ledPin, LOW);
}
