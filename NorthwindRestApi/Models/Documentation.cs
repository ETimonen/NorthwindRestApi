using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindRestApi.Models;

public partial class Documentation
{
    public int DocumentationId { get; set; }

    public string? AvaibleRoute { get; set; }
    [Required]
    public string Method { get; set; }

    public string? Description { get; set; }
}
