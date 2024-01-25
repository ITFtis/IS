$(document).ready(function () {
    douHelper.getField(dou_options.fields, 'Status').visibleEdit = false;

    var orgDetailFormatter = dou_options.tableOptions.detailFormatter;
    dou_options.tableOptions.detailFormatter = function detailFormatter(index, row) {
        var fs = (this.settings || this.DouEditableTable.settings).fields;

        $_c = $('<div class="row">');
        $_info = $('<div class="col-sm-7"><h5>諮詢資訊</h5></div>').appendTo($_c);
        $_reply = $('<div class="col-sm-5"><h5>回覆資訊</h5></div>').appendTo($_c);
        //var html = [];

        //諮詢內容，呼叫Dou.js預設
        $($.edittableoptions.tableOptions.detailFormatter.call(this, index, row)).appendTo($_info);

        //回復內容
        var roptions = {
            showHeader: false,
            classes: 'table table-bordered table-striped table-sm table-creply',
            columns: [{
                field: 'v', title: 'v1', formatter: function (v, r) {
                    if (r.ReplyContent) {
                        var rcontents = r.ReplyContent.split(/[\r\n]+/g);
                        if (r.Status == 1) {//看有沒有不需追蹤說明
                            return '<div><span class="reply-f-title"><b>時間</b></span><span class="reply-v-content">' + helper.format.JsonDateStr2Datetime(r.LogDatettime).DateFormat('yyyy/MM/dd HH:mm:ss') + '</span></div>' +
                                '<div><span class="reply-f-title"><b>同仁</b></span><span class="reply-v-content">' + r.ReplyEmpName + '</span></div>' +
                                '<div><span class="reply-f-title"><b>狀態</b></span><span class="reply-v-content">' + douHelper.getField(fs, 'Status').formatter(r.Status) + '</span></div>' +
                                '<div><span class="reply-f-title"><b>不需追蹤說明</b></span><span class="reply-v-content">' + douHelper.getField(fs, 'StatusReason').formatter(r.StatusReason) + '</span></div>' +
                                '<div class="reply-f-title"><b>回覆內容</b></div><div class="reply-v-content">' + rcontents.join('<br>') + '</div>';
                        }
                        else {
                            return '<div><span class="reply-f-title"><b>時間</b></span><span class="reply-v-content">' + helper.format.JsonDateStr2Datetime(r.LogDatettime).DateFormat('yyyy/MM/dd HH:mm:ss') + '</span></div>' +
                                '<div><span class="reply-f-title"><b>同仁</b></span><span class="reply-v-content">' + r.ReplyEmpName + '</span></div>' +
                                '<div><span class="reply-f-title"><b>狀態</b></span><span class="reply-v-content">' + douHelper.getField(fs, 'Status').formatter(r.Status) + '</span></div>' +
                                //'<div><span class="reply-f-title"><b>不需追蹤說明</b></span><span class="reply-v-content">' + douHelper.getField(fs, 'StatusReason').formatter(r.StatusReason) + '</span></div>' +
                                '<div class="reply-f-title"><b>回覆內容</b></div><div class="reply-v-content">' + rcontents.join('<br>') + '</div>';
                        }
                    }
                    else { //新增回覆用
                        var sradios = [];
                        $.each(douHelper.getField(dou_options.fields, 'Status').selectitems, function (k, v) {
                            if (k == 0) //未回覆
                                return;
                            sradios.push('<label><input type="radio" class="reply-status-group" onclick ="StatusReason()" name="reply-status-group" value="' + k + '" ' + (k == 1 ?'checked':'')+' >' + v + '</label>');
                        });

                       //不需追蹤原因
                        var StatusReasonTEXT = '</br><label class="reply-StatusReason-group">不需追蹤原因</label><select class="reply-StatusReason-group" name="reply-StatusReason-group" id="StatusReasonSelect">';
                        $.each(douHelper.getField(dou_options.fields, 'StatusReason').selectitems, function (k, v) {
                            if (k == 0) //未回覆
                                return;
                            StatusReasonTEXT = StatusReasonTEXT + '<option value="' + k + '">' + v+'</option>'                              
                        });
                        StatusReasonTEXT = StatusReasonTEXT +'</select>'
                        sradios.push(StatusReasonTEXT);






                        //是否要能刪除
                        if (AdminOrMe(r.seq) == "True" )
                        {
                            return '<div class="reply-f-title"><b>請輸入回覆內容及案件狀態</b></div>' +
                                '<textarea rows="5" class="reply-area form-control" data-fn="ContactContent" maxlength="512" placeholder="回覆內容"></textarea>' +
                                sradios.join(' ') +
                                '<br><button onclick="del(' + r.seq + ')" type="button" class="btn btn-primary ">刪除最新一筆回覆</button>';
                        }
                        else
                        {
                            return '<div class="reply-f-title"><b>請輸入回覆內容及案件狀態</b></div>' +
                                '<textarea rows="5" class="reply-area form-control" data-fn="ContactContent" maxlength="512" placeholder="回覆內容"></textarea>' +
                                sradios.join(' ');
                        }
                    }

                }
            }],
            data: getReplysData( row), //reply回復作業會新增一筆{}空的新增編輯用
            formatNoMatches: function formatNoMatches() {
                return '尚未作任何回覆';
            }
        }

        $('<table>').appendTo($_reply).bootstrapTable(roptions);
        return $_c.prop("outerHTML");
    };

    var $_t = $('<table class="consult-table">').appendTo('.body-content');

    //#region  匯出EXCEL
    dou_options.tableOptions.buttonsAlign = 'none';
    dou_options.tableOptions.buttonsClass = 'default';
    dou_options.tableOptions.showExport = true;
    dou_options.tableOptions.iconsPrefix = "glyphicon";
    dou_options.tableOptions.icons = { export: 'glyphicon-export' };
    dou_options.tableOptions.exportTypes = ['xlsx'];
    dou_options.tableOptions.exportOptions = { fileName: 'myfile' };
    dou_options.tableOptions.formatExport = function () { return "匯出Excel" }
    //#endregion






    dou_options.fields.push({
        field: 'ReplyLogs', visible: false, title: '回覆', formatter: function (v, r) {

            var text = "";
            $.each(v, function (index, value) { text = text + (parseInt(index) + 1).toString() + "." + value.ReplyEmpName + ":" + value.ReplyContent + "    " })

            return (text.length > 0 ? text : '-');
        }
    })



    dou_options.tableOptions.pageSize = 15;
    dou_options.tableOptions.pageList = [15, 30, 80, 200,1000000000];



    setDouOptions(dou_options);
    $_t.DouEditableTable(dou_options);
});
var setDouOptions = function (options) {

}
var getReplysData = function (row) {
    return row.ReplyLogs;
}

//只能刪除最新一筆而且是自己的回覆
function del(seq) {
    if (confirm('您即將刪除最新一筆回覆')) {

        $.ajax({
            url: $.AppConfigOptions.baseurl + 'ConsultReply/Del?Seq=' + seq,
        }).done(function () {
            alert("已刪除");
            location.reload();
        }).fail(function () {

            alert("刪除失敗，您只能刪除自己的資料");

        })

    } 
}



function StatusReason() {
    if ($('input[name="reply-status-group"]:checked').val() == 1) {
        $('.reply-StatusReason-group').removeClass('d-none')
    }
    else {
        $('.reply-StatusReason-group').addClass('d-none')
    }
}

//確認最新一筆是自己的回覆
function AdminOrMe(seq) {
    var returnval = "False"
        $.ajax({
            url: $.AppConfigOptions.baseurl + 'ConsultReply/AdminOrMe?Seq=' + seq,
            async: false
        }).done(function (result) {
             returnval = result
        })
    return returnval;
    
}