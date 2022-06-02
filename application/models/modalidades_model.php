<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class modalidades_model extends CI_Model{

    function __construct() 
    {
        parent::__construct();
    }

    public function getModalidades()
    {        
        $this->db->select("id, nome, data, descricao");
        $rst = $this->db->get("modalidades")->result();

        return $rst;        
    }
}