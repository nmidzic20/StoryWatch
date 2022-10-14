# Inicijalne upute za prijavu projekta iz Razvoja programskih proizvoda

Poštovane kolegice i kolege, 

čestitamo vam jer ste uspješno prijavili svoj projektni tim na kolegiju Razvoj programskih proizvoda, te je za vas automatski kreiran repozitorij koji ćete koristiti za verzioniranje vašega koda, ali i za pisanje dokumentacije.

Ovaj dokument (README.md) predstavlja **osobnu iskaznicu vašeg projekta**. Vaš prvi zadatak je **prijaviti vlastiti projektni prijedlog** na način da ćete prijavu vašeg projekta, sukladno uputama danim u ovom tekstu, napisati upravo u ovaj dokument, umjesto ovoga teksta.

Za upute o sintaksi koju možete koristiti u ovom dokumentu i kod pisanje vaše projektne dokumentacije pogledajte [ovaj link](https://guides.github.com/features/mastering-markdown/).
Sav programski kod potrebno je verzionirati u glavnoj **master** grani i **obvezno** smjestiti u mapu Software. Sve artefakte (npr. slike) koje ćete koristiti u vašoj dokumentaciju obvezno verzionirati u posebnoj grani koja je već kreirana i koja se naziva **master-docs** i smjestiti u mapu Documentation.

Nakon vaše prijave bit će vam dodijeljen mentor s kojim ćete tijekom semestra raditi na ovom projektu. Mentor će vam slati povratne informacije kroz sekciju Discussions također dostupnu na GitHubu vašeg projekta. A sada, vrijeme je da prijavite vaš projekt. Za prijavu vašeg projektnog prijedloga molimo vas koristite **predložak** koji je naveden u nastavku, a započnite tako da kliknete na *olovku* u desnom gornjem kutu ovoga dokumenta :) 

# Naziv projekta
StoryWatch

## Projektni tim

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime
------------  | ------------------- | ----- | ---------------------
Hrvoje Lukšić | hluksic20@student.foi.hr | 0016148613 | HLuksic
David Kajzogaj | dkajzogaj20@student.foi.hr | 0016146827 | davidkajzogaj
Noa Midžić | nmidzic20@student.foi.hr | 0108082571 | nmidzic20

## Opis domene
Sustav za upravljanje sadržajem o medijima (filmovi, igre i knjige). 
Ideja je proizašla iz toga da većina mlađih generacija prati razni sadržaj iz popularne kulture i zabave, tj. knjige, filmove, igre, serije, anime itd. Osobe koje konzumiraju puno sadržaja mogu imati neke svoje vlastite popise toga što bi još htjeli odgledati/odigrati/pročitati, ali i onoga što su već konzumirali radi toga da se podsjete što im se sviđalo odnosno što im je najdraže, kako bi se tome mogli jednom vratiti u budućnosti ili opet pogledati/odigrati/itd. kad su u raspoloženju za to. Za to da korisnik može upravljati takvim sadržajem na praktičan način bi poslužila desktop aplikacija koja ima tri grupe (igre, knjige, filmove + eventualno custom grupe) unutar kojih korisnik može po kategorijama (TODO, odgledano/odigrano/pročitano, favoriti + custom definirane kategorije) unositi naslov filma, knjige ili igre preko kratke forme, pretraživati, ažurirati i brisati uneseni sadržaj. Aplikacija bi kod prikaza nekog naslova koristila informacije s IMDb-a i drugih portala s otvorenim API za detaljne informacije i imala bi sustav preporuke (Recommend) koji bi na temelju raznih faktora korisniku na zahtjev izbacivao preporuku sadržaja za danas, i imala bi opciju generiranja izvještaja o nečemu (npr. koji žanr korisnik najviše preferira na temelju unesenih naslova). Namijenjeno filmofilima, gamerima i svakom drugom tko misli da bi mu ovakva aplikacija dobro došla da prati sav svoj sadržaj koji su već konzumirali i koji planiraju konzumirati.


## Specifikacija projekta

