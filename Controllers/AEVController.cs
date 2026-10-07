using GUI_2.Data;
using GUI_2.DTO.Anlage;
using GUI_2.DTO.Auftrag;
using GUI_2.DTO.Fehlerbericht;
using GUI_2.DTO.Kunde;
using GUI_2.DTO.Schicht;
using GUI_2.DTO.Spindel;
using GUI_2.DTO.Station;
using GUI_2.DTO.Status;
using GUI_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using static System.Collections.Specialized.BitVector32;


namespace GUI_2.Controllers
{
    [ApiController] //gibt an, dass es sich um einen API-Controller handelt
    [Route("api/[controller]")] //Standart Route
    public class AEVController : ControllerBase
    {

        private readonly AppDbContext _context;

        public AEVController(AppDbContext context)
        {
            _context = context;
        }

        #region KUNDE
        // GETALL mit Pagination //cancelation token hinzufügen!!!!!!!!// hinzugefügt
        [HttpGet("kunde")]
        public async Task<ActionResult<IEnumerable<KundeGetDTO>>> GetAll(
            int page = 1,
            int pageSize = 20,
            [FromQuery] string? name = null,
            [FromQuery] string? ort = null,
            [FromQuery] string? plz = null,
            [FromQuery] string? land = null,
            [FromQuery] string? email = null,
            CancellationToken cancellationToken = default)
        {
            if (page is < 1 or > 20 || pageSize is < 1 or > 20) //sind fix gesetzt und kann man fromquery setzen// da int primitiv ist es query
                return BadRequest("Page und PageSize müssen größer als 0 und kleiner als 20 sein."); //pagesize limitieren DAU //fertig

            var query = _context.Kunden.AsNoTracking();

            //TotalCount mit CancellationToken
            var totalCount = await query.CountAsync(cancellationToken);

            //Filter für FromQuery

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(k => k.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(ort))
            {
                query = query.Where(k => k.Ort.Contains(ort));
            }

            if (!string.IsNullOrEmpty(plz))
            {
                query = query.Where(k => k.PLZ.Contains(plz));
            }

            if (!string.IsNullOrEmpty(land))
            {
                query = query.Where(k => k.Land.Contains(land));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(k => k.Email.Contains(email));
            }
            var kunden = await query
                .OrderBy(k => k.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(k => MapKundeToDto(k))      //aufruf der GetDTO variablen unten aus dem Code / (k) kleines k weil LINQ-Query. außerhalb von linq "kunde" in die klammern
                .ToListAsync(cancellationToken);

            Response.Headers["X-Total-Count"] = totalCount.ToString();
            return Ok(kunden);
        }

        // GET mit ID
        [HttpGet("kunde/{id:int}")]
        public async Task<ActionResult<KundeGetDTO>> GetByIdKunde(int id)
        {
            var kunde = await _context.Kunden
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kunde == null)
                return NotFound($"Kunde mit ID {id} wurde nicht gefunden.");

            // ETag-Header setzen
            Response.Headers["ETag"] = ETagHelper.ToEtag(kunde.RowVersion);

            return Ok(MapKundeToDto(kunde));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // GET mit GUID
        [HttpGet("kundeguid/{guid:guid}")]
        public async Task<ActionResult<KundeGetDTO>> GetByGuid(Guid guid)
        {
            var kunde = await _context.Kunden
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.Guid == guid);

            if (kunde == null)
                return NotFound($"Kunde mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(kunde.RowVersion);

            return Ok(MapKundeToDto(kunde));       //aufruf der GetDTO variablen unten aus dem Code
        }

        // POST
        [HttpPost("kunde")]
        public async Task<ActionResult<KundeGetDTO>> Create([FromBody] KundeCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //richtige validationslogiken einfügen: wie bei der mailadresse!!!!!!

            var kunde = new Kunde
            {
                Guid = Guid.NewGuid(),
                Name = dto.Name,
                Strasse = dto.Strasse,
                Hausnummer = dto.Hausnummer,
                PLZ = dto.PLZ,
                Ort = dto.Ort,
                Land = dto.Land,
                Email = dto.Email,
                Telefonnummer = dto.Telefonnummer
                //ErstelltUtc = DateTime.UtcNow, // nicht änderbar setzen auf serverzeit // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon  // wird automatisch gesetzt durch dbcontext
            };

            _context.Kunden.Add(kunde);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapKundeToDto(kunde); //Ausgabe des GetDTO als Rückmeldung was erstellt wurde

           
            Response.Headers["ETag"] = ETagHelper.ToEtag(kunde.RowVersion);

            return CreatedAtAction(nameof(GetByIdKunde), new { id = kunde.Id }, result);
        }

        // PUT
        [HttpPut("kunde/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] KundePutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kunde = await _context.Kunden.FirstOrDefaultAsync(k => k.Id == id);
            if (kunde == null)
                return NotFound($"Kunde mit ID {id} wurde nicht gefunden.");

            if (string.IsNullOrEmpty(ifMatch))
                return StatusCode(428, "If-Match Header ist erforderlich.");

            if (!ETagHelper.Matches(ifMatch, kunde.RowVersion))
                return StatusCode(412, "ETag mismatch – der Datensatz wurde zwischenzeitlich geändert.");

            // Änderungen übernehmen
            kunde.Name = dto.Name;
            kunde.Strasse = dto.Strasse;
            kunde.Hausnummer = dto.Hausnummer;
            kunde.PLZ = dto.PLZ;
            kunde.Ort = dto.Ort;
            kunde.Land = dto.Land;
            kunde.Email = dto.Email;
            kunde.Telefonnummer = dto.Telefonnummer;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            // neuen ETag im Response mitschicken
            Response.Headers["ETag"] = ETagHelper.ToEtag(kunde.RowVersion);

            return Ok($"Kunde mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("kunde/{id:int}")] //rowversions implementieren?
        public async Task<IActionResult> Delete(int id)
        {
            var kunde = await _context.Kunden.FindAsync(id);
            if (kunde == null)
                return NotFound($"Kunde mit ID {id} wurde nicht gefunden.");

            _context.Kunden.Remove(kunde);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region AUFTRAG
        // GETAll mit Pagination
        [HttpGet("auftrag")]
        public async Task<ActionResult<IEnumerable<AuftragGetDTO>>> GetAllAuftraege(int page = 1, int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page und PageSize müssen größer als 0 sein.");

            var query = _context.Auftraege.AsNoTracking();
            var totalCount = await query.CountAsync();

            var auftraege = await query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => MapAuftragToDto(a))        //aufruf der GetDTO variablen unten aus dem Code
                .ToListAsync();

            Response.Headers["X-Total-Count"] = totalCount.ToString();
            return Ok(auftraege);
        }

        // GET mit ID
        [HttpGet("auftrag/{id:int}")]
        public async Task<ActionResult<AuftragGetDTO>> GetByIdAuftrag(int id)
        {
            var auftrag = await _context.Auftraege
                .AsNoTracking()
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (auftrag == null)
                return NotFound($"Auftrag mit ID {id} wurde nicht gefunden.");

            // ETag in Response-Header setzen
            Response.Headers["ETag"] = ETagHelper.ToEtag(auftrag.RowVersion);

            return Ok(MapAuftragToDto(auftrag));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // GET mit GUID
        [HttpGet("auftragguid/{guid:guid}")]
        public async Task<ActionResult<AuftragGetDTO>> GetByGuidAuftraege(Guid guid)
        {
            var auftrag = await _context.Auftraege
                .AsNoTracking()
                .Where(a => a.AuftragGuid == guid)
                .FirstOrDefaultAsync();

            if (auftrag == null)
                return NotFound($"Auftrag mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(auftrag.RowVersion);

            return Ok(MapAuftragToDto(auftrag));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // POST
        [HttpPost("auftrag")]
        public async Task<ActionResult<AuftragGetDTO>> Create([FromBody] AuftragCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var auftrag = new Auftrag
            {
                KundeId = dto.KundeId,
                AnlageId = dto.AnlageId,
                Auftragsname = dto.Auftragsname,
                Auftragstyp = dto.Auftragstyp,
                MengeSOLL = dto.MengeSOLL,
                MengeIO = dto.MengeIO,
                MengeNIO = dto.MengeNIO,
                TaktZiel = dto.TaktZiel,
                TaktBerechnet = dto.TaktBerechnet,
                StatusId = dto.StatusId,
                Prioritaet = dto.Prioritaet,
                Quelle = dto.Quelle,
                Beladen = dto.Beladen,
                BeladenAnzahl = dto.BeladenAnzahl,
                Bilddaten = dto.Bilddaten
                //ErstelltUtc = DateTime.UtcNow, // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon,
            };

            _context.Auftraege.Add(auftrag);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler." + ex.ToString());
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapAuftragToDto(auftrag);      //aufruf der GetDTO variablen unten aus dem Code

            return CreatedAtAction(nameof(GetByIdAuftrag), new { id = auftrag.Id }, result);
        }

        // PUT
        [HttpPut("auftrag/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] AuftragPutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(ifMatch))
                return StatusCode(428, "If-Match Header ist erforderlich.");

            var auftrag = await _context.Auftraege.FirstOrDefaultAsync(a => a.Id == id);
            if (auftrag == null)
                return NotFound($"Auftrag mit ID {id} wurde nicht gefunden.");

            if (!ETagHelper.Matches(ifMatch, auftrag.RowVersion))
                return StatusCode(412, "ETag mismatch – der Datensatz wurde zwischenzeitlich geändert.");

            // Änderungen übernehmen
            auftrag.Auftragsname = dto.Auftragsname;
            auftrag.Auftragstyp = dto.Auftragstyp;
            auftrag.MengeSOLL = dto.MengeSOLL;
            auftrag.MengeIO = dto.MengeIO;
            auftrag.MengeNIO = dto.MengeNIO;
            auftrag.TaktZiel = dto.TaktZiel;
            auftrag.TaktBerechnet = dto.TaktBerechnet;
            auftrag.StatusId = dto.StatusId;
            auftrag.Prioritaet = dto.Prioritaet;
            auftrag.Quelle = dto.Quelle;
            
            auftrag.Beladen = dto.Beladen;
            auftrag.BeladenAnzahl = dto.BeladenAnzahl;
            auftrag.Bilddaten = dto.Bilddaten;
            //auftrag.GeaendertUtc = DateTime.UtcNow;    // wird automatisch gesetzt durch dbcontext
            //auftrag.GeaendertVon = dto.GeaendertVon;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler." + ex.ToString());
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            // Neue ETag zurückliefern
            var updatedEtag = ETagHelper.ToEtag(auftrag.RowVersion);
            Response.Headers["ETag"] = updatedEtag;

            return Ok($"Auftrag mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("auftrag/{id:int}")]
        public async Task<IActionResult> DeleteAuftraege(int id)
        {
            var auftrag = await _context.Auftraege.FindAsync(id);
            if (auftrag == null)
                return NotFound($"Auftrag mit ID {id} wurde nicht gefunden.");

            _context.Auftraege.Remove(auftrag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region ANLAGE
        // GETAll mit Pagination
        [HttpGet("anlage")]
        public async Task<ActionResult<IEnumerable<AnlageGetDTO>>> GetAllAnlage(
            int page = 1, int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page und PageSize müssen größer als 0 sein.");

            var query = _context.Anlagen.AsNoTracking();

            var totalCount = await query.CountAsync();

            var anlagen = await query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(an => MapAnlageToDto(an))       //aufruf der GetDTO variablen unten aus dem Code
                .ToListAsync();

            Response.Headers["X-Total-Count"] = totalCount.ToString();
            return Ok(anlagen);
        }

        // GET mit ID
        [HttpGet("anlage/{id:int}")]
        public async Task<ActionResult<AnlageGetDTO>> GetByIdAnlage(int id)
        {
            var anlage = await _context.Anlagen
                .AsNoTracking()
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (anlage == null)
                return NotFound($"Anlage mit ID {id} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(anlage.RowVersion);
            return Ok(MapAnlageToDto(anlage));
        }

        // GET mit GUID
        [HttpGet("anlageguid/{guid:guid}")]
        public async Task<ActionResult<AnlageGetDTO>> GetByGuidAnlage(Guid guid)
        {
            var anlage = await _context.Anlagen
                .AsNoTracking()
                .Where(a => a.AnlageGuid == guid)
                .FirstOrDefaultAsync();

            if (anlage == null)
                return NotFound($"Anlage mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(anlage.RowVersion);
            return Ok(MapAnlageToDto(anlage));
        }

        // POST
        [HttpPost("anlage")]
        public async Task<ActionResult<AnlageGetDTO>> Create([FromBody] AnlageCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var anlage = new Anlage
            {
                KundeId = dto.KundeId,
                Bezeichnung = dto.Bezeichnung,
                AnlagenCode = dto.AnlagenCode
                //ErstelltUtc = DateTime.UtcNow,  // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon
            };

            _context.Anlagen.Add(anlage);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapAnlageToDto(anlage);

            Response.Headers["ETag"] = ETagHelper.ToEtag(anlage.RowVersion);
            return CreatedAtAction(nameof(GetByIdAnlage), new { id = anlage.Id }, result);
        }

        // PUT
        [HttpPut("anlage/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] AnlagePutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var anlage = await _context.Anlagen.FirstOrDefaultAsync(a => a.Id == id);
            if (anlage == null)
                return NotFound($"Anlage mit ID {id} wurde nicht gefunden.");

            // Optimistisches Locking via If-Match + ETag/RowVersion
            var ifMatchHeader = Request.Headers["If-Match"].ToString();
            if (!ETagHelper.Matches(ifMatchHeader, anlage.RowVersion))
            {
                return StatusCode(412, "ETag Mismatch – der Datensatz wurde zwischenzeitlich geändert.");
            }

            // Änderungen übernehmen
            anlage.Bezeichnung = dto.Bezeichnung;
            anlage.AnlagenCode = dto.AnlagenCode;
            //anlage.GeaendertUtc = DateTime.UtcNow;  // wird automatisch gesetzt durch dbcontext
            //anlage.GeaendertVon = dto.GeaendertVon;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            // Neue RowVersion ins ETag-Header zurückgeben
            Response.Headers["ETag"] = ETagHelper.ToEtag(anlage.RowVersion);
            return Ok($"Anlage mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("anlage/{id:int}")]
        public async Task<IActionResult> DeleteAnlage(int id)
        {
            var anlage = await _context.Anlagen.FindAsync(id);
            if (anlage == null)
                return NotFound($"Anlage mit ID {id} wurde nicht gefunden.");

            _context.Anlagen.Remove(anlage);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region FEHLERBERICHT
        // GETAll mit Pagination
        [HttpGet("fehlerbericht")]
        public async Task<ActionResult<IEnumerable<FehlerberichtGetDTO>>> GetAllFehlerbericht(
            int page = 1, int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page und PageSize müssen größer als 0 sein.");

            var query = _context.Fehlerberichte.AsNoTracking();

            var totalCount = await query.CountAsync();

            var fehlerberichte = await query
                .OrderBy(f => f.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(fb => MapFehlerberichtToDto(fb))      //aufruf der GetDTO variablen unten aus dem Code
                .ToListAsync();

            Response.Headers["X-Total-Count"] = totalCount.ToString();
            return Ok(fehlerberichte);
        }

        // GET
        [HttpGet("fehlerbericht/{id:int}")]
        public async Task<ActionResult<FehlerberichtGetDTO>> GetByIdFehlerbericht(int id)
        {
            var fehlerbericht = await _context.Fehlerberichte
                .AsNoTracking()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (fehlerbericht == null)
                return NotFound($"Fehlerbericht mit ID {id} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(fehlerbericht.RowVersion);
            return Ok(MapFehlerberichtToDto(fehlerbericht));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // GET
        [HttpGet("fehlerberichtguid/{guid:guid}")]
        public async Task<ActionResult<FehlerberichtGetDTO>> GetByGuidFehlerbericht(Guid guid)
        {
            var fehlerbericht = await _context.Fehlerberichte
                .AsNoTracking()
                .Where(f => f.FehlerberichtGuid == guid)
                .FirstOrDefaultAsync();

            if (fehlerbericht == null)
                return NotFound($"Fehlerbericht mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(fehlerbericht.RowVersion);
            return Ok(MapFehlerberichtToDto(fehlerbericht));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // POST
        [HttpPost("fehlerbericht")]
        public async Task<ActionResult<FehlerberichtGetDTO>> Create([FromBody] FehlerberichtCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fehlerbericht = new Fehlerbericht
            {
                StationId = dto.StationId,
                AuftragId = dto.AuftragId,
                SpindelId = dto.SpindelId,
                Spindelnummer = dto.Spindelnummer,
                Code = dto.Code,
                Titel = dto.Titel,
                Beschreibung = dto.Beschreibung,
                Schwere = dto.Schwere,
                Status = dto.Status,
                CustomData = dto.CustomData
                //ErstelltUtc = DateTime.UtcNow,  // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon
            };

            _context.Fehlerberichte.Add(fehlerbericht);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler. " + ex.ToString());
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapFehlerberichtToDto(fehlerbericht);      //aufruf der GetDTO variablen unten aus dem Code

            Response.Headers["ETag"] = ETagHelper.ToEtag(fehlerbericht.RowVersion);
            // event zur ui
            return CreatedAtAction(nameof(GetByIdFehlerbericht), new { id = fehlerbericht.Id }, result);
        }

        // PUT
        [HttpPut("fehlerbericht/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] FehlerberichtPutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fehlerbericht = await _context.Fehlerberichte.FirstOrDefaultAsync(f => f.Id == id);
            if (fehlerbericht == null)
                return NotFound($"Fehlerbericht mit ID {id} wurde nicht gefunden.");

            var ifMatchHeader = Request.Headers["If-Match"].ToString();
            if (!ETagHelper.Matches(ifMatchHeader, fehlerbericht.RowVersion))
            {
                return StatusCode(412, "ETag Mismatch – der Datensatz wurde zwischenzeitlich geändert.");
            }

            fehlerbericht.StationId = dto.StationId;
            fehlerbericht.AuftragId = dto.AuftragId;
            fehlerbericht.SpindelId = dto.SpindelId;
            fehlerbericht.Spindelnummer = dto.Spindelnummer;
            fehlerbericht.Code = dto.Code;
            fehlerbericht.Titel = dto.Titel;
            fehlerbericht.Beschreibung = dto.Beschreibung;
            fehlerbericht.Schwere = dto.Schwere;
            fehlerbericht.Status = dto.Status;
            fehlerbericht.CustomData = dto.CustomData;
            //fehlerbericht.GeaendertUtc = DateTime.UtcNow;  // wird automatisch gesetzt durch dbcontext
            //fehlerbericht.GeaendertVon = dto.GeaendertVon;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            Response.Headers["ETag"] = ETagHelper.ToEtag(fehlerbericht.RowVersion);
            return Ok($"Fehlerbericht mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("fehlerbericht/{id:int}")]
        public async Task<IActionResult> DeleteFehlerbericht(int id)
        {
            var fehlerbericht = await _context.Fehlerberichte.FindAsync(id);
            if (fehlerbericht == null)
                return NotFound($"Fehlerbericht mit ID {id} wurde nicht gefunden.");

            _context.Fehlerberichte.Remove(fehlerbericht);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region SCHICHT

        // GETAll mit Pagination
        [HttpGet("schicht")]
        public async Task<ActionResult<IEnumerable<SchichtGetDTO>>> GetAllSchicht(
            int page = 1,
            int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page und PageSize müssen größer als 0 sein.");

            var query = _context.Schichten.AsNoTracking();

            var totalCount = await query.CountAsync();

            var schichten = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => MapSchichtToDto(s))      //aufruf der GetDTO variablen unten aus dem Code
                .ToListAsync();

            Response.Headers["X-Total-Count"] = totalCount.ToString();

            return Ok(schichten);
        }

        // GET mit ID
        [HttpGet("schicht/{id:int}")]
        public async Task<ActionResult<SchichtGetDTO>> GetByIdSchicht(int id)
        {
            var schicht = await _context.Schichten
                .AsNoTracking()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (schicht == null)
                return NotFound($"Schicht mit ID {id} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(schicht.RowVersion);
            return Ok(MapSchichtToDto(schicht));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // GET mit GUID
        [HttpGet("schichtguid/{guid:guid}")]
        public async Task<ActionResult<SchichtGetDTO>> GetByGuidSchicht(Guid guid)
        {
            var schicht = await _context.Schichten
                .AsNoTracking()
                .Where(s => s.SchichtGuid == guid)
                .FirstOrDefaultAsync();

            if (schicht == null)
                return NotFound($"Schicht mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(schicht.RowVersion);
            return Ok(MapSchichtToDto(schicht));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // POST
        [HttpPost("schicht")]
        public async Task<ActionResult<SchichtGetDTO>> Create([FromBody] SchichtCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schicht = new Schicht
            {
                KundeId = dto.KundeId,
                Name = dto.Name,
                StartzeitTag = dto.StartzeitTag,
                EndzeitTag = dto.EndzeitTag,
                Mitternachtsarbeit = dto.Mitternachtsarbeit
                //ErstelltUtc = DateTime.UtcNow,  // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon
            };

            _context.Schichten.Add(schicht);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapSchichtToDto(schicht);      //aufruf der GetDTO variablen unten aus dem Code    

            return CreatedAtAction(nameof(GetByIdSchicht), new { id = schicht.Id }, result);
        }

        // PUT
        [HttpPut("schicht/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] SchichtPutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schicht = await _context.Schichten.FirstOrDefaultAsync(s => s.Id == id);
            if (schicht == null)
                return NotFound($"Schicht mit ID {id} wurde nicht gefunden.");

            //  ETag Abgleich
            var etagHeader = Request.Headers["If-Match"].ToString();
            if (string.IsNullOrWhiteSpace(etagHeader) || !ETagHelper.Matches(etagHeader, schicht.RowVersion))
                return StatusCode(StatusCodes.Status412PreconditionFailed, "ETag mismatch oder fehlt.");

            // Änderungen übernehmen
            schicht.KundeId = dto.KundeId;
            schicht.Name = dto.Name;
            schicht.StartzeitTag = dto.StartzeitTag;
            schicht.EndzeitTag = dto.EndzeitTag;
            schicht.Mitternachtsarbeit = dto.Mitternachtsarbeit;
            //schicht.GeaendertUtc = DateTime.UtcNow;  // wird automatisch gesetzt durch dbcontext
            //schicht.GeaendertVon = dto.GeaendertVon;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            return Ok($"Schicht mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("schicht/{id:int}")]
        public async Task<IActionResult> DeleteSchicht(int id)
        {
            var schicht = await _context.Schichten.FindAsync(id);
            if (schicht == null)
                return NotFound($"Schicht mit ID {id} wurde nicht gefunden.");

            _context.Schichten.Remove(schicht);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region SPINDEL

        // GETAll mit Pagination
        [HttpGet("spindel")]
        public async Task<ActionResult<IEnumerable<SpindelGetDTO>>> GetAllSpindel(int page = 1, int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page und PageSize müssen größer als 0 sein.");

            var query = _context.Spindeln.AsNoTracking();
            var totalCount = await query.CountAsync();

            var spindeln = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(sp => MapSpindelToDto(sp))      //aufruf der GetDTO variablen unten aus dem Code
                .ToListAsync();

            Response.Headers["X-Total-Count"] = totalCount.ToString();
            return Ok(spindeln);
        }

        // GET mit ID
        [HttpGet("spindel/{id:int}")]
        public async Task<ActionResult<SpindelGetDTO>> GetByIdSpindel(int id)
        {
            var spindel = await _context.Spindeln
                .AsNoTracking()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (spindel == null)
                return NotFound($"Spindel mit ID {id} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(spindel.RowVersion);
            return Ok(MapSpindelToDto(spindel));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // GET mit GUID
        [HttpGet("spindelguid/{guid:guid}")]
        public async Task<ActionResult<SpindelGetDTO>> GetByGuidSpindel(Guid guid)
        {
            var spindel = await _context.Spindeln
                .AsNoTracking()
                .Where(s => s.SpindelGuid == guid)
                .FirstOrDefaultAsync();

            if (spindel == null)
                return NotFound($"Spindel mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(spindel.RowVersion);
            return Ok(MapSpindelToDto(spindel));        //aufruf der GetDTO variablen unten aus dem Code
        }

        // POST
        [HttpPost("spindel")]
        public async Task<ActionResult<SpindelGetDTO>> Create([FromBody] SpindelCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spindel = new Spindel
            {
                StationId = dto.StationId,
                Nummer = dto.Nummer,
                Bezeichnung = dto.Bezeichnung,
                IsActive = dto.IsActive
                //ErstelltUtc = DateTime.UtcNow,  // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon
            };

            _context.Spindeln.Add(spindel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapSpindelToDto(spindel);      //aufruf der GetDTO variablen unten aus dem Code

            return CreatedAtAction(nameof(GetByIdSpindel), new { id = spindel.Id }, result);
        }

        // PUT
        [HttpPut("spindel/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] SpindelPutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spindel = await _context.Spindeln.FirstOrDefaultAsync(s => s.Id == id);
            if (spindel == null)
                return NotFound($"Spindel mit ID {id} wurde nicht gefunden.");

            // ETag prüfen
            var etagHeader = Request.Headers["If-Match"].ToString();
            if (string.IsNullOrWhiteSpace(etagHeader) || !ETagHelper.Matches(etagHeader, spindel.RowVersion))
                return StatusCode(StatusCodes.Status412PreconditionFailed, "ETag mismatch oder fehlt.");

            // Werte übernehmen
            spindel.StationId = dto.StationId;
            spindel.Nummer = dto.Nummer;
            spindel.Bezeichnung = dto.Bezeichnung;
            spindel.IsActive = dto.IsActive;
            //spindel.GeaendertUtc = DateTime.UtcNow;  // wird automatisch gesetzt durch dbcontext
            //spindel.GeaendertVon = dto.GeaendertVon;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            return Ok($"Spindel mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("spindel/{id:int}")]
        public async Task<IActionResult> DeleteSpindel(int id)
        {
            var spindel = await _context.Spindeln.FindAsync(id);
            if (spindel == null)
                return NotFound($"Spindel mit ID {id} wurde nicht gefunden.");

            _context.Spindeln.Remove(spindel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region STATION

        // GETAll mit Pagination
        [HttpGet("station")]
        public async Task<ActionResult<IEnumerable<StationGetDTO>>> GetAllStation(
            int page = 1, int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page und PageSize müssen größer als 0 sein.");

            var query = _context.Stationen.AsNoTracking();
            var totalCount = await query.CountAsync();

            var stationen = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(st => MapStationToDto(st))      //aufruf der GetDTO variablen unten aus dem Code
                .ToListAsync();

            Response.Headers["X-Total-Count"] = totalCount.ToString();
            return Ok(stationen);
        }


        // GET mit ID
        [HttpGet("station/{id:int}")]
        public async Task<ActionResult<StationGetDTO>> GetByIdStation(int id)
        {
            var station = await _context.Stationen
                .AsNoTracking()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (station == null)
                return NotFound($"Station mit ID {id} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(station.RowVersion);

            return Ok(MapStationToDto(station));       //aufruf der GetDTO variablen unten aus dem Code
        }

        // GET mit GUID
        [HttpGet("stationguid/{guid:guid}")]
        public async Task<ActionResult<StationGetDTO>> GetByGuidStation(Guid guid)
        {
            var station = await _context.Stationen
                .AsNoTracking()
                .Where(s => s.StationGuid == guid)
                .FirstOrDefaultAsync();

            if (station == null)
                return NotFound($"Station mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(station.RowVersion);

            return Ok(MapStationToDto(station));       //aufruf der GetDTO variablen unten aus dem Code
        }

        // POST: api/stationen
        [HttpPost("station")]
        public async Task<ActionResult<StationGetDTO>> Create([FromBody] StationCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var station = new Station
            {
                AnlageId = dto.AnlageId,
                Name = dto.Name,
                IsActive = dto.IsActive
                //ErstelltUtc = DateTime.UtcNow,  // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon
            };

            _context.Stationen.Add(station);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapStationToDto(station);      //aufruf der GetDTO variablen unten aus dem Code

            return CreatedAtAction(nameof(GetByIdStation), new { id = station.Id }, result);
        }

        // PUT
        [HttpPut("station/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] StationPutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var station = await _context.Stationen.FirstOrDefaultAsync(s => s.Id == id);
            if (station == null)
                return NotFound($"Station mit ID {id} wurde nicht gefunden.");

            // ETag / RowVersion validieren
            var etagHeader = Request.Headers["If-Match"].ToString();
            if (string.IsNullOrWhiteSpace(etagHeader) || !ETagHelper.Matches(etagHeader, station.RowVersion))
                return StatusCode(StatusCodes.Status412PreconditionFailed, "ETag mismatch oder fehlt.");

            // Änderungen übernehmen
            station.AnlageId = dto.AnlageId;
            station.Name = dto.Name;
            station.IsActive = dto.IsActive;
            //station.GeaendertUtc = DateTime.UtcNow;  // wird automatisch gesetzt durch dbcontext
            //station.GeaendertVon = dto.GeaendertVon;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            return Ok($"Station mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("station/{id:int}")]
        public async Task<IActionResult> DeleteStation(int id)
        {
            var station = await _context.Stationen.FindAsync(id);
            if (station == null)
                return NotFound($"Station mit ID {id} wurde nicht gefunden.");

            _context.Stationen.Remove(station);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region STATUS

        // GETAll mit Pagination
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<StatusGetDTO>>> GetAllStatus(int page = 1, int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page und PageSize müssen größer als 0 sein.");

            var query = _context.Stati.AsNoTracking();
            var totalCount = await query.CountAsync();

            var stati = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => MapStatusToDto(s))      //aufruf der GetDTO variablen unten aus dem Code
                .ToListAsync();

            Response.Headers["X-Total-Count"] = totalCount.ToString();

            return Ok(stati);
        }

        // GET mit ID
        [HttpGet("status/{id:int}")]
        public async Task<ActionResult<StatusGetDTO>> GetByIdStatus(int id)
        {
            var status = await _context.Stati
                .AsNoTracking()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (status == null)
                return NotFound($"Status mit ID {id} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(status.RowVersion);
            return Ok(MapStatusToDto(status));      //aufruf der GetDTO variablen unten aus dem Code
        }

        // GET mit GUID
        [HttpGet("statusguid/{guid:guid}")]
        public async Task<ActionResult<StatusGetDTO>> GetByGuidStatus(Guid guid)
        {
            var status = await _context.Stati
                .AsNoTracking()
                .Where(s => s.Guid == guid)
                .FirstOrDefaultAsync();

            if (status == null)
                return NotFound($"Status mit GUID {guid} wurde nicht gefunden.");

            Response.Headers["ETag"] = ETagHelper.ToEtag(status.RowVersion);

            return Ok(MapStatusToDto(status));      //aufruf der GetDTO variablen unten aus dem Code
        }

        // POST
        [HttpPost("status")]
        public async Task<ActionResult<StatusGetDTO>> Create([FromBody] StatusCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var status = new Status
            {
                Bezeichnung = dto.Bezeichnung
                //ErstelltUtc = DateTime.UtcNow,  // wird automatisch gesetzt durch dbcontext
                //ErstelltVon = dto.ErstelltVon
            };

            _context.Stati.Add(status);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            var result = MapStatusToDto(status);      //aufruf der GetDTO variablen unten aus dem Code

            return CreatedAtAction(nameof(GetByIdStatus), new { id = status.Id }, result);
        }

        // PUT
        [HttpPut("status/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromHeader(Name = "If-Match")] string ifMatch, [FromBody] StatusPutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var status = await _context.Stati.FirstOrDefaultAsync(s => s.Id == id);
            if (status == null)
                return NotFound($"Status mit ID {id} wurde nicht gefunden.");

            // ETag / RowVersion validieren
            var etagHeader = Request.Headers["If-Match"].ToString();
            if (string.IsNullOrWhiteSpace(etagHeader) || !ETagHelper.Matches(etagHeader, status.RowVersion))
                return StatusCode(StatusCodes.Status412PreconditionFailed, "ETag mismatch oder fehlt.");

            // Änderungen übernehmen
            status.Bezeichnung = dto.Bezeichnung;
            //status.GeaendertUtc = DateTime.UtcNow;  // wird automatisch gesetzt durch dbcontext
            //status.GeaendertVon = dto.GeaendertVon;  

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDatabaseUnavailable(ex))
            {
                return StatusCode(503, "Datenbank nicht erreichbar oder Verbindungsfehler.");
            }
            catch (DbUpdateException ex) when (IsDatabaseFull(ex))
            {
                return StatusCode(507, "Speicherplatz erschöpft – Datenspeicher ist voll.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Allgemeiner Datenbankfehler.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unbekannter Serverfehler.");
            }

            return Ok($"Status mit ID {id} erfolgreich aktualisiert.");
        }

        // DELETE
        [HttpDelete("status/{id:int}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.Stati.FindAsync(id);
            if (status == null)
                return NotFound($"Status mit ID {id} wurde nicht gefunden.");

            _context.Stati.Remove(status);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Map
        private static KundeGetDTO MapKundeToDto(Kunde k)
                => new KundeGetDTO
                {
                    Id = k.Id,
                    Guid = k.Guid,
                    Name = k.Name,
                    Strasse = k.Strasse,
                    Hausnummer = k.Hausnummer,
                    PLZ = k.PLZ,
                    Ort = k.Ort,
                    Land = k.Land,
                    Email = k.Email,
                    Telefonnummer = k.Telefonnummer,
                    ErstelltUtc = k.ErstelltUtc,
                    ErstelltVon = k.ErstelltVon,
                    GeaendertUtc = k.GeaendertUtc,
                    GeaendertVon = k.GeaendertVon,
                    RowVersion = k.RowVersion
                };

        private static AuftragGetDTO MapAuftragToDto(Auftrag a)
                => new AuftragGetDTO
                {
                    Id = a.Id,
                    AuftragGuid = a.AuftragGuid,
                    KundeId = a.KundeId,
                    AnlageId = a.AnlageId,
                    Auftragsname = a.Auftragsname,
                    Auftragstyp = a.Auftragstyp,
                    MengeSOLL = a.MengeSOLL,
                    MengeIO = a.MengeIO,
                    MengeNIO = a.MengeNIO,
                    TaktZiel = a.TaktZiel,
                    TaktBerechnet = a.TaktBerechnet,
                    StatusId = a.StatusId,
                    Prioritaet = a.Prioritaet,
                    Quelle = a.Quelle,
                    ErstelltUtc = a.ErstelltUtc,
                    ErstelltVon = a.ErstelltVon,
                    GeaendertUtc = a.GeaendertUtc,
                    GeaendertVon = a.GeaendertVon,
                    Beladen = a.Beladen,
                    BeladenAnzahl = a.BeladenAnzahl,
                    Bilddaten = a.Bilddaten,
                    RowVersion = a.RowVersion
                };

        private static AnlageGetDTO MapAnlageToDto(Anlage an)
            => new AnlageGetDTO
            {
                Id = an.Id,
                AnlageGuid = an.AnlageGuid,
                KundeId = an.KundeId,
                Bezeichnung = an.Bezeichnung,
                AnlagenCode = an.AnlagenCode,
                ErstelltUtc = an.ErstelltUtc,
                ErstelltVon = an.ErstelltVon,
                GeaendertUtc = an.GeaendertUtc,
                GeaendertVon = an.GeaendertVon,
                RowVersion = an.RowVersion
            };

        private static FehlerberichtGetDTO MapFehlerberichtToDto(Fehlerbericht fb)
            => new FehlerberichtGetDTO
            {

                Id = fb.Id,
                FehlerberichtGuid = fb.FehlerberichtGuid,
                StationId = fb.StationId,
                AuftragId = fb.AuftragId,
                SpindelId = fb.SpindelId,
                Spindelnummer = fb.Spindelnummer,
                Code = fb.Code,
                Titel = fb.Titel,
                Beschreibung = fb.Beschreibung,
                Schwere = fb.Schwere,
                Status = fb.Status,
                AufgetretenUtc = fb.AufgetretenUtc,
                BestaetigtUtc = fb.BestaetigtUtc,
                BehobenUtc = fb.BehobenUtc,
                CustomData = fb.CustomData,
                ErstelltUtc = fb.ErstelltUtc,
                ErstelltVon = fb.ErstelltVon,
                GeaendertUtc = fb.GeaendertUtc,
                GeaendertVon = fb.GeaendertVon,
                RowVersion = fb.RowVersion
            };

        private static SchichtGetDTO MapSchichtToDto(Schicht s)
            => new SchichtGetDTO
            {
                Id = s.Id,
                SchichtGuid = s.SchichtGuid,
                KundeId = s.KundeId,
                Name = s.Name,
                StartzeitTag = s.StartzeitTag,
                EndzeitTag = s.EndzeitTag,
                Mitternachtsarbeit = s.Mitternachtsarbeit,
                ErstelltUtc = s.ErstelltUtc,
                ErstelltVon = s.ErstelltVon,
                GeaendertUtc = s.GeaendertUtc,
                GeaendertVon = s.GeaendertVon,
                RowVersion = s.RowVersion
            };

        private static SpindelGetDTO MapSpindelToDto(Spindel sp)
            => new SpindelGetDTO
            {
                Id = sp.Id,
                SpindelGuid = sp.SpindelGuid,
                StationId = sp.StationId,
                Nummer = sp.Nummer,
                Bezeichnung = sp.Bezeichnung,
                IsActive = sp.IsActive,
                ErstelltUtc = sp.ErstelltUtc,
                ErstelltVon = sp.ErstelltVon,
                GeaendertUtc = sp.GeaendertUtc,
                GeaendertVon = sp.GeaendertVon,
                RowVersion = sp.RowVersion
            };

        private static StationGetDTO MapStationToDto(Station st)
            => new StationGetDTO
            {
                Id = st.Id,
                StationGuid = st.StationGuid,
                AnlageId = st.AnlageId,
                Name = st.Name,
                IsActive = st.IsActive,
                ErstelltUtc = st.ErstelltUtc,
                ErstelltVon = st.ErstelltVon,
                GeaendertUtc = st.GeaendertUtc,
                GeaendertVon = st.GeaendertVon,
                RowVersion = st.RowVersion
            };

        public static StatusGetDTO MapStatusToDto(Status s)

            => new StatusGetDTO
            {
                Id = s.Id,
                Guid = s.Guid,
                Bezeichnung = s.Bezeichnung,
                ErstelltUtc = s.ErstelltUtc,
                ErstelltVon = s.ErstelltVon,
                GeaendertUtc = s.GeaendertUtc,
                GeaendertVon = s.GeaendertVon,
                RowVersion = s.RowVersion
            };
        #endregion

        #region ex helper
        private static bool IsDatabaseUnavailable(DbUpdateException ex)
        {
            var msg = ex.InnerException?.Message ?? "";
            return msg.Contains("timeout", StringComparison.OrdinalIgnoreCase)
                || msg.Contains("could not open", StringComparison.OrdinalIgnoreCase)
                || msg.Contains("network-related", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsDatabaseFull(DbUpdateException ex)
        {
            var msg = ex.InnerException?.Message ?? "";
            return msg.Contains("diks-full", StringComparison.OrdinalIgnoreCase)
                || msg.Contains("Out of space", StringComparison.OrdinalIgnoreCase);
        }
        #endregion

    }
}