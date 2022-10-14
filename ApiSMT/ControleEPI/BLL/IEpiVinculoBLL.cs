using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IEpiVinculoBLL
    {
        Task<IEnumerable<EpiVinculoDTO>> GetVinculos();
        Task<EpiVinculoDTO> GetVinculo(int Id);
        Task<IEnumerable<EpiVinculoDTO>> GetUsuarioVinculo(int Id);
        Task<IEnumerable<EpiVinculoDTO>> GetUsuarioVinculoStatus(int Id, int status);
        Task<EpiVinculoDTO> GetProdutoVinculo(int IdProduto);
        Task<EpiVinculoDTO> Insert(EpiVinculoDTO epivinculo);
        Task Update(EpiVinculoDTO epivinculo);
        Task Delete(int Id);
    }
}
