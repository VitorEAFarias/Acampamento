<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Usuarios extends CI_Controller {

	public function __construct()
	{
		parent::__construct();

		$this->dados = $this->session->userdata("dados" . APPNAME);
        $data["dados"] = $this->dados;		
		
		$this->load->model("usuario_model", "m_usuario");
		$this->load->model("login_model", "m_login");

		$this->data = array();
		$this->data["header"] = $this->load->view("template/header", null, true);
		$this->data["footer"] = $this->load->view("template/footer", null, true); 
	}
	
	public function index()
	{	
		$id = $this->dados->id;
		$this->data["dataUser"] = $this->m_usuario->getUser($id);
			
		$this->data["sidebar"] = $this->load->view("template/sidebar", null, true);
		$this->data["content"] = $this->load->view("Profile/profile", $this->data, true);
        
		$this->load->view("template/content", $this->data);
	}

	public function userIndex()
	{	
		$this->data["javascript"] = [
            base_url("assets/js/Usuarios/usuarios.js")
        ];	

		$this->data["sidebar"] = $this->load->view("template/sidebar", null, true);
		$this->data["content"] = $this->load->view("Usuarios/usuarios", $this->data, true);
        $this->data["footer"] = $this->load->view("template/footer", $this->data, true);
		
		$this->load->view("template/content", $this->data);
	}

	public function getAllUsers()
	{
		$rst = $this->m_usuario->getUser();
        echo json_encode($rst, JSON_UNESCAPED_UNICODE);
	}

	public function signup()
	{
		$this->data["javascript"] = [
            base_url("assets/js/Usuarios/usuarios.js")
        ];
		
		$this->data["content"] = $this->load->view("Sign-up/signup", $this->data, true);
		$this->data["footer"] = $this->load->view("template/footer", $this->data, true); 		
		$this->load->view("template/content", $this->data);
	}

	public function signin()
	{
		$this->data["javascript"] = [
            base_url("assets/js/Usuarios/login.js")
        ];

		$this->data["content"] = $this->load->view("Sign-in/signin", $this->data, true);
		$this->data["footer"] = $this->load->view("template/footer", $this->data, true); 
		$this->load->view("template/content", $this->data);
	}

    public function criarCadastro()
    {
        $rst = $this->m_usuario->insereUsuario();
        echo json_encode($rst, JSON_UNESCAPED_UNICODE);
    }

	public function login()
	{
		$rst = $this->m_login->login();

		if($rst->logged)
            $this->session->set_userdata(array("is_logged" => true, "dados" . APPNAME => $rst));  

        echo json_encode($rst, JSON_UNESCAPED_UNICODE);
	}

	public function logout($redir = true)
    {
        $rst = $this->m_login->logout($redir);
        echo json_encode($rst);
    }
}