Dokumentation Leveldatei:

gesamtAufbau:
[timer]
[impulse]
[Hebel 1]
[...]
[Hebel x]
[Schraube 1]
[Schraube 2]
[...]
[Schraube x]
[Lampe 1]
[Lampe 2]
[....]
[Lampe x]
[Leerzeile! wichtig]

Timeraufbau:
timer;[time]

time - Zeit in sekunden

impulseaufbau:
impulse;[time]

time - Impulszeit in Sekunden

hebelaufbau:
lever;[x];[y];[on];[orientation]

on - t = true; f = false
orientation - v = vertical, h = horizontal

Schraubenaufbau:
bolt;[x];[y];[typ]
x - x Koordinate
y - y Koordinate
typ - Schraubentyp: p = Kreuz, m = Schlitz

Lampenaufbau:
lamp;[x];[y];[typ];[farbe]{Schrauben}
typ - Lampentyp: w = working, b = broken
farbe - Farbe: r = red, y = yellow
{Schrauben} - Liste an verbundenen Schrauben; ";4;2" bedeutet, dass Schraube 4 und 2 mit der Lampe verbunden sind