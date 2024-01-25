$(document).ready(function () {
    douHelper.getField(dou_options.fields, 'Order').visible = false;

    dou_options.fields.unshift({ field: "OrderCtrl", title: '排序', class: 'order-ctrl', visible: false, visibleEdit: false, width: 60, formatter: function (v, r) { return '<span class="btn btn-info glyphicon glyphicon-sort"></span>' } });

    dou_options.tableOptions.sortName = "Order";

    dou_options.appendCustomToolbars = [{
        item: '<span class="btn btn-primary btn-sm  glyphicon glyphicon-sort" title="編輯">排 序</span>', event: 'click .glyphicon-sort',
        callback: function (e) {
            $(this).parent().find('> *').toggleClass('d-none');

            createTableDragger();
        }
    },
    {
        item: '<span class="btn btn-success btn-sm  glyphicon glyphicon-ok d-none" title="儲存">儲存</span>', event: 'click .glyphicon-sort',
        callback: function (e) {
            $(this).parent().find('> *').toggleClass('d-none');
            var cdatas = $_t.bootstrapTable('getData');
            $.each(cdatas, function (_i, _d) {
                this.Order = _i + 1;
            });
            dou_options.updateServerData(cdatas, function (r) {
                if (r.Success)
                    helper.jspanel.jspAlertMsg(undefined, { title: '編輯結果', autoclose:r.Success?3000:20000, content: r.Success ? '編輯成功' :'<div style="color:red;">編輯失敗<br>'+r.Desc+'</div>'})
                destroyTableDragger();
            });
        }
    },
    {
        item: '<span class="btn btn-warning btn-sm  glyphicon glyphicon-remove d-none" title="取消">取消</span>', event: 'click .glyphicon-sort',
        callback: function (e) {
            $(this).parent().find('> *').toggleClass('d-none');
            destroyTableDragger();
        }
    }];
    //douHelper.getField(dou_options.fields, 'Order')
    var $_t = $("<table>").appendTo('.body-content').DouEditableTable(dou_options);

    var dragger;
    var createTableDragger = function () {
        $_t.DouEditableTable('showTableColumn', 'OrderCtrl', true);
        if (dragger) dragger.destroy();
        dragger = tableDragger($_t[0], {
            mode: 'row',
            dragHandler: 'table .order-ctrl',
            onlyBody: true,
            animation: 300
        });
        var cdatas = $_t.bootstrapTable('getData');
        dragger.on('drag', function () {
            cdatas = $_t.bootstrapTable('getData');
        });
        dragger.on('drop', function (from, to) {
            var element = cdatas[from - 1];
            cdatas.splice(from - 1, 1);
            cdatas.splice(to - 1, 0, element);
            //console.log("from, to:" + from + " " + to)
        });
    }
    var destroyTableDragger = function () {
        if (dragger)  dragger.destroy();
        $_t.DouEditableTable('showTableColumn', 'OrderCtrl', false);
    }
});