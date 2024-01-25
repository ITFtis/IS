var setDouOptions = function (options) {
    var timerflag;
    options.tableOptions.onResetView = function () {
        clearTimeout(timerflag);
        timerflag = setTimeout(function () {
            var $_t = $('.body-content > .bootstrap-table > .fixed-table-container > .fixed-table-body >table ');
            var cdatas = $_t.bootstrapTable('getData');
            if (cdatas.length == 0)
                return;
            var $_trs = $_t.find('>tbody tr');
            $.each($_trs, function (_i, _d) {
                $(this).find(cdatas[_i].Status == 0 ? '.btn-view-data-manager' : '.btn-update-data-manager').addClass('d-none');
                $(this).find(cdatas[_i].Status == 0 ? '.btn-view-data-manager' : '.btn-delete-data-manager').addClass('d-none');
            });
        }, 0);
    }
}
