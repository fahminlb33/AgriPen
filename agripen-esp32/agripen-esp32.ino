/* 
  AgriPen ESP-32

  => Board
  DOIT ESP32 DEVKIT V1
  https://espressif.github.io/arduino-esp32/package_esp32_index.json

  => Included Libraries
  - Adafruit Unified Sensor v1.1.9
  - DHT sensor library v1.4.4

  => Data Format
  Terminators:
  B    = begin
  S    = stop
  CRLF = new line (\r\n)

  Format:
  <terminator: begin>,<code_1>:<data_1>[,<code_n>:<data_n>],<terminator: stop><CRLF>

  Valid Codes:
  - air_temp = air temperature
  - air_humi = air humidity
  - air_heat = air heat index
  - soil_moi = soil moisture
  - sun_illu = sunlight illumination

  Example:
  B,air_temp:25.00,air_humi:50.00,air_heat:25.00,soil_moi:50.00,sun_illu:50.00,S<CRLF>
*/

#include "DHT.h"
#include "BluetoothSerial.h"

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

#if !defined(CONFIG_BT_SPP_ENABLED)
#error Serial Bluetooth not available or not enabled. It is only available for the ESP32 chip.
#endif

#define ADC_MAX          4095
#define ADC_MIN          0

#define BAUD_RATE        115200

// define GPIO pins
// BLACK   = LDR
// RED     = DHT
// YELLOW  = SOIL MOISTURE
#define DHT_TYPE          DHT11
#define LDR_PIN           27
#define DHT_PIN           26
#define SOIL_MOISTURE_PIN 25

DHT dht(DHT_PIN, DHT_TYPE);
BluetoothSerial serialBT;

void sendTerminator(bool start = true) {
  if (start) {
    serialBT.print("B,");

    Serial.println("");
    Serial.println(F("---- START MEASUREMENTS"));
  } else {
    serialBT.println("S");
    
    Serial.println(F("---- END MEASUREMENTS"));
  }
}

void send(char* code, float data) {
  // print to bluetooth serial
  serialBT.print(code);
  serialBT.print(":");
  serialBT.print(data, 2);
  serialBT.print(",");

  // print to USB serial
  Serial.print(code);
  Serial.print(F("    : "));
  Serial.println(data, 2);
}

void blink() {
  digitalWrite(LED_BUILTIN, HIGH);
  delay(250);
  digitalWrite(LED_BUILTIN, LOW);
}

void setup() {
  // initialize pins
  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(LDR_PIN, INPUT);
  pinMode(SOIL_MOISTURE_PIN, INPUT);

  // initialize serial
  Serial.begin(BAUD_RATE);
  serialBT.begin("AgriPen", false);

  // initialize DHT
  dht.begin();

  // READY!
  Serial.print(F("AgriPen ready to pair!"));
}

void loop() {
  // BEGIN message
  sendTerminator(true);

  // --- read from DHT
  auto air_h = dht.readHumidity();
  auto air_t = dht.readTemperature();
  if (isnan(air_h) || isnan(air_t)) {
    Serial.println(F("Failed to read from DHT sensor!"));
    return;
  }

  auto air_hic = dht.computeHeatIndex(air_t, air_h, false);
  send("air_temp", air_h);
  send("air_humi", air_t);
  send("air_heat", air_hic);

  // --- read from capacitive soil moisture
  float soil_h = map(analogRead(SOIL_MOISTURE_PIN), ADC_MIN, ADC_MAX, 100, 0); 
  send("soil_moi", soil_h);

  // --- read from LDR
  float ldr = map(analogRead(LDR_PIN), ADC_MIN, ADC_MAX, 100, 0); 
  send("sun_illu", ldr);

  // END message
  sendTerminator(false);

  // blink to indicate successful iteration
  blink();

  // delay for a second
  delay(1000);
}
