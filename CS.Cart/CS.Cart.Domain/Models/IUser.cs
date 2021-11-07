using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Shared.Domain.Models
{
    public interface IUser
    {
        int Id { get; set; }
        string Email { get; set; }
        
    }
}
