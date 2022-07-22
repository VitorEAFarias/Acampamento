using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IProdutosDAL
    {
        Task<IEnumerable<ProdutosDTO>> getProdutos();
        Task<ProdutosDTO> getProduto(int Id);
        Task<ProdutosDTO> getCategoriaProduto(int IdCategoria);
        Task<IEnumerable<ProdutosDTO>> getCategoriaProdutos(int IdCategoria);
        Task<ProdutosDTO> getFornecedorProduto(int IdFornecedor);
        Task<ProdutosDTO> getNomeProduto(string nome);
        Task<ProdutosDTO> Insert(ProdutosDTO produtos);
        Task Update(ProdutosDTO produtos);
        Task Delete(int Id);
    }
}
