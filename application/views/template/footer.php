</div>
<!--Core JS Files-->
<script src="<?=base_url('assets/js/core/popper.min.js')?>"></script>
<script src="<?=base_url("assets/js/core/bootstrap.min.js")?>"></script>
<!--TimeLine-->
<!-- <script src="<?= base_url('/assets/js/plugins/TimeLine/main.js') ?>"></script> -->
<!--Scrollbars-->
<script src="<?=base_url('assets/js/plugins/Scrollbar/perfect-scrollbar.min.js')?>"></script>
<script src="<?=base_url('assets/js/plugins/Scrollbar/smooth-scrollbar.min.js')?>"></script>
<!--Charts-->
<script src="<?=base_url("assets/js/plugins/Charts/chartjs.min.js")?>"></script>
<!--Sweetalert-->
<script src="<?=base_url("assets/js/plugins/sweetalert2/sweetalert2.min.js") ?>"></script>
<!--JQuery-->
<script type="text/javascript" src="<?= base_url("assets/js/plugins/jquery/jquery.min.js") ?>"></script>
<!--Datepicker-->
<script src="<?= base_url('/assets/js/plugins/Datepicker/flatpickr.min.js') ?>"></script>
<!-- DataTables -->
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/JSZip-2.5.0/jszip.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/pdfmake-0.1.36/pdfmake.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/pdfmake-0.1.36/vfs_fonts.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/DataTables-1.10.22/js/jquery.dataTables.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/DataTables-1.10.22/js/dataTables.bootstrap4.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/Buttons-1.6.5/js/dataTables.buttons.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/Buttons-1.6.5/js/buttons.bootstrap4.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/Buttons-1.6.5/js/buttons.colVis.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/Buttons-1.6.5/js/buttons.html5.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/Responsive-2.2.6/js/dataTables.responsive.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/Responsive-2.2.6/js/responsive.bootstrap4.min.js") ?>"></script>
<script type="text/javascript" src="<?= base_url("assets/js/plugins/DataTables/Select-1.3.1/js/dataTables.select.min.js") ?>"></script> 

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

<?php 
if(isset($javascript)): 
    foreach($javascript as $item):
       echo '<script src="'. $item . '?v=' . time() .' "></script>';
    endforeach;
endif;
?>

<!-- Control Center for Soft Dashboard: parallax effects, scripts for the example pages etc -->
<script src="<?= base_url('assets/js/argon-dashboard.min.js?v=2.0.2') ?>"></script>
</body>
</html>