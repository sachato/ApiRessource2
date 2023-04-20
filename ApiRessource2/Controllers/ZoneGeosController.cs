using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2;
using ApiRessource2.Models;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneGeosController : ControllerBase
    {
        private readonly DataContext _context;

        public ZoneGeosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZoneGeo>>> GetZoneGeos()
        {
            List<ZoneGeo> zonegeos = await _context.ZoneGeos.ToListAsync();
            return Ok(zonegeos);
        }




        // GET: api/ZoneGeos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZoneGeo>> GetZoneGeo(int id)
        {
            var zoneGeo = await _context.ZoneGeos.FindAsync(id);

            if (zoneGeo == null)
                return NotFound();

            return zoneGeo;
        }

        [HttpGet("getzonegeobynomcommune/{NomCommune}")]
        public async Task<ActionResult<ZoneGeo>> GetZoneGeoByNomCommune(string NomCommune)
        {
            var zoneGeo = await _context.ZoneGeos.Where(z => z.NomCommune == NomCommune).FirstOrDefaultAsync();

            if (zoneGeo == null)
                return NotFound();

            return zoneGeo;
        }

        [HttpGet("getzonegeobycodepostale/{CodePostale}")]
        public async Task<ActionResult<ZoneGeo>> GetZoneGeoByCodePostale(int CodePostale)
        {
            var zoneGeo = await _context.ZoneGeos.Where(z => z.CodePostale == CodePostale).FirstOrDefaultAsync();

            if (zoneGeo == null)
                return NotFound();

            return zoneGeo;
        }
    }
}
