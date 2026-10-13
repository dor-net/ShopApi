using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTOs.Roles;


public class UpdateRoleRequestDto
{
    public int Id { get; set; }


    [Required(ErrorMessage = "نام الزامی می باشد")]
    [MaxLength(100, ErrorMessage = "نام نمی تواند بیشتر از 100 کاراکتر باشد")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "نام نمایشی الزامی می باشد")]
    [MaxLength(100, ErrorMessage = "نام نمایشی نمی تواند بیشتر از 100 کاراکتر باشد")]
    public string DisplayName { get; set; } = null!;

}
