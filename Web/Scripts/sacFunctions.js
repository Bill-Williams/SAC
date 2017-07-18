function buildTable1(tableId, action) {
    var table = $(tableId).DataTable();
    
    $(".dataTables_filter input[type='search']").on("keydown",
        function (evtObj) {
            if (evtObj.keyCode === 13) {
                evtObj.stopPropagation();
                evtObj.preventDefault();

                var entityId = table.row(":eq(0)", { page: "current" }).id();

                window.location.href = action + entityId;

                return false;
            }
            return true;
        });
    $("div.dataTables_filter input").focus();
}