using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore2WebAPI.Models.Interfaces
{
    public interface IJoinEntity<TEntity>
    {
        TEntity Navigation { get; set; }
    }
}
