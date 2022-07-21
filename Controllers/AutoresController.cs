using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Filtros;
using WebAPIAutores.Models;

namespace WebAPIAutores.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AutoresController> _logger;
    private readonly IMapper _mapper;

    public AutoresController(ApplicationDbContext context, ILogger<AutoresController> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AutorDTO>>> Get()
    {
        var autores = await _context.Autores.ToListAsync();

        return _mapper.Map<List<AutorDTO>>(autores);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AutorDTO>> Get(int id)
    {
        var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);

        return await _context.Autores.AnyAsync(x => x.Id == id) ? 
            _mapper.Map<AutorDTO>(autor): 
            NotFound("Autor no encontrado");
    }

    [HttpGet("{nombre}")]
    public async Task<ActionResult<IEnumerable<AutorDTO>>> Get( [FromRoute] string nombre)
    {
        var autores = await _context.Autores.Where(autorBd => autorBd.Nombre.Contains(nombre)).ToListAsync();
        
        return await _context.Autores.AnyAsync(autorBd => autorBd.Nombre.Contains(nombre))
            ? _mapper.Map<List<AutorDTO>>(autores)
            : NotFound("Autores no encontrados");
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody]AutorCreacionDTO autorCreacionDto)
    {
        
        var existeAutorConElMismoNombre = await _context.Autores.AnyAsync(x => x.Nombre == autorCreacionDto.Nombre);
        
        if (existeAutorConElMismoNombre)
        {
            return BadRequest($"Ya existe un autor con ese {autorCreacionDto.Nombre}");
        }

        var autor = _mapper.Map<Autor>(autorCreacionDto);
        
        await _context.Autores.AddAsync(autor);
        await _context.SaveChangesAsync();
        return Ok(autor);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put([FromBody] Autor autor,  [FromRoute] int id)
    {
        if (autor.Id != id)
        {
            return BadRequest("El id del autor no coincide");
        }
        _context.Update(autor);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (await _context.Autores.AnyAsync(x => x.Id == id))
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return Ok();
        }else
        {
            return BadRequest("No existe el autor");
        }
    }
}