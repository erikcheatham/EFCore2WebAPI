using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore2.Models.Interfaces
{
    public interface IJoinEntity<TEntity>
    {
        TEntity Navigation { get; set; }
    }
}
