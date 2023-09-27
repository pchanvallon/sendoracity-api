namespace SendoraCityApi.Repositories.Database.Models;

public partial class City
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Name { get; set; } = null!;

    public bool Touristic { get; set; }

    public virtual ICollection<House> Houses { get; set; } = new List<House>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
