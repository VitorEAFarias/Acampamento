using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IEpiVinculoDAL
    {
        Task<IEnumerable<EpiVinculoDTO>> GetVinculos();
        Task<EpiVinculoDTO> GetVinculo(int Id);
        Task<IEnumerable<EpiVinculoDTO>> GetUsuarioVinculo(int Id);
        Task<EpiVinculoDTO> GetProdutoVinculo(int IdProduto);
        Task<EpiVinculoDTO> Insert(EpiVinculoDTO epivinculo);
        Task Update(EpiVinculoDTO epivinculo);
        Task Delete(int Id);
    }
}
