var setDouOptions = function (options) {

    options.afterCreateEditDataForm = function ($_c, row) {
        var ss = this;
        var $_t = this.$table;
        var cdatas = $_t.bootstrapTable('getData');
        var cidx = cdatas.indexOf(row);




        var _nhtml = options.tableOptions.detailFormatter.call(this, cidx, row);

        $_c.find('.modal-dialog').addClass('modal-lg');
        $_c.find('.modal-body > .data-edit-form-group').addClass('d-none');//隱藏dou的修改畫面，改_nhtml客製化UI


       






        $_c.find('.modal-footer > button:eq(0)').off('click').on('click', function () { //新增一筆回覆
            var rcontent = $('table.table-creply .reply-area', $_c).val();
            var rstatus = $('input[name="reply-status-group"]:checked', $_c).val();
            var StatusReasonStatus = $('select[name="reply-StatusReason-group"]').val()
            if (!rcontent || !(rcontent.trim())) {
                alert("回覆內容不能空!!");
            } else {
                transactionDouClientDataToServer({ No: row.No, ReplyContent: rcontent, Status: rstatus, StatusReason: StatusReasonStatus }, $.AppConfigOptions.baseurl+'ConsultReply/AddReplyLog', function (result) {
                    if (result.Success) {
                        //將新增的回覆回存到前端的row
                        row.ReplyLogs = row.ReplyLogs || [];
                        row.ReplyLogs.push(result.data[0]);
                        row.Status = result.data[0].Status;
                        $_t.bootstrapTable('updateRow', { index: cidx, row: row });
                        helper.jspanel.jspAlertMsg(undefined, { content: '新增回覆成功', classes: 'modal-sm', autoclose: 5000 });

                        //因為下拉式選單不重新整理會抓到之前的，所以就重新整理網頁了
                        alert('新增回覆成功')
                        location.reload();
                    }
                    else
                        alert(result.Desc);
                    $_c.find('.data-dismiss').trigger("click");

                });
            }
            return false;
        });

        $(_nhtml).appendTo($_c.find('.modal-body'));
    }
}

var getReplysData = function (row) {//新增一筆{}空的新增編輯用
    var replys = row.ReplyLogs ? JSON.parse(JSON.stringify(row.ReplyLogs)) : [];
    if (replys.length > 0) {
        var lastreplys = replys[replys.length - 1]
        replys.push({ seq: lastreplys.Seq });
    }
    else {
        replys.push({});
    }
    setTimeout(function () {
        $('table.table-creply .reply-area').focus(); //聚焦輸入
    }, 500);
    return replys;
}
