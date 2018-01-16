 #include <LiquidCrystal.h>

 const int rs = 11, en = 10, d4 = 5, d5 = 4, d6 = 3, d7 = 2;
 LiquidCrystal lcd(rs, en, d4, d5, d6, d7);
byte data[64] = {0xE2,0x00,0x00,0x32,0x80,0x00,0x00,0x00,
                 0x00,0x10,0x02,0xA5,0x07,0x90,0x02,0x68,
                 0x00,0x00,0x00,0x08,0x00,0x08,0x00,0x00,
                 0x00,0x00,0x00,0x00,0x00,0x00,0x08,0x00,
                 0x00,0x00,0x00,0x80,0x06,0x01,0x90,0x00,
                 0x13,0x63,0xB3,0x24,0x00,0x00,0x00,0x10,
                 0x10,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
//added 11 0s to 64  
 const int ledPin = 13;
void setup() {
  lcd.begin(16, 2);
  // Print a message to the LCD.
  lcd.print("Association!");
  
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, HIGH);   // set the LED on
  delay(1000);                  // wait for a second
  digitalWrite(ledPin, LOW);    // set the LED off
   
//byte bigarray[64];
//lcd.setCursor(0, 1);
//int h = 0;
//for (int i=0; i<=16; i=i+8){
//  String submes = message.substring(i,i+8);
//  //lcd.print(submes);
//  byte smallarray[4];
//  char *hexstring = const_cast<char*>(submes.c_str());
//  // *hexstring = "b4af981a";
//  unsigned long number = strtoul( hexstring, nullptr, 16);
//  for(int i=3; i>=0; i--)    // start with lowest byte of number
//  {
//    smallarray[i] = number & 0xFF;  // or: = byte( number);
//    number >>= 8;            // get next byte into position
//  }
//  
//  memcpy(bigarray+h,smallarray,4);
//  h = h+4;
//}

// Use 'nullptr' or 'NULL' for the second parameter 
 // byte test[4]={0xb4,0xaf,0x98,0x1a};
 
   lcd.setCursor(0, 1);
//  // lcd.print("0x");
   for(int i=0; i<15; i++)
  {
       if (data[i] < 0x10){
         lcd.print("0");
         lcd.print(data[i], HEX);
       }else {
          lcd.print(data[i], HEX);
       }
  }
}

void loop() {
  // put your main code here, to run repeatedly:
    delay(100);
}

