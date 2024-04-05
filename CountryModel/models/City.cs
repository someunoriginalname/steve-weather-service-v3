using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountryModel.models;
[Table("City")]
[Index(nameof(Latitude))]
[Index(nameof(Longitude))]
[Index(nameof(Name))]
[Index(nameof(Population))]
public class City
{
    #region Properties
    [Key]
    [Required]
    public int CityId { get; set; }

    [Column(TypeName = "numeric(18, 4)")]
    public decimal Latitude { get; set; }

    [Column(TypeName = "numeric(18, 4)")]
    public decimal Longitude { get; set; }

    [ForeignKey("CountryId")]
    public int CountryId { get; set; }
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public int Population { get; set; }

    [InverseProperty("Cities")]
    public virtual Country Country { get; set; } = null!;
    #endregion
}
