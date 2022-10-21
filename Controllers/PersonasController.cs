using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiUsuarios.Context;
using apiUsuarios.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Collections;
namespace apiUsuarios.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonasController : ControllerBase
{
    private readonly AppDbContext _context;

    public PersonasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Personas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Persona>>> Getpersonas()
    {
        if (_context.personas == null)
        {
            return NotFound();
        }
        return await _context.personas.ToListAsync();
    }

    // GET: api/Personas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> GetPersona(string id)
    {
        if (_context.personas == null)
        {
            return NotFound();
        }
        var persona = await _context.personas.FindAsync(id);

        if (persona == null)
        {
            return NotFound();
        }

        return persona;
    }

    // PUT: api/Personas/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}/{correo}")]
    public async Task<IActionResult> PutPersona(string id,string correo, Persona persona)
    {
        var personaUpdate = await _context.personas.FindAsync(id);
        if (!PersonaExistsCorreo(correo) || personaUpdate == null)
            return BadRequest();

        personaUpdate.contrasenhia = persona.contrasenhia;

        if (await _context.SaveChangesAsync() != 4)
            return Ok(personaUpdate);
        return NoContent();
    }


    // POST: api/Personas
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Persona>> PostPersona(Persona persona)
    {
        if (_context.personas == null)
        {
            return Problem("Entity set 'AppDbContext.personas'  is null.");
        }
        _context.personas.Add(persona);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (PersonaExists(persona.numero_identificacion))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetPersona", new { id = persona.numero_identificacion }, persona);
    }

    // DELETE: api/Personas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersona(string id)
    {
        if (_context.personas == null)
        {
            return NotFound();
        }
        var persona = await _context.personas.FindAsync(id);
        if (persona == null)
        {
            return NotFound();
        }

        _context.personas.Remove(persona);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{numero_identificacion}/{contrasenhia}")]
    public ActionResult<List<Persona>> GetIniciarSesion(string numero_identificacion, string contrasenhia)
    {
        var usuarios = _context.personas.Where(usuario => usuario.numero_identificacion.Equals(numero_identificacion) && usuario.contrasenhia.Equals(contrasenhia)).ToList();

        if (usuarios == null)
        {
            return NotFound();
        }

        return usuarios;
    }
    private bool PersonaExists(string id)
    {
        return (_context.personas?.Any(e => e.numero_identificacion == id)).GetValueOrDefault();
    }

    private bool PersonaExistsCorreo( string correo)
    {
        return (_context.personas?.Any(e =>  e.correo == correo)).GetValueOrDefault();
    }


    [HttpGet("/datos/{id}")]


    public async Task<ActionResult<IEnumerable<pagos>>> listaPagos(String id)
    {
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        HttpResponseMessage response;
        String[] informacion = null;
       
        List<pagos> list = new List<pagos>();
        var arlist = new ArrayList();
        using (var httpCliente = new HttpClient())

        {
            response = await httpCliente.GetAsync("https://f57d-2800-484-ae86-3928-e4dc-8149-5f45-a6aa.ngrok.io/pagos/" + id);
        }
        var info = await response.Content.ReadAsStringAsync();
        using var sr = new StringReader(info);

        int count = 0;
        string line;
        String crr = "";

        while ((line = sr.ReadLine()) != null)
        {
            var objetoPrueba = new pagos();
            informacion = line.Split(',');
            for (int i = 0; i < informacion.Length; i++)
            {
                for (int j = 0; j < informacion[i].Length; j++)
                {
                    if (i == 0)
                    {
                        crr += (char)(informacion[i][j] - 3);
                        objetoPrueba.id_Pago = Int32.Parse(crr); ;
                    }
                    else if (i == 1)
                    {
                        crr += (char)(informacion[i][j] - 12);
                        objetoPrueba.valor_Pagado = Int32.Parse(crr); ;
                    }
                    else if (i == 3)
                    {
                        crr += (char)(informacion[i][j] -    7);
                        objetoPrueba.id_Obligacion = Int32.Parse(crr);
                    }
                    else if (i == 4)
                    {
                        crr = crr + (char)(informacion[i][j] - 32) + "" ;
                        objetoPrueba.id_propiedad = crr;
                    }
                    else if (i == 2)
                    {
                        crr += (char)(informacion[i][j]);
                        objetoPrueba.fecha = crr;
                    }
                }
                
                informacion[i] = crr;
                crr = "";
            }
            list.Add(objetoPrueba);
            count++;
            
        }
        return list;
    }


}