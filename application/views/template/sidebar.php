<body class="g-sidenav-show bg-gray-100">
    <div class="min-height-300 bg-primary position-absolute w-100" style="background-image: url('<?=base_url('assets/img/topoheader.jpg')?>');"></div>
    <aside class="sidenav bg-white navbar navbar-vertical navbar-expand-xs border-0 border-radius-xl my-3 fixed-start ms-4 " id="sidenav-main">
        <div class="sidenav-header">
            <i class="fas fa-times p-3 cursor-pointer text-secondary opacity-5 position-absolute end-0 top-0 d-none d-xl-none" aria-hidden="true" id="iconSidenav"></i>
            <a class="navbar-brand m-0" href="<?= base_url('Dashboard/index') ?>" > <!--target="_blank"-->
            <!--<img src="<?= base_url('assets/js/logo-ct-dark.png') ?>" class="navbar-brand-img h-100" alt="main_logo">-->
            <span class="ms-1 font-weight-bold">Menu</span>
            </a>
        </div>
        <hr class="horizontal dark mt-0">
        <div class="collapse navbar-collapse  w-auto " id="sidenav-collapse-main">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link active" href="<?= base_url('Dashboard/index') ?>">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-tv-2 text-primary text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Início</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="<?= base_url('Cronograma/index') ?>">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-calendar-grid-58 text-warning text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Cronograma</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="<?= base_url('Modalidades/index') ?>">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-box-2 text-success text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Modalidades</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="./pages/virtual-reality.html">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-lock-circle-open text-info text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Usuários</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="./pages/rtl.html">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-chart-bar-32 text-danger text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Ativos</span>
                    </a>
                </li>
                <li class="nav-item mt-3">
                    <h6 class="ps-4 ms-2 text-uppercase text-xs font-weight-bolder opacity-6">Gerenciamento de Contas</h6>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="<?= base_url('Usuarios/index')?>">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-single-02 text-info text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Perfil</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="<?= base_url('Usuarios/signin')?>">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-key-25 text-warning text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Entrar</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="<?= base_url('Usuarios/signup')?>">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-align-left-2 text-info text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Cadastrar</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link " href="<?= base_url('Usuarios/logout')?>">
                        <div class="icon icon-shape icon-sm border-radius-md text-center me-2 d-flex align-items-center justify-content-center">
                            <i class="ni ni-button-power text-dark text-sm opacity-10"></i>
                        </div>
                        <span class="nav-link-text ms-1">Sair</span>
                    </a>
                </li>
            </ul>
        </div>
    </aside>
    <div class="main-content position-relative max-height-vh-100 h-100">