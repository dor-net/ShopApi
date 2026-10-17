namespace Shop.Api.DTOs.Sliders;

public class CreateSliderRequestDto
{
    public IFormFile Image { get; set; } = null!;
    public bool IsActive { get; set; }
}

