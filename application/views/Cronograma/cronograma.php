<div class="card shadow-lg mx-4 card-profile-bottom">
    <form id="frmModalidades">
        <div class="card-body p-3">             
            <section class="cd-timeline js-cd-timeline">                
                <div class="container max-width-lg cd-timeline__container">    
                    <?php foreach($cronograma as $item):?>     
                        <div class="cd-timeline__block">
                            <div class="cd-timeline__img cd-timeline__img--picture">
                                <img src="<?= base_url('assets/img/TimeLine/cd-icon-picture.svg') ?>" alt="Picture">
                            </div> <!-- cd-timeline__img -->
                            <div class="cd-timeline__content text-component">
                                <h2 value="<?= $item->id ?>"><?= $item->nome ?></h2>
                                <p class="color-contrast-medium"><?= $item->descricao ?></p>
                                <div class="flex justify-between items-center">
                                    <span class="cd-timeline__date"><?= $item->data ?></span>
                                    <a href="#0" class="btn btn--subtle">Leia Mais</a>
                                </div>
                            </div> <!-- cd-timeline__content -->                            
                        </div> <!-- cd-timeline__block -->                        
                    <?php endforeach; ?>                 
                </div>                     
            </section> <!-- cd-timeline -->                
        </div>
    </form>
</div>