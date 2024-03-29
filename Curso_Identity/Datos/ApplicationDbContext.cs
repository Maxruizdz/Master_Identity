﻿using Curso_Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Curso_Identity.Datos

{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }


        public DbSet<AppUsuario> AppUsuario { get; set;}

    }
}
