﻿using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Models;

namespace WebAPIAutores.AutoMapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AutorCreacionDTO, Autor>();
        CreateMap<Autor, AutorDTO>();

        CreateMap<LibroCreacionDTO, Libro>();
        CreateMap<Libro, LibroDTO>();

        CreateMap<ComentarioCreacionDTO, Comentario>();
        CreateMap<Comentario, ComentarioDTO>();
    }
}