﻿using System.ComponentModel.DataAnnotations;

namespace AplicacionApiPeliculas.Entidades
{
    public class Cine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 75)]
        public string Nombre { get; set; }


    }
}
