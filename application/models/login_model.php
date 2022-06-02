<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class login_model extends CI_Model{
    
    function __construct() 
    {
        parent::__construct();
    }

    private $login = array(        
        "logged" => false,
        "error" => "",
        "usuario" => "",
        "id" => 0,
        "msg" => "",
    ); 

    public function login()
    {        
        $data = (object)$this->input->post();  
        $login = (object)$this->login;
        $dataAgora = date('Y-m-d H:i:s');

        if(!empty($data))
        {   
            $query = $this->db->get_where("usuarios", "usuario = '".$data->usuario."'")->row();
            
            if($query)
            {
                $login->id = $query->id;
                
                if($query->senha == $data->senha)
                {
                    $this->db->set("data_login", $dataAgora);
                    $this->db->where("id = $query->id");
                    $this->db->update("usuarios");
                    $login->usuario = $query->usuario;
                    $login->logged = true;
                    $login->msg = "Logado com sucesso!!!";
                }
                else
                {
                    $login->error = "A senha está incorreta";
                }
            }            
            else
            {
                $login->error = "O usuário digitado não existe";
            }
        }

        return $login;
    }  
    
    public function logout($redir = true)
    {
        $dados = $this->session->userdata("dados" . APPNAME);
        $query = $this->db->get_where("usuarios", "id = '".$dados->id."'")->row();
    
        if($query)
        {   
            $this->session->unset_userdata(array("is_logged", "dados" . APPNAME));
            if ($redir) 
            {
                $this->session->set_flashdata("message", "<div class='alert alert-info'>Até logo $dados->login.</div>");
                redirect(base_url("Usuarios/signin"));
            }                        
        }        
    }
}