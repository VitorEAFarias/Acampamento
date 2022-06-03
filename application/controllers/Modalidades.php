<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Modalidades extends CI_Controller {

	public function __construct()
	{
		parent::__construct();

		$this->dados = $this->session->userdata("dados" . APPNAME);
        $data["dados"] = $this->dados;

        $this->load->model("modalidades_model", "m_modalidade");

        $this->data = array();
		$this->data["header"] = $this->load->view("template/header", null, true);	
    }

    public function index()
	{		
        $this->data["javascript"] = [
            base_url("assets/js/Modalidades/modalidades.js")
        ];

		$this->data["content"] = $this->load->view("Modalidades/modalidades", null, true);
		$this->data["sidebar"] = $this->load->view("template/sidebar", null, true);
		$this->data["footer"] = $this->load->view("template/footer", $this->data, true); 
		$this->load->view("template/content", $this->data);
	}	

    public function getModalidade()
    {
        $rst = $this->m_modalidade->getModalidades();
        echo json_encode($rst, JSON_UNESCAPED_UNICODE);
    }

    public function cadastraModalidade()
    {
        $rst = $this->m_modalidade->CadastraModalidade();
        echo json_encode($rst, JSON_UNESCAPED_UNICODE);
    }
}