using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class LocationInput
{
    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; } = string.Empty;
}
