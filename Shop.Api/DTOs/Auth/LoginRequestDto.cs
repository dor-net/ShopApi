using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "نام کاربری الزامی می باشد")]
    [MaxLength(10, ErrorMessage = "نام کاربری نمی تواند بیشتر از 10 کاراکتر باشد")]
    [MinLength(3, ErrorMessage = "نام کاربری نمی تواند کمتر از 3 کاراکتر باشد")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "کلمه عبور الزامی می باشد")]
    [MaxLength(20, ErrorMessage = "کلمه عبور نمی تواند بیشتر از 20 کاراکتر باشد")]
    [MinLength(8, ErrorMessage = "کلمه عبور نمی تواند کمتر از 8 کاراکتر باشد")]
    public string Password { get; set; } = null!;
}
