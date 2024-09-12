using System.ComponentModel;

namespace ContactManager_WebApp.Models;

public partial class Contact
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [DisplayName("Date Of Birth")]
    public DateOnly DateOfBirth { get; set; }

    public bool Married { get; set; }

    public string Phone { get; set; } = null!;

    public decimal Salary { get; set; }
}
