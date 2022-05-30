<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class usuario_model extends CI_Model{
    
    function __construct() 
    {
        parent::__construct();
    }

    public function insereUsuario()
    {
        $rst = (object)array('result' => false, 'msg' => "");
        $post = (object)$this->input->post();

        echo '<pre>';
        print_r($post);
        echo '</pre>';
        exit;
    }
}