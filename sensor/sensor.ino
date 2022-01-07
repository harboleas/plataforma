// Sensor acelerometro triaxial para plataforma
#include <ADXL345.h>

#define ETAPAS_FILTRO 7

ADXL345 sensor;

int x, y, z;
// Para filtrar un poco (media movil)
int x0[ETAPAS_FILTRO], y0[ETAPAS_FILTRO], z0[ETAPAS_FILTRO];


void setup() 
{
  Serial.begin(115200);
  sensor.powerOn();
  sensor.setRangeSetting(2); 
}

void loop() 
{
  // Para sincronizar la lectura //
  while(!Serial.available());
  Serial.read();
  /////////////////////////////////
  sensor.readXYZ(x0, y0, z0);

  x = y = z = 0;

  for (int i = 0; i < ETAPAS_FILTRO; i++)
  {
    x += x0[i];
    y += y0[i];
    z += z0[i];   
  }
  x = x / ETAPAS_FILTRO;
  y = y / ETAPAS_FILTRO;
  z = z / ETAPAS_FILTRO;

  // Actualizacion de variables para el filtrado
  for (int i = ETAPAS_FILTRO - 1; i > 0; i--)
  {
    x0[i] = x0[i-1];
    y0[i] = y0[i-1];
    z0[i] = z0[i-1];   
  }

  Serial.print(x);
  Serial.print(",");
  Serial.print(y);
  Serial.print(",");
  Serial.println(z);
}
