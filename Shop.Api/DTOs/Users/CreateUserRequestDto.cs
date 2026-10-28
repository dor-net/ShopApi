using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTOs.Users;

public class CreateUserRequestDto
{
    [Required(ErrorMessage = "نام الزامی می باشد")]
    [MaxLength(50, ErrorMessage = "نام نمی تواند بیشتر از 50 کاراکتر باشد")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "نام خانوادگی الزامی می باشد")]
    [MaxLength(50, ErrorMessage = "نام خانوادگی نمی تواند بیشتر از 50 کاراکتر باشد")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "موبایل الزامی می باشد")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "طول موبایل باید 11 کاراکتر باشد")]
    public string Mobile { get; set; } = null!;

    [Required(ErrorMessage = "نام کاربری الزامی می باشد")]
    [MaxLength(10, ErrorMessage = "نام کاربری نمی تواند بیشتر از 10 کاراکتر باشد")]
    [MinLength(3, ErrorMessage = "نام کاربری نمی تواند کمتر از 3 کاراکتر باشد")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "کلمه عبور الزامی می باشد")]
    [MaxLength(20, ErrorMessage = "کلمه عبور نمی تواند بیشتر از 20 کاراکتر باشد")]
    [MinLength(8, ErrorMessage = "کلمه عبور نمی تواند کمتر از 8 کاراکتر باشد")]
    public string Password { get; set; } = null!;
}
