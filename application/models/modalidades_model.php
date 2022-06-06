<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class modalidades_model extends CI_Model{

    function __construct() 
    {
        parent::__construct();
    }

    public function CadastraModalidade()
    {
        $rst = (object)array('rst' => false, 'msg' => "");
        $data = (object)$this->input->post();

        $existente = $this->db->get_where("modalidades", "nome = '$data->nomeModalidade'")->row();
        if(!$existente)
        {
            if($data->nomeModalidade && $data->dataOcorrer)
            {
                $this->db->set("nome", $data->nomeModalidade);
                $this->db->set("data", $data->dataOcorrer);
                $this->db->set("descricao", $data->descricaoModalidade);

                if($this->db->insert("modalidades"))
                {
                    $rst->rst = true;
                    $rst->msg = "Modalidade cadastra com sucesso!!!";
                }
                else
                {
                    $rst->rst = false;
                    $rst->msg = "Erro ao cadastrar modalidade";
                }             
            }
            else
            {
                $rst->rst = false;
                $rst->msg = "Campos obrigatórios nao preenchidos";
            }
        }
        else
        {
            $rst->rst = false;
            $rst->msg = "Já existe uma modalidade com esse nome";
        }

        return $rst;
    }

    public function getModalidades()
    {        
        $this->db->select("id, nome, data, descricao");
        $rst = $this->db->get("modalidades")->result();

        return $rst;        
    }
}