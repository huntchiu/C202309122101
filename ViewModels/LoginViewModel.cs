using System.ComponentModel.DataAnnotations;

namespace C202309122101.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(dataType: DataType.Password)]
    public string Password { get; set; } = string.Empty;
} 