Koristit ćemo troslojnu arhitekturu, najviši sloj: grafički, idući sloj: korisnički s poslovnom logikom i najniži sloj: rad s podacima.

Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
F01 | Login i registracija | Korisnici se mogu registrirati u aplikaciju kako bi više korisnika koji koriste isto računalo moglo imati pristup vlastitom sadržaju. Korisnik se logira u vlastiti račun podacima koje je dobio prilikom registracije. | Hrvoje Lukšić
F02 | CRUD funkcionalnost za igre | Unos preko forme, ažuriranje, pretraživanje i brisanje podataka za naslove igara. Osim podataka koje je sam unio, korisniku se za naslov igre prikazuju i podaci koji se dohvaćaju s web servisa s otvorenim API-jem (npr. IGDB). | Hrvoje Lukšić
F03 | Sustav preporuke za igre | Korisnici mogu kliknuti opciju da im se nešto preporuči za odigrati, tako da ispune kratki upitnik od par pitanja s ikonama kao odgovorima (npr. imamo par ponuđenih emojia za žanr - chill/scary/akcija…), bi li htjeli ponovno nešto igrati ili nešto novo), pa se na temelju toga, ali i na temelju drugih faktora izbaci korisniku lista preporuka. Algoritam za preporuku bi npr. davao određen broj bodova ako je određeni sadržaj na favorit listi, ako na IGDB-u ima višu ocjenu itd. | Hrvoje Lukšić
F04 | Generiranje izvještaja za igre | Korisnik će imati opciju generiranja izvještaja s vizualnim elementima (grafikoni) o nekom aspektu vezanom za igre, npr. distribucija žanrova koje korisnik preferira na temelju unesenih naslova, distribucija firmi koje su izdale korisnikove najdraže igre i sl. | Hrvoje Lukšić
F05 | CRUD funkcionalnost za filmove |  Unos preko forme, ažuriranje, pretraživanje i brisanje podataka za naslove filmova. Osim podataka koje je sam unio, korisniku se za naslov filma prikazuju i podaci koji se dohvaćaju s web servisa s otvorenim API-jem (npr. IMDb). | Noa Midžić
F06 | Trailer za film unutar aplikacije | Za odabrani naslov filma postoji opcija gledanja trailera, koja će dohvatiti trailer s web servisa i korisniku ga prikazivati unutar embedded video playera u aplikaciji. | Noa Midžić
F07 | Sustav preporuke za filmove | Korisnici mogu kliknuti opciju da im se nešto preporuči za pogledati, tako da ispune kratki upitnik od par pitanja s ikonama kao odgovorima (npr. imamo par ponuđenih emojia za žanr - chill/scary/akcija…), bi li htjeli ponovno nešto gledati ili nešto novo), pa se na temelju toga, ali i na temelju drugih faktora izbaci korisniku lista preporuka. Algoritam za preporuku bi npr. davao određen broj bodova ako je određeni sadržaj na favorit listi, ako na IMDb-u ima višu ocjenu itd. | Noa Midžić
F08 | Generiranje izvještaja za filmove | Korisnik će imati opciju generiranja izvještaja s vizualnim elementima (grafikoni) o nekom aspektu vezanom za filmove, npr. distribucija žanrova koje korisnik preferira na temelju unesenih naslova, distribucija redatelja i glumaca vezanih za korisnikove najdraže filmove i sl. | Noa Midžić
F09 | CRUD funkcionalnost za knjige |  Unos preko forme, ažuriranje, pretraživanje i brisanje podataka za naslove knjiga. Osim podataka koje je sam unio, korisniku se za naslov knjige prikazuju i podaci koji se dohvaćaju s web servisa s otvorenim API-jem (npr. Google Books). | David Kajzogaj
F10 | E-book preview unutar aplikacije | Za odabrani naslov knjige će, ako je ona na web servisu dostupna u elektroničkom formatu i ima preview, postojati opcija čitanja previewa, koja će dohvatiti preview s web servisa (npr. Google Books) i korisniku ga prikazati unutar aplikacije. | David Kajzogaj
F11 | Sustav preporuke za knjige | Korisnici mogu kliknuti opciju da im se nešto preporuči za pročitati, tako da ispune kratki upitnik od par pitanja s ikonama kao odgovorima (npr. imamo par ponuđenih emojia za žanr - chill/scary/akcija…), bi li htjeli ponovno nešto čitati ili nešto novo), pa se na temelju toga, ali i na temelju drugih faktora izbaci korisniku lista preporuka. Algoritam za preporuku bi npr. davao određen broj bodova ako je određeni sadržaj na favorit listi, ako na web servisu ima višu ocjenu itd. | David Kajzogaj
F12 | Generiranje izvještaja za knjige | Korisnik će imati opciju generiranja izvještaja s vizualnim elementima (grafikoni) o nekom aspektu vezanom za knjige, npr. distribucija žanrova koje korisnik preferira na temelju unesenih naslova, distribucija pisaca/izdavača vezanih uz korisnikove najdraže knjige i sl. | David Kajzogaj

Nefunkcionalni zahtjevi:
- Jednostavna navigacija (tranzicija između zaslona)
- Intuitivnost grafičkog sučelja za korisnika (UX/UI, razumljive ikone, sve opcije dostupne gdje se očekuju)
- Dohvat podataka s weba i njihov prikaz u prihvatljivom vremenu (nema zastoja zbog toga u aplikaciji)
- Validacija podataka koje korisnik unosi (ako se npr. za unos očekuje godina, aplikacija se neće srušiti ako se unese nešto drugo)
- F1 dokument koji tekstualno ili vizualno daje hint vezano za taj dio gdje se korisnik nalazi u aplikaciji i gdje mu treba pomoć

## Tehnologije i oprema
- GitHub Wiki stranice za pisanje tehničke i projektne dokumentacije
- Git i GitHub za verzioniranje softvera
- GitHub Projects za projektni menadžment tj. podjelu zadataka i praćenje napretka/faza projekta (Kanban, u taskovima se može referencirati relevantni dio Wiki dokumentacije)
- .NET Framework/Core kao razvojni okvir
- Vrsta projekta: WinForm/WPF/UWP
- Visual Studio 2022 (Community) kao IDE
- Microsoft SQL Server i SQL Server Management Studio za izradu tablica 
- Biblioteke treće strane koje ćemo možda koristiti još ne znamo, ali ćemo naknadno dodavati kroz semestar

