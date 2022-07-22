﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IMotivosDAL
    {
        Task<IEnumerable<MotivoDTO>> getMotivos();
        Task<MotivoDTO> getMotivo(int Id);
        Task<MotivoDTO> Insert(MotivoDTO motivo);
        Task Update(MotivoDTO motivo);
        Task Delete(int Id);
    }
}
