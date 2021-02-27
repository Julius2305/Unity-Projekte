# XRaySimulation

Das Handbuch liefert eine Einführung und Beschreibung der Interaktionsmöglichkeiten zwischen Nutzer und der Röntgenstrahlen Simulation.  
Die in diesem Handbuch beschriebene Version der Röntgenstrahlen Simulation ist auf der dieser Arbeit beiliegenden DVD enthalten, oder unter https://github.com/Jakalama/XRaySimulation.  
Stellen Sie sicher, dass Sie die Unity Version 2019.3.0f5 und den Unity Hub installiert haben.  
In dem ersten Abschnitt werden die Möglichkeiten der Simulation beschrieben. Für eine weitere Erklärung dieser sind die späteren Abschnitte zu konsumieren.

## Beschreibung
Mit der Röntgenstrahlen Simulation ist es möglich virtuell und interaktiv die Tätigkeiten eines Kardiologen auszuüben.  
Der Fokus der Anwendung liegt auf der Röntgenstrahlen Behandlung eines virtuellen Patienten. Dazu finden Sie sich in einem Katheterlabor wider. Sie gehen durch das Labor, 
schalten den C-Bogen ein. Die Bestrahlung des Patienten beginnt. Doch nicht nur der Patient wird bestrahlt. Sie auch.
Der Patient emittiert eine Streustrahlung. Diese Streustrahlung wird Ihnen am Modell Ihres Arztes an-gezeigt. Dazu verfärbt sich Ihr Arzt von einem grellen Gelb 
allmählich über Grün, Blau und Lila zu einem kräftigen Rot-Ton. Aber natürlich nur, wenn Sie keine Strahlenschutzeinrichtungen zwischen sich und der Strahlung bringen.

## Schnellstartanleitung
Sie können die Simulation durch befolgen der nachfolgenden Schritte schnell und einfach beginnen.
1.	Öffnen Sie die Webseite https://github.com/Jakalama/XRaySimulation.
2.	Klicken Sie auf die Schaltfläche „Code“ und dort klicken Sie auf „Download ZIP“.
3.	Speichern Sie die .zip-Datei auf Ihrem Computer und entpacken Sie diese.
4.	Öffnen Sie den Unity Hub und klicken Sie unter „Projects“ auf die Schaltfläche „Add“.
5.	Navigieren Sie zu der von Ihnen entpackten .zip-Datei.
6.	Öffnen Sie den Ordner „XRaySimulation-master“ und den folgenden Ordner „XRaySimulation“.
7.	Klicken Sie nun auf „Ordner auswählen“.
8.	Wählen Sie nun im Unity Hub die Unity Version 2019.3.0f5 aus.
9.	Machen Sie einen Klick auf die Schaltfläche „XRaySimulation“. Nun öffnet sich der Unity Editor.
10.	Stellen Sie sicher, dass die Szene „Labor“ aktiv ist.
11.	Klicken Sie auf „Play“ am oberen Rand des Editors.
12.	Wählen Sie im Menü „Start“ aus.
13.	Bewegen Sie sich mit den Tasten W, A, S und D durch den Raum.
14.	Rotieren Sie die Kamera durch Bewegen der Maus.
Nun können Sie sich in direkte Sichtlinie zu dem Patienten bringen.
15.	Schalten Sie den C-Bogen ein durch Drücken der Leertaste ein.
16.	Beobachten Sie wie sich die Anzeige der „avg. Dose“ im UI verändert.

## Systemanforderungen
Es ist zu beachten, dass die hier genannten Systemanforderungen keineswegs dem bestmöglichen Fall für Ihre Hardware entsprechen. Die Anforderungen sind 
lediglich ein Richtwert und beziehen sich auf ein Minimum der erforderlichen Hardware auf der die Anwendung getestet wurde.  

|                      |                              |
| -------------------- | ---------------------------- |
| Betriebssystem       | Microsft Windows 10          |
| Prozesor             | Intel Core i5-8600k, 3,6 GHz |
| Systemspeicher       | 8 GB                         |
| Festplattenspeicher  | 4 GB                         |
| Grafikkarte          | Nvidea GeForce GTX 1070      |




## Tastenbefehle & Maussteuerung
|                                   |                               |
| --------------------------------- | ----------------------------- |
| Arzt bewegen                      | W/A/S/D                       |
| Kamera / Arzt rotieren            | Bewegung mit der Maus         |
| Third-Person-Kamera frei rotieren | SHIFT + Bewegung mit der Maus |
| Kamera-Perspektove wechseln       | TAB                           |
| Mesh-Ansicht rotieren             | Pfeiltasten                   |
| Strahlenquelle ein-/ausschalten   | Leertaste                     |
| Menü öffnen                       | P                             |
| Mit Einrichtung interagieren      | F                             |
| Patiententisch verstellen         | Bild AUF / Bild AB            |


