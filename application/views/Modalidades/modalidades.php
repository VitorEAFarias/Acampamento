<div class="main-content position-relative max-height-vh-100 h-100">
    <div class="card shadow-lg mx-4 card-profile-bottom">
        <form id="frmModalidades">
            <div class="card-body p-3">
                <div class="row gx-4">           
                    <div class="col-lg-12 col-sm-12 col-xs-12">            
                        <div class="card">                
                            <div class="card-body p-0">
                                <div class="row">
                                    <div class="card-header border-transparent">
                                        <h3 class="card-title">Modalidades</h3>                                   
                                    </div>
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-left m-2">
                                        <button type="button" class="btn bg-gradient-info" data-bs-toggle="modal" data-bs-target="#modalModalidade">Nova Modalidade</button>                            
                                    </div>                                                  
                                </div>                   
                                <div class="row m-2">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <table class="table m-0" id="tbModalidades">    
                                                <thead>                                  
                                                </thead>
                                                <tbody>                            
                                                </tbody>
                                            </table>                            
                                        </div>
                                    </div>
                                </div>                        
                            </div>
                        </div>
                    </div>            
                </div>
            </div>
        </form>
    </div>
</div>
    
<!-- Modal -->
<div class="modal fade" id="modalModalidade" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <form id="frmModalidade">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Cadastrar Modalidade</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="example-text-input" class="form-control-label">Nome</label>
                                <input class="form-control" name="nomeModalidade" type="text" value="">
                            </div>
                        </div>                
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="example-text-input" name="dataModalidade" class="form-control-label">Data</label>
                                <input class="form-control datepicker" name="dataOcorrer" id="selector" placeholder="Selecione a data que ocorrera" type="text" onfocus="focused(this)" onfocusout="defocused(this)">
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-group">
                                <label for="example-text-input" class="form-control-label">Descrição</label>
                                <textarea class="form-control" name="descricaoModalidade" aria-label="With textarea"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn bg-gradient-secondary" data-bs-dismiss="modal">Fechar</button>
                    <button type="submit" class="btn bg-gradient-primary">Salvar</button>
                </div>
            </div>
        </form>
    </div>
</div>