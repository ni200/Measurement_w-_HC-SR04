# Measurement_with_HC-SR04
IoT - Measurement with a HC-SR04 sensor, plus optical options and database.

Auf unserem Landwirtschaftlichen Betrieb werden unter anderen auch Fütterungsversuche durchgeführt. Hierfür wird eine große Anzahl an Futterlagerhochsilos vorgehalten. Um die Tiere versorgen zu können, aber auch um kostenaufwendige Fütterungsversuchsreihen nicht
unterbrechen zu müssen, dürfen diese Silos nicht ungewollt leerfallen. Da sich der rechnerisch zu ermittelnde Verbrauch von dem Tatsächlichen Verbrauch abweichen kann, ist eine visuelle Kontrolle der Füllständeeine routinemäßige Notwendigkeit. Auch hierbei kann es jedes Mal zu Fehleinschätzungen durch den Mitarbeiter kommen. Eine automatisierte Füllstandmessung könnte hier Informationen zuverlässiger und Arbeitszeit liefern. 

Unser Lösungsanzatz ist es, die Messungen zu Automatisieren. Dies geschieht mit Hilfe eines Raspberry Pi 3 
und einem Ulatraschallsensor (HC-SR04). Der Ultraschallsensor misst Entfernungen. Wir befestigen den Sensor 
unter der Silodecke, sodass dieser die Entfernung zum Boden misst. Je nach Füllmenge gibt der Sensor eine 
Distanz zurück, die in der Software im Verhältniss zu der Silohöhe gesetzt werden soll, um den prozentualen Füllstand zu erhalten. Dieser Füllstand soll in einer Datenbank gespeichert werden um ihm später wieder abrufen zu können. Die Vergangenheitswerte können z.B. für Statistiken genutzt werden, die den Verlauf des Füllstands anzeigt. Desweiteren soll neben dem Sensor eine Warnlampe auf dem Silo angebracht werden, die bemerkbar macht, wenn ein bestimmter Meldebestand erreicht wird.  
Für den Versuchsaufbau verwenden wir eine Raspberry Pi 3 Model B V1.2 sowie den HCSR04 Ultraschallsensor. Für den Anschluss des Sensors an den Raspberry Pi sind zusätzlich ein 330 Ohm und ein 470 Ohm Wiederstand erforderlich. Die Simulation der Warnlampe übernimmt eine Led. Auf dem Raspberry Pi läuft das Betriebssystem „Windows 10 IoT Core“ und Programmiert wird mit Visual Studio Community 2015 in der Programmiersprache C#.

### Extensions
Diese Erweiterungen wurden für die Umsetzung das Projektes verwendet: 

**Erweiterung SDK**<br>
SQLite for Universal Windows Platform (3.18.0) <br>

**Referenzen**<br>
Microsoft Visual C++ 2013 Runtime Package for Windows Universal (14.0.0.0) <br>
Visual C++ 2015 Runtime for Universal Windows Platform Apps (14.0.0.0) <br>
SQLite for Universal Windows Platform (3.18.0.0) <br>
Windows IoT Extensions for the UWP (10.0.14393.0) <br>

**NuGet Pakete**<br>
Microsoft.NETCore.UniversalWindowsPlatform (v5.1.0) <br>
WinRTXamlToolkit.Controls.DataVisualization.UWP (v2.3.0) <br>
SQLite.Net-PCL (v3.1.1)<br>




 