## Das Menu
 
![Menü](/images/Start_View.PNG)
 
Das Menü bietet Ihnen diese Optionen:
-	START -	Mit Klicken auf Start starten Sie die Anwendung.
	Oder fortgesetzt, falls Sie die Anwendung aktuell pausiert haben.
-	RESTART - Mit der Restart-Option starten Sie die Simulation neu.
	Dadurch setzen Sie alle bisherigen Tätigkeiten auf den Ausgangszustand zurück
-	EXIT – Schließt die Anwendung.  

### Die Dosis einsehen
Sie können die Dosis welche der Arzt zu aktuellen Zeitpunkt abbekommen hat auf mehrere Arten ein-sehen.  

**Wichtig:** Die maximale Färbung des Arztes ist ein kräftiger Rot-Ton. Dieser entspricht einer örtlichen Dosis von 50 Gy. Dies entspricht etwa einer tödlichen Strahlungsdosis.

**Dosis Info Fenster**  

![Dosis Info Fenster](/images/DoseInfoView.PNG)

Das Dosis Info Fenster ist für Sie zu jedem Zeitpunkt sichtbar. Dort können Sie die Dosis einsehen  

1.	Neben dem Text „avg. Dose:“ finden Sie die aktuelle mittlere Dosis in Gray [Gy] die Ihr Arzt abbekommen hat.
2.	Über die Pfeiltasten drehen Sie die Mesh-Ansicht des Arztes. So kann der Arzt von allen Seiten betrachtet werden. 

**Live Ansicht des Arztes**  
Die Dosis ist für Sie zu jedem Zeitpunkt direkt an Ihrem Arzt sichtbar.  
Die Third-Person-Perspektive der Kamera bietet Ihnen die Möglichkeit die Strahlungsdosis außerhalb des Dosis Info Fensters zu begutachten.  

### Ein- und Ausschalten der Quelle
Sie können die Strahlenquelle mit der Leertaste ein- und ausschalten.  
Der aktuelle Zustand der Strahlenquelle ist im Dosis Info Fenster sichtbar.  
Neben dem Text „Source is:“ finden Sie den aktuellen Zustand der Strahlenquelle. Insgesamt gibt es zwei verschiedene Zustände. Die Quelle kann entweder aktiv (active) oder inaktiv (inactive) sein.  

![active Source](/images/DosisFensterBeiaktiverQuelle.PNG)

### Wie die Mesh-Ansicht rotiert wird
Sie können die Mesh-Ansicht Ihres Arztes mit den Pfeiltasten rotieren.  
Die Miniatur dreht sich bei drücken des linken Pfeils im Uhrzeigersinn. Wenn Sie die Pfeiltaste nach oben drücken dreht sich die Ansicht in Richtung der Füße des Arztes.  

**Wichtig:** Die Ansicht rotiert jederzeit um die Mitte des Arztes. Es ist nicht möglich den Arzt beliebig weit mit den Pfeiltasten nach oben und unten zu drehen.  

![left](/images/meshView_01.PNG)
![right](/images/MeshView_02.PNG)

### Interaktion mit der Einrichtung
Interagieren Sie mit einigen der Einrichtungsgegenstände.  
Die folgenden Einrichtungsgegenstände sind in dieser Simulation verfügbar:  
-	C-Bogen
-	Patiententisch
-	Fahrbarer Arbeitstisch
-	Fester Tisch
-	Strahlenschutzwand

Mit Ausnahme des festen Tisches kann mit allen Einrichtungsgegenständen interagiert werden.  
Nähern Sie sich dazu einem Einrichtungsgegenstand. Wenn sie nah genug herangegangen sind er-scheint in der oberen linken Ecke ein Einrichtungsgegenstand Info Fenster. Dieses zeigt Ihnen den Na-men, eine Beschreibung und die Tasten an mit denen Sie mit dem Gegenstand interagieren.

**Wichtig:** Da die Röntgenstrahlung einen zentralen Punkt der Simulation darstellt kann mit der Strahlen-quelle (der C-Bogen) jederzeit interagiert werden.  

In der folgenden Abbildung sehen Sie ein Beispiel des Einrichtungsgegenstand Info Fensters.  
In diesem ist der Inhalt für den Patiententisch sichtbar.  

![patientable](/images/patienttable_text.PNG)

