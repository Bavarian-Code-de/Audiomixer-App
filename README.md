
---

# Audiomixer‑App 🎛️

**WPF front-end to control an ESP32-based audio controller via serial port**

---

## 📌 Purpose

GUI tool for simple control of audio parameters (volume, mute, pan…) on an ESP32 audio controller via **COM port (serial)**.
(Arduino/ESP32 project in my Repositorys)

---

## 🧱 Technology Stack

* **Frontend**: C# WPF (.NET Core/Framework) with MVVM pattern
* **Communication**: `System.IO.Ports.SerialPort`
* **ESP32 Firmware**: e.g. 115200 baud, reads commands via UART

---

## ⚙️ Features

* Control volume, mute, and balance with sliders
* Automatic COM-port detection and connection
* Real-time feedback (e.g., level meters)
* Save/load presets (XML/JSON)
* Error messages if baud rate or port is invalid


---

## 🏛️ Architecture

```
+----------------+       +-------------------------+      +------------------+
|   WPF UI App   | <---> | Serial Communication    | <--->| ESP32 Audio Board |
+----------------+       +-------------------------+      +------------------+
```

* **UI**: Sliders and buttons bound to ViewModels
* **Serial Layer**: .NET’s `SerialPort`, typically at 115200 baud
* **ESP32**: listens on UART, controls DAC/I²S

---

## 🔧 Requirements

* Windows 10 or higher
* Visual Studio 2022+ with .NET Desktop development
* ESP32 board with serial-capable firmware (baud rate must match)
* USB cable for COM port connection

---

## 🚀 Installation & Build

1. Clone the repo:

   ```bash
   git clone https://github.com/Bavarian-Code-de/Audiomixer-App.git
   ```
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build and run the app

---

## ⚙️ Configuration

* **appsettings.json** or UI settings:

  ```json
  {
    "Serial": {
      "PortName": "COM5",
      "BaudRate": 115200,
      "DataBits": 8,
      "StopBits": 1,
      "Parity": "None"
    }
  }
  ```
* Or use the UI “Settings” tab to select COM port

---

## 🧩 Usage

1. Launch the app → choose COM port in Settings → click “Connect”
2. Adjust sliders for volume, balance, mute in real time
3. Watch for connection or baud-rate errors
4. Save and load presets

---

## 🔄 Coming Updates

* Auto-scan available ports (`SerialPort.GetPortNames()`)
* OLED Screens to display volume or icon
* Support for multiple ESP32 devices
* Lillygo T3 Touchscreen
* magnetic connection of OLEDs or Lillygo Screen
* Wireless Support

---

## 🛠️ Contributing

* **Bugs or feature requests** → open an Issue
* **Code contributions** → submit a PR to the `development` branch
* **Presets, UI themes & examples** are welcome

---

## 📄 License

MIT License – see `LICENSE` file - coming soon

---

## 📫 Contact

* Repository: Bavarian‑Code‑de/Audiomixer‑App
* Questions or support → open an Issue

---
