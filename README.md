# FileCheckApp
Je pouzita standardni sablona.Zaslouzila by si vycistit od nepotrebnych veci.
Apka drzi pouze posledni zmeneny stav pro kazdy dotazovany adresar. Verze se drzi zvlast pro kazdy dotazovany adresar nikoli pro soubor globalne.
data se drzi v jednom Json objektu ulozenem do txt souboru. konflikty v pristupech extra neresim, asi by davalo smysl ho zamknout pred porovnanim a pak teprv odemknout, ale v kontextu jednoduche single user apky to nema smysl.
porovnani zmeny je na checksum.je tam MD5 jako dostatecna(uvazovana kontrola prejmenovani).Objekt je injektovan a pripraven pro alternativy stejne jako StateKeeper ktery se stara o udrzeni historie.
