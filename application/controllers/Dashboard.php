<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Dashboard extends CI_Controller {

	public function __construct()
	{
		parent::__construct();
		//poe model aqui

		$this->data = array();
		$this->data["header"] = $this->load->view("template/header", null, true);
		$this->data["footer"] = $this->load->view("template/footer", null, true); 
	}
	
	public function index()
	{		
		$this->data["sidebar"] = $this->load->view("template/sidebar", null, true);
		$this->load->view("template/content", $this->data);
	}

	public function signin()
	{
		$this->data["content"] = $this->load->view("Sign-in/signin", $this->data, true);
		$this->load->view("template/content", $this->data);
	}

	public function signup()
	{
		$this->data["content"] = $this->load->view("Sign-up/signup", $this->data, true);
		$this->load->view("template/content", $this->data);
	}
}
