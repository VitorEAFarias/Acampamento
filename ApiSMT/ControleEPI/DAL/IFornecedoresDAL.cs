﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IFornecedoresDAL
    {
        Task<FornecedorDTO> Insert(FornecedorDTO fornecedor);
        Task<FornecedorDTO> getFornecedor(int Id);
        Task<FornecedorDTO> getNomeFornecedor(string nome);
        Task<IEnumerable<FornecedorDTO>> getFornecedores();
        Task Update(FornecedorDTO fornecedor);
        Task Delete(int Id);
    }
}
