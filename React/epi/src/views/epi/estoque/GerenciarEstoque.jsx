import React, { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Api from "../../../components/Api";
import DataTable from 'react-data-table-component';
import Filter from '../../../components/Filter';
import Swal from 'sweetalert2';
import Session from "../../../components/Session";

const GerenciarEstoque = _ => {

    const[estoques, setEstoque] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {

        const loadEstoque = async () => {
            const res = await Api.queryGet('/ControllerProdutosEstoque');
            if(res.result) {
                Swal.close();
                setEstoque(await res.lista.map((estoque) => {
                    return(
                        {
                            id: estoque.id,
                            produto: estoque.produto,
                            preco: estoque.preco,
                            quantidade: estoque.quantidade,
                            tamanho: estoque.tamanho,
                            certificado: estoque.certificado,
                            validadeCertificado: estoque.validadeCertificado,
                            ativo: <button type="button" className={`btn btn-inverse-${estoque.ativo === "N" ? 'danger' : 'warning'} btn-rounded btn-fw`} onClick={ _ => {
                                handleAtivaEstoque(estoque)
                            }}>{estoque.ativo === "N" ? <i className='mdi mdi-close-circle-outline'></i> : <i className=' mdi mdi-checkbox-marked-circle-outline'></i>}</button>,
                            editar: <button type="button" className="btn btn-inverse-warning  btn-rounded btn-fw" onClick={e => {
                                Session.setParam(estoque.id)
                                navigate("/inserirEstoque");
                            }}><i className='mdi mdi-border-color'></i></button>
                        }
                    )
                }));
            }
        }

        const handleAtivaEstoque = (data) => {
            let texto = data.ativo === "S" ? "desativar" : "ativar";
            let texto2 = data.ativo === "S" ? "Desativando" : "Ativando";
            let status = data.ativo === "S" ? "N" : "S";
            Swal.fire({
                title: 'Tem certeza?',
                html: `Deseja ${texto} o produto: ${data.produto}?`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sim',
                cancelButtonText: 'Não',
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        text: `${texto2} produto ${data.produto} ...`,
                        allowEscapeKey: false,
                        allowOutsideClick: false,
                        didOpen: async () => {
                            Swal.showLoading();
                            let res = await Api.queryPut(`/ControllerProdutosEstoque/status/${data.idProduto}/${status}`);

                            if (res.result === true)
                            {
                                Swal.fire("Sucesso!", "Produto: " +data.produto+" "+(status === "S" ? 'Ativado' : 'Desativado')+" com sucesso", "success").then(() => {
                                    loadEstoque();
                                });
                            }
                            else
                            {
                                Swal.fire("Ops!", res.message, "warning");
                            }
                        }                        
                    })
                }
            })
        }

        loadEstoque();

    }, []);

    const columns = [
        {
            name: 'Produto',
            selector: row => row.produto,
            sortable: true,
        },
        {
            name: 'Preço',
            selector: row => row.preco,
            sortable: true,
        },
        {
            name: 'Quantidade',
            selector: row => row.quantidade,
            sortable: true,
        },
        {
            name: 'Tamanho',
            selector: row => row.tamanho,
            sortable: true,
        },
        {
            name: 'Certificado de Aprovação',
            selector: row => row.certificado,
            sortable: true,
        },
        {
            name: 'Validade Certificado',
            selector: row => row.validadeCertificado,
            sortable: true,
        },
        {
            name: 'Ativo/Desativo',
            width: '15%',
            selector: row => row.ativo,
            sortable: true,
        },
        {
            name: 'Editar',
            selector: row => row.editar,
            sortable: true,
        },
    ]

    const [filterText, setFilterText] = useState('');
	const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
	const filteredItems = estoques.filter(
		item => (item.produto && item.produto.toLowerCase().includes(filterText.toLowerCase())) ||
                (item.certificado && item.certificado.toLowerCase().includes(filterText.toLowerCase()))
	);

    const subHeaderComponentMemo = useMemo(() => {
		const handleClear = () => {
			if (filterText) {
				setResetPaginationToggle(!resetPaginationToggle);
				setFilterText('');
			}
		};

		return (
			<Filter onFilter={e => setFilterText(e.target.value)} onClear={handleClear} filterText={filterText} />
		);
	}, [filterText, resetPaginationToggle]);

    return (
        <div className="col-lg-12 stretch-card">
            <div className='card'>
                <div className='card-body'>
                    <h4 className='card-title'>Lista de Produtos em Estoque</h4>
                    <button type="button" className="btn btn-inverse-primary btn-rounded btn-fw" onClick={e => {
                        Session.setParam("")
                        navigate("/inserirEstoque")
                    }}>Inserir Estoque</button>
                    <DataTable 
                        columns={columns} 
                        data={filteredItems || []}
                        pagination
                        noDataComponent="Nenhum resultado encontrado"
                        paginationResetDefaultPage={resetPaginationToggle}
                        subHeader
                        subHeaderComponent={subHeaderComponentMemo}
                        persistTableHead
                    />
                </div>
            </div>
        </div>
    )
}

export default GerenciarEstoque;