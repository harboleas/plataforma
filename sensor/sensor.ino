// Sensor acelerometro triaxial para plataforma
#include <ADXL345.h>

ADXL345 sensor;
void setup() 
{
  Serial.begin(115200);
  sensor.powerOn();
  sensor.setRangeSetting(16);
}

void loop() 
{
  int x, y, z;
  // Para sinc //
  while(!Serial.available());
  Serial.read();
  ///////////////
  sensor.readXYZ(&x, &y, &z);
  Serial.print(x);
  Serial.print(",");
  Serial.print(y);
  Serial.print(",");
  Serial.println(z);
}
