namespace SendoraCityApi.Repositories.Database.Models;

public partial class Building
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Address { get; set; } = null!;

    public int? Cityid { get; set; }

    public virtual City? City { get; set; }
}
