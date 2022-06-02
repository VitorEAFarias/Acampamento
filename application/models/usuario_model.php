<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class usuario_model extends CI_Model{
    
    function __construct() 
    {
        parent::__construct();
    }

    public function insereUsuario()
    {
        $rst = (object)array('rst' => false, 'msg' => "");
        $data = (object)$this->input->post();

        $dataAgora = date('Y-m-d H:i:s');

        if($data)
        {
            if(isset($data->termosUso))
            {
                $this->db->set("usuario", $data->usuario);
                $this->db->set("email", $data->email);
                $this->db->set("senha", $data->senha);
                $this->db->set("data_cadastro", $dataAgora);
                $this->db->set("ativo", 1);

                if($this->db->insert("usuarios"))
                {
                    $rst->id = $this->db->insert_id();
                    $rst->rst = true;
                    $rst->msg = "Cadastro realizado com sucesso!!!";
                }
                else
                {
                    $rst->msg = "Erro ao efetuar cadastro";
                }            
            }
            else
            {
                $rst->rst = false;
                $rst->msg = "Para prosseguir é necessário aceitar os termos de uso";
            }
        }
        else
        {
            $rst->rst = false;
            $rst->msg = "È necessário preencher todos os campos";
        }

        return $rst;        
    }

    public function getUser($id)
    {
        if($id != null)
        {
            $rst = $this->db->get_where("usuarios", "id = $id")->row();
            return $rst;
        }
        else
        {
            return array();
        }
    }
}