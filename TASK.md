# NTI-Admin

NTI-Admin är ett administrationsverktyg för anställda och elever vid NTI.

Systemet hanterar:

## Användare, Roller och Passer/Lånekort

### Användare

Alla användare importeras från [Google Admin Directory](https://developers.google.com/admin-sdk/directory/v1/guides). Från terminalen ska en av de importerade användarna kunna utses till admin. Denne användare kan sen sätta roller på övriga deltagare utöver de som kan kan automagiskt importeras/översättas från Google Admin Directory.

Det finns ett "service account"som har (läs)rättigheter till Google Admin Directory för NTI, som utvecklare av *Användare & Rättigheter* kommer ha åtkomst till. Inloggningsuppgifterna till kontot får inte spridas eller sparas i kodbasen.

Det ska finnas ett "jobb" som kan köras för att importera/synka användare mot applikationens databas. Jobbet ska kunna triggas manuellt av administratörer och kunna schemaläggas (exempelvis varje natt kl 02).

För att undvika onödiga request till Google Admin Directory ska data under *utveckling* hämtas från en local "cache" av något slag. Datan till "cachen" *kan* hämtas från Google Admin Directory. Då datan innehåller personuppgifter får den inte lagras i repot. Eventuellt kan man skapa en identiskt datastruktur med fingerade värden eller obfuskera värdena innan de sparas.

Lämpliga värden som hämtats från directory (t.ex namn, email, etc) ska lagras i applikationens databas.

När en användare slutat vid skolan ska hens data inom den tidsfrist som GDPR/PUL tillåter "raderas" från databasen och där så är erfoderligt ersättas av anonymiserad data (t.ex kan namnet bli "Avslutad Elev 2023-08-09") där det inte är lämpligt att datan helt raderas (t.ex. om andra tabeller i databasen kan vara beroende av värdet och innehållet i dessa poster är viktigt att spara).

### Roller

Det finns 6 olika användar*roller*. En användare kan ha *flera* roller.

1. Elev

   Elever kan 

   * logga in i systemet.
   * hantera lån för biblioteksböcker till sig själv.
   * se vilken kurslitteratur som är utlånad till sig själv.
   * se vem som lånat ut och vem som lånat (upphittad) kurslitteratur.
   * se status på sitt datorlån och eventuella serviceärenden.
   * registrera serviceförfrågan för sin dator.

2. Personal kan

   * allt som elever kan.
   * låna ut kurslitteratur till elever.
   * registrera utlånad kurslitteratur som återlämnad.
   * CRUD på kurslitteratur.

3. Bibliotekarie kan

   * allt som personal kan.
   * se översikt över lån (inkl. historiska lån), hantera desamma, och skicka ut påminnelser etc.
   * CRUD på biblioteksböcker.

4. Vaktmästare kan

   * allt som personal kan.
   * hantera serviceförfrågan.
   * registrera datorer och datorlån.
   * hantera serviceärenden.
   * hantera roller för personal (personal, bibliotekarie, vaktmästare).
   * CRUD för passer/lånekort.

5. Rektor kan

   * allt som vaktmästare kan.
   * allt som bibliotekarie kan.
   * hantera alla roller för all personal.

6. Admin kan

   * allt som rektor kan (denna roll sätts från terminalen för en av de importerade användarna).

### Passer/Lånekort

Varje användare får ett kombinerat passer/lånekort.

Passerkortet har ett RFID-nummer som ej är kopplat till NTI-Admin men som används i ett annat system för inpassering. RFID-numret är alltså ointressant för NTI-Admin och kan helt ignoreras.

På lånekortet finns även tryckt en bild på personen, dess namn och ett unikt id (\<ntijoh>\<startår>\<tresiffrigt löpnummer>) för elever (t.ex ntijoh23001) för elever och (\<ntijoh>-<två-bokstäver från förnamnet><två-bokstäver från efternamnet>) för lärare (t.ex ntijoh-dabe) lärare. Id:t för personal och elev bör således innehålla lika många tecken.

Id:t står även som en streckkod (Code39). Streckkoden används för utlåning av kurslitteratur och biblioteksböcker och ska kunna scannas av en streckkodsscanner.

Utöver detta står den "rollen" personen har (Elev, Personal, Vaktmästare, Rektor).

Ett slutgiltigt utseende på korten kommer komma från elever på NTIs Designprogram och kan komma att uppdateras för olika läsår, så utseende (bakgrundsbild, positionering av porträtt, texter och serienummer) bör vara flexibelt och konfigurerbart på något sätt.

Kortbilderna ska genereras på backend och gå att ladda ner för bulkutskrift på en kortsskrivare (fungerar som en vanlig skrivare, men skriver ut på plastkort i stället för papper).

## Biblioteksböcker och Kurslitteratur

Följande begrepp är centrala:

Titel: En abstrakt "bok" - dvs det som identifieras av ett ISBN-nummer

Bok: En fysisk bok man kan låna ut till någon ("tillhör" en titel).

### Biblioteksböcker

Elev och personal ska kunna låna biblioteksböcker till sig själv. 

Bibliotekarier kan markera böcker som återlämnade

En Titel har ett ISBN-nummer (används för att unikt identifiera en bok och hämta data om boken från t.ex Google Books). och annan aktuell information, inkl. inköpspris och ersättningspris, och status om dess skick.

En Bok har en unik streckkod som identifierar den fysiska boken, och används för utlåning av densamma. En bok har dessutom status om dess skick.

Ett lån har en begränsad tid (x veckor, kan justeras av bibliotekarie (samma för samtliga böcker, men påverkar inte pågående lån))

En bok kan bara vara utlånad åt en person åt gången.

Bibliotekssystemet utlåningsdel ska även gå att använda som en separat webapp (åtkomstbar från en annan address (typ bibliotek.ntig.se)) utöver att vara åtkomstbar inifrån admin.ntig.se

Relevanta attribut för Titlar, Böcker och Lån sparas i databasen.

Låntagare kan skriva recensioner på böcker och ge dem betyg.

### Kurslitteratur

Lärare kan låna ut kurslitteratur till sig själva och elever.

En Titel har ett ISBN-nummer (används för att unikt identifiera en Titel och hämta data om Titeln från t.ex Google Books), och annan aktuell information, inkl. inköpspris och ersättningspris,

En Bok har en unik streckkod som identifierar den fysiska boken, och används för utlåning av densamma. En Bok har dessutom status om dess skick.

När Personal lånar ut en bok kan hen välja från godtyckligt återlämningsdatum, eller förifyllda datum motsvarande terminsslut.

Det ska finnas stöd för "bulkutlåning", dvs en lärare "blippar" en Bok, väljer ett återlämningsdatum, och blippar sen ett elevkort, och bekräftar lånet med `Enter` eller annat smidigt sätt.

Om läraren sen blippar en Bok av samma Titel ska föregående återlämningsdatum vara förifyllt, och resten av utlåningen sker som ovan.

Vid återlämning räcker det att en lärare blippar en Bok och väljer "Återlämna". Om en lärare sen blippar en bok av samma titel ska "Återlämna" vara förvalt och läraren behöver enbart trycka Enter för att bekräfta återlämningen.

En Bok kan bara vara utlånad till en person åt gången.

Lärare kan se historik över bokens lån.

Relevanta attribut för Titlar, Böcker och Lån sparas i databasen.

## Datorer, Lån och Serviceärenden

Vaktmästare administrerar datorer.

### Datorer & Lån

Datorer kan lånas ut till elever och personal. 

Återlämningsdatum ska kunna sättas till när eleven beräknas sluta utbildningen, annars till när leasingavtalet går ut, eller godtyckligt datum *innan* de ovanstående datumen. De två första alternativen är standard.

För lärare sätts återlämningsdatum som standard till när leasingavtalet gått ut, annars godtyckligt datum *innan* leasingavtalet går ut.

Det ska finnas möjlighet till bulkimport av datorer från extern fil.

Det ska finnas möjlighet till bulkexport av lån till extern fil.

En Dator kan bara vara utlånad till en person åt gången. En person kan ha flera datorer utlånade (t.ex som lånedator). En Ipad eller liknande kan anses vara en Dator.

Relevanta attribut för datorer sparas i databasen, bland annat: serienummer (sträng), märke, modell

### Serviceärenden

Vaktmästare administrerar servicerärenden för datorer.

En dator kan ha många serviceärenden.

Personal och elever kan lägga en serviceärendenförfrågan, som hanteras av vaktmästaren. Det finns ett meddelandefält där vaktmästare kan kommunicera med den som lagt serviceärendeförfrågan. När nya meddelanden tillkommer aviseras den som lagt förfrågan via email.

Från en serviceärendeförfrågan kan vaktmästaren skapa ett serviceärende.

Serviceärenden kan uppdateras med status, och avslutas (förhoppningsvis när datorns fel är åtgärdat). När ett serviceärende uppdateras kan vaktmästaren välja att avisera låntagaren via email.

Vaktmästare kan skapa serviceärenden direkt utan att först skapa serviceärendeförfrågan.

Vaktmästare ska smidigt kunna se status över utlånade/outlånade datorer och serviceärenden.

## Tech Stack

### Versionshantering

Applikationen ska lagras i ett (1) repo med en frontend i en mapp och backend i en annan mapp.

Arbete ska ske i branches enligt github flow.

Inga branches som inte tillhör en issue/todo-item.

Master branch är protected

Pull request kräver code review av 2 andra teammedlemmar.

### Struktur:

#### Backend

* C#

* ASP.NET Core Web Api
* Entity Framework Core (EF Core)

Användarimport/synk och inloggning via Google.

Dockerbaserad PostgreSQL Databas.

* timestamps (skapad, uppdaterad) på de flesta resurser.
* "soft delete" där så är tillämpbart.

#### Frontend

Språk: TypeScript.

* Vue 3
* Pinia
* Vue-Router
* Vuetify
* StoryBook

### Testing:

* Dotnet test (med xUnit?)
* End-to-End-tester med Cypress

### Dokumentering

* Swagger/Swashbuckle för API routes
* ?

### Deployment

en MVP ska vara körbar på TE4-servern i slutet av projektperioden.

### Arbetssätt

* SCRUM
* Daily Standups
* GitHub Projects (en board per team)
* Vecko-sprinter med sprintplanerings/uppföljningsmöte första arbetsdagen i veckan. Dessutom synkningsmöte mellan gruppledarna.

## Teams

### Accounts & Cards (RED)

**Lead:** Mortaza

**UX/UI:** Christian

**Testing:** Tobias

**Documentation:** Benjamin

**Code Quality:** Mathias

**Security:** Lukas

### Books and Library (GREEN)

**Lead:** Theo

**UX/UI:** William

**Testing:** Samantha

**Documentation:** Frans

**Code Quality:** Adrian

### Computers and Equipment (BLUE)

**Lead:** Linus

**UX/UI:** Axel L

**Testing:** Viktor

**Documentation:** Nicolai

**Code Quality:** Axel H
