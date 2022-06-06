<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class cronograma_model extends CI_Model{

    function __construct() 
    {
        parent::__construct();
    }

    public function getCronograma()
    {
        $rst = $this->db->get_where("modalidades", "id > 0")->result();    
        return $rst;
    }
}