</div>
<!--   Core JS Files   -->
<script src="<?=base_url('assets/js/core/popper.min.js')?>"></script>
<script src="<?=base_url("assets/js/core/bootstrap.min.js")?>"></script>
<script src="<?=base_url('assets/js/plugins/perfect-scrollbar.min.js')?>"></script>
<script src="<?=base_url('assets/js/plugins/smooth-scrollbar.min.js')?>"></script>
<script src="<?=base_url("assets/js/plugins/chartjs.min.js")?>"></script>
<script src="<?=base_url("assets/js/plugins/sweetalert2/sweetalert2.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/jquery/jquery.min.js") ?>"></script>

<script>
    var BASE_URL = "<?= base_url()?>";
    var win = navigator.platform.indexOf('Win') > -1;
    if (win && document.querySelector('#sidenav-scrollbar')) {
        var options = {
            damping: '0.5'
        }
        Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
    }
</script>
<!-- Github buttons -->
<script async defer src="https://buttons.github.io/buttons.js"></script>
<!-- Control Center for Soft Dashboard: parallax effects, scripts for the example pages etc -->
<script src="<?= base_url('assets/js/argon-dashboard.min.js?v=2.0.2') ?>"></script>

<?php 
if(isset($javascript)): 
    foreach($javascript as $item):
       echo '<script src="'. $item . '?v=' . time() .' "></script>';
    endforeach;
endif;
?>
</body>
</html>