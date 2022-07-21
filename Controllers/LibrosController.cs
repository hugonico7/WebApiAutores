using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Models;

namespace WebAPIAutores.Controllers;
[ApiController]
[Route("api/libros")]
public class LibrosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public LibrosController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibroDTO>> Get(int id)
    {
        var libro = await _context.Libros.Include(libro1 => libro1.Comentarios )
            .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(_mapper.Map<LibroDTO>(libro));
    }

    [HttpGet]
    public async Task<ActionResult<List<LibroDTO>>> Get()
    {
        var libros = await _context.Libros.Include(libro1 => libro1.Comentarios ).ToListAsync();
        return Ok(_mapper.Map<List<LibroDTO>>(libros));
    }
    
    [HttpPost]
    public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDto)
    {
        var autoresIds = await _context.Autores.Where(autor => libroCreacionDto.AutoresIds.Contains(autor.Id))
            .Select(x => x.Id).ToListAsync();

        if (libroCreacionDto.AutoresIds.Count != autoresIds.Count)
        {
            return BadRequest();
        }
        
        var libro = _mapper.Map<Libro>(libroCreacionDto);
        await _context.Libros.AddAsync(libro);
        await _context.SaveChangesAsync();
        return Ok(libro);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(Libro libro, int id)
    {
        if (libro.Id != id)
        {
            return BadRequest("El id del libro no coincide");
        }
        _context.Update(libro);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _context.Libros.AnyAsync(x => x.Id == id))
        {
            var libro = await _context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            return Ok();
        }else
        {
            return BadRequest("No existe el autor");
        }
    }
}