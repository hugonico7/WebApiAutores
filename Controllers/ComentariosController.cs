using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Models;

namespace WebAPIAutores.Controllers;
[ApiController]
[Route("api/libros/{libroId:int}/comentarios")]
public class ComentariosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ComentariosController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ComentarioDTO>>> Get([FromRoute] int libroId)
    {
        var comentarios = await _context.Comentarios.Where(cometarioDB => cometarioDB.LibroId == libroId).ToListAsync();

        return Ok(_mapper.Map<List<ComentarioDTO>>(comentarios));
    }

    [HttpPost]
    public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO creacionDto)
    {
        var existeLibro = await _context.Libros.AnyAsync(x => x.Id == libroId);

        if (!existeLibro)
        {
            return BadRequest();
        }
        
        var comentario = _mapper.Map<Comentario>(creacionDto);
        comentario.LibroId = libroId;

        await _context.AddAsync(comentario);
        await _context.SaveChangesAsync();

        return Ok();
    }
}