Neben der Vergrößerung finden Sie die Bedeutung der einzelnen Textinhalte.
 

![patientable text](/images/patienttable_text - Kopie.PNG)

**Wichtig:** Nicht jeder Einrichtungsgegenstand besitzt Tastenbefehle. In diesem Fall bleibt der Bereich leer.  
Es ist auch nicht möglich, dass mehr als zwei Tastenbefehle angezeigt werden.

### Verändern der Szene
Die Katheterlabor Szene kann von Ihnen im Unity Editor verändert werden. Auch können Sie eine weitere Szene erstellen.  

**Wichtig:** Eine Szene muss für die Simulation der Röntgenstrahlen einige Elemente enthalten. Dazu muss in der Szenen Hierarchie das GUI Prefab, ein PauseMenu Prefab, ein Player Prefab, ein Eventsystem und das XRaySource Prefab enthalten sein.  

Eine Szene besitzt erst den vollen Funktionsumfang wenn zusätzlich alle Einrichtungsgegenstände enthalten sind. Sehen Sie dazu im Abschnitt Einrichtungsgegenstände.
Die Abbildung rechts zeigt dazu den Hierarchie-Baum der Labor Szene.

![patientable with patient](/images/hierarchy.PNG)


### Verändern von Einrichtungsgegenständen
Wenn Sie Eigenschaften eines Einrichtungsgegenstandes verändern wollen, dann müssen Sie dessen Prefab unter „Assets/Ressources/Prefabs“ öffnen. 
Die Furniture-Klasse befindet sich ebenso wie der Trigger-Collider dazu auf dem ersten GameObject. 
Im Inspector können Sie den Radius des Trigger-Colliders verändern. Damit erhöhen oder reduzieren Sie die Interaktionsreichweite. Weiter können Sie den Namen, die Beschreibung und die Tastenbefehle zum Interagieren hier ändern.  

**Wichtig:** Sie sollten nicht den Eintrag „Type“ ändern. Dies kann zu Fehlern während der Laufzeit der Anwendung führen.  

![patientable with patient](/images/addFurniture.PNG)


### Testen der Anwendung
Wenn Sie die Simulation in Ihrer Funktion anpassen oder erweitern wollen, ist es nötig dass Sie die Testfälle der Anwendung dementsprechend erweitern.  

#### Tests durchführen
Öffnen Sie den Unity Editor. Klicken Sie auf „Windows/General/Test Runner“. Nun sehen Sie das geöffnete „Test Runner“ Fenster. Hierin befinden sich alle existierenden Testfälle.  
Im Reiter „EditMode“ können Sie alle Unit-Tests einsehen. Unter dem Reiter „PlayMode“ finden Sie weitere Tests. Hier finden Sie alle Tests die als die mehr als nur eine Klasse testen. Klicken Sie auf einen der Tests wird dieser ausgeführt. Klicken Sie auf „Run All“ werden alle Tests ausgeführt.  

#### Tests hinzufügen
Haben Sie die Simulation erweitert und wollen neue Testfälle hinzufügen, so müssen Sie beachten in welchem Ordner Sie die Tests abspeichern.  
Ein Unit-Test kann nur unter „Assets/Editor/Tests“ abgespeichert werden. Während Sie einen Play-Mode Test unter „Assets/Scripts/PlayModeTest“ abspeichern müssen.
In den meisten Fällen fügen Sie einen Unit-Test hinzu. Dies ist der Fall, wenn Sie die Funktionen einer einzelnen Klasse testen wollen. Testen Sie hingegen ein Konstrukt aus mehreren Klassen, so sollten Sie einen Play-Mode Test hinzufügen.  

**Wichtig:** Sie sollten im Falle einer Änderung des Quellcodes immer alle Tests durchführen.  

**Wichtig:** Stellen Sie sicher, dass Sie die Tests in der Test-Szene durchführen. Diese finden Sie unter „Assets/Resources/Scenes/AutomaticTestScene“.


### Einrichtungsgegenstände
Nachfolgend finden Sie Abbildungen der Einrichtungsgegenstände.  

|                                   |                                                                   |
| --------------------------------- | ----------------------------------------------------------------- |
| C-Bogen mit Patiententisch        | ![patientable with patient](/images/CBogenMitPatientUndTisch.PNG) |
| Strahlenschutzwand                | ![protectionwall](/images/Strahlenschutzwand.PNG)                 |
| fester Tisch                      | ![table](/images/festerTisch.PNG)                                 |
| Fahrbarer Tisch                   | ![mobileTable](/images/mobiletable.PNG)                           |
| Tür                               | ![door](/images/Door.PNG)                                         |


