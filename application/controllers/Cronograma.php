<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Cronograma extends CI_Controller {

	public function __construct()
	{
		parent::__construct();		

		$this->data = array();
		$this->data["header"] = $this->load->view("template/header", null, true);	
		$this->load->model("cronograma_model", "m_cronograma");	
	}
	
	public function index()
	{		
		$this->data["cronograma"] = $this->m_cronograma->getCronograma();
		$this->data["content"] = $this->load->view("Cronograma/cronograma", $this->data, true);
		$this->data["sidebar"] = $this->load->view("template/sidebar", null, true);
		$this->data["footer"] = $this->load->view("template/footer", null, true); 
		$this->load->view("template/content", $this->data);
	}
}
