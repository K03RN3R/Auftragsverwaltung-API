Dies ist das Abschlussprojekt meiner IHK-Umschulung.

Ich habe meine Umschulung zum Fachinformatiker für Anwendungsentwicklung erfolgreich abgeschlossen, mit Schwerpunkt C#, ASP.NET Core und Entity Framework Core. 

Mein Abschlussprojekt war eine REST-API Schnittstelle zur Auftragsverwaltung für eine Industrieanlage mit robotergestützten Maschinen, inklusive Anbindung an eine SPS über einen Webservice. 

Das Datenmodell umfasst Kunde, Auftrag, Anlage, Station, Spindel, Schicht und Fehlerbericht. 

Technisch relevant sind vor allem: 

• Optimistic Concurrency über RowVersion/ETag, um parallele Zugriffe auf Anlagendaten korrekt zu behandeln 

• Idempotente Endpunkte für die Kommunikation mit der SPS über den Webservice, damit wiederholte Anfragen keine doppelten Zustände erzeugen 

• Saubere DTOs, Validierung, Audit-Felder und Pagination für eine API, die auch im Produktivbetrieb nachvollziehbar bleibt 

Diesen Code und die zugehörige Projektdokumentation stelle ich als Arbeitsprobe gern zur Verfügung. 
