using System.ComponentModel;

namespace ContactManager_WebApp.Models;

public partial class Contact
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    [DisplayName("Date Of Birth")]
    public DateOnly DateOfBirth { get; init; }

    public bool Married { get; init; }

    public string Phone { get; init; } = null!;

    public decimal Salary { get; init; }
}
