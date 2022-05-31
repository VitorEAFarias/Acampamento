<body class="">
  <!-- Navbar -->
    <nav class="navbar navbar-expand-lg position-absolute top-0 z-index-3 w-100 shadow-none my-3 navbar-transparent mt-4">
        <div class="container">
            <button class="navbar-toggler shadow-none ms-2" type="button" data-bs-toggle="collapse" data-bs-target="#navigation" aria-controls="navigation" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon mt-2">
                    <span class="navbar-toggler-bar bar1"></span>
                    <span class="navbar-toggler-bar bar2"></span>
                    <span class="navbar-toggler-bar bar3"></span>
                </span>
            </button>
            <div class="collapse navbar-collapse" id="navigation">
                <ul class="navbar-nav mx-auto">
                    <li class="nav-item">
                        <a class="nav-link d-flex align-items-center me-2 active" aria-current="page" href="<?= base_url('Dashboard') ?>">
                            <i class="fa fa-chart-pie opacity-6  me-1"></i>
                            Dashboard
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link me-2" href="<?= base_url('Usuarios/index') ?>">
                            <i class="fa fa-user opacity-6  me-1"></i>
                            Perfil
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link me-2" href="<?= base_url('Usuarios/signup') ?>">
                            <i class="fas fa-user-circle opacity-6  me-1"></i>
                            Cadastrar
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link me-2" href="<?= base_url('Usuarios/signin') ?>">
                            <i class="fas fa-key opacity-6  me-1"></i>
                            Entrar
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <!-- End Navbar -->
    <main class="main-content  mt-0">
        <div class="page-header align-items-start min-vh-50 pt-5 pb-11 m-3 border-radius-lg" style="background-image: url('<?= base_url('assets/img/cadastro-fnd.jpg')?>'); background-position: top;">
            <span class="mask bg-gradient-dark opacity-6"></span>
            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-lg-5 text-center mx-auto">
                        <h1 class="text-white mb-2 mt-5">Bem Vindo(a)!</h1>
                        <p class="text-lead text-white"></p>
                    </div>
                </div>
            </div>
        </div>
        <form id="frmUsuario">
            <div class="container">
                <div class="row mt-lg-n10 mt-md-n11 mt-n10 justify-content-center">
                    <div class="col-xl-4 col-lg-5 col-md-7 mx-auto">
                        <div class="card z-index-0">
                                <div class="text-center pt-4">
                                    <h5>Registre-se</h5>
                                </div>
                            <div class="card-body">                            
                                <form role="form">
                                    <div class="mb-3">
                                        <input type="text" name="nome" class="form-control" placeholder="Nome" aria-label="Name">
                                    </div>
                                    <div class="mb-3">
                                        <input type="email" name="email" class="form-control" placeholder="E-mail" aria-label="Email">
                                    </div>
                                    <div class="mb-3">
                                        <input type="password" name="senha" class="form-control" placeholder="Senha" aria-label="Password">
                                    </div>
                                    <div class="form-check form-check-info text-start">
                                        <input class="form-check-input" type="checkbox" value="1" name="termosUso" id="flexCheckDefault" checked>
                                        <label class="form-check-label" for="flexCheckDefault">
                                            Eu aceito os <a href="javascript:;" class="text-dark font-weight-bolder">Termos e Condições</a>
                                        </label>
                                    </div>
                                    <div class="text-center">
                                        <button type="submit" class="btn bg-gradient-dark w-100 my-4 mb-2">Cadastrar</button>
                                    </div>
                                    <p class="text-sm mt-3 mb-0">Já tem uma conta? <a href="<?= base_url('Dashboard/signin')?>" class="text-dark font-weight-bolder">Entrar</a></p>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </main>
    <!-- -------- START FOOTER 3 w/ COMPANY DESCRIPTION WITH LINKS & SOCIAL ICONS & COPYRIGHT ------- -->
    <footer class="footer py-5">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 mx-auto text-center mb-4 mt-2">
                    <a href="javascript:;" target="_blank" class="text-secondary me-xl-4 me-4">
                        <span class="text-lg fab fa-dribbble"></span>
                    </a>
                    <a href="javascript:;" target="_blank" class="text-secondary me-xl-4 me-4">
                        <span class="text-lg fab fa-twitter"></span>
                    </a>
                    <a href="javascript:;" target="_blank" class="text-secondary me-xl-4 me-4">
                        <span class="text-lg fab fa-instagram"></span>
                    </a>
                    <a href="javascript:;" target="_blank" class="text-secondary me-xl-4 me-4">
                        <span class="text-lg fab fa-pinterest"></span>
                    </a>
                    <a href="javascript:;" target="_blank" class="text-secondary me-xl-4 me-4">
                        <span class="text-lg fab fa-github"></span>
                    </a>
                </div>
            </div>
            <div class="row">
                <div class="col-8 mx-auto text-center mt-1">
                    <p class="mb-0 text-secondary">
                        Copyright © <script>
                        document.write(new Date().getFullYear())
                        </script> Soft by Creative Tim.
                    </p>
                </div>
            </div>
        </div>
    </footer>
</body>