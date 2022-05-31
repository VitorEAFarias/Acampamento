<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Dashboard extends CI_Controller {

	public function __construct()
	{
		parent::__construct();

		$this->data = array();
		$this->data["header"] = $this->load->view("template/header", null, true);		
	}
	
	public function index()
	{		
		$this->data["content"] = $this->load->view("Dashboard/dashboard", null, true);
		$this->data["sidebar"] = $this->load->view("template/sidebar", null, true);
		$this->data["footer"] = $this->load->view("template/footer", null, true); 
		$this->load->view("template/content", $this->data);
	}		
}
