Pro řešení jsem využil rozdělování kódu do logických celků do jednotlivých statických metod. 

Přejmenoval jsem vhodně proměnné a uplatnil u nich camelCase.

Všechny používané proměnné jsem deklaroval staticky nad funkcí main a postupně s nimi pracuji pomocí statických funkcí.

V metodě Main jsem použil šablonu herní smyčky UpdateState,Render,ProcessUserInput plus kontrolu konce hry a kód jsem logicky rozdělil pod tyto metody. Před smyčkou ještě probíhá jednorázově metoda Initialize.

Opravil jsem bug v původním kódu, kdy se nová bobule mohla objevit uvnitř těla hada.

Chtěl jsem podle informací v knížce Clean Code i metody nazvat camelCase způsobem, ale Visual Studio 2022 hlásí že je to proti C# konvenci, nechal jsem tedy UpperCamelCase.

Dále jsem předělal berry a listy z int na třídu Pixel
