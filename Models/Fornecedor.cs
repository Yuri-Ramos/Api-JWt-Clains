using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_JWt_Clains.Models
{
    public class Fornecedor
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }

        public string? Documentacao { get; set; }

        public bool Ativo { get; set; }
    }
}