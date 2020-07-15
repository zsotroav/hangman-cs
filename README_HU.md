# Hangman (Akasztófa) készítette: zsotroav

Ez a játék egy publikus iskolai projekt volt, amit én úgy döntöttem, hogy GitHub-on is elérhetővé teszek.

## Első futtatás / Frissítés után:

Rakd be a `words.txt` fájlt az exe mellé, ha Windows-on vagy.

Ha Linux-ot használlsz akkor a futtatási mappába kell berakni.

> **Fontos:** Nincsenek szavak az alkalmazáshoz. Angol szavakhoz, [Xethron](https://github.com/Xethron/Hangman/blob/master/words.txt) akasztófájának a szólistáját ajánlom.

## Hogyan játszhatok vele?
Gyors leszek: 
1.	Indítsd el a Hangman.exe alkalmazást (készítsd el Visual Studio 2019-ben, vagy töltsd le innen a legfrissebb verziót)
2.	Válassz nehézségi szintet (Írd be a számot és nyomj entert)
    -	1 -> 9 hiba lehetőség
    -	2 -> 7 
    -	3 -> 5
3.	Tippelj! Nyomd meg a betűt és akkor azt leteszteli, hogy jó-e.
4.	Ha jó, akkor felül megjelenik a betű összes előfordulása
5.	Ha nem, akkor a Piros **Wrong Letters:** sorba fogja kiírni és fel fog akasztani a program
6.	Ha kitaláltad a szót, vagy meghaltál akkor az ’Y’ gombal tudsz új játékot indítani.
    -	Ekkor ugorj vissza a második lépéshez
## Kocka vagyok, megnézhetem a kódot?
Igen! Habár most a GitHub-on lévő readme fájlt olvasod, szóval a kódot már valoszínűleg megtaláltad. Itt eléred a teljes kódot a(z) `AGPL 3.0` license alatt.

## Pár adat:
C# .NET Framework 4.7.2 (Windows Exe)

C# .NET Core 3.1 (Linux port GitHub-on)

~366 sor kód
