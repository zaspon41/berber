namespace Application.DTOs;

public class CreateServiceRequest
{
    public string HizmetAdi { get; set; } = null!;
    public int Fiyat { get; set; }
}

public class ServiceResponse
{
    public int Id { get; set; }
    public string HizmetAdi { get; set; } = null!;
    public int Fiyat { get; set; }
}

public class UpdateServiceRequest
{
    public int Id { get; set; }
    public string HizmetAdi { get; set; } = null!;
    public int Fiyat { get; set; }
}
