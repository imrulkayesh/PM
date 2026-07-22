
var dataTable;
   function loadTable(BILL_OWNR) {
       var dataTable = $('#Old_Data').DataTable({
           "ajax": {
               "url": "ViewOldDataJson/" + BILL_OWNR,
               "type": "GET",
               "datatype": "json"
           },
           "bDestroy": true,
           "columns": [
                          { "mData": "ownr_id" },
                          {
                              "mData": "pmnt_date", "render": function (mData) {
                                  if (mData === null) return "";
                                  return moment(mData).format('DD/MM/YYYY');
                              }
                          },
                          { "mData": "pmnt_amnt" },
                          { "mData": "ownr_name" },
                          { "mData": "pmnt_note" },
                          { "mData": "vhcl_id" },
                          { "mData": "vhcl_name" },
                          {
                              "mData": "vhcl_rgdt", "render": function (mData) {
                                  if (mData === null) return "";
                                  return moment(mData).format('DD/MM/YYYY');
                              }
                          },
                          {
                              "mData": "ownr_rdat", "render": function (mData) {
                                  if (mData === null) return "";
                                  return moment(mData).format('DD/MM/YYYY');
                              }
                          },
                          {
                              "mData": "ownr_tdat", "render": function (mData) {
                                  if (mData === null) return "";
                                  return moment(mData).format('DD/MM/YYYY');
                              }
                          },
                          {
                              "data": "id",
                              "render": function (data) {
                                  return `<div class="text-center"><a href="#" class="btn btn-info">Paid</a></div>`;
                              }
                          }
           ],
           "language": { "emptyTable": "no data found" }, "width": "100%"
       });
   };