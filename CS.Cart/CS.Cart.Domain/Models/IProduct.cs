using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Shared.Domain.Models
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
