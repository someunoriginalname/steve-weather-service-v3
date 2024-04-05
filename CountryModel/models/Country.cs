using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace CountryModel.models;
[Table("Country")]
[Index(nameof(Name))]
[Index(nameof(Iso2))]
[Index(nameof(Iso3))]
public class Country
{
    #region
    [Key]
    [Required]
    public int CountryId { get; set; }
    [Unicode(false)]
    public string Name { get; set; } = null!;
    [Unicode(false)]
    public string Iso2 { get; set; } = null!;
    [Unicode(false)]
    public string Iso3 { get; set; } = null!;
    [InverseProperty("Country")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
    #endregion
}
