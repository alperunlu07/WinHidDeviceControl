
String inputString = ""; 
bool stringComplete = false;   
int rPin = 4;
void setup() {
  Serial.begin(9600);
  pinMode(rPin,OUTPUT);
}

void loop() {
    if (stringComplete) {
      //Serial.println(inputString);
      if(inputString.indexOf("close")>-1){
        digitalWrite(rPin, HIGH);
      }else if(inputString.indexOf("open")>-1){
        digitalWrite(rPin, LOW);
      }
      inputString = "";
      stringComplete = false;
  }
}


void serialEvent() {
  while (Serial.available()) {
    char inChar = (char)Serial.read();
    inputString += inChar;
  

    if (inChar == '\n') {
      
      stringComplete = true;
    }
  }
}
