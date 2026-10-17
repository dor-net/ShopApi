namespace Shop.Api.DTOs.Sliders;

public class UpdateSliderRequestDto
{
    public int Id { get; set; }
    public IFormFile? Image { get; set; } 
    public bool IsActive { get; set; }
}
