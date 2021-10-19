#include <Wire.h>

const int Address = 0x68; // dirección I2C de MPU-6050
int button = 7;
int buttonval;
void setup() {
  Serial.begin(115200); // Inicializa el puerto serie
  Wire.begin(); // Inicializa la biblioteca Wire
  WriteMPUReg(0x6B, 0); // Iniciar MPU-6050
  
  pinMode(button, INPUT);
}

void loop()
{
  Wire.write(0x00);
  buttonval = digitalRead(button);
  Serial.write(buttonval);
  Wire.write(0x6B);
  byte date[16]; // 14 + 2 = 16 (14: datos, 2: separador AA AA)
  ReadAccGyr(date); // Leer el valor medido
  
  for (int i = 0; i < 16; i++) {
    Serial.write(date[i]);
  }
  
}

void WriteMPUReg(int nReg, unsigned char nVal) {
  Wire.beginTransmission(Address);
  Wire.write(nReg);
  Wire.write(nVal);
  Wire.endTransmission(true);
}

// Leer un byte de datos de MPU-6050
// Especifique la dirección del bloc de notas y devuelva un valor de Byte
unsigned char ReadMPUReg(int nReg) {
  Wire.beginTransmission(Address);
  Wire.write(nReg);
  Wire.requestFrom(Address, 1, true);
  Wire.endTransmission(true);
  return Wire.read();
}

// Secuencia de lectura (14 artículos):
// [
// Acc_X_H, Acc_X_L, Acc_Y_H, Acc_Y_L, Acc_Z_H, Acc_Z_L,
// Temp_H, Temp_L
// Gyro_X_H, Gyro_X_L, Gyro_Y_H, Gyro_Y_L, Gyro_Z_H, Gyro_Z_L
// ]

// Leer tres valores de acelerómetro, valores de temperatura, tres valores de velocidad angular de MPU-6050
// guardar en la matriz especificada
void ReadAccGyr(byte *pVals) {
  Wire.beginTransmission(Address);
  Wire.write(0x3B);
  Wire.requestFrom(Address, 14, true);
  Wire.endTransmission(true);

  for (int i = 0; i < 14; i++) {
    pVals[i] = Wire.read();
  }

  pVals[14] = 170; // AA
  pVals[15] = 170; // AA
}
